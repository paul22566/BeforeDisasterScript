using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilKingDarkLightVertical : MonoBehaviour
{
    public bool isForever;
    private Animator ThisAni;
    private GameObject DarkLightAni;
    private GameObject DarkLightJudgement;
    private float Timer;
    public float TimerSet;
    // Start is called before the first frame update
    void Start()
    {
        Timer = TimerSet;
        ThisAni = this.GetComponent<Animator>();
        DarkLightAni = this.transform.GetChild(0).gameObject;
        DarkLightJudgement = this.transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Timer -= Time.deltaTime;
        if (Timer <= (TimerSet - 0.7))
        {
            DarkLightAni.SetActive(true);
            if (Timer <= (TimerSet - 0.72))
            {
                DarkLightJudgement.SetActive(true);
                if (Timer <= (TimerSet - 0.95))
                {
                    if (!isForever)
                    {
                        DarkLightJudgement.SetActive(false);
                        ThisAni.SetBool("Disappear", true);
                        if (Timer <= 0)
                        {
                            Destroy(this.gameObject);
                        }
                    }
                }
            }
        }
    }
}
