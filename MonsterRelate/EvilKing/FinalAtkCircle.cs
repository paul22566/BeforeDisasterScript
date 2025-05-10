using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalAtkCircle : MonoBehaviour
{
    private float Timer = 1.5f;
    private float TimerSet = 1.5f;
    private GameObject Object1;
    private GameObject Object2;
    private GameObject Object3;
    private GameObject Object4;
    private GameObject Object5;
    private GameObject Object6;
    private GameObject Object7;
    private GameObject Object8;
    private GameObject Object9;
    private GameObject Object10;
    private GameObject Object11;
    // Start is called before the first frame update
    void Start()
    {
        Object1 = this.transform.GetChild(0).gameObject;
        Object2 = this.transform.GetChild(1).gameObject;
        Object3 = this.transform.GetChild(2).gameObject;
        Object4 = this.transform.GetChild(3).gameObject;
        Object5 = this.transform.GetChild(4).gameObject;
        Object6 = this.transform.GetChild(5).gameObject;
        Object7 = this.transform.GetChild(6).gameObject;
        Object8 = this.transform.GetChild(7).gameObject;
        Object9 = this.transform.GetChild(8).gameObject;
        Object10 = this.transform.GetChild(9).gameObject;
        Object11 = this.transform.GetChild(10).gameObject;
    }

    private void Update()
    {
        Timer -= Time.deltaTime;
        if (Timer <= (TimerSet - 0.2))
        {
            Object1.SetActive(true);
        }
        if (Timer <= (TimerSet - 0.4))
        {
            Object2.SetActive(true);
        }
        if (Timer <= (TimerSet - 0.6))
        {
            Object3.SetActive(true);
        }
        if (Timer <= (TimerSet - 0.8))
        {
            Object4.SetActive(true);
        }
        if (Timer <= (TimerSet - 1))
        {
            Object5.SetActive(true);
        }
        if (Timer <= (TimerSet - 1.2))
        {
            Object6.SetActive(true);
        }
        if (Timer <= (TimerSet - 1.4))
        {
            Object7.SetActive(true);
        }
        if (Timer <= (TimerSet - 1.6))
        {
            Object8.SetActive(true);
        }
        if (Timer <= (TimerSet - 1.8))
        {
            Object9.SetActive(true);
        }
        if (Timer <= (TimerSet - 2))
        {
            Object10.SetActive(true);
        }
        if (Timer <= (TimerSet - 2.2))
        {
            Object11.SetActive(true);
        }
    }
}
