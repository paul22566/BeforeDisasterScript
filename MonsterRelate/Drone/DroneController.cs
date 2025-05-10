using Cinemachine.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

public class DroneController : MonoBehaviour
{
    [Header("基本參數")]
    private float hp;
    private int maxHp = 1;
    public int OpenDistance;
    private int ChasingDistance = 15;
    public float OpenSpeedSet;
    private float Speed;
    public float SpeedSet;
    public float RotateSpeed;
    private enum Side { AtPlayerLeft, AtPlayerRight };
    private Side side;
    public enum Ver { Normal, UnMove, Boss1 , Solid};
    public Ver ver;
    private enum Status { Standby, wait, Open, Alert, walk, Atk, AtkWalk };
    private Status status;
    private enum AniStatus { wait, Alert, Atk}
    private AniStatus NowAni;

    [Header("系統參數")]
    private Transform _transform;
    private Transform AtkTemporaryArea;
    private BoxCollider2D _boxCollider;
    private DroneSE _droneSE;
    private float _deltaTime;
    private float _fixDeltaTime;
    private float StandbyTimer = 0.1f;
    public float MoveEndTimerSet;
    public float AlertTimerSet;//一般1.5 unMove 2
    private float AlertTimer;
    private float AtkTimerSet = 0.7f;
    private float AtkTimer;
    private Transform playerTransform;
    [HideInInspector] public float AbsDistanceX;//怪物與玩家的距離
    private float OriginalDistanceX;
    private float AbsDistanceY;//怪物與玩家的距離
    private int AtkNumber;
    public GameObject Bullet;
    private float OriginalRotate;
    private float TargetRotate;
    private float NowRotate;
    private ParabolaVar _parabolaVar = new ParabolaVar();
    private SlashResult _slashResult = new SlashResult();
    private Vector2 OpenTargetPoint = new Vector2(5, 4);//以相對於玩家而言紀錄
    private Vector2 BeginEndPoint = new Vector2(6, 5);//以相對於玩家而言紀錄
    private Vector2 AtkWalkTargetPoint = new Vector2(9, 2.6f);//以相對於玩家而言紀錄
    private float AtkWalkMiddlePointY = 4;

    public GameObject MonsterDieAnimation;
    private GameObject MonsterWaitAnimation;
    private GameObject MonsterAlertAnimation;
    private GameObject MonsterAtkAnimation;
    private Animator MonsterAtkAni;

    //開關
    private bool MoveEnd;
    private bool RouteCalculate;
    private bool MoveToRight;
    private bool MoveToLeft;
    private bool CanFire;
    private bool AtkWalkSlashMove = true;
    private bool FirstTrigger;
    private bool TouchWall;
    private bool CriticAtkBeginTrigger;
    public bool OpenAtBegining;
    private bool TargetOutRange;

    [Header("不動型角度範圍")]
    [SerializeField] private float StartAngle;
    [SerializeField] private float EndAngle;
    private enum Range { Null, Increase, Across};
    [SerializeField] private Range range;
    
