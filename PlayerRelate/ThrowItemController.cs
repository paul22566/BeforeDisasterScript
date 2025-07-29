using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowItemController : MonoBehaviour
{
    public bool isWalkThrowItem;
    private Transform _transform;
    private Vector2 RPower = new Vector2(132f, 200);
    private Vector2 LPower = new Vector2(-132f, 200);
    private SpriteRenderer Spr;
    private Rigidbody2D Rigid2D;
    private Animator Animation;
    private GameObject ExplosionRange;
    private float PowerX;
    private float PowerY;
    private bool HasAtkAppear;
    private float Timer;
    private bool BeginingExplosion;
    public GameObject SE;
    private bool HasSEAppear;
    // Start is called before the first frame update
    void Awake()
    {
        _transform = this.transform;
        HasAtkAppear = false;
        Timer = 0.5f;

        Rigid2D = this.GetComponent<Rigidbody2D>();
        ExplosionRange = _transform.GetChild(0).gameObject;
        Spr = _transform.GetChild(1).GetComponent<SpriteRenderer>();
        Animation = _transform.GetChild(1).GetComponent<Animator>();
        /*if (isWalkThrowItem)
        {
            switch (PlayerController._player.face)
            {
                case Creature.Face.Left:
                    Spr.flipX = false;
                    PowerX = _battleSystem.ThrowItemPowerX;
                    PowerY = _battleSystem.ThrowItemPowerY;
                    RPower = new Vector2(PowerX, PowerY);
                    Rigid2D.AddForce(RPower, ForceMode2D.Impulse);
                    break;
                case Creature.Face.Right:
                    Spr.flipX = true;
                    PowerX = _battleSystem.ThrowItemPowerX;
                    PowerY = _battleSystem.ThrowItemPowerY;
                    LPower = new Vector2(-PowerX, PowerY);
                    Rigid2D.AddForce(LPower, ForceMode2D.Impulse);
                    break;
            }
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        TimerMethod();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<CollisionType>() != null && collision.GetComponent<CollisionType>().EntityCollision)
        {
            Explosion();
        }
        if (collision.gameObject.tag == "Player" && !isWalkThrowItem)
        {
            Explosion();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null)
        {
            Explosion();
        }
    }

    void TimerMethod()
    {
        if (BeginingExplosion)
        {
            Timer -= Time.deltaTime;
            if (!HasSEAppear)
            {
                Instantiate(SE, this.transform.position, Quaternion.identity);
                HasSEAppear = true;
            }
            if (Timer <= (0.5 - 0.15))
            {
                if (!HasAtkAppear)
                {
                    ExplosionRange.SetActive(true);
                    HasAtkAppear = true;
                }
                if (Timer <= 0)
                {
                    Destroy(this.gameObject);
                }
            }
        }
    }

    private void Explosion()
    {
        Animation.SetBool("Explosion", true);
        Rigid2D.velocity = new Vector2(0, 0);
        Rigid2D.gravityScale = 0;
        BeginingExplosion = true;
    }

    public void ObjectThrow((float, float) power)
    {
        PowerX = power.Item1;
        PowerY = power.Item2;
        
        if (PowerX < 0)
        {
            Spr.flipX = true;
        }
        else
        {
            Spr.flipX = false;
        }
        Rigid2D.AddForce(new Vector2(PowerX, PowerY), ForceMode2D.Impulse);
    }
}
