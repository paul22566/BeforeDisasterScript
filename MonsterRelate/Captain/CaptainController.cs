using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaptainController : MonoBehaviour
{
    [Header("基本參數")]
    [HideInInspector] public float hp;//有被其他script用到(CaptainHpUi，Boss2RoomController)
    public int maxHp;
    private float CloseAtkDistance = 5;
    private float JumpCloseDistance = 10;
    private float speed = 2;
    private float AtkSpeed = 3.317f;
    private float Atk3Speed = 10;
    private float Atk3EndSpeed = 2.6f;
    private float Atk4Speed = 2;
    private float WalkTimeRecord;
    private float JumpSpeed = 15;
    private int ImpulseAtk2PowerY = 370;
    public float MapMiddle;
    public float LeftEdge;
    public float RightEdge;
    public float YButtomEdge;
    private float Atk2ContinueDistance = 4;
    private float Atk3EndDistance = 2.5f;
    private float JumpEndLine = -12.3f;//跳躍停止點
    private float LowestLine = -12.5f;//怪物最低界線
    private float ShouldOnPoint = -12.49f;//怪物修正點
    public enum Status { wait, walk, Jump, Atk, Atk2, Atk3, Atk4, Atk5, Weak, Block, BeBlock, BlockAtkSuccess, Burning };
    public static Status status;
    private Status LastStatus;
    public enum Face { Right, Left };
    [HideInInspector] public Face face;//script(2王眼睛)
    private enum JumpStatus { JumpFar, JumpClose };
    private JumpStatus jumpStatus;

    public enum AniStatus { Wait, Walk, Jump, Atk, Atk2, Atk3, Atk4, Atk5, Weak, TurnPhase, Block, BeBlock, Stop, Burning }
    [HideInInspector] public AniStatus NowAni;//script(2王受傷)

    [Header("系統參數")]
    private Transform AtkTemporaryArea;
    private GameObject MonsterRightJudgement;
    private GameObject MonsterLeftJudgement;
    private Transform _transform;
    private BattleSystem playerBattleSystem;
    private float _deltaTime;
    private float _time;
    private float _fixDeltaTime;
    private float WaitTimerSet = 0.1f;
    private float WaitTimer;
    private float AtkTimerSet = 1.7f;
    private float AtkTimer;
    private float Atk2TimerSet = 2.35f;
    private float Atk2Timer;
    private float Atk3PrepareTimerSet = 0.35f;
    private float Atk3PrepareTimer;
    private float Atk3TimerSet = 1.2f;
    private float Atk3Timer;
    private float Atk4TimerSet = 3.85f;
    private float Atk4Timer;
    private float Atk5TimerSet = 1.7f;
    private float Atk5Timer;
    private float SecondPhaseTimer;
    private float SecondPhaseTimerSet = 1;
    private float JumpTimerSet = 0.1f;
    private float JumpTimer;
    private float JumpEndTimerSet = 0.5f;
    private float JumpEndTimer;
    private Transform playerTransform;
    private Rigidbody2D Rigid2D;
    private BoxCollider2D ThisBoxCollider;
    private MonsterHurtedController _hurtedController;
    private MonsterCaptureController _captureController;
    private RaycastHit2D GroundCheck;
    private RaycastHit2D LeftWallCheck;
    private RaycastHit2D RightWallCheck;
    private float DistanceX;//怪物與玩家的距離
    private Vector3 JumpJudgementOriginalPoint;
    private Vector3 JumpJudgementNextPoint;
    private float ParabolaConstant;
    private float ParabolaMiddlePointX;
    private float ParabolaNowX;
    private float BurningTimer;
    private float BurningTimerSet = 1.5f;


    //開關
    private bool isWait;
    private bool isWalking;
    private bool isAtk;
    private bool isAtk2;
    private bool isAtk2Continue;
    private bool isAtk2End;
    private bool isAtk3;
    private bool isAtk3Prepare;
    private bool isAtk4;
    private bool isAtk5;
    private bool isTurnFaceBeforeAtk;
    private bool isJump;
    private bool isJumpOpen;
    private bool canJumpStop = false;
    private bool isFirstWeak;
    private bool isSecondWeak;
    public static bool isSecondPhase = false;//script(眼睛控制)
    private bool timerSwitch;
    private bool JumpTimerSwitch;
    public static bool SecondPhaseTimerSwitch;//script(2王的攻擊script)
    private bool CanGoSecondPhase;
    private bool isWalkTimeRecord;
    private bool AtkFirstAppear;
    private bool AtkSecondAppear;
    private bool AtkThirdAppear;
    private bool AtkFourAppear;
    private bool isGround;
    private bool AtkMoveEnd = false;
    private bool Atk2MoveEnd = false;
    [HideInInspector] public bool Atk3MoveEnd = false;//有被其他script用到(GroundAtk)
    private bool Atk3EndMoveEnd = false;
    private bool Atk4MoveEnd = false;
    private bool isAtk1Continue;//script(眼睛script)
    private bool isAtk1End;
    private bool touchRightWall;
    private bool touchLefttWall;
    private bool isWeakByBlock;
    private bool JumpToLeft;
    private bool JumpToRight;
    private bool ParabolaCaculate;
    private bool StatusChooseEnd;
    private bool isJumpEnd;
    private bool isBurning;
    private bool isFireAppear;
    private bool isWaveAppear;

    [Header("動畫相關物件")]
    public Image HpImage;
    private Animator MonsterJumpAni;
    private Animator MonsterAtk1Ani;
    private Animator MonsterAtk2Ani;
    private Animator MonsterAtk3Ani;
    private Animator MonsterAtk4Ani;
    private Animator MonsterWeakAni;
    private GameObject MonsterWaitAnimation;
    private GameObject MonsterWalkAnimation;
    private GameObject MonsterJumpAnimation;
    private GameObject MonsterAtkAnimation;
    private GameObject MonsterAtk2Animation;
    private GameObject MonsterAtk3Animation;
    private GameObject MonsterAtk4Animation;
    private GameObject MonsterAtk5Animation;
    private GameObject MonsterWeakAnimation;
    public GameObject RMonsterDieAnimation;
    public GameObject LMonsterDieAnimation;
    private GameObject MonsterSecondPhaseAnimation;
    private GameObject MonsterBurningAnimation;
    public GameObject ExplosionAni;
    private Transform MonsterWaitTr;
    private Transform MonsterWalkTr;
    private Transform MonsterJumpTr;
    private Transform MonsterAtkTr;
    private Transform MonsterAtk2Tr;
    private Transform MonsterAtk3Tr;
    private Transform MonsterAtk5Tr;
    private Transform MonsterWeakTr;
    private Transform MonsterSecondPhaseTr;
    private Transform MonsterBurningTr;
    private SpriteRenderer MonsterAtk4Sprite;
    public GameObject WaveAni;
    public GameObject Fire;
    private GameObject _fire;

    [Header("攻擊物件")]
    public GameObject RAtk2;
    public GameObject LAtk2;
    public GameObject RTurnAtk2;
    public GameObject LTurnAtk2;
    public GameObject RAtk3;
    public GameObject LAtk3;
    public GameObject Atk4;
    public GameObject REndAtk4;
    public GameObject LEndAtk4;
    public GameObject RDragonFlash;
    public GameObject LDragonFlash;
    public GameObject REvilDragonFlash;
    public GameObject LEvilDragonFlash;
    public GameObject ShockWave;
    public GameObject Explosion;
    private Vector3 RDragonFlashAppear = new Vector3(1.73f, 0.22f, 0);
    private Vector3 LDragonFlashAppear = new Vector3(-1.73f, 0.22f, 0);
    private Vector3 RShockWaveAppear = new Vector3(1.894f, -0.796f, 0);
    private Vector3 LShockWaveAppear = new Vector3(-1.894f, -0.796f, 0);
    private Vector3 RExplosionAniAppear = new Vector3(0.358f, 0.831f, 0);
    private Vector3 LExplosionAniAppear = new Vector3(-0.358f, 0.831f, 0);

    [Header("被大招攻擊")]
    public GameObject CriticAtkHurtedObject;
    public static bool isCriticAtkHurted = false;//script(2王的攻擊script)
    private bool CriticAtkHurtedSwitch;
    private bool HasCriticAtkAppear;
    private bool HasImpulse;
    private float CriticAtkHurtedTimerSet = 1.65f;
    private float CriticAtkHurtedTimer;
    private GameObject MonsterStopAnimation;
    private Transform MonsterStopTr;
    private bool isHurtedByBigGun;
    private float CriticAtkImpulsePower = 150;

    [Header("格檔")]
    private MonsterBlockController _blockController;
    private WeakData _weakData = new WeakData();
    private GameObject MonsterBeBlockAnimation;
    private Transform MonsterBeBlockTr;
    //主動格檔
    private bool CanBlock;
    private float BlockCoolDown = 10;
    private float BlockLastTime;
    private bool isPlayerAtRightSide;
    private bool isPlayerAtLeftSide;
    private GameObject MonsterBlockAnimation;
    private Animator MonsterBlockAni;
    private Transform MonsterBlockTr;
    void Start()
    {
        _transform = transform;
        NowAni = AniStatus.Wait;
        isSecondPhase = false;
        if (GameEvent.PassBoss2)
        {
            Destroy(this.gameObject);
        }
        //抓取動畫物件
        MonsterWaitAnimation = this.transform.GetChild(0).gameObject;
        MonsterWalkAnimation = this.transform.GetChild(1).gameObject;
        MonsterJumpAnimation = this.transform.GetChild(2).gameObject;
        MonsterAtkAnimation = this.transform.GetChild(3).gameObject;
        MonsterAtk2Animation = this.transform.GetChild(4).gameObject;
        MonsterAtk3Animation = this.transform.GetChild(5).gameObject;
        MonsterAtk4Animation = this.transform.GetChild(6).gameObject;
        MonsterAtk5Animation = this.transform.GetChild(7).gameObject;
        MonsterWeakAnimation = this.transform.GetChild(8).gameObject;
        MonsterStopAnimation = this.transform.GetChild(9).gameObject;
        MonsterSecondPhaseAnimation = this.transform.GetChild(10).gameObject;
        MonsterBeBlockAnimation = this.transform.GetChild(12).gameObject;
        MonsterBlockAnimation = this.transform.GetChild(13).gameObject;
        MonsterBurningAnimation = this.transform.GetChild(14).gameObject;

        MonsterWaitTr = MonsterWaitAnimation.transform.GetChild(0);
        MonsterWalkTr = MonsterWalkAnimation.transform.GetChild(0);
        MonsterJumpTr = MonsterJumpAnimation.transform.GetChild(0);
        MonsterAtkTr = MonsterAtkAnimation.transform.GetChild(0);
        MonsterAtk2Tr = MonsterAtk2Animation.transform.GetChild(0);
        MonsterAtk3Tr = MonsterAtk3Animation.transform.GetChild(0);
        MonsterAtk5Tr = MonsterAtk5Animation.transform.GetChild(0);
        MonsterWeakTr = MonsterWeakAnimation.transform.GetChild(0);
        MonsterStopTr = MonsterStopAnimation.transform.GetChild(0);
        MonsterSecondPhaseTr = MonsterSecondPhaseAnimation.transform.GetChild(0);
        MonsterBeBlockTr = MonsterBeBlockAnimation.transform.GetChild(0);
        MonsterBlockTr = MonsterBlockAnimation.transform.GetChild(0);
        MonsterBurningTr = MonsterBurningAnimation.transform.GetChild(0);

        MonsterJumpAni = MonsterJumpTr.GetComponent<Animator>();
        MonsterAtk1Ani = MonsterAtkTr.GetComponent<Animator>();
        MonsterAtk2Ani = MonsterAtk2Tr.GetComponent<Animator>();
        MonsterAtk3Ani = MonsterAtk3Tr.GetComponent<Animator>();
        MonsterAtk4Ani = MonsterAtk4Animation.GetComponent<Animator>();
        MonsterWeakAni = MonsterWeakTr.GetComponent<Animator>();
        MonsterBlockAni = MonsterBlockTr.GetComponent<Animator>();

        MonsterAtk4Sprite = MonsterAtk4Animation.GetComponent<SpriteRenderer>();

        AtkTemporaryArea = this.transform.GetChild(11);
        hp = maxHp;
        status = Status.wait;
        if (MonsterWaitTr.localScale.x < 0)
        {
            face = Face.Left;
        }
        else
        {
            face = Face.Right;
        }
        if (GameObject.Find("player") != null)
        {
            playerTransform = GameObject.Find("player").transform;
            playerBattleSystem = playerTransform.GetComponent<BattleSystem>();
        }
        Rigid2D = this.transform.GetComponent<Rigidbody2D>();
        ThisBoxCollider = this.transform.GetComponent<BoxCollider2D>();
        _hurtedController = this.transform.GetComponent<MonsterHurtedController>();
        _blockController = this.GetComponent<MonsterBlockController>();
        _captureController = this.GetComponent<MonsterCaptureController>();

        SecondPhaseTimer = SecondPhaseTimerSet;
        MonsterRightJudgement = AtkTemporaryArea.transform.GetChild(0).gameObject;
        MonsterLeftJudgement = AtkTemporaryArea.transform.GetChild(1).gameObject;
        AtkTemporaryArea.transform.DetachChildren();

        //WeakData初始化
        WeakDataInisialize();
    }

    // Update is called once per frame
    void Update()
    {
        _deltaTime = Time.deltaTime;
        _time = Time.time;
        if (hp <= 0)
        {
            switch (face)
            {
                case Face.Left:
                    Boss2RoomController.FaceLeft = true;
                    Instantiate(LMonsterDieAnimation, this.transform.position, Quaternion.identity);
                    break;
                case Face.Right:
                    Boss2RoomController.FaceRight = true;
                    Instantiate(RMonsterDieAnimation, this.transform.position, Quaternion.identity);
                    break;
            }
            playerBattleSystem.isCaptured = false;
            Boss2RoomController.Boss2DiePosition = this.transform.position;
            Boss2RoomController.Boss2DiePosition = new Vector3(Boss2RoomController.Boss2DiePosition.x, -12.462f, 0);
            Destroy(this.gameObject);
            return;
        }
        if (hp <= (maxHp * 0.33))
        {
            if (!isSecondPhase)
            {
                CanGoSecondPhase = true;
            }
        }
        if (hp <= (maxHp * 0.5))
        {
            if (!isSecondWeak)
            {
                isWeakByBlock = false;
                status = Status.Weak;
                isFirstWeak = true;
                isSecondWeak = true;
            }
        }
        if (hp <= (maxHp * 0.75))
        {
            if (!isFirstWeak)
            {
                isWeakByBlock = false;
                status = Status.Weak;
                isFirstWeak = true;
            }
        }
        //最低高度
        if (_transform.position.y < LowestLine)
        {
            _transform.position = new Vector3(_transform.position.x, ShouldOnPoint, 0);
        }
        //特定情況變數重置
        if (isHurtedByBigGun || isCriticAtkHurted || _blockController.isWeak || isBurning)
        {
            if (isBurning && status != Status.Jump && !SecondPhaseTimerSwitch)
            {
                status = Status.Burning;
            }

            if (isCriticAtkHurted)
            {
                Rigid2D.mass = 20;
            }
            else
            {
                Rigid2D.mass = 10000;
            }
            if (!isBurning)
            {
                JumpTimerSwitch = false;
                ParabolaCaculate = false;
                canJumpStop = false;
                JumpToLeft = false;
                JumpToRight = false;
                isJump = false;
                isJumpOpen = false;
                isJumpEnd = false;
                JumpTimer = JumpTimerSet;
                JumpEndTimer = JumpEndTimerSet;
            }
            ThisBoxCollider.isTrigger = false;
            playerBattleSystem.isCaptured = false;
            isTurnFaceBeforeAtk = false;
            timerSwitch = false;
            isWalkTimeRecord = false;
            isWalking = false;
            isAtk = false;
            isAtk2 = false;
            isAtk3Prepare = false;
            isAtk3 = false;
            isAtk4 = false;
            isAtk5 = false;
            CanBlock = false;
            AtkMoveEnd = false;
            Atk2MoveEnd = false;
            Atk3MoveEnd = false;
            Atk3EndMoveEnd = false;
            Atk4MoveEnd = false;
            isAtk1End = false;
            isAtk1Continue = false;
            isAtk2End = false;
            isAtk2Continue = false;
            AtkFirstAppear = false;
            AtkSecondAppear = false;
            AtkThirdAppear = false;
            AtkFourAppear = false;
            AtkTimer = AtkTimerSet;
            Atk2Timer = Atk2TimerSet;
            Atk3Timer = Atk3TimerSet;
            Atk4Timer = Atk4TimerSet;
            Atk5Timer = Atk5TimerSet;
            BlockLastTime = _time;
            _captureController.AllVariableReset();
            _blockController.BlockVariableReset();
            //weak
            if (isCriticAtkHurted && _blockController.isWeak)
            {
                _blockController.WeakVariableReset();
            }
        }
        SecondPhaseTimerMethod();
        CriticAtkHurtedTimerMethod();
        _hurtedController.HurtedTimerMethod(_deltaTime);
        HurtedByBigGunAni();
        HpImage.transform.localScale = new Vector3((float)hp / (float)maxHp, HpImage.transform.localScale.y, HpImage.transform.localScale.z);
        GroundCheck = Physics2D.Raycast(_transform.position, -Vector2.up, 1f, 1024);
        if (GroundCheck)
        {
            isGround = true;
        }
        else
        {
            isGround = false;
        }
        LeftWallCheck = Physics2D.Raycast(_transform.position, Vector2.left, 0.2f, 64);
        if (LeftWallCheck)
        {
            touchLefttWall = true;
        }
        else
        {
            touchLefttWall = false;
        }
        RightWallCheck = Physics2D.Raycast(_transform.position, Vector2.right, 0.2f, 128);
        if (RightWallCheck)
        {
            touchRightWall = true;
        }
        else
        {
            touchRightWall = false;
        }

        if (GameEvent.isAniPlay || isCriticAtkHurted || isHurtedByBigGun)
        {
            return;
        }
        if (playerTransform)
        {
            DistanceX = Mathf.Abs(_transform.position.x - playerTransform.position.x);
            if (_transform.position.x - playerTransform.position.x >= 0f)
            {
                isPlayerAtLeftSide = true;
                isPlayerAtRightSide = false;
            }
            else
            {
                isPlayerAtLeftSide = false;
                isPlayerAtRightSide = true;
            }
        }
        if (_time >= BlockCoolDown + BlockLastTime)
        {
            CanBlock = true;
        }

        switch (status)
        {
            case Status.wait:
                if (!isWait)
                {
                    isWait = true;
                }
                WaitTimer -= _deltaTime;
                if (WaitTimer <= 0)
                {
                    StatusJudge();
                    StatusChooseEnd = false;
                    isWait = false;
                    WaitTimer = WaitTimerSet;
                }
                break;
            case Status.walk:
                LastStatus = Status.walk;
                if (CanGoSecondPhase)
                {
                    jumpStatus = JumpStatus.JumpFar;
                    status = Status.Jump;
                    return;
                }
                if (!isWalking)
                {
                    isWalking = true;
                }
                if (!isWalkTimeRecord)
                {
                    WalkTimeRecord = Random.Range(1, 3);
                    isWalkTimeRecord = true;
                }
                WalkTimeRecord -= _deltaTime;
                TurnFaceJudge();

                if (DistanceX <= CloseAtkDistance || WalkTimeRecord <= 0f)
                {
                    isWalkTimeRecord = false;
                    isWalking = false;
                    status = Status.wait;
                }
                break;
            case Status.Jump:
                LastStatus = Status.Jump;
                TurnFaceJudge();
                if (!isJump)
                {
                    ThisBoxCollider.isTrigger = true;
                    isJump = true;
                    JumpTimerSwitch = true;
                }
                if (!isJumpOpen)
                {
                    switch (jumpStatus)
                    {
                        case JumpStatus.JumpFar:
                            if (playerTransform.position.x >= MapMiddle)
                            {
                                JumpToLeft = true;
                                JumpToRight = false;
                                JumpJudgementOriginalPoint = _transform.position;
                                JumpJudgementNextPoint = _transform.position + new Vector3(-12f, 0f, 0f);
                                if (JumpJudgementNextPoint.x <= LeftEdge)
                                {
                                    JumpJudgementNextPoint = new Vector3(LeftEdge + 1f, -12.462f, 0f);
                                }
                                else
                                {
                                    JumpJudgementNextPoint = new Vector3(JumpJudgementNextPoint.x, -12.462f, 0f);
                                }
                            }
                            else
                            {
                                JumpToRight = true;
                                JumpToLeft = false;
                                JumpJudgementOriginalPoint = _transform.position;
                                JumpJudgementNextPoint = _transform.position + new Vector3(12f, 0f, 0f);
                                if (JumpJudgementNextPoint.x >= RightEdge)
                                {
                                    JumpJudgementNextPoint = new Vector3(RightEdge - 1f, -12.462f, 0f);
                                }
                                else
                                {
                                    JumpJudgementNextPoint = new Vector3(JumpJudgementNextPoint.x, -12.462f, 0f);
                                }
                            }
                            break;
                        case JumpStatus.JumpClose:
                            if (face != Face.Right)
                            {
                                if (face == Face.Left)
                                {
                                    JumpToLeft = true;
                                    JumpToRight = false;
                                    JumpJudgementOriginalPoint = _transform.position;
                                    JumpJudgementNextPoint = playerTransform.position + new Vector3(2f, 0f, 0f);
                                    JumpJudgementNextPoint = new Vector3(JumpJudgementNextPoint.x, -12.462f, 0f);
                                }
                            }
                            else
                            {
                                JumpToRight = true;
                                JumpToLeft = false;
                                JumpJudgementOriginalPoint = _transform.position;
                                JumpJudgementNextPoint = playerTransform.position + new Vector3(-2f, 0f, 0f);
                                JumpJudgementNextPoint = new Vector3(JumpJudgementNextPoint.x, -12.462f, 0f);
                            }
                            break;
                    }
                    JumpSpeed = 1.2f * Mathf.Abs(JumpJudgementOriginalPoint.x - JumpJudgementNextPoint.x);
                    isJumpOpen = true;
                }
                if (canJumpStop && _transform.position.y <= JumpEndLine)
                {
                    JumpTimerSwitch = false;
                    ParabolaCaculate = false;
                    canJumpStop = false;
                    _transform.position = new Vector3(_transform.position.x, ShouldOnPoint, 0f);
                    ThisBoxCollider.isTrigger = false;
                    JumpToLeft = false;
                    JumpToRight = false;
                    isJumpEnd = true;
                }
                JumpEndTimerMethod();
                break;
            case Status.Atk:
                LastStatus = Status.Atk;
                if (!isAtk)
                {
                    isAtk = true;
                    timerSwitch = true;
                }
                break;
            case Status.Atk2:
                LastStatus = Status.Atk2;
                if (!isAtk2)
                {
                    isAtk2 = true;
                    timerSwitch = true;
                }
                break;
            case Status.Atk3:
                LastStatus = Status.Atk3;
                if (!isAtk3Prepare)
                {
                    isAtk3Prepare = true;
                    timerSwitch = true;
                }
                break;
            case Status.Atk4:
                LastStatus = Status.Atk4;
                if (!isAtk4)
                {
                    isAtk4 = true;
                    timerSwitch = true;
                }
                break;
            case Status.Atk5:
                LastStatus = Status.Atk5;
                if (!isAtk5)
                {
                    isAtk5 = true;
                    timerSwitch = true;
                }
                break;
            case Status.Weak:
                _blockController.isWeak = true;
                switch (face)
                {
                    case Face.Left:
                        _weakData.WeakOffsetX = 0.65f;
                        break;
                    case Face.Right:
                        _weakData.WeakOffsetX = -0.65f;
                        break;
                }
                if (isWeakByBlock)
                {
                    _blockController.WeakTimerMethod(_weakData, 1);
                }
                else
                {
                    _blockController.WeakTimerMethod(_weakData, 3.5f);
                }

                if (_blockController.isWeakEndJudge)
                {
                    isWeakByBlock = false;
                    status = Status.Jump;
                    jumpStatus = JumpStatus.JumpFar;
                    _blockController.isWeakEndJudge = false;
                }
                break;
            case Status.Block:
                LastStatus = Status.Block;
                if (_captureController.isCaptureSuccess)
                {
                    _blockController.BlockVariableReset();
                    status = Status.BlockAtkSuccess;
                    return;
                }
                switch (face)
                {
                    case Face.Right:
                        _blockController.FaceLeft = false;
                        _blockController.FaceRight = true;
                        break;
                    case Face.Left:
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
            case Status.BlockAtkSuccess:
                switch (face)
                {
                    case Face.Right:
                        _captureController.FaceLeft = false;
                        _captureController.FaceRight = true;
                        break;
                    case Face.Left:
                        _captureController.FaceLeft = true;
                        _captureController.FaceRight = false;
                        break;
                }
                if (_captureController.CaptureAtkEnd)
                {
                    status = Status.walk;
                    _captureController.CaptureAtkEnd = false;
                }
                break;
        }

        switch (face)
        {
            case Face.Right:
                MonsterWaitTr.localScale = new Vector3(0.28f, 0.28f, 0);
                MonsterWalkTr.localScale = new Vector3(0.28f, 0.28f, 0);
                MonsterJumpTr.localScale = new Vector3(0.28f, 0.28f, 0);
                MonsterAtkTr.localScale = new Vector3(0.28f, 0.28f, 0);
                MonsterAtk2Tr.localScale = new Vector3(0.28f, 0.28f, 0);
                MonsterAtk3Tr.localScale = new Vector3(0.28f, 0.28f, 0);
                MonsterAtk5Tr.localScale = new Vector3(0.28f, 0.28f, 0);
                MonsterWeakTr.localScale = new Vector3(0.28f, 0.28f, 0);
                MonsterStopTr.localScale = new Vector3(0.28f, 0.28f, 0);
                MonsterBeBlockTr.localScale = new Vector3(0.28f, 0.28f, 0);
                MonsterBlockTr.localScale = new Vector3(0.28f, 0.28f, 0);
                MonsterBurningTr.localScale = new Vector3(0.28f, 0.28f, 0);
                MonsterSecondPhaseTr.localScale = new Vector3(0.28f, 0.28f, 0);
                MonsterAtk4Sprite.flipX = false;
                break;
            case Face.Left:
                MonsterWaitTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                MonsterWalkTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                MonsterJumpTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                MonsterAtkTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                MonsterAtk2Tr.localScale = new Vector3(-0.28f, 0.28f, 0);
                MonsterAtk3Tr.localScale = new Vector3(-0.28f, 0.28f, 0);
                MonsterAtk5Tr.localScale = new Vector3(-0.28f, 0.28f, 0);
                MonsterWeakTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                MonsterStopTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                MonsterBeBlockTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                MonsterBlockTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                MonsterBurningTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                MonsterSecondPhaseTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                MonsterAtk4Sprite.flipX = true;
                break;
        }
        SwitchAnimation();
    }

    private void FixedUpdate()
    {
        _time = Time.time;
        _fixDeltaTime = Time.fixedDeltaTime;

        if(status == Status.BlockAtkSuccess)
        {
            _captureController.CaptureSuccessTimerMethod(_fixDeltaTime);
        }

        if(isWalking)
        {
            switch (face)
            {
                case Face.Right:
                    if (!touchRightWall && !MonsterRightJudgement.GetComponent<MonsterRightJudgement>().isPlayerAtRightSide)
                    {
                        _transform.position += new Vector3(speed * _fixDeltaTime, 0f, 0f);
                    }
                    break;
                case Face.Left:
                    if (!touchLefttWall && !MonsterLeftJudgement.GetComponent<MonsterLeftJudgement>().isPlayerAtLeftSide)
                    {
                        _transform.position -= new Vector3(speed * _fixDeltaTime, 0f, 0f);
                    }
                    break;
            }
        }

        timer();

        if (isHurtedByBigGun)
        {
            if (_blockController.isWeak)
            {
                hp -= BattleSystem.BigGunPower;
            }
            else
            {
                hp -= BattleSystem.BigGunPower / 3f;
            }
        }
        if (isCriticAtkHurted || isHurtedByBigGun)
        {
            return;
        }
        JumpTimerMethod();

        BurningTimerMethod();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "bullet")
        {
            if (SecondPhaseTimerSwitch)
            {
                hp -= (BattleSystem.BulletHurtPower / 5);
            }
            else
            {
                hp -= BattleSystem.BulletHurtPower;
            }
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "normalAtk")
        {
            if (_blockController.isBlock)
            {
                if (face != Face.Right)
                {
                    if (face == Face.Left && isPlayerAtRightSide)
                    {
                        _hurtedController.isHurted = true;
                        if (SecondPhaseTimerSwitch)
                        {
                            hp -= (BattleSystem.NormalAtkHurtPower / 5);
                        }
                        else
                        {
                            BattleSystem.IncreaseTimes += BattleSystem.IncresePlayerPowerNumber;
                            hp -= BattleSystem.NormalAtkHurtPower;
                        }
                        Destroy(other.gameObject);
                    }
                }
                else if (isPlayerAtLeftSide)
                {
                    _hurtedController.isHurted = true;
                    if (SecondPhaseTimerSwitch)
                    {
                        hp -= (BattleSystem.NormalAtkHurtPower / 5);
                    }
                    else
                    {
                        BattleSystem.IncreaseTimes += BattleSystem.IncresePlayerPowerNumber;
                        hp -= BattleSystem.NormalAtkHurtPower;
                    }
                    Destroy(other.gameObject);
                }
            }
            else
            {
                _hurtedController.isHurted = true;
                if (SecondPhaseTimerSwitch)
                {
                    hp -= (BattleSystem.NormalAtkHurtPower / 5);
                }
                else
                {
                    BattleSystem.IncreaseTimes += BattleSystem.IncresePlayerPowerNumber;
                    hp -= BattleSystem.NormalAtkHurtPower;
                }
                Destroy(other.gameObject);
            }
        }
        if (other.gameObject.tag == "CAtk")
        {
            _hurtedController.isHurted = true;
            if (SecondPhaseTimerSwitch)
            {
                hp -= (BattleSystem.CAtkHurtPower / 5);
            }
            else
            {
                hp -= BattleSystem.CAtkHurtPower;
            }
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "JumpCAtk")
        {
            _hurtedController.isHurted = true;
            if (SecondPhaseTimerSwitch)
            {
                hp -= (BattleSystem.CAtkHurtPower / 5);
            }
            else
            {
                hp -= BattleSystem.CAtkHurtPower;
            }
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "ExplosionBottle" && !_hurtedController.isHurted)
        {
            _hurtedController.isHurted = true;
            if (SecondPhaseTimerSwitch)
            {
                hp -= (BattleSystem.ExplosionHurtPower / 5);
            }
            else
            {
                hp -= BattleSystem.ExplosionHurtPower;
            }
        }
        if (other.gameObject.tag == "Cocktail" && !_hurtedController.isHurted)
        {
            if (!isBurning)
            {
                isBurning = true;
                BurningTimer = BurningTimerSet;
            }
        }
        if (other.gameObject.tag == "CriticAtk" && !SecondPhaseTimerSwitch)
        {
            isCriticAtkHurted = true;
            CriticAtkHurtedSwitch = true;
        }
        if (other.gameObject.tag == "BNAtk")
        {
            _hurtedController.isHurted = true;
            _blockController.isHurtedByNormalAtk = true;
            BattleSystem.IncreaseTimes += BattleSystem.IncresePlayerPowerNumber;
            hp -= BattleSystem.BlockNormalAtkHurtPower;
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "BSAtk")
        {
            _hurtedController.isHurted = true;
            _blockController.isHurtedByCAtk = true;
            hp -= BattleSystem.BlockStrongAtkHurtPower;
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "BigGun")
        {
            isHurtedByBigGun = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BigGun")
        {
            isHurtedByBigGun = false;
        }
    }

    private void SwitchAnimation()
    {
        if (isWait)
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

        if (isAtk3Prepare || isAtk3)
        {
            NowAni = AniStatus.Atk3;
        }

        if (isAtk4)
        {
            NowAni = AniStatus.Atk4;
        }

        if (isAtk5)
        {
            NowAni = AniStatus.Atk5;
        }

        if (_blockController.isWeak)
        {
            NowAni = AniStatus.Weak;
        }

        if (isJump)
        {
            NowAni = AniStatus.Jump;
        }

        if (_blockController.BeBlockSuccess && _blockController.CanBeBlockAniAppear)
        {
            _blockController.CanBeBlockAniAppear = false;
            NowAni = AniStatus.BeBlock;
        }

        if (_blockController.isBlock)
        {
            NowAni = AniStatus.Block;
        }

        if (_captureController.isCaptureSuccess)
        {
            NowAni = AniStatus.Block;
        }

        if(status == Status.Burning)
        {
            NowAni = AniStatus.Burning;
        }

        if (SecondPhaseTimerSwitch)
        {
            NowAni = AniStatus.TurnPhase;
        }

        switch (NowAni)
        {
            case AniStatus.Wait:
                AllAniFalse();
                MonsterWaitAnimation.SetActive(true);
                break;
            case AniStatus.Walk:
                AllAniFalse();
                MonsterWalkAnimation.SetActive(true);
                break;
            case AniStatus.Jump:
                AllAniFalse();
                MonsterJumpAnimation.SetActive(true);
                if (isJumpEnd)
                {
                    MonsterJumpAni.SetBool("End", true);
                }
                break;
            case AniStatus.Atk:
                AllAniFalse();
                MonsterAtkAnimation.SetActive(true);
                if (isAtk1Continue)
                {
                    MonsterAtk1Ani.SetBool("Atk", true);
                }
                break;
            case AniStatus.Atk2:
                AllAniFalse();
                MonsterAtk2Animation.SetActive(true);
                if (isAtk2Continue)
                {
                    MonsterAtk2Ani.SetBool("Atk", true);
                }
                if (isAtk2End)
                {
                    MonsterAtk2Ani.SetBool("GetUp", true);
                }
                break;
            case AniStatus.Atk3:
                AllAniFalse();
                MonsterAtk3Animation.SetActive(true);
                if (Atk3MoveEnd)
                {
                    MonsterAtk3Ani.SetBool("Stop", true);
                }
                break;
            case AniStatus.Atk4:
                AllAniFalse();
                MonsterAtk4Animation.SetActive(true);
                if (Atk4MoveEnd)
                {
                    MonsterAtk4Ani.SetBool("Stop", true);
                }
                break;
            case AniStatus.Atk5:
                AllAniFalse();
                MonsterAtk5Animation.SetActive(true);
                break;
            case AniStatus.Weak:
                AllAniFalse();
                MonsterWeakAnimation.SetActive(true);
                if (_blockController.isGetUp)
                {
                    MonsterWeakAni.SetBool("GetUp", true);
                }
                break;
            case AniStatus.Stop:
                AllAniFalse();
                MonsterStopAnimation.SetActive(true);
                break;
            case AniStatus.BeBlock:
                AllAniFalse();
                MonsterBeBlockAnimation.SetActive(true);
                break;
            case AniStatus.Block:
                AllAniFalse();
                MonsterBlockAnimation.SetActive(true);
                if (_blockController.CanBlockAniAppear)
                {
                    MonsterBlockAni.SetBool("Atk", true);
                }
                if (_captureController.isCaptureSuccess)
                {
                    MonsterBlockAni.SetBool("Success", true);
                }
                break;
            case AniStatus.Burning:
                AllAniFalse();
                MonsterBurningAnimation.SetActive(true);
                break;
            case AniStatus.TurnPhase:
                AllAniFalse();
                MonsterSecondPhaseAnimation.SetActive(true);
                break;
        }
    }

    private void HurtedByBigGunAni()
    {
        if (isHurtedByBigGun && !_blockController.isWeak)
        {
            NowAni = AniStatus.Stop;
            AllAniFalse();
            MonsterStopAnimation.SetActive(true);
            switch (face)
            {
                case Face.Left:
                    MonsterStopTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                    break;
                case Face.Right:
                    MonsterStopTr.localScale = new Vector3(0.28f, 0.28f, 0);
                    break;
            }
        }
    }

    private void timer()
    {
        if (timerSwitch)
        {
            if (isAtk)
            {
                if (_blockController.BeBlockSuccess)
                {
                    AtkMoveEnd = false;
                    isTurnFaceBeforeAtk = false;
                    AtkThirdAppear = false;
                    isAtk1End = false;
                    isAtk = false;
                    isAtk1Continue = false;
                    timerSwitch = false;
                    AtkFirstAppear = false;
                    AtkSecondAppear = false;
                    AtkTimer = AtkTimerSet;
                    status = Status.BeBlock;
                    return;
                }
                if (!isTurnFaceBeforeAtk)
                {
                    TurnFaceJudge();
                    isTurnFaceBeforeAtk = true;
                }
                AtkTimer -= _fixDeltaTime;
                if (AtkTimer <= AtkTimerSet - 0.25)
                {
                    if (!AtkMoveEnd)
                    {
                        switch (face)
                        {
                            case Face.Right:
                                if (!touchRightWall && !MonsterRightJudgement.GetComponent<MonsterRightJudgement>().isPlayerAtRightSide)
                                {
                                    _transform.localPosition = new Vector3(_transform.localPosition.x + AtkSpeed * _fixDeltaTime, _transform.localPosition.y, 0);
                                }
                                break;
                            case Face.Left:
                                if (!touchLefttWall && !MonsterLeftJudgement.GetComponent<MonsterLeftJudgement>().isPlayerAtLeftSide)
                                {
                                    _transform.localPosition = new Vector3(_transform.localPosition.x - AtkSpeed * _fixDeltaTime, _transform.localPosition.y, 0);
                                }
                                break;
                        }
                    }
                    if (AtkTimer <= AtkTimerSet - 0.5)
                    {
                        AtkMoveEnd = true;
                        if (!AtkFirstAppear)
                        {
                            if (isSecondPhase)
                            {
                                switch (face)
                                {
                                    case Face.Right:
                                        Instantiate(REvilDragonFlash, _transform.position + RDragonFlashAppear, Quaternion.identity, AtkTemporaryArea);
                                        break;
                                    case Face.Left:
                                        Instantiate(LEvilDragonFlash, _transform.position + LDragonFlashAppear, Quaternion.identity, AtkTemporaryArea);
                                        break;
                                }
                            }
                            else
                            {
                                switch (face)
                                {
                                    case Face.Right:
                                        Instantiate(RDragonFlash, _transform.position + RDragonFlashAppear, Quaternion.identity, AtkTemporaryArea);
                                        break;
                                    case Face.Left:
                                        Instantiate(LDragonFlash, _transform.position + LDragonFlashAppear, Quaternion.identity, AtkTemporaryArea);
                                        break;
                                }
                            }
                            AtkTemporaryArea.DetachChildren();
                            AtkFirstAppear = true;
                        }
                        if (!AtkThirdAppear)
                        {
                            switch (Random.Range(0, 3))
                            {
                                case 0:
                                    isAtk1End = true;
                                    break;
                                case 1:
                                    isAtk1Continue = true;
                                    break;
                                case 2:
                                    isAtk1Continue = true;
                                    break;
                            }
                            AtkThirdAppear = true;
                        }
                        if (isAtk1End && AtkTimer <= AtkTimerSet - 1.2f)
                        {
                            AtkMoveEnd = false;
                            isAtk = false;
                            isAtk1End = false;
                            AtkThirdAppear = false;
                            isAtk1Continue = false;
                            timerSwitch = false;
                            AtkFirstAppear = false;
                            AtkTimer = AtkTimerSet;
                            status = Status.wait;
                        }
                        if (isAtk1Continue)
                        {
                            if (AtkTimer <= AtkTimerSet - 0.85f)
                            {
                                AtkMoveEnd = false;
                            }
                            if (AtkTimer <= AtkTimerSet - 1.1f)
                            {
                                AtkMoveEnd = true;
                                if (!AtkSecondAppear)
                                {
                                    if (isSecondPhase)
                                    {
                                        switch (face)
                                        {
                                            case Face.Right:
                                                Instantiate(REvilDragonFlash, _transform.position + RDragonFlashAppear, Quaternion.identity, AtkTemporaryArea);
                                                break;
                                            case Face.Left:
                                                Instantiate(LEvilDragonFlash, _transform.position + LDragonFlashAppear, Quaternion.identity, AtkTemporaryArea);
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        switch (face)
                                        {
                                            case Face.Right:
                                                Instantiate(RDragonFlash, _transform.position + RDragonFlashAppear, Quaternion.identity, AtkTemporaryArea);
                                                break;
                                            case Face.Left:
                                                Instantiate(LDragonFlash, _transform.position + LDragonFlashAppear, Quaternion.identity, AtkTemporaryArea);
                                                break;
                                        }
                                    }
                                    AtkTemporaryArea.DetachChildren();
                                    AtkSecondAppear = true;
                                }
                                if (AtkTimer <= 0f)
                                {
                                    AtkMoveEnd = false;
                                    isTurnFaceBeforeAtk = false;
                                    isAtk = false;
                                    AtkThirdAppear = false;
                                    isAtk1End = false;
                                    isAtk1Continue = false;
                                    timerSwitch = false;
                                    AtkFirstAppear = false;
                                    AtkSecondAppear = false;
                                    AtkTimer = AtkTimerSet;
                                    status = Status.wait;
                                }
                            }
                        }
                    }
                }
            }
            if (isAtk2)
            {
                if (_blockController.BeBlockSuccess)
                {
                    switch (face)
                    {
                        case Face.Right:
                            face = Face.Left;
                            break;
                        case Face.Left:
                            face = Face.Right;
                            break;
                    }
                    isAtk2 = false;
                    isTurnFaceBeforeAtk = false;
                    Atk2MoveEnd = false;
                    timerSwitch = false;
                    AtkFirstAppear = false;
                    AtkSecondAppear = false;
                    AtkThirdAppear = false;
                    AtkFourAppear = false;
                    isAtk2Continue = false;
                    isAtk2End = false;
                    Atk2Timer = Atk2TimerSet;
                    Rigid2D.mass = 10000;
                    status = Status.BeBlock;
                    return;
                }
                if (!isTurnFaceBeforeAtk)
                {
                    TurnFaceJudge();
                    isTurnFaceBeforeAtk = true;
                }
                Atk2Timer -= _fixDeltaTime;
                Rigid2D.mass = 20;
                if (Atk2Timer <= Atk2TimerSet - 0.5)
                {
                    if (!Atk2MoveEnd)
                    {
                        Rigid2D.AddForce(new Vector2(0f, ImpulseAtk2PowerY), ForceMode2D.Impulse);
                        ThisBoxCollider.isTrigger = true;
                        Atk2MoveEnd = true;
                    }
                    if (isGround)
                    {
                        ThisBoxCollider.isTrigger = false;
                    }
                    if (Atk2Timer <= Atk2TimerSet - 0.8)
                    {
                        if (!AtkFirstAppear)
                        {
                            switch (face)
                            {
                                case Face.Right:
                                    Instantiate(RAtk2, _transform.position, Quaternion.identity, AtkTemporaryArea.transform);
                                    break;
                                case Face.Left:
                                    Instantiate(LAtk2, _transform.position, Quaternion.identity, AtkTemporaryArea.transform);
                                    break;
                            }
                            AtkTemporaryArea.transform.DetachChildren();
                            AtkFirstAppear = true;
                        }
                        if (Atk2Timer <= Atk2TimerSet - 0.9)
                        {
                            if (!AtkSecondAppear)
                            {
                                switch (face)
                                {
                                    case Face.Right:
                                        Instantiate(ShockWave, _transform.position + RShockWaveAppear, Quaternion.identity);
                                        break;
                                    case Face.Left:
                                        Instantiate(ShockWave, _transform.position + LShockWaveAppear, Quaternion.identity);
                                        break;
                                }
                                AtkSecondAppear = true;
                            }
                            if (Atk2Timer <= Atk2TimerSet - 1.25)
                            {
                                if (!AtkThirdAppear)
                                {
                                    switch (face)
                                    {
                                        case Face.Right:
                                            if (playerTransform.position.x < _transform.position.x && DistanceX <= Atk2ContinueDistance)
                                            {
                                                isAtk2Continue = true;
                                            }
                                            else
                                            {
                                                isAtk2End = true;
                                            }
                                            break;
                                        case Face.Left:
                                            if (playerTransform.position.x > _transform.position.x && DistanceX <= Atk2ContinueDistance)
                                            {
                                                isAtk2Continue = true;
                                            }
                                            else
                                            {
                                                isAtk2End = true;
                                            }
                                            break;
                                    }
                                    AtkThirdAppear = true;
                                }
                                if (isAtk2Continue)
                                {
                                    if (!AtkFourAppear && Atk2Timer <= Atk2TimerSet - 1.6)
                                    {
                                        switch (face)
                                        {
                                            case Face.Right:
                                                Instantiate(RTurnAtk2, _transform.position, Quaternion.identity, AtkTemporaryArea.transform);
                                                break;
                                            case Face.Left:
                                                Instantiate(LTurnAtk2, _transform.position, Quaternion.identity, AtkTemporaryArea.transform);
                                                break;
                                        }
                                        AtkTemporaryArea.transform.DetachChildren();
                                        AtkFourAppear = true;
                                    }
                                    if (Atk2Timer <= 0f)
                                    {
                                        switch (face)
                                        {
                                            case Face.Right:
                                                face = Face.Left;
                                                break;
                                            case Face.Left:
                                                face = Face.Right;
                                                break;
                                        }
                                        Rigid2D.mass = 10000;
                                        isAtk2 = false;
                                        isTurnFaceBeforeAtk = false;
                                        Atk2MoveEnd = false;
                                        timerSwitch = false;
                                        AtkFirstAppear = false;
                                        AtkSecondAppear = false;
                                        AtkThirdAppear = false;
                                        AtkFourAppear = false;
                                        isAtk2Continue = false;
                                        isAtk2End = false;
                                        Atk2Timer = Atk2TimerSet;
                                        status = Status.wait;
                                    }
                                }
                                if (isAtk2End && Atk2Timer <= Atk2TimerSet - 1.8)
                                {
                                    Rigid2D.mass = 10000;
                                    isAtk2 = false;
                                    Atk2MoveEnd = false;
                                    isTurnFaceBeforeAtk = false;
                                    timerSwitch = false;
                                    AtkFirstAppear = false;
                                    AtkSecondAppear = false;
                                    AtkThirdAppear = false;
                                    isAtk2Continue = false;
                                    isAtk2End = false;
                                    Atk2Timer = Atk2TimerSet;
                                    status = Status.wait;
                                }
                            }
                        }
                    }
                }
            }
            if (isAtk3Prepare)
            {
                if (!isTurnFaceBeforeAtk)
                {
                    TurnFaceJudge();
                    isTurnFaceBeforeAtk = true;
                }
                Atk3PrepareTimer -= _fixDeltaTime;
                if (Atk3PrepareTimer <= 0f)
                {
                    if (!AtkFirstAppear)
                    {
                        switch (face)
                        {
                            case Face.Right:
                                Instantiate(RAtk3, _transform.position, Quaternion.identity, AtkTemporaryArea.transform);
                                AtkTemporaryArea.transform.DetachChildren();
                                break;
                            case Face.Left:
                                Instantiate(LAtk3, _transform.position, Quaternion.identity, AtkTemporaryArea.transform);
                                AtkTemporaryArea.transform.DetachChildren();
                                break;
                        }
                        AtkFirstAppear = true;
                    }
                    isAtk3 = true;
                    isAtk3Prepare = false;
                }
            }
            if (isAtk3)
            {
                if (_blockController.BeBlockSuccess)
                {
                    isAtk3 = false;
                    isTurnFaceBeforeAtk = false;
                    Atk3MoveEnd = false;
                    Atk3EndMoveEnd = false;
                    timerSwitch = false;
                    isAtk3Prepare = false;
                    AtkFirstAppear = false;
                    AtkSecondAppear = false;
                    Atk3Timer = Atk3TimerSet;
                    status = Status.BeBlock;
                    return;
                }
                if (!Atk3MoveEnd)
                {
                    switch (face)
                    {
                        case Face.Right:
                            _transform.position += new Vector3(Atk3Speed * _fixDeltaTime, 0f, 0f);
                            break;
                        case Face.Left:
                            _transform.position += new Vector3(-Atk3Speed * _fixDeltaTime, 0f, 0f);
                            break;
                    }
                }
                switch (face)
                {
                    case Face.Right:
                        if (playerTransform.position.x < _transform.position.x || DistanceX <= Atk3EndDistance)
                        {
                            Atk3MoveEnd = true;
                        }
                        break;
                    case Face.Left:
                        if (playerTransform.position.x > _transform.position.x || DistanceX <= Atk3EndDistance)
                        {
                            Atk3MoveEnd = true;
                        }
                        break;
                }
                if (Atk3MoveEnd)
                {
                    Atk3Timer -= _fixDeltaTime;
                    if (Atk3Timer <= Atk3TimerSet - 0.15)
                    {
                        if (!Atk3EndMoveEnd)
                        {
                            switch (face)
                            {
                                case Face.Right:
                                    _transform.localPosition += new Vector3(Atk3EndSpeed * _fixDeltaTime, 0f, 0f);
                                    break;
                                case Face.Left:
                                    _transform.localPosition += new Vector3(-Atk3EndSpeed * _fixDeltaTime, 0f, 0f);
                                    break;
                            }
                        }
                        if (Atk3Timer <= Atk3TimerSet - 0.35)
                        {
                            if (!AtkSecondAppear)
                            {
                                if (isSecondPhase)
                                {
                                    switch (face)
                                    {
                                        case Face.Right:
                                            Instantiate(REvilDragonFlash, _transform.position + RDragonFlashAppear, Quaternion.identity, AtkTemporaryArea);
                                            break;
                                        case Face.Left:
                                            Instantiate(LEvilDragonFlash, _transform.position + LDragonFlashAppear, Quaternion.identity, AtkTemporaryArea);
                                            break;
                                    }
                                }
                                else
                                {
                                    switch (face)
                                    {
                                        case Face.Right:
                                            Instantiate(RDragonFlash, _transform.position + RDragonFlashAppear, Quaternion.identity, AtkTemporaryArea);
                                            break;
                                        case Face.Left:
                                            Instantiate(LDragonFlash, _transform.position + LDragonFlashAppear, Quaternion.identity, AtkTemporaryArea);
                                            break;
                                    }
                                }
                                AtkTemporaryArea.DetachChildren();
                                AtkSecondAppear = true;
                            }
                            if (Atk3Timer <= Atk3TimerSet - 0.4)
                            {
                                Atk3EndMoveEnd = true;
                                if (Atk3Timer <= 0f)
                                {
                                    isTurnFaceBeforeAtk = false;
                                    isAtk3 = false;
                                    Atk3MoveEnd = false;
                                    Atk3EndMoveEnd = false;
                                    timerSwitch = false;
                                    isAtk3Prepare = false;
                                    AtkFirstAppear = false;
                                    AtkSecondAppear = false;
                                    Atk3Timer = Atk3TimerSet;
                                    status = Status.wait;
                                }
                            }
                        }
                    }
                }
            }
            if (isAtk4)
            {
                if (_blockController.BeBlockSuccess)
                {
                    isTurnFaceBeforeAtk = false;
                    isAtk4 = false;
                    Atk4MoveEnd = false;
                    timerSwitch = false;
                    AtkFirstAppear = false;
                    AtkSecondAppear = false;
                    Atk4Timer = Atk4TimerSet;
                    status = Status.BeBlock;
                    return;
                }
                if (!isTurnFaceBeforeAtk)
                {
                    TurnFaceJudge();
                    isTurnFaceBeforeAtk = true;
                }
                Atk4Timer -= _fixDeltaTime;
                if (Atk4Timer <= Atk4TimerSet - 0.4)
                {
                    if (!AtkFirstAppear)
                    {
                        Instantiate(Atk4, _transform.position, Quaternion.identity, AtkTemporaryArea.transform);
                        AtkTemporaryArea.transform.DetachChildren();
                        AtkFirstAppear = true;
                    }
                    if (!Atk4MoveEnd)
                    {
                        TurnFaceJudge();
                        switch (face)
                        {
                            case Face.Right:
                                _transform.position += new Vector3(Atk4Speed * _fixDeltaTime, 0f, 0f);
                                break;
                            case Face.Left:
                                _transform.position += new Vector3(-Atk4Speed * _fixDeltaTime, 0f, 0f);
                                break;
                        }
                    }
                    if (Atk4Timer <= Atk4TimerSet - 3.15)
                    {
                        Atk4MoveEnd = true;
                        if (!AtkSecondAppear)
                        {
                            switch (face)
                            {
                                case Face.Right:
                                    Instantiate(REndAtk4, _transform.position, Quaternion.identity, AtkTemporaryArea.transform);
                                    AtkTemporaryArea.transform.DetachChildren();
                                    break;
                                case Face.Left:
                                    Instantiate(LEndAtk4, _transform.position, Quaternion.identity, AtkTemporaryArea.transform);
                                    AtkTemporaryArea.transform.DetachChildren();
                                    break;
                            }
                            AtkSecondAppear = true;
                        }
                        if (Atk4Timer <= 0f)
                        {
                            isAtk4 = false;
                            isTurnFaceBeforeAtk = false;
                            AtkFirstAppear = false;
                            AtkSecondAppear = false;
                            Atk4MoveEnd = false;
                            timerSwitch = false;
                            Atk4Timer = Atk4TimerSet;
                            status = Status.wait;
                        }
                    }
                }
            }
            if (isAtk5)
            {
                if (!isTurnFaceBeforeAtk)
                {
                    TurnFaceJudge();
                    isTurnFaceBeforeAtk = true;
                }
                Atk5Timer -= _fixDeltaTime;
                if (Atk5Timer <= Atk5TimerSet - 0.15)
                {
                    if (!AtkFirstAppear)
                    {
                        switch (face)
                        {
                            case Face.Right:
                                Instantiate(ExplosionAni, _transform.position + RExplosionAniAppear, Quaternion.identity);
                                break;
                            case Face.Left:
                                Instantiate(ExplosionAni, _transform.position + LExplosionAniAppear, Quaternion.identity);
                                break;
                        }
                        AtkFirstAppear = true;
                    }
                    if (Atk5Timer <= Atk5TimerSet - 0.7)
                    {
                        if (!AtkSecondAppear)
                        {
                            Instantiate(Explosion, new Vector3(LeftEdge + Random.Range(1, 7), YButtomEdge + Random.Range(1, 3), 0f), Quaternion.identity);
                            Instantiate(Explosion, new Vector3(MapMiddle - Random.Range(1, 7), YButtomEdge + Random.Range(1, 3), 0f), Quaternion.identity);
                            Instantiate(Explosion, new Vector3(MapMiddle + Random.Range(1, 7), YButtomEdge + Random.Range(1, 3), 0f), Quaternion.identity);
                            Instantiate(Explosion, new Vector3(RightEdge - Random.Range(1, 7), YButtomEdge + Random.Range(1, 3), 0f), Quaternion.identity);
                            AtkSecondAppear = true;
                        }
                        if (Atk5Timer <= 0f)
                        {
                            isAtk5 = false;
                            isTurnFaceBeforeAtk = false;
                            AtkFirstAppear = false;
                            AtkSecondAppear = false;
                            timerSwitch = false;
                            Atk5Timer = Atk5TimerSet;
                            status = Status.wait;
                        }
                    }
                }
            }
        }
        else
        {
            AtkTimer = AtkTimerSet;
            Atk2Timer = Atk2TimerSet;
            Atk3Timer = Atk3TimerSet;
            Atk3PrepareTimer = Atk3PrepareTimerSet;
            Atk4Timer = Atk4TimerSet;
            Atk5Timer = Atk5TimerSet;
        }
    }

    private void JumpTimerMethod()
    {
        if (JumpTimerSwitch)
        {
            JumpTimer -= _fixDeltaTime;
            if (JumpTimer <= 0f)
            {
                canJumpStop = true;
            }
            if (!ParabolaCaculate)
            {
                ParabolaMiddlePointX = (JumpJudgementOriginalPoint.x + JumpJudgementNextPoint.x) / 2f;
                ParabolaConstant = (JumpJudgementOriginalPoint.x - ParabolaMiddlePointX) * (JumpJudgementOriginalPoint.x - ParabolaMiddlePointX) / -25.848f;
                if (ParabolaConstant > 0f)
                {
                    ParabolaConstant *= -1f;
                }
                ParabolaNowX = JumpJudgementOriginalPoint.x;
                ParabolaCaculate = true;
            }
            _transform.position = new Vector3(ParabolaNowX, (ParabolaNowX - ParabolaMiddlePointX) * (ParabolaNowX - ParabolaMiddlePointX) / (4f * ParabolaConstant) - 6f, 0f);
            if (JumpToRight)
            {
                ParabolaNowX += JumpSpeed * _fixDeltaTime;
            }
            if (JumpToLeft)
            {
                ParabolaNowX -= JumpSpeed * _fixDeltaTime;
            }
        }
        else
        {
            JumpTimer = JumpTimerSet;
        }
    }

    private void JumpEndTimerMethod()
    {
        if (isJumpEnd)
        {
            JumpEndTimer -= _deltaTime;
            if (JumpEndTimer <= 0)
            {
                isJump = false;
                isJumpOpen = false;
                isJumpEnd = false;
                JumpTimer = JumpTimerSet;
                JumpEndTimer = JumpEndTimerSet;
                if (CanGoSecondPhase)
                {
                    SecondPhaseTimerSwitch = true;
                    status = Status.wait;
                    return;
                }
                switch (jumpStatus)
                {
                    case JumpStatus.JumpFar:
                        status = Status.walk;
                        break;
                    case JumpStatus.JumpClose:
                        switch (Random.Range(0, 3))
                        {
                            case 0:
                                status = Status.Atk;
                                break;
                            case 1:
                                status = Status.Atk2;
                                break;
                            case 2:
                                status = Status.Atk4;
                                break;
                        }
                        break;
                }
            }
        }
        else
        {
            JumpEndTimer = JumpEndTimerSet;
        }
    }

    private void SecondPhaseTimerMethod()
    {
        if (SecondPhaseTimerSwitch)
        {
            SecondPhaseTimer -= _deltaTime;
            if (SecondPhaseTimer <= 0f)
            {
                isSecondPhase = true;
                isSecondWeak = true;
                isFirstWeak = true;
                CanGoSecondPhase = false;
                SecondPhaseTimerSwitch = false;
                status = Status.Atk5;
                return;
            }
        }
    }

    private void CriticAtkHurtedTimerMethod()
    {
        if (CriticAtkHurtedSwitch)
        {
            switch (face)
            {
                case Face.Left:
                    MonsterWeakTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                    MonsterStopTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                    break;
                case Face.Right:
                    MonsterWeakTr.localScale = new Vector3(0.28f, 0.28f, 0);
                    MonsterStopTr.localScale = new Vector3(0.28f, 0.28f, 0);
                    break;
            }
            CriticAtkHurtedTimer -= _deltaTime;
            if (CriticAtkHurtedTimer <= (CriticAtkHurtedTimerSet - 0.03))
            {
                if (!HasCriticAtkAppear)
                {
                    Instantiate(CriticAtkHurtedObject, _transform.position, Quaternion.identity);
                    if (status == Status.Weak)
                    {
                        NowAni = AniStatus.Weak;
                        AllAniFalse();
                        MonsterWeakAnimation.SetActive(true);
                    }
                    else
                    {
                        NowAni = AniStatus.Stop;
                        AllAniFalse();
                        MonsterStopAnimation.SetActive(true);
                    }
                    HasCriticAtkAppear = true;
                }
            }
            if (CriticAtkHurtedTimer <= (CriticAtkHurtedTimerSet - 0.95))
            {
                if (!HasImpulse)
                {
                    switch (face)
                    {
                        case Face.Right:
                            Rigid2D.AddForce(new Vector2(-CriticAtkImpulsePower, 0), ForceMode2D.Impulse);
                            break;
                        case Face.Left:
                            Rigid2D.AddForce(new Vector2(CriticAtkImpulsePower, 0), ForceMode2D.Impulse);
                            break;
                    }
                    if (status == Status.Weak)
                    {
                        hp -= BattleSystem.CriticAtkHurtPower;
                    }
                    else
                    {
                        hp -= (BattleSystem.CriticAtkHurtPower / 3);
                    }
                    HasImpulse = true;
                }
                if (CriticAtkHurtedTimer <= 0)
                {
                    HasImpulse = false;
                    HasCriticAtkAppear = false;
                    status = Status.walk;
                    isCriticAtkHurted = false;
                    CriticAtkHurtedSwitch = false;
                }
            }
        }
        else
        {
            CriticAtkHurtedTimer = CriticAtkHurtedTimerSet;
        }
    }

    private void BeBlockEndJudge()
    {
        if (_blockController.BeBlockUnHurtedEnd)
        {
            _blockController.BeBlockUnHurtedEnd = false;
            status = Status.Atk;
        }
        if (_blockController.BeBlockNormalAtkEnd)
        {
            status = Status.Jump;
            jumpStatus = JumpStatus.JumpFar;
            _blockController.BeBlockNormalAtkEnd = false;
        }
        if (_blockController.BeBlockCAtkEnd)
        {
            isWeakByBlock = true;
            status = Status.Weak;
            _blockController.BeBlockCAtkEnd = false;
        }
    }

    private void BlockEndJudge()
    {
        if (_blockController.BlockUnSuccessEnd)
        {
            BlockLastTime = _time;
            CanBlock = false;
            status = Status.walk;
            _blockController.BlockUnSuccessEnd = false;
        }
        if (_blockController.BlockSuccessEnd)
        {
            status = Status.walk;
            _blockController.BlockSuccessEnd = false;
        }
        if (_blockController.BlockHurtedEnd)
        {
            BlockLastTime = _time;
            CanBlock = false;
            status = Status.Jump;
            jumpStatus = JumpStatus.JumpFar;
            _blockController.BlockHurtedEnd = false;
        }
    }

    private void TurnFaceJudge()
    {
        if (playerTransform.position.x <= _transform.position.x)
        {
            face = Face.Left;
        }
        else
        {
            face = Face.Right;
        }
    }

    private void AllAniFalse()
    {
        if (NowAni != AniStatus.Wait)
        {
            MonsterWaitAnimation.SetActive(false);
        }
        if (NowAni != AniStatus.Walk)
        {
            MonsterWalkAnimation.SetActive(false);
        }
        if (NowAni != AniStatus.Jump)
        {
            MonsterJumpAnimation.SetActive(false);
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
        if (NowAni != AniStatus.Atk4)
        {
            MonsterAtk4Animation.SetActive(false);
        }
        if (NowAni != AniStatus.Atk5)
        {
            MonsterAtk5Animation.SetActive(false);
        }
        if (NowAni != AniStatus.Weak)
        {
            MonsterWeakAnimation.SetActive(false);
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
        if (NowAni != AniStatus.Burning)
        {
            MonsterBurningAnimation.SetActive(false);
        }
        if (NowAni != AniStatus.TurnPhase)
        {
            MonsterSecondPhaseAnimation.SetActive(false);
        }
    }

    private void StatusJudge()
    {
        while (!StatusChooseEnd)
        {
            if (CanGoSecondPhase)
            {
                jumpStatus = JumpStatus.JumpFar;
                status = Status.Jump;
                return;
            }
            if (DistanceX <= CloseAtkDistance)
            {
                switch (Random.Range(0, 5))
                {
                    case 0:
                        if (LastStatus != Status.Jump)
                        {
                            status = Status.Jump;
                            jumpStatus = JumpStatus.JumpFar;
                            StatusChooseEnd = true;
                        }
                        break;
                    case 1:
                        if (LastStatus != Status.Atk)
                        {
                            status = Status.Atk;
                            StatusChooseEnd = true;
                        }
                        break;
                    case 2:
                        if (LastStatus != Status.Atk2)
                        {
                            status = Status.Atk2;
                            StatusChooseEnd = true;
                        }
                        break;
                    case 3:
                        if (LastStatus != Status.Atk4)
                        {
                            status = Status.Atk4;
                            StatusChooseEnd = true;
                        }
                        break;
                    case 4:
                        if (CanBlock && LastStatus != Status.Block)
                        {
                            status = Status.Block;
                            StatusChooseEnd = true;
                        }
                        break;
                }
            }
            else if (isSecondPhase)
            {
                switch (Random.Range(0, 5))
                {
                    case 0:
                        if (LastStatus != Status.walk)
                        {
                            status = Status.walk;
                            StatusChooseEnd = true;
                        }
                        break;
                    case 1:
                        if (LastStatus != Status.Atk)
                        {
                            status = Status.Atk;
                            StatusChooseEnd = true;
                        }
                        break;
                    case 2:
                        if (LastStatus != Status.Atk3)
                        {
                            status = Status.Atk3;
                            StatusChooseEnd = true;
                        }
                        break;
                    case 3:
                        if (LastStatus != Status.Atk5)
                        {
                            status = Status.Atk5;
                            StatusChooseEnd = true;
                        }
                        break;
                    case 4:
                        if (DistanceX <= JumpCloseDistance && LastStatus != Status.Jump)
                        {
                            status = Status.Jump;
                            jumpStatus = JumpStatus.JumpClose;
                            StatusChooseEnd = true;
                        }
                        break;
                }
            }
            else
            {
                switch (Random.Range(0, 4))
                {
                    case 0:
                        if (LastStatus != Status.walk)
                        {
                            status = Status.walk;
                            StatusChooseEnd = true;
                        }
                        break;
                    case 1:
                        if (LastStatus != Status.Atk)
                        {
                            status = Status.Atk;
                            StatusChooseEnd = true;
                        }
                        break;
                    case 2:
                        if (LastStatus != Status.Atk3)
                        {
                            status = Status.Atk3;
                            StatusChooseEnd = true;
                        }
                        break;
                    case 3:
                        if (DistanceX <= JumpCloseDistance && LastStatus != Status.Jump)
                        {
                            status = Status.Jump;
                            jumpStatus = JumpStatus.JumpClose;
                            StatusChooseEnd = true;
                        }
                        break;
                }
            }
        }
    }

    private void BurningTimerMethod()
    {
        if (isBurning)
        {
            if (status == Status.Burning)
            {
                BurningTimer -= _fixDeltaTime;
            }

            if (!isFireAppear)
            {
                Instantiate(Fire, transform.position, Quaternion.identity, AtkTemporaryArea.transform);
                _fire = AtkTemporaryArea.transform.GetChild(0).gameObject;
                AtkTemporaryArea.transform.DetachChildren();
                isFireAppear = true;
            }
            if (hp > 0 && !isWaveAppear)
            {
                hp -= BattleSystem.CocktailHurtPower * 3;
            }
            if (BurningTimer <= (BurningTimerSet - 1))
            {
                if (!isWaveAppear)
                {
                    Instantiate(WaveAni, _transform.position, Quaternion.identity);
                    Destroy(_fire);
                    isWaveAppear = true;
                }
            }
            if (BurningTimer <= 0)
            {
                isBurning = false;
                isFireAppear = false;
                isWaveAppear = false;
                BurningTimer = BurningTimerSet;
                status = Status.wait;
            }
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
