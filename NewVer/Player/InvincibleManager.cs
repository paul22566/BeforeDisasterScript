using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibleManager
{
    /*Weak���|�Q�Ǫ����� ���|����S������Τ��i�j�ק���
    Strong�򥻤W���|�Q�Ǫ����� ���|����S�����
    Absolute���|�������@�اΦ�������*/
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
