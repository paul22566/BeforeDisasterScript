using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgainComeInPlace : MonoBehaviour
{
    [HideInInspector] public bool AgainGoIn;//script(Boss3Controller)
    // Start is called before the first frame update
    void Start()
    {
        if (!GameEvent.GoInBoss3)
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            AgainGoIn = true;
            Destroy(this.gameObject);
        }
    }
}
