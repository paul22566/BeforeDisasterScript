using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager;
using UnityEngine;
using static PlayerStatus;

public abstract class PlayerStatus
{
    public enum Status
    {
        Wait, Gliding, LeftMove, RightMove, Accumulate, NormalAtk, StrongAtk, 
        CriticAtk, Jump, JumpAtk, JumpThrow, Restore, UseNormalItem, UseThrowItem, 
        Dash, Shoot, Block,BlockAtk, BeBlock, Weak, WalkThrow, Hurted, FallWait, Die,
        ImpulseJump
    };
    public enum OperateType { Update, FixedUpdate, Both}
    protected PlayerController _controller;
    protected OperateType _operateType;
    protected Status _statusType;
    public HashSet<Status> CommandReplaceSet = new HashSet<Status>();
    public HashSet<Status> CommandCoexistSet = new HashSet<Status>();
    protected bool isInitializeSuccess;

    public abstract void Inisialize(Status status, OperateType operateType);
    public abstract void Begin();
    public abstract void End();
    public virtual void Execute(float deltaTime) { }
    public virtual void FixedExecute(float deltaTime) { }

    public virtual bool AddCommandToSet()
    {
        HashSet<PlayerStatus> TargetSet;

        switch (_operateType)
        {
            case OperateType.Update:
                TargetSet = _controller.UpdateOperateCommands;

                 return AddCommand(this, TargetSet);
            case OperateType.FixedUpdate:
                TargetSet = _controller.FixedUpdateOperateCommands;

                return AddCommand(this, TargetSet);
            case OperateType.Both:
                if (_controller.UpdateOperateCommands.Contains(this))
                {
                    return true;
                }
                else if (_controller.CheckCommandApply(_statusType))
                {
                    _controller.UpdateOperateCommands.Add(this);
                    _controller.FixedUpdateOperateCommands.Add(this);
                    Begin();
                    return true;
                }
                break;
        }

        return false;
    }//在架上就會回傳true
    private bool AddCommand(PlayerStatus TargetCommand, HashSet<PlayerStatus> TargetSet)
    {
        if (TargetSet.Contains(TargetCommand))
        {
            return true;
        }
        else if (_controller.CheckCommandApply(_statusType))
        {
            TargetSet.Add(TargetCommand);
            Begin();
            return true;
        }

        return false;
    }
    public virtual void RemoveCommandFromSet()
    {
        switch (_operateType)
        {
            case OperateType.Update:
                if (_controller.UpdateOperateCommands.Contains(this))
                {
                    _controller.UpdateOperateCommands.Remove(this);
                    End();
                }
                break;
            case OperateType.FixedUpdate:
                if (_controller.FixedUpdateOperateCommands.Contains(this))
                {
                    _controller.FixedUpdateOperateCommands.Remove(this);
                    End();
                }
                break;
            case OperateType.Both:
                if (_controller.UpdateOperateCommands.Contains(this))
                {
                    _controller.FixedUpdateOperateCommands.Remove(this);
                    _controller.UpdateOperateCommands.Remove(this);
                    End();
                }
                break;
        }
    }
}

public class PlayerWaitStatus : PlayerStatus, IPlayerAniUser
{
    private PlayerWaitAni _ani;

    public PlayerWaitStatus(PlayerController controller, PlayerWaitAni ani)
    {
        _controller = controller;
        _ani = ani;
        Inisialize(Status.Wait, OperateType.Update);
    }

    public int GetAnimationPriority()
    {
        return _ani.PlayPriority;
    }
    public AnimationController GetAnimation()
    {
        return _ani;
    }
    public override bool AddCommandToSet()
    {
        if (_controller.UpdateOperateCommands.Contains(this))
        {
            return true;
        }

        if (_controller.CheckOperateSetEmpty())
        {
            _controller.UpdateOperateCommands.Add(this);
            Begin();
            return true;
        }
        else if (_controller.CheckCommandApply(_statusType))
        {
            _controller.UpdateOperateCommands.Add(this);
            Begin();
            return true;
        }

        return false;
    }
    public override void RemoveCommandFromSet()
    {
        if (_controller.UpdateOperateCommands.Contains(this))
        {
            _controller.UpdateOperateCommands.Remove(this);
            End();
        }
    }
    public override void Inisialize(Status status, OperateType operateType)
    {
        _statusType = status;
        _operateType = operateType;

        CommandCoexistSet.Add(Status.Accumulate);

        CommandReplaceSet.Add(Status.LeftMove);
        CommandReplaceSet.Add(Status.RightMove);
        CommandReplaceSet.Add(Status.Jump);
        CommandReplaceSet.Add(Status.JumpAtk);
        CommandReplaceSet.Add(Status.WalkThrow);
        CommandReplaceSet.Add(Status.Shoot);
        CommandReplaceSet.Add(Status.Hurted);
        CommandReplaceSet.Add(Status.Die);
        CommandReplaceSet.Add(Status.NormalAtk);
        CommandReplaceSet.Add(Status.StrongAtk);
        CommandReplaceSet.Add(Status.CriticAtk);
        CommandReplaceSet.Add(Status.UseNormalItem);
        CommandReplaceSet.Add(Status.UseThrowItem);
        CommandReplaceSet.Add(Status.Restore);
        CommandReplaceSet.Add(Status.Dash);
        CommandReplaceSet.Add(Status.Block);
        CommandReplaceSet.Add(Status.BeBlock);
        CommandReplaceSet.Add(Status.Weak);
        CommandReplaceSet.Add(Status.ImpulseJump);
    }
    public override void Begin()
    {
        _controller.SetAnimation(_ani);
        _controller.PlayAnimation();
        _ani.AniTurnFace(PlayerController._player.face);
    }
    public override void End()
    {
        _controller.StopAnimation(_ani);
    }
    public override void Execute(float deltaTime)
    {
        if (!PlayerController.isGround)
        {
            RemoveCommandFromSet();
        }
    }
}
public class PlayerGlidingStatus : PlayerStatus, IPlayerAniUser
{
    private PlayerJumpAni _ani;
    private PlayerFallWaitStatus _fallWaitStatus;
    private float LongFallTimerSet;
    private float LongFallTimer;

    public PlayerGlidingStatus(PlayerController controller, PlayerJumpAni ani, PlayerFallWaitStatus fallWaitStatus, float longFallTimerSet)
    {
        _controller = controller;
        _fallWaitStatus = fallWaitStatus;
        _ani = ani;
        Inisialize(Status.Gliding, OperateType.Update);
        LongFallTimerSet = longFallTimerSet;
        LongFallTimer = 0;
    }

    public int GetAnimationPriority()
    {
        return _ani.PlayPriority;
    }
    public AnimationController GetAnimation()
    {
        return _ani;
    }
    public override bool AddCommandToSet()
    {
        if (_controller.UpdateOperateCommands.Contains(this))
        {
            return true;
        }

        if (_controller.CheckOperateSetEmpty())
        {
            _controller.UpdateOperateCommands.Add(this);
            Begin();
            return true;
        }
        else if (_controller.CheckCommandApply(_statusType))
        {
            _controller.UpdateOperateCommands.Add(this);
            Begin();
            return true;
        }
        return false;
    }
    public override void RemoveCommandFromSet()
    {
        if (_controller.UpdateOperateCommands.Contains(this))
        {
            _controller.UpdateOperateCommands.Remove(this);
            End();
        }
    }
    public override void Inisialize(Status status, OperateType operateType)
    {
        _statusType = status;
        _operateType = operateType;

        CommandCoexistSet.Add(Status.Accumulate);
        CommandCoexistSet.Add(Status.LeftMove);
        CommandCoexistSet.Add(Status.RightMove);

        CommandReplaceSet.Add(Status.FallWait);
        CommandReplaceSet.Add(Status.Jump);
        CommandReplaceSet.Add(Status.JumpAtk);
        CommandReplaceSet.Add(Status.JumpThrow);
        CommandReplaceSet.Add(Status.Shoot);
        CommandReplaceSet.Add(Status.Hurted);
        CommandReplaceSet.Add(Status.Die);
        CommandReplaceSet.Add(Status.StrongAtk);
        CommandReplaceSet.Add(Status.Dash);
        CommandReplaceSet.Add(Status.Weak);
        CommandReplaceSet.Add(Status.ImpulseJump);
    }
    public override void Begin()
    {
        _controller.SetAnimation(_ani);
        _controller.PlayAnimation();
        if (_controller.GetYVelocity() < 0)
        {
            _ani.ChangeAnimator("isFalling", "true");
        }
        _ani.AniTurnFace(PlayerController._player.face);
        LongFallTimer = LongFallTimerSet;
    }
    public override void End()
    {
        LongFallTimer = 0;
        _controller.StopAnimation(_ani);
    }
    public override void Execute(float deltaTime)
    {
        if (LongFallTimer > 0)
        {
            LongFallTimer -= deltaTime;
        }
        
        if (_controller.GetYVelocity() < 0)
        {
            _ani.ChangeAnimator("isFalling", "true");
        }
        _ani.AniTurnFace(PlayerController._player.face);

        if (PlayerController.isGround)
        {
            if (LongFallTimer <= 0)
            {
                _fallWaitStatus.AddCommandToSet();
            }
            else
            {
                RemoveCommandFromSet();
            }
        }
    }
}
public class PlayerFallWaitStatus : PlayerStatus, IPlayerAniUser
{
    private PlayerJumpAni _ani;
    private float LongFallWaitTimerSet;
    private float LongFallWaitTimer;

    public PlayerFallWaitStatus(PlayerController controller, PlayerJumpAni ani, float longFallTimerSet)
    {
        _controller = controller;
        _ani = ani;
        Inisialize(Status.FallWait, OperateType.Update);
        LongFallWaitTimerSet = longFallTimerSet;
        LongFallWaitTimer = LongFallWaitTimerSet;
    }

    public int GetAnimationPriority()
    {
        return _ani.PlayPriority;
    }
    public AnimationController GetAnimation()
    {
        return _ani;
    }
    public override void Inisialize(Status status, OperateType operateType)
    {
        _statusType = status;
        _operateType = operateType;

        CommandReplaceSet.Add(Status.Hurted);
    }
    public override void Begin()
    {
        _controller.SetAnimation(_ani);
        _controller.PlayAnimation();
        _ani.ChangeAnimator("isFalling", "true");
        _ani.ChangeAnimator("LongFall", "true");
        _ani.AniTurnFace(PlayerController._player.face);
    }
    public override void End()
    {
        LongFallWaitTimer = LongFallWaitTimerSet;
        _controller.StopAnimation(_ani);
    }
    public override void Execute(float deltaTime)
    {
        LongFallWaitTimer -= deltaTime;

        if (LongFallWaitTimer <= 0)
        {
            RemoveCommandFromSet();
        }
    }
}
public class PlayerLeftMoveStatus : PlayerStatus, IObserver, IPlayerAniUser
{
    private PlayerRunAni _ani;
    private Action<float, float> Move;
    private float Speed;
    private float SpeedSet;
    private bool isUsing;
    private bool isShooting;

    public PlayerLeftMoveStatus(PlayerController controller, PlayerRunAni ani, float speed,Action<float, float> move)
    {
        _controller = controller;
        _ani = ani;
        SpeedSet = speed;
        Speed = SpeedSet;
        Move = move;
        Inisialize(Status.LeftMove, OperateType.Both);
    }

