using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasterHurted : MonoBehaviour
{
    private int SpriteNumber = 16;
    private int NewSpriteNumber = 17;
    private SpriteRenderer[] MoveSprList;
    private SpriteRenderer[] AtkSprList;
    private CasterController _controller;
    private MonsterHurtedController _hurtedController;

    void Start()
    {
        MoveSprList = new SpriteRenderer[SpriteNumber];
        AtkSprList = new SpriteRenderer[NewSpriteNumber];
        _controller = this.GetComponent<CasterController>();
        _hurtedController = this.GetComponent<MonsterHurtedController>();
        for (int i = 0; i < SpriteNumber; i++)
        {
            MoveSprList[i] = this.transform.GetChild(0).GetChild(0).GetChild(i).GetComponent<SpriteRenderer>();
        }
        for (int i = 0; i < NewSpriteNumber; i++)
        {
            AtkSprList[i] = this.transform.GetChild(1).GetChild(0).GetChild(i).GetComponent<SpriteRenderer>();
        }
    }

    void Update()
    {
        if (_hurtedController.isHurted)
        {
            switch (_controller.NowAni)
            {
                case CasterController.AniStatus.Wait:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        MoveSprList[i].color = new Color(0.65f, 0.48f, 0.48f, 1);
                    }
                    break;
                case CasterController.AniStatus.Walk:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        MoveSprList[i].color = new Color(0.65f, 0.48f, 0.48f, 1);
                    }
                    break;
                case CasterController.AniStatus.Atk:
                    for (int i = 0; i < NewSpriteNumber; i++)
                    {
                        AtkSprList[i].color = new Color(0.65f, 0.48f, 0.48f, 1);
                    }
                    break;
            }
        }
        else
        {
            switch (_controller.NowAni)
            {
                case CasterController.AniStatus.Wait:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        MoveSprList[i].color = new Color(1, 1, 1, 1);
                    }
                    break;
                case CasterController.AniStatus.Walk:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        MoveSprList[i].color = new Color(1, 1, 1, 1);
                    }
                    break;
                case CasterController.AniStatus.Atk:
                    for (int i = 0; i < NewSpriteNumber; i++)
                    {
                        AtkSprList[i].color = new Color(1, 1, 1, 1);
                    }
                    break;
            }
        }
    }
}
