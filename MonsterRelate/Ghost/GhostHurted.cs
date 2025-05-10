using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostHurted : MonoBehaviour
{
    private SpriteRenderer thisSpr;
    private MonsterHurtedController _hurtedController;

    void Start()
    {
        thisSpr = this.GetComponent<SpriteRenderer>();
        _hurtedController = this.GetComponent<MonsterHurtedController>();
    }

    void Update()
    {
        if (_hurtedController.isHurted)
        {
            thisSpr.color = new Color(0.39f, 0.39f, 0.39f, 1);
        }
        else
        {
            thisSpr.color = new Color(1, 1, 1, 1);
        }
    }
}
