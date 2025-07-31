using UnityEngine;
using static Creature;

public class BattleSystem : MonoBehaviour
{
    private Transform _transform;
    private PlayerController _playerController;
    public ItemManage _itemManage;
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

    [HideInInspector] public int ShootCost = 30;
    [HideInInspector] public int StrongAtkCost = 300;
    [HideInInspector] public int CriticAtkCost = 600;

    public static float NormalAtkHurtPower;//�Ǫ�script���|�Ψ�(enemyEscape)
    public static float CAtkHurtPower;//�Ǫ�script���|�Ψ�(enemyEscape)
    public static float BulletHurtPower ;//�Ǫ�script���|�Ψ�(enemyEscape)
    public static float ExplosionHurtPower;//�Ǫ�script���|�Ψ�(enemyEscape)
    public static float CocktailHurtPower;//�Ǫ�script���|�Ψ�(enemyEscape)
    public static float CriticAtkHurtPower;//�Ǫ�script���|�Ψ�(enemyEscape)
    public static float BlockNormalAtkHurtPower;//�Ǫ�script���|�Ψ�
    public static float BlockStrongAtkHurtPower;//�Ǫ�script���|�Ψ�
    public static float BigGunPower;//�Ǫ�script���|�Ψ�
    public static int IncresePlayerPowerNumber = 10;//���𥴨�ɼW�[����q(�H�������)  �Ǫ�script���|�Ψ� (enemyEscape)

    private Rigidbody2D Rigid2D;
    private SlopeControll _slopeControll;
    private Transform TemporaryArea;
    [HideInInspector] public int NowMaxSkillPower = 900;//���Q��Lscript�Ψ�(playerController�AplayerspecialAni)
    [HideInInspector] public int SkillPower;//���Q��Lscript�Ψ�(playerController�APowerUI�AplayerspecialAni)
    [HideInInspector] public int TrueMaxSkillPower = 900;//UI�]�O������
    public static int KillerPoint;
    [HideInInspector] public int MaxKillerPoint = 200;
    private float AtkCoolDown = 0.2f;
    private float AtkLastTime = -10;
    public static int DecreaseTimes = 0;//���Q��Lscript�Ψ�(Training2Controller)
    public static int IncreaseTimes = 0;//���Q��Lscript�Ψ�(�Ǫ�script�A(enemyEscape)) 
    [HideInInspector] public float ThrowItemPowerX;//�������Y�����O��  ���Q��Lscript�Ψ�(ThrowItem)
    [HideInInspector] public float ThrowItemPowerY;//�������Y�����O��  ���Q��Lscript�Ψ�(ThrowItem)
    private float ShootLastTime;
    private float ShootCoolDown = 0.05f;
    [HideInInspector] public (float, float) InisialAimPower = (5, 3);
    [HideInInspector] public float DebugAimSpeed = 15;
    [HideInInspector] public float AimLimit = 10;
    [HideInInspector] public (float, float) WalkThrowCocktailPower = (200, 100);
    [HideInInspector] public (float, float) JumpThrowCocktailPower = (200, -100);
    private float ImpulseJumpPowerX = 800;
    private float ImpulseJumpPowerY = 600;
    private float SharpenerRate = 1.3f;
    private float _deltaTime;
    private float _UnScaleDeltaTime;
    private float _time;
    private float _fixedDeltaTime;