    [Header("被大招攻擊")]
    public GameObject CriticAtkHurtedObject;
    private bool isCriticAtkHurted;
    private bool CriticAtkHurtedSwitch;
    private bool HasCriticAtkAppear;
    private float CriticAtkHurtedTimerSet = 1.65f;
    private float CriticAtkHurtedTimer;
    private bool isHurtedByBigGun;
    private bool hasImpulse;
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("player") != null)
        {
            playerTransform = GameObject.Find("player").transform;
        }
        _boxCollider = this.GetComponent<BoxCollider2D>();
        _transform = this.transform;
        _droneSE = this.GetComponent<DroneSE>();

        hp = maxHp;
        Speed = 0;

        status = Status.Standby;
        NowAni = AniStatus.wait;
        if (OpenAtBegining)
        {
            status = Status.Alert;
            NowAni = AniStatus.Alert;
        }

        MonsterWaitAnimation = this.transform.GetChild(0).gameObject;
        MonsterAlertAnimation = this.transform.GetChild(1).gameObject;
        MonsterAtkAnimation = this.transform.GetChild(2).gameObject;

        MonsterAtkAni = MonsterAtkAnimation.GetComponent<Animator>();

        AtkTemporaryArea = this.transform.GetChild(3);
    }

    // Update is called once per frame
    void Update()
    {
        _deltaTime = Time.deltaTime;
        if (RestPlace.MonsterShouldDestroy)
        {
            Destroy(this.gameObject);
        }
        if (hp <= 0)
        {
            Instantiate(MonsterDieAnimation, _transform.localPosition, Quaternion.identity);
            Destroy(this.gameObject);
            return;
        }
        CriticAtkHurtedTimerMethod();
        //被大招打不能動
        if (isCriticAtkHurted || isHurtedByBigGun)
        {
            status = Status.Alert;
        }
        //return
        if (GameEvent.isAniPlay || isCriticAtkHurted || isHurtedByBigGun || PauseMenuController.isPauseMenuOpen)
        {
            return;
        }

        //計算距離
        if (playerTransform)
        {
            OriginalDistanceX = _transform.position.x - playerTransform.position.x;
            if(OriginalDistanceX <= 0)
            {
                side = Side.AtPlayerLeft;
            }
            else
            {
                side = Side.AtPlayerRight;
            }
            AbsDistanceX = Mathf.Abs(OriginalDistanceX);
            AbsDistanceY = Mathf.Abs(_transform.position.y - playerTransform.position.y);
        }

        //角度控制
        if (playerTransform && ver!=Ver.Solid)
        {
            OriginalRotate = AngleCaculate.CaculateAngle("R", _transform, playerTransform);
            TargetRotate = OriginalRotate;

            //target限制
            switch (range)
            {
                case Range.Increase:
                    if (TargetRotate < StartAngle)
                    {
                        TargetRotate = StartAngle;
                    }
                    if (TargetRotate> EndAngle)
                    {
                        TargetRotate = EndAngle;
                    }
                    break;
                case Range.Across:
                    if (TargetRotate > StartAngle && TargetRotate < (StartAngle + EndAngle) / 2)
                    {
                        TargetRotate = StartAngle;
                    }
                    if (TargetRotate < EndAngle && TargetRotate > (StartAngle + EndAngle) / 2)
                    {
                        TargetRotate = EndAngle;
                    }
                    break;
            }
        }
        //怪物AI
        switch (status)
        {
            case Status.Standby:
                StandbyTimer -= _deltaTime;

                if (StandbyTimer <= 0)
                {
                    status = Status.wait;
                }
                break;
            case Status.wait:
                if (playerTransform)
                {
                    if (AbsDistanceX < OpenDistance && AbsDistanceY < OpenDistance)
                    {
                        _droneSE.OpenSoundPlay(0);
                        switch (ver)
                        {
                            case Ver.Normal:
                                status = Status.Open;
                                break;
                            case Ver.UnMove:
                                status = Status.Alert;
                                break;
                            case Ver.Solid:
                                status = Status.Alert;
                                break;
                        }
                    }
                }
                break;
            case Status.Open:
                if (!RouteCalculate)
                {
                    //決定往哪飄
                    switch (side)
                    {
                        case Side.AtPlayerLeft:
                            MoveToRight = true;
                            break;
                        case Side.AtPlayerRight:
                            MoveToLeft = true;
                            break;
                    }
                    //設定起訖點
                    SlashRateCalculate();
                    _slashResult.Speed = 0;
                    RouteCalculate = true;
                }
                break;
            case Status.Alert:
                if (AbsDistanceX >= ChasingDistance && ver!=Ver.UnMove && ver != Ver.Solid)
                {
                    status = Status.walk;
                }
                break;
            case Status.walk:
                if (!RouteCalculate)
                {
                    switch (side)
                    {
                        case Side.AtPlayerLeft:
                            MoveToRight = true;
                            break;
                        case Side.AtPlayerRight:
                            MoveToLeft = true;
                            break;
                    }
                    RouteCalculate = true;
                }
                break;
            case Status.AtkWalk:
                if (!RouteCalculate)
                {
                    switch (side)
                    {
                        case Side.AtPlayerLeft:
                            MoveToRight = true;
                            break;
                        case Side.AtPlayerRight:
                            MoveToLeft = true;
                            break;
                    }

                    _parabolaVar.MiddlePoint = new Vector3(playerTransform.localPosition.x, playerTransform.localPosition.y + AtkWalkMiddlePointY, 0);
                    if (MoveToRight)
                    {
                        _parabolaVar.HorizontalDirection = "Right";
                        _parabolaVar.OtherPoint = new Vector3(playerTransform.localPosition.x + AtkWalkTargetPoint.x, playerTransform.localPosition.y + AtkWalkTargetPoint.y, 0);
                    }
                    if (MoveToLeft)
                    {
                        _parabolaVar.HorizontalDirection = "Left";
                        _parabolaVar.OtherPoint = new Vector3(playerTransform.localPosition.x - AtkWalkTargetPoint.x, playerTransform.localPosition.y + AtkWalkTargetPoint.y, 0);
                    }
                    _parabolaVar.VerticalDirection = "Down";

                    _parabolaVar.ParabolaConstant = Parabola.CalculateParabolaConstant(_parabolaVar);
                    _parabolaVar.ParabolaNowX = playerTransform.localPosition.x;
                    _parabolaVar.Speed = SpeedSet;

                    AtkWalkSlashRateCalculate();
                    _slashResult.Speed = 0;
                    RouteCalculate = true;
                }
                break;
        }

        SwitchAni();
    }

    private void FixedUpdate()
    {
        _fixDeltaTime = Time.fixedDeltaTime;

        if (isHurtedByBigGun)
        {
            hp -= BattleSystem.BigGunPower;
        }

        if (isCriticAtkHurted || isHurtedByBigGun)
        {
            return;
        }

        //判斷Target是否在範圍內
        if (playerTransform && ver != Ver.Solid)
        {
            switch (range)
            {
                case Range.Increase:
                    if (OriginalRotate < StartAngle)
                    {
                        TargetOutRange = true;
                    }
                    if (OriginalRotate > EndAngle)
                    {
                        TargetOutRange = true;
                    }
                    break;
                case Range.Across:
                    if (OriginalRotate > StartAngle && OriginalRotate < (StartAngle + EndAngle) / 2)
                    {
                        TargetOutRange = true;
                    }
                    if (OriginalRotate < EndAngle && OriginalRotate > (StartAngle + EndAngle) / 2)
                    {
                        TargetOutRange = true;
                    }
                    break;
            }
        }

        switch (status)
        {
            case Status.Open:
                if (RouteCalculate)
                {
                    if (_slashResult.Speed <= OpenSpeedSet && !MoveEnd)
                    {
                        _slashResult.Speed += OpenSpeedSet / 25;
                    }

                    Slash.SlashMove(_slashResult, _fixDeltaTime, _transform);

                    if (!MoveEnd)
                    {
                        OpenEndRange();
                    }

                    if (MoveEnd)
                    {
                        _slashResult.Speed -= OpenSpeedSet / (MoveEndTimerSet * 50);
                        if (_slashResult.Speed <= 0)
                        {
                            _slashResult.Speed = 0;
                            status = Status.Alert;
                            RouteCalculate = false;
                            MoveToLeft = false;
                            MoveToRight = false;
                            MoveEnd = false;
                        }
                    }
                }
                break;
            case Status.Alert:
                if (AlertTimer <= 0)
                {
                    AlertTimer = AlertTimerSet;
                }

                AlertTimer -= _fixDeltaTime;

                if (AlertTimer <= 0)
                {
                    if (AtkNumber <= 2 || ver == Ver.UnMove || ver == Ver.Solid)
                    {
                        if (!TargetOutRange)
                        {
                            AtkNumber += 1;
                            status = Status.Atk;
                        }
                    }
                    else
                    {
                        AtkNumber = 0;
                        status = Status.AtkWalk;
                    }
                }
                break;
            case Status.Atk:
                if (AtkTimer <= 0)
                {
                    AtkTimer = AtkTimerSet;
                }

                AtkTimer -= _fixDeltaTime;

                if (AtkTimer <= (AtkTimerSet - 0.5))
                {
                    CanFire = true;
                }
                if (AtkTimer <= (AtkTimerSet - 0.6f))
                {
                    if (!FirstTrigger)
                    {
                        Instantiate(Bullet, _transform.localPosition, Quaternion.identity, AtkTemporaryArea);
                        AtkTemporaryArea.DetachChildren();
                        FirstTrigger = true;
                    }
                    if (AtkTimer <= 0)
                    {
                        CanFire = false;
                        FirstTrigger = false;
                        status = Status.Alert;
                    }
                }
                break;
            case Status.AtkWalk:
                if (TouchWall)
                {
                    FirstTrigger = false;
                    _parabolaVar.Speed = 0;
                    _slashResult.Speed = 0;
                    CanFire = false;
                    MoveEnd = false;
                    RouteCalculate = false;
                    MoveToLeft = false;
                    MoveToRight = false;
                    status = Status.Alert;
                    TouchWall = false;
                }

                if (RouteCalculate)
                {
                    if (_slashResult.Speed < SpeedSet && AtkWalkSlashMove)
                    {
                        _slashResult.Speed += SpeedSet / 25;
                    }
                    if (_slashResult.Speed >= SpeedSet && AtkWalkSlashMove)
                    {
                        _slashResult.Speed = SpeedSet;
                    }

                    if (AtkWalkSlashMove)
                    {
                        Slash.SlashMove(_slashResult, _fixDeltaTime, _transform);
                    }
                    else
                    {
                        Parabola.ParabolaMove(_parabolaVar, _fixDeltaTime, _transform);
                    }

                    if (MoveToRight)
                    {
                        if (_transform.localPosition.x >= _parabolaVar.MiddlePoint.x)
                        {
                            AtkWalkSlashMove = false;
                        }
                        if (_transform.localPosition.x >= _parabolaVar.MiddlePoint.x + 6)
                        {
                            MoveEnd = true;
                        }
                    }
                    if (MoveToLeft)
                    {
                        if (_transform.localPosition.x <= _parabolaVar.MiddlePoint.x)
                        {
                            AtkWalkSlashMove = false;
                        }
                        if (_transform.localPosition.x <= _parabolaVar.MiddlePoint.x - 6)
                        {
                            MoveEnd = true;
                        }
                    }

                    if (AbsDistanceX <= 1)
                    {
                        CanFire = true;
                    }
                    if (AbsDistanceX <= 0.9)
                    {
                        if (!FirstTrigger)
                        {
                            Instantiate(Bullet, _transform.localPosition, Quaternion.identity, AtkTemporaryArea);
                            AtkTemporaryArea.DetachChildren();
                            FirstTrigger = true;
                        }
                    }

                    if (MoveEnd)
                    {
                        _parabolaVar.Speed -= SpeedSet / (50 * MoveEndTimerSet);
                        if (_parabolaVar.Speed <= 0)
                        {
                            _parabolaVar.Speed = 0;
                            _slashResult.Speed = 0;
                            FirstTrigger = false;
                            AtkWalkSlashMove = true;
                            CanFire = false;
                            MoveEnd = false;
                            RouteCalculate = false;
                            MoveToLeft = false;
                            MoveToRight = false;
                            status = Status.Alert;
                        }
                    }
                }
                break;
            case Status.walk:
                if (RouteCalculate)
                {
                    if (Speed <= SpeedSet && !MoveEnd)
                    {
                        Speed += SpeedSet / 25;
                    }
                    if (MoveToRight)
                    {
                        _transform.localPosition = new Vector3(_transform.localPosition.x + Speed * _fixDeltaTime, _transform.localPosition.y, 0);
                    }
                    if (MoveToLeft)
                    {
                        _transform.localPosition = new Vector3(_transform.localPosition.x - Speed * _fixDeltaTime, _transform.localPosition.y, 0);
                    }

                    if (AbsDistanceX <= BeginEndPoint.x)
                    {
                        MoveEnd = true;
                    }
                    if (MoveEnd)
                    {
                        Speed -= SpeedSet / (MoveEndTimerSet * 50);
                        if (Speed <= 0)
                        {
                            Speed = 0;
                            RouteCalculate = false;
                            MoveToLeft = false;
                            MoveToRight = false;
                            MoveEnd = false;
                            status = Status.Alert;
                        }
                    }
                }
                break;
        }

        if (!TargetOutRange)
        {
            AngleRotate();
        }

        TargetOutRange = false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //暫時變得可穿透
        if (collision.gameObject.tag == "monster")
        {
            _boxCollider.isTrigger = true;
        }
        if (collision.gameObject.tag == "LeftWall")
        {
            TouchWall = true;
        }
        if (collision.gameObject.tag == "RightWall")
        {
            TouchWall = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "monster")
        {
            _boxCollider.isTrigger = false;
        }
        if (collision.gameObject.tag == "LeftWall")
        {
            TouchWall = false;
        }
        if (collision.gameObject.tag == "RightWall")
        {
            TouchWall = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "bullet")
        {
            hp -= BattleSystem.BulletHurtPower;
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "normalAtk")
        {
            hp -= BattleSystem.NormalAtkHurtPower;
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "Cocktail")
        {
            hp -= BattleSystem.ExplosionHurtPower;
        }
        if (other.gameObject.tag == "ExplosionBottle")
        {
            hp -= BattleSystem.ExplosionHurtPower;
        }
        if (other.gameObject.tag == "CAtk")
        {
            hp -= BattleSystem.CAtkHurtPower;
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "CriticAtk")
        {
            isCriticAtkHurted = true;
            CriticAtkHurtedSwitch = true;
        }
        if (other.gameObject.tag == "BigGun")
        {
            isHurtedByBigGun = true;
        }
        if (other.gameObject.tag == "LeftWall")
        {
            TouchWall = true;
        }
        if (other.gameObject.tag == "RightWall")
        {
            TouchWall = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "monster")
        {
            _boxCollider.isTrigger = false;
        }
        if (collision.gameObject.tag == "BigGun")
        {
            isHurtedByBigGun = false;
        }
        if (collision.gameObject.tag == "LeftWall")
        {
            TouchWall = false;
        }
        if (collision.gameObject.tag == "RightWall")
        {
            TouchWall = false;
        }
    }

    private void SwitchAni()
    {
        if (status == Status.wait && !OpenAtBegining)
        {
            NowAni = AniStatus.wait;
        }
        if (status == Status.Alert || status == Status.walk || status == Status.Open)
        {
            NowAni = AniStatus.Alert;
        }
        if (status == Status.Atk || status == Status.AtkWalk)
        {
            NowAni = AniStatus.Atk;
        }

        switch (NowAni)
        {
            case AniStatus.wait:
                AllAniFalse();
                MonsterWaitAnimation.SetActive(true);
                break;
            case AniStatus.Alert:
                AllAniFalse();
                MonsterAlertAnimation.SetActive(true);
                break;
            case AniStatus.Atk:
                AllAniFalse();
                MonsterAtkAnimation.SetActive(true);
                if (CanFire)
                {
                    MonsterAtkAni.SetBool("Atk", true);
                }
                break;
        }

        ContinueSEControll();
    }

    private void AngleRotate()
    {
        //執行
        NowRotate = _transform.eulerAngles.z;
        if (status != Status.wait && status !=Status.Standby)
        {
            if (NowRotate < TargetRotate - 0.4)
            {
                if (Mathf.Abs(NowRotate - TargetRotate) <= 180)
                {
                    NowRotate += RotateSpeed * _fixDeltaTime;
                    _transform.rotation = Quaternion.Euler(0, 0, NowRotate);
                }
                else
                {
                    NowRotate -= RotateSpeed * _fixDeltaTime;
                    _transform.rotation = Quaternion.Euler(0, 0, NowRotate);
                }
            }
            if (NowRotate > TargetRotate + 0.4)
            {
                if (Mathf.Abs(NowRotate - TargetRotate) <= 180)
                {
                    NowRotate -= RotateSpeed * _fixDeltaTime;
                    _transform.rotation = Quaternion.Euler(0, 0, NowRotate);
                }
                else
                {
                    NowRotate += RotateSpeed * _fixDeltaTime;
                    _transform.rotation = Quaternion.Euler(0, 0, NowRotate);
                }
            }
        }
    }

    private void SlashRateCalculate()
    {
        bool ToDown = false;
        bool ToUp = false;

        if (_transform.localPosition.y - (playerTransform.localPosition.y + OpenTargetPoint.y) >= 0)
        {
            ToDown = true;
        }
        if (_transform.localPosition.y - (playerTransform.localPosition.y + OpenTargetPoint.y) < 0)
        {
            ToUp = true;
        }

        if (ToDown)
        {
            if (MoveToRight)
            {
                _slashResult = Slash.SlashCalculate(_transform.localPosition.x - (playerTransform.localPosition.x - OpenTargetPoint.x), _transform.localPosition.y - (playerTransform.localPosition.y + OpenTargetPoint.y), "R", "D");
            }
            if (MoveToLeft)
            {
                _slashResult = Slash.SlashCalculate(_transform.localPosition.x - (playerTransform.localPosition.x + OpenTargetPoint.x), _transform.localPosition.y - (playerTransform.localPosition.y + OpenTargetPoint.y), "L", "D");
            }
        }
        if(ToUp)
        {
            if (MoveToRight)
            {
                _slashResult = Slash.SlashCalculate(_transform.localPosition.x - (playerTransform.localPosition.x - OpenTargetPoint.x), _transform.localPosition.y - (playerTransform.localPosition.y + OpenTargetPoint.y), "R", "U");
            }
            if (MoveToLeft)
            {
                _slashResult = Slash.SlashCalculate(_transform.localPosition.x - (playerTransform.localPosition.x + OpenTargetPoint.x), _transform.localPosition.y - (playerTransform.localPosition.y + OpenTargetPoint.y), "L", "U");
            }
        }
    }

    private void AtkWalkSlashRateCalculate()
    {
        bool ToDown = false;
        bool ToUp = false;

        if (_transform.localPosition.y - (playerTransform.localPosition.y + AtkWalkMiddlePointY) >= 0)
        {
            ToDown = true;
        }
        if (_transform.localPosition.y - (playerTransform.localPosition.y + AtkWalkMiddlePointY) < 0)
        {
            ToUp = true;
        }

        if (ToDown)
        {
            if (MoveToRight)
            {
                _slashResult = Slash.SlashCalculate(_transform.localPosition.x - playerTransform.localPosition.x, _transform.localPosition.y - (playerTransform.localPosition.y + AtkWalkMiddlePointY), "R", "D");
            }
            if (MoveToLeft)
            {
                _slashResult = Slash.SlashCalculate(_transform.localPosition.x - playerTransform.localPosition.x, _transform.localPosition.y - (playerTransform.localPosition.y + AtkWalkMiddlePointY), "L", "D");
            }
        }
        if (ToUp)
        {
            if (MoveToRight)
            {
                _slashResult = Slash.SlashCalculate(_transform.localPosition.x - playerTransform.localPosition.x, _transform.localPosition.y - (playerTransform.localPosition.y + AtkWalkMiddlePointY), "R", "U");
            }
            if (MoveToLeft)
            {
                _slashResult = Slash.SlashCalculate(_transform.localPosition.x - playerTransform.localPosition.x, _transform.localPosition.y - (playerTransform.localPosition.y + AtkWalkMiddlePointY), "L", "U");
            }
        }
    }

    private void OpenEndRange()
    {
        if (_transform.localPosition.x > playerTransform.localPosition.x + BeginEndPoint.x)
        {
            return;
        }
        if (_transform.localPosition.x < playerTransform.localPosition.x - BeginEndPoint.x)
        {
            return;
        }
        if (_transform.localPosition.y > playerTransform.localPosition.y + BeginEndPoint.y)
        {
            return;
        }
        MoveEnd = true;
    }

    void CriticAtkHurtedTimerMethod()
    {
        if (CriticAtkHurtedSwitch)
        {
            if (!CriticAtkBeginTrigger)
            {
                _droneSE.StopAllSound();
                CriticAtkBeginTrigger = true;
            }
            CriticAtkHurtedTimer -= _deltaTime;
            if (CriticAtkHurtedTimer <= (CriticAtkHurtedTimerSet - 0.03))
            {
                if (!HasCriticAtkAppear)
                {
                    Instantiate(CriticAtkHurtedObject, _transform.position, Quaternion.identity);
                    HasCriticAtkAppear = true;
                }
            }
            if (CriticAtkHurtedTimer <= (CriticAtkHurtedTimerSet - 0.95f))
            {
                if (!hasImpulse)
                {
                    hp -= BattleSystem.CriticAtkHurtPower;
                    hasImpulse = true;
                }
                if (CriticAtkHurtedTimer <= 0)
                {
                    hasImpulse = false;
                    HasCriticAtkAppear = false;
                    status = Status.Alert;
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

    private void AllAniFalse()
    {
        if(NowAni != AniStatus.wait)
        {
            MonsterWaitAnimation.SetActive(false);
        }
        if (NowAni != AniStatus.Alert)
        {
            MonsterAlertAnimation.SetActive(false);
        }
        if (NowAni != AniStatus.Atk)
        {
            MonsterAtkAnimation.SetActive(false);
        }
    }

    private void ContinueSEControll()
    {
        if (status == Status.walk || status == Status.AtkWalk || status == Status.Open)
        {
            _droneSE.NormalRunSoundPlay();
            if (MoveEnd)
            {
                _droneSE.TurnOffNormalRunSound();
            }
        }
        else
        {
            _droneSE.AbsoluteTurnOffNormalRunSound();
        }
    }
}
