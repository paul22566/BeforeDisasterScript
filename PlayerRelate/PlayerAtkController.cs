using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAtkController : MonoBehaviour, IAttackObject
{
    public int Damage;
    private AtkTrigger _trigger;
    public float AtkTimer;
    public bool CanBeBlock;
    private BattleSystem _battleSystem;
    private float _deltaTime;
    [HideInInspector] public bool CanHurt = true;
    private int CampID;

    private float LagTimer = 0.03f;

    public bool AdditionalSE;
    public GameObject HitWallSound;
    public GameObject HitMeetSound;
    public GameObject HitMetalSound;
    public GameObject HitSkinSound;
    public GameObject HitStoneSound;
    public GameObject HitPlasticSound;
    private bool HitSomething;
    private bool HitWall;
    private bool HitMeet;
    private bool HitMetal;
    private bool HitSkin;
    private bool HitStone;
    private bool HitPlastic;

    private bool TurnOffMeet;
    private bool TurnOffMetal;
    private bool TurnOffSkin;
    private bool TurnOffStone;
    private bool TurnOffPlastic;
    private bool HasSEPlay;
    private void Awake()
    {
        if (GameObject.Find("player") != null)
        {
            _battleSystem = GameObject.Find("player").GetComponent<BattleSystem>();
        }
    }

    void Update()
    {
        _deltaTime = Time.deltaTime;
        AtkTimer -= _deltaTime;

        if (!HasSEPlay && HitSomething && !AdditionalSE)
        {
            LagTimer -= _deltaTime;

            if(LagTimer < 0)
            {
                TurnOffUnValidSE();
                ChoosePlaySE();
                HasSEPlay = true;
            }
        }

        if (AtkTimer <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<CollisionType>() != null && !collision.GetComponent<CollisionType>().BeAtkNoSound)
        {
            switch (collision.GetComponent<CollisionType>()._type)
            {
                case CollisionType.Type.Meet:
                    HitMeet = true;
                    break;
                case CollisionType.Type.Metal:
                    HitMetal = true;
                    break;
                case CollisionType.Type.Skin:
                    HitSkin = true;
                    break;
                case CollisionType.Type.Cement:
                    HitWall = true;
                    break;
                case CollisionType.Type.Stone:
                    HitStone = true;
                    break;
                case CollisionType.Type.Plastic:
                    HitPlastic = true;
                    break;
            }
            HitSomething = true;
        }
        if (collision.tag == "MonsterBlockJudgement" && CanBeBlock)
        {
            if (!_battleSystem.isBlock && collision.GetComponent<BlockJudgement>().CanBlock)
            {
                HitSomething = false;
                BattleSystem.isBeBlockSuccess = true;
                BackgroundSystem.CantPause = true;
                BackgroundSystem.GameSpeed = 0;
                CanHurt = false;
            }
        }
    }

    private void ChoosePlaySE()
    {
        if (HitMeet)
        {
            Instantiate(HitMeetSound);
            return;
        }
        if (HitMetal)
        {
            Instantiate(HitMetalSound);
            return;
        }
        if (HitPlastic)
        {
            Instantiate(HitPlasticSound);
            return;
        }
        if (HitSkin)
        {
            Instantiate(HitSkinSound);
            return;
        }
        if (HitStone)
        {
            Instantiate(HitStoneSound);
            return;
        }
        if (HitWall)
        {
            Instantiate(HitWallSound);
            return;
        }
    }

    private void TurnOffUnValidSE()
    {
        if (TurnOffMeet)
        {
            HitMeet = false;
        }
        if (TurnOffMetal)
        {
            HitMetal = false;
        }
        if (TurnOffSkin)
        {
            HitSkin = false;
        }
        if (TurnOffStone)
        {
            HitStone = false;
        }
        if (TurnOffPlastic)
        {
            HitPlastic = false;
        }
    }

    public void HitShield(CollisionType.Type _ProtectedType)
    {
        switch(_ProtectedType)
        {
            case CollisionType.Type.Meet:
                TurnOffMeet = true;
                break;
            case CollisionType.Type.Metal:
                TurnOffMetal = true;
                break;
            case CollisionType.Type.Skin:
                TurnOffSkin = true;
                break;
            case CollisionType.Type.Stone:
                TurnOffStone = true;
                break;
            case CollisionType.Type.Plastic:
                TurnOffPlastic = true;
                break;
        }
    }

    public void InitializeAtk(AtkData _data)
    {
        Damage *= (int)_data.AtkRate;
        CampID = _data.CampID;
        _trigger = this.transform.GetChild(0).GetComponent<AtkTrigger>();
        _trigger.OnMakeDamage += MakeDamage;
    }

    public void MakeDamage(IHurtedObject _hurtedObject)
    {
        if (CampID != _hurtedObject.GetCamp())
        {
            _hurtedObject.HurtedControll(Damage);
        }
    }
}
