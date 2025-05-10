using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LFireBall : MonoBehaviour
{
    private Transform _transform;
    private Transform PlayerTransform;
    public float AtkTimerSet;
    private float AtkTimer;
    public float Speed;
    private float DistanceX;
    private float DistanceY;
    private float Sin;
    private float FinalSin;
    private float FinalCos;
    private bool Aim = true;
    private bool isRecord = false;
    // Start is called before the first frame update
    void Start()
    {
        _transform = transform;
        if (GameObject.Find("player"))
        {
            PlayerTransform = GameObject.Find("player").transform;
        }

        AtkTimer = AtkTimerSet;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerTransform)
        {
            DistanceX = Mathf.Abs(_transform.position.x - PlayerTransform.position.x);
            DistanceY = Mathf.Abs(_transform.position.y - (PlayerTransform.position.y + 0.682f));
            Sin = DistanceY / DistanceX;
            Sin = Mathf.Asin(Sin);
            Sin = Sin / Mathf.PI * 180;
        }
        timer();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "LeftWall")
        {
            Destroy(this.gameObject);
        }
        if (collision.gameObject.tag == "RightWall")
        {
            Destroy(this.gameObject);
        }
        if (collision.gameObject.tag == "platform")
        {
            Destroy(this.gameObject);
        }
    }

    void timer()
    {
        AtkTimer -= Time.deltaTime;
        if (Aim)
        {
            if (_transform.position.y <= PlayerTransform.position.y)
            {
                if (!(Sin < 89)|| PlayerTransform.position.x > _transform.position.x)
                {
                    Sin = 70;
                }
                _transform.rotation = Quaternion.Euler(0, 0, -Sin);
            }
            else
            {
                if (!(Sin < 89)|| PlayerTransform.position.x > _transform.position.x)
                {
                    Sin = 70;
                }
                if(PlayerTransform.position.x > _transform.position.x)
                {
                    _transform.rotation = Quaternion.Euler(0, 0, -Sin);
                }
                else
                {
                    _transform.rotation = Quaternion.Euler(0, 0, Sin);
                }
            }
        }
        if (AtkTimer <= (AtkTimerSet - 0.7))
        {
            if (!isRecord)
            {
                FinalSin = Sin;
                FinalSin = Mathf.PI * FinalSin / 180;
                FinalSin = Mathf.Sin(FinalSin);
                FinalCos = Mathf.Pow(1 - (FinalSin * FinalSin), 0.5f);
                isRecord = true;
            }
            Aim = false;
            if (AtkTimer <= (AtkTimerSet - 0.8))
            {
                _transform.position += new Vector3(-Speed * FinalCos * Time.deltaTime, Speed * FinalSin * Time.deltaTime, 0);
                if (AtkTimer <= 0)
                {
                    Destroy(this.gameObject);
                }
            }
        }
    }
}
