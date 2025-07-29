using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VeryBigMonsterController : MonoBehaviour
{
    [Header("基本參數")]
    public float CloseAtkDistance;
    public float VeryCloseAtkDistance;
    public float WalkSpeed;
    public float DashSpeed;
    public float WallCheckDistance;
    public enum Status { wait, walk, Dash, Jump, Atk1, Atk1_5, Atk2,  Atk3, Atk4, Summon, Capture, CaptureSuccess, Weak};
    [HideInInspector] public Status status;
    private enum LastStatus { wait, walk, Dash, Jump, Atk1, Atk1_5, Atk2,  Atk3, Atk4, Summon, Capture};
    private LastStatus lastStatus;
    private enum DashMode {BackDash, Dash , DashEnd, Default};
    private DashMode dashMode;
    private enum Atk1Mode { Default, FirstAtk, FirstEnd, FirstContinue, SecondEnd, SecondContinue };
    private Atk1Mode atk1Mode;
    public enum AniStatus { wait, walk, Dash, Jump, Atk1, Atk1_5, Atk2, Atk3, Atk4, Summon, Capture, Weak, Stop, Begining}
    [HideInInspector] public AniStatus NowAni;//script(Hurted)
    private AniStatus LastAni;

    [Header("系統參數")]
    [SerializeField] private Boss1RoomController _roomController;
    [HideInInspector] public Transform BodyJudgement;
    [HideInInspector] public Transform WeakBodyJudgement;
    [HideInInspector] public Transform HighBodyJudgement;
    private Transform AtkTemporaryArea;
    private Transform _transform;
    private MonsterCaptureController _captureController;
    private MonsterBasicData _basicData;
     ParabolaVar _parabolaVar = new ParabolaVar();
    private VBMonsterSE _VBMonsterSE;
    private AniMethod _aniMethod;
    private Vector3 MonsterLeftSize = new Vector3(-0.58f, 0.58f, 0);
    private Vector3 MonsterRightSize = new Vector3(0.58f, 0.58f, 0);
    private float _time;
    private float _deltaTime;
    private float _fixedDeltaTime;
    [SerializeField] private float MonsterBaseLine;
    [SerializeField] private float MapMiddleLine;
    public float WalkTimerSet;
    public float DashTimerSet;
    public float BackDashTimerSet;
    private float BackDashSpeed = 10;
    private float DashRepectTimeSet = 0.45f;
    private float DashRepectTime = 0.45f;
    public float DashEndTimerSet;
    private float WalkTimer;
    public float Atk1TimerSet;
    private float AtkTimer;
    public float Atk1_5TimerSet;
    public float Atk2TimerSet;
    public float Atk3TimerSet;
    public float Atk4TimerSet;
    public float SummonTimerSet;
    public float CaptureAtkTimerSet;
    private float WeakTimer;
    public float WeakTimerSet;
    public float JumpTimerSet;
    private float WaitTimerSet = 0.3f;//0.3
    private float WaitTimer;
    private MonsterHurtedController _hurtedController;
    public float CaptureAtkCoolDown;
    private float CaptureAtkLastTime = -60;
    public float SummonCoolDown;
    private float SummonLastTime = -60;
    private float GroundPointY;
    private RaycastHit2D LeftBackAtkCheck;
    private RaycastHit2D RightBackAtkCheck;
    private Transform SummonHowlTarget;
    private float Atk1CheckPoint;
    private float Atk1Distance;//玩家與一技檢查點距離

    //開關
    private bool isDashJudge;
    private bool isGetUp;
    private bool JumpEnd;
    [HideInInspector] public bool isSecondPhase = false;//script(VBMonsterSwitchPhase，hurted)
    private bool ParabolaCaculate;
    private bool FirstTrigger;
    private bool SecondTrigger;
    private bool ThirdTrigger;
    private bool ForthTrigger;
    private bool StartCalculateDashTime;
    [HideInInspector] public bool DashMove = false;//script(WalkAtk)
    private bool CanUseAtk4;
    private bool FirstAtk4;
    [HideInInspector] public bool Atk4GetHurtedByExplosion;//script(判定框)
    private bool CanSummon;
    private bool CanCapture;
    private bool CloseToLeftWall;
    private bool CloseToRightWall;
    private bool DefineStatus;
    [HideInInspector] public bool SwitchPhaseing;
    private bool Boss1RoomEventJudge;//額外獨立是為了避免頭部script沒跑完整

    [Header("動畫相關物件")]
    private Animator MonsterJumpAni;
    private Animator MonsterAtk1Ani;
    private Animator SMonsterWeakAni;
    private Animator MonsterWalkAni;
    private Animator MonsterCaptureAni;
    private GameObject MonsterWaitAnimation;
    private GameObject MonsterWalkAnimation;
    private GameObject MonsterJumpAnimation;
    private GameObject MonsterAtkAnimation;
    private GameObject MonsterAtk1_5Animation;
    private GameObject MonsterAtk2Animation;
    private GameObject MonsterAtk3Animation;
    private GameObject MonsterAtk4Animation;
    private GameObject MonsterSummonAnimation;
    private GameObject MonsterCaptureAnimation;
    private GameObject SMonsterSecondPhaseAnimation;
    private GameObject SMonsterWeakAnimation;
    private GameObject MonsterBeginAnimation;
    private Transform MonsterWaitTransform;
    private Transform MonsterWalkTransform;
    private Transform MonsterJumpTransform;
    private Transform MonsterAtkTransform;
    private Transform MonsterAtk1_5Transform;
    private Transform MonsterAtk2Transform;
    private Transform MonsterAtk3Transform;
    private Transform MonsterAtk4Transform;
    private Transform MonsterSummonTransform;
    private Transform MonsterCaptureTransform;
    private Transform SMonsterWeakTransform;
    private Transform SMonsterSwitchPhaseTransform;
    private GameObject Atk4Arm;
    private GameObject Atk4SArm;
    private GameObject Atk4DownBody;
    private GameObject Atk4SDownBody;
    public GameObject DisappearLight;

    //頭
    private VBMonsterHead Head;
    private VBMonsterHeadValue WaitHead = new VBMonsterHeadValue(false, 0, 310, 34, 0, 0, false);
    private VBMonsterHeadValue WalkHead = new VBMonsterHeadValue(true, 0, 310, 34, 0, 0, false);
    private VBMonsterHeadValue JumpHead = new VBMonsterHeadValue(true, 0, 310, 34, 0, 0, false);
    private VBMonsterHeadValue AtkHead = new VBMonsterHeadValue(false, 0, 310, 34, 0.75f, 1, false);
    private VBMonsterHeadValue Atk1_5Head = new VBMonsterHeadValue(false, 0, 310, 34, 0, 0, false);
    private VBMonsterHeadValue Atk2Head = new VBMonsterHeadValue(true, 0, 310, 34, 0.6f, 0.85f, false);
    private VBMonsterHeadValue Atk3Head = new VBMonsterHeadValue(true, 239, 310, 34, 0, 0, true);
    private VBMonsterHeadValue Atk4Head = new VBMonsterHeadValue(true, 0, 310, 34, 4.95f, 5.7f, false);
    private VBMonsterHeadValue SummonHead = new VBMonsterHeadValue(true, 0, 310, 34, 2.75f, 3f, false);
    private VBMonsterHeadValue CaptureHead = new VBMonsterHeadValue(true, 0, 310, 34, 0, 0, false);
    private VBMonsterHeadValue WeakHead = new VBMonsterHeadValue(true, 0, 310, 34, 0, 0, false);
    private VBMonsterHeadValue StopHead = new VBMonsterHeadValue(true, 0, 310, 34, 0, 0, false);

    [Header("攻擊物件")]
    public GameObject WalkAtk;
    private GameObject _walkAtk;
    public GameObject JumpAtk;
    public GameObject RAtk1;
    public GameObject LAtk1;
    public GameObject RAtk1_5;
    public GameObject LAtk1_5;
    public GameObject QuickLightAtk;
    public GameObject RAtk2;
    public GameObject LAtk2;
    public GameObject RAtk4;
    public GameObject LAtk4;
    public GameObject Assemble;
    public GameObject ShockWave;
    public GameObject Howl;
    public GameObject Phase1Drone;
    public GameObject Phase2Drone;
    public GameObject RCaptureAtk;
    public GameObject LCaptureAtk;
    private Vector3 LAtk2Appear = new Vector3(-2.337f, -1.315f, 0);
    private Vector3 RAtk2Appear = new Vector3(2.337f, -1.315f, 0);
    private Vector3 LAtk4Appear = new Vector3(-0.75f, -0.41f, 0);
    private Vector3 RAtk4Appear = new Vector3(0.75f, -0.41f, 0);

    [Header("被大招攻擊")]
    private GameObject MonsterStopAnimation;
    private Animator MonsterStopAni;
    private Transform MonsterStopTr;
    private bool Atk4Interrupted;

    void Start()
    {
        //抓取動畫物件
        MonsterWaitAnimation = this.transform.GetChild(0).gameObject;
        MonsterWalkAnimation = this.transform.GetChild(1).gameObject;
        MonsterJumpAnimation = this.transform.GetChild(2).gameObject;
        MonsterAtkAnimation = this.transform.GetChild(3).gameObject;
        MonsterAtk1_5Animation = this.transform.GetChild(4).gameObject;
        MonsterAtk2Animation = this.transform.GetChild(5).gameObject;
        MonsterAtk3Animation = this.transform.GetChild(6).gameObject;
        MonsterAtk4Animation = this.transform.GetChild(7).gameObject;
        MonsterSummonAnimation = this.transform.GetChild(8).gameObject;
        MonsterCaptureAnimation = this.transform.GetChild(9).gameObject;
        SMonsterWeakAnimation = this.transform.GetChild(10).gameObject;
        MonsterStopAnimation = this.transform.GetChild(11).gameObject;
        SMonsterSecondPhaseAnimation = this.transform.GetChild(12).gameObject;

        Head = this.transform.GetChild(18).GetComponent<VBMonsterHead>();

        MonsterWaitTransform = MonsterWaitAnimation.transform.GetChild(0);
        MonsterWalkTransform = MonsterWalkAnimation.transform.GetChild(0);
        MonsterJumpTransform = MonsterJumpAnimation.transform.GetChild(0);
        MonsterAtkTransform = MonsterAtkAnimation.transform.GetChild(0);
        MonsterAtk1_5Transform = MonsterAtk1_5Animation.transform.GetChild(0);
        MonsterAtk2Transform = MonsterAtk2Animation.transform.GetChild(0);
        MonsterAtk3Transform = MonsterAtk3Animation.transform.GetChild(0);
        MonsterAtk4Transform = MonsterAtk4Animation.transform.GetChild(0);
        MonsterSummonTransform = MonsterSummonAnimation.transform.GetChild(0);
        MonsterCaptureTransform = MonsterCaptureAnimation.transform.GetChild(0);
        SMonsterWeakTransform = SMonsterWeakAnimation.transform.GetChild(0);
        MonsterStopTr = MonsterStopAnimation.transform.GetChild(0);
        SMonsterSwitchPhaseTransform = SMonsterSecondPhaseAnimation.transform;

        Atk4Arm = MonsterAtk4Transform.GetChild(3).gameObject;
        Atk4SArm = MonsterAtk4Transform.GetChild(4).gameObject;
        Atk4DownBody = MonsterAtk4Transform.GetChild(13).gameObject;
        Atk4SDownBody = MonsterAtk4Transform.GetChild(14).gameObject;

        MonsterJumpAni = MonsterJumpTransform.GetComponent<Animator>();
        MonsterWalkAni = MonsterWalkTransform.GetComponent<Animator>();
        MonsterAtk1Ani = MonsterAtkTransform.GetComponent<Animator>();
        SMonsterWeakAni = SMonsterWeakTransform.GetComponent<Animator>();
        MonsterCaptureAni = MonsterCaptureTransform.GetComponent<Animator>();
        MonsterStopAni = MonsterStopTr.GetComponent<Animator>();

        AtkTemporaryArea = this.transform.GetChild(14);
        BodyJudgement = this.transform.GetChild(15);
        WeakBodyJudgement = this.transform.GetChild(16);
        HighBodyJudgement = this.transform.GetChild(17);
        MonsterBeginAnimation = this.transform.GetChild(13).gameObject;
        AtkTemporaryArea.DetachChildren();

        if (GameObject.Find("FollowSystem") != null)
        {
            _aniMethod = GameObject.Find("FollowSystem").GetComponent<AniMethod>();
        }

        SummonHowlTarget = Head.transform;

        WaitTimer = WaitTimerSet;
        WeakTimer = WeakTimerSet;
        WalkTimer = WalkTimerSet;
        DashRepectTime = DashRepectTimeSet;

        status = Status.wait;
        if (_roomController != null)
        {
            NowAni = AniStatus.Begining;
        }
        else
        {
            NowAni = AniStatus.wait;
        }

        atk1Mode = Atk1Mode.Default;
        dashMode = DashMode.Default;

        _captureController = this.transform.GetComponent<MonsterCaptureController>();
        _hurtedController = this.transform.GetComponent<MonsterHurtedController>();
        _basicData = this.GetComponent<MonsterBasicData>();
        _VBMonsterSE = this.GetComponent<VBMonsterSE>();
        _hurtedController._getCriticHurted += ResetValue;
        _hurtedController._getCriticHurted += PlayStopAni;
        _hurtedController._reactCriticHurted += StopAniCnange;

        _basicData.BasicVarInisialize(MonsterWaitTransform, "R");

        _transform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        _time = Time.time;
        _deltaTime = Time.deltaTime;
        if (_basicData.hp <= 0 && _basicData.isGround)
        {
            _basicData.ConfirmDie();
            if (_roomController != null)
            {
                Boss1RoomController._deadInfo = _basicData.RecordCompleteDeadInfo(new Vector3(_transform.position.x, MonsterBaseLine, 0));
            }
            return;
        }
        if (_basicData.hp <= (_basicData.maxHp * 0.6))
        {
            if (!FirstAtk4)
            {
                CanUseAtk4 = true;
                FirstAtk4 = true;
            }
        }

        DetectAtk4Interrupted();

        _hurtedController.CriticAtkHurtedTimerMethod(_deltaTime);
        _hurtedController.HurtedTimerMethod(_deltaTime);

        //判斷是否觸地
        _basicData.CheckTouchGround(ref GroundPointY);
        //判斷是否碰到牆壁
        _basicData.CheckTouchWall();
        LeftBackAtkCheck = Physics2D.Raycast(_transform.position, Vector2.left, WallCheckDistance, 64);
        if (LeftBackAtkCheck)
        {
            CloseToLeftWall = true;
        }
        else
        {
            CloseToLeftWall = false;
        }
        RightBackAtkCheck = Physics2D.Raycast(_transform.position, Vector2.right, WallCheckDistance, 128);
        if (RightBackAtkCheck)
        {
            CloseToRightWall = true;
        }
        else
        {
            CloseToRightWall = false;
        }

        //王房特殊事件調整
        if (!Boss1RoomEventJudge && _roomController != null)
        {
            if (!GameEvent.GoInBoss1)
            {
                AllAniFalse();
                Head.gameObject.SetActive(false);
                this.gameObject.SetActive(false);
            }
            else
            {
                NowAni = AniStatus.wait;
                AllAniFalse();
            }
            Boss1RoomEventJudge = true;
        }

        //限制Y軸範圍
        if (status != Status.Jump)
        {
            _transform.position = new Vector3(_transform.position.x, MonsterBaseLine, 0);
        }
        if (status == Status.Jump && _transform.position.y < MonsterBaseLine)
        {
            _transform.position = new Vector3(_transform.position.x, MonsterBaseLine, 0);
        }

        //寫入實際位置
        _basicData.MonsterPlace = _transform.position;
        
        //return
        if (GameEvent.isAniPlay || _hurtedController.isCriticAtkHurted || PauseMenuController.isPauseMenuOpen)
        {
            return;
        }
        if(_roomController!= null && _roomController.Phase != 2)
        {
            return;
        }

        //計算距離
        _basicData.DistanceCalculate();
        //冷卻時間計算
        _basicData.CoolDownCalculate(_time, CaptureAtkLastTime, CaptureAtkCoolDown, ref CanCapture);
        _basicData.CoolDownCalculate(_time, SummonLastTime, SummonCoolDown, ref CanSummon);
        //被大招打之後的動作
        if (_hurtedController.BeCriticAtkEnd)
        {
            Head.gameObject.SetActive(true);
            _hurtedController.BeCriticAtkEnd = false;
            if (status != Status.Weak)
            {
                status = Status.wait;
            }
        }
        
        //怪物AI
        switch (status)
        {
            case Status.wait:
                WaitTimer -= _deltaTime;
                
                if (WaitTimer <= 0)
                {
                    StatusJudge();
                    AtkTimer = 0;//預防沒重置的狀況
                    WaitTimer = WaitTimerSet;
                }
                break;
            case Status.walk:

                if (_basicData.AbsDistanceX < CloseAtkDistance)
                {
                    WalkTimer = WalkTimerSet;
                    FirstTrigger = false;
                    StatusJudge();
                }

                WalkTimer -= _deltaTime;

                if (WalkTimer <= 0)
                {
                    WalkTimer = WalkTimerSet;
                    FirstTrigger = false;
                    StatusJudge();
                }
                break;
            case Status.Dash:
                if (!isDashJudge && AtkTimer <= 0)
                {
                    if(dashMode == DashMode.Default)
                    {
                        if (_transform.localPosition.x >= MapMiddleLine)
                        {
                            _basicData.face = MonsterBasicData.Face.Left;
                            if (CloseToRightWall)
                            {
                                dashMode = DashMode.Dash;
                            }
                            else
                            {
                                dashMode = DashMode.BackDash;
                            }
                        }
                        if (_transform.localPosition.x < MapMiddleLine)
                        {
                            _basicData.face = MonsterBasicData.Face.Right;
                            if (CloseToLeftWall)
                            {
                                dashMode = DashMode.Dash;
                            }
                            else
                            {
                                dashMode = DashMode.BackDash;
                            }
                        }
                    }

                    switch (dashMode)
                    {
                        case DashMode.Dash:
                            AtkTimer = DashTimerSet;
                            break;
                        case DashMode.BackDash:
                            AtkTimer = BackDashTimerSet;
                            break;
                        case DashMode.DashEnd:
                            AtkTimer = DashEndTimerSet;
                            break;
                    }

                    isDashJudge = true;
                }

                if (DashRepectTime <= 0)
                {
                    switch (_basicData.face)
                    {
                        case MonsterBasicData.Face.Left:
                            if (CloseToLeftWall)
                            {
                                isDashJudge = false;
                                StartCalculateDashTime = false;
                                dashMode = DashMode.DashEnd;
                                DashRepectTime = DashRepectTimeSet;
                            }
                            else
                            {
                                DashRepectTime = DashRepectTimeSet;
                            }
                            break;
                        case MonsterBasicData.Face.Right:
                            if (CloseToRightWall)
                            {
                                isDashJudge = false;
                                StartCalculateDashTime = false;
                                dashMode = DashMode.DashEnd;
                                DashRepectTime = DashRepectTimeSet;
                            }
                            else
                            {
                                DashRepectTime = DashRepectTimeSet;
                            }
                            break;
                    }
                }
                break;
            case Status.Capture:
                if (_captureController.isCaptureSuccess)
                {
                    CanCapture = false;
                    CaptureAtkLastTime = _time;
                    FirstTrigger = false;
                    status = Status.CaptureSuccess;
                    return;
                }
                break;
            case Status.CaptureSuccess:
                switch (_basicData.face)
                {
                    case MonsterBasicData.Face.Right:
                        _captureController.FaceLeft = false;
                        _captureController.FaceRight = true;
                        break;
                    case MonsterBasicData.Face.Left:
                        _captureController.FaceLeft = true;
                        _captureController.FaceRight = false;
                        break;
                }
                if (_captureController.CaptureAtkEnd)
                {
                    AtkTimer = 0;
                    ReturnToWait();
                    lastStatus = LastStatus.Capture;
                    _captureController.CaptureAtkEnd = false;
                }
                break;
        }
        
        if (status != Status.Weak && status != Status.Atk3)
        {
            BodyJudgement.gameObject.SetActive(true);
        }
        //轉向控制
        switch (_basicData.face)
        {
            case MonsterBasicData.Face.Left:
                MonsterWaitTransform.localScale = MonsterLeftSize;
                MonsterWalkTransform.localScale = MonsterLeftSize;
                MonsterAtk2Transform.localScale = MonsterLeftSize;
                MonsterJumpTransform.localScale = MonsterLeftSize;
                MonsterAtkTransform.localScale = MonsterLeftSize;
                MonsterAtk1_5Transform.localScale = MonsterLeftSize;
                MonsterAtk3Transform.localScale = MonsterLeftSize;
                MonsterAtk4Transform.localScale = MonsterLeftSize;
                SMonsterWeakTransform.localScale = MonsterLeftSize;
                MonsterSummonTransform.localScale = MonsterLeftSize;
                MonsterCaptureTransform.localScale = MonsterLeftSize;
                MonsterStopTr.localScale = MonsterLeftSize;
                SMonsterSwitchPhaseTransform.localScale = new Vector3(-1, 1, 0);
                switch (status)
                {
                    case Status.Atk3:
                        HighBodyJudgement.localScale = new Vector3(-1, 1, 1);
                        break;
                    case Status.Weak:
                        WeakBodyJudgement.localScale = new Vector3(-1, 1, 1);
                        break;
                    default:
                        BodyJudgement.localScale = new Vector3(-1, 1, 1);
                        break;
                }
                break;
            case MonsterBasicData.Face.Right:
                MonsterWaitTransform.localScale = MonsterRightSize;
                MonsterWalkTransform.localScale = MonsterRightSize;
                MonsterAtk2Transform.localScale = MonsterRightSize;
                MonsterJumpTransform.localScale = MonsterRightSize;
                MonsterAtkTransform.localScale = MonsterRightSize;
                MonsterAtk1_5Transform.localScale = MonsterRightSize;
                MonsterAtk3Transform.localScale = MonsterRightSize;
                MonsterAtk4Transform.localScale = MonsterRightSize;
                MonsterSummonTransform.localScale = MonsterRightSize;
                MonsterCaptureTransform.localScale = MonsterRightSize;
                SMonsterWeakTransform.localScale = MonsterRightSize;
                MonsterStopTr.localScale = MonsterRightSize;
                SMonsterSwitchPhaseTransform.localScale = new Vector3(1, 1, 0);
                switch (status)
                {
                    case Status.Atk3:
                        HighBodyJudgement.localScale = new Vector3(1, 1, 1);
                        break;
                    case Status.Weak:
                        WeakBodyJudgement.localScale = new Vector3(1, 1, 1);
                        break;
                    default:
                        BodyJudgement.localScale = new Vector3(1, 1, 1);
                        break;
                }
                break;
        }
        SwitchAnimation();
    }

    private void FixedUpdate()
    {
        _fixedDeltaTime = Time.fixedDeltaTime;

        //return
        if (GameEvent.isAniPlay || _hurtedController.isCriticAtkHurted)
        {
            return;
        }
        if (_roomController != null && _roomController.Phase != 2)
        {
            return;
        }

        //執行動作
        switch (status)
        {
            case Status.walk:
                switch (_basicData.face)
                {
                    case MonsterBasicData.Face.Right:
                        if (!_basicData.touchRightWall)
                        {
                            _transform.localPosition = new Vector3(_transform.localPosition.x + WalkSpeed * _fixedDeltaTime, _transform.localPosition.y, 0);
                        }
                        break;
                    case MonsterBasicData.Face.Left:
                        if (!_basicData.touchLeftWall)
                        {
                            _transform.localPosition = new Vector3(_transform.localPosition.x - WalkSpeed * _fixedDeltaTime, _transform.localPosition.y, 0);
                        }
                        break;
                }
                break;
            case Status.Atk1:
                if (atk1Mode == Atk1Mode.Default)
                {
                    _basicData.TurnFaceJudge();
                    atk1Mode = Atk1Mode.FirstAtk;
                    switch (_basicData.face)
                    {
                        case MonsterBasicData.Face.Left:
                            Atk1CheckPoint = _transform.localPosition.x - 5.45f;
                            break;
                        case MonsterBasicData.Face.Right:
                            Atk1CheckPoint = _transform.localPosition.x + 5.45f;
                            break;
                    }
                    AtkTimer = Atk1TimerSet;
                }

                AtkTimer -= _fixedDeltaTime;

                if (AtkTimer <= (Atk1TimerSet - 0.8))
                {
                    if (!FirstTrigger)
                    {
                        switch (_basicData.face)
                        {
                            case MonsterBasicData.Face.Left:
                                Instantiate(LAtk1, _transform.position, Quaternion.identity);
                                break;
                            case MonsterBasicData.Face.Right:
                                Instantiate(RAtk1, _transform.position, Quaternion.identity);
                                break;
                        }
                        FirstTrigger = true;
                    }
                }
                if (AtkTimer <= (Atk1TimerSet - 1.05))
                {
                    if (atk1Mode == Atk1Mode.FirstAtk)
                    {
                        Atk1Distance = Atk1CheckPoint - _basicData.playerTransform.localPosition.x;
                        Atk1Distance = Mathf.Abs(Atk1Distance);
                        switch (Random.Range(0, 3))
                        {
                            case 0:
                                atk1Mode = Atk1Mode.FirstContinue;
                                if (Atk1Distance > 5.5f)
                                {
                                    atk1Mode = Atk1Mode.FirstEnd;
                                }
                                break;
                            case 1:
                                atk1Mode = Atk1Mode.FirstContinue;
                                if (Atk1Distance > 5.5f)
                                {
                                    atk1Mode = Atk1Mode.FirstEnd;
                                }
                                break;
                            case 2:
                                atk1Mode = Atk1Mode.FirstEnd;
                                break;
                        }
                    }
                }
                //初次攻擊就結束
                if (AtkTimer <= (Atk1TimerSet - 1.55))
                {
                    if (atk1Mode == Atk1Mode.FirstEnd)
                    {
                        FirstTrigger = false;
                        SecondTrigger = false;
                        ThirdTrigger = false;
                        ForthTrigger = false;
                        AtkTimer = 0;
                        lastStatus = LastStatus.Atk1;
                        ReturnToWait();
                        atk1Mode = Atk1Mode.Default;
                    }
                }
                //二次攻擊
                if (AtkTimer <= (Atk1TimerSet - 1.6))
                {
                    if (atk1Mode == Atk1Mode.FirstContinue)
                    {
                        if (!SecondTrigger)
                        {
                            switch (_basicData.face)
                            {
                                case MonsterBasicData.Face.Left:
                                    Instantiate(LAtk1_5, _transform.position, Quaternion.identity);
                                    break;
                                case MonsterBasicData.Face.Right:
                                    Instantiate(RAtk1_5, _transform.position, Quaternion.identity);
                                    break;
                            }
                            SecondTrigger = true;
                        }
                    }
                }

                if (AtkTimer <= (Atk1TimerSet - 1.8))
                {
                    if (atk1Mode == Atk1Mode.FirstContinue)
                    {
                        Atk1Distance = Atk1CheckPoint - _basicData.playerTransform.localPosition.x;
                        Atk1Distance = Mathf.Abs(Atk1Distance);
                        switch (Random.Range(0, 3))
                        {
                            case 0:
                                if (isSecondPhase)
                                {
                                    atk1Mode = Atk1Mode.SecondContinue;
                                    if (Atk1Distance > 5.5f)
                                    {
                                        atk1Mode = Atk1Mode.SecondEnd;
                                    }
                                }
                                else
                                {
                                    atk1Mode = Atk1Mode.SecondEnd;
                                }
                                break;
                            case 1:
                                if (isSecondPhase)
                                {
                                    atk1Mode = Atk1Mode.SecondContinue;
                                    if (Atk1Distance > 5.5f)
                                    {
                                        atk1Mode = Atk1Mode.SecondEnd;
                                    }
                                }
                                else
                                {
                                    atk1Mode = Atk1Mode.SecondEnd;
                                }
                                break;
                            case 2:
                                atk1Mode = Atk1Mode.SecondEnd;
                                break;
                        }
                    }
                }
                //二次攻擊結束
                if (AtkTimer <= (Atk1TimerSet - 2.4))
                {
                    if (atk1Mode == Atk1Mode.SecondEnd)
                    {
                        FirstTrigger = false;
                        SecondTrigger = false;
                        ThirdTrigger = false;
                        ForthTrigger = false;
                        AtkTimer = 0;
                        lastStatus = LastStatus.Atk1;
                        ReturnToWait();
                        atk1Mode = Atk1Mode.Default;
                    }
                }

                //三次攻擊
                if (AtkTimer <= (Atk1TimerSet - 2.05))
                {
                    if (atk1Mode == Atk1Mode.SecondContinue)
                    {
                        if (!ForthTrigger)
                        {
                            Head.OpenMouth();
                            ForthTrigger = true;
                        }
                    }
                }
                if (AtkTimer <= (Atk1TimerSet - 2.3))
                {
                    if (atk1Mode == Atk1Mode.SecondContinue)
                    {
                        if (!ThirdTrigger)
                        {
                            Instantiate(QuickLightAtk, Head.AtkTemporaryArea.position, QuickLightAtk.transform.rotation, Head.AtkTemporaryArea);
                            Head.AtkTemporaryArea.DetachChildren();
                            ThirdTrigger = true;
                        }
                    }
                }
                if (AtkTimer <= 0 && atk1Mode == Atk1Mode.SecondContinue)
                {
                    FirstTrigger = false;
                    SecondTrigger = false;
                    ThirdTrigger = false;
                    ForthTrigger = false;
                    lastStatus = LastStatus.Atk1;
                    ReturnToWait();
                    atk1Mode = Atk1Mode.Default;
                }
                break;
            case Status.Atk1_5:
                if (AtkTimer <= 0)
                {
                    _basicData.TurnFaceJudge();
                    AtkTimer = Atk1_5TimerSet;
                }

                AtkTimer -= _fixedDeltaTime;

                if (AtkTimer <= (Atk1_5TimerSet - 0.55))
                {
                    if (!FirstTrigger)
                    {
                        switch (_basicData.face)
                        {
                            case MonsterBasicData.Face.Left:
                                Instantiate(LAtk1_5, _transform.position, Quaternion.identity);
                                break;
                            case MonsterBasicData.Face.Right:
                                Instantiate(RAtk1_5, _transform.position, Quaternion.identity);
                                break;
                        }
                        FirstTrigger = true;
                    }
                    if (AtkTimer <= 0)
                    {
                        FirstTrigger = false;
                        lastStatus = LastStatus.Atk1_5;
                        ReturnToWait();
                    }
                }
                break;
            case Status.Atk2:
                if (AtkTimer <= 0)
                {
                    _basicData.TurnFaceJudge();
                    AtkTimer = Atk2TimerSet;
                }

                AtkTimer -= _fixedDeltaTime;

                if (AtkTimer <= (Atk2TimerSet - 0.4))
                {
                    if (!SecondTrigger)
                    {
                        Head.OpenMouth();
                        SecondTrigger = true;
                    }
                }
                if (AtkTimer <= (Atk2TimerSet - 0.68))
                {
                    if (!FirstTrigger)
                    {
                        switch (_basicData.face)
                        {
                            case MonsterBasicData.Face.Left:
                                Instantiate(LAtk2, _transform.position + LAtk2Appear, Quaternion.identity);
                                break;
                            case MonsterBasicData.Face.Right:
                                Instantiate(RAtk2, _transform.position + RAtk2Appear, Quaternion.identity);
                                break;
                        }
                        FirstTrigger = true;
                    }
                    if (AtkTimer <= 0)
                    {
                        FirstTrigger = false;
                        SecondTrigger = false;
                        lastStatus = LastStatus.Atk2;
                        ReturnToWait();
                    }
                }
                break;
            case Status.Atk3:
                if (AtkTimer <= 0)
                {
                    AtkTimer = Atk3TimerSet;
                }

                AtkTimer -= _fixedDeltaTime;

                if (AtkTimer <= (Atk3TimerSet - 0.38))
                {
                    if (!FirstTrigger)
                    {
                        BodyJudgement.gameObject.SetActive(false);
                        HighBodyJudgement.gameObject.SetActive(true);
                        switch (_basicData.face)
                        {
                            case MonsterBasicData.Face.Left:
                                HighBodyJudgement.localScale = new Vector3(-1,1,1);
                                break;
                            case MonsterBasicData.Face.Right:
                                HighBodyJudgement.localScale = new Vector3(1, 1, 1);
                                break;
                        }
                        FirstTrigger = true;
                    }
                    if (AtkTimer <= (Atk3TimerSet - 5.83))
                    {
                        _VBMonsterSE.TurnOffAtk2loopSound();
                    }
                    if (AtkTimer <= 0)
                    {
                        BodyJudgement.gameObject.SetActive(true);
                        HighBodyJudgement.gameObject.SetActive(false);
                        FirstTrigger = false;
                        lastStatus = LastStatus.Atk3;
                        ReturnToWait();
                    }
                }
                break;
            case Status.Atk4:
                if (AtkTimer <= 0)
                {
                    AtkTimer = Atk4TimerSet;
                    if (_transform.localPosition.x >= MapMiddleLine)
                    {
                        _basicData.face = MonsterBasicData.Face.Left;
                    }
                    if (_transform.localPosition.x < MapMiddleLine)
                    {
                        _basicData.face = MonsterBasicData.Face.Right;
                    }
                    if (!isSecondPhase)
                    {
                        SwitchPhaseing = true;
                        SMonsterSecondPhaseAnimation.SetActive(true);
                    }
                }

                AtkTimer -= _fixedDeltaTime;

                if (AtkTimer <= (Atk4TimerSet - 1.55))
                {
                    if (!FirstTrigger)
                    {
                        isSecondPhase = true;
                        SwitchPhaseing = false;
                        Head.ChangePhase();
                        Head.OpenMouth();
                        Atk4Arm.SetActive(false);
                        Atk4SArm.SetActive(true);
                        Atk4DownBody.SetActive(false);
                        Atk4SDownBody.SetActive(true);
                        switch (_basicData.face)
                        {
                            case MonsterBasicData.Face.Left:
                                Instantiate(Assemble, Head.transform.position, Quaternion.identity, Head.transform);
                                break;
                            case MonsterBasicData.Face.Right:
                                Instantiate(Assemble, Head.transform.position, Quaternion.identity, Head.transform);
                                break;
                        }
                        FirstTrigger = true;
                    }
                }
                if (AtkTimer <= (Atk4TimerSet - 4.9))
                {
                    if (!SecondTrigger)
                    {
                        if (_roomController != null)
                        {
                            _roomController.BeginPhaseChange(2);
                        }
                        _aniMethod.OpenFlash();
                        switch (_basicData.face)
                        {
                            case MonsterBasicData.Face.Left:
                                Instantiate(LAtk4, Head.transform.position + LAtk4Appear, Quaternion.identity, Head.transform);
                                break;
                            case MonsterBasicData.Face.Right:
                                Instantiate(RAtk4, Head.transform.position + RAtk4Appear, Quaternion.identity, Head.transform);
                                break;
                        }
                        SecondTrigger = true;
                    }
                }
                if (AtkTimer <= 0)
                {
                    FirstTrigger = false;
                    SecondTrigger = false;
                    ThirdTrigger = false;
                    Atk4GetHurtedByExplosion = false;
                    lastStatus = LastStatus.Atk4;
                    ReturnToWait();
                }
                break;
            case Status.Dash:
                if (isDashJudge)
                {
                    AtkTimer -= _fixedDeltaTime;

                    switch (dashMode)
                    {
                        case DashMode.Dash:
                            if (AtkTimer <= (DashTimerSet - 0.35))
                            {
                                if (!SecondTrigger)
                                {
                                    DashSpeed = 4;
                                    SecondTrigger = true;
                                }
                                DashMove = true;
                            }
                            if (AtkTimer <= (DashTimerSet - 1))
                            {
                                if (!ForthTrigger)
                                {
                                    DashSpeed = 12.5f;
                                    WallCheckDistance = 9.2f;
                                    ForthTrigger = true;
                                }
                            }
                            if (AtkTimer <= (DashTimerSet - 1.2))
                            {
                                if (!FirstTrigger)
                                {
                                    Instantiate(WalkAtk, _transform.position, Quaternion.identity, AtkTemporaryArea);
                                    _walkAtk = AtkTemporaryArea.GetChild(0).gameObject;
                                    AtkTemporaryArea.DetachChildren();
                                    FirstTrigger = true;
                                }
                            }
                            if (AtkTimer <= 0)
                            {
                                StartCalculateDashTime = true;
                            }
                            break;
                        case DashMode.BackDash:
                            if (!SecondTrigger)
                            {
                                switch (_basicData.face)
                                {
                                    case MonsterBasicData.Face.Left:
                                        _transform.position += new Vector3(BackDashSpeed * _fixedDeltaTime, 0, 0);
                                        break;
                                    case MonsterBasicData.Face.Right:
                                        _transform.position += new Vector3(-BackDashSpeed * _fixedDeltaTime, 0, 0);
                                        break;
                                }
                            }
                            if (AtkTimer <= (BackDashTimerSet - 0.9))
                            {
                                SecondTrigger = true;
                            }
                            if (AtkTimer <= (BackDashTimerSet - 1.3))
                            {
                                if (!ThirdTrigger)
                                {
                                    DashSpeed = 4;
                                    ThirdTrigger = true;
                                }
                                DashMove = true;
                            }
                            if (AtkTimer <= (BackDashTimerSet - 1.95))
                            {
                                if (!ForthTrigger)
                                {
                                    DashSpeed = 12.5f;
                                    WallCheckDistance = 9.2f;
                                    ForthTrigger = true;
                                }
                            }
                            if (AtkTimer <= (BackDashTimerSet - 2.15))
                            {
                                if (!FirstTrigger)
                                {
                                    Instantiate(WalkAtk, _transform.position, Quaternion.identity, AtkTemporaryArea);
                                    _walkAtk = AtkTemporaryArea.GetChild(0).gameObject;
                                    AtkTemporaryArea.DetachChildren();
                                    FirstTrigger = true;
                                }
                            }
                            if (AtkTimer <= 0)
                            {
                                StartCalculateDashTime = true;
                            }
                            break;
                        case DashMode.DashEnd:
                            if (_walkAtk != null)
                            {
                                Destroy(_walkAtk);
                            }

                            if (DashSpeed > 0)
                            {
                                DashSpeed -= 0.7f;
                            }
                            if (DashSpeed <= 0)
                            {
                                DashSpeed = 0;
                            }

                            if (AtkTimer <= 0)
                            {
                                FirstTrigger = false;
                                SecondTrigger = false;
                                ThirdTrigger = false;
                                ForthTrigger = false;
                                isDashJudge = false;
                                DashMove = false;
                                WallCheckDistance = 11.5f;
                                dashMode = DashMode.Default;
                                lastStatus = LastStatus.Dash;
                                ReturnToWait();
                                if (CanUseAtk4)
                                {
                                    CanUseAtk4 = false;
                                    status = Status.Atk4;
                                    AnimationPreSet();
                                }
                            }
                            break;
                    }

                    if (StartCalculateDashTime)
                    {
                        DashRepectTime -= _fixedDeltaTime;
                    }

                    if (DashMove)
                    {
                        switch (_basicData.face)
                        {
                            case MonsterBasicData.Face.Left:
                                if (!_basicData.touchLeftWall)
                                {
                                    _transform.position += new Vector3(-DashSpeed * _fixedDeltaTime, 0, 0);
                                }
                                break;
                            case MonsterBasicData.Face.Right:
                                if (!_basicData.touchRightWall)
                                {
                                    _transform.position += new Vector3(DashSpeed * _fixedDeltaTime, 0, 0);
                                }
                                break;
                        }
                    }
                }

                break;
            case Status.Jump:
                if (AtkTimer <= 0)
                {
                    _basicData.TurnFaceJudge();
                    AtkTimer = JumpTimerSet;
                }

                AtkTimer -= _fixedDeltaTime;

                if (AtkTimer <= (JumpTimerSet - 0.75))
                {
                    if (!ParabolaCaculate)
                    {
                        if (_basicData.isPlayerAtLeftSide)
                        {
                            _parabolaVar.HorizontalDirection = "Left";
                        }
                        if (_basicData.isPlayerAtRightSide)
                        {
                            _parabolaVar.HorizontalDirection = "Right";
                        }
                        _parabolaVar.VerticalDirection = "Down";
                        _parabolaVar.ParabolaNowX = _transform.localPosition.x;
                        _parabolaVar.OtherPoint = new Vector3(_transform.localPosition.x, _transform.localPosition.y, 0);

                        _parabolaVar.MiddlePoint = new Vector3((_basicData.playerTransform.localPosition.x + _transform.localPosition.x) / 2, _transform.localPosition.y + 5, 0);
                        _parabolaVar.Speed = Mathf.Abs(_basicData.playerTransform.localPosition.x - _transform.localPosition.x) / 0.8f;

                        _parabolaVar.ParabolaConstant = Parabola.CalculateParabolaConstant(_parabolaVar);

                        ParabolaCaculate = true;
                    }
                }
                if (AtkTimer <= (JumpTimerSet - 0.95))
                {
                    if (!JumpEnd)
                    {
                        Parabola.ParabolaMove(_parabolaVar, _fixedDeltaTime, _transform, _basicData.touchLeftWall, _basicData.touchRightWall);
                    }
                }
                if (AtkTimer <= (JumpTimerSet - 1.45))
                {
                    if (!FirstTrigger)
                    {
                        _basicData.TurnFaceJudge();
                        Instantiate(JumpAtk, _transform.position, Quaternion.identity, AtkTemporaryArea);
                        AtkTemporaryArea.DetachChildren();
                        FirstTrigger = true;
                    }
                }
                if (AtkTimer <= (JumpTimerSet - 1.75))
                {
                    if (!JumpEnd)
                    {
                        Instantiate(ShockWave, new Vector3(_transform.localPosition.x, MonsterBaseLine, 0), Quaternion.identity);
                        JumpEnd = true;
                    }
                }
                if (AtkTimer <= 0)
                {
                    _basicData.TurnFaceJudge();
                    FirstTrigger = false;
                    ParabolaCaculate = false;
                    JumpEnd = false;
                    lastStatus = LastStatus.Jump;
                    ReturnToWait();
                }
                break;
            case Status.Capture:
                if (AtkTimer <= 0)
                {
                    _basicData.TurnFaceJudge();
                    AtkTimer = CaptureAtkTimerSet;
                }

                AtkTimer -= _fixedDeltaTime;

                if (AtkTimer <= (CaptureAtkTimerSet - 1.2))
                {
                    if (!FirstTrigger)
                    {
                        switch (_basicData.face)
                        {
                            case MonsterBasicData.Face.Right:
                                Instantiate(RCaptureAtk, _transform.position, Quaternion.identity, AtkTemporaryArea);
                                break;
                            case MonsterBasicData.Face.Left:
                                Instantiate(LCaptureAtk, _transform.position, Quaternion.identity, AtkTemporaryArea);
                                break;
                        }
                        AtkTemporaryArea.DetachChildren();
                        FirstTrigger = true;
                    }
                    if (AtkTimer <= 0)
                    {
                        CanCapture = false;
                        CaptureAtkLastTime = _time;
                        lastStatus = LastStatus.Capture;
                        ReturnToWait();
                        FirstTrigger = false;
                    }
                }
                break;
            case Status.CaptureSuccess:
                _captureController.CaptureSuccessTimerMethod(_fixedDeltaTime);
                break;
            case Status.Summon:
                if (AtkTimer <= 0)
                {
                    _basicData.TurnFaceJudge();
                    AtkTimer = SummonTimerSet;
                }

                AtkTimer -= _fixedDeltaTime;

                if (AtkTimer <= (SummonTimerSet - 0.25))
                {
                    if (!ThirdTrigger)
                    {
                        Head.OpenMouth();
                        ThirdTrigger = true;
                    }
                }
                if (AtkTimer <= (SummonTimerSet - 0.75))
                {
                    if (!FirstTrigger)
                    {
                        Instantiate(Howl, _transform.position, Quaternion.identity, AtkTemporaryArea);
                        AtkTemporaryArea.GetChild(0).GetComponent<FollowObject>().Target = SummonHowlTarget;
                        AtkTemporaryArea.DetachChildren();
                        FirstTrigger = true;
                    }
                }
                if (AtkTimer <= (SummonTimerSet - 2.5))
                {
                    if (!SecondTrigger)
                    {
                        if (_basicData.hp > _basicData.maxHp * 0.33)
                        {
                            switch (Random.Range(0, 2))
                            {
                                case 0:
                                    Instantiate(Phase1Drone, _transform.position, Quaternion.identity);
                                    break;
                                case 1:
                                    Instantiate(Phase2Drone, _transform.position, Quaternion.identity);
                                    break;
                            }
                        }
                        else
                        {
                            Instantiate(Phase1Drone, _transform.position, Quaternion.identity);
                            Instantiate(Phase2Drone, _transform.position, Quaternion.identity);
                        }
                        SecondTrigger = true;
                    }

                    if (AtkTimer <= 0)
                    {
                        lastStatus = LastStatus.Summon;
                        SummonLastTime = _time;
                        CanSummon = false;
                        FirstTrigger = false;
                        SecondTrigger = false;
                        ThirdTrigger = false;
                        ReturnToWait();
                    }
                }
                break;
            case Status.Weak:
                if (AtkTimer <= 0)
                {
                    AtkTimer = WeakTimerSet;
                }

                WeakTimer -= _fixedDeltaTime;

                if (!FirstTrigger)
                {
                    if (_roomController != null)
                    {
                        _roomController.BeginPhaseChange(3);
                    }
                    BodyJudgement.gameObject.SetActive(false);
                    WeakBodyJudgement.gameObject.SetActive(true);
                    FirstTrigger = true;
                }
                if (WeakTimer <= (WeakTimerSet - 1))
                {
                    if (!SecondTrigger)
                    {
                        Instantiate(DisappearLight, Head.transform.position, Quaternion.identity);
                        SecondTrigger = true;
                    }
                }
                if (WeakTimer <= (WeakTimerSet - 3))
                {
                    isGetUp = true;
                    if (WeakTimer <= 0)
                    {
                        BodyJudgement.gameObject.SetActive(true);
                        WeakBodyJudgement.gameObject.SetActive(false);
                        isGetUp = false;
                        FirstTrigger = false;
                        SecondTrigger = false;
                        ReturnToWait();
                    }
                }
                break;
        }

        _hurtedController.BurningTimerMethod(AtkTemporaryArea, _fixedDeltaTime);
    }

    void SwitchAnimation()
    {
        if (status == Status.wait)
        {
            NowAni = AniStatus.wait;
        }
        if (status == Status.walk)
        {
            NowAni = AniStatus.walk;
        }
        if (status == Status.Dash)
        {
            NowAni = AniStatus.Dash;
        }
        if (status == Status.Atk1)
        {
            NowAni = AniStatus.Atk1;
        }
        if (status == Status.Atk1_5)
        {
            NowAni = AniStatus.Atk1_5;
        }
        if (status == Status.Atk2)
        {
            NowAni = AniStatus.Atk2;
        }
        if (status == Status.Atk3)
        {
            NowAni = AniStatus.Atk3;
        }
        if (status == Status.Atk4)
        {
            NowAni = AniStatus.Atk4;
        }
        if (status == Status.Summon)
        {
            NowAni = AniStatus.Summon;
        }
        if (status == Status.Capture || _captureController.isCaptureSuccess)
        {
            NowAni = AniStatus.Capture;
        }
        if (status == Status.Weak)
        {
            NowAni = AniStatus.Weak;
        }
        if (status == Status.Jump)
        {
            NowAni = AniStatus.Jump;
        }

        if (LastAni != NowAni)
        {
            _VBMonsterSE.BoolReset();
        }

        switch (NowAni)
        {
            case AniStatus.wait:
                AllAniFalse();
                MonsterWaitAnimation.SetActive(true);
                break;
            case AniStatus.walk:
                AllAniFalse();
                MonsterWalkAnimation.SetActive(true);
                MonsterWalkAni.SetInteger("Status", 1);
                break;
            case AniStatus.Dash:
                AllAniFalse();
                MonsterWalkAnimation.SetActive(true);
                switch (dashMode)
                {
                    case DashMode.Dash:
                        MonsterWalkAni.SetInteger("Status", 3);
                        if (!_VBMonsterSE.SEAppear)
                        {
                            _VBMonsterSE.PrepareAtkSoundPlay();
                            _VBMonsterSE.SEAppear = true;
                        }
                        break;
                    case DashMode.BackDash:
                        MonsterWalkAni.SetInteger("Status", 2);
                        if (!_VBMonsterSE.SEAppear)
                        {
                            _VBMonsterSE.BackDashPlay();
                            _VBMonsterSE.SEAppear = true;
                        }
                        if (!_VBMonsterSE.SE2Appear)
                        {
                            _VBMonsterSE.PrepareAtkSoundPlay(1);
                            _VBMonsterSE.SE2Appear = true;
                        }
                        break;
                    case DashMode.DashEnd:
                        MonsterWalkAni.SetBool("End", true);
                        break;
                }
                break;
            case AniStatus.Jump:
                AllAniFalse();
                MonsterJumpAnimation.SetActive(true);
                if (!_VBMonsterSE.SEAppear)
                {
                    _VBMonsterSE.PrepareAtkSoundPlay();
                    _VBMonsterSE.SEAppear = true;
                }
                if (JumpEnd)
                {
                    MonsterJumpAni.SetBool("isGround", true);
                    if (!_VBMonsterSE.SE2Appear)
                    {
                        _VBMonsterSE.TouchGroundSoundPlay();
                        _VBMonsterSE.SE2Appear = true;
                    }
                }
                break;
            case AniStatus.Atk1:
                AllAniFalse();
                MonsterAtkAnimation.SetActive(true);
                switch (atk1Mode)
                {
                    case Atk1Mode.Default:
                        MonsterAtk1Ani.SetInteger("Status", 0);
                        break;
                    case Atk1Mode.FirstAtk:
                        MonsterAtk1Ani.SetInteger("Status", 0);
                        if (!_VBMonsterSE.SEAppear)
                        {
                            _VBMonsterSE.AtkSoundPlay();
                            _VBMonsterSE.SEAppear = true;
                        }
                        break;
                    case Atk1Mode.FirstEnd:
                        MonsterAtk1Ani.SetInteger("Status", 1);
                        break;
                    case Atk1Mode.FirstContinue:
                        MonsterAtk1Ani.SetInteger("Status", 2);
                        if (!_VBMonsterSE.SE2Appear)
                        {
                            _VBMonsterSE.Atk1_5SoundPlay(0.55f);
                            _VBMonsterSE.SE2Appear = true;
                        }
                        break;
                    case Atk1Mode.SecondEnd:
                        MonsterAtk1Ani.SetInteger("Status", 3);
                        break;
                    case Atk1Mode.SecondContinue:
                        MonsterAtk1Ani.SetInteger("Status", 4);
                        break;
                }
                break;
            case AniStatus.Atk1_5:
                AllAniFalse();
                MonsterAtk1_5Animation.SetActive(true);
                if (!_VBMonsterSE.SEAppear)
                {
                    _VBMonsterSE.Atk1_5SoundPlay();
                    _VBMonsterSE.SEAppear = true;
                }
                break;
            case AniStatus.Atk2:
                AllAniFalse();
                MonsterAtk2Animation.SetActive(true);
                if (!_VBMonsterSE.SEAppear)
                {
                    _VBMonsterSE.Atk2SoundPlay("Atk2");
                    _VBMonsterSE.SEAppear = true;
                }
                break;
            case AniStatus.Atk3:
                AllAniFalse();
                MonsterAtk3Animation.SetActive(true);
                if (!_VBMonsterSE.SEAppear)
                {
                    _VBMonsterSE.Atk2BeginSoundPlay();
                    _VBMonsterSE.SEAppear = true;
                }
                if (!_VBMonsterSE.SE2Appear)
                {
                    _VBMonsterSE.Atk2loopSoundPlay();
                    _VBMonsterSE.SE2Appear = true;
                }
                break;
            case AniStatus.Atk4:
                AllAniFalse();
                MonsterAtk4Animation.SetActive(true);
                if (!_VBMonsterSE.SEAppear)
                {
                    _VBMonsterSE.Atk4StepSoundPlay();
                    _VBMonsterSE.SEAppear = true;
                }
                break;
            case AniStatus.Summon:
                AllAniFalse();
                MonsterSummonAnimation.SetActive(true);
                break;
            case AniStatus.Capture:
                AllAniFalse();
                MonsterCaptureAnimation.SetActive(true);
                if (_captureController.isCaptureSuccess)
                {
                    MonsterCaptureAni.SetBool("Success", true);
                    if (!_VBMonsterSE.SEAppear)
                    {
                        _VBMonsterSE.CaptureAtk1SoundPlay(1);
                        _VBMonsterSE.SEAppear = true;
                    }
                    if (!_VBMonsterSE.SE2Appear)
                    {
                        _VBMonsterSE.CaptureAtk2SoundPlay();
                        _VBMonsterSE.SE2Appear = true;
                    }
                    if (!_VBMonsterSE.SE3Appear)
                    {
                        _VBMonsterSE.CaptureAtk3SoundPlay(2.52f);
                        _VBMonsterSE.SE3Appear = true;
                    }
                }
                break;
            case AniStatus.Weak:
                AllAniFalse();
                SMonsterWeakAnimation.SetActive(true);
                if (!_VBMonsterSE.SEAppear)
                {
                    _VBMonsterSE.FallDownSoundPlay();
                    _VBMonsterSE.SEAppear = true;
                }
                if (isGetUp)
                {
                    SMonsterWeakAni.SetBool("GetUp", true);
                    if (!_VBMonsterSE.SE2Appear)
                    {
                        _VBMonsterSE.GetUpSoundPlay();
                        _VBMonsterSE.SE2Appear = true;
                    }
                }
                break;
            case AniStatus.Stop:
                AllAniFalse();
                MonsterStopAnimation.SetActive(true);
                break;
        }

        ContinueTypeSEControll();

        if (LastAni != NowAni)
        {
            LastAni = NowAni;
        }
    }

    private void PlayStopAni()
    {
        switch (_basicData.face)
        {
            case MonsterBasicData.Face.Left:
                MonsterStopTr.localScale = new Vector3(-0.58f, 0.58f, 0);
                break;
            case MonsterBasicData.Face.Right:
                MonsterStopTr.localScale = new Vector3(0.58f, 0.58f, 0);
                break;
        }
        NowAni = AniStatus.Stop;
        AllAniFalse();
        _VBMonsterSE.StopAllSE();
        Head.InitializeHead(StopHead);
        MonsterStopAnimation.SetActive(true);
    }

    private void StopAniCnange()
    {
        MonsterStopAni.SetBool("End", true);
        if (status != Status.Weak)
        {
            MonsterStopAni.SetBool("Return", true);
        }
    }

    private void AllAniFalse()
    {
        if (NowAni != AniStatus.wait)
        {
            MonsterWaitAnimation.SetActive(false);
        }
        if (NowAni != AniStatus.walk && NowAni != AniStatus.Dash)
        {
            MonsterWalkAnimation.SetActive(false);
        }
        if (NowAni != AniStatus.Jump)
        {
            MonsterJumpAnimation.SetActive(false);
        }
        if (NowAni != AniStatus.Atk1)
        {
            MonsterAtkAnimation.SetActive(false);
        }
        if (NowAni != AniStatus.Atk1_5)
        {
            MonsterAtk1_5Animation.SetActive(false);
        }
        if (NowAni != AniStatus.Atk2)
        {
            MonsterAtk2Animation.SetActive(false);
        }
        if (NowAni != AniStatus.Atk3)
        {
            MonsterAtk3Animation.SetActive(false);
        }
        if (NowAni != AniStatus.Atk4)
        {
            MonsterAtk4Animation.SetActive(false);
        }
        if (NowAni != AniStatus.Summon)
        {
            MonsterSummonAnimation.SetActive(false);
        }
        if (NowAni != AniStatus.Capture)
        {
            MonsterCaptureAnimation.SetActive(false);
        }
        if (NowAni != AniStatus.Weak)
        {
            SMonsterWeakAnimation.SetActive(false);
        }
        if (NowAni != AniStatus.Stop)
        {
            MonsterStopAnimation.SetActive(false);
        }
        if (NowAni != AniStatus.Begining)
        {
            MonsterBeginAnimation.SetActive(false);
        }
    }

    private void StatusJudge()//(1)
    {
        while (!DefineStatus)
         {
            if (_basicData.AbsDistanceX <= CloseAtkDistance)
            {
                switch (Random.Range(0, 3))
                {
                    case 0:
                        if (lastStatus != LastStatus.Atk1)
                        {
                            status = Status.Atk1;
                            DefineStatus = true;
                        }
                        break;
                    case 1:
                        if (lastStatus != LastStatus.Atk1_5)
                        {
                            status = Status.Atk1_5;
                            DefineStatus = true;
                        }
                        break;
                    case 2:
                        if (lastStatus != LastStatus.Capture && CanCapture)
                        {
                            status = Status.Capture;
                            DefineStatus = true;
                        }
                        break;
                }
            }
            if (_basicData.AbsDistanceX <= VeryCloseAtkDistance)
            {
                if (lastStatus != LastStatus.Jump)
                {
                    status = Status.Jump;
                    DefineStatus = true;
                }
                if (lastStatus == LastStatus.Jump)
                {
                    status = Status.Dash;
                    DefineStatus = true;
                }
            }
            if (_basicData.AbsDistanceX > CloseAtkDistance)
            {
                switch (Random.Range(0, 3))
                {
                    case 0:
                        if (lastStatus != LastStatus.Atk2)
                        {
                            status = Status.Atk2;
                            DefineStatus = true;
                        }
                        break;
                    case 1:
                        if (lastStatus != LastStatus.Jump)
                        {
                            status = Status.Jump;
                            DefineStatus = true;
                        }
                        break;
                    case 2:
                        if (lastStatus != LastStatus.walk)
                        {
                            status = Status.walk;
                            DefineStatus = true;
                        }
                        break;
                }
            }

            if (isSecondPhase && CanSummon)
            {
                switch (Random.Range(0, 4))
                {
                    case 0:
                        status = Status.Summon;
                        DefineStatus = true;
                        break;
                }
            }

            if(lastStatus == LastStatus.Dash)
            {
                status = Status.Atk3;
                DefineStatus = true;
            }

            if (CanUseAtk4)
            {
                status = Status.Dash;
                DefineStatus = true;
            }

            if (lastStatus == LastStatus.Atk4)
            {
                status = Status.Summon;
                DefineStatus = true;
            }

            if (Atk4Interrupted)
            {
                status = Status.Atk4;
                Atk4Interrupted = false;
                DefineStatus = true;
            }

            if (PlayerController.isDie)
            {
                status = Status.wait;
                Atk4Interrupted = false;
                DefineStatus = true;
            }
         }
        DefineStatus = false;

        if (status != Status.Dash && status != Status.Atk3 && status != Status.Atk4)
        {
            _basicData.TurnFaceJudge();
        }
        if (status == Status.Atk3 || status == Status.Atk4 || status == Status.Dash)
        {
            if (_transform.localPosition.x >= MapMiddleLine)
            {
                _basicData.face = MonsterBasicData.Face.Left;
            }
            if (_transform.localPosition.x < MapMiddleLine)
            {
                _basicData.face = MonsterBasicData.Face.Right;
            }
        }

        AnimationPreSet();
    }

    private void AnimationPreSet()//(1)
    {
        switch (status)
        {
            case Status.wait:
                Head.InitializeHead(WaitHead);
                break;
            case Status.walk:
                Head.InitializeHead(WalkHead);
                break;
            case Status.Dash:
                Head.InitializeHead(WalkHead);
                break;
            case Status.Jump:
                Head.InitializeHead(JumpHead);
                break;
            case Status.Atk1:
                Head.InitializeHead(AtkHead);
                break;
            case Status.Atk1_5:
                Head.InitializeHead(Atk1_5Head);
                break;
            case Status.Atk2:
                Head.InitializeHead(Atk2Head);
                break;
            case Status.Atk3:
                Head.InitializeHead(Atk3Head);
                break;
            case Status.Atk4:
                Head.InitializeHead(Atk4Head);
                break;
            case Status.Summon:
                Head.InitializeHead(SummonHead);
                break;
            case Status.Capture:
                Head.InitializeHead(CaptureHead);
                break;
            case Status.Weak:
                Head.InitializeHead(WeakHead);
                break;
        }
    }

    private void ReturnToWait()//(1)
    {
        status = Status.wait;
        AnimationPreSet();
    }

    private void DetectAtk4Interrupted()
    {
        if (status == Status.Atk4)
        {
            if (Head.HeadHurtNumber == 4)
            {
                Atk4GetHurtedByExplosion = true;
                ResetValue();
            }
        }
    }

    public void BeginWalk()//(1)事件專用
    {
        status = Status.walk;
        NowAni = AniStatus.walk;
    }

    private void ContinueTypeSEControll()
    {
        if (NowAni == AniStatus.walk && _basicData.isGround)
        {
            _VBMonsterSE.WalkSoundPlay();
        }

        if (DashMove && _basicData.isGround && dashMode != DashMode.DashEnd)
        {
            _VBMonsterSE.DashSoundPlay();
        }

        if (NowAni != AniStatus.walk)
        {
            _VBMonsterSE.TurnOffWalkSound();
        }

        if (dashMode == DashMode.DashEnd)
        {
            _VBMonsterSE.TurnOffDashSound();
        }
    }

    private void ResetValue()
    {
        if (_walkAtk != null)
        {
            Destroy(_walkAtk);
        }
        isDashJudge = false;
        DashMove = false;
        WallCheckDistance = 11.5f;
        DashMove = false;
        JumpEnd = false;
        ParabolaCaculate = false;
        FirstTrigger = false;
        SecondTrigger = false;
        ThirdTrigger = false;
        ForthTrigger = false;
        WalkTimer = WalkTimerSet;
        AtkTimer = 0;
        atk1Mode = Atk1Mode.Default;
        dashMode = DashMode.Default;
        lastStatus = LastStatus.wait;
        _captureController.AllVariableReset();

        //大招被斷
        if (status == Status.Atk4)
        {
            if (!Atk4GetHurtedByExplosion)
            {
                if (SwitchPhaseing)
                {
                    isSecondPhase = true;
                    SwitchPhaseing = false;
                    SMonsterSecondPhaseAnimation.SetActive(false);
                    lastStatus = LastStatus.Atk4;
                    Atk4Interrupted = true;
                    ReturnToWait();
                    return;
                }
                if (!SwitchPhaseing)
                {
                    isSecondPhase = true;
                    lastStatus = LastStatus.Atk4;
                    status = Status.Weak;

                    BodyJudgement.gameObject.SetActive(false);
                    WeakBodyJudgement.gameObject.SetActive(true);
                    switch (_basicData.face)
                    {
                        case MonsterBasicData.Face.Left:
                            WeakBodyJudgement.localScale = new Vector3(-1, 1, 1);
                            break;
                        case MonsterBasicData.Face.Right:
                            WeakBodyJudgement.localScale = new Vector3(1, 1, 1);
                            break;
                    }
                    AnimationPreSet();
                }//Weak
            }
            else
            {
                if (!SwitchPhaseing)
                {
                    isSecondPhase = true;
                    Atk4GetHurtedByExplosion = false;
                    lastStatus = LastStatus.Atk4;
                    status = Status.Weak;

                    BodyJudgement.gameObject.SetActive(false);
                    WeakBodyJudgement.gameObject.SetActive(true);
                    switch (_basicData.face)
                    {
                        case MonsterBasicData.Face.Left:
                            WeakBodyJudgement.localScale = new Vector3(-1, 1, 1);
                            break;
                        case MonsterBasicData.Face.Right:
                            WeakBodyJudgement.localScale = new Vector3(1, 1, 1);
                            break;
                    }
                    AnimationPreSet();
                }//Weak
            }
        }
    }
}
