using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class NewSwordManController : Monster
{
    private SwordManAniController _aniController;

    private MonsterAlertStatus AlertStatus;
    private MonsterWalkStatus WalkStatus;
    private MonsterCooldownStatus CooldownStatus;
    private MonsterRunStatus RunStatus;
    private MonsterAtkStatus AtkStatus;

    private IMonsterPatrolStrategy PatrolStrategy;

    private IMonsterCooldownStrategy CooldownStrategy;

    private IMonsterAtkStrategy NormalAtkStrategy;
    private IMonsterAtkStrategy StringAtkStrategy;

    // 用字典存放所有狀態物件
    private Dictionary<Status, IMonsterStatus> _statusMap;

    private Func<bool> normalAtkCondition;
    private Func<bool> stringAtkCondition;

    public NewSwordManController(MonsterData _data, SwordManData _specificData, 
        MonsterTransitionTable defaultTable, MonsterTransitionTable table, Transform trans)
    {
        InitBasicData(_data, _specificData);
        _transform = trans;

        _targetPosition = new Vector3(PlayerController.PlayerPlaceX, PlayerController.PlayerPlaceY, 0);
        _aniController = new SwordManAniController(trans, _data.AniScale);

        InitStrategies(_data, _specificData);
        InitStatuses(_specificData);
        InitStatusJudge(defaultTable , table, _specificData);

        _statusManager = new MonsterStatusManager(this, AlertStatus);
    }

    private void InitBasicData(MonsterData _data, SwordManData _specificData)
    {
        Hp = _data.Hp;
        NormalSpeed = _data.NormalSpeed;
        CampID = _data.CampID;
        face = Face.Left;
    }
    private void InitStrategies(MonsterData _data, SwordManData _specificData)
    {
        PatrolStrategy = new MonsterWalkPatrolStrategy(this, _aniController.alertAni, _data.initialFace, _data.NormalSpeed, _specificData.PatrolTime);
        CooldownStrategy = new MonsterGoBackCooldownStrategy(this, _aniController.cooldownAni, _data.NormalSpeed, _data.CooldownTime);
        NormalAtkStrategy = new SwordManNormalAtkStrategy(this, _aniController.normalAtkAni, _specificData.Atk1_1, _specificData.Atk1_2, _specificData.AtkSpeed, _specificData.AtkTime);
        StringAtkStrategy = new SwordManStringAtkStrategy(this, _aniController.stringAtkAni, _specificData.StringAtk, _specificData.StringAtkSpeed, _specificData.StringAtkTime);
    }
    private void InitStatuses(SwordManData _specificData)
    {
        AlertStatus = new MonsterAlertStatus(PatrolStrategy, _aniController.alertAni);
        WalkStatus = new MonsterWalkStatus(NormalSpeed, _aniController.walkAni);
        RunStatus = new MonsterRunStatus(_specificData.RunSpeed, _aniController.runAni);
        AtkStatus = new MonsterAtkStatus(AtkStrategyJudge);
        CooldownStatus = new MonsterCooldownStatus(CooldownStrategy);
    }
    private void InitStatusJudge(MonsterTransitionTable defaultTable, MonsterTransitionTable table, SwordManData specificData)
    {
        _statusMap = new Dictionary<Status, IMonsterStatus>
        {
            { Status.Alert, AlertStatus },
            { Status.Walk, WalkStatus },
            { Status.Run, RunStatus },
            { Status.Attack, AtkStatus },
            { Status.Cooldown, CooldownStatus },
        };

        // 建立預設轉狀態
        foreach (var rule in defaultTable.Rules)
        {
            if(_statusMap.TryGetValue(rule.FromStatus, out var fromStatus) &&
                _statusMap.TryGetValue(rule.ToStatus, out var toStatus))
            {
                ((IOnceStrategyUser)fromStatus).AddDefaultNextStatus(toStatus);
            }
            else
            {
                Debug.LogWarning($"Invalid transition rule: {rule.FromStatus} → {rule.ToStatus}");
            }
        }
        // 根據配置表建立轉狀態方式
        foreach (var rule in table.Rules)
        {
            if (_statusMap.TryGetValue(rule.FromStatus, out var fromStatus) &&
                _statusMap.TryGetValue(rule.ToStatus, out var toStatus))
            {
                if (rule.Condition == null)
                {
                    Debug.LogWarning($"Transition {rule.FromStatus} → {rule.ToStatus} has no Condition assigned.");
                    continue;
                }

                ((BaseMonsterStatus)fromStatus).AddTransition(() => rule.Condition.Evaluate(this), toStatus);
            }
            else
            {
                Debug.LogWarning($"Invalid transition rule: {rule.FromStatus} → {rule.ToStatus}");
            }
        }

        normalAtkCondition = () => specificData.NormalAtkCondition.Evaluate(this);
        stringAtkCondition = () => specificData.StringAtkCondition.Evaluate(this);
    }

    public override void ParameterCalculate()
    {
        _targetPosition = new Vector3(PlayerController.PlayerPlaceX, PlayerController.PlayerPlaceY, 0);
        DistanceWithTargetX = DistanceClass.CalculateAbsoluteDistanceX(_transform.position, _targetPosition);
        DistanceWithTargetY = DistanceClass.CalculateAbsoluteDistanceY(_transform.position, _targetPosition);
    }
    public override void HurtedControll(int damage)
    {
        Hp -= damage;
        if (Hp < 0)
        {
            Hp = 0;
        }

        if (Hp == 0)
        {
            UnityEngine.Debug.Log("Die");
        }
    }

    private IMonsterAtkStrategy AtkStrategyJudge()
    {
        if (normalAtkCondition())
        {
            return NormalAtkStrategy;
        }
        if (stringAtkCondition())
        {
            return StringAtkStrategy;
        }

        return NormalAtkStrategy;
    }
}

