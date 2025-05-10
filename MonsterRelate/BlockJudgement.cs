using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockJudgement : MonoBehaviour
{
    [HideInInspector] public bool CanBlock;//script用到 (playerBlockJudgement)
    public bool isRBlock;
    public bool isLBlock;
    private bool isPlayerAtLeftSide;
    private bool isPlayerAtRightSide;
    private Transform PlayerTransform;
    private GameObject Parent;
    private Transform target;
    private float Timer;
    public float TimerSet;
    private float _deltaTime;
    private MonsterHurtedController _hurtedController;
    // Start is called before the first frame update
    private void Awake()
    {
        Timer = TimerSet;
        Parent = this.transform.parent.gameObject.transform.parent.gameObject;
    }
    void Start()
    {
        if(GameObject.Find("player") != null)
        {
            PlayerTransform = GameObject.Find("player").transform;
        }
        target = Parent.transform;
        if (Parent.GetComponent<MonsterHurtedController>() != null)
        {
            _hurtedController = Parent.GetComponent<MonsterHurtedController>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        _deltaTime = Time.deltaTime;
        //判斷玩家方向and是否能格檔人
        if (PlayerTransform)
        {
            if ((this.transform.position.x - PlayerTransform.position.x) >= 0)
            {
                isPlayerAtLeftSide = true;
                isPlayerAtRightSide = false;
            }
            else
            {
                isPlayerAtLeftSide = false;
                isPlayerAtRightSide = true;
            }
            if (isRBlock)
            {
                if (isPlayerAtRightSide)
                {
                    CanBlock = true;
                }
                else
                {
                    CanBlock = false;
                }
            }
            if (isLBlock)
            {
                if (isPlayerAtLeftSide)
                {
                    CanBlock = true;
                }
                else
                {
                    CanBlock = false;
                }
            }
        }

        Timer -= _deltaTime;
        if (Timer <= 0)
        {
            Destroy(this.gameObject);
        }
        if (_hurtedController.isHurted)
        {
            Destroy(this.gameObject);
        }
        if (target != null)
        {
            Vector3 followPos = new Vector3(target.position.x, target.position.y, this.transform.position.z);
            this.transform.position = followPos;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "normalAtk")
        {
            if (Parent.GetComponent<MonsterBlockController>().isBlock && CanBlock && collision.GetComponent<PlayerAtkController>().CanBeBlock)
            {
                Parent.GetComponent<MonsterBlockController>().isBlockSuucess = true;
                Destroy(this.gameObject);
            }
        }
    }
}
