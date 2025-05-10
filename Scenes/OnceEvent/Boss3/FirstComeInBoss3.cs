using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstComeInBoss3 : MonoBehaviour
{
    [HideInInspector] public bool FirstGoIn;//script(Boss3Controller)
    // Start is called before the first frame update
    void Start()
    {
        if (GameEvent.GoInBoss3)
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameEvent.GoInBoss3)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            FirstGoIn = true;
        }
    }
}
