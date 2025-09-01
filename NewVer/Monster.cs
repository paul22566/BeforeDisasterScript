using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public abstract class Monster : Creature
{
    protected int Hp;
    protected float NormalSpeed;
    public Transform _transform;
    protected MonsterStatusManager _statusManager;
    protected Vector3 _targetPosition;
    public enum Status { Alert, Walk, Run, Attack, Cooldown }
    public float DistanceWithTargetX { get; protected set; } = 0;
    public float DistanceWithTargetY { get; protected set; } = 0;
    protected Action OnCalculateParameter;

    public abstract void ParameterCalculate();
    public abstract void HurtedControll(int damage);
    public void UpdateLogic(float deltaTime)
    {
        ParameterCalculate();
        _statusManager.Update(deltaTime);
    }
    public void FixedUpdateLogic(float deltaTime)
    {
        _statusManager.FixedUpdate(deltaTime);
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
    private IMonsterStatus _currentStatus;

    public MonsterStatusManager(Monster m, IMonsterStatus initStatus)
    {
        _monster = m;
        ChangeStatus(initStatus);
    }

    public void ChangeStatus(IMonsterStatus newStatus)
    {
        _currentStatus?.End(_monster);
        _currentStatus = newStatus;
        _currentStatus.Begin(_monster);
    }
    public void Update(float deltaTime)
    {
        if(_currentStatus == null)
            return;

        _currentStatus.Execute(_monster, deltaTime);

        var next = _currentStatus.CheckTransition();
        if(next != null && next != _currentStatus)
        {
            ChangeStatus(next);
        }
    }
    public void FixedUpdate(float deltaTime)
    {
        if (_currentStatus == null)
            return;

        _currentStatus.FixedExecute(_monster, deltaTime);
    }
}

[Serializable]
public class TransitionRule
{
    public Monster.Status FromStatus;
    public TransitionCondition Condition;
    public Monster.Status ToStatus;
}
public struct StatusTransition
{
    public Func<bool> Condition;
    public IMonsterStatus TargetStatus;

    public StatusTransition(Func<bool> condition, IMonsterStatus targetStatus)
    {
        Condition = condition;
        TargetStatus = targetStatus;
    }
}

public abstract class BaseMonsterStatus : IMonsterStatus
{
    protected readonly List<StatusTransition> transitions = new();

    public void AddTransition(Func<bool> condition, IMonsterStatus target)
    {
        transitions.Add(new StatusTransition(condition, target));
    }

    public virtual void Begin(Monster monster) { }
    public virtual void Execute(Monster monster, float deltaTime) { }
    public virtual void FixedExecute(Monster monster, float deltaTime) { }
    public virtual void End(Monster monster) { }

    public IMonsterStatus CheckTransition()
    {
        foreach (var t in transitions)
        {
            if (t.Condition()) return t.TargetStatus;
        }
        return null; // 沒有符合條件
    }
}
public abstract class BaseMonsterStrategy
{
    protected AnimationController _ani;

    public abstract void Begin();
    public abstract void End();
}

public class MonsterAlertStatus : BaseMonsterStatus
{
    private IMonsterPatrolStrategy _currentStrategy;
    public MonsterAlertStatus(IMonsterPatrolStrategy strategy, AnimationController aniController)
    {
        _currentStrategy = strategy;
    }

    public override void Execute(Monster monster, float deltaTime)
    {
        _currentStrategy.Execute(deltaTime);
    }
    public override void FixedExecute(Monster monster, float deltaTime)
    {
        _currentStrategy.FixedExecute(deltaTime);
    }
    public override void Begin(Monster monster)
    {
        ((BaseMonsterStrategy)_currentStrategy).Begin();
    }
    public override void End(Monster monster)
    {
        ((BaseMonsterStrategy)_currentStrategy).End();
    }
}
public class MonsterWalkPatrolStrategy: BaseMonsterStrategy, IMonsterPatrolStrategy
{
    private float _speed;
    private float _totalTime;
    private float _timer;
    private Queue<TimedEvent> _eventsQueue;
    private bool isMoving;
    private Creature.Face _initialFace;
    private Monster monster;

    public MonsterWalkPatrolStrategy(Monster m, AnimationController ani, Monster.Face initFace, float speed, float time)
    {
        monster = m;
        _ani = ani;
        _initialFace = initFace;
        _speed = speed;
        _totalTime = time;
    }

    public override void Begin()
    {
        _ani.AniPlay();
        _ani.AniTurnFace(monster.face);

        StrategyReset();
    }
    public override void End()
    {
        _ani.AniStop();
    }
    public void Execute(float deltaTime)
    {
        _ani.AniTurnFace(monster.face);
    }
    public void FixedExecute(float deltaTime)
    {
        _timer -= deltaTime;

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
                    monster.Move(_speed, deltaTime);
                    break;
                case Monster.Face.Left:
                    monster.Move(-_speed, deltaTime);
                    break;
            }
        }
    }

    private void StrategyReset()
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
    private void BeginMove()
    {
        isMoving = true;
        monster.TurnFace(_initialFace);
        _ani.ChangeAnimator("isSlowWalk", "true");
    }
    private void StopMove()
    {
        isMoving = false;
        _ani.ChangeAnimator("isSlowWalk", "false");
    }
    private void TurnFaceAndMove()
    {
        isMoving = true;
        monster.TurnFace();
        _ani.ChangeAnimator("isSlowWalk", "true");
    }
}

