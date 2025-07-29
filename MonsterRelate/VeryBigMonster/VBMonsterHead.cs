using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Cinemachine.CinemachineOrbitalTransposer;

public class VBMonsterHead : MonoBehaviour
{
    private VeryBigMonsterController _controller;
    private VBigMonsterHurted _HurtedAni;
    private MonsterHurtedController _hurtedController;
    private MonsterBasicData _basicData;
    private ObjectShield _objectShield;
    private Animator HeadAnimator;
    private Animator SHeadAnimator;
    private GameObject Head;
    private GameObject SHead;
    private SpriteRenderer _spr;
    private SpriteRenderer S_spr;
    private Transform _transform;
    private Transform playerTransform;
    private CollisionType _collisionType;
    [HideInInspector] public int HeadHurtNumber;
    private float _fixDeltaTime;

    private float HeadCorrectionAngle = 20;
    public bool RotateFollowBone;
    private int ExcessMultiple;
    private float RotateSpeed = 100;
    private float FollowBoneSpeed = 400;
    private float AutoRotateSpeed = 50;
    private float TargetRotate;
    private float NowRotate;
    private static float StandardNowRotate;
    private enum LastFace { Left, Right };
    private LastFace lastFace;
    private enum Side { AtPlayerLeft, AtPlayerRight };
    private Side side;

    private VeryBigMonsterController.Status LastStatus;
    private bool isHigherThanPlayer;
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
    public Transform Atk1_5Head;
    public Transform Atk2Head;
    public Transform Atk3Head;
    public Transform Atk4Head;
    public Transform SummonHead;
    public Transform CaptureHead;
    public Transform WeakHead;
    public Transform StopHead;

    [Header("張嘴")]
    [HideInInspector] public bool CanOpenMouth;
    public float DefaultCloseMouthTime;
    private float CloseMouthTime;
    public float MouthTimerSet;
    private float Timer;

    [Header("高嘴砲")]
    [SerializeField] private bool isAtk3Head;
    [SerializeField] private GameObject QuickLightAtk;
    [HideInInspector] public Transform AtkTemporaryArea;
    private float Atk3Timer;
    private float Atk3TimerSet;
    public float WidthLowAngle;
    private VBigMonsterLightAtk LightAtk;
    private Transform _lightAtkTransform;
    private RaycastHit2D GroundCheck;
    private RaycastHit2D SpecialGroundCheck;
    private RaycastHit2D LeftWallCheck;
    private RaycastHit2D RightWallCheck;
    private float InitialRotate = 36.3f;
    private float RotationLimit = 25.7f;
    private float Atk3RotateSpeed = 14.25f;
    private bool JudgeAngleByDefault;
    private bool isReSet;
    private bool Trigger1;
    private bool Trigger2;
    private bool Trigger3;
    private bool Trigger4;
    private bool Trigger5;

    //高嘴砲射線角度比例
    private float RaycastRotation;
    private const float ShouldDecressAngle = 28.3f;
    private float LightLengthProportion = 0.16f;
    private Vector2 LightProportion = new Vector2();

