using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atk5SummonDoor : MonoBehaviour
{
    private float Timer;
    public float TimerSet;
    private Vector3 Appear = new Vector3(-5.6f,1.14f,0);
    private Vector3 SwordAppear = new Vector3(-12.6f, 3.32f, 0);
    public GameObject SmallDarkBall;
    public GameObject SmallDarkLightSword;
    private bool AtkFirstAppear;
    private bool AtkSecondAppear;
    private bool AtkThirdAppear;
    private bool AtkFourAppear;
    private bool AtkFifthAppear;
    private int Situation;
    // Start is called before the first frame update
    void Start()
    {
        Timer = TimerSet;
        switch (Random.Range(0, 2))
        {
            case 0:
                Situation = 1;
                break;
            case 1:
                Situation = 2;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Timer -= Time.deltaTime;
        switch (Situation)
        {
            case 1:
                if (Timer <= (TimerSet - 1))
                {
                    if (!AtkFirstAppear)
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            Instantiate(SmallDarkBall, this.transform.position + Appear + new Vector3(Random.Range(0,4) + (i*4),0,0), Quaternion.identity);
                        }
                        AtkFirstAppear = true;
                    }
                    if (Timer <= (TimerSet - 2))
                    {
                        if (!AtkSecondAppear)
                        {
                            for (int i = 0; i < 8; i++)
                            {
                                Instantiate(SmallDarkBall, this.transform.position + Appear + new Vector3(Random.Range(0, 4) + (i * 4), 0, 0), Quaternion.identity);
                            }
                            AtkSecondAppear = true;
                        }
                        if (Timer <= (TimerSet - 3))
                        {
                            if (!AtkThirdAppear)
                            {
                                for (int i = 0; i < 8; i++)
                                {
                                    Instantiate(SmallDarkBall, this.transform.position + Appear + new Vector3(Random.Range(0, 4) + (i * 4), 0, 0), Quaternion.identity);
                                }
                                AtkThirdAppear = true;
                            }
                            if (Timer <= (TimerSet - 4))
                            {
                                if (!AtkFourAppear)
                                {
                                    for (int i = 0; i < 8; i++)
                                    {
                                        Instantiate(SmallDarkBall, this.transform.position + Appear + new Vector3(Random.Range(0, 4) + (i * 4), 0, 0), Quaternion.identity);
                                    }
                                    AtkFourAppear = true;
                                }
                                if (Timer <= (TimerSet - 5))
                                {
                                    if (!AtkFifthAppear)
                                    {
                                        for (int i = 0; i < 8; i++)
                                        {
                                            Instantiate(SmallDarkBall, this.transform.position + Appear + new Vector3(Random.Range(0, 4) + (i * 4), 0, 0), Quaternion.identity);
                                        }
                                        AtkFifthAppear = true;
                                    }
                                    if (Timer <= (TimerSet - 6))
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
                    }
                }
                break;
            case 2:
                if (Timer <= (TimerSet - 1))
                {
                    if (!AtkFirstAppear)
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            Instantiate(SmallDarkLightSword, this.transform.position + SwordAppear + new Vector3(Random.Range(0, 4) + (i * 4), 0, 0), Quaternion.identity);
                        }
                        AtkFirstAppear = true;
                    }
                    if (Timer <= (TimerSet - 2))
                    {
                        if (!AtkSecondAppear)
                        {
                            for (int i = 0; i < 8; i++)
                            {
                                Instantiate(SmallDarkLightSword, this.transform.position + SwordAppear + new Vector3(Random.Range(0, 4) + (i * 4), 0, 0), Quaternion.identity);
                            }
                            AtkSecondAppear = true;
                        }
                        if (Timer <= (TimerSet - 3))
                        {
                            if (!AtkThirdAppear)
                            {
                                for (int i = 0; i < 8; i++)
                                {
                                    Instantiate(SmallDarkLightSword, this.transform.position + SwordAppear + new Vector3(Random.Range(0, 4) + (i * 4), 0, 0), Quaternion.identity);
                                }
                                AtkThirdAppear = true;
                            }
                            if (Timer <= (TimerSet - 4))
                            {
                                if (!AtkFourAppear)
                                {
                                    for (int i = 0; i < 8; i++)
                                    {
                                        Instantiate(SmallDarkLightSword, this.transform.position + SwordAppear + new Vector3(Random.Range(0, 4) + (i * 4), 0, 0), Quaternion.identity);
                                    }
                                    AtkFourAppear = true;
                                }
                                if (Timer <= (TimerSet - 5))
                                {
                                    if (!AtkFifthAppear)
                                    {
                                        for (int i = 0; i < 8; i++)
                                        {
                                            Instantiate(SmallDarkLightSword, this.transform.position + SwordAppear + new Vector3(Random.Range(0, 4) + (i * 4), 0, 0), Quaternion.identity);
                                        }
                                        AtkFifthAppear = true;
                                    }
                                    if (Timer <= (TimerSet - 6))
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
                    }
                }
                break;
        }
    }
}
