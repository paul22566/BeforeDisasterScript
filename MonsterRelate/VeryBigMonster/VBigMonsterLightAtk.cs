using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VBigMonsterLightAtk : MonoBehaviour
{
    private float Timer;
    public float ShortTimerSet;
    public float LongTimerSet;
    private GameObject Judgement;
    public int CaseNumber;//1���u�A2����
    [HideInInspector] public float LongVerLong;//���L���ޯ��}�l�ɼL������������
    private float LongVerNowScale = 0;
    private Animator Ani;
    private bool AtkFirstAppear;
    private Transform _transform;
    [HideInInspector] public bool LongVerBegin;//script(VBMonsterAtk3Head)
    [HideInInspector] public bool LongVerReSet;//script(VBMonsterAtk3Head)
    void Start()
    {
        Ani = this.GetComponent<Animator>();
        Judgement = this.transform.GetChild(0).gameObject;
        _transform = transform;
        switch (CaseNumber)
        {
            case 1:
                Timer = ShortTimerSet;
                break;
            case 2:
                Timer = LongTimerSet;
                break;
        }
    }

    void Update()
    {
        if(CaseNumber == 2)
        {
            LongVerInitialize();
        }
        Timer -= Time.deltaTime;
        switch (CaseNumber)
        {
            case 1:
                if (Timer <= (ShortTimerSet - 0.4))
                {
                    if (!AtkFirstAppear)
                    {
                        Judgement.SetActive(true);
                        AtkFirstAppear = true;
                    }
                }
                if (Timer <= (ShortTimerSet - 0.9))
                {
                    Judgement.SetActive(false);
                }
                if (Timer <= 0)
                {
                    Destroy(this.gameObject);
                }
                break;
            case 2:
                if (Timer <= (LongTimerSet - 0.4))
                {
                    if (!AtkFirstAppear)
                    {
                        Judgement.SetActive(true);
                        LongVerBegin = true;
                        AtkFirstAppear = true;
                    }
                }
                if (Timer <= (LongTimerSet - 4.6))
                {
                    Ani.SetBool("Disappear", true);
                    Judgement.SetActive(false);
                    if (Timer <= 0)
                    {
                        LongVerReSet = false;
                        this.gameObject.SetActive(false);
                    }
                }
                break;
        }
    }

    private void FixedUpdate()
    {
        //���L����g�X�ɪ��ܤƹL�{
        if (CaseNumber == 2 && !LongVerBegin)
        {
            _transform.localScale = new Vector3(LongVerNowScale, _transform.localScale.y, 0);
            if(LongVerNowScale < LongVerLong)
            {
                LongVerNowScale += (LongVerLong / 20);
            }
            else
            {
                LongVerBegin = true;
            }
        }
    }

    private void LongVerInitialize()
    {
        if (!LongVerReSet)
        {
            Timer = LongTimerSet;
            LongVerBegin = false;
            LongVerNowScale = 0;
            _transform.localScale = new Vector3(LongVerNowScale, _transform.localScale.y, 0);
            Ani.SetBool("Disappear", false);
            AtkFirstAppear = false;
            LongVerReSet = true;
        }
    }
}
