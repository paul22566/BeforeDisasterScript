using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretRoomController : MonoBehaviour
{
    private PlayerData _PlayerData;
    private ItemManage _itemManage;
    private BattleSystem _battleSystem;
    [HideInInspector] public int Status;//0: 待命 1: 魔力量低 2: 魔力量高

    //隱藏門
    private bool isOpenDoor;
    private float DoorTimer = 3.25f;
    private GameObject DoorSE;
    public InteractableObject HiddenDoorSwitch;
    private Animator SwitchAni;
    private GameObject HasOpenSwitch;
    public Transform LeftHiddenDoor;
    private GameObject LeftHiddenDoorCover;
    private GameObject LeftHiddenDoorJudgement;
    public Transform RightHiddenDoor;
    private GameObject RightHiddenDoorJudgement;
    private Animator RightHiddenDoorAni;

    //觸發hint
    public TouchTrigger UndeadHintTrigger;
    public Animator HintAni;
    //獲取不死蛇
    public InteractableObject Altar;
    public GameObject UndeadSnakeItemImage;
    //蛇神嘶吼
    public GameObject HowlSound;

    private bool Trigger1;
    private float _fixedDeltaTime;
    [HideInInspector] public bool isDoEvent;
    private float AniTimer = 0;
    private float Ani1TimerSet = 11.6f;
    private float Ani2TimerSet = 12.6f;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("player") != null)
        {
            _battleSystem = GameObject.Find("player").GetComponent<BattleSystem>();
        }
        else
        {
            return;
        }

        if (GameObject.Find("FollowSystem") != null)
        {
            _PlayerData = GameObject.Find("FollowSystem").GetComponent<PlayerData>();
            _itemManage = GameObject.Find("FollowSystem").GetComponent<ItemManage>();
        }

        SwitchAni = HiddenDoorSwitch.transform.GetChild(0).GetComponent<Animator>();
        HasOpenSwitch = HiddenDoorSwitch.transform.GetChild(2).gameObject;

        LeftHiddenDoorCover = LeftHiddenDoor.GetChild(1).gameObject;
        LeftHiddenDoorJudgement = LeftHiddenDoor.GetChild(0).gameObject;
        RightHiddenDoorJudgement = RightHiddenDoor.GetChild(0).gameObject;
        RightHiddenDoorAni = RightHiddenDoor.GetComponent<Animator>();
        DoorSE = RightHiddenDoor.transform.GetChild(2).gameObject;

        if (GameEvent.OpenSecretRoom1ShortCut)
        {
            HiddenDoorSwitch.isValidable = false;
            SwitchAni.gameObject.SetActive(false);
            HasOpenSwitch.SetActive(true);

            LeftHiddenDoorCover.SetActive(false);
            LeftHiddenDoorJudgement.SetActive(true);
            RightHiddenDoorAni.SetInteger("Status", 2);
            RightHiddenDoorJudgement.SetActive(true);
        }

        if (GameEvent.ReadUndeadSnakeHint)
        {
            Destroy(UndeadHintTrigger.gameObject);
        }

        HiddenDoorSwitch._interact += OnInteractSwitch;
        Altar._interact += OnInteractAltar;
        if (ItemManage.CheckItemExist(ItemID.UnDeadSnake) || ItemManage.CheckItemExist(ItemID.WeakUnDeadSnake))
        {
            Altar.isValidable = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ControllUndeadHint();
    }

    private void FixedUpdate()
    {
        _fixedDeltaTime = Time.fixedDeltaTime;

        TimerMethod();
    }

    private void ControllUndeadHint()
    {
        if (UndeadHintTrigger.isTouch)
        {
            UndeadHintTrigger.isTouch = false;
            UndeadHintTrigger.gameObject.SetActive(false);
            HintAni.SetBool("Shake", true);
            _PlayerData.NoSignSave();
        }
    }

    private void OnInteractSwitch()
    {
        isOpenDoor = true;
        SwitchAni.SetBool("Open", true);
        HiddenDoorSwitch.OnceTimeInteractSuccess();
        GameEvent.OpenSecretRoom1ShortCut = true;
        _PlayerData.CommonSave();
    }

    private void OnInteractAltar()
    {
        Altar.OnceTimeInteractSuccess();
        isDoEvent = true;
        if (_battleSystem.SkillPower > 600)
        {
            AniTimer = Ani2TimerSet;
            Status = 2;
        }
        else
        {
            AniTimer = Ani1TimerSet;
            Status = 1;
        }
        MusicController.BeginFadeOutBGM(0.5f, 0.3f);
        GameEvent.isAniPlay = true;
    }

    private void TimerMethod()
    {
        if (isOpenDoor)
        {
            DoorTimer -= _fixedDeltaTime;

            if (DoorTimer <= 2.25f)
            {
                DoorSE.SetActive(true);
                RightHiddenDoorAni.SetInteger("Status", 1);
            }

            if (DoorTimer <= 0)
            {
                isOpenDoor = false;
                LeftHiddenDoorCover.SetActive(false);
                LeftHiddenDoorJudgement.SetActive(true);
                RightHiddenDoorAni.SetInteger("Status", 2);
                RightHiddenDoorJudgement.SetActive(true);
                SwitchAni.gameObject.SetActive(false);
                HasOpenSwitch.SetActive(true);
            }
        }

        if (isDoEvent)
        {
            switch (Status)
            {
                case 1:
                    AniTimer -= _fixedDeltaTime;

                    if (AniTimer <= (Ani1TimerSet - 9.6))
                    {
                        if (!Trigger1)
                        {
                            _itemManage.ItemGet(ItemID.UnDeadSnake, 999);
                            _PlayerData.CommonSave();
                            UndeadSnakeItemImage.SetActive(true);
                            Trigger1 = true;
                        }
                    }

                    if (AniTimer <= 0)
                    {
                        isDoEvent = false;
                        MusicController.BeginFadeInBGM();
                        GameEvent.isAniPlay = false;
                        Instantiate(HowlSound);
                        Status = 0;
                    }
                    break;
                case 2:
                    AniTimer -= _fixedDeltaTime;

                    if (AniTimer <= (Ani2TimerSet - 9.6))
                    {
                        if (!Trigger1)
                        {
                            _itemManage.ItemGet(ItemID.UnDeadSnake, 999);
                            _PlayerData.CommonSave();
                            UndeadSnakeItemImage.SetActive(true);
                            Trigger1 = true;
                        }
                    }

                    if (AniTimer <= 0)
                    {
                        isDoEvent = false;
                        MusicController.BeginFadeInBGM();
                        GameEvent.isAniPlay = false;
                        Instantiate(HowlSound);
                        Status = 0;
                    }
                    break;
            }
        }
    }
}
