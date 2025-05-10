using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilKingHurted : MonoBehaviour
{
    private int SpriteNumber = 14;
    private SpriteRenderer[] MoveSprList;
    private SpriteRenderer[] AtkSprList;
    private SpriteRenderer[] AtkWaitSprList;
    private SpriteRenderer[] AtkEndSprList;
    private SpriteRenderer[] WeakSprList;
    private EvilKingController _controller;
    private MonsterHurtedController _hurtedController;
    // Start is called before the first frame update
    void Start()
    {
        MoveSprList = new SpriteRenderer[SpriteNumber];
        AtkSprList = new SpriteRenderer[SpriteNumber];
        AtkWaitSprList = new SpriteRenderer[SpriteNumber];
        AtkEndSprList = new SpriteRenderer[SpriteNumber];
        WeakSprList = new SpriteRenderer[SpriteNumber];
        _controller = this.GetComponent<EvilKingController>();
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
            AtkWaitSprList[i] = this.transform.GetChild(2).GetChild(0).GetChild(i).GetComponent<SpriteRenderer>();
        }
        for (int i = 0; i < SpriteNumber; i++)
        {
            AtkEndSprList[i] = this.transform.GetChild(3).GetChild(0).GetChild(i).GetComponent<SpriteRenderer>();
        }
        for (int i = 0; i < SpriteNumber; i++)
        {
            WeakSprList[i] = this.transform.GetChild(4).GetChild(0).GetChild(i).GetComponent<SpriteRenderer>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_hurtedController.isHurted)
        {
            switch (_controller.NowAni)
            {
                case EvilKingController.AniStatus.Wait:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        MoveSprList[i].color = new Color(1, 0, 0, 1);
                    }
                    break;
                case EvilKingController.AniStatus.Move:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        MoveSprList[i].color = new Color(1, 0, 0, 1);
                    }
                    break;
                case EvilKingController.AniStatus.Atk:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        AtkSprList[i].color = new Color(1, 0, 0, 1);
                    }
                    break;
                case EvilKingController.AniStatus.AtkWait:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        AtkWaitSprList[i].color = new Color(1, 0, 0, 1);
                    }
                    break;
                case EvilKingController.AniStatus.AtkEnd:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        AtkEndSprList[i].color = new Color(1, 0, 0, 1);
                    }
                    break;
                case EvilKingController.AniStatus.Weak:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        WeakSprList[i].color = new Color(1, 0, 0, 1);
                    }
                    break;
            }
        }
        else
        {
            switch (_controller.NowAni)
            {
                case EvilKingController.AniStatus.Wait:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        MoveSprList[i].color = new Color(1, 1, 1, 1);
                    }
                    break;
                case EvilKingController.AniStatus.Move:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        MoveSprList[i].color = new Color(1, 1, 1, 1);
                    }
                    break;
                case EvilKingController.AniStatus.Atk:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        AtkSprList[i].color = new Color(1, 1, 1, 1);
                    }
                    break;
                case EvilKingController.AniStatus.AtkWait:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        AtkWaitSprList[i].color = new Color(1, 1, 1, 1);
                    }
                    break;
                case EvilKingController.AniStatus.AtkEnd:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        AtkEndSprList[i].color = new Color(1, 1, 1, 1);
                    }
                    break;
                case EvilKingController.AniStatus.Weak:
                    for (int i = 0; i < SpriteNumber; i++)
                    {
                        WeakSprList[i].color = new Color(1, 1, 1, 1);
                    }
                    break;
            }
        }
    }
}

