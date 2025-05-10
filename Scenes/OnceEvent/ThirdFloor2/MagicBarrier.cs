using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBarrier : MonoBehaviour
{
    public Animator DoorAnimation;
    private bool isDoEvent = false;
    private int EventNumber;
    public float DoorTimerSet;
    private float DoorTimer;
    private bool timerSwitch;
    private BoxCollider2D thisBox;
    private float DisappearTimer;
    // Start is called before the first frame update
    void Start()
    {
        DoorTimer = DoorTimerSet;
        DisappearTimer = 3f;
        thisBox = this.gameObject.GetComponent<BoxCollider2D>();
        if (!GameEvent.HasPassThirdFloor2)
        {
            isDoEvent = true;
            if (GameEvent.HasGoThirdFloor2)
            {
                EventNumber = 2;
            }
            else
            {
                EventNumber = 1;
                thisBox.isTrigger = true;
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isDoEvent)
        {
            Timer();
            if (EventNumber == 2)
            {
                timerSwitch = true;
            }
            if (EventNumber == 1)
            {
                timerSwitch = true;
            }
            if (EventNumber == 3)
            {
                if (GameEvent.HasPassThirdFloor2)
                {
                    timerSwitch = true;
                }
            }
        }
    }

    void Timer()
    {
        if (timerSwitch)
        {
            switch (EventNumber)
            {
                case 1:
                    DoorTimer -= Time.deltaTime;
                    if (DoorTimer <= (DoorTimerSet - 3))
                    {
                        thisBox.isTrigger = false;
                        DoorAnimation.SetBool("Open", true);
                        if (DoorTimer <= 0)
                        {
                            EventNumber = 3;
                            timerSwitch = false;
                        }
                    }
                    break;
                case 2:
                    DoorTimer -= Time.deltaTime;
                    if (DoorTimer <= (DoorTimerSet - 0.5))
                    {
                        DoorAnimation.SetBool("Open", true);
                        EventNumber = 3;
                        timerSwitch = false;
                    }
                    break;
                case 3:
                    DisappearTimer -= Time.deltaTime;
                    if(DisappearTimer <= 0)
                    {
                        DoorTimer -= Time.deltaTime;
                        DoorAnimation.SetBool("Open", false);
                        if (DoorTimer <= (DoorTimerSet - 1))
                        {
                            Destroy(this.gameObject);
                        }
                    }
                    break;
            }
        }
        else
        {
            DoorTimer = DoorTimerSet;
        }
    }
}
