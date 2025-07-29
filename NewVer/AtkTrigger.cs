using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkTrigger : MonoBehaviour
{
    public event Action<IHurtedObject> OnMakeDamage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision != null)
        {
            var hurted = collision.GetComponent<IHurtedObject>();
            if (hurted != null)
            {
                OnMakeDamage?.Invoke(hurted);
            }
        }
    }
}