public class MonsterWalkStatus: BaseMonsterStatus
{
    private float Speed;
    private AnimationController _aniController;

    public MonsterWalkStatus(float speed, AnimationController aniController)
    {
        Speed = speed;
        _aniController = aniController;
    }
    public override void Execute(Monster monster, float deltaTime)
    {
        monster.FaceTarget();
        _aniController.AniTurnFace(monster.face);
    }
    public override void FixedExecute(Monster monster, float deltaTime)
    {
        switch (monster.face)
        {
            case Creature.Face.Left:
                monster.Move(-Speed, deltaTime);
                break;
            case Creature.Face.Right:
                monster.Move(Speed, deltaTime);
                break;
        }
    }
    public override void Begin(Monster monster)
    {
        _aniController.AniPlay();
        _aniController.AniTurnFace(monster.face);
    }
    public override void End(Monster monster)
    {
        _aniController.AniStop();
    }
}
public class MonsterRunStatus : BaseMonsterStatus
{
    private float Speed;
    private AnimationController _aniController;

    public MonsterRunStatus(float speed, AnimationController aniController)
    {
        Speed = speed;
        _aniController = aniController;
    }

    public override void Execute(Monster monster, float deltaTime)
    {
        monster.FaceTarget();
        _aniController.AniTurnFace(monster.face);
    }
    public override void FixedExecute(Monster monster, float deltaTime)
    {
        switch (monster.face)
        {
            case Monster.Face.Left:
                monster.Move(-Speed, deltaTime);
                break;
            case Monster.Face.Right:
                monster.Move(Speed, deltaTime);
                break;
        }
    }
    public override void Begin(Monster monster)
    {
        _aniController.AniPlay();
        _aniController.AniTurnFace(monster.face);
    }
    public override void End(Monster monster)
    {
        _aniController.AniStop();
    }
}

public class MonsterCooldownStatus : BaseMonsterStatus, IOnceStrategyUser
{
    private IMonsterCooldownStrategy _currentStrategy;

    public MonsterCooldownStatus(IMonsterCooldownStrategy strategy)
    {
        _currentStrategy = strategy;
    }
    public void AddDefaultNextStatus(IMonsterStatus DefaultNextStatus)
    {
        AddTransition(DetectStrategyEnd, DefaultNextStatus);
    }

