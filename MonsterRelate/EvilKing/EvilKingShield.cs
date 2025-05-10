using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilKingShield : MonoBehaviour
{
    private float Hp;
    public int MaxHp;
    private Transform EvilKingTransform;
    private EvilKingController _controller;
    private Animator ShieldAni;
    private Animator ShieldAni2;
    private Animator ShieldAni3;
    private Animator ShieldAni4;
    public float RestoreTimerSet;
    private float RestoreTimer;
    private bool RestoreTimerSwitch;
    private bool isInPhase1;
    private bool isInPhase2;
    private bool isInPhase3;
    private bool Bool1;//避免isShieldDestroy被重複設置
    private bool isWalking;
    private bool HurtedTimerSwitch;
    private float HurtedTimerSet = 0.3f;
    private float HurtedTimer;
    private bool isHurted = false;

    private Transform _transform;
    private float _deltaTime;

    private GameObject ShieldRestoreAnimation;
    private GameObject ShieldAnimation;
    private GameObject Shield1PhaseAnimation;
    private GameObject Shield2PhaseAnimation;
    private GameObject Shield3PhaseAnimation;
    public GameObject ShieldDestroyAnimation;

    [Header("被大招攻擊")]
    public GameObject CriticAtkHurtedObject;
    public static bool isCriticAtkHurted = false;
    private bool CriticAtkHurtedSwitch;
    private bool HasCriticAtkAppear;
    private bool HasHurtedByCtiticAtk;
    private float CriticAtkHurtedTimerSet = 1.65f;
    private float CriticAtkHurtedTimer;
    private bool isHurtedByBigGun;

    void Start()
    {
        _transform = transform;

        Hp = MaxHp;
        EvilKingTransform = GameObject.Find("Boss3").transform;
        _controller = EvilKingTransform.GetComponent<EvilKingController>();
        ShieldRestoreAnimation = this.transform.GetChild(0).gameObject;
        ShieldAnimation = this.transform.GetChild(1).gameObject;
        Shield1PhaseAnimation = this.transform.GetChild(2).gameObject;
        Shield2PhaseAnimation = this.transform.GetChild(3).gameObject;
        Shield3PhaseAnimation = this.transform.GetChild(4).gameObject;
        ShieldAni = ShieldAnimation.GetComponent<Animator>();
        ShieldAni2 = Shield1PhaseAnimation.GetComponent<Animator>();
        ShieldAni3 = Shield2PhaseAnimation.GetComponent<Animator>();
        ShieldAni4 = Shield3PhaseAnimation.GetComponent<Animator>();
        RestoreTimerSwitch = true;
        RestoreTimer = RestoreTimerSet;
        this.transform.GetChild(6).DetachChildren();
    }

    void Update()
    {
        _deltaTime = Time.deltaTime;

        _transform.position = new Vector3(EvilKingTransform.position.x, EvilKingTransform.position.y + 0.18f, 0);
        if (Hp <= 0)
        {
            if(!Bool1)
            {
                _controller.isShieldDestroy = true;
                Bool1 = true;
            }
            Instantiate(ShieldDestroyAnimation, _transform.position, Quaternion.identity);
            Destroy(this.gameObject);
            return;
        }
        if (Hp <= (MaxHp * 0.25))
        {
            isInPhase3 = true;
            isInPhase2 = false;
            isInPhase1 = false;
            ShieldAnimation.SetActive(false);
            Shield1PhaseAnimation.SetActive(false);
            Shield2PhaseAnimation.SetActive(false);
            Shield3PhaseAnimation.SetActive(true);
        }
        if (Hp <= (MaxHp * 0.5) && !isInPhase3)
        {
            isInPhase2 = true;
            isInPhase1 = false;
            ShieldAnimation.SetActive(false);
            Shield1PhaseAnimation.SetActive(false);
            Shield2PhaseAnimation.SetActive(true);
        }
        if (Hp <= (MaxHp * 0.75) && !isInPhase2 && !isInPhase3)
        {
            isInPhase1 = true;
            ShieldAnimation.SetActive(false);
            Shield1PhaseAnimation.SetActive(true);
        }
        if (_controller.isWalking)
        {
            isWalking = true;
            if(!isInPhase1 && !isInPhase2 && !isInPhase3)
            {
                ShieldAni.SetBool("MoveBegin", true);
            }
            if (isInPhase1)
            {
                ShieldAni2.SetBool("MoveBegin", true);
            }
            if (isInPhase2)
            {
                ShieldAni3.SetBool("MoveBegin", true);
            }
            if (isInPhase3)
            {
                ShieldAni4.SetBool("MoveBegin", true);
            }
            if (_controller.isWalkMove)
            {
                if (!isInPhase1 && !isInPhase2 && !isInPhase3)
                {
                    ShieldAni.SetBool("MoveEnd", true);
                }
                if (isInPhase1)
                {
                    ShieldAni2.SetBool("MoveEnd", true);
                }
                if (isInPhase2)
                {
                    ShieldAni3.SetBool("MoveEnd", true);
                }
                if (isInPhase3)
                {
                    ShieldAni4.SetBool("MoveEnd", true);
                }
            }
        }
        else
        {
            if (isWalking)
            {
                if (!isInPhase1 && !isInPhase2 && !isInPhase3)
                {
                    ShieldAni.SetBool("MoveBegin", false);
                    ShieldAni.SetBool("MoveEnd", false);
                }
                if (isInPhase1)
                {
                    ShieldAni2.SetBool("MoveBegin", false);
                    ShieldAni2.SetBool("MoveEnd", false);
                }
                if (isInPhase2)
                {
                    ShieldAni3.SetBool("MoveBegin", false);
                    ShieldAni3.SetBool("MoveEnd", false);
                }
                if (isInPhase3)
                {
                    ShieldAni4.SetBool("MoveBegin", false);
                    ShieldAni4.SetBool("MoveEnd", false);
                }
                isWalking = false;
            }
        }
        RestoreTimerMethod();
        HurtedTimerMethod();
        CriticAtkHurtedTimerMethod();
    }
    private void FixedUpdate()
    {
        if (isHurtedByBigGun)
        {
            Hp -= BattleSystem.BigGunPower;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "bullet")
        {
            Hp -= BattleSystem.BulletHurtPower;
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "normalAtk")
        {
            BattleSystem.IncreaseTimes += BattleSystem.IncresePlayerPowerNumber;
            Hp -= BattleSystem.NormalAtkHurtPower;
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "CAtk")
        {
            Hp -= BattleSystem.CAtkHurtPower;
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "Cocktail" && !isHurted)
        {
            isHurted = true;
            HurtedTimerSwitch = true;
            Hp -= BattleSystem.ExplosionHurtPower / 2;
        }
        if (other.gameObject.tag == "ExplosinBottle" && !isHurted)
        {
            isHurted = true;
            HurtedTimerSwitch = true;
            Hp -= BattleSystem.ExplosionHurtPower;
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
    }

    void RestoreTimerMethod()
    {
        if (RestoreTimerSwitch)
        {
            RestoreTimer -= _deltaTime;
            if (RestoreTimer <= 0)
            {
                ShieldRestoreAnimation.SetActive(false);
                ShieldAnimation.SetActive(true);
                RestoreTimerSwitch = false;
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

    void CriticAtkHurtedTimerMethod()
    {
        if (CriticAtkHurtedSwitch)
        {
            CriticAtkHurtedTimer -= _deltaTime;
            if (!HasCriticAtkAppear)
            {
                Instantiate(CriticAtkHurtedObject, _transform.position, Quaternion.identity);
                HasCriticAtkAppear = true;
            }
            if (CriticAtkHurtedTimer <= (CriticAtkHurtedTimerSet - 0.95f))
            {
                if (!HasHurtedByCtiticAtk)
                {
                    Hp -= BattleSystem.CriticAtkHurtPower;
                    HasHurtedByCtiticAtk = true;
                }
                if (CriticAtkHurtedTimer <= 0)
                {
                    HasHurtedByCtiticAtk = false;
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
}
