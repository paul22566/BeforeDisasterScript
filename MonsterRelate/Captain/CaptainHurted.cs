using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptainHurted : MonoBehaviour
{
    private int SpriteNumber = 16;
    private SpriteRenderer[] WaitSprList;
    private SpriteRenderer[] WalkSprList;
    private SpriteRenderer[] JumpSprList;
    private SpriteRenderer[] AtkSprList;
    private SpriteRenderer[] Atk2SprList;
    private SpriteRenderer[] Atk3SprList;
    private SpriteRenderer[] Atk5SprList;
    private SpriteRenderer[] WeakSprList;
    private SpriteRenderer Atk4Spr;
    private CaptainController _controller;
    private MonsterHurtedController _hurtedController;

    void Start()
    {
        WaitSprList = new SpriteRenderer[SpriteNumber];
        WalkSprList = new SpriteRenderer[SpriteNumber];
        JumpSprList = new SpriteRenderer[SpriteNumber];
        AtkSprList = new SpriteRenderer[SpriteNumber];
        AtkSprList = new SpriteRenderer[SpriteNumber];
        Atk2SprList = new SpriteRenderer[SpriteNumber];
        Atk3SprList = new SpriteRenderer[SpriteNumber];
        Atk5SprList = new SpriteRenderer[SpriteNumber];
        WeakSprList = new SpriteRenderer[SpriteNumber];
        Atk4Spr = this.transform.GetChild(6).GetComponent<SpriteRenderer>();
        _controller = this.GetComponent<CaptainController>();
        _hurtedController = this.GetComponent<MonsterHurtedController>();
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
            Atk3SprList[i] = this.transform.GetChild(5).GetChild(0).GetChild(i).GetComponent<SpriteRenderer>();
        }
        for (int i = 0; i < SpriteNumber; i++)
        {
            Atk5SprList[i] = this.transform.GetChild(7).GetChild(0).GetChild(i).GetComponent<SpriteRenderer>();
        }
        for (int i = 0; i < SpriteNumber; i++)
        {
            WeakSprList[i] = this.transform.GetChild(8).GetChild(0).GetChild(i).GetComponent<SpriteRenderer>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_hurtedController.isHurted)
        {
            switch (_controller.NowAni)
            {
                case CaptainController.AniStatus.Wait:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        WaitSprList[i].color = new Color(0.65f, 0.48f, 0.48f, 1);
                    }
                    break;
                case CaptainController.AniStatus.Walk:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        WalkSprList[i].color = new Color(0.65f, 0.48f, 0.48f, 1);
                    }
                    break;
                case CaptainController.AniStatus.Jump:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        JumpSprList[i].color = new Color(0.65f, 0.48f, 0.48f, 1);
                    }
                    break;
                case CaptainController.AniStatus.Atk:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        AtkSprList[i].color = new Color(0.65f, 0.48f, 0.48f, 1);
                    }
                    break;
                case CaptainController.AniStatus.Atk2:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        Atk2SprList[i].color = new Color(0.65f, 0.48f, 0.48f, 1);
                    }
                    break;
                case CaptainController.AniStatus.Atk3:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        Atk3SprList[i].color = new Color(0.65f, 0.48f, 0.48f, 1);
                    }
                    break;
                case CaptainController.AniStatus.Atk5:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        Atk5SprList[i].color = new Color(0.65f, 0.48f, 0.48f, 1);
                    }
                    break;
                case CaptainController.AniStatus.Weak:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        WeakSprList[i].color = new Color(0.65f, 0.48f, 0.48f, 1);
                    }
                    break;
                case CaptainController.AniStatus.Atk4:
                    Atk4Spr.color = new Color(0.65f, 0.48f, 0.48f, 1);
                    break;
            }
        }
        else
        {
            switch (_controller.NowAni)
            {
                case CaptainController.AniStatus.Wait:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        WaitSprList[i].color = new Color(1, 1, 1, 1);
                    }
                    break;
                case CaptainController.AniStatus.Walk:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        WalkSprList[i].color = new Color(1, 1, 1, 1);
                    }
                    break;
                case CaptainController.AniStatus.Jump:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        JumpSprList[i].color = new Color(1, 1, 1, 1);
                    }
                    break;
                case CaptainController.AniStatus.Atk:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        AtkSprList[i].color = new Color(1, 1, 1, 1);
                    }
                    break;
                case CaptainController.AniStatus.Atk2:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        Atk2SprList[i].color = new Color(1, 1, 1, 1);
                    }
                    break;
                case CaptainController.AniStatus.Atk3:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        Atk3SprList[i].color = new Color(1, 1, 1, 1);
                    }
                    break;
                case CaptainController.AniStatus.Atk5:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        Atk5SprList[i].color = new Color(1, 1, 1, 1);
                    }
                    break;
                case CaptainController.AniStatus.Weak:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        WeakSprList[i].color = new Color(1, 1, 1, 1);
                    }
                    break;
                case CaptainController.AniStatus.Atk4:
                    Atk4Spr.color = new Color(1, 1, 1, 1);
                    break;
            }
        }
    }
}
