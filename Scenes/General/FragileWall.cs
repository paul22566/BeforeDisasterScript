using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragileWall : MonoBehaviour
{
    public Animator Ani;
    private bool isFalling;
    [HideInInspector] public bool isOpen;
    private float Timer;
    public float TimerSet;
    private BoxCollider2D _boxCollider;

    public GameObject WallFallSound;

    // Update is called once per frame
    void Update()
    {
        if (isFalling)
        {
            Ani.SetBool("Fall", true);
            Timer -= Time.deltaTime;
            if (Timer <= (TimerSet - 0.9))
            {
                _boxCollider.isTrigger = true;
            }
            if (Timer <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "ExplosionBottle")
        {
            Instantiate(WallFallSound);
            isFalling = true;
            isOpen = true;
        }
    }

    public void InisializeFragileWall()
    {
        _boxCollider = this.GetComponent<BoxCollider2D>();
        Timer = TimerSet;
    }
}
