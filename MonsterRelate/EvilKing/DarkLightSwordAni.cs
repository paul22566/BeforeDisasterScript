using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkLightSwordAni : MonoBehaviour
{
    private float Timer;
    public float TimerSet;
    void Start()
    {
        Timer = TimerSet;
    }

    void Update()
    {
        Timer -= Time.deltaTime;
        if (Timer <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
