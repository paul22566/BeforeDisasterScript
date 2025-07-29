using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class NewSwordManController: Monster
{
    private SwordManAniController _aniController;

    private IMonsterStatus AlertStatus;
    private IMonsterStatus WalkStatus;
    private IMonsterStatus CooldownStatus;
    private IMonsterStatus RunStatus;
    private IMonsterStatus AtkStatus;

    private IMonsterPatrolStrategy PatrolStrategy;

    private IMonsterCooldownStrategy CooldownStrategy;

    private IMonsterAtkStrategy NormalAtkStrategy;
    private IMonsterAtkStrategy StringAtkStrategy;

    private float AlertDistanceX;
    private float ValidDistanceY;
    private float GiveUpDistanceX;
    private float GiveUpDistanceY;
    private float ChasingDistance;
    private float AtkDistance;
    private float StringAtkDistance;

    public NewSwordManController(MonsterData _data, SwordManData _specificData, Transform trans)
    {
        InitBasicData(_data, _specificData);
        _transform = trans;

        _targetPosition = new Vector3(PlayerController.PlayerPlaceX, PlayerController.PlayerPlaceY, 0);
        _aniController = new SwordManAniController(trans, _data.AniScale);

        InitStrategies(_data, _specificData);
        InitStatuses(_specificData);

        _statusManager = new MonsterStatusManager(this, AlertStatus);
    }

    private void InitBasicData(MonsterData _data, SwordManData _specificData)
    {
        Hp = _data.Hp;
        NormalSpeed = _data.NormalSpeed;
        AlertDistanceX = _data.AlertDistanceX;
        ValidDistanceY = _data.ValidDistanceY;
        GiveUpDistanceX = _data.GiveUpDistanceX;
        GiveUpDistanceY = _data.GiveUpDistanceY;
        CampID = _data.CampID;
        face = Face.Left;

        ChasingDistance = _specificData.ChasingDistance;
        AtkDistance = _specificData.AtkDistance;
        StringAtkDistance = _specificData.StringAtkDistance;
    }
    private void InitStrategies(MonsterData _data, SwordManData _specificData)
    {
        PatrolStrategy = new MonsterWalkPatrolStrategy(this, _aniController.alertAni, _data.initialFace, _data.NormalSpeed, _specificData.PatrolTime);
        CooldownStrategy = new MonsterGoBackCooldownStrategy(_data.NormalSpeed, _data.CooldownTime);
        NormalAtkStrategy = new SwordManNormalAtkStrategy(this, _specificData.Atk1_1, _specificData.Atk1_2, _specificData.AtkSpeed, _specificData.AtkTime);
        StringAtkStrategy = new SwordManStringAtkStrategy(this, _specificData.StringAtk, _specificData.StringAtkSpeed, _specificData.StringAtkTime);
    }
    private void InitStatuses(SwordManData _specificData)
    {
        AlertStatus = new MonsterAlertStatus(PatrolStrategy, alertBreakJudge, _aniController.alertAni);
        WalkStatus = new MonsterWalkStatus(NormalSpeed, walkBreakJudge, _aniController.walkAni);
        RunStatus = new MonsterRunStatus(_specificData.RunSpeed, runBreakJudge, _aniController.runAni);
        AtkStatus = new MonsterAtkStatus(NormalAtkStrategy, atkBreakJudge, _aniController.normalAtkAni);
        CooldownStatus = new MonsterCooldownStatus(CooldownStrategy, CooldownBreakJudge, _aniController.cooldownAni);
    }

    public override void StatusJudge()
    {
        _statusManager.SetPreStatus(WalkStatus);

        if (RunCondition())
        {
            _statusManager.SetPreStatus(RunStatus);
        }
        if (NormalAtkCondition())
        {
            _statusManager.SetPreStatus(AtkStatus);
        }
        if (StringAtkCondition())
        {
            _statusManager.SetPreStatus(AtkStatus);
        }
        
        if (_statusManager.EqualStatus(AlertStatus))
        {
            _statusManager.SetPreStatus(RunStatus);
        }
        if (_statusManager.EqualStatus(AtkStatus))
        {
            _statusManager.SetPreStatus(CooldownStatus);
        }

        if (GiveUpCondition())
        {
            _statusManager.SetPreStatus(AlertStatus);
        }

        _statusManager.ConfirmStatus();
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

        if(Hp == 0)
        {
            UnityEngine.Debug.Log("Die");
        }
    }
    public override (object strategy, AnimationController ani)? GetStrategyAndAniForStatus(IMonsterStatus status)
    {
        if (status == AtkStatus)
        {
            if (StringAtkCondition())
            {
                return (StringAtkStrategy, _aniController.stringAtkAni);
            }
            else
            {
                return (NormalAtkStrategy, _aniController.normalAtkAni);
            }
        }

        return null; // 預設不提供任何策略
    }

    private bool alertBreakJudge()
    {
        if (DistanceWithTargetX <= AlertDistanceX && DistanceWithTargetY <= ValidDistanceY)
        {
            return true;
        }

        return false;
    }
    private bool walkBreakJudge()
    {
        if (RunCondition())
        {
            return true;
        }
        if (StringAtkCondition())
        {
            return true;
        }
        if (NormalAtkCondition())
        {
            return true;
        }
        if (GiveUpCondition())
        {
            return true;
        }

        return false;
    }
    private bool runBreakJudge()
    {
        if (GiveUpCondition())
        {
            return true;
        }
        if (StringAtkCondition())
        {
            return true;
        }

        return false;
    }
    private bool atkBreakJudge()
    {
        return false;
    }
    private bool CooldownBreakJudge()
    {
        if (GiveUpCondition())
        {
            return true;
        }

        return false;
    }

    private bool RunCondition()
    {
        if (DistanceWithTargetX > ChasingDistance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool NormalAtkCondition()
    {
        if (DistanceWithTargetX <= AtkDistance && DistanceWithTargetY <= ValidDistanceY)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool StringAtkCondition()
    {
        if (DistanceWithTargetX > AtkDistance * 3 && DistanceWithTargetX <= StringAtkDistance && DistanceWithTargetY <= ValidDistanceY)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool GiveUpCondition()
    {
        if (DistanceWithTargetX > GiveUpDistanceX || DistanceWithTargetY > GiveUpDistanceY)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

public class SwordManNormalAtkStrategy: IMonsterAtkStrategy, IStrategyReset
{
    private float _speed;
    private Transform _atkObject1;
    private Transform _atkObject2;
    private float _totalTime;
    private float _timer;
    private float _deltaTime = Time.fixedDeltaTime;
    private Queue<TimedEvent> _eventQueue;
    private bool EndTrigger;
    private bool AtkMove;
    private AtkData _atkData;
    Monster monster;

    public SwordManNormalAtkStrategy(Monster m, GameObject atk1, GameObject atk2, float speed, float time)
    {
        monster = m;
        _atkObject1 = atk1.transform;
        _atkObject2 = atk2.transform;
        _speed = speed;
        _totalTime = time;
        _atkData = new AtkData(monster.CampID, 1, monster._transform);
        StrategyReset();
    }

    public void Atk(Action endNotice)
    {
        _timer -= _deltaTime;
        
        if(_eventQueue.Count() > 0 && _eventQueue.Peek().TriggerTime >= _timer)
        {
            var timedEvent = _eventQueue.Dequeue();
            timedEvent.Callback?.Invoke();
        }

        if (_timer <= 0 && !EndTrigger)
        {
            EndTrigger = true;
            endNotice();
        }

        if (AtkMove)
        {
            switch (monster.face)
            {
                case Creature.Face.Right:
                    monster.Move(_speed, _deltaTime);
                    break;
                case Creature.Face.Left:
                    monster.Move(-_speed, _deltaTime);
                    break;
            }
        }
    }
    public void StrategyReset()
    {
        ResetTimeEvent();
        _timer = _totalTime;
        EndTrigger = false;
        AtkMove = false;
    }

    private void ResetTimeEvent()
    {
        _eventQueue = new Queue<TimedEvent>(
            new[]
            {
                new TimedEvent { TriggerTime = _totalTime - 0.3f, Callback = Event1 },
                new TimedEvent { TriggerTime = _totalTime - 0.6f, Callback = Event2 },
                new TimedEvent { TriggerTime = _totalTime - 0.75f, Callback = Event3 },
                new TimedEvent { TriggerTime = _totalTime - 0.81f, Callback = Event4 }
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
}

public class SwordManStringAtkStrategy : IMonsterAtkStrategy, IStrategyReset
{
    private float _speed;
    private Transform _atkObject;
    private float _totalTime;
    private float _timer;
    private float _deltaTime = Time.fixedDeltaTime;
    private Queue<TimedEvent> _eventQueue;
    private bool EndTrigger;
    private bool isMoving;
    private AtkData _atkData;
    Monster monster;

    public SwordManStringAtkStrategy(Monster m, GameObject atk, float speed, float time)
    {
        monster = m;
        _atkObject = atk.transform;
        _speed = speed;
        _totalTime = time;
        _atkData = new AtkData(monster.CampID, 1, monster._transform);
        StrategyReset();
    }

    public void Atk(Action endNotice)
    {
        _timer -= _deltaTime;

        if (_eventQueue.Count() > 0 && _eventQueue.Peek().TriggerTime >= _timer)
        {
            var timedEvent = _eventQueue.Dequeue();
            timedEvent.Callback?.Invoke();
        }

        if (_timer <= 0 && !EndTrigger)
        {
            EndTrigger = true;
            endNotice();
        }

        if (isMoving)
        {
            switch (monster.face)
            {
                case Monster.Face.Right:
                    monster.Move(_speed, _deltaTime);
                    break;
                case Monster.Face.Left:
                    monster.Move(-_speed, _deltaTime);
                    break;
            }
        }
    }
    public void StrategyReset()
    {
        ResetTimeEvent();
        _timer = _totalTime;
        EndTrigger = false;
        isMoving = false;
    }

    private void ResetTimeEvent()
    {
        _eventQueue = new Queue<TimedEvent>(
            new[]
            {
                new TimedEvent { TriggerTime = _totalTime - 0.35f, Callback = Event1 },
                new TimedEvent { TriggerTime = _totalTime - 0.8f, Callback = Event2 },
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
}
