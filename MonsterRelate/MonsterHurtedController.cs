using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHurtedController : MonoBehaviour
{
    /*
     使用到被大招打功能時需要重置怪物腳本中的部分變數，動畫也需另外設置
     
     BurningTimerMethod 、HurtedMove放fixedupdate
     */
    [HideInInspector] public bool isHurted;//script(怪物script，怪物hurted，blockJudgement)
    [HideInInspector] public bool HurtedByFarAtk;
    [HideInInspector] public float HurtedTimer;//script(GhostController)
    [HideInInspector] public float HurtedTimerSet = 0.3f;//script(GhostController)
    private float HurtedSpeed;
    [SerializeField] private float HurtedSpeedSet;
    public int ImpulsePowerY;
    private bool BlockControllerExist;
    private Transform _transform;
    private MonsterBasicData _basicData;
    private Rigidbody2D Rigid2D;
    private MonsterBlockController _blockController;
    [SerializeField] private float HurtedRate = 1;
    [HideInInspector] public int HurtedNumber = 0;
    private PlayerAtkController NowPlayerAtk;
    [SerializeField] private bool UseAdditionalTrigger;
    [SerializeField] private bool isDelayJudge;
    private float LagTimerSet = 0.03f;
    private float LagTimer;
    private bool CanStart = false;
    private bool CanJudge = false;
    private float _deltaTime;
    public ObjectShield _shield;

    [Header("被大招攻擊")]
    [HideInInspector] public bool isHurtedByBigGun;
    public GameObject CriticAtkHurtedObject;
    [HideInInspector] public bool isCriticAtkHurted;
    private bool CriticAtkHurtedSwitch;
    private bool HasCriticAtkAppear;
    [SerializeField] private float CriticAtkHurtedTimerSet = 2.6f;//2.6是預設
    private float CriticAtkHurtedTimer;
    private bool hasReact;
    [HideInInspector] public bool BeCriticAtkEnd;
    public delegate void GetCriticHurted();
    public event GetCriticHurted _getCriticHurted;
    public event GetCriticHurted _reactCriticHurted;//動畫層面的反應(停滯結束的瞬間)

    [Header("燃燒")]
    public GameObject Fire;
    public bool HasBurningDie;
    public bool IgnoreBurning;
    private GameObject _fire;
    private bool isBurning;
    private bool isFireAppear;
    private float BurningTimer;
    private float BurningTimerSet = 5;
    // Start is called before the first frame update
    void Start()
    {
        HurtedTimerSet = 0.3f;
        HurtedTimer = HurtedTimerSet;
        HurtedSpeed = HurtedSpeedSet;
        _basicData = this.GetComponent<MonsterBasicData>();
        Rigid2D = this.GetComponent<Rigidbody2D>();
        _transform = this.transform;
        if(this.GetComponent<MonsterBlockController>() != null)
        {
            _blockController = this.GetComponent<MonsterBlockController>();
            BlockControllerExist = true;
        }

        _getCriticHurted += StartCriticAtkHurted;
        _reactCriticHurted += DefaultCriticHurtReact;
    }

    private void Update()
    {
        _deltaTime = Time.deltaTime;

        if (CanStart)
        {
            LagTimer -= _deltaTime;

            if (LagTimer <= 0)
            {
                CanStart = false;
                CanJudge = true;
            }
        }

        if (CanJudge)
        {
            HurtedJudge();
            CanJudge = false;
        }

        //處理僅有盾牌受到攻擊的情況
        if (isDelayJudge && !CanStart && _shield.ProtectSuccess)
        {
            _shield.ProtectSuccess = false;
        }
    }

    private void FixedUpdate()
    {
        if (isHurtedByBigGun)
        {
            _basicData.hp -= BattleSystem.BigGunPower;
        }
    }

    public void HurtedTimerMethod(float _deltaTime)//script(怪物script)
    {
        if (isHurted)
        {
            HurtedTimer -= _deltaTime;
            if (HurtedTimer <= 0)
            {
                isHurted = false;
            }
        }
        else
        {
            HurtedTimer = HurtedTimerSet;
        }
    }

    public void HurtedMove(float _fixedDeltaTime)
    {
        if (isHurted)
        {
            if (_basicData.isPlayerAtRightSide && !_basicData.touchLeftWall)
            {
                _transform.localPosition = new Vector3(-HurtedSpeed * _fixedDeltaTime + _transform.localPosition.x, _transform.localPosition.y, _transform.localPosition.z);
                HurtedSpeed -= HurtedSpeedSet / ((HurtedTimerSet - 0.1f) * 50);
                if (HurtedSpeed <= 0)
                {
                    HurtedSpeed = 0;
                }
            }
            if (_basicData.isPlayerAtLeftSide && !_basicData.touchRightWall)
            {
                _transform.localPosition = new Vector3(HurtedSpeed * _fixedDeltaTime + _transform.localPosition.x, _transform.localPosition.y, _transform.localPosition.z);
                HurtedSpeed -= HurtedSpeedSet / ((HurtedTimerSet - 0.1f) * 50);
                if (HurtedSpeed <= 0)
                {
                    HurtedSpeed = 0;
                }
            }
        }
        else
        {
            HurtedSpeed = HurtedSpeedSet;
        }
    }

    public void Boss1HurtedTimerMethod(ref bool SpecialBool, float _deltaTime)//script(VeryBigMonsterController)
    {
        if (isHurted)
        {
            HurtedTimer -= _deltaTime;
            if (HurtedTimer <= 0)
            {
                isHurted = false;
                SpecialBool = false;
            }
        }
        else
        {
            HurtedTimer = HurtedTimerSet;
        }
    }

    private void StartCriticAtkHurted()
    {
        isCriticAtkHurted = true;
        CriticAtkHurtedSwitch = true;
    }

    public void CriticAtkHurtedTimerMethod(float _deltaTime)
    {
        if (CriticAtkHurtedSwitch)
        {
            CriticAtkHurtedTimer -= _deltaTime;
            if (CriticAtkHurtedTimer <= (CriticAtkHurtedTimerSet - 1))
            {
                if (!HasCriticAtkAppear)
                {
                    Instantiate(CriticAtkHurtedObject, _transform.position, Quaternion.identity);
                    HasCriticAtkAppear = true;
                }
            }
            if (CriticAtkHurtedTimer <= (CriticAtkHurtedTimerSet - 1.9f))
            {
                if (!hasReact)
                {
                    if (_reactCriticHurted != null)
                    {
                        _reactCriticHurted();
                    }
                    _basicData.hp -= BattleSystem.CriticAtkHurtPower;
                    hasReact = true;
                }
                if (CriticAtkHurtedTimer <= 0)
                {
                    hasReact = false;
                    HasCriticAtkAppear = false;
                    BeCriticAtkEnd = true;
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

    private void DefaultCriticHurtReact()
    {
        Rigid2D.AddForce(new Vector2(0, ImpulsePowerY), ForceMode2D.Impulse);
    }

    public void StartBurning()
    {
        isBurning = true;
        BurningTimer = BurningTimerSet;
    }

    public void BurningTimerMethod(Transform AtkTemporaryArea, float _fixedDeltaTime)
    {
        if (isBurning)
        {
            BurningTimer -= _fixedDeltaTime;
            if (!isFireAppear)
            {
                Instantiate(Fire, _transform.position, Quaternion.identity, AtkTemporaryArea);
                _fire = AtkTemporaryArea.GetChild(0).gameObject;
                AtkTemporaryArea.DetachChildren();
                isFireAppear = true;
            }
            if (_basicData.hp > 0)
            {
                _basicData.hp -= BattleSystem.CocktailHurtPower;
            }
            if (BurningTimer <= 0)
            {
                Destroy(_fire);
                isBurning = false;
                isFireAppear = false;
                BurningTimer = BurningTimerSet;
            }
        }
    }

    //受傷害
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<PlayerAtkController>() != null && collider.GetComponent<PlayerAtkController>().CanHurt && !isHurted && !UseAdditionalTrigger)
        {
            DetectHurtedType(collider.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BigGun")
        {
            isHurtedByBigGun = false;
        }
    }

    
    public void DetectHurtedType(GameObject Object)
    {
        NowPlayerAtk = Object.GetComponent<PlayerAtkController>();
        switch(Object.tag)
        {
            case "bullet":
                HurtedNumber = 1;
                if (NowPlayerAtk.gameObject != null)
                {
                    Destroy(NowPlayerAtk.gameObject);
                }
                break;
            case "normalAtk":
                HurtedNumber = 2;
                break;
            case "CAtk":
                HurtedNumber = 3;
                break;
            case "ExplosionBottle":
                HurtedNumber = 4;
                break;
            case "Cocktail":
                HurtedNumber = 5;
                break;
            case "BNAtk":
                HurtedNumber = 6;
                break;
            case "BSAtk":
                HurtedNumber = 7;
                break;
            case "CriticAtk":
                HurtedNumber = 8;
                break;
            case "BigGun":
                HurtedNumber = 9;
                break;
        }

        if (isDelayJudge)
        {
            CanStart = true;
            LagTimer = LagTimerSet;
        }
        else
        {
            CanJudge = true;
        }
    } 

    private void HurtedJudge()
    {
        if (isDelayJudge && _shield.ProtectSuccess)
        {
            _shield.ProtectSuccess = false;
            return;
        }
        HurtedControll(HurtedRate);
    }

    public void HurtedControll(float Rate)
    {
        switch (HurtedNumber)
        {
            case 1:
                HurtedByFarAtk = true;
                _basicData.hp -= BattleSystem.BulletHurtPower * Rate;
                break;
            case 2:
                if (!BlockControllerExist)
                {
                    isHurted = true;
                    BattleSystem.IncreaseTimes += (int)(BattleSystem.IncresePlayerPowerNumber * Rate);
                    _basicData.hp -= BattleSystem.NormalAtkHurtPower * Rate;
                }
                else
                {
                    if (_blockController.isBlock)
                    {
                        switch (_basicData.face)
                        {
                            case MonsterBasicData.Face.Left:
                                if (_basicData.isPlayerAtRightSide)
                                {
                                    isHurted = true;
                                    BattleSystem.IncreaseTimes += (int)(BattleSystem.IncresePlayerPowerNumber * Rate);
                                    _basicData.hp -= BattleSystem.NormalAtkHurtPower * Rate;
                                }
                                break;
                            case MonsterBasicData.Face.Right:
                                if (_basicData.isPlayerAtLeftSide)
                                {
                                    isHurted = true;
                                    BattleSystem.IncreaseTimes += (int)(BattleSystem.IncresePlayerPowerNumber * Rate);
                                    _basicData.hp -= BattleSystem.NormalAtkHurtPower * Rate;
                                }
                                break;
                        }
                    }
                    else
                    {
                        isHurted = true;
                        BattleSystem.IncreaseTimes += (int)(BattleSystem.IncresePlayerPowerNumber * Rate);
                        _basicData.hp -= BattleSystem.NormalAtkHurtPower * Rate;
                    }
                }
                break;
            case 3:
                isHurted = true;
                _basicData.hp -= BattleSystem.CAtkHurtPower * Rate;
                break;
            case 4:
                HurtedByFarAtk = true;
                isHurted = true;
                _basicData.hp -= BattleSystem.ExplosionHurtPower * Rate;
                break;
            case 5:
                if (!IgnoreBurning)
                {
                    StartBurning();
                    HurtedByFarAtk = true;
                    if (!HasBurningDie)
                    {
                        return;
                    }
                    else
                    {
                        this.GetComponent<MonsterBasicData>()._deadInformation.BurningDie = true;
                        return;
                    }
                }
                break;
            case 6:
                if(BlockControllerExist && _blockController.BeBlockSuccess)
                {
                    isHurted = true;
                    _blockController.isHurtedByNormalAtk = true;
                    BattleSystem.IncreaseTimes += BattleSystem.IncresePlayerPowerNumber;
                    _basicData.hp -= BattleSystem.BlockNormalAtkHurtPower * Rate;
                }
                break;
            case 7:
                if (BlockControllerExist && _blockController.BeBlockSuccess)
                {
                    isHurted = true;
                    _blockController.isHurtedByCAtk = true;
                    _basicData.hp -= BattleSystem.BlockStrongAtkHurtPower * Rate;
                }
                break;
            case 8:
                if (_getCriticHurted != null)
                {
                    _getCriticHurted();
                }
                break;
            case 9:
                isHurtedByBigGun = true;
                break;
        }
        HurtedNumber = 0;
    }
}
