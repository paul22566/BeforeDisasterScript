using System.Collections;
using UnityEngine;
using static Creature;

public class OldPlayerAnimationController : MonoBehaviour
{
    private Transform _transform;
    private BattleSystem _battleSystem;
    private PlayerController _controller;
    private ItemWindow _itemWindow;

    private GameObject PlayerHurtedAnimation;
    private GameObject PlayerDashAnimation;
    private GameObject PlayerAtkAnimation;
    private GameObject PlayerDieAnimation;
    private GameObject PlayerJumpAnimation;
    private GameObject PlayerMoveAnimation;
    private GameObject PlayerRestoreAnimation;
    private GameObject CocktailCriticAtkAnimation;
    private GameObject PlayerCAtkAnimation;
    private GameObject PlayerCriticAtkAnimation;
    private GameObject PlayerThrowAnimation;
    private GameObject PlayerShootAnimation;
    private GameObject PlayerBlockAnimation;
    private GameObject PlayerBeBlockAnimation;
    private GameObject PlayerJumpBeBlockAnimation;
    private GameObject PlayerSecondJumpAnimation;
    private GameObject PlayerSecondAtkAnimation;
    private GameObject PlayerWeakAnimation;
    private GameObject ImpulseJumpAnimation;
    private GameObject PlayerDanceAnimation;
    private GameObject SharpenBladeAnimation;
    private GameObject UseRitualSwordAnimation;
    private Animator PlayerMoveAni;
    private Animator PlayerJumpAni;
    private Animator PlayerThrowAni;
    private Animator PlayerCriticAtkAni;
    private Animator PlayerShootAni;
    private Animator PlayerBlockAni;
    private Animator PlayerJumpBeBlockAni;
    private Animator PlayerWeakAni;
    private Animator PlayerDanceAni;
    private Transform PlayerHurtedTr;
    private Transform PlayerAtkTr;
    private Transform PlayerDieTr;
    private Transform PlayerJumpTr;
    private Transform PlayerMoveTr;
    private Transform PlayerRestoreTr;
    private Transform CocktailCriticAtkTr;
    private Transform PlayerCAtkTr;
    private Transform PlayerCriticAtkTr;
    private Transform PlayerThrowTr;
    private Transform PlayerShootTr;
    private Transform PlayerBlockTr;
    private Transform PlayerBeBlockTr;
    private Transform PlayerSecondJumpTr;
    private Transform PlayerSecondAtkTr;
    private Transform ImpulseJumpTr;
    private Transform PlayerJumpBeBlockTr;
    private Transform PlayerWeakTr;
    private Transform PlayerDanceTr;
    private Transform SharpenBladeTr;
    private Transform UseRitualSwordTr;
    private SpriteRenderer playerSrDash;
    [SerializeField] private Transform RitualSwordParticle;

    //外部動畫
    public GameObject RFire;
    public GameObject LFire;
    public GameObject AccumulateLightA;
    public GameObject AccumulateLightB;

    private float BeBlockGetUpTimer;
    private float BeBlockGetUpTimerSet = 1f;
    private float BeBlockLagTimer;
    private float BeBlockLagTimerSet = 0.03f;
    private float DanceTimer;
    private float DanceTimerSet = 2;
    private float GoDanceTimer;
    private float GoDanceTimerSet = 60;
    private float TouchGroundCooldownTime;
    private  float TouchGroundCooldownTimeSet = 0.21f;

    private int DancePhase = 0;

    private bool isAccumulateAniAppear;//蓄力動畫有無出現
    private bool isSecondJumpOtherAction;//是否用其他動作打斷二段跳
    [HideInInspector] public bool isSecondJumpFireAppear;
    private bool isBlockAniAppear;//格檔動畫有無出現
    private bool isBeBlockAniAppear;//被格檔動畫有無出現

    //配合音效使用
    private float AtkSoundTime = 0.06f;
    private float JumpAtkSoundTime = 0.03f;
    private float HeavyAtkSoundTime = 0.16f;
    private float CriticAtkBeginingSoundTime = 1;
    private float CriticAtkFlashSoundTime = 2.8f;
    private float ThrowSoundTime = 0.05f;
    private float WalkThrowSoundTime = 0.1f;
    private float JumpThrowSoundTime = 0.1f;
    private float DashSoundTime = 0;
    private float RestoreSoundTime = 0.5f;
    private float BlockSoundTime = 0.05f;
    private float BlockAtkSoundTime = 1.06f;
    private float BlockHeavyAtkSoundTime = 1.55f;
    private float CocktailCriticAtkSoundTime = 0.9f;
    private float ImpulseJumpSoundTime = 0;
    private float SharpenSoundTime = 0.55f;
    private float UseRitualSwordTime = 1.1f;
    private float SETimer;
    private float SETimer2;
    private bool SEAppear = false;
    private bool SEAppear2 = false;
    private bool SEMethodReset = false;
    private bool CanTouchGroundSEPlay;
    private bool HasSpecialSEAppear;
    private PlayerSE _playerSE;
    public delegate void CallPlayerSE();

