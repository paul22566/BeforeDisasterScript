using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdFloor2Controller : MonoBehaviour
{//1.5轉鏡頭  2刀男倒下  2.5幽靈出現 3.5幽靈出現完畢 4.8幽靈附身完畢 5人類起身 7人類起身完畢 7.3攝影機移動 7.5結束
    private GameObject Player;
    public GameObject Ghost;
    private int EventNumber;
    private bool isDoEvent = false;
    public float AniTimerSet;
    private float AniTimer;
    private bool timerSwitch;
    public int FirstGhostOrder;
    public int SecondGhostOrder;
    public int ThirdGhostOrder;
    private bool FirstSummon = false;
    private bool SecondSummon = false;
    private bool ThirdSummon = false;
    private Vector3 FirstGhostPlace = new Vector3(-9.51f , -11.53f , 0f);
    private Vector3 SecondGhostPlace = new Vector3(-8.72f, -5.69f, 0f);
    private Vector3 ThirdGhostPlace = new Vector3(11.91f, -6f, 0f);
    private Vector3 ForthGhostPlace = new Vector3(14.53f, -11.57f, 0f);
    private PlayerData _PlayerData;
    public static int ThirdFloor2KilledNumber = 0;//3F-2死亡人數  其他script有用到(3F-2相關)
    // Start is called before the first frame update
    private void Awake()
    {
        AniTimer = AniTimerSet;
        if (!GameEvent.HasPassThirdFloor2)
        {
            isDoEvent = true;
            GameEvent.isAniPlay = true;
            if (GameEvent.HasGoThirdFloor2)
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
            this.GetComponent<MusicJudgement>().Number = 2;
        }
    }

    private void Start()
    {
        ThirdFloor2KilledNumber = 0;
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
        if (isDoEvent)
        {
            Timer();
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
                if (ThirdFloor2KilledNumber >= FirstGhostOrder)
                {
                    if (!FirstSummon)
                    {
                        Instantiate(Ghost, FirstGhostPlace, Quaternion.identity);
                        FirstSummon = true;
                    }
                }
                if (ThirdFloor2KilledNumber >= SecondGhostOrder)
                {
                    if (!SecondSummon)
                    {
                        Instantiate(Ghost, SecondGhostPlace, Quaternion.identity);
                        Instantiate(Ghost, ThirdGhostPlace, Quaternion.identity);
                        SecondSummon = true;
                    }
                }
                if (ThirdFloor2KilledNumber >= ThirdGhostOrder)
                {
                    if (!ThirdSummon)
                    {
                        Instantiate(Ghost, ForthGhostPlace, Quaternion.identity);
                        ThirdSummon = true;
                    }
                }
                if (ThirdFloor2KilledNumber >= 8)
                {
                    GameEvent.HasPassThirdFloor2 = true;
                    MusicController.BeginFadeOutBGM();
                    isDoEvent = false;
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
                    if (AniTimer <= 0)
                    {
                        GameEvent.HasGoThirdFloor2 = true;
                        GameEvent.isAniPlay = false;
                        this.GetComponent<MusicJudgement>().Number = 8;
                        EventNumber = 3;
                        timerSwitch = false;
                        _PlayerData.CommonSave();
                    }
                    break;
                case 2:
                    AniTimer -= Time.deltaTime;
                    if (AniTimer <= (AniTimerSet - 1))
                    {
                        EventNumber = 3;
                        GameEvent.isAniPlay = false;
                        this.GetComponent<MusicJudgement>().Number = 8;
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
