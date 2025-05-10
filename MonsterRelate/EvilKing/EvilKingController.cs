using UnityEngine;
using UnityEngine.UI;

public class EvilKingController : MonoBehaviour
{
    [Header("基本參數")]
    [HideInInspector] public float hp;//有被其他script用到(EvilKingHpUi，Boss3RoomController，Boss3Wall)
    public int maxHp;
    public float CloseAtkDistance;
    public float FloatPower;
    public float GetUpSpeed;
    private float LeftEdge = 10.5f;
    private float YButtomEdge = -10.5f;
    public float CreatePlatformCoolDown;
    public float SummonGhostCoolDown;
    public float BigBallCoolDown;
    public GameObject WalkPlace1;
    public GameObject WalkPlace2;
    public GameObject WalkPlace3;
    public GameObject WalkPlace4;
    private enum Status { wait, walk, Atk, Atk2, Atk3, Atk4, Atk5, Atk6, SummonGhost, CreateShield, CreatePlatform, MoveAtk, Weak };
    private Status status;
    private Status LastStatus;
    private Status NextStatus;
    private enum Face { Right, Left };
    private Face face;
    private enum FloatStatus { Wait, Up, Down };
    private FloatStatus floatStatus;

    public enum AniStatus { Wait, Move, Atk, AtkWait, AtkEnd, Weak, Stop, FinalAtk }
    [HideInInspector] public AniStatus NowAni;//script(hurted)

    [Header("系統參數")]
    private Transform _transform;
    private float _deltaTime;
    private float _time;
    private GameObject AtkTemporaryArea;
    public GameObject Platform;
    public GameObject Platform2;
    public GameObject Platform3;
    public GameObject Platform4;
    public GameObject Platform5;
    private float FloatChangeSpeed = 0.05f;
    private float ParabolaX = -10;
    private float ParabolaY;
    private float ParabolaConstant = 100;
    private float FloatYCenter;
    private int WalkPlaceNumber;
    private float SummonGhostLastTime;
    private float BigBallLastTime;
    public float CreatingPlatformTimerSet;
    private float CreatingPlatformTimer;
    public float AtkTimerSet;
    private float AtkTimer;
    public float Atk2TimerSet;
    private float Atk2Timer;
    public float Atk3TimerSet;
    private float Atk3Timer;
    public float Atk4TimerSet;
    private float Atk4Timer;
    public float Atk5TimerSet;
    private float Atk5Timer;
    public float Atk6TimerSet;
    private float Atk6EndTimer;
    public float Atk6EndTimerSet;
    private float Atk6Timer;
    public float SummonGhostTimerSet;
    private float SummonGhostTimer;
    public float CreatingShieldTimerSet;
    private float CreatingShieldTimer;
    public float ReSetShieldTimerSet;
    private float ReSetShieldTimer;
    public float WalkTimerSet;
    private float WalkTimer;
    private float WeakTimer;
    public float WeakTimerSet;
    private float FinalAtkTimer;
    public float FinalAtkTimerSet;
    private float WaitTimerSet = 0.1f;
    private float WaitTimer;
    private Transform playerTransform;
    private Rigidbody2D Rigid2D;
    private BoxCollider2D ThisBoxCollider;
    private MonsterHurtedController _hurtedController;
    private RaycastHit2D GroundCheck;
    private float DistanceX;//怪物與玩家的距離
    private float DistanceY;//怪物與玩家的距離
    private float BurningTimer;
    private float BurningTimerSet = 5;

    //開關
    private bool isWait;
    [HideInInspector] public bool isWalking;//Script(EvilKingShield)
    [HideInInspector] public bool isWalkMove;//Script(EvilKingShield)
    private bool isPlatformAppear = false;
    private bool isAtk;
    private bool isAtk2;
    private bool isAtk3;
    private bool isAtk4;
    private bool isAtk4Continue;
    private bool isAtk4End;
    private bool isAtk5;
    private bool isAtk6;
    private bool isCreatingPlatform;
    private bool isSummonGhost;
    private bool isCreatingShield;
    private bool CanUseAtk1;
    private bool ShouldCreateShield = true;
    public static bool isWeak;//script(3王的攻擊script，monsterweakJudgement)
    private bool isGetUp;
    private bool isShieldExist;
    [HideInInspector] public bool isShieldDestroy;//script(EvilKingShield)
    private bool isFirstWeak;
    private bool isSecondWeak;
    public static bool isSecondPhase = false;//script(攻擊script)
    private bool CanGoSecondPhase;
    private bool timerSwitch;
    private bool WalkTimerSwitch;
    private bool WeakTimerSwitch;
    private bool CreatingPlatformTimerSwitch;
    private bool ReSetShieldTimerSwitch;
    private bool CanFloat = true;
    private bool AtkFirstAppear;
    private bool AtkSecondAppear;
    private bool AtkThirdAppear;
    private bool AtkFourthAppear;
    private bool AtkFifthAppear;
    private bool AtkSixAppear;
    private bool AtkSevenAppear;
    private bool AtkEightAppear;
    private bool AtkNineAppear;
    private bool AtkTenAppear;
    private bool AtkElevenAppear;
    private bool isGround;
    private bool isAtkEnd;
    private bool Atk1Set1;
    private bool Atk1Set2;
    [HideInInspector] public bool isFighting;//script(Boss3Controller)
    private bool ShouldUseMoveAtk;
    private bool isFinalAtk;
    private bool isFinalWeakAniAppear;
    private bool isFinalActionAppear;
    private bool CanSummonGhost;
    private bool CanUseBigBall;
    private bool StatusChooseEnd;
    private bool isBurning;
    private bool isFireAppear;
    private bool isMoveAtk;

    [Header("動畫相關物件")]
    public Image HpImage;
    private Animator MonsterWeakAni;
    private Animator MonsterMoveAni;
    private Animator MonsterFinalAtkAni;
    private GameObject MonsterMoveAnimation;
    private GameObject MonsterAtkAnimation;
    private GameObject MonsterAtkWaitAnimation;
    private GameObject MonsterAtkEndAnimation;
    private GameObject MonsterWeakAnimation;
    private GameObject MonsterFinalAtkAnimation;
    public GameObject MonsterDieAnimation;
    private Transform MonsterMoveTr;
    private Transform MonsterAtkTr;
    private Transform MonsterAtkWaitTr;
    private Transform MonsterAtkEndTr;
    private Transform MonsterWeakTr;
    private Transform MonsterFinalAtkTr;
    public GameObject Fire;
    private GameObject _fire;
    private Animator _fire1;
    private Animator _fire2;
    private Animator _fire3;

