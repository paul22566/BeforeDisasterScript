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
    void Execute(Monster monster, float deltaTime);
    void FixedExecute(Monster monster, float deltaTime);
    void Begin(Monster monster);
    void End(Monster monster);

    IMonsterStatus CheckTransition();
}
public interface IMonsterParameter
{
    void CalculateParameter();
}

public interface IOnceStrategyUser
{
    public void AddDefaultNextStatus(IMonsterStatus status);
    public bool DetectStrategyEnd();
}
public interface IOnceStrategy
{
    public bool ExecuteComplete();
}
public interface IMonsterAtkStrategy
{
    void FixedExecute(float deltaTime);
}
public interface IMonsterPatrolStrategy
{
    void Execute(float deltaTime);
    void FixedExecute(float deltaTime);
}
public interface IMonsterCooldownStrategy
{
    void Execute(Monster monster, float deltaTime);
    void FixedExecute(Monster monster, float deltaTime);
}
