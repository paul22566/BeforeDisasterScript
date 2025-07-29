using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : Creature
{
    protected int Hp;
    protected float NormalSpeed;
    public Transform _transform;
    protected MonsterStatusManager _statusManager;
    protected Vector3 _targetPosition;

    protected float DistanceWithTargetX = 0;
    protected float DistanceWithTargetY = 0;
    protected Action OnCalculateParameter;

    public abstract void StatusJudge();
    public abstract void ParameterCalculate();
    public abstract void HurtedControll(int damage);
    public virtual (object strategy, AnimationController ani)? GetStrategyAndAniForStatus(IMonsterStatus status)
    {
        return null; // 預設不提供任何策略
    }

    public void UpdateLogic()
    {
        ParameterCalculate();
        _statusManager.StatusJudgeInManager(StatusJudge);
    }
    public void DoAction()
    {
        _statusManager.DoAction();
    }
    public void Move(float Speed, float DeltaTime)
    {
        _transform.localPosition = new Vector3(_transform.localPosition.x + Speed * DeltaTime, _transform.localPosition.y, 0);
    }
    public void FaceTarget()
    {
        float relativeDistance = DistanceClass.CalculateRelativelyDistanceX(_transform.position, _targetPosition);

        face = relativeDistance > 0 ? Face.Left : Face.Right;
    }
}

public class MonsterStatusManager
{
    private Monster _monster;
    private IMonsterStatus status;
    public IMonsterStatus preStatus;

    public MonsterStatusManager(Monster m, IMonsterStatus initStatus)
    {
        _monster = m;
        SetPreStatus(initStatus);
        ConfirmStatus();
    }

    public void StatusJudgeInManager(Action judgeFunc)
    {
        if (status.leaveJudge(_monster))
        {
            judgeFunc?.Invoke();
        }
    }

    public void SetPreStatus(IMonsterStatus _status)
    {
        preStatus = _status;
    }

    public bool ConfirmStatus()
    {
        if (status == preStatus || preStatus == null)
        {
            return false;
        }

        if (status != null)
        {
            status.End(_monster);
        }
        status = preStatus;

        var data = _monster.GetStrategyAndAniForStatus(preStatus);

        if (data != null)
        {
            var (strategyObj, aniController) = data.Value;

            // 泛型策略處理：AtkStrategy
            if (status is IStrategyUser<IMonsterAtkStrategy> atkUser && strategyObj is IMonsterAtkStrategy atk)
            {
                atkUser.SetStrategy(atk);
                atkUser.SetAni(aniController);
            }
        }

        status.Begin(_monster);

        preStatus = null;
        return true;
    }

    public bool EqualStatus(IMonsterStatus _status)
    {
        return status == _status;
    }

    public void DoAction()
    {
        status.action(_monster);
    }
}


public class MonsterAlertStatus: IMonsterStatus, IStrategyUser<IMonsterPatrolStrategy>
{
    private IMonsterPatrolStrategy _strategy;
    private Func<bool> _breakJudge;
    private AnimationController _aniController;
    public MonsterAlertStatus(IMonsterPatrolStrategy strategy, Func<bool> breakJudge, AnimationController aniController)
    {
        SetStrategy(strategy);
        _breakJudge = breakJudge;
        _aniController = aniController;
    }

    public void action(Monster monster)
    {
        _aniController.AniTurnFace(monster.face);
        _strategy.Patrol();
    }
    public bool leaveJudge(Monster monster)
    {
        return _breakJudge?.Invoke() ?? false;
    }
    public void Begin(Monster monster)
    {
        _aniController.AniTurnFace(monster.face);
    }
    public void End(Monster monster)
    {
        _aniController.AniStop();
        if (_strategy is IStrategyReset resettable)
        {
            resettable.StrategyReset();
        }
    }

    public void SetAni(AnimationController ani)
    {
        _aniController = ani;
    }
    public void SetStrategy(IMonsterPatrolStrategy strategy)
    {
        _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
    }
}
public class MonsterWalkPatrolStrategy: IMonsterPatrolStrategy, IStrategyReset
{
    private float _speed;
    private float _totalTime;
    private float _timer;
    private float _deltaTime = Time.fixedDeltaTime;
    private Queue<TimedEvent> _eventsQueue;
    private bool isMoving;
    private Creature.Face _initialFace;
    private AnimationController _ani;
    private Monster monster;

