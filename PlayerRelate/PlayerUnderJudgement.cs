using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnderJudgement : MonoBehaviour
{
    public Transform Player;
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
            PlayerController.Rigid2D.velocity = new Vector2(PushSpeed, PlayerController.Rigid2D.velocity.y);
        }
        if (PushToLeft && !PushToRight)
        {
            PlayerController.Rigid2D.velocity = new Vector2(-PushSpeed, PlayerController.Rigid2D.velocity.y);
        }
        if(PushToRight && PushToLeft)
        {
            PlayerController.Rigid2D.velocity = new Vector2(PushSpeed, PlayerController.Rigid2D.velocity.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "monster" && collision.GetComponent<IgnorePlayerUnder>() == null)
        {
            if(this.transform.position.x >= collision.transform.position.x)
            {
                if (!Player.GetComponent<PlayerController>().touchRightWall)
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
                if (!Player.GetComponent<PlayerController>().touchLeftWall)
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
            PlayerController.Rigid2D.velocity = new Vector2(0, PlayerController.Rigid2D.velocity.y);
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
