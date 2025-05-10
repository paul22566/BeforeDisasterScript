using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkLightSword : MonoBehaviour
{
    private float Timer;
    private float TimerSet = 1;
    public GameObject SwordRange;
    private bool AtkAppear;
    public int Number;//는쩵 쨁1 는ⅹ 쨁2 는쩵 짾3 는ⅹ 짾4 
    private Vector3 Number1Appear = new Vector3(3.65f, 1.57f, 0);
    private Vector3 Number2Appear = new Vector3(-3.144f, 1.07f, 0);
    private Vector3 Number3Appear = new Vector3(4.844f, -0.198f, 0);
    private Vector3 Number4Appear = new Vector3(-4.489f, -1.468f, 0);
    void Start()
    {
        Timer = TimerSet;
    }

    void Update()
    {
        Timer -= Time.deltaTime;
        if (Timer <= (TimerSet-0.55))
        {
            if (!AtkAppear)
            {
                switch (Number)
                {
                    case 1:
                        Instantiate(SwordRange, this.transform.position + Number1Appear, Quaternion.identity);
                        break;
                    case 2:
                        Instantiate(SwordRange, this.transform.position + Number2Appear, Quaternion.identity);
                        break;
                    case 3:
                        Instantiate(SwordRange, this.transform.position + Number3Appear, Quaternion.identity);
                        break;
                    case 4:
                        Instantiate(SwordRange, this.transform.position + Number4Appear, Quaternion.identity);
                        break;
                }
                AtkAppear = true;
            }
            if (Timer <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