    private void Start()
    {
        _transform = this.transform;
        if (GameObject.Find("player") != null)
        {
            playerTransform = GameObject.Find("player").transform;
        }

        HeadAnimator = _transform.GetChild(0).GetComponent<Animator>();
        SHeadAnimator = _transform.GetChild(1).GetComponent<Animator>();
        Head = _transform.GetChild(0).gameObject;
        SHead = _transform.GetChild(1).gameObject;
        _spr = Head.GetComponent<SpriteRenderer>();
        S_spr = SHead.GetComponent<SpriteRenderer>();
        Timer = MouthTimerSet;
        _controller = this.transform.parent.GetComponent<VeryBigMonsterController>();
        _hurtedController = this.transform.parent.GetComponent<MonsterHurtedController>();
        _HurtedAni = this.transform.parent.GetComponent<VBigMonsterHurted>();
        _basicData = this.transform.parent.GetComponent<MonsterBasicData>();
        _collisionType = this.GetComponent<CollisionType>();
        _objectShield = this.GetComponent<ObjectShield>();
        AtkTemporaryArea = _transform.GetChild(2);

        LightAtk = _transform.GetChild(3).GetComponent<VBigMonsterLightAtk>();
        _lightAtkTransform = LightAtk.transform;
        LastStatus = VeryBigMonsterController.Status.wait;
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
            case VeryBigMonsterController.AniStatus.wait:
                PositionTarget = WaitHead;
                RotationTarget = WaitHead;
                break;
            case VeryBigMonsterController.AniStatus.walk:
                PositionTarget = WalkHead;
                RotationTarget = WalkHead;
                break;
            case VeryBigMonsterController.AniStatus.Dash:
                PositionTarget = WalkHead;
                RotationTarget = WalkHead;
                break;
            case VeryBigMonsterController.AniStatus.Jump:
                PositionTarget = JumpHead;
                RotationTarget = JumpHead;
                break;
            case VeryBigMonsterController.AniStatus.Atk1:
                PositionTarget = AtkHead;
                RotationTarget = AtkHead;
                break;
            case VeryBigMonsterController.AniStatus.Atk1_5:
                PositionTarget = Atk1_5Head;
                RotationTarget = Atk1_5Head;
                break;
            case VeryBigMonsterController.AniStatus.Atk2:
                PositionTarget = Atk2Head;
                RotationTarget = Atk2Head;
                break;
            case VeryBigMonsterController.AniStatus.Atk3:
                PositionTarget = Atk3Head;
                RotationTarget = Atk3Head;
                break;
            case VeryBigMonsterController.AniStatus.Atk4:
                PositionTarget = Atk4Head;
                RotationTarget = Atk4Head;
                break;
            case VeryBigMonsterController.AniStatus.Summon:
                PositionTarget = SummonHead;
                RotationTarget = SummonHead;
                break;
            case VeryBigMonsterController.AniStatus.Capture:
                PositionTarget = CaptureHead;
                RotationTarget = CaptureHead;
                break;
            case VeryBigMonsterController.AniStatus.Weak:
                PositionTarget = WeakHead;
                RotationTarget = WeakHead;
                break;
            case VeryBigMonsterController.AniStatus.Stop:
                PositionTarget = StopHead;
                RotationTarget = StopHead;
                break;
            default:
                PositionTarget = WaitHead;
                RotationTarget = WaitHead;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //指定Target
        switch (_controller.NowAni)
        {
            case VeryBigMonsterController.AniStatus.wait:
                PositionTarget = WaitHead;
                RotationTarget = WaitHead;
                break;
            case VeryBigMonsterController.AniStatus.walk:
                PositionTarget = WalkHead;
                RotationTarget = WalkHead;
                break;
            case VeryBigMonsterController.AniStatus.Dash:
                PositionTarget = WalkHead;
                RotationTarget = WalkHead;
                break;
            case VeryBigMonsterController.AniStatus.Jump:
                PositionTarget = JumpHead;
                RotationTarget = JumpHead;
                break;
            case VeryBigMonsterController.AniStatus.Atk1:
                PositionTarget = AtkHead;
                RotationTarget = AtkHead;
                break;
            case VeryBigMonsterController.AniStatus.Atk1_5:
                PositionTarget = Atk1_5Head;
                RotationTarget = Atk1_5Head;
                break;
            case VeryBigMonsterController.AniStatus.Atk2:
                PositionTarget = Atk2Head;
                RotationTarget = Atk2Head;
                break;
            case VeryBigMonsterController.AniStatus.Atk3:
                PositionTarget = Atk3Head;
                RotationTarget = Atk3Head;
                break;
            case VeryBigMonsterController.AniStatus.Atk4:
                PositionTarget = Atk4Head;
                RotationTarget = Atk4Head;
                break;
            case VeryBigMonsterController.AniStatus.Summon:
                PositionTarget = SummonHead;
                RotationTarget = SummonHead;
                break;
            case VeryBigMonsterController.AniStatus.Capture:
                PositionTarget = CaptureHead;
                RotationTarget = CaptureHead;
                break;
            case VeryBigMonsterController.AniStatus.Weak:
                PositionTarget = WeakHead;
                RotationTarget = WeakHead;
                break;
            case VeryBigMonsterController.AniStatus.Stop:
                PositionTarget = StopHead;
                RotationTarget = StopHead;
                break;
            default:
                PositionTarget = WaitHead;
                RotationTarget = WaitHead;
                break;
        }

        //保護目標判斷
        switch (_controller.status)
        {
            case VeryBigMonsterController.Status.Weak:
                _objectShield.ProtectTarget = _controller.WeakBodyJudgement;
                break;
            case VeryBigMonsterController.Status.Atk3:
                _objectShield.ProtectTarget = _controller.HighBodyJudgement;
                break;
            default:
                _objectShield.ProtectTarget = _controller.BodyJudgement;
                break;
        }

        if (_hurtedController.isCriticAtkHurted && isAtk3Head)
        {
            isReSet = false;
            LightAtk.LongVerReSet = false;
            LightAtk.gameObject.SetActive(false);
            return;
        }

        switch (_basicData.face)
        {
            case MonsterBasicData.Face.Right:
                if (!_controller.isSecondPhase)
                {
                    _spr.flipX = true;
                }
                else
                {
                    S_spr.flipX = true;
                }
                if (lastFace != LastFace.Right)
                {
                    TurnFace();
                    lastFace = LastFace.Right;
                }
                break;
            case MonsterBasicData.Face.Left:
                if (!_controller.isSecondPhase)
                {
                    _spr.flipX = false;
                }
                else
                {
                    S_spr.flipX = false;
                }
                if (lastFace != LastFace.Left)
                {
                    TurnFace();
                    lastFace = LastFace.Left;
                }
                break;
        }
        
        //剛切換時的第一幀不指定位置(應對瞬間偏移bug)
        if(LastStatus == _controller.status)
        {
            _transform.position = PositionTarget.position;
        }
        LastStatus = _controller.status;

        CalculateStandardRotate();

        HurtedControll();

        if (isAtk3Head)
        {
            InitializeAtk3Head();

            if (JudgeAngleByDefault)
            {
                RotateJudge();
            }

            Atk3LightJudge();

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

            return;
        }

        if (RotateFollowBone)
        {
            TargetRotate = RotationTarget.eulerAngles.z;
            if (TargetRotate < 0)
            {
                ExcessMultiple = (int)(RotationTarget.eulerAngles.z / 360);
                ExcessMultiple += 1;
                TargetRotate =  360 * ExcessMultiple + TargetRotate;
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

        Atk3Run();

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
        if (!_controller.isSecondPhase)
        {
            HeadAnimator.SetBool("Open", true);
        }
        else
        {
            SHeadAnimator.SetBool("Open", true);
        }
        if (isAtk3Head)
        {
            if (!_controller.isSecondPhase)
            {
                HeadAnimator.SetBool("Fast", true);
            }
            else
            {
                SHeadAnimator.SetBool("Fast", true);
            }
        }
    }

    public void OpenMouth()//使用預設(1)
    {
        Timer = MouthTimerSet;
        CanOpenMouth = true;
        CloseMouthTime = MouthTimerSet - DefaultCloseMouthTime;
        if (!_controller.isSecondPhase)
        {
            HeadAnimator.SetBool("Open", true);
        }
        else
        {
            SHeadAnimator.SetBool("Open", true);
        }
        if (isAtk3Head)
        {
            if (!_controller.isSecondPhase)
            {
                HeadAnimator.SetBool("Fast", true);
            }
            else
            {
                SHeadAnimator.SetBool("Fast", true);
            }
        }
    }

    private void ControllMouth()//(2)
    {
        if (CanOpenMouth)
        {
            Timer -= _fixDeltaTime;

            if (Timer <= CloseMouthTime)
            {
                if (!_controller.isSecondPhase)
                {
                    HeadAnimator.SetBool("Open", false);
                    HeadAnimator.SetBool("Fast", false);
                }
                else
                {
                    SHeadAnimator.SetBool("Open", false);
                    SHeadAnimator.SetBool("Fast", false);
                }
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
                        if(NowRotate > TargetRotate)
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
        NowRotate = AngleCaculate.AngleDirectionChange(NowRotate);
        _transform.rotation = Quaternion.Euler(0, 0, NowRotate);
    }

    public void InitializeHead(VBMonsterHeadValue Value)//(1)
    {
        RotateFollowBone = Value.RotateFollowBone;
        WidthLowAngle = Value.WidthLowAngle;
        LowAngle = Value.LowAngle;
        HighAngle = Value.HighAngle;
        DefaultCloseMouthTime = Value.CloseMouthTime;
        MouthTimerSet = Value.MouthTimer;
        isAtk3Head = Value.isAtk3Head;

        if (WidthLowAngle != 0)
        {
            UseWidthAngleLimit = true;
        }

        _transform.position = PositionTarget.position;
        
        if (!_controller.isSecondPhase)
        {
            Head.SetActive(true);
            SHead.SetActive(false);
            _collisionType._type = CollisionType.Type.Skin;
        }
        else
        {
            Head.SetActive(false);
            SHead.SetActive(true);
            _collisionType._type = CollisionType.Type.Metal;
        }

        switch (_basicData.face)
        {
            case MonsterBasicData.Face.Left:
                _transform.rotation = Quaternion.Euler(0, 0, AngleCaculate.AngleDirectionChange(StandardNowRotate));
                lastFace = LastFace.Left;
                if (!_controller.isSecondPhase)
                {
                    _spr.flipX = false;
                }
                else
                {
                    S_spr.flipX = false;
                }
                break;
            case MonsterBasicData.Face.Right:
                _transform.rotation = Quaternion.Euler(0, 0, StandardNowRotate);
                lastFace = LastFace.Right;
                if (!_controller.isSecondPhase)
                {
                    _spr.flipX = true;
                }
                else
                {
                    S_spr.flipX = true;
                }
                break;
        }

        if (isAtk3Head)
        {
            isReSet = false;
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
            HeadHurtNumber = _hurtedController.HurtedNumber;
            _hurtedController.HurtedControll(1);
        }

        if (_HurtedAni.isValidHurted)
        {
            if (!_controller.isSecondPhase)
            {
                _spr.color = new Color(0.65f, 0.48f, 0.48f, 1);
            }
            else
            {
                S_spr.color = new Color(0.65f, 0.48f, 0.48f, 1);
            }
        }
        else
        {
            if (!_controller.isSecondPhase)
            {
                _spr.color = new Color(1, 1, 1, 1);
            }
            else
            {
                S_spr.color = new Color(1, 1, 1, 1);
            }
        }
    }

    private void InitializeAtk3Head()//(1)
    {
        if (!isAtk3Head)
        {
            return;
        }

        if (!isReSet)
        {
            Atk3Timer = Atk3TimerSet;
            BeginRotateByBone();
            if (!_controller.isSecondPhase)
            {
                HeadAnimator.SetBool("Open", false);
            }
            else
            {
                SHeadAnimator.SetBool("Open", false);
            }
            Trigger1 = false;
            Trigger2 = false;
            Trigger3 = false;
            Trigger4 = false;
            Trigger5 = false;
            JudgeAngleByDefault = false;
            isReSet = true;
        }
    }

    private void Atk3LightJudge()//(2)
    {
        if (!isAtk3Head)
        {
            return;
        }

        //光線角度
        switch (_basicData.face)
        {
            case MonsterBasicData.Face.Right:
                if (NowRotate < 180 && NowRotate < ShouldDecressAngle)
                {
                    RaycastRotation =  -Mathf.Abs(NowRotate - ShouldDecressAngle) + 360;
                }
                else
                {
                    RaycastRotation = Mathf.Abs(NowRotate - ShouldDecressAngle);
                }
                break;
            case MonsterBasicData.Face.Left:
                RaycastRotation = Mathf.Abs(NowRotate + ShouldDecressAngle);
                RaycastRotation = AngleCaculate.AngleConvert("L", "R", RaycastRotation, 0);
                break;
        }
        
        LightProportion = AngleCaculate.CaculateSpeedRate("R", RaycastRotation);

        //光線長度
        float PointDistance = 0;
        float SGPointDistanceX = 0;
        float SGPointDistanceY = 0;
        float SGPointDistance = 0;
        float GPointDistanceX = 0;
        float GPointDistanceY = 0;
        float GPointDistance = 0;
        float WPointDistanceX = 0;
        float WPointDistanceY = 0;
        float WPointDistance = 0;
        List<float> CompareList = new List<float>();

        switch (_basicData.face)
        {
            case MonsterBasicData.Face.Right:
                SpecialGroundCheck = Physics2D.Raycast(transform.position, LightProportion, 100f, 32768);
                LeftWallCheck = Physics2D.Raycast(transform.position, LightProportion, 100f, 64);
                RightWallCheck = Physics2D.Raycast(transform.position, LightProportion, 100f, 128);
                GroundCheck = Physics2D.Raycast(transform.position, LightProportion, 100f, 1024);
                Debug.DrawRay(_transform.position, LightProportion, Color.white, 1);
                break;
            case MonsterBasicData.Face.Left:
                SpecialGroundCheck = Physics2D.Raycast(transform.position, LightProportion, 100f, 32768);
                LeftWallCheck = Physics2D.Raycast(transform.position, LightProportion, 100f, 64);
                RightWallCheck = Physics2D.Raycast(transform.position, LightProportion, 100f, 128);
                GroundCheck = Physics2D.Raycast(transform.position, LightProportion, 100f, 1024);
                Debug.DrawRay(_transform.position, LightProportion, Color.white, 1);
                break;
        }

        if (GroundCheck)
        {
            GPointDistanceX = Mathf.Abs(GroundCheck.point.x - _transform.position.x);
            GPointDistanceY = Mathf.Abs(GroundCheck.point.y - _transform.position.y);
            GPointDistance = Mathf.Pow(GPointDistanceX * GPointDistanceX + GPointDistanceY * GPointDistanceY, 0.5f);
            CompareList.Add(GPointDistance);
        }
        if (SpecialGroundCheck)
        {
            SGPointDistanceX = Mathf.Abs(SpecialGroundCheck.point.x - _transform.position.x);
            SGPointDistanceY = Mathf.Abs(SpecialGroundCheck.point.y - _transform.position.y);
            SGPointDistance = Mathf.Pow(SGPointDistanceX * SGPointDistanceX + SGPointDistanceY * SGPointDistanceY, 0.5f);
            CompareList.Add(SGPointDistance);
        }
        if (LeftWallCheck)
        {
            WPointDistanceX = Mathf.Abs(LeftWallCheck.point.x - _transform.position.x);
            WPointDistanceY = Mathf.Abs(LeftWallCheck.point.y - _transform.position.y);
            WPointDistance = Mathf.Pow(WPointDistanceX * WPointDistanceX + WPointDistanceY * WPointDistanceY, 0.5f);
            CompareList.Add(WPointDistance);
        }
        if (RightWallCheck)
        {
            WPointDistanceX = Mathf.Abs(RightWallCheck.point.x - _transform.position.x);
            WPointDistanceY = Mathf.Abs(RightWallCheck.point.y - _transform.position.y);
            WPointDistance = Mathf.Pow(WPointDistanceX * WPointDistanceX + WPointDistanceY * WPointDistanceY, 0.5f);
            CompareList.Add(WPointDistance);
        }

        if (CompareList.Count > 0)
        {
            CompareList.Sort();
            PointDistance = CompareList[0];
        }

        if (LightAtk.LongVerBegin)
        {
            _lightAtkTransform.localScale = new Vector3(PointDistance * LightLengthProportion, _lightAtkTransform.localScale.y, 0);
        }
        else
        {
            LightAtk.LongVerLong = PointDistance * LightLengthProportion;
        }
    }

    private void Atk3Run()//(2)
    {
        if (!isAtk3Head)
        {
            return;
        }

        Atk3Timer -= _fixDeltaTime;

        if (Atk3Timer <= (Atk3TimerSet - 1.2))
        {
            if (!Trigger1)
            {
                StopRotateByBone();
                OpenMouth(5.05f, 4.8f);
                LightAtk.gameObject.SetActive(true);
                _lightAtkTransform.localScale = new Vector3(0, _lightAtkTransform.localScale.y, 0);
                switch (_basicData.face)
                {
                    case MonsterBasicData.Face.Right:
                        TargetRotate = 360 - InitialRotate;
                        _lightAtkTransform.rotation = Quaternion.Euler(0, 0, _transform.eulerAngles.z - ShouldDecressAngle);
                        break;
                    case MonsterBasicData.Face.Left:
                        TargetRotate = InitialRotate;
                        _lightAtkTransform.rotation = Quaternion.Euler(0, 0, _transform.eulerAngles.z + ShouldDecressAngle + 180);
                        break;
                }
                Trigger1 = true;
            }
        }
        if (Atk3Timer <= (Atk3TimerSet - 1.65))
        {
            if (!Trigger2)
            {
                switch (_basicData.face)
                {
                    case MonsterBasicData.Face.Right:
                        TargetRotate = RotationLimit;
                        break;
                    case MonsterBasicData.Face.Left:
                        TargetRotate = 360 - RotationLimit;
                        break;
                }
                RotateSpeedSet(Atk3RotateSpeed);
                Trigger2 = true;
            }
        }
        if (Atk3Timer <= (Atk3TimerSet - 6))
        {
            if (!Trigger3)
            {
                BeginRotateByBone();
                RotateSpeedSet(FollowBoneSpeed);
                Trigger3 = true;
            }
        }
        if (Atk3Timer <= (Atk3TimerSet - 6.85))
        {
            if (!Trigger4)
            {
                StopRotateByBone();
                RotateSpeedSet(FollowBoneSpeed);
                JudgeAngleByDefault = true;
            }
            if (!Trigger5)
            {
                OpenMouth(0.9f, 0.65f);
                Trigger5 = true;
            }
        }
        if (Atk3Timer <= (Atk3TimerSet - 7.2))
        {
            if (!Trigger4)
            {
                Instantiate(QuickLightAtk, AtkTemporaryArea.position, Quaternion.identity, AtkTemporaryArea);
                AtkTemporaryArea.DetachChildren();
                JudgeAngleByDefault = false;
                RotateSpeedSet(0);
                Trigger4 = true;
            }
        }
        if (Atk3Timer <= (Atk3TimerSet - 7.8))
        {
            BeginRotateByBone();
        }
    }

    public void ChangePhase()//(1)
    {
        Head.SetActive(false);
        SHead.SetActive(true);
        _collisionType._type = CollisionType.Type.Metal;
    }
}

public struct VBMonsterHeadValue
{
    public bool RotateFollowBone;
    public float WidthLowAngle;
    public float LowAngle;
    public float HighAngle;
    public float CloseMouthTime;
    public float MouthTimer;
    public bool isAtk3Head;

    public VBMonsterHeadValue(bool Bone, float WLA, float Low, float High, float CLT, float MT, bool Atk3)
    {
        RotateFollowBone = Bone;
        WidthLowAngle = WLA;
        LowAngle = Low;
        HighAngle = High;
        CloseMouthTime = CLT;
        MouthTimer = MT;
        isAtk3Head = Atk3;
    }
}