    public int GetAnimationPriority()
    {
        return _ani.PlayPriority;
    }
    public AnimationController GetAnimation()
    {
        return _ani;
    }
    public void ReceiveNotify()
    {
        AddCommandToSet();
    }
    public override void Inisialize(Status status, OperateType operateType)
    {
        _statusType = status;
        _operateType = operateType;

        CommandCoexistSet.Add(Status.Gliding);
        CommandCoexistSet.Add(Status.Accumulate);
        CommandCoexistSet.Add(Status.Jump);
        CommandCoexistSet.Add(Status.JumpAtk);
        CommandCoexistSet.Add(Status.JumpThrow);
        CommandCoexistSet.Add(Status.WalkThrow);
        CommandCoexistSet.Add(Status.Shoot);

        CommandReplaceSet.Add(Status.FallWait);
        CommandReplaceSet.Add(Status.Hurted);
        CommandReplaceSet.Add(Status.Die);
        CommandReplaceSet.Add(Status.NormalAtk);
        CommandReplaceSet.Add(Status.StrongAtk);
        CommandReplaceSet.Add(Status.CriticAtk);
        CommandReplaceSet.Add(Status.Weak);
        CommandReplaceSet.Add(Status.UseNormalItem);
        CommandReplaceSet.Add(Status.UseThrowItem);
        CommandReplaceSet.Add(Status.Restore);
        CommandReplaceSet.Add(Status.Dash);
        CommandReplaceSet.Add(Status.Block);
        CommandReplaceSet.Add(Status.ImpulseJump);
    }
    public override void Begin()
    {
        _controller.SetAnimation(_ani);
        _controller.PlayAnimation();
        if (!isShooting)
        {
            PlayerController._player.TurnFace(Creature.Face.Left);
            _ani.AniTurnFace(PlayerController._player.face);
        }

        isUsing = true;
    }
    public override void End()
    {
        _controller.StopAnimation(_ani);
        isUsing = false;
    }
    public override void Execute(float deltaTime)
    {
        if (!isShooting)
        {
            PlayerController._player.TurnFace(Creature.Face.Left);
            _ani.AniTurnFace(PlayerController._player.face);
        }

        if (!PlayerController.isGround)
        {
            _controller.AddDefaultMode();
        }
    }
    public override void FixedExecute(float deltaTime)
    {
        Move?.Invoke(-Speed, deltaTime);
    }

    public bool CheckUsing()
    {
        return isUsing;
    }
    public void BeginShoot(float speed)
    {
        Speed = speed;
        isShooting = true;
    }
    public void StopShoot()
    {
        Speed = SpeedSet;
        isShooting = false;
    }
}
public class PlayerRightMoveStatus : PlayerStatus, IObserver, IPlayerAniUser
{
    private PlayerRunAni _ani;
    private Action<float, float> Move;
    private float Speed;
    private float SpeedSet;
    private bool isUsing;
    private bool isShooting;

    public PlayerRightMoveStatus(PlayerController controller, PlayerRunAni ani, float speed, Action<float, float> move)
    {
        _controller = controller;
        _ani = ani;
        SpeedSet = speed;
        Speed = SpeedSet;
        Move = move;
        Inisialize(Status.RightMove, OperateType.Both);
    }

    public void ReceiveNotify()
    {
        AddCommandToSet();
    }
    public int GetAnimationPriority()
    {
        return _ani.PlayPriority;
    }
    public AnimationController GetAnimation()
    {
        return _ani;
    }
    public override void Inisialize(Status status, OperateType operateType)
    {
        _statusType = status;
        _operateType = operateType;

        CommandCoexistSet.Add(Status.Gliding);
        CommandCoexistSet.Add(Status.Accumulate);
        CommandCoexistSet.Add(Status.Jump);
        CommandCoexistSet.Add(Status.JumpAtk);
        CommandCoexistSet.Add(Status.JumpThrow);
        CommandCoexistSet.Add(Status.WalkThrow);
        CommandCoexistSet.Add(Status.Shoot);

        CommandReplaceSet.Add(Status.FallWait);
        CommandReplaceSet.Add(Status.Hurted);
        CommandReplaceSet.Add(Status.Die);
        CommandReplaceSet.Add(Status.NormalAtk);
        CommandReplaceSet.Add(Status.StrongAtk);
        CommandReplaceSet.Add(Status.CriticAtk);
        CommandReplaceSet.Add(Status.Weak);
        CommandReplaceSet.Add(Status.UseNormalItem);
        CommandReplaceSet.Add(Status.UseThrowItem);
        CommandReplaceSet.Add(Status.Restore);
        CommandReplaceSet.Add(Status.Dash);
        CommandReplaceSet.Add(Status.Block);
        CommandReplaceSet.Add(Status.ImpulseJump);
    }
    public override void Begin()
    {
        _controller.SetAnimation(_ani);
        _controller.PlayAnimation();
        if (!isShooting)
        {
            PlayerController._player.TurnFace(Creature.Face.Right);
            _ani.AniTurnFace(PlayerController._player.face);
        }

        isUsing = true;
    }
    public override void End()
    {
        _controller.StopAnimation(_ani);
        isUsing = false;
    }
    public override void Execute(float deltaTime)
    {
        if (!isShooting)
        {
            PlayerController._player.TurnFace(Creature.Face.Right);
            _ani.AniTurnFace(PlayerController._player.face);
        }

        if (!PlayerController.isGround)
        {
            _controller.AddDefaultMode();
        }
    }
    public override void FixedExecute(float deltaTime)
    {
        Move?.Invoke(Speed, deltaTime);
    }

    public bool CheckUsing()
    {
        return isUsing;
    }
    public void BeginShoot(float speed)
    {
        Speed = speed;
        isShooting = true;
    }
    public void StopShoot()
    {
        Speed = SpeedSet;
        isShooting = false;
    }
}
public class PlayerLeftWalkThrowStatus : PlayerStatus, IPlayerAniUser
{
    private PlayerWalkThrowAni _ani;
    private Item _item;
    private IThrowItem _throwItem;
    private float Timer;
    private float TimerSet;
    private Vector3 AppearPlace;
    private Queue<TimedEvent> _eventQueue;
    private (float, float) Power;

    public PlayerLeftWalkThrowStatus(PlayerController controller, PlayerWalkThrowAni ani, BattleSystem battleSystem)
    {
        _controller = controller;
        _ani = ani;
        AppearPlace = battleSystem.WalkThrowItemAppear;
        Power = battleSystem.WalkThrowCocktailPower;

        Inisialize(Status.WalkThrow, OperateType.FixedUpdate);
    }
    public int GetAnimationPriority()
    {
        return _ani.PlayPriority;
    }
    public AnimationController GetAnimation()
    {
        return _ani;
    }
    public override void Inisialize(Status status, OperateType operateType)
    {
        _statusType = status;
        _operateType = operateType;

        CommandCoexistSet.Add(Status.LeftMove);

        CommandReplaceSet.Add(Status.RightMove);
        CommandReplaceSet.Add(Status.Wait);
        CommandReplaceSet.Add(Status.Gliding);
        CommandReplaceSet.Add(Status.Hurted);
        CommandReplaceSet.Add(Status.Die);
        CommandReplaceSet.Add(Status.Jump);
    }
    public override void Begin()
    {
        _controller.SetAnimation(_ani);
        _controller.PlayAnimation();
        _ani.PlayThrowItemAni(_item.Name);
        _ani.AniTurnFace(PlayerController._player.face);
        Timer = TimerSet;
        ResetQueue();
    }
    public override void End()
    {
        _controller.StopAnimation(_ani);
    }

    public override void FixedExecute(float deltaTime)
    {
        Timer -= deltaTime;

        if (_eventQueue.Count() > 0 && _eventQueue.Peek().TriggerTime >= Timer)
        {
            var timedEvent = _eventQueue.Dequeue();
            timedEvent.Callback?.Invoke();
        }
    }
    public void SetItem(Item item, IThrowItem throwItem)
    {
        _item = item;
        TimerSet = _item.GetUseTimer();
        _throwItem = throwItem;
    }
    private void ResetQueue()
    {
        _eventQueue = new Queue<TimedEvent>(
            new[]
            {
                new TimedEvent { TriggerTime = TimerSet - 0.2f, Callback = Event1 },
                new TimedEvent { TriggerTime = 0, Callback = Event2 },
            }
        );
    }
    private void Event1()
    {
        Vector3 location = _controller._transform.localPosition;
        location = new Vector3(location.x - AppearPlace.x, location.y + AppearPlace.y, location.z);
        _throwItem.UseSuccess();
        _throwItem.WalkThrow(Power, location);
    }
    private void Event2()
    {
        RemoveCommandFromSet();
    }
}
public class PlayerRightWalkThrowStatus : PlayerStatus, IPlayerAniUser
{
    private PlayerWalkThrowAni _ani;
    private Item _item;
    private IThrowItem _throwItem;
    private float Timer;
    private float TimerSet;
    private Vector3 AppearPlace;
    private Queue<TimedEvent> _eventQueue;
    private (float, float) Power;

    public PlayerRightWalkThrowStatus(PlayerController controller, PlayerWalkThrowAni ani, BattleSystem battleSystem)
    {
        _controller = controller;
        _ani = ani;
        AppearPlace = battleSystem.WalkThrowItemAppear;
        Power = battleSystem.WalkThrowCocktailPower;
        Power.Item1 = -Power.Item1;

        Inisialize(Status.WalkThrow, OperateType.FixedUpdate);
    }
    public int GetAnimationPriority()
    {
        return _ani.PlayPriority;
    }
    public AnimationController GetAnimation()
    {
        return _ani;
    }
    public override void Inisialize(Status status, OperateType operateType)
    {
        _statusType = status;
        _operateType = operateType;

        CommandCoexistSet.Add(Status.RightMove);

        CommandReplaceSet.Add(Status.LeftMove);
        CommandReplaceSet.Add(Status.Wait);
        CommandReplaceSet.Add(Status.Gliding);
        CommandReplaceSet.Add(Status.Hurted);
        CommandReplaceSet.Add(Status.Die);
        CommandReplaceSet.Add(Status.Jump);
    }
    public override void Begin()
    {
        _controller.SetAnimation(_ani);
        _controller.PlayAnimation();
        _ani.PlayThrowItemAni(_item.Name);
        _ani.AniTurnFace(PlayerController._player.face);
        Timer = TimerSet;
        ResetQueue();
    }
    public override void End()
    {
        _controller.StopAnimation(_ani);
    }

    public override void FixedExecute(float deltaTime)
    {
        Timer -= deltaTime;

        if (_eventQueue.Count() > 0 && _eventQueue.Peek().TriggerTime >= Timer)
        {
            var timedEvent = _eventQueue.Dequeue();
            timedEvent.Callback?.Invoke();
        }
    }
    public void SetItem(Item item, IThrowItem throwItem)
    {
        _item = item;
        TimerSet = _item.GetUseTimer();
        _throwItem = throwItem;
    }
    private void ResetQueue()
    {
        _eventQueue = new Queue<TimedEvent>(
            new[]
            {
                new TimedEvent { TriggerTime = TimerSet - 0.2f, Callback = Event1 },
                new TimedEvent { TriggerTime = 0, Callback = Event2 },
            }
        );
    }
    private void Event1()
    {
        Vector3 location = _controller._transform.localPosition;
        location = new Vector3(location.x + AppearPlace.x, location.y + AppearPlace.y, location.z);
        _throwItem.UseSuccess();
        _throwItem.WalkThrow(Power, location);
    }
    private void Event2()
    {
        RemoveCommandFromSet();
    }
}

public class PlayerDashStatus : PlayerStatus, IPlayerAniUser, IObserver
{
    private PlayerDashAni _ani;
    private float Speed;
    private float TimerSet;
    private float Timer;
    private Action<float, float> Move;
    public PlayerDashStatus(PlayerController controller, PlayerDashAni ani, Action<float, float> move)
    {
        _controller = controller;
        _ani = ani;
        Inisialize(Status.Dash, OperateType.FixedUpdate);

        Speed = _controller.DashSpeed;
        TimerSet = _controller.DashTime;
        Move = move;
    }

