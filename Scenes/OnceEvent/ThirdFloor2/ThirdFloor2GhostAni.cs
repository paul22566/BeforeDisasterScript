using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdFloor2GhostAni : MonoBehaviour
{
    private float Timer = 2.35f;
    // Start is called before the first frame update
    void Start()
    {
        if (GameEvent.HasGoThirdFloor2)
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameEvent.HasGoThirdFloor2)
        {
            Timer -= Time.deltaTime;
            if (Timer <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
