using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VBMonsterDieAni : MonoBehaviour
{
    [SerializeField] private Animator Eye;
    [SerializeField] private Animator Head;

    private float Timer = 5.5f;
    private bool Bool1;
    private bool Bool2;
    private void FixedUpdate()
    {
        Timer -= Time.fixedDeltaTime;
        if (Timer <= (5.5 - 0.85))
        {
            if (!Bool1)
            {
                Head.SetBool("Open", true);
                Bool1 = true;
            }
        }
        if (Timer <= (5.5 - 3.25))
        {
            if(!Bool2)
            {
                Eye.SetBool("Close", true);
                Bool2 = true;
            }
        }
    }
}
