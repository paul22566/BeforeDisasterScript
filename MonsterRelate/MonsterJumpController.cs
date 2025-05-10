using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterJumpController : MonoBehaviour
{
    /*
    ���}���ݭn����BasicData

    �D�}���ݭקﶵ��:

    �ŧi�ܼƨç�����}���BĲ�o�覡(�b�U�i����Jump��Status���K�[) �B�ʵe �BStatus(JumpEndJudge�B�����ɰʧ@�BisJumping ��o��) �B�Q���_

    jump��k�ݩ�bFixedUpdate
    */
    private Transform _transform;
    private MonsterBasicData _basicData;
    private MonsterJumpPoint NowUseJumpPoint;

    private bool isInJumpRange;
    private bool HJump;
    private bool VJump;
    [HideInInspector] public bool JumpStart;
    [HideInInspector] public bool JumpEnd;
    [HideInInspector] public bool isJumping;

    private bool ParabolaCaculate;
    public float DistanceXDemand;
    public float DistanceYDemand;
    private float JumpPointX;
    private ParabolaVar _parabolaVer = new ParabolaVar();

    [HideInInspector] public bool isPrepareRun;
    private bool ReverseWalkBindPrepareRun;

    private void Start()
    {
        _transform = transform;
        _basicData = this.GetComponent<MonsterBasicData>();
        _parabolaVer.VerticalDirection = "Dowm";
    }

    private void Update()
    {
        ReverseWalkJudge();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "MonsterJumpPoint" && !isJumping)
        {
            NowUseJumpPoint = collision.GetComponent<MonsterJumpPoint>();
            JumpPointX = NowUseJumpPoint.transform.localPosition.x;
            _parabolaVer.MiddlePoint = NowUseJumpPoint.MonsterJumpMiddlePoint.position;
            if (NowUseJumpPoint.isGoRight)
            {
                _parabolaVer.HorizontalDirection = "Right";
            }
            if (NowUseJumpPoint.isGoLeft)
            {
                _parabolaVer.HorizontalDirection = "Left";
            }
            if (NowUseJumpPoint.isHorizonJump)
            {
                HJump = true;
            }
            if (NowUseJumpPoint.isVerticalJump)
            {
                VJump = true;
            }
            isInJumpRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "MonsterJumpPoint")
        {
            if (!isJumping)
            {
                HJump = false;
                VJump = false;
            }
            isInJumpRange = false;
            if (isPrepareRun)
            {
                isPrepareRun = false;
            }
        }
    }

    public void Jump(float _fixedDeltaTime)
    {
        if (isJumping)
        {
            if (!ParabolaCaculate)
            {
                _parabolaVer.OtherPoint = _transform.localPosition;
                _parabolaVer.ParabolaConstant = Parabola.CalculateParabolaConstant(_parabolaVer);
                _parabolaVer.ParabolaNowX = _transform.localPosition.x;
                _parabolaVer.Speed = NowUseJumpPoint.JumpSpeedCalculate(_parabolaVer, _transform.localPosition);
                ParabolaCaculate = true;
            }

            Parabola.ParabolaMove(_parabolaVer, _fixedDeltaTime, _transform);
        }
    }

    public void JumpStartJudge()
    {
        bool ShouldJump = false;

        if (isInJumpRange)
        {
            if (HJump && _basicData.AbsDistanceX > DistanceXDemand)
            {
                if (_parabolaVer.HorizontalDirection == "Right" && _basicData.face == MonsterBasicData.Face.Right)
                {
                    ShouldJump = true;
                }
                if (_parabolaVer.HorizontalDirection == "Left" && _basicData.face == MonsterBasicData.Face.Left)
                {
                    ShouldJump = true;
                }
            }
            if (VJump && _basicData.AbsDistanceY > DistanceYDemand)
            {
                if (_parabolaVer.HorizontalDirection == "Right" && _basicData.face == MonsterBasicData.Face.Right)
                {
                    ShouldJump = true;
                }
                if (_parabolaVer.HorizontalDirection == "Left" && _basicData.face == MonsterBasicData.Face.Left)
                {
                    ShouldJump = true;
                }
            }

            if (VJump && ShouldJump)
            {
                if (_parabolaVer.HorizontalDirection == "Right" && _transform.localPosition.x > JumpPointX)
                {
                    isPrepareRun = true;
                    ReverseWalkBindPrepareRun = true;
                }
                if (_parabolaVer.HorizontalDirection == "Left" && _transform.localPosition.x < JumpPointX)
                {
                    isPrepareRun = true;
                    ReverseWalkBindPrepareRun = true;
                }
            }//�P�_�O�_�n�U�]

            if (ShouldJump && !isPrepareRun)
            {
                JumpStart = true;
            }
        }
    }

    public void JumpEndJudge()
    {
        if (_basicData.isGround && !isInJumpRange)
        {
            AllVariableFalse();
            JumpEnd = true;
        }
    }

    private void ReverseWalkJudge()
    {
        if (ReverseWalkBindPrepareRun)
        {
            if (isPrepareRun)
            {
                _basicData.ReverseWalk = true;
            }
            else
            {
                _basicData.ReverseWalk = false;
                ReverseWalkBindPrepareRun = false;

            }
        }
    }

    public void AllVariableFalse()
    {
        ParabolaCaculate = false;
        HJump = false;
        VJump = false;
        JumpStart = false;
        isJumping = false;
    }
}
