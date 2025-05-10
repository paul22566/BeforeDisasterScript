using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VBigMonsterAssembleLight : MonoBehaviour
{
    private float Timer;
    public float TimerSet;
    private float DisappearTime;
    private Animator Ani;
    // Start is called before the first frame update
    void Start()
    {
        Timer = TimerSet;
        DisappearTime = TimerSet - 0.2f;
        Ani = this.transform.GetChild(0).GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Timer -= Time.fixedDeltaTime;
        if (Timer <= (TimerSet - DisappearTime))
        {
            Ani.SetBool("End", true);
            if (Timer <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