    public int GetAnimationPriority()
    {
        return _ani.PlayPriority;
    }
    public AnimationController GetAnimation()
    {
        return _ani;
    }
    public void ReceiveNotify()
    {
        AddCommandToSet();
    }
    public override void Inisialize(Status status, OperateType operateType)
    {
        _statusType = status;
        _operateType = operateType;

        CommandReplaceSet.Add(Status.Hurted);
        CommandReplaceSet.Add(Status.Die);
    }
    public override void Begin()
    {
        _controller.SetAnimation(_ani);
        _controller.PlayAnimation();
        
        _ani.AniTurnFace(PlayerController._player.face);

        _controller.ReadyToDash();
        Timer = TimerSet;

        _controller._invincibleManager.AddInvincible(InvincibleManager.InvincibleType.Weak);
    }
    public override void End()
    {
        _controller.StopAnimation(_ani);
        _controller.DashEnd();
        _controller._invincibleManager.RemoveInvincible(InvincibleManager.InvincibleType.Weak);
    }
    public override void FixedExecute(float deltaTime)
    {
        Timer -= deltaTime;

        ShadowPool.instance.GetFromPool();

        switch (PlayerController._player.face)
        {
            case Creature.Face.Right:
                Move?.Invoke(Speed, deltaTime);
                break;
            case Creature.Face.Left:
                Move?.Invoke(-Speed, deltaTime);
                break;
        }

        if (Timer <= 0)
        {
            RemoveCommandFromSet();
        }
    }
}
public class PlayerJumpStatus : PlayerStatus, IObserver, IPlayerAniUser
{
    private PlayerJumpAni _ani;
    private GameObject SecondJumpFire;
    private Func<bool> _jumpBreakJudge;
    private float JumpForce;
    private int JumpForceCount;
    private float SpeedLimit;
    private float Timer;
    private float TimerSet;
    private int JumpState;
    private bool isLeaveGround;

    private float ConstantNumber;

    public PlayerJumpStatus(PlayerController controller, PlayerJumpAni ani,
        GameObject jumpFire, Func<bool> breakJudge, float Force, float Limit, float SecondJumpTimerSet)
    {
        _controller = controller;
        _ani = ani;
        SecondJumpFire = jumpFire;
        _jumpBreakJudge = breakJudge;
        JumpForce = Force;
        SpeedLimit = Limit;
        TimerSet = SecondJumpTimerSet;

        Inisialize(Status.Jump, OperateType.Both);

        JumpForceCount = 0;

        ConstantNumber = JumpForce / 2 / JumpForce / 10;
    }

    public void ReceiveNotify()
    {
        if (_controller.JumpCount > 0)
        {
            AddCommandToSet();
        }
    }
    public int GetAnimationPriority()
    {
        return _ani.PlayPriority;
    }
    public AnimationController GetAnimation()
    {
        return _ani;
    }
    public override void Inisialize(Status status, OperateType operateType)
    {
        _statusType = status;
        _operateType = operateType;

        CommandCoexistSet.Add(Status.LeftMove);
        CommandCoexistSet.Add(Status.RightMove);
        CommandCoexistSet.Add(Status.Accumulate);
        CommandCoexistSet.Add(Status.JumpAtk);
        CommandCoexistSet.Add(Status.JumpThrow);
        CommandCoexistSet.Add(Status.Shoot);

        CommandReplaceSet.Add(Status.Hurted);
        CommandReplaceSet.Add(Status.Die);
        CommandReplaceSet.Add(Status.Weak);
        CommandReplaceSet.Add(Status.StrongAtk);
        CommandReplaceSet.Add(Status.Dash);
        CommandReplaceSet.Add(Status.ImpulseJump);
    }
    public override void Begin()
    {
        _controller.SetAnimation(_ani);
        _controller.PlayAnimation();
        _ani.AniTurnFace(PlayerController._player.face);
        JumpState = 1;
        if (_controller.JumpCount == 1)
        {
            JumpState = 2;
            _ani.ChangeAnimator("isSecondJump", "true");

            Transform fire = GameObject.Instantiate(SecondJumpFire, _controller._transform.localPosition, Quaternion.identity).transform;
            if (PlayerController._player.face == Creature.Face.Left)
                fire.localScale = new(-1, 1, 0);

            Timer = TimerSet;
        }

        isLeaveGround = false;
        _controller.JumpCount--;
    }
    public override void End()
    {
        _controller.StopAnimation(_ani);
        JumpState = 0;
        JumpForceCount = 0;
        Timer = 0;
    }
    public override void Execute(float deltaTime)
    {
        if (Timer > 0)
        {
            Timer -= deltaTime;
        }
        if (!PlayerController.isGround)
        {
            isLeaveGround = true;
        }

        if (_jumpBreakJudge())
        {
            JumpState = 0;
        }
        if (PlayerController.isGround && isLeaveGround)
        {
            RemoveCommandFromSet();
        }

        if (JumpState == 1 && _controller.GetYVelocity() > SpeedLimit)
        {
            JumpState = 0;
        }
        if (JumpState == 2 && _controller.GetYVelocity() > (SpeedLimit / 1.2f))
        {
            JumpState = 0;
        }

        if (JumpState == 0 && Timer <= 0)
        {
            RemoveCommandFromSet();
        }

        _ani.AniTurnFace(PlayerController._player.face);
    }
    public override void FixedExecute(float deltaTime)
    {
        /*if (JumpState == 1)
        {
            if (JumpForceCount <= _controller.TestJumpForceCount)
            {
                _rigidBody.AddForce(new Vector2(0, (_controller.TestJumpForce / 2) + testNumber * _controller.TestJumpForce * JumpForceCount), ForceMode2D.Impulse);
                JumpForceCount++;
            }
            else
            {
                _rigidBody.AddForce(new Vector2(0, _controller.TestJumpForce), ForceMode2D.Impulse);
            }
        }
        if (JumpState == 2)
        {
            if (JumpForceCount <= _controller.TestJumpForceCount)
            {
                _rigidBody.AddForce(new Vector2(0, (_controller.TestJumpForce / 2) + (0.05f * _controller.TestJumpForce * JumpForceCount)), ForceMode2D.Impulse);
                JumpForceCount++;
            }
        }*/
        if (JumpState == 1)
        {
            if (JumpForceCount <= 10)
            {
                JumpForceCount++;
                _controller.ForceRigidBody(new Vector2(0, (JumpForce / 2) + (ConstantNumber * JumpForce * JumpForceCount)), ForceMode2D.Impulse);
            }
            else
            {
                _controller.ForceRigidBody(new Vector2(0, JumpForce), ForceMode2D.Impulse);
            }
        }
        if (JumpState == 2)
        {
            if (JumpForceCount <= 10)
            {
                JumpForceCount++;
                _controller.ForceRigidBody(new Vector2(0, (JumpForce / 2) + (ConstantNumber * JumpForce * JumpForceCount)), ForceMode2D.Impulse);
            }
        }
    }
    public void StopJump()
    {
        JumpState = 0;
    }
}
public class PlayerAcumulateStatus : PlayerStatus, IObserver
{
    private GameObject AccumulateLightPrefab;
    private GameObject _accumulateLight;
    private float AccumulateTimeSet;
    private float AccumulateTime;
    public bool isAccumulateBegin;
    public bool isAccumulateComplete;
    public PlayerAcumulateStatus(PlayerController controller, float TimeSet, GameObject Light)
    {
        _controller = controller;
        Inisialize(Status.Accumulate, OperateType.Update);
        AccumulateTimeSet = TimeSet;
        AccumulateTime = AccumulateTimeSet;
        AccumulateLightPrefab = Light;
    }

    public void ReceiveNotify()
    {
        AddCommandToSet();
    }
    public override void Inisialize(Status status, OperateType operateType)
    {
        _statusType = status;
        _operateType = operateType;

        CommandCoexistSet.Add(Status.Gliding);
        CommandCoexistSet.Add(Status.Wait);
        CommandCoexistSet.Add(Status.LeftMove);
        CommandCoexistSet.Add(Status.RightMove);
        CommandCoexistSet.Add(Status.Jump);

        CommandReplaceSet.Add(Status.Hurted);
        CommandReplaceSet.Add(Status.Die);
        CommandReplaceSet.Add(Status.NormalAtk);
        CommandReplaceSet.Add(Status.JumpAtk);
        CommandReplaceSet.Add(Status.JumpThrow);
        CommandReplaceSet.Add(Status.StrongAtk);
        CommandReplaceSet.Add(Status.CriticAtk);
        CommandReplaceSet.Add(Status.UseNormalItem);
        CommandReplaceSet.Add(Status.UseThrowItem);
        CommandReplaceSet.Add(Status.Restore);
        CommandReplaceSet.Add(Status.Dash);
        CommandReplaceSet.Add(Status.Block);
        CommandReplaceSet.Add(Status.WalkThrow);
        CommandReplaceSet.Add(Status.Shoot);
    }
    public override void Begin()
    {
        isAccumulateBegin = true;
    }
    public override void End()
    {
        if (_accumulateLight != null)
        {
            GameObject.Destroy(_accumulateLight);
        }
        AccumulateTime = AccumulateTimeSet;
        isAccumulateBegin = false;
        isAccumulateComplete = false;
    }
    public override void Execute(float deltaTime)
    {
        if (!GameEvent.TutorialComplete)
        {
            return;
        }

        AccumulateTime -= deltaTime;

        if (AccumulateTime <= 0 && !isAccumulateComplete)
        {
            _accumulateLight = Transform.Instantiate(AccumulateLightPrefab, _controller._transform.position, Quaternion.identity);
            isAccumulateComplete = true;
        }

        if (_accumulateLight != null) 
        {
            _accumulateLight.transform.localPosition = _controller._transform.localPosition;
        }
    }
}
public class PlayerHurtedStatus : PlayerStatus, IPlayerAniUser
{
    private PlayerHurtedAni _ani;
    private float Timer;
    private float TimerSet;
    public bool MoveAtk;
    public PlayerHurtedStatus(PlayerController controller, PlayerHurtedAni ani)
    {
        _controller = controller;

        Inisialize(Status.Hurted, OperateType.Update);
        _ani = ani;
        TimerSet = controller.HurtedTimerSet;
    }

    public int GetAnimationPriority()
    {
        return _ani.PlayPriority;
    }
    public AnimationController GetAnimation()
    {
        return _ani;
    }
    public override void Inisialize(Status status, OperateType operateType)
    {
        _statusType = status;
        _operateType = operateType;

        CommandReplaceSet.Add(Status.Die);
    }
    public override void Begin()
    {
        _controller.SetAnimation(_ani);
        _controller.PlayAnimation();
        _ani.AniTurnFace(PlayerController._player.face);

        Timer = TimerSet;

        if (MoveAtk)
        {
            _controller._invincibleManager.AddInvincible(InvincibleManager.InvincibleType.Absolute);
        }
    }
    public override void End()
    {
        _controller.StopAnimation(_ani);
        if (MoveAtk)
        {
            _controller._invincibleManager.RemoveInvincible(InvincibleManager.InvincibleType.Absolute);
            MoveAtk = false;
        }
    }
    public override void Execute(float deltaTime)
    {
        if (MoveAtk)
            return;

        Timer -= deltaTime;

        if (Timer <= 0)
        {
            RemoveCommandFromSet();
        }
    }
}

public class PlayerNormalAtkStatus :PlayerStatus, IPlayerAniUser
{
    private BattleSystem _battleSystem;
    private PlayerNormalAtkAni _ani;
    private GameObject NormalAtkPrefab1;
    private GameObject NormalAtkPrefab2;
    private float TimerSet;
    private float Timer;
    private Queue<TimedEvent> _eventQueue;
    private bool isUseAlterAtk;
    public PlayerNormalAtkStatus(PlayerController controller, BattleSystem battle, PlayerNormalAtkAni Ani)
    {
        _controller = controller;
        _battleSystem = battle;
        _ani = Ani;
        Inisialize(Status.NormalAtk, OperateType.FixedUpdate);
        TimerSet = _battleSystem.AtkTimerSet;
        Reset();
        NormalAtkPrefab1 = _battleSystem.NormalAtk;
        NormalAtkPrefab2 = _battleSystem.AlterAtk;
    }

