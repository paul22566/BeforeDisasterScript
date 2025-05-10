using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerRoomOpen : MonoBehaviour
{
    public Animator Animation;
    private double DestroyTimer;
    public double DestroyTimerSet;
    private bool timerSwitch;
    // Start is called before the first frame update
    void Start()
    {
        if (GameEvent.ControllerRoomUnlock)
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameEvent.ControllerRoomUnlock)
        {
            Animation.SetBool("Open", true);
            timerSwitch = true;
        }

        timer();
    }

    void timer()
    {
        if (timerSwitch)
        {
            DestroyTimer -= Time.deltaTime;
            if(DestroyTimer <= 0)
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            DestroyTimer = DestroyTimerSet;
        }
    }
}
