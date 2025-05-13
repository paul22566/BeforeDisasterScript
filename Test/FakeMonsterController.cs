using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeMonsterController : MonoBehaviour
{
    //受傷部分
    private SpriteRenderer _spr;
    private MonsterHurtedController _hurtedController;
    // Start is called before the first frame update
    void Start()
    {
        //受傷部分
        _spr = GetComponent<SpriteRenderer>();
        _hurtedController = GetComponent<MonsterHurtedController>();
    }

    // Update is called once per frame
    void Update()
    {
        //受傷部分
        _hurtedController.HurtedTimerMethod(Time.deltaTime);
        if (_hurtedController.isHurted)
        {
            _spr.color = new Color(0.65f, 0.48f, 0.48f, 1);
        }
        else
        {
            _spr.color = new Color(1, 1, 1, 1);
        }
    }
}