    public int GetAnimationPriority()
    {
        return _ani.PlayPriority;
    }
    public AnimationController GetAnimation()
    {
        return _ani;
    }
    public override void Inisialize(Status status, OperateType operateType)
    {
        _statusType = status;
        _operateType = operateType;

        CommandReplaceSet.Add(Status.BeBlock);
        CommandReplaceSet.Add(Status.Hurted);
        CommandReplaceSet.Add(Status.Die);
    }
    public override void Begin()
    {
        isUseAlterAtk = _battleSystem.JudgeUseAlterNormalAtk();

        _controller.SetAnimation(_ani);
        _controller.PlayAnimation();
        _ani.AniTurnFace(PlayerController._player.face);

        if (isUseAlterAtk)
        {
            _ani.ChangeAnimator("Alter", "true");
        }
    }
    public override void End()
    {
        _controller.StopAnimation(_ani);
        if (isUseAlterAtk)
        {
            _battleSystem.StopCalculateAlterAtk();
            _ani.ChangeAnimator("Alter", "false");
        }
        else
        {
            _battleSystem.BeginCalculateAlterAtk();
        }
        _battleSystem.BeginAtkCoolDown();
        Reset();
    }
    public override void FixedExecute(float deltaTime)
    {
        Timer -= deltaTime;

        if (_eventQueue.Count() > 0 && _eventQueue.Peek().TriggerTime >= Timer)
        {
            var timedEvent = _eventQueue.Dequeue();
            timedEvent.Callback?.Invoke();
        }
    }

    private void Reset()
    {
        Timer = TimerSet;
        _eventQueue = new Queue<TimedEvent>(
            new[]
            {
                new TimedEvent { TriggerTime = TimerSet - 0.067f, Callback = Event1 },
                new TimedEvent { TriggerTime = 0, Callback = Event2 },
            }
        );
    }
    private void Event1()
    {
        Transform atk;
        if (!isUseAlterAtk)
        {
            atk = GameObject.Instantiate(NormalAtkPrefab1, _controller._transform.position, Quaternion.identity).transform;
        }
        else
        {
            atk = GameObject.Instantiate(NormalAtkPrefab2, _controller._transform.position, Quaternion.identity).transform;
        }
        switch (PlayerController._player.face)
        {
            case Creature.Face.Right:
                atk.localScale = new Vector3(1, 1, 1);
                break;
            case Creature.Face.Left:
                atk.localScale = new Vector3(-1, 1, 1);
                break;
        }
    }
    private void Event2()
    {
        RemoveCommandFromSet();
    }
}
public class PlayerStrongAtkStatus : PlayerStatus, IPlayerAniUser, IObserver
{
    private PlayerStrongAtkAni _ani;
    private BattleSystem _battleSystem;
    private GameObject AtkPrefab;
    private GameObject JumpAtkPrefab;
    private Action<bool> IgnoreGravity;
    private Action RestoreGravity;
    private float TimerSet;
    private float JumpTimerSet;
    private float Timer;
    private Queue<TimedEvent> _eventQueue;
    private bool isJumpAtk;
    private int AtkCost;
    public PlayerStrongAtkStatus(PlayerController controller, BattleSystem battle, PlayerStrongAtkAni Ani, float TotalTime, float JumpTotalTime, 
        GameObject AtkObject, GameObject jumpAtk, Action<bool> ignore, Action Restore, int atkCost)
    {
        _controller = controller;
        _battleSystem = battle;
        _ani = Ani;
        TimerSet = TotalTime;
        JumpTimerSet = JumpTotalTime;
       AtkPrefab =AtkObject;
        JumpAtkPrefab = jumpAtk;
        IgnoreGravity = ignore;
        RestoreGravity = Restore;
        AtkCost = atkCost;

        Inisialize(Status.StrongAtk, OperateType.FixedUpdate);
    }

    public int GetAnimationPriority()
    {
        return _ani.PlayPriority;
    }
    public AnimationController GetAnimation()
    {
        return _ani;
    }
    public void ReceiveNotify()
    {
        if (_battleSystem.SkillPower >= AtkCost)
        {
            AddCommandToSet();
        }
    }
    public override void Inisialize(Status status, OperateType operateType)
    {
        _statusType = status;
        _operateType = operateType;

        CommandReplaceSet.Add(Status.Hurted);
        CommandReplaceSet.Add(Status.Die);
    }
    public override void Begin()
    {
        _battleSystem.UseSkillPower(AtkCost);

        _controller.SetAnimation(_ani);
        _controller.PlayAnimation();
        _ani.AniTurnFace(PlayerController._player.face);
        Timer = TimerSet;

        if (!PlayerController.isGround)
        {
            isJumpAtk = true;
            IgnoreGravity?.Invoke(true);
            Timer = JumpTimerSet;
            _ani.ChangeAnimator("isJump", "true");
        }

        ResetQueue();
    }
    public override void End()
    {
        _controller.StopAnimation(_ani);
        isJumpAtk = false;
        _battleSystem.BeginAtkCoolDown();
        RestoreGravity?.Invoke();
    }
    public override void FixedExecute(float deltaTime)
    {
        Timer -= deltaTime;

        if (_eventQueue.Count() > 0 && _eventQueue.Peek().TriggerTime >= Timer)
        {
            var timedEvent = _eventQueue.Dequeue();
            timedEvent.Callback?.Invoke();
        }
    }

    private void ResetQueue()
    {
        if (!isJumpAtk)
        {
            _eventQueue = new Queue<TimedEvent>(
            new[]
            {
                new TimedEvent { TriggerTime = TimerSet - 0.2f, Callback = Event1 },
                new TimedEvent { TriggerTime = 0, Callback = Event2 },
            }
            );
        }
        else
        {
            _eventQueue = new Queue<TimedEvent>(
            new[]
            {
                new TimedEvent { TriggerTime = TimerSet - 0.1f, Callback = Event1 },
                new TimedEvent { TriggerTime = TimerSet - 0.3f, Callback = Event1_5 },
                new TimedEvent { TriggerTime = 0, Callback = Event2 },
            }
            );
        }
    }
    private void Event1()
    {
        Transform atk;
        if (!isJumpAtk)
        {
            atk = GameObject.Instantiate(AtkPrefab, _controller._transform.position, Quaternion.identity).transform;
        }
        else
        {
            atk = GameObject.Instantiate(JumpAtkPrefab, _controller._transform.position, Quaternion.identity).transform;
        }
        switch (PlayerController._player.face)
        {
            case Creature.Face.Right:
                atk.localScale = new Vector3(1, 1, 1);
                break;
            case Creature.Face.Left:
                atk.localScale = new Vector3(-1, 1, 1);
                break;
        }
    }
    private void Event2()
    {
        RemoveCommandFromSet();
    }
    private void Event1_5()
    {
        RestoreGravity?.Invoke();
    }
}
public class PlayerJumpAtkStatus : PlayerStatus, IPlayerAniUser
{
    private BattleSystem _battleSystem;
    private PlayerNormalAtkAni _ani;
    private GameObject AtkPrefab;
    private float TimerSet;
    private float Timer;
    private Queue<TimedEvent> _eventQueue;

    public PlayerJumpAtkStatus(PlayerController controller, BattleSystem battleSystem, PlayerNormalAtkAni Ani)
    {
        _controller = controller;
        _battleSystem = battleSystem;
        _ani = Ani;
        Inisialize(Status.JumpAtk, OperateType.FixedUpdate);
        TimerSet = _battleSystem.AtkTimerSet;
        Reset();
        AtkPrefab = _battleSystem.JumpAtk;
    }

    public int GetAnimationPriority()
    {
        return _ani.PlayPriority;
    }
    public AnimationController GetAnimation()
    {
        return _ani;
    }
    public override void Inisialize(Status status, OperateType operateType)
    {
        _statusType = status;
        _operateType = operateType;

        CommandCoexistSet.Add(Status.RightMove);
        CommandCoexistSet.Add(Status.LeftMove);
        CommandCoexistSet.Add(Status.Gliding);
        CommandCoexistSet.Add(Status.Jump);

        CommandReplaceSet.Add(Status.BeBlock);
        CommandReplaceSet.Add(Status.Weak);
        CommandReplaceSet.Add(Status.Hurted);
        CommandReplaceSet.Add(Status.Die);
    }
    public override void Begin()
    {
        _controller.SetAnimation(_ani);
        _controller.PlayAnimation();
        _ani.ChangeAnimator("isJump", "true");
        _ani.AniTurnFace(PlayerController._player.face);
    }
    public override void End()
    {
        _controller.StopAnimation(_ani);
        _battleSystem.BeginAtkCoolDown();
        Reset();
    }
    public override void FixedExecute(float deltaTime)
    {
        Timer -= deltaTime;

        if (_eventQueue.Count() > 0 && _eventQueue.Peek().TriggerTime >= Timer)
        {
            var timedEvent = _eventQueue.Dequeue();
            timedEvent.Callback?.Invoke();
        }
    }

    private void Reset()
    {
        Timer = TimerSet;
        _eventQueue = new Queue<TimedEvent>(
            new[]
            {
                new TimedEvent { TriggerTime = TimerSet - 0.067f, Callback = Event1 },
                new TimedEvent { TriggerTime = 0, Callback = Event2 },
            }
        );
    }
    private void Event1()
    {
        Transform atk;
        atk = GameObject.Instantiate(AtkPrefab, _controller._transform.position, Quaternion.identity).transform;
        switch (PlayerController._player.face)
        {
            case Creature.Face.Right:
                atk.localScale = new Vector3(1, 1, 1);
                break;
            case Creature.Face.Left:
                atk.localScale = new Vector3(-1, 1, 1);
                break;
        }
    }
    private void Event2()
    {
        RemoveCommandFromSet();
    }
}
public class PlayerShootStatus : PlayerStatus, IObserver, IPlayerAniUser
{
    private BattleSystem _battleSystem;
    private PlayerShootAni _ani;
    private PlayerLeftMoveStatus _leftMoveStatus;
    private PlayerRightMoveStatus _rightMoveStatus;
    private GameObject Bullet;
    private Vector3 AppearPlace;
    private Vector3 JumpAppearPlace;
    private float Speed;
    private float Timer;
    private float TimerSet;

    public PlayerShootStatus(PlayerController controller, BattleSystem battleSystem, PlayerLeftMoveStatus left, PlayerRightMoveStatus right, PlayerShootAni ani)
    {
        _controller = controller;
        _battleSystem = battleSystem;
        _ani = ani;
        _leftMoveStatus = left;
        _rightMoveStatus = right;
        Speed = _controller.ShootingSpeed;
        Bullet = _battleSystem.Bullet;
        AppearPlace = _battleSystem.BulletAppear;
        JumpAppearPlace = _battleSystem.BulletJumpAppear;
        TimerSet = _battleSystem.ShootingEndTimerSet;

        Inisialize(Status.Shoot, OperateType.Update);
    }

