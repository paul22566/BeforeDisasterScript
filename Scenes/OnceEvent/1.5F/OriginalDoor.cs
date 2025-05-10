using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OriginalDoor : MonoBehaviour
{
    public Animator OriginalDoorAnimation;
    private float Timer = 1.05f;
    private bool timerSwitch;
    private bool isDoEvent;
    // Start is called before the first frame update
    void Start()
    {
        if (GameEvent.OpenOriginalDoor)
        {
            Destroy(this.gameObject);
        }
        else
        {
            isDoEvent = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isDoEvent)
        {
            if (GameEvent.OpenOriginalDoor)
            {
                timerSwitch = true;
            }
            TimerMethod();
        }
    }

    void TimerMethod()
    {
        if (timerSwitch)
        {
            Timer -= Time.deltaTime;
            OriginalDoorAnimation.SetBool("Open", true);
            if (Timer <= 0)
            {
                GameEvent.OpenOriginalDoor = true;
                isDoEvent = false;
                timerSwitch = false;
                Destroy(this.gameObject);
            }
        }
    }
}
