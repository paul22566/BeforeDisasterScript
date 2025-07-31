using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuffManager
{
    public AtkPowerBuff atkPowerBuff;
    public InhibitBuff inhibitBuff;
    public StrongInvincibleBuff strongInvincibleBuff;
    public PlayerBuffManager(PlayerController controller, BattleSystem battleSystem)
    {
        atkPowerBuff = new AtkPowerBuff(controller, battleSystem);
        inhibitBuff = new InhibitBuff(controller, battleSystem);
        strongInvincibleBuff = new StrongInvincibleBuff(controller, controller._invincibleManager);
    }
}
public abstract class Buff
{
    protected PlayerController _controller;
    protected float Timer;
    protected float TimerSet;

    protected bool isInitializeCorrect = false;

    public abstract void Begin();
    public abstract void End();
    public virtual void Execute(float deltaTime)
    {
        if (!isInitializeCorrect)
        {
            return;
        }

        Timer -= deltaTime;
        if (Timer <= 0)
        {
            RemoveBuffFromSet();
        }
    }
    public void AddBuffToSet()
    {
        if (!isInitializeCorrect)
        {
            return;
        }

        if (_controller.RunningBuffs.Contains(this))
        {
            TimeReset();
            return;
        }

        _controller.RunningBuffs.Add(this);
        Begin();
    }
    public void RemoveBuffFromSet()
    {
        if (!isInitializeCorrect)
        {
            return;
        }

        if (_controller.RunningBuffs.Contains(this))
        {
            _controller.RunningBuffs.Remove(this);
            End();
        }
    }
    private void TimeReset()
    {
        Timer = TimerSet;
    }
}
public class AtkPowerBuff : Buff
{
    BattleSystem _battleSystem;
    private GameObject BuffUI;

    public AtkPowerBuff(PlayerController controller, BattleSystem battleSystem)
    {
        _controller = controller;
        _battleSystem = battleSystem;
        TimerSet = _battleSystem.SharpTimeSet;
        BuffUI = controller.AtkBuffUI;

        if (_controller != null && _battleSystem != null && BuffUI != null)
        {
            isInitializeCorrect = true;
        }
        else
        {
            Debug.LogWarning("InisialBuffWrong");
        }
    }
    public override void Begin()
    {
        if (!isInitializeCorrect)
        {
            return;
        }

        Timer = TimerSet;
        BuffUI.SetActive(true);
        _battleSystem.SharpBladeSuccess();
    }
    public override void End()
    {
        if (!isInitializeCorrect)
        {
            return;
        }

        BuffUI.SetActive(false);
        _battleSystem.ResetAtkPower();
    }
}
public class InhibitBuff : Buff
{
    BattleSystem _battleSystem;

    public InhibitBuff(PlayerController controller, BattleSystem battleSystem)
    {
        _controller = controller;
        _battleSystem = battleSystem;
        TimerSet = _battleSystem.InhibitTimeSet;

        if (_controller != null && _battleSystem != null)
        {
            isInitializeCorrect = true;
        }
        else
        {
            Debug.LogWarning("InisialBuffWrong");
        }
    }
    public override void Begin()
    {
        if (!isInitializeCorrect)
        {
            return;
        }

        Timer = TimerSet;
        _battleSystem.InhibitSuccess();
    }
    public override void End()
    {
        if (!isInitializeCorrect)
        {
            return;
        }

        _battleSystem.InhibitEnd();
    }
}
public class StrongInvincibleBuff : Buff
{
    private InvincibleManager _invincibleManager;
    private float SparkTimer;
    private float SparkTimerSet;

    public StrongInvincibleBuff(PlayerController controller, InvincibleManager invincible)
    {
        _controller = controller;
        _invincibleManager = invincible;
        TimerSet = _controller.HurtedInvincibleTimerSet;
        SparkTimerSet = _controller.HurtedInvincibleSparkTimerSet;

        if (_controller != null && _invincibleManager != null)
        {
            isInitializeCorrect = true;
        }
        else
        {
            Debug.LogWarning("InisialBuffWrong");
        }
    }

    public override void Execute(float deltaTime)
    {
        if (!isInitializeCorrect)
        {
            return;
        }

        base.Execute(deltaTime);

        SparkTimer -= deltaTime;

        if (SparkTimer <= 0.1f)
        {
            _controller.NowPlayingAni?.ChangeAniColor(new Color(1, 1, 1, 1));
        }
        if (SparkTimer <= 0)
        {
            _controller.NowPlayingAni?.ChangeAniColor(new Color(1, 1, 1, 0.5f));
            SparkTimer = SparkTimerSet;
        }
    }  

    public override void Begin()
    {
        if (!isInitializeCorrect)
        {
            return;
        }

        Timer = TimerSet;
        SparkTimer = SparkTimerSet;
        _invincibleManager?.AddInvincible(InvincibleManager.InvincibleType.Strong);

        _controller.NowPlayingAni?.ChangeAniColor(new Color(1,1,1,0.5f));
    }
    public override void End()
    {
        if (!isInitializeCorrect)
        {
            return;
        }

        _invincibleManager?.RemoveInvincible(InvincibleManager.InvincibleType.Strong);
        _controller.NowPlayingAni?.ChangeAniColor(new Color(1, 1, 1, 1));
    }
}
