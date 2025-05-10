using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterCaptureController : MonoBehaviour
{
    /*
    主腳本需修改項目:

    宣告變數並抓取此腳本、觸發方式 、動畫 、Status(決定方向、結束時動作、isCaptureSuccess 放這裡) 、被中斷

    Capture成功時需把原動作變數關閉(此動作寫在抓捕Status)

    TimerMethod需放在FixedUpdate
    */
    private Transform _transform;

    public string MonsterType;
    [SerializeField] private GameObject SuccessObject;
    [SerializeField] private Transform PlayerPoint;
    private Transform playerTransform;
    private PlayerController _playerController;
    [SerializeField] private float CaptureAtkSuccessTimerSet;
    private float CaptureAtkSuccessTimer;
    [HideInInspector] public bool isCaptureSuccess;
    [HideInInspector] public bool isPlayerFollow;
    private bool AtkFirstAppear;
    private bool AtkSecondAppear;
    private bool AtkThirdAppear;
    private bool SuccessObjectAppear;
    private bool CanReSet;
    [HideInInspector] public bool FaceRight;
    [HideInInspector] public bool FaceLeft;
    [HideInInspector] public bool CaptureAtkEnd;
    [SerializeField] private float EndAppearX;
    [SerializeField] private float EndAppearY;
    [SerializeField] private float SuccessObjectAppearTime;

    [SerializeField] private float HurtNumber;
    [SerializeField] private float CaptureAtkAppearTime1;
    [SerializeField] private float CaptureAtkAppearTime2;
    [SerializeField] private float CaptureAtkAppearTime3;
    [SerializeField] private float Damage1;
    [SerializeField] private float Damage2;
    [SerializeField] private float Damage3;

    [Header("持續性傷害")]
    [SerializeField] private int Order;
    [SerializeField] private int SuccessiveHurtTotalNumber;
    private int SuccessiveHurtNumber = 0;

    // Start is called before the first frame update
    void Start()
    {
        _transform = transform;
        if(GameObject.Find("player") != null)
        {
            playerTransform = GameObject.Find("player").transform;
            _playerController = playerTransform.GetComponent<PlayerController>();
        }
        CaptureAtkSuccessTimer = CaptureAtkSuccessTimerSet;
    }

    private void Update()
    {
        if (isPlayerFollow)
        {
            playerTransform.position = PlayerPoint.position;
        }
    }

    public void CaptureSuccessTimerMethod(float _fixedDeltaTime)
    {
        if (isCaptureSuccess)
        {
            CanReSet = true;
            CaptureAtkSuccessTimer -= _fixedDeltaTime;
            if (HurtNumber >= 1 && CaptureAtkSuccessTimer <= CaptureAtkSuccessTimerSet - CaptureAtkAppearTime1)
            {
                if (Order == 1)
                {
                    HurtPlayer(Damage1, ref SuccessiveHurtNumber, SuccessiveHurtTotalNumber, ref AtkFirstAppear);
                }
                else
                {
                    HurtPlayer(Damage1, ref AtkFirstAppear);
                }

                if (HurtNumber >= 2 && CaptureAtkSuccessTimer <= CaptureAtkSuccessTimerSet - CaptureAtkAppearTime2)
                {
                    if (Order == 2)
                    {
                        HurtPlayer(Damage2, ref SuccessiveHurtNumber, SuccessiveHurtTotalNumber, ref AtkSecondAppear);
                    }
                    else
                    {
                        HurtPlayer(Damage2, ref AtkSecondAppear);
                    }

                    if (HurtNumber >= 3 && CaptureAtkSuccessTimer <= CaptureAtkSuccessTimerSet - CaptureAtkAppearTime3)
                    {
                        if (Order == 3)
                        {
                            HurtPlayer(Damage3, ref SuccessiveHurtNumber, SuccessiveHurtTotalNumber, ref AtkThirdAppear);
                        }
                        else
                        {
                            HurtPlayer(Damage3, ref AtkThirdAppear);
                        }
                    }
                }
            }

            if (CaptureAtkSuccessTimer <= CaptureAtkSuccessTimerSet - SuccessObjectAppearTime)
            {
                if (!SuccessObjectAppear)
                {
                    if (FaceLeft)
                    {
                        Instantiate(SuccessObject, new Vector3(_transform.position.x - EndAppearX, _transform.position.y + EndAppearY, 0), Quaternion.identity);
                    }
                    if (FaceRight)
                    {
                        Instantiate(SuccessObject, new Vector3(_transform.position.x + EndAppearX, _transform.position.y + EndAppearY, 0), Quaternion.identity);
                    }
                    SuccessObjectAppear = true;
                }
                isPlayerFollow = false;
            }

            if (CaptureAtkSuccessTimer <= 0)
            {
                CaptureAtkEnd = true;
                AllVariableReset();
            }
        }
    }

    private void HurtPlayer(float Damage, ref bool AtkAppear)//只打傷一次型
    {
        if (!AtkAppear)
        {
            _playerController.Hp -= Damage;
            AtkAppear = true;
        }
    }

    private void HurtPlayer(float Damage, ref int Number, int TotalNumber, ref bool AtkAppear)//每次調用都要重置Number
    {
        if (!AtkAppear)
        {
            if (Number < TotalNumber)
            {
                _playerController.Hp -= Damage;
                Number += 1;
            }
            else
            {
                AtkAppear = true;
            }
        }
    }

    public void AllVariableReset()
    {
        if (CanReSet)
        {
            CaptureAtkSuccessTimer = CaptureAtkSuccessTimerSet;
            isCaptureSuccess = false;
            isPlayerFollow = false;
            AtkFirstAppear = false;
            AtkSecondAppear = false;
            AtkThirdAppear = false;
            SuccessObjectAppear = false;
            SuccessiveHurtNumber = 0;
            CanReSet = false;
        }
    }
}
