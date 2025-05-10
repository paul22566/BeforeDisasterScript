using UnityEngine;

public class DogController : MonoBehaviour
{
    [Header("基本參數")]
    public int ChasingDistanceX;
    public float ChasingDistanceY;
    public int GiveUpDistanceX;
    public int GiveUpDistanceY;
    public float AtkDistance;
    public float speed;
    private enum Status {Standby, Wait, walk, Atk, AtkWait, jump };
    private Status status;

    public enum AniStatus { Wait, Walk, Atk, AtkWait, Null, Stop }
    [HideInInspector] public AniStatus NowAni;//script(hurted)
    private AniStatus LastAni;

    [Header("系統參數")]
    private Transform _transform;
    private float _deltaTime;
    private float _fixedDeltaTime;
    private float _time;
    private Transform TemporaryArea;
    private GameObject MonsterRightJudgement;
    private GameObject MonsterLeftJudgement;
    private GameObject MonsterUnderJudgement;
    private MonsterBasicData _basicData;
    private MonsterHurtedController _hurtedController;
    private MonsterJumpController _jumpController;
    private MonsterTouchTrigger _touchTrigger;
    private DogSE _dogSE;
    private float StandbyTimer = 0.1f;
    public float AtkTimerSet;
    private float AtkTimer;
    public float AtkCoolDown;
    private float AtkLastTime = -10;
    public float LittleWaitTimerSet;
    private float LittleWaitTimer;
    private float TurnFaceLagTimerSet = 0.2f;
    private float TurnFaceLagTimer;
    private Rigidbody2D Rigid2D;
    private BoxCollider2D _boxCollider;
    private float GroundPointY;
    private RaycastHit2D RightAtkCheck;
    private RaycastHit2D LeftAtkCheck;

    //開關
    private bool FirstTrigger;
    private bool SecondTrigger;
    private bool ThirdTrigger;
    private bool isAtkTouchRightWall;
    private bool isAtkTouchLeftWall;
    private bool isAtkTwice;
    private bool isMonsterTemporaryDissappear;
    private bool CanAtk;
    private bool isLittleWait;

    [Header("動畫相關物件")]
    private Animator MonsterMoveAni;
    private GameObject MonsterMoveAnimation;
    private GameObject MonsterAtkAnimation;
    private GameObject MonsterAtkWaitAnimation;
    private SpriteRenderer MonsterMoveSprite;
    private SpriteRenderer MonsterAtkSprite;
    private SpriteRenderer MonsterAtkWaitSprite;
    public GameObject RRedAir;
    public GameObject LRedAir;

    [Header("攻擊物件")]
    public GameObject Atk;
    private Vector3 RAtkAppear = new Vector3(2.34f, 0.22f, 0);
    private Vector3 LAtkAppear = new Vector3(-2.34f, 0.22f, 0);

    private GameObject MonsterStopAnimation;
    private SpriteRenderer MonsterStopSprite;

    //音效
    private float BarkSoundTime = 0;
    private float LowBarkSoundTime = 0;
    private bool SEAppear;

    void Start()
    {
        _transform = transform;
        _basicData = this.GetComponent<MonsterBasicData>();
        _hurtedController = this.transform.GetComponent<MonsterHurtedController>();
        _jumpController = this.GetComponent<MonsterJumpController>();
        Rigid2D = this.transform.GetComponent<Rigidbody2D>();
        _boxCollider = this.transform.GetComponent<BoxCollider2D>();
        _dogSE = this.GetComponent<DogSE>();

        MonsterMoveAnimation = this.transform.GetChild(0).gameObject;
        MonsterAtkAnimation = this.transform.GetChild(1).gameObject;
        MonsterStopAnimation = this.transform.GetChild(3).gameObject;
        MonsterAtkWaitAnimation = this.transform.GetChild(4).gameObject;

        MonsterMoveSprite = MonsterMoveAnimation.GetComponent<SpriteRenderer>();
        MonsterAtkSprite = MonsterAtkAnimation.GetComponent<SpriteRenderer>();
        MonsterAtkWaitSprite = MonsterAtkWaitAnimation.GetComponent<SpriteRenderer>();
        MonsterStopSprite = MonsterStopAnimation.GetComponent<SpriteRenderer>();

        MonsterMoveAni = MonsterMoveAnimation.GetComponent<Animator>();

        TurnFaceLagTimer = TurnFaceLagTimerSet;
        TemporaryArea = this.transform.GetChild(2);
        MonsterRightJudgement = TemporaryArea.transform.GetChild(0).gameObject;
        MonsterLeftJudgement = TemporaryArea.transform.GetChild(1).gameObject;
        MonsterUnderJudgement = TemporaryArea.transform.GetChild(2).gameObject;
        TemporaryArea.transform.DetachChildren();

        if (!_basicData.SpecialOpening)
        {
            status = Status.Standby;
        }
        NowAni = AniStatus.Wait;

        _basicData.BasicVarInisialize(MonsterMoveSprite, "R");

        _hurtedController._getCriticHurted += PlayStopAni;

        //初始化MonsterTouch系統
        InisializMonsterTouchTrigger();
    }