public class SwordManNormalAtkStrategy: BaseMonsterStrategy, IMonsterAtkStrategy, IOnceStrategy
{
    private float _speed;
    private Transform _atkObject1;
    private Transform _atkObject2;
    private float _totalTime;
    private float _timer;
    private Queue<TimedEvent> _eventQueue;
    private bool EndTrigger;
    private bool AtkMove;
    private AtkData _atkData;
    Monster monster;

    public SwordManNormalAtkStrategy(Monster m, AnimationController ani, GameObject atk1, GameObject atk2, float speed, float time)
    {
        monster = m;
        _ani = ani;
        _atkObject1 = atk1.transform;
        _atkObject2 = atk2.transform;
        _speed = speed;
        _totalTime = time;
        _atkData = new AtkData(monster.CampID, 1, monster._transform);
    }

    public override void Begin()
    {
        _ani.AniPlay();
        _ani.AniTurnFace(monster.face);

        ResetTimeEvent();
        _timer = _totalTime;
        EndTrigger = false;
        AtkMove = false;
    }
    public override void End()
    {
        _ani.AniStop();
    }
    public void FixedExecute(float deltaTime)
    {
        _timer -= deltaTime;
        
        if(_eventQueue.Count() > 0 && _eventQueue.Peek().TriggerTime >= _timer)
        {
            var timedEvent = _eventQueue.Dequeue();
            timedEvent.Callback?.Invoke();
        }

        if (AtkMove)
        {
            switch (monster.face)
            {
                case Creature.Face.Right:
                    monster.Move(_speed, deltaTime);
                    break;
                case Creature.Face.Left:
                    monster.Move(-_speed, deltaTime);
                    break;
            }
        }
    }
    public bool ExecuteComplete()
    {
        return EndTrigger;
    }