    public enum AniStatus { Wait, Walk, Jump, SecondJump, Fall, Dash, Die, Restore, Atk, SecondAtk, JumpAtk, 
        CAtk, JumpCAtk, CriticAtk, Throw, ShootWait, Shoot, JumpShoot, BigGun, Block, BeBlock, JumpBeBlock,
        Weak, WalkThrow, JumpThrow, CocktailCriticAtk, ImpulseJump, Hurted, Dance, Sharpen, UseRitualSword,
        AllFalse }
    [HideInInspector] public AniStatus NowAni;//script(CheckPoint，invinvibleTime)
    private AniStatus LastAni;
    // Start is called before the first frame update
    void Start()
    {
        _transform = this.transform;
        _battleSystem = this.GetComponent<BattleSystem>();
        _controller = this.GetComponent<PlayerController>();
        _playerSE = _transform.GetChild(3).GetComponent<PlayerSE>();

        PlayerMoveAnimation = _transform.GetChild(0).GetChild(0).gameObject;
        PlayerJumpAnimation = _transform.GetChild(0).GetChild(1).gameObject;
        PlayerSecondJumpAnimation = _transform.GetChild(0).GetChild(2).gameObject;
        PlayerDashAnimation = _transform.GetChild(0).GetChild(3).gameObject;
        PlayerAtkAnimation = _transform.GetChild(0).GetChild(4).gameObject;
        PlayerSecondAtkAnimation = _transform.GetChild(0).GetChild(5).gameObject;
        PlayerCAtkAnimation = _transform.GetChild(0).GetChild(6).gameObject;
        PlayerCriticAtkAnimation = _transform.GetChild(0).GetChild(7).gameObject;
        PlayerThrowAnimation = _transform.GetChild(0).GetChild(8).gameObject;
        PlayerShootAnimation = _transform.GetChild(0).GetChild(9).gameObject;
        PlayerBlockAnimation = _transform.GetChild(0).GetChild(10).gameObject;
        PlayerBeBlockAnimation = _transform.GetChild(0).GetChild(11).gameObject;
        PlayerJumpBeBlockAnimation = _transform.GetChild(0).GetChild(12).gameObject;
        PlayerWeakAnimation = _transform.GetChild(0).GetChild(13).gameObject;
        PlayerRestoreAnimation = _transform.GetChild(0).GetChild(14).gameObject;
        ImpulseJumpAnimation = _transform.GetChild(0).GetChild(15).gameObject;
        CocktailCriticAtkAnimation = _transform.GetChild(0).GetChild(16).gameObject;
        PlayerHurtedAnimation = _transform.GetChild(0).GetChild(17).gameObject;
        PlayerDieAnimation = _transform.GetChild(0).GetChild(18).gameObject;
        PlayerDanceAnimation = _transform.GetChild(0).GetChild(19).gameObject;
        SharpenBladeAnimation = _transform.GetChild(0).GetChild(20).gameObject;
        UseRitualSwordAnimation = _transform.GetChild(0).GetChild(21).gameObject;

        PlayerHurtedTr = PlayerHurtedAnimation.transform.GetChild(0);
        PlayerAtkTr = PlayerAtkAnimation.transform.GetChild(0);
        PlayerDieTr = PlayerDieAnimation.transform.GetChild(0);
        PlayerJumpTr = PlayerJumpAnimation.transform.GetChild(0);
        PlayerMoveTr = PlayerMoveAnimation.transform.GetChild(0);
        PlayerRestoreTr = PlayerRestoreAnimation.transform.GetChild(0);
        CocktailCriticAtkTr = CocktailCriticAtkAnimation.transform.GetChild(0);
        PlayerCAtkTr = PlayerCAtkAnimation.transform.GetChild(0);
        PlayerCriticAtkTr = PlayerCriticAtkAnimation.transform.GetChild(0);
        PlayerThrowTr = PlayerThrowAnimation.transform.GetChild(0);
        PlayerShootTr = PlayerShootAnimation.transform.GetChild(0);
        PlayerBlockTr = PlayerBlockAnimation.transform.GetChild(0);
        PlayerBeBlockTr = PlayerBeBlockAnimation.transform.GetChild(0);
        PlayerSecondJumpTr = PlayerSecondJumpAnimation.transform.GetChild(0);
        PlayerSecondAtkTr = PlayerSecondAtkAnimation.transform.GetChild(0);
        ImpulseJumpTr = ImpulseJumpAnimation.transform.GetChild(0);
        PlayerJumpBeBlockTr = PlayerJumpBeBlockAnimation.transform.GetChild(0);
        PlayerWeakTr = PlayerWeakAnimation.transform.GetChild(0);
        PlayerDanceTr = PlayerDanceAnimation.transform.GetChild(0);
        SharpenBladeTr = SharpenBladeAnimation.transform.GetChild(0);
        UseRitualSwordTr = UseRitualSwordAnimation.transform.GetChild(0);

        PlayerMoveAni = PlayerMoveTr.GetComponent<Animator>();
        PlayerJumpAni = PlayerJumpTr.GetComponent<Animator>();
        PlayerThrowAni = PlayerThrowTr.GetComponent<Animator>();
        PlayerCriticAtkAni = PlayerCriticAtkTr.GetComponent<Animator>();
        PlayerShootAni = PlayerShootTr.GetComponent<Animator>();
        PlayerBlockAni = PlayerBlockTr.GetComponent<Animator>();
        PlayerJumpBeBlockAni = PlayerJumpBeBlockTr.GetComponent<Animator>();
        PlayerWeakAni = PlayerWeakTr.GetComponent<Animator>();
        PlayerDanceAni = PlayerDanceTr.GetComponent<Animator>();

        playerSrDash = PlayerDashAnimation.gameObject.GetComponent<SpriteRenderer>();

        BeBlockGetUpTimer = BeBlockGetUpTimerSet;
        BeBlockLagTimer = BeBlockLagTimerSet;
        DanceTimer = DanceTimerSet;
        GoDanceTimer = GoDanceTimerSet;
        TouchGroundCooldownTime = TouchGroundCooldownTimeSet;

        NowAni = AniStatus.Wait;
        LastAni = AniStatus.Wait;

        if (GameObject.FindGameObjectWithTag("UI") != null)
        {
            Transform UICanvas;
            UICanvas = GameObject.FindGameObjectWithTag("UI").transform;
            _itemWindow = IdentifyID.FindObject(UICanvas, UIID.Item).GetComponent<ItemWindow>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //重置
        if (Portal.isPortal || PlayerController.isDie || PauseMenuController.OpenAnyMenu || GameEvent.isAniPlay || _controller.isImpulse || _battleSystem.isCaptured)
        {
            if (PauseMenuController.OpenAnyMenu && !PauseMenuController.isPauseMenuOpen)
            {
                NowAni = AniStatus.Wait;
                WaitAniPlay();
            }
            if (!PauseMenuController.OpenAnyMenu)
            {
                CanTouchGroundSEPlay = false;//在進入動畫時，所有判定都須重置及暫停
            }
            if (Portal.isPortal)
            {
                NowAni = AniStatus.Wait;
                WaitAniPlay();
            }
            if (_controller.isImpulse)
            {
                NowAni = AniStatus.Fall;
                AllAnimationFalse();
                PlayerJumpAnimation.SetActive(true);
                PlayerJumpAni.SetBool("isFalling", true);
            }
            return;
        }
        return;
        /*if (PlayerController.isGround)
        {
            isSecondJumpOtherAction = false;
        }
        
        SwitchAni();
        //轉向
        switch (PlayerController._player.face)
        {
            case Face.Right:
                PlayerHurtedTr.localScale = new Vector3(0.28f, 0.28f, 0);
                PlayerAtkTr.localScale = new Vector3(0.28f, 0.28f, 0);
                PlayerDieTr.localScale = new Vector3(0.28f, 0.28f, 0);
                PlayerJumpTr.localScale = new Vector3(0.28f, 0.28f, 0);
                PlayerMoveTr.localScale = new Vector3(0.28f, 0.28f, 0);
                PlayerRestoreTr.localScale = new Vector3(0.28f, 0.28f, 0);
                CocktailCriticAtkTr.localScale = new Vector3(0.28f, 0.28f, 0);
                PlayerCAtkTr.localScale = new Vector3(0.28f, 0.28f, 0);
                PlayerCriticAtkTr.localScale = new Vector3(0.28f, 0.28f, 0);
                PlayerThrowTr.localScale = new Vector3(0.28f, 0.28f, 0);
                PlayerShootTr.localScale = new Vector3(0.28f, 0.28f, 0);
                PlayerBlockTr.localScale = new Vector3(0.28f, 0.28f, 0);
                PlayerBeBlockTr.localScale = new Vector3(0.28f, 0.28f, 0);
                PlayerSecondJumpTr.localScale = new Vector3(0.28f, 0.28f, 0);
                PlayerSecondAtkTr.localScale = new Vector3(0.28f, 0.28f, 0);
                ImpulseJumpTr.localScale = new Vector3(0.28f, 0.28f, 0);
                PlayerJumpBeBlockTr.localScale = new Vector3(0.28f, 0.28f, 0);
                PlayerWeakTr.localScale = new Vector3(0.28f, 0.28f, 0);
                SharpenBladeTr.localScale = new Vector3(0.28f, 0.28f, 0);
                UseRitualSwordTr.localScale = new Vector3(0.28f, 0.28f, 0);
                playerSrDash.flipX = false;
                break;
            case Face.Left:
                PlayerHurtedTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                PlayerAtkTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                PlayerDieTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                PlayerJumpTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                PlayerMoveTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                PlayerRestoreTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                CocktailCriticAtkTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                PlayerCAtkTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                PlayerCriticAtkTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                PlayerThrowTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                PlayerShootTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                PlayerBlockTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                PlayerBeBlockTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                PlayerSecondJumpTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                PlayerSecondAtkTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                ImpulseJumpTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                PlayerJumpBeBlockTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                PlayerWeakTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                SharpenBladeTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                UseRitualSwordTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                playerSrDash.flipX = true;
                break;
        }*/
    }

    /*void SwitchAni()
    {
        //沒被替代就放Wait
        if (PlayerController.isGround && !_controller.isLongFall && NowAni != AniStatus.Dance)
        {
            NowAni = AniStatus.Wait;
        }
        //跳躍動畫
        if (!_battleSystem.isShooting && !BattleSystem.isBeBlockSuccess)
        {
            if (_controller.isJump && !_controller.isSecondJump)
            {
                NowAni = AniStatus.Jump;
            }
            if (_controller.isSecondJump)
            {
                NowAni = AniStatus.SecondJump;
            }
            if (_controller.isFall)
            {
                NowAni = AniStatus.Fall;
            }
            if (isSecondJumpOtherAction && _controller.isSecondFall)
            {
                NowAni = AniStatus.Fall;
            }
        }
        //噴跳動畫
        if (_battleSystem.isImpulseJump)
        {
            if (_controller.isSecondJump)
            {
                isSecondJumpOtherAction = true;
            }
            NowAni = AniStatus.ImpulseJump;
        }
        //走路動畫
        if (_controller.isWalking && PlayerController.isGround && !_battleSystem.isShooting)
        {
            NowAni = AniStatus.Walk;
        }
        if (_controller.isLongFall && PlayerController.isGround)
        {
            NowAni = AniStatus.Fall;
        }
        //攻擊動畫
        if (_battleSystem.isAtk || _battleSystem.isCAtk || _battleSystem.isAim || _battleSystem.isThrowing || _battleSystem.isCriticAtk || _battleSystem.isJumpCAtk)
        {
            if (_battleSystem.isAtk)
            {
                if (!_battleSystem.isJumpAtk)
                {
                    NowAni = AniStatus.Atk;
                }
                else
                {
                    if (_controller.isSecondJump)
                    {
                        isSecondJumpOtherAction = true;
                    }
                    NowAni = AniStatus.JumpAtk;
                }
            }
            if (_battleSystem.isCAtk)
            {
                NowAni = AniStatus.CAtk;
            }

            if (_battleSystem.isJumpCAtk)
            {
                if (_controller.isSecondJump)
                {
                    isSecondJumpOtherAction = true;
                }
                NowAni = AniStatus.JumpCAtk;
            }

            if (_battleSystem.isAim || _battleSystem.isThrowing)
            {
                NowAni = AniStatus.Throw;
            }

            if (_battleSystem.isCriticAtk)
            {
                NowAni = AniStatus.CriticAtk;
            }
        }
        if (_battleSystem.isSecondAtk)
        {
            NowAni = AniStatus.SecondAtk;
        }
        if (_battleSystem.isWalkThrow)
        {
            NowAni = AniStatus.WalkThrow;
        }
        if (_battleSystem.isJumpThrow)
        {
            if (_controller.isSecondJump)
            {
                isSecondJumpOtherAction = true;
            }
            NowAni = AniStatus.JumpThrow;
        }
        if (_battleSystem.isCocktailCriticAtk)
        {
            NowAni = AniStatus.CocktailCriticAtk;
        }
        //蓄力動畫
        if (_battleSystem.isAccumulateComplete)
        {
            if (!isAccumulateAniAppear)
            {
                Instantiate(AccumulateLightA, _transform.position, Quaternion.identity);
                Instantiate(AccumulateLightB, _transform.position, Quaternion.identity);
                isAccumulateAniAppear = true;
            }
        }
        else
        {
            if (isAccumulateAniAppear)
            {
                isAccumulateAniAppear = false;
            }
        }
        //補血動畫
        if (_controller.isRestore)
        {
            NowAni = AniStatus.Restore;
        }
        //磨刀
        if (_battleSystem.isSharpen)
        {
            NowAni = AniStatus.Sharpen;
        }
        //使用儀式刀
        if (_battleSystem.isUsingRitualSword)
        {
            NowAni = AniStatus.UseRitualSword;
        }

        //開槍動畫
        if (_battleSystem.isShooting)
        {
            if (!PlayerController.isGround)
            {
                if (_controller.isSecondJump)
                {
                    isSecondJumpOtherAction = true;
                }
                NowAni = AniStatus.JumpShoot;
            }
            else
            {
                if (_controller.isWalking)
                {
                    NowAni = AniStatus.Shoot;
                }
                else
                {
                    NowAni = AniStatus.ShootWait;
                }
            }
        }
        //Dash動畫
        if (_controller.isDash)
        {
            if (_controller.isSecondJump)
            {
                isSecondJumpOtherAction = true;
            }
            NowAni = AniStatus.Dash;
        }
        //格檔動畫
        if (_battleSystem.isBlock)
        {
            isBlockAniAppear = true;
            NowAni = AniStatus.Block;
        }
        else
        {
            if (isBlockAniAppear)
            {
                NowAni = AniStatus.Wait;
                isBlockAniAppear = false;
            }
        }
        //被格檔動畫
        if (BattleSystem.isBeBlockSuccess)
        {
            BeBlockLagTimer -= Time.deltaTime;
            if (BeBlockLagTimer <= 0)
            {
                isBeBlockAniAppear = true;
                if (_battleSystem.BeBlockisGround)
                {
                    NowAni = AniStatus.BeBlock;
                }
                else
                {
                    NowAni = AniStatus.JumpBeBlock;
                }
            }
        }
        else
        {
            if (isBeBlockAniAppear)
            {
                isBeBlockAniAppear = false;
                BeBlockGetUpTimer = BeBlockGetUpTimerSet;
                BeBlockLagTimer = BeBlockLagTimerSet;
                NowAni = AniStatus.Wait;
            }
        }
        //加農砲動畫
        if (_battleSystem.isBigGunProcess)
        {
            NowAni = AniStatus.BigGun;
        }
        //虛弱動畫
        if (_battleSystem.isWeak)
        {
            NowAni = AniStatus.Weak;
        }
        //被moveAtk打到
        if (_controller.HurtingByMoveAtk)
        {
            NowAni = AniStatus.Hurted;
        }

        switch (NowAni)
        {
            case AniStatus.Wait:
                WaitAniPlay();
                break;
            case AniStatus.Walk:
                WalkAniPlay();
                break;
            case AniStatus.Jump:
                AllAnimationFalse();
                PlayerJumpAnimation.SetActive(true);
                PlayerJumpAni.SetInteger("AtkStatus", 0);
                break;
            case AniStatus.SecondJump:
                AllAnimationFalse();
                if (!isSecondJumpFireAppear)
                {
                    _playerSE.SecondJumpSoundPlay();
                    switch (PlayerController._player.face)
                    {
                        case Face.Left:
                            Instantiate(LFire, this.transform.position, Quaternion.identity);
                            break;
                        case Face.Right:
                            Instantiate(RFire, this.transform.position, Quaternion.identity);
                            break;
                    }
                    isSecondJumpFireAppear = true;
                }
                PlayerSecondJumpAnimation.SetActive(true);
                break;
            case AniStatus.Fall:
                FallAniPlay();
                break;
            case AniStatus.Dash:
                AllAnimationFalse();
                PlayerDashAnimation.SetActive(true);
                SETimerMethod(DashSoundTime, _playerSE.DashSoundPlay);
                break;
            case AniStatus.Restore:
                AllAnimationFalse();
                PlayerRestoreAnimation.SetActive(true);
                SETimerMethod(RestoreSoundTime, _playerSE.RestoreSoundPlay);
                break;
            case AniStatus.Atk:
                AllAnimationFalse();
                PlayerAtkAnimation.SetActive(true);
                SETimerMethod(AtkSoundTime, _playerSE.AtkSound1Play);
                break;
            case AniStatus.SecondAtk:
                AllAnimationFalse();
                PlayerSecondAtkAnimation.SetActive(true);
                SETimerMethod(AtkSoundTime, _playerSE.AtkSound1Play);
                break;
            case AniStatus.CAtk:
                AllAnimationFalse();
                PlayerCAtkAnimation.SetActive(true);
                SETimerMethod(HeavyAtkSoundTime, _playerSE.HeavyAtkSoundPlay);
                break;
            case AniStatus.JumpAtk:
                AllAnimationFalse();
                PlayerJumpAnimation.SetActive(true);
                PlayerJumpAni.SetInteger("AtkStatus", 1);
                SETimerMethod(JumpAtkSoundTime, _playerSE.AtkSound1Play);
                break;
            case AniStatus.JumpCAtk:
                AllAnimationFalse();
                PlayerJumpAnimation.SetActive(true);
                PlayerJumpAni.SetInteger("AtkStatus", 2);
                SETimerMethod(HeavyAtkSoundTime, _playerSE.HeavyAtkSoundPlay);
                break;
            case AniStatus.CriticAtk:
                AllAnimationFalse();
                PlayerCriticAtkAnimation.SetActive(true);
                if (_battleSystem.isCriticAtkChangeStage)
                {
                    PlayerCriticAtkAni.SetBool("isChange", true);
                }
                SETimerMethod(CriticAtkBeginingSoundTime, _playerSE.CriticAtkBeginingSoundPlay);
                SETimerMethod2(CriticAtkFlashSoundTime, _playerSE.CriticAtkFlashSoundPlay);
                break;
            case AniStatus.Throw:
                AllAnimationFalse();
                PlayerThrowAnimation.SetActive(true);
                NormalThrowAni.JudgeThrowType();
                if (_battleSystem.isThrowing)
                {
                    PlayerThrowAni.SetBool("Throw", true);
                    HandResetSEMethod(ThrowSoundTime);
                    SETimerMethod(ThrowSoundTime, _playerSE.ThrowSoundPlay);
                }
                break;
            case AniStatus.ShootWait:
                AllAnimationFalse();
                PlayerShootAnimation.SetActive(true);
                PlayerShootAni.SetBool("Move", false);
                PlayerShootAni.SetBool("Jump", false);
                break;
            case AniStatus.Shoot:
                AllAnimationFalse();
                PlayerShootAnimation.SetActive(true);
                PlayerShootAni.SetBool("Move", true);
                PlayerShootAni.SetBool("Jump", false);
                break;
            case AniStatus.JumpShoot:
                AllAnimationFalse();
                PlayerShootAnimation.SetActive(true);
                PlayerShootAni.SetBool("Move", false);
                PlayerShootAni.SetBool("Jump", true);
                break;
            case AniStatus.BigGun:
                AllAnimationFalse();
                PlayerShootAnimation.SetActive(true);
                PlayerShootAni.SetBool("BigGun", true);
                break;
            case AniStatus.Block:
                AllAnimationFalse();
                PlayerBlockAnimation.SetActive(true);
                SETimerMethod2(BlockSoundTime, _playerSE.BlockSoundPlay);
                if (_battleSystem.BlockAtkSwitch)
                {
                    if (_battleSystem.isBlockNormalAtk)
                    {
                        PlayerBlockAni.SetInteger("Stage", 1);
                        HandResetSEMethod(BlockAtkSoundTime);
                        SETimerMethod(BlockAtkSoundTime, _playerSE.AtkSound1Play);
                    }
                    if (_battleSystem.isBlockStrongAtk)
                    {
                        PlayerBlockAni.SetInteger("Stage", 2);
                        HandResetSEMethod(BlockHeavyAtkSoundTime);
                        SETimerMethod(BlockHeavyAtkSoundTime, _playerSE.HeavyAtkSoundPlay);
                    }
                }
                if (!_battleSystem.isBlockNormalAtk && !_battleSystem.isBlockStrongAtk && _battleSystem.isBlockEnd)
                {
                    PlayerBlockAni.SetBool("End", true);
                }
                break;
            case AniStatus.BeBlock:
                AllAnimationFalse();
                PlayerBeBlockAnimation.SetActive(true);
                break;
            case AniStatus.JumpBeBlock:
                AllAnimationFalse();
                PlayerJumpBeBlockAnimation.SetActive(true);
                if (PlayerController.isGround)
                {
                    PlayerJumpBeBlockAni.SetBool("isGround", true);
                    BeBlockGetUpTimer -= Time.deltaTime;
                    if (BeBlockGetUpTimer <= 0)
                    {
                        PlayerJumpBeBlockAni.SetBool("GetUp", true);
                    }
                }
                break;
            case AniStatus.Weak:
                AllAnimationFalse();
                PlayerWeakAnimation.SetActive(true);
                break;
            case AniStatus.WalkThrow:
                AllAnimationFalse();
                PlayerMoveAnimation.SetActive(true);
                MoveThrowAni.JudgeThrowType();
                PlayerMoveAni.SetBool("Throw", true);
                SETimerMethod(WalkThrowSoundTime, _playerSE.SpecialThrowSoundPlay);
                break;
            case AniStatus.JumpThrow:
                AllAnimationFalse();
                PlayerJumpAnimation.SetActive(true);
                JumpThrowAni.JudgeThrowType();
                PlayerJumpAni.SetInteger("AtkStatus", 3);
                SETimerMethod(JumpThrowSoundTime, _playerSE.SpecialThrowSoundPlay);
                break;
            case AniStatus.CocktailCriticAtk:
                AllAnimationFalse();
                CocktailCriticAtkAnimation.SetActive(true);
                SETimerMethod(CocktailCriticAtkSoundTime, _playerSE.CocktailCriticAtkSoundPlay);
                break;
            case AniStatus.ImpulseJump:
                AllAnimationFalse();
                ImpulseJumpAnimation.SetActive(true);
                SETimerMethod(ImpulseJumpSoundTime, _playerSE.ImpulseJumpSoundPlay);
                break;
            case AniStatus.Hurted:
                AllAnimationFalse();
                PlayerHurtedAnimation.SetActive(true);
                break;
            case AniStatus.Dance:
                DanceAniPlay();
                break;
            case AniStatus.Sharpen:
                AllAnimationFalse();
                SharpenBladeAnimation.SetActive(true);
                SETimerMethod(SharpenSoundTime, _playerSE.SharpenSoundPlay);
                break;
            case AniStatus.UseRitualSword:
                AllAnimationFalse();
                UseRitualSwordAnimation.SetActive(true);
                switch (PlayerController._player.face)
                {
                    case Face.Left:
                        RitualSwordParticle.localRotation = Quaternion.Euler(0, 0, -272.4f);
                        break;
                    case Face.Right:
                        RitualSwordParticle.localRotation = Quaternion.Euler(0, 0, -162.8f);
                        break;
                }
                SETimerMethod(UseRitualSwordTime, _playerSE.RitualSwordSoundPlay);
                break;
        }

        ContinueTypeSE();//控制持續性音效

        SpecialSEControll();

        if (LastAni != NowAni)
        {
            LastAni = NowAni;
            SEMethodReset = false;
        }
    }*/

    public void SwitchFace()
    {
        if (PlayerController._player.face == Face.Right)
        {
            PlayerController._player.face = Face.Left;
        }
        if (PlayerController._player.face == Face.Left)
        {
            PlayerController._player.face = Face.Right;
        }
        switch (PlayerController._player.face)
        {
            case Face.Right:
                PlayerHurtedTr.localScale = new Vector3(0.28f, 0.28f, 0);
                PlayerAtkTr.localScale = new Vector3(0.28f, 0.28f, 0);
                PlayerDieTr.localScale = new Vector3(0.28f, 0.28f, 0);
                PlayerJumpTr.localScale = new Vector3(0.28f, 0.28f, 0);
                PlayerMoveTr.localScale = new Vector3(0.28f, 0.28f, 0);
                PlayerRestoreTr.localScale = new Vector3(0.28f, 0.28f, 0);
                CocktailCriticAtkTr.localScale = new Vector3(0.28f, 0.28f, 0);
                PlayerCAtkTr.localScale = new Vector3(0.28f, 0.28f, 0);
                PlayerCriticAtkTr.localScale = new Vector3(0.28f, 0.28f, 0);
                PlayerThrowTr.localScale = new Vector3(0.28f, 0.28f, 0);
                PlayerShootTr.localScale = new Vector3(0.28f, 0.28f, 0);
                PlayerBlockTr.localScale = new Vector3(0.28f, 0.28f, 0);
                PlayerBeBlockTr.localScale = new Vector3(0.28f, 0.28f, 0);
                PlayerSecondJumpTr.localScale = new Vector3(0.28f, 0.28f, 0);
                PlayerSecondAtkTr.localScale = new Vector3(0.28f, 0.28f, 0);
                ImpulseJumpTr.localScale = new Vector3(0.28f, 0.28f, 0);
                PlayerJumpBeBlockTr.localScale = new Vector3(0.28f, 0.28f, 0);
                PlayerWeakTr.localScale = new Vector3(0.28f, 0.28f, 0);
                playerSrDash.flipX = false;
                break;
            case Face.Left:
                PlayerHurtedTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                PlayerAtkTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                PlayerDieTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                PlayerJumpTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                PlayerMoveTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                PlayerRestoreTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                CocktailCriticAtkTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                PlayerCAtkTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                PlayerCriticAtkTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                PlayerThrowTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                PlayerShootTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                PlayerBlockTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                PlayerBeBlockTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                PlayerSecondJumpTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                PlayerSecondAtkTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                ImpulseJumpTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                PlayerJumpBeBlockTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                PlayerWeakTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                playerSrDash.flipX = true;
                break;
        }
    }

    public void SwitchFace(string _face)
    {
        switch (_face)
        {
            case "R":
                PlayerController._player.face = Face.Right;
                break;
            case "L":
                PlayerController._player.face = Face.Left;
                break;
        }
        switch (PlayerController._player.face)
        {
            case Face.Right:
                PlayerHurtedTr.localScale = new Vector3(0.28f, 0.28f, 0);
                PlayerAtkTr.localScale = new Vector3(0.28f, 0.28f, 0);
                PlayerDieTr.localScale = new Vector3(0.28f, 0.28f, 0);
                PlayerJumpTr.localScale = new Vector3(0.28f, 0.28f, 0);
                PlayerMoveTr.localScale = new Vector3(0.28f, 0.28f, 0);
                PlayerRestoreTr.localScale = new Vector3(0.28f, 0.28f, 0);
                CocktailCriticAtkTr.localScale = new Vector3(0.28f, 0.28f, 0);
                PlayerCAtkTr.localScale = new Vector3(0.28f, 0.28f, 0);
                PlayerCriticAtkTr.localScale = new Vector3(0.28f, 0.28f, 0);
                PlayerThrowTr.localScale = new Vector3(0.28f, 0.28f, 0);
                PlayerShootTr.localScale = new Vector3(0.28f, 0.28f, 0);
                PlayerBlockTr.localScale = new Vector3(0.28f, 0.28f, 0);
                PlayerBeBlockTr.localScale = new Vector3(0.28f, 0.28f, 0);
                PlayerSecondJumpTr.localScale = new Vector3(0.28f, 0.28f, 0);
                PlayerSecondAtkTr.localScale = new Vector3(0.28f, 0.28f, 0);
                ImpulseJumpTr.localScale = new Vector3(0.28f, 0.28f, 0);
                PlayerJumpBeBlockTr.localScale = new Vector3(0.28f, 0.28f, 0);
                PlayerWeakTr.localScale = new Vector3(0.28f, 0.28f, 0);
                playerSrDash.flipX = false;
                break;
            case Face.Left:
                PlayerHurtedTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                PlayerAtkTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                PlayerDieTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                PlayerJumpTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                PlayerMoveTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                PlayerRestoreTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                CocktailCriticAtkTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                PlayerCAtkTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                PlayerCriticAtkTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                PlayerThrowTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                PlayerShootTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                PlayerBlockTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                PlayerBeBlockTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                PlayerSecondJumpTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                PlayerSecondAtkTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                ImpulseJumpTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                PlayerJumpBeBlockTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                PlayerWeakTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                playerSrDash.flipX = true;
                break;
        }
    }

    public void WaitAniPlay()
    {
        AllAnimationFalse();
        PlayerMoveAnimation.SetActive(true);
        PlayerMoveAni.SetBool("Move", false);
        PlayerMoveAni.SetBool("Throw", false);
        if (GameEvent.isAniPlay)
        {
            return;
        }
        GoDanceTimer -= Time.deltaTime;
        if (GoDanceTimer <= 0)
        {
            NowAni = AniStatus.Dance;
        }
    }

    public void WalkAniPlay()
    {
        AllAnimationFalse();
        PlayerMoveAnimation.SetActive(true);
        PlayerMoveAni.SetBool("Move", true);
        PlayerMoveAni.SetBool("Throw", false);
    }

    public void FallAniPlay()
    {
        /*AllAnimationFalse();
        PlayerJumpAnimation.SetActive(true);
        JumpThrowAni.TurnOffThrow();
        PlayerJumpAni.SetBool("isFalling", true);
        PlayerJumpAni.SetInteger("AtkStatus", 0);
        if (_controller.isLongFall && PlayerController.isGround)
        {
            LongFallAniPlay();
        }*/
    }

    public void LongFallAniPlay()
    {
        PlayerJumpAni.SetBool("LongFall", true);
    }

    public void DieAniPlay()
    {
        AllAnimationFalse();
        NowAni = AniStatus.Die;
        PlayerDieAnimation.SetActive(true);
    }

    public void WeakDieAniPlay()
    {
        AllAnimationFalse();
        NowAni = AniStatus.Weak;
        PlayerWeakAnimation.SetActive(true);
        PlayerWeakAni.SetBool("Die", true);
    }

    public void DanceAniPlay()
    {
        AllAnimationFalse();
        PlayerDanceAnimation.SetActive(true);
        switch (DancePhase)
        {
            case 0:
                PlayerDanceAni.SetInteger("Phase", 0);
                break;
            case 1:
                PlayerDanceAni.SetInteger("Phase", 1);
                break;
            case 2:
                PlayerDanceAni.SetInteger("Phase", 2);
                break;
            case 3:
                PlayerDanceAni.SetInteger("Phase", 3);
                break;
            case 4:
                PlayerDanceAni.SetInteger("Phase", 4);
                break;
            case 5:
                PlayerDanceAni.SetInteger("Phase", 5);
                break;
        }
        DanceTimerCalculate();
    }

    private void DanceTimerCalculate()
    {
        DanceTimer -= Time.deltaTime;
        if (DanceTimer <= 0)
        {
            if (DancePhase < 5)
            {
                DancePhase += 1;
            }
            DanceTimer = DanceTimerSet;
        }
    }

    public void ReSetAni()
    {
        NowAni = AniStatus.Wait;
    }

    public void AllAnimationFalse()
    {
        if (NowAni != AniStatus.Dash)
        {
            PlayerDashAnimation.SetActive(false);
        }
        if (NowAni != AniStatus.Atk)
        {
            PlayerAtkAnimation.SetActive(false);
        }
        if (NowAni != AniStatus.Die)
        {
            PlayerDieAnimation.SetActive(false);
        }
        if (NowAni != AniStatus.Jump && NowAni != AniStatus.Fall && NowAni != AniStatus.JumpAtk && NowAni != AniStatus.JumpCAtk && NowAni != AniStatus.JumpThrow)
        {
            PlayerJumpAnimation.SetActive(false);
        }
        if (NowAni != AniStatus.Wait)
        {
            GoDanceTimer = GoDanceTimerSet;
        }
        if (NowAni != AniStatus.Wait && NowAni != AniStatus.Walk && NowAni != AniStatus.WalkThrow)
        {
            PlayerMoveAnimation.SetActive(false);
        }
        if (NowAni != AniStatus.Restore)
        {
            PlayerRestoreAnimation.SetActive(false);
        }
        if (NowAni != AniStatus.CAtk)
        {
            PlayerCAtkAnimation.SetActive(false);
        }
        if (NowAni != AniStatus.CriticAtk)
        {
            PlayerCriticAtkAnimation.SetActive(false);
        }
        if (NowAni != AniStatus.Throw)
        {
            PlayerThrowAnimation.SetActive(false);
        }
        if (NowAni != AniStatus.ShootWait && NowAni != AniStatus.Shoot && NowAni != AniStatus.JumpShoot && NowAni != AniStatus.BigGun)
        {
            PlayerShootAnimation.SetActive(false);
        }
        if (NowAni != AniStatus.Block)
        {
            PlayerBlockAnimation.SetActive(false);
        }
        if (NowAni != AniStatus.BeBlock)
        {
            PlayerBeBlockAnimation.SetActive(false);
        }
        if (NowAni != AniStatus.JumpBeBlock)
        {
            PlayerJumpBeBlockAnimation.SetActive(false);
        }
        if (NowAni != AniStatus.SecondJump)
        {
            PlayerSecondJumpAnimation.SetActive(false);
        }
        if (NowAni != AniStatus.SecondAtk)
        {
            PlayerSecondAtkAnimation.SetActive(false);
        }
        if (NowAni != AniStatus.Weak)
        {
            PlayerWeakAnimation.SetActive(false);
        }
        if (NowAni != AniStatus.CocktailCriticAtk)
        {
            CocktailCriticAtkAnimation.SetActive(false);
        }
        if (NowAni != AniStatus.ImpulseJump)
        {
            ImpulseJumpAnimation.SetActive(false);
        }
        if (NowAni != AniStatus.Hurted)
        {
            PlayerHurtedAnimation.SetActive(false);
        }
        if (NowAni != AniStatus.Dance)
        {
            DancePhase = 0;
            PlayerDanceAnimation.SetActive(false);
        }
        if (NowAni != AniStatus.Sharpen)
        {
            SharpenBladeAnimation.SetActive(false);
        }
        if (NowAni != AniStatus.UseRitualSword)
        {
            UseRitualSwordAnimation.SetActive(false);
        }
    }

    public void AbsoluteAniFalse()//使用特殊動畫時使用
    {
        NowAni = AniStatus.AllFalse;
        AllAnimationFalse();
    }

    private void ContinueTypeSE()
    {
        if(NowAni == AniStatus.Walk || NowAni == AniStatus.Shoot)
        {
            if (_controller.isWalking)
            {
                _playerSE.WalkSoundPlay();
            }
        }
        else
        {
            _playerSE.TurnOffWalkSound();
        }

        if (_battleSystem.isAccumulateComplete)
        {
            _playerSE.AccumulateSoundPlay();
        }
        else
        {
            _playerSE.TurnOffAccumulateSound();
        }

        //中斷磨刀聲
        if (NowAni != AniStatus.Sharpen)
        {
            _playerSE.SharpenSoundStop();
        }
    }

    private void HandResetSEMethod(float SEAppearTime)
    {
        if (!SEMethodReset)
        {
            SEAppear = false;
            SETimer = SEAppearTime;
            SEMethodReset = true;
        }
    }//用在進入動畫後，由玩家行為決定播放時機的音效

    private void SETimerMethod(float SEAppearTime, CallPlayerSE callPlayerSE)
    {
        if (LastAni != NowAni)
        {
            SEAppear = false;
            SETimer = SEAppearTime;
        }

        SETimer -= Time.deltaTime;

        if (SETimer <= 0 && !SEAppear)
        {
            callPlayerSE();
            SEAppear = true;
        }
    }

    private void SETimerMethod2(float SEAppearTime, CallPlayerSE callPlayerSE)
    {
        if (NowAni != LastAni)
        {
            SEAppear2 = false;
            SETimer2 = SEAppearTime;
        }

        SETimer2 -= Time.deltaTime;

        if (SETimer2 <= 0 && !SEAppear2)
        {
            callPlayerSE();
            SEAppear2 = true;
        }
    }

    private void SpecialSEControll()
    {
        if (PlayerController.isGround)
        {
            if (TouchGroundCooldownTime != TouchGroundCooldownTimeSet)
            {
                TouchGroundCooldownTime = TouchGroundCooldownTimeSet;
            }
            if (CanTouchGroundSEPlay)
            {
                _playerSE.TouchGroundSoundPlay();
                CanTouchGroundSEPlay = false;
            }
        }

        if (!PlayerController.isGround)
        {
            TouchGroundCooldownTime -= Time.deltaTime;
            if(TouchGroundCooldownTime <= 0)
            {
                CanTouchGroundSEPlay = true;
            }
        }

        /*if (BattleSystem.isBlockSuccess)
        {
            if (!HasSpecialSEAppear)
            {
                _playerSE.BlockSuccessSoundPlay();
                HasSpecialSEAppear = true;
            }
        }*/

        if (NowAni != LastAni)
        {
            HasSpecialSEAppear = false;
        }
    }
}
