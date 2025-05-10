using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Elevator : MonoBehaviour
{
    public int ElevatorNumber;//其他script有用到(playerController)
    private SelectElevatorController _controller;
    public GameObject ElevatorStillLock;
    private GameObject ElevatorUnlockAnimation;
    private GameObject ElevatorOpenAnimation;
    private Animator ElevatorUnlockAni;
    private Animator ElevatorOpenAni;
    private bool BeginUnlock;
    private bool BeginOpenDoor;
    private float Timer;
    private float UnLockTimerSet = 0.75f;
    private float OpenDoorTimerSet = 1f;
    private PlayerData _PlayerData;
    private InteractableObject _Interactable;
    private Portal _portal;

    public AudioClip UnlockSound;
    public AudioClip DoorOpenSound;

    private AudioSource UnlockSource;
    private AudioSource DoorOpenSource;
    // Start is called before the first frame update
    void Start()
    {
        _Interactable = this.GetComponent<InteractableObject>();
        _portal = this.GetComponent<Portal>();
        if(GameObject.Find("FollowSystem")!= null)
        {
            _PlayerData = GameObject.Find("FollowSystem").GetComponent<PlayerData>();
        }
        if(GameObject.FindGameObjectWithTag("UI") != null)
        {
            Transform UICanvas;
            UICanvas = GameObject.FindGameObjectWithTag("UI").transform;

            _controller = IdentifyID.FindObject(UICanvas, UIID.SelectPortalMenu).GetComponent<SelectElevatorController>();
            UICanvas.GetComponent<PauseMenuController>().Select4FButton.onClick.AddListener(GoToFourFloor);
            UICanvas.GetComponent<PauseMenuController>().Select3FButton.onClick.AddListener(GoToThirdFloor);
            UICanvas.GetComponent<PauseMenuController>().Select2FButton.onClick.AddListener(GoToSecondFloor);
            UICanvas.GetComponent<PauseMenuController>().Select1FButton.onClick.AddListener(GoToOneFloor);
        }
        ElevatorUnlockAnimation = this.transform.GetChild(2).gameObject;
        ElevatorOpenAnimation = this.transform.GetChild(3).gameObject;
        ElevatorUnlockAni = ElevatorUnlockAnimation.gameObject.GetComponent<Animator>();
        ElevatorOpenAni = ElevatorOpenAnimation.gameObject.GetComponent<Animator>();
        switch (ElevatorNumber)
        {
            case 1:
                if (GameEvent.Elevator1FUnlock)
                {
                    ElevatorOpenAnimation.SetActive(true);
                    ElevatorUnlockAnimation.SetActive(false);
                }
                break;
            case 2:
                if (GameEvent.Elevator2FUnlock)
                {
                    ElevatorOpenAnimation.SetActive(true);
                    ElevatorUnlockAnimation.SetActive(false);
                }
                break;
            case 4:
                if (GameEvent.Elevator4FUnlock)
                {
                    ElevatorOpenAnimation.SetActive(true);
                    ElevatorUnlockAnimation.SetActive(false);
                }
                break;
        }

        SelectButtonController.MaxSelectNumberSet(1, 4);
        if (GameEvent.GoIn3F1)
        {
            if (!_controller.isTurnOff3F)
            {
                _controller.TurnOff3FButton();
            }
        }

        SEController.inisializeAudioSource(ref UnlockSource, UnlockSound, this.transform);
        SEController.inisializeAudioSource(ref DoorOpenSource, DoorOpenSound, this.transform);

        _Interactable._interact += OnInteract;
    }

    // Update is called once per frame
    void Update()
    {
        TimerMethod();

        SEController.CalculateSystemSound(UnlockSource);
        SEController.CalculateSystemSound(DoorOpenSource);
    }

    void TimerMethod()
    {
        if (BeginUnlock)
        {
            if (Timer <= 0)
            {
                UnlockSource.Play();
                Timer = UnLockTimerSet;
            }

            Timer -= Time.deltaTime;

            switch (ElevatorNumber)
            {
                case 1:
                    ElevatorUnlockAni.SetBool("Unlock", true);
                    if (Timer <= 0)
                    {
                        GameEvent.Elevator1FUnlock = true;
                        _PlayerData.CommonSave();
                        ElevatorUnlockAni.SetBool("Unlock", false);
                        ElevatorOpenAnimation.SetActive(true);
                        ElevatorUnlockAnimation.SetActive(false);
                        BeginUnlock = false;
                    }
                    break;
                case 2:
                    ElevatorUnlockAni.SetBool("Unlock", true);
                    if (Timer <= 0)
                    {
                        GameEvent.Elevator2FUnlock = true;
                        _PlayerData.CommonSave();
                        ElevatorUnlockAni.SetBool("Unlock", false);
                        ElevatorOpenAnimation.SetActive(true);
                        ElevatorUnlockAnimation.SetActive(false);
                        BeginUnlock = false;
                    }
                    break;
                case 4:
                    ElevatorUnlockAni.SetBool("Unlock", true);
                    if (Timer <= 0)
                    {
                        GameEvent.Elevator4FUnlock = true;
                        _PlayerData.CommonSave();
                        ElevatorUnlockAni.SetBool("Unlock", false);
                        ElevatorOpenAnimation.SetActive(true);
                        ElevatorUnlockAnimation.SetActive(false);
                        BeginUnlock = false;
                    }
                    break;
            }
        }
        if (BeginOpenDoor)
        {
            if (Timer <= 0)
            {
                DoorOpenSource.Play();
                Timer = OpenDoorTimerSet;
            }

            Timer -= Time.deltaTime;

            ElevatorOpenAni.SetBool("Open", true);
            if (Timer <= 0)
            {
                ElevatorOpenAni.SetBool("Open", false);
                BeginOpenDoor = false;
            }
        }
    }

    private void OnInteract()
    {
        //已啟動電梯
        switch (ElevatorNumber)
        {
            case 1:
                if (GameEvent.Elevator1FUnlock)
                {
                    _controller.OpenSelectElevatorMenu(ElevatorNumber);
                }
                break;
            case 2:
                if (GameEvent.Elevator2FUnlock)
                {
                    _controller.OpenSelectElevatorMenu(ElevatorNumber);
                }
                break;
            case 4:
                if (GameEvent.Elevator4FUnlock)
                {
                    _controller.OpenSelectElevatorMenu(ElevatorNumber);
                }
                break;
        }
        //未啟動電梯
        switch (ElevatorNumber)
        {
            case 1:
                if (!GameEvent.Elevator1FUnlock)
                {
                    BeginUnlock = true;
                }
                break;
            case 2:
                if (!GameEvent.Elevator2FUnlock)
                {
                    BeginUnlock = true;
                }
                break;
            case 4:
                if (!GameEvent.Elevator4FUnlock)
                {
                    BeginUnlock = true;
                }
                break;
        }
    }

    public void GoToOneFloor()
    {
        if (GameEvent.Elevator1FUnlock)
        {
            BeginGoFloor(11);
        }
        else
        {
            ElevatorStillLock.SetActive(true);
        }
    }//Button專用 PauseMenuController會用到
    public void GoToSecondFloor()
    {
        if (GameEvent.Elevator2FUnlock)
        {
            BeginGoFloor(12);
        }
        else
        {
            ElevatorStillLock.SetActive(true);
        }
    }//Button專用 PauseMenuController會用到
    public void GoToThirdFloor()
    {
        ElevatorStillLock.SetActive(true);
    }//Button專用 PauseMenuController會用到
    public void GoToFourFloor()
    {
        if (GameEvent.Elevator4FUnlock)
        {
            BeginGoFloor(14);
        }
        else
        {
            ElevatorStillLock.SetActive(true);
        }
    }//Button專用 PauseMenuController會用到

    private void BeginGoFloor(int GoNumber)
    {
        switch (GoNumber)
        {
            case 11:
                _portal.SceneName = "1F-1";
                break;
            case 12:
                _portal.SceneName = "2F-2";
                break;
            case 14:
                _portal.SceneName = "FinalStair";
                break;
        }
        _portal.pointNumber = GoNumber;
        _portal.ChangeBGM = true;
        _controller.CloseSelectElevatorMenu();
        _portal.BeginChangeScene();
        BeginOpenDoor = true;
    }
}
