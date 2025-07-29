using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3Controller : MonoBehaviour
{
    public float TimerSet;
    private float Timer;
    private float Timer2 = 3;//±qºC°Ê§@«ì´_
    private bool isDoEvent;
    public GameObject RWall;
    public GameObject LWall;
    private int EventNumber;
    public GameObject Boss3CameraObject;
    public GameObject Boss3;
    private EvilKingController _EC;
    public GameObject HpUI;
    public GameObject HpUI2;
    private PlayerData _PlayerData;
    public static bool EndAppear = false;//script(pauseMenuController)
    public GameObject End;
    public GameObject Platform;
    public GameObject DarkPower;
    public FirstComeInBoss3 _FirstJudgement;
    public AgainComeInPlace _SecondJudgement;

    public static bool PlayerShoot;//script(EvilKingFinalAni¡AplayerSpecialAni)
    public static bool PlayerSecondJump;//script(EvilKingFinalAni¡AplayerSpecialAni)
    public static bool EvilKingAtkAni;//script(EvilKingFinalAni¡AplayerSpecialAni)
    public static bool EvilKingPrepare;//script(EvilKingFinalAni¡AplayerSpecialAni)
    public static bool PlayerPrepare;//script(EvilKingFinalAni¡AplayerSpecialAni)
    public static bool BeginToSlowSpeed;//script(EvilKingFinalAni¡AplayerSpecialAni)
    public static bool EvilKingLeave;//script(EvilKingFinalAni¡AplayerSpecialAni)
    public static bool EvilKingEnd;//script(EvilKingFinalAni¡AplayerSpecialAni¡ABoss3Wall)
    public static bool isFinalAtkDisappear;//script(EvilKingSpecialDisappear)
    public static bool isWinBoss3AniPlay;//script(playerSpecialAni¡AblackScreen)

    // Start is called before the first frame update
    void Start()
    {
        _EC = Boss3.GetComponent<EvilKingController>();
        if (GameObject.Find("FollowSystem") != null)
        {
            _PlayerData = GameObject.Find("FollowSystem").GetComponent<PlayerData>();
        }
        Timer = TimerSet;
        if (GameEvent.GoInBoss3)
        {
            isDoEvent = true;
            EventNumber = 2;
        }
        else
        {
            isDoEvent = true;
            EventNumber = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isDoEvent)
        {
            switch (EventNumber)
            {
                case 1:
                    if (_FirstJudgement.FirstGoIn)
                    {
                        Timer -= Time.deltaTime;
                        Boss3CameraObject.SetActive(true);
                        Boss3Camera.isFighting = true;
                        GameEvent.isAniPlay = true;
                        if (Timer <= (TimerSet - 5))
                        {
                            GameEvent.GoInBoss3 = true;
                            GameEvent.isAniPlay = false;
                            _EC.isFighting = true;
                            _FirstJudgement.FirstGoIn = false;
                            this.GetComponent<MusicJudgement>().Number = 6;
                            MusicController.BeginFadeInBGM();
                            RWall.SetActive(true);
                            LWall.SetActive(true);
                            HpUI.SetActive(true);
                            HpUI2.SetActive(true);
                            EventNumber = 3;
                            _PlayerData.CommonSave();
                            Timer = TimerSet;
                        }
                    }
                    break;
                case 2:
                    if (_SecondJudgement.AgainGoIn)
                    {
                        Timer -= Time.deltaTime;
                        Boss3CameraObject.SetActive(true);
                        Boss3CameraObject.GetComponent<Animator>().SetBool("Skip",true);
                        Boss3Camera.isFighting = true;
                        GameEvent.isAniPlay = true;
                        if (Timer <= (TimerSet - 1))
                        {
                            GameEvent.isAniPlay = false;
                            _EC.isFighting = true;
                            this.GetComponent<MusicJudgement>().Number = 6;
                            MusicController.BeginFadeInBGM();
                            RWall.SetActive(true);
                            LWall.SetActive(true);
                            HpUI.SetActive(true);
                            HpUI2.SetActive(true);
                            EventNumber = 3;
                            _SecondJudgement.AgainGoIn = false;
                            Timer = TimerSet;
                        }
                    }
                    break;
                case 3:
                    if(Boss3 == null)
                    {
                        HpUI.SetActive(false);
                        HpUI2.SetActive(false);
                        isWinBoss3AniPlay = true;
                        GameEvent.isAniPlay = true;
                        Boss3CameraObject.SetActive(false);
                        FinalTimer();
                    }
                    break;
            }
        }
    }

    private void FinalTimer()
    {
        if (PlayerShoot)
        {
            Destroy(Platform);
            isFinalAtkDisappear = true;
        }
        if (EvilKingAtkAni && !PlayerPrepare)
        {
            DarkPower.SetActive(true);
        }
        if (BeginToSlowSpeed)
        {
            Time.timeScale = 0.2f;
            Timer2 -= Time.unscaledDeltaTime;
            if (Timer2 <= 0)
            {
                Time.timeScale = 1;
            }
        }
        if (EvilKingLeave)
        {
            Timer -= Time.deltaTime;
            if (Timer <= (TimerSet - 1))
            {
                if (!GameEvent.PassBoss3)
                {
                    GameEvent.PassBoss3 = true;
                    GameEvent.KilledBossNumber += 1;
                    ItemManage.ItemGettingNumber += 1;
                }
                GameEvent.isAniPlay = false;
                MusicController.BeginFadeOutBGM();
                EvilKingEnd = true;
                isWinBoss3AniPlay = false;
                Boss3Camera.isFighting = false;
                EndAppear = true;
                End.SetActive(true);
                isDoEvent = false;
                _PlayerData.CommonSave();
            }
        }
    }
}
