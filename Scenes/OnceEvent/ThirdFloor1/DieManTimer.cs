using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieManTimer : MonoBehaviour
{
    private float Timer = 3.05f;
    // Start is called before the first frame update
    void Start()
    {
        if (GameEvent.GoIn3F1)
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Timer -= Time.deltaTime;
        if (Timer <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