    [Header("攻擊物件")]
    public GameObject Shield;
    public GameObject ShieldImpulse;
    public GameObject Atk1;//大爆炸
    public GameObject Atk2;//橫光柱
    public GameObject ForeverAtk;
    public GameObject Atk3;//豎光柱
    public GameObject RAtk14;//暗光劍
    public GameObject LAtk14;//暗光劍
    public GameObject RAtk24;//暗光劍
    public GameObject LAtk24;//暗光劍
    public GameObject Atk5;//流星雨
    public GameObject Atk6;//黑暗球
    public GameObject Ghost;
    public GameObject FinalAtk3Up;
    public GameObject FinalAtk3Under;
    public GameObject FinalAtk4;
    public GameObject FinalAtk5;
    public GameObject FinalBigDarkBall;
    public GameObject TrueFinalAni;
    private Vector3 ShieldAppear = new Vector3(0, 0.18f, 0);
    private Vector3 Atk1Appear = new Vector3(22.27f, -5.22f, 0);
    private Vector3 ForeverAtkAppear = new Vector3(35.74f, -9.7f, 0);
    private Vector3 Atk2Appear2 = new Vector3(32.16f, -7.4f, 0);
    private Vector3 Atk2Appear3 = new Vector3(32.16f, -5.4f, 0);
    private Vector3 Atk2Appear4 = new Vector3(32.16f, -3.4f, 0);
    private Vector3 Atk3Appear1 = new Vector3(11f, -11.5f, 0);
    private Vector3 RAtk4Appear1 = new Vector3(0.4f, 1.1f, 0);
    private Vector3 LAtk4Appear1 = new Vector3(-0.4f, 1.1f, 0);
    private Vector3 RAtk4Appear2 = new Vector3(0.45f, -0.9f, 0);
    private Vector3 LAtk4Appear2 = new Vector3(-0.45f, -0.9f, 0);
    private Vector3 Atk5Appear = new Vector3(24.41f, 2.2f, 0);
    private Vector3 Atk6Appear = new Vector3(23f, -0.7f, 0);
    private Vector3 Final1Appear1 = new Vector3(17.8f, -9.7f, 0);
    private Vector3 Final1Appear2 = new Vector3(27.8f, -9.7f, 0);
    private Vector3 Final2Appear = new Vector3(32.72f, -5.44f, 0);
    private Vector3 Final3Appear1 = new Vector3(30f, -2.76f, 0);
    private Vector3 Final3Appear2 = new Vector3(30f, -9.2f, 0);
    private Vector3 Final4Appear = new Vector3(15.455f, -10, 0);
    private Vector3 Final5Appear = new Vector3(32, -8.92f, 0);
    private Vector3 Final6Appear1 = new Vector3(17.8f, 1, 0);
    private Vector3 Final6Appear2 = new Vector3(27.79f, 1, 0);
    private Vector3 Final7Appear = new Vector3(22.82f, 2f, 0);
    private Vector3 Final8AppearL = new Vector3(-3.77f, 0.28f, 0);
    private Vector3 Final8AppearR = new Vector3(4.43f, 0.34f, 0);
    private Vector3 TrueFinalAppear = new Vector3(22.87f, 0.82f, 0);

    [Header("被大招攻擊")]
    public GameObject CriticAtkHurtedObject;
    public static bool isCriticAtkHurted = false;//script(3王的攻擊script)
    private bool CriticAtkHurtedSwitch;
    private bool HasCriticAtkAppear;
    private bool HasHurtedByCtiticAtk;
    private float CriticAtkHurtedTimerSet = 1.65f;
    private float CriticAtkHurtedTimer;
    private GameObject MonsterStopAnimation;
    private Transform MonsterStopTr;
    public static bool isHurtedByBigGun;
    void Start()
    {
        _transform = transform;

        isSecondPhase = false;
        ShouldCreateShield = true;
        //抓取動畫物件
        MonsterMoveAnimation = this.transform.GetChild(0).gameObject;
        MonsterAtkAnimation = this.transform.GetChild(1).gameObject;
        MonsterAtkWaitAnimation = this.transform.GetChild(2).gameObject;
        MonsterAtkEndAnimation = this.transform.GetChild(3).gameObject;
        MonsterWeakAnimation = this.transform.GetChild(4).gameObject;
        MonsterStopAnimation = this.transform.GetChild(6).gameObject;
        MonsterFinalAtkAnimation = this.transform.GetChild(7).gameObject;

        MonsterMoveTr = MonsterMoveAnimation.transform.GetChild(0);
        MonsterAtkTr = MonsterAtkAnimation.transform.GetChild(0);
        MonsterAtkWaitTr = MonsterAtkWaitAnimation.transform.GetChild(0);
        MonsterAtkEndTr = MonsterAtkEndAnimation.transform.GetChild(0);
        MonsterWeakTr = MonsterWeakAnimation.transform.GetChild(0);
        MonsterStopTr = MonsterStopAnimation.transform.GetChild(0);
        MonsterFinalAtkTr = MonsterFinalAtkAnimation.transform.GetChild(0);

        MonsterMoveAni = MonsterMoveTr.GetComponent<Animator>();
        MonsterWeakAni = MonsterWeakTr.GetComponent<Animator>();
        MonsterFinalAtkAni = MonsterFinalAtkTr.GetComponent<Animator>();

        AtkTemporaryArea = this.transform.GetChild(5).gameObject;
        AtkTemporaryArea.transform.DetachChildren();

        hp = maxHp;
        NowAni = AniStatus.Wait;
        status = Status.wait;
        LastStatus = Status.wait;
        if (MonsterMoveTr.localScale.x > 0)
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
        }
        Rigid2D = this.transform.GetComponent<Rigidbody2D>();
        ThisBoxCollider = this.transform.GetComponent<BoxCollider2D>();
        _hurtedController = this.transform.GetComponent<MonsterHurtedController>();

        floatStatus = FloatStatus.Up;
        FloatYCenter = this.transform.position.y;
        FinalAtkTimer = FinalAtkTimerSet;
        WaitTimer = WaitTimerSet;

