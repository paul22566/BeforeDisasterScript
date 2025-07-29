using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Net;
using UnityEngine;

public class RestRooomAni : MonoBehaviour
{
    private Transform PlayerTransform;
    private OldPlayerAnimationController _aniController;
    private PlayerSpecialAni _specialAni;
    private AniMethod _aniMethod;
    private RestRoomController _RoomController;
    public CameraController _camera;

    private int EventNumber;//1普通首次進入，2正面再次進入，3背面首次進入，4背面再次進入

    public GameObject DeadMan1;
    public GameObject DeadMan2;
    private Animator DeadManAni1;
    private Animator DeadManAni2;
    public GameObject SmallCaptain;

    public Transform SwordManTransform;
    public GameObject SwordManBeginAni1;
    public GameObject SwordManBeginAni2;
    private Animator ManBeginAni1;
    private Animator ManBeginAni2;
    public GameObject SwordManSlowWalkAni1;
    public GameObject SwordManSlowWalkAni2;
    public SideDoor LeftDoor;
    public SideDoor RightDoor;

    private float SwordManMoveDistance;

    public AudioClip WalkSound;
    public AudioClip StringAtkSound;
    public AudioClip PullSwordSound;
    public AudioClip RotateSwordSound;
    public AudioClip SwordSound;
    public AudioClip Sword2Sound;

    public AudioClip FallSound;

    private AudioSource WalkSource;
    private AudioSource StringAtkSource;
    private AudioSource PullSwordSource;
    private AudioSource RotateSwordSource;
    private AudioSource SwordSource;
    private AudioSource Sword2Source;

    private AudioSource FallSource;
    private AudioSource Walk2Source;
    private AudioSource Walk3Source;

    private float WalkSoundRate = 0.5f;
    private float Walk2SoundRate = 0.2f;
    private float FallSoundRate = 0.5f;

    private float _fixedDeltaTime;

    private float _time;
    private float BeginTime;
    private float RunningTime;

    private int PlayerNumber = 1;
    private int DeadMan1Number = 1;
    private int DeadMan2Number = 1;
    private int SwordMan1Number = 1;
    private int SwordMan2Number = 1;
    private int SmallCaptainNumber = 1;
    private int BackgroundNumber = 1;
    private int CameraNumber = 1;
    private int SENumber = 1;

    // Start is called before the first frame update
    void Start()
    {
        if (GameEvent.PassRestRoom)
        {
            Destroy(DeadMan1);
            Destroy(DeadMan2);
            Destroy(SmallCaptain);
            return;
        }

        _RoomController = this.GetComponent<RestRoomController>();
        EventNumber = _RoomController.EventNumber;

        if (GameEvent.GoRestRoom || GameEvent.PassBoss1)
        {
            Destroy(DeadMan1);
            Destroy(DeadMan2);
            Destroy(SmallCaptain);
            SwordManBeginAni1.SetActive(false);
            SwordManBeginAni2.SetActive(false);
            SwordManSlowWalkAni1.SetActive(true);
            SwordManSlowWalkAni2.SetActive(true);
        }

        if (GameObject.Find("player") != null)
        {
            PlayerTransform = GameObject.Find("player").transform;
            _aniController = GameObject.Find("player").GetComponent<OldPlayerAnimationController>();
            _specialAni = GameObject.Find("player").GetComponent<PlayerSpecialAni>();
        }
        if (GameObject.Find("FollowSystem") != null)
        {
            _aniMethod = GameObject.Find("FollowSystem").GetComponent<AniMethod>();
        }

        DeadManAni1 = DeadMan1.transform.GetChild(0).GetComponent<Animator>();
        DeadManAni2 = DeadMan2.transform.GetChild(0).GetComponent<Animator>();
        ManBeginAni1 = SwordManBeginAni1.transform.GetChild(0).GetComponent<Animator>();
        ManBeginAni2 = SwordManBeginAni2.transform.GetChild(0).GetComponent<Animator>();

        SEController.inisializeAudioSource(ref WalkSource, WalkSound, this.transform);
        SEController.inisializeAudioSource(ref StringAtkSource, StringAtkSound, this.transform);
        SEController.inisializeAudioSource(ref PullSwordSource, PullSwordSound, this.transform);
        SEController.inisializeAudioSource(ref RotateSwordSource, RotateSwordSound, this.transform);
        SEController.inisializeAudioSource(ref SwordSource, SwordSound, this.transform);
        SEController.inisializeAudioSource(ref Sword2Source, Sword2Sound, this.transform);
        SEController.inisializeAudioSource(ref FallSource, FallSound, this.transform);
        SEController.inisializeAudioSource(ref Walk2Source, WalkSound, this.transform);
        SEController.inisializeAudioSource(ref Walk3Source, WalkSound, this.transform);
        WalkSource.loop = true;
        Walk2Source.loop = true;
        Walk3Source.loop = true;
    }

