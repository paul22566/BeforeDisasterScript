using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeakCaptainController : MonoBehaviour
{
    [Header("基本參數")]
    public int ChasingDistance;
    public int GiveUpDistanceX;
    public int GiveUpDistanceY;
    public float AtkDistance;
    public float speed;
    private enum Status {Standby, wait,  walk, Rest, Atk };
    private Status status;
    public enum AniStatus { Wait, Walk,  Atk }
    [HideInInspector] public AniStatus NowAni;//script(Hurted)
    private AniStatus LastAni;

    [Header("系統參數")]
    private Transform _transform;
    private MonsterBasicData _basicData;
    private float _deltaTime;
    private float _fixedDeltaTime;
    private float _time;
    private GameObject AtkTemporaryArea;
    private GameObject MonsterRightJudgement;
    private GameObject MonsterLeftJudgement;
    private MonsterHurtedController _hurtedController;
    private float StandbyTimer = 0.1f;
    private float AtkTimerSet = 1.5f;
    private float AtkTimer;
    public float AtkCoolDown;//2種Atk共用
    private float AtkLastTime = -10;
    public float RestTimerSet;
    private float RestTimer;
    private float TurnFaceLagTimerSet = 0.2f;
    private float TurnFaceLagTimer;
    private Rigidbody2D Rigid2D;
    private float GroundPointY;

    //開關
    private bool RestEnd;
    private bool AtkFirstAppear;
    private bool isAlert;
    private bool isAlertByFarAtk;
    private bool CanAtk;

    [Header("動畫相關物件")]
    private Animator MonsterMoveAni;
    private GameObject MonsterMoveAnimation;
    private GameObject MonsterAtkAnimation;
    private Transform MonsterMoveTr;
    private Transform MonsterAtkTr;

    [Header("攻擊物件")]
    public GameObject RAtk;
    public GameObject LAtk;

    public AudioClip AtkSound;
    private AudioSource AtkSource;
    private float AtkValidDistance = 15;
    private float AtkSoundTime = 0.7f;
    private bool SEAppear;

    void Start()
    {
        //抓取動畫相關物件
        MonsterMoveAnimation = this.transform.GetChild(0).gameObject;
        MonsterAtkAnimation = this.transform.GetChild(1).gameObject;

        MonsterMoveTr = MonsterMoveAnimation.transform.GetChild(0).transform;
        MonsterAtkTr = MonsterAtkAnimation.transform.GetChild(0).transform;

        MonsterMoveAni = MonsterMoveTr.GetComponent<Animator>();

        AtkTemporaryArea = this.transform.GetChild(2).gameObject;
        status = Status.Standby;
        NowAni = AniStatus.Wait;
        _basicData = this.GetComponent<MonsterBasicData>();
        _hurtedController = this.transform.GetComponent<MonsterHurtedController>();

        _basicData.BasicVarInisialize(MonsterMoveTr, "R");

        _transform = this.transform;
        Rigid2D = this.GetComponent<Rigidbody2D>();

        TurnFaceLagTimer = TurnFaceLagTimerSet;
        MonsterRightJudgement = AtkTemporaryArea.transform.GetChild(0).gameObject;
        MonsterLeftJudgement = AtkTemporaryArea.transform.GetChild(1).gameObject;
        AtkTemporaryArea.transform.DetachChildren();

        //音效
        AtkSource = this.AddComponent<AudioSource>();
        AtkSource.clip = AtkSound;
    }

    // Update is called once per frame
    void Update()
    {
        _time = Time.time;
        _deltaTime = Time.deltaTime;
        
        _basicData.DieJudge();

        _hurtedController.HurtedTimerMethod(_deltaTime);

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

        //寫入實際位置
        _basicData.MonsterPlace = _transform.position;
        
        //return
        if (GameEvent.isAniPlay || _hurtedController.isHurtedByBigGun || _hurtedController.isCriticAtkHurted)
        {
            return;
        }

        //計算距離
        _basicData.DistanceCalculate();
        //冷卻時間計算
        _basicData.CoolDownCalculate(_time, AtkLastTime, AtkCoolDown, ref CanAtk);

        //被遠程打時的反應
        if (_hurtedController.HurtedByFarAtk && !isAlertByFarAtk && !isAlert)
        {
            isAlertByFarAtk = true;
            status = Status.walk;
            isAlert = true;
            _basicData.TurnFaceJudge();
        }
        if (_hurtedController.HurtedByFarAtk)
        {
            _hurtedController.HurtedByFarAtk = false;
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
                    if (_basicData.AbsDistanceX < ChasingDistance && _basicData.AbsDistanceY <= 2)
                    {
                        _basicData.TurnFaceJudge();
                        status = Status.walk;
                        isAlert = true;
                    }

                    if (_basicData.AbsDistanceX <= AtkDistance && CanAtk && _basicData.AbsDistanceY <= 1.5)
                    {
                        status = Status.Atk;
                        isAlert = true;
                    }
                }
                break;
            case Status.walk:
                _basicData.LagTurnFaceJudge(ref TurnFaceLagTimer, TurnFaceLagTimerSet, _deltaTime);

                if (_basicData.playerTransform)
                {
                    CanAtk = true;
                    if (_basicData.AbsDistanceX >= GiveUpDistanceX || _basicData.AbsDistanceY >= GiveUpDistanceY)
                    {
                        status = Status.wait;
                        isAlert = false;
                        isAlertByFarAtk = false;
                    }

                    if (_basicData.AbsDistanceX <= AtkDistance && _basicData.AbsDistanceY <= 1.5 && CanAtk)
                    {
                        status = Status.Atk;
                    }
                }
                break;
            case Status.Rest:
                if (RestEnd)
                {
                    if (_basicData.AbsDistanceX < ChasingDistance && _basicData.AbsDistanceY <= 2)
                    {
                        _basicData.TurnFaceJudge();
                        status = Status.walk;
                        RestEnd = false;
                        AtkFirstAppear = false;
                    }

                    if (_basicData.AbsDistanceX <= AtkDistance && CanAtk && _basicData.AbsDistanceY <= 1.5)
                    {
                        status = Status.Atk;
                        RestEnd = false;
                        AtkFirstAppear = false;
                    }
                }

                _basicData.TurnFaceJudge();
                break;
        }

        //轉向控制
        switch (_basicData.face)
        {
            case MonsterBasicData.Face.Left:
                MonsterMoveTr.localScale = new Vector3(-0.28f, 0.28f, 1);
                MonsterAtkTr.localScale = new Vector3(-0.28f, 0.28f, 1);
                break;

            case MonsterBasicData.Face.Right:
                MonsterMoveTr.localScale = new Vector3(0.28f, 0.28f, 1);
                MonsterAtkTr.localScale = new Vector3(0.28f, 0.28f, 1);
                break;
        }
        SwitchAnimation();

        //音效
        if (AtkSource.isPlaying)
        {
            AtkSource.volume = SEController.FOVCalculate(_basicData.AbsDistanceX, AtkValidDistance);
        }
    }

    private void FixedUpdate()
    {
        _fixedDeltaTime = Time.fixedDeltaTime;

        switch (status)
        {
            case Status.walk:
                if (!_hurtedController.isHurted)
                {
                    switch (_basicData.face)
                    {
                        case MonsterBasicData.Face.Right:
                            if (!_basicData.touchRightWall && !MonsterRightJudgement.GetComponent<MonsterRightJudgement>().isPlayerAtRightSide)
                            {
                                _transform.position += new Vector3(speed * _fixedDeltaTime, 0, 0);
                            }
                            break;
                        case MonsterBasicData.Face.Left:
                            if (!_basicData.touchLeftWall && !MonsterLeftJudgement.GetComponent<MonsterLeftJudgement>().isPlayerAtLeftSide)
                            {
                                _transform.position -= new Vector3(speed * _fixedDeltaTime, 0, 0);
                            }
                            break;
                    }
                }
                break;
            case Status.Atk:
                if (AtkTimer <= 0)
                {
                    AtkTimer = AtkTimerSet;
                }

                AtkTimer -= _fixedDeltaTime;

                if (AtkTimer <= (AtkTimerSet - 0.7))
                {
                    if (!AtkFirstAppear)
                    {
                        switch (_basicData.face)
                        {
                            case MonsterBasicData.Face.Right:
                                Instantiate(RAtk, _transform.position, Quaternion.identity, AtkTemporaryArea.transform);
                                AtkTemporaryArea.transform.DetachChildren();
                                break;
                            case MonsterBasicData.Face.Left:
                                Instantiate(LAtk, _transform.position, Quaternion.identity, AtkTemporaryArea.transform);
                                AtkTemporaryArea.transform.DetachChildren();
                                break;
                        }
                        AtkFirstAppear = true;
                    }
                    if (AtkTimer <= 0)
                    {
                        _basicData.TurnFaceJudge();
                        AtkLastTime = _time;
                        CanAtk = false;
                        status = Status.Rest;
                        AtkFirstAppear = false;
                    }
                }
                break;
            case Status.Rest:
                if (RestTimer <= 0)
                {
                    RestTimer = RestTimerSet;
                }

                RestTimer -= _fixedDeltaTime;

                if (RestTimer < 0)
                {
                    if (!AtkFirstAppear)
                    {
                        RestEnd = true;
                        AtkFirstAppear = true;
                    }
                }
                break;
        }

        _hurtedController.HurtedMove(_fixedDeltaTime);
    }

    void SwitchAnimation()
    {
        NowAni = AniStatus.Wait;

        if (status == Status.walk)
        {
            NowAni = AniStatus.Walk;
        }

        if (status == Status.Atk)
        {
            NowAni = AniStatus.Atk;
        }

        if (status == Status.Rest)
        {
            NowAni = AniStatus.Wait;
        }

        if (LastAni != NowAni)
        {
            SEAppear = false;
        }

        switch (NowAni)
        {
            case AniStatus.Wait:
                AllAniFalse();
                MonsterMoveAnimation.SetActive(true);
                MonsterMoveAni.SetBool("isWalking", false);
                break;
            case AniStatus.Walk:
                AllAniFalse();
                MonsterMoveAnimation.SetActive(true);
                MonsterMoveAni.SetBool("isWalking", true);
                break;
            case AniStatus.Atk:
                AllAniFalse();
                MonsterAtkAnimation.SetActive(true);
                if (!SEAppear)
                {
                    AtkSource.PlayDelayed(AtkSoundTime);
                    SEAppear = true;
                }
                break;
        }

        if (LastAni != NowAni)
        {
            LastAni = NowAni;
        }
    }

    private void AllAniFalse()
    {
        if (NowAni != AniStatus.Walk && NowAni != AniStatus.Wait)
        {
            MonsterMoveAnimation.SetActive(false);
        }
        if (NowAni != AniStatus.Atk)
        {
            MonsterAtkAnimation.SetActive(false);
        }
    }
}
