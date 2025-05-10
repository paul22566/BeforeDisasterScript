using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Training2Ani : MonoBehaviour
{
    private Transform PlayerTransform;
    private PlayerAnimationController _aniController;
    private PlayerSpecialAni _specialAni;
    private AniMethod _aniMethod;
    private Training2Controller _roomController;

    public GameObject WhiteFadeOut;

    private float _fixedDeltaTime;

    private float _time;
    private float BeginTime;
    private float RunningTime;

    private int PlayerNumber = 1;
    private int BackgroundNumber = 1;
    private int BeginTimeResetNumber = 1;

    private float SpeedWeight = 10;
    private float NowParabolaX = -1.5f;
    private float ParabolaSpeed = 7f;
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("player") != null)
        {
            PlayerTransform = GameObject.Find("player").transform;
            _aniController = GameObject.Find("player").GetComponent<PlayerAnimationController>();
            _specialAni = GameObject.Find("player").GetComponent<PlayerSpecialAni>();
        }
        if (GameObject.Find("FollowSystem") != null)
        {
            _aniMethod = GameObject.Find("FollowSystem").GetComponent<AniMethod>();
        }
        _roomController = this.GetComponent<Training2Controller>();

        BeginTime = Time.time;
    }

    private void FixedUpdate()
    {
        _fixedDeltaTime = Time.fixedDeltaTime;
        _time = Time.time;
        RunningTime = _time - BeginTime;

        if (!GameEvent.isAniPlay || GameObject.Find("player") == null)
        {
            return;
        }

        if (_roomController.BeginAtk)
        {
            if (BeginTimeResetNumber == 2)
            {
                BeginTimeReset();
            }
            //Player
            if (PlayerNumber == 3)
            {
                _specialAni.TrainingEndAniChange();
                PlayerNumber += 1;
            }
            if (RunningTime >= 1.5 && PlayerNumber == 4)
            {
                _aniMethod.ObjectHorizontalMove(-0.4f * SpeedWeight, 0.15f , _fixedDeltaTime, PlayerTransform);
            }
            if (RunningTime >=1.55f && PlayerNumber == 4)
            {
                ParabolaCalculate();
            }
            //Background
            if (BackgroundNumber == 3)
            {
                BackgroundNumber += 1;
            }
            if (RunningTime >= 1.5 && BackgroundNumber == 4)
            {
                WhiteFadeOut.SetActive(true);
                BackgroundNumber += 1;
            }
        }
        else
        {
            if (BeginTimeResetNumber == 1)
            {
                BeginTimeReset();
            }
            //Player
            if(PlayerNumber == 1)
            {
                _aniController.AbsoluteAniFalse();
                _aniController.WaitAniPlay();
                PlayerNumber += 1;
            }
            if(RunningTime >= 1 && PlayerNumber == 2)
            {
                _specialAni.TrainingEndAniPlay();
                PlayerNumber += 1;
            }
            //Background
            if (BackgroundNumber == 1)
            {
                BackgroundNumber += 1;
            }
            if (RunningTime >= 1.5 && BackgroundNumber == 2)
            {
                _aniMethod.OpenBlackScreen();
                BackgroundNumber += 1;
            }
        }
    }

    private void ParabolaCalculate()
    {
        NowParabolaX = NowParabolaX + ParabolaSpeed * _fixedDeltaTime;
        if(NowParabolaX >= 0)
        {
            NowParabolaX = 0;
        }
        SpeedWeight = NowParabolaX * NowParabolaX / 0.25f + 1;
    }

    private void BeginTimeReset()
    {
        BeginTime = _time;
        RunningTime = _time - BeginTime;
        BeginTimeResetNumber += 1;
    }
}
