using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VBigMonsterSuperLight : MonoBehaviour
{
    private float Timer;
    public float TimerSet;
    private GameObject Judgement;
    private Animator Ani;
    void Start()
    {
        Timer = TimerSet;
        Ani = this.GetComponent<Animator>();
        Judgement = this.transform.GetChild(0).gameObject;
    }

    void Update()
    {
        Timer -= Time.deltaTime;
        if (Timer <= (TimerSet - 1.9))
        {
            Ani.SetBool("Disappear", true);
            Judgement.SetActive(false);
            if (Timer <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
