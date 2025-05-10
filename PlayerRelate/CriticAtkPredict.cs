using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticAtkPredict : MonoBehaviour
{
    private bool GoRight = false;
    private bool GoLeft = false;
    private bool isGround;
    //牆壁
    private Transform _transform;
    private Transform RightJudgement;
    private Transform LeftJudgement;
    private bool IgnoreCheck;
    private bool TouchRightWall;
    private bool TouchLeftWall;

    private RaycastHit2D RightWallCheck;
    private RaycastHit2D LeftWallCheck;
    public float TouchWallLineDistance = 1.83f;

    //斜坡
    private Transform LeftDetecter;
    private Transform RightDetecter;//優先探測器
    private Transform LeftDirectionDetecter;
    private Transform RightDirectionDetecter;//低精度 方向專用探測器

    private RaycastHit2D LeftSlopeRaycast;
    private RaycastHit2D RightSlopeRaycast;
    private RaycastHit2D LeftDirectionRaycast;
    private RaycastHit2D RightDirectionRaycast;
    private RaycastHit2D LeftGroundRaycast;
    private RaycastHit2D RightGroundRaycast;

    public float FlatDetectDistance = 0.21f;
    public float SlopeDetectDistance = 0.39f;
    public float DirectionDetectDistance = 1.5f;

    private Vector2 LeftDirection = new Vector2();
    private Vector2 RightDirection = new Vector2();

    private bool LeftDetecterOnLeftSlope = false;
    private bool LeftDetecterOnRightSlope = false;
    private bool RightDetecterOnLeftSlope = false;
    private bool RightDetecterOnRightSlope = false;

    private bool onLeftSlope;
    private bool onRightSlope;
    private bool AbsoluteLeaveSlope;

    //移動
    private float _fixedDeltaTime;
    public float Speed = 15;
    private bool ShouldWalkRight;
    private bool ShouldWalkLeft;
    private Vector2 MoveDirection = new Vector2(1, 0);
    private Vector2 MoveDirectionDefault = new Vector2(1, 0);

    // Start is called before the first frame update
    void Start()
    {
        switch (PlayerController.face)
        {
            case PlayerController.Face.Right:
                GoRight = true;
                break;
            case PlayerController.Face.Left:
                GoLeft = true;
                break;
        }
        _transform = this.transform;

        RightJudgement = _transform.GetChild(0).GetChild(0);
        LeftJudgement = _transform.GetChild(0).GetChild(1);
        LeftDetecter = _transform.GetChild(1).GetChild(0);
        RightDetecter = _transform.GetChild(1).GetChild(1);
        LeftDirectionDetecter = _transform.GetChild(1).GetChild(2);
        RightDirectionDetecter = _transform.GetChild(1).GetChild(3);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastCheck();
        BoolJudgement();

        DetectDirection();
        DetectGround();
        DetectOnSlope();

        IgnoreCheck = false;
    }

    private void FixedUpdate()
    {
        _fixedDeltaTime = Time.fixedDeltaTime;

        JudgeMoveDirection();

        GroundMovement();
    }

    private void JudgeMoveDirection()
    {
        bool ShouldReset = false;

        if (!TouchRightWall && GoRight)
        {
            ShouldWalkRight = true;
        }
        if (!TouchLeftWall && GoLeft)
        {
            ShouldWalkLeft = true;
        }

        if (AbsoluteLeaveSlope)
        {
            ShouldReset = true;
        }

        if (!onLeftSlope && !onRightSlope)
        {
            if (GoLeft && RightDetecterOnLeftSlope)
            {
                ShouldReset = true;
            }
            if (GoRight && LeftDetecterOnRightSlope)
            {
                ShouldReset = true;
            }
        }

        //左斜坡
        if (onLeftSlope)
        {
            if (GoRight)
            {
                MoveDirection = new Vector2(LeftDirection.x, -LeftDirection.y);
            }
            if (GoLeft)
            {
                MoveDirection = new Vector2(LeftDirection.x, LeftDirection.y);
            }
        }
        else if (RightDetecterOnLeftSlope)
        {
            if (GoRight)
            {
                MoveDirection = new Vector2(RightDirection.x, -RightDirection.y);
            }
        }

        //右斜坡
        if (onRightSlope)
        {
            if (GoRight)
            {
                MoveDirection = new Vector2(RightDirection.x, -RightDirection.y);
            }
            if (GoLeft)
            {
                MoveDirection = new Vector2(RightDirection.x, RightDirection.y);
            }
        }
        else if (LeftDetecterOnRightSlope)
        {
            if (GoLeft)
            {
                MoveDirection = new Vector2(LeftDirection.x, LeftDirection.y);
            }
        }

        if (ShouldReset)
        {
            if (MoveDirection != MoveDirectionDefault)
            {
                MoveDirection = MoveDirectionDefault;
            }
            return;
        }
    }

    //地面移動
    private void GroundMovement()
    {
        if (ShouldWalkRight && isGround)
        {
            _transform.position = new Vector3(Speed * MoveDirection.x * _fixedDeltaTime + _transform.position.x, Speed * MoveDirection.y * _fixedDeltaTime + _transform.position.y, 0);
        }
        if (ShouldWalkLeft && isGround)
        {
            _transform.position = new Vector3(-Speed * MoveDirection.x * _fixedDeltaTime + _transform.position.x, Speed * MoveDirection.y * _fixedDeltaTime + _transform.position.y, 0);
        }

        ShouldWalkLeft = false;
        ShouldWalkRight = false;
    }

    private void RaycastCheck()
    {
        RightWallCheck = Physics2D.Raycast(RightJudgement.position, -Vector2.up, TouchWallLineDistance, 1024);
        if (!RightWallCheck)
        {
            RightWallCheck = Physics2D.Raycast(RightJudgement.position, -Vector2.up, TouchWallLineDistance, 32768);
        }
        if (!RightWallCheck)
        {
            if (Physics2D.Raycast(RightJudgement.position, -Vector2.up, TouchWallLineDistance, 128))
            {
                RightWallCheck = Physics2D.Raycast(RightJudgement.position, -Vector2.up, TouchWallLineDistance, 128);
            }
        }

        LeftWallCheck = Physics2D.Raycast(LeftJudgement.position, -Vector2.up, TouchWallLineDistance, 1024);
        if (!LeftWallCheck)
        {
            LeftWallCheck = Physics2D.Raycast(LeftJudgement.position, -Vector2.up, TouchWallLineDistance, 32768);
        }
        if (!LeftWallCheck)
        {
            if (Physics2D.Raycast(LeftJudgement.position, -Vector2.up, TouchWallLineDistance, 64))
            {
                LeftWallCheck = Physics2D.Raycast(LeftJudgement.position, -Vector2.up, TouchWallLineDistance, 64);
            }
        }

        //斜坡上不計入touchWall
        if (RightWallCheck)
        {
            if (Mathf.Abs(RightWallCheck.normal.x) != 0 || onRightSlope)
            {
                IgnoreCheck = true;
            }
        }
        if (LeftWallCheck)
        {
            if (Mathf.Abs(LeftWallCheck.normal.x) != 0 || onLeftSlope)
            {
                IgnoreCheck = true;
            }
        }
    }

    private void BoolJudgement()
    {
        if (LeftWallCheck && !IgnoreCheck)
        {
            TouchLeftWall = true;
        }
        else
        {
            TouchLeftWall = false;
        }
        if (RightWallCheck && !IgnoreCheck)
        {
            TouchRightWall = true;
        }
        else
        {
            TouchRightWall = false;
        }
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

        if (!onLeftSlope && !onRightSlope && !LeftDetecterOnLeftSlope && !LeftDetecterOnRightSlope && !RightDetecterOnLeftSlope && !RightDetecterOnRightSlope)
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
        RightGroundRaycast = Physics2D.Raycast(RightDetecter.position, -Vector2.up, FlatDetectDistance, 1024);
        if (!RightGroundRaycast)
        {
            RightGroundRaycast = Physics2D.Raycast(RightDetecter.position, -Vector2.up, FlatDetectDistance, 32768);
        }
        LeftGroundRaycast = Physics2D.Raycast(LeftDetecter.position, -Vector2.up, FlatDetectDistance, 1024);
        if (!LeftGroundRaycast)
        {
            LeftGroundRaycast = Physics2D.Raycast(LeftDetecter.position, -Vector2.up, FlatDetectDistance, 32768);
        }

        if (RightGroundRaycast && LeftGroundRaycast)
        {
            isGround = true;
        }
        else
        {
            isGround = false;
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
