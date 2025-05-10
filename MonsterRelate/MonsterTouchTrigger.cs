using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTouchTrigger : MonoBehaviour
{
    //怪物互相碰撞觸發器
    //怪物Controller須提供接觸，改狀態，離開的方法

    public delegate void MonsterTouch();

    public event MonsterTouch OnTouch;
    public event MonsterTouch StatusChange;
    public event MonsterTouch OnLeave;

    private MonsterBasicData _basicData;
    private MonsterBasicData _touchMonsterData;
    [HideInInspector] public bool isMonsterInRange;
    private bool isMonsterOrderChange;
    private int MonsterOrderRecord;
    [HideInInspector] public MonsterBasicData.MonsterType _type;

    // Start is called before the first frame update
    void Start()
    {
        _basicData = this.GetComponent<MonsterBasicData>();
        MonsterOrderRecord = _basicData.Order;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //暫時變得可穿透
        if (collision.gameObject.tag == "monster" && collision.gameObject.GetComponent<MonsterBasicData>() != null)
        {
            _touchMonsterData = collision.gameObject.GetComponent<MonsterBasicData>();
            if (_touchMonsterData.EntityMonster)
            {
                _type = _touchMonsterData.Type;
                if (OnTouch != null)
                {
                    OnTouch();
                }
                if (_basicData.Order > _touchMonsterData.Order && !_touchMonsterData.ShouldIgnore)
                {
                    if (!isMonsterOrderChange)
                    {
                        MonsterOrderRecord = _touchMonsterData.Order;
                        _touchMonsterData.Order = _basicData.Order;
                        isMonsterOrderChange = true;
                    }
                    isMonsterInRange = true;
                    if (StatusChange != null)
                    {
                        StatusChange();
                    }
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "monster")
        {
            isMonsterInRange = false;
            if (isMonsterOrderChange)
            {
                isMonsterOrderChange = false;
                _basicData.Order = MonsterOrderRecord;
            }
            _touchMonsterData = null;
            if (OnLeave != null)
            {
                OnLeave();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //暫時變得可穿透
        if (collision.gameObject.tag == "monster" && collision.gameObject.GetComponent<MonsterBasicData>() != null)
        {
            _touchMonsterData = collision.gameObject.GetComponent<MonsterBasicData>();
            if (_touchMonsterData.EntityMonster)
            {
                _type = _touchMonsterData.Type;
                if (OnTouch != null)
                {
                    OnTouch();
                }
                if (_basicData.Order > _touchMonsterData.Order && !_touchMonsterData.ShouldIgnore)
                {
                    if (!isMonsterOrderChange)
                    {
                        MonsterOrderRecord = _touchMonsterData.Order;
                        _touchMonsterData.Order = _basicData.Order;
                        isMonsterOrderChange = true;
                    }
                    isMonsterInRange = true;
                    if(StatusChange != null)
                    {
                        StatusChange();
                    }
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "monster")
        {
            isMonsterInRange = false;
            if (isMonsterOrderChange)
            {
                isMonsterOrderChange = false;
                _basicData.Order = MonsterOrderRecord;
            }
            _touchMonsterData = null;
            if (OnLeave != null)
            {
                OnLeave();
            }
        }
    }
}