    private void FixedUpdate()
    {
        _fixedDeltaTime = Time.fixedDeltaTime;
        _time = Time.time;
        RunningTime = _time - BeginTime;

        if (!GameEvent.isAniPlay || GameEvent.PassRestRoom || GameObject.Find("player") == null)
        {
            return;
        }

        switch (EventNumber)
        {
            case 1:
                //Background
                if (BackgroundNumber == 1)
                {
                    BeginTimeReset();
                    _aniMethod.OpenBlackScreen();
                    BackgroundNumber += 1;
                }
                if (RunningTime >= 5 && BackgroundNumber == 2)
                {
                    RightDoor.CloseDoor();
                    LeftDoor.CloseDoor();
                    BackgroundNumber += 1;
                }
                if (RunningTime >= 6.8 && BackgroundNumber == 3)
                {
                    _aniMethod.CloseBlackScreen();
                }
                //Player
                if (PlayerNumber == 1)
                {
                    _aniController.SwitchFace("R");
                    _aniController.AbsoluteAniFalse();
                    _specialAni.GoInRestRoomPlay();
                    PlayerNumber += 1;
                }
                if (RunningTime >= 2.25f && RunningTime < 2.55f && PlayerNumber == 2)
                {
                    _aniMethod.ObjectHorizontalMove(6.72f, 0.3f, _fixedDeltaTime, PlayerTransform);
                }
                else if (RunningTime >= 2.25f && PlayerNumber == 2)
                {
                    PlayerNumber += 1;
                }
                if (RunningTime >= 6 && PlayerNumber == 3)
                {
                    _specialAni.SpecialAniFalse();
                    _aniController.WaitAniPlay();
                    PlayerNumber += 1;
                }
                //SwordMan1
                if (RunningTime < 1.15f && SwordMan1Number == 1)
                {
                    _aniMethod.ObjectHorizontalMove(-0.4f, 0.33f, _fixedDeltaTime, SwordManTransform);
                }
                else if (SwordMan1Number == 1)
                {
                    SwordMan1Number += 1;
                }
                if (RunningTime < 2.3f && SwordMan1Number == 2)
                {
                    _aniMethod.TurnFace(SwordManBeginAni1.transform.GetChild(0), 0.28f);
                    _aniMethod.ObjectHorizontalMove(0.4f, 0.33f, _fixedDeltaTime, SwordManTransform);
                }
                else if (SwordMan1Number == 2)
                {
                    WalkSource.Stop();
                    SwordMan1Number += 1;
                }
                if (SwordMan1Number == 3)
                {
                    ManBeginAni1.SetBool("Prepare", true);
                    _aniMethod.TurnFace(SwordManBeginAni1.transform.GetChild(0), -0.28f);
                    SwordMan1Number += 1;
                }
                if (RunningTime >= 2.3 && RunningTime < 2.8 && SwordMan1Number == 4)
                {
                    ManBeginAni1.SetBool("Prepare", true);
                    ManBeginAni1.SetInteger("Status", 1);
                    _aniMethod.ObjectHorizontalMove(0.44f, 0.5f, _fixedDeltaTime, SwordManTransform);
                }
                else if (RunningTime >= 2.8 && SwordMan1Number == 4)
                {
                    SwordMan1Number += 1;
                }

                //SwordMan2
                if (RunningTime >= 2.3 && SwordMan2Number == 1)
                {
                    ManBeginAni2.SetBool("Prepare", true);
                    ManBeginAni2.SetInteger("Status", 1);
                    SwordMan2Number += 1;
                }
                //DeadMan1
                if (RunningTime >= 2.3 && RunningTime < 2.55 && DeadMan1Number == 1)
                {
                    DeadManAni1.SetInteger("Status", 1);
                    _aniMethod.ObjectSlashMove(Slash.SlashCalculate(4.98f, 0.18f, "R", "U"), 0.25f, _fixedDeltaTime, DeadMan1.transform);
                }
                else if (RunningTime >= 2.55 && DeadMan1Number == 1)
                {
                    DeadMan1Number += 1;
                }
                if (RunningTime >= 4.05 && RunningTime < 4.15 && DeadMan1Number == 2)
                {
                    _aniMethod.ObjectVerticalMove(-0.15f, 0.1f, _fixedDeltaTime, DeadMan1.transform);
                }
                else if (RunningTime >= 4.15 && DeadMan1Number == 2)
                {
                    DeadManAni1.SetBool("Die", true);
                    DeadMan1Number += 1;
                }
                //DeadMan2
                if (RunningTime >= 2.35 && RunningTime < 2.55 && DeadMan2Number == 1)
                {
                    DeadManAni2.SetInteger("Status", 1);
                    _aniMethod.ObjectSlashMove(Slash.SlashCalculate(3.98f, 0.35f, "R", "U"), 0.2f, _fixedDeltaTime, DeadMan2.transform);
                }
                else if (RunningTime >= 2.55 && DeadMan2Number == 1)
                {
                    DeadMan2Number += 1;
                }
                if (RunningTime >= 4.15 && RunningTime < 4.35 && DeadMan2Number == 2)
                {
                    _aniMethod.ObjectVerticalMove(-0.34f, 0.2f, _fixedDeltaTime, DeadMan2.transform);
                }
                else if (RunningTime >= 4.35 && DeadMan2Number == 2)
                {
                    DeadManAni2.SetBool("Die", true);
                    DeadMan2Number += 1;
                }

                //SE
                if (SENumber == 1)
                {
                    WalkSource.Play();
                    SENumber += 1;
                }
                /*else if (SwordMan1Number == 2)
                {
                    WalkSource.Stop();
                    SwordMan1Number += 1;
                }*/
                if (SENumber == 2 && RunningTime >= 2.25f)
                {
                    StringAtkSource.Play();
                    SENumber += 1;
                }
                if (SENumber == 3 && RunningTime >= 4)
                {
                    PullSwordSource.Play();
                    SENumber += 1;
                }
                if (SENumber == 4 && RunningTime >= 4.51f)
                {
                    RotateSwordSource.Play();
                    SENumber += 1;
                }
                if (SENumber == 5 && RunningTime >= 4.7f)
                {
                    SwordSource.Play();
                    SENumber += 1;
                }
                if (SENumber == 6 && RunningTime >= 5.95f)
                {
                    Sword2Source.Play();
                    SENumber += 1;
                }

                SEController.CalculateSystemSound(WalkSource, WalkSoundRate);
                SEController.CalculateSystemSound(StringAtkSource);
                SEController.CalculateSystemSound(PullSwordSource);
                SEController.CalculateSystemSound(RotateSwordSource);
                SEController.CalculateSystemSound(SwordSource);
                SEController.CalculateSystemSound(Sword2Source);
                break;
            case 2:
                //Background
                if (BackgroundNumber == 1)
                {
                    BeginTimeReset();
                    BackgroundNumber += 1;
                }
                if (RunningTime >= 1 && BackgroundNumber == 2)
                {
                    LeftDoor.CloseDoor();
                    RightDoor.CloseDoor();
                    BackgroundNumber += 1;
                }
                //Player
                if (PlayerNumber == 1 && RunningTime < 1)
                {
                    _aniController.WalkAniPlay();
                    _aniController.SwitchFace("R");
                    _aniMethod.ObjectHorizontalMove(5, 1, _fixedDeltaTime, PlayerTransform);
                }
                else if (PlayerNumber == 1 && RunningTime >= 1)
                {
                    PlayerNumber += 1;
                }
                if (PlayerNumber == 2)
                {
                    _aniController.WaitAniPlay();
                    PlayerNumber += 1;
                }
                //SwordMan1
                if (SwordMan1Number == 1)
                {
                    _aniMethod.TurnFace(SwordManSlowWalkAni1.transform.GetChild(0), -0.28f);
                    SwordMan1Number += 1;
                }
                //SwordMan2
                if (SwordMan1Number == 1)
                {
                    _aniMethod.TurnFace(SwordManSlowWalkAni1.transform.GetChild(0), -0.28f);
                    SwordMan2Number += 1;
                }
                break;
            case 3:
                //Background
                if (BackgroundNumber == 1)
                {
                    BeginTimeReset();
                    _aniMethod.OpenBlackScreen();
                    BackgroundNumber += 1;
                }
                if (BackgroundNumber == 2 && RunningTime >= 18.5f)
                {
                    LeftDoor.CloseDoor();
                    RightDoor.CloseDoor();
                    _aniMethod.CloseBlackScreen();
                    BackgroundNumber += 1;
                }
                //Player
                if (PlayerNumber == 1)
                {
                    _aniController.SwitchFace("L");
                    _aniController.AbsoluteAniFalse();
                    _aniController.WalkAniPlay();
                    PlayerNumber += 1;
                }
                if (PlayerNumber == 2 && RunningTime >= 13.56f)
                {
                    _aniMethod.ObjectHorizontalMove(-3.5f, 0.5f, _fixedDeltaTime, PlayerTransform);
                }
                if (PlayerNumber == 2 && RunningTime >= 14.06f)
                {
                    _aniController.WaitAniPlay();
                    PlayerNumber += 1;
                }
                //SmallCaptain
                if (SmallCaptainNumber == 1 && RunningTime >= 2.6f)
                {
                    SmallCaptain.SetActive(true);
                    SmallCaptainNumber += 1;
                }
                if (SmallCaptainNumber == 2 && RunningTime >= 11)
                {
                    SmallCaptain.SetActive(false);
                    SmallCaptainNumber += 1;
                }
                //SwordMan1
                if (RunningTime < 1.15f && SwordMan1Number == 1)
                {
                    _aniMethod.ObjectHorizontalMove(-0.4f, 0.33f, _fixedDeltaTime, SwordManTransform);
                }
                else if (SwordMan1Number == 1)
                {
                    SwordMan1Number += 1;
                }
                if(RunningTime >= 2 && SwordMan1Number == 2)
                {
                    ManBeginAni1.SetBool("Prepare", true);
                    ManBeginAni1.SetInteger("Status", 2);
                    SwordMan1Number += 1;
                }
                if (RunningTime < 2.9f && SwordMan1Number <= 3 && SwordMan1Number >= 2)
                {
                    _aniMethod.TurnFace(SwordManBeginAni1.transform.GetChild(0), 0.28f);
                    _aniMethod.ObjectHorizontalMove(0.4f, 0.33f, _fixedDeltaTime, SwordManTransform);
                }
                else if (SwordMan1Number <= 3 && SwordMan1Number >= 2)
                {
                    SwordManMoveDistance =  _aniMethod.CalculateDistance(SwordManTransform, SmallCaptain.transform) - 1.392f;
                    SwordMan1Number += 1;
                }
                if (RunningTime >= 7.35f && SwordMan1Number == 4)
                {
                    _aniMethod.ObjectHorizontalMove(SwordManMoveDistance, 1.16f, _fixedDeltaTime, SwordManTransform);
                }
                if (RunningTime >= 8.51f && SwordMan1Number == 4)
                {
                    SwordMan1Number += 1;
                }
                if (RunningTime >= 12.6f && SwordMan1Number == 5)
                {
                    _aniMethod.ObjectHorizontalMove(0.4f, 0.33f, _fixedDeltaTime, SwordManTransform);
                }
                if (RunningTime >= 14.5f && SwordMan1Number == 5)
                {
                    SwordMan1Number += 1;
                }
                //SwordMan2
                if (RunningTime >= 2.54f && SwordMan2Number == 1)
                {
                    ManBeginAni2.SetInteger("Status", 2);
                    ManBeginAni2.SetBool("Prepare", true);
                    SwordMan2Number += 1;
                }
                //DeadMan1
                if (RunningTime >= 3 && DeadMan1Number == 1)
                {
                    DeadManAni1.SetInteger("Status", 2);
                    DeadMan1Number += 1;
                }
                if (RunningTime >= 5.9f && DeadMan1Number == 2)
                {
                    _aniMethod.ObjectHorizontalMove(-1.5f, 1, _fixedDeltaTime, DeadMan1.transform);
                }
                if (RunningTime>=8.9f && DeadMan1Number == 2)
                {
                    DeadMan1.SetActive(false);
                    Walk2Source.Stop();
                    DeadMan1Number += 1;
                }
                //DeadMan2
                if (DeadMan2Number == 1)
                {
                    DeadManAni2.SetInteger("Status", 2);
                    DeadMan2Number += 1;
                }
                if (RunningTime >= 5.95f && DeadMan2Number == 2)
                {
                    _aniMethod.ObjectHorizontalMove(-1.5f, 1, _fixedDeltaTime, DeadMan2.transform);
                }
                if (RunningTime >= 8.95f && DeadMan2Number == 2)
                {
                    DeadMan2.SetActive(false);
                    Walk3Source.Stop();
                    DeadMan2Number += 1;
                }
                //Camera
                if (CameraNumber == 1)
                {
                    _camera.FollowSpecialTarget();
                }
                if (RunningTime >= 14 && CameraNumber == 1)
                {
                    _aniMethod.ObjectHorizontalMove(11, 0.5f, _fixedDeltaTime, _camera.SpecialTarget);
                }
                if (RunningTime >= 14.5f && CameraNumber == 1)
                {
                    CameraNumber += 1;
                }
                if (CameraNumber == 2)
                {
                    _camera.FollowPlayer();
                }

                //SE
                if (SENumber == 1)
                {
                    WalkSource.Play();
                    SENumber += 1;
                }
                if (SENumber == 2 && RunningTime >= 2.9f)
                {
                    WalkSource.Stop();
                    SENumber += 1;
                }
                if (SENumber == 3 && RunningTime >= 2.95f)
                {
                    FallSource.Play();
                    SENumber += 1;
                }
                if (SENumber == 4 && RunningTime >= 6f)
                {
                    Walk2Source.Play();
                    SENumber += 1;
                }
                if (SENumber == 5 && RunningTime >= 6.05f)
                {
                    Walk3Source.Play();
                    SENumber += 1;
                }
                if (SENumber == 6 && RunningTime >= 7.35f)
                {
                    WalkSource.Play();
                    SENumber += 1;
                }
                if (SENumber == 7 && RunningTime >= 8.51f)
                {
                    WalkSource.Stop();
                    SENumber += 1;
                }
                /*if (RunningTime>=8.9f && DeadMan1Number == 2)
                {
                    Walk2Source.Stop();
                }
                if (RunningTime >= 8.95f && DeadMan2Number == 2)
                {
                    Walk3Source.Stop();
                }*/
                if (SENumber == 8 && RunningTime >= 12.6f)
                {
                    WalkSource.Play();
                    SENumber += 1;
                }
                if (SENumber == 9 && RunningTime >= 14.5f)
                {
                    WalkSource.Stop();
                    SENumber += 1;
                }
                if (SENumber == 10 && RunningTime >= 16.85f)
                {
                    SwordSource.Play();
                    SENumber += 1;
                }
                SEController.CalculateSystemSound(WalkSource, WalkSoundRate);
                SEController.CalculateSystemSound(Walk2Source, Walk2SoundRate);
                SEController.CalculateSystemSound(Walk3Source, Walk2SoundRate);
                SEController.CalculateSystemSound(FallSource, FallSoundRate);
                SEController.CalculateSystemSound(SwordSource);
                break;
            case 4:
                //Background
                if (BackgroundNumber == 1)
                {
                    BeginTimeReset();
                    BackgroundNumber += 1;
                }
                if (RunningTime >= 1 && BackgroundNumber == 2)
                {
                    LeftDoor.CloseDoor();
                    RightDoor.CloseDoor();
                    BackgroundNumber += 1;
                }
                //Player
                if (PlayerNumber == 1 && RunningTime < 1)
                {
                    _aniController.WalkAniPlay();
                    _aniController.SwitchFace("L");
                    _aniMethod.ObjectHorizontalMove(-5, 1, _fixedDeltaTime, PlayerTransform);
                }
                else if (PlayerNumber == 1 && RunningTime >= 1)
                {
                    PlayerNumber += 1;
                }
                if (PlayerNumber == 2)
                {
                    _aniController.WaitAniPlay();
                    PlayerNumber += 1;
                }
                //SwordMan1
                if (SwordMan1Number == 1)
                {
                    _aniMethod.TurnFace(SwordManSlowWalkAni1.transform.GetChild(0), 0.28f);
                    SwordMan1Number += 1;
                }
                //SwordMan2
                if (SwordMan1Number == 1)
                {
                    _aniMethod.TurnFace(SwordManSlowWalkAni1.transform.GetChild(0), 0.28f);
                    SwordMan2Number += 1;
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
