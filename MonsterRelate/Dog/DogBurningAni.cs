using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogBurningAni : MonoBehaviour
{
    private Transform _transform;
    private float _deltaTime;
    private float RunTimer;
    public float RunTimerSet;
    private float DieTimer;
    public float DieTimerSet;
    public float RunSpeed;
    private SpriteRenderer DogSpr;
    private Animator DogAni;
    public Transform Fire1;
    public Transform Fire2;
    public Transform Fire3;
    public Transform Fire4;
    public Transform Fire5;
    private Animator Fire1Ani;
    private Animator Fire2Ani;
    private Animator Fire3Ani;
    private Animator Fire4Ani;
    private Animator Fire5Ani;
    private int TurnFaceTime = 3;

    private RaycastHit2D GroundCheck;
    private Rigidbody2D Rigid2D;

    private bool TouchRightWall;
    private bool TouchLeftWall;

    private enum Face { Left, Right }
    private Face face;
    // Start is called before the first frame update
    void Start()
    {
        _transform = transform;
        DogAni = this.GetComponent<Animator>();
        DogSpr = this.GetComponent<SpriteRenderer>();
        Rigid2D = this.GetComponent<Rigidbody2D>();
        RunTimer = RunTimerSet;
        DieTimer = DieTimerSet;
        if (DogSpr.flipX == false)
        {
            face = Face.Right;
        }
        else
        {
            face = Face.Left;
        }
        Fire1Ani = Fire1.GetComponent<Animator>();
        Fire2Ani = Fire2.GetComponent<Animator>();
        Fire3Ani = Fire3.GetComponent<Animator>();
        Fire4Ani = Fire4.GetComponent<Animator>();
        Fire5Ani = Fire5.GetComponent<Animator>();

        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        _deltaTime = Time.deltaTime;

        GroundCheck = Physics2D.Raycast(transform.position, -Vector2.up, 1.1f, 1024);
        if (GroundCheck)
        {
            Rigid2D.gravityScale = 0;
            Rigid2D.velocity = new Vector2(Rigid2D.velocity.x, 0);
            if (_transform.position.y <= (GroundCheck.point.y + 0.7))
            {
                _transform.position = new Vector3(_transform.position.x, GroundCheck.point.y + 1.01f, 0);
            }
        }
        else
        {
            Rigid2D.gravityScale = 7;
        }

        if (TurnFaceTime > 0)
        {
            RunTimer -= _deltaTime;
            switch (face)
            {
                case Face.Left:
                    if (!TouchLeftWall)
                        _transform.position += new Vector3(-RunSpeed * _deltaTime, 0, 0);
                    break;
                case Face.Right:
                    if (!TouchRightWall)
                        _transform.position += new Vector3(RunSpeed * _deltaTime, 0, 0);
                    break;
            }
            if (RunTimer <= 0)
            {
                RunTimer = RunTimerSet;
                TurnFaceTime -= 1;
                if (TurnFaceTime > 0)
                {
                    TurnFace();
                }
            }
        }

        if (TurnFaceTime <= 0)
        {
            DieTimer -= _deltaTime;
            DogAni.SetBool("Die", true);

            Fire1Ani.SetBool("Die", true);
            Fire2Ani.SetBool("Die", true);
            Fire3Ani.SetBool("Die", true);
            Fire4Ani.SetBool("Die", true);
            Fire5Ani.SetBool("Die", true);
            if (DieTimer <= 0)
            {
                Destroy(this.gameObject);
            }
        }

        switch (face)
        {
            case Face.Left:
                DogSpr.flipX = true;
                break;
            case Face.Right:
                DogSpr.flipX = false;
                break;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "LeftWall")
        {
            TouchLeftWall = true;
        }
        if (collision.tag == "RightWall")
        {
            TouchRightWall = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "LeftWall")
        {
            TouchLeftWall = false;
        }
        if (collision.tag == "RightWall")
        {
            TouchRightWall = false;
        }
    }

    private void TurnFace()
    {
        switch (face)
        {
            case Face.Left:
                face = Face.Right;
                break;
            case Face.Right:
                face = Face.Left;
                break;
        }
    }
}
