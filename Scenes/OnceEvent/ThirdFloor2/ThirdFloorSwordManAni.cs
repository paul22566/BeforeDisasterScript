using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdFloorSwordManAni : MonoBehaviour
{
    public Animator ThisAni;
    public GameObject GhostAnimation;
    public GameObject GhostMan;
    private float Timer = 7.45f;
    private bool HasGhostBegin = false;
    // Start is called before the first frame update
    void Start()
    {
        if (GameEvent.HasGoThirdFloor2)
        {
            if (!GameEvent.HasPassThirdFloor2)
            {
                ThisAni.SetBool("Pass", true);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameEvent.HasGoThirdFloor2)
        {
            Timer -= Time.deltaTime;
            if (Timer <= (7.45 - 2))
            {
                ThisAni.SetBool("Begin", true);
                if (Timer <= (7.45 - 2.5))
                {
                    if (!HasGhostBegin)
                    {
                        GhostAnimation.SetActive(true);
                        HasGhostBegin = true;
                    }
                    if (Timer <= (7.45 - 5))
                    {
                        ThisAni.SetBool("GetUp",true);
                    }
                }
                if (Timer <= 0)
                {
                    Instantiate(GhostMan, this.transform.position, Quaternion.identity);
                    Destroy(this.gameObject);
                }
            }
        }
        else
        {
            Timer -= Time.deltaTime;
            if (Timer <= (6.5 - 1))
            {
                Instantiate(GhostMan, this.transform.position, Quaternion.identity);
                Destroy(this.gameObject);
            }
        }
    }
}
