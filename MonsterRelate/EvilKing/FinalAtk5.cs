using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalAtk5 : MonoBehaviour
{
    private float Timer = 2.6f;
    private float TimerSet = 2.6f;
    private GameObject LightAni;
    private GameObject Judgement;
    private GameObject Object1;
    private GameObject Object2;
    private GameObject Object3;
    private GameObject Object4;
    private GameObject Object5;
    private GameObject Object6;
    private GameObject Object7;
    // Start is called before the first frame update
    void Start()
    {
        LightAni = this.transform.GetChild(0).gameObject;
        Judgement = this.transform.GetChild(1).gameObject;
        Object1 = this.transform.GetChild(2).gameObject;
        Object2 = this.transform.GetChild(3).gameObject;
        Object3 = this.transform.GetChild(4).gameObject;
        Object4 = this.transform.GetChild(5).gameObject;
        Object5 = this.transform.GetChild(6).gameObject;
        Object6 = this.transform.GetChild(7).gameObject;
        Object7 = this.transform.GetChild(8).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Timer -= Time.deltaTime;
        if (Timer <= (TimerSet - 0.2))
        {
            if (Object1 != null)
            {
                Object1.SetActive(true);
            }
        }
        if (Timer <= (TimerSet - 0.4))
        {
            if (Object2 != null)
            {
                Object2.SetActive(true);
            }
        }
        if (Timer <= (TimerSet - 0.7))
        {
            LightAni.SetActive(true);
            if (Timer <= (TimerSet - 0.72))
            {
                Judgement.SetActive(true);
            }
        }
        if (Timer <= (TimerSet - 0.6))
        {
            if (Object3 != null)
            {
                Object3.SetActive(true);
            }
        }
        if (Timer <= (TimerSet - 0.95))
        {
            Judgement.SetActive(false);
            this.GetComponent<Animator>().SetBool("DiSappgar", true);
        }
        if (Timer <= (TimerSet - 0.8))
        {
            if (Object4 != null)
            {
                Object4.SetActive(true);
            }
        }
        if (Timer <= (TimerSet - 1))
        {
            if (Object5 != null)
            {
                Object5.SetActive(true);
            }
        }
        if (Timer <= (TimerSet - 1.2))
        {
            if (Object6 != null)
            {
                Object6.SetActive(true);
            }
        }
        if (Timer <= (TimerSet - 1.4))
        {
            if (Object7 != null)
            {
                Object7.SetActive(true);
            }
        }
        if (Timer <= TimerSet - 2.6)
        {
            Destroy(this.gameObject);
        }
    }
}
