using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Training0Controller : MonoBehaviour
{
    //這個script有與Ani合併
    private int Phase;// 1開頭 2按下enter後 3自由活動
    public GameObject Mask;
    public GameObject CommandLetter;
    public transparencyChange _MaskChange;
    public GraduallyShowWord _firstLine;
    [SerializeField] private CameraController _camera;

    private float Timer;
    private float Phase1TimerSet = 16;//文字11.05
    private float Phase2TimerSet = 4.7f;

    private float _deltaTime;

    public Animator FadeIn;
    public GameObject EnterContinue;
    private bool WaitingPressEnter;
    private PlayerAnimationController _aniController;
    private PlayerSpecialAni _specialAni;

    public AudioClip PaperSound;
    public AudioClip SwordSound;
    private AudioSource PaperSource;
    private AudioSource SwordSource;
    private bool SEAppear;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("player") != null)
        {
            _aniController = GameObject.Find("player").GetComponent<PlayerAnimationController>();
            _specialAni = GameObject.Find("player").GetComponent<PlayerSpecialAni>();
            _aniController.AbsoluteAniFalse();
            _specialAni.OpeningAniPlay(1);
        }
        FadeIn.SetFloat("Speed", 2);
        GameEvent.isAniPlay = true;
        BackgroundSystem.CantPause = true;
        FadeInController.ContinueCantPause = true;
        Phase = 1;

        PaperSource = this.AddComponent<AudioSource>();
        SwordSource = this.AddComponent<AudioSource>();
        PaperSource.clip = PaperSound;
        SwordSource.clip = SwordSound;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameObject.Find("player") == null)
        {
            return;
        }

        _deltaTime = Time.deltaTime;

        if (WaitingPressEnter)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.JoystickButton0))
            {
                CommandLetter.SetActive(false);
                Mask.SetActive(false);
                _specialAni.OpeningAniPlay(2);
                MusicController.PlayBGM(10);
                MusicController.BeginFadeInBGM(1, 1);
                BackgroundSystem.CantPause = false;
                Phase = 2;
                WaitingPressEnter = false;
            }
        }

        _camera.FollowPlayer();

        TimerMethod();

        SEController.CalculateSystemSound(PaperSource);
        SEController.CalculateSystemSound(SwordSource);
    }

    private void TimerMethod()
    {
        switch (Phase)
        {
            case 1:
                if (Timer <= 0 && !WaitingPressEnter)
                {
                    PaperSource.Play();
                    Timer = Phase1TimerSet;
                }

                Timer -=_deltaTime;

                if (Timer <= (Phase1TimerSet - 2))
                {
                    _MaskChange.StartChange = true;
                }
                if (Timer <= (Phase1TimerSet - 3.5))
                {
                    _firstLine.CanBegin = true;
                }

                if (Timer <= 0)
                {
                    WaitingPressEnter = true;
                    EnterContinue.SetActive(true);
                }
                break;
            case 2:
                if (Timer <= 0 )
                {
                    Timer = Phase2TimerSet;
                }

                Timer -= _deltaTime;

                if (Timer <= (Phase2TimerSet - 3.75))
                {
                    if (!SEAppear)
                    {
                        SwordSource.Play();
                        SEAppear = true;
                    }
                }

                if (Timer <= 0)
                {
                    GameEvent.isAniPlay = false;
                    _specialAni.SpecialAniFalse();
                    _aniController.WaitAniPlay();
                    Phase = 3;
                }
                break;
        }
    }
}