    public int GetAnimationPriority()
    {
        return _ani.PlayPriority;
    }
    public AnimationController GetAnimation()
    {
        return _ani;
    }
    public void ReceiveNotify()
    {
        if (_battleSystem.CanShoot && _battleSystem.SkillPower >= _battleSystem.ShootCost && AddCommandToSet())
        {
            Shoot();
        }
    }
    public override void Inisialize(Status status, OperateType operateType)
    {
        _statusType = status;
        _operateType = operateType;

        CommandCoexistSet.Add(Status.RightMove);
        CommandCoexistSet.Add(Status.LeftMove);
        CommandCoexistSet.Add(Status.Gliding);

        CommandReplaceSet.Add(Status.FallWait);
        CommandReplaceSet.Add(Status.Hurted);
        CommandReplaceSet.Add(Status.Die);
        CommandReplaceSet.Add(Status.Dash);
        CommandReplaceSet.Add(Status.ImpulseJump);
    }
    public override void Begin()
    {
        _controller.SetAnimation(_ani);
        _controller.PlayAnimation();
        _ani.AniTurnFace(PlayerController._player.face);

        _leftMoveStatus.BeginShoot(Speed);
        _rightMoveStatus.BeginShoot(Speed);
    }
    public override void End()
    {
        _leftMoveStatus.StopShoot();
        _rightMoveStatus.StopShoot();
        _controller.StopAnimation(_ani);
    }
    public override void Execute(float deltaTime)
    {
        Timer -= deltaTime;

        float AniSpeed = 1;

        if (PlayerController.isGround && _rightMoveStatus.CheckUsing() || _leftMoveStatus.CheckUsing())
        {
            _ani.ChangeAnimator("isWalking", "true");
            switch (PlayerController._player.face)
            {
                case Creature.Face.Left:
                    if (_rightMoveStatus.CheckUsing())
                    {
                        AniSpeed = -1;
                    }
                    break;
                case Creature.Face.Right:
                    if (_leftMoveStatus.CheckUsing())
                    {
                        AniSpeed = -1;
                    }
                    break;
            }
        }
        else
        {
            _ani.ChangeAnimator("isWalking", "false");
        }

        _ani.ChangeSpeed("Speed", AniSpeed);

        if (!PlayerController.isGround)
        {
            _ani.ChangeAnimator("isJump", "true");
        }
        else
        {
            _ani.ChangeAnimator("isJump", "false");
        }

        if (Timer <= 0)
        {
            RemoveCommandFromSet();
        }
    }

    private void Shoot()
    {
        Timer = TimerSet;
        _battleSystem.BeginShootCooldown();
        _battleSystem.UseSkillPower(_battleSystem.ShootCost);

        Vector3 location = _controller._transform.localPosition;
        Vector3 appearPlace = AppearPlace;
        if (!PlayerController.isGround)
        {
            appearPlace = JumpAppearPlace;
        }

        switch (PlayerController._player.face)
        {
            case Creature.Face.Left:
                location = new Vector3(location.x - appearPlace.x, location.y + appearPlace.y, location.z);
                GameObject.Instantiate(Bullet, location, Quaternion.identity);
                break;
            case Creature.Face.Right:
                location = new Vector3(location.x + appearPlace.x, location.y + appearPlace.y, location.z);
                GameObject.Instantiate(Bullet, location, Quaternion.identity);
                break;
        }
    }
}

public class PlayerCriticAtkStatus : PlayerStatus, IPlayerAniUser
{
    private BattleSystem _battleSystem;
    private PlayerCriticAtkAni _ani;
    private AniMethod _aniMethod;
    private GameObject CriticAtkPredictPrefab;
    private Transform CriticAtkPredictObject;
    private GameObject CriticAtk;
    private float TimerSet;
    private float Timer;
    private Vector3 CriticAtkPredictPosition;
    private Queue<TimedEvent> _eventQueue;

    public PlayerCriticAtkStatus(PlayerController controller, BattleSystem battle, AniMethod aniMethod, PlayerCriticAtkAni Ani)
    {
        _controller = controller;
        _battleSystem = battle;
        _aniMethod = aniMethod;
        _ani = Ani;

        Inisialize(Status.CriticAtk, OperateType.FixedUpdate);

        TimerSet = _battleSystem.CriticAtkTimerSet;
        CriticAtkPredictPrefab = _battleSystem.CriticAtkPredictObject;
        CriticAtk = _battleSystem.CriticAtk;

        if (_controller != null && _battleSystem != null && _ani != null && _aniMethod != null)
        {
            isInitializeSuccess = true;
        }
        else
        {
            Debug.LogWarning("InitialPlayerStatusFail");
        }
    }

    public int GetAnimationPriority()
    {
        return _ani.PlayPriority;
    }
    public AnimationController GetAnimation()
    {
        return _ani;
    }
    public override void Inisialize(Status status, OperateType operateType)
    {
        _statusType = status;
        _operateType = operateType;

        CommandReplaceSet.Add(Status.Hurted);
        CommandReplaceSet.Add(Status.Die);
    }
    public override void Begin()
    {
        if (!isInitializeSuccess)
        {
            return;
        }

        _battleSystem.UseSkillPower(_battleSystem.CriticAtkCost);

        _controller.SetAnimation(_ani);
        _controller.PlayAnimation();
        _ani.AniTurnFace(PlayerController._player.face);

        CriticAtkPredictObject = GameObject.Instantiate(CriticAtkPredictPrefab, _controller._transform.localPosition, Quaternion.identity).transform;
        Timer = TimerSet;
        ResetQueue();
    }
    public override void End()
    {
        if (!isInitializeSuccess)
        {
            return;
        }

        _controller.StopAnimation(_ani);
    }
    public override void FixedExecute(float deltaTime)
    {
        if (!isInitializeSuccess)
        {
            return;
        }

        Timer -= deltaTime;

        if (_eventQueue.Count() > 0 && _eventQueue.Peek().TriggerTime >= Timer)
        {
            var timedEvent = _eventQueue.Dequeue();
            timedEvent.Callback?.Invoke();
        }
    }
    //大招位置計算
    private Vector3 CalculateCriticAtkPosition()
    {
        float PositionX = 0;
        float PositionY = 0;

        PositionX = (_controller._transform.position.x + CriticAtkPredictObject.position.x) / 2;
        PositionY = (_controller._transform.position.y + CriticAtkPredictObject.position.y) / 2;

        return new Vector3(PositionX, PositionY, 0);
    }
    private void ResetQueue()
    {
        _eventQueue = new Queue<TimedEvent>(
            new[]
            {
                new TimedEvent { TriggerTime = TimerSet - 0.97f, Callback = Event1 },
                new TimedEvent { TriggerTime = TimerSet - 1f, Callback = Event2 },
                new TimedEvent { TriggerTime = TimerSet - 2.9f, Callback = Event3 },
                new TimedEvent { TriggerTime = 0, Callback = Event4 }
            }
        );
    }
    private void Event1()
    {
        CriticAtkPredictPosition = CriticAtkPredictObject.position;
        float CriticAtkPredictAngle = AngleCaculate.CaculateAngle("R", CriticAtkPredictObject, _controller._transform);
        SpriteRenderer atkSprite;
        switch (PlayerController._player.face)
        {
            case  Creature.Face.Left:
                atkSprite = GameObject.Instantiate(CriticAtk, CalculateCriticAtkPosition(), Quaternion.Euler(0, 0, CriticAtkPredictAngle))?.GetComponent<SpriteRenderer>();
                atkSprite.flipX = true;
                break;
            case Creature.Face.Right:
                GameObject.Instantiate(CriticAtk, CalculateCriticAtkPosition(), Quaternion.Euler(0, 0, CriticAtkPredictAngle));
                break;
        }
    }
    private void Event2()
    {
        _aniMethod.OpenFlash();
        if (CriticAtkPredictObject != null)
        {
            //預設距離15
            _controller._transform.position = CriticAtkPredictPosition;
            GameObject.Destroy(CriticAtkPredictObject.gameObject);
        }
        _ani.ChangeAnimator("isChange", "true");
    }
    private void Event3()
    {
        _aniMethod.OpenFlash();
    }
    private void Event4()
    {
        RemoveCommandFromSet();
    }
}
public class PlayerUseNormalItemStatus : PlayerStatus, IPlayerAniUser
{
    private INormalItem _item;
    private int _itemID;
    private PlayerUseItemAni _ani;

    public PlayerUseNormalItemStatus(PlayerController controller, PlayerUseItemAni ani)
    {
        _controller = controller;
        _ani = ani;

        Inisialize(Status.UseNormalItem, OperateType.FixedUpdate);
    }

    public int GetAnimationPriority()
    {
        return _ani.PlayPriority;
    }
    public AnimationController GetAnimation()
    {
        return _ani;
    }
    public override void Inisialize(Status status, OperateType operateType)
    {
        _statusType = status;
        _operateType = operateType;

        CommandReplaceSet.Add(Status.Hurted);
        CommandReplaceSet.Add(Status.Dash);
        CommandReplaceSet.Add(Status.Die);
    }
    public override void Begin()
    {
        _item.Begin();

        _controller.SetAnimation(_ani);
        _controller.PlayAnimation();
        _ani.ChangeAnimator("Type", _itemID.ToString());
        _ani.AniTurnFace(PlayerController._player.face);
    }
    public override void End()
    {
        _controller.StopAnimation(_ani);
    }
    public override void FixedExecute(float deltaTime)
    {
        _item.Using(deltaTime);
    }

    public void SetItem(INormalItem item, int id)
    {
        _item = item;
        _itemID = id;
    }
}
public class PlayerUseThrowItemStatus: PlayerStatus, IAimSystemUser, IPlayerAniUser
{
    private IThrowItem _throwItem;
    private Item _item;
    private PlayerThrowItemAni _ani;
    private PlayerAimLineAnimation AimLineAni;
    private InputManager _InputManager;
    private (float, float) AimPower;
    private (float, float) InisialAimPower;
    private float AimSpeed;
    private float AimLimit;
    public bool isAimBegin;
    public bool isBeginSuccess;
    private float Timer;
    private float TimerSet;
    private Vector3 AppearPlace;
    private Queue<TimedEvent> _eventQueue;

    public PlayerUseThrowItemStatus(PlayerController controller, PlayerThrowItemAni ani, PlayerAimLineAnimation aimLineAni,
        InputManager inputManager, BattleSystem battle)
    {
        _controller = controller;
        _ani = ani;
        AimLineAni = aimLineAni;
        Inisialize(Status.UseThrowItem, OperateType.Update);

        _InputManager = inputManager;
        InisialAimPower = battle.InisialAimPower;
        AimSpeed = battle.DebugAimSpeed;
        AimLimit = battle.AimLimit;
        AppearPlace = battle.ThrowItemAppear;
    }

    public int GetAnimationPriority()
    {
        return _ani.PlayPriority;
    }
    public AnimationController GetAnimation()
    {
        return _ani;
    }
    public void ReceiveAimDirection((float, float) direction, float deltaTime)
    {
        AimPower.Item1 += direction.Item1 * AimSpeed * deltaTime;
        AimPower.Item2 += direction.Item2 * AimSpeed * deltaTime;
    }
    public override void Inisialize(Status status, OperateType operateType)
    {
        _statusType = status;
        _operateType = operateType;

        CommandReplaceSet.Add(Status.Hurted);
        CommandReplaceSet.Add(Status.Dash);
        CommandReplaceSet.Add(Status.Die);
    }
    public override void Begin()
    {
        _InputManager.SubscribeAimInput(this);

        isAimBegin = true;
        AimPower = InisialAimPower;
        if (PlayerController._player.face == Creature.Face.Left)
        {
            AimPower.Item1 = -InisialAimPower.Item1;
        }
        Timer = TimerSet;
        ResetQueue();

        _controller.SetAnimation(_ani);
        _controller.PlayAnimation();
        _ani.AniTurnFace(PlayerController._player.face);
        AimLineAni.AniOpen(PlayerController._player.face);
    }
    public override void End()
    {
        _controller.StopAnimation(_ani);
        isAimBegin = false;
        isBeginSuccess = false;
        _InputManager.UnSubscribeAimInput(this);
    }
    public override void Execute(float deltaTime)
    {
        if (isBeginSuccess)
        {
            Timer -= deltaTime;
        }
        LimitJudgement();
        _ani.PlayThrowItemAni(_item.Name);
        AimLineAni.OperateAni(PlayerController._player.face, AimPower, _controller._transform.localPosition);
        if (_eventQueue.Count() > 0 && _eventQueue.Peek().TriggerTime >= Timer)
        {
            var timedEvent = _eventQueue.Dequeue();
            timedEvent.Callback?.Invoke();
        }
    }

