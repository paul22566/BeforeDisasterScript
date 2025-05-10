using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VBMSwitchPhaseAni : MonoBehaviour
{
    private float Timer;
    private float TimerSet = 1.65f;
    public GameObject Head;
    public GameObject Body;
    public GameObject Arm;
    // Start is called before the first frame update
    void Start()
    {
        Timer = TimerSet;
    }

    private void FixedUpdate()
    {
        Timer -= Time.fixedDeltaTime;
        if(Timer<=(TimerSet - 0.45))
        {
            Body.SetActive(true);
        }
        if (Timer <= (TimerSet - 1.15))
        {
            Head.SetActive(true);
            Arm.SetActive(true);
        }
        if (Timer <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }
}
