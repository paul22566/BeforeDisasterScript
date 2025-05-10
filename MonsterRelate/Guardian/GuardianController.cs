using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class GuardianController : MonoBehaviour
{
    [Header("基本參數")]
    public float CloseAtkDistance;
    public float TooCloseDistance;
    public float speed;
    private enum Status { wait, walk, Jump, Atk1, Atk2, BackAtk2, BeginAtk, Howl };
    private Status status;
    private enum LastStatus { wait, walk, Jump, Atk1, Atk2, BackAtk2, Howl };
    private LastStatus lastStatus;

    private enum JumpMode { Atk, Adjust};
    private JumpMode jumpMode;
    public enum AniStatus { wait, walk, Jump, Atk1, Atk2, BackAtk2, Stop, Begining, Howl }
    [HideInInspector] public AniStatus NowAni;//script(Hurted)
    private AniStatus LastAni;

    [Header("系統參數")]
    private Transform BodyJudgement;
    private Transform AtkTemporaryArea;
    private Transform _transform;
    private MonsterBasicData _basicData;
    private VBMonsterSE _VBMonsterSE;
    [SerializeField] private HallController RoomController;
    [SerializeField] private float MonsterBaseLine;
    private float AtkTimer;
    private float _deltaTime;
    private float _fixedDeltaTime;
    public float WalkTimerSet;
    private float WalkTimer;
    public float BeginAtkTimerSet;
    public float Atk1TimerSet;
    public float Atk2TimerSet;
    public float BackAtk2TimerSet;
    public float JumpTimerSet;
    public float WaitTimerSet;
    private float WaitTimer;
    public float HowlTimerSet;
    private MonsterHurtedController _hurtedController;
    private float GroundPointY;
    public float BackAtk2Speed1;
    public float BackAtk2Speed2;
    public float WallCheckDistance;
    private RaycastHit2D LeftBackAtkCheck;
    private RaycastHit2D RightBackAtkCheck;

    //開關
    [HideInInspector] public bool JumpEnd;//script(跳躍攻擊)
    private bool FirstTrigger;
    private bool SecondTrigger;
    private bool Atk1End;
    private bool BackAtk2MoveEnd1;
    private bool BackAtk2MoveEnd2;
    private bool CloseToLeftWall;
    private bool CloseToRightWall;
    private bool DefineStatus;

    private bool HallRoomEventJudge;
    [HideInInspector] public bool BeginFight;

    //BeginAtk
    ParabolaVar _parabolaVar = new ParabolaVar();
    private bool BeginAtkJumpEnd;
    private bool ParabolaCaculate;

    [Header("動畫相關物件")]
    private Animator MonsterJumpAni;
    private GameObject MonsterWaitAnimation;
    private GameObject MonsterWalkAnimation;
    private GameObject MonsterJumpAnimation;
    private GameObject MonsterAtkAnimation;
    private GameObject MonsterAtk2Animation;
    private GameObject MonsterBackAtk2Animation;
    public GameObject RMonsterDieAnimation;
    public GameObject LMonsterDieAnimation;
    private GameObject MonsterBeginAnimation;
    private GameObject MonsterHowlAnimation;
    private Transform MonsterWaitTransform;
    private Transform MonsterWalkTransform;
    private Transform MonsterJumpTransform;
    private Transform MonsterAtkTransform;
    private Transform MonsterBackAtk2Transform;
    private Transform MonsterAtk2Transform;
    private Transform MonsterBeginAtkTransform;
    private Transform MonsterHowlTransform;

    private Animator MonsterAtk1Ani;
    private Animator MonsterWalkAni;
    private Animator MonsterBeginAtkAni;
    public GameObject Howl;
    //頭
    private GuardianHead Head;
    private GuardianHeadValue WaitHeadValue = new GuardianHeadValue(false, 0, 310, 34, 0, 0);
    private GuardianHeadValue WalkHeadValue = new GuardianHeadValue(false, 0, 310, 34, 0, 0);
    private GuardianHeadValue JumpHeadValue = new GuardianHeadValue(true, 0, 310, 34, 0, 0);
    private GuardianHeadValue AtkHeadValue = new GuardianHeadValue(false, 0, 310, 34, 0, 0);
    private GuardianHeadValue Atk2HeadValue = new GuardianHeadValue(true, 0, 310, 34, 0.6f, 0.85f);
    private GuardianHeadValue BackAtk2HeadValue = new GuardianHeadValue(true, 0, 310, 34, 1.3f, 1.55f);
    private GuardianHeadValue BegingingHeadValue = new GuardianHeadValue(true, 0, 310, 34, 0, 0);
    private GuardianHeadValue HowlHeadValue = new GuardianHeadValue(true, 0, 310, 34, 2.75f, 3f);
    private GuardianHeadValue StopHead = new GuardianHeadValue(true, 0, 310, 34, 0, 0);

    [Header("攻擊物件")]
    public GameObject RBeginAtk;
    public GameObject LBeginAtk;
    public GameObject RAtk1;
    public GameObject LAtk1;
    public GameObject RAtk2;
    public GameObject LAtk2;
    public GameObject ShockWave;
    private Vector3 LAtk2Appear = new Vector3(-2.36f, -1.39f, 0);
    private Vector3 RAtk2Appear = new Vector3(2.36f, -1.39f, 0);
    private Vector3 LBackAtk2Appear = new Vector3(-2.37f, -0.97f, 0);
    private Vector3 RBackAtk2Appear = new Vector3(2.37f, -0.97f, 0);

    [Header("被大招攻擊")]
    private GameObject MonsterStopAnimation;
    private Transform MonsterStopTr;
    private Animator MonsterStopAni;

    void Start()
    {
        _transform = transform;
        _VBMonsterSE = this.GetComponent<VBMonsterSE>();
        //抓取動畫物件
        MonsterWaitAnimation = this.transform.GetChild(0).gameObject;
        MonsterWalkAnimation = this.transform.GetChild(1).gameObject;
        MonsterJumpAnimation = this.transform.GetChild(2).gameObject;
        MonsterAtkAnimation = this.transform.GetChild(3).gameObject;
        MonsterAtk2Animation = this.transform.GetChild(4).gameObject;
        MonsterBackAtk2Animation = this.transform.GetChild(5).gameObject;
        MonsterStopAnimation = this.transform.GetChild(6).gameObject;
        MonsterBeginAnimation = this.transform.GetChild(7).gameObject;
        MonsterHowlAnimation = this.transform.GetChild(8).gameObject;

        Head = this.transform.GetChild(9).GetComponent<GuardianHead>();

        MonsterWaitTransform = MonsterWaitAnimation.transform.GetChild(0);
        MonsterWalkTransform = MonsterWalkAnimation.transform.GetChild(0);
        MonsterJumpTransform = MonsterJumpAnimation.transform.GetChild(0);
        MonsterAtkTransform = MonsterAtkAnimation.transform.GetChild(0);
        MonsterAtk2Transform = MonsterAtk2Animation.transform.GetChild(0);
        MonsterBackAtk2Transform = MonsterBackAtk2Animation.transform.GetChild(0);
        MonsterStopTr = MonsterStopAnimation.transform.GetChild(0);
        MonsterBeginAtkTransform = MonsterBeginAnimation.transform.GetChild(0);
        MonsterHowlTransform = MonsterHowlAnimation.transform.GetChild(0);

        MonsterWalkAni = MonsterWalkTransform.GetComponent<Animator>();
        MonsterAtk1Ani = MonsterAtkTransform.GetComponent<Animator>();
        MonsterBeginAtkAni = MonsterBeginAtkTransform.GetComponent<Animator>();
        MonsterJumpAni = MonsterJumpTransform.GetComponent<Animator>();
        MonsterStopAni = MonsterStopTr.GetComponent<Animator>();

        AtkTemporaryArea = this.transform.GetChild(11);
        BodyJudgement = this.transform.GetChild(10);
        WalkTimer = WalkTimerSet;
        WaitTimer = WaitTimerSet;
        AtkTimer = 0;

        status = Status.wait;
        lastStatus = LastStatus.wait;
        NowAni = AniStatus.wait;
        LastAni = AniStatus.wait;

        if (RoomController != null)
        {
            status = Status.BeginAtk;
            Head.BeginRotateByBone();
            NowAni = AniStatus.Begining;
        }

        _hurtedController = this.transform.GetComponent<MonsterHurtedController>();
        _basicData = this.GetComponent<MonsterBasicData>();

        _basicData.BasicVarInisialize(MonsterWaitTransform, "R");

        _hurtedController._getCriticHurted += PlayStopAni;
        _hurtedController._reactCriticHurted += StopAniCnange;
    }

    // Update is called once per frame
    void Update()
    {
        _deltaTime = Time.deltaTime;
        if (RoomController != null)
        {
            if (_basicData.hp <= (_basicData.maxHp * 0.5) && _transform.localPosition.y <= MonsterBaseLine)
            {
                switch (_basicData.face)
                {
                    case MonsterBasicData.Face.Left:
                        LMonsterDieAnimation.transform.localPosition = _transform.localPosition;
                        LMonsterDieAnimation.SetActive(true);
                        RoomController.GuardianFaceLeft = true;
                        break;
                    case MonsterBasicData.Face.Right:
                        RMonsterDieAnimation.transform.localPosition = _transform.localPosition;
                        RMonsterDieAnimation.SetActive(true);
                        RoomController.GuardianFaceRight = true;
                        break;
                }
                Destroy(this.gameObject);
                return;
            }
        }

        if (_hurtedController.isCriticAtkHurted)
        {
            status = Status.wait;
            AtkTimer = 0;
            ParabolaCaculate = false;
            JumpEnd = false;
            Atk1End = false;
            FirstTrigger = false;
            SecondTrigger = false;
            WaitTimer = WaitTimerSet;
            WalkTimer = WalkTimerSet;
        }

        _hurtedController.CriticAtkHurtedTimerMethod(_deltaTime);
        _hurtedController.HurtedTimerMethod(_deltaTime);

        //判斷是否觸地
        _basicData.CheckTouchGround(ref GroundPointY);
        //判斷是否碰到牆壁
        _basicData.CheckTouchWall();
        LeftBackAtkCheck = Physics2D.Raycast(transform.position, Vector2.left, WallCheckDistance, 64);
        if (LeftBackAtkCheck)
        {
            CloseToLeftWall = true;
        }
        else
        {
            CloseToLeftWall = false;
        }
        RightBackAtkCheck = Physics2D.Raycast(transform.position, Vector2.right, WallCheckDistance, 128);
        if (RightBackAtkCheck)
        {
            CloseToRightWall = true;
        }
        else
        {
            CloseToRightWall = false;
        }

        //寫入實際位置
        _basicData.MonsterPlace = _transform.position;

        if (!HallRoomEventJudge && RoomController != null)
        {
            AllAniFalse();
            this.gameObject.SetActive(false);
            HallRoomEventJudge = true;
            return;
        }

        //return
        if (GameEvent.isAniPlay || _hurtedController.isCriticAtkHurted || PauseMenuController.isPauseMenuOpen)
        {
            return;
        }
        
        if (status != Status.Jump && status != Status.BeginAtk)
        {
            _transform.position = new Vector3(_transform.position.x, MonsterBaseLine, 0);
        }
        if (status == Status.Jump)
        {
            if (_transform.localPosition.y < MonsterBaseLine)
            {
                _transform.position = new Vector3(_transform.position.x, MonsterBaseLine, 0);
            }
        }

        //計算距離
        _basicData.DistanceCalculate();

        //被大招打之後的動作
        if (_hurtedController.BeCriticAtkEnd)
        {
            status = Status.wait;
            _hurtedController.BeCriticAtkEnd = false;
        }

        //怪物AI
        switch (status)
        {
            case Status.wait:
                WaitTimer -= _deltaTime;
                if (WaitTimer <= 0)
                {
                    StatusJudge();
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
        }
        //轉向控制
        switch (_basicData.face)
        {
            case MonsterBasicData.Face.Left:
                MonsterWaitTransform.localScale = new Vector3(-0.58f, 0.58f, 0);
                MonsterWalkTransform.localScale = new Vector3(-0.58f, 0.58f, 0);
                MonsterJumpTransform.localScale = new Vector3(-0.58f, 0.58f, 0);
                MonsterAtkTransform.localScale = new Vector3(-0.58f, 0.58f, 0);
                MonsterAtk2Transform.localScale = new Vector3(-0.58f, 0.58f, 0);
                MonsterBackAtk2Transform.localScale = new Vector3(-0.58f, 0.58f, 0);
                MonsterStopTr.localScale = new Vector3(-0.58f, 0.58f, 0);
                MonsterBeginAtkTransform.localScale = new Vector3(-0.58f, 0.58f, 0);
                MonsterHowlTransform.localScale = new Vector3(-0.58f, 0.58f, 0);
                BodyJudgement.localScale = new Vector3(-1, 1, 0);
                break;
            case MonsterBasicData.Face.Right:
                MonsterWaitTransform.localScale = new Vector3(0.58f, 0.58f, 0);
                MonsterWalkTransform.localScale = new Vector3(0.58f, 0.58f, 0);
                MonsterJumpTransform.localScale = new Vector3(0.58f, 0.58f, 0);
                MonsterAtkTransform.localScale = new Vector3(0.58f, 0.58f, 0);
                MonsterAtk2Transform.localScale = new Vector3(0.58f, 0.58f, 0);
                MonsterBackAtk2Transform.localScale = new Vector3(0.58f, 0.58f, 0);
                MonsterStopTr.localScale = new Vector3(0.58f, 0.58f, 0);
                MonsterBeginAtkTransform.localScale = new Vector3(0.58f, 0.58f, 0);
                MonsterHowlTransform.localScale = new Vector3(0.58f, 0.58f, 0);
                BodyJudgement.localScale = new Vector3(1, 1, 0);
                break;
        }

        SwitchAnimation();

        //執行動作
        switch (status)
        {
            case Status.BeginAtk:
                if (AtkTimer <= 0)
                {
                    AtkTimer = BeginAtkTimerSet;
                }

                AtkTimer -= _deltaTime;

                if (AtkTimer <= (BeginAtkTimerSet - 1))
                {
                    if (!ParabolaCaculate)
                    {
                        //跳躍點計算
                        _parabolaVar.MiddlePoint = _transform.localPosition;
                        _parabolaVar.OtherPoint = new Vector3(_basicData.playerTransform.localPosition.x - 5.15f, MonsterBaseLine, 0);
                        if (_parabolaVar.OtherPoint.x < -14.3f)
                        {
                            _parabolaVar.OtherPoint = new Vector3(_basicData.playerTransform.localPosition.x + 5.15f, _parabolaVar.OtherPoint.y, 0);
                        }
                        //拋物線計算
                        _parabolaVar.HorizontalDirection = "Right";
                        _parabolaVar.VerticalDirection = "Down";
                        _parabolaVar.ParabolaConstant = Parabola.CalculateParabolaConstant(_parabolaVar);
                        _parabolaVar.ParabolaNowX = _parabolaVar.MiddlePoint.x;
                        _parabolaVar.Speed = Mathf.Abs(_parabolaVar.OtherPoint.x - _parabolaVar.MiddlePoint.x) / 0.6f;

                        ParabolaCaculate = true;
                    }
                }
                if (AtkTimer <= (BeginAtkTimerSet - 1.15))
                {
                    if (!BeginAtkJumpEnd)
                    {
                        Parabola.ParabolaMove(_parabolaVar, _deltaTime, _transform);
                        if (_transform.localPosition.y <= MonsterBaseLine)
                        {
                            _transform.localPosition = new Vector3(_transform.localPosition.x, MonsterBaseLine, 0);
                        }
                    }
                }
                if (AtkTimer <= (BeginAtkTimerSet - 1.75))
                {
                    if (!BeginAtkJumpEnd)
                    {
                        _basicData.TurnFaceJudge();
                        BeginAtkJumpEnd = true;
                    }
                }
                if (AtkTimer <= (BeginAtkTimerSet - 2.3))
                {
                    if (!FirstTrigger)
                    {
                        switch (_basicData.face)
                        {
                            case MonsterBasicData.Face.Left:
                                Instantiate(LBeginAtk, _transform.position, Quaternion.identity);
                                break;
                            case MonsterBasicData.Face.Right:
                                Instantiate(RBeginAtk, _transform.position, Quaternion.identity);
                                break;
                        }
                        FirstTrigger = true;
                    }
                }
                if (AtkTimer <= 0)
                {
                    FirstTrigger = false;
                    ParabolaCaculate = false;
                    BeginAtkJumpEnd = false;
                    ReturnToWait();
                }
                break;
        }
    }

    private void FixedUpdate()
    {
        _fixedDeltaTime = Time.fixedDeltaTime;

        _hurtedController.BurningTimerMethod(AtkTemporaryArea, _fixedDeltaTime);

        //return
        if (GameEvent.isAniPlay || _hurtedController.isCriticAtkHurted)
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
                            _transform.localPosition = new Vector3(_transform.localPosition.x + speed * _fixedDeltaTime, _transform.localPosition.y, 0);
                        }
                        break;
                    case MonsterBasicData.Face.Left:
                        if (!_basicData.touchLeftWall)
                        {
                            _transform.localPosition = new Vector3(_transform.localPosition.x - speed * _fixedDeltaTime, _transform.localPosition.y, 0);
                        }
                        break;
                }
                break;
            case Status.Atk1:
                if (AtkTimer <= 0)
                {
                    _basicData.TurnFaceJudge();
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
                    if (!Atk1End)
                    {
                        Atk1End = true;
                    }
                }
                if (AtkTimer <= 0)
                {
                    FirstTrigger = false;
                    Atk1End = false;
                    lastStatus = LastStatus.Atk1;
                    ReturnToWait();
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
                if (AtkTimer <= (Atk2TimerSet - 0.65))
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
            case Status.Jump:
                if (AtkTimer <= 0)
                {
                    _basicData.TurnFaceJudge();
                    AtkTimer = JumpTimerSet;
                }

                AtkTimer -= _fixedDeltaTime;

                if (AtkTimer<=(JumpTimerSet - 0.75))
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
                        switch (jumpMode)
                        {
                            case JumpMode.Atk:
                                _parabolaVar.MiddlePoint = new Vector3((_basicData.playerTransform.localPosition.x + _transform.localPosition.x) /2, _transform.localPosition.y + 5, 0);
                                _parabolaVar.Speed = Mathf.Abs(_basicData.playerTransform.localPosition.x - _transform.localPosition.x) / 0.8f;
                                break;
                            case JumpMode.Adjust:
                                if (_basicData.isPlayerAtLeftSide)
                                {
                                    _parabolaVar.MiddlePoint = new Vector3((_basicData.playerTransform.localPosition.x - 7 + _transform.localPosition.x) /2, _transform.localPosition.y + 5, 0);
                                }
                                if (_basicData.isPlayerAtRightSide)
                                {
                                    _parabolaVar.MiddlePoint = new Vector3((_basicData.playerTransform.localPosition.x + 7 + _transform.localPosition.x) / 2, _transform.localPosition.y + 5, 0);
                                }
                                _parabolaVar.Speed = Mathf.Abs(_parabolaVar.MiddlePoint.x - _transform.localPosition.x) * 2 / 0.8f;
                                break;
                        }
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
                        FirstTrigger = true;
                    }
                }
                if (AtkTimer<= (JumpTimerSet - 1.75))
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
                    if (jumpMode == JumpMode.Adjust)
                    {
                        status = Status.BackAtk2;
                        AnimationPreSet();
                    }
                    else
                    {
                        ReturnToWait();
                    }
                }
                break;
            case Status.BackAtk2:
                if (AtkTimer <= 0)
                {
                    _basicData.TurnFaceJudge();
                    AtkTimer = BackAtk2TimerSet;
                }

                AtkTimer -= _fixedDeltaTime;

                if (!BackAtk2MoveEnd1)
                {
                    switch (_basicData.face)
                    {
                        case MonsterBasicData.Face.Right:
                            if (!_basicData.touchLeftWall)
                            {
                                _transform.localPosition = new Vector3(_transform.localPosition.x - BackAtk2Speed1 * _fixedDeltaTime, _transform.localPosition.y, _transform.localPosition.z);
                            }
                            break;
                        case MonsterBasicData.Face.Left:
                            if (!_basicData.touchRightWall)
                            {
                                _transform.localPosition = new Vector3(_transform.localPosition.x + BackAtk2Speed1 * _fixedDeltaTime, _transform.localPosition.y, _transform.localPosition.z);
                            }
                            break;
                    }
                }
                if (AtkTimer <= (BackAtk2TimerSet - 0.42))
                {
                    if (!BackAtk2MoveEnd1)
                    {
                        BackAtk2MoveEnd1 = true;
                    }
                    if (!BackAtk2MoveEnd2)
                    {
                        switch (_basicData.face)
                        {
                            case MonsterBasicData.Face.Right:
                                if (!_basicData.touchLeftWall)
                                {
                                    _transform.localPosition = new Vector3(_transform.localPosition.x - BackAtk2Speed2 * _fixedDeltaTime, _transform.localPosition.y, _transform.localPosition.z);
                                }
                                break;
                            case MonsterBasicData.Face.Left:
                                if (!_basicData.touchRightWall)
                                {
                                    _transform.localPosition = new Vector3(_transform.localPosition.x + BackAtk2Speed2 * _fixedDeltaTime, _transform.localPosition.y, _transform.localPosition.z);
                                }
                                break;
                        }
                    }
                }
                if (AtkTimer <= (BackAtk2TimerSet - 0.85))
                {
                    if (!BackAtk2MoveEnd2)
                    {
                        Head.OpenMouth();
                        BackAtk2MoveEnd2 = true;
                    }
                }
                if (AtkTimer <= (BackAtk2TimerSet - 1.35))
                {
                    if (!FirstTrigger)
                    {
                        switch (_basicData.face)
                        {
                            case MonsterBasicData.Face.Left:
                                Instantiate(LAtk2, _transform.position + LBackAtk2Appear, Quaternion.identity);
                                break;
                            case MonsterBasicData.Face.Right:
                                Instantiate(RAtk2, _transform.position + RBackAtk2Appear, Quaternion.identity);
                                break;
                        }
                        FirstTrigger = true;
                    }
                }
                if (AtkTimer <= 0)
                {
                    BackAtk2MoveEnd1 = false;
                    BackAtk2MoveEnd2 = false;
                    FirstTrigger = false;
                    ReturnToWait();
                    lastStatus = LastStatus.BackAtk2;
                }
                break;
            /*case Status.BeginAtk:
                if (AtkTimer <= 0)
                {
                    AtkTimer = BeginAtkTimerSet;
                }

                AtkTimer -= _fixedDeltaTime;

                if (AtkTimer <= (BeginAtkTimerSet - 1))
                {
                    if (!ParabolaCaculate)
                    {
                        //跳躍點計算
                        _parabolaVar.MiddlePoint = _transform.localPosition;
                        _parabolaVar.OtherPoint = new Vector3(_basicData.playerTransform.localPosition.x - 5.15f, MonsterBaseLine, 0);
                        if (_parabolaVar.OtherPoint.x < -14.3f)
                        {
                            _parabolaVar.OtherPoint = new Vector3(_basicData.playerTransform.localPosition.x + 5.15f, _parabolaVar.OtherPoint.y, 0);
                        }
                        //拋物線計算
                        _parabolaVar.HorizontalDirection = "Right";
                        _parabolaVar.VerticalDirection = "Down";
                        _parabolaVar.ParabolaConstant = Parabola.CalculateParabolaConstant(_parabolaVar);
                        _parabolaVar.ParabolaNowX = _parabolaVar.MiddlePoint.x;
                        _parabolaVar.Speed = Mathf.Abs(_parabolaVar.OtherPoint.x - _parabolaVar.MiddlePoint.x) / 0.6f;

                        ParabolaCaculate = true;
                    }
                }
                if (AtkTimer <= (BeginAtkTimerSet - 1.15))
                {
                    if (!BeginAtkJumpEnd)
                    {
                        Parabola.ParabolaMove(_parabolaVar, _fixedDeltaTime, _transform);
                        if (_transform.localPosition.y <= MonsterBaseLine)
                        {
                            _transform.localPosition = new Vector3(_transform.localPosition.x, MonsterBaseLine, 0);
                        }
                    }
                }
                if (AtkTimer <= (BeginAtkTimerSet - 1.75))
                {
                    if (!BeginAtkJumpEnd)
                    {
                        _basicData.TurnFaceJudge();
                        BeginAtkJumpEnd = true;
                    }
                }
                if (AtkTimer <= (BeginAtkTimerSet - 2.3))
                {
                    if (!FirstTrigger)
                    {
                        switch (_basicData.face)
                        {
                            case MonsterBasicData.Face.Left:
                                Instantiate(LBeginAtk, _transform.position, Quaternion.identity);
                                break;
                            case MonsterBasicData.Face.Right:
                                Instantiate(RBeginAtk, _transform.position, Quaternion.identity);
                                break;
                        }
                        FirstTrigger = true;
                    }
                }
                if (AtkTimer <= 0)
                {
                    FirstTrigger = false;
                    ParabolaCaculate = false;
                    BeginAtkJumpEnd = false;
                    ReturnToWait();
                }
                break;*/
            case Status.Howl:
                if (AtkTimer <= 0)
                {
                    _basicData.TurnFaceJudge();
                    AtkTimer = HowlTimerSet;
                }

                AtkTimer -= _fixedDeltaTime;

                if (AtkTimer <= (HowlTimerSet - 0.25))
                {
                    if (!FirstTrigger)
                    {
                        Head.OpenMouth();
                        FirstTrigger = true;
                    }
                }
                if (AtkTimer <= (HowlTimerSet - 0.75))
                {
                    if (!SecondTrigger)
                    {
                        Instantiate(Howl, _transform.position, Quaternion.identity, AtkTemporaryArea);
                        AtkTemporaryArea.GetChild(0).GetComponent<FollowObject>().Target = Head.transform;
                        AtkTemporaryArea.DetachChildren();
                        SecondTrigger = true;
                    }
                }
                if (AtkTimer <= 0)
                {
                    lastStatus = LastStatus.Howl;
                    FirstTrigger = false;
                    SecondTrigger = false;
                    ReturnToWait();
                }
                break;
        }
    }

    private void SwitchAnimation()
    {
        if (status == Status.wait)
        {
            NowAni = AniStatus.wait;
        }
        if (status == Status.walk)
        {
            NowAni = AniStatus.walk;
        }
        if (status == Status.Atk1)
        {
            NowAni = AniStatus.Atk1;
        }
        if (status == Status.Atk2)
        {
            NowAni = AniStatus.Atk2;
        }
        if (status == Status.BackAtk2)
        {
            NowAni = AniStatus.BackAtk2;
        }
        if (status == Status.Jump)
        {
            NowAni = AniStatus.Jump;
        }
        if (status == Status.BeginAtk)
        {
            NowAni = AniStatus.Begining;
        }
        if (status == Status.Howl)
        {
            NowAni = AniStatus.Howl;
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
                    if (!_VBMonsterSE.SE2Appear)
                    {
                        _VBMonsterSE.TouchGroundSoundPlay();
                        _VBMonsterSE.SE2Appear = true;
                    }
                    MonsterJumpAni.SetBool("isGround", true);
                }
                break;
            case AniStatus.Atk1:
                AllAniFalse();
                MonsterAtkAnimation.SetActive(true);
                if (!_VBMonsterSE.SEAppear)
                {
                    _VBMonsterSE.AtkSoundPlay();
                    _VBMonsterSE.SEAppear = true;
                }
                if (Atk1End)
                {
                    MonsterAtk1Ani.SetInteger("Status", 1);
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
            case AniStatus.BackAtk2:
                AllAniFalse();
                MonsterBackAtk2Animation.SetActive(true);
                if (!_VBMonsterSE.SEAppear)
                {
                    _VBMonsterSE.BackAtkSoundPlay();
                    _VBMonsterSE.SEAppear = true;
                }
                if (!_VBMonsterSE.SE2Appear)
                {
                    _VBMonsterSE.Atk2SoundPlay("BackAtk2");
                    _VBMonsterSE.SE2Appear = true;
                }
                break;
            case AniStatus.Stop:
                AllAniFalse();
                MonsterStopAnimation.SetActive(true);
                break;
            case AniStatus.Begining:
                AllAniFalse();
                MonsterBeginAtkAni.SetBool("Atk", true);
                if (BeginAtkJumpEnd)
                {
                    if (!_VBMonsterSE.SEAppear)
                    {
                        _VBMonsterSE.BeginTouchSoundPlay();
                        _VBMonsterSE.SEAppear = true;
                    }
                }
                if (!_VBMonsterSE.SE3Appear)
                {
                    _VBMonsterSE.PrepareAtkSoundPlay();
                    _VBMonsterSE.SE3Appear = true;
                }
                if (!_VBMonsterSE.SE2Appear)
                {
                    _VBMonsterSE.BeginAtkSoundPlay();
                    _VBMonsterSE.SE2Appear = true;
                }
                if (!_VBMonsterSE.SE4Appear)
                {
                    _VBMonsterSE.BeginAtkWalkSoundPlay();
                    _VBMonsterSE.SE4Appear = true;
                }
                break;
            case AniStatus.Howl:
                AllAniFalse();
                MonsterHowlAnimation.SetActive(true);
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
        _VBMonsterSE.StopAllSE();
        AllAniFalse();
        Head.InitializeHead(StopHead);
        MonsterStopAnimation.SetActive(true);
    }

    private void StopAniCnange()
    {
        MonsterStopAni.SetBool("End", true);
        MonsterStopAni.SetBool("Return", true);
    }

    private void AllAniFalse()
    {
        if (NowAni != AniStatus.wait)
        {
            MonsterWaitAnimation.SetActive(false);
        }
        if (NowAni != AniStatus.walk)
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
            MonsterAtk1Ani.SetInteger("Status", 0);
        }
        if (NowAni != AniStatus.Atk2)
        {
            MonsterAtk2Animation.SetActive(false);
        }
        if (NowAni != AniStatus.BackAtk2)
        {
            MonsterBackAtk2Animation.SetActive(false);
        }
        if (NowAni != AniStatus.Stop)
        {
            MonsterStopAnimation.SetActive(false);
        }
        if (NowAni != AniStatus.Begining)
        {
            MonsterBeginAnimation.SetActive(false);
        }
        if (NowAni != AniStatus.Howl)
        {
            MonsterHowlAnimation.SetActive(false);
        }
    }

    private void StatusJudge()
    {
        while (!DefineStatus)
        {
            if (_basicData.AbsDistanceX <= CloseAtkDistance)
            {
                if (CloseToRightWall || CloseToLeftWall)
                {
                    switch (Random.Range(0, 2))
                    {
                        case 0:
                            if(lastStatus != LastStatus.Atk1)
                            {
                                status = Status.Atk1;
                                DefineStatus = true;
                            }
                            break;
                        case 1:
                            if (lastStatus != LastStatus.Jump)
                            {
                                status = Status.Jump;
                                jumpMode = JumpMode.Adjust;
                                DefineStatus = true;
                            }
                            break;
                    }
                }
                else
                {
                    if (_basicData.AbsDistanceX <= TooCloseDistance)
                    {
                        switch (Random.Range(0, 2))
                        {
                            case 0:
                                if (lastStatus != LastStatus.Jump)
                                {
                                    status = Status.Jump;
                                    jumpMode = JumpMode.Atk;
                                    DefineStatus = true;
                                }
                                break;
                            case 1:
                                if (lastStatus != LastStatus.BackAtk2)
                                {
                                    status = Status.BackAtk2;
                                    DefineStatus = true;
                                }
                                break;
                        }
                    }
                    else
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
                                if (lastStatus != LastStatus.Jump)
                                {
                                    status = Status.Jump;
                                    jumpMode = JumpMode.Atk;
                                    DefineStatus = true;
                                }
                                break;
                            case 2:
                                if (lastStatus != LastStatus.BackAtk2)
                                {
                                    status = Status.BackAtk2;
                                    DefineStatus = true;
                                }
                                break;
                        }
                    }
                }
            }
            else
            {
                switch (Random.Range(0, 2))
                {
                    case 0:
                        if (lastStatus != LastStatus.Atk2)
                        {
                            status = Status.Atk2;
                            DefineStatus = true;
                        }
                        break;
                    case 1:
                        if (lastStatus != LastStatus.walk)
                        {
                            status = Status.walk;
                            DefineStatus = true;
                        }
                        break;
                }
            }
            if (lastStatus == LastStatus.Jump && jumpMode == JumpMode.Adjust)
            {
                status = Status.BackAtk2;
                DefineStatus = true;
            }

            if (PlayerController.isDie)
            {
                if (lastStatus == LastStatus.Howl)
                {
                    status = Status.wait;
                }
                else
                {
                    status = Status.Howl;
                }
                DefineStatus = true;
            }
        }
        DefineStatus = false;

        _basicData.TurnFaceJudge();

        AnimationPreSet();
    }

    private void AnimationPreSet()//(1)
    {
        switch (status)
        {
            case Status.wait:
                Head.InitializeHead(WaitHeadValue);
                break;
            case Status.walk:
                Head.InitializeHead(WalkHeadValue);
                break;
            case Status.Jump:
                Head.InitializeHead(JumpHeadValue);
                break;
            case Status.Atk1:
                Head.InitializeHead(AtkHeadValue);
                break;
            case Status.Atk2:
                Head.InitializeHead(Atk2HeadValue);
                break;
            case Status.BackAtk2:
                Head.InitializeHead(BackAtk2HeadValue);
                break;
            case Status.BeginAtk:
                Head.InitializeHead(BegingingHeadValue);
                break;
            case Status.Howl:
                Head.InitializeHead(HowlHeadValue);
                break;
        }
    }

    private void ReturnToWait()//(1)
    {
        status = Status.wait;
        AnimationPreSet();
    }

    private void ContinueTypeSEControll()
    {
        if (NowAni == AniStatus.walk && _basicData.isGround)
        {
            _VBMonsterSE.WalkSoundPlay();
        }

        if (NowAni != AniStatus.walk)
        {
            _VBMonsterSE.TurnOffWalkSound();
        }
    }
}
