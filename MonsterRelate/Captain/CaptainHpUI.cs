using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptainHpUI : MonoBehaviour
{
    public GameObject Boss2;
    void Start()
    {
        if (GameEvent.PassBoss2)
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Boss2 == null)
        {
            Destroy(this.gameObject);
        }
    }
}