    //TimerSet
    [HideInInspector] public float AtkTimerSet = 5;
    [HideInInspector] public float CAtkTimerSet = 0.5f;
    [HideInInspector] public float JumpCAtkTimerSet = 0.55f;
    [HideInInspector] public float ThrowTimerSet = 0.5f;
    private float CocktailCriticAtkTimerSet = 1.6f;
    private float CriticAtkTimerSet = 3.25f;
    private float AtkTimer;
    private float AtkSwitchTimerSet = 2;
    private float AtkSwitchTimer;
    [HideInInspector] public float AccumulateTimerSet = 1;
    [HideInInspector] public float ShootingEndTimerSet = 0.3f;
    private float ShootAccumulateTimer;
    private float ShootAccumulateTimerSet = 2f;
    private float BigGunTimer;
    private float BigGunTimerSet = 2.35f;
    private float ImpulseJumpTimer;
    private float ImpulseJumpTimerSet = 0.5f;
    [HideInInspector] public float BlockTimerSet = 1.25f;
    private float BlockTimer;
    private float BlockPrepareAtkTimerSet = 0.5f;
    private float BlockPrepareAtkTimer;
    [HideInInspector] public float BlockAtkTimerSet = 1.9f;
    private float BlockAtkTimer;
    [HideInInspector] public float BeBlockTimerSet = 2.3f;
    private float BeBlockTimer;
    private float WeakTimerSet = 2.3f;
    private float WeakTimer;
    [HideInInspector] public float SharpenBladeTimerSet = 3.5f;
    [HideInInspector] public float UseRitualSwordTimerSet = 2.2f;
    [HideInInspector] public float SharpTimeSet = 61;//Buff����ɶ�
    [HideInInspector] public float InhibitTimeSet = 61;//Buff����ɶ�


    public static bool isBlockSuccessWait;//���ɦ��\�ᵥ�ݫ��O��  ���Q��Lscript�Ψ�(playerblockjudgement)
    public static bool isBeBlockSuccess;//���Q��Lscript�Ψ�(playerblockjudgement�AAtkController)
    [HideInInspector] public bool isBlockSuccess;//���ɦ��\�}��  ���Q��Lscript�Ψ�(playerBlockJudgement�AnormalmonsterAtk)
    [HideInInspector] public bool isAtk;//�O���O�b������ ���Q��Lscript�Ψ�(PlayerController)
    [HideInInspector] public bool isCAtk;//�O���O�bc������ ���Q��Lscript�Ψ�(PlayerController)
    [HideInInspector] public bool isCriticAtk;//�O���O�b�j�� ���Q��Lscript�Ψ�(playerController)
    [HideInInspector] public bool isThrowing;//�O���O�b���Y ���Q��Lscript�Ψ�(playerController)
    [HideInInspector] public bool isSharpen;//�O���O�b�i�M
    [HideInInspector] public bool isAtkBuff;//�M�O�_�O�W�Q���A
    [HideInInspector] public bool isDefendBuff;//�O�_�B�b���m�j�ƪ��A
    [HideInInspector] public bool isInhibit;//�O�_�B�b���A
    [HideInInspector] public bool isUsingRitualSword;//�O�_�b�ϥλ����M
    [HideInInspector] public bool isPowerSeal;//�O�_�B�b�]�O�ʦL���A
    [HideInInspector] public bool isAccumulate;//�O���O�b�W�O ���Q��Lscript�Ψ�(playerController)
    [HideInInspector] public bool AccumulateTimerSwitch;//���Q��Lscript�Ψ�(playerController)
    [HideInInspector] public bool isJumpCAtk;//�O���O�bc������ ���Q��Lscript�Ψ�(playerController)
    [HideInInspector] public bool isAccumulateComplete;//�ण��Τj�� ���Q��Lscript�Ψ�(playerController)
    [HideInInspector] public bool isCriticAtkChangeStage;//�j�۰ʵe�ഫ�}�� ���Q��Lscript�Ψ�(playerController)
    [HideInInspector] public bool isAim;//�O���O���b�˷�(�U�N�~) ���Q��Lscript�Ψ�(PredictPowerBase�AAimPowerController)
    [HideInInspector] public bool isShooting;//���Q��Lscript�Ψ�(playerController)
    [HideInInspector] public bool isShootAccumulate;//���Q��Lscript�Ψ�(playerController)
    [HideInInspector] public bool isBigGunShoot;//���Q��Lscript�Ψ�(playerController,BigGunController)
    [HideInInspector] public bool isBigGunProcess;//���Q��Lscript�Ψ�(playerController)
    [HideInInspector] public bool isBigGunEnd;//���Q��Lscript�Ψ�(playerController,BigGunController)
    [HideInInspector] public bool HasAimAppear = false;//���Q��Lscript�Ψ�(playerController)
    [HideInInspector] public bool isBlock;//�O�_�b���� ���Q��Lscript�Ψ�(playerController�Aatkcontroller)
    [HideInInspector] public bool isBlockEnd;//�S�����ɦ��\�����}��  ���Q��Lscript�Ψ�(playerController)
    [HideInInspector] public bool isBlockNormalAtk;//���Q��Lscript�Ψ�(playerController)
    [HideInInspector] public bool isBlockStrongAtk;//���Q��Lscript�Ψ�(playerController)
    [HideInInspector] public bool isJumpAtk;//�Ϥ��O�_������ ���Q��Lscript�Ψ�(playerController)
    [HideInInspector] public bool BeBlockisGround;//���Q��Lscript�Ψ�(playerController)
    [HideInInspector] public bool isBlockActualAppear;//���Q��Lscript�Ψ�(playerController�AplayerBlockJudgement)
    [HideInInspector] public bool BlockAtkSwitch;
    [HideInInspector] public bool isSecondAtk;//���Q��Lscript�Ψ�(playerController)
    [HideInInspector] public bool isCaptured;//���Q��Lscript�Ψ�(playerController�A����ު�monster)
    [HideInInspector] public bool isWeak;//���Q��Lscript�Ψ�(playerController)
    [HideInInspector] public bool WeakTimerSwitch;//���Q��Lscript�Ψ�(playerController)
    [HideInInspector] public bool CanWalkThrow;//���Q��Lscript�Ψ�(playerController)
    [HideInInspector] public bool isWalkThrow;//���Q��Lscript�Ψ�(playerController)
    [HideInInspector] public bool isJumpThrow;//���Q��Lscript�Ψ�(playerController)
    [HideInInspector] public bool isCocktailCriticAtk;//���Q��Lscript�Ψ�(playerController)
    [HideInInspector] public bool isImpulseJump;//���Q��Lscript�Ψ�(playerController)
    [HideInInspector] public bool CanAtk;
    [HideInInspector] public bool CanShoot;
    private bool TimerSwitch;
    private bool FirstTrigger = false;
    private bool SecondTrigger = false;
    private bool ThirdTrigger = false;
    private bool shootTimerSwitch;
    private bool BlockTimerSwitch;
    private bool isBeBlockJudgement;
    private bool ShootAccumulateTimerSwitch;
    private bool isAllowBigGun;
    private bool BigGunTimerSwitch;
    private bool ShouldCalculateAlterAtk;
    private bool CanSecondAtk;

