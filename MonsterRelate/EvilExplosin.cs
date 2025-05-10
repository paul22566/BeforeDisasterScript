using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilExplosin : MonoBehaviour
{
    private GameObject Judgement;
    public float TimerSet;
    private float Timer;
    public float ExplosinAppearTime;
    private bool AtkAppear;
    // Start is called before the first frame update
    void Start()
    {
        Timer = TimerSet;
        Judgement = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Timer -= Time.deltaTime;
        if(Timer <= (TimerSet - ExplosinAppearTime))
        {
            if (!AtkAppear)
            {
                Judgement.SetActive(true);
                AtkAppear = true;
            }
            if (Timer <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
