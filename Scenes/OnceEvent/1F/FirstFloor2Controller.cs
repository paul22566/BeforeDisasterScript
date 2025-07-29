using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FirstFloor2Controller : MonoBehaviour
{
    private PlayerData _PlayerData;
    public GameObject LeftPlatform;
    public GameObject RightPlatform;
    public GameObject LeftPlatform2;
    public GameObject RightPlatform2;
    public CameraController _cameraFollow;
    private float Timer;
    private float Timer2;
    private bool DoPlatformEvent;

    private MapFrame NewMap = new MapFrame();

    public GameObject UpMonster1;
    public GameObject UpMonster2;
    public GameObject DownMonster;

    //隱藏牆壁控制
    public TouchTrigger _hiddenWallDownTrigger;
    public TouchTrigger _hiddenWallUpTrigger;
    public TouchTrigger UpLeftTrigger;
    public TouchTrigger UpRightTrigger;
    public GameObject TotalHiddenWall;
    private OnceTimeHiddenWall _FirstWall;
    private OnceTimeHiddenWall _SecondWall;
    private OnceTimeHiddenWall _ThirdWall;
    private int MapFrameStatus = 1;
    //隱藏地區小按鈕
    private float PlatformTimerSet = 2;
    public InteractableObject PlatformButton;
    private bool SEAppear;
    public AudioClip PlatformMoveSound;
    private AudioSource PlatformMoveSource;

    //地下室電梯
    public Transform UnderElevator;
    private InteractableObject ElevatorButton;
    private GameObject ElevatorUnlockAnimation;
    private GameObject ElevatorOpenAnimation;
    private Animator ElevatorUnlockAni;
    private Animator ElevatorOpenAni;
    private float UnlockTimerSet = 0.58f;
    private float OpenTimerSet = 0.75f;
    private bool isUnlockElevator;
    private bool isOpenElevator;

    //控制室
    public InteractableObject ControllerRoomButton;
    public GameObject NeedKeyNotice;

    //管線
    public static bool Use1F_2Pipeline;
    public InteractableObject PipelineButton;
    private Portal _pipelinePortal;
    private OldPlayerAnimationController _aniController;
    private GameObject FadeOut;


    private void Start()
    {
        if (GameObject.Find("FollowSystem") != null)
        {
            _PlayerData = GameObject.Find("FollowSystem").GetComponent<PlayerData>();
        }

        if (GameObject.FindGameObjectWithTag("UI") != null)
        {
            Transform UICanvas;
            UICanvas = GameObject.FindGameObjectWithTag("UI").transform;
            FadeOut = IdentifyID.FindObject(UICanvas, UIID.FadeOut);
        }

        _cameraFollow.InitializeMapFrame(NewMap, 18.79f, 1.59f, 71.46f, 29.07f);
        if (GameEvent.Find1F_2SecretArea)
        {
            if (UpMonster1 != null)
            {
                UpMonster1.SetActive(false);
            }
            if (UpMonster2 != null)
            {
                UpMonster2.SetActive(false);
            }
            if (DownMonster != null)
            {
                DownMonster.SetActive(true);
            }
            Destroy(TotalHiddenWall);
            LeftPlatform.SetActive(false);
            RightPlatform.SetActive(false);
            LeftPlatform2.SetActive(true);
            RightPlatform2.SetActive(true);
            PlatformButton.isValidable = false;
        }
        else
        {
            if (UpMonster1 != null)
            {
                UpMonster1.SetActive(true);
            }
            if (UpMonster2 != null)
            {
                UpMonster2.SetActive(true);
            }
            if (DownMonster != null)
            {
                DownMonster.SetActive(false);
            }
            _FirstWall = TotalHiddenWall.transform.GetChild(0).GetComponent<OnceTimeHiddenWall>();
            _SecondWall = TotalHiddenWall.transform.GetChild(1).GetComponent<OnceTimeHiddenWall>();
            _ThirdWall = TotalHiddenWall.transform.GetChild(2).GetComponent<OnceTimeHiddenWall>();
            SEController.inisializeAudioSource(ref PlatformMoveSource, PlatformMoveSound, this.transform);
            DoPlatformEvent = true;
        }

        if (UnderElevator != null)
        {
            ElevatorUnlockAnimation = UnderElevator.GetChild(2).gameObject;
            ElevatorOpenAnimation = UnderElevator.GetChild(3).gameObject;
            ElevatorUnlockAni = ElevatorUnlockAnimation.gameObject.GetComponent<Animator>();
            ElevatorOpenAni = ElevatorOpenAnimation.gameObject.GetComponent<Animator>();
            ElevatorButton = UnderElevator.GetComponent<InteractableObject>();
        }

        if (GameEvent.ControllerRoomUnlock)
        {
            ControllerRoomButton.isValidable = false;
        }

        if (!GameEvent.Enter1F_2PipeLine)
        {
            if (GameObject.Find("player") != null)
            {
                _aniController = GameObject.Find("player").GetComponent<OldPlayerAnimationController>();
            }
        }
        else
        {
            PipelineButton.isValidable = false;
        }

        _pipelinePortal = PipelineButton.GetComponent<Portal>();

        PlatformButton._interact += OnInteractPlatformButton;
        ElevatorButton._interact += OnInteractUnderElevator;
        ControllerRoomButton._interact += OnInteractControllerRoomDoor;
        PipelineButton._interact += OnInteractPipeline;
    }

    // Update is called once per frame
    void Update()
    {
        ControllHiddenWall();

        ControllPlatformButton();

        ControllUnderElevator();
    }

    private void ControllHiddenWall()
    {
        switch (MapFrameStatus)
        {
            case 1:
                if (!_hiddenWallDownTrigger.isTouch)
                {
                    _hiddenWallUpTrigger.gameObject.SetActive(false);
                    UpLeftTrigger.gameObject.SetActive(false);
                    UpRightTrigger.gameObject.SetActive(false);
                    _hiddenWallDownTrigger.gameObject.SetActive(true);
                }
                if (_hiddenWallDownTrigger.isTouch)
                {
                    if (!GameEvent.Find1F_2SecretArea)
                    {
                        _FirstWall.BeginDisappear();
                        _SecondWall.BeginDisappear();
                        _ThirdWall.BeginDisappear();
                    }

                    _cameraFollow.FrameSet(NewMap);

                    _hiddenWallUpTrigger.gameObject.SetActive(true);
                    UpLeftTrigger.gameObject.SetActive(true);
                    UpRightTrigger.gameObject.SetActive(true);
                    _hiddenWallDownTrigger.gameObject.SetActive(false);
                    _hiddenWallDownTrigger.isTouch = false;
                    MapFrameStatus = 2;
                }
                break;
            case 2:
                if (!_hiddenWallUpTrigger.isTouch)
                {
                    _hiddenWallUpTrigger.gameObject.SetActive(true);
                    UpLeftTrigger.gameObject.SetActive(true);
                    UpRightTrigger.gameObject.SetActive(true);
                    _hiddenWallDownTrigger.gameObject.SetActive(false);
                }
                if (_hiddenWallUpTrigger.isTouch)
                {
                    if (UpLeftTrigger.isTouch || UpRightTrigger.isTouch)
                    {
                        _cameraFollow.FrameSet(_cameraFollow.MainMapFrame);

                        _hiddenWallUpTrigger.gameObject.SetActive(false);
                        UpLeftTrigger.gameObject.SetActive(false);
                        UpRightTrigger.gameObject.SetActive(false);
                        _hiddenWallDownTrigger.gameObject.SetActive(true);

                        _hiddenWallUpTrigger.isTouch = false;
                        UpLeftTrigger.isTouch = false;
                        UpRightTrigger.isTouch = false;
                        MapFrameStatus = 1;
                    }
                }
                break;
        }
    }

    private void ControllPlatformButton()
    {
        if (!DoPlatformEvent)
        {
            return;
        }

        if (DoPlatformEvent && GameEvent.Find1F_2SecretArea)
        {
            if (!SEAppear)
            {
                PlatformMoveSource.Play();
                SEAppear = true;
            }
            Timer -= Time.deltaTime;
            LeftPlatform.GetComponent<Animator>().SetBool("Open", true);
            RightPlatform.GetComponent<Animator>().SetBool("Open", true);
            if (Timer <= 0)
            {
                DoPlatformEvent = false;
                LeftPlatform.SetActive(false);
                RightPlatform.SetActive(false);
                LeftPlatform2.SetActive(true);
                RightPlatform2.SetActive(true);
            }
        }

        SEController.CalculateSystemSound(PlatformMoveSource);
    }

    private void OnInteractPlatformButton()
    {
        if (!GameEvent.Find1F_2SecretArea)
        {
            GameEvent.Find1F_2SecretArea = true;
            Timer = PlatformTimerSet;
            _PlayerData.CommonSave();
            PlatformButton.OnceTimeInteractSuccess();
        }
    }

    private void ControllUnderElevator()
    {
        if (isUnlockElevator || isOpenElevator)
        {
            Timer2 -= Time.deltaTime;
            if (isUnlockElevator)
            {
                ElevatorUnlockAni.SetBool("Unlock", true);
                if (Timer2 <= 0)
                {
                    ElevatorUnlockAni.SetBool("Unlock", false);
                    ElevatorOpenAnimation.SetActive(true);
                    ElevatorUnlockAnimation.SetActive(false);
                    isUnlockElevator = false;
                    GameEvent.UndergroundUnlock = true;
                    _PlayerData.CommonSave();
                }
            }
            if (isOpenElevator)
            {
                ElevatorOpenAni.SetBool("Open", true);
                if (Timer2 <= 0)
                {
                    ElevatorOpenAni.SetBool("Open", false);
                    isUnlockElevator = false;
                }
            }
        }
    }

    private void OnInteractUnderElevator()
    {
        if (!BackgroundSystem.isNoticeDialogAppear)
        {
            if (GameEvent.UndergroundUnlock)
            {
                FadeOut.SetActive(true);
                Portal.isPortal = true;
                LoadScene.SceneName = "B1-1";
                BackgroundSystem.startPointNumber = 1;
                isOpenElevator = true;
                Timer2 = OpenTimerSet;
            }
            else
            {
                if (ItemManage.CheckItemExist(ItemID.UnderGroundKey))
                {
                    isUnlockElevator = true;
                    Timer2 = UnlockTimerSet;
                }
                else
                {
                    NeedKeyNotice.SetActive(true);
                    ElevatorButton.OnceTimeInteractFail();
                    BackgroundSystem.isNoticeDialogAppear = true;
                }
            }
        }
    }

    private void OnInteractControllerRoomDoor()
    {
        if (!GameEvent.ControllerRoomUnlock && !BackgroundSystem.isNoticeDialogAppear)
        {
            if (ItemManage.CheckItemExist(ItemID.ControllerRoomKey))
            {
                GameEvent.ControllerRoomUnlock = true;
                _PlayerData.CommonSave();
                ControllerRoomButton.OnceTimeInteractSuccess();
            }
            else
            {
                NeedKeyNotice.SetActive(true);
                BackgroundSystem.isNoticeDialogAppear = true;
                ControllerRoomButton.OnceTimeInteractFail();
            }
        }
    }

    private void OnInteractPipeline()
    {
        if (!GameEvent.Enter1F_2PipeLine)
        {
            Use1F_2Pipeline = true;
            GameEvent.Enter1F_2PipeLine = true;
            if (!GameEvent.GoRestRoom)
            {
                GameEvent.SkipRestRoom = true;
            }
            _aniController.WaitAniPlay();
            _pipelinePortal.BeginChangeScene();
            _PlayerData.CommonSave();
            PipelineButton.OnceTimeInteractSuccess();
        }
    }
}
