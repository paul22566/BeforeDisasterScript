using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdditionalHurtedTrigger : MonoBehaviour
{
    public MonsterHurtedController _hurtedController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerAtkController>() != null && collision.GetComponent<PlayerAtkController>().CanHurt && !_hurtedController.isHurted)
        {
            _hurtedController.DetectHurtedType(collision.gameObject);
        }
    }
}
