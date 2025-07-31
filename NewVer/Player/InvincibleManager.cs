using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibleManager
{
    /*Weak不會被怪物普攻 但會受到特殊攻擊及不可迴避攻擊
    Strong基本上不會被怪物攻擊 但會受到特殊攻擊
    Absolute不會受到任何一種形式的攻擊*/
    public enum InvincibleType {Weak, Strong, Absolute };
    private Dictionary<InvincibleType, int> InvincibleSubscribers = new Dictionary<InvincibleType, int>();

    public InvincibleManager()
    {
        InvincibleSubscribers.Add(InvincibleType.Weak, 0);
        InvincibleSubscribers.Add(InvincibleType.Strong, 0);
        InvincibleSubscribers.Add(InvincibleType.Absolute, 0);
    }

    public bool GetInvincible(InvincibleType invincibleType)
    {
        return InvincibleSubscribers[invincibleType] > 0;
    }
    public void AddInvincible(InvincibleType invincibleType)
    {
        InvincibleSubscribers[invincibleType] += 1;
    }
    public void RemoveInvincible(InvincibleType invincibleType)
    {
        InvincibleSubscribers[invincibleType] -= 1;
    }
}
