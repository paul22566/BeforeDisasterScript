using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneBullet : MonoBehaviour
{
    public float Speed;
    private Transform _transform;
    private Transform DroneTransform;
    private float Angle;
    private float AngleProportion;
    private float XProportion;
    private float YProportion;
    private float CaseNumber;//¨Ì¶H­­ 5 90«× 6 270«×
    private float _fixedDeltaTime;

    private void Awake()
    {
        _transform = transform;
        DroneTransform = _transform.parent;

        _transform.rotation = Quaternion.Euler(0, 0, DroneTransform.eulerAngles.z);
    }
    // Start is called before the first frame update
    void Start()
    {
        if (_transform.eulerAngles.z < 90)
        {
            Angle = _transform.eulerAngles.z;
            CaseNumber = 1;
        }
        if (_transform.eulerAngles.z > 90 && _transform.eulerAngles.z < 180)
        {
            Angle = 180 - _transform.eulerAngles.z;
            CaseNumber = 2;
        }
        if (_transform.eulerAngles.z >= 180 && _transform.eulerAngles.z < 270)
        {
            Angle = _transform.eulerAngles.z - 180;
            CaseNumber = 3;
        }
        if (_transform.eulerAngles.z > 270 && _transform.eulerAngles.z < 360)
        {
            Angle = 360 - _transform.eulerAngles.z;
            CaseNumber = 4;
        }
        if (_transform.eulerAngles.z == 90)
        {
            CaseNumber = 5;
        }
        if (_transform.eulerAngles.z == 270)
        {
            CaseNumber = 6;
        }
        AngleProportion = Mathf.Tan(Angle * Mathf.PI / 180);
        AngleProportion = Mathf.Abs(AngleProportion);
        XProportion = 1 / (AngleProportion * AngleProportion + 1);
        XProportion = Mathf.Pow(XProportion, 0.5f);
        YProportion = XProportion * AngleProportion;
    }

    private void FixedUpdate()
    {
        _fixedDeltaTime = Time.fixedDeltaTime;
        switch (CaseNumber)
        {
            case 1:
                _transform.localPosition += new Vector3(Speed * XProportion * _fixedDeltaTime, Speed * YProportion * _fixedDeltaTime, 0);
                break;
            case 2:
                _transform.localPosition += new Vector3(-Speed * XProportion * _fixedDeltaTime, Speed * YProportion * _fixedDeltaTime, 0);
                break;
            case 3:
                _transform.localPosition += new Vector3(-Speed * XProportion * _fixedDeltaTime, -Speed * YProportion * _fixedDeltaTime, 0);
                break;
            case 4:
                _transform.localPosition += new Vector3(Speed * XProportion * _fixedDeltaTime, -Speed * YProportion * _fixedDeltaTime, 0);
                break;
            case 5:
                _transform.localPosition += new Vector3(0, Speed * _fixedDeltaTime, 0);
                break;
            case 6:
                _transform.localPosition += new Vector3(0, -Speed * _fixedDeltaTime, 0);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "platform")
        {
            Destroy(this.gameObject);
        }
        if (collision.gameObject.tag == "LeftWall")
        {
            Destroy(this.gameObject);
        }
        if (collision.gameObject.tag == "RightWall")
        {
            Destroy(this.gameObject);
        }
        /*if (collision.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
        }*/
        if (collision.gameObject.tag == "monster" && collision.GetComponent<DroneController>() == null)
        {
            Destroy(this.gameObject);
        }
    }
}
