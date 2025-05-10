using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    [Header("�򥻰Ѽ�")]
    public int ChasingDistance;
    private float speed = 200;
    private float SlowWalkSpeed = 100;
    public enum Status { wait, walk};
    private Status status;
    private enum WalkStatus { chasing, wait };
    private WalkStatus walkStatus;

    [Header("�t�ΰѼ�")]
    private Transform AtkTemporaryArea;
    private MonsterBasicData _basicData;
    private Transform _transform;
    private float _deltaTime;
    private float _fixDeltaTime;
    public float SlowWalkTimerSet;
    private float SlowWalkTimer;
    private float TurnFaceLagTimerSet = 0.05f;
    private float TurnFaceLagTimer;
    private Rigidbody2D Rigid2D;
    private MonsterHurtedController _hurtedController;
    private FloatController _floatController = new FloatController();
    private float DistanceProportion;
    public GameObject Atk;
    private GameObject NowAtk;

    //�}��
    private bool slowWalkTimerSwitch;
    private bool isAlertByShoot;
    private bool isFirstTurnRun;

    [Header("�ʵe��������")]
    private SpriteRenderer thisSpr;

    public enum FloatStatus { CPositive, CNegetive };
    [HideInInspector] public FloatStatus floatStatus;//script(Dialog)

    void Start()
    {//����ʵe��������
        thisSpr = this.GetComponent<SpriteRenderer>();
        AtkTemporaryArea = this.transform.GetChild(0);

        walkStatus = WalkStatus.wait;
        status = Status.wait;

        _basicData = this.GetComponent<MonsterBasicData>();
        _hurtedController = this.GetComponent<MonsterHurtedController>();

        _basicData.BasicVarInisialize(thisSpr, "L");

        TurnFaceLagTimer = TurnFaceLagTimerSet;
        _transform = this.transform;
        Rigid2D = this.gameObject.GetComponent<Rigidbody2D>();

        _floatController.FloatVarInisialize(_transform, 4, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        _deltaTime = Time.deltaTime;
        _basicData.DieJudge();

        _hurtedController.HurtedTimerMethod(_deltaTime);
        _hurtedController.CriticAtkHurtedTimerMethod(_deltaTime);

        //�Q�j�ۥ������
        if (_hurtedController.isCriticAtkHurted || _hurtedController.isHurtedByBigGun)
        {
            status = Status.walk;
            walkStatus = WalkStatus.wait;
            Rigid2D.velocity = new Vector2(0, 0);
        }
        //return
        if (GameEvent.isAniPlay || _hurtedController.isCriticAtkHurted || _hurtedController.isHurtedByBigGun)
        {
            return;
        }

        //�p��Z��
        _basicData.DistanceCalculate();
        if (_basicData.playerTransform)
        {
            if(_basicData.AbsDistanceX < 0.01)
            {
                _basicData.AbsDistanceX = 0.01f;
                DistanceProportion = _basicData.AbsDistanceY / _basicData.AbsDistanceX;
            }
            else
            {
                DistanceProportion = _basicData.AbsDistanceY / _basicData.AbsDistanceX;
            }
        }

        //�Q���{���ɪ�����
        if (_hurtedController.HurtedByFarAtk && !isAlertByShoot && status == Status.wait)
        {
            isAlertByShoot = true;
            status = Status.walk;
            walkStatus = WalkStatus.chasing;
            _basicData.TurnFaceJudge();
        }
        if (_hurtedController.HurtedByFarAtk)
        {
            _hurtedController.HurtedByFarAtk = false;
        }
        //�Q�j�ۥ����᪺�ʧ@
        if (_hurtedController.BeCriticAtkEnd)
        {
            status = Status.walk;
            _hurtedController.BeCriticAtkEnd = false;
        }

        //��V����
        switch (_basicData.face)
        {
            case MonsterBasicData.Face.Left:
                thisSpr.flipX = false;
                break;

            case MonsterBasicData.Face.Right:
                thisSpr.flipX = true;
                break;
        }
        //�����ͦ�
        if (NowAtk == null)
        {
            Instantiate(Atk, _transform.position, Quaternion.identity, AtkTemporaryArea);
            NowAtk = AtkTemporaryArea.GetChild(0).gameObject;
            walkStatus = WalkStatus.wait;
            AtkTemporaryArea.DetachChildren();
        }

        //�Ǫ�AI
        switch (status)
        {
            case Status.wait:
                if (_basicData.playerTransform)
                {
                    if (!isFirstTurnRun)
                    {
                        //�b�o�̩���V�P�w�O���F�קK�Ǫ��@�X�ͧ�����V���~Bug
                        _basicData.TurnFaceJudge();
                    }

                    _floatController.Float(_transform, _deltaTime);

                    if (_basicData.AbsDistanceX < ChasingDistance && _basicData.AbsDistanceY < ChasingDistance && isFirstTurnRun)
                    {
                        status = Status.walk;
                        walkStatus = WalkStatus.chasing;
                    }
                }
                break;
            case Status.walk:
                break;
        }

        isFirstTurnRun = true;

        SlowWalkTimerMethod();
    }

    private void FixedUpdate()
    {
        _fixDeltaTime = Time.fixedDeltaTime;

        if (_hurtedController.isCriticAtkHurted || _hurtedController.isHurtedByBigGun)
        {
            return;
        }

        if (_basicData.playerTransform && status == Status.walk && !_hurtedController.isHurted)
        {
            switch (walkStatus)
            {
                case WalkStatus.chasing:
                    _basicData.LagTurnFaceJudge(ref TurnFaceLagTimer, TurnFaceLagTimerSet, _fixDeltaTime);

                    switch (_basicData.face)
                    {
                        case MonsterBasicData.Face.Right:
                            if (_transform.position.y >= _basicData.playerTransform.position.y)
                            {
                                Rigid2D.velocity = new Vector2(speed / (Mathf.Pow((DistanceProportion * DistanceProportion + 1), 0.5f)) * _fixDeltaTime, -speed * DistanceProportion / (Mathf.Pow((DistanceProportion * DistanceProportion + 1), 0.5f)) * _fixDeltaTime);
                            }
                            if (_transform.position.y < _basicData.playerTransform.position.y)
                            {
                                Rigid2D.velocity = new Vector2(speed / (Mathf.Pow((DistanceProportion * DistanceProportion + 1), 0.5f)) * _fixDeltaTime, speed * DistanceProportion / (Mathf.Pow((DistanceProportion * DistanceProportion + 1), 0.5f)) * _fixDeltaTime);
                            }
                            break;
                        case MonsterBasicData.Face.Left:
                            if (_transform.position.y >= _basicData.playerTransform.position.y)
                            {
                                Rigid2D.velocity = new Vector2(-speed / (Mathf.Pow((DistanceProportion * DistanceProportion + 1), 0.5f)) * _fixDeltaTime, -speed / (Mathf.Pow((DistanceProportion * DistanceProportion + 1), 0.5f)) * DistanceProportion * _fixDeltaTime);
                            }
                            if (_transform.position.y < _basicData.playerTransform.position.y)
                            {
                                Rigid2D.velocity = new Vector2(-speed / (Mathf.Pow((DistanceProportion * DistanceProportion + 1), 0.5f)) * _fixDeltaTime, speed / (Mathf.Pow((DistanceProportion * DistanceProportion + 1), 0.5f)) * DistanceProportion * _fixDeltaTime);
                            }
                            break;
                    }
                    break;

                case WalkStatus.wait:
                    _basicData.TurnFaceJudge();
                    slowWalkTimerSwitch = true;
                    switch (_basicData.face)
                    {
                        case MonsterBasicData.Face.Right:
                            Rigid2D.velocity = new Vector2(-SlowWalkSpeed / (Mathf.Pow((DistanceProportion * DistanceProportion + 1), 0.5f)) * _fixDeltaTime, SlowWalkSpeed / (Mathf.Pow((DistanceProportion * DistanceProportion + 1), 0.5f)) * DistanceProportion * _fixDeltaTime);
                            break;
                        case MonsterBasicData.Face.Left:
                            Rigid2D.velocity = new Vector2(SlowWalkSpeed / (Mathf.Pow((DistanceProportion * DistanceProportion + 1), 0.5f)) * _fixDeltaTime, SlowWalkSpeed / (Mathf.Pow((DistanceProportion * DistanceProportion + 1), 0.5f)) * DistanceProportion * _fixDeltaTime);
                            break;
                    }
                    break;
            }
        }

        _hurtedController.HurtedMove(_fixDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            if(!_basicData.playerTransform.GetComponent<PlayerController>().isDash && !_basicData.playerTransform.GetComponent<PlayerController>().WeakInvincible)
            {
                walkStatus = WalkStatus.wait;
                slowWalkTimerSwitch = true;
            }
        }
    }

    void SlowWalkTimerMethod()
    {
        if (slowWalkTimerSwitch)
        {
            SlowWalkTimer -= _deltaTime;
            if (SlowWalkTimer <= 0)
            {
                walkStatus = WalkStatus.chasing;
                slowWalkTimerSwitch = false;
            }
        }
        else
        {
            SlowWalkTimer = SlowWalkTimerSet;
        }
    }
}