    public void SetItem(Item item, IThrowItem throwItem)
    {
        _item = item;
        TimerSet = _item.GetUseTimer();
        _throwItem = throwItem;
    }
    public void BeginThrow()
    {
        isBeginSuccess = true;
        AimLineAni.AniClose(PlayerController._player.face);
        _ani.ChangeAnimator("Throw", "true");
    }
    private (float, float) PowerCalculate((float, float) OriginalPower, Creature.Face face)
    {
        (float, float) result = OriginalPower;

        if (face == Creature.Face.Left)
        {
            result.Item1 = -OriginalPower.Item1; 
        }

        float LeftUpPowerX = 45;
        float LeftUpPowerY = 330;
        float RightUpPowerX = 210;
        float RightUpPowerY = 300;
        float LeftDownPowerX = 80;
        float LeftDownPowerY = 100;
        float RightDownPowerX = 240;
        float RightDownPowerY = 100;
        float NowUpProportionXPower;//上半段平均Xscale
        float NowDownProportionXPower;//下半段平均Xscale
        float NowUpProportionYPower;//上半段平均Yscale
        float NowDownProportionYPower;//下半段平均Yscale
        
        float NowProportionX = result.Item1 / 10;//當前X軸比例 由左而右
        float NowProportionY = result.Item2 / 10;//當前Y軸比例 由下而上
        
        NowUpProportionXPower = LeftUpPowerX * (1 - NowProportionX) + RightUpPowerX * NowProportionX;
        NowDownProportionXPower = LeftDownPowerX * (1 - NowProportionX) + RightDownPowerX * NowProportionX;
        result.Item1 = NowDownProportionXPower * (1 - NowProportionY) + NowUpProportionXPower * NowProportionY;
        NowUpProportionYPower = LeftUpPowerY * (1 - NowProportionX) + RightUpPowerY * NowProportionX;
        NowDownProportionYPower = LeftDownPowerY * (1 - NowProportionX) + RightDownPowerY * NowProportionX;
        result.Item2 = NowDownProportionYPower * (1 - NowProportionY) + NowUpProportionYPower * NowProportionY;

        if (face == Creature.Face.Left)
        {
            result.Item1 = -result.Item1;
        }
        return result;
    }
    private void LimitJudgement()
    {
        switch (PlayerController._player.face)
        {
            case Creature.Face.Right:
                if (AimPower.Item1 >= AimLimit)
                {
                    AimPower.Item1 = AimLimit;
                }

                if (AimPower.Item1 <= 0)
                {
                    AimPower.Item1 = 0;
                }
                break;
            case Creature.Face.Left:
                if (AimPower.Item1 >= 0)
                {
                    AimPower.Item1 = 0;
                }

                if (AimPower.Item1 <= -AimLimit)
                {
                    AimPower.Item1 = -AimLimit;
                }
                break;
        }
        if (AimPower.Item2 >= AimLimit)
        {
            AimPower.Item2 = AimLimit;
        }
        if (AimPower.Item2 <= 0)
        {
            AimPower.Item2 = 0;
        }
    }
    private void ResetQueue()
    {
        _eventQueue = new Queue<TimedEvent>(
            new[]
            {
                new TimedEvent { TriggerTime = TimerSet - 0.25f, Callback = Event1 },
                new TimedEvent { TriggerTime = 0, Callback = Event2 },
            }
        );
    }
    private void Event1()
    {
        Vector3 location = _controller._transform.localPosition;
        switch (PlayerController._player.face)
        {
            case Creature.Face.Left:
                location = new Vector3(location.x - AppearPlace.x, location.y + AppearPlace.y, location.z);
                break;
            case Creature.Face.Right:
                location = new Vector3(location.x + AppearPlace.x, location.y + AppearPlace.y, location.z);
                break;
        }

        _throwItem.UseSuccess();
        _throwItem.Throw(PowerCalculate(AimPower, PlayerController._player.face), location);
    }
    private void Event2()
    {
        RemoveCommandFromSet();
    }
}
public class PlayerJumpThrowStatus : PlayerStatus, IPlayerAniUser
{
    private PlayerJumpThrowAni _ani;
    private Item _item;
    private IThrowItem _throwItem;
    private float Timer;
    private float TimerSet;
    private Vector3 AppearPlace;
    private Queue<TimedEvent> _eventQueue;
    private (float, float) Power;

    public PlayerJumpThrowStatus(PlayerController controller, PlayerJumpThrowAni ani, BattleSystem battleSystem)
    {
        _controller = controller;
        _ani = ani;
        AppearPlace = battleSystem.JumpThrowItemAppear;
        Power = battleSystem.JumpThrowCocktailPower;

        Inisialize(Status.JumpThrow, OperateType.Both);
    }
    public int GetAnimationPriority()
    {
        return _ani.PlayPriority;
    }
    public AnimationController GetAnimation()
    {
        return _ani;
    }
    public override void Inisialize(Status status, OperateType operateType)
    {
        _statusType = status;
        _operateType = operateType;

        CommandCoexistSet.Add(Status.RightMove);
        CommandCoexistSet.Add(Status.LeftMove);
        CommandCoexistSet.Add(Status.Gliding);
        CommandCoexistSet.Add(Status.Jump);

        CommandReplaceSet.Add(Status.Hurted);
        CommandReplaceSet.Add(Status.Die);
    }
    public override void Begin()
    {
        _controller.SetAnimation(_ani);
        _controller.PlayAnimation();
        _ani.PlayThrowItemAni(_item.Name);
        _ani.AniTurnFace(PlayerController._player.face);
        Timer = TimerSet;
        ResetQueue();
    }
    public override void End()
    {
        _controller.StopAnimation(_ani);
    }
    public override void Execute(float deltaTime)
    {
        _ani.AniTurnFace(PlayerController._player.face);
    }
    public override void FixedExecute(float deltaTime)
    {
        Timer -= deltaTime;

        if (_eventQueue.Count() > 0 && _eventQueue.Peek().TriggerTime >= Timer)
        {
            var timedEvent = _eventQueue.Dequeue();
            timedEvent.Callback?.Invoke();
        }
    }
    public void SetItem(Item item, IThrowItem throwItem)
    {
        _item = item;
        TimerSet = _item.GetUseTimer();
        _throwItem = throwItem;
    }
    private void ResetQueue()
    {
        _eventQueue = new Queue<TimedEvent>(
            new[]
            {
                new TimedEvent { TriggerTime = TimerSet - 0.2f, Callback = Event1 },
                new TimedEvent { TriggerTime = 0, Callback = Event2 },
            }
        );
    }
    private void Event1()
    {
        Vector3 location = _controller._transform.localPosition;
        (float, float) power = Power;

        switch (PlayerController._player.face)
        {
            case Creature.Face.Left:
                location = new Vector3(location.x - AppearPlace.x, location.y + AppearPlace.y, location.z);
                power.Item1 = -Power.Item1;
                break;
            case Creature.Face.Right:
                location = new Vector3(location.x + AppearPlace.x, location.y + AppearPlace.y, location.z);
                break;
        }
        location = new Vector3(location.x + AppearPlace.x, location.y + AppearPlace.y, location.z);
        _throwItem.UseSuccess();
        _throwItem.WalkThrow(power, location);
    }
    private void Event2()
    {
        RemoveCommandFromSet();
    }
}
public class PlayerRestoreStatus : PlayerStatus, IPlayerAniUser, IObserver
{
    private ItemManage _itemManage;
    private PlayerRestoreAni _ani;
    private float TimerSet;
    private float Timer;
    private bool BeginRestore;
    private float RestoreHP;
    private Queue<TimedEvent> _eventQueue;

    public PlayerRestoreStatus(PlayerController controller, PlayerRestoreAni Ani, ItemManage itemManage)
    {
        _controller = controller;
        _itemManage = itemManage;
        _ani = Ani;

        Inisialize(Status.Block, OperateType.FixedUpdate);

        RestoreHP = _controller.RestoreHP;
        TimerSet = _controller.RestoreTimerSet;

        if (_controller != null && _ani != null && _itemManage != null)
        {
            isInitializeSuccess = true;
        }
        else
        {
            Debug.LogWarning("InitialPlayerStatusFail");
        }
    }

    public int GetAnimationPriority()
    {
        return _ani.PlayPriority;
    }
    public AnimationController GetAnimation()
    {
        return _ani;
    }
    public void ReceiveNotify()
    {
        if (!isInitializeSuccess)
        {
            return;
        }

        if (_itemManage.RestoreItemNumber > 0 && PlayerController.isGround)
        {
            AddCommandToSet();
        }
    }
    public override void Inisialize(Status status, OperateType operateType)
    {
        _statusType = status;
        _operateType = operateType;

        CommandReplaceSet.Add(Status.Dash);
        CommandReplaceSet.Add(Status.Hurted);
        CommandReplaceSet.Add(Status.Die);
    }
    public override void Begin()
    {
        if (!isInitializeSuccess)
        {
            return;
        }

        _itemManage.RestoreItemNumber -= 1;

        _controller.SetAnimation(_ani);
        _controller.PlayAnimation();
        _ani.AniTurnFace(PlayerController._player.face);

        Timer = TimerSet;
        ResetQueue();
    }
    public override void End()
    {
        if (!isInitializeSuccess)
        {
            return;
        }

        BeginRestore = false;
        _controller.StopAnimation(_ani);
    }
    public override void FixedExecute(float deltaTime)
    {
        if (!isInitializeSuccess)
        {
            return;
        }

        Timer -= deltaTime;

        if (BeginRestore)
        {
            _controller.Hp += RestoreHP;
        }

        if (_eventQueue.Count() > 0 && _eventQueue.Peek().TriggerTime >= Timer)
        {
            var timedEvent = _eventQueue.Dequeue();
            timedEvent.Callback?.Invoke();
        }
    }

    private void ResetQueue()
    {
        _eventQueue = new Queue<TimedEvent>(
            new[]
            {
                new TimedEvent { TriggerTime = TimerSet - 0.5f, Callback = Event1 },
                new TimedEvent { TriggerTime = 0, Callback = Event2 }
            }
        );
    }
    private void Event1()
    {
        BeginRestore = true;
    }
    private void Event2()
    {
        RemoveCommandFromSet();
    }
}

public class PlayerDieStatus : PlayerStatus, IPlayerAniUser
{
    private PlayerData _playerData;
    private PlayerDieController DieUI;
    private InvincibleManager _invincibleManager;
    private PlayerDieAni _ani;
    private float Timer;
    private float TimerSet;
    private Queue<TimedEvent> _eventQueue;
    public PlayerDieStatus(PlayerController controller, PlayerDieAni ani, InvincibleManager invincible, PlayerData playerData, PlayerDieController dieUI)
    {
        _controller = controller;
        _invincibleManager = invincible;
        _playerData = playerData;
        _ani = ani;
        DieUI = dieUI;

        Inisialize(Status.Die, OperateType.Update);

        TimerSet = controller.DieTimerSet;

        if(_controller != null && _playerData!= null && _ani != null && DieUI != null && _invincibleManager != null)
        {
            isInitializeSuccess = true;
        }
        else
        {
            Debug.LogWarning("InitialPlayerStatusFail");
        }
    }

