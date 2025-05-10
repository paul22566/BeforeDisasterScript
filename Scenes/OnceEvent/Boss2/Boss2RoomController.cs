using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2RoomController : MonoBehaviour
{
    private GameObject Player;
    private int EventNumber;
    private bool isDoEvent = false;
    public float AniTimerSet;
    private float AniTimer;
    private bool timerSwitch;
    public static Vector3 Boss2DiePosition;//script(CaptainController¡AplayerData¡ATitleController)
    public static bool FaceRight = false;//script(playerspecialAni¡ACaptainController¡AplayerData¡ATitleController)
    public static bool FaceLeft = false;//script(playerspecialAni¡ACaptainController¡AplayerData¡ATitleController)
    public GameObject HpUi;
    public GameObject HpFrame;
    public GameObject Boss2;
    public GameObject UnderGroundKey;
    private PlayerData _PlayerData;
    public GameObject SoulItem;
    public GameObject RDeadBody;
    public GameObject LDeadBody;
    private Vector3 RPlayerPosition = new Vector3(1.982f, -0.747f, 0);
    private Vector3 LPlayerPosition = new Vector3(-1.982f, -0.747f, 0);
    // Start is called before the first frame update
    private void Awake()
    {
        AniTimer = AniTimerSet;
        if (!GameEvent.PassBoss2)
        {
            isDoEvent = true;
            GameEvent.isAniPlay = true;
            if (GameEvent.GoInBoss2)
            {
                EventNumber = 2;
            }
            else
            {
                EventNumber = 1;
            }
        }
        else
        {
            if (!GameEvent.AbsorbBoss2)
            {
                isDoEvent = true;
                EventNumber = 4;
            }
            else
            {
                if (!itemManage.CheckItemExist(ItemID.UnderGroundKey))
                {
                    UnderGroundKey.transform.position = Boss2DiePosition + new Vector3(0, -0.5f, 0);
                    UnderGroundKey.SetActive(true);
                }
            }
            if (FaceRight)
            {
                Instantiate(RDeadBody, Boss2DiePosition, Quaternion.identity);
            }
            if (FaceLeft)
            {
                Instantiate(LDeadBody, Boss2DiePosition, Quaternion.identity);
            }
        }
    }

    private void Start()
    {
        if (GameObject.Find("FollowSystem") != null)
        {
            _PlayerData = GameObject.Find("FollowSystem").GetComponent<PlayerData>();
        }
        if (GameObject.Find("player") != null)
        {
            Player = GameObject.Find("player");
        }
        if (Player)
        {
            if (EventNumber == 1)
            {
                GameObject g = GameObject.Find("0") as GameObject;
                if (g != null)
                {
                    Player.transform.position = g.transform.position;
                }
            }
            if (EventNumber == 2)
            {
                GameObject g = GameObject.Find("1") as GameObject;
                if (g != null)
                {
                    Player.transform.position = g.transform.position;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Timer();
        if (isDoEvent)
        {
            if (EventNumber == 1)
            {
                timerSwitch = true;
            }
            if (EventNumber == 2)
            {
                timerSwitch = true;
            }
            if (EventNumber == 3)
            {
                if (Boss2 == null)
                {
                    GameEvent.PassBoss2 = true;
                    GameEvent.isAniPlay = true;
                    timerSwitch = true;
                    _PlayerData.CommonSave();
                }
            }
            if (EventNumber == 4)
            {
                if (GameEvent.AbsorbBoss2)
                {
                    GameEvent.KilledBossNumber += 1;
                    GameEvent.isAniPlay = true;
                    timerSwitch = true;
                    isDoEvent = false;
                    if (FaceRight)
                    {
                        Player.transform.position = Boss2DiePosition + RPlayerPosition;
                    }
                    if (FaceLeft)
                    {
                        Player.transform.position = Boss2DiePosition + LPlayerPosition;
                    }
                    _PlayerData.CommonSave();
                }
            }
        }
    }

    void Timer()
    {
        if (timerSwitch)
        {
            switch (EventNumber)
            {
                case 1:
                    AniTimer -= Time.deltaTime;
                    if (AniTimer <= (AniTimerSet - 3))
                    {
                        HpUi.SetActive(true);
                        HpFrame.SetActive(true);
                        GameEvent.GoInBoss2 = true;
                        GameEvent.isAniPlay = false;
                        this.GetComponent<MusicJudgement>().Number = 5;
                        EventNumber = 3;
                        timerSwitch = false;
                        _PlayerData.CommonSave();
                    }
                    break;
                case 2:
                    AniTimer -= Time.deltaTime;
                    if (AniTimer <= (AniTimerSet - 1))
                    {
                        HpUi.SetActive(true);
                        HpFrame.SetActive(true);
                        EventNumber = 3;
                        GameEvent.isAniPlay = false;
                        this.GetComponent<MusicJudgement>().Number = 5;
                        timerSwitch = false;
                    }
                    break;
                case 3:
                    AniTimer -= Time.deltaTime;
                    if (AniTimer <= (AniTimerSet - 1))
                    {
                        if (FaceRight)
                        {
                            Instantiate(RDeadBody, Boss2DiePosition, Quaternion.identity);
                        }
                        if (FaceLeft)
                        {
                            Instantiate(LDeadBody, Boss2DiePosition, Quaternion.identity);
                        }
                        GameEvent.isAniPlay = false;
                        MusicController.BeginFadeOutBGM();
                        EventNumber = 4;
                        timerSwitch = false;
                    }
                    break;
                case 4:
                    AniTimer -= Time.deltaTime;
                    if (AniTimer <= (AniTimerSet - 3.6))
                    {
                        UnderGroundKey.transform.position = Boss2DiePosition + new Vector3(0, -0.5f, 0);
                        UnderGroundKey.SetActive(true);
                        SoulItem.SetActive(true);
                        itemManage.ItemGettingNumber += 1;
                        GameEvent.isAniPlay = false;
                        timerSwitch = false;
                    }
                    break;
            }
        }
        else
        {
            AniTimer = AniTimerSet;
        }
    }
}
