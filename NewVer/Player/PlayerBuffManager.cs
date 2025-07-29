using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuffManager
{
    public AtkPowerBuff atkPowerBuff;
    public InhibitBuff inhibitBuff;
    public PlayerBuffManager(PlayerController controller, BattleSystem battleSystem)
    {
        atkPowerBuff = new AtkPowerBuff(controller, battleSystem);
        inhibitBuff = new InhibitBuff(controller, battleSystem);
    }
}
public abstract class Buff
{
    protected PlayerController _controller;
    protected float Timer;
    protected float TimerSet;

    public abstract void Begin();
    public abstract void End();
    public void CalculateTimer(float deltaTime)
    {
        Timer -= deltaTime;
        if (Timer <= 0)
        {
            RemoveBuffFromSet();
        }
    }
    public void AddBuffToSet()
    {
        if (_controller.RunningBuffs.Contains(this))
        {
            return;
        }

        _controller.RunningBuffs.Add(this);
        Begin();
    }
    public void RemoveBuffFromSet()
    {
        if (_controller.RunningBuffs.Contains(this))
        {
            _controller.RunningBuffs.Remove(this);
            End();
        }
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
    }
    public override void Begin()
    {
        Timer = TimerSet;
        BuffUI.SetActive(true);
        _battleSystem.SharpBladeSuccess();
    }
    public override void End()
    {
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
    }
    public override void Begin()
    {
        Timer = TimerSet;
        _battleSystem.InhibitSuccess();
    }
    public override void End()
    {
        _battleSystem.InhibitEnd();
    }
}
