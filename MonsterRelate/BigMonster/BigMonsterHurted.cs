using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BigMonsterHurted : MonoBehaviour
{
    private int SpriteNumber = 13;
    private SpriteRenderer[] MoveSprList;
    private SpriteRenderer[] AtkSprList;
    private SpriteRenderer[] Atk2SprList;
    private SpriteRenderer[] Atk3SprList;
    private SpriteRenderer[] JumpSprList;
    private BigMonsterController _controller;
    private MonsterHurtedController _hurtedController;

    void Start()
    {
        MoveSprList = new SpriteRenderer[SpriteNumber];
        AtkSprList = new SpriteRenderer[SpriteNumber];
        Atk2SprList = new SpriteRenderer[SpriteNumber];
        Atk3SprList = new SpriteRenderer[SpriteNumber];
        JumpSprList = new SpriteRenderer[SpriteNumber];
        _controller = this.GetComponent<BigMonsterController>();
        _hurtedController = this.GetComponent<MonsterHurtedController>();
        for (int i = 0; i < SpriteNumber; i++)
        {
            MoveSprList[i] = this.transform.GetChild(0).GetChild(0).GetChild(i).GetComponent<SpriteRenderer>();
        }
        for (int i = 0; i < SpriteNumber; i++)
        {
            AtkSprList[i] = this.transform.GetChild(1).GetChild(0).GetChild(i).GetComponent<SpriteRenderer>();
        }
        for (int i = 0; i < SpriteNumber; i++)
        {
            Atk2SprList[i] = this.transform.GetChild(2).GetChild(0).GetChild(i).GetComponent<SpriteRenderer>();
        }
        for (int i = 0; i < SpriteNumber; i++)
        {
            Atk3SprList[i] = this.transform.GetChild(3).GetChild(0).GetChild(i).GetComponent<SpriteRenderer>();
        }
        for (int i = 0; i < SpriteNumber; i++)
        {
            JumpSprList[i] = this.transform.GetChild(8).GetChild(0).GetChild(i).GetComponent<SpriteRenderer>();
        }
    }

    void Update()
    {
        if (_hurtedController.isHurted)
        {
            switch (_controller.NowAni)
            {
                case BigMonsterController.AniStatus.Wait:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        MoveSprList[i].color = new Color(0.65f, 0.48f, 0.48f, 1);
                    }
                    break;
                case BigMonsterController.AniStatus.Walk:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        MoveSprList[i].color = new Color(0.65f, 0.48f, 0.48f, 1);
                    }
                    break;
                case BigMonsterController.AniStatus.Atk:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        AtkSprList[i].color = new Color(0.65f, 0.48f, 0.48f, 1);
                    }
                    break;
                case BigMonsterController.AniStatus.Atk2:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        Atk2SprList[i].color = new Color(0.65f, 0.48f, 0.48f, 1);
                    }
                    break;
                case BigMonsterController.AniStatus.Atk3:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        Atk3SprList[i].color = new Color(0.65f, 0.48f, 0.48f, 1);
                    }
                    break;
                case BigMonsterController.AniStatus.Jump:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        JumpSprList[i].color = new Color(0.65f, 0.48f, 0.48f, 1);
                    }
                    break;
            }
        }
        else
        {
            switch (_controller.NowAni)
            {
                case BigMonsterController.AniStatus.Wait:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        MoveSprList[i].color = new Color(1, 1, 1, 1);
                    }
                    break;
                case BigMonsterController.AniStatus.Walk:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        MoveSprList[i].color = new Color(1, 1, 1, 1);
                    }
                    break;
                case BigMonsterController.AniStatus.Atk:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        AtkSprList[i].color = new Color(1, 1, 1, 1);
                    }
                    break;
                case BigMonsterController.AniStatus.Atk2:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        Atk2SprList[i].color = new Color(1, 1, 1, 1);
                    }
                    break;
                case BigMonsterController.AniStatus.Atk3:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        Atk3SprList[i].color = new Color(1, 1, 1, 1);
                    }
                    break;
                case BigMonsterController.AniStatus.Jump:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        JumpSprList[i].color = new Color(1, 1, 1, 1);
                    }
                    break;
            }
        }       
    }
}

