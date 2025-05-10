using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SmallCaptainController : MonoBehaviour
{
    [Header("基本參數")]
    public int ChasingDistance;
    public int GiveUpDistanceX;
    public int GiveUpDistanceY;
    private int AlertDistance = 10;
    private float AtkDistance = 2;
    private float Atk2Distance = 9;
    private float speed = 4.5f;
    private float SlowWalkSpeed = 1.5f;
    private float AtkSpeed = 3.447f;
    public float Atk2Speed;
    private float Atk3FallSpeed = 10;
    public enum Status { Standby, Alert, Prepare, walk, jump, Atk, Atk2, Atk3, Summon, Block, BeBlock, Weak };
    [HideInInspector] public Status status;
    private enum SlowWalkStatus { walkLeft, wait, walkRight };
    private SlowWalkStatus slowWalkStatus;
    [SerializeField] private SlowWalkStatus InitialSlowWalkStatus;

    private enum Atk2Status { Default, Phase1, Phase2}
    private Atk2Status atk2Status;

    private enum Atk3Status { Default, Success, SuccessEnd, Fail }
    private Atk3Status atk3Status;
    public enum AniStatus { Wait, Walk, SlowMove, Atk, Atk2, Atk3, Summon, Jump, Stop, BeBlock, Block }
    [HideInInspector] public AniStatus NowAni;//script(Hurted)
    private AniStatus LastAni;

    [Header("系統參數")]
    private Transform _transform;
    private float _deltaTime;
    private float _fixDeltaTime;
    private float _time;
    private Transform AtkTemporaryArea;
    private GameObject MonsterRightJudgement;
    private GameObject MonsterLeftJudgement;
    private GameObject MonsterUnderJudgement;
    private MonsterBasicData _basicData;
    private MonsterHurtedController _hurtedController;
    private MonsterJumpController _jumpController;
    private MonsterTouchTrigger _touchTrigger;
    private SwordManSE _swordManSE;
    private float StandbyTimer = 0.1f;
    private float AtkTimerSet = 1.1f;
    private float AtkTimer;
    private float Atk2TimerSet = 1.5f;
    private float Atk3TimerSet = 3.9f;
    private float Atk3EndTimerSet = 1.5f;
    private float SummonTimerSet = 1.5f;
    private float SummonCoolDown = 20;
    private float SummonLastTime = -100;
    private float AtkCoolDown = 1.5f;//2種Atk共用
    private float AtkLastTime = -10;
    public float Atk2StopDistance;
    private float Atk3CoolDown = 20;
    private float Atk3LastTime = -100;
    private float SlowWalkTimer;
    private float AlertSlowWalkTimerSet = 14;
    private float PrepareSlowWalkTimerSet = 4;
    private float PrepareSlowWalkTurnTime = 2;
    private float LittleWaitTimerSet = 1;
    private float LittleWaitTimer;
    private float TurnFaceLagTimerSet = 0.2f;
    private float TurnFaceLagTimer;
    private Rigidbody2D Rigid2D;
    private BoxCollider2D _boxCollider;
    private Transform MoveAtkTarget;
    private float GroundPointY;

    //開關
    private bool isDroneExist = false;
    private bool CanSummon;
    private bool FirstTrigger;
    private bool SecondTrigger;
    private bool AtkMove = true;
    private bool CanAtk;
    private bool CanAtk3;
    private bool isLittleWait;
    private bool isBackWalk;
    private bool DefineStatus;

    [Header("動畫相關物件")]
    private Animator MonsterMoveAni;
    private Animator MonsterSlowMoveAni;
    private Animator MonsterAtk2Ani;
    private Animator MonsterAtk3Ani;
    private GameObject MonsterMoveAnimation;
    private GameObject MonsterSlowMoveAnimation;
    private GameObject MonsterAtkAnimation;
    private GameObject MonsterAtk2Animation;
    private GameObject MonsterAtk3Animation;
    private GameObject MonsterJumpAnimation;
    private GameObject MonsterSummonAnimation;
    private Transform MonsterMoveTr;
    private Transform MonsterAtkTr;
    private Transform MonsterAtk2Tr;
    private Transform MonsterAtk3Tr;
    private Transform MonsterSlowMoveTr;
    private Transform MonsterJumpTr;
    private Transform MonsterSummonTr;

    [Header("攻擊物件")]
    public GameObject RAtk;
    public GameObject LAtk;
    public GameObject RAtk1_2;
    public GameObject LAtk1_2;
    public GameObject RAtk3;
    public GameObject LAtk3;
    private GameObject _atk3;
    public GameObject RAtk2;
    public GameObject LAtk2;
    public GameObject SummonDrone;
    private float DroneAppearY = 5;
    [HideInInspector] public GameObject SummonMonster;

    [Header("被大招攻擊")]
    private GameObject MonsterStopAnimation;
    private Transform MonsterStopTr;

    [Header("格檔")]
    private MonsterBlockController _blockController;
    private WeakData _weakData = new WeakData();
    private GameObject MonsterBeBlockAnimation;
    private Animator MonsterBeBlockAni;
    private Transform MonsterBeBlockTr;
    //主動格檔
    private bool CanBlock;
    private float BlockCoolDown = 10;
    private float BlockLastTime;
    private GameObject MonsterBlockAnimation;
    private Animator MonsterBlockAni;
    private Transform MonsterBlockTr;

    void Start()
    {
        _transform = this.transform;
        Rigid2D = this.transform.GetComponent<Rigidbody2D>();
        _boxCollider = this.transform.GetComponent<BoxCollider2D>();
        _basicData = this.GetComponent<MonsterBasicData>();
        _hurtedController = this.GetComponent<MonsterHurtedController>();
        _blockController = this.GetComponent<MonsterBlockController>();
        _jumpController = this.GetComponent<MonsterJumpController>();
        _swordManSE = this.GetComponent<SwordManSE>();
        AtkTemporaryArea = this.transform.GetChild(5);

        //抓取動畫相關物件
        MonsterMoveAnimation = this.transform.GetChild(0).gameObject;
        MonsterSlowMoveAnimation = this.transform.GetChild(4).gameObject;
        MonsterAtkAnimation = this.transform.GetChild(1).gameObject;
        MonsterAtk2Animation = this.transform.GetChild(2).gameObject;
        MonsterAtk3Animation = this.transform.GetChild(3).gameObject;
        MonsterStopAnimation = this.transform.GetChild(6).gameObject;
        MonsterBeBlockAnimation = this.transform.GetChild(7).gameObject;
        MonsterBlockAnimation = this.transform.GetChild(8).gameObject;
        MonsterJumpAnimation = this.transform.GetChild(9).gameObject;
        MonsterSummonAnimation = this.transform.GetChild(10).gameObject;

        MonsterMoveTr = MonsterMoveAnimation.transform.GetChild(0).transform;
        MonsterAtkTr = MonsterAtkAnimation.transform.GetChild(0).transform;
        MonsterAtk2Tr = MonsterAtk2Animation.transform.GetChild(0).transform;
        MonsterAtk3Tr = MonsterAtk3Animation.transform.GetChild(0).transform;
        MonsterSlowMoveTr = MonsterSlowMoveAnimation.transform.GetChild(0).transform;
        MonsterStopTr = MonsterStopAnimation.transform.GetChild(0).transform;
        MonsterBeBlockTr = MonsterBeBlockAnimation.transform.GetChild(0).transform;
        MonsterBlockTr = MonsterBlockAnimation.transform.GetChild(0).transform;
        MonsterJumpTr = MonsterJumpAnimation.transform.GetChild(0).transform;
        MonsterSummonTr = MonsterSummonAnimation.transform.GetChild(0).transform;

        MonsterMoveAni = MonsterMoveTr.GetComponent<Animator>();
        MonsterSlowMoveAni = MonsterSlowMoveTr.GetComponent<Animator>();
        MonsterAtk2Ani = MonsterAtk2Tr.GetComponent<Animator>();
        MonsterAtk3Ani = MonsterAtk3Tr.GetComponent<Animator>();
        MonsterBeBlockAni = MonsterBeBlockTr.GetComponent<Animator>();
        MonsterBlockAni = MonsterBlockTr.GetComponent<Animator>();

        if (!_basicData.SpecialOpening)
        {
            status = Status.Standby;
        }
        NowAni = AniStatus.Wait;
        MatchInitialSlowWalk();
        atk3Status = Atk3Status.Default;
        atk2Status = Atk2Status.Default;
        _basicData.BasicVarInisialize(MonsterSlowMoveTr, "R");

        TurnFaceLagTimer = TurnFaceLagTimerSet;
        MonsterRightJudgement = AtkTemporaryArea.GetChild(0).gameObject;
        MonsterLeftJudgement = AtkTemporaryArea.GetChild(1).gameObject;
        MonsterUnderJudgement = AtkTemporaryArea.GetChild(2).gameObject;
        MoveAtkTarget = AtkTemporaryArea.GetChild(3).GetChild(0);
        AtkTemporaryArea.DetachChildren();

        WeakDataInisialize();

        _hurtedController._getCriticHurted += PlayStopAni;

        //初始化MonsterTouch系統
        InisializMonsterTouchTrigger();
    }

    // Update is called once per frame
    void Update()
    {
        _time = Time.time;
        _deltaTime = Time.deltaTime;
        _basicData.DieJudge();

        //多種變數重置管理
        if (_hurtedController.isCriticAtkHurted || _hurtedController.isHurtedByBigGun)
        {
            _boxCollider.offset = new Vector2(0, 0);
            _boxCollider.size = new Vector2(0.3f, 1.8f);
            Rigid2D.mass = 50;
            AtkMove = true;
            CanAtk = false;
            CanBlock = false;
            FirstTrigger = false;
            SecondTrigger = false;
            AtkLastTime = _time;
            BlockLastTime = _time;
            SummonLastTime = _time;
            if (_atk3 != null)
            {
                Destroy(_atk3);
            }
            _jumpController.AllVariableFalse();
            _blockController.AllVariableReset();
            atk2Status = Atk2Status.Default;
            atk3Status = Atk3Status.Default;
        }
        if (_blockController.BeBlockSuccess)
        {
            status = Status.BeBlock;
            AtkTimer = 0;
            AtkMove = true;
            CanAtk = false;
            FirstTrigger = false;
            SecondTrigger = false;
            AtkLastTime = _time;
        }

        _hurtedController.HurtedTimerMethod(_deltaTime);
        _hurtedController.CriticAtkHurtedTimerMethod(_deltaTime);

        //判斷是否觸地
        _basicData.CheckTouchGround(ref GroundPointY);
        if (_basicData.isGround && !_hurtedController.isCriticAtkHurted)
        {
            _transform.position = new Vector3(_transform.position.x, GroundPointY + 0.99f, 0);
        }
        if (!_basicData.isGround)
        {
            Rigid2D.gravityScale = 7;
        }
        //判斷是否碰到牆壁
        _basicData.CheckTouchWall();

        //寫入實際位置
        _basicData.MonsterPlace = _transform.position;

        //return
        if (GameEvent.isAniPlay || _hurtedController.isHurtedByBigGun || _hurtedController.isCriticAtkHurted || PauseMenuController.isPauseMenuOpen)
        {
            return;
        }

        if (SummonMonster == null)
        {
            isDroneExist = false;
        }
        else
        {
            isDroneExist = true;
        }
        //計算距離
        _basicData.DistanceCalculate();
        //冷卻時間計算
        _basicData.CoolDownCalculate(_time, AtkLastTime, AtkCoolDown, ref CanAtk);
        _basicData.CoolDownCalculate(_time, Atk3LastTime, Atk3CoolDown, ref CanAtk3);
        _basicData.CoolDownCalculate(_time, BlockLastTime, BlockCoolDown, ref CanBlock);
        if (!isDroneExist)
        {
            _basicData.CoolDownCalculate(_time, SummonLastTime, SummonCoolDown, ref CanSummon);
        }

        //被大招打之後的動作
        if (_hurtedController.BeCriticAtkEnd)
        {
            status = Status.walk;
            _hurtedController.BeCriticAtkEnd = false;
        }

        //怪物AI
        switch (status)
        {
            case Status.Standby:
                StandbyTimer -= _deltaTime;

                if (StandbyTimer <= 0)
                {
                    status = Status.Alert;
                }
                break;
            case Status.Alert:
                if (_basicData.playerTransform)
                {
                    SlowWalk();

                    //被遠程打時的反應
                    if (_hurtedController.HurtedByFarAtk)
                    {
                        SlowWalkTimer = 0;
                        StatusJudge();
                        _basicData.TurnFaceJudge();
                        if (status == Status.Alert)
                        {
                            status = Status.walk;
                        }
                    }
                    if (_hurtedController.HurtedByFarAtk)
                    {
                        _hurtedController.HurtedByFarAtk = false;
                    }

                    if (_basicData.AbsDistanceX < ChasingDistance && _basicData.AbsDistanceY <= 2)
                    {
                        SlowWalkTimer = 0;
                        StatusJudge(Status.walk);
                        _basicData.TurnFaceJudge();
                    }
                }
                break;
            case Status.Prepare:
                _basicData.LagTurnFaceJudge(ref TurnFaceLagTimer, TurnFaceLagTimerSet, _deltaTime);

                SlowWalk();

                if(_basicData.touchLeftWall || _basicData.touchRightWall)
                {
                    SlowWalkTimer = PrepareSlowWalkTurnTime;
                }

                StatusJudge();

                if (status != Status.Prepare)
                {
                    SlowWalkTimer = 0;
                    FirstTrigger = false;
                }
                break;
            case Status.walk:
                CanAtk = true;

                if (_basicData.ReverseWalk)
                {
                    _basicData.ReverseLagTurnFaceJudge(ref TurnFaceLagTimer, TurnFaceLagTimerSet, _deltaTime);
                }
                else
                {
                    _basicData.LagTurnFaceJudge(ref TurnFaceLagTimer, TurnFaceLagTimerSet, _deltaTime);
                }

                StatusJudge();
                break;
            case Status.jump:
                _jumpController.isJumping = true;
                _jumpController.JumpEndJudge();
                if (_jumpController.JumpEnd)
                {
                    status = Status.walk;
                    _jumpController.JumpEnd = false;
                }
                break;
            case Status.Block:
                switch (_basicData.face)
                {
                    case MonsterBasicData.Face.Right:
                        _blockController.FaceLeft = false;
                        _blockController.FaceRight = true;
                        break;
                    case MonsterBasicData.Face.Left:
                        _blockController.FaceLeft = true;
                        _blockController.FaceRight = false;
                        break;
                }
                _blockController.BlockSuccessTimerMethod(_deltaTime);
                BlockEndJudge();
                break;
            case Status.BeBlock:
                _blockController.BeBlockTimerMethod(_deltaTime);
                BeBlockEndJudge();
                break;
            case Status.Weak:
                _blockController.isWeak = true;
                switch (_basicData.face)
                {
                    case MonsterBasicData.Face.Left:
                        _weakData.WeakOffsetX = Mathf.Abs(_weakData.WeakOffsetX);
                        break;
                    case MonsterBasicData.Face.Right:
                        _weakData.WeakOffsetX = -Mathf.Abs(_weakData.WeakOffsetX);
                        break;
                }
                _blockController.WeakTimerMethod(_weakData, _deltaTime);
                if (_blockController.isWeakEndJudge)
                {
                    _basicData.TurnFaceJudge();
                    status = Status.Prepare;
                    _blockController.isWeakEndJudge = false;
                }
                break;
        }

        if (status == Status.Block || status == Status.BeBlock || status == Status.Weak || status == Status.Summon)
        {
            _basicData.ShouldIgnore = true;
        }
        else
        {
            _basicData.ShouldIgnore = false;
        }

        //轉向控制
        switch (_basicData.face)
        {
            case MonsterBasicData.Face.Left:
                MonsterMoveTr.localScale = new Vector3(-0.28f, 0.28f, 1);
                MonsterAtkTr.localScale = new Vector3(-0.28f, 0.28f, 1);
                MonsterAtk2Tr.localScale = new Vector3(-0.28f, 0.28f, 1);
                MonsterAtk3Tr.localScale = new Vector3(-0.28f, 0.28f, 1);
                MonsterSlowMoveTr.localScale = new Vector3(-0.28f, 0.28f, 1);
                MonsterStopTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                MonsterBeBlockTr.localScale = new Vector3(-0.28f, 0.28f, 1);
                MonsterBlockTr.localScale = new Vector3(-0.28f, 0.28f, 1);
                MonsterJumpTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                MonsterSummonTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                break;

            case MonsterBasicData.Face.Right:
                MonsterMoveTr.localScale = new Vector3(0.28f, 0.28f, 1);
                MonsterAtkTr.localScale = new Vector3(0.28f, 0.28f, 1);
                MonsterAtk2Tr.localScale = new Vector3(0.28f, 0.28f, 1);
                MonsterAtk3Tr.localScale = new Vector3(0.28f, 0.28f, 1);
                MonsterSlowMoveTr.localScale = new Vector3(0.28f, 0.28f, 1);
                MonsterStopTr.localScale = new Vector3(0.28f, 0.28f, 0);
                MonsterBeBlockTr.localScale = new Vector3(0.28f, 0.28f, 1);
                MonsterBlockTr.localScale = new Vector3(0.28f, 0.28f, 1);
                MonsterJumpTr.localScale = new Vector3(0.28f, 0.28f, 0);
                MonsterSummonTr.localScale = new Vector3(0.28f, 0.28f, 0);
                break;
        }
        SwitchAnimation();
    }

    private void FixedUpdate()
    {
        _fixDeltaTime = Time.fixedDeltaTime;

        //return
        if (GameEvent.isAniPlay || _hurtedController.isHurtedByBigGun || _hurtedController.isCriticAtkHurted)
        {
            return;
        }

        switch (status)
        {
            case Status.walk:
                if (!_hurtedController.isHurted)
                {
                    switch (_basicData.face)
                    {
                        case MonsterBasicData.Face.Right:
                            if (!_basicData.touchRightWall && !MonsterRightJudgement.GetComponent<MonsterRightJudgement>().isPlayerAtRightSide)
                            {
                                if (!MonsterUnderJudgement.GetComponent<MonsterUnderJudgement>().isPlayerUnder)
                                {
                                    _transform.position += new Vector3(speed * _fixDeltaTime, 0, 0);
                                }
                            }
                            break;
                        case MonsterBasicData.Face.Left:
                            if (!_basicData.touchLeftWall && !MonsterLeftJudgement.GetComponent<MonsterLeftJudgement>().isPlayerAtLeftSide)
                            {
                                if (!MonsterUnderJudgement.GetComponent<MonsterUnderJudgement>().isPlayerUnder)
                                {
                                    _transform.position -= new Vector3(speed * _fixDeltaTime, 0, 0);
                                }
                            }
                            break;
                    }
                }
                break;
            case Status.Atk:
                if (AtkTimer <= 0)
                {
                    AtkTimer = AtkTimerSet;
                }

                AtkTimer -= _fixDeltaTime;

                if (AtkTimer <= (AtkTimerSet - 0.3))
                {
                    if (!FirstTrigger)
                    {
                        switch (_basicData.face)
                        {
                            case MonsterBasicData.Face.Right:
                                Instantiate(RAtk, _transform.position, Quaternion.identity, AtkTemporaryArea);
                                AtkTemporaryArea.DetachChildren();
                                break;
                            case MonsterBasicData.Face.Left:
                                Instantiate(LAtk, _transform.position, Quaternion.identity, AtkTemporaryArea);
                                AtkTemporaryArea.DetachChildren();
                                break;
                        }
                        FirstTrigger = true;
                    }
                }
                if (AtkTimer <= (AtkTimerSet - 0.6))
                {
                    if (AtkMove)
                    {
                        switch (_basicData.face)
                        {
                            case MonsterBasicData.Face.Right:
                                if (!_basicData.touchRightWall && !MonsterRightJudgement.GetComponent<MonsterRightJudgement>().isPlayerAtRightSide)
                                {
                                    if (!MonsterUnderJudgement.GetComponent<MonsterUnderJudgement>().isPlayerUnder)
                                    {
                                        _transform.position += new Vector3(AtkSpeed * _fixDeltaTime, 0, 0);
                                    }
                                }
                                break;
                            case MonsterBasicData.Face.Left:
                                if (!_basicData.touchLeftWall && !MonsterLeftJudgement.GetComponent<MonsterLeftJudgement>().isPlayerAtLeftSide)
                                {
                                    if (!MonsterUnderJudgement.GetComponent<MonsterUnderJudgement>().isPlayerUnder)
                                    {
                                        _transform.position -= new Vector3(AtkSpeed * _fixDeltaTime, 0, 0);
                                    }
                                }
                                break;
                        }
                    }
                }
                if (AtkTimer <= (AtkTimerSet - 0.75))
                {
                    if (!SecondTrigger)
                    {
                        switch (_basicData.face)
                        {
                            case MonsterBasicData.Face.Right:

                                Instantiate(RAtk1_2, _transform.position, Quaternion.identity, AtkTemporaryArea);
                                AtkTemporaryArea.DetachChildren();
                                break;
                            case MonsterBasicData.Face.Left:

                                Instantiate(LAtk1_2, _transform.position, Quaternion.identity, AtkTemporaryArea);
                                AtkTemporaryArea.DetachChildren();
                                break;
                        }
                        SecondTrigger = true;
                    }
                }
                if (AtkTimer <= (AtkTimerSet - 0.81))
                {
                    AtkMove = false;
                }
                if (AtkTimer <= 0)
                {
                    _basicData.TurnFaceJudge();
                    AtkMove = true;
                    AtkLastTime = _time;
                    CanAtk = false;
                    status = Status.Prepare;
                    FirstTrigger = false;
                    SecondTrigger = false;
                }
                break;
            case Status.Atk2:
                if (AtkTimer <= 0 && atk2Status == Atk2Status.Default)
                {
                    _basicData.TurnFaceJudge();
                    AtkTimer = Atk2TimerSet;
                    atk2Status = Atk2Status.Phase1;
                }

                AtkTimer -= _fixDeltaTime;

                if (AtkTimer <= (Atk2TimerSet - 0.6) && atk2Status == Atk2Status.Phase1)
                {
                    switch (_basicData.face)
                    {
                        case MonsterBasicData.Face.Right:
                            if (!_basicData.touchRightWall)
                            {
                                _transform.position += new Vector3(Atk2Speed * _fixDeltaTime, 0, 0);
                            }
                            break;
                        case MonsterBasicData.Face.Left:
                            if (!_basicData.touchLeftWall)
                            {
                                _transform.position -= new Vector3(Atk2Speed * _fixDeltaTime, 0, 0);
                            }
                            break;
                    }

                    if (_basicData.AbsDistanceX <= Atk2StopDistance)
                    {
                        atk2Status = Atk2Status.Phase2;
                        AtkTimer = Atk2TimerSet;
                    }
                    switch (_basicData.face)
                    {
                        case MonsterBasicData.Face.Right:
                            if (_basicData.DistanceX > 0)
                            {
                                atk2Status = Atk2Status.Phase2;
                                AtkTimer = Atk2TimerSet;
                            }
                            break;
                        case MonsterBasicData.Face.Left:
                            if (_basicData.DistanceX < 0)
                            {
                                atk2Status = Atk2Status.Phase2;
                                AtkTimer = Atk2TimerSet;
                            }
                            break;
                    }
                }
                if (atk2Status == Atk2Status.Phase2)
                {
                    if (AtkTimer <= (Atk2TimerSet - 0.25))
                    {
                        if (!FirstTrigger)
                        {
                            _basicData.TurnFaceJudge();
                            FirstTrigger = true;
                        }
                    }
                    if (AtkTimer <= (Atk2TimerSet - 0.68))
                    {
                        if (!SecondTrigger)
                        {
                            switch (_basicData.face)
                            {
                                case MonsterBasicData.Face.Right:
                                    Instantiate(RAtk2, _transform.position, Quaternion.identity, AtkTemporaryArea);
                                    AtkTemporaryArea.DetachChildren();
                                    break;
                                case MonsterBasicData.Face.Left:
                                    Instantiate(LAtk2, _transform.position, Quaternion.identity, AtkTemporaryArea);
                                    AtkTemporaryArea.DetachChildren();
                                    break;
                            }
                            SecondTrigger = true;
                        }
                    }
                    if (AtkTimer <= 0)
                    {
                        _basicData.TurnFaceJudge();
                        AtkLastTime = _time;
                        CanAtk = false;
                        status = Status.Prepare;
                        atk2Status = Atk2Status.Default;
                        FirstTrigger = false;
                        SecondTrigger = false;
                    }
                }
                break;
            case Status.Atk3:
                if (AtkTimer <= 0 && atk3Status == Atk3Status.Default)
                {
                    AtkTimer = Atk3TimerSet;
                }

                AtkTimer -= _fixDeltaTime;

                if (AtkTimer <= (Atk3TimerSet - 0.9) && atk3Status == Atk3Status.Default)
                {
                    if (!GameEvent.ControllerRoomCasterDie)
                    {
                        atk3Status = Atk3Status.Success;
                    }
                    else
                    {
                        atk3Status = Atk3Status.Fail;
                    }
                }
                if (atk3Status == Atk3Status.Success)
                {
                    if (AtkTimer <= (Atk3TimerSet - 1.4))
                    {
                        if (!FirstTrigger)
                        {
                            _boxCollider.isTrigger = true;
                            Rigid2D.gravityScale = 0;
                            switch (_basicData.face)
                            {
                                case MonsterBasicData.Face.Right:
                                    Instantiate(RAtk3, _transform.localPosition, Quaternion.identity, AtkTemporaryArea);
                                    break;
                                case MonsterBasicData.Face.Left:
                                    Instantiate(LAtk3, _transform.localPosition, Quaternion.identity, AtkTemporaryArea);
                                    break;
                            }
                            _atk3 = AtkTemporaryArea.transform.GetChild(0).gameObject;
                            AtkTemporaryArea.DetachChildren();
                            _transform.localPosition = new Vector3(_basicData.playerTransform.localPosition.x, MoveAtkTarget.position.y, 0);
                            FirstTrigger = true;
                        }
                        _transform.localPosition = new Vector3(_transform.localPosition.x, _transform.localPosition.y - Atk3FallSpeed * _fixDeltaTime, 0);
                    }
                    if (_basicData.isGround && AtkTimer <= (Atk3TimerSet - 1.5))
                    {
                        atk3Status = Atk3Status.SuccessEnd;
                        AtkTimer = Atk3EndTimerSet;
                        _boxCollider.isTrigger = false;
                        Rigid2D.gravityScale = 7;
                    }
                }
                if (atk3Status == Atk3Status.SuccessEnd)
                {
                    if (_atk3 != null)
                    {
                        Destroy(_atk3);
                    }
                    if (AtkTimer <= 0)
                    {
                        CanAtk3 = false;
                        Atk3LastTime = _time;
                        FirstTrigger = false;
                        atk3Status = Atk3Status.Default;
                        status = Status.Prepare;
                    }
                }
                if (atk3Status == Atk3Status.Fail && AtkTimer <= 0)
                {
                    CanAtk3 = false;
                    Atk3LastTime = _time;
                    atk3Status = Atk3Status.Default;
                    status = Status.Prepare;
                }
                break;
            case Status.Summon:
                if (AtkTimer <= 0)
                {
                    AtkTimer = SummonTimerSet;
                }

                AtkTimer -= _fixDeltaTime;

                if (AtkTimer <= (SummonTimerSet - 0.75))
                {
                    if (!FirstTrigger)
                    {
                        Instantiate(SummonDrone, new Vector3(_transform.position.x, _transform.position.y + DroneAppearY, _transform.position.z), SummonDrone.transform.rotation, AtkTemporaryArea);
                        AtkTemporaryArea.DetachChildren();
                        FirstTrigger = true;
                        CanSummon = false;
                        SummonLastTime = _time;
                    }
                }
                if (AtkTimer <= 0)
                {
                    SummonLastTime = _time;
                    FirstTrigger = false;
                    StatusJudge(Status.walk);
                }
                break;
            case Status.Block:
                _blockController.BlockTimerMethod(_fixDeltaTime);
                break;
        }

        if (status == Status.Alert || status == Status.Prepare)
        {
            switch (slowWalkStatus)
            {
                case SlowWalkStatus.walkLeft:
                    if (!_basicData.touchLeftWall && !_hurtedController.isHurted && !MonsterLeftJudgement.GetComponent<MonsterLeftJudgement>().isPlayerAtLeftSide)
                    {
                        if (!MonsterUnderJudgement.GetComponent<MonsterUnderJudgement>().isPlayerUnder)
                        {
                            _transform.position -= new Vector3(SlowWalkSpeed * _fixDeltaTime, 0, 0);
                        }
                    }
                    break;

                case SlowWalkStatus.walkRight:
                    if (!_basicData.touchRightWall && !_hurtedController.isHurted && !MonsterRightJudgement.GetComponent<MonsterRightJudgement>().isPlayerAtRightSide)
                    {
                        if (!MonsterUnderJudgement.GetComponent<MonsterUnderJudgement>().isPlayerUnder)
                        {
                            _transform.position += new Vector3(SlowWalkSpeed * _fixDeltaTime, 0, 0);
                        }
                    }
                    break;
            }
        }

        _jumpController.Jump(_fixDeltaTime);

        _hurtedController.HurtedMove(_fixDeltaTime);
    }

    void SwitchAnimation()
    {
        if (status != Status.BeBlock && status != Status.Block)
        {
            NowAni = AniStatus.Wait;
        }

        if (status == Status.walk)
        {
            NowAni = AniStatus.Walk;
        }

        if (status == Status.Alert || status == Status.Prepare)
        {
            NowAni = AniStatus.SlowMove;
        }

        if (status == Status.Atk)
        {
            NowAni = AniStatus.Atk;
        }

        if (status == Status.Atk2)
        {
            NowAni = AniStatus.Atk2;
        }

        if (status == Status.Atk3)
        {
            NowAni = AniStatus.Atk3;
        }

        if (status == Status.Summon)
        {
            NowAni = AniStatus.Summon;
        }

        if (_blockController.BeBlockSuccess && _blockController.CanBeBlockAniAppear)
        {
            _blockController.CanBeBlockAniAppear = false;
            NowAni = AniStatus.BeBlock;
        }

        if (_blockController.isWeak)
        {
            NowAni = AniStatus.BeBlock;
        }

        if (status == Status.Block)
        {
            NowAni = AniStatus.Block;
        }

        if (status == Status.jump)
        {
            NowAni = AniStatus.Jump;
        }

        if (LastAni != NowAni)
        {
            _swordManSE.SEAppear = false;
            _swordManSE.SE2Appear = false;
            _swordManSE.SE3Appear = false;
        }

        switch (NowAni)
        {
            case AniStatus.Wait:
                AllAniFalse();
                MonsterMoveAnimation.SetActive(true);
                MonsterMoveAni.SetBool("isWalking", false);
                break;
            case AniStatus.Walk:
                AllAniFalse();
                MonsterMoveAnimation.SetActive(true);
                MonsterMoveAni.SetBool("isWalking", true);
                break;
            case AniStatus.SlowMove:
                AllAniFalse();
                MonsterSlowMoveAnimation.SetActive(true);
                if (slowWalkStatus != SlowWalkStatus.wait)
                {
                    MonsterSlowMoveAni.SetBool("isSlowWalk", true);
                }
                else
                {
                    MonsterSlowMoveAni.SetBool("isSlowWalk", false);
                }
                if (!isBackWalk)
                {
                    MonsterSlowMoveAni.SetFloat("Speed", 1);
                }
                else
                {
                    MonsterSlowMoveAni.SetFloat("Speed", -1);
                }
                break;
            case AniStatus.Atk:
                AllAniFalse();
                MonsterAtkAnimation.SetActive(true);
                if (!_swordManSE.SEAppear)
                {
                    _swordManSE.AtkSound1Play();
                    _swordManSE.SEAppear = true;
                }
                if (!_swordManSE.SE2Appear)
                {
                    _swordManSE.AtkSound2Play();
                    _swordManSE.SE2Appear = true;
                }
                break;
            case AniStatus.Atk2:
                AllAniFalse();
                MonsterAtk2Animation.SetActive(true);
                if (!_swordManSE.SEAppear)
                {
                    _swordManSE.StringSoundPlay();
                    _swordManSE.SEAppear = true;
                }
                if (atk2Status != Atk2Status.Phase2)
                {
                    MonsterAtk2Ani.SetBool("Continue", false);
                }
                if (atk2Status == Atk2Status.Phase2)
                {
                    MonsterAtk2Ani.SetBool("Continue", true);
                    if (!_swordManSE.SE2Appear)
                    {
                        _swordManSE.StringAtk2SoundPlay();
                        _swordManSE.SE2Appear = true;
                    }
                }
                break;
            case AniStatus.Atk3:
                AllAniFalse();
                MonsterAtk3Animation.SetActive(true);
                if (!_swordManSE.SEAppear)
                {
                    _swordManSE.TakeSwordSoundPlay();
                    _swordManSE.SEAppear = true;
                }
                if (atk3Status == Atk3Status.Success)
                {
                    MonsterAtk3Ani.SetBool("Success", true);
                    if (!_swordManSE.SE2Appear)
                    {
                        _swordManSE.FallAtkSoundPlay();
                        _swordManSE.SE2Appear = true;
                    }
                }
                if (atk3Status == Atk3Status.Fail)
                {
                    MonsterAtk3Ani.SetBool("Fail", true);
                }
                if (atk3Status == Atk3Status.SuccessEnd)
                {
                    MonsterAtk3Ani.SetBool("isGround", true);
                    if (!_swordManSE.SE3Appear)
                    {
                        _swordManSE.FallGroundSoundPlay();
                        _swordManSE.SE3Appear = true;
                    }
                }
                break;
            case AniStatus.Summon:
                AllAniFalse();
                MonsterSummonAnimation.SetActive(true);
                break;
            case AniStatus.Jump:
                AllAniFalse();
                MonsterJumpAnimation.SetActive(true);
                break;
            case AniStatus.Stop:
                AllAniFalse();
                MonsterStopAnimation.SetActive(true);
                break;
            case AniStatus.BeBlock:
                AllAniFalse();
                MonsterBeBlockAnimation.SetActive(true);
                if (_blockController.isWeak)
                {
                    MonsterBeBlockAni.SetBool("GetHurted", true);
                }
                break;
            case AniStatus.Block:
                AllAniFalse();
                MonsterBlockAnimation.SetActive(true);
                if (_blockController.CanBlockAniAppear)
                {
                    MonsterBlockAni.SetBool("Atk", true);
                }
                if (!_swordManSE.SEAppear)
                {
                    _swordManSE.BlockSoundPlay();
                    _swordManSE.SEAppear = true;
                }
                break;
        }

        if (_blockController.isBlockSuucess && !_swordManSE.SE2Appear)
        {
            _swordManSE.BlockSuccessSoundPlay();
            _swordManSE.SE2Appear = true;
        }

        ContinueTypeSEControll();

        if (LastAni != NowAni)
        {
            LastAni = NowAni;
        }
    }

    private void PlayStopAni()
    {
        if (_hurtedController.isHurtedByBigGun)
        {
            NowAni = AniStatus.Stop;
            AllAniFalse();
            MonsterStopAnimation.SetActive(true);
            switch (_basicData.face)
            {
                case MonsterBasicData.Face.Left:
                    MonsterStopTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                    break;
                case MonsterBasicData.Face.Right:
                    MonsterStopTr.localScale = new Vector3(0.28f, 0.28f, 0);
                    break;
            }
        }
        if (_hurtedController.isCriticAtkHurted)
        {
            switch (_basicData.face)
            {
                case MonsterBasicData.Face.Left:
                    MonsterStopTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                    break;
                case MonsterBasicData.Face.Right:
                    MonsterStopTr.localScale = new Vector3(0.28f, 0.28f, 0);
                    break;
            }
            NowAni = AniStatus.Stop;
            AllAniFalse();
            _swordManSE.StopAllSE();
            MonsterStopAnimation.SetActive(true);
        }
    }

    private void SlowWalk()
    {
        if (SlowWalkTimer <= 0)
        {
            switch (status)
            {
                case Status.Alert:
                    SlowWalkTimer = AlertSlowWalkTimerSet;
                    break;
                case Status.Prepare:
                    if (!FirstTrigger)
                    {
                        isBackWalk = true;
                        SlowWalkTimer = PrepareSlowWalkTimerSet;
                    }
                    break;
            }
        }

        if (isLittleWait)
        {
            isBackWalk = true;
            if (!_touchTrigger.isMonsterInRange)
            {
                LittleWaitTimer -= _deltaTime;
            }
            switch (_basicData.face)
            {
                case MonsterBasicData.Face.Left:
                    slowWalkStatus = SlowWalkStatus.walkRight;
                    break;
                case MonsterBasicData.Face.Right:
                    slowWalkStatus = SlowWalkStatus.walkLeft;
                    break;
            }
            if (LittleWaitTimer <= 0)
            {
                LittleWaitTimer = LittleWaitTimerSet;
                isBackWalk = false;
                isLittleWait = false;
            }
        }
        else
        {
            switch (status)
            {
                case Status.Alert:
                    SlowWalkTimer -= _deltaTime;

                    switch (slowWalkStatus)
                    {
                        case SlowWalkStatus.walkLeft:
                            _basicData.face = MonsterBasicData.Face.Left;
                            break;
                        case SlowWalkStatus.walkRight:
                            _basicData.face = MonsterBasicData.Face.Right;
                            break;
                    }

                    if (SlowWalkTimer <= (AlertSlowWalkTimerSet - 5))
                    {
                        slowWalkStatus = SlowWalkStatus.wait;
                    }
                    if (SlowWalkTimer <= (AlertSlowWalkTimerSet - 7))
                    {
                        UnMatchInitialSlowWalk();
                    }
                    if (SlowWalkTimer <= (AlertSlowWalkTimerSet - 12))
                    {
                        slowWalkStatus = SlowWalkStatus.wait;
                    }
                    if (SlowWalkTimer <= 0)
                    {
                        MatchInitialSlowWalk();
                        SlowWalkTimer = AlertSlowWalkTimerSet;
                    }
                    break;
                case Status.Prepare:
                    SlowWalkTimer -= _deltaTime;

                    switch (_basicData.face)
                    {
                        case MonsterBasicData.Face.Left:
                            slowWalkStatus = SlowWalkStatus.walkRight;
                            break;
                        case MonsterBasicData.Face.Right:
                            slowWalkStatus = SlowWalkStatus.walkLeft;
                            break;
                    }

                    if (SlowWalkTimer <= PrepareSlowWalkTurnTime)
                    {
                        isBackWalk = false;
                        FirstTrigger = true;
                        //第二階段接近玩家
                        switch (_basicData.face)
                        {
                            case MonsterBasicData.Face.Left:
                                slowWalkStatus = SlowWalkStatus.walkLeft;
                                break;
                            case MonsterBasicData.Face.Right:
                                slowWalkStatus = SlowWalkStatus.walkRight;
                                break;
                        }
                    }
                    break;
            }
        }
    }

    private void BeBlockEndJudge()
    {
        if (_blockController.BeBlockUnHurtedEnd)
        {
            _basicData.TurnFaceJudge();
            _blockController.BeBlockUnHurtedEnd = false;
            status = Status.Atk;
        }
        if (_blockController.BeBlockNormalAtkEnd)
        {
            status = Status.Prepare;
            AtkLastTime = _time;
            CanAtk = false;
            _blockController.BeBlockNormalAtkEnd = false;
        }
        if (_blockController.BeBlockCAtkEnd)
        {
            status = Status.Weak;
            _blockController.BeBlockCAtkEnd = false;
        }
    }

    private void BlockEndJudge()
    {
        if (_blockController.BlockUnSuccessEnd)
        {
            _basicData.TurnFaceJudge();
            BlockLastTime = _time;
            CanBlock = false;
            _blockController.isBlock = false;
            AtkLastTime = _time;
            CanAtk = false;
            status = Status.Prepare;
            _blockController.BlockUnSuccessEnd = false;
        }
        if (_blockController.BlockSuccessEnd)
        {
            BlockLastTime = _time;
            CanBlock = false;
            AtkLastTime = _time;
            CanAtk = false;
            _blockController.isBlock = false;
            status = Status.Prepare;
            _blockController.BlockSuccessEnd = false;
        }
        if (_blockController.BlockHurtedEnd)
        {
            BlockLastTime = _time;
            AtkLastTime = _time;
            CanAtk = false;
            CanBlock = false;
            _blockController.isBlock = false;
            status = Status.Prepare;
            _blockController.BlockHurtedEnd = false;
        }
    }

    private void AllAniFalse()
    {
        if (NowAni != AniStatus.Walk && NowAni != AniStatus.Wait)
        {
            MonsterMoveAnimation.SetActive(false);
        }
        if (NowAni != AniStatus.SlowMove)
        {
            MonsterSlowMoveAnimation.SetActive(false);
        }
        if (NowAni != AniStatus.Atk)
        {
            MonsterAtkAnimation.SetActive(false);
        }
        if (NowAni != AniStatus.Atk2)
        {
            MonsterAtk2Animation.SetActive(false);
        }
        if (NowAni != AniStatus.Atk3)
        {
            MonsterAtk3Animation.SetActive(false);
        }
        if (NowAni != AniStatus.Summon)
        {
            MonsterSummonAnimation.SetActive(false);
        }
        if (NowAni != AniStatus.Jump)
        {
            MonsterJumpAnimation.SetActive(false);
        }
        if (NowAni != AniStatus.Stop)
        {
            MonsterStopAnimation.SetActive(false);
        }
        if (NowAni != AniStatus.BeBlock)
        {
            MonsterBeBlockAnimation.SetActive(false);
        }
        if (NowAni != AniStatus.Block)
        {
            MonsterBlockAnimation.SetActive(false);
        }
    }

    private void WeakDataInisialize()
    {
        _weakData.WeakMass = 10000;
        _weakData.EndMass = 50;
        _weakData.WeakOffsetX = 0.65f;
        _weakData.WeakOffsetY = -0.472f;
        _weakData.WeakSizeX = 1.4f;
        _weakData.WeakSizeY = 0.845f;
        _weakData.EndOffsetX = 0;
        _weakData.EndOffsetY = 0.03f;
        _weakData.EndSizeX = 0.33f;
        _weakData.EndSizeY = 2;
    }

    private void MatchInitialSlowWalk()
    {
        slowWalkStatus = InitialSlowWalkStatus;
        if (InitialSlowWalkStatus == SlowWalkStatus.wait)
        {
            slowWalkStatus = SlowWalkStatus.walkLeft;
        }
    }

    private void UnMatchInitialSlowWalk()
    {
        switch (InitialSlowWalkStatus)
        {
            case SlowWalkStatus.walkLeft:
                slowWalkStatus = SlowWalkStatus.walkRight;
                break;
            case SlowWalkStatus.walkRight:
                slowWalkStatus = SlowWalkStatus.walkLeft;
                break;
            case SlowWalkStatus.wait:
                slowWalkStatus = SlowWalkStatus.walkRight;
                break;
        }
    }

    private void InisializMonsterTouchTrigger()
    {
        _touchTrigger = this.GetComponent<MonsterTouchTrigger>();

        _touchTrigger.OnTouch += TouchMonster;
        _touchTrigger.StatusChange += TouchMonsterStatusChange;
        _touchTrigger.OnLeave += LeaveMonster;
    }

    private void TouchMonster()
    {
        Rigid2D.gravityScale = 0;
        Rigid2D.drag = 10;
        _boxCollider.isTrigger = true;
    }

    private void TouchMonsterStatusChange()
    {
        if (status == Status.walk || status == Status.Prepare)
        {
            status = Status.Prepare;
            isLittleWait = true;
        }
    }

    private void LeaveMonster()
    {
        Rigid2D.gravityScale = 7;
        Rigid2D.drag = 4;
        _boxCollider.isTrigger = false;
    }

    private void StatusJudge()
    {
        if (isLittleWait)
        {
            return;
        }

        if (status != Status.walk && _basicData.AbsDistanceX > AlertDistance)
        {
            status = Status.walk;
            if (CanAtk3)
            {
                status = Status.Atk3;
            }
        }

        if (SlowWalkTimer <= 0)
        {
            if (_basicData.AbsDistanceX <= Atk2Distance && _basicData.AbsDistanceX > AtkDistance * 2 && CanAtk && _basicData.AbsDistanceY <= 1.5)
            {
                while (!DefineStatus)
                {
                    switch (Random.Range(0, 3))
                    {
                        case 0:
                            if (CanAtk3)
                            {
                                status = Status.Atk3;
                                DefineStatus = true;
                            }
                            break;
                        default:
                            status = Status.Atk2;
                            DefineStatus = true;
                            break;
                    }
                }
                DefineStatus = false;
            }
        }

        if (SlowWalkTimer <= PrepareSlowWalkTurnTime)
        {
            if (_basicData.AbsDistanceX <= AtkDistance && _basicData.AbsDistanceY <= 1.5 && CanAtk)
            {
                while (!DefineStatus)
                {
                    switch (Random.Range(0, 2))
                    {
                        case 0:
                            if (CanBlock)
                            {
                                status = Status.Block;
                                DefineStatus = true;
                            }
                            break;
                        default:
                            status = Status.Atk;
                            DefineStatus = true;
                            break;
                    }
                }
                DefineStatus = false;
            }
        }

        _jumpController.JumpStartJudge();
        if (_jumpController.JumpStart)
        {
            status = Status.jump;
        }
        if (_jumpController.isPrepareRun)
        {
            status = Status.walk;
            _basicData.ReverseTurnFaceJudge();
        }

        if (CanSummon)
        {
            status = Status.Summon;
        }

        if (_basicData.AbsDistanceX >= GiveUpDistanceX || _basicData.AbsDistanceY >= GiveUpDistanceY || PlayerController.isDie == true)
        {
            if (status != Status.Alert)
            {
                status = Status.Alert;
            }
        }
    }//有可能全部都不符合而維持現狀(3)

    private void StatusJudge(Status _status)
    {
        bool JudgeSuccess = false;

        if (isLittleWait)
        {
            return;
        }

        if (status != Status.walk && _basicData.AbsDistanceX > AlertDistance)
        {
            status = Status.walk;
            if (CanAtk3)
            {
                status = Status.Atk3;
            }
            JudgeSuccess = true;
        }

        if (SlowWalkTimer <= 0)
        {
            if (_basicData.AbsDistanceX <= Atk2Distance && _basicData.AbsDistanceX > AtkDistance * 2 && CanAtk && _basicData.AbsDistanceY <= 1.5)
            {
                while (!DefineStatus)
                {
                    switch (Random.Range(0, 3))
                    {
                        case 0:
                            if (CanAtk3)
                            {
                                status = Status.Atk3;
                                DefineStatus = true;
                            }
                            break;
                        default:
                            status = Status.Atk2;
                            DefineStatus = true;
                            break;
                    }
                }
                DefineStatus = false;
                JudgeSuccess = true;
            }
        }

        if (SlowWalkTimer <= PrepareSlowWalkTurnTime)
        {
            if (_basicData.AbsDistanceX <= AtkDistance && _basicData.AbsDistanceY <= 1.5 && CanAtk)
            {
                while (!DefineStatus)
                {
                    switch (Random.Range(0, 2))
                    {
                        case 0:
                            if (CanBlock)
                            {
                                status = Status.Block;
                                DefineStatus = true;
                            }
                            break;
                        default:
                            status = Status.Atk;
                            DefineStatus = true;
                            break;
                    }
                }
                DefineStatus = false;
                JudgeSuccess = true;
            }
        }

        _jumpController.JumpStartJudge();
        if (_jumpController.JumpStart)
        {
            status = Status.jump;
        }
        if (_jumpController.isPrepareRun)
        {
            status = Status.walk;
            _basicData.ReverseTurnFaceJudge();
        }

        if (CanSummon)
        {
            status = Status.Summon;
            JudgeSuccess = true;
        }

        if (_basicData.AbsDistanceX >= GiveUpDistanceX || _basicData.AbsDistanceY >= GiveUpDistanceY || PlayerController.isDie == true)
        {
            if (status != Status.Alert)
            {
                status = Status.Alert;
                JudgeSuccess = true;
            }
        }

        if (!JudgeSuccess)
        {
            status = _status;
        }
    }//全不符合時使用預設status(1)

    private void ContinueTypeSEControll()
    {
        if (NowAni == AniStatus.Walk && _basicData.isGround)
        {
            _swordManSE.WalkSoundPlay();
        }

        if (NowAni == AniStatus.SlowMove && _basicData.isGround)
        {
            _swordManSE.SlowWalkSoundPlay();
        }

        if (NowAni != AniStatus.Walk && NowAni != AniStatus.SlowMove)
        {
            _swordManSE.TurnOffWalkSound();
        }
        else if (NowAni == AniStatus.SlowMove && slowWalkStatus == SlowWalkStatus.wait)
        {
            _swordManSE.TurnOffWalkSound();
        }
    }
}
