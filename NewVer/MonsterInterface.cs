using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public interface IMonsterFactory
{
    Monster CreateMonster();
}

public interface IMonsterStatus
{
    void action(Monster monster);
    bool leaveJudge(Monster monster);
    void Begin(Monster monster);
    void End(Monster monster);
}
public interface IMonsterParameter
{
    void CalculateParameter();
}

public interface IStrategyUser<TStrategy>
{
    public void SetStrategy(TStrategy strategy);
    public void SetAni(AnimationController ani);
}
public interface IOnceStrategyUser
{
    public void StrategyEnd();//該策略結束時的動作
}

public interface IStrategyReset
{
    public void StrategyReset();
}
public interface IMonsterAtkStrategy
{
    void Atk(Action endNotice);
}
public interface IMonsterPatrolStrategy
{
    void Patrol();
}
public interface IMonsterCooldownStrategy
{
    void CooldownAction(Monster monster, Action endAction);
}
