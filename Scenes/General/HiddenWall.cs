using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenWall : MonoBehaviour
{
    private bool inRange;
    public Animator thisAni;

    // Update is called once per frame
    void Update()
    {
        if (inRange)
        {
            thisAni.SetBool("Transparent", true);
        }
        else
        {
            thisAni.SetBool("Transparent", false);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            inRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            inRange = false;
        }
    }
}
