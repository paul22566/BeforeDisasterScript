using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Boss1AniController : MonoBehaviour
{
    private Transform PlayerTransform;
    private PlayerAnimationController _aniController;
    private PlayerSpecialAni _specialAni;
    private AniMethod _aniMethod;
    [SerializeField] private CameraController _camera;
    public Boss1RoomController RoomController;

    public GameObject Boss1;

    //初始入場
    [SerializeField] private GameObject OpenMouthHead;
    [SerializeField] private Animator OpenMouthHeadAni;
    [SerializeField] private GameObject Howl;
    [SerializeField] private Animator Eye;
    [SerializeField] private Animator Boss1BeginAni;
    [SerializeField] private GameObject BeginingAniHead;
    [SerializeField] private Animator WindowAni;
    [SerializeField] private GameObject Boss1WaitAni;
    [SerializeField] private GameObject BossName;

    //戰鬥結束
    //玩家開槍點(-3.69 -10.43, 12.7 -10.43, 12.1 -6.84)
    private float PlayerMoveDistance;
    private Vector3 TargetPoint = new Vector3();
    [SerializeField] private Animator HumanBossAni;
    private enum PlayerMoveType { Long, Mid, Short, Turn, UnMove};
    private PlayerMoveType _MoveType;
    [SerializeField] private Transform RBullet;
    [SerializeField] private Transform LBullet;
    [SerializeField] private Transform UpRBullet;
    [SerializeField] private Transform UpLBullet;
    private GameObject RBulletParticle;
    private GameObject LBulletParticle;
    private GameObject UpRBulletParticle;
    private GameObject UpLBulletParticle;
    private float BulletSpeed = 30;
    private Vector2 BulletSpeedRate = new Vector2();
    private ParabolaVar _parabolaVar = new ParabolaVar();

    //吸收
    public GameObject RParticle;//朝右飛
    public GameObject LParticle;//朝左飛

    public GameObject WindowSound;

    public AudioClip LowHowlSound;
    public AudioClip StareSound;
    public AudioClip TouchGroundSound;
    public AudioClip HowlSound;
    public AudioClip GunReloadSound;
    public AudioClip DashSound;
    public AudioClip JumpSound;
    public AudioClip AbsorbSound1;
    public AudioClip AbsorbSound2;
    public AudioClip GetSoulSound;

    private AudioSource LowHowlSource;
    private AudioSource StareSource;
    private AudioSource TouchGroundSource;
    private AudioSource HowlSource;
    private AudioSource GunReloadSource;
    private AudioSource DashSource;
    private AudioSource JumpSource;
    private AudioSource AbsorbSource1;
    private AudioSource AbsorbSource2;
    private AudioSource GetSoulSource;


    private float _fixedDeltaTime;

    private float _time;
    private float BeginTime;
    private float RunningTime;

    private int ResetNumber = 1;
    private int PlayerNumber = 1;
    private int BossNumber = 1;
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
            _specialAni = GameObject.Find("player").GetComponent<PlayerSpecialAni>();
        }
        if (GameObject.Find("FollowSystem") != null)
        {
            _aniMethod = GameObject.Find("FollowSystem").GetComponent<AniMethod>();
        }

        RBulletParticle = RBullet.GetChild(1).gameObject;
        LBulletParticle = LBullet.GetChild(1).gameObject;
        UpRBulletParticle = UpRBullet.GetChild(1).gameObject;
        UpLBulletParticle = UpLBullet.GetChild(1).gameObject;

        SEController.inisializeAudioSource(ref LowHowlSource, LowHowlSound, this.transform);
        SEController.inisializeAudioSource(ref StareSource, StareSound, this.transform);
        SEController.inisializeAudioSource(ref TouchGroundSource, TouchGroundSound, this.transform);
        SEController.inisializeAudioSource(ref HowlSource, HowlSound, this.transform);
        SEController.inisializeAudioSource(ref GunReloadSource, GunReloadSound, this.transform);
        SEController.inisializeAudioSource(ref DashSource, DashSound, this.transform);
        SEController.inisializeAudioSource(ref JumpSource, JumpSound, this.transform);
        SEController.inisializeAudioSource(ref AbsorbSource1, AbsorbSound1, this.transform);
        SEController.inisializeAudioSource(ref AbsorbSource2, AbsorbSound2, this.transform);
        SEController.inisializeAudioSource(ref GetSoulSource, GetSoulSound, this.transform);
    }

    private void FixedUpdate()
    {
        if (RoomController.isDoEvent == false)
        {
            return;
        }

        _fixedDeltaTime = Time.fixedDeltaTime;
        _time = Time.time;
        RunningTime = _time - BeginTime;

        switch (RoomController.Phase)
        {
            case 1:
                //Background
                if (BackgroundNumber == 1)
                {
                    BeginTimeReset();
                    _aniMethod.OpenBlackScreen();
                    _specialAni.HidePlayerUI();
                    BackgroundNumber += 1;
                }
                if (RunningTime >= 8.05 && BackgroundNumber == 2)
                {
                    BossName.SetActive(true);
                    BackgroundNumber += 1;
                }
                if (RunningTime >= 10.5 && BackgroundNumber == 3)
                {
                    _aniMethod.CloseBlackScreen();
                    _specialAni.ShowPlayerUI();
                    BackgroundNumber += 1;
                }
                //Player
                if (PlayerNumber == 1)
                {
                    _aniController.AbsoluteAniFalse();
                    _specialAni.Boss1RoomPlay(1);
                    PlayerNumber += 1;
                }
                if (RunningTime >= 4.2 && PlayerNumber == 2)
                {
                    _aniMethod.ObjectHorizontalMove(1.6f, 1.5f, _fixedDeltaTime, PlayerTransform);
                }
                if (RunningTime >= 5.7 && PlayerNumber == 2)
                {
                    PlayerNumber += 1;
                }
                if (RunningTime >= 11 && PlayerNumber == 3)
                {
                    _specialAni.SpecialAniFalse();
                    _aniController.WaitAniPlay();
                    PlayerNumber += 1;
                }
                //Boss1
                if (RunningTime >= 2 && BossNumber == 1)
                {
                    Boss1BeginAni.SetBool("Begin", true);
                    Eye.SetBool("Open", true);
                    BossNumber += 1;
                }
                if (RunningTime >= 5.6 && BossNumber == 2)
                {
                    Eye.gameObject.SetActive(false);
                    BossNumber += 1;
                }
                if (RunningTime >= 7.8 && BossNumber == 3)
                {
                    BeginingAniHead.SetActive(false);
                    OpenMouthHead.SetActive(true);
                    BossNumber += 1;
                }
                if (RunningTime >= 7.9 && BossNumber == 4)
                {
                    OpenMouthHeadAni.SetBool("Fast", true);
                    OpenMouthHeadAni.SetBool("Open", true);
                    BossNumber += 1;
                }
                if (RunningTime >= 8.05 && BossNumber == 5)
                {
                    Howl.SetActive(true);
                    BossNumber += 1;
                }
                if (RunningTime >= 9.85 && BossNumber == 6)
                {
                    OpenMouthHeadAni.SetBool("Open", false);
                    BossNumber += 1;
                }
                //Camera
                if (CameraNumber == 1)
                {
                    _camera.SpecialTarget.localPosition = PlayerTransform.localPosition;
                    CameraNumber += 1;
                }
                if (CameraNumber == 2)
                {
                    _camera.SpecialTargetHorizontalMove(-11.13f, 2, _fixedDeltaTime, _camera.MainMapFrame);
                    _camera.FollowSpecialTarget();
                }
                if (RunningTime >= 2 && CameraNumber == 2)
                {
                    CameraNumber += 1;
                }

                //SE
                if (RunningTime >= 3.75 && SENumber == 1)
                {
                    LowHowlSource.Play();
                    SENumber += 1;
                }
                if (RunningTime >= 5.5 && SENumber == 2)
                {
                    StareSource.Play();
                    SENumber += 1;
                }
                if (RunningTime >= 7.45f && SENumber == 3)
                {
                    TouchGroundSource.Play();
                    SENumber += 1;
                }
                if (RunningTime >= 8.05f && SENumber == 4)
                {
                    HowlSource.Play();
                    MusicController.PlayBGM(4);
                    MusicController.BeginFadeInBGM(5, 1);
                    SENumber += 1;
                }

                SEController.CalculateSystemSound(LowHowlSource);
                SEController.CalculateSystemSound(StareSource);
                SEController.CalculateSystemSound(TouchGroundSource);
                SEController.CalculateSystemSound(HowlSource);
                break;
            case 3:
                HumanBossAni.SetInteger("Phase", 1);
                break;
            case 4:
                //Reset
                if (ResetNumber == 1)
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
                if (BackgroundNumber == 2)
                {
                    HumanBossAni.SetInteger("Phase", 2);
                    BackgroundNumber += 1;
                }
                if (RunningTime >= 9.05 && BackgroundNumber == 3)
                {
                    WindowAni.SetBool("Open", false);
                    BackgroundNumber += 1;
                }
                if (RunningTime >= 10.5 && BackgroundNumber == 4)
                {
                    _aniMethod.CloseBlackScreen();
                    _specialAni.ShowPlayerUI();
                    BackgroundNumber += 1;
                }
                //Player
                if (PlayerNumber == 1)
                {
                    _aniController.AbsoluteAniFalse();
                    _specialAni.Boss1RoomPlay(0);
                    DecidePlayerAction();
                    PlayerNumber += 1;
                }
                RunPlayerAction();
                if (RunningTime >= 2.5 && PlayerNumber == 6)
                {
                    CalculateBulletAngle();
                    if (PlayerTransform.localPosition.y <= -8)
                    {
                        _specialAni.Boss1RoomPlay(7);
                        switch (TargetPoint.x)
                        {
                            case -3.69f:
                                RBullet.localPosition = new Vector3(PlayerTransform.localPosition.x + 0.86f, PlayerTransform.localPosition.y + 1.67f, PlayerTransform.localPosition.z);
                                break;
                            case 12.7f:
                                LBullet.localPosition = new Vector3(PlayerTransform.localPosition.x - 0.86f, PlayerTransform.localPosition.y + 1.67f, PlayerTransform.localPosition.z);
                                break;
                        }
                    }
                    if (PlayerTransform.localPosition.y > -8)
                    {
                        _specialAni.Boss1RoomPlay(8);
                        switch (TargetPoint.x)
                        {
                            case -3.69f:
                                UpRBullet.localPosition = new Vector3(PlayerTransform.localPosition.x + 1.22f, PlayerTransform.localPosition.y + 1.32f, PlayerTransform.localPosition.z);
                                break;
                            case 12.1f:
                                UpLBullet.localPosition = new Vector3(PlayerTransform.localPosition.x - 1.22f, PlayerTransform.localPosition.y + 1.32f, PlayerTransform.localPosition.z);
                                break;
                        }
                    }
                    PlayerNumber += 1;
                }
                if (RunningTime >= 3 && PlayerNumber == 7)
                {
                    if (PlayerTransform.localPosition.y <= -8)
                    {
                        switch (TargetPoint.x)
                        {
                            case -3.69f:
                                RBullet.gameObject.SetActive(true);
                                _aniMethod.ObjectSlashMove(BulletSpeedRate.x, BulletSpeedRate.y, BulletSpeed, _fixedDeltaTime, RBullet);
                                if (RBullet.localPosition.x >= 4.49f)
                                {
                                    RBullet.GetChild(0).GetComponent<Animator>().SetBool("Disappear", true);
                                    RBulletParticle.SetActive(true);
                                    PlayerNumber += 1;
                                }
                                break;
                            case 12.7f:
                                LBullet.gameObject.SetActive(true);
                                _aniMethod.ObjectSlashMove(BulletSpeedRate.x, BulletSpeedRate.y, BulletSpeed, _fixedDeltaTime, LBullet);
                                if (LBullet.localPosition.x <= 4.78f)
                                {
                                    LBullet.GetChild(0).GetComponent<Animator>().SetBool("Disappear", true);
                                    LBulletParticle.SetActive(true);
                                    PlayerNumber += 1;
                                }
                                break;
                        }
                    }
                    if (PlayerTransform.localPosition.y > -8)
                    {
                        switch (TargetPoint.x)
                        {
                            case -3.69f:
                                UpRBullet.gameObject.SetActive(true);
                                _aniMethod.ObjectSlashMove(BulletSpeedRate.x, BulletSpeedRate.y, BulletSpeed, _fixedDeltaTime, UpRBullet);
                                if (UpRBullet.localPosition.x >= 4.49f)
                                {
                                    UpRBullet.GetChild(0).GetComponent<Animator>().SetBool("Disappear", true);
                                    UpRBulletParticle.SetActive(true);
                                    PlayerNumber += 1;
                                }
                                break;
                            case 12.1f:
                                UpLBullet.gameObject.SetActive(true);
                                _aniMethod.ObjectSlashMove(BulletSpeedRate.x, BulletSpeedRate.y, BulletSpeed, _fixedDeltaTime, UpLBullet);
                                if (UpLBullet.localPosition.x <= 4.78f)
                                {
                                    UpLBullet.GetChild(0).GetComponent<Animator>().SetBool("Disappear", true);
                                    UpLBulletParticle.SetActive(true);
                                    PlayerNumber += 1;
                                }
                                break;
                        }
                    }
                }
                if (RunningTime >= 10.9 && PlayerNumber == 8)
                {
                    _aniController.WaitAniPlay();
                    _specialAni.SpecialAniFalse();
                    switch (TargetPoint.x)
                    {
                        case -3.69f:
                            _aniController.SwitchFace("R");
                            break;
                        case 12.7f:
                            _aniController.SwitchFace("L");
                            break;
                        case 12.1f:
                            _aniController.SwitchFace("L");
                            break;
                    }
                    PlayerNumber += 1;
                }
                //Camera
                if (CameraNumber == 1)
                {
                    _camera.SpecialTarget.localPosition = HumanBossAni.transform.position;
                    CameraNumber += 1;
                }
                if (RunningTime >= 3 && CameraNumber == 2)
                {
                    _camera.ChangeCameraSize(-1.5f, 6, _fixedDeltaTime);
                }
                if (RunningTime >= 4 && CameraNumber == 2)
                {
                    CameraNumber += 1;
                }
                if (RunningTime >= 9.05f && CameraNumber == 3)
                {
                    _camera.ChangeCameraSize(1, 7, _fixedDeltaTime);
                }
                if (RunningTime >= 10.05f && CameraNumber == 3)
                {
                    CameraNumber += 1;
                }
                if (RunningTime >= 3 && CameraNumber <= 4)
                {
                    _camera.FollowSpecialTarget();
                }
                //SE
                switch (_MoveType)
                {
                    case PlayerMoveType.UnMove:
                        if (RunningTime >= 1.6 && SENumber == 1)
                        {
                            GunReloadSource.Play();
                            SENumber += 1;
                        }
                        break;
                    case PlayerMoveType.Short:
                        if (RunningTime >= 1.6 && SENumber == 1)
                        {
                            GunReloadSource.Play();
                            SENumber += 1;
                        }
                        break;
                    case PlayerMoveType.Long:
                        if (RunningTime >= 0.6 && SENumber == 1)
                        {
                            GunReloadSource.Play();
                            SENumber += 1;
                        }
                        if (RunningTime >= 1.3 && SENumber == 2)
                        {
                            DashSource.Play();
                            SENumber += 1;
                        }
                        break;
                    case PlayerMoveType.Mid:
                        if (RunningTime >= 1.5 && SENumber == 1)
                        {
                            DashSource.Play();
                            SENumber += 1;
                        }
                        if (RunningTime >= 1.6 && SENumber == 2)
                        {
                            GunReloadSource.Play();
                            SENumber += 1;
                        }
                        break;
                    case PlayerMoveType.Turn:
                        if (RunningTime >= 0.6 && SENumber == 1)
                        {
                            GunReloadSource.Play();
                            SENumber += 1;
                        }
                        if (RunningTime >= 1.95 && SENumber == 2)
                        {
                            JumpSource.Play();
                            SENumber += 1;
                        }
                        break;
                }

                SEController.CalculateSystemSound(GunReloadSource);
                SEController.CalculateSystemSound(DashSource);
                SEController.CalculateSystemSound(JumpSource);
                break;
            case 5:
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
                if (RunningTime >= 6.5 && BackgroundNumber == 2)
                {
                    _aniMethod.OpenFlash();
                    BackgroundNumber += 1;
                }
                //Player
                if (PlayerNumber == 1)
                {
                    _aniController.AbsoluteAniFalse();
                    _specialAni.Boss1RoomPlay(9);
                    if (Boss1RoomController._deadInfo.FaceRight)
                    {
                        _specialAni.TurnSpecialFace("L", 7);
                    }
                    if (Boss1RoomController._deadInfo.FaceLeft)
                    {
                        _specialAni.TurnSpecialFace("R", 7);
                    }
                    PlayerNumber += 1;
                }
                if (RunningTime >= 3 && PlayerNumber == 2)
                {
                    if (Boss1RoomController._deadInfo.FaceRight)
                    {
                        Instantiate(RParticle, PlayerTransform.localPosition, Quaternion.identity);
                    }
                    if (Boss1RoomController._deadInfo.FaceLeft)
                    {
                        Instantiate(LParticle, PlayerTransform.localPosition, Quaternion.identity);
                    }
                    PlayerNumber += 1;
                }
                if (RunningTime >= 8.5 && PlayerNumber == 3)
                {
                    _aniController.WaitAniPlay();
                    _specialAni.SpecialAniFalse();
                    if (Boss1RoomController._deadInfo.FaceRight)
                    {
                        _aniController.SwitchFace("L");
                    }
                    if (Boss1RoomController._deadInfo.FaceLeft)
                    {
                        _aniController.SwitchFace("R");
                    }
                    PlayerNumber += 1;
                }

                //SE
                if (RunningTime >= 1.9f && SENumber == 1)
                {
                    AbsorbSource1.Play();
                    SENumber += 1;
                }
                if (RunningTime >= 3 && SENumber == 2)
                {
                    AbsorbSource2.Play();
                    SENumber += 1;
                }
                if (RunningTime >= 6.5 && SENumber == 3)
                {
                    GetSoulSource.Play();
                    SENumber += 1;
                }

                SEController.CalculateSystemSound(AbsorbSource1);
                SEController.CalculateSystemSound(AbsorbSource2);
                SEController.CalculateSystemSound(GetSoulSource);
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
        CameraNumber = 1;
        SENumber = 1;
    }

    public void WindowOpen()
    {
        WindowAni.SetBool("Open", true);
        Instantiate(WindowSound);
    }

    private void DecidePlayerAction()
    {
        if (PlayerTransform.localPosition.y <= -8)
        {
            if (Mathf.Abs(PlayerController.PlayerPlaceX + 3.69f) > Mathf.Abs(PlayerController.PlayerPlaceX - 12.7f))
            {
                //使用12.7
                TargetPoint = new Vector3(12.7f, -10.43f, 0);
                PlayerMoveDistance = PlayerController.PlayerPlaceX - 12.7f;
                
                //決定Type
                if (Mathf.Abs(PlayerMoveDistance) <= 1)
                {
                    _MoveType = PlayerMoveType.UnMove;
                }
                else
                {
                    if (PlayerMoveDistance < 0)
                    {
                        _MoveType = PlayerMoveType.Turn;
                    }
                    if (PlayerMoveDistance > 0)
                    {
                        _MoveType = PlayerMoveType.Long;
                        if (PlayerMoveDistance <= 8)
                        {
                            _MoveType = PlayerMoveType.Mid;
                        }
                        if (PlayerMoveDistance <= 4)
                        {
                            _MoveType = PlayerMoveType.Short;
                        }
                    }
                }
            }
            if (Mathf.Abs(PlayerTransform.localPosition.x + 3.69f) <= Mathf.Abs(PlayerTransform.localPosition.x - 12.7f))
            {
                //使用-3.69
                TargetPoint = new Vector3(-3.69f, -10.43f, 0);
                PlayerMoveDistance = PlayerTransform.localPosition.x + 3.69f;

                //決定Type
                if (Mathf.Abs(PlayerMoveDistance) <= 1)
                {
                    _MoveType = PlayerMoveType.UnMove;
                }
                else
                {
                    if (PlayerMoveDistance > 0)
                    {
                        _MoveType = PlayerMoveType.Turn;
                    }
                    if (PlayerMoveDistance < 0)
                    {
                        _MoveType = PlayerMoveType.Long;
                        if (PlayerMoveDistance >= -8)
                        {
                            _MoveType = PlayerMoveType.Mid;
                        }
                        if (PlayerMoveDistance >= -4)
                        {
                            _MoveType = PlayerMoveType.Short;
                        }
                    }
                }
            }
        }
        if (PlayerTransform.localPosition.y > -8)
        {
            if (Mathf.Abs(PlayerTransform.localPosition.x + 3.69f) > Mathf.Abs(PlayerTransform.localPosition.x - 12.1f))
            {
                //使用12.1
                TargetPoint = new Vector3(12.1f, -6.84f, 0);
                PlayerMoveDistance = PlayerTransform.localPosition.x - 12.1f;

                //決定Type
                if (Mathf.Abs(PlayerMoveDistance) <= 1)
                {
                    _MoveType = PlayerMoveType.UnMove;
                }
                else
                {
                    if (PlayerMoveDistance < 0)
                    {
                        _MoveType = PlayerMoveType.Turn;
                    }
                    if (PlayerMoveDistance > 0)
                    {
                        _MoveType = PlayerMoveType.Short;
                    }
                }
            }
            if (Mathf.Abs(PlayerTransform.localPosition.x + 3.69f) <= Mathf.Abs(PlayerTransform.localPosition.x - 12.1f))
            {
                //使用-3.69
                TargetPoint = new Vector3(-3.69f, -6.84f, 0);
                PlayerMoveDistance = PlayerTransform.localPosition.x + 3.69f;

                //決定Type
                if (Mathf.Abs(PlayerMoveDistance) <= 1)
                {
                    _MoveType = PlayerMoveType.UnMove;
                }
                else
                {
                    if (PlayerMoveDistance > 0)
                    {
                        _MoveType = PlayerMoveType.Turn;
                    }
                    if (PlayerMoveDistance < 0)
                    {
                        _MoveType = PlayerMoveType.Short;
                    }
                }
            }
        }
        
        switch (_MoveType)
        {
            case PlayerMoveType.UnMove:
                BeginTime -= 1.3f;
                break;
            case PlayerMoveType.Turn:
                BeginTime -= 0.3f;
                break;
            case PlayerMoveType.Short:
                BeginTime -= 1.3f;
                break;
            case PlayerMoveType.Mid:
                BeginTime -= 1.3f;
                break;
            case PlayerMoveType.Long:
                BeginTime -= 0.3f;
                break;
        }
    }

    private void RunPlayerAction()
    {
        switch (TargetPoint.x)
        {
            case -3.69f:
                if (PlayerNumber == 2)
                {
                    _specialAni.TurnSpecialFace("R", 7);
                    PlayerNumber += 1;
                }
                break;
            case 12.7f:
                if(PlayerNumber == 2)
                {
                    _specialAni.TurnSpecialFace("L", 7);
                    PlayerNumber += 1;
                }
                break;
            case 12.1f:
                if (PlayerNumber == 2)
                {
                    _specialAni.TurnSpecialFace("L", 7);
                    PlayerNumber += 1;
                }
                break;
        }
        
        switch (_MoveType)
        {
            case PlayerMoveType.UnMove:
                if (PlayerNumber == 3)
                {
                    _specialAni.Boss1RoomPlay(2);
                    PlayerNumber = 6;
                }
                break;
            case PlayerMoveType.Short:
                if (PlayerNumber == 3)
                {
                    _specialAni.Boss1RoomPlay(4);
                    _aniMethod.ObjectHorizontalMove(-PlayerMoveDistance, 1, _fixedDeltaTime, PlayerTransform);
                }
                if (RunningTime >= 2.3 && PlayerNumber == 3)
                {
                    _specialAni.Boss1RoomPlay(6);
                    PlayerNumber = 6;
                }
                break;
            case PlayerMoveType.Mid:
                if (PlayerNumber == 3)
                {
                    _specialAni.Boss1RoomPlay(5);
                    _aniMethod.ObjectHorizontalMove(-PlayerMoveDistance, 1, _fixedDeltaTime, PlayerTransform);
                }
                if (RunningTime >= 2.3 && PlayerNumber == 3)
                {
                    _specialAni.Boss1RoomPlay(6);
                    PlayerNumber = 6;
                }
                break;
            case PlayerMoveType.Long:
                if (PlayerNumber == 3)
                {
                    _specialAni.Boss1RoomPlay(4);
                    switch (TargetPoint.x)
                    {
                        case -3.69f:
                            _aniMethod.ObjectHorizontalMove(-PlayerMoveDistance - 8, 1, _fixedDeltaTime, PlayerTransform);
                            break;
                        case 12.7f:
                            _aniMethod.ObjectHorizontalMove(-PlayerMoveDistance + 8, 1, _fixedDeltaTime, PlayerTransform);
                            break;
                    }
                }
                if (RunningTime >= 1.3 && PlayerNumber == 3)
                {
                    PlayerNumber += 1;
                }
                if (RunningTime >= 1.3 && PlayerNumber == 4)
                {
                    _specialAni.Boss1RoomPlay(5);
                    switch (TargetPoint.x)
                    {
                        case -3.69f:
                            _aniMethod.ObjectHorizontalMove(8, 1, _fixedDeltaTime, PlayerTransform);
                            break;
                        case 12.7f:
                            _aniMethod.ObjectHorizontalMove(-8, 1, _fixedDeltaTime, PlayerTransform);
                            break;
                    }
                }
                if (RunningTime >= 2.3 && PlayerNumber == 4)
                {
                    _specialAni.Boss1RoomPlay(6);
                    PlayerNumber = 6;
                }
                break;
            case PlayerMoveType.Turn:
                if (PlayerNumber == 3)
                {
                    _specialAni.Boss1RoomPlay(3);
                    CalculateParabola();
                    PlayerNumber += 1;
                }
                if (RunningTime >= 1.45 && PlayerNumber == 4)
                {
                    _aniMethod.ObjectParabolaMove(_parabolaVar, Mathf.Abs(PlayerMoveDistance), 0.5f, _fixedDeltaTime, PlayerTransform);
                }
                if (RunningTime >= 1.95 && PlayerNumber == 4)
                {
                    PlayerNumber += 1;
                }
                if (RunningTime >= 2.3&& PlayerNumber == 5)
                {
                    _specialAni.Boss1RoomPlay(6);
                    PlayerNumber = 6;
                }
                break;
        }
    }

    private void CalculateBulletAngle()
    {
        if (PlayerTransform.localPosition.y <= -8)
        {
            switch (TargetPoint.x)
            {
                case -3.69f:
                    BulletSpeedRate = AngleCaculate.CaculateSpeedRate("R", RBullet.eulerAngles.z);
                    break;
                case 12.7f:
                    BulletSpeedRate = AngleCaculate.CaculateSpeedRate("R", LBullet.eulerAngles.z);
                    break;
            }
        }
        if (PlayerTransform.localPosition.y > -8)
        {
            switch (TargetPoint.x)
            {
                case -3.69f:
                    BulletSpeedRate = AngleCaculate.CaculateSpeedRate("R", UpRBullet.eulerAngles.z);
                    break;
                case 12.1f:
                    BulletSpeedRate = AngleCaculate.CaculateSpeedRate("R", UpLBullet.eulerAngles.z);
                    break;
            }
        }
    }

    private void CalculateParabola()
    {
        _parabolaVar.OtherPoint = PlayerTransform.localPosition;
        switch (TargetPoint.x)
        {
            case -3.69f:
                _parabolaVar.MiddlePoint = new Vector3((PlayerTransform.localPosition.x - 3.69f) / 2, PlayerTransform.localPosition.y + 1, 0);
                _parabolaVar.HorizontalDirection = "Left";
                break;
            case 12.7f:
                _parabolaVar.MiddlePoint = new Vector3((PlayerTransform.localPosition.x + 12.7f) / 2, PlayerTransform.localPosition.y + 1, 0);
                _parabolaVar.HorizontalDirection = "Right";
                break;
            case 12.1f:
                _parabolaVar.MiddlePoint = new Vector3((PlayerTransform.localPosition.x + 12.1f) / 2, PlayerTransform.localPosition.y + 1, 0);
                _parabolaVar.HorizontalDirection = "Right";
                break;
        }
        _parabolaVar.ParabolaNowX = PlayerTransform.localPosition.x;
        _parabolaVar.VerticalDirection = "Down";
        _parabolaVar.ParabolaConstant = Parabola.CalculateParabolaConstant(_parabolaVar);
    }
}
