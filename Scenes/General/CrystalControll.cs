using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class CrystalControll : MonoBehaviour
{
    private SpriteRenderer CrystalSprite;
    private GameObject CrystalBreakAni;
    [HideInInspector] public bool CrystalBroken;
    public GameObject AbsorbSound;
    public GameObject BrokenSound;

    public ObjectShield _objectShield;

    private float _deltaTime;
    private enum Status {Wait, Increase, Decrease, Broken };
    private Status status;
    private float IncreaseChangeSpeed = 1;
    private float DecreaseChangeSpeed = 0.1f;
    private float TransparentSpeed = 10;
    private Color TargetColor = new Color();
    private Color NowColor = new Color();
    private float BrokenColor = 0.25f;//必較g即可
    private float BeAtkIncreaseNumber = 0.253f;//1 -> 0.24
    private bool isCooldown;
    private float CooldownTime;
    private float CooldownTimeSet = 1f;

    private bool CanStart;
    private float LagTimerSet = 0.03f;
    private float LagTimer;

    // Start is called before the first frame update
    void Start()
    {
        status = Status.Wait;
        CrystalSprite = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
        CrystalBreakAni = this.transform.GetChild(1).gameObject;

        if (CrystalBroken)
        {
            status = Status.Broken;
            CrystalSprite.gameObject.SetActive(false);
            CrystalBreakAni.SetActive(false);
            return;
        }

        TargetColor = CrystalSprite.color;
        NowColor = CrystalSprite.color;

        LagTimer = LagTimerSet;
    }

    // Update is called once per frame
    void Update()
    {
        _deltaTime = Time.deltaTime;

        if (CrystalBroken)
        {
            return;
        }

        //break運作
        if (status == Status.Broken)
        {
            NowColor = new Color(NowColor.r, NowColor.g, NowColor.b, NowColor.a - TransparentSpeed * _deltaTime);
            CrystalSprite.color = NowColor;
            if (NowColor.a <= 0)
            {
                CrystalSprite.gameObject.SetActive(false);
                CrystalBroken = true;
            }
            return;
        }

        //爆裂瓶判斷
        if (CanStart)
        {
            LagTimer -= _deltaTime;

            if (LagTimer <= 0 && !_objectShield.ProtectSuccess)
            {
                switch (status)
                {
                    case Status.Wait:
                        TargetColor = new Color(TargetColor.r, TargetColor.g - 3 * BeAtkIncreaseNumber, TargetColor.b - 3 * BeAtkIncreaseNumber);
                        break;
                    case Status.Increase:
                        TargetColor = new Color(TargetColor.r, TargetColor.g - 3 * BeAtkIncreaseNumber, TargetColor.b - 3 * BeAtkIncreaseNumber);
                        break;
                    case Status.Decrease:
                        TargetColor = new Color(NowColor.r, NowColor.g - 3 * BeAtkIncreaseNumber, NowColor.b - 3 * BeAtkIncreaseNumber);
                        break;
                }
                status = Status.Increase;
                Instantiate(AbsorbSound);
                CanStart = false;
                LagTimer = LagTimerSet;
            }
        }

        //color變化
        if (TargetColor != NowColor)
        {
            switch (status)
            {
                case Status.Increase:
                    NowColor = new Color(NowColor.r, NowColor.g - IncreaseChangeSpeed * _deltaTime, NowColor.b - IncreaseChangeSpeed * _deltaTime);
                    if(NowColor.g < TargetColor.g)
                    {
                        NowColor = TargetColor;
                        status = Status.Wait;
                        isCooldown = true;
                        CooldownTime = CooldownTimeSet;
                    }
                    break;
                case Status.Decrease:
                    NowColor = new Color(NowColor.r, NowColor.g + DecreaseChangeSpeed * _deltaTime, NowColor.b + DecreaseChangeSpeed * _deltaTime);
                    if (NowColor.g > TargetColor.g)
                    {
                        NowColor = TargetColor;
                        status = Status.Wait;
                    }
                    break;
            }
        }

        CrystalSprite.color = NowColor;

        //顏色變化停滯時間
        if (isCooldown)
        {
            CooldownTime -= _deltaTime;

            if(CooldownTime <= 0)
            {
                isCooldown = false;
                TargetColor = new Color(1, 1, 1);
                status = Status.Decrease;
            }
        }

        //broken界線
        if(NowColor.g <= BrokenColor)
        {
            Instantiate(BrokenSound);
            CrystalBreakAni.SetActive(true);
            status = Status.Broken;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "bullet" && status !=Status.Broken)
        {
            switch (status)
            {
                case Status.Wait:
                    TargetColor = new Color(TargetColor.r, TargetColor.g - BeAtkIncreaseNumber, TargetColor.b - BeAtkIncreaseNumber);
                    break;
                case Status.Increase:
                    TargetColor = new Color(TargetColor.r, TargetColor.g - BeAtkIncreaseNumber, TargetColor.b - BeAtkIncreaseNumber);
                    break;
                case Status.Decrease:
                    TargetColor = new Color(NowColor.r, NowColor.g - BeAtkIncreaseNumber, NowColor.b - BeAtkIncreaseNumber);
                    break;
            }
            Destroy(collision.gameObject);
            status = Status.Increase;
            Instantiate(AbsorbSound);
        }
        if (collision.tag == "ExplosionBottle" && status != Status.Broken)
        {
            if (_objectShield != null)
            {
                CanStart = true;
            }
            else
            {
                switch (status)
                {
                    case Status.Wait:
                        TargetColor = new Color(TargetColor.r, TargetColor.g - 3 * BeAtkIncreaseNumber, TargetColor.b - 3 * BeAtkIncreaseNumber);
                        break;
                    case Status.Increase:
                        TargetColor = new Color(TargetColor.r, TargetColor.g - 3 * BeAtkIncreaseNumber, TargetColor.b - 3 * BeAtkIncreaseNumber);
                        break;
                    case Status.Decrease:
                        TargetColor = new Color(NowColor.r, NowColor.g - 3 * BeAtkIncreaseNumber, NowColor.b - 3 * BeAtkIncreaseNumber);
                        break;
                }
                status = Status.Increase;
                Instantiate(AbsorbSound);
            }
        }
    }
}
