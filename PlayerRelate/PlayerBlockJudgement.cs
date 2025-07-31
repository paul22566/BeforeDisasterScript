using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlockJudgement : MonoBehaviour
{
    private float BlockTimer;
    private float BlockTimerSet = 0.2f;
    [HideInInspector] public bool isRBlock;
    [HideInInspector] public bool isLBlock;
    private BattleSystem _battleSystem;
    public Action BlockSuccess;

    private void Awake()
    {
        if (GameObject.Find("player") != null)
        {
            _battleSystem = GameObject.Find("player").GetComponent<BattleSystem>();
            _battleSystem.isBlockActualAppear = true;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        BlockTimer = BlockTimerSet;
    }

    // Update is called once per frame
    void Update()
    {
        BlockTimer -= Time.deltaTime;
        if (BlockTimer <= 0 || PlayerController.isHurted)
        {
            _battleSystem.isBlockActualAppear = false;
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "monsterAtk")
        {
            if (isRBlock)
            {
                if(collision.transform.position.x >= this.transform.position.x)
                {
                    if (collision.GetComponent<NormalMonsterAtk>().CanBeBlock)
                    {
                        BlockSuccess?.Invoke();
                        Destroy(this.gameObject);
                    }
                }
            }
            if (isLBlock)
            {
                if (collision.transform.position.x <= this.transform.position.x)
                {
                    if (collision.GetComponent<NormalMonsterAtk>().CanBeBlock)
                    {
                        BlockSuccess?.Invoke();
                        Destroy(this.gameObject);
                    }
                }
            }
        }
    }
}
