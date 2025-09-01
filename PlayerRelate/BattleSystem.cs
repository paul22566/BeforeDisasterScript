using System;
using System.Threading.Tasks;
using UnityEngine;
using static Creature;

public class BattleSystem : MonoBehaviour
{
    private Transform _transform;
    private PlayerController _playerController;
    public ItemManage _itemManage;

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
    [HideInInspector] public Vector2 ImpulseJumpPower = new Vector2(800, 600);
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
    [HideInInspector] public float CocktailCriticAtkTimerSet = 1.6f;
    [HideInInspector] public float CriticAtkTimerSet = 3.25f;
    private float AtkSwitchTimerSet = 2;
    private float AtkSwitchTimer;
    [HideInInspector] public float AccumulateTimerSet = 1;
    [HideInInspector] public float ShootingEndTimerSet = 0.3f;
    private float ShootAccumulateTimer;
    private float ShootAccumulateTimerSet = 2f;
    private float BigGunTimer;
    private float BigGunTimerSet = 2.35f;
    private float ImpulseJumpTimer;
    [HideInInspector] public float ImpulseJumpTimerSet = 0.5f;
    [HideInInspector] public float BlockTimerSet = 1.25f;
    private float BlockPrepareAtkTimerSet = 2f;
    private float BlockPrepareAtkTimer;
    [HideInInspector] public float BlockNormalAtkTimerSet = 1.6f;
    [HideInInspector] public float BlockStrongAtkTimerSet = 2.4f;
    [HideInInspector] public float BeBlockTimerSet = 2;
    [HideInInspector] public float WeakTimerSet = 2.3f;
    private float WeakTimer;
    [HideInInspector] public float SharpenBladeTimerSet = 3.5f;
    [HideInInspector] public float UseRitualSwordTimerSet = 2.2f;
    [HideInInspector] public float SharpTimeSet = 61;//Buff����ɶ�
    [HideInInspector] public float InhibitTimeSet = 61;//Buff����ɶ�

    [HideInInspector] public bool isBeBlockSuccess;//���Q��Lscript�Ψ�(playerblockjudgement�AAtkController)
    [HideInInspector] public bool isBlockSuccess;//���ɦ��\�}��  ���Q��Lscript�Ψ�(playerBlockJudgement�AnormalmonsterAtk)
    [HideInInspector] public bool isAtkBuff;//�M�O�_�O�W�Q���A
    [HideInInspector] public bool isDefendBuff;//�O�_�B�b���m�j�ƪ��A
    [HideInInspector] public bool isInhibit;//�O�_�B�b���A
    [HideInInspector] public bool isPowerSeal;//�O�_�B�b�]�O�ʦL���A
    [HideInInspector] public bool isAccumulate;//�O���O�b�W�O ���Q��Lscript�Ψ�(playerController)
    [HideInInspector] public bool AccumulateTimerSwitch;//���Q��Lscript�Ψ�(playerController)
    [HideInInspector] public bool isAccumulateComplete;//�ण��Τj�� ���Q��Lscript�Ψ�(playerController)
    [HideInInspector] public bool isAim;//�O���O���b�˷�(�U�N�~) ���Q��Lscript�Ψ�(PredictPowerBase�AAimPowerController)
    [HideInInspector] public bool isShooting;//���Q��Lscript�Ψ�(playerController)
    [HideInInspector] public bool isShootAccumulate;//���Q��Lscript�Ψ�(playerController)
    [HideInInspector] public bool isBigGunShoot;//���Q��Lscript�Ψ�(playerController,BigGunController)
    [HideInInspector] public bool isBigGunProcess;//���Q��Lscript�Ψ�(playerController)
    [HideInInspector] public bool isBigGunEnd;//���Q��Lscript�Ψ�(playerController,BigGunController)
    [HideInInspector] public bool isBlock;//�O�_�b���� ���Q��Lscript�Ψ�(playerController�Aatkcontroller)
    [HideInInspector] public bool isBlockActualAppear;//���Q��Lscript�Ψ�(playerController�AplayerBlockJudgement)
    [HideInInspector] public bool isCaptured;//���Q��Lscript�Ψ�(playerController�A����ު�monster)
    [HideInInspector] public bool isWeak;//���Q��Lscript�Ψ�(playerController)
    [HideInInspector] public bool WeakTimerSwitch;//���Q��Lscript�Ψ�(playerController)
    [HideInInspector] public bool isImpulseJump;//���Q��Lscript�Ψ�(playerController)
    [HideInInspector] public bool CanAtk;
    [HideInInspector] public bool CanShoot;
    private bool FirstTrigger = false;
    private bool ShootAccumulateTimerSwitch;
    private bool isAllowBigGun;
    private bool BigGunTimerSwitch;
    private bool ShouldCalculateAlterAtk;

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
    public GameObject CriticAtk;
    public GameObject JumpCAtk;
    public GameObject Block;
    public GameObject BlockNormalAtkAni;
    public GameObject BlockNormalAtk;
    public GameObject BlockStrongAtk;
    public GameObject RBigGun;
    public GameObject LBigGun;
    public GameObject CocktailCriticAtk;
    public GameObject ImpulseJumpExplosion;
    //�U�����������ӥX�{�a�I
    [HideInInspector] public Vector3 BulletAppear = new Vector3(1.237f, 0.533f, 0);
    [HideInInspector] public Vector3 BulletJumpAppear = new Vector3(1.441f, 0.516f, 0);
    [HideInInspector] public Vector3 ThrowItemAppear = new Vector3(0.8f, 1.21f, 0);
    [HideInInspector] public Vector3 WalkThrowItemAppear = new Vector3(0.3f, 1.07f, 0);
    [HideInInspector] public Vector3 JumpThrowItemAppear = new Vector3(0.859f, 0.332f, 0);
    [HideInInspector] public Vector3 ImpulseExplosionAppear = new Vector3(-1.057f, -0.055f, 0);
    public Transform BigGunAppear;//script(BigGunController)

