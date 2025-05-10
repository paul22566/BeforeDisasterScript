using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class RestRoomController : MonoBehaviour
{
    public static int RestRoomKilledNumber = 0;//休息室死亡人數   其他script有用到(RestRoomEnemy，leftDoor，RightDoor，EnemyEscape)
    private bool DoorOpen;
    private Transform PlayerTransform;
    [HideInInspector] public int EventNumber;//1初次左側進入 2再次左側 3初次右側 4再次右側 5正常進入戰鬥中 6背面進入戰鬥中
    private bool isDoEvent = false;
    private float AniTimer;
    private float Ani1TimerSet = 7.3f;
    private float Ani2TimerSet = 2f;
    private float Ani3TimerSet = 19f;
    private float Ani4TimerSet = 2f;
    private BackgroundSystem _system;
    private PlayerData _PlayerData;
    public InteractableObject Announcement2;
    private float MusicRate = 0.6f;
    public CameraController _cameraController;

    public SummonMonsterController RightSummonDoor;
    public SummonMonsterController LeftSummonDoor;
    public SummonMonsterController SummonArea;
    private Vector3 SummonPlace;
    private int SummonNumber = 1;

    public GameObject Monster1;
    public GameObject Monster2;
    public GameObject Door1;
    public GameObject Door2;
    public GameObject MonsterBegin1;
    public GameObject MonsterBegin2;
    public SideDoor RDoor;
    public SideDoor LDoor;
    public GameObject Letter;

    [HideInInspector] public Transform EscapeEnemy;
    private bool EscapeEnemyAppear;

    private bool Trigger1 = false;
    private bool Trigger2 = false;

    [HideInInspector] public Vector3 Monster1DiePosition = new Vector3();
    // Start is called before the first frame update
    private void Awake()
    {
        if (!GameEvent.PassRestRoom && !GameEvent.PassBoss1)
        {
            isDoEvent = true;
            GameEvent.isAniPlay = true;
            EventNumber = 1;
            AniTimer = Ani1TimerSet;
            if (GameEvent.SkipRestRoom)
            {
                EventNumber = 3;
                AniTimer = Ani3TimerSet;
            }
            if (GameEvent.GoRestRoom)
            {
                if (BackgroundSystem.startPointNumber == 1)
                {
                    EventNumber = 2;
                    AniTimer = Ani2TimerSet;
                }
                if (BackgroundSystem.startPointNumber == 2)
                {
                    EventNumber = 4;
                    AniTimer = Ani4TimerSet;
                }
            }
        }
    }

    private void Start()
    {
        if (GameEvent.PassRestRoom || GameEvent.PassBoss1)
        {
            Destroy(Monster1);
            Destroy(Monster2);
            Destroy(Door1);
            Destroy(Door2);
            if (!itemManage.CheckDocumentExist(4) && GameEvent.SkipRestRoom)
            {
                Letter.SetActive(true);
            }
            return;
        }

        Announcement2.isValidable = false;
        RestRoomKilledNumber = 0;
        _system = this.GetComponent<BackgroundSystem>();
        if (GameObject.Find("FollowSystem") != null)
        {
            _PlayerData = GameObject.Find("FollowSystem").GetComponent<PlayerData>();
        }
        if (GameObject.Find("player") != null)
        {
            PlayerTransform = GameObject.Find("player").transform;
        }
        if (PlayerTransform)
        {
            if (EventNumber == 1)
            {
                MusicController.PlayBGM(12);
                MusicController.BeginFadeInBGM();
                GameObject g = GameObject.Find("0") as GameObject;
                if (g != null)
                {
                    PlayerTransform.position = g.transform.position;
                }
            }
            if (EventNumber == 2)
            {
                GameObject g = GameObject.Find("0") as GameObject;
                if (g != null)
                {
                    PlayerTransform.position = g.transform.position;
                }
            }
            if (EventNumber == 3)
            {
                MusicController.PlayBGM(12);
                MusicController.BeginFadeInBGM();
                GameObject g = GameObject.Find("01") as GameObject;
                if (g != null)
                {
                    PlayerTransform.position = g.transform.position;
                }
            }
            if (EventNumber == 4)
            {
                GameObject g = GameObject.Find("01") as GameObject;
                if (g != null)
                {
                    PlayerTransform.position = g.transform.position;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isDoEvent)
        {
            if (EventNumber == 5)
            {
                if (RestRoomKilledNumber >= 2 && SummonNumber == 1)
                {
                    RightSummonDoor.Summon();
                    SummonNumber += 1;
                }
                if (RestRoomKilledNumber >= 3 && SummonNumber == 2)
                {
                    SummonPlace = new Vector3(PlayerTransform.localPosition.x, PlayerTransform.localPosition.y + 1000, 0);
                    if (PlayerTransform.localPosition.x > _system.RightEdgeX - 1)
                    {
                        SummonPlace = new Vector3(SummonPlace.x - 1, SummonPlace.y, 0);
                    }
                    if (PlayerTransform.localPosition.x < _system.LeftEdgeX + 1)
                    {
                        SummonPlace = new Vector3(SummonPlace.x + 1, SummonPlace.y, 0);
                    }
                    SummonArea.SpecialAssignAppearPlace(1, SummonPlace);
                    SummonArea.Summon();
                    SummonNumber += 1;
                }
                if (RestRoomKilledNumber >= 4 && SummonNumber == 3)
                {
                    RightSummonDoor.Summon();
                    LeftSummonDoor.Summon();
                    SummonNumber += 1;
                }
                if (RestRoomKilledNumber >= 5 && SummonNumber == 4)
                {
                    LeftSummonDoor.Summon();
                    SummonNumber += 1;
                }
                if (RestRoomKilledNumber >= 6)
                {
                    if (!DoorOpen)
                    {
                        RDoor.OpenDoor();
                        LDoor.OpenDoor();
                        DoorOpen = true;
                    }
                    GameEvent.PassRestRoom = true;
                    Announcement2.isValidable = true;
                    isDoEvent = false;
                    MusicController.BeginFadeOutBGM();
                    _PlayerData.CommonSave();
                }
            }
            if (EventNumber == 6)
            {
                if (RestRoomKilledNumber >= 2 && SummonNumber == 1)
                {
                    RightSummonDoor.Summon();
                    SummonNumber += 1;
                }
                if (RestRoomKilledNumber >= 3 && SummonNumber == 2)
                {
                    RightSummonDoor.Summon();
                    LeftSummonDoor.Summon();
                    SummonNumber += 1;
                }
                if (RestRoomKilledNumber >= 4 && SummonNumber == 3)
                {
                    LeftSummonDoor.Summon();
                    SummonNumber += 1;
                }
                if (RestRoomKilledNumber >= 5)
                {
                    if (!DoorOpen)
                    {
                        RDoor.OpenDoor();
                        LDoor.OpenDoor();
                        DoorOpen = true;
                    }
                    GameEvent.PassRestRoom = true;
                    Announcement2.isValidable = true;
                    isDoEvent = false;
                    Letter.SetActive(true);
                    Letter.transform.localPosition = new Vector3(Monster1DiePosition.x, -4.37f, Monster1DiePosition.z);
                    MusicController.BeginFadeOutBGM();
                    _PlayerData.CommonSave();
                }
            }
        }

        EscapeEnemyControll();
    }

    private void FixedUpdate()
    {
        if (isDoEvent)
        {
            if (EventNumber < 5)
            {
                Timer();
            }
        }
    }

    void Timer()
    {
        switch (EventNumber)
        {
            case 1:
                AniTimer -= Time.fixedDeltaTime;
                if (AniTimer <= (Ani1TimerSet - 2.55))
                {
                    if (!Trigger1)
                    {
                        MusicController.BeginFadeOutBGM(3, 0);
                        Trigger1 = true;
                    }
                }
                if (AniTimer <= 0.5f)
                {
                    MusicController.PlayBGM(7);
                    MusicController.BeginFadeInBGM(0.5f, MusicRate);
                }
                if (AniTimer <= 0)
                {
                    GameEvent.GoRestRoom = true;
                    GameEvent.isAniPlay = false;
                    EventNumber = 5;
                    MonsterBegin1.SetActive(false);
                    MonsterBegin2.SetActive(false);
                    _PlayerData.CommonSave();
                }
                break;
            case 2:
                AniTimer -= Time.fixedDeltaTime;
                if (AniTimer <= 0)
                {
                    EventNumber = 5;
                    MusicController.PlayBGM(7);
                    MusicController.BeginFadeInBGM(0.5f, MusicRate);
                    GameEvent.isAniPlay = false;
                }
                break;
            case 3:
                AniTimer -= Time.fixedDeltaTime;
                if (AniTimer <= (Ani3TimerSet - 2.7) && !Trigger1)
                {
                    Trigger1 = true;
                    MusicController.BeginFadeOutBGM(2, 0.2f);
                }
                if (AniTimer <= (Ani3TimerSet - 16) && !Trigger2)
                {
                    Trigger2 = true;
                    MusicController.BeginFadeOutBGM(3, 0);
                }
                if (AniTimer <= 0.5f)
                {
                    MusicController.PlayBGM(7);
                    MusicController.BeginFadeInBGM(0.5f, MusicRate);
                }
                if (AniTimer <= 0)
                {
                    GameEvent.GoRestRoom = true;
                    GameEvent.isAniPlay = false;
                    EventNumber = 6;
                    MonsterBegin1.SetActive(false);
                    MonsterBegin2.SetActive(false);
                    _PlayerData.CommonSave();
                }
                break;
            case 4:
                AniTimer -= Time.fixedDeltaTime;
                if (AniTimer <= 0)
                {
                    EventNumber = 6;
                    MusicController.PlayBGM(7);
                    MusicController.BeginFadeInBGM(0.5f, MusicRate);
                    GameEvent.isAniPlay = false;
                }
                break;
        }
    }

    private void EscapeEnemyControll()
    {
        if (EscapeEnemy == null)
        {
            if (EscapeEnemyAppear)
            {
                _cameraController.ChangeCameraSize(-4.5f, 7, Time.deltaTime);
            }
            return;
        }

        EscapeEnemyAppear = true;
        _cameraController.MovingChangeCameraSize(EscapeEnemy, 1.5f, Time.deltaTime);
    }
}