    private void ResetTimeEvent()
    {
        _eventQueue = new Queue<TimedEvent>(
            new[]
            {
                new TimedEvent { TriggerTime = _totalTime - 0.3f, Callback = Event1 },
                new TimedEvent { TriggerTime = _totalTime - 0.6f, Callback = Event2 },
                new TimedEvent { TriggerTime = _totalTime - 0.75f, Callback = Event3 },
                new TimedEvent { TriggerTime = _totalTime - 0.81f, Callback = Event4 },
                new TimedEvent { TriggerTime = 0, Callback = Event5 }
            }
        );
    }
    public void Event1()
    {
        Transform atk = GameObject.Instantiate(_atkObject1, monster._transform.position, Quaternion.identity);
        atk.GetComponent<MonsterAtk>().InitializeAtk(_atkData);
        switch (monster.face)
        {
            case Monster.Face.Right:
                atk.localScale = new Vector3(1, 1, 1);
                break;
            case Monster.Face.Left:
                atk.localScale = new Vector3(-1, 1, 1);
                break;
        }
    }
    public void Event2()
    {
        AtkMove = true;
    }
    public void Event3()
    {
        Transform atk = GameObject.Instantiate(_atkObject2, monster._transform.position, Quaternion.identity);
        atk.GetComponent<MonsterAtk>().InitializeAtk(_atkData);
        switch (monster.face)
        {
            case Monster.Face.Right:
                atk.localScale = new Vector3(1, 1, 1);
                break;
            case Monster.Face.Left:
                atk.localScale = new Vector3(-1, 1, 1);
                break;
        }
    }
    public void Event4()
    {
        AtkMove = false;
    }
    public void Event5()
    {
        EndTrigger = true;
    }
}

public class SwordManStringAtkStrategy : BaseMonsterStrategy, IMonsterAtkStrategy, IOnceStrategy
{
    private float _speed;
    private Transform _atkObject;
    private float _totalTime;
    private float _timer;
    private Queue<TimedEvent> _eventQueue;
    private bool EndTrigger;
    private bool isMoving;
    private AtkData _atkData;
    Monster monster;

    public SwordManStringAtkStrategy(Monster m, AnimationController ani, GameObject atk, float speed, float time)
    {
        monster = m;
        _ani = ani;
        _atkObject = atk.transform;
        _speed = speed;
        _totalTime = time;
        _atkData = new AtkData(monster.CampID, 1, monster._transform);
    }

    public override void Begin()
    {
        _ani.AniPlay();
        _ani.AniTurnFace(monster.face);

        ResetTimeEvent();
        _timer = _totalTime;
        EndTrigger = false;
        isMoving = false;
    }
    public override void End()
    {
        _ani.AniStop();
    }
    public void FixedExecute(float deltaTime)
    {
        _timer -= deltaTime;

        if (_eventQueue.Count() > 0 && _eventQueue.Peek().TriggerTime >= _timer)
        {
            var timedEvent = _eventQueue.Dequeue();
            timedEvent.Callback?.Invoke();
        }

        if (isMoving)
        {
            switch (monster.face)
            {
                case Monster.Face.Right:
                    monster.Move(_speed, deltaTime);
                    break;
                case Monster.Face.Left:
                    monster.Move(-_speed, deltaTime);
                    break;
            }
        }
    }
    public bool ExecuteComplete()
    {
        return EndTrigger;
    }

    private void ResetTimeEvent()
    {
        _eventQueue = new Queue<TimedEvent>(
            new[]
            {
                new TimedEvent { TriggerTime = _totalTime - 0.35f, Callback = Event1 },
                new TimedEvent { TriggerTime = _totalTime - 0.8f, Callback = Event2 },
                new TimedEvent { TriggerTime = 0, Callback = Event3 }
            }
        );
    }
    public void Event1()
    {
        isMoving = true;
        Transform atk = GameObject.Instantiate(_atkObject, monster._transform.position, Quaternion.identity);
        atk.GetComponent<MonsterAtk>().InitializeAtk(_atkData);
        switch (monster.face)
        {
            case Monster.Face.Right:
                atk.localScale = new Vector3(1, 1, 1);
                break;
            case Monster.Face.Left:
                atk.localScale = new Vector3(-1, 1, 1);
                break;
        }
    }
    public void Event2()
    {
        isMoving = false;
    }
    public void Event3()
    {
        EndTrigger = true;
    }
}
