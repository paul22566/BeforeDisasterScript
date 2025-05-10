using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldManController : MonoBehaviour
{
    [Header("基本參數")]
    public int AlertDistance;
    public int GiveUpDistanceX;
    public int GiveUpDistanceY;
    public int ChasingDistance;
    public float AtkDistance;
    public float Atk2Distance;
    public float speed;
    public float HeavySpeed;
    public enum Status { Standby, Alert, Prepare, HeavyWalk, Atk, Atk2, BeBlock, Weak };
    [HideInInspector] public Status status;

    private enum AlertStatus { walkLeft, wait, walkRight };
    private AlertStatus alertStatus;
    [SerializeField] private AlertStatus InitialAlertStatus;
    public enum AniStatus { Wait, Walk, DefandWait, HeavyWalk, Atk, Atk2, Stop, BeBlock}
    [HideInInspector] public AniStatus NowAni;//script(Hurted)
    private AniStatus LastAni;

    [Header("系統參數")]
    private Transform _transform;
    private Transform BeProtectPoint;
    private MonsterBasicData _basicData;
    private ShieldManSE _shieldManSE;
    private float _deltaTime;
    private float _fixedDeltaTime;
    private float _time;
    private Transform TemporaryArea;
    private GameObject MonsterRightJudgement;
    private GameObject MonsterLeftJudgement;
    private GameObject MonsterUnderJudgement;
    private MonsterHurtedController _hurtedController;
    private MonsterTouchTrigger _touchTrigger;
    private float StandbyTimer = 0.1f;
    private float Atk1TimerSet = 1.5f;
    private float AtkTimer;
    private float Atk2TimerSet = 1.5f;
    public float AtkCoolDown;//2種Atk共用
    private float AtkLastTime = -10;
    public float LittleWaitTimerSet;
    private float LittleWaitTimer;
    public float AlertWalkTimerSet;
    private float AlertWalkTimer;
    private float TurnFaceLagTimerSet = 0.2f;
    private float TurnFaceLagTimer;
    private BoxCollider2D _boxCollider;
    private Rigidbody2D Rigid2D;
    private float GroundPointY;

    //開關
    private bool FirstTrigger;
    private bool CanAtk;
    private bool isLittleWait;
    private bool DefineStatus;

    [Header("動畫相關物件")]
    private Animator MonsterMoveAni;
    private GameObject MonsterMoveAnimation;
    private GameObject MonsterAtkAnimation;
    private GameObject MonsterAtk2Animation;
    private Transform MonsterMoveTr;
    private Transform MonsterAtkTr;
    private Transform MonsterAtk2Tr;

    [Header("攻擊物件")]
    public ObjectShield _shield;
    public GameObject RAtk;
    public GameObject LAtk;
    public GameObject RAtk2;
    public GameObject LAtk2;

    [Header("格檔")]
    private MonsterBlockController _blockController;
    private WeakData _weakData = new WeakData();
    private GameObject MonsterBeBlockAnimation;
    private Animator MonsterBeBlockAni;
    private Transform MonsterBeBlockTr;

    private GameObject MonsterStopAnimation;
    private Transform MonsterStopTr;
    // Start is called before the first frame update
    void Start()
    {
        _transform = this.transform;
        Rigid2D = this.GetComponent<Rigidbody2D>();
        _boxCollider = this.GetComponent<BoxCollider2D>();
        _basicData = this.GetComponent<MonsterBasicData>();
        _hurtedController = this.transform.GetComponent<MonsterHurtedController>();
        _blockController = this.GetComponent<MonsterBlockController>();
        _shieldManSE = this.GetComponent<ShieldManSE>();

        //抓取動畫相關物件
        MonsterMoveAnimation = this.transform.GetChild(0).gameObject;
        MonsterAtkAnimation = this.transform.GetChild(1).gameObject;
        MonsterAtk2Animation = this.transform.GetChild(2).gameObject;
        MonsterBeBlockAnimation = this.transform.GetChild(3).gameObject;
        MonsterStopAnimation = this.transform.GetChild(4).gameObject;

        MonsterMoveTr = MonsterMoveAnimation.transform.GetChild(0).transform;
        MonsterAtkTr = MonsterAtkAnimation.transform.GetChild(0).transform;
        MonsterAtk2Tr = MonsterAtk2Animation.transform.GetChild(0).transform;
        MonsterBeBlockTr = MonsterBeBlockAnimation.transform.GetChild(0).transform;
        MonsterStopTr = MonsterStopAnimation.transform.GetChild(0).transform;

        MonsterMoveAni = MonsterMoveTr.GetComponent<Animator>();
        MonsterBeBlockAni = MonsterBeBlockTr.GetComponent<Animator>();

        TemporaryArea = this.transform.GetChild(5);

        if (!_basicData.SpecialOpening)
        {
            status = Status.Standby;
        }
        NowAni = AniStatus.Wait;
        MatchInitialAlertWalk();
        _basicData.BasicVarInisialize(MonsterMoveTr, "R");
        _basicData.ShouldIgnore = true;

        TurnFaceLagTimer = TurnFaceLagTimerSet;
        MonsterRightJudgement = TemporaryArea.GetChild(0).gameObject;
        MonsterLeftJudgement = TemporaryArea.GetChild(1).gameObject;
        MonsterUnderJudgement = TemporaryArea.GetChild(2).gameObject;
        BeProtectPoint = TemporaryArea.GetChild(3);
        TemporaryArea.DetachChildren();

        _hurtedController._getCriticHurted += PlayStopAni;

        //WeakData初始化
        WeakDataInisialize();
        //初始化MonsterTouch系統
        InisializMonsterTouchTrigger();
        LittleWaitTimer = LittleWaitTimerSet;
        //初始化shield
        _shield.ProtectTarget = BeProtectPoint;
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
            _boxCollider.offset = new Vector2(_weakData.EndOffsetX, _weakData.EndOffsetY);
            _boxCollider.size = new Vector2(_weakData.EndSizeX, _weakData.EndSizeY);
            Rigid2D.mass = 50;
            AtkTimer = 0;
            CanAtk = false;
            FirstTrigger = false;
            AtkLastTime = _time;
            _blockController.AllVariableReset();
        }
        if (_blockController.BeBlockSuccess)
        {
            status = Status.BeBlock;
            AtkTimer = 0;
            CanAtk = false;
            FirstTrigger = false;
            AtkLastTime = _time;
        }

        _hurtedController.HurtedTimerMethod(_deltaTime);
        _hurtedController.CriticAtkHurtedTimerMethod(_deltaTime);
        PlayStopAni();

        //判斷是否觸地
        _basicData.CheckTouchGround(ref GroundPointY);
        if (_basicData.isGround && !_hurtedController.isCriticAtkHurted)
        {
            _transform.position = new Vector3(_transform.position.x, GroundPointY + _basicData.GroundPlace, 0);
        }
        if (!_basicData.isGround)
        {
            Rigid2D.gravityScale = 7;
        }
        //判斷是否碰到牆壁
        _basicData.CheckTouchWall();

        //寫入實際位置
        _basicData.MonsterPlace = _transform.position;
        BeProtectPoint.position = _basicData.MonsterPlace;

        //return
        if (GameEvent.isAniPlay || _hurtedController.isHurtedByBigGun || _hurtedController.isCriticAtkHurted || PauseMenuController.isPauseMenuOpen)
        {
            return;
        }
        
        //計算距離
        _basicData.DistanceCalculate();
        //冷卻時間計算
        _basicData.CoolDownCalculate(_time, AtkLastTime, AtkCoolDown, ref CanAtk);

        //被大招打之後的動作
        if (_hurtedController.BeCriticAtkEnd)
        {
            status = Status.Prepare;
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
                    AlertWalk();

                    //被遠程打時的反應
                    if (_hurtedController.HurtedByFarAtk)
                    {
                        AlertWalkTimer = 0;
                        StatusJudge(Status.HeavyWalk);
                        _basicData.TurnFaceJudge();
                    }
                    if (_hurtedController.HurtedByFarAtk)
                    {
                        _hurtedController.HurtedByFarAtk = false;
                    }

                    if (_basicData.AbsDistanceX < AlertDistance && _basicData.AbsDistanceY <= 2)
                    {
                        AlertWalkTimer = 0;
                        StatusJudge(Status.HeavyWalk);
                        _basicData.TurnFaceJudge();
                    }
                }
                break;
            case Status.Prepare:
                _basicData.LagTurnFaceJudge(ref TurnFaceLagTimer, TurnFaceLagTimerSet, _deltaTime);

                if (isLittleWait && !_touchTrigger.isMonsterInRange)
                {
                    LittleWaitTimer -= _deltaTime;
                    if (LittleWaitTimer <= 0)
                    {
                        LittleWaitTimer = LittleWaitTimerSet;
                        isLittleWait = false;
                    }
                }

                StatusJudge();
                break;
            case Status.HeavyWalk:
                _basicData.LagTurnFaceJudge(ref TurnFaceLagTimer, TurnFaceLagTimerSet, _deltaTime);

                CanAtk = true;

                StatusJudge();
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

        //轉向控制
        switch (_basicData.face)
        {
            case MonsterBasicData.Face.Left:
                MonsterMoveTr.localScale = new Vector3(-0.28f, 0.28f, 1);
                MonsterAtkTr.localScale = new Vector3(-0.28f, 0.28f, 1);
                MonsterAtk2Tr.localScale = new Vector3(-0.28f, 0.28f, 1);
                MonsterBeBlockTr.localScale = new Vector3(-0.28f, 0.28f, 1);
                MonsterStopTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                break;

            case MonsterBasicData.Face.Right:
                MonsterMoveTr.localScale = new Vector3(0.28f, 0.28f, 1);
                MonsterAtkTr.localScale = new Vector3(0.28f, 0.28f, 1);
                MonsterAtk2Tr.localScale = new Vector3(0.28f, 0.28f, 1);
                MonsterBeBlockTr.localScale = new Vector3(0.28f, 0.28f, 1);
                MonsterStopTr.localScale = new Vector3(0.28f, 0.28f, 0);
                break;
        }

        SwitchAnimation();
    }

    private void FixedUpdate()
    {
        _fixedDeltaTime = Time.fixedDeltaTime;

        //return
        if (GameEvent.isAniPlay || _hurtedController.isHurtedByBigGun || _hurtedController.isCriticAtkHurted)
        {
            return;
        }

        switch (status)
        {
            case Status.Alert:
                switch (alertStatus)
                {
                    case AlertStatus.walkLeft:
                        if (!_basicData.touchLeftWall && !_hurtedController.isHurted && !MonsterLeftJudgement.GetComponent<MonsterLeftJudgement>().isPlayerAtLeftSide)
                        {
                            if (!MonsterUnderJudgement.GetComponent<MonsterUnderJudgement>().isPlayerUnder)
                            {
                                _transform.position = new Vector3(_transform.position.x - speed * _fixedDeltaTime, _transform.position.y, 0);
                            }
                        }
                        break;

                    case AlertStatus.walkRight:
                        if (!_basicData.touchRightWall && !_hurtedController.isHurted && !MonsterRightJudgement.GetComponent<MonsterRightJudgement>().isPlayerAtRightSide)
                        {
                            if (!MonsterUnderJudgement.GetComponent<MonsterUnderJudgement>().isPlayerUnder)
                            {
                                _transform.position = new Vector3(_transform.position.x + speed * _fixedDeltaTime, _transform.position.y, 0);
                            }
                        }
                        break;
                }
                break;
            case Status.HeavyWalk:
                if (!_hurtedController.isHurted)
                {
                    switch (_basicData.face)
                    {
                        case MonsterBasicData.Face.Right:
                            if (!_basicData.touchRightWall && !MonsterRightJudgement.GetComponent<MonsterRightJudgement>().isPlayerAtRightSide)
                            {
                                if (!MonsterUnderJudgement.GetComponent<MonsterUnderJudgement>().isPlayerUnder)
                                {
                                    _transform.position = new Vector3(_transform.position.x + HeavySpeed * _fixedDeltaTime, _transform.position.y, 0);
                                }
                            }
                            break;
                        case MonsterBasicData.Face.Left:
                            if (!_basicData.touchLeftWall && !MonsterLeftJudgement.GetComponent<MonsterLeftJudgement>().isPlayerAtLeftSide)
                            {
                                if (!MonsterUnderJudgement.GetComponent<MonsterUnderJudgement>().isPlayerUnder)
                                {
                                    _transform.position = new Vector3(_transform.position.x - HeavySpeed * _fixedDeltaTime, _transform.position.y, 0);
                                }
                            }
                            break;
                    }
                }
                break;
            case Status.Atk:
                if (AtkTimer <= 0)
                {
                    AtkTimer = Atk1TimerSet;
                }

                AtkTimer -= _fixedDeltaTime;

                if (AtkTimer <= (Atk1TimerSet - 0.6))
                {
                    if (!FirstTrigger)
                    {
                        switch (_basicData.face)
                        {
                            case MonsterBasicData.Face.Right:
                                Instantiate(RAtk, _transform.position, Quaternion.identity, TemporaryArea);
                                TemporaryArea.DetachChildren();
                                break;
                            case MonsterBasicData.Face.Left:
                                Instantiate(LAtk, _transform.position, Quaternion.identity, TemporaryArea);
                                TemporaryArea.DetachChildren();
                                break;
                        }
                        FirstTrigger = true;
                    }
                }
                if (AtkTimer <= 0)
                {
                    _basicData.TurnFaceJudge();
                    AtkLastTime = _time;
                    CanAtk = false;
                    status = Status.Prepare;
                    FirstTrigger = false;
                }
                break;
            case Status.Atk2:
                if (AtkTimer <= 0)
                {
                    AtkTimer = Atk2TimerSet;
                }

                AtkTimer -= _fixedDeltaTime;

                if (AtkTimer <= (Atk2TimerSet - 0.8))
                {
                    if (!FirstTrigger)
                    {
                        switch (_basicData.face)
                        {
                            case MonsterBasicData.Face.Right:
                                Instantiate(RAtk2, _transform.position, Quaternion.identity, TemporaryArea);
                                TemporaryArea.DetachChildren();
                                break;
                            case MonsterBasicData.Face.Left:
                                Instantiate(LAtk2, _transform.position, Quaternion.identity, TemporaryArea);
                                TemporaryArea.DetachChildren();
                                break;
                        }
                        FirstTrigger = true;
                    }
                }
                if (AtkTimer <= 0)
                {
                    _basicData.TurnFaceJudge();
                    AtkLastTime = _time;
                    CanAtk = false;
                    status = Status.Prepare;
                    FirstTrigger = false;
                }
                break;
        }

        _hurtedController.HurtedMove(_fixedDeltaTime);
    }

    private void SwitchAnimation()
    {
        if (status != Status.BeBlock)
        {
            NowAni = AniStatus.Wait;
        }

        if (status == Status.Alert && alertStatus != AlertStatus.wait)
        {
            NowAni = AniStatus.Walk;
        }

        if (status == Status.Prepare)
        {
            NowAni = AniStatus.DefandWait;
        }

        if (status == Status.HeavyWalk)
        {
            NowAni = AniStatus.HeavyWalk;
        }

        if (status == Status.Atk)
        {
            NowAni = AniStatus.Atk;
        }

        if (status == Status.Atk2)
        {
            NowAni = AniStatus.Atk2;
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

        if (LastAni != NowAni)
        {
            _shieldManSE.BoolReset();
        }

        switch (NowAni)
        {
            case AniStatus.Wait:
                AllAniFalse();
                MonsterMoveAnimation.SetActive(true);
                MonsterMoveAni.SetBool("isWalking", false);
                MonsterMoveAni.SetBool("isAlert", false);
                break;
            case AniStatus.Walk:
                AllAniFalse();
                MonsterMoveAnimation.SetActive(true);
                MonsterMoveAni.SetBool("isWalking", true);
                MonsterMoveAni.SetBool("isAlert", false);
                break;
            case AniStatus.DefandWait:
                AllAniFalse();
                MonsterMoveAnimation.SetActive(true);
                MonsterMoveAni.SetBool("isWalking", false);
                MonsterMoveAni.SetBool("isAlert", true);
                break;
            case AniStatus.HeavyWalk:
                AllAniFalse();
                MonsterMoveAnimation.SetActive(true);
                MonsterMoveAni.SetBool("isWalking", true);
                MonsterMoveAni.SetBool("isAlert", true);
                break;
            case AniStatus.Atk:
                AllAniFalse();
                MonsterAtkAnimation.SetActive(true);
                if (!_shieldManSE.SEAppear)
                {
                    _shieldManSE.AtkSound1Play();
                    _shieldManSE.SEAppear = true;
                }
                break;
            case AniStatus.Atk2:
                AllAniFalse();
                MonsterAtk2Animation.SetActive(true);
                if (!_shieldManSE.SEAppear)
                {
                    _shieldManSE.AtkSound2Play();
                    _shieldManSE.SEAppear = true;
                }
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
            _shieldManSE.StopAllSE();
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
            _shieldManSE.StopAllSE();
            MonsterStopAnimation.SetActive(true);
        }
    }

    private void AllAniFalse()
    {
        if (NowAni != AniStatus.Walk && NowAni != AniStatus.Wait && NowAni !=AniStatus.DefandWait && NowAni != AniStatus.HeavyWalk)
        {
            MonsterMoveAnimation.SetActive(false);
        }
        if (NowAni != AniStatus.Atk)
        {
            MonsterAtkAnimation.SetActive(false);
        }
        if (NowAni != AniStatus.Atk2)
        {
            MonsterAtk2Animation.SetActive(false);
        }
        if (NowAni != AniStatus.Stop)
        {
            MonsterStopAnimation.SetActive(false);
        }
        if (NowAni != AniStatus.BeBlock)
        {
            MonsterBeBlockAnimation.SetActive(false);
        }
    }

    private void AlertWalk()
    {
        if (AlertWalkTimer <= 0)
        {
            AlertWalkTimer = AlertWalkTimerSet;
        }

        AlertWalkTimer -= _deltaTime;

        switch (alertStatus)
        {
            case AlertStatus.walkLeft:
                _basicData.face = MonsterBasicData.Face.Left;
                break;
            case AlertStatus.walkRight:
                _basicData.face = MonsterBasicData.Face.Right;
                break;
        }

        if (AlertWalkTimer <= (AlertWalkTimerSet - 5))
        {
            alertStatus = AlertStatus.wait;
        }
        if (AlertWalkTimer <= (AlertWalkTimerSet - 7))
        {
            UnMatchInitialAlertWalk();
        }
        if (AlertWalkTimer <= (AlertWalkTimerSet - 12))
        {
            alertStatus = AlertStatus.wait;
        }
        if (AlertWalkTimer <= 0)
        {
            MatchInitialAlertWalk();
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

    private void WeakDataInisialize()
    {
        _weakData.WeakMass = 10000;
        _weakData.EndMass = 50;
        _weakData.WeakOffsetX = 1.4f;
        _weakData.WeakOffsetY = -0.22f;
        _weakData.WeakSizeX = 0.22f;
        _weakData.WeakSizeY = 0.94f;
        _weakData.EndOffsetX = 0;
        _weakData.EndOffsetY = 0.14f;
        _weakData.EndSizeX = 0.3f;
        _weakData.EndSizeY = 2.16f;
    }

    private void MatchInitialAlertWalk()
    {
        alertStatus = InitialAlertStatus;
        if (InitialAlertStatus == AlertStatus.wait)
        {
            alertStatus = AlertStatus.walkLeft;
        }
    }

    private void UnMatchInitialAlertWalk()
    {
        switch (InitialAlertStatus)
        {
            case AlertStatus.walkLeft:
                alertStatus = AlertStatus.walkRight;
                break;
            case AlertStatus.walkRight:
                alertStatus = AlertStatus.walkLeft;
                break;
            case AlertStatus.wait:
                alertStatus = AlertStatus.walkRight;
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
        if (_touchTrigger._type == MonsterBasicData.MonsterType.ShieldMan)
        {
            _basicData.ShouldIgnore = false;
        }
        Rigid2D.gravityScale = 0;
        Rigid2D.drag = 10;
        _boxCollider.isTrigger = true;
    }

    private void TouchMonsterStatusChange()
    {
        if (status == Status.HeavyWalk || status == Status.Prepare)
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
        _basicData.ShouldIgnore = true;
    }

    private void StatusJudge()
    {
        if (isLittleWait)
        {
            status = Status.Prepare;
            return;
        }

        if (status != Status.HeavyWalk && _basicData.AbsDistanceX > ChasingDistance)
        {
            status = Status.HeavyWalk;
        }

        if (_basicData.AbsDistanceX > AtkDistance && CanAtk)
        {
            status = Status.HeavyWalk;
        }

        if (_basicData.AbsDistanceX <= AtkDistance && _basicData.AbsDistanceY <= 1.5 && CanAtk)
        {
            while (!DefineStatus)
            {
                switch (Random.Range(0, 2))
                {
                    case 0:
                        status = Status.Atk;
                        DefineStatus = true;
                        break;
                    default:
                        if (_basicData.AbsDistanceX <= Atk2Distance)
                        {
                            status = Status.Atk2;
                            DefineStatus = true;
                        }
                        break;
                }
            }
            DefineStatus = false;
        }

        if (_basicData.AbsDistanceX >= GiveUpDistanceX || _basicData.AbsDistanceY >= GiveUpDistanceY || PlayerController.isDie == true)
        {
            if (status != Status.Alert)
            {
                status = Status.Alert;
            }
        }
    }//有可能全部都不符合而維持現狀

    private void StatusJudge(Status _status)//全不符合時使用預設status(1)
    {
        bool JudgeSucess = false;

        if (isLittleWait)
        {
            status = Status.Prepare;
            JudgeSucess = true;
            return;
        }

        if (status != Status.HeavyWalk && _basicData.AbsDistanceX > ChasingDistance)
        {
            status = Status.HeavyWalk;
            JudgeSucess = true;
        }

        if (_basicData.AbsDistanceX > AtkDistance && CanAtk)
        {
            JudgeSucess = true;
            status = Status.HeavyWalk;
        }

        if (_basicData.AbsDistanceX <= AtkDistance && _basicData.AbsDistanceY <= 1.5 && CanAtk)
        {
            while (!DefineStatus)
            {
                switch (Random.Range(0, 2))
                {
                    case 0:
                        status = Status.Atk;
                        DefineStatus = true;
                        break;
                    default:
                        if (_basicData.AbsDistanceX <= Atk2Distance)
                        {
                            status = Status.Atk2;
                            DefineStatus = true;
                        }
                        break;
                }
            }
            JudgeSucess = true;
            DefineStatus = false;
        }

        if (_basicData.AbsDistanceX >= GiveUpDistanceX || _basicData.AbsDistanceY >= GiveUpDistanceY || PlayerController.isDie == true)
        {
            if (status != Status.Alert)
            {
                status = Status.Alert;
                JudgeSucess = true;
            }
        }

        if (!JudgeSucess)
        {
            status = _status;
        }
    }//有可能全部都不符合而維持現狀

    private void ContinueTypeSEControll()
    {
        if (NowAni == AniStatus.Walk && _basicData.isGround)
        {
            _shieldManSE.WalkSoundPlay();
        }

        if (NowAni == AniStatus.HeavyWalk && _basicData.isGround)
        {
            _shieldManSE.HeavyWalkSoundPlay();
        }

        if (NowAni != AniStatus.Walk && NowAni != AniStatus.HeavyWalk)
        {
            _shieldManSE.TurnOffWalkSound();
        }
    }
}