    [Header("��������")]
    public GameObject Bullet;
    public GameObject NormalAtk;
    public GameObject AlterAtk;
    public GameObject CAtk;
    public GameObject JumpAtk;
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
    public GameObject JumpCAtk;
    public GameObject Block;
    public GameObject BlockNormalAtkAni;
    public GameObject BlockNormalAtk;
    public GameObject BlockStrongAtk;
    public GameObject RBigGun;
    public GameObject LBigGun;
    public GameObject RCocktailCriticAtk;
    public GameObject LCocktailCriticAtk;
    public GameObject ImpulseJumpExplosion;
    //�U�����������ӥX�{�a�I
    [HideInInspector] public Vector3 BulletAppear = new Vector3(1.237f, 0.533f, 0);
    [HideInInspector] public Vector3 BulletJumpAppear = new Vector3(1.441f, 0.516f, 0);
    [HideInInspector] public Vector3 ThrowItemAppear = new Vector3(0.8f, 1.21f, 0);
    [HideInInspector] public Vector3 WalkThrowItemAppear = new Vector3(0.3f, 1.07f, 0);
    [HideInInspector] public Vector3 JumpThrowItemAppear = new Vector3(0.859f, 0.332f, 0);
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
        
        //�N�o�ɶ�
        if ((AtkCoolDown + AtkLastTime) <= _time)
        {
            CanAtk = true;
        }
        if ((ShootLastTime + ShootCoolDown) <= _time)
        {
            CanShoot = true;
        }

        if (isBlockSuccess)
        {
            BlockPrepareAtkTimer -= _UnScaleDeltaTime;
            if (BlockPrepareAtkTimer <= 0)
            {
                StopBulletTime();
            }
        }

        CalculateAlterNormalAtk();

        BigGunTimerMethod();

        ShootAccumulateTimerMethod();

        BlockTimerMethod();

