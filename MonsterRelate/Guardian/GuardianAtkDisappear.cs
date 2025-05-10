using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianAtkDisappear : MonoBehaviour
{
    private MonsterHurtedController _hurtedController;

    private void Start()
    {
        _hurtedController = GameObject.Find("ªùÆU¦u½Ã").GetComponent<MonsterHurtedController>();
    }
    void Update()
    {
        if (_hurtedController == null || _hurtedController.isCriticAtkHurted)
        {
            Destroy(this.gameObject);
        }
    }
}
