using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonFlash : MonoBehaviour
{
    private GameObject Parent;
    public float Timer;
    public float MoveTimer;

    private void Awake()
    {
        Parent = this.transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Parent.transform.position;
        if (!Parent.GetComponent<DragonFlashAni>().isMove)
        {
            Timer -= Time.deltaTime;
            if (Timer <= 0)
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            this.GetComponent<NormalMonsterAtk>().CanBeBlock = false;
            MoveTimer -= Time.deltaTime;
            if (MoveTimer <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
