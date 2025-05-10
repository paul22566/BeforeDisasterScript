using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class SecretRoomAni : MonoBehaviour
{
    private PlayerAnimationController _aniController;
    private PlayerSpecialAni _specialAni;
    private AniMethod _aniMethod;
    private float _time;
    private float BeginTime;
    private float RunningTime;
    private float _fixedDeltaTime;

    public CameraController _camera;
    private SecretRoomController RoomController;

    //§l¦¬¯¬ºÖ
    private Transform PlayerTransform;
    public Transform PlayerPoint;
    public GameObject ShadowSnake;
    public GameObject Flash;

    public AudioClip ShadowSnakeSound1;
    public AudioClip ShadowSnakeSound3;
    public AudioClip SnakeCrashSound;
    public AudioClip SealSound;
    public AudioClip SealCompleteSound;
    public AudioClip LosePowerSound;

    private AudioSource ShadowSnakeSource1;
    private AudioSource ShadowSnakeSource3;
    private AudioSource SnakeCrashSource;
    private AudioSource SealSource;
    private AudioSource SealCompleteSource;
    private AudioSource LosePowerSource;

    private int PlayerNumber = 1;
    private int BackgroundNumber = 1;
    private int CameraNumber = 1;
    private int SENumber = 1;
    // Start is called before the first frame update
    void Start()
    {
        if (itemManage.CheckItemExist(ItemID.UnDeadSnake) || itemManage.CheckItemExist(ItemID.WeakUnDeadSnake))
        {
            return;
        }

        if (GameObject.Find("player") != null)
        {
            PlayerTransform = GameObject.Find("player").transform;
            _specialAni = GameObject.Find("player").GetComponent<PlayerSpecialAni>();
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
        RoomController = this.GetComponent<SecretRoomController>();

        SEController.inisializeAudioSource(ref ShadowSnakeSource1, ShadowSnakeSound1, this.transform);
        SEController.inisializeAudioSource(ref ShadowSnakeSource3, ShadowSnakeSound3, this.transform);
        SEController.inisializeAudioSource(ref SnakeCrashSource, SnakeCrashSound, this.transform);
        SEController.inisializeAudioSource(ref SealSource, SealSound, this.transform);
        SEController.inisializeAudioSource(ref SealCompleteSource, SealCompleteSound, this.transform);
        SEController.inisializeAudioSource(ref LosePowerSource, LosePowerSound, this.transform);
    }

    private void FixedUpdate()
    {
        if (RoomController == null || !RoomController.isDoEvent)
        {
            return;
        }

        _fixedDeltaTime = Time.fixedDeltaTime;
        _time = Time.time;
        RunningTime = _time - BeginTime;

        switch (RoomController.Status)
        {
            case 1:
                //Background
                if (BackgroundNumber == 1)
                {
                    BeginTimeReset();
                    BackgroundNumber += 1;
                }
                if (RunningTime >= 3 && BackgroundNumber == 2)
                {
                    ShadowSnake.SetActive(true);
                    BackgroundNumber += 1;
                }
                if (RunningTime >= 8.6 && BackgroundNumber == 3)
                {
                    _specialAni.SealSkillPower(1);
                    BackgroundNumber += 1;
                }
                if (RunningTime >= 9.6 && BackgroundNumber == 4)
                {
                    Flash.SetActive(true);
                    _specialAni.SealSkillPower(2);
                    BackgroundNumber += 1;
                }
                //Player
                if (PlayerNumber == 1)
                {
                    _aniController.AbsoluteAniFalse();
                    _specialAni.SecretRoom1Play();
                    _specialAni.TurnSpecialFace("R", 6);
                    PlayerTransform.position = new Vector3(PlayerPoint.position.x, PlayerTransform.position.y, 0);
                    PlayerNumber += 1;
                }
                if (RunningTime >= 11.5 && PlayerNumber == 2)
                {
                    _specialAni.SpecialAniFalse();
                    _aniController.WaitAniPlay();
                    PlayerNumber += 1;
                }

                //Camera
                if (CameraNumber == 1)
                {
                    _camera.FollowSpecialTarget();
                }
                if (RunningTime >= 11.5 && CameraNumber == 1)
                {
                    CameraNumber += 1;
                }
                if (CameraNumber == 2)
                {
                    _camera.FollowPlayer();
                }

                //SE
                if (RunningTime >= 3 && SENumber == 1)
                {
                    ShadowSnakeSource1.Play();
                    SENumber += 1;
                }
                if (RunningTime >= 6.95 && SENumber == 2)
                {
                    ShadowSnakeSource3.Play();
                    SENumber += 1;
                }
                if (RunningTime >= 7.75 && SENumber == 3)
                {
                    SnakeCrashSource.Play();
                    SENumber += 1;
                }
                if (RunningTime >= 8.6 && SENumber == 4)
                {
                    SealSource.Play();
                    SENumber += 1;
                }
                if (RunningTime >= 9.6 && SENumber == 5)
                {
                    SealCompleteSource.Play();
                    SENumber += 1;
                }
                break;
            case 2:
                //Background
                if (BackgroundNumber == 1)
                {
                    BeginTimeReset();
                    BackgroundNumber += 1;
                }
                if (RunningTime >= 3 && BackgroundNumber == 2)
                {
                    ShadowSnake.SetActive(true);
                    BackgroundNumber += 1;
                }
                if (RunningTime >= 8.6 && BackgroundNumber == 3)
                {
                    _specialAni.SealSkillPower(1);
                    BackgroundNumber += 1;
                }
                if (RunningTime >= 9.6 && BackgroundNumber == 4)
                {
                    Flash.SetActive(true);
                    _specialAni.SealSkillPower(2);
                    _aniMethod.PlaySpecialAni(0);
                    BackgroundNumber += 1;
                }
                //Player
                if (PlayerNumber == 1)
                {
                    _aniController.AbsoluteAniFalse();
                    _specialAni.SecretRoom1Play();
                    _specialAni.TurnSpecialFace("R", 6);
                    PlayerTransform.position = new Vector3(PlayerPoint.position.x, PlayerTransform.position.y, 0);
                    PlayerNumber += 1;
                }
                if (RunningTime >= 12.5 && PlayerNumber == 2)
                {
                    _specialAni.SpecialAniFalse();
                    _aniController.WaitAniPlay();
                    PlayerNumber += 1;
                }

                //Camera
                if (CameraNumber == 1)
                {
                    _camera.FollowSpecialTarget();
                }
                if (RunningTime >= 12.5 && CameraNumber == 1)
                {
                    CameraNumber += 1;
                }
                if (CameraNumber == 2)
                {
                    _camera.FollowPlayer();
                }

                //SE
                if (RunningTime >= 3 && SENumber == 1)
                {
                    ShadowSnakeSource1.Play();
                    SENumber += 1;
                }
                if (RunningTime >= 6.95 && SENumber == 2)
                {
                    ShadowSnakeSource3.Play();
                    SENumber += 1;
                }
                if (RunningTime >= 7.75 && SENumber == 3)
                {
                    SnakeCrashSource.Play();
                    SENumber += 1;
                }
                if (RunningTime >= 8.6 && SENumber == 4)
                {
                    SealSource.Play();
                    SENumber += 1;
                }
                if (RunningTime >= 9.6 && SENumber == 5)
                {
                    SealCompleteSource.Play();
                    LosePowerSource.Play();
                    SENumber += 1;
                }
                break;
        }
    }

    private void BeginTimeReset()
    {
        BeginTime = Time.time;
        RunningTime = _time - BeginTime;
    }
}
