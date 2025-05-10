using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMonsterAtk : MonoBehaviour
{
    public enum AtkType { Normal, Laser, NoSound}//決定玩家受傷害的聲音
    public AtkType Type;

    public bool isCompleleAtk;
    public bool isSeparateAtk;
    public bool isForeverAtk;

    public bool NoAvoid;
    public bool CanBeBlock;//script用到 (playerBlockJudgement，playercontroller，dragonFlash)
    public float ImpulsePowerX;//script用到 (playercontroller)
    public float ImpulsePowerY;//script用到 (playercontroller)
    public float AtkTimer;
    public int Damage;//script用到 (playercontroller)
    [HideInInspector] public bool isMoveAtk;
    [HideInInspector] public GameObject Parent;//有被其他script用到(各種攻擊follow)

    private void Awake()
    {
        //抓取父親的父親
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
        }//自動開啟 省去麻煩
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
