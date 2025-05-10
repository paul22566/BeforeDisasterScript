using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonFlashAni : MonoBehaviour
{
    public float Speed;
    public float MoveDistance;
    [HideInInspector] public bool isMove = false;//script(DragonFlash)
    private bool TimerSwitch;
    public float Timer;
    public float MoveTimer;
    private bool MoveTimerSwitch;
    private Transform Player;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("player").transform;
        if(Mathf.Abs(Player.position.x - this.transform.position.x) > MoveDistance)
        {
            isMove = true;
        }
        this.transform.DetachChildren();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMove)
        {
            if(this.GetComponent<SpriteRenderer>().flipX == true)
            {
                this.transform.position += new Vector3(-Speed * Time.deltaTime, 0, 0);
            }
            else
            {
                this.transform.position += new Vector3(Speed * Time.deltaTime, 0, 0);
            }
            MoveTimerSwitch = true;
        }
        else
        {
            TimerSwitch = true;
        }
        TimerMethod();
    }

    void TimerMethod()
    {
        if (MoveTimerSwitch)
        {
            MoveTimer -= Time.deltaTime;
            if (MoveTimer <= 0)
            {
                Destroy(this.gameObject);
            }
        }
        if (TimerSwitch)
        {
            Timer -= Time.deltaTime;
            if (Timer <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
