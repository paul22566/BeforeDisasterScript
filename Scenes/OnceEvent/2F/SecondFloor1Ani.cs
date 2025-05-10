using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SecondFloor1Ani : MonoBehaviour
{
    private PlayerAnimationController _aniController;
    private AniMethod _aniMethod;
    private float _time;
    private float BeginTime;
    private float RunningTime;
    private float _fixedDeltaTime;

    public CameraController _cameraFollow;
    private SecondFloor1Controller RoomController;

    public Transform SpecialTarget;

    private int PlayerNumber = 1;
    private int BackgroundNumber = 1;
    private int CameraNumber = 1;
    private int RotateCircleNumber = 1;
    private int SENumber = 1;

    private int ResetNumber = 1;

    //轉盤運作
    public Animator RotateCircleAni;
    public Animator RotateCenterAni;
    public Animator MainSwitchAni;
    public Animator LeftFenceAni;
    public Animator RightFenceAni;

    //轉盤毀壞
    private Transform PlayerTransform;
    private Vector3 SpecialTargetPoint = new Vector3(-79.489f, 5.74f, 0);
    private Vector2 SpecialMoveDirection = new Vector2();
    public GameObject Explosion1;
    public GameObject Explosion2;
    public GameObject Explosion3;
    public GameObject Explosion4;
    public GameObject AniCircle;
    public GameObject RotateCircle;
    public Animator CircleCenterAni;
    private SpriteRenderer _spr1;
    private SpriteRenderer _spr2;
    private SpriteRenderer _spr3;
    private SpriteRenderer _spr4;
    private SpriteRenderer _spr5;

    //SE
    public AudioClip MetalFenceOpenSound;
    public AudioClip MetalFenceCloseSound;
    public AudioClip CircleRunSound;
    public AudioClip CircleCenterSound;
    public AudioClip CrashSound;
    public AudioClip RollSound;

    private AudioSource MetalFenceOpenSource;
    private AudioSource MetalFenceCloseSource;
    private AudioSource CircleRunSource;
    private AudioSource CircleCenterSource;
    private AudioSource CrashSource;
    private AudioSource RollSource;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("player") != null)
        {
            PlayerTransform = GameObject.Find("player").transform;
            _aniController = GameObject.Find("player").GetComponent<PlayerAnimationController>();
        }
        else
        {
            return;
        }
        if (GameObject.Find("FollowSystem") != null)
        {
            _aniMethod = GameObject.Find("FollowSystem").GetComponent<AniMethod>();
        }
        RoomController = this.GetComponent<SecondFloor1Controller>();

        if (!GameEvent.DestroyCircleDoor)
        {
            _spr1 = AniCircle.transform.GetChild(0).GetComponent<SpriteRenderer>();
            _spr2 = AniCircle.transform.GetChild(1).GetComponent<SpriteRenderer>();
            _spr3 = AniCircle.transform.GetChild(2).GetComponent<SpriteRenderer>();
            _spr4 = AniCircle.transform.GetChild(2).GetChild(0).GetComponent<SpriteRenderer>();
            _spr5 = AniCircle.transform.GetChild(3).GetComponent<SpriteRenderer>();
        }

        SEController.inisializeAudioSource(ref MetalFenceOpenSource, MetalFenceOpenSound, this.transform);
        SEController.inisializeAudioSource(ref MetalFenceCloseSource, MetalFenceCloseSound, this.transform);
        SEController.inisializeAudioSource(ref CircleRunSource, CircleRunSound, this.transform);
        SEController.inisializeAudioSource(ref CircleCenterSource, CircleCenterSound, this.transform);
        SEController.inisializeAudioSource(ref CrashSource, CrashSound, this.transform);
        SEController.inisializeAudioSource(ref RollSource, RollSound, this.transform);
    }

    private void FixedUpdate()
    {
        _time = Time.time;
        RunningTime = _time - BeginTime;
        _fixedDeltaTime = Time.fixedDeltaTime;

        if (!GameEvent.isAniPlay || GameObject.Find("player") == null)
        {
            return;
        }

        switch (RoomController.StatusNumber)
        {
            case 2:
                //BackgroundSystem
                if (BackgroundNumber == 1)
                {
                    BeginTimeReset();
                    BackgroundNumber += 1;
                }
                if (BackgroundNumber == 2)
                {
                    MainSwitchAni.SetBool("Open", true);
                    BackgroundNumber += 1;
                }
                if (RunningTime >= 4 && BackgroundNumber == 3)
                {
                    RightFenceAni.SetBool("Open", true);
                    LeftFenceAni.SetBool("Open", true);
                    BackgroundNumber += 1;
                }
                if (RunningTime >= 6 && BackgroundNumber == 4)
                {
                    RightFenceAni.SetBool("Open", false);
                    LeftFenceAni.SetBool("Open", false);
                    BackgroundNumber += 1;
                }
                //Player
                if (PlayerNumber == 1)
                {
                    _aniController.WaitAniPlay();
                    PlayerNumber += 1;
                }
                //轉盤
                if (RunningTime >= 3 && RotateCircleNumber == 1)
                {
                    RotateCenterAni.SetBool("Open", true);
                    RotateCircleNumber += 1;
                }
                if (RunningTime >= 4 && RotateCircleNumber == 2)
                {
                    RotateCircleAni.SetInteger("Status", 1);
                    RotateCircleNumber += 1;
                }
                //Camera
                if (CameraNumber <= 3)
                {
                    _cameraFollow.FollowSpecialTarget();
                }
                if (RunningTime >= 1f && CameraNumber == 1)
                {
                    _cameraFollow.SpecialTargetVerticalMove(-11.82f, 2f, Time.fixedDeltaTime, _cameraFollow.MainMapFrame);
                }
                if (RunningTime >= 3f && CameraNumber == 1)
                {
                    CameraNumber += 1;
                }
                if (RunningTime >= 6f && CameraNumber == 2)
                {
                    _cameraFollow.SpecialTargetVerticalMove(11.82f, 0.5f, Time.fixedDeltaTime, _cameraFollow.MainMapFrame);
                }
                if (RunningTime >= 6.5f && CameraNumber == 2)
                {
                    CameraNumber += 1;
                }
                //SE
                if (RunningTime >= 3f && SENumber == 1)
                {
                    CircleCenterSource.Play();
                    SENumber += 1;
                }
                if (RunningTime >= 4f && SENumber == 2)
                {
                    MetalFenceOpenSource.Play();
                    CircleRunSource.Play();
                    SENumber += 1;
                }
                if (RunningTime >= 6f && SENumber == 3)
                {
                    MetalFenceCloseSource.Play();
                    SENumber += 1;
                }

                SEController.CalculateSystemSound(MetalFenceOpenSource);
                SEController.CalculateSystemSound(MetalFenceCloseSource);
                SEController.CalculateSystemSound(CircleRunSource);
                SEController.CalculateSystemSound(CircleCenterSource);
                break;
            case 3:
                //reset
                if (ResetNumber == 1)
                {
                    NumberReset();
                    ResetNumber += 1;
                }
                //BackgroundSystem
                if (BackgroundNumber == 1)
                {
                    BeginTimeReset();
                    BackgroundNumber += 1;
                }
                if (RunningTime >= 1 && BackgroundNumber == 2)
                {
                    Explosion1.SetActive(true);
                    BackgroundNumber += 1;
                }
                if (RunningTime >= 1.3f && BackgroundNumber == 3)
                {
                    Explosion2.SetActive(true);
                    BackgroundNumber += 1;
                }
                if (RunningTime >= 2 && BackgroundNumber == 4)
                {
                    Explosion3.SetActive(true);
                    BackgroundNumber += 1;
                }
                if (RunningTime >= 2.5f && BackgroundNumber == 5)
                {
                    Explosion4.SetActive(true);
                    BackgroundNumber += 1;
                }
                //Player
                if (PlayerNumber == 1)
                {
                    _aniController.WaitAniPlay();
                    PlayerNumber += 1;
                }
                //轉輪
                if (RunningTime >= 1 && RotateCircleNumber == 1)
                {
                    CircleCenterAni.SetBool("Disappear", true);
                    RotateCircleNumber += 1;
                }
                if (RunningTime >= 3.2f && RotateCircleNumber == 2)
                {
                    Destroy(RotateCircle);
                    AniCircle.SetActive(true);
                    RotateCircleNumber += 1;
                }
                if (RunningTime >= 3.6f && RotateCircleNumber == 3)
                {
                    _spr1.sortingLayerName = "Default";
                    _spr2.sortingLayerName = "Default";
                    _spr3.sortingLayerName = "Default";
                    _spr4.sortingLayerName = "Default";
                    _spr5.sortingLayerName = "Default";
                    RotateCircleNumber += 1;
                }
                if (RunningTime >= 7.7f && RotateCircleNumber == 4)
                {
                    Destroy(AniCircle);
                    RotateCircleNumber += 1;
                }
                //Camera
                if (CameraNumber == 1)
                {
                    _aniMethod.FollowTarget(PlayerTransform, SpecialTarget);
                    SpecialMoveDirection = new Vector2(SpecialTargetPoint.x - PlayerTransform.localPosition.x, SpecialTargetPoint.y - PlayerTransform.localPosition.y);
                    CameraNumber += 1;
                }
                if (CameraNumber == 2)
                {
                    _cameraFollow.SpecialTargetHorizontalMove(SpecialMoveDirection.x, 0.5f, _fixedDeltaTime, _cameraFollow.MainMapFrame);
                    _cameraFollow.SpecialTargetVerticalMove(SpecialMoveDirection.y, 0.5f, _fixedDeltaTime, _cameraFollow.MainMapFrame);
                }
                if (RunningTime >= 0.5f && CameraNumber == 2)
                {
                    CameraNumber += 1;
                }
                if (RunningTime >= 7.7f && CameraNumber == 3)
                {
                    _cameraFollow.SpecialTargetHorizontalMove(-SpecialMoveDirection.x, 0.5f, _fixedDeltaTime, _cameraFollow.MainMapFrame);
                    _cameraFollow.SpecialTargetVerticalMove(-SpecialMoveDirection.y, 0.5f, _fixedDeltaTime, _cameraFollow.MainMapFrame);
                }
                if (RunningTime >= 8.2f && CameraNumber == 3)
                {
                    CameraNumber += 1;
                }
                if (CameraNumber <= 3)
                {
                    _cameraFollow.FollowSpecialTarget();
                }
                //SE
                if (RunningTime >= 3.7f && SENumber == 1)
                {
                    CrashSource.Play();
                    SENumber += 1;
                }
                if (RunningTime >= 4.1f && SENumber == 2)
                {
                    RollSource.Play();
                    SENumber += 1;
                }

                SEController.CalculateSystemSound(CrashSource);
                SEController.CalculateSystemSound(RollSource);
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
        PlayerNumber = 1;
        BackgroundNumber = 1;
        CameraNumber = 1;
        RotateCircleNumber = 1;
        SENumber = 1;
    }
}
