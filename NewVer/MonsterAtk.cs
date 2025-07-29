using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAtk : MonoBehaviour, IAttackObject
{
    private Transform _transform;
    private AtkTrigger _trigger;
    private int CampID;
    public enum AtkType { Normal, Laser, NoSound }//�M�w���a���ˮ`���n��
    public AtkType Type;

    public bool isCompleleAtk;
    public bool isSeparateAtk;
    public bool isForeverAtk;
    public bool isFollowMonster;

    public bool NoAvoid;
    public bool CanBeBlock;//script�Ψ� (playerBlockJudgement�Aplayercontroller�AdragonFlash)
    public float ImpulsePowerX;//script�Ψ� (playercontroller)
    public float ImpulsePowerY;//script�Ψ� (playercontroller)
    public float AtkTimer;
    public int Damage;//script�Ψ� (playercontroller)
    [HideInInspector] public bool isMoveAtk;
    private Transform FollowTarget;//���Q��Lscript�Ψ�(�U�ا���follow)

    private void Start()
    {
        if (this.GetComponent<MoveAtk>() != null)
        {
            isMoveAtk = true;
            NoAvoid = true;
        }//�۰ʶ}�� �٥h�·�
    }

    void Update()
    {
        if (FollowTarget == null && !isSeparateAtk)
        {
            Destroy(this.gameObject);
            return;
        }

        if (!isForeverAtk)
        {
            AtkTimer -= Time.deltaTime;
            if (AtkTimer <= 0)
            {
                Destroy(this.gameObject);
            }
        }

        if (isFollowMonster && FollowTarget != null)
        {
            Vector3 followPos = new Vector3(FollowTarget.position.x, FollowTarget.position.y, _transform.position.z);
            _transform.position = followPos;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerBlockJudgement" && CanBeBlock)
        {
            if (collision.GetComponent<PlayerBlockJudgement>().isRBlock)
            {
                if (collision.transform.position.x <= this.transform.position.x)
                {
                    //FollowTarget.GetComponent<MonsterBlockController>().BeBlockSuccess = true;
                    Destroy(this.gameObject);
                }
            }
            if (collision.GetComponent<PlayerBlockJudgement>().isLBlock)
            {
                if (collision.transform.position.x >= this.transform.position.x)
                {
                    //FollowTarget.GetComponent<MonsterBlockController>().BeBlockSuccess = true;
                    Destroy(this.gameObject);
                }
            }
        }
    }

    public void InitializeAtk(AtkData _data)
    {
        Damage *= (int)_data.AtkRate;
        CampID = _data.CampID;
        FollowTarget = _data.MainObjectTransform;
        _transform = this.transform;
        _trigger = _transform.GetChild(0).GetComponent<AtkTrigger>();
        _trigger.OnMakeDamage += MakeDamage;
    }
    public void MakeDamage(IHurtedObject _hurtedObject)
    {
        if (CampID != _hurtedObject.GetCamp())
        {
            _hurtedObject.HurtedControll(Damage);
        }
    }
    public void HurtPlayer(PlayerController player)
    {
        Debug.Log("atk�覡�ݧﵽ");
        try
        {
            player.NewRecordMonsterAtkData(this, _transform);
        }
        catch
        {
            Debug.Log("noObject");
        }
    }
}
