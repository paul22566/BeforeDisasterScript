using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atk6SummpnDoor : MonoBehaviour
{
    private float Timer;
    public float TimerSet;
    private Vector3 Appear = new Vector3(-0.11f, -3f, 0);
    private bool AtkFirstAppear;
    public GameObject BigDarkBall;
    // Start is called before the first frame update
    void Start()
    {
        Timer = TimerSet;
    }

    // Update is called once per frame
    void Update()
    {
        Timer -= Time.deltaTime;
        if (Timer <= (TimerSet - 1))
        {
            if (!AtkFirstAppear)
            {
                Instantiate(BigDarkBall, this.transform.position + Appear, Quaternion.identity);
                AtkFirstAppear = true;
            }
            if (Timer <= (TimerSet - 3))
            {
                this.GetComponent<Animator>().SetBool("Disappear", true);
                if (Timer <= 0)
                {
                    Destroy(this.gameObject);
                }
            }
        }
    }
}
