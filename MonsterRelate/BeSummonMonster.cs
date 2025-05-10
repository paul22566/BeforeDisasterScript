using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeSummonMonster : MonoBehaviour
{
    //此腳本用在被召喚怪物的出場動畫
    public GameObject Monster;
    private float Timer;
    public float TimerSet;
    [HideInInspector] public GameObject _parent;
    [HideInInspector] public SpriteRenderer _spr;

    public bool DontNeedGetParent;
    private void Awake()
    {
        if (!DontNeedGetParent)
        {
            _parent = this.transform.parent.parent.gameObject;
        }
    }

    private void Start()
    {
        _spr = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
        Timer = TimerSet;
    }

    void Update()
    {
        Timer -= Time.deltaTime;
        if (Timer <= 0)
        {
            Instantiate(Monster, this.transform.position, Monster.transform.rotation, this.transform);
            this.transform.DetachChildren();
            Destroy(this.gameObject);
        }
    }
}
