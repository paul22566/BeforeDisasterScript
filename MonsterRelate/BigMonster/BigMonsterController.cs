using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigMonsterController : MonoBehaviour
{
    [Header("基本參數")]
    public int ChasingDistance;
    public int GiveUpDistanceX;
    public int GiveUpDistanceY;
    public int AlertDistance;
    public float AtkDistance;
    public float speed;
    public float SlowWalkSpeed;
    public enum Status { wait, alert, walk, jump, Atk, Atk2, Atk3, Block, BeBlock, Weak };
    private Status status;
    public enum SlowWalkStatus { walkLeft, wait, walkRight };
    private SlowWalkStatus slowWalkStatus;
    public enum AniStatus { Wait, Walk, Atk, Atk2, Atk3, Stop, BeBlock, Block, Jump }
    [HideInInspector] public AniStatus NowAni;//script(Hurted)

    [Header("系統參數")]
    private Transform _transform;
    private float _deltaTime;
    private float _time;
    private float _fixedDeltatime;
    private Transform AtkTemporaryArea;
    private GameObject MonsterRightJudgement;
    private GameObject MonsterLeftJudgement;
    private GameObject MonsterUnderJudgement;
    private float AtkTimerSet = 0.75f;
    private float AtkTimer;
    private float Atk2TimerSet = 0.65f;
    private float Atk2Timer;
    private float Atk3TimerSet = 1.15f;
    private float Atk3Timer;
    public float AtkCoolDown;//2種Atk共用
    private float AtkLastTime = -10;
    public float Atk3CoolDown;
    private float Atk3LastTime = -10;
    public float AlertSlowWalkTimerSet;
    private float AlertSlowWalkTimer;
    public float LittleWaitTimerSet;
    private float LittleWaitTimer;
    private int MonsterOrderRecord;
    private float TurnFaceLagTimerSet = 0.2f;
    private float TurnFaceLagTimer;
    private Rigidbody2D Rigid2D;
    private BoxCollider2D _boxCollider;
    private MonsterHurtedController _hurtedController;
    private MonsterJumpController _jumpController;
    private MonsterBasicData _basicData;
    private int AtkChooseNumbr;
    private float GroundPointY;

    //開關
    private bool isWalking;
    private bool isAtk;
    private bool isAtk2;
    private bool isAtk3;
    private bool timerSwitch;
    private bool SlowWalkTimerSwitch;
    private bool AtkFirstAppear;
    private bool isAlert;
    private bool isAlertByFarAtk = false;
    private bool CanAtk;
    private bool CanAtk3;
    private bool isLittleWait;
    private bool isMonsterInRange;
    private bool isMonsterOrderChange;
    private bool isFirstTurnRun;

    [Header("動畫相關物件")]
    private Animator MonsterMoveAni;
    private GameObject MonsterMoveAnimation;
    private GameObject MonsterAtkAnimation;
    private GameObject MonsterAtk2Animation;
    private GameObject MonsterAtk3Animation;
    private GameObject MonsterJumpAnimation;
    private Transform MonsterMoveTr;
    private Transform MonsterAtkTr;
    private Transform MonsterAtk2Tr;
    private Transform MonsterAtk3Tr;
    private Transform MonsterJumpTr;
    public GameObject RBlockRoute;
    public GameObject LBlockRoute;

    [Header("攻擊物件")]
    public GameObject RAtk;
    public GameObject LAtk;
    public GameObject RTurnAtk;
    public GameObject LTurnAtk;
    public GameObject Explosion;
    private Vector3 LExplosionAppear = new Vector3(-0.24f, 0, 0);
    private Vector3 RExplosionAppear = new Vector3(0.24f, 0, 0);

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
    public float BlockCoolDown;
    private float BlockLastTime;
    private GameObject MonsterBlockAnimation;
    private Animator MonsterBlockAni;
    private Transform MonsterBlockTr;
    void Start()
    {
        //抓取動畫物件
        MonsterMoveAnimation = this.transform.GetChild(0).gameObject;
        MonsterAtkAnimation = this.transform.GetChild(1).gameObject;
        MonsterAtk2Animation = this.transform.GetChild(2).gameObject;
        MonsterAtk3Animation = this.transform.GetChild(3).gameObject;
        MonsterStopAnimation = this.transform.GetChild(5).gameObject;
        MonsterBeBlockAnimation = this.transform.GetChild(6).gameObject;
        MonsterBlockAnimation = this.transform.GetChild(7).gameObject;
        MonsterJumpAnimation = this.transform.GetChild(8).gameObject;

        MonsterMoveTr = MonsterMoveAnimation.transform.GetChild(0);
        MonsterAtkTr = MonsterAtkAnimation.transform.GetChild(0);
        MonsterAtk2Tr = MonsterAtk2Animation.transform.GetChild(0);
        MonsterAtk3Tr = MonsterAtk3Animation.transform.GetChild(0);
        MonsterStopTr = MonsterStopAnimation.transform.GetChild(0);
        MonsterBeBlockTr = MonsterBeBlockAnimation.transform.GetChild(0);
        MonsterBlockTr = MonsterBlockAnimation.transform.GetChild(0);
        MonsterJumpTr = MonsterJumpAnimation.transform.GetChild(0);

        MonsterMoveAni = MonsterMoveTr.GetComponent<Animator>();
        MonsterBeBlockAni = MonsterBeBlockTr.GetComponent<Animator>();
        MonsterBlockAni = MonsterBlockTr.GetComponent<Animator>();

        AtkTemporaryArea = this.transform.GetChild(4);
        status = Status.wait;
        slowWalkStatus = SlowWalkStatus.wait;

        _basicData = this.GetComponent<MonsterBasicData>();
        _hurtedController = this.transform.GetComponent<MonsterHurtedController>();
        _blockController = this.GetComponent<MonsterBlockController>();
        _jumpController = this.GetComponent<MonsterJumpController>();

        _basicData.BasicVarInisialize(MonsterMoveTr, "L");

        _transform = this.transform;
        Rigid2D = this.GetComponent<Rigidbody2D>();
        _boxCollider = this.GetComponent<BoxCollider2D>();

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
            CanAtk = false;
            CanAtk3 = false;
            CanBlock = false;
            isAtk = false;
            isAtk2 = false;
            isAtk3 = false;
            timerSwitch = false;
            AtkFirstAppear = false;
            status = Status.alert;
            AtkLastTime = _time;
            Atk3LastTime = _time;
            BlockLastTime = _time;
            AtkTimer = AtkTimerSet;
            Atk2Timer = Atk2TimerSet;
            Atk3Timer = Atk3TimerSet;
            _jumpController.AllVariableFalse();
            _blockController.AllVariableReset();
        }

        timer();
        _hurtedController.HurtedTimerMethod(_deltaTime);
        _hurtedController.CriticAtkHurtedTimerMethod(_deltaTime);
        HurtedByStopAtkAni();
        //判斷是否在地上
        _basicData.CheckTouchGround(ref GroundPointY);
        if (_basicData.isGround && !_hurtedController.isCriticAtkHurted)
        {
            _transform.position = new Vector3(_transform.position.x, GroundPointY + 1.29f, 0);
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
            isWalking = true;
            isAlert = true;
            _basicData.TurnFaceJudge();
        }
        if (_hurtedController.HurtedByFarAtk)
        {
            _hurtedController.HurtedByFarAtk = false;
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
            case Status.wait:
                if (_basicData.playerTransform)
                {
                    if (!isFirstTurnRun)
                    {
                        //在這裡放轉向判定是為了避免怪物剛出生攻擊方向錯誤Bug
                        _basicData.TurnFaceJudge();
                        goto SkipStatusJudgement;
                    }

                    if (_basicData.AbsDistanceX < ChasingDistance)
                    {
                        if (!isAtk && !isAtk2 && !isAtk3 && !_blockController.isBlock && isFirstTurnRun)
                        {
                            status = Status.walk;
                            isWalking = true;
                            isAlert = true;
                            goto SkipStatusJudgement;
                        }
                    }

                    if (_basicData.AbsDistanceX <= AtkDistance && CanAtk && _basicData.AbsDistanceY <= 3 && !isAtk2 && isFirstTurnRun)
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
                                        isAlert = true;
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
                                        isAlert = true;
                                        AtkChooseNumbr = 0;
                                        goto SkipStatusJudgement;
                                    }
                                    else
                                    {
                                        if (CanAtk)
                                        {
                                            status = Status.Atk;
                                            isWalking = false;
                                            isAlert = true;
                                            AtkChooseNumbr = 0;
                                            goto SkipStatusJudgement;
                                        }
                                    }
                                }
                                break;
                            case 2:
                                if (CanAtk3)
                                {
                                    status = Status.Atk3;
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
                                            isAlert = true;
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
                                            isAlert = true;
                                            AtkChooseNumbr = 0;
                                            goto SkipStatusJudgement;
                                        }
                                        else
                                        {
                                            if (CanAtk)
                                            {
                                                status = Status.Atk;
                                                isWalking = false;
                                                isAlert = true;
                                                AtkChooseNumbr = 0;
                                                goto SkipStatusJudgement;
                                            }
                                        }
                                    }
                                }
                                break;
                        }
                    }
                }
                break;
            case Status.alert:
                SlowWalkTimerSwitch = true;

                isWalking = true;

                if (_basicData.isPlayerAtLeftSide)
                {
                    if (_basicData.DistanceY < 5f)
                    {
                        if (_basicData.face == MonsterBasicData.Face.Right && !isAtk && !isAtk3 && !_blockController.isBlock)
                        {
                            status = Status.Atk2;
                            isWalking = false;
                            isLittleWait = false;
                            LittleWaitTimer = LittleWaitTimerSet;
                            goto SkipStatusJudgement;
                        }
                    }
                    else
                    {
                        if (_basicData.face == MonsterBasicData.Face.Right)
                        {
                            TurnFaceLagTimer -= _deltaTime;
                            if (TurnFaceLagTimer <= 0)
                            {
                                TurnFaceLagTimer = TurnFaceLagTimerSet;
                                _basicData.face = MonsterBasicData.Face.Left;
                            }
                        }
                        else
                        {
                            TurnFaceLagTimer = TurnFaceLagTimerSet;
                        }
                    }
                }
                if (_basicData.isPlayerAtRightSide)
                {
                    if (_basicData.DistanceY < 5f)
                    {
                        if (_basicData.face == MonsterBasicData.Face.Left && !isAtk && !isAtk3 && !_blockController.isBlock)
                        {
                            status = Status.Atk2;
                            isWalking = false;
                            isLittleWait = false;
                            LittleWaitTimer = LittleWaitTimerSet;
                            goto SkipStatusJudgement;
                        }
                    }
                    else
                    {
                        if (_basicData.face == MonsterBasicData.Face.Left)
                        {
                            TurnFaceLagTimer -= _deltaTime;
                            if (TurnFaceLagTimer <= 0)
                            {
                                TurnFaceLagTimer = TurnFaceLagTimerSet;
                                _basicData.face = MonsterBasicData.Face.Right;
                            }
                        }
                        else
                        {
                            TurnFaceLagTimer = TurnFaceLagTimerSet;
                        }
                    }
                }

                switch (slowWalkStatus)
                {
                    case SlowWalkStatus.walkLeft:
                        if (!_basicData.touchLeftWall && !_hurtedController.isHurted && !MonsterLeftJudgement.GetComponent<MonsterLeftJudgement>().isPlayerAtLeftSide)
                        {
                            if (!MonsterUnderJudgement.GetComponent<MonsterUnderJudgement>().isPlayerUnder)
                            {
                                _transform.position -= new Vector3(SlowWalkSpeed * _deltaTime, 0, 0);
                            }
                        }
                        break;

                    case SlowWalkStatus.wait:
                        break;

                    case SlowWalkStatus.walkRight:
                        if (!_basicData.touchRightWall && !_hurtedController.isHurted && !MonsterRightJudgement.GetComponent<MonsterRightJudgement>().isPlayerAtRightSide)
                        {
                            if (!MonsterUnderJudgement.GetComponent<MonsterUnderJudgement>().isPlayerUnder)
                            {
                                _transform.position += new Vector3(SlowWalkSpeed * _deltaTime, 0, 0);
                            }
                        }
                        break;
                }

                if (_basicData.AbsDistanceX >= GiveUpDistanceX || _basicData.AbsDistanceY >= GiveUpDistanceY)
                {
                    isAlert = false;
                    isWalking = false;
                    status = Status.wait;
                    isAlertByFarAtk = false;
                    goto SkipStatusJudgement;
                }

                if (isLittleWait)
                {
                    goto SkipStatusJudgement;
                }

                _jumpController.JumpStartJudge();
                if (_jumpController.JumpStart)
                {
                    status = Status.jump;
                    isWalking = false;
                    goto SkipStatusJudgement;
                }

                if (_basicData.AbsDistanceX <= AtkDistance && CanAtk && _basicData.AbsDistanceY <= 3 && !isAtk2)
                {
                    switch (Random.Range(1, 3))
                    {
                        case 1:
                            AtkChooseNumbr = Random.Range(1, 4);
                            if (AtkChooseNumbr != 3)
                            {
                                status = Status.Atk;
                                isWalking = false;
                                AtkChooseNumbr = 0;
                                goto SkipStatusJudgement;
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
                                    status = Status.Atk;
                                    isWalking = false;
                                    AtkChooseNumbr = 0;
                                    goto SkipStatusJudgement;
                                }
                            }
                        case 2:
                            if (CanAtk3)
                            {
                                status = Status.Atk3;
                            }
                            else
                            {
                                AtkChooseNumbr = Random.Range(1, 4);
                                if (AtkChooseNumbr != 3)
                                {
                                    status = Status.Atk;
                                    isWalking = false;
                                    AtkChooseNumbr = 0;
                                    goto SkipStatusJudgement;
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
                                        status = Status.Atk;
                                        isWalking = false;
                                        AtkChooseNumbr = 0;
                                        goto SkipStatusJudgement;
                                    }
                                }
                            }
                            break;
                    }
                    isWalking = false;
                }

                if (_basicData.AbsDistanceX > AlertDistance && _basicData.AbsDistanceY <= 3)
                {
                    if (!isAtk && !isAtk2 && !isAtk3 && !_blockController.isBlock)
                    {
                        status = Status.walk;
                        isWalking = true;
                        isAlert = true;
                        goto SkipStatusJudgement;
                    }
                }
                break;
            case Status.walk:
                if (_basicData.playerTransform)
                {
                    if (_basicData.isPlayerAtLeftSide)
                    {
                        if (_basicData.DistanceY < 5f)
                        {
                            if (_basicData.face == MonsterBasicData.Face.Right && !isAtk && !isAtk3 && !_blockController.isBlock)
                            {
                                status = Status.Atk2;
                                isWalking = false;
                                isLittleWait = false;
                                LittleWaitTimer = LittleWaitTimerSet;
                                goto SkipStatusJudgement;
                            }
                        }
                        else
                        {
                            if (_basicData.face == MonsterBasicData.Face.Right)
                            {
                                TurnFaceLagTimer -= _deltaTime;
                                if (TurnFaceLagTimer <= 0)
                                {
                                    TurnFaceLagTimer = TurnFaceLagTimerSet;
                                    _basicData.face = MonsterBasicData.Face.Left;
                                }
                            }
                            else
                            {
                                TurnFaceLagTimer = TurnFaceLagTimerSet;
                            }
                        }
                    }
                    if (_basicData.isPlayerAtRightSide)
                    {
                        if (_basicData.DistanceY < 5f)
                        {
                            if (_basicData.face == MonsterBasicData.Face.Left && !isAtk && !isAtk3 && !_blockController.isBlock)
                            {
                                status = Status.Atk2;
                                isWalking = false;
                                isLittleWait = false;
                                LittleWaitTimer = LittleWaitTimerSet;
                                goto SkipStatusJudgement;
                            }
                        }
                        else
                        {
                            if (_basicData.face == MonsterBasicData.Face.Left)
                            {
                                TurnFaceLagTimer -= _deltaTime;
                                if (TurnFaceLagTimer <= 0)
                                {
                                    TurnFaceLagTimer = TurnFaceLagTimerSet;
                                    _basicData.face = MonsterBasicData.Face.Right;
                                }
                            }
                            else
                            {
                                TurnFaceLagTimer = TurnFaceLagTimerSet;
                            }
                        }
                    }
                }

                if (!_hurtedController.isHurted)
                {
                    switch (_basicData.face)
                    {
                        case MonsterBasicData.Face.Right:
                            if (!_basicData.touchRightWall && !_hurtedController.isHurted && !MonsterRightJudgement.GetComponent<MonsterRightJudgement>().isPlayerAtRightSide)
                            {
                                if (!MonsterUnderJudgement.GetComponent<MonsterUnderJudgement>().isPlayerUnder)
                                {
                                    _transform.position += new Vector3(speed * _deltaTime, 0, 0);
                                }
                            }
                            break;
                        case MonsterBasicData.Face.Left:
                            if (!_basicData.touchLeftWall && !_hurtedController.isHurted && !MonsterLeftJudgement.GetComponent<MonsterLeftJudgement>().isPlayerAtLeftSide)
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

                    if (_basicData.AbsDistanceX <= AtkDistance && CanAtk && _basicData.AbsDistanceY <= 3 && !isAtk2)
                    {
                        switch (Random.Range(1, 3))
                        {
                            case 1:
                                AtkChooseNumbr = Random.Range(1, 4);
                                if (AtkChooseNumbr != 3)
                                {
                                    status = Status.Atk;
                                    isWalking = false;
                                    AtkChooseNumbr = 0;
                                    goto SkipStatusJudgement;
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
                                        status = Status.Atk;
                                        isWalking = false;
                                        AtkChooseNumbr = 0;
                                        goto SkipStatusJudgement;
                                    }
                                }
                            case 2:
                                if (CanAtk3)
                                {
                                    status = Status.Atk3;
                                }
                                else
                                {
                                    AtkChooseNumbr = Random.Range(1, 4);
                                    if (AtkChooseNumbr != 3)
                                    {
                                        status = Status.Atk;
                                        isWalking = false;
                                        AtkChooseNumbr = 0;
                                        goto SkipStatusJudgement;
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
                                            status = Status.Atk;
                                            isWalking = false;
                                            AtkChooseNumbr = 0;
                                            goto SkipStatusJudgement;
                                        }
                                    }
                                }
                                break;
                        }
                        isWalking = false;
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
                        _weakData.WeakOffsetX = 0.65f;
                        break;
                    case MonsterBasicData.Face.Right:
                        _weakData.WeakOffsetX = -0.65f;
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
        isFirstTurnRun = true;

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
                MonsterMoveTr.localScale = new Vector3(0.39f, 0.39f, 0);
                MonsterAtkTr.localScale = new Vector3(0.39f, 0.39f, 0);
                MonsterAtk2Tr.localScale = new Vector3(0.39f, 0.39f, 0);
                MonsterAtk3Tr.localScale = new Vector3(0.39f, 0.39f, 0);
                MonsterStopTr.localScale = new Vector3(0.39f, 0.39f, 0);
                MonsterBeBlockTr.localScale = new Vector3(0.39f, 0.39f, 0);
                MonsterBlockTr.localScale = new Vector3(0.39f, 0.39f, 0);
                MonsterJumpTr.localScale = new Vector3(0.39f, 0.39f, 0);
                break;
            case MonsterBasicData.Face.Right:
                MonsterMoveTr.localScale = new Vector3(-0.39f, 0.39f, 0);
                MonsterAtkTr.localScale = new Vector3(-0.39f, 0.39f, 0);
                MonsterAtk2Tr.localScale = new Vector3(-0.39f, 0.39f, 0);
                MonsterAtk3Tr.localScale = new Vector3(-0.39f, 0.39f, 0);
                MonsterStopTr.localScale = new Vector3(-0.39f, 0.39f, 0);
                MonsterBeBlockTr.localScale = new Vector3(-0.39f, 0.39f, 0);
                MonsterBlockTr.localScale = new Vector3(-0.39f, 0.39f, 0);
                MonsterJumpTr.localScale = new Vector3(-0.39f, 0.39f, 0);
                break;
        }
        SlowWalkTimerMethod();
        SwitchAnimation();
    }

    private void FixedUpdate()
    {
        _fixedDeltatime = Time.fixedDeltaTime;

        _jumpController.Jump(_fixedDeltatime);

        _hurtedController.HurtedMove(_fixedDeltatime);

        _hurtedController.BurningTimerMethod(AtkTemporaryArea, _fixedDeltatime);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //暫時變得可穿透
        if (collision.gameObject.tag == "monster")
        {
            if (collision.gameObject.GetComponent<MonsterBasicData>() != null && collision.gameObject.GetComponent<BigMonsterController>() != null)
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
            _boxCollider.isTrigger = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "monster")
        {
            Rigid2D.gravityScale = 7;
            Rigid2D.drag = 4;
            _boxCollider.isTrigger = false;
            isMonsterInRange = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //暫時變得可穿透
        if (collision.gameObject.tag == "monster" && collision.gameObject.GetComponent<GhostController>() == null)
        {
            if (collision.gameObject.GetComponent<MonsterBasicData>() != null && collision.gameObject.GetComponent<BigMonsterController>() != null)
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
            _boxCollider.isTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "monster")
        {
            Rigid2D.gravityScale = 7;
            Rigid2D.drag = 4;
            _boxCollider.isTrigger = false;
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
            case AniStatus.Stop:
                AllAniFalse();
                MonsterStopAnimation.SetActive(true);
                break;
            case AniStatus.Jump:
                AllAniFalse();
                MonsterJumpAnimation.SetActive(true);
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
                    MonsterStopTr.localScale = new Vector3(0.39f, 0.39f, 0);
                    break;
                case MonsterBasicData.Face.Right:
                    MonsterStopTr.localScale = new Vector3(-0.39f, 0.39f, 0);
                    break;
            }
        }
        if (_hurtedController.isCriticAtkHurted)
        {
            switch (_basicData.face)
            {
                case MonsterBasicData.Face.Left:
                    MonsterStopTr.localScale = new Vector3(0.39f, 0.39f, 0);
                    break;
                case MonsterBasicData.Face.Right:
                    MonsterStopTr.localScale = new Vector3(-0.39f, 0.39f, 0);
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

    void timer()
    {
        if (timerSwitch)
        {
            if (isAtk)
            {
                if (_blockController.BeBlockSuccess)
                {
                    AtkLastTime = _time;
                    CanAtk = false;
                    isAtk = false;
                    isWalking = false;
                    status = Status.BeBlock;
                    timerSwitch = false;
                    AtkFirstAppear = false;
                    return;
                }
                AtkTimer -= _deltaTime;
                if (AtkTimer <= (AtkTimerSet - 0.5))
                {
                    if (!AtkFirstAppear)
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
                        AtkFirstAppear = true;
                    }
                    if (AtkTimer <= 0)
                    {
                        _basicData.TurnFaceJudge();
                        AtkLastTime = _time;
                        CanAtk = false;
                        isAtk = false;
                        status = Status.alert;
                        timerSwitch = false;
                        AtkFirstAppear = false;
                    }
                }
            }
            if (isAtk2)
            {
                if (_blockController.BeBlockSuccess)
                {
                    switch (_basicData.face)
                    {
                        case MonsterBasicData.Face.Left:
                            _basicData.face = MonsterBasicData.Face.Right;
                            break;
                        case MonsterBasicData.Face.Right:
                            _basicData.face = MonsterBasicData.Face.Left;
                            break;
                    }
                    isAtk2 = false;
                    status = Status.BeBlock;
                    isWalking = false;
                    timerSwitch = false;
                    AtkFirstAppear = false;
                    return;
                }
                Atk2Timer -= _deltaTime;
                if (Atk2Timer <= (Atk2TimerSet - 0.45))
                {
                    if (!AtkFirstAppear)
                    {
                        switch (_basicData.face)
                        {
                            case MonsterBasicData.Face.Right:
                                Instantiate(RTurnAtk, _transform.position, Quaternion.identity, AtkTemporaryArea);
                                AtkTemporaryArea.DetachChildren();
                                break;
                            case MonsterBasicData.Face.Left:
                                Instantiate(LTurnAtk, _transform.position, Quaternion.identity, AtkTemporaryArea);
                                AtkTemporaryArea.DetachChildren();
                                break;
                        }
                        AtkFirstAppear = true;
                    }
                    if (Atk2Timer <= 0)
                    {
                        if (_basicData.face == MonsterBasicData.Face.Left)
                        {
                            _basicData.face = MonsterBasicData.Face.Right;
                            _transform.position += new Vector3(1.45f, 0, 0);
                        }
                        else
                        {
                            _basicData.face = MonsterBasicData.Face.Left;
                            _transform.position -= new Vector3(1.45f, 0, 0);
                        }
                        isAtk2 = false;
                        status = Status.alert;
                        timerSwitch = false;
                        AtkFirstAppear = false;
                    }
                }
            }
            if (isAtk3)
            {
                Atk3Timer -= _deltaTime;
                if (Atk3Timer <= (Atk3TimerSet - 0.1))
                {
                    if (!AtkFirstAppear)
                    {
                        switch (_basicData.face)
                        {
                            case MonsterBasicData.Face.Left:
                                Instantiate(Explosion, _transform.position + LExplosionAppear, Quaternion.identity, AtkTemporaryArea);
                                AtkTemporaryArea.DetachChildren();
                                break;
                            case MonsterBasicData.Face.Right:
                                Instantiate(Explosion, _transform.position + RExplosionAppear, Quaternion.identity, AtkTemporaryArea);
                                AtkTemporaryArea.DetachChildren();
                                break;
                        }
                        AtkFirstAppear = true;
                    }
                    if (Atk3Timer <= 0)
                    {
                        _basicData.TurnFaceJudge();
                        AtkLastTime = _time;
                        CanAtk = false;
                        CanAtk3 = false;
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
                    isLittleWait = false;
                    isMonsterOrderChange = false;
                    _basicData.Order = MonsterOrderRecord;
                }
            }
            else
            {
                if (isAlert)
                {
                    AlertSlowWalkTimer -= _deltaTime;
                    if (status != Status.alert)
                    {
                        SlowWalkTimerSwitch = false;
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
            }
        }
        else
        {
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
        if (NowAni != AniStatus.Wait && NowAni != AniStatus.Walk)
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
        if (NowAni != AniStatus.Atk3)
        {
            MonsterAtk3Animation.SetActive(false);
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
        if (NowAni != AniStatus.Jump)
        {
            MonsterJumpAnimation.SetActive(false);
        }
    }

    private void WeakDataInisialize()
    {
        _weakData.WeakMass = 10000;
        _weakData.EndMass = 50;
        _weakData.WeakOffsetX = 0;
        _weakData.WeakOffsetY = -0.09f;
        _weakData.WeakSizeX = 0.77f;
        _weakData.WeakSizeY = 2.37f;
        _weakData.EndOffsetX = 0;
        _weakData.EndOffsetY = -0.09f;
        _weakData.EndSizeX = 0.77f;
        _weakData.EndSizeY = 2.37f;
    }
}

