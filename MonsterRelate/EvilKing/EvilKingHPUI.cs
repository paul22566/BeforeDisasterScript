using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilKingHPUI : MonoBehaviour
{
    public GameObject Boss3;
    void Update()
    {
        if (Boss3 == null)
        {
            Destroy(this.gameObject);
        }
    }
}
