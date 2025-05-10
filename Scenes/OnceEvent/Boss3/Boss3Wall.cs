using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3Wall : MonoBehaviour
{
    private float Timer = 0.5f;

    void Update()
    {
        if (Boss3Controller.EvilKingEnd)
        {
            this.GetComponent<Animator>().SetBool("Disappear", true);
            Timer -= Time.deltaTime;
            if (Timer <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