    public MonsterWalkPatrolStrategy(Monster m, AnimationController ani, Monster.Face initFace, float speed, float time)
    {
        monster = m;
        _ani = ani;
        _initialFace = initFace;
        _speed = speed;
        _totalTime = time;
        StrategyReset();
    }

    public void Patrol()
    {
        _timer -= _deltaTime;

        if (_eventsQueue.Count > 0 && _timer <= _eventsQueue.Peek().TriggerTime)
        {
            var timedEvent = _eventsQueue.Dequeue();
            timedEvent.Callback?.Invoke();
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
        _timer = _totalTime;
        ResetTimeEvent();
    }

    private void ResetTimeEvent()
    {
        _eventsQueue = new Queue<TimedEvent>(
            new[]
            {
                new TimedEvent { TriggerTime = _totalTime, Callback = BeginMove },
                new TimedEvent { TriggerTime = _totalTime - 5, Callback = StopMove },
                new TimedEvent { TriggerTime = _totalTime - 7, Callback = TurnFaceAndMove },
                new TimedEvent { TriggerTime = _totalTime - 12, Callback = StopMove },
                new TimedEvent { TriggerTime = 0, Callback = StrategyReset }
            }
        );
    }

    public void BeginMove()
    {
        isMoving = true;
        monster.TurnFace(_initialFace);
        _ani.ChangeAnimator("isSlowWalk", "true");
    }
    public void StopMove()
    {
        isMoving = false;
        _ani.ChangeAnimator("isSlowWalk", "false");
    }
    public void TurnFaceAndMove()
    {
        isMoving = true;
        monster.TurnFace();
        _ani.ChangeAnimator("isSlowWalk", "true");
    }
}

public class MonsterWalkStatus: IMonsterStatus
{
    private float Speed;
    private float DeltaTime = Time.fixedDeltaTime;
    private Func<bool> _breakJudge;
    private AnimationController _aniController;

    public MonsterWalkStatus(float speed, Func<bool> breakJudge, AnimationController aniController)
    {
        Speed = speed;
        _breakJudge = breakJudge;
        _aniController = aniController;
    }

    public void action(Monster monster)
    {
        monster.FaceTarget();
        _aniController.AniTurnFace(monster.face);

        switch (monster.face)
        {
            case Creature.Face.Left:
                monster.Move(-Speed, DeltaTime);
                break;
            case Creature.Face.Right:
                monster.Move(Speed, DeltaTime);
                break;
        }
    }

    public bool leaveJudge(Monster monster)
    {
        return _breakJudge?.Invoke() ?? false;
    }

    public void Begin(Monster monster)
    {
        _aniController.AniPlay();
        _aniController.AniTurnFace(monster.face);
    }

    public void End(Monster monster)
    {
        _aniController.AniStop();
    }
}
public class MonsterRunStatus : IMonsterStatus
{
    private float Speed;
    private float DeltaTime = Time.fixedDeltaTime;
    private Func<bool> _breakJudge;
    private AnimationController _aniController;

    public MonsterRunStatus(float speed, Func<bool> breakJudge, AnimationController aniController)
    {
        Speed = speed;
        _breakJudge = breakJudge;
        _aniController = aniController;
    }

    public void action(Monster monster)
    {
        monster.FaceTarget();
        _aniController.AniTurnFace(monster.face);

        switch (monster.face)
        {
            case Monster.Face.Left:
                monster.Move(-Speed, DeltaTime);
                break;
            case Monster.Face.Right:
                monster.Move(Speed, DeltaTime);
                break;
        }
    }

    public bool leaveJudge(Monster monster)
    {
        return _breakJudge?.Invoke() ?? false;
    }

    public void Begin(Monster monster)
    {
        _aniController.AniPlay();
        _aniController.AniTurnFace(monster.face);
    }

    public void End(Monster monster)
    {
        _aniController.AniStop();
    }
}

public class MonsterCooldownStatus : IMonsterStatus, IStrategyUser<IMonsterCooldownStrategy>, IOnceStrategyUser
{
    private bool _cooldownEnd;
    private IMonsterCooldownStrategy _strategy;
    private Func<bool> _breakJudge;
    private AnimationController _aniController;

    public MonsterCooldownStatus(IMonsterCooldownStrategy strategy, Func<bool> breakJudge, AnimationController aniController)
    {
        _breakJudge = breakJudge;
        _aniController = aniController;
        _strategy = strategy;
    }

    public void action(Monster monster)
    {
        _aniController.AniTurnFace(monster.face);
        _strategy.CooldownAction(monster, StrategyEnd);
    }

