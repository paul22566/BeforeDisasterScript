using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonMonsterController : MonoBehaviour
{
    //可放3個怪，超過放新物件，一樣的話，填入第一個就行
    [SerializeField] private GameObject Monster1;
    [SerializeField] private GameObject Monster2;
    [SerializeField] private GameObject Monster3;
    //如果地點一樣，填入第一個就行
    [SerializeField] private Transform AppearPlace1;
    [SerializeField] private Transform AppearPlace2;
    [SerializeField] private Transform AppearPlace3;
    private Vector3 Place1;
    private Vector3 Place2;
    private Vector3 Place3;

    [HideInInspector] public bool SummonComplete;
    public float SummonTimerSet;//怪物本身召喚
    private float SummonTimer;
    public float AniTimerSet;//召喚動畫全程
    private float AniTimer;
    private float CoolDownTimeSet = 0.1f;
    private float CooldownTime;
    [HideInInspector] public bool isAniPlay;
    private int HasSummonNumber = 0;
    private int ShouldSummonNumber = 0;
    private bool isSummon;
    // Start is called before the first frame update
    void Start()
    {
        CooldownTime = CoolDownTimeSet;
        AniTimer = AniTimerSet;
        SummonTimer = SummonTimerSet;
        if (Monster2 == null)
        {
            Monster2 = Monster1;
        }
        if (Monster3 == null)
        {
            Monster3 = Monster1;
        }
        if (AppearPlace2 == null)
        {
            AppearPlace2 = AppearPlace1;
        }
        if (AppearPlace3 == null)
        {
            AppearPlace3 = AppearPlace1;
        }

        //指定出現地
        if (AppearPlace1 == null)
        {
            Place1 = new Vector3 (0, 0, 0);
        }
        if (AppearPlace2 == null)
        {
            Place2 = new Vector3(0, 0, 0);
        }
        if (AppearPlace3 == null)
        {
            Place3 = new Vector3(0, 0, 0);
        }

        if (AppearPlace1 != null)
        {
            Place1 = AppearPlace1.position;
        }
        if (AppearPlace2 != null)
        {
            Place2 = AppearPlace1.position;
        }
        if (AppearPlace3 != null)
        {
            Place3 = AppearPlace1.position;
        }
    }

    private void FixedUpdate()
    {
        if (!isSummon && !isAniPlay)
        {
            if (HasSummonNumber != ShouldSummonNumber)
            {
                isSummon = true;
                isAniPlay = true;
            }
        }
        if (isSummon)
        {
            SummonTimer -= Time.fixedDeltaTime;
            if (SummonTimer <= 0)
            {
                switch (HasSummonNumber)
                {
                    case 0:
                        Instantiate(Monster1, Place1, Quaternion.identity, this.transform);
                        break;
                    case 1:
                        Instantiate(Monster2, Place2, Quaternion.identity, this.transform);
                        break;
                    case 2:
                        Instantiate(Monster3, Place3, Quaternion.identity, this.transform);
                        break;
                }
                isSummon = false;
                SummonTimer = SummonTimerSet;
            }
        }
        if (isAniPlay)
        {
            AniTimer -= Time.fixedDeltaTime;
            if (AniTimer<= 0)
            {
                HasSummonNumber += 1;
                AniTimer = AniTimerSet;
                isAniPlay = false;
            }
        }
    }

    public void Summon()
    {
        ShouldSummonNumber += 1;
        if (!isSummon && !isAniPlay)
        {
            isSummon = true;
            isAniPlay = true;
        }
    }

    public void SpecialAssignAppearPlace(int Number, Vector3 Place)
    {
        switch (Number) 
        {
            case 1:
                Place1 = Place;
                break;
            case 2:
                Place2 = Place;
                break;
            case 3:
                Place3 = Place;
                break;
        }
    }

}
