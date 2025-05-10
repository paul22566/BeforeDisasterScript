using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B1Elevator : MonoBehaviour
{
    private GameObject FadeOut;
    private GameObject Player;
    private GameObject ElevatorOpenAnimation;
    private Animator ElevatorOpenAni;
    private bool timerSwitch;
    private float Timer;
    private float TimerSet = 0.75f;
    private InteractableObject _interactable;
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("UI") != null)
        {
            Transform UICanvas;
            UICanvas = GameObject.FindGameObjectWithTag("UI").transform;
            FadeOut = IdentifyID.FindObject(UICanvas, UIID.FadeOut);
        }
        ElevatorOpenAnimation = this.transform.GetChild(2).gameObject;
        ElevatorOpenAni = ElevatorOpenAnimation.gameObject.GetComponent<Animator>();
        if (GameObject.Find("player") != null)
        {
            Player = GameObject.Find("player");
        }
        _interactable = this.GetComponent<InteractableObject>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (_interactable.PlayerInteract)
        {
            FadeOut.SetActive(true);
            FadeOutNormal.isPortal = true;
            LoadScene.SceneName = "1F-2";
            BackgroundSystem.startPointNumber = 4;
            timerSwitch = true;
            _interactable.PlayerInteract = false;
        }*/
        TimerMethod();
    }

    void TimerMethod()
    {
        if (timerSwitch)
        {
            Timer -= Time.deltaTime;
            ElevatorOpenAni.SetBool("Open", true);
            if (Timer <= (TimerSet - 0.58))
            {
                ElevatorOpenAni.SetBool("Open", false);
                timerSwitch = false;
            }
        }
        else
        {
            Timer = TimerSet;
        }
    }
}
