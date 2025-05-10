using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleAni : MonoBehaviour
{
    //大門開啟動畫
    public Animator DoorAni;
    public Animator CameraAni;
    private bool isOpenDoor;
    private float DoorOpenTimer = 7.1f;

    public AudioClip DoorCenterRotateSound;
    public AudioClip DoorOpenSound;

    private AudioSource DoorCenterRotateSource;
    private static AudioSource DoorOpenSource;

    private bool FirstTrigger;
    private bool SecondTrigger;
    private bool ThirdTrigger;

    private void Start()
    {
        DoorCenterRotateSource = this.AddComponent<AudioSource>();
        DoorOpenSource = this.AddComponent<AudioSource>();

        DoorCenterRotateSource.clip = DoorCenterRotateSound;
        DoorOpenSource.clip = DoorOpenSound;
    }

    private void Update()
    {
        SEController.CalculateSystemSound(DoorCenterRotateSource);
        SEController.CalculateSystemSound(DoorOpenSource);
    }

    private void FixedUpdate()
    {
        TitleDoorRun();
    }

    public void BeginOpenDoor()
    {
        isOpenDoor = true;
        GameEvent.isAniPlay = true;
    }

    private void TitleDoorRun()
    {
        if (isOpenDoor)
        {
            DoorOpenTimer -= Time.fixedDeltaTime;
            if (!FirstTrigger)
            {
                DoorAni.SetInteger("Status", 1);
                DoorCenterRotateSource.Play();
                FirstTrigger = true;
            }
            if (DoorOpenTimer <= (7.1 - 2.5))
            {
                if (!SecondTrigger)
                {
                    DoorAni.SetInteger("Status", 2);
                    DoorOpenSource.Play();
                    SecondTrigger = true;
                }
            }
            if (DoorOpenTimer <= (7.1 - 4))
            {
                if (!ThirdTrigger)
                {
                    CameraAni.SetBool("Move", true);
                    MusicController.ChangeBGM();
                    ThirdTrigger = true;
                }
            }
            if (DoorOpenTimer <= 0)
            {
                GameEvent.isAniPlay = false;
                SceneManager.LoadScene("CreatePlayer");
            }
        }
    }
}
