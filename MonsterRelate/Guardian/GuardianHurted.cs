using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianHurted : MonoBehaviour
{
    private const int SpriteNumber = 16;
    private GuardianController _controller;
    [HideInInspector] public bool isValidHurted;
    public ObjectShield _shield;
    private SpriteRenderer[] WaitSprList;
    private SpriteRenderer[] WalkSprList;
    private SpriteRenderer[] JumpSprList;
    private SpriteRenderer[] AtkSprList;
    private SpriteRenderer[] Atk2SprList;
    private SpriteRenderer[] BackAtk2SprList;
    private SpriteRenderer[] BeginAtkSprList;
    private MonsterHurtedController _hurtedController;
    void Start()
    {
        WaitSprList = new SpriteRenderer[SpriteNumber];
        WalkSprList = new SpriteRenderer[SpriteNumber];
        JumpSprList = new SpriteRenderer[SpriteNumber];
        AtkSprList = new SpriteRenderer[SpriteNumber];
        Atk2SprList = new SpriteRenderer[SpriteNumber];
        BackAtk2SprList = new SpriteRenderer[SpriteNumber];
        BeginAtkSprList = new SpriteRenderer[SpriteNumber];
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
            Atk2SprList[i] = this.transform.GetChild(4).GetChild(0).GetChild(i).GetComponent<SpriteRenderer>();
        }
        for (int i = 0; i < SpriteNumber; i++)
        {
            BackAtk2SprList[i] = this.transform.GetChild(5).GetChild(0).GetChild(i).GetComponent<SpriteRenderer>();
        }
        for (int i = 0; i < SpriteNumber; i++)
        {
            BeginAtkSprList[i] = this.transform.GetChild(7).GetChild(0).GetChild(i).GetComponent<SpriteRenderer>();
        }
        _controller = this.GetComponent<GuardianController>();
        _hurtedController = this.GetComponent<MonsterHurtedController>();
    }

    // Update is called once per frame
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
                case GuardianController.AniStatus.wait:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        WaitSprList[i].color = new Color(0.65f, 0.48f, 0.48f, 1);
                    }
                    break;
                case GuardianController.AniStatus.walk:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        WalkSprList[i].color = new Color(0.65f, 0.48f, 0.48f, 1);
                    }
                    break;
                case GuardianController.AniStatus.Jump:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        JumpSprList[i].color = new Color(0.65f, 0.48f, 0.48f, 1);
                    }
                    break;
                case GuardianController.AniStatus.Atk1:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        AtkSprList[i].color = new Color(0.65f, 0.48f, 0.48f, 1);
                    }
                    break;
                case GuardianController.AniStatus.Atk2:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        Atk2SprList[i].color = new Color(0.65f, 0.48f, 0.48f, 1);
                    }
                    break;
                case GuardianController.AniStatus.BackAtk2:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        BackAtk2SprList[i].color = new Color(0.65f, 0.48f, 0.48f, 1);
                    }
                    break;
                case GuardianController.AniStatus.Begining:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        BeginAtkSprList[i].color = new Color(0.65f, 0.48f, 0.48f, 1);
                    }
                    break;
            }
        }
        else
        {
            switch (_controller.NowAni)
            {
                case GuardianController.AniStatus.wait:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        WaitSprList[i].color = new Color(1, 1, 1, 1);
                    }
                    break;
                case GuardianController.AniStatus.walk:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        WalkSprList[i].color = new Color(1, 1, 1, 1);
                    }
                    break;
                case GuardianController.AniStatus.Jump:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        JumpSprList[i].color = new Color(1, 1, 1, 1);
                    }
                    break;
                 case GuardianController.AniStatus.Atk1:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        AtkSprList[i].color = new Color(1, 1, 1, 1);
                    }
                    break;
                case GuardianController.AniStatus.Atk2:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        Atk2SprList[i].color = new Color(1, 1, 1, 1);
                    }
                    break;
                case GuardianController.AniStatus.BackAtk2:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        BackAtk2SprList[i].color = new Color(1, 1, 1, 1);
                    }
                    break;
                case GuardianController.AniStatus.Begining:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        BeginAtkSprList[i].color = new Color(1, 1, 1, 1);
                    }
                    break;
            }
        }
    }
}
