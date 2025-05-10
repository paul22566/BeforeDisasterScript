using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondFloor1Controller : MonoBehaviour
{
    private PlayerData _PlayerData;
    [HideInInspector] public int StatusNumber = 0;//1是大雷射消失 2是轉盤啟動 3是轉盤壞掉
    private float AniTimer;
    private float Ani1TimerSet = 1;
    private float Ani2TimerSet = 6.5f;
    private float Ani3TimerSet = 8.2f;
    private float _fixedDeltaTime;

    //大雷射光
    public GameObject BigLight;
    private Animator LightAnimator;
    public InteractableObject ShortcutButton;

    //轉盤運作
    public InteractableObject MainSwitch;
    public InteractableObject LeftSwitch;
    public InteractableObject RightSwitch;
    public InteractableObject UpSwitch;
    public GameObject CloseDoorCenter;
    public GameObject OpenDoorCenter;
    public Animator RotateCircleAni;
    public Transform MainSwitchTransform;
    public GameObject OpenedMainSwitch;
    public Animator LeftFenceAni;
    public Animator RightFenceAni;
    private bool isCircleRun;
    private float CircleRunTimer;
    private float CircleRunTimerSet = 2;

    //轉盤毀壞
    public GameObject TotalCircleDoor;
    public CrystalControll DoorCrystal;
    public GameObject HiddenDoor;

    //隱藏牆壁
    public TouchTrigger _hiddenTrigger;
    public OnceTimeHiddenWall _hiddenWall;

    //SE
    public AudioClip MetalFenceOpenSound;
    public AudioClip MetalFenceCloseSound;
    public AudioClip CircleRunSound;

    private AudioSource MetalFenceOpenSource;
    private AudioSource MetalFenceCloseSource;
    private AudioSource CircleRunSource;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("FollowSystem") != null)
        {
            _PlayerData = GameObject.Find("FollowSystem").GetComponent<PlayerData>();
        }

        if (GameEvent.SecondFloorOneBigLightClose)
        {
            Destroy(BigLight);
        }
        else
        {
            LightAnimator = BigLight.GetComponent<Animator>();
        }

        if (GameEvent.Find2F_1HiddenArea)
        {
            Destroy(_hiddenTrigger.gameObject);
            Destroy(_hiddenWall.gameObject);
        }

        if (!GameEvent.OpenCirclePlatform)
        {
            UpSwitch.isValidable = false;
            LeftSwitch.isValidable = false;
            RightSwitch.isValidable = false;
        }

        if (GameEvent.OpenCirclePlatform && !GameEvent.DestroyCircleDoor)
        {
            switch (GameEvent.CirclePlatformStatus)
            {
                case 1:
                    RotateCircleAni.SetInteger("Status", 0);
                    break;
                case 2:
                    RotateCircleAni.SetInteger("Status", 2);
                    break;
            }

            MainSwitch.isValidable = false;
            MainSwitchTransform.gameObject.SetActive(false);
            OpenedMainSwitch.SetActive(true);
            CloseDoorCenter.SetActive(false);
            OpenDoorCenter.SetActive(true);
        }

        if (GameEvent.DestroyCircleDoor)
        {
            Destroy(TotalCircleDoor);
            UpSwitch.isValidable = false;
            LeftSwitch.isValidable = false;
            RightSwitch.isValidable = false;
            HiddenDoor.SetActive(true);
        }

        ShortcutButton._interact += OnInteractBigLight;
        MainSwitch._interact += OnInteractMainSwitch;
        UpSwitch._interact += OnInteractUpSwitch;
        LeftSwitch._interact += OnInteractLeftSwitch;
        RightSwitch._interact += OnInteractRightSwitch;

        SEController.inisializeAudioSource(ref MetalFenceOpenSource, MetalFenceOpenSound, this.transform);
        SEController.inisializeAudioSource(ref MetalFenceCloseSource, MetalFenceCloseSound, this.transform);
        SEController.inisializeAudioSource(ref CircleRunSource, CircleRunSound, this.transform);
    }

    // Update is called once per frame
    void Update()
    {
        RotateCircleControll();

        HiddenWallControll();

        DestroyCircleDoor();
    }

    private void FixedUpdate()
    {
        _fixedDeltaTime = Time.fixedDeltaTime;

        TimerMethod();
    }

    private void OnInteractBigLight()
    {
        LightAnimator.SetBool("Disappear", true);
        AniTimer = Ani1TimerSet;
        StatusNumber = 1;
        _PlayerData.CommonSave();
        ShortcutButton.OnceTimeInteractSuccess();
        GameEvent.SecondFloorOneBigLightClose = true;
    }

    private void RotateCircleControll()
    {
        if (GameEvent.DestroyCircleDoor)
        {
            return;
        }

        if (!GameEvent.OpenCirclePlatform)
        {
            return;
        }

        SEController.CalculateSystemSound(MetalFenceOpenSource);
        SEController.CalculateSystemSound(MetalFenceCloseSource);
        SEController.CalculateSystemSound(CircleRunSource);
    }

    private void OnInteractMainSwitch()
    {
        if (!GameEvent.OpenCirclePlatform)
        {
            GameEvent.OpenCirclePlatform = true;
            GameEvent.CirclePlatformStatus = 2;
            StatusNumber = 2;
            GameEvent.isAniPlay = true;
            UpSwitch.isValidable = true;
            LeftSwitch.isValidable = true;
            RightSwitch.isValidable = true;
            _PlayerData.CommonSave();
            MainSwitch.OnceTimeInteractSuccess();
            AniTimer = Ani2TimerSet;
        }
    }

    private void OnInteractUpSwitch()
    {
        UpSwitch.InteractSuccess();
        RightFenceAni.SetBool("Open", true);
        LeftFenceAni.SetBool("Open", true);
        MetalFenceOpenSource.Play();
        isCircleRun = true;
        CircleRunTimer = CircleRunTimerSet;
        switch (GameEvent.CirclePlatformStatus)
        {
            case 1:
                RotateCircleAni.SetInteger("Status", 1);
                GameEvent.CirclePlatformStatus = 2;
                break;
            case 2:
                RotateCircleAni.SetInteger("Status", 0);
                GameEvent.CirclePlatformStatus = 1;
                break;
        }
        CircleRunSource.Play();
    }
    private void OnInteractLeftSwitch()
    {
        LeftSwitch.InteractSuccess();
        RightFenceAni.SetBool("Open", true);
        LeftFenceAni.SetBool("Open", true);
        MetalFenceOpenSource.Play();
        isCircleRun = true;
        CircleRunTimer = CircleRunTimerSet;
        switch (GameEvent.CirclePlatformStatus)
        {
            case 1:
                RotateCircleAni.SetInteger("Status", 1);
                GameEvent.CirclePlatformStatus = 2;
                break;
            case 2:
                RotateCircleAni.SetInteger("Status", 0);
                GameEvent.CirclePlatformStatus = 1;
                break;
        }
        CircleRunSource.Play();
    }
    private void OnInteractRightSwitch()
    {
        RightSwitch.InteractSuccess();
        RightFenceAni.SetBool("Open", true);
        LeftFenceAni.SetBool("Open", true);
        MetalFenceOpenSource.Play();
        isCircleRun = true;
        CircleRunTimer = CircleRunTimerSet;
        switch (GameEvent.CirclePlatformStatus)
        {
            case 1:
                RotateCircleAni.SetInteger("Status", 1);
                GameEvent.CirclePlatformStatus = 2;
                break;
            case 2:
                RotateCircleAni.SetInteger("Status", 0);
                GameEvent.CirclePlatformStatus = 1;
                break;
        }
        CircleRunSource.Play();
    }

    private void HiddenWallControll()
    {
        if (GameEvent.Find2F_1HiddenArea)
        {
            return;
        }

        if (_hiddenTrigger.isTouch)
        {
            _hiddenWall.BeginDisappear();
            GameEvent.Find2F_1HiddenArea = true;
            _PlayerData.CommonSave();
            Destroy(_hiddenTrigger.gameObject);
        }
    }

    private void DestroyCircleDoor()
    {
        if (!GameEvent.DestroyCircleDoor && DoorCrystal.CrystalBroken)
        {
            GameEvent.DestroyCircleDoor = true;
            _PlayerData.CommonSave();
            UpSwitch.isValidable = false;
            LeftSwitch.isValidable = false;
            RightSwitch.isValidable = false;
            GameEvent.isAniPlay = true;
            AniTimer = Ani3TimerSet;
            StatusNumber = 3;
        }
    }

    private void TimerMethod()
    {
        if (isCircleRun)
        {
            CircleRunTimer -= _fixedDeltaTime;

            if (CircleRunTimer <= 0)
            {
                RightFenceAni.SetBool("Open", false);
                LeftFenceAni.SetBool("Open", false);
                MetalFenceCloseSource.Play();
                isCircleRun = false;
            }
        }

        switch (StatusNumber)
        {
            case 1:
                AniTimer -= _fixedDeltaTime;

                if (AniTimer <= 0)
                {
                    Destroy(BigLight);
                    StatusNumber = 0;
                    AniTimer = 0;
                }
                break;
            case 2:
                AniTimer -= _fixedDeltaTime;

                if (AniTimer <= 0)
                {
                    StatusNumber = 0;
                    AniTimer = 0;
                    GameEvent.isAniPlay = false;
                    CloseDoorCenter.SetActive(false);
                    OpenDoorCenter.SetActive(true);
                }
                break;
            case 3:
                AniTimer -= _fixedDeltaTime;

                if (AniTimer <= 0)
                {
                    StatusNumber = 0;
                    AniTimer = 0;
                    GameEvent.isAniPlay = false;
                    HiddenDoor.SetActive(true);
                    Destroy(TotalCircleDoor);
                }
                break;
        }
    }
}
