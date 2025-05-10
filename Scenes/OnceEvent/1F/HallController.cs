using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HallController : MonoBehaviour
{
    [HideInInspector] public bool isDoEvent;
    [HideInInspector] public int AniPhase;//1:進房間 2:大巨怪登場 3:大巨怪退場1  4:大巨怪退場2  5:大巨怪偷襲
    private Transform PlayerTransform;
    public GameObject Guardian;
    private PlayerData _PlayerData;
    private Portal _portal;
    private FadeOutUI _FadeOutUI;

    [SerializeField] private GameObject Portal1;
    [SerializeField] private GameObject Portal2;
    [SerializeField] private GameObject HPUI;
    [SerializeField] private GameObject Monster1;
    [SerializeField] private GameObject Monster2;
    [SerializeField] private GameObject Monster3;

    private MapFrame LeftHallwayFrame = new MapFrame();
    private MapFrame RightHallwayFrame = new MapFrame();

    private bool FirstTrigger;

    //進房間白光
    [SerializeField] private Image WhiteScreen;
    [SerializeField] private Transform DoorPoint;
    [SerializeField] private TouchTrigger WhiteLightTrigger;
    [SerializeField] private GameObject WhiteFadeIn;
    private bool PassDoorWhiteLight;
    private float DistanceWithPlayerAndDoor;
    //關門及門衛
    [SerializeField] private SideDoor LeftDoor;
    [SerializeField] private SideDoor RightDoor;
    [SerializeField] private TouchTrigger RightDoorTrigger;
    [SerializeField] private TouchTrigger FirstStepTrigger;
    [SerializeField] private TouchTrigger SecondStepTrigger;
    [SerializeField] private TouchTrigger GuardianTrigger;
    private bool GuardianAppear;

    //門衛逃跑
    [HideInInspector] public bool GuardianFaceRight;
    [HideInInspector] public bool GuardianFaceLeft;
    private bool GuardianDie;
    [SerializeField] private GameObject RGuardianEscape;
    [SerializeField] private GameObject LGuardianEscape;

    //走廊
    [SerializeField] private TouchTrigger HallwayTrigger;
    [SerializeField] private TouchTrigger HallTrigger;
    [SerializeField] private TouchTrigger AtkTrigger;
    private int MapFrameStatus = 1;//1 Hall 2 Hallway
    private bool MapFrameRecordSet;
    private bool MapFrameChangeComplete;
    private bool GuardianAtk;

    [SerializeField] private CameraController _camera;

    private float AniTimer;
    private float Ani1TimerSet = 1.5f;
    private float Ani2TimerSet = 16.5f;
    private float Ani3TimerSet = 3.3f;
    private float Ani4TimerSet = 5.5f;
    private float Ani5TimerSet = 5;
    private float _deltaTime;

    public AudioClip FootStepSound;
    public AudioClip SmallHowlSound;
    public AudioClip SuddenStopSound;

    private AudioSource FootStepSource;
    private AudioSource SmallHowlSource;
    private AudioSource SuddenStopSource;

    private void Awake()
    {
        if (GameEvent.PassHall)
        {
            return;
        }

        _camera.UseDefaultMapFrame = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (GameEvent.PassHall)
        {
            Destroy(HPUI);
            Destroy(LeftDoor.gameObject);
            Destroy(RightDoor.gameObject);
            return;
        }
        if (GameObject.Find("FollowSystem") != null)
        {
            _PlayerData = GameObject.Find("FollowSystem").GetComponent<PlayerData>();
        }
        if (GameObject.Find("player") != null)
        {
            PlayerTransform = GameObject.Find("player").transform;
            GameObject g = GameObject.Find("0") as GameObject;
            if (g != null)
            {
                PlayerTransform.position = g.transform.position;
            }
        }
        if (GameObject.FindGameObjectWithTag("UI") != null)
        {
            Transform UICanvas = GameObject.FindGameObjectWithTag("UI").transform;

            _FadeOutUI = IdentifyID.FindObject(UICanvas, UIID.FadeOut).GetComponent<FadeOutUI>();
        }

        _portal = this.GetComponent<Portal>();

        Destroy(Monster1);
        Destroy(Monster2);
        Destroy(Monster3);
        Portal1.SetActive(false);
        Portal2.SetActive(false);

        _camera.InitializeMapFrame(LeftHallwayFrame, -37.1f, 2.86f, 35.35f, 20.57f);
        _camera.InitializeMapFrame(RightHallwayFrame, 41.86f, 2.86f, 35.93f, 20.57f);

        _camera.ImmediatelyFrameSet(LeftHallwayFrame);
        //左側 -37.1f, 2.86f, 35.35f, 20.57f
        //中間 2.24f, 2.86f, 43.25f, 20.57f
        //右側 41.86f, 2.86f, 35.93f, 20.57f

        SEController.inisializeAudioSource(ref FootStepSource, FootStepSound, this.transform);
        SEController.inisializeAudioSource(ref SmallHowlSource, SmallHowlSound, this.transform);
        SEController.inisializeAudioSource(ref SuddenStopSource, SuddenStopSound, this.transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameEvent.PassHall)
        {
            return;
        }

        //死亡離場
        if (PlayerController.isDie)
        {
            GameEvent.PassHall = true;
            _FadeOutUI._fadeOutEnd += PlayAni;
            _PlayerData.CommonSave();
        }

        if (_camera.ShouldReCalculateValidLine)
        {
            _camera.CalculateValidLine(LeftHallwayFrame);
            _camera.CalculateValidLine(RightHallwayFrame);
            _camera.ShouldReCalculateValidLine = false;
        }

        _deltaTime = Time.deltaTime;

        ControllBeginingDoor();

        ControllGuardianAppear();

        ControllGuardianEscape();

        AniTimerMethod();
    }

    private void FixedUpdate()
    {
        if (GameEvent.PassHall)
        {
            return;
        }

        ControllHallway();
    }

    private void ControllBeginingDoor()//開頭門口白光
    {
        float Transparent = 0;
        if (!PassDoorWhiteLight)
        {
            //計算玩家與門口的距離
            if(PlayerTransform!= null)
            {
                DistanceWithPlayerAndDoor = Mathf.Abs(DoorPoint.localPosition.x - PlayerTransform.localPosition.x);
            }

            if(DistanceWithPlayerAndDoor <= 11.28f)
            {
                Transparent = (10 - (DistanceWithPlayerAndDoor - 1.28f)) / 10;
                if(DistanceWithPlayerAndDoor <= 1.28f)
                {
                    Transparent = 1;
                }
                WhiteScreen.color = new Vector4(1, 1, 1, Transparent);
            }

            if (WhiteLightTrigger.isTouch)
            {
                PassDoorWhiteLight = true;
                WhiteScreen.gameObject.SetActive(false);
                _camera.ImmediatelyFrameSet(_camera.MainMapFrame);
                WhiteFadeIn.SetActive(true);
                isDoEvent = true;
                GameEvent.isAniPlay = true;
                AniPhase = 1;
                AniTimer = Ani1TimerSet;
                WhiteLightTrigger.gameObject.SetActive(false);
            }
        }
    }

    private void ControllGuardianAppear()//關門及門衛
    {
        if (!GuardianAppear)
        {
            if (RightDoorTrigger.isTouch)
            {
                RightDoor.CloseDoor();
                RightDoorTrigger.gameObject.SetActive(false);
                RightDoorTrigger.isTouch = false;
                FirstStepTrigger.gameObject.SetActive(true);
                SecondStepTrigger.gameObject.SetActive(true);
                GuardianTrigger.gameObject.SetActive(true);
            }

            if (FirstStepTrigger.isTouch)
            {
                FootStepSource.Play();
                FirstStepTrigger.gameObject.SetActive(false);
                FirstStepTrigger.isTouch = false;
            }
            if (SecondStepTrigger.isTouch)
            {
                SmallHowlSource.Play();
                SecondStepTrigger.gameObject.SetActive(false);
                SecondStepTrigger.isTouch = false;
            }
            if (GuardianTrigger.isTouch)
            {
                GuardianAppear = true;
                GuardianTrigger.gameObject.SetActive(false);
                GuardianTrigger.isTouch = false;
                GameEvent.isAniPlay = true;
                isDoEvent = true;
                AniPhase = 2;
                AniTimer = Ani2TimerSet;
            }

            SEController.CalculateSystemSound(FootStepSource);
            SEController.CalculateSystemSound(SmallHowlSource);
        }
    }

    private void ControllGuardianEscape()//門衛逃跑
    {
        if (Guardian == null && !GuardianDie)
        {
            GameEvent.isAniPlay = true;
            isDoEvent = true;
            AniPhase = 3;
            AniTimer = Ani3TimerSet;
            GuardianDie = true;
        }
    }

    private void ControllHallway()//走廊
    {
        if (!GuardianAtk)
        {
            switch (MapFrameStatus)
            {
                case 1:
                    if (!HallwayTrigger.isTouch)
                    {
                        HallTrigger.gameObject.SetActive(false);
                        HallwayTrigger.gameObject.SetActive(true);
                    }
                    if (HallwayTrigger.isTouch)
                    {
                        if (!MapFrameRecordSet)
                        {
                            HallTrigger.gameObject.SetActive(true);
                            HallwayTrigger.gameObject.SetActive(false);
                            _camera.ChangeTimeReset();
                            MapFrameRecordSet = true;
                        }
                        if (!MapFrameChangeComplete)
                        {
                            _camera.FrameTransformChange(_camera.MainMapFrame, RightHallwayFrame, Time.fixedDeltaTime, ref MapFrameChangeComplete);
                        }
                        else
                        {
                            HallwayTrigger.isTouch = false;
                            MapFrameRecordSet = false;
                            MapFrameChangeComplete = false;
                            MapFrameStatus = 2;
                        }
                    }
                    if (HallTrigger.isTouch)
                    {
                        HallwayTrigger.isTouch = false;
                        MapFrameRecordSet = false;
                        MapFrameChangeComplete = false;
                        MapFrameStatus = 2;
                    }
                    break;
                case 2:
                    if (!HallTrigger.isTouch)
                    {
                        HallTrigger.gameObject.SetActive(true);
                        HallwayTrigger.gameObject.SetActive(false);
                    }
                    if (HallTrigger.isTouch)
                    {
                        if (!MapFrameRecordSet)
                        {
                            HallTrigger.gameObject.SetActive(false);
                            HallwayTrigger.gameObject.SetActive(true);
                            _camera.ChangeTimeReset();
                            MapFrameRecordSet = true;
                        }
                        if (!MapFrameChangeComplete)
                        {
                            _camera.FrameTransformChange(RightHallwayFrame, _camera.MainMapFrame, Time.fixedDeltaTime, ref MapFrameChangeComplete);
                        }
                        else
                        {
                            HallTrigger.isTouch = false;
                            MapFrameRecordSet = false;
                            MapFrameChangeComplete = false;
                            MapFrameStatus = 1;
                        }
                    }
                    if (HallwayTrigger.isTouch)
                    {
                        HallTrigger.isTouch = false;
                        MapFrameRecordSet = false;
                        MapFrameChangeComplete = false;
                        MapFrameStatus = 1;
                    }
                    break;
            }

            if (AtkTrigger.isTouch)
            {
                GuardianAtk = true;
                AtkTrigger.gameObject.SetActive(false);
                AtkTrigger.isTouch = false;
                GameEvent.isAniPlay = true;
                SuddenStopSource.Play();
                isDoEvent = true;
                AniPhase = 5;
                AniTimer = Ani5TimerSet;
            }
        }

        SEController.CalculateSystemSound(SuddenStopSource);
    }

    private void AniTimerMethod()
    {
        if (isDoEvent)
        {
            switch (AniPhase)
            {
                case 1:
                    AniTimer -= _deltaTime;

                    if (AniTimer <= 0.5f)
                    {
                        if (!FirstTrigger)
                        {
                            LeftDoor.CloseDoor();
                            FirstTrigger = true;
                        }
                    }
                    if (AniTimer <= 0)
                    {
                        isDoEvent = false;
                        FirstTrigger = false;
                        GameEvent.isAniPlay = false;
                    }
                    break;
                case 2:
                    AniTimer -= _deltaTime;
                    if (AniTimer <= 0)
                    {
                        isDoEvent = false;
                        HPUI.SetActive(true);
                        GameEvent.isAniPlay = false;
                    }
                    break;
                case 3:
                    AniTimer -= _deltaTime;
                    if (AniTimer <= 0)
                    {
                        if (GuardianFaceLeft && LGuardianEscape.transform.localPosition.x <= -13.8f)
                        {
                            AniPhase = 4;
                            AniTimer = Ani4TimerSet;
                        }
                        if (GuardianFaceRight && RGuardianEscape.transform.localPosition.x >= 18.2f)
                        {
                            AniPhase = 4;
                            AniTimer = Ani4TimerSet;
                        }
                    }
                    break;
                case 4:
                    AniTimer -= _deltaTime;
                    if (AniTimer <= 0)
                    {
                        if (GuardianFaceLeft)
                        {
                            Destroy(LGuardianEscape);
                        }
                        if (GuardianFaceRight)
                        {
                            Destroy(RGuardianEscape);
                        }
                        isDoEvent = false;
                        RightDoor.OpenDoor();
                        GameEvent.isAniPlay = false;
                    }
                    break;
                case 5:
                    AniTimer -= _deltaTime;

                    if (AniTimer <= 0)
                    {
                        GameEvent.PassHall = true;
                        _PlayerData.CommonSave();
                        _portal.BeginChangeScene();
                    }
                    break;
            }
        }
    }

    private void PlayAni()
    {
        GameEvent.isAniPlay = true;
    }
}
