using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OriginalController : MonoBehaviour
{
    private Transform PlayerTransform;
    private PlayerSpecialAni _specialAni;
    [HideInInspector] public bool isDoEvent;
    [HideInInspector] public int AniPhase = 0;//1 教學結束， 2 大廳結束
    public GameObject NormalFadeIn;
    public GameObject WhiteFadeIn;

    private float Ani1TimerSet = 20.75f;
    private float Ani2TimerSet = 9f;
    private float AniTimer;

    private void Awake()
    {
        if (GameEvent.isAniPlay == true)
        {
            isDoEvent = true;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("player") != null)
        {
            PlayerTransform = GameObject.Find("player").transform;
            _specialAni = GameObject.Find("player").GetComponent<PlayerSpecialAni>();
        }
        else
        {
            return;
        }

        if (!isDoEvent)
        {
            return;
        }

        if (isDoEvent)
        {
            if (!GameEvent.PassHall)
            {
                AniPhase = 1;
            }
            else
            {
                AniPhase = 2;
            }
        }

        switch(AniPhase)
        {
            case 1:
                GameObject g1 = GameObject.Find("0") as GameObject;
                if (g1 != null)
                {
                    PlayerTransform.position = g1.transform.position;
                }

                NormalFadeIn.SetActive(false);
                WhiteFadeIn.SetActive(true);

                AniTimer = Ani1TimerSet;
                break;
            case 2:
                GameObject g2 = GameObject.Find("1") as GameObject;
                if (g2 != null)
                {
                    PlayerTransform.position = g2.transform.position;
                }

                AniTimer = Ani2TimerSet;
                break;
        }
    }

    private void FixedUpdate()
    {
        if (isDoEvent)
        {
            AniTimer -= Time.fixedDeltaTime;

            if (AniTimer <= 0)
            {
                GameEvent.isAniPlay = false;
                _specialAni.ShowPlayerUI();
                isDoEvent = false;
            }
        }
    }
}
