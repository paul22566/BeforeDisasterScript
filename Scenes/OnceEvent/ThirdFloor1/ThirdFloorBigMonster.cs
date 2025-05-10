using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdFloorBigMonster : MonoBehaviour
{
    public GameObject MonsterSpecialAnimation;
    public GameObject MonsterMoveAnimation;
    public GameObject HowlAnimation;
    private float Timer = 3.9f;
    private bool HasHowlAppear = false;
    // Start is called before the first frame update
    void Start()
    {
        if (GameEvent.GoIn3F1)
        {
            MonsterMoveAnimation.SetActive(true);
            MonsterSpecialAnimation.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameEvent.GoIn3F1)
        {
            Timer -= Time.deltaTime;
            if (Timer <= (3.9f - 2.45f))
            {
                if (!HasHowlAppear)
                {
                    HowlAnimation.SetActive(true);
                    HasHowlAppear = true;
                }
                if (Timer <= (3.9f - 3.1f))
                {
                    HowlAnimation.SetActive(false);
                    if (Timer <= 0)
                    {
                        MonsterMoveAnimation.SetActive(true);
                        MonsterSpecialAnimation.SetActive(false);
                    }
                }
            }
        }
    }
}
