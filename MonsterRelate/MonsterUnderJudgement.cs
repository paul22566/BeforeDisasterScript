using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterUnderJudgement : MonoBehaviour
{
    private GameObject Monster;
    public float PushSpeed;
    [HideInInspector] public bool isPlayerUnder;//script(¦U©Ç)
    private bool PushToRight;
    private bool PushToLeft;

    private void Awake()
    {
        Monster = this.transform.parent.transform.parent.gameObject;
    }
    // Update is called once per frame
    void Update()
    {
        if (Monster == null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            this.transform.position = Monster.transform.position;
        }
    }

    private void FixedUpdate()
    {
        if (PushToRight && !PushToLeft)
        {
            Monster.transform.position += new Vector3(PushSpeed, 0, 0);
        }
        if (PushToLeft && !PushToRight)
        {
            Monster.transform.position += new Vector3(-PushSpeed, 0, 0);
        }
        if (PushToRight && PushToLeft)
        {
            Monster.transform.position += new Vector3(PushSpeed, 0, 0);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (this.transform.position.x >= collision.transform.position.x)
            {
                PushToRight = true;
                isPlayerUnder = true;
            }
            else
            {
                PushToLeft = true;
                isPlayerUnder = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "monster")
        {
            isPlayerUnder = false;
            PushToRight = false;
            PushToLeft = false;
        }
    }
}
