using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ICommand
{
    void Execute();
}
public interface IObservableCommand : ICommand
{
    void Subscribe(IObserver observer);
    void Unsubscribe(IObserver observer);
}
public interface IObservable
{
    void Subscribe(IObserver observer);
    void Unsubscribe(IObserver observer);
}
public interface IObserver
{
    void ReceiveNotify();
}
public interface IAimSystemUser
{
    public void ReceiveAimDirection((float, float) direction, float deltaTime);
}
public interface IInputSource
{
    bool IsLeftMovePressing(NewKeyCodeManager manager);
    bool IsLeftMoveUp(NewKeyCodeManager manager);
    bool IsRightMovePressing(NewKeyCodeManager manager);
    bool IsRightMoveUp(NewKeyCodeManager manager);
    bool IsNormalAttackPressed(NewKeyCodeManager manager);
    bool IsNormalAttackPressing(NewKeyCodeManager manager);
    bool IsNormalAttackUp(NewKeyCodeManager manager);
    bool IsStrongAttackPressed(NewKeyCodeManager manager);
    bool IsJumpPressed(NewKeyCodeManager manager);
    bool IsJumpUp(NewKeyCodeManager manager);
    bool IsDashPressed(NewKeyCodeManager manager);
    bool IsRestorePressed(NewKeyCodeManager manager);
    bool IsUseItemPressed(NewKeyCodeManager manager);
    bool IsUseItemUp(NewKeyCodeManager manager);
    bool IsInteractPressed(NewKeyCodeManager manager);
    bool IsInteractUp(NewKeyCodeManager manager);
    bool IsShootPressed(NewKeyCodeManager manager);
    bool IsShootUp(NewKeyCodeManager manager);
    bool IsBlockPressed(NewKeyCodeManager manager);
    bool IsItemWindowPressed(NewKeyCodeManager manager);
    bool IsChangeItemPressed(NewKeyCodeManager manager);
    bool IsWalkThrowPrepare(NewKeyCodeManager manager);
    (float, float) InputAimDirection();

    void InputSourceDetect(NewKeyCodeManager manager);
}
public interface IAttackObject
{
    void InitializeAtk(AtkData _data);
    void MakeDamage(IHurtedObject _hurtedObject);
}
public interface IHurtedObject
{
    void HurtedControll(int damage);
    int GetCamp();
}

public struct AtkData
{
    public int CampID;
    public float AtkRate;
    public Transform MainObjectTransform;

    public AtkData(int id, float rate, Transform trans)
    {
        CampID = id;
        AtkRate = rate;
        MainObjectTransform = trans;
    }
}
public struct TimedEvent
{
    public float TriggerTime;
    public Action Callback;
}
