using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class CasterPortalJudge : MonoBehaviour
{
    private CasterController _controller;
    private RaycastHit2D GroundCheck;

    private bool inCollision;
    private void Awake()
    {
        _controller = this.transform.parent.parent.GetComponent<CasterController>();
    }

    void Update()
    {
        if (_controller == null)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "platform" && collision.GetComponent<BoxCollider2D>()!=null)
        {
            if(collision.GetComponent<BoxCollider2D>().isTrigger == false)
            {
                inCollision = true;
            }
        }
    }

    public void BoolFalse()
    {
        inCollision = false;
    }
    public bool JudgePortalPoint()
    {
        if (!inCollision)
        {
            GroundCheck = Physics2D.Raycast(transform.position, -Vector2.up, 50f, 1024);
            _controller.PortalPoint = new Vector3(GroundCheck.point.x, GroundCheck.point.y + 1.1f, 0);
            return true;
        }
        else
        {
            return false;
        }
    }//記錄傳送點且回傳是否成功 回傳true是有換地方 false是沒換
}
