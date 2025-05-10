using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMonsterAtk : MonoBehaviour
{
    public enum AtkType { Normal, Laser, NoSound}//�M�w���a���ˮ`���n��
    public AtkType Type;

    public bool isCompleleAtk;
    public bool isSeparateAtk;
    public bool isForeverAtk;

    public bool NoAvoid;
    public bool CanBeBlock;//script�Ψ� (playerBlockJudgement�Aplayercontroller�AdragonFlash)
    public float ImpulsePowerX;//script�Ψ� (playercontroller)
    public float ImpulsePowerY;//script�Ψ� (playercontroller)
    public float AtkTimer;
    public int Damage;//script�Ψ� (playercontroller)
    [HideInInspector] public bool isMoveAtk;
    [HideInInspector] public GameObject Parent;//���Q��Lscript�Ψ�(�U�ا���follow)

    private void Awake()
    {
        //������˪�����
        if (!isSeparateAtk)
        {
            if (isCompleleAtk)
            {
                Parent = this.transform.parent.transform.parent.transform.parent.gameObject;
            }
            else
            {
                Parent = this.transform.parent.gameObject.transform.parent.gameObject;
            }
        }
    }

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
        if(Parent == null && !isSeparateAtk)
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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerBlockJudgement" && CanBeBlock)
        {
            if (collision.GetComponent<PlayerBlockJudgement>().isRBlock)
            {
                if(collision.transform.position.x <= this.transform.position.x)
                {
                    Parent.GetComponent<MonsterBlockController>().BeBlockSuccess = true;
                    Destroy(this.gameObject);
                }
            }
            if (collision.GetComponent<PlayerBlockJudgement>().isLBlock)
            {
                if (collision.transform.position.x >= this.transform.position.x)
                {
                    Parent.GetComponent<MonsterBlockController>().BeBlockSuccess = true;
                    Destroy(this.gameObject);
                }
            }
        }
    }
}