        BlockSuccessTimerMethod();

        BlockAtkTimerMethod();

        BeBlockTimerMethod();

        WeakTimerMethod();
    }

    private void FixedUpdate()
    {
        _fixedDeltaTime = Time.fixedDeltaTime;

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
                    //�a�W����
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
                    //�Ť�����
                    if (!PlayerController.isGround && !isJumpThrow && CanAtk && !_playerController.CantDoAnyThing)
                    {
                        _playerController.CantDoAnyThing = true;
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
                //�a�W����
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
                //�Ť�����
                if (!PlayerController.isGround && !isJumpThrow && CanAtk && !_playerController.CantDoAnyThing)
                {
                    _playerController.CantDoAnyThing = true;
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
                        switch (PlayerController._player.face)
                        {
                            case Face.Left:
                                Instantiate(LCriticAtk, CalculateCriticAtkPosition(), Quaternion.Euler(0, 0, CriticAtkPredictAngle));
                                break;
                            case Face.Right:
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
                            //�w�]�Z��15
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
                        switch (PlayerController._player.face)
                        {
                            case Face.Right:
                                Instantiate(RCocktailCriticAtk, this.transform.position, Quaternion.identity);
                                break;
                            case Face.Left:
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
        }
    }

    public void BeginAtkCoolDown()
    {
        CanAtk = false;
        AtkLastTime = _time;
    }
    public void BeginCalculateAlterAtk()
    {
        ShouldCalculateAlterAtk = true;
        AtkSwitchTimer = AtkSwitchTimerSet;
    }
    public void StopCalculateAlterAtk()
    {
        AtkSwitchTimer = 0;
        ShouldCalculateAlterAtk = false;
    }
    private void CalculateAlterNormalAtk()
    {
        if (ShouldCalculateAlterAtk)
        {
            AtkSwitchTimer -= _deltaTime;
            if (AtkSwitchTimer <= 0)
            {
                StopCalculateAlterAtk();
            }
        }
    }
    public bool JudgeUseAlterNormalAtk()
    {
        if(AtkSwitchTimer > 0)
        {
            return true;
        }

        return false;
    }

    public void SharpBladeSuccess()//(1)
    {
        NormalAtkHurtPower *= SharpenerRate;
        CAtkHurtPower *= SharpenerRate;
        CriticAtkHurtPower *= SharpenerRate;
        BlockNormalAtkHurtPower *= SharpenerRate;
        BlockStrongAtkHurtPower *= SharpenerRate;
    }
    public void ResetAtkPower()//(1) ���m�����O
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
    public void InhibitSuccess()//(1)
    {
        isInhibit = true;
    }
    public void InhibitEnd()//(1)
    {
        isInhibit = false;
    }

    public void BeginShootCooldown()
    {
        CanShoot = false;
        ShootLastTime = _time;
    }


    /*public void ShootSystem()
    {
        if (!_playerController.CantDoAnyThing)
        {
            if (GameEvent.AbsorbBoss1 && PlayerController.isGround)
            {
                if (SkillPower >= 900)
                {
                    switch (PlayerController._player.face)
                    {
                        case Face.Left:
                            Instantiate(LBigGun, BigGunAppear.position, Quaternion.identity);
                            break;
                        case Face.Right:
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
                        switch (PlayerController._player.face)
                        {
                            case Face.Left:
                                Instantiate(Bullet, this.transform.position + LBulletAppear, Quaternion.identity);
                                break;
                            case Face.Right:
                                Instantiate(Bullet, this.transform.position + RBulletAppear, Quaternion.identity);
                                break;
                        }
                        DecreaseTimes += 1;
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
                        switch (PlayerController._player.face)
                        {
                            case Face.Left:
                                Instantiate(Bullet, this.transform.position + LBulletAppear, Quaternion.identity);
                                break;
                            case Face.Right:
                                Instantiate(Bullet, this.transform.position + RBulletAppear, Quaternion.identity);
                                break;
                        }
                    }
                    else
                    {
                        switch (PlayerController._player.face)
                        {
                            case Face.Left:
                                Instantiate(Bullet, this.transform.position + LBulletJumpAppear, Quaternion.identity);
                                break;
                            case Face.Right:
                                Instantiate(Bullet, this.transform.position + RBulletJumpAppear, Quaternion.identity);
                                break;
                        }
                    }
                    DecreaseTimes += 1;
                    isShooting = true;
                    shootTimerSwitch = true;
                    ShootingEndTimer = ShootingEndTimerSet;
                    ShootLastTime = _time;
                }
            }
        }
    }*/
    public void ShootAccumulateEnd()
    {
        if (!isAllowBigGun && SkillPower >= 30 && isBigGunProcess)
        {
            /*isBigGunProcess = false;
            isShootAccumulate = false;
            ShootAccumulateTimerSwitch = false;
            _playerController.CantDoAnyThing = false;
            switch (PlayerController._player.face)
            {
                case Face.Left:
                    Instantiate(Bullet, this.transform.position + LBulletAppear, Quaternion.identity);
                    break;
                case Face.Right:
                    Instantiate(Bullet, this.transform.position + RBulletAppear, Quaternion.identity);
                    break;
            }
            DecreaseTimes += 1;
            isShooting = true;
            shootTimerSwitch = true;
            ShootingEndTimer = ShootingEndTimerSet;
            ShootLastTime = _time;*/
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

    public void BeginBulletTime()
    {
        isBlockSuccess = true;
        BackgroundSystem.CantPause = true;
        BackgroundSystem.GameSpeed = 0;
        isBlockActualAppear = false;

        BlockPrepareAtkTimer = BlockPrepareAtkTimerSet;
    }
    public void StopBulletTime()
    {
        BackgroundSystem.GameSpeed = 1;
        BackgroundSystem.CantPause = true;
        isBlockSuccess = false;
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
                    switch (PlayerController._player.face)
                    {
                        case Face.Left:
                            //Instantiate(LBlock, this.transform.position, Quaternion.identity);
                            break;
                        case Face.Right:
                            //Instantiate(RBlock, this.transform.position, Quaternion.identity);
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
                    switch (PlayerController._player.face)
                    {
                        case Face.Left:
                            //Instantiate(LBlockNormalAtkAni, this.transform.position, Quaternion.identity);
                            break;
                        case Face.Right:
                            //Instantiate(RBlockNormalAtkAni, this.transform.position, Quaternion.identity);
                            break;
                    }
                    FirstTrigger = true;
                }
                if (BlockAtkTimer <= (BlockAtkTimerSet - 1.15))
                {
                    if (!SecondTrigger)
                    {
                        switch (PlayerController._player.face)
                        {
                            case Face.Left:
                               // Instantiate(LBlockNormalAtk, this.transform.position, Quaternion.identity);
                                break;
                            case Face.Right:
                                //Instantiate(RBlockNormalAtk, this.transform.position, Quaternion.identity);
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
                        switch (PlayerController._player.face)
                        {
                            case Face.Left:
                                //Instantiate(LBlockStrongAtk, this.transform.position, Quaternion.identity);
                                break;
                            case Face.Right:
                                //Instantiate(RBlockStrongAtk, this.transform.position, Quaternion.identity);
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

    public void UseSkillPower(int Cost)
    {
        DecreaseTimes += (Cost / 30);
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
                if (BeBlockTimer <= (BeBlockTimerSet - 0.03))//�]���ݭn������~�g�b�o
                {
                    FirstTrigger = false;
                    isAtk = false;
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
                switch (PlayerController._player.face)
                {
                    case Face.Right:
                        Instantiate(ImpulseJumpExplosion, this.transform.position + RImpulseExplosionAppear, Quaternion.identity);
                        Rigid2D.AddForce(new Vector2(ImpulseJumpPowerX, ImpulseJumpPowerY), ForceMode2D.Impulse);
                        break;
                    case Face.Left:
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
        if (ItemManage.CheckItemExist(ItemID.UnDeadSnake) && !isInhibit)
        {
            defend = true;
            SealPower = true;
        }
        isDefendBuff = defend;
        isPowerSeal = SealPower;

    }//(2)�P�_�A�έ��@��Buff

    //�j�ۦ�m�p��
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
        //K��
        if (!GameEvent.OpenCheat)
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
