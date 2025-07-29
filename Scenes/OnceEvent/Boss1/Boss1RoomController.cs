using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1RoomController : MonoBehaviour
{
    private GameObject Player;
    private Boss1AniController _roomAniController;
    [HideInInspector] public int Phase;//1 初次進入 2 戰鬥中 3 怪物瀕死 4 打贏 5 吸收
    [HideInInspector] public bool isDoEvent = false;
    [HideInInspector] public bool isBreak = false;//大招是否被打斷
    public static MonsterDeadInformation _deadInfo = new MonsterDeadInformation();
    public GameObject HpUi;
    public Canvas HPCanvas;
    public GameObject Boss1;
    private VeryBigMonsterController _monsterController;
    private PlayerData _PlayerData;
    private ItemManage _itemManage;
    public GameObject RDeadBody;
    public GameObject LDeadBody;
    public SideDoor RDoor;
    public SideDoor LDoor;
    private float _deltaTime;

    private bool Trigger1;

    //入場
    public TouchTrigger FirstMeetBoss;//位置在-3.81
    [SerializeField] private GameObject CommonHead;
    //再次入場
    public TouchTrigger AgainMeetBoss;//位置在-9.95

    //中途
    private float LongChangeTime = 2.2f;
    private float ShortChangeTime = 2f;
    public Animator DarkFlash;
    public GameObject LowPowerSound;
    //勝利
    public Transform HumanBoss;
    private DeadBodyBoss1 _deadBodyScript;

    //吸收
    public GameObject SoulItem;
    private GameObject _newTutorial;
    private Vector3 RPlayerPosition = new Vector3(4.08f, -1.32f, 0);
    private Vector3 LPlayerPosition = new Vector3(-4.08f, -1.32f, 0);

    public float Ani1TimerSet;
    public float Ani3TimerSet;
    public float Ani4TimerSet;
    public float Ani5TimerSet;
    private float AniTimer;

    private void Start()
    {
        if (GameObject.Find("player") != null)
        {
            Player = GameObject.Find("player");
        }
        else
        {
            return;
        }

        if (GameEvent.PassBoss1)
        {
            Destroy(Boss1);
            //Destroy(RDoor.gameObject);
            RDoor.CloseDoor();
            Destroy(LDoor.gameObject);
            Destroy(HpUi);
            Destroy(FirstMeetBoss);
            Destroy(AgainMeetBoss);
            if (_deadInfo.FaceRight)
            {
                Instantiate(RDeadBody, _deadInfo.DiePosition, Quaternion.identity, this.transform);
                _deadBodyScript = this.transform.GetChild(0).GetComponent<DeadBodyBoss1>();
                this.transform.DetachChildren();
            }
            if (_deadInfo.FaceLeft)
            {
                Instantiate(LDeadBody, _deadInfo.DiePosition, Quaternion.identity, this.transform);
                _deadBodyScript = this.transform.GetChild(0).GetComponent<DeadBodyBoss1>();
                this.transform.DetachChildren();
            }
            if (!GameEvent.AbsorbBoss1)
            {
                _deadBodyScript.OpenInteract();
            }
        }
        if (GameEvent.AbsorbBoss1)
        {
            return;
        }
        if (GameObject.Find("FollowSystem") != null)
        {
            _PlayerData = GameObject.Find("FollowSystem").GetComponent<PlayerData>();
            _itemManage = GameObject.Find("FollowSystem").GetComponent<ItemManage>();
        }
        if (GameObject.FindGameObjectWithTag("UI") != null)
        {
            Transform UICanvas;
            UICanvas = GameObject.FindGameObjectWithTag("UI").transform;
            _newTutorial = IdentifyID.FindObject(UICanvas, UIID.NewTutorial);
        }
        if (GameObject.Find("UICamera") != null)
        {
            HPCanvas.worldCamera = GameObject.Find("UICamera").GetComponent<Camera>();
        }

        _monsterController = Boss1.GetComponent<VeryBigMonsterController>();
        _roomAniController = this.GetComponent<Boss1AniController>();

        if (GameEvent.GoInBoss1 && !GameEvent.PassBoss1)
        {
            FirstMeetBoss.gameObject.SetActive(false);
            AgainMeetBoss.gameObject.SetActive(true);
            Boss1.transform.localPosition = new Vector3(-9.95f, -9.11f, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        _deltaTime = Time.deltaTime;

        ControllMeetBoss();

        ControllHumanBossFace();

        ControllBossDie();

        AbsorbBoss1();

        AniTimerMethod();

        return;
    }

    private void ControllMeetBoss()
    {
        if (FirstMeetBoss.isTouch)
        {
            RDoor.CloseDoor();
            LDoor.CloseDoor();
            Boss1.SetActive(true);
            Phase = 1;
            AniTimer = Ani1TimerSet;
            isDoEvent = true;
            GameEvent.isAniPlay = true;
            FirstMeetBoss.isTouch = false;
            FirstMeetBoss.gameObject.SetActive(false);
        }
        if(AgainMeetBoss.isTouch)
        {
            RDoor.CloseDoor();
            LDoor.CloseDoor();
            Phase = 2;
            AgainMeetBoss.isTouch = false;
            AgainMeetBoss.gameObject.SetActive(false);
            HpUi.SetActive(true);
            MusicController.PlayBGM(4);
            MusicController.BeginFadeInBGM();
            Boss1.SetActive(true);
            _monsterController.BeginWalk();
        }
    }

    private void ControllHumanBossFace()
    {
        if (PlayerController.PlayerPlaceX > HumanBoss.position.x)
        {
            HumanBoss.localScale = new Vector3(-1, 1, 0);
        }
        else
        {
            HumanBoss.localScale = new Vector3(1, 1, 0);
        }
    }

    public void BeginPhaseChange(int status)//大嘴砲轉場
    {
        switch (status)
        {
            case 2:
                MusicController.BeginFadeOutBGM(1, 0.1f);
                isDoEvent = true;
                isBreak = false;
                AniTimer = LongChangeTime;
                DarkFlash.gameObject.SetActive(true);
                Instantiate(LowPowerSound);
                break;
            case 3:
                isDoEvent = true;
                isBreak = true;
                AniTimer = ShortChangeTime;
                break;
        }
    }

    private void ControllBossDie()
    {
        if (Boss1 == null && !GameEvent.PassBoss1)
        {
            GameEvent.PassBoss1 = true;
            AniTimer = Ani3TimerSet;
            Phase = 3;
            isDoEvent = true;
            _PlayerData.CommonSave();
        }
    }

    private void AbsorbBoss1()
    {
        if (_deadBodyScript != null && _deadBodyScript.BeginAbsorb)
        {
            _deadBodyScript.BeginAbsorb = false;
            if (_deadInfo.FaceRight)
            {
                Player.transform.position = _deadInfo.DiePosition + RPlayerPosition;
            }
            if (_deadInfo.FaceLeft)
            {
                Player.transform.position = _deadInfo.DiePosition + LPlayerPosition;
            }
            Phase = 5;
            isDoEvent = true;
            GameEvent.isAniPlay = true;
            AniTimer = Ani5TimerSet;
        }
    }

    private void AniTimerMethod()
    {
        if (isDoEvent)
        {
            switch (Phase)
            {
                case 1:
                    AniTimer -= _deltaTime;
                    if (AniTimer <= 0)
                    {
                        HpUi.SetActive(true);
                        CommonHead.SetActive(true);
                        Phase = 2;
                        GameEvent.GoInBoss1 = true;
                        GameEvent.isAniPlay = false;
                        isDoEvent = false;
                        _PlayerData.CommonSave();
                    }
                    break;
                case 2:
                    AniTimer -= _deltaTime;
                    if (!isBreak)
                    {
                        if (AniTimer <= 0)
                        {
                            _roomAniController.WindowOpen();
                            DarkFlash.SetBool("End", true);
                            MusicController.BeginFadeInBGM(0.2f, 1);
                            isDoEvent = false;
                        }
                    }
                    else
                    {
                        if (AniTimer <= (ShortChangeTime - 1.4))
                        {
                            if (!Trigger1)
                            {
                                DarkFlash.gameObject.SetActive(true);
                                Instantiate(LowPowerSound);
                                Trigger1 = true;
                            }
                        }
                        if (AniTimer <= 0)
                        {
                            _roomAniController.WindowOpen();
                            DarkFlash.SetBool("End", true);
                            Trigger1 = false;
                            isDoEvent = false;
                        }
                    }
                    break;
                case 3:
                    AniTimer -= _deltaTime;
                    if (AniTimer <= 0 && PlayerController.isGround)
                    {
                        if (_deadInfo.FaceRight)
                        {
                            Instantiate(RDeadBody, _deadInfo.DiePosition, Quaternion.identity, this.transform);
                            _deadBodyScript = this.transform.GetChild(0).GetComponent<DeadBodyBoss1>();
                            this.transform.DetachChildren();
                        }
                        if (_deadInfo.FaceLeft)
                        {
                            Instantiate(LDeadBody, _deadInfo.DiePosition, Quaternion.identity, this.transform);
                            _deadBodyScript = this.transform.GetChild(0).GetComponent<DeadBodyBoss1>();
                            this.transform.DetachChildren();
                        }
                        GameEvent.isAniPlay = true;
                        MusicController.BeginFadeOutBGM(1, 0.3f);
                        Phase = 4;
                        AniTimer = Ani4TimerSet;
                    }
                    break;
                case 4:
                    AniTimer -= _deltaTime;
                    if (AniTimer <= 0)
                    {
                        isDoEvent = false;
                        GameEvent.isAniPlay = false;
                        MusicController.BeginFadeOutBGM(0.3f, 0);
                        _deadBodyScript.OpenInteract();
                        //RDoor.OpenDoor(); 正式版運行
                        LDoor.OpenDoor();
                    }
                    break;
                case 5:
                    AniTimer -= _deltaTime;
                    if (AniTimer <= (Ani5TimerSet - 6.5))
                    {
                        SoulItem.SetActive(true);
                    }
                    if (AniTimer <= 0)
                    {
                        //_newTutorial.SetActive(true);
                        print("正式版出現教學");
                        _itemManage.ItemGet(ItemID.Boss1Soul, 999);
                        GameEvent.KilledBossNumber += 1;
                        TutorialWindow.TutorialGettingNumber += 1;
                        isDoEvent = false;
                        GameEvent.isAniPlay = false;
                        GameEvent.AbsorbBoss1 = true;
                        _PlayerData.CommonSave();
                    }
                    break;
            }
        }
    }
}
