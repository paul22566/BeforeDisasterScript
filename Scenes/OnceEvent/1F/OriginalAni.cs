using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OriginalAni : MonoBehaviour
{
    private Transform PlayerTransform;
    private PlayerAnimationController _aniController;
    private PlayerSpecialAni _specialAni;
    private AniMethod _aniMethod;
    public OriginalController RoomController;

    public CameraController _Camera;
    public Transform RopeAppearPlace;
    public GameObject FrontRope;
    public GameObject BackRope;
    public GameObject NormalFadeIn;
    public GameObject WhiteFadeIn;

    private float _time;
    private float BeginTime;
    private float RunningTime;

    private int PlayerNumber = 1;
    private int BackgroundNumber = 1;
    private int SENumber = 1;

    public AudioClip RopeGunSound;
    public AudioClip ShootRopeSound;
    public AudioClip RopeBreakSound;
    public AudioClip RopeFallSound;
    public AudioClip SwordSound;

    private AudioSource RopeGunSource;
    private AudioSource ShootRopeSource;
    private AudioSource RopeBreakSource;
    private AudioSource RopeFallSource;
    private AudioSource SwordSource;

    // Start is called before the first frame update
    void Start()
    {
        if (RoomController.isDoEvent == false)
        {
            return;
        }

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

        if (RoomController.AniPhase == 1)
        {
            NormalFadeIn.SetActive(false);
            WhiteFadeIn.SetActive(true);
        }

        RopeGunSource = this.AddComponent<AudioSource>();
        ShootRopeSource = this.AddComponent<AudioSource>();
        RopeBreakSource = this.AddComponent<AudioSource>();
        RopeFallSource = this.AddComponent<AudioSource>();
        SwordSource = this.AddComponent<AudioSource>();

        RopeGunSource.clip = RopeGunSound;
        ShootRopeSource.clip = ShootRopeSound;
        RopeBreakSource.clip = RopeBreakSound;
        RopeFallSource.clip = RopeFallSound;
        SwordSource.clip = SwordSound;
    }

    private void FixedUpdate()
    {
        if(RoomController.isDoEvent == false)
        {
            return;
        }

        _time = Time.time;
        RunningTime = _time - BeginTime;

        switch (RoomController.AniPhase)
        {
            case 1:
                //Background
                if (BackgroundNumber == 1)
                {
                    BeginTime = Time.time;
                    RunningTime = _time - BeginTime;
                    BackgroundNumber += 1;
                }
                if (BackgroundNumber == 2)
                {
                    _aniMethod.OpenBlackScreen();
                    BackgroundNumber += 1;
                }
                if (RunningTime >= 0.5 && BackgroundNumber == 3)
                {
                    WhiteFadeIn.GetComponent<FadeInController>().OpenFadeIn();
                    BackgroundNumber += 1;
                }
                if (RunningTime >= 5.75 && BackgroundNumber == 4)
                {
                    Instantiate(FrontRope, RopeAppearPlace.localPosition, Quaternion.identity);
                    BackgroundNumber += 1;
                }
                if (RunningTime >= 7.5 && BackgroundNumber == 5)
                {
                    Instantiate(BackRope, RopeAppearPlace.localPosition, Quaternion.identity);
                    BackgroundNumber += 1;
                }
                if (RunningTime >= 20.25 && BackgroundNumber == 6)
                {
                    _aniMethod.CloseBlackScreen();
                    BackgroundNumber += 1;
                }

                //Player
                if (PlayerNumber == 1)
                {
                    _specialAni.SpecialAniFalse();
                    _aniController.AbsoluteAniFalse();
                    PlayerNumber += 1;
                }
                if (RunningTime >= 0.5 && PlayerNumber == 2)
                {
                    _specialAni.GoInOriginalAniPlay(1);
                    PlayerNumber += 1;
                }
                if (RunningTime >= 20.75f && PlayerNumber == 3)
                {
                    _specialAni.SpecialAniFalse();
                    _aniController.WaitAniPlay();
                    _aniController.SwitchFace("R");
                    PlayerNumber += 1;
                }

                //SE
                if (RunningTime >= 6.75 && SENumber == 1)
                {
                    RopeGunSource.Play();
                    SENumber += 1;
                }
                if (RunningTime >= 7.5 && SENumber == 2)
                {
                    ShootRopeSource.Play();
                    SENumber += 1;
                }
                if (RunningTime >= 8.16 && SENumber == 3)
                {
                    RopeBreakSource.Play();
                    SENumber += 1;
                }
                if (RunningTime >= 9.2 && SENumber == 4)
                {
                    RopeFallSource.Play();
                    SENumber += 1;
                }
                if (RunningTime >= 11.95 && SENumber == 5)
                {
                    RopeFallSource.Play();
                    SENumber += 1;
                }
                if (RunningTime >= 17.7 && SENumber == 6)
                {
                    SwordSource.Play();
                    SENumber += 1;
                }

                //Camera
                _Camera.FollowSpecialTarget();
                break;
            case 2:
                //Background
                if (BackgroundNumber == 1)
                {
                    BeginTime = Time.time;
                    RunningTime = _time - BeginTime;
                    BackgroundNumber += 1;
                }

                //Player
                if (PlayerNumber == 1)
                {
                    _specialAni.SpecialAniFalse();
                    _aniController.AbsoluteAniFalse();
                    PlayerNumber += 1;
                }
                if (PlayerNumber == 2)
                {
                    _specialAni.GoInOriginalAniPlay(2);
                    PlayerNumber += 1;
                }
                if (RunningTime >= 8.95f && PlayerNumber == 3)
                {
                    _specialAni.SpecialAniFalse();
                    _aniController.SwitchFace("R");
                    _aniController.WaitAniPlay();
                    PlayerNumber += 1;
                }

                //Camera
                _Camera.FollowPlayer();
                break;
        }
    }
}
