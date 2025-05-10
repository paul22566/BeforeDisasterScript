using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightManAni : MonoBehaviour
{
    public Animator StandUpAni;
    public GameObject MoveAnimation;
    public GameObject StandUpAnimation;
    private float Timer = 4.53f;
    // Start is called before the first frame update
    void Start()
    {
        if (GameEvent.GoRestRoom)
        {
            if (!GameEvent.PassRestRoom)
            {
                StandUpAnimation.SetActive(false);
                MoveAnimation.SetActive(true);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameEvent.GoRestRoom)
        {
            Timer -= Time.deltaTime;
            if (Timer <= (4.53 - 2))
            {
                StandUpAni.SetBool("StandUp", true);
                if (Timer <= 0)
                {
                    StandUpAnimation.SetActive(false);
                    MoveAnimation.SetActive(true);
                }
            }
        }
    }
}
