using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigGunController : MonoBehaviour
{
    public bool FaceRight;
    public bool FaceLeft;
    private Transform _transform;
    private Transform TemporaryArea;
    private BattleSystem _battleSystem;
    public float TimerSet;
    private float Timer;

    public GameObject AccumulateLight;
    private Transform AccumulateLightRecord;
    public GameObject LightAtk;
    private Transform LightAtkRecord;
    private GameObject LightAtkCollider;
    private Animator LightAtkAni;

    private float LightAtkApearPointX = 0.35f;
    private float LightAtkApearPointY = 0.12f;

    private bool AccumulateLightAppear;
    private bool LightAtkAppear;

    // Start is called before the first frame update
    void Start()
    {
        _transform = this.transform;
        TemporaryArea = _transform.GetChild(1);
        _battleSystem = GameObject.Find("player").GetComponent<BattleSystem>();
        Timer = TimerSet;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_battleSystem.isBigGunProcess || _battleSystem.isBigGunEnd)
        {
            if (LightAtkRecord != null)
            {
                Destroy(LightAtkRecord.gameObject);
            }
            if (AccumulateLightRecord != null)
            {
                Destroy(AccumulateLightRecord.gameObject);
            }
            Destroy(this.gameObject);
            return;
        }
        Timer -= Time.deltaTime;
        _transform.localPosition = _battleSystem.BigGunAppear.position;
        if (Timer <= (TimerSet - 0.2))
        {
            if (!AccumulateLightAppear)
            {
                Instantiate(AccumulateLight, _transform.position, Quaternion.identity, TemporaryArea);
                AccumulateLightRecord = TemporaryArea.GetChild(0);
                TemporaryArea.DetachChildren();
                AccumulateLightAppear = true;
            }
            if (AccumulateLightRecord != null)
            {
                AccumulateLightRecord.transform.localPosition = _transform.position;
            }
        }
        if (Timer <= (TimerSet - 1.7))
        {
            if (AccumulateLightRecord != null)
            {
                Destroy(AccumulateLightRecord.gameObject);
            }
        }
        if (Timer <= (TimerSet - 2))
        {
            if (!LightAtkAppear)
            {
                if (FaceRight)
                {
                    Instantiate(LightAtk, new Vector3(_transform.position.x + LightAtkApearPointX, _transform.position.y + LightAtkApearPointY, 0), Quaternion.identity, TemporaryArea);
                }
                if (FaceLeft)
                {
                    Instantiate(LightAtk, new Vector3(_transform.position.x - LightAtkApearPointX, _transform.position.y + LightAtkApearPointY, 0), Quaternion.identity, TemporaryArea);
                }
                LightAtkRecord = TemporaryArea.GetChild(0);
                TemporaryArea.DetachChildren();
                LightAtkCollider = LightAtkRecord.GetChild(0).gameObject;
                LightAtkAni = LightAtkRecord.GetComponent<Animator>();
                LightAtkAppear = true;
            }
            if (LightAtkRecord != null)
            {
                if (FaceRight)
                {
                    LightAtkRecord.position = new Vector3(_transform.position.x + LightAtkApearPointX, LightAtkRecord.position.y, 0);
                }
                if (FaceLeft)
                {
                    LightAtkRecord.position = new Vector3(_transform.position.x - LightAtkApearPointX, LightAtkRecord.position.y, 0);
                }
            }
        }
        if (Timer <= (TimerSet - 4))
        {
            LightAtkCollider.SetActive(false);
            LightAtkAni.SetBool("End", true);
            if (Timer <= 0)
            {
                if (LightAtkRecord != null)
                {
                    Destroy(LightAtkRecord.gameObject);
                }
                Destroy(this.gameObject);
                return;
            }
        }
    }
}
