using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldManHurted : MonoBehaviour
{
    private int SpriteNumber = 17;
    private SpriteRenderer[] MoveSprList;
    private SpriteRenderer[] AtkSprList;
    private SpriteRenderer[] Atk2SprList;
    private ShieldManController _controller;
    private MonsterHurtedController _hurtedController;
    // Start is called before the first frame update
    void Start()
    {
        MoveSprList = new SpriteRenderer[SpriteNumber];
        AtkSprList = new SpriteRenderer[SpriteNumber];
        Atk2SprList = new SpriteRenderer[SpriteNumber];
        _controller = this.GetComponent<ShieldManController>();
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
    }

    // Update is called once per frame
    void Update()
    {
        if (_hurtedController.isHurted)
        {
            switch (_controller.NowAni)
            {
                case ShieldManController.AniStatus.Wait:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        MoveSprList[i].color = new Color(0.65f, 0.48f, 0.48f, 1);
                    }
                    break;
                case ShieldManController.AniStatus.Walk:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        MoveSprList[i].color = new Color(0.65f, 0.48f, 0.48f, 1);
                    }
                    break;
                case ShieldManController.AniStatus.Atk:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        AtkSprList[i].color = new Color(0.65f, 0.48f, 0.48f, 1);
                    }
                    break;
                case ShieldManController.AniStatus.Atk2:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        Atk2SprList[i].color = new Color(0.65f, 0.48f, 0.48f, 1);
                    }
                    break;
                case ShieldManController.AniStatus.HeavyWalk:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        MoveSprList[i].color = new Color(0.65f, 0.48f, 0.48f, 1);
                    }
                    break;
                case ShieldManController.AniStatus.DefandWait:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        MoveSprList[i].color = new Color(0.65f, 0.48f, 0.48f, 1);
                    }
                    break;
            }
        }
        else
        {
            switch (_controller.NowAni)
            {
                case ShieldManController.AniStatus.Wait:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        MoveSprList[i].color = new Color(1, 1, 1, 1);
                    }
                    break;
                case ShieldManController.AniStatus.Walk:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        MoveSprList[i].color = new Color(1, 1, 1, 1);
                    }
                    break;
                case ShieldManController.AniStatus.Atk:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        AtkSprList[i].color = new Color(1, 1, 1, 1);
                    }
                    break;
                case ShieldManController.AniStatus.Atk2:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        Atk2SprList[i].color = new Color(1, 1, 1, 1);
                    }
                    break;
                case ShieldManController.AniStatus.HeavyWalk:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        MoveSprList[i].color = new Color(1, 1, 1, 1);
                    }
                    break;
                case ShieldManController.AniStatus.DefandWait:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        MoveSprList[i].color = new Color(1, 1, 1, 1);
                    }
                    break;
            }
        }
    }
}
