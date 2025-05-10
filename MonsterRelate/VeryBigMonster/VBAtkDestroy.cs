using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VBAtkDestroy : MonoBehaviour
{
    private MonsterHurtedController _hurtedController;
    private VeryBigMonsterController _controller;

    private void Start()
    {
        _controller = GameObject.Find("Boss1").GetComponent<VeryBigMonsterController>();
        _hurtedController = GameObject.Find("Boss1").GetComponent<MonsterHurtedController>();
    }
    void Update()
    {
        if (_controller == null || _hurtedController.isCriticAtkHurted || _controller.status == VeryBigMonsterController.Status.Weak)
        {
            Destroy(this.gameObject);
        }
    }
}
