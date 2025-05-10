using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RestRoomEnemyEscape : MonoBehaviour
{
    private MonsterBasicData _basicData;
    private BoxCollider2D _boxCollider;
    private Transform TempoparyArea;
    private float _fixedDeltaTime;
    private bool GoRight;
    private bool GoLeft;
    private Transform AniTransform;
    private Transform StopTransform;
    public float EscapeTimerSet;
    private float EscapeTimer;
    public float Speed;
    public float FastSpeed;
    private GameObject EscapeAnimation;
    private GameObject StopAnimation;
    private PlayerData _PlayerData;
    private bool TriggerMove1;

    public GameObject CriticAtkHurtedObject;
    private bool isCriticAtkHurted;
    private bool CriticAtkHurtedSwitch;
    private bool HasCriticAtkAppear;
    private float CriticAtkHurtedTimerSet = 0.95f;
    private float CriticAtkHurtedTimer;

    private MonsterDieController _dieController;
    private MonsterDeadInformation _MonsterDeadInformation = new MonsterDeadInformation();

    private enum EscapeStatus { walkLeft, wait, walkRight, walkRightFast, walkLeftFast };
    private EscapeStatus escapeStatus;
    // Start is called before the first frame update
    void Start()
    {
        GameEvent.SaveSurvivor = true;
        if (GameObject.Find("FollowSystem") != null)
        {
            _PlayerData = GameObject.Find("FollowSystem").GetComponent<PlayerData>();
        }
        if (GameObject.Find("system") != null)
        {
            GameObject.Find("system").GetComponent<RestRoomController>().EscapeEnemy = this.transform;
        }
        _basicData = this.GetComponent<MonsterBasicData>();
        _boxCollider = this.GetComponent<BoxCollider2D>();
        _dieController = this.GetComponent<MonsterDieController>();
        EscapeAnimation = this.transform.GetChild(0).gameObject;
        StopAnimation = this.transform.GetChild(1).gameObject;
        TempoparyArea = this.transform.GetChild(2);
        AniTransform = EscapeAnimation.transform.GetChild(0);
        StopTransform = StopAnimation.transform.GetChild(0);
        EscapeTimer = EscapeTimerSet;
        escapeStatus = EscapeStatus.wait;
        CriticAtkHurtedTimer = CriticAtkHurtedTimerSet;

        TempoparyArea.DetachChildren();

        _basicData.hp = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (_basicData.hp <= 0)
        {
            RestRoomController.RestRoomKilledNumber += 1;
            _dieController.BeginDie(_MonsterDeadInformation);
            GameEvent.SaveSurvivor = false;
            _PlayerData.CommonSave();
            Destroy(this.gameObject);
            return;
        }
        CriticAtkHurtedTimerMethod();
        if (isCriticAtkHurted)
        {
            return;
        }

        _basicData.MonsterPlace = this.transform.position;

        EscapeTimer -= Time.deltaTime;

        if (PlayerController.PlayerPlaceX - this.transform.position.x >= 0)
        {
            GoLeft = true;
            GoRight = false;
        }
        else
        {
            GoRight = true;
            GoLeft = false;
        }
        if (GoRight)
        {
            _MonsterDeadInformation.FaceRight = true;
            AniTransform.localScale = new Vector3(-0.28f, 0.28f, 0);
            StopTransform.localScale = new Vector3(0.28f, 0.28f, 0);
        }
        if (GoLeft)
        {
            _MonsterDeadInformation.FaceLeft = true;
            AniTransform.localScale = new Vector3(0.28f, 0.28f, 0);
            StopTransform.localScale = new Vector3(0.28f, 0.28f, 0);
        }

        escapeStatus = EscapeStatus.wait;
        if (EscapeTimer <= (EscapeTimerSet - 0.3) && !TriggerMove1)
        {
            if (GoRight)
            {
                _boxCollider.offset = new Vector2(0.46f, _boxCollider.offset.y);
                TriggerMove1 = true;
            }
            if (GoLeft)
            {
                _boxCollider.offset = new Vector2(-0.46f, _boxCollider.offset.y);
                TriggerMove1 = true;
            }
        }
        if (EscapeTimer <= (EscapeTimerSet - 2.1))
        {
            if (GoRight)
            {
                escapeStatus = EscapeStatus.walkRight;
            }
            if (GoLeft)
            {
                escapeStatus = EscapeStatus.walkLeft;
            }
            if (EscapeTimer <= (EscapeTimerSet - 3))
            {
                if (GoRight)
                {
                    escapeStatus = EscapeStatus.walkRightFast;
                }
                if (GoLeft)
                {
                    escapeStatus = EscapeStatus.walkLeftFast;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        _fixedDeltaTime = Time.fixedDeltaTime;
        switch (escapeStatus)
        {
            case EscapeStatus.wait:
                break;
            case EscapeStatus.walkLeft:
                this.transform.position -= new Vector3(Speed * _fixedDeltaTime, 0, 0);
                break;
            case EscapeStatus.walkRight:
                this.transform.position += new Vector3(Speed * _fixedDeltaTime, 0, 0);
                break;
            case EscapeStatus.walkRightFast:
                this.transform.position += new Vector3(FastSpeed * _fixedDeltaTime, 0, 0);
                break;
            case EscapeStatus.walkLeftFast:
                this.transform.position -= new Vector3(FastSpeed * _fixedDeltaTime, 0, 0);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "bullet")
        {
            _basicData.hp -= BattleSystem.BulletHurtPower;
        }
        if (collision.gameObject.tag == "normalAtk")
        {
            BattleSystem.IncreaseTimes += BattleSystem.IncresePlayerPowerNumber;
            _basicData.hp -= BattleSystem.NormalAtkHurtPower;
        }
        if (collision.gameObject.tag == "CAtk")
        {
            _basicData.hp -= BattleSystem.CAtkHurtPower;
        }
        if (collision.gameObject.tag == "Cocktail")
        {
            RestRoomController.RestRoomKilledNumber += 1;
            _MonsterDeadInformation.BurningDie = true;
            _dieController.BeginDie(_MonsterDeadInformation);
            GameEvent.SaveSurvivor = false;
            _PlayerData.CommonSave();
            Destroy(this.gameObject);
            return;
        }
        if (collision.gameObject.tag == "ExplosionBottle")
        {
            _basicData.hp -= BattleSystem.ExplosionHurtPower;
        }
        if (collision.gameObject.tag == "CriticAtk")
        {
            isCriticAtkHurted = true;
            CriticAtkHurtedSwitch = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Portal")
        {
            Destroy(this.gameObject);
        }
    }

    void CriticAtkHurtedTimerMethod()
    {
        if (CriticAtkHurtedSwitch)
        {
            CriticAtkHurtedTimer -= Time.deltaTime;
            if (!HasCriticAtkAppear)
            {
                Instantiate(CriticAtkHurtedObject, this.transform.position, Quaternion.identity);
                HasCriticAtkAppear = true;
                EscapeAnimation.SetActive(false);
                StopAnimation.SetActive(true);
            }
            if (CriticAtkHurtedTimer <= 0)
            {
                _basicData.hp -= BattleSystem.CriticAtkHurtPower;
            }
        }
    }
}