    public bool leaveJudge(Monster monster)
    {
        bool breakResult = false;

        try
        {
            breakResult = _breakJudge?.Invoke() ?? false;
        }
        catch (Exception ex)
        {
            Debug.Log("Invoke 失敗：" + ex.Message);
        }

        if (breakResult || _cooldownEnd)
        {
            return true;
        }

        return false;
    }

    public void Begin(Monster monster)
    {
        _aniController.AniPlay();
        _aniController.AniTurnFace(monster.face);
    }

    public void End(Monster monster)
    {
        _cooldownEnd = false;
        _aniController.AniStop();
        if (_strategy is IStrategyReset resettable)
        {
            resettable.StrategyReset();
        }
    }

    public void SetStrategy(IMonsterCooldownStrategy strategy)
    {
        _strategy = strategy ?? throw new ArgumentException("Invalid strategy type for MonsterAtkStatus");
    }
    public void SetAni(AnimationController ani)
    {
        _aniController = ani;
    }
    public void StrategyEnd()
    {
        _cooldownEnd = true;
    }
}
public class MonsterWaitCooldownStrategy : IMonsterCooldownStrategy
{
    private float _totalTime;
    private float _timer;
    private bool EndTrigger;
    private float DeltaTime = Time.fixedDeltaTime;

    public MonsterWaitCooldownStrategy(float time)
    {
        _totalTime = time;
        ResetValue();
    }

    public void CooldownAction(Monster monster, Action endAction)
    {
        _timer -= DeltaTime;

        monster.FaceTarget();

        if (_timer <= 0 && !EndTrigger)
        {
            EndTrigger = true;
            endAction();
        }
    }

    private void ResetValue()
    {
        _timer = _totalTime;
        EndTrigger = false;
    }
}
public class MonsterGoBackCooldownStrategy : IMonsterCooldownStrategy, IStrategyReset
{
    private float _speed;
    private float _totalTime;
    private float _timer;
    private bool EndTrigger;
    private float DeltaTime = Time.fixedDeltaTime;

    public MonsterGoBackCooldownStrategy(float speed, float time)
    {
        _speed = speed;
        _totalTime = time;
        StrategyReset();
    }

    public void CooldownAction(Monster monster, Action endAction)
    {
        _timer -= DeltaTime;

        monster.FaceTarget();

        switch (monster.face)
        {
            case Monster.Face.Left:
                monster.Move(_speed, DeltaTime);
                break;
            case Monster.Face.Right:
                monster.Move(-_speed, DeltaTime);
                break;
        }

        if (_timer <= 0 && !EndTrigger)
        {
            EndTrigger = true;
            endAction();
        }
    }

    public void StrategyReset()
    {
        _timer = _totalTime;
        EndTrigger = false;
    }
}

public class MonsterAtkStatus : IMonsterStatus, IStrategyUser<IMonsterAtkStrategy>, IOnceStrategyUser
{
    private bool _atkEnd = false;
    private IMonsterAtkStrategy _strategy;
    private Func<bool> _breakJudge;
    private AnimationController _aniController;

    public MonsterAtkStatus(IMonsterAtkStrategy strategy, Func<bool> breakJudge, AnimationController aniController)
    {
        SetStrategy(strategy);
        _breakJudge = breakJudge;
        _aniController = aniController;
    }

    public void action(Monster monster)
    {
        if(!_atkEnd)
            _strategy.Atk(StrategyEnd);
    }
    public bool leaveJudge(Monster monster)
    {
        bool breakResult = false;

        try
        {
            breakResult = _breakJudge?.Invoke() ?? false;
        }
        catch (Exception ex)
        {
            Debug.Log("Invoke 失敗：" + ex.Message);
        }

        if (breakResult || _atkEnd)
        {
            return true;
        }

        return false;
    }
    public void Begin(Monster monster)
    {
        _aniController.AniPlay();
        _aniController.AniTurnFace(monster.face);
    }
    public void End(Monster monster)
    {
        _atkEnd = false;
        if (_strategy is IStrategyReset resettable)
        {
            resettable.StrategyReset();
        }
        _aniController.AniStop();
    }

    public void SetStrategy(IMonsterAtkStrategy strategy)
    {
        _strategy = strategy ?? throw new ArgumentException("Invalid strategy type for MonsterAtkStatus");
    }
    public void SetAni(AnimationController ani)
    {
        _aniController = ani;
    }
    public void StrategyEnd()
    {
        _atkEnd = true;
    }
}
