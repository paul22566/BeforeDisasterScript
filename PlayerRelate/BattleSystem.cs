using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    private Transform _transform;
    private PlayerController _playerController;
    public itemManage _itemManage;
    private AniMethod _aniMethod;

    private float NormalAtkHurtPowerSet = 10;
    private float CAtkHurtPowerSet = 20;
    private float BulletHurtPowerSet = 3;
    private float ExplosionHurtPowerSet = 45;
    private float CocktailHurtPowerSet = 0.32f;
    private float CriticAtkHurtPowerSet = 60;
    private float BlockNormalAtkHurtPowerSet = 20;
    private float BlockStrongAtkHurtPowerSet = 40;
    private float BigGunPowerSet = 1.5f;

    public static float NormalAtkHurtPower;//怪物script都會用到(enemyEscape)
    public static float CAtkHurtPower;//怪物script都會用到(enemyEscape)
    public static float BulletHurtPower ;//怪物script都會用到(enemyEscape)
    public static float ExplosionHurtPower;//怪物script都會用到(enemyEscape)
    public static float CocktailHurtPower;//怪物script都會用到(enemyEscape)
    public static float CriticAtkHurtPower;//怪物script都會用到(enemyEscape)
    public static float BlockNormalAtkHurtPower;//怪物script都會用到
    public static float BlockStrongAtkHurtPower;//怪物script都會用到
    public static float BigGunPower;//怪物script都會用到
    public static int IncresePlayerPowerNumber = 10;//普攻打到時增加的能量(以次為單位)  怪物script都會用到 (enemyEscape)

    private Rigidbody2D Rigid2D;
    private SlopeControll _slopeControll;
    private Transform TemporaryArea;
    [HideInInspector] public int NowMaxSkillPower = 900;//有被其他script用到(playerController，playerspecialAni)
    [HideInInspector] public int SkillPower;//有被其他script用到(playerController，PowerUI，playerspecialAni)
    [HideInInspector] public int TrueMaxSkillPower = 900;//UI魔力條滿值
    public static int KillerPoint;
    [HideInInspector] public int MaxKillerPoint = 200;
    private float AtkCoolDown = 0.25f;
    private float AtkLastTime = -10;
    public static int DecreaseTimes = 0;//有被其他script用到(Training2Controller)
    public static int IncreaseTimes = 0;//有被其他script用到(怪物script，(enemyEscape)) 
    [HideInInspector] public float ThrowItemPowerX;//紀錄投擲物的力度  有被其他script用到(ThrowItem)
    [HideInInspector] public float ThrowItemPowerY;//紀錄投擲物的力度  有被其他script用到(ThrowItem)
    private float ShootLastTime;
    private float ShootCoolDown = 0.05f;
    private float WalkThrowCocktailPowerX = 200;
    private float WalkThrowCocktailPowerY = 100;
    private float JumpThrowCocktailPowerX = 200;
    private float JumpThrowCocktailPowerY = -100;
    private float ImpulseJumpPowerX = 800;
    private float ImpulseJumpPowerY = 600;
    private float SharpenerRate = 1.3f;
    private float _deltaTime;
    private float _UnScaleDeltaTime;
    private float _time;
    private float _fixedDeltaTime;

    //TimerSet
    private float AtkTimerSet = 0.25f;
    private float CAtkTimerSet = 0.5f;
    private float JumpCAtkTimerSet = 0.55f;
    private float ThrowTimerSet = 0.5f;
    private float CocktailCriticAtkTimerSet = 1.6f;
    private float CriticAtkTimerSet = 3.25f;
    private float AtkTimer;
    private float AtkSwitchTimerSet = 2;
    private float AtkSwitchTimer;
    private float AccumulateTimer;
    private float AccumulateTimerSet = 1;
    private float ShootingEndTimer;
    private float ShootingEndTimerSet = 0.3f;
    private float ShootAccumulateTimer;
    private float ShootAccumulateTimerSet = 2f;
    private float BigGunTimer;
    private float BigGunTimerSet = 2.35f;
    private float ImpulseJumpTimer;
    private float ImpulseJumpTimerSet = 0.5f;
    private float BlockTimerSet = 1.25f;
    private float BlockTimer;
    private float BlockPrepareAtkTimerSet = 1.55f;
    private float BlockPrepareAtkTimer;
    private float BlockAtkTimerSet = 1.9f;
    private float BlockAtkTimer;
    private float BeBlockTimerSet = 2.3f;
    private float BeBlockTimer;
    private float WeakTimerSet = 2.3f;
    private float WeakTimer;
    private float SharpenBladeTimerSet = 3.5f;
    private float UseRitualSwordTimerSet = 2.2f;
    private float SharpTimeSet = 61;//Buff持續時間
    private float SharpTimeLeft;
    private float InhibitTimeSet = 61;//Buff持續時間
    private float InhibitTimeLeft;


    public static bool isBlockSuccessWait;//格檔成功後等待指令期  有被其他script用到(playerblockjudgement)
    public static bool isBeBlockSuccess;//有被其他script用到(playerblockjudgement，AtkController)
    public static bool isBlockSuccess;//格檔成功開關  有被其他script用到(playerBlockJudgement，normalmonsterAtk)
    [HideInInspector] public bool isAtk;//是不是在攻擊中 有被其他script用到(PlayerController)
    [HideInInspector] public bool isCAtk;//是不是在c攻擊中 有被其他script用到(PlayerController)
    [HideInInspector] public bool isCriticAtk;//是不是在大招 有被其他script用到(playerController)
    [HideInInspector] public bool isThrowing;//是不是在投擲 有被其他script用到(playerController)
    [HideInInspector] public bool isSharpen;//是不是在磨刀
    [HideInInspector] public bool isAtkBuff;//刀是否是鋒利狀態
    [HideInInspector] public bool isDefendBuff;//是否處在防禦強化狀態
    [HideInInspector] public bool isInhibit;//是否處在抑制狀態
    [HideInInspector] public bool isUsingRitualSword;//是否在使用儀式刀
    [HideInInspector] public bool isPowerSeal;//是否處在魔力封印狀態
    [HideInInspector] public bool isAccumulate;//是不是在蓄力 有被其他script用到(playerController)
    [HideInInspector] public bool AccumulateTimerSwitch;//有被其他script用到(playerController)
    [HideInInspector] public bool isJumpCAtk;//是不是在c攻擊中 有被其他script用到(playerController)
    [HideInInspector] public bool isAccumulateComplete;//能不能用大招 有被其他script用到(playerController)
    [HideInInspector] public bool isCriticAtkChangeStage;//大招動畫轉換開關 有被其他script用到(playerController)
    [HideInInspector] public bool isAim;//是不是正在瞄準(燃燒瓶) 有被其他script用到(PredictPowerBase，AimPowerController)
    [HideInInspector] public bool isShooting;//有被其他script用到(playerController)
    [HideInInspector] public bool isShootAccumulate;//有被其他script用到(playerController)
    [HideInInspector] public bool isBigGunShoot;//有被其他script用到(playerController,BigGunController)
    [HideInInspector] public bool isBigGunProcess;//有被其他script用到(playerController)
    [HideInInspector] public bool isBigGunEnd;//有被其他script用到(playerController,BigGunController)
    [HideInInspector] public bool HasAimAppear = false;//有被其他script用到(playerController)
    [HideInInspector] public bool isBlock;//是否在格檔 有被其他script用到(playerController，atkcontroller)
    [HideInInspector] public bool isBlockEnd;//沒有格檔成功結束開關  有被其他script用到(playerController)
    [HideInInspector] public bool isBlockNormalAtk;//有被其他script用到(playerController)
    [HideInInspector] public bool isBlockStrongAtk;//有被其他script用到(playerController)
    [HideInInspector] public bool isJumpAtk;//區分是否為跳打 有被其他script用到(playerController)
    [HideInInspector] public bool BeBlockisGround;//有被其他script用到(playerController)
    [HideInInspector] public bool isBlockActualAppear;//有被其他script用到(playerController，playerBlockJudgement)
    [HideInInspector] public bool BlockAtkSwitch;
    [HideInInspector] public bool isSecondAtk;//有被其他script用到(playerController)
    [HideInInspector] public bool isCaptured;//有被其他script用到(playerController，有投技的monster)
    [HideInInspector] public bool isWeak;//有被其他script用到(playerController)
    [HideInInspector] public bool WeakTimerSwitch;//有被其他script用到(playerController)
    [HideInInspector] public bool CanWalkThrow;//有被其他script用到(playerController)
    [HideInInspector] public bool isWalkThrow;//有被其他script用到(playerController)
    [HideInInspector] public bool isJumpThrow;//有被其他script用到(playerController)
    [HideInInspector] public bool isCocktailCriticAtk;//有被其他script用到(playerController)
    [HideInInspector] public bool isImpulseJump;//有被其他script用到(playerController)
    private bool CanAtk;
    private bool TimerSwitch;
    private bool FirstTrigger = false;
    private bool SecondTrigger = false;
    private bool ThirdTrigger = false;
    private bool shootTimerSwitch;
    private bool BlockTimerSwitch;
    private bool isBeBlockJudgement;
    private bool isJumpAtkJudgement;
    private bool ShootAccumulateTimerSwitch;
    private bool isAllowBigGun;
    private bool BigGunTimerSwitch;
    private bool AtkSwitchTimerSwitch;
    private bool CanSecondAtk;
    private bool WalkThrowFaceRight;
    private bool WalkThrowFaceLeft;

    [Header("攻擊物件")]
    public GameObject Bullet;
    public GameObject RNormalAtk;
    public GameObject LNormalAtk;
    public GameObject RSecondAtk;
    public GameObject LSecondAtk;
    public GameObject RCAtk;
    public GameObject LCAtk;
    public GameObject RJumpAtk;
    public GameObject LJumpAtk;
    public GameObject CockTail;
    public GameObject WalkThrowCocktail;
    public GameObject ExplosionBottle;
    public GameObject WalkThrowExplosionBottle;
    public GameObject RPowerLine;
    public GameObject LPowerLine;
    public GameObject RPredictPowerBase;
    public GameObject LPredictPowerBase;
    public GameObject RAimPoint;
    public GameObject LAimPoint;
    public GameObject RCriticAtk;
    public GameObject LCriticAtk;
    public GameObject RJumpCAtk;
    public GameObject LJumpCAtk;
    public GameObject RBlock;
    public GameObject LBlock;
    public GameObject RBlockNormalAtkAni;
    public GameObject LBlockNormalAtkAni;
    public GameObject RBlockNormalAtk;
    public GameObject LBlockNormalAtk;
    public GameObject RBlockStrongAtk;
    public GameObject LBlockStrongAtk;
    public GameObject RBigGun;
    public GameObject LBigGun;
    public GameObject RCocktailCriticAtk;
    public GameObject LCocktailCriticAtk;
    public GameObject ImpulseJumpExplosion;
    //各類型攻擊應該出現地點
    Vector3 RBulletAppear = new Vector3(1.237f, 0.533f, 0);
    Vector3 LBulletAppear = new Vector3(-1.237f, 0.533f, 0);
    Vector3 RBulletJumpAppear = new Vector3(1.441f, 0.516f, 0);
    Vector3 LBulletJumpAppear = new Vector3(-1.441f, 0.516f, 0);
    Vector3 RThrowItemAppear = new Vector3(0.8f, 1.21f, 0);
    Vector3 LThrowItemAppear = new Vector3(-0.8f, 1.21f, 0);
    Vector3 RWalkThrowItemAppear = new Vector3(0.3f, 1.07f, 0);
    Vector3 LWalkThrowItemAppear = new Vector3(-0.3f, 1.07f, 0);
    Vector3 RJumpThrowItemAppear = new Vector3(0.859f, 0.332f, 0);
    Vector3 LJumpThrowItemAppear = new Vector3(-0.859f, 0.332f, 0);
    Vector3 RAimAppear = new Vector3(2.505f, 2.175f, 0);
    Vector3 LAimAppear = new Vector3(-2.505f, 2.175f, 0);
    Vector3 RCriticAtkAppear = new Vector3(7f, 0.21f, 0);
    Vector3 LCriticAtkAppear = new Vector3(-7f, 0.21f, 0);
    Vector3 RImpulseExplosionAppear = new Vector3(-1.057f, -0.055f, 0);
    Vector3 LImpulseExplosionAppear = new Vector3(1.057f, -0.055f, 0);
    public Transform BigGunAppear;//script(BigGunController)

    public GameObject CriticAtkPredictObject;
    private Vector3 CriticAtkPredictPosition = new Vector3();
    private float CriticAtkPredictAngle;
    private Transform _criticAtkPredictObject;

    // Start is called before the first frame update
    void Start()
    {
        _transform = this.transform;
        _playerController = this.GetComponent<PlayerController>();
        _slopeControll = _playerController._slopeControll;
        SkillPower = 0;
        Rigid2D = this.GetComponent<Rigidbody2D>();
        TemporaryArea = this.transform.GetChild(5);

        JudgeBuff();
        NowMaxSkillPower = 900;
        TrueMaxSkillPower = 900;
        if (GameEvent.AbsorbBoss2)
        {
            NowMaxSkillPower = 1500;
            TrueMaxSkillPower = 1500;
        }
        if (isPowerSeal)
        {
            NowMaxSkillPower = TrueMaxSkillPower - 300;
        }

        if (GameObject.Find("FollowSystem") != null)
        {
            _aniMethod = GameObject.Find("FollowSystem").GetComponent<AniMethod>();
        }

        ResetAtkPower();
    }

    // Update is called once per frame
    void Update()
    {
        _time = Time.time;
        _deltaTime = Time.deltaTime;
        _UnScaleDeltaTime = Time.unscaledDeltaTime * BackgroundSystem.BasicGameSpeed;

        JudgeBuff();

        if (isPowerSeal)
        {
            NowMaxSkillPower = TrueMaxSkillPower - 300;
        }
        else
        {
            NowMaxSkillPower = TrueMaxSkillPower;
        }
        if (SkillPower > NowMaxSkillPower)
        {
            SkillPower = NowMaxSkillPower;
        }
        if (SkillPower <= 0)
        {
            SkillPower = 0;
        }

        //KillerPointControll();
        
        //冷卻時間
        if ((AtkCoolDown + AtkLastTime) <= _time)
        {
            CanAtk = true;
        }

        //瞄準被中斷
        if (isAim)
        {
            if (PlayerController.isHurted || _playerController.isDash)
            {
                isAim = false;
                HasAimAppear = false;
            }
        }

        AtkSwitchTimerMethod();

        BigGunTimerMethod();

        AccumulateTimerMethod();

        ShootAccumulateTimerMethod();

        BlockTimerMethod();

        ShootTimerMethod();

        BlockSuccessTimerMethod();

        BlockAtkTimerMethod();

        BeBlockTimerMethod();

        WeakTimerMethod();
    }

    private void FixedUpdate()
    {
        _fixedDeltaTime = Time.fixedDeltaTime;
        
        //鋒利度持續時間
        if (isAtkBuff && !GameEvent.isAniPlay)
        {
            SharpTimeLeft -= _fixedDeltaTime;
            if (SharpTimeLeft <= 0)
            {
                isAtkBuff = false;
                ResetAtkPower();
            }
        }
        //抑制蛇神持續時間
        if (isInhibit && !GameEvent.isAniPlay)
        {
            InhibitTimeLeft -= _fixedDeltaTime;
            if (InhibitTimeLeft <= 0)
            {
                isInhibit = false;
            }
        }

        timer();

        ImpulseJumpTimerMethod();

        IncreaseSkillPower();
        DecreaseSkillPower();
    }

    public void AccumulateSystem()
    {
        if (isBlockSuccessWait)
        {
            BlockAtk(1);
        }
        else
        {
            if (!_playerController.CantDoAnyThing)
            {
                AccumulateTimerSwitch = true;
                isAccumulate = true;
            }
        }
    }
    public void NormalAtkSystem()
    {
        if (isAccumulate)
        {
            if (isAccumulateComplete)
            {
                if (SkillPower >= 600 && PlayerController.isGround && GameEvent.TutorialComplete)
                {
                    DecreaseTimes += 20;
                    isCriticAtk = true;
                    _playerController.CantDoAnyThing = true;
                    TimerSwitch = true;
                    isAccumulate = false;
                    AccumulateTimerSwitch = false;
                    isAccumulateComplete = false;
                }
                else
                {
                    //地上普攻
                    if (PlayerController.isGround && CanAtk && !_playerController.CantDoAnyThing)
                    {
                        if (CanSecondAtk)
                        {
                            isSecondAtk = true;
                            _playerController.CantDoAnyThing = true;
                            TimerSwitch = true;
                            AtkLastTime = _time;
                            isAccumulate = false;
                            AccumulateTimerSwitch = false;
                            isAccumulateComplete = false;
                        }
                        else
                        {
                            isAtk = true;
                            _playerController.CantDoAnyThing = true;
                            TimerSwitch = true;
                            AtkLastTime = _time;
                            isAccumulate = false;
                            AccumulateTimerSwitch = false;
                            isAccumulateComplete = false;
                        }
                    }
                    //空中普攻
                    if (!PlayerController.isGround && !isJumpThrow && CanAtk && !_playerController.CantDoAnyThing)
                    {
                        _playerController.CantDoAnyThing = true;
                        _playerController.OnlyCanMove = true;
                        isAtk = true;
                        TimerSwitch = true;
                        AtkLastTime = _time;
                        isAccumulate = false;
                        AccumulateTimerSwitch = false;
                        isAccumulateComplete = false;
                    }
                }
            }
            else
            {
                //地上普攻
                if (PlayerController.isGround && CanAtk && !_playerController.CantDoAnyThing)
                {
                    if (CanSecondAtk)
                    {
                        isSecondAtk = true;
                        _playerController.CantDoAnyThing = true;
                        TimerSwitch = true;
                        AtkLastTime = _time;
                        isAccumulate = false;
                        AccumulateTimerSwitch = false;
                        isAccumulateComplete = false;
                    }
                    else
                    {
                        isAtk = true;
                        _playerController.CantDoAnyThing = true;
                        TimerSwitch = true;
                        AtkLastTime = _time;
                        isAccumulate = false;
                        AccumulateTimerSwitch = false;
                        isAccumulateComplete = false;
                    }
                }
                //空中普攻
                if (!PlayerController.isGround && !isJumpThrow && CanAtk && !_playerController.CantDoAnyThing)
                {
                    _playerController.CantDoAnyThing = true;
                    _playerController.OnlyCanMove = true;
                    isAtk = true;
                    TimerSwitch = true;
                    AtkLastTime = _time;
                    isAccumulate = false;
                    AccumulateTimerSwitch = false;
                    isAccumulateComplete = false;
                }
            }
        }
    }
    public void StrongAtkSystem()
    {
        if (isBlockSuccessWait)
        {
            BlockAtk(2);
        }
        else
        {
            if (CanAtk && !_playerController.CantDoAnyThing)
            {
                if (PlayerController.isGround)
                {
                    if (SkillPower >= 300)
                    {
                        DecreaseTimes += 10;
                        isCAtk = true;
                        _playerController.CantDoAnyThing = true;
                        TimerSwitch = true;
                        AtkLastTime = _time;
                    }
                }
                else
                {
                    if (SkillPower >= 300 && !isJumpThrow)
                    {
                        DecreaseTimes += 10;
                        isJumpCAtk = true;
                        _playerController.CantDoAnyThing = true;
                        TimerSwitch = true;
                        AtkLastTime = _time;
                    }
                }
            }
        }
    }

    private void timer()
    {
        if (TimerSwitch)
        {
            if (isAtk)
            {
                if (isBeBlockSuccess)
                {
                    FirstTrigger = false;
                    isAtk = false;
                    isJumpAtkJudgement = false;
                    TimerSwitch = false;
                    AtkSwitchTimerSwitch = true;
                    return;
                }
                if (!isJumpAtkJudgement)
                {
                    if (!PlayerController.isGround)
                    {
                        isJumpAtk = true;
                    }
                    else
                    {
                        isJumpAtk = false;
                    }
                    isJumpAtkJudgement = true;
                }
                if (isJumpAtk)
                {
                    if (AtkTimer <= 0)
                    {
                        AtkTimer = AtkTimerSet;
                    }

                    AtkTimer -= _fixedDeltaTime;
                    if (AtkTimer <= (AtkTimerSet - 0.067))
                    {
                        if (!FirstTrigger)
                        {
                            switch (PlayerController.face)
                            {
                                case PlayerController.Face.Left:
                                    Instantiate(LJumpAtk, this.transform.position, Quaternion.identity);
                                    FirstTrigger = true;
                                    break;
                                case PlayerController.Face.Right:
                                    Instantiate(RJumpAtk, this.transform.position, Quaternion.identity);
                                    FirstTrigger = true;
                                    break;
                            }
                        }
                        if (AtkTimer <= 0)
                        {
                            _playerController.OnlyCanMove = false;
                            FirstTrigger = false;
                            isAtk = false;
                            isJumpAtkJudgement = false;
                            _playerController.CantDoAnyThing = false;
                            TimerSwitch = false;
                        }
                    }
                }
                else
                {
                    if (AtkTimer <= 0)
                    {
                        AtkTimer = AtkTimerSet;
                    }

                    AtkTimer -= _fixedDeltaTime;
                    if (AtkTimer <= (AtkTimerSet - 0.067))
                    {
                        if (!FirstTrigger)
                        {
                            switch (PlayerController.face)
                            {
                                case PlayerController.Face.Left:
                                    Instantiate(LNormalAtk, this.transform.position, Quaternion.identity);
                                    FirstTrigger = true;
                                    break;
                                case PlayerController.Face.Right:
                                    Instantiate(RNormalAtk, this.transform.position, Quaternion.identity);
                                    FirstTrigger = true;
                                    break;
                            }
                        }
                        if (AtkTimer <= 0)
                        {
                            AtkSwitchTimerSwitch = true;
                            _playerController.OnlyCanMove = false;
                            FirstTrigger = false;
                            isJumpAtkJudgement = false;
                            isAtk = false;
                            _playerController.CantDoAnyThing = false;
                            TimerSwitch = false;
                        }
                    }
                }
            }
            if (isSecondAtk)
            {
                if (AtkTimer <= 0)
                {
                    AtkTimer = AtkTimerSet;
                }

                if (isBeBlockSuccess)
                {
                    FirstTrigger = false;
                    isSecondAtk = false;
                    TimerSwitch = false;
                    CanSecondAtk = false;
                    AtkSwitchTimerSwitch = false;
                    return;
                }

                AtkTimer -= _fixedDeltaTime;
                if (AtkTimer <= (AtkTimerSet - 0.067))
                {
                    if (!FirstTrigger)
                    {
                        switch (PlayerController.face)
                        {
                            case PlayerController.Face.Left:
                                Instantiate(LSecondAtk, this.transform.position, Quaternion.identity);
                                FirstTrigger = true;
                                break;
                            case PlayerController.Face.Right:
                                Instantiate(RSecondAtk, this.transform.position, Quaternion.identity);
                                FirstTrigger = true;
                                break;

                        }
                    }
                    if (AtkTimer <= 0)
                    {
                        FirstTrigger = false;
                        isSecondAtk = false;
                        _playerController.CantDoAnyThing = false;
                        TimerSwitch = false;
                        CanSecondAtk = false;
                        AtkSwitchTimerSwitch = false;
                    }
                }
            }
            if (isCAtk)
            {
                if (AtkTimer <= 0)
                {
                    AtkTimer =  CAtkTimerSet;
                }

                AtkTimer -= _fixedDeltaTime;

                if (AtkTimer <= (CAtkTimerSet - 0.2))
                {
                    if (!FirstTrigger)
                    {
                        switch (PlayerController.face)
                        {
                            case PlayerController.Face.Left:
                                Instantiate(LCAtk, this.transform.position, Quaternion.identity);
                                FirstTrigger = true;
                                break;
                            case PlayerController.Face.Right:
                                Instantiate(RCAtk, this.transform.position, Quaternion.identity);
                                FirstTrigger = true;
                                break;
                        }
                    }
                    if (AtkTimer <= 0)
                    {
                        FirstTrigger = false;
                        isCAtk = false;
                        _playerController.CantDoAnyThing = false;
                        TimerSwitch = false;
                    }
                }
            }
            if (isJumpCAtk)
            {
                if (AtkTimer <= 0)
                {
                    _playerController.ShouldIgnoreGravity = true;
                    Rigid2D.velocity = new Vector2(0, 0);
                    AtkTimer = JumpCAtkTimerSet;
                }

                AtkTimer -= _fixedDeltaTime;

                if (AtkTimer <= (JumpCAtkTimerSet - 0.1))
                {
                    if (!FirstTrigger)
                    {
                        switch (PlayerController.face)
                        {
                            case PlayerController.Face.Left:
                                Instantiate(LJumpCAtk, this.transform.position, Quaternion.identity);
                                FirstTrigger = true;
                                break;
                            case PlayerController.Face.Right:
                                Instantiate(RJumpCAtk, this.transform.position, Quaternion.identity);
                                FirstTrigger = true;
                                break;
                        }
                    }
                }
                if (AtkTimer <= (JumpCAtkTimerSet - 0.3))
                {
                    if (!SecondTrigger)
                    {
                        _playerController.ShouldIgnoreGravity = false;
                        SecondTrigger = true;
                    }
                }
                if (AtkTimer <= 0)
                {
                    FirstTrigger = false;
                    SecondTrigger = false;
                    isJumpCAtk = false;
                    _playerController.CantDoAnyThing = false;
                    TimerSwitch = false;
                }
                if (PlayerController.isHurted)
                {
                    _playerController.ShouldIgnoreGravity = false;
                    FirstTrigger = false;
                    SecondTrigger = false;
                    isJumpCAtk = false;
                    TimerSwitch = false;
                    AtkTimer = 0;
                }
            }
            if (isThrowing)
            {
                if (AtkTimer <= 0)
                {
                    AtkTimer = ThrowTimerSet;
                }

                AtkTimer -= _fixedDeltaTime;

                if (AtkTimer <= (ThrowTimerSet - 0.25))
                {
                    if (!FirstTrigger)
                    {
                        switch (_itemManage.NowPrepareItem)
                        {
                            case itemManage.PrepareItem.Cocktail:
                                switch (PlayerController.face)
                                {
                                    case PlayerController.Face.Left:
                                        Instantiate(CockTail, this.transform.position + LThrowItemAppear, Quaternion.identity);
                                        break;
                                    case PlayerController.Face.Right:
                                        Instantiate(CockTail, this.transform.position + RThrowItemAppear, Quaternion.identity);
                                        break;
                                }
                                break;
                            case itemManage.PrepareItem.ExplosionBottle:
                                switch (PlayerController.face)
                                {
                                    case PlayerController.Face.Left:
                                        Instantiate(ExplosionBottle, this.transform.position + LThrowItemAppear, Quaternion.identity);
                                        break;
                                    case PlayerController.Face.Right:
                                        Instantiate(ExplosionBottle, this.transform.position + RThrowItemAppear, Quaternion.identity);
                                        break;
                                }
                                break;
                        }
                        FirstTrigger = true;
                    }
                    if (AtkTimer <= 0)
                    {
                        FirstTrigger = false;
                        isThrowing = false;
                        _playerController.CantDoAnyThing = false;
                        TimerSwitch = false;
                    }
                }
            }
            if (isCriticAtk)
            {
                if (AtkTimer <= 0)
                {
                    AtkTimer = CriticAtkTimerSet;
                    Instantiate(CriticAtkPredictObject, _transform.localPosition, Quaternion.identity, TemporaryArea);
                    _criticAtkPredictObject = TemporaryArea.GetChild(0);
                    TemporaryArea.DetachChildren();
                }

                AtkTimer -= _fixedDeltaTime;

                if (AtkTimer <= (CriticAtkTimerSet - 0.97))
                {
                    if (!FirstTrigger)
                    {
                        CriticAtkPredictPosition = new Vector3(_criticAtkPredictObject.position.x, _criticAtkPredictObject.position.y, _criticAtkPredictObject.position.z);
                        CriticAtkPredictAngle = AngleCaculate.CaculateAngle("R", _criticAtkPredictObject, _transform);
                        switch (PlayerController.face)
                        {
                            case PlayerController.Face.Left:
                                Instantiate(LCriticAtk, CalculateCriticAtkPosition(), Quaternion.Euler(0, 0, CriticAtkPredictAngle));
                                break;
                            case PlayerController.Face.Right:
                                CriticAtkPredictAngle = AngleCaculate.AngleDirectionChange(CriticAtkPredictAngle);
                                Instantiate(RCriticAtk, CalculateCriticAtkPosition(), Quaternion.Euler(0, 0, CriticAtkPredictAngle));
                                break;
                        }
                        FirstTrigger = true;
                    }
                }
                if (AtkTimer <= (CriticAtkTimerSet - 1))
                {
                    if (!SecondTrigger)
                    {
                        _aniMethod.OpenFlash();
                        if (_criticAtkPredictObject != null)
                        {
                            //預設距離15
                            _transform.position = CriticAtkPredictPosition;
                            Destroy(_criticAtkPredictObject.gameObject);
                        }
                        isCriticAtkChangeStage = true;
                        SecondTrigger = true;
                    }
                }
                if (AtkTimer <= (CriticAtkTimerSet - 2.9f))
                {
                    if (!ThirdTrigger)
                    {
                        _aniMethod.OpenFlash();
                        ThirdTrigger = true;
                    }
                }
                if (AtkTimer <= 0)
                {
                    ThirdTrigger = false;
                    FirstTrigger = false;
                    SecondTrigger = false;
                    isCriticAtk = false;
                    _playerController.CantDoAnyThing = false;
                    isCriticAtkChangeStage = false;
                    TimerSwitch = false;
                    AtkLastTime = _time;
                }
                if (PlayerController.isHurted)
                {
                    ThirdTrigger = false;
                    FirstTrigger = false;
                    SecondTrigger = false;
                    isCriticAtk = false;
                    isCriticAtkChangeStage = false;
                    TimerSwitch = false;
                    AtkTimer = 0;
                    AtkLastTime = _time;
                    if (_criticAtkPredictObject != null)
                    {
                        Destroy(_criticAtkPredictObject.gameObject);
                    }
                }
            }
            if (isWalkThrow)
            {
                if (AtkTimer <= 0)
                {
                    AtkTimer = ThrowTimerSet;
                }

                AtkTimer -= _fixedDeltaTime;

                if (!_playerController.isWalking || PlayerController.isHurted)
                {
                    isWalkThrow = false;
                    TimerSwitch = false;
                    AtkTimer = CriticAtkTimerSet;
                    WalkThrowFaceRight = false;
                    WalkThrowFaceLeft = false;
                    FirstTrigger = false;
                    AtkTimer = 0;
                    return;
                }
                if (WalkThrowFaceLeft && PlayerController.face != PlayerController.Face.Left)
                {
                    isWalkThrow = false;
                    TimerSwitch = false;
                    AtkTimer = CriticAtkTimerSet;
                    WalkThrowFaceRight = false;
                    WalkThrowFaceLeft = false;
                    FirstTrigger = false;
                }
                if (WalkThrowFaceRight && PlayerController.face != PlayerController.Face.Right)
                {
                    isWalkThrow = false;
                    TimerSwitch = false;
                    AtkTimer = CriticAtkTimerSet;
                    WalkThrowFaceRight = false;
                    WalkThrowFaceLeft = false;
                    FirstTrigger = false;
                }
                if (AtkTimer <= (ThrowTimerSet - 0.2))
                {
                    if (!FirstTrigger)
                    {
                        ThrowItemPowerX = WalkThrowCocktailPowerX;
                        ThrowItemPowerY = WalkThrowCocktailPowerY;
                        switch (_itemManage.NowPrepareItem)
                        {
                            case itemManage.PrepareItem.Cocktail:
                                switch (PlayerController.face)
                                {
                                    case PlayerController.Face.Left:
                                        Instantiate(WalkThrowCocktail, this.transform.position + LWalkThrowItemAppear, Quaternion.identity);
                                        break;
                                    case PlayerController.Face.Right:
                                        Instantiate(WalkThrowCocktail, this.transform.position + RWalkThrowItemAppear, Quaternion.identity);
                                        break;
                                }
                                _itemManage.CocktailNumber -= 1;
                                break;
                            case itemManage.PrepareItem.ExplosionBottle:
                                switch (PlayerController.face)
                                {
                                    case PlayerController.Face.Left:
                                        Instantiate(WalkThrowExplosionBottle, this.transform.position + LWalkThrowItemAppear, Quaternion.identity);
                                        break;
                                    case PlayerController.Face.Right:
                                        Instantiate(WalkThrowExplosionBottle, this.transform.position + RWalkThrowItemAppear, Quaternion.identity);
                                        break;
                                }
                                _itemManage.ExplosionBottleNumber -= 1;
                                break;
                        }
                        FirstTrigger = true;
                    }
                    if(AtkTimer <= 0)
                    {
                        isWalkThrow = false;
                        TimerSwitch = false;
                        AtkTimer = 0;
                        WalkThrowFaceRight = false;
                        WalkThrowFaceLeft = false;
                        FirstTrigger = false;
                    }
                }

            }
            if (isJumpThrow)
            {
                if (AtkTimer <= 0)
                {
                    AtkTimer = ThrowTimerSet;
                }

                AtkTimer -= _fixedDeltaTime;

                if (AtkTimer<=(ThrowTimerSet - 0.2))
                {
                    if (!FirstTrigger)
                    {
                        ThrowItemPowerX = JumpThrowCocktailPowerX;
                        ThrowItemPowerY = JumpThrowCocktailPowerY;
                        switch (_itemManage.NowPrepareItem)
                        {
                            case itemManage.PrepareItem.Cocktail:
                                switch (PlayerController.face)
                                {
                                    case PlayerController.Face.Right:
                                        Instantiate(CockTail, new Vector3(this.transform.position.x + RJumpThrowItemAppear.x, this.transform.position.y + RJumpThrowItemAppear.y, 0), Quaternion.identity);
                                        break;
                                    case PlayerController.Face.Left:
                                        Instantiate(CockTail, new Vector3(this.transform.position.x + LJumpThrowItemAppear.x, this.transform.position.y + LJumpThrowItemAppear.y, 0), Quaternion.identity);
                                        break;
                                }
                                _itemManage.CocktailNumber -= 1;
                                break;
                            case itemManage.PrepareItem.ExplosionBottle:
                                switch (PlayerController.face)
                                {
                                    case PlayerController.Face.Right:
                                        Instantiate(ExplosionBottle, new Vector3(this.transform.position.x + RJumpThrowItemAppear.x, this.transform.position.y + RJumpThrowItemAppear.y, 0), Quaternion.identity);
                                        break;
                                    case PlayerController.Face.Left:
                                        Instantiate(ExplosionBottle, new Vector3(this.transform.position.x + LJumpThrowItemAppear.x, this.transform.position.y + LJumpThrowItemAppear.y, 0), Quaternion.identity);
                                        break;
                                }
                                _itemManage.ExplosionBottleNumber -= 1;
                                break;
                        }
                        FirstTrigger = true;
                    }
                    if (AtkTimer <= 0)
                    {
                        _playerController.CantDoAnyThing = false;
                        _playerController.OnlyCanMove = false;
                        FirstTrigger = false;
                        isJumpThrow = false;
                        TimerSwitch = false;
                        AtkTimer = CriticAtkTimerSet;
                    }
                }
            }
            if (isCocktailCriticAtk)
            {
                if (PlayerController.isHurted)
                {
                    isCocktailCriticAtk = false;
                    TimerSwitch = false;
                    FirstTrigger = false;
                    return;
                }
                if (AtkTimer <= 0)
                {
                    AtkTimer = CocktailCriticAtkTimerSet;
                }

                AtkTimer -= _fixedDeltaTime;

                if (AtkTimer <= CocktailCriticAtkTimerSet - 0.9f)
                {
                    if (!FirstTrigger)
                    {
                        switch (PlayerController.face)
                        {
                            case PlayerController.Face.Right:
                                Instantiate(RCocktailCriticAtk, this.transform.position, Quaternion.identity);
                                break;
                            case PlayerController.Face.Left:
                                Instantiate(LCocktailCriticAtk, this.transform.position, Quaternion.identity);
                                break;
                        }
                        FirstTrigger = true;
                    }
                    if (AtkTimer <= 0)
                    {
                        isCocktailCriticAtk = false;
                        TimerSwitch = false;
                        _playerController.CantDoAnyThing = false;
                        FirstTrigger = false;
                    }
                }
            }
            if (isSharpen)
            {
                if (AtkTimer <= 0)
                {
                    AtkTimer = SharpenBladeTimerSet;
                }

                if (PlayerController.isHurted || _playerController.isDash || GameEvent.isAniPlay)
                {
                    isSharpen = false;
                    TimerSwitch = false;
                    FirstTrigger = false;
                    AtkTimer = 0;
                    _playerController.CantDoAnyThing = false;
                    return;
                }

                AtkTimer -= _fixedDeltaTime;
                
                if (AtkTimer <= (SharpenBladeTimerSet - 2.5f))
                {
                    if (!FirstTrigger)
                    {
                        SharpBladeSuccess();
                        FirstTrigger = true;
                    }
                }
                if(AtkTimer <= 0)
                {
                    isSharpen = false;
                    TimerSwitch = false;
                    FirstTrigger = false;
                    _playerController.CantDoAnyThing = false;
                }
            }
            if (isUsingRitualSword)
            {
                if (AtkTimer <= 0)
                {
                    AtkTimer = UseRitualSwordTimerSet;
                }

                if (PlayerController.isHurted || _playerController.isDash || GameEvent.isAniPlay)
                {
                    isUsingRitualSword = false;
                    TimerSwitch = false;
                    FirstTrigger = false;
                    AtkTimer = 0;
                    _playerController.CantDoAnyThing = false;
                    return;
                }

                AtkTimer -= _fixedDeltaTime;

                if (AtkTimer <= (UseRitualSwordTimerSet - 1.1f))
                {
                    if (!FirstTrigger)
                    {
                        UseRitualSwordSuccess();
                        FirstTrigger = true;
                    }
                }
                if (AtkTimer <= 0)
                {
                    isUsingRitualSword = false;
                    TimerSwitch = false;
                    FirstTrigger = false;
                    _playerController.CantDoAnyThing = false;
                }
            }
        }
    }

    private void AtkSwitchTimerMethod()
    {
        if (AtkSwitchTimerSwitch)
        {
            CanSecondAtk = true;
            AtkSwitchTimer -= _deltaTime;
            if (AtkSwitchTimer <= 0)
            {
                AtkSwitchTimer = AtkSwitchTimerSet;
                AtkSwitchTimerSwitch = false;
            }
        }
        else
        {
            if (!isSecondAtk)
            {
                CanSecondAtk = false;
            }
            AtkSwitchTimer = AtkSwitchTimerSet;
        }
    }

    private void AccumulateTimerMethod()
    {
        if (AccumulateTimerSwitch)
        {
            AccumulateTimer -= _deltaTime;
            if (AccumulateTimer <= 0 && GameEvent.TutorialComplete)
            {
                isAccumulateComplete = true;
            }
            if (PlayerController.isHurted)
            {
                isAccumulate = false;
                _playerController.CantDoAnyThing = false;
                AccumulateTimerSwitch = false;
                isAccumulateComplete = false;
            }
        }
        else
        {
            AccumulateTimer = AccumulateTimerSet;
        }
    }

    public void CocktailJudgeSystem()
    {
        if (!_playerController.CantDoAnyThing && PlayerController.isGround)
        {
            switch (_itemManage.NowPrepareItem)
            {
                case itemManage.PrepareItem.Cocktail:
                    if (_itemManage.CocktailNumber > 0)
                    {
                        if (isAccumulateComplete && SkillPower >= 300)
                        {
                            _itemManage.CocktailNumber -= 1;
                            DecreaseTimes += 10;
                            isCocktailCriticAtk = true;
                            _playerController.CantDoAnyThing = true;
                            TimerSwitch = true;
                            isAccumulate = false;
                            AccumulateTimerSwitch = false;
                            isAccumulateComplete = false;
                        }
                        if (CanWalkThrow)
                        {
                            isWalkThrow = true;
                            CanWalkThrow = false;
                            TimerSwitch = true;
                            switch (PlayerController.face)
                            {
                                case PlayerController.Face.Left:
                                    WalkThrowFaceLeft = true;
                                    break;
                                case PlayerController.Face.Right:
                                    WalkThrowFaceRight = true;
                                    break;
                            }
                        }
                        if (!isWalkThrow && !isCocktailCriticAtk)
                        {
                            isAim = true;
                            _playerController.CantDoAnyThing = true;
                            AimSystem();
                        }
                    }
                    break;
                case itemManage.PrepareItem.ExplosionBottle:
                    if (_itemManage.ExplosionBottleNumber > 0)
                    {
                        if (isAccumulateComplete && SkillPower >= 300)
                        {
                            _itemManage.ExplosionBottleNumber -= 1;
                            DecreaseTimes += 10;
                            _playerController.CantDoAnyThing = true;
                            isImpulseJump = true;
                            isAccumulate = false;
                            AccumulateTimerSwitch = false;
                            isAccumulateComplete = false;
                        }
                        if (CanWalkThrow)
                        {
                            isWalkThrow = true;
                            CanWalkThrow = false;
                            TimerSwitch = true;
                            switch (PlayerController.face)
                            {
                                case PlayerController.Face.Left:
                                    WalkThrowFaceLeft = true;
                                    break;
                                case PlayerController.Face.Right:
                                    WalkThrowFaceRight = true;
                                    break;
                            }
                        }
                        if (!isWalkThrow && !isImpulseJump)
                        {
                            isAim = true;
                            _playerController.CantDoAnyThing = true;
                            AimSystem();
                        }
                    }
                    break;
            }
        }
        if (!_playerController.CantDoAnyThing && !PlayerController.isGround)
        {
            switch (_itemManage.NowPrepareItem)
            {
                case itemManage.PrepareItem.Cocktail:
                    if (_itemManage.CocktailNumber > 0)
                    {
                        _playerController.CantDoAnyThing = true;
                        _playerController.OnlyCanMove = true;
                        TimerSwitch = true;
                        isJumpThrow = true;
                    }
                    break;
                case itemManage.PrepareItem.ExplosionBottle:
                    if (_itemManage.ExplosionBottleNumber > 0)
                    {
                        if (isAccumulateComplete && SkillPower >= 300)
                        {
                            _itemManage.ExplosionBottleNumber -= 1;
                            DecreaseTimes += 10;
                            _playerController.CantDoAnyThing = true;
                            isImpulseJump = true;
                            isAccumulate = false;
                            AccumulateTimerSwitch = false;
                            isAccumulateComplete = false;
                        }
                        if (!isImpulseJump)
                        {
                            _playerController.CantDoAnyThing = true;
                            _playerController.OnlyCanMove = true;
                            TimerSwitch = true;
                            isJumpThrow = true;
                        }
                    }
                    break;
            }
        }
    }

    private void AimSystem()
    {
        if (!HasAimAppear)
        {
            switch (PlayerController.face)
            {
                case PlayerController.Face.Left:
                    LPredictPowerBase.SetActive(true);
                    LPowerLine.SetActive(true);
                    LAimPoint.SetActive(true);
                    LAimPoint.transform.localPosition = LAimAppear;
                    LAimPoint.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                    LPredictPowerBase.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                    LPowerLine.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                    break;
                case PlayerController.Face.Right:
                    RPredictPowerBase.SetActive(true);
                    RPowerLine.SetActive(true);
                    RAimPoint.SetActive(true);
                    RAimPoint.transform.localPosition = RAimAppear;
                    RAimPoint.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                    RPredictPowerBase.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                    RPowerLine.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                    break;
            }
            HasAimAppear = true;
        }
    }

    public void AimThrowSystem()
    {
        if (isAim)
        {
            switch (PlayerController.face)
            {
                case PlayerController.Face.Left:
                    ThrowItemPowerX = LAimPoint.GetComponent<AimPowerController>().PowerX;
                    ThrowItemPowerY = LAimPoint.GetComponent<AimPowerController>().PowerY;
                    break;
                case PlayerController.Face.Right:
                    ThrowItemPowerX = RAimPoint.GetComponent<AimPowerController>().PowerX;
                    ThrowItemPowerY = RAimPoint.GetComponent<AimPowerController>().PowerY;
                    break;
            }
            HasAimAppear = false;
            isAim = false;
            switch (_itemManage.NowPrepareItem)
            {
                case itemManage.PrepareItem.Cocktail:
                    _itemManage.CocktailNumber -= 1;
                    break;
                case itemManage.PrepareItem.ExplosionBottle:
                    _itemManage.ExplosionBottleNumber -= 1;
                    break;
            }
            isThrowing = true;
            TimerSwitch = true;
            AtkLastTime = _time;
        }
    }

    public void BeginSharpenBlade()
    {
        if (!isSharpen)
        {
            isSharpen = true;
            TimerSwitch = true;
            _playerController.CantDoAnyThing = true;
        }
    }

    private void SharpBladeSuccess()//(1)
    {
        SharpTimeLeft = SharpTimeSet;

        if (isAtkBuff)
        {
            return;
        }

        NormalAtkHurtPower *= SharpenerRate;
        CAtkHurtPower *= SharpenerRate;
        CriticAtkHurtPower *= SharpenerRate;
        BlockNormalAtkHurtPower *= SharpenerRate;
        BlockStrongAtkHurtPower *= SharpenerRate;
        isAtkBuff = true;
    }

    public void BeginUseRitualSword()
    {
        if(!itemManage.CheckItemExist(ItemID.UnDeadSnake) && !itemManage.CheckItemExist(ItemID.RebornSnake))
        {
            return;
        }

        if (!isUsingRitualSword)
        {
            isUsingRitualSword = true;
            TimerSwitch = true;
            _playerController.CantDoAnyThing = true;
        }
    }

    private void UseRitualSwordSuccess()//(1)
    {
        InhibitTimeLeft = InhibitTimeSet;

        if (isInhibit)
        {
            return;
        }
        isInhibit = true;
    }

    private void ResetAtkPower()//(1) 重置攻擊力
    {
        NormalAtkHurtPower = NormalAtkHurtPowerSet;
        CAtkHurtPower = CAtkHurtPowerSet;
        BulletHurtPower = BulletHurtPowerSet;
        ExplosionHurtPower = ExplosionHurtPowerSet;
        CocktailHurtPower = CocktailHurtPowerSet;
        CriticAtkHurtPower = CriticAtkHurtPowerSet;
        BlockNormalAtkHurtPower = BlockNormalAtkHurtPowerSet;
        BlockStrongAtkHurtPower = BlockStrongAtkHurtPowerSet;
        BigGunPower = BigGunPowerSet;
    }

    public void ShootSystem()
    {
        if (!_playerController.CantDoAnyThing)
        {
            if (GameEvent.AbsorbBoss1 && PlayerController.isGround)
            {
                if (SkillPower >= 900)
                {
                    switch (PlayerController.face)
                    {
                        case PlayerController.Face.Left:
                            Instantiate(LBigGun, BigGunAppear.position, Quaternion.identity);
                            break;
                        case PlayerController.Face.Right:
                            Instantiate(RBigGun, BigGunAppear.position, Quaternion.identity);
                            break;
                    }
                    isBigGunProcess = true;
                    isShootAccumulate = true;
                    ShootAccumulateTimerSwitch = true;
                    _playerController.CantDoAnyThing = true;
                }
                else
                {
                    if (SkillPower >= 30)
                    {
                        switch (PlayerController.face)
                        {
                            case PlayerController.Face.Left:
                                Instantiate(Bullet, this.transform.position + LBulletAppear, Quaternion.identity);
                                break;
                            case PlayerController.Face.Right:
                                Instantiate(Bullet, this.transform.position + RBulletAppear, Quaternion.identity);
                                break;
                        }
                        DecreaseTimes += 1;
                        _playerController.canTurn = false;
                        isShooting = true;
                        shootTimerSwitch = true;
                        ShootingEndTimer = ShootingEndTimerSet;
                        ShootLastTime = _time;
                    }
                }
            }
            else
            {
                if ((ShootLastTime + ShootCoolDown) <= _time && SkillPower >= 30)
                {
                    if (PlayerController.isGround)
                    {
                        switch (PlayerController.face)
                        {
                            case PlayerController.Face.Left:
                                Instantiate(Bullet, this.transform.position + LBulletAppear, Quaternion.identity);
                                break;
                            case PlayerController.Face.Right:
                                Instantiate(Bullet, this.transform.position + RBulletAppear, Quaternion.identity);
                                break;
                        }
                    }
                    else
                    {
                        switch (PlayerController.face)
                        {
                            case PlayerController.Face.Left:
                                Instantiate(Bullet, this.transform.position + LBulletJumpAppear, Quaternion.identity);
                                break;
                            case PlayerController.Face.Right:
                                Instantiate(Bullet, this.transform.position + RBulletJumpAppear, Quaternion.identity);
                                break;
                        }
                    }
                    DecreaseTimes += 1;
                    _playerController.canTurn = false;
                    isShooting = true;
                    shootTimerSwitch = true;
                    ShootingEndTimer = ShootingEndTimerSet;
                    ShootLastTime = _time;
                }
            }
        }
    }

    public void ShootAccumulateEnd()
    {
        if (!isAllowBigGun && SkillPower >= 30 && isBigGunProcess)
        {
            isBigGunProcess = false;
            isShootAccumulate = false;
            ShootAccumulateTimerSwitch = false;
            _playerController.CantDoAnyThing = false;
            switch (PlayerController.face)
            {
                case PlayerController.Face.Left:
                    Instantiate(Bullet, this.transform.position + LBulletAppear, Quaternion.identity);
                    break;
                case PlayerController.Face.Right:
                    Instantiate(Bullet, this.transform.position + RBulletAppear, Quaternion.identity);
                    break;
            }
            DecreaseTimes += 1;
            _playerController.canTurn = false;
            isShooting = true;
            shootTimerSwitch = true;
            ShootingEndTimer = ShootingEndTimerSet;
            ShootLastTime = _time;
        }
    }

    private void ShootTimerMethod()
    {
        if (shootTimerSwitch)
        {
            ShootingEndTimer -= _deltaTime;
            if (ShootingEndTimer <= 0)
            {
                isShooting = false;
                shootTimerSwitch = false;
                _playerController.canTurn = true;
            }
            if (isAccumulate || isAtk || isCAtk || isJumpCAtk || _playerController.isDash || _playerController.isRestore || isAim || PlayerController.isHurted || GameEvent.isAniPlay)
            {
                _playerController.canTurn = true;
                isShooting = false;
                shootTimerSwitch = false;
            }
        }
        else
        {
            ShootingEndTimer = ShootingEndTimerSet;
        }
    }

    private void ShootAccumulateTimerMethod()
    {
        if (ShootAccumulateTimerSwitch)
        {
            ShootAccumulateTimer -= _deltaTime;
            if (ShootAccumulateTimer <= 0)
            {
                DecreaseTimes += 30;
                BigGunTimerSwitch = true;
                isShootAccumulate = false;
                isAllowBigGun = true;
                isBigGunShoot = true;
                ShootAccumulateTimerSwitch = false;
                ShootAccumulateTimer = ShootAccumulateTimerSet;
            }
            if (PlayerController.isHurted || GameEvent.isAniPlay)
            {
                isBigGunProcess = false;
                isShootAccumulate = false;
                _playerController.CantDoAnyThing = false;
                ShootAccumulateTimerSwitch = false;
                isAllowBigGun = false;
                ShootAccumulateTimer = ShootAccumulateTimerSet;
            }
        }
        else
        {
            ShootAccumulateTimer = ShootAccumulateTimerSet;
        }
    }

    private void BigGunTimerMethod()
    {
        if (BigGunTimerSwitch)
        {
            if (PlayerController.isHurted || GameEvent.isAniPlay)
            {
                isBigGunEnd = false;
                isBigGunProcess = false;
                isBigGunShoot = false;
                BigGunTimer = BigGunTimerSet;
                isAllowBigGun = false;
                BigGunTimerSwitch = false;
                FirstTrigger = false;
                _playerController.CantDoAnyThing = false;
                return;
            }
            BigGunTimer -= _deltaTime;
            if (BigGunTimer <= (BigGunTimerSet - 2.1))
            {
                isBigGunEnd = true;
                if (BigGunTimer <= 0)
                {
                    isBigGunEnd = false;
                    isBigGunProcess = false;
                    isBigGunShoot = false;
                    BigGunTimer = BigGunTimerSet;
                    isAllowBigGun = false;
                    BigGunTimerSwitch = false;
                    FirstTrigger = false;
                    _playerController.CantDoAnyThing = false;
                }
            }
        }
        else
        {
            BigGunTimer = BigGunTimerSet;
        }
    }

    private void BlockTimerMethod()
    {
        if (BlockTimerSwitch)
        {
            BlockTimer -= _deltaTime;
            if (isBlockSuccess)
            {
                FirstTrigger = false;
                isBlockEnd = false;
                BlockTimer = BlockTimerSet;
                BlockTimerSwitch = false;
                return;
            }
            if (PlayerController.isHurted || GameEvent.isAniPlay)
            {
                FirstTrigger = false;
                isBlock = false;
                isBlockEnd = false;
                BlockTimer = BlockTimerSet;
                BlockTimerSwitch = false;
                return;
            }
            if (BlockTimer <= (BlockTimerSet - 0.05))
            {
                if (!FirstTrigger)
                {
                    switch (PlayerController.face)
                    {
                        case PlayerController.Face.Left:
                            Instantiate(LBlock, this.transform.position, Quaternion.identity);
                            break;
                        case PlayerController.Face.Right:
                            Instantiate(RBlock, this.transform.position, Quaternion.identity);
                            break;
                    }
                    FirstTrigger = true;
                }
                if (BlockTimer <= (BlockTimerSet - 0.5))
                {
                    isBlockEnd = true;
                    if (BlockTimer <= 0)
                    {
                        isBlockEnd = false;
                        isBlock = false;
                        FirstTrigger = false;
                        _playerController.CantDoAnyThing = false;
                        BlockTimer = BlockTimerSet;
                        BlockTimerSwitch = false;
                    }
                }
            }
        }
        else
        {
            BlockTimer = BlockTimerSet;
        }
    }

    private void BlockAtkTimerMethod()
    {
        if (BlockAtkSwitch)
        {
            BlockAtkTimer -= _deltaTime;
            if (isBlockNormalAtk)
            {
                if (!FirstTrigger)
                {
                    switch (PlayerController.face)
                    {
                        case PlayerController.Face.Left:
                            Instantiate(LBlockNormalAtkAni, this.transform.position, Quaternion.identity);
                            break;
                        case PlayerController.Face.Right:
                            Instantiate(RBlockNormalAtkAni, this.transform.position, Quaternion.identity);
                            break;
                    }
                    FirstTrigger = true;
                }
                if (BlockAtkTimer <= (BlockAtkTimerSet - 1.15))
                {
                    if (!SecondTrigger)
                    {
                        switch (PlayerController.face)
                        {
                            case PlayerController.Face.Left:
                                Instantiate(LBlockNormalAtk, this.transform.position, Quaternion.identity);
                                break;
                            case PlayerController.Face.Right:
                                Instantiate(RBlockNormalAtk, this.transform.position, Quaternion.identity);
                                break;
                        }
                        SecondTrigger = true;
                    }
                    if (BlockAtkTimer <= (BlockAtkTimerSet - 1.6))
                    {
                        BlockAtkSwitch = false;
                        FirstTrigger = false;
                        SecondTrigger = false;
                        isBlockNormalAtk = false;
                        isBlock = false;
                        _playerController.CantDoAnyThing = false;
                        BlockAtkTimer = BlockAtkTimerSet;
                        _playerController.isBlockAtkInvincible = true;
                    }
                }
            }
            if (isBlockStrongAtk)
            {
                if (BlockAtkTimer <= (BlockAtkTimerSet - 1.55))
                {
                    if (!FirstTrigger)
                    {
                        switch (PlayerController.face)
                        {
                            case PlayerController.Face.Left:
                                Instantiate(LBlockStrongAtk, this.transform.position, Quaternion.identity);
                                break;
                            case PlayerController.Face.Right:
                                Instantiate(RBlockStrongAtk, this.transform.position, Quaternion.identity);
                                break;
                        }
                        FirstTrigger = true;
                    }
                    if (BlockAtkTimer <= (BlockAtkTimerSet - 1.9))
                    {
                        BlockAtkSwitch = false;
                        FirstTrigger = false;
                        isBlockStrongAtk = false;
                        isBlock = false;
                        _playerController.CantDoAnyThing = false;
                        BlockAtkTimer = BlockAtkTimerSet;
                        _playerController.isBlockAtkInvincible = true;
                    }
                }
            }
        }
        else
        {
            BlockAtkTimer = BlockAtkTimerSet;
        }
    }

    private void IncreaseSkillPower()
    {
        if (IncreaseTimes > 0)
        {
            SkillPower += 10;
            IncreaseTimes -= 1;
        }
    }

    private void DecreaseSkillPower()
    {
        if (DecreaseTimes > 0)
        {
            SkillPower -= 30;
            DecreaseTimes -= 1;
        }
    }

    public void BlockSystem()
    {
        if(!_playerController.CantDoAnyThing && PlayerController.isGround)
        {
            isBlock = true;
            BlockTimerSwitch = true;
            _playerController.CantDoAnyThing = true;
        }
    }

    public void BlockSuccessTimerMethod()
    {
        if (isBlockSuccess)
        {
            if (PlayerController.isHurted || GameEvent.isAniPlay)
            {
                isBlock = false;
                isBlockEnd = false;
                BackgroundSystem.CantPause = false;
                BackgroundSystem.GameSpeed = 1;
                isBlockSuccess = false;
                BlockPrepareAtkTimer = BlockPrepareAtkTimerSet;
                return;
            }
            BlockPrepareAtkTimer -= _UnScaleDeltaTime;
            if(BlockPrepareAtkTimer <= (BlockPrepareAtkTimerSet - 0.5))
            {
                BackgroundSystem.GameSpeed = 1;
                BackgroundSystem.CantPause = true;
                if (isBlockNormalAtk || isBlockStrongAtk)
                {
                    BlockAtkBegin();
                }
            }
            if (BlockPrepareAtkTimer <= (BlockPrepareAtkTimerSet - 0.8))
            {
                isBlockEnd = true;
                isBlockSuccessWait = false;
            }
            if (BlockPrepareAtkTimer <= 0)
            {
                isBlock = false;
                isBlockEnd = false;
                _playerController.CantDoAnyThing = false;
                isBlockSuccess = false;
                BlockPrepareAtkTimer = BlockPrepareAtkTimerSet;
            }
        }
        else
        {
            BlockPrepareAtkTimer = BlockPrepareAtkTimerSet;
        }
    }

    private void BlockAtk(int Ver)
    {
        switch (Ver)
        {
            case 1:
                isBlockNormalAtk = true;
                break;
            case 2:
                if (SkillPower >= 300)
                {
                    DecreaseTimes += 10;
                    isBlockStrongAtk = true;
                }
                break;
        }
    }

    private void BlockAtkBegin()
    {
        isBlockSuccess = false;
        isBlockSuccessWait = false;
        BlockAtkSwitch = true;
    }

    private void BeBlockTimerMethod()
    {
        if (isBeBlockSuccess)
        {
            if (PlayerController.isHurted || GameEvent.isAniPlay)
            {
                _playerController.CantDoAnyThing = false;
                isBeBlockSuccess = false;
                BeBlockTimer = BeBlockTimerSet;
                isBeBlockJudgement = false;
                return;
            }
            _playerController.CantDoAnyThing = true;
            if (!isBeBlockJudgement)
            {
                if (PlayerController.isGround)
                {
                    BeBlockisGround = true;
                }
                else
                {
                    BeBlockisGround = false;
                }
                isBeBlockJudgement = true;
            }
            if (BeBlockisGround)
            {
                BeBlockTimer -= _deltaTime;
                if (BeBlockTimer <= (BeBlockTimerSet - 0.03))//因為需要有延遲才寫在這
                {
                    FirstTrigger = false;
                    isAtk = false;
                    isJumpAtkJudgement = false;
                    TimerSwitch = false;
                }
                if (BeBlockTimer <= (BeBlockTimerSet - 2))
                {
                    _playerController.CantDoAnyThing = false;
                    isBeBlockSuccess = false;
                    BeBlockTimer = BeBlockTimerSet;
                    isBeBlockJudgement = false;
                }
            }
            else
            {
                if (PlayerController.isGround)
                {
                    BeBlockTimer -= Time.deltaTime;
                    if (BeBlockTimer <= 0)
                    {
                        _playerController.CantDoAnyThing = false;
                        isBeBlockSuccess = false;
                        BeBlockTimer = BeBlockTimerSet;
                        isBeBlockJudgement = false;
                    }
                }
            }
        }
        else
        {
            BeBlockTimer = BeBlockTimerSet;
        }
    }

    private void WeakTimerMethod()
    {
        if(WeakTimerSwitch)
        {
            if (PlayerController.isHurted || GameEvent.isAniPlay)
            {
                isWeak = false;
                WeakTimerSwitch = false;
                _playerController.CantDoAnyThing = false;
                WeakTimer = WeakTimerSet;
            }
            WeakTimer -= _deltaTime;
            _playerController.CantDoAnyThing = true;
            if (WeakTimer <= 0)
            {
                isWeak = false;
                WeakTimerSwitch = false;
                _playerController.CantDoAnyThing = false;
                WeakTimer = WeakTimerSet;
            }
        }
        else
        {
            WeakTimer = WeakTimerSet;
        }
    }

    private void ImpulseJumpTimerMethod()
    {
        if (isImpulseJump)
        {
            ImpulseJumpTimer -= _fixedDeltaTime;
            if (!FirstTrigger)
            {
                Rigid2D.drag = 5;
                switch (PlayerController.face)
                {
                    case PlayerController.Face.Right:
                        Instantiate(ImpulseJumpExplosion, this.transform.position + RImpulseExplosionAppear, Quaternion.identity);
                        Rigid2D.AddForce(new Vector2(ImpulseJumpPowerX, ImpulseJumpPowerY), ForceMode2D.Impulse);
                        break;
                    case PlayerController.Face.Left:
                        Instantiate(ImpulseJumpExplosion, this.transform.position + LImpulseExplosionAppear, Quaternion.identity);
                        Rigid2D.AddForce(new Vector2(-ImpulseJumpPowerX, ImpulseJumpPowerY), ForceMode2D.Impulse);
                        break;
                }
                FirstTrigger = true;
            }
            if (ImpulseJumpTimer <= (ImpulseJumpTimerSet - 0.1))
            {
                _playerController.CantDoAnyThing = false;
            }
            if (ImpulseJumpTimer <= 0)
            {
                isImpulseJump = false;
                FirstTrigger = false;
                ImpulseJumpTimer = ImpulseJumpTimerSet;
            }
        }
        else
        {
            ImpulseJumpTimer = ImpulseJumpTimerSet;
        }
    }

    private void JudgeBuff()
    {
        bool defend = false;
        bool SealPower = false;
        if (itemManage.CheckItemExist(ItemID.UnDeadSnake) && !isInhibit)
        {
            defend = true;
            SealPower = true;
        }
        isDefendBuff = defend;
        isPowerSeal = SealPower;

    }//(2)判斷適用哪一個Buff

    //大招位置計算
    private Vector3 CalculateCriticAtkPosition()
    {
        float PositionX = 0;
        float PositionY = 0;

        PositionX = (_transform.position.x + _criticAtkPredictObject.position.x) / 2;
        PositionY = (_transform.position.y + _criticAtkPredictObject.position.y) / 2;

        return new Vector3(PositionX, PositionY, 0);
    }

    private void KillerPointControll()
    {
        //K值
        if (!_playerController.isCheat)
        {
            if ((KillerPoint / MaxKillerPoint) >= 0.25 && (KillerPoint / MaxKillerPoint) < 0.75)
            {
                IncresePlayerPowerNumber = 15;
            }
            if ((KillerPoint / MaxKillerPoint) >= 0.5 && (KillerPoint / MaxKillerPoint) < 1)
            {
                NormalAtkHurtPower = 15;
                CAtkHurtPower = 30;
                CriticAtkHurtPower = 90;
                BlockNormalAtkHurtPower = 30;
                BlockStrongAtkHurtPower = 60;
                BigGunPower = 2.25f;
            }
            if ((KillerPoint / MaxKillerPoint) >= 0.75)
            {
                IncresePlayerPowerNumber = 20;
            }
            if ((KillerPoint / MaxKillerPoint) == 1)
            {
                NormalAtkHurtPower = 20;
                CAtkHurtPower = 40;
                CriticAtkHurtPower = 135;
                BlockNormalAtkHurtPower = 40;
                BlockStrongAtkHurtPower = 80;
                BigGunPower = 3f;
            }
        }
        if (KillerPoint > MaxKillerPoint)
        {
            KillerPoint = MaxKillerPoint;
        }
    }
}