    public override void Execute(Monster monster, float deltaTime)
    {
        _currentStrategy.Execute(monster, deltaTime);
    }
    public override void FixedExecute(Monster monster, float deltaTime)
    {
        _currentStrategy.FixedExecute(monster, deltaTime);
    }
    public override void Begin(Monster monster)
    {
        ((BaseMonsterStrategy)_currentStrategy).Begin();
    }
    public override void End(Monster monster)
    {
        ((BaseMonsterStrategy)_currentStrategy).End();
    }
    public bool DetectStrategyEnd()
    {
        return ((IOnceStrategy)_currentStrategy).ExecuteComplete();
    }
}
public class MonsterWaitCooldownStrategy : BaseMonsterStrategy, IMonsterCooldownStrategy, IOnceStrategy
{
    private Monster monster;
    private float _totalTime;
    private float _timer;
    private bool EndTrigger;

    public MonsterWaitCooldownStrategy(Monster m, AnimationController ani, float time)
    {
        monster = m;
        _ani = ani;
        _totalTime = time;
    }

    public override void Begin()
    {
        _ani.AniPlay();
        _ani.AniTurnFace(monster.face);

        _timer = _totalTime;
        EndTrigger = false;
    }
    public override void End()
    {
        _ani.AniStop();
    }
    public void Execute(Monster monster, float deltaTime)
    {
        monster.FaceTarget();
        _ani.AniTurnFace(monster.face);
    }
    public void FixedExecute(Monster monster, float deltaTime)
    {
        _timer -= deltaTime;

        if (_timer <= 0 && !EndTrigger)
        {
            EndTrigger = true;
        }
    }

    public bool ExecuteComplete()
    {
        return EndTrigger;
    }
}
public class MonsterGoBackCooldownStrategy : BaseMonsterStrategy, IMonsterCooldownStrategy, IOnceStrategy
{
    private Monster monster;
    private float _speed;
    private float _totalTime;
    private float _timer;
    private bool EndTrigger;

    public MonsterGoBackCooldownStrategy(Monster m, AnimationController ani, float speed, float time)
    {
        monster = m;
        _ani = ani;
        _speed = speed;
        _totalTime = time;
    }

    public override void Begin()
    {
        _ani.AniPlay();
        _ani.AniTurnFace(monster.face);

        _timer = _totalTime;
        EndTrigger = false;
    }
    public override void End()
    {
        _ani.AniStop();
    }

    public void Execute(Monster monster, float deltaTime)
    {
        monster.FaceTarget();
        _ani.AniTurnFace(monster.face);
    }
    public void FixedExecute(Monster monster, float deltaTime)
    {
        _timer -= deltaTime;

        switch (monster.face)
        {
            case Monster.Face.Left:
                monster.Move(_speed, deltaTime);
                break;
            case Monster.Face.Right:
                monster.Move(-_speed, deltaTime);
                break;
        }

        if (_timer <= 0 && !EndTrigger)
        {
            EndTrigger = true;
        }
    }
    public bool ExecuteComplete()
    {
        return EndTrigger;
    }
}

public class MonsterAtkStatus : BaseMonsterStatus, IOnceStrategyUser
{
    private IMonsterAtkStrategy _currentStrategy;
    private Func<IMonsterAtkStrategy> _strategyJudge;

    public MonsterAtkStatus(Func<IMonsterAtkStrategy> strategyJudge)
    {
        _strategyJudge = strategyJudge;
    }
    public void AddDefaultNextStatus(IMonsterStatus DefaultNextStatus)
    {
        AddTransition(DetectStrategyEnd, DefaultNextStatus);
    }

    public override void FixedExecute(Monster monster, float deltaTime)
    {
        _currentStrategy.FixedExecute(deltaTime);
    }
    public override void Begin(Monster monster)
    {
        _currentStrategy = _strategyJudge?.Invoke();
        ((BaseMonsterStrategy)_currentStrategy).Begin();
    }
    public override void End(Monster monster)
    {
        ((BaseMonsterStrategy)_currentStrategy).End();
    }
    public bool DetectStrategyEnd()
    {
        return ((IOnceStrategy)_currentStrategy).ExecuteComplete();
    }
}
