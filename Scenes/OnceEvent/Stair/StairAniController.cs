using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class StairAniController : MonoBehaviour
{
    private Transform PlayerTransform;
    private PlayerAnimationController _aniController;
    private PlayerSpecialAni _specialAniController;
    private AniMethod _aniMethod;
    private int EventNumber;

    private float _time;
    private float BeginTime;
    private float RunningTime;

    public CameraController _cameraFollow;
    private StairController RoomController;
    public FadeInController _fadeIn;
    public Transform DontMovePoint;

    public AudioClip FenceSound;
    public AudioClip FallSound;

    private AudioSource FenceSource;
    private AudioSource FallSource;

    private int PlayerNumber = 1;
    private int BackgroundNumber = 1;
    private int CameraNumber = 1;
    private int SENumber = 1;
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("player") != null)
        {
            PlayerTransform = GameObject.Find("player").transform;
            _aniController = GameObject.Find("player").GetComponent<PlayerAnimationController>();
            _specialAniController = GameObject.Find("player").GetComponent<PlayerSpecialAni>();
        }
        else
        {
            return;
        }
        if (GameObject.Find("FollowSystem") != null)
        {
            _aniMethod = GameObject.Find("FollowSystem").GetComponent<AniMethod>();
        }
        RoomController = this.GetComponent<StairController>();
        EventNumber = RoomController.EventNumber;

        if (EventNumber != 1)
        {
            return;
        }

        SEController.inisializeAudioSource(ref FenceSource, FenceSound, this.transform);
        SEController.inisializeAudioSource(ref FallSource, FallSound, this.transform);
    }

    private void FixedUpdate()
    {
        _time = Time.time;
        RunningTime = _time - BeginTime;

        if (!GameEvent.isAniPlay || GameObject.Find("player") == null)
        {
            return;
        }

        switch (EventNumber)
        {
            case 1:
                //BackgroundSystem
                if (BackgroundNumber == 1)
                {
                    BeginTimeReset();
                    BackgroundNumber += 1;
                }
                if (RunningTime >= 1.5f && BackgroundNumber == 2)
                {
                    _fadeIn.OpenFadeIn();
                    BackgroundNumber += 1;
                }
                //Player
                if (PlayerNumber == 1)
                {
                    _aniController.AbsoluteAniFalse();
                    _aniController.SwitchFace("R");
                    _aniController.FallAniPlay();
                    PlayerNumber += 1;
                }
                if (PlayerNumber == 2)
                {
                    _aniMethod.FollowTarget(DontMovePoint, PlayerTransform);
                }
                if (RunningTime >= 1.5f && PlayerNumber == 2)
                {
                    PlayerNumber += 1;
                }
                if (PlayerController.isGround && PlayerNumber == 3)
                {
                    _aniController.LongFallAniPlay();
                    PlayerNumber += 1;
                }
                //Camera
                if (CameraNumber == 1)
                {
                    _cameraFollow.FollowSpecialTarget();
                }
                if (RunningTime >= 2.5f && CameraNumber == 1)
                {
                    CameraNumber += 1;
                }
                if (CameraNumber == 2)
                {
                    _cameraFollow.FollowPlayer();
                }
                //SE
                if (RunningTime >= 0.5f && SENumber == 1)
                {
                    FenceSource.Play();
                    SENumber += 1;
                }
                if (PlayerController.isGround && SENumber == 2)
                {
                    FallSource.Play();
                    SENumber += 1;
                }

                SEController.CalculateSystemSound(FenceSource);
                SEController.CalculateSystemSound(FallSource);
                break;
        }
    }

    private void BeginTimeReset()
    {
        BeginTime = Time.time;
        RunningTime = _time - BeginTime;
    }
}
