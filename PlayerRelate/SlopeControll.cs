using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class SlopeControll : MonoBehaviour
{
    public Transform MainObject;

    private Transform LeftDetecter;
    private Transform RightDetecter;//優先探測器
    private Transform LeftDirectionDetecter;
    private Transform RightDirectionDetecter;//低精度 方向專用探測器

    [HideInInspector] public RaycastHit2D LeftSlopeRaycast;
    [HideInInspector] public RaycastHit2D RightSlopeRaycast;
    [HideInInspector] public RaycastHit2D LeftDirectionRaycast;
    [HideInInspector] public RaycastHit2D RightDirectionRaycast;
    [HideInInspector] public RaycastHit2D LeftGroundRaycast;
    [HideInInspector] public RaycastHit2D RightGroundRaycast;

    public float FlatDetectDistance = 0.1f;
    public float SlopeDetectDistance = 0.39f;
    public float DirectionDetectDistance = 1.5f;

    [HideInInspector] public Vector2 LeftDirection = new Vector2();
    [HideInInspector] public Vector2 RightDirection = new Vector2();

    [HideInInspector] public bool LeftDetecterOnGround;
    [HideInInspector] public bool RightDetecterOnGround;

    private bool LeftDetecterOnLeftSlope = false;
    [HideInInspector] public bool LeftDetecterOnRightSlope = false;
    [HideInInspector] public bool RightDetecterOnLeftSlope = false;
    private bool RightDetecterOnRightSlope = false;

    [HideInInspector] public bool onLeftSlope;
    [HideInInspector] public bool onRightSlope;
    [HideInInspector] public bool AbsoluteLeaveSlope;

    // Start is called before the first frame update
    void Start()
    {
        LeftDetecter = this.transform.GetChild(0);
        RightDetecter = this.transform.GetChild(1);
        LeftDirectionDetecter = this.transform.GetChild(2);
        RightDirectionDetecter = this.transform.GetChild(3);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = MainObject.position;

        DetectDirection();

        DetectGround();

        DetectOnSlope();
    }

    private void DetectOnSlope()
    {
        LeftSlopeRaycast = Physics2D.Raycast(LeftDetecter.position, -Vector2.up, SlopeDetectDistance, 1024);
        RightSlopeRaycast = Physics2D.Raycast(RightDetecter.position, -Vector2.up, SlopeDetectDistance, 1024);

        if (LeftSlopeRaycast.collider != null && Mathf.Abs(LeftSlopeRaycast.normal.x) > 0.1f)
        {
            if (LeftSlopeRaycast.normal.x > 0)
            {
                LeftDetecterOnLeftSlope = true;
            }
            else
            {
                LeftDetecterOnRightSlope = true;
            }
        }
        if (RightSlopeRaycast.collider != null && Mathf.Abs(RightSlopeRaycast.normal.x) > 0.1f)
        {
            if (RightSlopeRaycast.normal.x > 0)
            {
                RightDetecterOnLeftSlope = true;
            }
            else
            {
                RightDetecterOnRightSlope = true;
            }
        }

        //判斷為正的條件
        if (LeftDetecterOnLeftSlope)
        {
            onLeftSlope = true;
        }
        if (!LeftDetecterOnLeftSlope && LeftSlopeRaycast && LeftSlopeRaycast.distance == 0)
        {
            if (Mathf.Abs(LeftDirectionRaycast.normal.x) > 0.1f)
            {
                onLeftSlope = true;
            }
        }

        if (RightDetecterOnRightSlope)
        {
            onRightSlope = true;
        }
        if (!RightDetecterOnLeftSlope && RightSlopeRaycast && RightSlopeRaycast.distance == 0)
        {
            if (Mathf.Abs(RightDirectionRaycast.normal.x) > 0.1f)
            {
                onRightSlope = true;
            }
        }

        //判斷為負的條件
        if (!RightSlopeRaycast)
        {
            RightDetecterOnLeftSlope = false;
            RightDetecterOnRightSlope = false;
            onRightSlope = false;
        }
        if (Mathf.Abs(RightSlopeRaycast.normal.x) < 0.1f && RightSlopeRaycast.distance != 0)
        {
            RightDetecterOnLeftSlope = false;
            RightDetecterOnRightSlope = false;
            onRightSlope = false;
        }
        if (!LeftSlopeRaycast)
        {
            LeftDetecterOnLeftSlope = false;
            LeftDetecterOnRightSlope = false;
            onLeftSlope = false;
        }
        if (Mathf.Abs(LeftSlopeRaycast.normal.x) < 0.1f && LeftSlopeRaycast.distance != 0)
        {
            LeftDetecterOnLeftSlope = false;
            LeftDetecterOnRightSlope = false;
            onLeftSlope = false;
        }

        if (!onLeftSlope && !onRightSlope && !LeftDetecterOnLeftSlope && !LeftDetecterOnRightSlope &&!RightDetecterOnLeftSlope && !RightDetecterOnRightSlope)
        {
            AbsoluteLeaveSlope = true;
        }
        else
        {
            AbsoluteLeaveSlope = false;
        }
    }

    private void DetectGround()
    {
        LeftGroundRaycast = Physics2D.Raycast(LeftDetecter.position, -Vector2.up, FlatDetectDistance, 1024);
        if (!LeftGroundRaycast)
        {
            LeftGroundRaycast = Physics2D.Raycast(LeftDetecter.position, -Vector2.up, FlatDetectDistance, 32768);
        }
        if (LeftGroundRaycast)
        {
            if (LeftGroundRaycast.distance > 0.01f)
            {
                LeftDetecterOnGround = true;
            }
            else
            {
                if (LeftGroundRaycast.transform.GetComponent<DashJudgement>() != null && LeftGroundRaycast.transform.GetComponent<DashJudgement>().type == DashJudgement.BoxType.Flat)
                {
                    LeftDetecterOnGround = false;
                }
                else
                {
                    LeftDetecterOnGround = true;
                }
            }
        }
        else
        {
            LeftDetecterOnGround = false;
        }

        RightGroundRaycast = Physics2D.Raycast(RightDetecter.position, -Vector2.up, FlatDetectDistance, 1024);
        if (!RightGroundRaycast)
        {
            RightGroundRaycast = Physics2D.Raycast(RightDetecter.position, -Vector2.up, FlatDetectDistance, 32768);
        }
        if (RightGroundRaycast)
        {
            if (RightGroundRaycast.distance > 0.01f)
            {
                RightDetecterOnGround = true;
            }
            else
            {
                if (RightGroundRaycast.transform.GetComponent<DashJudgement>() != null && RightGroundRaycast.transform.GetComponent<DashJudgement>().type == DashJudgement.BoxType.Flat)
                {
                    RightDetecterOnGround = false;
                }
                else
                {
                    RightDetecterOnGround = true;
                }
            }
        }
        else
        {
            RightDetecterOnGround = false;
        }
    }

    private void DetectDirection()
    {
        LeftDirectionRaycast = Physics2D.Raycast(LeftDirectionDetecter.position, -Vector2.up, DirectionDetectDistance, 1024);
        RightDirectionRaycast = Physics2D.Raycast(RightDirectionDetecter.position, -Vector2.up, DirectionDetectDistance, 1024);

        if (LeftDirectionRaycast)
        {
            LeftDirection = new Vector2(LeftDirectionRaycast.normal.y, LeftDirectionRaycast.normal.x);
        }
        if (RightDirectionRaycast)
        {
            RightDirection = new Vector2(RightDirectionRaycast.normal.y, RightDirectionRaycast.normal.x);
        }
    }//只負責偵測Direction 不負責決定是否使用
}
