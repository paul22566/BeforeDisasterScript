using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDieAni : MonoBehaviour
{
    public float DieTimerSet;
    private float DieTimer;
    private Rigidbody2D Rigid2D;

    [Header("Boss±M¥Î")]
    public bool isBoss;
    public float LowestLine;
    // Start is called before the first frame update
    void Start()
    {
        DieTimer = DieTimerSet;
        Rigid2D = this.GetComponent<Rigidbody2D>();
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isBoss && transform.position.y < LowestLine)
        {
            transform.position = new Vector3(transform.position.x, LowestLine, 0);
        }
        DieTimer -= Time.deltaTime;
        if (DieTimer <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "platform")
        {
            Rigid2D.gravityScale = 0;
            Rigid2D.velocity = new Vector2(0, 0);
        }
    }
}
