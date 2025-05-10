using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogHurted : MonoBehaviour
{
    private SpriteRenderer MoveSpr;
    private SpriteRenderer AtkSpr;
    private SpriteRenderer AtkWaitSpr;
    private DogController _controller;
    private MonsterHurtedController _hurtedController;

    void Start()
    {
        _controller = this.GetComponent<DogController>();
        _hurtedController = this.GetComponent<MonsterHurtedController>();
        MoveSpr = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
        AtkSpr = this.transform.GetChild(1).GetComponent<SpriteRenderer>();
        AtkWaitSpr = this.transform.GetChild(4).GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (_hurtedController.isHurted)
        {
            switch (_controller.NowAni)
            {
                case DogController.AniStatus.Wait:
                    MoveSpr.color = new Color(0.65f, 0.48f, 0.48f, 1);
                    break;
                case DogController.AniStatus.Walk:
                    MoveSpr.color = new Color(0.65f, 0.48f, 0.48f, 1);
                    break;
                case DogController.AniStatus.Atk:
                    AtkSpr.color = new Color(0.65f, 0.48f, 0.48f, 1);
                    break;
                case DogController.AniStatus.AtkWait:
                    AtkWaitSpr.color = new Color(0.65f, 0.48f, 0.48f, 1);
                    break;
            }
        }
        else
        {
            switch (_controller.NowAni)
            {
                case DogController.AniStatus.Wait:
                    MoveSpr.color = new Color(1, 1, 1, 1);
                    break;
                case DogController.AniStatus.Walk:
                    MoveSpr.color = new Color(1, 1, 1, 1);
                    break;
                case DogController.AniStatus.Atk:
                    AtkSpr.color = new Color(1, 1, 1, 1);
                    break;
                case DogController.AniStatus.AtkWait:
                    AtkWaitSpr.color = new Color(1, 1, 1, 1);
                    break;
            }
        }
    }
}