    public int GetAnimationPriority()
    {
        return _ani.PlayPriority;
    }
    public AnimationController GetAnimation()
    {
        return _ani;
    }
    public override void Inisialize(Status status, OperateType operateType)
    {
        _statusType = status;
        _operateType = operateType;
    }
    public override void Begin()
    {
        if (!isInitializeSuccess)
        {
            return;
        }

        PlayerController.isDie = true;

        _controller.SetAnimation(_ani);
        _controller.PlayAnimation();
        _ani.AniTurnFace(PlayerController._player.face);

        _invincibleManager.AddInvincible(InvincibleManager.InvincibleType.Absolute);
        _playerData.CommonSave();
        ResetQueue();
        Timer = TimerSet;
    }
    public override void End()
    {
        if (!isInitializeSuccess)
        {
            return;
        }

        _controller.StopAnimation(_ani);
        _invincibleManager.RemoveInvincible(InvincibleManager.InvincibleType.Absolute);
        DieUI.ResetDie();
    }
    public override void Execute(float deltaTime)
    {
        if (!isInitializeSuccess)
        {
            return;
        }

        Timer -= deltaTime;

        if (_eventQueue.Count() > 0 && _eventQueue.Peek().TriggerTime >= Timer)
        {
            var timedEvent = _eventQueue.Dequeue();
            timedEvent.Callback?.Invoke();
        }

        if (_controller.Hp > 0)
        {
            RemoveCommandFromSet();
        }
    }
    private void ResetQueue()
    {
        _eventQueue = new Queue<TimedEvent>(
            new[]
            {
                new TimedEvent { TriggerTime = 0, Callback = Event1 },
            }
        );
    }
    private void Event1()
    {
        DieUI.gameObject.SetActive(true);
    }
}
public class PlayerBlockStatus : PlayerStatus, IPlayerAniUser, IObserver
{
    private BattleSystem _battleSystem;
    private PlayerBlockAni _ani;
    private GameObject BlockPrefab;
    private float TimerSet;
    private float Timer;
    private Queue<TimedEvent> _eventQueue;

    public PlayerBlockStatus(PlayerController controller, BattleSystem battle, PlayerBlockAni Ani)
    {
        _controller = controller;
        _battleSystem = battle;
        _ani = Ani;

        Inisialize(Status.Block, OperateType.FixedUpdate);

        TimerSet = _battleSystem.BlockTimerSet;
        BlockPrefab = _battleSystem.Block;

        if (_controller != null && _battleSystem != null && _ani!= null)
        {
            isInitializeSuccess = true;
        }
        else
        {
            Debug.LogWarning("InitialPlayerStatusFail");
        }
    }

    public int GetAnimationPriority()
    {
        return _ani.PlayPriority;
    }
    public AnimationController GetAnimation()
    {
        return _ani;
    }
    public void ReceiveNotify()
    {
        if (!isInitializeSuccess)
        {
            return;
        }

        if (GameEvent.TutorialComplete && PlayerController.isGround && _battleSystem.CanAtk)
        {
            AddCommandToSet();
        }
    }
    public override void Inisialize(Status status, OperateType operateType)
    {
        _statusType = status;
        _operateType = operateType;

        CommandReplaceSet.Add(Status.Hurted);
        CommandReplaceSet.Add(Status.BlockAtk);
        CommandReplaceSet.Add(Status.Die);
    }
    public override void Begin()
    {
        if (!isInitializeSuccess)
        {
            return;
        }

        _controller.SetAnimation(_ani);
        _controller.PlayAnimation();
        _ani.AniTurnFace(PlayerController._player.face);
        _battleSystem.isBlock = true;

        Reset();
    }
    public override void End()
    {
        if (!isInitializeSuccess)
        {
            return;
        }

        _controller.StopAnimation(_ani);
        _battleSystem.BeginAtkCoolDown();
        _battleSystem.isBlock = false;
    }
    public override void FixedExecute(float deltaTime)
    {
        if (!isInitializeSuccess)
        {
            return;
        }

        Timer -= deltaTime;

        if (_eventQueue.Count() > 0 && _eventQueue.Peek().TriggerTime >= Timer)
        {
            var timedEvent = _eventQueue.Dequeue();
            timedEvent.Callback?.Invoke();
        }
    }

    private void BlockSuccess()
    {
        Timer = TimerSet - 0.49f;
    }
    private void Reset()
    {
        Timer = TimerSet;
        _eventQueue = new Queue<TimedEvent>(
            new[]
            {
                new TimedEvent { TriggerTime = TimerSet - 0.05f, Callback = Event1 },
                new TimedEvent { TriggerTime = TimerSet - 0.5f, Callback = Event2 },
                new TimedEvent { TriggerTime = 0, Callback = Event3 },
            }
        );
    }
    private void Event1()
    {
        PlayerBlockJudgement block;

        block = GameObject.Instantiate(BlockPrefab, _controller._transform.localPosition, Quaternion.identity)?.GetComponent<PlayerBlockJudgement>();
        block.BlockSuccess += _battleSystem.BeginBlockSuccessBulletTime;
        block.BlockSuccess += BlockSuccess;
        switch (PlayerController._player.face)
        {
            case Creature.Face.Right:
                block.isRBlock = true;
                block.isLBlock = false;
                break;
            case Creature.Face.Left:
                block.isRBlock = false;
                block.isLBlock = true;
                block.transform.localScale = new Vector3(-1, 1, 1);
                break;
        }
    }
    private void Event2()
    {
        _ani.ChangeAnimator("End", "true");
    }
    private void Event3()
    {
        RemoveCommandFromSet();
    }
}
public class PlayerBlockNormalAtkStatus : PlayerStatus, IPlayerAniUser, IObserver
{
    private InvincibleManager _invincibleManager;
    private BattleSystem _battleSystem;
    private PlayerBlockAtkAni _ani;
    private Buff InvincibleBuff;
    private GameObject AtkPrefab1;
    private GameObject AtkPrefab2;
    private float TimerSet;
    private float Timer;
    private Queue<TimedEvent> _eventQueue;

    public PlayerBlockNormalAtkStatus(PlayerController controller, BattleSystem battle, InvincibleManager invincible, PlayerBlockAtkAni Ani, Buff invincibleBuff)
    {
        _controller = controller;
        _battleSystem = battle;
        _invincibleManager = invincible;
        _ani = Ani;
        InvincibleBuff = invincibleBuff;

        Inisialize(Status.BlockAtk, OperateType.FixedUpdate);

        TimerSet = _battleSystem.BlockNormalAtkTimerSet;
        AtkPrefab1 = _battleSystem.BlockNormalAtkAni;
        AtkPrefab2 = _battleSystem.BlockNormalAtk;

        if (_controller != null && _battleSystem != null && _invincibleManager != null && _ani != null && InvincibleBuff != null)
        {
            isInitializeSuccess = true;
        }
        else
        {
            Debug.LogWarning("InitialPlayerStatusFail");
        }
    }

    public int GetAnimationPriority()
    {
        return _ani.PlayPriority;
    }
    public AnimationController GetAnimation()
    {
        return _ani;
    }
    public void ReceiveNotify()
    {
        if (!isInitializeSuccess)
        {
            return;
        }

        if (_battleSystem.isBlockSuccess)
        {
            _battleSystem.StopBlockSuccessBulletTime();
            AddCommandToSet();
        }
    }
    public override void Inisialize(Status status, OperateType operateType)
    {
        _statusType = status;
        _operateType = operateType;
    }
    public override void Begin()
    {
        if (!isInitializeSuccess)
        {
            return;
        }

        _controller.SetAnimation(_ani);
        _controller.PlayAnimation();
        _ani.ChangeAnimator("Type", "1");
        _ani.AniTurnFace(PlayerController._player.face);

        _invincibleManager.AddInvincible(InvincibleManager.InvincibleType.Absolute);

        Reset();
    }
    public override void End()
    {
        if (!isInitializeSuccess)
        {
            return;
        }

        _controller.StopAnimation(_ani);
        _battleSystem.BeginAtkCoolDown();
        _invincibleManager.RemoveInvincible(InvincibleManager.InvincibleType.Absolute);
        InvincibleBuff.AddBuffToSet();
    }
    public override void FixedExecute(float deltaTime)
    {
        if (!isInitializeSuccess)
        {
            return;
        }

        Timer -= deltaTime;

        if (_eventQueue.Count() > 0 && _eventQueue.Peek().TriggerTime >= Timer)
        {
            var timedEvent = _eventQueue.Dequeue();
            timedEvent.Callback?.Invoke();
        }
    }

    private void Reset()
    {
        Timer = TimerSet;
        _eventQueue = new Queue<TimedEvent>(
            new[]
            {
                new TimedEvent { TriggerTime = TimerSet, Callback = Event1 },
                new TimedEvent { TriggerTime = TimerSet - 1.15f, Callback = Event2 },
                new TimedEvent { TriggerTime = TimerSet - 1.6f, Callback = Event3 }
            }
        );
    }
    private void Event1()
    {
        Transform atk;
        atk = GameObject.Instantiate(AtkPrefab1, _controller._transform.position, Quaternion.identity).transform;
        switch (PlayerController._player.face)
        {
            case Creature.Face.Right:
                atk.localScale = new Vector3(1, 1, 1);
                break;
            case Creature.Face.Left:
                atk.localScale = new Vector3(-1, 1, 1);
                break;
        }
    }
    private void Event2()
    {
        Transform atk;
        atk = GameObject.Instantiate(AtkPrefab2, _controller._transform.position, Quaternion.identity).transform;
        switch (PlayerController._player.face)
        {
            case Creature.Face.Right:
                atk.localScale = new Vector3(1, 1, 1);
                break;
            case Creature.Face.Left:
                atk.localScale = new Vector3(-1, 1, 1);
                break;
        }
    }
    private void Event3()
    {
        RemoveCommandFromSet();
    }
}
public class PlayerBlockStrongAtkStatus : PlayerStatus, IPlayerAniUser, IObserver
{
    private InvincibleManager _invincibleManager;
    private BattleSystem _battleSystem;
    private PlayerBlockAtkAni _ani;
    private Buff InvincibleBuff;
    private GameObject AtkPrefab;
    private float TimerSet;
    private float Timer;
    private Queue<TimedEvent> _eventQueue;

    public PlayerBlockStrongAtkStatus(PlayerController controller, BattleSystem battle, InvincibleManager invincible, PlayerBlockAtkAni Ani, Buff invincibleBuff)
    {
        _controller = controller;
        _battleSystem = battle;
        _invincibleManager = invincible;
        InvincibleBuff = invincibleBuff;
        _ani = Ani;

        Inisialize(Status.BlockAtk, OperateType.FixedUpdate);

        TimerSet = _battleSystem.BlockStrongAtkTimerSet;
        AtkPrefab = _battleSystem.BlockStrongAtk;

        if (_controller != null && _battleSystem != null && _invincibleManager != null && _ani != null && InvincibleBuff != null)
        {
            isInitializeSuccess = true;
        }
        else
        {
            Debug.LogWarning("InitialPlayerStatusFail");
        }
    }

    public int GetAnimationPriority()
    {
        return _ani.PlayPriority;
    }
    public AnimationController GetAnimation()
    {
        return _ani;
    }
    public void ReceiveNotify()
    {
        if (!isInitializeSuccess)
        {
            return;
        }

        if (_battleSystem.isBlockSuccess && _battleSystem.SkillPower >= _battleSystem.StrongAtkCost)
        {
            _battleSystem.StopBlockSuccessBulletTime();
            AddCommandToSet();
        }
    }
    public override void Inisialize(Status status, OperateType operateType)
    {
        _statusType = status;
        _operateType = operateType;
    }
    public override void Begin()
    {
        if (!isInitializeSuccess)
        {
            return;
        }

        _controller.SetAnimation(_ani);
        _controller.PlayAnimation();
        _ani.ChangeAnimator("Type", "2");
        _ani.AniTurnFace(PlayerController._player.face);

        _battleSystem.UseSkillPower(_battleSystem.StrongAtkCost);
        _invincibleManager.AddInvincible(InvincibleManager.InvincibleType.Absolute);

        Timer = TimerSet;
        ResetQueue();
    }
    public override void End()
    {
        if (!isInitializeSuccess)
        {
            return;
        }

        _controller.StopAnimation(_ani);
        _battleSystem.BeginAtkCoolDown();
        _invincibleManager.RemoveInvincible(InvincibleManager.InvincibleType.Absolute);
        InvincibleBuff.AddBuffToSet();
    }
    public override void FixedExecute(float deltaTime)
    {
        if (!isInitializeSuccess)
        {
            return;
        }

        Timer -= deltaTime;

        if (_eventQueue.Count() > 0 && _eventQueue.Peek().TriggerTime >= Timer)
        {
            var timedEvent = _eventQueue.Dequeue();
            timedEvent.Callback?.Invoke();
        }
    }

