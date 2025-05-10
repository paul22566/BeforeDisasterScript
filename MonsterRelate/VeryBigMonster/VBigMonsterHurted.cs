using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VBigMonsterHurted : MonoBehaviour
{
    private const int SpriteNumber = 16;
    private VeryBigMonsterController _controller;
    [HideInInspector] public bool isValidHurted;
    public ObjectShield _shield;
    private SpriteRenderer[] WaitSprList;
    private SpriteRenderer[] WalkSprList;
    private SpriteRenderer[] JumpSprList;
    private SpriteRenderer[] AtkSprList;
    private SpriteRenderer[] Atk1_5SprList;
    private SpriteRenderer[] Atk2SprList;
    private SpriteRenderer[] Atk3SprList;
    private SpriteRenderer[] Atk4SprList;
    private SpriteRenderer[] SummonSprList;
    private SpriteRenderer[] CaptureSprList;
    private SpriteRenderer[] WeakSprList;
    private MonsterHurtedController _hurtedController;

    void Start()
    {
        WaitSprList = new SpriteRenderer[SpriteNumber];
        WalkSprList = new SpriteRenderer[SpriteNumber];
        JumpSprList = new SpriteRenderer[SpriteNumber];
        AtkSprList = new SpriteRenderer[SpriteNumber];
        Atk1_5SprList = new SpriteRenderer[SpriteNumber];
        Atk2SprList = new SpriteRenderer[SpriteNumber];
        Atk3SprList = new SpriteRenderer[SpriteNumber];
        Atk4SprList = new SpriteRenderer[SpriteNumber];
        SummonSprList = new SpriteRenderer[SpriteNumber];
        CaptureSprList = new SpriteRenderer[SpriteNumber];
        WeakSprList = new SpriteRenderer[SpriteNumber];
        for (int i = 0; i < SpriteNumber; i++)
        {
            WaitSprList[i] = this.transform.GetChild(0).GetChild(0).GetChild(i).GetComponent<SpriteRenderer>();
        }
        for (int i = 0; i < SpriteNumber; i++)
        {
            WalkSprList[i] = this.transform.GetChild(1).GetChild(0).GetChild(i).GetComponent<SpriteRenderer>();
        }
        for (int i = 0; i < SpriteNumber; i++)
        {
            JumpSprList[i] = this.transform.GetChild(2).GetChild(0).GetChild(i).GetComponent<SpriteRenderer>();
        }
        for (int i = 0; i < SpriteNumber; i++)
        {
            AtkSprList[i] = this.transform.GetChild(3).GetChild(0).GetChild(i).GetComponent<SpriteRenderer>();
        }
        for (int i = 0; i < SpriteNumber; i++)
        {
            Atk1_5SprList[i] = this.transform.GetChild(4).GetChild(0).GetChild(i).GetComponent<SpriteRenderer>();
        }
        for (int i = 0; i < SpriteNumber; i++)
        {
            Atk2SprList[i] = this.transform.GetChild(5).GetChild(0).GetChild(i).GetComponent<SpriteRenderer>();
        }
        for (int i = 0; i < SpriteNumber; i++)
        {
            Atk3SprList[i] = this.transform.GetChild(6).GetChild(0).GetChild(i).GetComponent<SpriteRenderer>();
        }
        for (int i = 0; i < SpriteNumber; i++)
        {
            Atk4SprList[i] = this.transform.GetChild(7).GetChild(0).GetChild(i).GetComponent<SpriteRenderer>();
        }
        for (int i = 0; i < SpriteNumber; i++)
        {
            SummonSprList[i] = this.transform.GetChild(8).GetChild(0).GetChild(i).GetComponent<SpriteRenderer>();
        }
        for (int i = 0; i < SpriteNumber; i++)
        {
            CaptureSprList[i] = this.transform.GetChild(9).GetChild(0).GetChild(i).GetComponent<SpriteRenderer>();
        }
        for (int i = 0; i < SpriteNumber; i++)
        {
            WeakSprList[i] = this.transform.GetChild(10).GetChild(0).GetChild(i).GetComponent<SpriteRenderer>();
        }
        _controller = this.GetComponent<VeryBigMonsterController>();
        _hurtedController = this.GetComponent<MonsterHurtedController>();
    }

    void Update()
    {
        if (_hurtedController.isHurted && _shield.ProtectSuccess)
        {
            isValidHurted = true;
        }
        if (!_hurtedController.isHurted)
        {
            isValidHurted = false;
        }

        if (isValidHurted)
        {
            switch (_controller.NowAni)
            {
                case VeryBigMonsterController.AniStatus.wait:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        WaitSprList[i].color = new Color(0.65f, 0.48f, 0.48f, 1);
                    }
                    break;
                case VeryBigMonsterController.AniStatus.walk:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        WalkSprList[i].color = new Color(0.65f, 0.48f, 0.48f, 1);
                    }
                    break;
                case VeryBigMonsterController.AniStatus.Dash:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        WalkSprList[i].color = new Color(0.65f, 0.48f, 0.48f, 1);
                    }
                    break;
                case VeryBigMonsterController.AniStatus.Jump:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        JumpSprList[i].color = new Color(0.65f, 0.48f, 0.48f, 1);
                    }
                    break;
                case VeryBigMonsterController.AniStatus.Atk1:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        AtkSprList[i].color = new Color(0.65f, 0.48f, 0.48f, 1);
                    }
                    break;
                case VeryBigMonsterController.AniStatus.Atk1_5:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        Atk1_5SprList[i].color = new Color(0.65f, 0.48f, 0.48f, 1);
                    }
                    break;
                case VeryBigMonsterController.AniStatus.Atk2:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        Atk2SprList[i].color = new Color(0.65f, 0.48f, 0.48f, 1);
                    }
                    break;
                case VeryBigMonsterController.AniStatus.Atk3:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        Atk3SprList[i].color = new Color(0.65f, 0.48f, 0.48f, 1);
                    }
                    break;
                case VeryBigMonsterController.AniStatus.Atk4:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        Atk4SprList[i].color = new Color(0.65f, 0.48f, 0.48f, 1);
                    }
                    break;
                case VeryBigMonsterController.AniStatus.Summon:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        SummonSprList[i].color = new Color(0.65f, 0.48f, 0.48f, 1);
                    }
                    break;
                case VeryBigMonsterController.AniStatus.Capture:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        CaptureSprList[i].color = new Color(0.65f, 0.48f, 0.48f, 1);
                    }
                    break;
                case VeryBigMonsterController.AniStatus.Weak:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        WeakSprList[i].color = new Color(0.65f, 0.48f, 0.48f, 1);
                    }
                    break;
            }
        }
        else
        {
            switch (_controller.NowAni)
            {
                case VeryBigMonsterController.AniStatus.wait:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        WaitSprList[i].color = new Color(1, 1, 1, 1);
                    }
                    break;
                case VeryBigMonsterController.AniStatus.walk:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        WalkSprList[i].color = new Color(1, 1, 1, 1);
                    }
                    break;
                case VeryBigMonsterController.AniStatus.Dash:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        WalkSprList[i].color = new Color(1, 1, 1, 1);
                    }
                    break;
                case VeryBigMonsterController.AniStatus.Jump:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        JumpSprList[i].color = new Color(1, 1, 1, 1);
                    }
                    break;
                case VeryBigMonsterController.AniStatus.Atk1:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        AtkSprList[i].color = new Color(1, 1, 1, 1);
                    }
                    break;
                case VeryBigMonsterController.AniStatus.Atk1_5:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        Atk1_5SprList[i].color = new Color(1,1,1,1);
                    }
                    break;
                case VeryBigMonsterController.AniStatus.Atk2:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        Atk2SprList[i].color = new Color(1, 1, 1, 1);
                    }
                    break;
                case VeryBigMonsterController.AniStatus.Atk3:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        Atk3SprList[i].color = new Color(1, 1, 1, 1);
                    }
                    break;
                case VeryBigMonsterController.AniStatus.Atk4:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        Atk4SprList[i].color = new Color(1, 1, 1, 1);
                    }
                    break;
                case VeryBigMonsterController.AniStatus.Summon:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        SummonSprList[i].color = new Color(1, 1, 1, 1);
                    }
                    break;
                case VeryBigMonsterController.AniStatus.Capture:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        CaptureSprList[i].color = new Color(1, 1, 1, 1);
                    }
                    break;
                case VeryBigMonsterController.AniStatus.Weak:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        WeakSprList[i].color = new Color(1, 1, 1, 1);
                    }
                    break;
            }
        }
    }
}
