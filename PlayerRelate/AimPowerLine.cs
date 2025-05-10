using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimPowerLine : MonoBehaviour
{
    public enum Face { Right, Left}
    public Face face;
    private Transform _transform;
    public GameObject AimPower;
    private Transform AimPowerTransform;
    private float DistanceX;
    private float DistanceY;
    private float NowLength;
    private float LengthConvert;
    private float Angel;
    private BattleSystem _battleSystem;
    // Start is called before the first frame update
    void Start()
    {
        AimPowerTransform = AimPower.transform;
        _transform = this.transform;
        DistanceX = Mathf.Abs(_transform.position.x - AimPowerTransform.position.x);
        DistanceY = Mathf.Abs(_transform.position.y - AimPowerTransform.position.y);
        NowLength = Mathf.Pow(DistanceX * DistanceX + DistanceY * DistanceY, 0.5f);
        LengthConvert = 0.05f / NowLength;
        if (GameObject.Find("player") != null)
        {
            _battleSystem = GameObject.Find("player").GetComponent<BattleSystem>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        DistanceX = Mathf.Abs(_transform.localPosition.x - AimPowerTransform.localPosition.x);
        DistanceY = Mathf.Abs(_transform.localPosition.y - AimPowerTransform.localPosition.y);
        //決定長度
        NowLength = Mathf.Pow(DistanceX * DistanceX + DistanceY * DistanceY, 0.5f);
        _transform.localScale = new Vector3(NowLength * LengthConvert, 0.15f, 0);
        //決定角度
        Angel = Mathf.Atan2(DistanceY, DistanceX) * 180 / Mathf.PI;
        switch (face)
        {
            case Face.Right:
                _transform.rotation = Quaternion.Euler(0, 0, Angel);
                break;
            case Face.Left:
                _transform.rotation = Quaternion.Euler(0, 0, 180 - Angel);
                break;
        }

        if (!_battleSystem.isAim || PlayerController.isHurted || PlayerController.isDie)
        {
            this.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            _transform.localScale = new Vector3(0.109f, 0.15f, 0);
            switch (face)
            {
                case Face.Right:
                    _transform.rotation = Quaternion.Euler(0, 0, 25.939f);
                    break;
                case Face.Left:
                    _transform.rotation = Quaternion.Euler(0, 0, 180 - 25.939f);
                    break;
            }
            _battleSystem.HasAimAppear = false;
            this.gameObject.SetActive(false);
        }
    }
}