    public GameObject CriticAtkPredictObject;

    // Start is called before the first frame update
    void Start()
    {
        _transform = this.transform;
        _playerController = this.GetComponent<PlayerController>();
        SkillPower = 0;
        Rigid2D = this.GetComponent<Rigidbody2D>();

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
                StopBlockSuccessBulletTime();
            }
        }

        CalculateAlterNormalAtk();

        BigGunTimerMethod();

        ShootAccumulateTimerMethod();

        WeakTimerMethod();
    }

    private void FixedUpdate()
    {
        _fixedDeltaTime = Time.fixedDeltaTime;

        IncreaseSkillPower();
        DecreaseSkillPower();
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

    public void BeginBlockSuccessBulletTime()
    {
        isBlockSuccess = true;
        _playerController._invincibleManager.AddInvincible(InvincibleManager.InvincibleType.Absolute);
        BackgroundSystem.CantPause = true;
        BackgroundSystem.GameSpeed = 0;
        isBlockActualAppear = false;

        BlockPrepareAtkTimer = BlockPrepareAtkTimerSet;
    }
    public void StopBlockSuccessBulletTime()
    {
        BackgroundSystem.GameSpeed = 1;
        BackgroundSystem.CantPause = true;
        _playerController._invincibleManager.RemoveInvincible(InvincibleManager.InvincibleType.Absolute);
        isBlockSuccess = false;
    }
    public async Task BeginBeBlockBulletTime()
    {
        BackgroundSystem.CantPause = true;
        BackgroundSystem.GameSpeed = 0;
        await DelayEnterBeBlock(0.5f);
    }
    public async Task DelayEnterBeBlock(float seconds)
    {
        await Task.Delay(TimeSpan.FromSeconds(seconds));
        if (PlayerController.isGround)
        {
            _playerController._beBlockStatus.AddCommandToSet();
        }
        else
        {
            _playerController._weakStatus.AddCommandToSet();
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

    /*private void ImpulseJumpTimerMethod()
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
    }*/

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
