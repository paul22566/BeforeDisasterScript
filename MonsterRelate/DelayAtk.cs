using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayAtk : MonoBehaviour
{
    private GameObject Judgement;
    private float Timer;
    public float TimerSet;
    public float JudgementAppearTime;
    private bool AtkFirstAppear;
    // Start is called before the first frame update
    void Start()
    {
        Judgement = transform.GetChild(0).gameObject;
        Timer = TimerSet;
    }

    // Update is called once per frame
    void Update()
    {
        Timer -= Time.deltaTime;
        if(Timer<=(TimerSet - JudgementAppearTime))
        {
            if (!AtkFirstAppear)
            {
                Judgement.SetActive(true);
                AtkFirstAppear = true;
            }
            if (Timer <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
