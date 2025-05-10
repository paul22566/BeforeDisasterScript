using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2RoomDoor : MonoBehaviour
{
    public Animator DoorAnimation;
    private bool isDoEvent = false;
    private int EventNumber;
    public float DoorTimerSet;
    private float DoorTimer;
    private bool timerSwitch;
    private BoxCollider2D thisBox;
    // Start is called before the first frame update
    void Start()
    {
        DoorTimer = DoorTimerSet;
        thisBox = this.gameObject.GetComponent<BoxCollider2D>();
        if (!GameEvent.PassBoss2)
        {
            isDoEvent = true;
            if (GameEvent.GoInBoss2)
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
                if (GameEvent.PassBoss2)
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
                    if (DoorTimer <= (DoorTimerSet - 1))
                    {
                        thisBox.isTrigger = false;
                        DoorAnimation.SetBool("Close", true);
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
                        DoorAnimation.SetBool("Close", true);
                        EventNumber = 3;
                        timerSwitch = false;
                    }
                    break;
                case 3:
                    DoorTimer -= Time.deltaTime;
                    DoorAnimation.SetBool("Close", false);
                    if (DoorTimer <= (DoorTimerSet - 1))
                    {
                        Destroy(this.gameObject);
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