    private void ResetQueue()
    {
        _eventQueue = new Queue<TimedEvent>(
            new[]
            {
                new TimedEvent { TriggerTime = TimerSet - 1.55f, Callback = Event1 },
                new TimedEvent { TriggerTime = 0, Callback = Event2 },
            }
        );
    }
    private void Event1()
    {
        Transform atk;
        atk = GameObject.Instantiate(AtkPrefab, _controller._transform.position, Quaternion.identity).transform;
        switch (PlayerController._player.face)
        {
            case Creature.Face.Right:
                atk.localScale = new Vector3(1, 1, 1);
                break;
            case Creature.Face.Left:
                atk.localScale = new Vector3(-1, 1, 1);
                break;
        }
    }
    private void Event2()
    {
        RemoveCommandFromSet();
    }
}
public class PlayerBeBlockStatus : PlayerStatus, IPlayerAniUser
{
    private BattleSystem _battleSystem;
    private PlayerBeBlockAni _ani;
    private float TimerSet;
    private float Timer;

    public PlayerBeBlockStatus(PlayerController controller, BattleSystem battle, PlayerBeBlockAni Ani)
    {
        _controller = controller;
        _battleSystem = battle;
        _ani = Ani;

        Inisialize(Status.BeBlock, OperateType.FixedUpdate);

        TimerSet = _battleSystem.BeBlockTimerSet;

        if (_controller != null && _battleSystem != null && _ani != null)
        {
            isInitializeSuccess = true;
        }
        else
        {
            Debug.LogWarning("InitialPlayerStatusFail");
        }
    }

    public int GetAnimationPriority()
    {
        return _ani.PlayPriority;
    }
    public AnimationController GetAnimation()
    {
        return _ani;
    }
    public override void Inisialize(Status status, OperateType operateType)
    {
        _statusType = status;
        _operateType = operateType;

        CommandReplaceSet.Add(Status.Hurted);
        CommandReplaceSet.Add(Status.Die);
    }
    public override void Begin()
    {
        if (!isInitializeSuccess)
        {
            return;
        }

        _controller.SetAnimation(_ani);
        _controller.PlayAnimation();
        _ani.AniTurnFace(PlayerController._player.face);

        Timer = TimerSet;
    }
    public override void End()
    {
        if (!isInitializeSuccess)
        {
            return;
        }

        _controller.StopAnimation(_ani);
    }
    public override void FixedExecute(float deltaTime)
    {
        if (!isInitializeSuccess)
        {
            return;
        }

        Timer -= deltaTime;

        if (Timer <= 0)
        {
            RemoveCommandFromSet();
        }
    }
}
public class PlayerWeakStatus : PlayerStatus, IPlayerAniUser
{
    private BattleSystem _battleSystem;
    private PlayerWeakAni _ani;
    private float TimerSet;
    private float Timer;
    private bool touchGround;
    private Queue<TimedEvent> _eventQueue;

    public PlayerWeakStatus(PlayerController controller, BattleSystem battle, PlayerWeakAni Ani)
    {
        _controller = controller;
        _battleSystem = battle;
        _ani = Ani;

        Inisialize(Status.Weak, OperateType.Both);

        TimerSet = _battleSystem.WeakTimerSet;

        if (_controller != null && _battleSystem != null && _ani != null)
        {
            isInitializeSuccess = true;
        }
        else
        {
            Debug.LogWarning("InitialPlayerStatusFail");
        }
    }

    public int GetAnimationPriority()
    {
        return _ani.PlayPriority;
    }
    public AnimationController GetAnimation()
    {
        return _ani;
    }
    public override void Inisialize(Status status, OperateType operateType)
    {
        _statusType = status;
        _operateType = operateType;

        CommandReplaceSet.Add(Status.Hurted);
        CommandReplaceSet.Add(Status.Die);
    }
    public override void Begin()
    {
        if (!isInitializeSuccess)
        {
            return;
        }

        _controller.SetAnimation(_ani);
        _controller.PlayAnimation();
        _ani.AniTurnFace(PlayerController._player.face);

        touchGround = false;
        Timer = TimerSet;
        ResetQueue();
    }
    public override void End()
    {
        if (!isInitializeSuccess)
        {
            return;
        }

        _controller.StopAnimation(_ani);
    }
    public override void Execute(float deltaTime)
    {
        if (!isInitializeSuccess)
        {
            return;
        }

        if (PlayerController.isGround)
        {
            _ani.ChangeAnimator("isGround", "true");
        }
    }
    public override void FixedExecute(float deltaTime)
    {
        if (!isInitializeSuccess || !touchGround)
        {
            return;
        }

        Timer -= deltaTime;

        if (_eventQueue.Count() > 0 && _eventQueue.Peek().TriggerTime >= Timer)
        {
            var timedEvent = _eventQueue.Dequeue();
            timedEvent.Callback?.Invoke();
        }
    }

    private void ResetQueue()
    {
        _eventQueue = new Queue<TimedEvent>(
            new[]
            {
                new TimedEvent { TriggerTime = TimerSet - 1.5f, Callback = Event1 },
                new TimedEvent { TriggerTime = 0, Callback = Event2 },
            }
        );
    }
    private void Event1()
    {
        _ani.ChangeAnimator("GetUp", "true");
    }
    private void Event2()
    {
        RemoveCommandFromSet();
    }
}

public class PlayerCocktailCriticAtkStatus : PlayerStatus, IPlayerAniUser
{
    private BattleSystem _battleSystem;
    private IThrowItem _item;
    private PlayerCocktailCriticAtkAni _ani;
    private GameObject AtkPrefab;
    private float TimerSet;
    private float Timer;
    private Queue<TimedEvent> _eventQueue;

    public PlayerCocktailCriticAtkStatus(PlayerController controller, BattleSystem battle, PlayerCocktailCriticAtkAni Ani)
    {
        _controller = controller;
        _battleSystem = battle;
        _ani = Ani;

        Inisialize(Status.UseThrowItem, OperateType.FixedUpdate);

        TimerSet = _battleSystem.CocktailCriticAtkTimerSet;
        AtkPrefab = _battleSystem.CocktailCriticAtk;

        if (_controller != null && _battleSystem != null && _ani != null)
        {
            isInitializeSuccess = true;
        }
        else
        {
            Debug.LogWarning("InitialPlayerStatusFail");
        }
    }

    public int GetAnimationPriority()
    {
        return _ani.PlayPriority;
    }
    public AnimationController GetAnimation()
    {
        return _ani;
    }
    public override void Inisialize(Status status, OperateType operateType)
    {
        _statusType = status;
        _operateType = operateType;

        CommandReplaceSet.Add(Status.Hurted);
        CommandReplaceSet.Add(Status.Die);
    }
    public override void Begin()
    {
        if (!isInitializeSuccess)
        {
            return;
        }

        _controller.SetAnimation(_ani);
        _controller.PlayAnimation();
        _ani.AniTurnFace(PlayerController._player.face);

        Timer = TimerSet;
        ResetQueue();
    }
    public override void End()
    {
        if (!isInitializeSuccess)
        {
            return;
        }

        _controller.StopAnimation(_ani);
    }
    public override void FixedExecute(float deltaTime)
    {
        if (!isInitializeSuccess)
        {
            return;
        }

        Timer -= deltaTime;

        if (_eventQueue.Count() > 0 && _eventQueue.Peek().TriggerTime >= Timer)
        {
            var timedEvent = _eventQueue.Dequeue();
            timedEvent.Callback?.Invoke();
        }
    }

    public void SetItem(IThrowItem item)
    {
        _item = item;
    }
    private void ResetQueue()
    {
        _eventQueue = new Queue<TimedEvent>(
            new[]
            {
                new TimedEvent { TriggerTime = TimerSet - 0.9f, Callback = Event1 },
                new TimedEvent { TriggerTime = 0, Callback = Event2 },
            }
        );
    }
    private void Event1()
    {
        _item.UseSuccess();
        switch (PlayerController._player.face)
        {
            case Creature.Face.Left:
                Transform atk = GameObject.Instantiate(AtkPrefab, _controller._transform.position, Quaternion.identity).transform;
                atk.localScale = new Vector3(-1, 1, 1);
                break;
            case Creature.Face.Right:
                GameObject.Instantiate(AtkPrefab, _controller._transform.position, Quaternion.identity);
                break;
        }
    }
    private void Event2()
    {
        RemoveCommandFromSet();
    }
}
public class PlayerImpulseJumpStatus : PlayerStatus, IPlayerAniUser
{
    private BattleSystem _battleSystem;
    private IThrowItem _item;
    private PlayerCocktailCriticAtkAni _ani;
    private GameObject Explosion;
    private float TimerSet;
    private float Timer;
    private Vector2 ImpulsePower;
    private Vector3 AppearPlace;

    public PlayerImpulseJumpStatus(PlayerController controller, BattleSystem battle, PlayerCocktailCriticAtkAni Ani)
    {
        _controller = controller;
        _battleSystem = battle;
        _ani = Ani;

        Inisialize(Status.UseThrowItem, OperateType.Update);

        TimerSet = _battleSystem.ImpulseJumpTimerSet;
        Explosion = _battleSystem.ImpulseJumpExplosion;
        ImpulsePower = _battleSystem.ImpulseJumpPower;
        AppearPlace = _battleSystem.ImpulseExplosionAppear;

        if (_controller != null && _battleSystem != null && _ani != null)
        {
            isInitializeSuccess = true;
        }
        else
        {
            Debug.LogWarning("InitialPlayerStatusFail");
        }
    }

    public int GetAnimationPriority()
    {
        return _ani.PlayPriority;
    }
    public AnimationController GetAnimation()
    {
        return _ani;
    }
    public override void Inisialize(Status status, OperateType operateType)
    {
        _statusType = status;
        _operateType = operateType;

        CommandReplaceSet.Add(Status.Hurted);
        CommandReplaceSet.Add(Status.Die);
    }
    public override void Begin()
    {
        if (!isInitializeSuccess)
        {
            return;
        }

        _controller.SetAnimation(_ani);
        _controller.PlayAnimation();
        _ani.AniTurnFace(PlayerController._player.face);

        Timer = TimerSet;

        _item.UseSuccess();

        Vector3 place = _controller._transform.localPosition;
        Vector2 power = ImpulsePower;
        switch (PlayerController._player.face)
        {
            case Creature.Face.Left:
                ImpulsePower = new Vector2(-ImpulsePower.x, ImpulsePower.y);
                place = new Vector2(place.x - AppearPlace.x, place.y + AppearPlace.y);
                break;
            case Creature.Face.Right:
                place = new Vector2(place.x + AppearPlace.x, place.y + AppearPlace.y);
                break;
        }
        GameObject.Instantiate(Explosion, place, Quaternion.identity);
        _controller.ForceRigidBody(power, ForceMode2D.Impulse);
    }
    public override void End()
    {
        if (!isInitializeSuccess)
        {
            return;
        }

        _controller.StopAnimation(_ani);
    }
    public override void Execute(float deltaTime)
    {
        if (!isInitializeSuccess)
        {
            return;
        }

        Timer -= deltaTime;

        if (PlayerController.isGround)
        {
            RemoveCommandFromSet();
        }

        if (Timer <= 0)
        {
            RemoveCommandFromSet();
        }
    }

    public void SetItem(IThrowItem item)
    {
        _item = item;
    }
}
