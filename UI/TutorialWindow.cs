using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TutorialWindow : MonoBehaviour
{
    private Transform _transform;
    private PlayerData _playerData;

    public GameObject DefaultTutorial;

    private GameObject TutorialScrollbar;
    private Scrollbar _tutorialScrollbar;
    private GameObject TutorialShowArea;
    private GameObject TutorialShowAreaBottom;
    private RectTransform _tutorialShowAreaTransform;

    [HideInInspector] public bool isTutorialOpen;//script(pauseMenuController)
    [HideInInspector] public int TutorialWindowNumber = 999;//定999為預設無指定
    [HideInInspector] public bool isTutorialButtonAppear;//script(pauseMenuController)
    [HideInInspector] public bool isTutorialDetailAppear;//script(pauseMenuController，TutorialDetail)

    public static int TotalTutorialNumber = 6;
    //需存檔變數
    public static int TutorialGettingNumber = 3;//script(playerData)
    public static List<bool> TutorialGetList = new List<bool>();
    public static List<bool> TutorialReadList = new List<bool>();

    //欄位Button
    public GameObject BasicKeyBoardButton;
    public GameObject AtkSystemButton;
    public GameObject BlockSystemButton;
    public GameObject ThrowSystemButton;
    public GameObject StrongThrowSystemButton;
    public GameObject BigGunSystemButton;

    //此列表排序為所有教學(遊戲初始就會固定)
    private List<TutorialPage> TutorialPageList = new List<TutorialPage>();

    [HideInInspector] public List<int> TutorialButtonOrderRecord = new List<int>();//script(TutorialButton，pauseMenuController)

    public Transform Place1;
    public Transform Place2;

    [HideInInspector] public Vector3 OrderOnePlace;//script(TutorialButton)
    [HideInInspector] public Vector3 ButtonDistance;//script(TutorialButton)

    private float TenItemHeight = 54.696f;
    private float HeightDistance = 73;
    private float NowScrollbarDistance;//現在長度下每次滾軸移動數值
    private int NowUpLimit = 1;//當前的選項頂點
    private int NowDownLimit = 3;//當前的選項低點
    private bool isLimitReset;

    private bool ShouldPlayMoveSound;
    private bool ShouldPlayConfirmSound;
    private bool ShouldLag;
    private float MoveLagTimerSet = 0.1f;
    private float MoveLagTimer;
    private float _unScaleDeltaTime;

    private bool ShouldSave;

    // Start is called before the first frame update
    void Start()
    {
        _transform = this.transform;
        _playerData = GameObject.Find("FollowSystem").GetComponent<PlayerData>();
        TutorialScrollbar = this.transform.GetChild(0).GetChild(1).gameObject;
        _tutorialScrollbar = TutorialScrollbar.GetComponent<Scrollbar>();
        TutorialShowArea = this.transform.GetChild(0).GetChild(0).gameObject;
        TutorialShowAreaBottom = TutorialShowArea.transform.GetChild(0).gameObject;
        _tutorialShowAreaTransform = TutorialShowAreaBottom.GetComponent<RectTransform>();

        MoveLagTimer = MoveLagTimerSet;

        InisializeTutorialPage();

        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        _unScaleDeltaTime = Time.unscaledDeltaTime * BackgroundSystem.BasicGameSpeed;
        if (isTutorialDetailAppear)
        {
            return;
        }
        if (!isTutorialOpen)
        {
            if (TutorialButton.isDestroy)
            {
                this.gameObject.SetActive(false);
            }
            return;
        }

        TutorialButton.isDestroy = false;

        OrderOnePlace = Place1.localPosition;
        ButtonDistance = new Vector3(Place1.localPosition.x, Place2.localPosition.y - Place1.localPosition.y, 0);

        //決定size
        if (TutorialGettingNumber > 10)
        {
            NowScrollbarDistance = 1 / (TutorialGettingNumber - 10);
            _tutorialShowAreaTransform.sizeDelta = new Vector2(_tutorialShowAreaTransform.sizeDelta.x, TenItemHeight + (TutorialGettingNumber - 10) * HeightDistance);
        }

        //產生按鈕
        ButtonSet();

        //new關閉偵測
        CloseReadNotice();

        //打開教學
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            OpenTutorialDetail(TutorialPageList[TutorialWindowNumber].Text);
        }
        //選項移動
        if (TutorialButton.NowTutorialButton > 1 && !ShouldLag)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || XboxControllerDetect.isControllerUpPressed || XboxControllerDetect.isCrossUpPressed)
            {
                ShouldLag = true;
                ShouldPlayMoveSound = true;
                TutorialButton.NowTutorialButton -= 1;
            }
        }
        if (TutorialButton.NowTutorialButton < TutorialGettingNumber && !ShouldLag)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow) || XboxControllerDetect.isControllerDownPressed || XboxControllerDetect.isCrossDownPressed)
            {
                ShouldLag = true;
                ShouldPlayMoveSound = true;
                TutorialButton.NowTutorialButton += 1;
            }
        }

        if (ShouldPlayMoveSound)
        {
            SelectButtonController.ItemMoveSoundPlay();
            ShouldPlayMoveSound = false;
        }
        if (ShouldPlayConfirmSound)
        {
            SelectButtonController.ConfirmSoundPlay();
            ShouldPlayConfirmSound = false;
        }

        //道具滾軸
        if (TutorialGettingNumber > 10)
        {
            if (!isLimitReset)
            {
                _tutorialScrollbar.value = 1;
                NowUpLimit = 1;
                NowDownLimit = 10;
                isLimitReset = true;
            }
            if (TutorialButton.NowTutorialButton > NowDownLimit)
            {
                _tutorialScrollbar.value = 1 - NowScrollbarDistance * (TutorialButton.NowTutorialButton - 10);
                NowDownLimit = TutorialButton.NowTutorialButton;
                NowUpLimit = NowDownLimit - 9;
            }
            if (TutorialButton.NowTutorialButton < NowUpLimit)
            {
                _tutorialScrollbar.value = 1 - (NowUpLimit - 2) * NowScrollbarDistance;
                NowUpLimit -= 1;
                NowDownLimit = NowUpLimit + 9;
            }
        }
        else
        {
            _tutorialScrollbar.value = 1;
        }

        LagTimer();
    }

    private void InisializeTutorialPage()
    {
        for (int i = 0; i < TotalTutorialNumber; i++)
        {
            GameObject TemporyButton = new GameObject();
            switch (i)
            {
                case 0:
                    TemporyButton = BasicKeyBoardButton;
                    break;
                case 1:
                    TemporyButton = AtkSystemButton;
                    break;
                case 2:
                    TemporyButton = BlockSystemButton;
                    break;
                case 3:
                    TemporyButton = ThrowSystemButton;
                    break;
                case 4:
                    TemporyButton = StrongThrowSystemButton;
                    break;
                case 5:
                    TemporyButton = BigGunSystemButton;
                    break;
            }
            //i 的位置必須確實有物件在
            TutorialPageList.Add(new TutorialPage(TemporyButton, _transform.GetChild(1).GetChild(i).gameObject));
            //指定ButtonOrder和決定是否關閉ReadNotice
            if (TutorialPageList[i].Button.GetComponent<TutorialButton>() != null)
            {
                TutorialPageList[i].Button.GetComponent<TutorialButton>().ButtonOrder = i;
                if (TutorialReadList[i])
                {
                    TutorialPageList[i].Button.GetComponent<TutorialButton>().TurnOffReadNotice();
                }
            }
        }
    }

    private void ButtonSet()//道具按鈕的判斷與生成(1)
    {
        if (!isTutorialButtonAppear)
        {
            int TutorialListOrder = 0;
            //決定要顯示幾個道具
            for (int i = 0; i < TutorialGettingNumber; i++)
            {
                TutorialButtonOrderRecord.Add(0);
            }
            //登記序號
            for (int i = 0; i < TutorialGettingNumber; i++)
            {
                bool AssignSuccess = false;
                while (!AssignSuccess)
                {
                    if (TutorialGetList[TutorialListOrder])
                    {
                        TutorialButtonOrderRecord[i] = TutorialListOrder;
                        AssignSuccess = true;
                    }
                    TutorialListOrder++;
                }
            }
            //實際產出
            for (int i = 0; i < TutorialGettingNumber; i++)
            {
                ProduceButton(TutorialPageList[TutorialButtonOrderRecord[i]].Button, i, TutorialShowAreaBottom);
            }
            isTutorialButtonAppear = true;
        }
    }

    private void ProduceButton(GameObject Button, int Order, GameObject ShowArea)
    {
        Button.GetComponent<TutorialButton>().NowOrder = Order;
        Instantiate(Button, _transform.position, Quaternion.identity, ShowArea.transform);
    }

    private void OpenTutorialDetail(GameObject Detail)
    {
        Detail.SetActive(true);
        ShouldPlayConfirmSound = true;
        isTutorialDetailAppear = true;
    }

    private void LagTimer()
    {
        if (ShouldLag)
        {
            MoveLagTimer -= _unScaleDeltaTime;
            if (MoveLagTimer <= 0)
            {
                ShouldLag = false;
                MoveLagTimer = MoveLagTimerSet;
            }
        }
    }

    private void OpenDefaultTutorial()
    {
        DefaultTutorial.SetActive(true);
    }

    private void CloseReadNotice()
    {
        if (TutorialWindowNumber != 999)
        {
            //關閉存放區的New
            if (!TutorialReadList[TutorialWindowNumber])
            {
                ShouldSave = true;
                TutorialReadList[TutorialWindowNumber] = true;
                TutorialPageList[TutorialWindowNumber].Button.GetComponent<TutorialButton>().TurnOffReadNotice();
            }
        }
    }

    public static void TutorialListReset()
    {
        TutorialGettingNumber = 3;

        TutorialGetList.Clear();
        TutorialReadList.Clear();
        for (int i = 0; i < TotalTutorialNumber; i++)
        {
            TutorialGetList.Add(false);
            TutorialReadList.Add(false);
        }

        TutorialGetList[0] = true;
        TutorialGetList[1] = true;
        TutorialGetList[2] = true;

        TutorialReadList[0] = true;
    }

    public void SaveReadStatus()
    {
        if (ShouldSave)
        {
            _playerData.NoSignSave();
            ShouldSave = false;
        }
    }

    public void TutorialGet(int TutorialID)
    {
        TutorialGetList[TutorialID] = true;
        TutorialGettingNumber += 1;
        OpenDefaultTutorial();
    }//本身無存檔功能，跟隨Item撿到的話，從RoomController操作
}

public struct TutorialPage
{
    public GameObject Button;
    public GameObject Text;

    public TutorialPage(GameObject button, GameObject text)
    {
        Button = button;
        Text = text;
    }
}
