using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Creature;
using static PlayerCommandManager;

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
    private float _deltaTime;
    private float _fixedDeltaTime;
    private float _time;

    [Header("系統參數")]
    public static Player _player = new Player();
    private PlayerAniController _aniController;
    private PlayerBuffManager _buffManager;
    [HideInInspector] public Transform _transform;
    private Rigidbody2D Rigid2D;//有被其他script用到(ghostAtk，normalMonsterAtk，Fireball，BattleSystem, playerUnderJudgement)
    [HideInInspector] public BoxCollider2D _boxCollider;//有被其他script用到(dashJudgement)
    private BattleSystem _battleSystem;
    private OldPlayerAnimationController _OldAniController;
    private PlayerSpecialAni _specialAni;
    private PlayerCapturedAnimation _captruedController;
    private PlayerSE _playerSE;
    public PlayerTouchJudgement _playerTouchJudgement;
    public SlopeControll _slopeControll;
    public ItemManage _itemManage;
    private GameObject CommonCollider;
    public Image HpUI;
    public Image SkillPowerUI;//有被其他script用到(PlayerSpecialAni)
    public Image SkillPowerUI2;
    public Image KillerPointUI;
    public GameObject AtkBuffUI;
    public GameObject DefendBuffUI;
    public GameObject PowerSealUI;
    public float HurtedInvincibleTimerSet;//script(battleSystem)
    [HideInInspector] public float HurtedInvincibleSparkTimerSet = 0.2f;
    public float BlockAtkInvicibleTimerSet;
    public float HurtedTimerSet;
    public float DieTimerSet;//有被其他script用到(BackGroundSystem)
    [HideInInspector] public float DieTimer;//有被其他script用到(BackGroundSystem)
    private float RestoreTimer;
    public float RestoreTimerSet;
    private float LongFallTimerSet = 1.1f;
    private float LongFallWaitTimerSet = 1;
    [HideInInspector] public int JumpCount;
    private RaycastHit2D GroundCheck;
    private RaycastHit2D SpecialGroundCheck;
    private RaycastHit2D CeilingCheck;
    private RaycastHit2D SpecialCeilingCheck;

    private PlayerData _PlayerData;
    private Vector3 PlayerFinalStandPlace = new Vector3();

    //UI
    private PlayerDieController DieUI;
    private GameObject Item;
    private ItemWindow _itemWindow;
    private InputManager _inputManager;

    //開關
    public static bool isGround;//是否接觸地面 其他script有用到(BattleSystem，EvilKingController)
    [HideInInspector] public bool isUpArrowPressed;//其他script有用到(BackgroundSystem)
    public static bool isDie;//有被其他script用到(fadeoutnormal，AimpowerController，predictPowerBase，BackGroundSystem，checkPoint，MonsterBurnController)
    public static bool CanInteract;//有被其他script用到(interactableObject)
    [HideInInspector] public bool isRestore;//有被其他script用到(battleSystem)
    [HideInInspector] public bool isImpulse;//進入地圖時的推力  有被其他script用到(backgroundsystem，keycodeManage)
    [HideInInspector] public bool isDash;//是不是dash中，有被其他script用到(dogAtk，BattleSystem，ghostController)
    [HideInInspector] public bool isSaveGame = false;//有被其他script用到(CheckPoint，RestPlace)
    [HideInInspector] public bool touchLeftWall;//有被其他script用到(backgroundsystem，DashJudgement，playerleftJudgement)
    [HideInInspector] public bool touchRightWall;//有被其他script用到(backgroundsystem，DashJudgement，playerRightJudgement)
    [HideInInspector] public bool CantDoAnyThing;//不能做其他事的開關 有被其他script用到(battleSystem，moveAtk，restoreSpace)
    [HideInInspector] public bool isWalking;//是不是在走路 (invincibleTime)
    [HideInInspector] public bool isCeiling;
    private bool isIgnoreGravity;//是否無視重力
    [HideInInspector] public bool isSecondJump;
    [HideInInspector] public bool isJump;
    [HideInInspector] public bool isFall;
    [HideInInspector] public bool isSecondFall;
    [HideInInspector] public bool TouchNormalGround;
    [HideInInspector] public bool TouchGrassGround;
    [HideInInspector] public bool TouchMetalGround;
    [HideInInspector] public bool CanDash;
    private bool RestoreTimerSwitch;
    private bool isDieUIAppear;
    private bool DieByCapture;
    private bool RestoreUsed;

    //戰鬥相關開關
    public static bool isHurted;//有被其他script用到(initializeColor，battleSystem，playerBlockJudgement，AimpowerController，predictPowerBase，itemWindow，itemButton，DocumentDetail)
    public InvincibleManager _invincibleManager;
    [HideInInspector] public bool HurtedInvincible;//被怪物攻擊無敵時間
    [HideInInspector] public bool isBlockAtkInvincible;//script(battleSystem)

    [Header("閃避參數")]
    public float DashTime;
    public float DashSpeed;
    public float DashCoolDown;
    public float DashEndPushPower;
    private float LastDashTime = -10;

    public HashSet<PlayerStatus> UpdateOperateCommands = new HashSet<PlayerStatus>();
    public HashSet<PlayerStatus> FixedUpdateOperateCommands = new HashSet<PlayerStatus>();

    public HashSet<Buff> RunningBuffs = new HashSet<Buff>();
    //狀態
    private PlayerWaitStatus _waitStatus;
    private PlayerGlidingStatus _glidingStatus;
    private PlayerFallWaitStatus _fallWaitStatus;
    private PlayerRightMoveStatus _rightMoveStatus;
    private PlayerLeftMoveStatus _leftMoveStatus;
    private PlayerDashStatus _dashStatus;
    private PlayerAcumulateStatus _accumulateStatus;
    private PlayerNormalAtkStatus _normalAtkStatus;
    private PlayerStrongAtkStatus _strongAtkStatus;
    private PlayerJumpAtkStatus _jumpAtkStatus;
    private PlayerJumpStatus _jumpStatus;
    private PlayerLeftWalkThrowStatus _leftWalkThrow;
    private PlayerRightWalkThrowStatus _rightWalkThrow;
    private PlayerJumpThrowStatus _jumpThrowStatus;
    private PlayerUseNormalItemStatus _useNormalItemStatus;
    private PlayerUseThrowItemStatus _throwItemStatus;
    private PlayerShootStatus _shootStatus;
    private PlayerBlockStatus _blockStatus;
    private PlayerHurtedStatus _hurtedStatus;
    private PlayerDieStatus _dieStatus;

    private PlayerUseItemStart _useItemStart;
    private PlayerAimStop _aimStop;
    private PlayerChangeItem _changeItem;
    private PlayerMoveStop _rightMoveStop;
    private PlayerMoveStop _leftMoveStop;
    private PlayerAcumulateStop _accumulateStop;
    private PlayerJumpStop _jumpStop;

    private PlayerAimLineAnimation _aimLineAnimation;

    public AnimationController NowPlayingAni;

    public GameObject AccumulateLight;
    public GameObject SecondJumpFire;
    public GameObject AimCocktailImage;
    public GameObject AimExplosionBottleImage;
    public GameObject WalkThrowCocktailImage;
    public GameObject WalkThrowExplosionBottleImage;
    public GameObject JumpThrowCocktailImage;
    public GameObject JumpThrowExplosionBottleImage;
    public Transform RitualSwordParticle;

    private float SecondJumpTimerSet = 0.66f;

    public TestGameSpeed _gameSpeed;

    void Start()
    {
        DebugBoolSet();
        Hp = MaxHp;
        _transform = transform;
        _itemManage.RestoreUseItem();
        Rigid2D = this.gameObject.GetComponent<Rigidbody2D>();
        _boxCollider = this.gameObject.GetComponent<BoxCollider2D>();
        _battleSystem = this.gameObject.GetComponent<BattleSystem>();
        _OldAniController = this.GetComponent<OldPlayerAnimationController>();
        _specialAni = this.GetComponent<PlayerSpecialAni>();
        _captruedController = this.GetComponent<PlayerCapturedAnimation>();
        _playerSE = _transform.GetChild(3).GetComponent<PlayerSE>();
        CommonCollider = _transform.GetChild(4).gameObject;

        DieTimer = DieTimerSet;
        if (GameEvent.AbsorbBoss2)
        {
            SkillPowerUI = SkillPowerUI2;
        }

        if (GameObject.FindGameObjectWithTag("UI") != null)
        {
            Transform UICanvas = GameObject.FindGameObjectWithTag("UI").transform;

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

        NewPlayerDataInisialize();
    }

    void Update()
    {
        //測試
        if (Input.GetKeyDown(KeyCode.L) && GameEvent.OpenCheat)
        {
            _gameSpeed.ChangeGameSpeed();
            print("測試專用變速");
        }
        if (Input.GetKeyDown(KeyCode.K) && GameEvent.OpenCheat)
        {
            BackgroundSystem.BasicGameSpeed = 1;
            print("測試專用回復速度");
        }

        _deltaTime = Time.deltaTime;
        _time = Time.time;

        HPJudge();
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

            TouchGroundSoundJudge(GroundCheck);
            TouchGroundSoundJudge(SpecialGroundCheck);
            TouchGroundSoundJudge(_slopeControll.RightGroundRaycast);
            TouchGroundSoundJudge(_slopeControll.LeftGroundRaycast);
        }
        else
        {
            isGround = false;
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
        AutoGravityJudge();

        if (isGround && isImpulse)
        {
            isImpulse = false;
        }

        UIControll();
        
        //常態判定框控制
        if (_invincibleManager.GetInvincible(InvincibleManager.InvincibleType.Strong))
        {
            CommonCollider.SetActive(false);
        }
        else
        {
            CommonCollider.SetActive(true);
        }

        JudgeMoveDirection();
        PlayAnimation();

        if (_time >= (LastDashTime + DashCoolDown))
        {
            CanDash = true;
        }

        //跳躍判斷
        JumpCountJudge();
        //重置
        if (Portal.isPortal || PauseMenuController.OpenAnyMenu || GameEvent.isAniPlay || isImpulse || _battleSystem.isCaptured || RestPlace.isOpenRestPlace)
        {
            if (!PauseMenuController.isPauseMenuOpen)
            {
                //蓄力
                _battleSystem.isAccumulate = false;
                _battleSystem.AccumulateTimerSwitch = false;
                _battleSystem.isAccumulateComplete = false;
                //回復
                RestoreUsed = false;
                isRestore = false;
                RestoreTimerSwitch = false;
                //雜項
                _boxCollider.isTrigger = false;
                CantDoAnyThing = false;
                _playerTouchJudgement.isLeftSideHaveMonster = false;
                _playerTouchJudgement.isRightSideHaveMonster = false;
                _playerTouchJudgement.isMonsterUnder = false;
            }

            if (!Portal.isPortal && !isImpulse)
            {
                RestoreGravity();
            }

            if (Portal.isPortal || RestPlace.isOpenRestPlace)
            {
                _OldAniController.WaitAniPlay();
            }

            _playerSE.TurnOffLoopSE();

            if (isImpulse || PauseMenuController.OpenAnyMenu  || GameEvent.isAniPlay || _battleSystem.isCaptured)
            {
                isUpArrowPressed = false;
            }
            return;
        }

        AddDefaultMode();
        OperateCommand(UpdateOperateCommands, _deltaTime);
        OperateBuff(RunningBuffs, _deltaTime);

        //是否互進行互動判斷
        if (isUpArrowPressed && !_battleSystem.isAim)
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
        if (Input.GetKeyDown(KeyCode.M) && GameEvent.OpenCheat)
        {
            _itemManage.CocktailNumber += 1;
            _itemManage.ExplosionBottleNumber += 1;
            _battleSystem.SkillPower += 900;
            GameEvent.TutorialComplete = true;
            BattleSystem.NormalAtkHurtPower = 100;
        }
        if (Input.GetKey(KeyCode.H) && Input.GetKey(KeyCode.P) && GameEvent.OpenCheat)
        {
            MaxHp = 2000;
            Hp = 2000;
        }
        if (Input.GetKey(KeyCode.I) && Input.GetKey(KeyCode.M))
        {
            GameEvent.OpenCheat = true;
        }
        if (Input.GetKeyDown(KeyCode.O) && Input.GetKey(KeyCode.R))
        {
            MaxHp = 200;
            BattleSystem.NormalAtkHurtPower = 10;
            GameEvent.OpenCheat = false;
        }
    }

    private void FixedUpdate()
    {
        if (Portal.isPortal ||  PauseMenuController.OpenAnyMenu || isImpulse ||  _battleSystem.isCaptured || RestPlace.isOpenRestPlace)
        {
            return;
        }

        _fixedDeltaTime = Time.fixedDeltaTime;

        FixedOperateCommand(FixedUpdateOperateCommands, _fixedDeltaTime);

        RestoreTimerMethod();
    }

    private void JudgeMoveDirection()
    {
        bool ShouldReset = false;

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
            if (_player.face == Face.Left && _slopeControll.RightDetecterOnLeftSlope)
            {
                ShouldReset = true;
            }
            if (_player.face == Face.Right && _slopeControll.LeftDetecterOnRightSlope)
            {
                ShouldReset = true;
            }
        }

        //Dash特殊狀況重置
        if (isDash)
        {
            switch (_player.face)
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
            switch (_player.face)
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
            if (_player.face == Face.Right)
            {
                MoveDirection = new Vector2(_slopeControll.RightDirection.x, -_slopeControll.RightDirection.y);
            }
        }

        //右斜坡
        if (_slopeControll.onRightSlope)
        {
            switch (_player.face)
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
            if (_player.face == Face.Left)
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
    private bool RightMoveJudge()
    {
        if (!touchRightWall && !_playerTouchJudgement.isRightSideHaveMonster)
        {
            return true;
        }

        return false;
    }
    private bool LeftMoveJudge()
    {
        if (!touchLeftWall && !_playerTouchJudgement.isLeftSideHaveMonster)
        {
            return true;
        }

        return false;
    }
    private void Move(float speed, float deltaTime)
    {
        if(speed > 0 && !RightMoveJudge())
        {
            return;
        }
        if (speed < 0 && !LeftMoveJudge())
        {
            return;
        }

        _transform.position = new Vector3(speed * MoveDirection.x * deltaTime + _transform.position.x, speed * MoveDirection.y * deltaTime + _transform.position.y, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "monsterAtk" && _invincibleManager.GetInvincible(InvincibleManager.InvincibleType.Strong))
        {
            GetHurted(collision.transform?.GetComponent<NormalMonsterAtk>(), collision.transform);
        }//處於強力無敵時才使用這個
        if (collision.GetComponent<DashJudgement>() != null)
        {
            _boxCollider.isTrigger = false;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Portal" && !_battleSystem.isCriticAtk && !GameEvent.isAniPlay && !Portal.isPortal)
        {
            collision.gameObject.GetComponent<Portal>().BeginChangeScene();
            IgnoreGravity(true);
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

    public void ReadyToDash()
    {
        IgnoreGravity(false);
        _boxCollider.isTrigger = true;
    }
    public void DashEnd()
    {
        CanDash = false;
        LastDashTime = _time;

        switch (_player.face)
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

        _boxCollider.isTrigger = false;
        RestoreGravity();
    }

    /*void Die()
    {
        if (Hp <= 0)
        {
            if (_battleSystem.isCaptured)
            {
                DieByCapture = true;
            }

            if (!DieByCapture)
            {
                _OldAniController.DieAniPlay();
            }
            else
            {
                _OldAniController.WeakDieAniPlay();
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
    }*/
    private void HPJudge()
    {
        //避免為負or超過
        if (Hp <= 0)
        {
            _dieStatus.AddCommandToSet();
            Hp = 0;
        }
        if (Hp > MaxHp)
        {
            Hp = MaxHp;
        }
    }
    private void OperateCommand(HashSet<PlayerStatus> Set, float deltaTime)
    {
        if (Set.Count == 0)
        {
            return;
        }

        Queue<PlayerStatus> OperateQueue = new Queue<PlayerStatus>();
        foreach (var item in Set)
        {
            OperateQueue.Enqueue(item);
        }

        while (OperateQueue.Count > 0)
        {
            OperateQueue.Peek().Execute(deltaTime);
            OperateQueue.Dequeue();
        }
    }
    private void FixedOperateCommand(HashSet<PlayerStatus> Set, float deltaTime)
    {
        if(Set.Count == 0)
        {
            return;
        }

        Queue<PlayerStatus> OperateQueue = new Queue<PlayerStatus>();
        foreach (var item in Set)
        {
            OperateQueue.Enqueue(item);
        }

        while (OperateQueue.Count > 0)
        {
            OperateQueue.Peek().FixedExecute(deltaTime);
            OperateQueue.Dequeue();
        }
    }
    private void OperateBuff(HashSet<Buff> Set, float deltaTime)
    {
        if (Set.Count == 0)
        {
            return;
        }

        Queue<Buff> OperateQueue = new Queue<Buff>();
        foreach (var item in Set)
        {
            OperateQueue.Enqueue(item);
        }

        while (OperateQueue.Count > 0)
        {
            OperateQueue.Peek().Execute(deltaTime);
            OperateQueue.Dequeue();
        }
    }

    public bool CheckOperateSetEmpty()
    {
        if(UpdateOperateCommands.Count == 0 && FixedUpdateOperateCommands.Count == 0)
        {
            return true;
        }

        return false;
    }
    public bool CheckCommandApply(PlayerStatus.Status Applicant)
    {
        Queue<PlayerStatus> RemoveQueue = new Queue<PlayerStatus>();

        if (UpdateOperateCommands.Count > 0)
        {
            foreach (var item in UpdateOperateCommands)
            {
                if (item.CommandReplaceSet.Contains(Applicant))
                {
                    RemoveQueue.Enqueue(item);
                }
                if (!item.CommandReplaceSet.Contains(Applicant) && !item.CommandCoexistSet.Contains(Applicant))
                {
                    return false;
                }
            }
        }
        if (FixedUpdateOperateCommands.Count > 0)
        {
            foreach (var item in FixedUpdateOperateCommands)
            {
                if (item.CommandReplaceSet.Contains(Applicant))
                {
                    RemoveQueue.Enqueue(item);
                }
                if (!item.CommandReplaceSet.Contains(Applicant) && !item.CommandCoexistSet.Contains(Applicant))
                {
                    return false;
                }
            }
        }

        while (RemoveQueue.Count > 0)
        {
            RemoveQueue.Peek().RemoveCommandFromSet();
            RemoveQueue.Dequeue();
        }

        return true;
    }
    public void AddDefaultMode()
    {
        if (isGround)
        {
            _waitStatus.AddCommandToSet();
        }
        else
        {
            _glidingStatus.AddCommandToSet();
        }
    }

    private void JumpCountJudge()
    {
        if (isGround && !UpdateOperateCommands.Contains(_jumpStatus))
        {
            JumpCount = 2;
        }

        if(!isGround && JumpCount == 2)
        {
            JumpCount--;
        }
    }
    public bool JumpBreakJudge()
    {
        if (isCeiling)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Restore()
    {
        if (Portal.isPortal || isDie || PauseMenuController.OpenAnyMenu || GameEvent.isAniPlay || isImpulse || _battleSystem.isCaptured || RestPlace.isOpenRestPlace) return;

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
    private void OpenItemWindow()
    {
        if (Portal.isPortal || isDie || PauseMenuController.OpenAnyMenu || GameEvent.isAniPlay || isImpulse || _battleSystem.isCaptured || RestPlace.isOpenRestPlace) return;

        if (!AnnouncementController.isOpenAnnouncement && GameEvent.TutorialComplete)
        {
            _itemWindow.isItemWindowAppear = true;
            PauseMenuController.OpenAnyMenu = true;
            BackgroundSystem.CantPause = true;
            Item.SetActive(true);
        }
    }
    private void Interact()
    {
        isUpArrowPressed = true;
    }
    private void InteractEnd()
    {
        isUpArrowPressed = false;
    }

    public void PlayAnimation()
    {
        NowPlayingAni?.AniPlay();
    }
    public void SetAnimation(AnimationController _ani)
    {
        if (NowPlayingAni == null) 
        {
            NowPlayingAni = _ani;
        }
        else if(_ani.PlayPriority > NowPlayingAni.PlayPriority)
        {
            NowPlayingAni.AniStop();
            NowPlayingAni = _ani;
        }
    }
    public void StopAnimation(AnimationController _ani)
    {
        AnimationController newAni = null;
        int priority = -1;

        if (NowPlayingAni == _ani)
        {
            if (UpdateOperateCommands.Count > 0)
                foreach (var item in UpdateOperateCommands)
                {
                    if(item is IPlayerAniUser AniUser && AniUser.GetAnimationPriority() > priority)
                    {
                        newAni = AniUser.GetAnimation();
                        priority = AniUser.GetAnimationPriority();
                    }
                }
            if (FixedUpdateOperateCommands.Count > 0)
                foreach (var item in FixedUpdateOperateCommands)
                {
                    if (item is IPlayerAniUser AniUser && AniUser.GetAnimationPriority() > priority)
                    {
                        newAni = AniUser.GetAnimation();
                    }
                }

            NowPlayingAni = newAni;
        }

        _ani.AniStop();
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

    public void HurtedByCaptureAtk(CaptureAtk _Atk)
    {
        _boxCollider.isTrigger = true;
        _battleSystem.isCaptured = true;

        _captruedController.InisializeCaptureAni(_Atk._basicData, _Atk._captureController);
    }
    public void HurtedByCaptureAtkEnd()
    {
        _captruedController._monsterCaptureController = null;
        _battleSystem.isCaptured = false;
        _battleSystem.isWeak = true;
        _battleSystem.WeakTimerSwitch = true;
        _boxCollider.isTrigger = false;
        HurtedInvincible = true;
    }

    public void GetHurted(NormalMonsterAtk _atk, Transform AtkTransform)
    {
        var data = new MonsterAtkData(_atk.ImpulsePowerX, _atk.ImpulsePowerY, _atk.Damage, _atk.CanBeBlock,
            _atk.NoAvoid, _atk.Type, AtkTransform.GetComponent<MoveAtk>(), AtkTransform.position);

        HurtedJudge(data);
    }
    private void HurtedJudge(MonsterAtkData data)
    {
        if (_invincibleManager.GetInvincible(InvincibleManager.InvincibleType.Absolute))
        {
            return;
        }

        if (_battleSystem.isDefendBuff)
        {
            data.MonsterDamageRecord = (int)(data.MonsterDamageRecord * 0.9f);
        }
        //無視強力無敵區域
        if (data.MonsterMoveAtkRecord)
        {
            _hurtedStatus.MoveAtk = true;
            IgnoreGravity(true);
            HurtedControll(data);
            return;
        }

        //-------------------------
        if (_invincibleManager.GetInvincible(InvincibleManager.InvincibleType.Strong))
        {
            return;
        }

        if (_invincibleManager.GetInvincible(InvincibleManager.InvincibleType.Weak) && !data.MonsterNoAvoidRecord)
        {
            return;
        }

        //格檔減傷計算
        if (_battleSystem.isBlockActualAppear && data.MonsterCanBeBlockRecord)
        {
            switch (_player.face)
            {
                case Face.Left:
                    if (data.MonsterAtkPlaceRecord.x >= _transform.position.x)
                    {
                        data.MonsterDamageRecord = data.MonsterDamageRecord / 2;
                    }
                    break;
                case Face.Right:
                    if (data.MonsterAtkPlaceRecord.x <= _transform.position.x)
                    {
                        data.MonsterDamageRecord = data.MonsterDamageRecord / 2;
                    }
                    break;
            }
        }
        else if (_battleSystem.isBlock)
        {
            data.MonsterDamageRecord = data.MonsterDamageRecord / 2;
        }

        if (data.MonsterAtkPlaceRecord.x > _transform.position.x)
        {
            data.MonsterImpulseXRecord = -data.MonsterImpulseXRecord;
        }

        HurtedControll(data);
    }
    private void HurtedControll(MonsterAtkData data)
    {
        Hp -= data.MonsterDamageRecord;
        HurtedInvincible = true;
        _hurtedStatus.AddCommandToSet();
        _buffManager.strongInvincibleBuff.AddBuffToSet();
        _playerSE.HurtedSoundPlay(data.MonsterAtkTypeRecord);
        Rigid2D.AddForce(new Vector2(data.MonsterImpulseXRecord, data.MonsterImpulseYRecord), ForceMode2D.Impulse);
    }
    public void MoveTrapComplete()
    {
        _transform.position = PlayerFinalStandPlace;
        RestoreGravity();
        _hurtedStatus.RemoveCommandFromSet();
    }

    private void UIControll()
    {
        //血量
        HpUI.transform.localScale = new Vector3((float)Hp / (float)MaxHp, HpUI.transform.localScale.y, HpUI.transform.localScale.z);
        //精力條
        SkillPowerUI.transform.localScale = new Vector3((float)_battleSystem.SkillPower / (float)_battleSystem.TrueMaxSkillPower, SkillPowerUI.transform.localScale.y, SkillPowerUI.transform.localScale.z);
        //殺戮值
        //KillerPointUI.transform.localScale = new Vector3((float)BattleSystem.KillerPoint / (float)_battleSystem.MaxKillerPoint, KillerPointUI.transform.localScale.y, KillerPointUI.transform.localScale.z);
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

    public float GetYVelocity()
    {
        return Rigid2D.velocity.y;
    }
    public void SetVelocity(Vector2 velocity)
    {
        Rigid2D.velocity = velocity;
    }
    public void ForceRigidBody(Vector2 Power, ForceMode2D mode)
    {
        Rigid2D.AddForce(Power, mode);
    }
    public void IgnoreGravity(bool AbsoluteStop)
    {
        isIgnoreGravity = true;
        Rigid2D.gravityScale = 0;
        if (AbsoluteStop)
        {
            Rigid2D.velocity = new Vector3(0, 0, 0);
        }
        else
        {
            Rigid2D.velocity = new Vector3(Rigid2D.velocity.x, 0, 0);
        }
    }
    public void RestoreGravity()
    {
        isIgnoreGravity = false;
        Rigid2D.gravityScale = 7;
    }
    private void AutoGravityJudge()
    {
        bool ShouldChange = false;
        (int, int) ChangeValue = (7,5);

        if (!isGround && !isIgnoreGravity && Rigid2D.gravityScale != 7)
        {
            ShouldChange = true;
            ChangeValue = (7, 5);
        }
        if (isGround && Rigid2D.gravityScale != 0)
        {
            ShouldChange = true;
            ChangeValue = (0, 20);
        }

        if (ShouldChange)
        {
            Rigid2D.gravityScale = ChangeValue.Item1;
            Rigid2D.drag = ChangeValue.Item2;
        }
    }

    private void DebugBoolSet()//debug專用
    {
        
    }

    public void NewRecordMonsterAtkData(MonsterAtk _atk, Transform AtkTransform)
    {
        /*MonsterImpulseXRecord = _atk.ImpulsePowerX;
        MonsterImpulseYRecord = _atk.ImpulsePowerY;
        MonsterDamageRecord = _atk.Damage;
        ShouldJudgeHurt = true;
        MonsterCanBeBlockRecord = _atk.CanBeBlock;
        MonsterNoAvoidRecord = _atk.NoAvoid;
        switch (_atk.Type)
        {
            case MonsterAtk.AtkType.Normal:
                MonsterAtkTypeRecord = NormalMonsterAtk.AtkType.Normal;
                break;
            case MonsterAtk.AtkType.Laser:
                MonsterAtkTypeRecord = NormalMonsterAtk.AtkType.Laser;
                break;
            case MonsterAtk.AtkType.NoSound:
                MonsterAtkTypeRecord = NormalMonsterAtk.AtkType.NoSound;
                break;
        }
        if (AtkTransform.GetComponent<MoveAtk>() != null)
        {
            MonsterMoveAtkRecord = true;
        }
        MonsterAtkPlaceRecord = AtkTransform.position;*/
    }

    public void NewPlayerDataInisialize()
    {
        if (GameObject.Find("FollowSystem") != null)
        {
            _inputManager = GameObject.Find("FollowSystem").GetComponent<InputManager>();
        }

        _aniController = new PlayerAniController(_transform.GetChild(0), 0.28f);
        _aniController.useItemAni.AssignParticle(RitualSwordParticle);
        _aniController.throwItemAni.AssignImage(AimCocktailImage, AimExplosionBottleImage);
        _aniController.walkThrowAni.AssignImage(WalkThrowCocktailImage, WalkThrowExplosionBottleImage);
        _aniController.jumpThrowAni.AssignImage(JumpThrowCocktailImage, JumpThrowExplosionBottleImage);
        _invincibleManager = new InvincibleManager();
        _buffManager = new PlayerBuffManager(this, _battleSystem);

        _waitStatus = new PlayerWaitStatus(this, _aniController.waitAni);
        _fallWaitStatus = new PlayerFallWaitStatus(this, _aniController.jumpAni, LongFallWaitTimerSet);
        _glidingStatus = new PlayerGlidingStatus(this, _aniController.jumpAni, _fallWaitStatus, LongFallTimerSet);
        _hurtedStatus = new PlayerHurtedStatus(this, _aniController.hurtedAni);
        _dieStatus = new PlayerDieStatus(this, _aniController.dieAni, _invincibleManager, _PlayerData, DieUI);

        _rightMoveStatus = new PlayerRightMoveStatus(this, _aniController.runAni, Speed, Move);
        _leftMoveStatus = new PlayerLeftMoveStatus(this, _aniController.runAni, Speed, Move);
        _rightMoveStop = new PlayerMoveStop(_rightMoveStatus);
        _leftMoveStop = new PlayerMoveStop(_leftMoveStatus);
        _jumpStatus = new PlayerJumpStatus(this, _aniController.jumpAni, SecondJumpFire, JumpBreakJudge, JumpForce, SpeedlimitY, SecondJumpTimerSet);
        _jumpStop = new PlayerJumpStop(_jumpStatus);
        _dashStatus = new PlayerDashStatus(this, _aniController.dashAni, Move);

        _shootStatus = new PlayerShootStatus(this, _battleSystem, _leftMoveStatus, _rightMoveStatus, _aniController.shootAni);
        _normalAtkStatus = new PlayerNormalAtkStatus(this,_battleSystem, _aniController.normalAtkAni);
        _jumpAtkStatus = new PlayerJumpAtkStatus(this, _battleSystem, _aniController.normalAtkAni);
        _strongAtkStatus = new PlayerStrongAtkStatus(this, _battleSystem, _aniController.strongAtkAni, _battleSystem.CAtkTimerSet, _battleSystem.JumpCAtkTimerSet,
            _battleSystem.CAtk, _battleSystem.JumpCAtk, IgnoreGravity, RestoreGravity, _battleSystem.StrongAtkCost);

        _accumulateStatus = new PlayerAcumulateStatus(this, _battleSystem.AccumulateTimerSet, AccumulateLight);
        _accumulateStop = new PlayerAcumulateStop(_battleSystem, _accumulateStatus, _normalAtkStatus, _jumpAtkStatus, _normalAtkStatus);
        _blockStatus = new PlayerBlockStatus(this, _battleSystem, _aniController.blockAni);

        _changeItem = new PlayerChangeItem(_itemManage);
        _aimLineAnimation = new PlayerAimLineAnimation(_battleSystem);
        _leftWalkThrow = new PlayerLeftWalkThrowStatus(this, _aniController.walkThrowAni, _battleSystem);
        _rightWalkThrow = new PlayerRightWalkThrowStatus(this, _aniController.walkThrowAni, _battleSystem);

        _useNormalItemStatus = new PlayerUseNormalItemStatus(this, _aniController.useItemAni);
        _throwItemStatus = new PlayerUseThrowItemStatus(this, _aniController.throwItemAni, _aimLineAnimation, _inputManager, _battleSystem);
        _jumpThrowStatus = new PlayerJumpThrowStatus(this, _aniController.jumpThrowAni, _battleSystem);
        _useItemStart = new PlayerUseItemStart(_itemManage, _useNormalItemStatus, _throwItemStatus, _leftWalkThrow, _rightWalkThrow, _jumpThrowStatus, _inputManager);
        _aimStop = new PlayerAimStop(_throwItemStatus);

        _itemManage.InisializeItemClass(_battleSystem, _useNormalItemStatus, _buffManager);

        if (_inputManager != null)
        {
            _inputManager.SubscribeCommand(Command.RightMove, CommandType.Pressing, _rightMoveStatus);
            _inputManager.SubscribeCommand(Command.RightMove, CommandType.Up, _rightMoveStop);
            _inputManager.SubscribeCommand(Command.LeftMove, CommandType.Pressing, _leftMoveStatus);
            _inputManager.SubscribeCommand(Command.LeftMove, CommandType.Up, _leftMoveStop);
            _inputManager.SubscribeCommand(Command.NormalAtk, CommandType.Pressing, _accumulateStatus);
            _inputManager.SubscribeCommand(Command.NormalAtk, CommandType.Up, _accumulateStop);
            _inputManager.SubscribeCommand(Command.StrongAtk, CommandType.Pressed, _strongAtkStatus);
            _inputManager.SubscribeCommand(Command.Jump, CommandType.Pressed, _jumpStatus);
            _inputManager.SubscribeCommand(Command.Jump, CommandType.Up, _jumpStop);
            _inputManager.SubscribeCommand(Command.UseItem, CommandType.Pressed, _useItemStart);
            _inputManager.SubscribeCommand(Command.UseItem, CommandType.Up, _aimStop);
            _inputManager.SubscribeCommand(Command.ChangeItem, CommandType.Pressed, _changeItem);
            _inputManager.SubscribeCommand(Command.Dash, CommandType.Pressed, _dashStatus);
            _inputManager.SubscribeCommand(Command.Shoot, CommandType.Pressed, _shootStatus);
            _inputManager.SubscribeCommand(Command.Block, CommandType.Pressed, _blockStatus);

            /*
            _playerCommandManager.SubscribeCommand(PlayerCommandManager.Command.Restore, PlayerCommandManager.CommandType.Pressed, Restore);
            _playerCommandManager.SubscribeCommand(PlayerCommandManager.Command.ItemWindow, PlayerCommandManager.CommandType.Pressed, OpenItemWindow);
            _playerCommandManager.SubscribeCommand(PlayerCommandManager.Command.Interact, PlayerCommandManager.CommandType.Pressed, Interact);
            _playerCommandManager.SubscribeCommand(PlayerCommandManager.Command.Interact, PlayerCommandManager.CommandType.Up, InteractEnd);*/
        }

        AddDefaultMode();
    }
}

public struct MonsterAtkData
{
    public float MonsterImpulseXRecord;
    public float MonsterImpulseYRecord;
    public int MonsterDamageRecord;
    public bool MonsterCanBeBlockRecord;
    public bool MonsterNoAvoidRecord;
    public NormalMonsterAtk.AtkType MonsterAtkTypeRecord;
    public bool MonsterMoveAtkRecord;
    public Vector3 MonsterAtkPlaceRecord;

    public MonsterAtkData(float powerX, float powerY, int damage, bool canBeBlock, bool avoid, 
        NormalMonsterAtk.AtkType type, bool moveAtk, Vector3 place)
    {
        MonsterImpulseXRecord = powerX;
        MonsterImpulseYRecord = powerY;
        MonsterDamageRecord = damage;
        MonsterCanBeBlockRecord = canBeBlock;
        MonsterNoAvoidRecord = avoid;
        MonsterAtkTypeRecord = type;
        MonsterMoveAtkRecord = moveAtk;
        MonsterAtkPlaceRecord = place;
    }
}