using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasterController : MonoBehaviour
{
    [Header("基本參數")]
    public int ChasingDistance;
    public enum Status { Standby, wait, walk, FireBall, Thunder };
    private Status status;

    public enum AniStatus { Wait, Walk, Atk, Stop}
    [HideInInspector]public AniStatus NowAni;//script(Hurted)

    [Header("系統參數")]
    private BackgroundSystem _system;
    private Transform PortalJudgeObject;
    private CasterPortalJudge _portalJudge;
    [HideInInspector] public Vector3 PortalPoint;
    private float StandbyTimer = 0.1f;
    private Transform _transform;
    private float _deltaTime;
    private float _fixedDeltaTime;
    private Transform AtkTemporaryArea;
    public float AtkTimerSet;
    private float AtkTimer;
    public float Atk2TimerSet;
    public float PortalTimerSet;
    private float PortalTimer;
    private Rigidbody2D Rigid2D;
    private BoxCollider2D _boxCollider;
    private MonsterBasicData _basicData;
    private MonsterHurtedController _hurtedController;
    private MonsterTouchTrigger _touchTrigger;

    //開關
    private bool JudgePointMove;
    private bool isMoveSuccess;
    private bool FirstTrigger;
    private bool SecondTrigger;
    private bool AtkFire;
    private Vector3 RMagicAppear = new Vector3(0.61f, 1.67f, 0);
    private Vector3 LMagicAppear = new Vector3(-0.61f, 1.67f, 0);

    [Header("動畫相關物件")]
    private Animator MonsterMoveAni;
    private Animator MonsterAtkAni;
    private GameObject MonsterMoveAnimation;
    private GameObject MonsterAtkAnimation;
    private Transform MonsterMoveTr;
    private Transform MonsterAtkTr;

    [Header("攻擊物件")]
    public GameObject RFireBall;
    public GameObject LFireBall;
    public GameObject FireBallPrepare;
    public GameObject ThunderPrepare;
    public GameObject ThunderPrepare2;

    private GameObject MonsterStopAnimation;
    private Transform MonsterStopTr;

    void Start()
    {
        Rigid2D = this.transform.GetComponent<Rigidbody2D>();
        _boxCollider = this.transform.GetComponent<BoxCollider2D>();
        _transform = transform;
        _basicData = this.GetComponent<MonsterBasicData>();
        _hurtedController = this.transform.GetComponent<MonsterHurtedController>();
        if (GameObject.Find("system") != null)
        {
            _system = GameObject.Find("system").GetComponent<BackgroundSystem>();
        }

        //抓取動畫相關物件
        MonsterMoveAnimation = this.transform.GetChild(0).gameObject;
        MonsterAtkAnimation = this.transform.GetChild(1).gameObject;
        MonsterStopAnimation = this.transform.GetChild(2).gameObject;

        MonsterMoveTr = MonsterMoveAnimation.transform.GetChild(0);
        MonsterAtkTr = MonsterAtkAnimation.transform.GetChild(0);
        MonsterStopTr = MonsterStopAnimation.transform.GetChild(0);

        MonsterMoveAni = MonsterMoveTr.GetComponent<Animator>();
        MonsterAtkAni = MonsterAtkTr.GetComponent<Animator>();

        AtkTemporaryArea = this.transform.GetChild(3);

        PortalJudgeObject = AtkTemporaryArea.GetChild(3);
        _portalJudge = PortalJudgeObject.GetComponent<CasterPortalJudge>();

        AtkTemporaryArea.DetachChildren();

        if (!_basicData.SpecialOpening)
        {
            status = Status.Standby;
        }
        NowAni = AniStatus.Wait;

        _basicData.BasicVarInisialize(MonsterMoveTr, "R");

        _hurtedController._getCriticHurted += PlayStopAni;

        //初始化MonsterTouch系統
        InisializMonsterTouchTrigger();
    }

    // Update is called once per frame
    void Update()
    {
        _deltaTime = Time.deltaTime;
        _basicData.DieJudge();

        //多種變數重置管理
        if (_hurtedController.isCriticAtkHurted)
        {
            AtkTimer = 0;
            PortalTimer = 0;
            JudgePointMove = false;
            isMoveSuccess = false;
            FirstTrigger = false;
            SecondTrigger = false;
            AtkFire = false;
        }

        _hurtedController.HurtedTimerMethod(_deltaTime);
        _hurtedController.CriticAtkHurtedTimerMethod(_deltaTime);

        //寫入實際位置
        _basicData.MonsterPlace = _transform.position;

        //return
        if (GameEvent.isAniPlay|| _hurtedController.isHurtedByBigGun || _hurtedController.isCriticAtkHurted || PauseMenuController.isPauseMenuOpen)
        {
            return;
        }

        //計算距離
        _basicData.DistanceCalculate();

        //被大招打之後的動作
        if (_hurtedController.BeCriticAtkEnd)
        {
            status = Status.walk;
            _hurtedController.BeCriticAtkEnd = false;
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
                if (_basicData.playerTransform)
                {
                    //被遠程打時的反應
                    if (_hurtedController.HurtedByFarAtk)
                    {
                        status = Status.walk;
                    }
                    if (_hurtedController.HurtedByFarAtk)
                    {
                        _hurtedController.HurtedByFarAtk = false;
                    }

                    if (_basicData.AbsDistanceX < ChasingDistance && _basicData.AbsDistanceY <= ChasingDistance)
                    {
                        status = Status.walk;
                    }
                }
                break;
        }

        //轉向控制
        switch (_basicData.face)
        {
            case MonsterBasicData.Face.Left:
                MonsterMoveTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                MonsterAtkTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                MonsterStopTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                break;
            case MonsterBasicData.Face.Right:
                MonsterMoveTr.localScale = new Vector3(0.28f, 0.28f, 0);
                MonsterAtkTr.localScale = new Vector3(0.28f, 0.28f, 0);
                MonsterStopTr.localScale = new Vector3(0.28f, 0.28f, 0);
                break;
        }

        SwitchAnimation();

        if (PlayerController.isDie)
        {
            status = Status.wait;
        }
    }

    private void FixedUpdate()
    {
        _fixedDeltaTime = Time.fixedDeltaTime;

        //return
        if (GameEvent.isAniPlay || _hurtedController.isHurtedByBigGun || _hurtedController.isCriticAtkHurted)
        {
            return;
        }

        switch(status)
        {
            case Status.walk:
                if (PortalTimer <= 0)
                {
                    PortalTimer = PortalTimerSet;
                }

                PortalTimer -= _fixedDeltaTime;

                if (PortalTimer <= 0.03 && !JudgePointMove)
                {
                    //0為右，1為左
                    switch (Random.Range(0, 2))
                    {
                        case 0:
                            if (_basicData.playerTransform.position.x + 10 >= _system.RightEdgeX)
                            {
                                PortalJudgeObject.position = new Vector3(_basicData.playerTransform.position.x - (6 + Random.Range(0, 5)), _basicData.playerTransform.position.y, _basicData.playerTransform.position.z);
                            }
                            else
                            {
                                PortalJudgeObject.position = new Vector3(_basicData.playerTransform.position.x + (6 + Random.Range(0, 5)), _basicData.playerTransform.position.y, _basicData.playerTransform.position.z);
                            }
                            break;
                        case 1:
                            if (_basicData.playerTransform.position.x - 10 <= _system.LeftEdgeX)
                            {
                                PortalJudgeObject.position = new Vector3(_basicData.playerTransform.position.x + (6 + Random.Range(0, 5)), _basicData.playerTransform.position.y, _basicData.playerTransform.position.z);
                            }
                            else
                            {
                                PortalJudgeObject.position = new Vector3(_basicData.playerTransform.position.x - (6 + Random.Range(0, 5)), _basicData.playerTransform.position.y, _basicData.playerTransform.position.z);
                            }
                            break;
                    }

                    _portalJudge.BoolFalse();
                    JudgePointMove = true;
                }
                if (PortalTimer <= 0)
                {
                    JudgePointMove = false;

                    //0為右，1為左
                    switch (Random.Range(0, 2))
                    {
                        case 0:
                            if (_basicData.playerTransform.position.x + 10 >= _system.RightEdgeX)
                            {
                                isMoveSuccess = _portalJudge.JudgePortalPoint();
                                _transform.position = PortalPoint;
                                _basicData.face = MonsterBasicData.Face.Right;
                            }
                            else
                            {
                                isMoveSuccess = _portalJudge.JudgePortalPoint();
                                _transform.position = PortalPoint;
                                _basicData.face = MonsterBasicData.Face.Left;
                            }
                            break;
                        case 1:
                            if (_basicData.playerTransform.position.x - 10 <= _system.LeftEdgeX)
                            {
                                isMoveSuccess = _portalJudge.JudgePortalPoint();
                                _transform.position = PortalPoint;
                                _basicData.face = MonsterBasicData.Face.Left;
                            }
                            else
                            {
                                isMoveSuccess = _portalJudge.JudgePortalPoint();
                                _transform.position = PortalPoint;
                                _basicData.face = MonsterBasicData.Face.Right;
                            }
                            break;
                    }

                    switch (Random.Range(0, 2))
                    {
                        case 0:
                            if (isMoveSuccess)
                            {
                                status = Status.FireBall;
                                status = Status.Thunder;
                            }
                            else
                            {
                                status = Status.Thunder;
                            }
                            break;
                        case 1:
                            status = Status.Thunder;
                            break;
                    }

                    isMoveSuccess = false;
                }
                break;
            case Status.FireBall:
                if (AtkTimer<=0)
                {
                    _basicData.TurnFaceJudge();
                    AtkTimer = AtkTimerSet;
                }

                AtkTimer -= _fixedDeltaTime;

                if (AtkTimer <= (AtkTimerSet - 1))
                {
                    if (!FirstTrigger)
                    {
                        switch (_basicData.face)
                        {
                            case MonsterBasicData.Face.Right:
                                Instantiate(FireBallPrepare, _transform.position + RMagicAppear, Quaternion.identity, AtkTemporaryArea);
                                AtkTemporaryArea.DetachChildren();
                                break;
                            case MonsterBasicData.Face.Left:
                                Instantiate(FireBallPrepare, _transform.position + LMagicAppear, Quaternion.identity, AtkTemporaryArea);
                                AtkTemporaryArea.DetachChildren();
                                break;
                        }
                        FirstTrigger = true;
                    }
                }
                if (AtkTimer <= (AtkTimerSet - 4))
                {
                    if (!SecondTrigger)
                    {
                        AtkFire = true;
                        switch (_basicData.face)
                        {
                            case MonsterBasicData.Face.Right:
                                Instantiate(RFireBall, _transform.position + RMagicAppear, Quaternion.identity, AtkTemporaryArea);
                                AtkTemporaryArea.DetachChildren();
                                break;
                            case MonsterBasicData.Face.Left:
                                Instantiate(LFireBall, _transform.position + LMagicAppear, Quaternion.identity, AtkTemporaryArea);
                                AtkTemporaryArea.DetachChildren();
                                break;
                        }
                        SecondTrigger = true;
                    }
                }
                if (AtkTimer <= 0)
                {
                    FirstTrigger = false;
                    SecondTrigger = false;
                    AtkFire = false;
                    status = Status.walk;
                }
                break;
            case Status.Thunder:
                if (AtkTimer <= 0)
                {
                    _basicData.TurnFaceJudge();
                    AtkTimer = Atk2TimerSet;
                }

                AtkTimer -= _fixedDeltaTime;

                if (AtkTimer <= (Atk2TimerSet - 1))
                {
                    if (!FirstTrigger)
                    {
                        switch (_basicData.face)
                        {
                            case MonsterBasicData.Face.Right:
                                Instantiate(ThunderPrepare, _transform.position + RMagicAppear, Quaternion.identity, AtkTemporaryArea);
                                AtkTemporaryArea.DetachChildren();
                                break;
                            case MonsterBasicData.Face.Left:
                                Instantiate(ThunderPrepare, _transform.position + LMagicAppear, Quaternion.identity, AtkTemporaryArea);
                                AtkTemporaryArea.DetachChildren();
                                break;
                        }
                        FirstTrigger = true;
                    }
                }
                if (AtkTimer <= (Atk2TimerSet - 2))
                {
                    if (!SecondTrigger)
                    {
                        Instantiate(ThunderPrepare2, new Vector3(_basicData.playerTransform.position.x, _basicData.playerTransform.position.y + 2, _basicData.playerTransform.position.z), Quaternion.identity, AtkTemporaryArea);
                        AtkTemporaryArea.DetachChildren();
                        SecondTrigger = true;
                    }
                }
                if (AtkTimer <= (Atk2TimerSet - 3.55))
                {
                    AtkFire = true;
                }
                if (AtkTimer <= 0)
                {
                    FirstTrigger = false;
                    SecondTrigger = false;
                    AtkFire = false;
                    status = Status.walk;
                }
                break;
        }
    }

    private void InisializMonsterTouchTrigger()
    {
        _touchTrigger = this.GetComponent<MonsterTouchTrigger>();

        _touchTrigger.OnTouch += TouchMonster;
        _touchTrigger.OnLeave += LeaveMonster;
    }

    private void TouchMonster()
    {
        Rigid2D.gravityScale = 0;
        Rigid2D.drag = 10;
        _boxCollider.isTrigger = true;
    }

    private void LeaveMonster()
    {
        Rigid2D.gravityScale = 7;
        Rigid2D.drag = 4;
        _boxCollider.isTrigger = false;
    }

    void SwitchAnimation()
    {
        NowAni = AniStatus.Wait;

        if (status == Status.walk)
        {
            NowAni = AniStatus.Walk;
        }

        if (status == Status.FireBall || status == Status.Thunder)
        {
            NowAni = AniStatus.Atk;
        }

        switch (NowAni)
        {
            case AniStatus.Wait:
                AllAniFalse();
                MonsterMoveAnimation.SetActive(true);
                MonsterMoveAni.SetBool("isWalk", false);
                break;
            case AniStatus.Walk:
                AllAniFalse();
                MonsterMoveAnimation.SetActive(true);
                MonsterMoveAni.SetBool("isWalk", true);
                break;
            case AniStatus.Atk:
                AllAniFalse();
                MonsterAtkAnimation.SetActive(true);
                if (AtkFire)
                {
                    MonsterAtkAni.SetBool("Atk", true);
                    switch (status)
                    {
                        case Status.FireBall:
                            MonsterAtkAni.SetInteger("Status", 1);
                            break;
                        case Status.Thunder:
                            MonsterAtkAni.SetInteger("Status", 2);
                            break;
                    }
                }
                break;
            case AniStatus.Stop:
                AllAniFalse();
                MonsterStopAnimation.SetActive(true);
                break;
        }
    }

    private void PlayStopAni()
    {
        if (_hurtedController.isHurtedByBigGun)
        {
            NowAni = AniStatus.Stop;
            AllAniFalse();
            MonsterStopAnimation.SetActive(true);
            switch (_basicData.face)
            {
                case MonsterBasicData.Face.Left:
                    MonsterStopTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                    break;
                case MonsterBasicData.Face.Right:
                    MonsterStopTr.localScale = new Vector3(0.28f, 0.28f, 0);
                    break;
            }
        }
        if (_hurtedController.isCriticAtkHurted)
        {
            switch (_basicData.face)
            {
                case MonsterBasicData.Face.Left:
                    MonsterStopTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                    break;
                case MonsterBasicData.Face.Right:
                    MonsterStopTr.localScale = new Vector3(0.28f, 0.28f, 0);
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
        if (NowAni != AniStatus.Stop)
        {
            MonsterStopAnimation.SetActive(false);
        }
    }
}
