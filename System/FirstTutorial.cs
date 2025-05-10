using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstTutorial : MonoBehaviour
{
    public GameObject Tutorial;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Tutorial.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Tutorial.SetActive(false);
        }
    }
}