        StatusJudge();
        StatusChooseEnd = false;
    }

    // Update is called once per frame
    void Update()
    {
        _time = Time.time;
        _deltaTime = Time.deltaTime;

        if (hp <= 0)
        {
            CanFloat = false;
            ReSetShieldTimerSwitch = false;
            ThisBoxCollider.isTrigger = true;
            isAtkEnd = false;
            isAtk = false;
            isAtk2 = false;
            isAtk3 = false;
            isAtk4 = false;
            isAtk4Continue = false;
            isAtk4End = false;
            isAtk5 = false;
            isAtk6 = false;
            BigDarkBall.isAtkEnd = false;
            isSummonGhost = false;
            isCreatingShield = false;
            isCreatingPlatform = false;
            isWalking = false;
            isWalkMove = false;
            timerSwitch = false;
            WalkTimerSwitch = false;
            CreatingPlatformTimerSwitch = false;
            AtkFirstAppear = false;
            AtkSecondAppear = false;
            AtkThirdAppear = false;
            AtkFourthAppear = false;
            AtkFifthAppear = false;
            AtkTimer = AtkTimerSet;
            Atk2Timer = Atk2TimerSet;
            Atk3Timer = Atk3TimerSet;
            Atk4Timer = Atk4TimerSet;
            Atk5Timer = Atk5TimerSet;
            Atk6Timer = Atk6TimerSet;
            Atk6EndTimer = Atk6TimerSet;
            SummonGhostTimer = SummonGhostTimerSet;
            CreatingShieldTimer = CreatingShieldTimerSet;
            CreatingPlatformTimer = CreatingPlatformTimerSet;
            WalkTimer = WalkTimerSet;
            isFinalAtk = true;
            hp = 1;
            HpImage.transform.localScale = new Vector3((float)hp / (float)maxHp, HpImage.transform.localScale.y, HpImage.transform.localScale.z);
            return;
        }
        if (hp <= (maxHp * 0.4))
        {
            if (!isSecondPhase)
            {
                ShouldUseMoveAtk = true;
                CanGoSecondPhase = true;
            }
        }
        if (hp <= (maxHp * 0.55))
        {
            if (!isSecondWeak)
            {
                status = Status.Weak;
                isFirstWeak = true;
                isSecondWeak = true;
            }
        }
        if (hp <= (maxHp * 0.6))
        {
            if (!Atk1Set2)
            {
                CanUseAtk1 = true;
                Atk1Set2 = true;
            }
        }
        if (hp <= (maxHp * 0.75))
        {
            if (!isFirstWeak)
            {
                status = Status.Weak;
                isFirstWeak = true;
            }
        }
        if (hp <= (maxHp * 0.8))
        {
            if (!Atk1Set1)
            {
                CanUseAtk1 = true;
                Atk1Set1 = true;
            }
        }
        //倒下時變數重置
        if (isWeak || isHurtedByBigGun || isCriticAtkHurted)
        {
            if (!isWeak && status != Status.Weak)
            {
                status = Status.wait;
                NextStatus = Status.walk;
            }
            else
            {
                status = Status.Weak;
                NextStatus = Status.walk;
            }

            if (!isHurtedByBigGun && isSecondPhase)
            {
                ShouldUseMoveAtk = true;
            }
            CanFloat = false;
            ThisBoxCollider.isTrigger = false;
            timerSwitch = false;
            WalkTimerSwitch = false;
            StatusChooseEnd = false;
            isAtkEnd = false;
            isAtk = false;
            isAtk2 = false;
            isAtk3 = false;
            isAtk4 = false;
            isAtk4End = false;
            isAtk4Continue = false;
            isAtk5 = false;
            BigDarkBall.isAtkEnd = false;
            isAtk6 = false;
            isSummonGhost = false;
            isCreatingShield = false;
            isWalking = false;
            isWalkMove = false;
            isMoveAtk = false;
            AtkFirstAppear = false;
            AtkSecondAppear = false;
            AtkThirdAppear = false;
            AtkFourthAppear = false;
            AtkFifthAppear = false;
            AtkTimer = AtkTimerSet;
            Atk2Timer = Atk2TimerSet;
            Atk3Timer = Atk3TimerSet;
            Atk4Timer = Atk4TimerSet;
            Atk5Timer = Atk5TimerSet;
            Atk6Timer = Atk6TimerSet;
            Atk6EndTimer = Atk6TimerSet;
            SummonGhostTimer = SummonGhostTimerSet;
            CreatingShieldTimer = CreatingShieldTimerSet;
            WalkTimer = WalkTimerSet;
            BigBallLastTime = _time;
            SummonGhostLastTime = _time;
            //weak
            if (isCriticAtkHurted && isWeak)
            {
                WeakTimerSwitch = false;
                isGetUp = false;
                isWeak = false;
                WeakTimer = WeakTimerSet;
            }
        }

        if (isShieldDestroy)
        {
            isShieldExist = false;
            isShieldDestroy = false;
            ReSetShieldTimerSwitch = true;
        }
        CriticAtkHurtedTimerMethod();
        WalkTimerMethod();
        _hurtedController.HurtedTimerMethod(_deltaTime);
        WeakTimerMethod();
        timer();
        FinalAtkAni();
        HurtedByBigGunAni();
        CreatePlatformTimerMethod();
        ReSetShieldTimerMethod();
        FinalAtk();
        //判斷是否在地上
        GroundCheck = Physics2D.Raycast(_transform.position, -Vector2.up, 1f, 1024);
        if (GroundCheck)
        {
            isGround = true;
        }
        else
        {
            isGround = false;
        }

        HpImage.transform.localScale = new Vector3((float)hp / (float)maxHp, HpImage.transform.localScale.y, HpImage.transform.localScale.z);

        Floating();

        //return
        if (GameEvent.isAniPlay || isCriticAtkHurted || !isFighting || isHurtedByBigGun || isFinalAtk)
        {
            return;
        }

        TurnFaceJudge();

        //計算距離
        if (playerTransform)
        {
            DistanceX = Mathf.Abs(_transform.position.x - playerTransform.position.x);
        }
        //冷卻時間計算
        if (_time >= (SummonGhostCoolDown + SummonGhostLastTime))
        {
            CanSummonGhost = true;
        }
        if (_time >= (BigBallCoolDown + BigBallLastTime) && status != Status.Atk6)
        {
            CanUseBigBall = true;
        }

        //怪物AI
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
                    status = NextStatus;
                    isWait = false;
                    WaitTimer = WaitTimerSet;
                }
                break;
            case Status.walk:
                if (!isWalking)
                {
                    isWalking = true;
                    WalkTimerSwitch = true;
                }
                break;
            case Status.Atk:
                if (!isAtk)
                {
                    isAtk = true;
                    timerSwitch = true;
                }
                break;
            case Status.Atk2:
                if (!isAtk2)
                {
                    isAtk2 = true;
                    timerSwitch = true;
                }
                break;
            case Status.Atk3:
                if (!isAtk3)
                {
                    isAtk3 = true;
                    timerSwitch = true;
                }
                break;
            case Status.Atk4:
                if (!isAtk4)
                {
                    isAtk4 = true;
                    timerSwitch = true;
                }
                break;
            case Status.Atk5:
                if (!isAtk5)
                {
                    isAtk5 = true;
                    timerSwitch = true;
                }
                break;
            case Status.Atk6:
                if (!isAtk6)
                {
                    isAtk6 = true;
                    timerSwitch = true;
                }
                break;
            case Status.SummonGhost:
                if (!isSummonGhost)
                {
                    isSummonGhost = true;
                    timerSwitch = true;
                }
                break;
            case Status.CreateShield:
                if (!isCreatingShield)
                {
                    isCreatingShield = true;
                    timerSwitch = true;
                }
                break;
            case Status.MoveAtk:
                if (!isMoveAtk)
                {
                    isMoveAtk = true;
                    timerSwitch = true;
                }
                break;
            case Status.CreatePlatform:
                if (!isCreatingPlatform)
                {
                    isCreatingPlatform = true;
                    CreatingPlatformTimerSwitch = true;
                }
                break;
            case Status.Weak:
                if (!isWeak)
                {
                    WeakTimerSwitch = true;
                    isWeak = true;
                }
                break;
        }
        //轉向控制
        switch (face)
        {
            case Face.Left:
                MonsterMoveTr.localScale = new Vector3(0.28f, 0.28f, 0);
                MonsterAtkTr.localScale = new Vector3(0.28f, 0.28f, 0);
                MonsterAtkWaitTr.localScale = new Vector3(0.28f, 0.28f, 0);
                MonsterAtkEndTr.localScale = new Vector3(0.28f, 0.28f, 0);
                MonsterWeakTr.localScale = new Vector3(0.28f, 0.28f, 0);
                MonsterStopTr.localScale = new Vector3(0.28f, 0.28f, 0);
                MonsterFinalAtkTr.localScale = new Vector3(0.28f, 0.28f, 0);
                break;
            case Face.Right:
                MonsterMoveTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                MonsterAtkTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                MonsterAtkWaitTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                MonsterAtkEndTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                MonsterWeakTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                MonsterStopTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                MonsterFinalAtkTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                break;
        }
        SwitchAnimation();
    }
    private void FixedUpdate()
    {
        if (isHurtedByBigGun && !isShieldExist)
        {
            if (isWeak)
            {
                hp -= BattleSystem.BigGunPower;
            }
            else
            {
                hp -= (BattleSystem.BigGunPower / 3);
            }
        }

        BurningTimerMethod();
    }

    //受傷害
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "bullet" && !isShieldExist && !isFinalAtk)
        {
            hp -= BattleSystem.BulletHurtPower;
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "normalAtk" && !isShieldExist && !isFinalAtk)
        {
            _hurtedController.isHurted = true;
            BattleSystem.IncreaseTimes += BattleSystem.IncresePlayerPowerNumber;
            hp -= BattleSystem.NormalAtkHurtPower;
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "CAtk" && !isShieldExist && !isFinalAtk)
        {
            _hurtedController.isHurted = true;
            hp -= BattleSystem.CAtkHurtPower;
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "ExplosionBottle" && !isShieldExist && !isFinalAtk && !_hurtedController.isHurted)
        {
            _hurtedController.isHurted = true;
            hp -= BattleSystem.ExplosionHurtPower;
        }
        if (other.gameObject.tag == "Cocktail" && !isShieldExist && !isFinalAtk && !_hurtedController.isHurted)
        {
            if (!isBurning)
            {
                isBurning = true;
                BurningTimer = BurningTimerSet;
            }
        }
        if (other.gameObject.tag == "CriticAtk" && !isCreatingPlatform && !isFinalAtk)
        {
            isCriticAtkHurted = true;
            CriticAtkHurtedSwitch = true;
        }
        if (other.gameObject.tag == "BigGun" && !isCreatingPlatform && !isFinalAtk)
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

    void SwitchAnimation()
    {
        if (isWait)
        {
            if (LastStatus == Status.walk || LastStatus == Status.wait)
            {
                NowAni = AniStatus.Wait;
            }
            else
            {
                if (NextStatus != Status.walk)
                {
                    NowAni = AniStatus.AtkWait;
                }
            }
        }

        if (isWalking)
        {
            NowAni = AniStatus.Move;
        }

        if (timerSwitch || isCreatingPlatform)
        {
            if (!StatusChooseEnd)
            {
                if (LastStatus == Status.walk || LastStatus == Status.wait)
                {
                    NowAni = AniStatus.Atk;
                }

                if (LastStatus != Status.walk && LastStatus != Status.wait)
                {
                    NowAni = AniStatus.AtkWait;
                }
            }

            if (isAtkEnd)
            {
                NowAni = AniStatus.AtkEnd;
            }
        }

        if (isWeak)
        {
            NowAni = AniStatus.Weak;
        }

        switch (NowAni)
        {
            case AniStatus.Wait:
                AllAniFalse();
                MonsterMoveAnimation.SetActive(true);
                MonsterMoveAni.SetBool("MoveBegin", false);
                MonsterMoveAni.SetBool("MoveEnd", false);
                break;
            case AniStatus.Move:
                AllAniFalse();
                MonsterMoveAnimation.SetActive(true);
                MonsterMoveAni.SetBool("MoveBegin", true);
                if (isWalkMove)
                {
                    MonsterMoveAni.SetBool("MoveEnd", true);
                }
                break;
            case AniStatus.Atk:
                AllAniFalse();
                MonsterAtkAnimation.SetActive(true);
                break;
            case AniStatus.AtkWait:
                AllAniFalse();
                MonsterAtkWaitAnimation.SetActive(true);
                break;
            case AniStatus.AtkEnd:
                AllAniFalse();
                MonsterAtkEndAnimation.SetActive(true);
                break;
            case AniStatus.Weak:
                AllAniFalse();
                MonsterWeakAnimation.SetActive(true);
                if (isGround)
                {
                    MonsterWeakAni.SetBool("isGround", true);
                }
                if (isGetUp)
                {
                    MonsterWeakAni.SetBool("Restore", true);
                }
                break;
            case AniStatus.Stop:
                AllAniFalse();
                MonsterStopAnimation.SetActive(true);
                break;
            case AniStatus.FinalAtk:
                AllAniFalse();
                MonsterFinalAtkAnimation.SetActive(true);
                break;
        }
    }

    void HurtedByBigGunAni()
    {
        if (isHurtedByBigGun && !isWeak)
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

    void FinalAtkAni()
    {
        if (isFinalWeakAniAppear)
        {
            NowAni = AniStatus.FinalAtk;
            AllAniFalse();
            MonsterFinalAtkAnimation.SetActive(true);
            if (isFinalActionAppear)
            {
                MonsterFinalAtkAni.SetBool("Move", true);
            }
        }
    }

    void Floating()
    {
        if (CanFloat)
        {
            switch (floatStatus)
            {
                case FloatStatus.Up:
                    ParabolaConstant = Mathf.Abs(ParabolaConstant) * -1;
                    ParabolaY = (ParabolaX * ParabolaX / (ParabolaConstant * 4)) + 0.25f;
                    ParabolaX += FloatChangeSpeed;
                    _transform.position = new Vector3(_transform.position.x, FloatYCenter + ParabolaY, 0);
                    if (ParabolaX >= 10)
                    {
                        ParabolaX = -10;
                        floatStatus = FloatStatus.Down;
                    }
                    break;
                case FloatStatus.Down:
                    ParabolaConstant = Mathf.Abs(ParabolaConstant);
                    ParabolaY = (ParabolaX * ParabolaX / (ParabolaConstant * 4)) - 0.25f;
                    ParabolaX += FloatChangeSpeed;
                    _transform.position = new Vector3(_transform.position.x, FloatYCenter + ParabolaY, 0);
                    if (ParabolaX >= 10)
                    {
                        ParabolaX = -10;
                        floatStatus = FloatStatus.Up;
                    }
                    break;
            }
        }
    }

    void timer()
    {
        if (timerSwitch)
        {
            if (isAtk)
            {
                AtkTimer -= _deltaTime;
                if (AtkTimer <= (AtkTimerSet - 1))
                {
                    if (!AtkFirstAppear)
                    {
                        Instantiate(Atk1, Atk1Appear, Quaternion.identity);
                        AtkFirstAppear = true;
                        CanUseAtk1 = false;
                    }
                    if (AtkTimer <= 0.2)
                    {
                        StatusJudge();
                        if (NextStatus == Status.walk)
                        {
                            isAtkEnd = true;
                        }
                    }
                    if (AtkTimer <= 0)
                    {
                        StatusChooseEnd = false;
                        isAtkEnd = false;
                        isAtk = false;
                        timerSwitch = false;
                        AtkFirstAppear = false;
                        AtkTimer = AtkTimerSet;
                        status = Status.wait;
                    }
                }
            }//大爆炸
            if (isAtk2)
            {
                Atk2Timer -= _deltaTime;
                if (Atk2Timer <= (Atk2TimerSet - 0.5))
                {
                    if (!AtkFirstAppear)
                    {
                        switch (Random.Range(0, 3))
                        {
                            case 0:
                                Instantiate(Atk2, Atk2Appear3, Quaternion.identity);
                                break;
                            case 1:
                                Instantiate(Atk2, Atk2Appear2, Quaternion.identity);
                                break;
                            case 2:
                                Instantiate(Atk2, Atk2Appear4, Quaternion.identity);
                                break;
                        }
                        AtkFirstAppear = true;
                    }
                    if (Atk2Timer <= (Atk2TimerSet - 1.7))
                    {
                        if (!AtkSecondAppear)
                        {
                            switch (Random.Range(0, 3))
                            {
                                case 0:
                                    Instantiate(Atk2, Atk2Appear3, Quaternion.identity);
                                    break;
                                case 1:
                                    Instantiate(Atk2, Atk2Appear2, Quaternion.identity);
                                    break;
                                case 2:
                                    Instantiate(Atk2, Atk2Appear4, Quaternion.identity);
                                    break;
                            }
                            AtkSecondAppear = true;
                        }
                        if (Atk2Timer <= (Atk2TimerSet - 2.9))
                        {
                            if (!AtkThirdAppear)
                            {
                                switch (Random.Range(0, 3))
                                {
                                    case 0:
                                        Instantiate(Atk2, Atk2Appear2, Quaternion.identity);
                                        Instantiate(Atk2, Atk2Appear3, Quaternion.identity);
                                        break;
                                    case 1:
                                        Instantiate(Atk2, Atk2Appear2, Quaternion.identity);
                                        Instantiate(Atk2, Atk2Appear4, Quaternion.identity);
                                        break;
                                    case 2:
                                        Instantiate(Atk2, Atk2Appear3, Quaternion.identity);
                                        Instantiate(Atk2, Atk2Appear4, Quaternion.identity);
                                        break;
                                }
                                AtkThirdAppear = true;
                            }
                            if (Atk2Timer <= 0.2)
                            {
                                StatusJudge();
                                if (NextStatus == Status.walk)
                                {
                                    isAtkEnd = true;
                                }
                            }
                            if (Atk2Timer <= 0)
                            {
                                StatusChooseEnd = false;
                                isAtkEnd = false;
                                isAtk2 = false;
                                timerSwitch = false;
                                AtkFirstAppear = false;
                                AtkSecondAppear = false;
                                AtkThirdAppear = false;
                                Atk2Timer = Atk2TimerSet;
                                status = Status.wait;
                            }
                        }
                    }
                }
            }//橫光柱
            if (isAtk3)
            {
                Atk3Timer -= _deltaTime;
                if (Atk3Timer <= (Atk3TimerSet - 0.5))
                {
                    if (!AtkFirstAppear)
                    {
                        for (int i = 0; i < 7; i++)
                        {
                            Instantiate(Atk3, Atk3Appear1 + new Vector3((i * 4) + Random.Range(0, 3), 0, 0), Quaternion.identity);
                        }
                        AtkFirstAppear = true;
                    }
                    if (Atk3Timer <= 0.2)
                    {
                        StatusJudge();
                        if (NextStatus == Status.walk)
                        {
                            isAtkEnd = true;
                        }
                    }
                    if (Atk3Timer <= 0)
                    {
                        StatusChooseEnd = false;
                        isAtkEnd = false;
                        isAtk3 = false;
                        timerSwitch = false;
                        AtkFirstAppear = false;
                        Atk3Timer = Atk3TimerSet;
                        status = Status.wait;
                    }
                }
            }//豎光柱
            if (isAtk4)
            {
                Atk4Timer -= _deltaTime;
                if (Atk4Timer <= (Atk4TimerSet - 0.5))
                {
                    if (!AtkFirstAppear)
                    {
                        switch (face)
                        {
                            case Face.Left:
                                Instantiate(LAtk14, _transform.position + LAtk4Appear1, Quaternion.identity);
                                break;
                            case Face.Right:
                                Instantiate(RAtk14, _transform.position + RAtk4Appear1, Quaternion.identity);
                                break;
                        }
                        AtkFirstAppear = true;
                    }
                    if (Atk4Timer <= (Atk4TimerSet - 1.5))
                    {
                        if (!AtkSecondAppear)
                        {
                            switch (Random.Range(0, 3))
                            {
                                case 0:
                                    isAtk4End = true;
                                    break;
                                case 1:
                                    isAtk4Continue = true;
                                    break;
                                case 2:
                                    isAtk4Continue = true;
                                    break;
                            }
                            AtkSecondAppear = true;
                        }
                    }
                    if (isAtk4End)
                    {
                        if (Atk4Timer <= (Atk4TimerSet - 2.3))
                        {
                            StatusJudge();
                            if (NextStatus == Status.walk)
                            {
                                isAtkEnd = true;
                            }
                        }
                        if (Atk4Timer <= (Atk4TimerSet - 2.5))
                        {
                            StatusChooseEnd = false;
                            isAtkEnd = false;
                            isAtk4 = false;
                            timerSwitch = false;
                            Atk4Timer = Atk4TimerSet;
                            AtkFirstAppear = false;
                            AtkSecondAppear = false;
                            AtkFourthAppear = false;
                            isAtk4End = false;
                            status = Status.wait;
                        }
                    }
                    if (isAtk4Continue)
                    {
                        if (!AtkThirdAppear)
                        {
                            switch (face)
                            {
                                case Face.Left:
                                    Instantiate(LAtk24, _transform.position + LAtk4Appear2, Quaternion.identity);
                                    break;
                                case Face.Right:
                                    Instantiate(RAtk24, _transform.position + RAtk4Appear2, Quaternion.identity);
                                    break;
                            }
                            AtkThirdAppear = true;
                        }
                        if (Atk4Timer <= 0.2)
                        {
                            StatusJudge();
                            if (NextStatus == Status.walk)
                            {
                                isAtkEnd = true;
                            }
                        }
                        if (Atk4Timer <= 0)
                        {
                            StatusChooseEnd = false;
                            isAtkEnd = false;
                            isAtk4 = false;
                            timerSwitch = false;
                            Atk4Timer = Atk4TimerSet;
                            AtkFirstAppear = false;
                            AtkSecondAppear = false;
                            AtkThirdAppear = false;
                            AtkFourthAppear = false;
                            AtkFifthAppear = false;
                            isAtk4Continue = false;
                            status = Status.wait;
                        }
                    }
                }
            }//暗光斬
            if (isAtk5)
            {
                Atk5Timer -= _deltaTime;
                if (Atk5Timer <= (Atk5TimerSet - 0.5))
                {
                    if (!AtkFirstAppear)
                    {
                        Instantiate(Atk5, Atk5Appear, Quaternion.identity);
                        AtkFirstAppear = true;
                    }
                    if (Atk5Timer <= 0.2)
                    {
                        StatusJudge();
                        if (NextStatus == Status.walk)
                        {
                            isAtkEnd = true;
                        }
                    }
                    if (Atk5Timer <= 0)
                    {
                        StatusChooseEnd = false;
                        isAtkEnd = false;
                        isAtk5 = false;
                        timerSwitch = false;
                        AtkFirstAppear = false;
                        Atk5Timer = Atk5TimerSet;
                        status = Status.wait;
                    }
                }
            }//雨
            if (isAtk6)
            {
                Atk6Timer -= _deltaTime;
                if (Atk6Timer <= 0)
                {
                    if (!AtkFirstAppear)
                    {
                        if (isSecondPhase)
                        {
                            Instantiate(Atk6, Atk6Appear + new Vector3(0, 3.7f, 0), Quaternion.identity);
                        }
                        else
                        {
                            Instantiate(Atk6, Atk6Appear, Quaternion.identity);
                        }
                        CanUseBigBall = false;
                        AtkFirstAppear = true;
                    }
                    if (BigDarkBall.isAtkEnd)
                    {
                        Atk6EndTimer -= _deltaTime;
                        StatusJudge();
                        if (NextStatus == Status.walk)
                        {
                            isAtkEnd = true;
                        }
                    }
                    if (Atk6EndTimer <= 0)
                    {
                        StatusChooseEnd = false;
                        BigBallLastTime = _time;
                        BigDarkBall.isAtkEnd = false;
                        Atk6EndTimer = Atk6TimerSet;
                        isAtkEnd = false;
                        isAtk6 = false;
                        timerSwitch = false;
                        AtkFirstAppear = false;
                        Atk6Timer = Atk6TimerSet;
                        status = Status.wait;
                    }
                }
            }//大球
            if (isSummonGhost)
            {
                SummonGhostTimer -= _deltaTime;
                if (SummonGhostTimer <= (SummonGhostTimerSet - 1))
                {
                    if (!AtkFirstAppear)
                    {
                        if (isSecondPhase)
                        {
                            Instantiate(Ghost, new Vector3(LeftEdge + Random.Range(6, 13), YButtomEdge + Random.Range(6, 10), 0), Quaternion.identity);
                            Instantiate(Ghost, new Vector3(LeftEdge + Random.Range(12, 19), YButtomEdge + Random.Range(6, 10), 0), Quaternion.identity);
                        }
                        else
                        {
                            Instantiate(Ghost, new Vector3(LeftEdge + Random.Range(9, 17), YButtomEdge + Random.Range(6, 10), 0), Quaternion.identity);
                        }
                        CanSummonGhost = false;
                        AtkFirstAppear = true;
                    }
                    if (SummonGhostTimer <= 0.2)
                    {
                        StatusJudge();
                        if (NextStatus == Status.walk)
                        {
                            isAtkEnd = true;
                        }
                    }
                    if (SummonGhostTimer <= 0)
                    {
                        StatusChooseEnd = false;
                        isAtkEnd = false;
                        isSummonGhost = false;
                        timerSwitch = false;
                        AtkFirstAppear = false;
                        SummonGhostLastTime = _time;
                        SummonGhostTimer = SummonGhostTimerSet;
                        status = Status.wait;
                    }
                }
            }
            if (isCreatingShield)
            {
                CreatingShieldTimer -= _deltaTime;
                if (CreatingShieldTimer <= (CreatingShieldTimerSet - 0.4))
                {
                    if (!AtkFirstAppear)
                    {
                        Instantiate(ShieldImpulse, _transform.position + ShieldAppear, Quaternion.identity);
                        AtkFirstAppear = true;
                    }
                    if (CreatingShieldTimer <= (CreatingShieldTimerSet - 0.5))
                    {
                        if (!AtkSecondAppear)
                        {
                            Instantiate(Shield, _transform.position + ShieldAppear, Quaternion.identity);
                            ShouldCreateShield = false;
                            isShieldExist = true;
                            AtkSecondAppear = true;
                        }
                        if (CreatingShieldTimer <= 0.2)
                        {
                            if (!CanUseAtk1 && !CanGoSecondPhase)
                            {
                                isAtkEnd = true;
                            }
                        }
                        if (CreatingShieldTimer <= 0)
                        {
                            StatusChooseEnd = false;
                            AtkSecondAppear = false;
                            isAtkEnd = false;
                            AtkFirstAppear = false;
                            isCreatingShield = false;
                            timerSwitch = false;
                            CreatingShieldTimer = CreatingShieldTimerSet;
                            status = Status.walk;
                            if (CanUseAtk1)
                            {
                                status = Status.Atk;
                            }
                            if (CanGoSecondPhase)
                            {
                                status = Status.Atk;
                            }
                        }
                    }
                }
            }
            if (isMoveAtk)
            {
                Atk2Timer -= _deltaTime;
                if (Atk2Timer <= (Atk2TimerSet - 0.5f))
                {
                    if (!AtkFirstAppear)
                    {
                        Instantiate(ForeverAtk, ForeverAtkAppear, Quaternion.identity);
                        ShouldUseMoveAtk = false;
                        AtkFirstAppear = true;
                    }
                    if (Atk2Timer <= (Atk2TimerSet - 1))
                    {
                        StatusJudge();
                        if (NextStatus == Status.walk)
                        {
                            isAtkEnd = true;
                        }
                        if (Atk2Timer <= (Atk2TimerSet - 1.2))
                        {
                            StatusChooseEnd = false;
                            isAtkEnd = false;
                            Atk2Timer = Atk2TimerSet;
                            AtkFirstAppear = false;
                            isMoveAtk = false;
                            timerSwitch = false;
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
            Atk4Timer = Atk4TimerSet;
            Atk5Timer = Atk5TimerSet;
            Atk6Timer = Atk6TimerSet;
            Atk6EndTimer = Atk6EndTimerSet;
            SummonGhostTimer = SummonGhostTimerSet;
            CreatingShieldTimer = CreatingShieldTimerSet;
        }
    }

    void WalkTimerMethod()
    {
        if (WalkTimerSwitch)
        {
            WalkTimer -= _deltaTime;
            if (WalkTimer <= (WalkTimerSet - 0.6))
            {
                if (!isWalkMove)
                {
                    if (isPlatformAppear)
                    {
                        switch (Random.Range(0, 6))
                        {
                            case 0:
                                if (WalkPlaceNumber != 1)
                                {
                                    _transform.position = WalkPlace1.transform.position;
                                    WalkPlaceNumber = 1;
                                }
                                else
                                {
                                    _transform.position = WalkPlace2.transform.position;
                                    WalkPlaceNumber = 2;
                                }
                                break;
                            case 1:
                                if (WalkPlaceNumber != 2)
                                {
                                    _transform.position = WalkPlace2.transform.position;
                                    WalkPlaceNumber = 2;
                                }
                                else
                                {
                                    _transform.position = WalkPlace1.transform.position;
                                    WalkPlaceNumber = 1;
                                }
                                break;
                            case 2:
                                if (WalkPlaceNumber != 3)
                                {
                                    _transform.position = WalkPlace3.transform.position;
                                    WalkPlaceNumber = 3;
                                }
                                else
                                {
                                    _transform.position = WalkPlace4.transform.position;
                                    WalkPlaceNumber = 4;
                                }
                                break;
                            case 3:
                                if (WalkPlaceNumber != 4)
                                {
                                    _transform.position = WalkPlace4.transform.position;
                                    WalkPlaceNumber = 4;
                                }
                                else
                                {
                                    _transform.position = WalkPlace3.transform.position;
                                    WalkPlaceNumber = 3;
                                }
                                break;
                            case 4:
                                if (WalkPlaceNumber != 3)
                                {
                                    _transform.position = WalkPlace3.transform.position;
                                    WalkPlaceNumber = 3;
                                }
                                else
                                {
                                    _transform.position = WalkPlace4.transform.position;
                                    WalkPlaceNumber = 4;
                                }
                                break;
                            case 5:
                                if (WalkPlaceNumber != 4)
                                {
                                    _transform.position = WalkPlace4.transform.position;
                                    WalkPlaceNumber = 4;
                                }
                                else
                                {
                                    _transform.position = WalkPlace3.transform.position;
                                    WalkPlaceNumber = 3;
                                }
                                break;
                        }
                        FloatYCenter = _transform.position.y;
                    }
                    else
                    {
                        switch (Random.Range(0, 2))
                        {
                            case 0:
                                if (WalkPlaceNumber != 1)
                                {
                                    _transform.position = WalkPlace1.transform.position;
                                    WalkPlaceNumber = 1;
                                }
                                else
                                {
                                    _transform.position = WalkPlace2.transform.position;
                                    WalkPlaceNumber = 2;
                                }
                                break;
                            case 1:
                                if (WalkPlaceNumber != 2)
                                {
                                    _transform.position = WalkPlace2.transform.position;
                                    WalkPlaceNumber = 2;
                                }
                                else
                                {
                                    _transform.position = WalkPlace1.transform.position;
                                    WalkPlaceNumber = 1;
                                }
                                break;
                        }
                        FloatYCenter = _transform.position.y;
                    }

                    if (!CanFloat)
                    {
                        CanFloat = true;
                    }
                    isWalkMove = true;
                }
                if (WalkTimer <= 0)
                {
                    StatusJudge();
                    StatusChooseEnd = false;
                    isWalking = false;
                    isWalkMove = false;
                    WalkTimerSwitch = false;
                    WalkTimer = WalkTimerSet;
                    status = Status.wait;
                }
            }
        }
        else
        {
            WalkTimer = WalkTimerSet;
        }
    }

    void WeakTimerMethod()
    {
        if (WeakTimerSwitch)
        {
            WeakTimer -= _deltaTime;
            if (!isGetUp && !isGround)
            {
                Rigid2D.gravityScale = 7;
            }
            if (isGround)
            {
                Rigid2D.gravityScale = 0;
            }
            if (WeakTimer <= (WeakTimerSet - 3))
            {
                isGetUp = true;
                _transform.position += new Vector3(0, GetUpSpeed * _deltaTime, 0);
                if (_transform.position.y > FloatYCenter)
                {
                    _transform.position = new Vector3(_transform.position.x, FloatYCenter, 0);
                    ThisBoxCollider.isTrigger = true;
                    CanFloat = true;
                    isGetUp = false;
                    isWeak = false;
                    ShouldCreateShield = true;
                    status = Status.walk;
                    WeakTimerSwitch = false;
                    WeakTimer = WeakTimerSet;
                }
            }
        }
        else
        {
            WeakTimer = WeakTimerSet;
        }
    }

    void CreatePlatformTimerMethod()
    {
        if (CreatingPlatformTimerSwitch)
        {
            CreatingPlatformTimer -= _deltaTime;
            if (CreatingPlatformTimer <= (CreatingPlatformTimerSet - 0.5))
            {
                if (!AtkFirstAppear)
                {
                    Platform.SetActive(true);
                    isPlatformAppear = true;
                    CanGoSecondPhase = false;
                    isSecondPhase = true;
                    AtkFirstAppear = true;
                }
                if (CreatingPlatformTimer <= 0.2)
                {
                    StatusJudge();
                    if (NextStatus == Status.walk)
                    {
                        isAtkEnd = true;
                    }
                }
                if (CreatingPlatformTimer <= 0)
                {
                    StatusChooseEnd = false;
                    isAtkEnd = false;
                    AtkFirstAppear = false;
                    isCreatingPlatform = false;
                    CreatingPlatformTimerSwitch = false;
                    CreatingPlatformTimer = CreatingPlatformTimerSet;
                    status = Status.wait;
                }
            }
        }
        else
        {
                CreatingPlatformTimer = CreatingPlatformTimerSet;
        }
    }

    void ReSetShieldTimerMethod()
    {
        if (ReSetShieldTimerSwitch)
        {
            ReSetShieldTimer -= _deltaTime;
            if (ReSetShieldTimer <= 0)
            {
                ShouldCreateShield = true;
                ReSetShieldTimerSwitch = false;
                ReSetShieldTimer = ReSetShieldTimerSet;
            }
        }
        else
        {
            ReSetShieldTimer = ReSetShieldTimerSet;
        }
    }

    void CriticAtkHurtedTimerMethod()
    {
        if (CriticAtkHurtedSwitch)
        {
            switch(face)
            {
                case Face.Left:
                    MonsterWeakTr.localScale = new Vector3(0.28f, 0.28f, 0);
                    MonsterStopTr.localScale = new Vector3(0.28f, 0.28f, 0);
                    break;
                case Face.Right:
                    MonsterWeakTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                    MonsterStopTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                    break;
            }
            CriticAtkHurtedTimer -= _deltaTime;
            if (CriticAtkHurtedTimer <= (CriticAtkHurtedTimerSet - 0.03))
            {
                if (!HasCriticAtkAppear)
                {
                    if (!isShieldExist)
                    {
                        Instantiate(CriticAtkHurtedObject, _transform.position, Quaternion.identity);
                        if(status == Status.Weak)
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
                    }
                    HasCriticAtkAppear = true;
                }
            }
            if (CriticAtkHurtedTimer <= (CriticAtkHurtedTimerSet - 0.95))
            {
                if (!HasHurtedByCtiticAtk)
                {
                    if (!isShieldExist)
                    {
                        hp -= BattleSystem.CriticAtkHurtPower;
                    }
                    HasHurtedByCtiticAtk = true;
                }
                if (CriticAtkHurtedTimer <= 0)
                {
                    if (status != Status.Weak)
                    {
                        CanFloat = true;
                    }
                    HasHurtedByCtiticAtk = false;
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

    private void FinalAtk()
    {
        if (isFinalAtk)
        {
            FinalAtkTimer -= _deltaTime;
            isFinalWeakAniAppear = true;
            if (FinalAtkTimer <= (FinalAtkTimerSet - 1.1))
            {
                if (!isFinalActionAppear)
                {
                    isFinalActionAppear = true;
                    _transform.position = new Vector3(22.89f, -0.7f, 0);
                }
            }
            if (FinalAtkTimer <= (FinalAtkTimerSet - 2))
            {
                if (!AtkFirstAppear)
                {
                    Instantiate(Atk3, Final1Appear1, Quaternion.identity);
                    Instantiate(Atk3, Final1Appear2, Quaternion.identity);
                    AtkFirstAppear = true;
                }
            }
            if (FinalAtkTimer <= (FinalAtkTimerSet - 3))
            {
                if (!AtkSecondAppear)
                {
                    Instantiate(Atk2, Final2Appear, Quaternion.identity);
                    AtkSecondAppear = true;
                }
            }
            if (FinalAtkTimer <= (FinalAtkTimerSet - 4.1))
            {
                if (!AtkThirdAppear)
                {
                    Instantiate(FinalAtk3Up, Final3Appear1, FinalAtk3Up.transform.rotation);
                    Instantiate(FinalAtk3Under, Final3Appear2, FinalAtk3Under.transform.rotation);
                    AtkThirdAppear = true;
                }
            }
            if (FinalAtkTimer <= (FinalAtkTimerSet - 5.1))
            {
                if (!AtkFourthAppear)
                {
                    Instantiate(FinalAtk4, Final4Appear, Quaternion.identity);
                    AtkFourthAppear = true;
                }
            }
            if (FinalAtkTimer <= (FinalAtkTimerSet - 8))
            {
                if (!AtkFifthAppear)
                {
                    Instantiate(FinalAtk5, Final5Appear, Quaternion.identity);
                    AtkFifthAppear = true;
                }
            }
            if (FinalAtkTimer <= (FinalAtkTimerSet - 10.5))
            {
                if (!AtkSixAppear)
                {
                    Instantiate(FinalBigDarkBall, Final6Appear1, Quaternion.identity);
                    Instantiate(FinalBigDarkBall, Final6Appear2, Quaternion.identity);
                    AtkSixAppear = true;
                }
            }
            if (FinalAtkTimer <= (FinalAtkTimerSet - 11.7))
            {
                if (!AtkSevenAppear)
                {
                    Instantiate(FinalBigDarkBall, Final7Appear, Quaternion.identity);
                    AtkSevenAppear = true;
                }
            }
            if (FinalAtkTimer <= (FinalAtkTimerSet - 14.5))
            {
                if (!AtkEightAppear)
                {
                    Instantiate(LAtk14, playerTransform.position + Final8AppearR, Quaternion.identity);
                    AtkEightAppear = true;
                }
            }
            if (FinalAtkTimer <= (FinalAtkTimerSet - 16.2))
            {
                if (!AtkNineAppear)
                {
                    Instantiate(RAtk24, playerTransform.position + Final8AppearL, Quaternion.identity);
                    AtkNineAppear = true;
                }
            }
            if (FinalAtkTimer <= (FinalAtkTimerSet - 17.9))
            {
                if (!AtkTenAppear)
                {
                    Instantiate(RAtk14, playerTransform.position + Final8AppearL, Quaternion.identity);
                    Instantiate(LAtk24, playerTransform.position + Final8AppearR, Quaternion.identity);
                    AtkTenAppear = true;
                }
            }
            if (FinalAtkTimer <= (FinalAtkTimerSet - 19.5))
            {
                Platform2.GetComponent<Animator>().SetBool("Disappear", true);
                Platform3.GetComponent<Animator>().SetBool("Disappear", true);
                Platform4.GetComponent<Animator>().SetBool("Disappear", true);
                Platform5.GetComponent<Animator>().SetBool("Disappear", true);
            }
            if (FinalAtkTimer <= (FinalAtkTimerSet - 20))
            {
                Platform2.SetActive(false);
                Platform3.SetActive(false);
                Platform4.SetActive(false);
                Platform5.SetActive(false);
            }
            if (FinalAtkTimer <= (FinalAtkTimerSet - 20.5))
            {
                if (!AtkElevenAppear)
                {
                    Instantiate(TrueFinalAni, TrueFinalAppear, TrueFinalAni.transform.rotation);
                    AtkElevenAppear = true;
                }
            }
            if (FinalAtkTimer <= (FinalAtkTimerSet - 24.5))
            {
                if (PlayerController.isGround && playerTransform.position.y > -8)
                {
                    Instantiate(MonsterDieAnimation, _transform.position, Quaternion.identity);
                    Destroy(this.gameObject);
                    return;
                }
            }
        }
    }

    private void AllAniFalse()
    {
        if (NowAni != AniStatus.Wait && NowAni != AniStatus.Move)
        {
            MonsterMoveAnimation.SetActive(false);
        }
        if (NowAni != AniStatus.Atk)
        {
            MonsterAtkAnimation.SetActive(false);
        }
        if (NowAni != AniStatus.AtkWait)
        {
            MonsterAtkWaitAnimation.SetActive(false);
        }
        if (NowAni != AniStatus.AtkEnd)
        {
            MonsterAtkEndAnimation.SetActive(false);
        }
        if (NowAni != AniStatus.Weak)
        {
            MonsterWeakAnimation.SetActive(false);
        }
        if (NowAni != AniStatus.Stop)
        {
            MonsterStopAnimation.SetActive(false);
        }
        if (NowAni != AniStatus.FinalAtk)
        {
            MonsterFinalAtkAnimation.SetActive(false);
        }
    }

    private void BurningTimerMethod()
    {
        if (isBurning)
        {
            BurningTimer -= Time.fixedDeltaTime;
            if (!isFireAppear)
            {
                Instantiate(Fire, transform.position, Quaternion.identity, AtkTemporaryArea.transform);
                _fire = AtkTemporaryArea.transform.GetChild(0).gameObject;
                _fire1 = _fire.transform.GetChild(0).GetComponent<Animator>();
                _fire2 = _fire.transform.GetChild(1).GetComponent<Animator>();
                _fire3 = _fire.transform.GetChild(2).GetComponent<Animator>();
                AtkTemporaryArea.transform.DetachChildren();
                isFireAppear = true;
            }

            if (!isWalking)
            {
                _fire1.SetBool("Disappear", false);
                _fire2.SetBool("Disappear", false);
                _fire3.SetBool("Disappear", false);
                _fire1.SetBool("Appear", false);
                _fire2.SetBool("Appear", false);
                _fire3.SetBool("Appear", false);
            }

            if (isWalking)
            {
                _fire1.SetBool("Disappear", true);
                _fire2.SetBool("Disappear", true);
                _fire3.SetBool("Disappear", true);
            }
            if (isWalkMove)
            {
                _fire1.SetBool("Disappear", false);
                _fire2.SetBool("Disappear", false);
                _fire3.SetBool("Disappear", false);
                _fire1.SetBool("Appear", true);
                _fire2.SetBool("Appear", true);
                _fire3.SetBool("Appear", true);
            }

            if (hp > 0)
            {
                hp -= BattleSystem.CocktailHurtPower;
            }
            if (BurningTimer <= 0)
            {
                Destroy(_fire);
                isBurning = false;
                isFireAppear = false;
                BurningTimer = BurningTimerSet;
            }
        }
    }

    private void StatusJudge()
    {
        LastStatus = status;
        while (!StatusChooseEnd)
        {
            if (isPlatformAppear)
            {
                if (DistanceX <= CloseAtkDistance && DistanceY <= CloseAtkDistance)
                {
                    switch (Random.Range(0, 6))
                    {
                        case 0:
                            if (status != Status.Atk2)
                            {
                                NextStatus = Status.Atk2;
                                StatusChooseEnd = true;
                            }
                            break;
                        case 1:
                            if (status != Status.Atk3)
                            {
                                NextStatus = Status.Atk3;
                                StatusChooseEnd = true;
                            }
                            break;
                        case 2:
                            if (status != Status.Atk4)
                            {
                                NextStatus = Status.Atk4;
                                StatusChooseEnd = true;
                            }
                            break;
                        case 3:
                            if (status != Status.Atk4)
                            {
                                NextStatus = Status.Atk4;
                                StatusChooseEnd = true;
                            }
                            break;
                        case 4:
                            if (status != Status.Atk5)
                            {
                                NextStatus = Status.Atk5;
                                StatusChooseEnd = true;
                            }
                            break;
                        case 5:
                            if (CanUseBigBall && status != Status.Atk6)
                            {
                                NextStatus = Status.Atk6;
                                StatusChooseEnd = true;
                            }
                            break;
                        case 6:
                            if (status != Status.walk)
                            {
                                NextStatus = Status.walk;
                                StatusChooseEnd = true;
                            }
                            break;
                    }
                }
                else
                {
                    switch (Random.Range(0, 5))
                    {
                        case 0:
                            if (status != Status.Atk2)
                            {
                                NextStatus = Status.Atk2;
                                StatusChooseEnd = true;
                            }
                            break;
                        case 1:
                            if (status != Status.Atk3)
                            {
                                NextStatus = Status.Atk3;
                                StatusChooseEnd = true;
                            }
                            break;
                        case 2:
                            if (status != Status.Atk5)
                            {
                                NextStatus = Status.Atk5;
                                StatusChooseEnd = true;
                            }
                            break;
                        case 3:
                            if (CanUseBigBall && status != Status.Atk6)
                            {
                                NextStatus = Status.Atk6;
                                StatusChooseEnd = true;
                            }
                            break;
                        case 5:
                            if (status != Status.walk)
                            {
                                NextStatus = Status.walk;
                                StatusChooseEnd = true;
                            }
                            break;
                    }
                }
            }
            else
            {
                if (DistanceX <= CloseAtkDistance && DistanceY <= CloseAtkDistance)
                {
                    switch (Random.Range(0, 6))
                    {
                        case 0:
                            if (status != Status.Atk3)
                            {
                                NextStatus = Status.Atk3;
                                StatusChooseEnd = true;
                            }
                            break;
                        case 1:
                            if (status != Status.Atk4)
                            {
                                NextStatus = Status.Atk4;
                                StatusChooseEnd = true;
                            }
                            break;
                        case 2:
                            if (status != Status.Atk4)
                            {
                                NextStatus = Status.Atk4;
                                StatusChooseEnd = true;
                            }
                            break;
                        case 3:
                            if (status != Status.Atk5)
                            {
                                NextStatus = Status.Atk5;
                                StatusChooseEnd = true;
                            }
                            break;
                        case 4:
                            if (CanUseBigBall && status != Status.Atk6)
                            {
                                NextStatus = Status.Atk6;
                                StatusChooseEnd = true;
                            }
                            break;
                        case 5:
                            if (status != Status.walk)
                            {
                                NextStatus = Status.walk;
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
                            if (status != Status.Atk3)
                            {
                                NextStatus = Status.Atk3;
                                StatusChooseEnd = true;
                            }
                            break;
                        case 1:
                            if (status != Status.Atk5)
                            {
                                NextStatus = Status.Atk5;
                                StatusChooseEnd = true;
                            }
                            break;
                        case 2:
                            if (CanUseBigBall && status != Status.Atk6)
                            {
                                NextStatus = Status.Atk6;
                                StatusChooseEnd = true;
                            }
                            break;
                        case 3:
                            if (status != Status.walk)
                            {
                                NextStatus = Status.walk;
                                StatusChooseEnd = true;
                            }
                            break;
                    }
                }
            }
            if (CanSummonGhost)
            {
                switch (Random.Range(0, 4))
                {
                    case 0:
                        NextStatus = Status.SummonGhost;
                        StatusChooseEnd = true;
                        break;
                }
            }
            if (CanUseAtk1)
            {
                NextStatus = Status.Atk;
                StatusChooseEnd = true;
            }
            if (ShouldCreateShield)
            {
                NextStatus = Status.CreateShield;
                StatusChooseEnd = true;
            }
            if (ShouldUseMoveAtk)
            {
                NextStatus = Status.MoveAtk;
                StatusChooseEnd = true;
            }
            if (CanGoSecondPhase)
            {
                NextStatus = Status.CreatePlatform;
                StatusChooseEnd = true;
            }
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
}

