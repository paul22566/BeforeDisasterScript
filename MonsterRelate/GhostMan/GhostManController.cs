using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostManController : MonoBehaviour
{
    [Header("基本參數")]
    public int ChasingDistance;
    public int GiveUpDistanceX;
    public int GiveUpDistanceY;
    public int AlertDistance;
    public float AtkDistance;
    public float Atk2Distance;
    public float speed;
    public float SlowWalkSpeed;
    public float AtkSpeed;
    public float Atk2Speed;
    private enum Status { wait, alert, walk, jump, Atk, Atk2, Atk3, Block, BeBlock, Weak };
    private Status status;
    private enum SlowWalkStatus { walkLeft, wait, walkRight };
    private SlowWalkStatus slowWalkStatus;
    public enum AniStatus { Wait, Walk, SlowMove, Atk, Atk2, Atk3, Jump, Stop, BeBlock, Block }
    [HideInInspector] public AniStatus NowAni;//script(hurted)

    [Header("系統參數")]
    private Transform AtkTemporaryArea;
    private GameObject MonsterRightJudgement;
    private GameObject MonsterLeftJudgement;
    private GameObject MonsterUnderJudgement;
    private Transform _transform;
    private float _time;
    private float _deltaTime;
    private float _fixDeltaTime;
    private float AtkTimerSet = 1.1f;
    private float AtkTimer;
    private float Atk2TimerSet = 1.2f;
    private float Atk2Timer;
    public float Atk3TimerSet;
    private float Atk3Timer;
    public float AtkCoolDown;//2種Atk共用
    private float AtkLastTime = -10;
    public float Atk3CoolDown;
    private float Atk3LastTime = -10;
    public float SlowWalkTimerSet;
    private float SlowWalkTimer;
    public float AlertSlowWalkTimerSet;
    private float AlertSlowWalkTimer;
    public float LittleWaitTimerSet;
    private float LittleWaitTimer;
    private int MonsterOrderRecord;
    private float TurnFaceLagTimerSet = 0.2f;
    private float TurnFaceLagTimer;
    private Rigidbody2D Rigid2D;
    private BoxCollider2D ThisBoxCollider;
    private MonsterBasicData _basicData;
    private MonsterHurtedController _hurtedController;
    private MonsterJumpController _jumpController;
    private int AtkChooseNumbr;
    private float GroundPointY;

    //開關
    private bool isWalking;//有被其他script用到(GhostManEye)
    private bool isAtk;//有被其他script用到(GhostManEye)
    private bool isAtk2;//有被其他script用到(GhostManEye)
    private bool isAtk3;
    private bool timerSwitch;
    private bool SlowWalkTimerSwitch;
    private bool AtkFirstAppear;
    private bool AtkSecondAppear;
    private bool AtkMove = true;
    private bool Atk2Move = true;
    private bool isAlert;
    private bool isAlertByFarAtk;
    private bool CanAtk;
    private bool CanAtk3;
    private bool isSlowWalk;//管理動畫專用 有被其他script用到(GhostManEye)
    private bool isLittleWait;
    private bool isMonsterInRange;
    private bool isMonsterOrderChange;
    private bool isFirstTurnRun;

    [Header("動畫相關物件")]
    private Animator MonsterMoveAni;
    private Animator MonsterSlowMoveAni;
    private GameObject MonsterMoveAnimation;
    private GameObject MonsterSlowMoveAnimation;
    private GameObject MonsterAtkAnimation;
    private GameObject MonsterAtk2Animation;
    private GameObject MonsterAtk3Animation;
    private GameObject MonsterJumpAnimation;
    private Transform MonsterMoveTr;
    private Transform MonsterSlowMoveTr;
    private Transform MonsterAtkTr;
    private Transform MonsterAtk2Tr;
    private Transform MonsterAtk3Tr;
    private Transform MonsterJumpTr;

    [Header("攻擊物件")]
    public GameObject RAtk;
    public GameObject LAtk;
    public GameObject RAtk2;
    public GameObject LAtk2;
    public GameObject RStringAtk;
    public GameObject LStringAtk;
    public GameObject RSwordExplosion;
    public GameObject LSwordExplosion;
    public GameObject Explosion;
    private Vector3 RExplosionAppear = new Vector3(0.3f, 0, 0);
    private Vector3 LExplosionAppear = new Vector3(-0.3f, 0, 0);

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
    private float BlockCoolDown = 8;
    private float BlockLastTime;
    private GameObject MonsterBlockAnimation;
    private Animator MonsterBlockAni;
    private Transform MonsterBlockTr;
    void Start()
    {
        NowAni = AniStatus.Wait;
        //抓取動畫物件
        MonsterMoveAnimation = this.transform.GetChild(0).gameObject;
        MonsterSlowMoveAnimation = this.transform.GetChild(4).gameObject;
        MonsterAtkAnimation = this.transform.GetChild(1).gameObject;
        MonsterAtk2Animation = this.transform.GetChild(2).gameObject;
        MonsterAtk3Animation = this.transform.GetChild(3).gameObject;
        MonsterStopAnimation = this.transform.GetChild(6).gameObject;
        MonsterBeBlockAnimation = this.transform.GetChild(7).gameObject;
        MonsterBlockAnimation = this.transform.GetChild(8).gameObject;
        MonsterJumpAnimation = this.transform.GetChild(9).gameObject;

        MonsterMoveTr = MonsterMoveAnimation.transform.GetChild(0);
        MonsterSlowMoveTr = MonsterSlowMoveAnimation.transform.GetChild(0);
        MonsterAtkTr = MonsterAtkAnimation.transform.GetChild(0);
        MonsterAtk2Tr = MonsterAtk2Animation.transform.GetChild(0);
        MonsterAtk3Tr = MonsterAtk3Animation.transform.GetChild(0);
        MonsterJumpTr = MonsterJumpAnimation.transform.GetChild(0);
        MonsterBeBlockTr = MonsterBeBlockAnimation.transform.GetChild(0);
        MonsterBlockTr = MonsterBlockAnimation.transform.GetChild(0);
        MonsterStopTr = MonsterStopAnimation.transform.GetChild(0);

        MonsterMoveAni = MonsterMoveTr.GetComponent<Animator>();
        MonsterSlowMoveAni = MonsterSlowMoveTr.GetComponent<Animator>();
        MonsterBeBlockAni = MonsterBeBlockTr.GetComponent<Animator>();
        MonsterBlockAni = MonsterBlockTr.GetComponent<Animator>();

        AtkTemporaryArea = this.transform.GetChild(5);
        status = Status.wait;
        slowWalkStatus = SlowWalkStatus.wait;

        _basicData = this.GetComponent<MonsterBasicData>();
        _hurtedController = this.transform.GetComponent<MonsterHurtedController>();
        _blockController = this.GetComponent<MonsterBlockController>();
        _jumpController = this.GetComponent<MonsterJumpController>();

        _basicData.BasicVarInisialize(MonsterSlowMoveTr, "R");

        _transform = this.transform;
        Rigid2D = this.transform.GetComponent<Rigidbody2D>();
        ThisBoxCollider = this.transform.GetComponent<BoxCollider2D>();

        TurnFaceLagTimer = TurnFaceLagTimerSet;
        MonsterRightJudgement = AtkTemporaryArea.transform.GetChild(0).gameObject;
        MonsterLeftJudgement = AtkTemporaryArea.transform.GetChild(1).gameObject;
        MonsterUnderJudgement = AtkTemporaryArea.transform.GetChild(2).gameObject;
        AtkTemporaryArea.transform.DetachChildren();

        WeakDataInisialize();
    }

    // Update is called once per frame
    void Update()
    {
        _time = Time.time;
        _deltaTime = Time.deltaTime;
        _basicData.DieJudge();

        if (_hurtedController.isCriticAtkHurted || _hurtedController.isHurtedByBigGun)
        {
            Rigid2D.mass = 50;
            ThisBoxCollider.offset = new Vector2(0, 0);
            ThisBoxCollider.size = new Vector2(0.3f, 1.8f);
            AtkMove = true;
            Atk2Move = true;
            CanAtk = false;
            CanBlock = false;
            isAtk = false;
            isAtk2 = false;
            isAtk3 = false;
            timerSwitch = false;
            AtkFirstAppear = false;
            AtkSecondAppear = false;
            AtkLastTime = _time;
            Atk3LastTime = _time;
            BlockLastTime = _time;
            AtkTimer = AtkTimerSet;
            Atk2Timer = Atk2TimerSet;
            Atk3Timer = Atk3TimerSet;
            _jumpController.AllVariableFalse();
            _blockController.AllVariableReset();
            status = Status.alert;
        }

        _hurtedController.HurtedTimerMethod(_deltaTime);
        _hurtedController.CriticAtkHurtedTimerMethod(_deltaTime);
        HurtedByStopAtkAni();
        AtkTimerMethod();
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

        //return
        if (GameEvent.isAniPlay || _hurtedController.isCriticAtkHurted || _hurtedController.isHurtedByBigGun)
        {
            return;
        }

        //計算距離
        _basicData.DistanceCalculate();
        //冷卻時間計算
        _basicData.CoolDownCalculate(_time, AtkLastTime, AtkCoolDown, ref CanAtk);
        _basicData.CoolDownCalculate(_time, Atk3LastTime, Atk3CoolDown, ref CanAtk3);
        _basicData.CoolDownCalculate(_time, BlockLastTime, BlockCoolDown, ref CanBlock);

        //被遠程打時的反應
        if (_hurtedController.HurtedByFarAtk && !isAlertByFarAtk && !isAlert)
        {
            isAlertByFarAtk = true;
            status = Status.walk;
            isSlowWalk = false;
            isWalking = true;
            isAlert = true;
            _basicData.TurnFaceJudge();
        }
        if (_hurtedController.HurtedByFarAtk)
        {
            _hurtedController.HurtedByFarAtk = false;
        }

        if (_hurtedController.BeCriticAtkEnd)
        {
            status = Status.walk;
            _hurtedController.BeCriticAtkEnd = false;
        }

        //怪物AI
        switch (status)
        {
            case Status.wait:
                if (_basicData.playerTransform)
                {
                    if (!isFirstTurnRun)
                    {
                        //在這裡放轉向判定是為了避免怪物一出生攻擊方向錯誤Bug
                        _basicData.TurnFaceJudge();
                        goto SkipStatusJudgement;
                    }
                    SlowWalkTimerSwitch = true;

                    if (_basicData.AbsDistanceX <= AtkDistance && CanAtk && _basicData.AbsDistanceY <= 1.5 && isFirstTurnRun)
                    {
                        switch (Random.Range(1, 3))
                        {
                            case 1:
                                status = Status.Atk;
                                isAlert = true;
                                isSlowWalk = false;
                                goto SkipStatusJudgement;
                            case 2:
                                if (CanAtk3)
                                {
                                    status = Status.Atk3;
                                    isAlert = true;
                                    isSlowWalk = false;
                                    goto SkipStatusJudgement;
                                }
                                else
                                {
                                    status = Status.Atk;
                                    isAlert = true;
                                    isSlowWalk = false;
                                    goto SkipStatusJudgement;
                                }
                        }
                    }

                    if (_basicData.AbsDistanceX <= Atk2Distance && _basicData.AbsDistanceX > AtkDistance * 2 && CanAtk && _basicData.AbsDistanceY <= 1.5 && isFirstTurnRun)
                    {
                        isAlert = true;
                        isSlowWalk = false;
                        status = Status.Atk2;
                        goto SkipStatusJudgement;
                    }

                    if (_basicData.AbsDistanceX < ChasingDistance && _basicData.AbsDistanceY <= 2 && isFirstTurnRun)
                    {
                        _basicData.TurnFaceJudge();
                        status = Status.walk;
                        isSlowWalk = false;
                        isAlert = true;
                        goto SkipStatusJudgement;
                    }
                }
                break;
            case Status.alert:

                isWalking = false;
                SlowWalkTimerSwitch = true;

                _basicData.LagTurnFaceJudge(ref TurnFaceLagTimer, TurnFaceLagTimerSet, _deltaTime);

                if (_basicData.AbsDistanceX >= GiveUpDistanceX || _basicData.AbsDistanceY >= GiveUpDistanceY)
                {
                    isAlert = false;
                    status = Status.wait;
                    isAlertByFarAtk = false;
                    goto SkipStatusJudgement;
                }

                if (isLittleWait)
                {
                    goto SkipStatusJudgement;
                }

                if (_basicData.AbsDistanceX <= AtkDistance && CanAtk && _basicData.AbsDistanceY <= 1.5)
                {
                    switch (Random.Range(1, 3))
                    {
                        case 1:
                            AtkChooseNumbr = Random.Range(1, 4);
                            if (AtkChooseNumbr != 3)
                            {
                                status = Status.Atk;
                                isSlowWalk = false;
                                AtkChooseNumbr = 0;
                                goto SkipStatusJudgement;
                            }
                            else
                            {
                                if (CanBlock)
                                {
                                    status = Status.Block;
                                    isSlowWalk = false;
                                    AtkChooseNumbr = 0;
                                    goto SkipStatusJudgement;
                                }
                                else
                                {
                                    status = Status.Atk;
                                    isSlowWalk = false;
                                    AtkChooseNumbr = 0;
                                    goto SkipStatusJudgement;
                                }
                            }
                        case 2:
                            if (CanAtk3)
                            {
                                status = Status.Atk3;
                                isSlowWalk = false;
                                goto SkipStatusJudgement;
                            }
                            else
                            {
                                AtkChooseNumbr = Random.Range(1, 4);
                                if (AtkChooseNumbr != 3)
                                {
                                    status = Status.Atk;
                                    isSlowWalk = false;
                                    AtkChooseNumbr = 0;
                                    goto SkipStatusJudgement;
                                }
                                else
                                {
                                    if (CanBlock)
                                    {
                                        status = Status.Block;
                                        isSlowWalk = false;
                                        AtkChooseNumbr = 0;
                                        goto SkipStatusJudgement;
                                    }
                                    else
                                    {
                                        status = Status.Atk;
                                        isSlowWalk = false;
                                        AtkChooseNumbr = 0;
                                        goto SkipStatusJudgement;
                                    }
                                }
                            }
                    }
                }

                if (_basicData.AbsDistanceX <= Atk2Distance && _basicData.AbsDistanceX > AtkDistance * 2 && CanAtk && _basicData.AbsDistanceY <= 1.5)
                {
                    status = Status.Atk2;
                    isSlowWalk = false;
                    goto SkipStatusJudgement;
                }

                _jumpController.JumpStartJudge();
                if (_jumpController.JumpStart)
                {
                    status = Status.jump;
                    isWalking = false;
                    goto SkipStatusJudgement;
                }

                if (_basicData.AbsDistanceX > AlertDistance && _basicData.AbsDistanceY <= 1.5)
                {
                    status = Status.walk;
                    isSlowWalk = false;
                    goto SkipStatusJudgement;
                }
                break;
            case Status.walk:
                isWalking = true;
                _basicData.LagTurnFaceJudge(ref TurnFaceLagTimer, TurnFaceLagTimerSet, _deltaTime);

                if (!_hurtedController.isHurted)
                {
                    switch (_basicData.face)
                    {
                        case MonsterBasicData.Face.Right:
                            if (!_basicData.touchRightWall && !MonsterRightJudgement.GetComponent<MonsterRightJudgement>().isPlayerAtRightSide)
                            {
                                if (!MonsterUnderJudgement.GetComponent<MonsterUnderJudgement>().isPlayerUnder)
                                {
                                    _transform.position += new Vector3(speed * _deltaTime, 0, 0);
                                }
                            }
                            break;
                        case MonsterBasicData.Face.Left:
                            if (!_basicData.touchLeftWall && !MonsterLeftJudgement.GetComponent<MonsterLeftJudgement>().isPlayerAtLeftSide)
                            {
                                if (!MonsterUnderJudgement.GetComponent<MonsterUnderJudgement>().isPlayerUnder)
                                {
                                    _transform.position -= new Vector3(speed * _deltaTime, 0, 0);
                                }
                            }
                            break;
                    }
                }

                _jumpController.JumpStartJudge();
                if (_jumpController.JumpStart)
                {
                    status = Status.jump;
                    isWalking = false;
                    goto SkipStatusJudgement;
                }

                if (_basicData.playerTransform)
                {
                    if (_basicData.AbsDistanceX >= GiveUpDistanceX || _basicData.AbsDistanceY >= GiveUpDistanceY)
                    {
                        status = Status.wait;
                        isWalking = false;
                        isAlert = false;
                        isAlertByFarAtk = false;
                        goto SkipStatusJudgement;
                    }

                    if (_basicData.AbsDistanceX <= AtkDistance && CanAtk && _basicData.AbsDistanceY <= 1.5)
                    {
                        switch (Random.Range(1, 3))
                        {
                            case 1:
                                AtkChooseNumbr = Random.Range(1, 4);
                                if (AtkChooseNumbr != 3)
                                {
                                    if (CanAtk)
                                    {
                                        status = Status.Atk;
                                        isWalking = false;
                                        AtkChooseNumbr = 0;
                                        goto SkipStatusJudgement;
                                    }
                                }
                                else
                                {
                                    if (CanBlock)
                                    {
                                        status = Status.Block;
                                        isWalking = false;
                                        AtkChooseNumbr = 0;
                                        goto SkipStatusJudgement;
                                    }
                                    else
                                    {
                                        if (CanAtk)
                                        {
                                            status = Status.Atk;
                                            isWalking = false;
                                            AtkChooseNumbr = 0;
                                            goto SkipStatusJudgement;
                                        }
                                    }
                                }
                                isWalking = false;
                                goto SkipStatusJudgement;
                            case 2:
                                if (_time >= (Atk3CoolDown + Atk3LastTime))
                                {
                                    status = Status.Atk3;
                                    isWalking = false;
                                    goto SkipStatusJudgement;
                                }
                                else
                                {
                                    AtkChooseNumbr = Random.Range(1, 4);
                                    if (AtkChooseNumbr != 3)
                                    {
                                        if (CanAtk)
                                        {
                                            status = Status.Atk;
                                            isWalking = false;
                                            AtkChooseNumbr = 0;
                                            goto SkipStatusJudgement;
                                        }
                                    }
                                    else
                                    {
                                        if (CanBlock)
                                        {
                                            status = Status.Block;
                                            isWalking = false;
                                            AtkChooseNumbr = 0;
                                            goto SkipStatusJudgement;
                                        }
                                        else
                                        {
                                            if (CanAtk)
                                            {
                                                status = Status.Atk;
                                                isWalking = false;
                                                AtkChooseNumbr = 0;
                                                goto SkipStatusJudgement;
                                            }
                                        }
                                    }
                                    isWalking = false;
                                    goto SkipStatusJudgement;
                                }
                        }
                    }

                    if (_basicData.AbsDistanceX <= Atk2Distance && _basicData.AbsDistanceX > AtkDistance * 4 && CanAtk && _basicData.AbsDistanceY <= 1.5)
                    {
                        status = Status.Atk2;
                        isWalking = false;
                        goto SkipStatusJudgement;
                    }
                }
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
            case Status.Atk:
                isAtk = true;
                timerSwitch = true;
                break;
            case Status.Atk2:
                isAtk2 = true;
                timerSwitch = true;
                break;
            case Status.Atk3:
                isAtk3 = true;
                timerSwitch = true;
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
                _blockController.isBlock = true;
                _blockController.BlockTimerMethod(_deltaTime);
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
                        _weakData.WeakOffsetX = -0.26f;
                        break;
                    case MonsterBasicData.Face.Right:
                        _weakData.WeakOffsetX = 0.26f;
                        break;
                }
                _blockController.WeakTimerMethod(_weakData, _deltaTime);
                if (_blockController.isWeakEndJudge)
                {
                    _basicData.TurnFaceJudge();
                    status = Status.alert;
                    _blockController.isWeakEndJudge = false;
                }
                break;
        }

    SkipStatusJudgement:
        if (!isFirstTurnRun)
        {
            isFirstTurnRun = true;
        }

        if (status == Status.Block || status == Status.BeBlock || status == Status.Weak || status == Status.Atk3)
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
                MonsterMoveTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                MonsterSlowMoveTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                MonsterAtkTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                MonsterAtk2Tr.localScale = new Vector3(-0.28f, 0.28f, 0);
                MonsterAtk3Tr.localScale = new Vector3(-0.28f, 0.28f, 0);
                MonsterStopTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                MonsterBeBlockTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                MonsterBlockTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                MonsterJumpTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                break;

            case MonsterBasicData.Face.Right:
                MonsterMoveTr.localScale = new Vector3(0.28f, 0.28f, 0);
                MonsterSlowMoveTr.localScale = new Vector3(0.28f, 0.28f, 0);
                MonsterAtkTr.localScale = new Vector3(0.28f, 0.28f, 0);
                MonsterAtk2Tr.localScale = new Vector3(0.28f, 0.28f, 0);
                MonsterAtk3Tr.localScale = new Vector3(0.28f, 0.28f, 0);
                MonsterStopTr.localScale = new Vector3(0.28f, 0.28f, 0);
                MonsterBeBlockTr.localScale = new Vector3(0.28f, 0.28f, 0);
                MonsterBlockTr.localScale = new Vector3(0.28f, 0.28f, 0);
                MonsterJumpTr.localScale = new Vector3(0.28f, 0.28f, 0);
                break;
        }
        SlowWalkTimerMethod();
        SwitchAnimation();
    }
    private void FixedUpdate()
    {
        _fixDeltaTime = Time.fixedDeltaTime;

        _jumpController.Jump(_fixDeltaTime);

        _hurtedController.HurtedMove(_fixDeltaTime);

        _hurtedController.BurningTimerMethod(AtkTemporaryArea, _fixDeltaTime);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //暫時變得可穿透
        if (collision.gameObject.tag == "monster")
        {
            if (collision.gameObject.GetComponent<MonsterBasicData>() != null && collision.gameObject.GetComponent<GhostManController>() != null)
            {
                if (_basicData.Order > collision.gameObject.GetComponent<MonsterBasicData>().Order && !collision.gameObject.GetComponent<MonsterBasicData>().ShouldIgnore)
                {
                    if (status == Status.walk || status == Status.alert)
                    {
                        if (!isMonsterOrderChange)
                        {
                            MonsterOrderRecord = collision.gameObject.GetComponent<MonsterBasicData>().Order;
                            collision.gameObject.GetComponent<MonsterBasicData>().Order = _basicData.Order;
                            isMonsterOrderChange = true;
                        }
                        status = Status.alert;
                        isWalking = false;
                        isMonsterInRange = true;
                        isLittleWait = true;
                    }
                }
            }
            Rigid2D.gravityScale = 0;
            Rigid2D.drag = 10;
            ThisBoxCollider.isTrigger = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "monster")
        {
            Rigid2D.gravityScale = 7;
            Rigid2D.drag = 4;
            ThisBoxCollider.isTrigger = false;
            isMonsterInRange = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //暫時變得可穿透
        if (collision.gameObject.tag == "monster" && collision.gameObject.GetComponent<GhostController>() == null)
        {
            if (collision.gameObject.GetComponent<MonsterBasicData>() != null && collision.gameObject.GetComponent<GhostManController>() != null)
            {
                if (_basicData.Order > collision.gameObject.GetComponent<MonsterBasicData>().Order && !collision.gameObject.GetComponent<MonsterBasicData>().ShouldIgnore)
                {
                    if (status == Status.walk || status == Status.alert)
                    {
                        if (!isMonsterOrderChange)
                        {
                            MonsterOrderRecord = collision.gameObject.GetComponent<MonsterBasicData>().Order;
                            collision.gameObject.GetComponent<MonsterBasicData>().Order = _basicData.Order;
                            isMonsterOrderChange = true;
                        }
                        status = Status.alert;
                        isWalking = false;
                        isMonsterInRange = true;
                        isLittleWait = true;
                    }
                }
            }
            Rigid2D.drag = 10;
            Rigid2D.gravityScale = 0;
            ThisBoxCollider.isTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "monster")
        {
            Rigid2D.gravityScale = 7;
            Rigid2D.drag = 4;
            ThisBoxCollider.isTrigger = false;
            isMonsterInRange = false;
        }
    }

    void SwitchAnimation()
    {
        if (status != Status.BeBlock && status != Status.Block)
        {
            NowAni = AniStatus.Wait;
        }

        if (isWalking)
        {
            NowAni = AniStatus.Walk;
        }

        if (isSlowWalk)
        {
            NowAni = AniStatus.SlowMove;
        }

        if (isAtk)
        {
            NowAni = AniStatus.Atk;
        }

        if (isAtk2)
        {
            NowAni = AniStatus.Atk2;
        }

        if (isAtk3)
        {
            NowAni = AniStatus.Atk3;
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

        if (_blockController.isBlock)
        {
            NowAni = AniStatus.Block;
        }

        if (status == Status.jump)
        {
            NowAni = AniStatus.Jump;
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
                MonsterSlowMoveAni.SetBool("isSlowWalk", true);
                break;
            case AniStatus.Atk:
                AllAniFalse();
                MonsterAtkAnimation.SetActive(true);
                break;
            case AniStatus.Atk2:
                AllAniFalse();
                MonsterAtk2Animation.SetActive(true);
                break;
            case AniStatus.Atk3:
                AllAniFalse();
                MonsterAtk3Animation.SetActive(true);
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
                break;
        }
    }

    private void HurtedByStopAtkAni()
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
            /*if (_hurtedController.CriticAtkAniAppear)
            {
                NowAni = AniStatus.Stop;
                AllAniFalse();
                MonsterStopAnimation.SetActive(true);
            }*/
        }
    }

    void AtkTimerMethod()
    {
        if (timerSwitch)
        {
            if (isAtk)
            {
                if (_blockController.BeBlockSuccess)
                {
                    AtkMove = true;
                    AtkLastTime = _time;
                    CanAtk = false;
                    isAtk = false;
                    status = Status.BeBlock;
                    timerSwitch = false;
                    AtkFirstAppear = false;
                    AtkSecondAppear = false;
                    return;
                }
                AtkTimer -= _deltaTime;
                if (AtkTimer <= (AtkTimerSet - 0.3))
                {
                    if (!AtkFirstAppear)
                    {
                        switch (_basicData.face)
                        {
                            case MonsterBasicData.Face.Right:
                                Instantiate(RAtk, _transform.position, Quaternion.identity, AtkTemporaryArea);
                                AtkTemporaryArea.transform.DetachChildren();
                                break;
                            case MonsterBasicData.Face.Left:
                                Instantiate(LAtk, _transform.position, Quaternion.identity, AtkTemporaryArea);
                                AtkTemporaryArea.transform.DetachChildren();
                                break;
                        }
                        AtkFirstAppear = true;
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
                                            _transform.position += new Vector3(AtkSpeed * _deltaTime, 0, 0);
                                        }
                                    }
                                    break;
                                case MonsterBasicData.Face.Left:
                                    if (!_basicData.touchLeftWall && !MonsterLeftJudgement.GetComponent<MonsterLeftJudgement>().isPlayerAtLeftSide)
                                    {
                                        if (!MonsterUnderJudgement.GetComponent<MonsterUnderJudgement>().isPlayerUnder)
                                        {
                                            _transform.position -= new Vector3(AtkSpeed * _deltaTime, 0, 0);
                                        }
                                    }
                                    break;
                            }
                        }
                        if (AtkTimer <= (AtkTimerSet - 0.75))
                        {
                            if (!AtkSecondAppear)
                            {
                                switch (_basicData.face)
                                {
                                    case MonsterBasicData.Face.Right:

                                        Instantiate(RAtk2, _transform.position, Quaternion.identity, AtkTemporaryArea);
                                        AtkTemporaryArea.transform.DetachChildren();
                                        break;
                                    case MonsterBasicData.Face.Left:

                                        Instantiate(LAtk2, _transform.position, Quaternion.identity, AtkTemporaryArea);
                                        AtkTemporaryArea.transform.DetachChildren();
                                        break;
                                }
                                AtkSecondAppear = true;
                            }
                            if (AtkTimer <= (AtkTimerSet - 0.81))
                            {
                                AtkMove = false;
                            }
                        }
                    }
                    if (AtkTimer <= 0)
                    {
                        _basicData.TurnFaceJudge();
                        AtkMove = true;
                        AtkLastTime = _time;
                        CanAtk = false;
                        isAtk = false;
                        status = Status.alert;
                        timerSwitch = false;
                        AtkFirstAppear = false;
                        AtkSecondAppear = false;
                    }
                }
            }
            if (isAtk2)
            {
                Atk2Timer -= _deltaTime;
                if (Atk2Timer <= (Atk2TimerSet - 0.35))
                {
                    if (!AtkFirstAppear)
                    {
                        switch (_basicData.face)
                        {
                            case MonsterBasicData.Face.Right:
                                Instantiate(RStringAtk, _transform.position, Quaternion.identity, AtkTemporaryArea);
                                AtkTemporaryArea.transform.DetachChildren();
                                break;
                            case MonsterBasicData.Face.Left:
                                Instantiate(LStringAtk, _transform.position, Quaternion.identity, AtkTemporaryArea);
                                AtkTemporaryArea.transform.DetachChildren();
                                break;
                        }
                        AtkFirstAppear = true;
                    }
                    if (Atk2Move)
                    {
                        switch (_basicData.face)
                        {
                            case MonsterBasicData.Face.Right:
                                if (!_basicData.touchRightWall)
                                {
                                    _transform.position += new Vector3(Atk2Speed * _deltaTime, 0, 0);
                                }
                                break;
                            case MonsterBasicData.Face.Left:
                                if (!_basicData.touchLeftWall)
                                {
                                    _transform.position -= new Vector3(Atk2Speed * _deltaTime, 0, 0);
                                }
                                break;
                        }
                    }
                    if (Atk2Timer <= (Atk2TimerSet - 0.8))
                    {
                        Atk2Move = false;
                        if (!AtkSecondAppear)
                        {
                            switch (_basicData.face)
                            {
                                case MonsterBasicData.Face.Right:
                                    Instantiate(RSwordExplosion, _transform.position, Quaternion.identity, AtkTemporaryArea);
                                    AtkTemporaryArea.transform.DetachChildren();
                                    break;
                                case MonsterBasicData.Face.Left:
                                    Instantiate(LSwordExplosion, _transform.position, Quaternion.identity, AtkTemporaryArea);
                                    AtkTemporaryArea.transform.DetachChildren();
                                    break;
                            }
                            AtkSecondAppear = true;
                        }
                        if (Atk2Timer <= 0)
                        {
                            _basicData.TurnFaceJudge();
                            AtkLastTime = _time;
                            CanAtk = false;
                            Atk2Move = true;
                            isAtk2 = false;
                            status = Status.alert;
                            timerSwitch = false;
                            AtkFirstAppear = false;
                            AtkSecondAppear = false;
                        }
                    }
                }
            }
            if (isAtk3)
            {
                Atk3Timer -= _deltaTime;
                if (Atk3Timer <= (Atk3TimerSet - 0.2))
                {
                    if (!AtkFirstAppear)
                    {
                        switch (_basicData.face)
                        {
                            case MonsterBasicData.Face.Left:
                                Instantiate(Explosion, _transform.position + LExplosionAppear, Quaternion.identity, AtkTemporaryArea);
                                break;
                            case MonsterBasicData.Face.Right:
                                Instantiate(Explosion, _transform.position + RExplosionAppear, Quaternion.identity, AtkTemporaryArea);
                                break;
                        }
                        AtkTemporaryArea.transform.DetachChildren();
                        AtkFirstAppear = true;
                    }
                    if (Atk3Timer <= 0)
                    {
                        AtkLastTime = _time;
                        CanAtk3 = false;
                        CanAtk = false;
                        Atk3LastTime = _time;
                        isAtk3 = false;
                        status = Status.alert;
                        timerSwitch = false;
                        AtkFirstAppear = false;
                    }
                }
            }
        }
        else
        {
            AtkTimer = AtkTimerSet;
            Atk2Timer = Atk2TimerSet;
            Atk3Timer = Atk3TimerSet;
        }
    }

    void SlowWalkTimerMethod()
    {
        if (SlowWalkTimerSwitch)
        {
            if (isLittleWait)
            {
                if (!isMonsterInRange)
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
                    isSlowWalk = false;
                    isLittleWait = false;
                    isMonsterOrderChange = false;
                    _basicData.Order = MonsterOrderRecord;
                }
            }
            else
            {
                if (isAlert)
                {
                    AlertSlowWalkTimer -= Time.deltaTime;
                    if (status != Status.alert)
                    {
                        SlowWalkTimerSwitch = false;
                        isSlowWalk = false;
                        return;
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
                    if (AlertSlowWalkTimer <= 3)
                    {
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
                        if (AlertSlowWalkTimer <= 0)
                        {
                            SlowWalkTimerSwitch = false;
                            AlertSlowWalkTimer = AlertSlowWalkTimerSet;
                        }
                    }
                }
                else
                {
                    SlowWalkTimer -= Time.deltaTime;
                    if (SlowWalkTimer <= (SlowWalkTimerSet - 2))
                    {
                        if (status != Status.wait)
                        {
                            SlowWalkTimerSwitch = false;
                            isSlowWalk = false;
                        }
                        _basicData.face = MonsterBasicData.Face.Left;
                        slowWalkStatus = SlowWalkStatus.walkLeft;
                        isSlowWalk = true;
                        if (SlowWalkTimer <= (SlowWalkTimerSet - 7))
                        {
                            slowWalkStatus = SlowWalkStatus.wait;
                            isSlowWalk = false;
                            if (SlowWalkTimer <= (SlowWalkTimerSet - 9))
                            {
                                _basicData.face = MonsterBasicData.Face.Right;
                                slowWalkStatus = SlowWalkStatus.walkRight;
                                isSlowWalk = true;
                                if (SlowWalkTimer <= 0)
                                {
                                    slowWalkStatus = SlowWalkStatus.wait;
                                    isSlowWalk = false;
                                    SlowWalkTimerSwitch = false;
                                    SlowWalkTimer = SlowWalkTimerSet;
                                }
                            }
                        }
                    }
                }
            }
            switch (slowWalkStatus)
            {
                case SlowWalkStatus.walkLeft:
                    isSlowWalk = true;
                    if (!_basicData.touchLeftWall && !_hurtedController.isHurted && !MonsterLeftJudgement.GetComponent<MonsterLeftJudgement>().isPlayerAtLeftSide)
                    {
                        if (!MonsterUnderJudgement.GetComponent<MonsterUnderJudgement>().isPlayerUnder)
                        {
                            this.transform.position -= new Vector3(SlowWalkSpeed * Time.deltaTime, 0, 0);
                        }
                    }
                    break;

                case SlowWalkStatus.wait:
                    break;

                case SlowWalkStatus.walkRight:
                    isSlowWalk = true;
                    if (!_basicData.touchRightWall && !_hurtedController.isHurted && !MonsterRightJudgement.GetComponent<MonsterRightJudgement>().isPlayerAtRightSide)
                    {
                        if (!MonsterUnderJudgement.GetComponent<MonsterUnderJudgement>().isPlayerUnder)
                        {
                            this.transform.position += new Vector3(SlowWalkSpeed * Time.deltaTime, 0, 0);
                        }
                    }
                    break;
            }
        }
        else
        {
            SlowWalkTimer = SlowWalkTimerSet;
            AlertSlowWalkTimer = AlertSlowWalkTimerSet;
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
            status = Status.alert;
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
            AtkLastTime = _time;
            CanAtk = false;
            status = Status.alert;
            _blockController.BlockUnSuccessEnd = false;
        }
        if (_blockController.BlockSuccessEnd)
        {
            BlockLastTime = _time;
            CanBlock = false;
            AtkLastTime = _time;
            CanAtk = false;
            status = Status.alert;
            _blockController.BlockSuccessEnd = false;
        }
        if (_blockController.BlockHurtedEnd)
        {
            BlockLastTime = _time;
            AtkLastTime = _time;
            CanAtk = false;
            CanBlock = false;
            status = Status.alert;
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
        _weakData.EndSizeY = 2f;
    }
}
