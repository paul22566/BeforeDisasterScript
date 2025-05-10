using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdFloor2CameraFollow : MonoBehaviour
{
    public Transform SpecialTarget;
    private Transform target;
    Transform _transform;
    private float Timer = 7.3f;
    private bool CameraFirstMove = false;
    private bool CameraSecondMove = false;
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("player") != null)
        {
            target = GameObject.Find("player").transform;
        }
        _transform = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameEvent.HasGoThirdFloor2)
        {
            Timer -= Time.deltaTime;
            if (target != null)
            {
                if (!CameraFirstMove)
                {
                    _transform.position = new Vector3(target.position.x, target.position.y, _transform.position.z);
                }
            }
            if (Timer <= (7.3 - 1.5))
            {
                if (!CameraFirstMove)
                {
                    _transform.position = new Vector3(SpecialTarget.position.x, SpecialTarget.position.y, _transform.position.z);
                    CameraFirstMove = true;
                }
                if (Timer <= 0)
                {
                    if (!CameraSecondMove)
                    {
                        _transform.position = new Vector3(target.position.x, target.position.y, _transform.position.z);
                        CameraSecondMove = true;
                    }
                }
            }
        }
        else
        {
            _transform.position = new Vector3(target.position.x, target.position.y, _transform.position.z);
        }
    }
}
