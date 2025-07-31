using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnderJudgement : MonoBehaviour
{
    public PlayerController _controller;
    public float PushSpeed;
    private PlayerTouchJudgement _touchJudgement;
    private bool PushToRight;
    private bool PushToLeft;

    private void Start()
    {
        _touchJudgement = this.transform.parent.GetComponent<PlayerTouchJudgement>();
    }

    private void FixedUpdate()
    {
        if (PushToRight && !PushToLeft)
        {
            _controller.SetVelocity(new Vector2(PushSpeed, _controller.GetYVelocity()));
        }
        if (PushToLeft && !PushToRight)
        {
            _controller.SetVelocity(new Vector2(-PushSpeed, _controller.GetYVelocity()));
        }
        if(PushToRight && PushToLeft)
        {
            _controller.SetVelocity(new Vector2(PushSpeed, _controller.GetYVelocity()));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "monster" && collision.GetComponent<IgnorePlayerUnder>() == null)
        {
            if(this.transform.position.x >= collision.transform.position.x)
            {
                if (!_controller.touchRightWall)
                {
                    PushToRight = true;
                }
                else
                {
                    PushToLeft = true;
                }
                _touchJudgement.isMonsterUnder = true;
            }
            else
            {
                if (!_controller.touchLeftWall)
                {
                    PushToLeft = true;
                }
                else
                {
                    PushToRight = true;
                }
                _touchJudgement.isMonsterUnder = true;
            }
        }
        if (collision.GetComponent<CollisionType>() != null)
        {
            if (collision.GetComponent<CollisionType>().isChangeByPlayer)
            {
                collision.GetComponent<CollisionType>().BeAtkNoSound = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "monster" && collision.GetComponent<IgnorePlayerUnder>() == null)
        {
            _touchJudgement.isMonsterUnder = false;
            PushToRight = false;
            PushToLeft = false;
        }
        if (collision.GetComponent<CollisionType>() != null)
        {
            if (collision.GetComponent<CollisionType>().isChangeByPlayer)
            {
                collision.GetComponent<CollisionType>().BeAtkNoSound = false;
            }
        }
    }
}
