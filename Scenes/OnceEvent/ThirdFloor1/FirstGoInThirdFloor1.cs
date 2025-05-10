using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstGoInThirdFloor1 : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !GameEvent.GoIn3F1 && !GameEvent.isAniPlay)
        {
            ThirdFloor1Controller.canDoEvent = true;
        }
    }
}
