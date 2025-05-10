using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("基本參數")]
    public float Speed;
    public float SpeedlimitY;
    public float JumpForce;
    public float MaxHp; //有被其他script用到(CheckPoint)
    [HideInInspector] public float Hp; //有被其他script用到(CheckPoint，moveAtk)
    public int JumpCountSet;//最大只能到2
    public float ImpulsePowerX;
    public float ImpulsePowerY;
    public float ShootingSpeed;
    public static float PlayerPlaceX;
    public static float PlayerPlaceY;
    private Vector2 MoveDirection = new Vector2(1, 0);
    private Vector2 MoveDirectionDefault = new Vector2(1, 0);

    [Header("系統參數")]
    private Transform _transform;
    private float _deltaTime;
    private float _fixedDeltaTime;
    public static Rigidbody2D Rigid2D;//有被其他script用到(ghostAtk，normalMonsterAtk，Fireball，BattleSystem, playerUnderJudgement)
    [HideInInspector] public BoxCollider2D BoxCollider;//有被其他script用到(dashJudgement)
    private BattleSystem _battleSystem;
    private PlayerAnimationController _aniController;
    private PlayerSpecialAni _specialAni;
    private PlayerCapturedAnimation _captruedController;
    private PlayerSE _playerSE;
    public PlayerTouchJudgement _playerTouchJudgement;
    public SlopeControll _slopeControll;
    public itemManage _itemManage;
    private GameObject CommonCollider;
    private Transform UICanvas;
    public Image HpUI;
    public Image SkillPowerUI;//有被其他script用到(PlayerSpecialAni)
    public Image SkillPowerUI2;
    public Image KillerPointUI;
    public GameObject AtkBuffUI;
    public GameObject DefendBuffUI;
    public GameObject PowerSealUI;
    private int JumpForceCount;
    public float InvincibleTimerSet;//script(battleSystem)
    public float BlockAtkInvicibleTimerSet;
    private float InvincibleTimer;
    public float HurtedTimerSet;
    private float HurtedTimer;
    public float DieTimerSet;//有被其他script用到(BackGroundSystem)
    [HideInInspector] public float DieTimer;//有被其他script用到(BackGroundSystem)
    private float RestoreTimer;
    public float RestoreTimerSet;
    private float LongFallTimer;
    private float LongFallTimerSet = 1.1f;
    private float LongFallWaitTimer;
    private float LongFallWaitTimerSet = 1;
    private int JumpCount;
    private RaycastHit2D GroundCheck;
    private RaycastHit2D SpecialGroundCheck;
    private RaycastHit2D CeilingCheck;
    private RaycastHit2D SpecialCeilingCheck;
    public enum Face {Right,Left };
    public static Face face;//有被其他script用到(FaceJudgement，playerAtkScript，Cocktail，AimPowerController，玩家攻擊的Follow，BattleSystem，PlayerAnimationController)
    private PlayerData _PlayerData;
    private int MonsterDamageRecord;
    private float MonsterImpulseXRecord;
    private float MonsterImpulseYRecord;
    private Vector3 MonsterAtkPlaceRecord;
    private NormalMonsterAtk.AtkType MonsterAtkTypeRecord;
    public static Vector3 PlayerFinalStandPlace = new Vector3();

    //UI
    private PlayerDieController DieUI;
    private GameObject Item;
    private ItemWindow _itemWindow;
    private KeyCodeManage _keyCodeManage;


    //開關
    public static bool isGround;//是否接觸地面 其他script有用到(BattleSystem，EvilKingController)
    [HideInInspector] public bool isUpArrowPressed;//其他script有用到(BackgroundSystem)
    public static bool isDie;//有被其他script用到(fadeoutnormal，AimpowerController，predictPowerBase，BackGroundSystem，checkPoint，MonsterBurnController)
    public static bool CanInteract;//有被其他script用到(interactableObject)
    [HideInInspector] public bool isRestore;//有被其他script用到(battleSystem)
    [HideInInspector] public bool isImpulse;//進入地圖時的推力  有被其他script用到(backgroundsystem，keycodeManage)
    [HideInInspector] public bool ReceiveRightWalkCommand;//有被其他script用到(backgroundsystem)
    [HideInInspector] public bool ReceiveLeftWalkCommand;//有被其他script用到(backgroundsystem)
    [HideInInspector] public bool isDash;//是不是dash中，有被其他script用到(dogAtk，BattleSystem，ghostController)
    [HideInInspector] public bool isSaveGame = false;//有被其他script用到(CheckPoint，RestPlace)
    [HideInInspector] public bool touchLeftWall;//有被其他script用到(backgroundsystem，DashJudgement，playerleftJudgement)
    [HideInInspector] public bool touchRightWall;//有被其他script用到(backgroundsystem，DashJudgement，playerRightJudgement)
    [HideInInspector] public bool canTurn = true;//控制能否轉向 有被其他script用到(BattleSystem)
    [HideInInspector] public bool CantDoAnyThing;//不能做其他事的開關 有被其他script用到(battleSystem，moveAtk，restoreSpace)
    [HideInInspector] public bool OnlyCanMove;//與CantDoAnyThing放在一起 有被其他script用到(battleSystem)
    [HideInInspector] public bool isKeyZPressed;//有沒有按著Z鍵 script(backGroundSystem)
    [HideInInspector] public bool isWalking;//是不是在走路 (invincibleTime)
    private bool ShouldWalkRight;//是否應當往右走
    private bool ShouldWalkLeft;
    [HideInInspector] public bool isCeiling;
    private bool canJump;//是不是處在可以跳躍的狀態(第一次)
    private bool canSecondJump;//能不能第2段跳
    private bool IgnoreGravity;//是否無視重力
    [HideInInspector] public bool ShouldIgnoreGravity;//是否應當無視重力
    [HideInInspector] public bool isSecondJump;
    [HideInInspector] public bool isJump;
    [HideInInspector] public bool isFall;
    [HideInInspector] public bool isSecondFall;
    [HideInInspector] public bool isLongFall;
    [HideInInspector] public bool TouchNormalGround;
    [HideInInspector] public bool TouchGrassGround;
    [HideInInspector] public bool TouchMetalGround;
    private bool TouchGroundDragSet = true;
    private bool JumpTouchCeilling;
    private bool isDecreaseJumpCount;//防止碰地前多扣跳躍次數
    private bool isDashOnGround;
    private bool RestoreTimerSwitch;
    private bool isJumpForceRun;
    private bool isDieUIAppear;
    private bool DieByCapture;
    private bool RestoreUsed;

    //戰鬥相關開關
    public static bool isHurted;//有被其他script用到(initializeColor，battleSystem，playerBlockJudgement，AimpowerController，predictPowerBase，itemWindow，itemButton，DocumentDetail)
    [HideInInspector] public bool HurtedInvincible;//被怪物攻擊無敵時間
    [HideInInspector] public bool WeakInvincible;//不會被怪物普攻 但會受到特殊攻擊
    [HideInInspector] public bool StrongInvincible;//基本上不會被怪物攻擊 但會受到特殊攻擊
    [HideInInspector] public bool AbsoluteInvincible;//不會受到任何一種形式的攻擊
    [HideInInspector] public bool HurtingByMoveAtk;//script(MoveAtk)
    [HideInInspector] public bool isBlockAtkInvincible;//script(battleSystem)
    [HideInInspector] public bool ShouldJudgeHurt;//script(BackGroundSystem)
    private bool MonsterMoveAtkRecord;
    private bool MonsterCanBeBlockRecord;
    private bool MonsterNoAvoidRecord;
    private bool HurtedTimerSwitch;

    [Header("閃避參數")]
    public float DashTime;
    public float DashSpeed;
    public float DashCoolDown;
    public float DashEndPushPower;
    private float DashTimeLeft;
    private float LastDash = -10;

    [HideInInspector] public bool isCheat;
    public TestGameSpeed _gameSpeed;

    void Start()
    {
        DebugBoolSet();
        Hp = MaxHp;
        _transform = transform;
        _itemManage.RestoreUseItem();
        Rigid2D = this.gameObject.GetComponent<Rigidbody2D>();
        BoxCollider = this.gameObject.GetComponent<BoxCollider2D>();
        _battleSystem = this.gameObject.GetComponent<BattleSystem>();
        _aniController = this.GetComponent<PlayerAnimationController>();
        _specialAni = this.GetComponent<PlayerSpecialAni>();
        _captruedController = this.GetComponent<PlayerCapturedAnimation>();
        _playerSE = _transform.GetChild(3).GetComponent<PlayerSE>();
        CommonCollider = _transform.GetChild(4).gameObject;

        face = Face.Right;

        DieTimer = DieTimerSet;
        LongFallTimer = LongFallTimerSet;
        LongFallWaitTimer = LongFallWaitTimerSet;
        if (GameEvent.AbsorbBoss2)
        {
            SkillPowerUI = SkillPowerUI2;
        }

        if (GameObject.FindGameObjectWithTag("UI") != null)
        {
            UICanvas = GameObject.FindGameObjectWithTag("UI").transform;
            _keyCodeManage = UICanvas.GetComponent<KeyCodeManage>();

            DieUI = IdentifyID.FindObject(UICanvas, UIID.Die).GetComponent<PlayerDieController>();
            Item = IdentifyID.FindObject(UICanvas, UIID.Item);
            _itemWindow = Item.GetComponent<ItemWindow>();
        }
            
        if (_PlayerData == null)
        {
            if (GameObject.Find("FollowSystem") != null)
            {
                _PlayerData = GameObject.Find("FollowSystem").GetComponent<PlayerData>();
            }
        }
    }

    void Update()
    {
        //測試
        if (Input.GetKeyDown(KeyCode.L))
        {
            /*_gameSpeed.ChangeGameSpeed();
            print("測試專用變速");*/
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            /*BackgroundSystem.BasicGameSpeed = 1;
            print("測試專用回復速度");*/
        }

        _deltaTime = Time.deltaTime;
        //避免為負or超過
        if (Hp <= 0)
        {
            Hp = 0;
            HpUI.transform.localScale = new Vector3((float)Hp / (float)MaxHp, HpUI.transform.localScale.y, HpUI.transform.localScale.z);
        }
        if (Hp > MaxHp)
        {
            Hp = MaxHp;
        }
        //玩家位置
        PlayerPlaceX = _transform.localPosition.x;
        PlayerPlaceY = _transform.localPosition.y;
        //地面偵測
        GroundCheck = Physics2D.Raycast(_transform.position, -Vector2.up, 0.91f, 1024);
        SpecialGroundCheck = Physics2D.Raycast(_transform.position, -Vector2.up, 0.91f, 32768);
        if (GroundCheck || SpecialGroundCheck || _slopeControll.RightDetecterOnGround || _slopeControll.LeftDetecterOnGround)
        {
            if (GroundCheck && _slopeControll.RightGroundRaycast && _slopeControll.LeftGroundRaycast && _slopeControll.AbsoluteLeaveSlope)
            {
                PlayerFinalStandPlace = _transform.position;
            }
            isGround = true;
            if (TouchGroundDragSet)
            {
                Rigid2D.velocity = new Vector2(0, 0);
                Rigid2D.gravityScale = 0;
                Rigid2D.drag = 20;
                TouchGroundDragSet = false;
            }//同樣的功能在onCollisionEnter也有，放這裡是雙重保險
            TouchGroundSoundJudge(GroundCheck);
            TouchGroundSoundJudge(SpecialGroundCheck);
            TouchGroundSoundJudge(_slopeControll.RightGroundRaycast);
            TouchGroundSoundJudge(_slopeControll.LeftGroundRaycast);
        }
        else
        {
            isGround = false;
            if (!TouchGroundDragSet)
            {
                TouchGroundDragSet = true;
            }
            TouchNormalGround = false;
            TouchGrassGround = false;
            TouchMetalGround = false;
        }

        //天花板偵測
        CeilingCheck = Physics2D.Raycast(_transform.position, Vector2.up, 1.1f, 1024);
        SpecialCeilingCheck = Physics2D.Raycast(_transform.position, Vector2.up, 1.1f, 32768);
        if (CeilingCheck || SpecialCeilingCheck)
        {
            isCeiling = true;
        }
        else
        {
            isCeiling = false;
        }

        //重力控制
        GravityJudge();

        if (isGround && isImpulse)
        {
            isImpulse = false;
        }
        InvincibleTimerMethod();
        HurtedTimerMethod();
        Die();
        AccumulateBreak();

        //互動
        if (Input.GetKeyDown(KeyCodeManage.Interact) || _keyCodeManage.InteractPressed)
        {
            isUpArrowPressed = true;
            _keyCodeManage.InteractPressed = false;
        }
        if (Input.GetKeyUp(KeyCodeManage.Interact) || _keyCodeManage.InteractUp)
        {
            isUpArrowPressed = false;
            _keyCodeManage.InteractUp = false;
        }

        UIControll();

        InvincibleJudge();
        //常態判定框控制
        if (StrongInvincible)
        {
            CommonCollider.SetActive(false);
        }
        else
        {
            CommonCollider.SetActive(true);
        }

        isWalking = false;

        //玩家操作
        PlayerOperate();

        //重置
        if (Portal.isPortal || isDie || PauseMenuController.OpenAnyMenu || GameEvent.isAniPlay || isImpulse || _battleSystem.isCaptured || RestPlace.isOpenRestPlace)
        {
            if (!PauseMenuController.isPauseMenuOpen)
            {
                //跳躍判定
                canSecondJump = false;
                JumpCount = JumpCountSet;
                isJump = false;
                isSecondJump = false;
                isSecondFall = false;
                isFall = false;
                _aniController.isSecondJumpFireAppear = false;
                canJump = true;
                isDecreaseJumpCount = false;
                //蓄力
                _battleSystem.isAccumulate = false;
                _battleSystem.AccumulateTimerSwitch = false;
                _battleSystem.isAccumulateComplete = false;
                //回復
                RestoreUsed = false;
                isRestore = false;
                RestoreTimerSwitch = false;
                //雜項
                BoxCollider.isTrigger = false;
                LongFallTimer = LongFallTimerSet;
                isLongFall = false;
                isDash = false;
                CantDoAnyThing = false;
                ShouldJudgeHurt = false;
                _playerTouchJudgement.isLeftSideHaveMonster = false;
                _playerTouchJudgement.isRightSideHaveMonster = false;
                _playerTouchJudgement.isMonsterUnder = false;
            }

            if (!Portal.isPortal && !isImpulse)
            {
                Rigid2D.gravityScale = 7;
            }

            if (Portal.isPortal || RestPlace.isOpenRestPlace)
            {
                _aniController.WaitAniPlay();
            }

            _playerSE.TurnOffLoopSE();

            if (isImpulse || PauseMenuController.OpenAnyMenu  || GameEvent.isAniPlay || _battleSystem.isCaptured)
            {
                isUpArrowPressed = false;
                ReceiveRightWalkCommand = false;
                ReceiveLeftWalkCommand = false;
                isKeyZPressed = false;
            }
            return;
        }

        if (HurtingByMoveAtk)
        {
            //蓄力
            _battleSystem.isAccumulate = false;
            _battleSystem.AccumulateTimerSwitch = false;
            _battleSystem.isAccumulateComplete = false;

            LongFallTimer = LongFallTimerSet;
            isLongFall = false;
            isDash = false;
        }

        //長時間墜落控制
        if (isGround)
        {
            LongFallTimer = LongFallTimerSet;
            if (isLongFall)
            {
                LongFallWaitTimer -= _deltaTime;
                CantDoAnyThing = true;
                if (isHurted)
                {
                    LongFallWaitTimer = LongFallWaitTimerSet;
                    isLongFall = false;
                }
                if (LongFallWaitTimer <= 0)
                {
                    LongFallWaitTimer = LongFallWaitTimerSet;
                    CantDoAnyThing = false;
                    isLongFall = false;
                }
            }
        }
        else
        {
            if (isFall || isSecondFall)
            {
                LongFallTimer -= _deltaTime;
                if (LongFallTimer <= 0)
                {
                    isLongFall = true;
                }
            }
            if (canSecondJump || isDash || _battleSystem.isJumpThrow || _battleSystem.isImpulseJump || _battleSystem.isJumpAtk || _battleSystem.isJumpCAtk)
            {
                LongFallTimer = LongFallTimerSet;
                isLongFall = false;
            }
        }
        //受傷控制
        HurtedControll();

        //TouchWallLimit
        if (touchRightWall)
        {
            if (Rigid2D.velocity.x > 0)
            {
                Rigid2D.velocity = new Vector2(0, Rigid2D.velocity.y);
            }
        }
        if (touchLeftWall)
        {
            if (Rigid2D.velocity.x < 0)
            {
                Rigid2D.velocity = new Vector2(0, Rigid2D.velocity.y);
            }
        }

        //集氣中斷
        AccumulateBreak();
        //跳躍判斷
        if (isGround)
        {
            canSecondJump = false;
            JumpCount = JumpCountSet;
            isJump = false;
            isSecondJump = false;
            isSecondFall = false;
            isFall = false;
            _aniController.isSecondJumpFireAppear = false;
        }
        if (isGround && !isKeyZPressed)
        {
            canJump = true;
            isDecreaseJumpCount = false;
        }
        if (Rigid2D.velocity.y < 0)
        {
            if (isSecondJump)
            {
                if (!_slopeControll.onRightSlope && !_slopeControll.onLeftSlope)
                {
                    isSecondFall = true;
                }
            }
            else
            {
                if(!_slopeControll.onRightSlope && !_slopeControll.onLeftSlope)
                {
                    isFall = true;
                }
            }
        }
        //跳躍限制
        if (isCeiling)
        {
            JumpTouchCeilling = true;
        }
        //是否互進行互動判斷
        if (isUpArrowPressed && !isDash && !_battleSystem.isAim)
        {
            CanInteract = true;
            isUpArrowPressed = false;
        }
        else
        {
            CanInteract = false;
        }

        //測試用
        if (Input.GetKeyDown(KeyCode.T))
        {
            
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            _itemManage.CocktailNumber += 1;
            _itemManage.ExplosionBottleNumber += 1;
            _battleSystem.SkillPower += 900;
            GameEvent.TutorialComplete = true;
        }
        if (Input.GetKey(KeyCode.H) && Input.GetKey(KeyCode.P))
        {
            MaxHp = 2000;
            Hp = 2000;
            isCheat = true;
        }
        if (Input.GetKey(KeyCode.I) && Input.GetKey(KeyCode.M))
        {
            BattleSystem.NormalAtkHurtPower = 100;
            isCheat = true;
        }
        if (Input.GetKeyDown(KeyCode.O) && Input.GetKey(KeyCode.R))
        {
            MaxHp = 200;
            BattleSystem.NormalAtkHurtPower = 10;
            isCheat = false;
        }
    }

    private void FixedUpdate()
    {
        if (Portal.isPortal || isDie ||  PauseMenuController.OpenAnyMenu || isImpulse ||  _battleSystem.isCaptured || RestPlace.isOpenRestPlace)
        {
            return;
        }

        _fixedDeltaTime = Time.fixedDeltaTime;

        JudgeMoveDirection();
        
        Dash();

        RestoreTimerMethod();

        if (isDash)
        {
            return;
        }
        
        if (isWalking && !GameEvent.isAniPlay)
        {
            GroundMovement();
        }

        if (isJumpForceRun)
        {
            JumpForceCount += 1;
        }
        
        if (isKeyZPressed && JumpCount > 0 && !CantDoAnyThing)
        {
            jump();
        }
        else if (isKeyZPressed && JumpCount > 0 && _battleSystem.isAim)
        {
            _battleSystem.isAim = false;
            CantDoAnyThing = false;
            jump();
        }
        else if (isKeyZPressed && JumpCount > 0 && isRestore)
        {
            isRestore = false;
            CantDoAnyThing = false;
            RestoreTimerSwitch = false;
            jump();
        }
    }
    void JudgeMoveDirection()
    {
        bool ShouldReset = false;

        if (!touchRightWall && !ReceiveLeftWalkCommand && !_playerTouchJudgement.isRightSideHaveMonster)
        {
            if (ReceiveRightWalkCommand)
            {
                ShouldWalkRight = true;
            }
        }
        if (!touchLeftWall && !ReceiveRightWalkCommand && !_playerTouchJudgement.isLeftSideHaveMonster)
        {
            if (ReceiveLeftWalkCommand)
            {
                ShouldWalkLeft = true;
            }
        }

        if (isJump)
        {
            ShouldReset = true;
        }

        if (_slopeControll.AbsoluteLeaveSlope)
        {
            ShouldReset = true;
        }

        if (!_slopeControll.onLeftSlope && !_slopeControll.onRightSlope)
        {
            if (face == Face.Left && _slopeControll.RightDetecterOnLeftSlope)
            {
                ShouldReset = true;
            }
            if (face == Face.Right && _slopeControll.LeftDetecterOnRightSlope)
            {
                ShouldReset = true;
            }
        }

        //Dash特殊狀況重置
        if (isDash)
        {
            switch (face)
            {
                case Face.Left:
                    if (_slopeControll.onRightSlope)
                    {
                        ShouldReset = true;
                    }
                    if (!_slopeControll.onRightSlope && _slopeControll.LeftDetecterOnRightSlope)
                    {
                        ShouldReset = true;
                    }
                    break;
                case Face.Right:
                    if (_slopeControll.onLeftSlope)
                    {
                        ShouldReset = true;
                    }
                    if (!_slopeControll.onLeftSlope && _slopeControll.RightDetecterOnLeftSlope)
                    {
                        ShouldReset = true;
                    }
                    break;
            }
        }

        //左斜坡
        if (_slopeControll.onLeftSlope)
        {
            switch (face)
            {
                case Face.Right:
                    MoveDirection = new Vector2(_slopeControll.LeftDirection.x, -_slopeControll.LeftDirection.y);
                    break;
                case Face.Left:
                    MoveDirection = new Vector2(_slopeControll.LeftDirection.x, _slopeControll.LeftDirection.y);
                    break;
            }
        }
        else if (_slopeControll.RightDetecterOnLeftSlope)
        {
            if (face == Face.Right)
            {
                MoveDirection = new Vector2(_slopeControll.RightDirection.x, -_slopeControll.RightDirection.y);
            }
        }

        //右斜坡
        if (_slopeControll.onRightSlope)
        {
            switch (face)
            {
                case Face.Right:
                    MoveDirection = new Vector2(_slopeControll.RightDirection.x, -_slopeControll.RightDirection.y);
                    break;
                case Face.Left:
                    MoveDirection = new Vector2(_slopeControll.RightDirection.x, _slopeControll.RightDirection.y);
                    break;
            }
        }
        else if (_slopeControll.LeftDetecterOnRightSlope)
        {
            if (face == Face.Left)
            {
                MoveDirection = new Vector2(_slopeControll.LeftDirection.x, _slopeControll.LeftDirection.y);
            }
        }

        if (ShouldReset)
        {
            if (MoveDirection != MoveDirectionDefault)
            {
                MoveDirection = MoveDirectionDefault;
            }
            return;
        }
    }

    //地面移動
    void GroundMovement()
    {
        //這裡不涉及圖片方向
        if (ShouldWalkRight)
        {
            if (_battleSystem.isShooting)
            {
                _transform.position = new Vector3(ShootingSpeed * MoveDirection.x * _fixedDeltaTime + _transform.position.x, ShootingSpeed * MoveDirection.y * _fixedDeltaTime + _transform.position.y, 0);
            }
            else
            {
                _transform.position = new Vector3(Speed * MoveDirection.x * _fixedDeltaTime + _transform.position.x, Speed * MoveDirection.y * _fixedDeltaTime + _transform.position.y, 0);
            }
        }
        if (ShouldWalkLeft)
        {
            if (_battleSystem.isShooting)
            {
                _transform.position = new Vector3(-ShootingSpeed * MoveDirection.x * _fixedDeltaTime + _transform.position.x, ShootingSpeed * MoveDirection.y * _fixedDeltaTime + _transform.position.y, 0);
            }
            else
            {
                _transform.position = new Vector3(-Speed * MoveDirection.x * _fixedDeltaTime + _transform.position.x, Speed * MoveDirection.y * _fixedDeltaTime + _transform.position.y, 0);
            }
        }

        ShouldWalkLeft = false;
        ShouldWalkRight = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "platform" || collision.gameObject.tag == "SpecialPlatform")
        {
            if (isGround)
            {
                Rigid2D.velocity = new Vector2(0, 0);
                Rigid2D.gravityScale = 0;
                Rigid2D.drag = 20;
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "LeftWall")
        {
            touchLeftWall = false;
        }
        if (collision.gameObject.tag == "RightWall")
        {
            touchRightWall = false;
        }
        if (collision.gameObject.tag == "platform" || collision.gameObject.tag == "SpecialPlatform")
        {
            if (!isKeyZPressed && !isDecreaseJumpCount)
            {
                canJump = false;
                JumpCount -= 1;
                canSecondJump = true;
                isDecreaseJumpCount = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "monsterAtk" && StrongInvincible)
        {
            RecordMonsterAtkData(collision.transform.GetComponent<NormalMonsterAtk>(), collision.transform);
        }//處於強力無敵時才使用這個
        if (collision.GetComponent<DashJudgement>() != null)
        {
            BoxCollider.isTrigger = false;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Portal" && !_battleSystem.isCriticAtk && !GameEvent.isAniPlay && !Portal.isPortal)
        {
            collision.gameObject.GetComponent<Portal>().BeginChangeScene();
            Rigid2D.gravityScale = 0;
        }
    }

    //跳躍
    void jump()
    {
        if(canJump && !JumpTouchCeilling)
        {
            isJump = true;
            if (JumpForceCount <= 10)
            {
                Rigid2D.AddForce(new Vector2(0, (JumpForce / 2) + (0.05f * JumpForce * JumpForceCount)), ForceMode2D.Impulse);
            }
            else
            {
                Rigid2D.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
            }
            if (Rigid2D.velocity.y > SpeedlimitY)
            {
                canJump = false;
            }
        }
        if(canSecondJump && !JumpTouchCeilling && JumpCount>0 && !isGround)
        {
            isFall = false;
            isSecondJump = true;
            if (JumpForceCount <= 10)
            {
                Rigid2D.AddForce(new Vector2(0, (JumpForce / 2) + (0.05f * JumpForce * JumpForceCount)), ForceMode2D.Impulse);
            }
            else
            {
                canSecondJump = false;
                JumpCount -= 1;
            }
            if (Rigid2D.velocity.y > (SpeedlimitY)/1.2f)
            {
                canSecondJump = false;
                JumpCount -= 1;
            }
        }
    }

    void InvincibleTimerMethod()
    {
        if (isBlockAtkInvincible)
        {
            if (InvincibleTimer <= 0 || HurtedInvincible)
            {
                if (HurtedInvincible)
                {
                    HurtedInvincible = false;
                }
                InvincibleTimer = BlockAtkInvicibleTimerSet;
            }

            InvincibleTimer -= _deltaTime;

            if (InvincibleTimer <= 0)
            {
                isBlockAtkInvincible = false;
            }

            return;
        }

        if (HurtedInvincible)
        {
            if(InvincibleTimer <= 0)
            {
                InvincibleTimer = InvincibleTimerSet;
            }

            InvincibleTimer -= _deltaTime;

            if (InvincibleTimer <= 0)
            {
                HurtedInvincible = false;
            }
        }
    }

    void HurtedTimerMethod()
    {
        if (HurtedTimerSwitch)
        {
            if (isHurted)
            {
                HurtedTimer -= _deltaTime;
                if (HurtedTimer <= 0)
                {
                    CantDoAnyThing = false;
                    isHurted = false;
                    HurtedTimerSwitch = false;
                }
            }
        }
        else
        {
            HurtedTimer = HurtedTimerSet;
        }
    }

    void RestoreTimerMethod()
    {
        if (RestoreTimerSwitch)
        {
            RestoreTimer -= Time.fixedDeltaTime;
            if (isHurted || isDash || GameEvent.isAniPlay)
            {
                RestoreUsed = false;
                isRestore = false;
                RestoreTimerSwitch = false;
                return;
            }           

            if (RestoreTimer <= (RestoreTimerSet - 0.5))
            {
                if (!RestoreUsed)
                {
                    _itemManage.RestoreItemNumber -= 1;
                    RestoreUsed = true;
                }
                Hp += 4;
                if (RestoreTimer <= 0)
                {
                    RestoreUsed = false;
                    RestoreTimerSwitch = false;
                    isRestore = false;
                    CantDoAnyThing = false;
                }
            }
        }
        else
        {
            RestoreTimer = RestoreTimerSet;
        }
    }

    void ReadyToDash()
    {
        if (isGround)
        {
            isDashOnGround = true;
        }

        Rigid2D.gravityScale = 0;
        
        Rigid2D.velocity = new Vector2(Rigid2D.velocity.x,0);

        BoxCollider.isTrigger = true;

        isDash = true;

        CantDoAnyThing = true;

        DashTimeLeft = DashTime;

        LastDash = Time.time;
    }

    void Dash()
    {
        if (isDash)
        {
            if (GameEvent.isAniPlay || isHurted)
            {
                if (isDashOnGround)
                {
                    canJump = false;
                    JumpCount -= 1;
                    canSecondJump = true;
                    isDashOnGround = false;
                }
                BoxCollider.isTrigger = false;
                if (isGround)
                {
                    Rigid2D.gravityScale = 0;
                    Rigid2D.drag = 20;
                }
                else
                {
                    Rigid2D.gravityScale = 7;
                    Rigid2D.drag = 5;
                }
                isDash = false;
                CantDoAnyThing = false;
            }
            if(DashTimeLeft > 0)
            {
                DashTimeLeft -= _fixedDeltaTime;

                ShadowPool.instance.GetFromPool();

                switch (face)
                {
                    case Face.Right:
                        if (!touchRightWall)
                        {
                            _transform.position = new Vector3(DashSpeed * MoveDirection.x * _fixedDeltaTime + _transform.position.x, DashSpeed * MoveDirection.y * _fixedDeltaTime + _transform.position.y, 0);
                        }
                        break;
                    case Face.Left:
                        if (!touchLeftWall)
                        {
                            _transform.position = new Vector3(-DashSpeed * MoveDirection.x * _fixedDeltaTime + _transform.position.x, DashSpeed * MoveDirection.y * _fixedDeltaTime + _transform.position.y, 0);
                        }
                        break;
                }
            }
            if(DashTimeLeft <= 0)
            {
                if (isDashOnGround)
                {
                    canJump = false;
                    JumpCount -= 1;
                    canSecondJump = true;
                    isDashOnGround = false;
                }
                BoxCollider.isTrigger = false;
                if (isGround)
                {
                    Rigid2D.gravityScale = 0;
                    Rigid2D.drag = 20;
                }
                else
                {
                    Rigid2D.gravityScale = 7;
                    Rigid2D.drag = 5;
                }

                switch (face)
                {
                    case Face.Right:
                        if (!touchRightWall)
                        {
                            Rigid2D.velocity = new Vector2(DashEndPushPower, Rigid2D.velocity.y);
                        }
                        break;
                    case Face.Left:
                        if (!touchLeftWall)
                        {
                            Rigid2D.velocity = new Vector2(-DashEndPushPower, Rigid2D.velocity.y);
                        }
                        break;
                }

                isDash = false;
                CantDoAnyThing = false;
            }
        }
    }

    void Die()
    {
        if (Hp <= 0)
        {
            HurtingByMoveAtk = false;
            MonsterDamageRecord = 0;
            ShouldJudgeHurt = false;
            MonsterCanBeBlockRecord = false;
            MonsterMoveAtkRecord = false;
            MonsterImpulseXRecord = 0;
            MonsterImpulseYRecord = 0;
            MonsterAtkPlaceRecord = new Vector3(0, 0, 0);

            if (_battleSystem.isCaptured)
            {
                DieByCapture = true;
            }

            if (!DieByCapture)
            {
                _aniController.DieAniPlay();
            }
            else
            {
                _aniController.WeakDieAniPlay();
            }

            DieTimer -= _deltaTime;
            isDie = true;
            CantDoAnyThing = true;

            if (!isSaveGame)
            {
                _PlayerData.CommonSave();
                isSaveGame = true;
            }

            if (DieTimer <= 0)
            {
                if (!isDieUIAppear)
                {
                    isDieUIAppear = true;
                    DieUI.gameObject.SetActive(true);
                }
            }
        }
        else
        {
            if (isDieUIAppear)
            {
                DieUI.ResetDie();
                DieByCapture = false;
                isDieUIAppear = false;
            }
        }
    }

    private void PlayerOperate()
    {
        bool _RightMove = false;
        bool _LeftMove = false;
        bool _WalkThrow = false;
        bool _Jump = false;
        bool _JumpEnd = false;
        bool _Dash = false;
        bool _NormalAtk = false;
        bool _NormalAtkEnd = false;
        bool _StrongAtk = false;
        bool _ChangeItem = false;
        bool _UseItem = false;
        bool _UseItemEnd = false;
        bool _Restore = false;
        bool _Shoot = false;
        bool _ShootEnd = false;
        bool _Block = false;
        bool _ItemWindow = false;

        //確認指令
        //移動
        if (Input.GetKey(KeyCodeManage.GoRight))
        {
            _RightMove = true;
        }
        if (Input.GetKey(KeyCodeManage.GoLeft))
        {
            _LeftMove = true;
        }
        if (Input.GetAxis("Horizontal") > 0.8 && !Input.GetKey(KeyCodeManage.GoRight))
        {
            _RightMove = true;
        }
        if (Input.GetAxis("Horizontal") < -0.8 && !Input.GetKey(KeyCodeManage.GoLeft))
        {
            _LeftMove = true;
        }
        //移動投擲
        if (Input.GetKey(KeyCodeManage.Interact))
        {
            if (_RightMove || _LeftMove)
            {
                _WalkThrow = true;
            }
        }
        if (_RightMove && Input.GetAxis("RightHorizontal") < -0.3)
        {
            _WalkThrow = true;
        }
        if (_LeftMove && Input.GetAxis("RightHorizontal") > 0.3)
        {
            _WalkThrow = true;
        }
        //跳躍
        if (Input.GetKeyDown(KeyCodeManage.Jump) || _keyCodeManage.JumpPressed)
        {
            _Jump = true;
        }
        if (Input.GetKeyUp(KeyCodeManage.Jump) || _keyCodeManage.JumpUp)
        {
            _JumpEnd = true;
        }
        //閃避
        if (Input.GetKeyDown(KeyCodeManage.Dash) || _keyCodeManage.DashPressed)
        {
            _Dash = true;
        }
        //揮劍
        if (Input.GetKeyDown(KeyCodeManage.NormalAtk) || _keyCodeManage.NormalAtkPressed)
        {
            _NormalAtk = true;
        }
        if (Input.GetKeyUp(KeyCodeManage.NormalAtk) ||  _keyCodeManage.NormalAtkUp)
        {
            _NormalAtkEnd = true;
        }
        if (Input.GetKeyDown(KeyCodeManage.StrongAtk) || _keyCodeManage.StrongAtkPressed)
        {
            _StrongAtk = true;
        }
        //使用道具
        if (Input.GetKeyDown(KeyCodeManage.ChangeUseItem) || _keyCodeManage.ChangeUseItemPressed)
        {
            _ChangeItem = true;
        }

        if (Input.GetKeyDown(KeyCodeManage.UseItem) || _keyCodeManage.UseItemPressed)
        {
            _UseItem = true;
        }
        if (Input.GetKeyUp(KeyCodeManage.UseItem) || _keyCodeManage.UseItemUp)
        {
            _UseItemEnd = true;
        }
        //補血
        if (Input.GetKeyDown(KeyCodeManage.Restore) || _keyCodeManage.RestorePressed)
        {
            _Restore = true;
        }
        //開槍
        if (Input.GetKeyDown(KeyCodeManage.Shoot) || _keyCodeManage.ShootPressed)
        {
            _Shoot = true;
        }
        if (Input.GetKeyUp(KeyCodeManage.Shoot) || _keyCodeManage.ShootUp)
        {
            _ShootEnd = true;
        }
        //格檔
        if (Input.GetKeyDown(KeyCodeManage.Block) || _keyCodeManage.BlockPressed)
        {
            _Block = true;
        }
        //道具
        if (Input.GetKeyDown(KeyCodeManage.OpenItemWindow) || Input.GetKeyDown(KeyCode.JoystickButton6))
        {
            _ItemWindow = true;
        }

        //QTE false
        if (_keyCodeManage.QTENormalAtkPressed)
        {
            _keyCodeManage.QTENormalAtkPressed = false;
        }
        if (_keyCodeManage.QTENormalAtkUp)
        {
            _keyCodeManage.QTENormalAtkUp = false;
        }

        //QTE處理
        if (GameEvent.isAniPlay)
        {
            if (Input.GetKeyDown(KeyCodeManage.NormalAtk) || _keyCodeManage.NormalAtkPressed)
            {
                _keyCodeManage.QTENormalAtkPressed = true;
            }
            if (Input.GetKeyUp(KeyCodeManage.NormalAtk) || _keyCodeManage.NormalAtkUp)
            {
                _keyCodeManage.QTENormalAtkUp = true;
            }
        }

        _keyCodeManage.ResetControllerCommand();

        if (Portal.isPortal || isDie || PauseMenuController.OpenAnyMenu || GameEvent.isAniPlay || isImpulse || _battleSystem.isCaptured || RestPlace.isOpenRestPlace)
        {
            return;
        }

        //執行指令
        //移動
        if (_RightMove)
        {
            ReceiveRightWalkCommand = true;
            if (!CantDoAnyThing && !ReceiveLeftWalkCommand)
            {
                if (face == Face.Left && canTurn)
                {
                    face = Face.Right;
                }
                isWalking = true;
            }
            else if (!ReceiveLeftWalkCommand && OnlyCanMove)
            {
                isWalking = true;
            }
        }
        if (!_RightMove && ReceiveRightWalkCommand)
        {
            ReceiveRightWalkCommand = false;
        }
        if (_LeftMove)
        {
            ReceiveLeftWalkCommand = true;
            if (!CantDoAnyThing && !ReceiveRightWalkCommand)
            {
                if (face == Face.Right && canTurn)
                {
                    face = Face.Left;
                }
                isWalking = true;
            }
            else if (!ReceiveRightWalkCommand && OnlyCanMove)
            {
                isWalking = true;
            }
        }
        if (!_LeftMove && ReceiveLeftWalkCommand)
        {
            ReceiveLeftWalkCommand = false;
        }
        //移動投擲
        if (_WalkThrow && isGround)
        {
            if (GameEvent.TutorialComplete)
            {
                _battleSystem.CanWalkThrow = true;
            }
        }
        if (!_WalkThrow && _battleSystem.CanWalkThrow)
        {
            _battleSystem.CanWalkThrow = false;
        }
        //跳躍
        if (_Jump)
        {
            isKeyZPressed = true;
            isJumpForceRun = true;
            isDecreaseJumpCount = true;
            JumpTouchCeilling = false;
        }
        if (_JumpEnd)
        {
            if (JumpCount > 0)
            {
                if (canSecondJump)
                {
                    canSecondJump = false;
                }
                else
                {
                    canSecondJump = true;
                }
            }
            JumpForceCount = 0;
            isJumpForceRun = false;
            isKeyZPressed = false;
            canJump = false;
            JumpCount -= 1;
        }
        //閃避
        if (_Dash)
        {
            if (Time.time >= (LastDash + DashCoolDown))
            {
                if (!CantDoAnyThing)
                {
                    //執行dash
                    ReadyToDash();
                }
                else if (isRestore || _battleSystem.isAim || _battleSystem.isSharpen || _battleSystem.isUsingRitualSword)
                {
                    //執行dash
                    ReadyToDash();
                }
            }
        }
        //揮劍
        if (_NormalAtk)
        {
            _battleSystem.AccumulateSystem();
        }
        if (_NormalAtkEnd)
        {
            _battleSystem.NormalAtkSystem();
        }
        if (_StrongAtk)
        {
            _battleSystem.StrongAtkSystem();
        }
        //使用道具
        if (_ChangeItem)
        {
            _itemManage.NowPrepareItemID = itemManage.ChangePrepareItem(_itemManage.NowPrepareItemID);
        }

        if (_UseItem && GameEvent.TutorialComplete)
        {
            switch (_itemManage.NowPrepareItem)
            {
                case itemManage.PrepareItem.Cocktail:
                    _battleSystem.CocktailJudgeSystem();
                    break;
                case itemManage.PrepareItem.ExplosionBottle:
                    _battleSystem.CocktailJudgeSystem();
                    break;
                case itemManage.PrepareItem.Sharpener:
                    _battleSystem.BeginSharpenBlade();
                    break;
                case itemManage.PrepareItem.RitualSword:
                    _battleSystem.BeginUseRitualSword();
                    break;
            }
        }
        if (_UseItemEnd && GameEvent.TutorialComplete)
        {
            switch (_itemManage.NowPrepareItem)
            {
                case itemManage.PrepareItem.Cocktail:
                    _battleSystem.AimThrowSystem();
                    break;
                case itemManage.PrepareItem.ExplosionBottle:
                    _battleSystem.AimThrowSystem();
                    break;
            }
        }
        //補血
        if (_Restore)
        {
            if (!CantDoAnyThing && isGround)
            {
                if (_itemManage.RestoreItemNumber > 0)
                {
                    isRestore = true;
                    CantDoAnyThing = true;
                    RestoreTimerSwitch = true;
                }
            }
        }
        //開槍
        if (_Shoot)
        {
            _battleSystem.ShootSystem();
        }
        if (_ShootEnd)
        {
            _battleSystem.ShootAccumulateEnd();
        }
        //格檔
        if (_Block)
        {
            if (GameEvent.TutorialComplete)
            {
                _battleSystem.BlockSystem();
            }
        }
        //道具
        if (_ItemWindow && !AnnouncementController.isOpenAnnouncement)
        {
            if (GameEvent.TutorialComplete)
            {
                _itemWindow.isItemWindowAppear = true;
                PauseMenuController.OpenAnyMenu = true;
                BackgroundSystem.CantPause = true;
                Item.SetActive(true);
            }
        }
    }

    private void TouchGroundSoundJudge(RaycastHit2D Raycast)
    {
        if (Raycast && Raycast.transform.GetComponent<CollisionType>() != null)
        {
            switch (Raycast.transform.GetComponent<CollisionType>()._type)
            {
                case CollisionType.Type.Cement:
                    TouchNormalGround = true;
                    TouchGrassGround = false;
                    TouchMetalGround = false;
                    break;
                case CollisionType.Type.Grass:
                    TouchNormalGround = false;
                    TouchGrassGround = true;
                    TouchMetalGround = false;
                    break;
                case CollisionType.Type.Metal:
                    TouchNormalGround = false;
                    TouchGrassGround = false;
                    TouchMetalGround = true;
                    break;
                default:
                    TouchNormalGround = false;
                    TouchGrassGround = false;
                    TouchMetalGround = false;
                    break;
            }
        }
    }

    private void AccumulateBreak()
    {
        if (_battleSystem.isCAtk || _battleSystem.isJumpCAtk || _battleSystem.isThrowing)
        {
            _battleSystem.isAccumulate = false;
            _battleSystem.isAccumulateComplete = false;
            _battleSystem.AccumulateTimerSwitch = false;
        }
        if (_battleSystem.isJumpThrow || _battleSystem.isWalkThrow || _battleSystem.isShootAccumulate)
        {
            _battleSystem.isAccumulate = false;
            _battleSystem.isAccumulateComplete = false;
            _battleSystem.AccumulateTimerSwitch = false;
        }
        if (_battleSystem.isShooting || isRestore || _battleSystem.isBlock)
        {
            _battleSystem.isAccumulate = false;
            _battleSystem.isAccumulateComplete = false;
            _battleSystem.AccumulateTimerSwitch = false;
        }
    }

    private void InvincibleJudge()
    {
        bool CanAbsInvincible = false;
        bool CanStrongInvincible = false;
        bool CanWeakInvincible = false;

        if (isDie || HurtingByMoveAtk || Portal.isPortal || BattleSystem.isBlockSuccess || _battleSystem.isBlockNormalAtk || _battleSystem.isBlockStrongAtk || isBlockAtkInvincible)
        {
            CanAbsInvincible = true;
        }//完全無敵

        if (HurtedInvincible)
        {
            CanStrongInvincible = true;
        }

        if (isDash)
        {
            CanWeakInvincible = true;
        }//Weak無敵

        if (CanAbsInvincible)
        {
            AbsoluteInvincible = true;
            StrongInvincible = true;
            WeakInvincible = true;
            return;
        }
        else
        {
            AbsoluteInvincible = false;
        }
        if (CanStrongInvincible)
        {
            StrongInvincible = true;
            WeakInvincible = true;
            return;
        }
        else
        {
            StrongInvincible = false;
        }
        if (CanWeakInvincible)
        {
            WeakInvincible = true;
        }
        else
        {
            WeakInvincible = false;
        }
    }

    public void RecordMonsterAtkData(NormalMonsterAtk _atk, Transform AtkTransform)
    {
        MonsterImpulseXRecord = _atk.ImpulsePowerX;
        MonsterImpulseYRecord = _atk.ImpulsePowerY;
        MonsterDamageRecord = _atk.Damage;
        ShouldJudgeHurt = true;
        MonsterCanBeBlockRecord = _atk.CanBeBlock;
        MonsterNoAvoidRecord = _atk.NoAvoid;
        MonsterAtkTypeRecord = _atk.Type;
        if (AtkTransform.GetComponent<MoveAtk>() != null)
        {
            MonsterMoveAtkRecord = true;
        }
        MonsterAtkPlaceRecord = AtkTransform.position;
    }
    public void HurtedByCaptureAtk(CaptureAtk _Atk)
    {
        _battleSystem.isCaptured = true;
        _captruedController._monsterCaptureController = _Atk._captureController;
    }
    public void HurtedByCaptureAtkEnd()
    {
        _captruedController._monsterCaptureController = null;
        _battleSystem.isCaptured = false;
        _battleSystem.isWeak = true;
        _battleSystem.WeakTimerSwitch = true;
        HurtedInvincible = true;
    }
    private void ResetMonsterAtkData()
    {
        MonsterDamageRecord = 0;
        ShouldJudgeHurt = false;
        MonsterCanBeBlockRecord = false;
        MonsterNoAvoidRecord = false;
        MonsterMoveAtkRecord = false;
        MonsterImpulseXRecord = 0;
        MonsterImpulseYRecord = 0;
        MonsterAtkPlaceRecord = new Vector3(0, 0, 0);
    }
    private void HurtedControll()
    {
        if (!ShouldJudgeHurt)
        {
            return;
        }

        if (!AbsoluteInvincible)
        {
            if (_battleSystem.isDefendBuff)
            {
                MonsterDamageRecord = (int)(MonsterDamageRecord * 0.9f);
            }
            //無視強力無敵區域
            if (MonsterMoveAtkRecord)
            {
                HurtingByMoveAtk = true;

                HurtedInvincible = true;
                Hp -= MonsterDamageRecord;
                _playerSE.HurtedSoundPlay(MonsterAtkTypeRecord);
                CantDoAnyThing = true;
                isDash = false;
                Rigid2D.velocity = new Vector2(0, 0);
                Rigid2D.gravityScale = 0;
                ResetMonsterAtkData();
                return;
            }

            //-------------------------
            if (StrongInvincible)
            {
                ResetMonsterAtkData();
                return;
            }

            if (WeakInvincible && !MonsterNoAvoidRecord)
            {
                ResetMonsterAtkData();
                return;
            }

            if (_battleSystem.isBlockActualAppear && MonsterCanBeBlockRecord)
            {
                switch (face)
                {
                    case Face.Left:
                        if (MonsterAtkPlaceRecord.x >= _transform.position.x)
                        {
                            if (_battleSystem.isBlock)
                            {
                                Hp -= MonsterDamageRecord / 2;
                            }
                            else
                            {
                                Hp -= MonsterDamageRecord;
                            }
                            HurtedInvincible = true;
                            isHurted = true;
                            HurtedTimerSwitch = true;
                            CantDoAnyThing = true;
                            _playerSE.HurtedSoundPlay(MonsterAtkTypeRecord);
                            if (MonsterAtkPlaceRecord.x < _transform.position.x)
                            {
                                Rigid2D.AddForce(new Vector2(MonsterImpulseXRecord, MonsterImpulseYRecord), ForceMode2D.Impulse);
                            }
                            else
                            {
                                Rigid2D.AddForce(new Vector2(-MonsterImpulseXRecord, MonsterImpulseYRecord), ForceMode2D.Impulse);
                            }
                        }
                        break;
                    case Face.Right:
                        if (MonsterAtkPlaceRecord.x <= _transform.position.x)
                        {
                            if (_battleSystem.isBlock)
                            {
                                Hp -= MonsterDamageRecord / 2;
                            }
                            else
                            {
                                Hp -= MonsterDamageRecord;
                            }
                            HurtedInvincible = true;
                            isHurted = true;
                            HurtedTimerSwitch = true;
                            CantDoAnyThing = true;
                            _playerSE.HurtedSoundPlay(MonsterAtkTypeRecord);
                            if (MonsterAtkPlaceRecord.x < _transform.position.x)
                            {
                                Rigid2D.AddForce(new Vector2(MonsterImpulseXRecord, MonsterImpulseYRecord), ForceMode2D.Impulse);
                            }
                            else
                            {
                                Rigid2D.AddForce(new Vector2(-MonsterImpulseXRecord, MonsterImpulseYRecord), ForceMode2D.Impulse);
                            }
                        }
                        break;
                }
            }
            else
            {
                if (_battleSystem.isBlock)
                {
                    Hp -= MonsterDamageRecord / 2;
                }
                else
                {
                    Hp -= MonsterDamageRecord;
                }
                HurtedInvincible = true;
                isHurted = true;
                HurtedTimerSwitch = true;
                CantDoAnyThing = true;
                _playerSE.HurtedSoundPlay(MonsterAtkTypeRecord);
                if (MonsterAtkPlaceRecord.x < _transform.position.x)
                {
                    Rigid2D.AddForce(new Vector2(MonsterImpulseXRecord, MonsterImpulseYRecord), ForceMode2D.Impulse);
                }
                else
                {
                    Rigid2D.AddForce(new Vector2(-MonsterImpulseXRecord, MonsterImpulseYRecord), ForceMode2D.Impulse);
                }
            }
        }

        ResetMonsterAtkData();
    }

    private void UIControll()
    {
        //血量
        HpUI.transform.localScale = new Vector3((float)Hp / (float)MaxHp, HpUI.transform.localScale.y, HpUI.transform.localScale.z);
        //精力條
        SkillPowerUI.transform.localScale = new Vector3((float)_battleSystem.SkillPower / (float)_battleSystem.TrueMaxSkillPower, SkillPowerUI.transform.localScale.y, SkillPowerUI.transform.localScale.z);
        //殺戮值
        //KillerPointUI.transform.localScale = new Vector3((float)BattleSystem.KillerPoint / (float)_battleSystem.MaxKillerPoint, KillerPointUI.transform.localScale.y, KillerPointUI.transform.localScale.z);
        //AtkBuff
        if (_battleSystem.isAtkBuff)
        {
            AtkBuffUI.SetActive(true);
        }
        else
        {
            AtkBuffUI.SetActive(false);
        }
        //DefendBuff
        if (_battleSystem.isDefendBuff)
        {
            DefendBuffUI.SetActive(true);
        }
        else
        {
            DefendBuffUI.SetActive(false);
        }
        //PowerSeal
        if (_battleSystem.isPowerSeal)
        {
            PowerSealUI.SetActive(true);
        }
        else
        {
            PowerSealUI.SetActive(false);
        }
    }

    private void GravityJudge()
    {
        if (isDash || HurtingByMoveAtk || ShouldIgnoreGravity)
        {
            IgnoreGravity = true;
        }
        else
        {
            IgnoreGravity = false;
        }

        if (!isGround)
        {
            if (IgnoreGravity)
            {
                Rigid2D.gravityScale = 0;
            }
            else
            {
                Rigid2D.gravityScale = 7;
            }
            Rigid2D.drag = 5;
        }
    }

    private void DebugBoolSet()//debug專用
    {
        
    }
}