    // Update is called once per frame
    void Update()
    {
        _deltaTime = Time.deltaTime;
        _time = Time.time;
        _basicData.DieJudge();

        //多種變數重置管理
        if (_hurtedController.isCriticAtkHurted || _hurtedController.isHurtedByBigGun)
        {
            status = Status.AtkWait;
            FirstTrigger = false;
            SecondTrigger = false;
            ThirdTrigger = false;
            isAtkTwice = false;
            AtkTimer = AtkTimerSet;
            AtkLastTime = _time;
            CanAtk = false;
            _jumpController.AllVariableFalse();
        }

        _hurtedController.HurtedTimerMethod(_deltaTime);
        _hurtedController.CriticAtkHurtedTimerMethod(_deltaTime);

        //判斷是否觸地
        _basicData.CheckTouchGround(ref GroundPointY);
        if (_basicData.isGround && !_hurtedController.isCriticAtkHurted)
        {
            _transform.position = new Vector3(_transform.position.x, GroundPointY + _basicData.GroundPlace, 0);
        }
        if (!_basicData.isGround)
        {
            Rigid2D.gravityScale = 7;
        }
        //判斷是否碰到牆壁
        _basicData.CheckTouchWall();

        RightAtkCheck = Physics2D.Raycast(_transform.position, Vector2.right, 4.78f, 128);
        if (RightAtkCheck)
        {
            isAtkTouchRightWall = true;
        }
        else
        {
            isAtkTouchRightWall = false;
        }

        LeftAtkCheck = Physics2D.Raycast(_transform.position, Vector2.left, 4.78f, 64);
        if (LeftAtkCheck)
        {
            isAtkTouchLeftWall = true;
        }
        else
        {
            isAtkTouchLeftWall = false;
        }

        //計算距離
        _basicData.DistanceCalculate();
        //冷卻時間計算
        _basicData.CoolDownCalculate(_time, AtkLastTime, AtkCoolDown, ref CanAtk);

        //被大招打之後的動作
        if (_hurtedController.BeCriticAtkEnd)
        {
            status = Status.walk;
            _hurtedController.BeCriticAtkEnd = false;
        }

        //寫入實際位置
        _basicData.MonsterPlace = _transform.position;

        //return
        if (GameEvent.isAniPlay || _hurtedController.isCriticAtkHurted || _hurtedController.isHurtedByBigGun || PauseMenuController.isPauseMenuOpen)
        {
            return;
        }

        //怪物AI
        switch (status)
        {
            case Status.Standby:
                StandbyTimer -= _deltaTime;

                if (StandbyTimer <= 0)
                {
                    status = Status.Wait;
                }
                break;
            case Status.Wait:
                if (_basicData.playerTransform)
                {
                    //被遠程打時的反應
                    if (_hurtedController.HurtedByFarAtk)
                    {
                        StatusJudge();
                        if (status == Status.Wait)
                        {
                            status = Status.walk;
                        }
                        _basicData.TurnFaceJudge();
                    }
                    if (_hurtedController.HurtedByFarAtk)
                    {
                        _hurtedController.HurtedByFarAtk = false;
                    }

                    if (_basicData.AbsDistanceX < ChasingDistanceX && _basicData.AbsDistanceY <= ChasingDistanceY)
                    {
                        _basicData.TurnFaceJudge();
                        StatusJudge();
                    }
                }
                break;
            case Status.walk:
                CanAtk = true;

                if (_basicData.ReverseWalk)
                {
                    _basicData.ReverseLagTurnFaceJudge(ref TurnFaceLagTimer, TurnFaceLagTimerSet, _deltaTime);
                }
                else
                {
                    _basicData.LagTurnFaceJudge(ref TurnFaceLagTimer, TurnFaceLagTimerSet, _deltaTime);
                }

                StatusJudge();
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
            case Status.AtkWait:
                _basicData.TurnFaceJudge();

                //稍等時間結束
                if (isLittleWait)
                {
                    if (!_touchTrigger.isMonsterInRange)
                    {
                        LittleWaitTimer -= _deltaTime;
                    }

                    if (LittleWaitTimer <= 0)
                    {
                        isLittleWait = false;
                        LittleWaitTimer = LittleWaitTimerSet;
                    }
                }
                else
                {
                    LittleWaitTimer = LittleWaitTimerSet;
                }

                StatusJudge();
                break;
        }

        if (status == Status.AtkWait)
        {
            _basicData.ShouldIgnore = true;
        }
        else
        {
            _basicData.ShouldIgnore = false;
        }

        switch (_basicData.face)
        {
            case MonsterBasicData.Face.Left:
                MonsterMoveSprite.flipX = true;
                MonsterAtkSprite.flipX = true;
                MonsterStopSprite.flipX = true;
                MonsterAtkWaitSprite.flipX = true;
                break;

            case MonsterBasicData.Face.Right:
                MonsterMoveSprite.flipX = false;
                MonsterAtkSprite.flipX = false;
                MonsterStopSprite.flipX = false;
                MonsterAtkWaitSprite.flipX = false;
                break;
        }

        SwitchAnimation();
    }
    private void FixedUpdate()
    {
        _fixedDeltaTime = Time.fixedDeltaTime;

        //return
        if (GameEvent.isAniPlay || _hurtedController.isCriticAtkHurted || _hurtedController.isHurtedByBigGun)
        {
            return;
        }

        switch (status)
        {
            case Status.Atk:
                if (AtkTimer <= 0)
                {
                    AtkTimer = AtkTimerSet;
                }

                AtkTimer -= _fixedDeltaTime;

                if (!FirstTrigger)
                {
                    switch (_basicData.face)
                    {
                        case MonsterBasicData.Face.Left:
                            Instantiate(LRedAir, _transform.position, Quaternion.identity, TemporaryArea);
                            TemporaryArea.DetachChildren();
                            break;
                        case MonsterBasicData.Face.Right:
                            Instantiate(RRedAir, _transform.position, Quaternion.identity, TemporaryArea);
                            TemporaryArea.DetachChildren();
                            break;
                    }
                    FirstTrigger = true;
                }
                if (AtkTimer <= (AtkTimerSet - 0.7))
                {
                    if (!SecondTrigger)
                    {
                        switch (_basicData.face)
                        {
                            case MonsterBasicData.Face.Left:
                                if (isAtkTouchLeftWall)
                                {
                                    isAtkTwice = true;
                                }
                                break;
                            case MonsterBasicData.Face.Right:
                                if (isAtkTouchRightWall)
                                {
                                    isAtkTwice = true;
                                }
                                break;
                        }
                        switch (_basicData.face)
                        {
                            case MonsterBasicData.Face.Right:
                                Instantiate(Atk, _transform.position + RAtkAppear, Quaternion.identity);
                                if (!isAtkTwice)
                                {
                                    _transform.position = new Vector3(_transform.position.x + 4.78f, _transform.position.y, _transform.position.z);
                                }
                                break;
                            case MonsterBasicData.Face.Left:
                                Instantiate(Atk, this.gameObject.transform.position + LAtkAppear, Quaternion.identity);
                                if (!isAtkTwice)
                                {
                                    _transform.position = new Vector3(_transform.position.x - 4.78f, _transform.position.y, _transform.position.z);
                                }
                                break;
                        }
                        SecondTrigger = true;
                    }
                    isMonsterTemporaryDissappear = true;
                }
                if (AtkTimer <= (AtkTimerSet - 0.9))
                {
                    if (isAtkTwice && !ThirdTrigger)
                    {
                        switch (_basicData.face)
                        {
                            case MonsterBasicData.Face.Right:
                                Instantiate(Atk, _transform.position + RAtkAppear, Quaternion.identity);
                                break;
                            case MonsterBasicData.Face.Left:
                                Instantiate(Atk, _transform.position + LAtkAppear, Quaternion.identity);
                                break;
                        }
                        ThirdTrigger = true;
                    }
                    if (!isAtkTwice)
                    {
                        isMonsterTemporaryDissappear = false;
                    }
                }
                if (AtkTimer <= (AtkTimerSet - 1.1))
                {
                    if (!isAtkTwice)
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
                        AtkTimer = 0;
                        status = Status.AtkWait;
                        FirstTrigger = false;
                        SecondTrigger = false;
                        ThirdTrigger = false;
                        isAtkTwice = false;
                        AtkTimer = AtkTimerSet;
                        AtkLastTime = _time;
                        CanAtk = false;
                        return;
                    }
                    if (isAtkTwice)
                    {
                        isMonsterTemporaryDissappear = false;
                    }
                }
                if (AtkTimer <= 0)
                {
                    if (isAtkTwice)
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
                        status = Status.AtkWait;
                        FirstTrigger = false;
                        SecondTrigger = false;
                        ThirdTrigger = false;
                        isAtkTwice = false;
                        AtkTimer = AtkTimerSet;
                        AtkLastTime = _time;
                        CanAtk = false;
                    }
                }
                break;
            case Status.walk:
                switch (_basicData.face)
                {
                    case MonsterBasicData.Face.Right:
                        if (!_basicData.touchRightWall && !_hurtedController.isHurted && !MonsterRightJudgement.GetComponent<MonsterRightJudgement>().isPlayerAtRightSide)
                        {
                            if (!MonsterUnderJudgement.GetComponent<MonsterUnderJudgement>().isPlayerUnder)
                            {
                                _transform.position += new Vector3(speed * _fixedDeltaTime, 0, 0);
                            }
                        }
                        break;
                    case MonsterBasicData.Face.Left:
                        if (!_basicData.touchLeftWall && !_hurtedController.isHurted && !MonsterLeftJudgement.GetComponent<MonsterLeftJudgement>().isPlayerAtLeftSide)
                        {
                            if (!MonsterUnderJudgement.GetComponent<MonsterUnderJudgement>().isPlayerUnder)
                            {
                                _transform.position -= new Vector3(speed * _fixedDeltaTime, 0, 0);
                            }
                        }
                        break;
                }
                break;
        }

        _jumpController.Jump(_fixedDeltaTime);

        _hurtedController.HurtedMove(_fixedDeltaTime);
    }
    void SwitchAnimation()
    {
        if(status == Status.Wait || status == Status.Standby)
        {
            NowAni = AniStatus.Wait;
        }

        if (status == Status.walk)
        {
            NowAni = AniStatus.Walk;
        }

        if (status == Status.jump)
        {
            NowAni = AniStatus.Wait;
        }

        if (status == Status.Atk)
        {
            if (isMonsterTemporaryDissappear)
            {
                NowAni = AniStatus.Null;
            }
            else
            {
                NowAni = AniStatus.Atk;
            }
        }

        if (status == Status.AtkWait)
        {
            NowAni = AniStatus.AtkWait;
        }

        if (LastAni != NowAni)
        {
            if (NowAni != AniStatus.Null && LastAni != AniStatus.Null)
            {
                SEAppear = false;
            }
        }

        switch (NowAni)
        {
            case AniStatus.Wait:
                AllAniFalse();
                MonsterMoveAnimation.SetActive(true);
                break;
            case AniStatus.Walk:
                AllAniFalse();
                MonsterMoveAnimation.SetActive(true);
                MonsterMoveAni.SetBool("isWalking", true);
                if (!SEAppear)
                {
                    _dogSE.BarkSoundPlay(BarkSoundTime);
                    SEAppear = true;
                }
                break;
            case AniStatus.Atk:
                AllAniFalse();
                MonsterAtkAnimation.SetActive(true);
                if (!SEAppear)
                {
                    _dogSE.LowBarkSoundPlay(LowBarkSoundTime);
                    SEAppear = true;
                }
                break;
            case AniStatus.AtkWait:
                AllAniFalse();
                MonsterAtkWaitAnimation.SetActive(true);
                break;
            case AniStatus.Null:
                AllAniFalse();
                break;
        }

        ContinueTypeSEControll();

        if (LastAni != NowAni)
        {
            LastAni = NowAni;
        }
    }

    private void PlayStopAni()
    {
        if (_hurtedController.isHurtedByBigGun)
        {
            NowAni = AniStatus.Stop;
            _dogSE.StopAllSE();
            AllAniFalse();
            MonsterStopAnimation.SetActive(true);
            switch (_basicData.face)
            {
                case MonsterBasicData.Face.Left:
                    MonsterStopSprite.flipX = true;
                    break;
                case MonsterBasicData.Face.Right:
                    MonsterStopSprite.flipX = false;
                    break;
            }
        }
        if (_hurtedController.isCriticAtkHurted)
        {
            _dogSE.StopAllSE();
            switch (_basicData.face)
            {
                case MonsterBasicData.Face.Left:
                    MonsterStopSprite.flipX = true;
                    break;
                case MonsterBasicData.Face.Right:
                    MonsterStopSprite.flipX = false;
                    break;
            }
            NowAni = AniStatus.Stop;
            AllAniFalse();
            MonsterStopAnimation.SetActive(true);
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
        if (NowAni != AniStatus.AtkWait)
        {
            MonsterAtkWaitAnimation.SetActive(false);
        }
        if (NowAni != AniStatus.Stop)
        {
            MonsterStopAnimation.SetActive(false);
        }
    }

    private void InisializMonsterTouchTrigger()
    {
        _touchTrigger = this.GetComponent<MonsterTouchTrigger>();

        _touchTrigger.OnTouch += TouchMonster;
        _touchTrigger.StatusChange += TouchMonsterStatusChange;
        _touchTrigger.OnLeave += LeaveMonster;
    }

    private void TouchMonster()
    {
        Rigid2D.gravityScale = 0;
        Rigid2D.drag = 10;
        _boxCollider.isTrigger = true;
    }

    private void TouchMonsterStatusChange()
    {
        if (status == Status.walk)
        {
            status = Status.AtkWait;
            CanAtk = false;
            AtkLastTime = _time;
            isLittleWait = true;
        }
    }

    private void LeaveMonster()
    {
        Rigid2D.gravityScale = 7;
        Rigid2D.drag = 4;
        _boxCollider.isTrigger = false;
    }

    private void StatusJudge()
    {
        if (isLittleWait)
        {
            return;
        }

        if (_basicData.AbsDistanceX >= AtkDistance * 4)
        {
            if(status != Status.walk)
            {
                status = Status.walk;
            }
        }

        if (_basicData.AbsDistanceX <= AtkDistance && _basicData.AbsDistanceY <= ChasingDistanceY && CanAtk)
        {
            status = Status.Atk;
        }

        if (CanAtk && status != Status.Atk && status!=Status.walk)
        {
            status = Status.walk;
        }

        _jumpController.JumpStartJudge();
        if (_jumpController.JumpStart)
        {
            status = Status.jump;
        }
        if (_jumpController.isPrepareRun)
        {
            status = Status.walk;
            _basicData.ReverseTurnFaceJudge();
        }

        if (_basicData.AbsDistanceX >= GiveUpDistanceX || _basicData.AbsDistanceY > GiveUpDistanceY || PlayerController.isDie)
        {
            if (status != Status.Wait)
            {
                status = Status.Wait;
            }
        }
    }//有可能全部都不符合而維持現狀
    
    private void ContinueTypeSEControll()
    {
        if (NowAni == AniStatus.Walk && _basicData.isGround)
        {
            _dogSE.WalkSoundPlay();
        }
        else
        {
            _dogSE.TurnOffWalkSound();
        }
    }
}

