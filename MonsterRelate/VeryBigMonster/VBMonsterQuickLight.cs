using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VBMonsterQuickLight : MonoBehaviour
{
    private MonsterBasicData _basicData;
    private float DecreaseAngle = -17.5f;
    private Transform Head;
    private Transform _transform;
    private float HeadAngle;
    private float LightAngle;
    public float Speed;
    private Vector2 LightSpeedRate = new Vector2();
    private float _fixedDeltaTime;
    private float LightProportion = 0.112f;

    private float DontDestroyTime = 0.1f;
    private bool CanDestroy;

    private bool isEnd;
    private bool isRun;

    private void Awake()
    {
        _transform = this.transform;
        Head = _transform.parent.parent.GetComponent<Transform>();
        _basicData = Head.parent.GetComponent<MonsterBasicData>(); ;
    }
    // Start is called before the first frame update
    void Start()
    {
        _transform.localScale = new Vector3(0, _transform.localScale.y, 0);
        HeadAngle = Head.eulerAngles.z;

        //計算Light角度
        switch (_basicData.face)
        {
            case MonsterBasicData.Face.Right:
                LightAngle = AngleCaculate.AngleConvert("R", "L", HeadAngle, DecreaseAngle);
                break;
            case MonsterBasicData.Face.Left:
                LightAngle = AngleCaculate.AngleConvert("L", "L", HeadAngle, DecreaseAngle);
                break;
        }
        _transform.rotation = Quaternion.Euler(0, 0, LightAngle);
        //計算速度比例
        LightSpeedRate = AngleCaculate.CaculateSpeedRate("L", LightAngle);
    }

    private void FixedUpdate()
    {
        _fixedDeltaTime = Time.fixedDeltaTime;

        if (!CanDestroy)
        {
            DontDestroyTime -= _fixedDeltaTime;
            if (DontDestroyTime <= 0)
            {
                CanDestroy = true;
            }
        }

        if (!isEnd)
        {
            if (_transform.localScale.x < 0.22)
            {
                _transform.localScale = new Vector3(_transform.localScale.x + Speed * LightProportion * _fixedDeltaTime, 0.07f, 0);
            }

            if (_transform.localScale.x >= 0.22)
            {
                isRun = true;
                _transform.localScale = new Vector3(0.22f, 0.07f, 0);
            }
            
            if (!isRun)
            {
                return;
            }
        }
        
        if (isEnd)
        {
            if (_transform.localScale.x > 0)
            {
                _transform.localScale = new Vector3(_transform.localScale.x - Speed * LightProportion * _fixedDeltaTime, _transform.localScale.y, 0);
            }
            if (_transform.localScale.x <= 0)
            {
                Destroy(this.gameObject);
            }
        }
        
        _transform.localPosition = new Vector3(_transform.localPosition.x + Speed * LightSpeedRate.x * _fixedDeltaTime, _transform.localPosition.y + Speed * LightSpeedRate.y * _fixedDeltaTime, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "platform" && CanDestroy)
        {
            isEnd = true;
        }
        if (collision.gameObject.tag == "SpecialPlatform" && CanDestroy)
        {
            isEnd = true;
        }
        if (collision.gameObject.tag == "LeftWall" && CanDestroy)
        {
            isEnd = true;
        }
        if (collision.gameObject.tag == "RightWall" && CanDestroy)
        {
            isEnd = true;
        }
    }
}
