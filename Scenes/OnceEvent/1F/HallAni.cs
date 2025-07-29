using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HallAni : MonoBehaviour
{
    private Transform PlayerTransform;
    private OldPlayerAnimationController _aniController;
    private PlayerSpecialAni _specialAni;
    private AniMethod _aniMethod;
    public HallController RoomController;
    [SerializeField] private CameraController _camera;
    [SerializeField] private GuardianHead BeginingHead;
    [SerializeField] private GameObject Howl;
    [SerializeField] private GameObject BlackScreen;

    public GameObject Guardian;
    public Transform GuardianTransform;
    public GameObject BossName;
    public Transform RGuardianEscapeTransform;
    public Transform LGuardianEscapeTransform;
    private Animator RGuardianEscapeAnimator;
    private Animator LGuardianEscapeAnimator;
    private Transform RGuardianFollowPoint;
    private Transform LGuardianFollowPoint;
    private float GuardianEscapeDistance;
    private float GuardianEscapeTime;
    public GameObject GuardianHideAtk;

    private float _fixedDeltaTime;

    private float _time;
    private float BeginTime;
    private float RunningTime;
    private float SlideTime;

    private int ResetNumber = 1;
    private int PlayerNumber = 1;
    private int GuardianNumber = 1;
    private int BackgroundNumber = 1;
    private int CameraNumber = 1;
    private int SENumber = 1;

    public AudioClip WallFootStepSound;
    public AudioClip HowlSound;
    public AudioClip SlideSound;
    public AudioClip RunSound;

    private AudioSource WallFootStepSource;
    private AudioSource HowlSource;
    private AudioSource SlideSource;
    private AudioSource RunSource;

    private float RunSourceRate = 1;
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("player") != null)
        {
            PlayerTransform = GameObject.Find("player").transform;
            _aniController = GameObject.Find("player").GetComponent<OldPlayerAnimationController>();
            _specialAni = GameObject.Find("player").GetComponent<PlayerSpecialAni>();
        }
        else
        {
            return;
        }

        if (GameEvent.PassHall)
        {
            return;
        }

        if (GameObject.Find("FollowSystem") != null)
        {
            _aniMethod = GameObject.Find("FollowSystem").GetComponent<AniMethod>();
        }

        RGuardianEscapeAnimator = RGuardianEscapeTransform.transform.GetChild(0).GetComponent<Animator>();
        LGuardianEscapeAnimator = LGuardianEscapeTransform.transform.GetChild(0).GetComponent<Animator>();
        RGuardianFollowPoint = RGuardianEscapeTransform.GetChild(0).GetChild(16).GetChild(0);
        LGuardianFollowPoint = LGuardianEscapeTransform.GetChild(0).GetChild(16).GetChild(0);

        SEController.inisializeAudioSource(ref WallFootStepSource, WallFootStepSound, this.transform);
        SEController.inisializeAudioSource(ref HowlSource, HowlSound, this.transform);
        SEController.inisializeAudioSource(ref SlideSource, SlideSound, this.transform);
        SEController.inisializeAudioSource(ref RunSource, RunSound, this.transform);

        RunSource.loop = true;
    }

    private void FixedUpdate()
    {
        if (!RoomController.isDoEvent)
        {
            return;
        }

        _fixedDeltaTime = Time.fixedDeltaTime;
        _time = Time.time;
        RunningTime = _time - BeginTime;

        switch (RoomController.AniPhase)
        {
            case 1:
                //BackGround
                if (BackgroundNumber == 1)
                {
                    BeginTimeReset();
                    BackgroundNumber += 1;
                }
                //player
                if(PlayerNumber == 1)
                {
                    _aniController.AbsoluteAniFalse();
                    _aniController.WaitAniPlay();
                    PlayerNumber += 1;
                }
                break;
            case 2:
                //Reset
                if(ResetNumber == 1)
                {
                    NumberReset();
                    ResetNumber += 1;
                }
                //Background
                if (BackgroundNumber == 1)
                {
                    BeginTimeReset();
                    _aniMethod.OpenBlackScreen();
                    _specialAni.HidePlayerUI();
                    BackgroundNumber += 1;
                }
                if (RunningTime >= 13.5f && BackgroundNumber == 2)
                {
                    Howl.SetActive(true);
                    BossName.SetActive(true);
                    BackgroundNumber += 1;
                }
                if (RunningTime >= 16f && BackgroundNumber == 3)
                {
                    _aniMethod.CloseBlackScreen();
                    _specialAni.ShowPlayerUI();
                    BackgroundNumber += 1;
                }
                //Player
                if (PlayerNumber == 1)
                {
                    _aniController.AbsoluteAniFalse();
                    _specialAni.HallAniPlay(1);
                    PlayerNumber += 1;
                }
                if (RunningTime >= 15.5f && PlayerNumber ==2)
                {
                    _specialAni.SpecialAniFalse();
                    _aniController.WaitAniPlay();
                    PlayerNumber += 1;
                }
                //Guardian
                if (RunningTime >= 4 && GuardianNumber == 1)
                {
                    Guardian.SetActive(true);
                    GuardianNumber += 1;
                }
                if (RunningTime >= 4.1 && GuardianNumber == 2)
                {
                    _aniMethod.ObjectVerticalMove(-5.89f, 1.25f, _fixedDeltaTime, GuardianTransform);
                }
                if (RunningTime >= 5.35f && GuardianNumber == 2)
                {
                    GuardianNumber += 1;
                }
                if (RunningTime >= 6.15f && GuardianNumber == 3)
                {
                    _aniMethod.ObjectVerticalMove(-5.3f, 1.85f, _fixedDeltaTime, GuardianTransform);
                }
                if (RunningTime >= 8f && GuardianNumber == 3)
                {
                    GuardianNumber += 1;
                }
                if (RunningTime >= 8.8f && GuardianNumber == 4)
                {
                    _aniMethod.ObjectVerticalMove(-4.97f, 2.25f, _fixedDeltaTime, GuardianTransform);
                }
                if (RunningTime >= 10.95f && GuardianNumber == 4)
                {
                    GuardianNumber += 1;
                }
                if (RunningTime >= 13.3f && GuardianNumber == 5)
                {
                    BeginingHead.OpenMouth(2.25f, 2f);
                    GuardianNumber += 1;
                }
                //-17.37     18.73 12.84 7.54 2.57
                //x-4.72  -9.87  y-4.573 -3.22
                //Camera
                if (CameraNumber == 1)
                {
                    _camera.SpecialTarget.localPosition = PlayerTransform.localPosition;
                    CameraNumber += 1;
                }
                if(RunningTime >= 1.25f && CameraNumber == 2)
                {
                    _camera.SpecialTargetVerticalMove(8, 2, _fixedDeltaTime, _camera.MainMapFrame);
                    _camera.SpecialTargetHorizontalMove(-2, 2, _fixedDeltaTime, _camera.MainMapFrame);
                    _camera.FollowSpecialTarget();
                }
                if (RunningTime >= 3.25f && CameraNumber == 2)
                {
                    CameraNumber += 1;
                }
                if (RunningTime >= 5.5 && CameraNumber == 3)
                {
                    _camera.SpecialTargetVerticalMove(-8, 4.9f, _fixedDeltaTime, _camera.MainMapFrame);
                    _camera.FollowSpecialTarget();
                }
                if (RunningTime >= 10.4 && CameraNumber == 3)
                {
                    CameraNumber += 1;
                }
                if (RunningTime >= 16f && CameraNumber == 4)
                {
                    _camera.CameraMoveDistanceX = PlayerTransform.localPosition.x - _camera.SpecialTarget.localPosition.x;
                    _camera.CameraMoveDistanceY = PlayerTransform.localPosition.y - _camera.SpecialTarget.localPosition.y;
                    CameraNumber += 1;
                }
                if (CameraNumber == 5)
                {
                    _camera.SpecialTargetHorizontalMove(_camera.CameraMoveDistanceX, 0.5f, _fixedDeltaTime, _camera.MainMapFrame);
                    _camera.SpecialTargetVerticalMove(_camera.CameraMoveDistanceY, 0.5f, _fixedDeltaTime, _camera.MainMapFrame);
                    _camera.FollowSpecialTarget();
                }

                //SE
                if (RunningTime >= 5.15f && SENumber == 1)
                {
                    WallFootStepSource.Play();
                    SENumber += 1;
                }
                if (RunningTime >= 7.8f && SENumber == 2)
                {
                    WallFootStepSource.Play();
                    SENumber += 1;
                }
                if (RunningTime >= 10.75f && SENumber == 3)
                {
                    WallFootStepSource.Play();
                    SENumber += 1;
                }
                if (RunningTime >= 13.5f && SENumber == 4)
                {
                    HowlSource.Play();
                    MusicController.PlayBGM(4);
                    MusicController.BeginFadeInBGM(5, 1);
                    SENumber += 1;
                }

                SEController.CalculateSystemSound(WallFootStepSource);
                SEController.CalculateSystemSound(HowlSource);
                break;
            case 3:
                //Reset
                if (ResetNumber == 2)
                {
                    NumberReset();
                    ResetNumber += 1;
                }
                //Background
                if (BackgroundNumber == 1)
                {
                    BeginTimeReset();
                    BackgroundNumber += 1;
                }
                //Player
                if (PlayerNumber == 1)
                {
                    _aniController.AbsoluteAniFalse();
                    _specialAni.HallAniPlay(2);
                    if (RoomController.GuardianFaceRight)
                    {
                        _specialAni.TurnSpecialFace("L", 3);
                    }
                    if (RoomController.GuardianFaceLeft)
                    {
                        _specialAni.TurnSpecialFace("R", 3);
                    }
                    PlayerNumber += 1;
                }
                if (RunningTime >= 2.2 && PlayerNumber == 2)
                {
                    if (RoomController.GuardianFaceRight)
                    {
                        if (9 >= _aniMethod.CalculateDistance(PlayerTransform, RGuardianEscapeTransform))
                        {
                            PlayerNumber += 1;
                        }
                    }
                    if (RoomController.GuardianFaceLeft)
                    {
                        if (9 >= _aniMethod.CalculateDistance(PlayerTransform, LGuardianEscapeTransform))
                        {
                            PlayerNumber += 1;
                        }
                    }
                }
                if (PlayerNumber == 3)
                {
                    _specialAni.HallAniPlay(3);
                    SlideTime = RunningTime;
                    PlayerNumber += 1;
                }
                if (PlayerNumber == 4)
                {
                    if (RoomController.GuardianFaceRight)
                    {
                        _aniMethod.ObjectHorizontalMove(-1, 0.4f, _fixedDeltaTime, PlayerTransform);
                    }
                    if (RoomController.GuardianFaceLeft)
                    {
                        _aniMethod.ObjectHorizontalMove(1, 0.4f, _fixedDeltaTime, PlayerTransform);
                    }
                }
                if (PlayerNumber == 4 && (RunningTime - SlideTime) >= 0.4)
                {
                    SlideSource.Play();
                    PlayerNumber += 1;
                }
                if (PlayerNumber == 5)
                {
                    if (RoomController.GuardianFaceRight)
                    {
                        _aniMethod.ObjectHorizontalMove(-9f, 0.6f, _fixedDeltaTime, PlayerTransform);
                    }
                    if (RoomController.GuardianFaceLeft)
                    {
                        _aniMethod.ObjectHorizontalMove(9f, 0.6f, _fixedDeltaTime, PlayerTransform);
                    }
                }
                if (PlayerNumber == 5 && (RunningTime - SlideTime) >= 1)
                {
                    PlayerNumber += 1;
                }
                //Guaedian
                if (GuardianNumber  == 1)
                {
                    GuardianEscapeDistance = 4.02f;
                    if (RoomController.GuardianFaceRight && RGuardianEscapeTransform.localPosition.x > -13.8f)
                    {
                        _aniMethod.ObjectHorizontalMove(-GuardianEscapeDistance, 0.25f, _fixedDeltaTime, RGuardianEscapeTransform);
                    }
                    if (RoomController.GuardianFaceLeft && LGuardianEscapeTransform.localPosition.x< 18.2f)
                    {
                        _aniMethod.ObjectHorizontalMove(GuardianEscapeDistance, 0.25f, _fixedDeltaTime, LGuardianEscapeTransform);
                    }
                }
                if (RunningTime >= 0.25 && GuardianNumber == 1)
                {
                    GuardianNumber += 1;
                }
                if (GuardianNumber == 2)
                {
                    GuardianEscapeDistance = 2.72f;
                    if (RoomController.GuardianFaceRight && RGuardianEscapeTransform.localPosition.x > -13.8f)
                    {
                        _aniMethod.ObjectHorizontalMove(-GuardianEscapeDistance, 0.3f, _fixedDeltaTime, RGuardianEscapeTransform);
                    }
                    if (RoomController.GuardianFaceLeft && LGuardianEscapeTransform.localPosition.x < 18.2f)
                    {
                        _aniMethod.ObjectHorizontalMove(GuardianEscapeDistance, 0.3f, _fixedDeltaTime, LGuardianEscapeTransform);
                    }
                }
                if (RunningTime >= 0.55 && GuardianNumber == 2)
                {
                    GuardianNumber += 1;
                }
                if (GuardianNumber == 3)
                {
                    GuardianEscapeDistance = 3.8f;
                    if (RoomController.GuardianFaceRight && RGuardianEscapeTransform.localPosition.x > -13.8f)
                    {
                        _aniMethod.ObjectHorizontalMove(-GuardianEscapeDistance, 0.35f, _fixedDeltaTime, RGuardianEscapeTransform);
                    }
                    if (RoomController.GuardianFaceLeft && LGuardianEscapeTransform.localPosition.x < 18.2f)
                    {
                        _aniMethod.ObjectHorizontalMove(GuardianEscapeDistance, 0.35f, _fixedDeltaTime, LGuardianEscapeTransform);
                    }
                }
                if (RunningTime >= 0.9 && GuardianNumber == 3)
                {
                    GuardianNumber += 1;
                }
                if (RunningTime >= 2 && GuardianNumber == 4)
                {
                    GuardianEscapeDistance = 4.16f;
                    if (RoomController.GuardianFaceRight)
                    {
                        _aniMethod.ObjectHorizontalMove(GuardianEscapeDistance, 0.85f, _fixedDeltaTime, RGuardianEscapeTransform);
                    }
                    if (RoomController.GuardianFaceLeft)
                    {
                        _aniMethod.ObjectHorizontalMove(-GuardianEscapeDistance, 0.85f, _fixedDeltaTime, LGuardianEscapeTransform);
                    }
                }
                if (RunningTime >= 2.85 && GuardianNumber == 4)
                {
                    if (RoomController.GuardianFaceRight)
                    {
                        GuardianEscapeDistance = 18.2f - RGuardianEscapeTransform.localPosition.x;
                    }
                    if (RoomController.GuardianFaceLeft)
                    {
                        GuardianEscapeDistance = LGuardianEscapeTransform.localPosition.x + 13.8f;
                    }
                    GuardianEscapeTime = CalculateGuardianEscapeTime();
                    GuardianNumber += 1;
                }
                if (GuardianNumber == 5)
                {
                    if (RoomController.GuardianFaceRight)
                    {
                        _aniMethod.ObjectHorizontalMove(GuardianEscapeDistance, GuardianEscapeTime, _fixedDeltaTime, RGuardianEscapeTransform);
                    }
                    if (RoomController.GuardianFaceLeft)
                    {
                        _aniMethod.ObjectHorizontalMove(-GuardianEscapeDistance, GuardianEscapeTime, _fixedDeltaTime, LGuardianEscapeTransform);
                    }
                }
                //еk18.2 ек-13.8  Y -3.22
                //Camera
                if (CameraNumber == 1)
                {
                    if (RoomController.GuardianFaceRight)
                    {
                        _camera.SpecialTarget.localPosition = RGuardianFollowPoint.position;
                    }
                    if (RoomController.GuardianFaceLeft)
                    {
                        _camera.SpecialTarget.localPosition = LGuardianFollowPoint.position;
                    }
                    _camera.FollowSpecialTarget();
                }
                //SE
                if (SENumber == 1)
                {
                    MusicController.BeginFadeOutBGM(1, 0.4f);
                    SENumber += 1;
                }
                if (RunningTime >= 2.85 && SENumber == 2)
                {
                    RunSource.Play();
                    SENumber += 1;
                }

                SEController.CalculateSystemSound(SlideSource);
                SEController.CalculateSystemSound(RunSource);
                break;
            case 4:
                //Reset
                if (ResetNumber == 3)
                {
                    NumberReset();
                    ResetNumber += 1;
                }
                //Background
                if (BackgroundNumber == 1)
                {
                    BeginTimeReset();
                    BackgroundNumber += 1;
                }
                //Player
                if (RunningTime >= 4f && PlayerNumber == 1)
                {
                    _specialAni.SpecialAniFalse();
                    _aniController.WaitAniPlay();
                }
                //Guaedian
                if (GuardianNumber == 1)
                {
                    if (RoomController.GuardianFaceRight)
                    {
                        RGuardianEscapeAnimator.SetBool("Stop", true);
                    }
                    if (RoomController.GuardianFaceLeft)
                    {
                        LGuardianEscapeAnimator.SetBool("Stop", true);
                    }
                    GuardianNumber += 1;
                }
                //Camera
                if (CameraNumber == 1)
                {
                    if (RoomController.GuardianFaceRight)
                    {
                        _camera.SpecialTarget.localPosition = RGuardianFollowPoint.position;
                    }
                    if (RoomController.GuardianFaceLeft)
                    {
                        _camera.SpecialTarget.localPosition = LGuardianFollowPoint.position;
                    }
                    _camera.FollowSpecialTarget();
                }
                if (RunningTime >= 4.5 && CameraNumber == 1)
                {
                    _camera.CameraMoveDistanceX = _camera.CalculateValidPoint(PlayerTransform.localPosition).x - _camera.CalculateValidPoint(_camera.SpecialTarget.localPosition).x;
                    _camera.CameraMoveDistanceY = _camera.CalculateValidPoint(PlayerTransform.localPosition).y - _camera.CalculateValidPoint(_camera.SpecialTarget.localPosition).y;
                    CameraNumber += 1;
                }
                if (CameraNumber == 2)
                {
                    _camera.SpecialTargetHorizontalMove(_camera.CameraMoveDistanceX, 0.5f, _fixedDeltaTime, _camera.MainMapFrame);
                    _camera.SpecialTargetVerticalMove(_camera.CameraMoveDistanceY, 0.5f, _fixedDeltaTime, _camera.MainMapFrame);
                    _camera.FollowSpecialTarget();
                }
                //SE
                if (SENumber == 1)
                {
                    RunSourceRate = SEController.VolumeFadeOut(RunSourceRate, 0.5f, _fixedDeltaTime);
                    RunSource.volume = RunSourceRate * SEController.SEVolume;
                    if (RunSourceRate <= 0)
                    {
                        RunSource.Stop();
                        SENumber += 1;
                    }
                }
                if (RunningTime >= 1.2f && SENumber == 2)
                {
                    WallFootStepSource.Play();
                    SENumber += 1;
                }
                if (RunningTime >= 1.6f && SENumber == 3)
                {
                    WallFootStepSource.Play();
                    SENumber += 1;
                }
                if (RunningTime >= 2.6f && SENumber == 4)
                {
                    WallFootStepSource.Play();
                    SENumber += 1;
                }
                if (RunningTime >= 3.4f && SENumber == 5)
                {
                    WallFootStepSource.Play();
                    SENumber += 1;
                }
                if (RunningTime >= 4.5f && SENumber == 6)
                {
                    MusicController.BeginFadeOutBGM();
                    SENumber += 1;
                }

                SEController.CalculateSystemSound(WallFootStepSource);
                break;
            case 5:
                //Reset
                if (ResetNumber == 4)
                {
                    NumberReset();
                    ResetNumber += 1;
                }
                //Background
                if (BackgroundNumber == 1)
                {
                    BeginTimeReset();
                    BackgroundNumber += 1;
                }
                if (RunningTime >= 0.2 && BackgroundNumber == 2)
                {
                    BlackScreen.SetActive(true);
                    _specialAni.HidePlayerUI();
                    BackgroundNumber += 1;
                }
                //Guardian
                if(GuardianNumber == 1)
                {
                    GuardianHideAtk.SetActive(true);
                    GuardianNumber += 1;
                }
                //Player
                if (PlayerNumber== 1)
                {
                    _aniController.AbsoluteAniFalse();
                    _aniController.WalkAniPlay();
                    PlayerNumber += 1;
                }
                if (PlayerNumber == 2)
                {
                    _aniMethod.ObjectHorizontalMove(4, 1, _fixedDeltaTime, PlayerTransform);
                }
                if (RunningTime >= 0.3f && PlayerNumber == 2)
                {
                    PlayerNumber += 1;
                }
                break;
        }
    }

    private void BeginTimeReset()
    {
        BeginTime = Time.time;
        RunningTime = _time - BeginTime;
    }

    private void NumberReset()
    {
        BackgroundNumber = 1;
        PlayerNumber = 1;
        GuardianNumber = 1;
        CameraNumber = 1;
        SENumber = 1;
    }

    private float CalculateGuardianEscapeTime()
    {
        float Time = 0;
        Time = 5.4f / 0.45f;
        Time = GuardianEscapeDistance / Time;
        Time = Time / 0.45f;
        Time = Mathf.Round(Time);
        Time = Time * 0.45f;
        return Time;
    }
}
