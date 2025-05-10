using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianHead : MonoBehaviour
{
    private GuardianController _controller;
    private MonsterHurtedController _hurtedController;
    private GuardianHurted _hurtedAni;
    private MonsterBasicData _basicData;
    private ObjectShield _objectShield;
    private Animator HeadAnimator;
    private SpriteRenderer _spr;
    private Transform _transform;
    private Transform playerTransform;
    private float _fixDeltaTime;

    private float HeadCorrectionAngle = 20;
    public bool RotateFollowBone;
    private int ExcessMultiple;
    private float RotateSpeed = 100;
    private float FollowBoneSpeed = 3600;
    private float AutoRotateSpeed = 50;
    private float TargetRotate;
    private float NowRotate;
    private static float StandardNowRotate;
    private enum LastFace { Left, Right };
    private LastFace lastFace;
    private enum Side { AtPlayerLeft, AtPlayerRight };
    private Side side;

    private bool isHigherThanPlayer;
    public float WidthLowAngle;
    [SerializeField] private float LowAngle;
    [SerializeField] private float HighAngle;
    public bool UseWidthAngleLimit;

    //Distance
    [HideInInspector] public float DistanceX;//怪物與玩家的距離
    [HideInInspector] public float AbsDistanceX;//怪物與玩家的距離
    [HideInInspector] public float AbsDistanceY;//怪物與玩家的距離(絕對值)
    [HideInInspector] public float DistanceY;//怪物與玩家的距離

    //Follow
    private Transform RotationTarget;
    private Transform PositionTarget;
    public Transform WaitHead;
    public Transform WalkHead;
    public Transform JumpHead;
    public Transform AtkHead;
    public Transform Atk2Head;
    public Transform BackAtk2Head;
    public Transform BeginingHead;
    public Transform HowlHead;
    public Transform StopHead;

    [Header("張嘴")]
    [HideInInspector] public bool CanOpenMouth;
    public float DefaultCloseMouthTime;
    private float CloseMouthTime;
    public float MouthTimerSet;
    private float Timer;

    private void Start()
    {
        _transform = this.transform;

        if (GameObject.Find("player") != null)
        {
            playerTransform = GameObject.Find("player").transform;
        }

        HeadAnimator = _transform.GetChild(0).GetComponent<Animator>();
        _spr = _transform.GetChild(0).GetComponent<SpriteRenderer>();
        Timer = MouthTimerSet;
        _controller = this.transform.parent.GetComponent<GuardianController>();
        _hurtedController = this.transform.parent.GetComponent<MonsterHurtedController>();
        _hurtedAni = this.transform.parent.GetComponent<GuardianHurted>();
        _basicData = this.transform.parent.GetComponent<MonsterBasicData>();
        _objectShield = this.GetComponent<ObjectShield>();

        switch (_basicData.face)
        {
            case MonsterBasicData.Face.Right:
                lastFace = LastFace.Right;
                break;
            case MonsterBasicData.Face.Left:
                lastFace = LastFace.Left;
                break;
        }

        //指定Target
        switch (_controller.NowAni)
        {
            case GuardianController.AniStatus.wait:
                PositionTarget = WaitHead;
                RotationTarget = WaitHead;
                break;
            case GuardianController.AniStatus.walk:
                PositionTarget = WalkHead;
                RotationTarget = WalkHead;
                break;
            case GuardianController.AniStatus.Jump:
                PositionTarget = JumpHead;
                RotationTarget = JumpHead;
                break;
            case GuardianController.AniStatus.Atk1:
                PositionTarget = AtkHead;
                RotationTarget = AtkHead;
                break;
            case GuardianController.AniStatus.Atk2:
                PositionTarget = Atk2Head;
                RotationTarget = Atk2Head;
                break;
            case GuardianController.AniStatus.BackAtk2:
                PositionTarget = BackAtk2Head;
                RotationTarget = BackAtk2Head;
                break;
            case GuardianController.AniStatus.Begining:
                PositionTarget = BeginingHead;
                RotationTarget = BeginingHead;
                break;
            case GuardianController.AniStatus.Howl:
                PositionTarget = HowlHead;
                RotationTarget = HowlHead;
                break;
            case GuardianController.AniStatus.Stop:
                PositionTarget = StopHead;
                RotationTarget = StopHead;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //指定Target
        switch (_controller.NowAni)
        {
            case GuardianController.AniStatus.wait:
                PositionTarget = WaitHead;
                RotationTarget = WaitHead;
                break;
            case GuardianController.AniStatus.walk:
                PositionTarget = WalkHead;
                RotationTarget = WalkHead;
                break;
            case GuardianController.AniStatus.Jump:
                PositionTarget = JumpHead;
                RotationTarget = JumpHead;
                break;
            case GuardianController.AniStatus.Atk1:
                PositionTarget = AtkHead;
                RotationTarget = AtkHead;
                break;
            case GuardianController.AniStatus.Atk2:
                PositionTarget = Atk2Head;
                RotationTarget = Atk2Head;
                break;
            case GuardianController.AniStatus.BackAtk2:
                PositionTarget = BackAtk2Head;
                RotationTarget = BackAtk2Head;
                break;
            case GuardianController.AniStatus.Begining:
                PositionTarget = BeginingHead;
                RotationTarget = BeginingHead;
                break;
            case GuardianController.AniStatus.Howl:
                PositionTarget = HowlHead;
                RotationTarget = HowlHead;
                break;
            case GuardianController.AniStatus.Stop:
                PositionTarget = StopHead;
                RotationTarget = StopHead;
                break;
        }

        switch (_basicData.face)
        {
            case MonsterBasicData.Face.Right:
                _spr.flipX = true;
                if (lastFace != LastFace.Right)
                {
                    TurnFace();
                    lastFace = LastFace.Right;
                }
                break;
            case MonsterBasicData.Face.Left:
                _spr.flipX = false;
                if (lastFace != LastFace.Left)
                {
                    TurnFace();
                    lastFace = LastFace.Left;
                }
                break;
        }

        _transform.position = PositionTarget.position;

        CalculateStandardRotate();

        HurtedControll();

        if (RotateFollowBone)
        {
            TargetRotate = RotationTarget.eulerAngles.z;
            if (TargetRotate < 0)
            {
                ExcessMultiple = (int)(RotationTarget.eulerAngles.z / 360);
                ExcessMultiple += 1;
                TargetRotate = 360 * ExcessMultiple + TargetRotate;
                if (TargetRotate == 360)
                {
                    TargetRotate = 0;
                }
            }
            if (TargetRotate > 360)
            {
                ExcessMultiple = (int)(RotationTarget.eulerAngles.z / 360);
                TargetRotate = TargetRotate - 360 * ExcessMultiple;
            }
            RotateSpeedSet(FollowBoneSpeed);
        }
        else
        {
            RotateSpeedSet(AutoRotateSpeed);
            RotateJudge();
        }
    }

    private void FixedUpdate()
    {
        _fixDeltaTime = Time.fixedDeltaTime;

        ControllMouth();

        AutoRotate();
    }

    public void StopRotateByBone()//(1)
    {
        RotateFollowBone = false;
    }

    public void BeginRotateByBone()//(3)
    {
        RotateFollowBone = true;
    }

    public void OpenMouth(float TotalTime, float CloseTime)//自行指定(1)
    {
        Timer = TotalTime;
        CloseMouthTime = CloseTime;
        CloseMouthTime = TotalTime - CloseMouthTime;
        CanOpenMouth = true;
        HeadAnimator.SetBool("Open", true);
    }

    public void OpenMouth()//使用預設(1)
    {
        Timer = MouthTimerSet;
        CanOpenMouth = true;
        CloseMouthTime = MouthTimerSet - DefaultCloseMouthTime;
        HeadAnimator.SetBool("Open", true);
    }

    private void ControllMouth()//(2)
    {
        if (CanOpenMouth)
        {
            Timer -= _fixDeltaTime;

            if (Timer <= CloseMouthTime)
            {
                HeadAnimator.SetBool("Open", false);
                HeadAnimator.SetBool("Fast", false);
            }
            if (Timer <= 0)
            {
                Timer = MouthTimerSet;
                CanOpenMouth = false;
            }
        }
    }

    private void RotateJudge()//(3)
    {
        if (playerTransform != null)
        {
            DistanceX = _transform.position.x - playerTransform.position.x;
            AbsDistanceX = Mathf.Abs(DistanceX);
            DistanceY = playerTransform.position.y - _transform.position.y;
            AbsDistanceY = Mathf.Abs(DistanceY);

            //計算相對方位
            if (DistanceX <= 0)
            {
                side = Side.AtPlayerLeft;
            }
            else
            {
                side = Side.AtPlayerRight;
            }
            if (DistanceY <= 0)
            {
                isHigherThanPlayer = true;
            }
            else
            {
                isHigherThanPlayer = false;
            }

            switch (_basicData.face)
            {
                case MonsterBasicData.Face.Right:
                    TargetRotate = AngleCaculate.CaculateAngle("R", _transform, playerTransform);
                    break;
                case MonsterBasicData.Face.Left:
                    TargetRotate = AngleCaculate.CaculateAngle("L", _transform, playerTransform);
                    break;
            }
        }

        //target限制
        if (!UseWidthAngleLimit)
        {
            switch (_basicData.face)
            {
                case MonsterBasicData.Face.Right:
                    if (TargetRotate < LowAngle && TargetRotate >= 180)
                    {
                        TargetRotate = LowAngle;
                    }
                    if (TargetRotate > HighAngle && TargetRotate < 180)
                    {
                        TargetRotate = HighAngle;
                    }

                    if (side == Side.AtPlayerRight)
                    {
                        if (isHigherThanPlayer)
                        {
                            TargetRotate = LowAngle;
                        }
                        else
                        {
                            TargetRotate = HighAngle;
                        }
                    }
                    break;
                case MonsterBasicData.Face.Left:
                    if (TargetRotate > (360 - LowAngle) && TargetRotate <= 180)
                    {
                        TargetRotate = (360 - LowAngle);
                    }
                    if (TargetRotate < (360 - HighAngle) && TargetRotate > 180)
                    {
                        TargetRotate = (360 - HighAngle);
                    }

                    if (side == Side.AtPlayerLeft)
                    {
                        if (isHigherThanPlayer)
                        {
                            TargetRotate = (360 - LowAngle);
                        }
                        else
                        {
                            TargetRotate = (360 - HighAngle);
                        }
                    }
                    break;
            }
        }
        if (UseWidthAngleLimit)
        {
            switch (_basicData.face)
            {
                case MonsterBasicData.Face.Right:
                    if (TargetRotate < WidthLowAngle && TargetRotate >= 180)
                    {
                        TargetRotate = WidthLowAngle;
                    }
                    if (TargetRotate > HighAngle && TargetRotate < 180)
                    {
                        TargetRotate = HighAngle;
                    }

                    if (side == Side.AtPlayerRight)
                    {
                        if (!isHigherThanPlayer)
                        {
                            TargetRotate = HighAngle;
                        }
                    }
                    break;
                case MonsterBasicData.Face.Left:
                    if (TargetRotate > (360 - WidthLowAngle) && TargetRotate <= 180)
                    {
                        TargetRotate = (360 - WidthLowAngle);
                    }
                    if (TargetRotate < (360 - HighAngle) && TargetRotate > 180)
                    {
                        TargetRotate = (360 - HighAngle);
                    }

                    if (side == Side.AtPlayerLeft)
                    {
                        if (!isHigherThanPlayer)
                        {
                            TargetRotate = (360 - HighAngle);
                        }
                    }
                    break;
            }
        }

        //最終補正
        switch (_basicData.face)
        {
            case MonsterBasicData.Face.Right:
                TargetRotate = HeadCorrectionAngle + TargetRotate;
                break;
            case MonsterBasicData.Face.Left:
                TargetRotate = TargetRotate - HeadCorrectionAngle;
                break;
        }
        if (TargetRotate >= 360)
        {
            TargetRotate -= 360;
        }
        if (TargetRotate < 0)
        {
            TargetRotate += 360;
        }
    }

    private void AutoRotate()//(2)
    {
        //執行
        NowRotate = _transform.eulerAngles.z;
        switch (_basicData.face)
        {
            case MonsterBasicData.Face.Left:
                if (NowRotate < TargetRotate)
                {
                    if (Mathf.Abs(NowRotate - TargetRotate) <= 180)
                    {
                        NowRotate += RotateSpeed * _fixDeltaTime;
                        if (NowRotate > TargetRotate)
                        {
                            NowRotate = TargetRotate;
                        }
                    }
                    else
                    {
                        NowRotate -= RotateSpeed * _fixDeltaTime;
                        NowRotateCorrection();
                        if (NowRotate < TargetRotate && Mathf.Abs(NowRotate - TargetRotate) <= 180)
                        {
                            NowRotate = TargetRotate;
                        }
                    }
                }
                if (NowRotate > TargetRotate)
                {
                    if (Mathf.Abs(NowRotate - TargetRotate) <= 180)
                    {
                        NowRotate -= RotateSpeed * _fixDeltaTime;
                        if (NowRotate < TargetRotate)
                        {
                            NowRotate = TargetRotate;
                        }
                    }
                    else
                    {
                        NowRotate += RotateSpeed * _fixDeltaTime;
                        NowRotateCorrection();
                        if (NowRotate > TargetRotate && Mathf.Abs(NowRotate - TargetRotate) <= 180)
                        {
                            NowRotate = TargetRotate;
                        }
                    }
                }
                break;
            case MonsterBasicData.Face.Right:
                if (NowRotate < TargetRotate)
                {
                    if (Mathf.Abs(NowRotate - TargetRotate) <= 180)
                    {
                        NowRotate += RotateSpeed * _fixDeltaTime;
                        if (NowRotate > TargetRotate)
                        {
                            NowRotate = TargetRotate;
                        }
                    }
                    else
                    {
                        NowRotate -= RotateSpeed * _fixDeltaTime;
                        NowRotateCorrection();
                        if (NowRotate < TargetRotate && Mathf.Abs(NowRotate - TargetRotate) <= 180)
                        {
                            NowRotate = TargetRotate;
                        }
                    }
                }
                if (NowRotate > TargetRotate)
                {
                    if (Mathf.Abs(NowRotate - TargetRotate) <= 180)
                    {
                        NowRotate -= RotateSpeed * _fixDeltaTime;
                        if (NowRotate < TargetRotate)
                        {
                            NowRotate = TargetRotate;
                        }
                    }
                    else
                    {
                        NowRotate += RotateSpeed * _fixDeltaTime;
                        NowRotateCorrection();
                        if (NowRotate > TargetRotate && Mathf.Abs(NowRotate - TargetRotate) <= 180)
                        {
                            NowRotate = TargetRotate;
                        }
                    }
                }
                break;
        }

        NowRotateCorrection();

        _transform.rotation = Quaternion.Euler(0, 0, NowRotate);
    }

    private void NowRotateCorrection()
    {
        if (NowRotate >= 360)
        {
            NowRotate = NowRotate - 360;
        }
        if (NowRotate < 0)
        {
            NowRotate = NowRotate + 360;
        }
    }

    private void CalculateStandardRotate()//(2)
    {
        switch (_basicData.face)
        {
            case MonsterBasicData.Face.Right:
                StandardNowRotate = NowRotate;
                break;
            case MonsterBasicData.Face.Left:
                StandardNowRotate = AngleCaculate.AngleDirectionChange(NowRotate);
                break;
        }
    }

    private void RotateSpeedSet(float Speed)//(3)
    {
        if (RotateSpeed != Speed)
        {
            RotateSpeed = Speed;
        }
    }

    private void TurnFace()//(1)
    {
        if (RotateFollowBone)
        {
            return;
        }

        NowRotate = AngleCaculate.AngleDirectionChange(NowRotate);
        _transform.rotation = Quaternion.Euler(0, 0, NowRotate);
    }

    public void InitializeHead(GuardianHeadValue Value)//(1)
    {
        RotateFollowBone = Value.RotateFollowBone;
        WidthLowAngle = Value.WidthLowAngle;
        LowAngle = Value.LowAngle;
        HighAngle = Value.HighAngle;
        DefaultCloseMouthTime = Value.CloseMouthTime;
        MouthTimerSet = Value.MouthTimer;

        if(WidthLowAngle != 0)
        {
            UseWidthAngleLimit = true;
        }

        _transform.position = PositionTarget.position;

        switch (_basicData.face)
        {
            case MonsterBasicData.Face.Left:
                _transform.rotation = Quaternion.Euler(0, 0, AngleCaculate.AngleDirectionChange(StandardNowRotate));
                lastFace = LastFace.Left;
                _spr.flipX = false;
                break;
            case MonsterBasicData.Face.Right:
                _transform.rotation = Quaternion.Euler(0, 0, StandardNowRotate);
                lastFace = LastFace.Right;
                _spr.flipX = true;
                break;
        }

        if (RotateFollowBone)
        {
            return;
        }

        RotateJudge();

        //玩家不存在時
        if (playerTransform == null)
        {
            TargetRotate = 0;
        }

    }//啟動時都要用到

    private void HurtedControll()//(2)
    {
        if (_objectShield.ProtectSuccess)
        {
            _hurtedController.HurtedControll(1);
        }

        if (_hurtedAni.isValidHurted)
        {
            _spr.color = new Color(0.65f, 0.48f, 0.48f, 1);
        }
        else
        {
            _spr.color = new Color(1, 1, 1, 1);
        }
    }
}

public struct GuardianHeadValue
{
    public bool RotateFollowBone;
    public float WidthLowAngle;
    public float LowAngle;
    public float HighAngle;
    public float CloseMouthTime;
    public float MouthTimer;

    public GuardianHeadValue(bool Bone, float WLA, float Low, float High, float CLT, float MT)
    {
        RotateFollowBone = Bone;
        WidthLowAngle = WLA;
        LowAngle = Low;
        HighAngle = High;
        CloseMouthTime = CLT;
        MouthTimer = MT;
    }
}
