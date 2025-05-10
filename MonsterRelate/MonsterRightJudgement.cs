using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterRightJudgement : MonoBehaviour
{
    private GameObject Monster;
    [HideInInspector] public bool isPlayerAtRightSide;//script(¦U©Çª«)

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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "PlayerRange")
        {
            if (collision.transform.position.x >= this.transform.position.x)
            {
                isPlayerAtRightSide = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "PlayerRange")
        {
            isPlayerAtRightSide = false;
        }
    }
}
