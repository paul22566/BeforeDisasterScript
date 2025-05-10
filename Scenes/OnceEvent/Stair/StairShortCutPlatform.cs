using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairShortCutPlatform : MonoBehaviour
{
    public Animator Ani;
    private float Timer;
    public float TimerSet;
    private bool isDoevent;

    public GameObject ShortCut;
    // Start is called before the first frame update
    void Start()
    {
        if (!GameEvent.StairShortCutUnlock)
        {
            Timer = TimerSet;
            isDoevent = true;
        }
        else
        {
            ShortCut.SetActive(true);
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameEvent.StairShortCutUnlock && isDoevent)
        {
            Ani.SetBool("Appear", true);
            Timer -= Time.deltaTime;
            if (Timer <= 0)
            {
                isDoevent = false;
                ShortCut.SetActive(true);
                Destroy(this.gameObject);
            }
        }
    }
}
