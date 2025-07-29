using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class ItemWindow : MonoBehaviour
{
    private Transform _transform;
    private PlayerData _playerData;

    [HideInInspector] public bool isItemWindowAppear;//script(playerController，pauseMenuController)
    [HideInInspector] public bool isDocumentDetailAppear;//script(pauseMenuController，DocumentDetail)
    public enum Status { Item, Document }
    [HideInInspector] public Status status;//script(ItemButton，pauseMenuController)
    private GameObject ItemScrollbar;
    private Scrollbar _itemScrollbar;
    private GameObject DocumentScrollbar;
    private Scrollbar _documentScrollbar;
    private GameObject ItemShowArea;
    private GameObject DocumentShowArea;
    private GameObject ItemShowAreaBottom;
    private GameObject DocumentShowAreaBottom;
    private GameObject ItemTitle;
    private GameObject DocumentTitle;
    private RectTransform _itemShowAreaTransform;
    private RectTransform _documentShowAreaTransform;
    private Transform ItemTextArea;
    private Transform DocumentTextArea;

    //預視視窗顯示選項 定999為預設不顯示
    [HideInInspector] public int ItemDisplayWindowNumber = 999;//script(ItemButton，pauseMenuController)
    [HideInInspector] public int DocumentDisplayWindowNumber = 999;//script(ItemButton，pauseMenuController)

    [HideInInspector] public bool isItemButtonAppear;//script(pauseMenuController)
    [HideInInspector] public bool isDocumentButtonAppear;//script(pauseMenuController)

    //道具Button
    public GameObject SwordButton;
    public GameObject GunButton;
    public GameObject RestoreItemButton;
    public GameObject CocktailButton;
    public GameObject ExplosionBottleButton;
    public GameObject Boss1SoulButton;
    public GameObject Boss2SoulButton;
    public GameObject Boss3SoulButton;
    public GameObject UnDeadSnakeButton;
    public GameObject WeakUnDeadSnakeButton;
    public GameObject ReBornSnakeButton;
    public GameObject WeakReBornSnakeButton;
    public GameObject SharpenerButton;
    public GameObject CashButton;
    public GameObject ControllerRoomKeyButton;
    public GameObject UnderGroundKeyButton;
    public GameObject FinalKeyButton;
    public GameObject RitualSwordButton;
    //文件Button
    public GameObject Document0Button;
    public GameObject Document1Button;
    public GameObject Document2Button;
    public GameObject Document3Button;
    public GameObject Document4Button;
    public GameObject Document5Button;

    //文件Detail
    public GameObject Document0Detail;
    public GameObject Document1Detail;
    public GameObject Document2Detail;
    public GameObject Document3Detail;
    public GameObject Document4Detail;
    public GameObject Document5Detail;

    private Transform DetailShowArea;
    private GameObject NowShowDocumentDetail;

    //此列表排序為所有道具(遊戲初始就會固定)
    private List<ItemPage> ItemPageList = new List<ItemPage>();
    private List<ItemPage> DocumentPageList = new List<ItemPage>();

    //此列表排序為實際獲得的道具(會變動)
    [HideInInspector] public List<int> ItemButtonOrderRecord = new List<int>();//script(ItemButton，pauseMenuController)
    [HideInInspector] public List<int> DocumentButtonOrderRecord = new List<int>();//script(ItemButton，pauseMenuController)

    public Transform Place1;
    public Transform Place2;
    public Transform Place3;
    [HideInInspector] public Vector3 OrderOnePlace;//script(ItemButton)
    [HideInInspector] public Vector3 ButtonDistance;//script(ItemButton)

    private float NineItemHeight = 651.274f;
    private float HeightDistance = 71;
    private float NowScrollbarDistance;//現在長度下每次滾軸移動數值
    private int NowUpLimit = 1;//當前的選項頂點
    private int NowDownLimit = 3;//當前的選項低點
    private bool isLimitReset;

    private bool ShouldPlayMoveSound;
    private bool ShouldPlayConfirmSound;
    private bool ShouldLag;
    private float MoveLagTimerSet = 0.1f;
    private float MoveLagTimer;
    private float _deltaTime;

    private bool ShouldSave;

    // Start is called before the first frame update
    void Start()
    {
        _transform = this.transform;
        _playerData = GameObject.Find("FollowSystem").GetComponent<PlayerData>();

        status = Status.Item;
        ItemTitle = _transform.GetChild(1).GetChild(0).gameObject;
        DocumentTitle = _transform.GetChild(1).GetChild(1).gameObject;
        ItemScrollbar = _transform.GetChild(1).GetChild(3).gameObject;
        _itemScrollbar = ItemScrollbar.GetComponent<Scrollbar>();
        DocumentScrollbar = _transform.GetChild(1).GetChild(5).gameObject;
        _documentScrollbar = DocumentScrollbar.GetComponent<Scrollbar>();
        ItemShowArea = _transform.GetChild(1).GetChild(2).gameObject;
        DocumentShowArea = _transform.GetChild(1).GetChild(4).gameObject;
        ItemShowAreaBottom = ItemShowArea.transform.GetChild(0).gameObject;
        DocumentShowAreaBottom = DocumentShowArea.transform.GetChild(0).gameObject;
        _itemShowAreaTransform = ItemShowAreaBottom.GetComponent<RectTransform>();
        _documentShowAreaTransform = DocumentShowAreaBottom.GetComponent<RectTransform>();
        DetailShowArea = _transform.GetChild(2);
        ItemTextArea = _transform.GetChild(0).GetChild(0);
        DocumentTextArea = _transform.GetChild(0).GetChild(1);

        MoveLagTimer = MoveLagTimerSet;

        InisializeItemPage();

        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        _deltaTime = Time.deltaTime;

        //關閉視窗
        if (PlayerController.isHurted)
        {
            BackgroundSystem.CantPause = false;
            PauseMenuController.OpenAnyMenu = false;
            isItemWindowAppear = false;
            isDocumentDetailAppear = false;
            status = Status.Item;
            DocumentDisplayWindowNumber = 999;
            ItemDisplayWindowNumber = 999;
            isDocumentButtonAppear = false;
            isItemButtonAppear = false;
            ItemButton.NowDocumentButton = 1;
            ItemButton.NowItemButton = 1;
            ItemButtonOrderRecord.Clear();
            DocumentButtonOrderRecord.Clear();
            SaveReadStatus();
        }
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            if (isItemWindowAppear && !isDocumentDetailAppear)
            {
                BackgroundSystem.CantPause = false;
                PauseMenuController.OpenAnyMenu = false;
                isItemWindowAppear = false;
                status = Status.Item;
                DocumentDisplayWindowNumber = 999;
                ItemDisplayWindowNumber = 999;
                isDocumentButtonAppear = false;
                isItemButtonAppear = false;
                ItemButton.NowDocumentButton = 1;
                ItemButton.NowItemButton = 1;
                ItemButtonOrderRecord.Clear();
                DocumentButtonOrderRecord.Clear();
                SaveReadStatus();
            }
            if (isDocumentDetailAppear)
            {
                isDocumentDetailAppear = false;
                Destroy(NowShowDocumentDetail);
            }
        }
        if (isDocumentDetailAppear)
        {
            return;
        }
        if (!isItemWindowAppear)
        {
            switch (status)
            {
                case Status.Item:
                    if (ItemButton.isDestroy)
                    {
                        this.gameObject.SetActive(false);
                    }
                    break;
                case Status.Document:
                    if (ItemButton.isDestroy)
                    {
                        this.gameObject.SetActive(false);
                    }
                    break;
            }
            return;
        }

        ItemButton.isDestroy = false;

        //決定size
        if (ItemManage.DocumentGettingNumber > ItemManage.ItemGettingNumber)
        {
            if (ItemManage.DocumentGettingNumber > 9)
            {
                NowScrollbarDistance = 1 / (ItemManage.DocumentGettingNumber - 9);
                _itemShowAreaTransform.sizeDelta = new Vector2(_itemShowAreaTransform.sizeDelta.x, NineItemHeight + (ItemManage.DocumentGettingNumber - 9) * HeightDistance);
                _documentShowAreaTransform.sizeDelta = new Vector2(_documentShowAreaTransform.sizeDelta.x, NineItemHeight + (ItemManage.DocumentGettingNumber - 9) * HeightDistance);
            }
        }
        else
        {
            if (ItemManage.ItemGettingNumber > 9)
            {
                NowScrollbarDistance = 1 / (ItemManage.ItemGettingNumber - 9);
                _itemShowAreaTransform.sizeDelta = new Vector2(_itemShowAreaTransform.sizeDelta.x, NineItemHeight + (ItemManage.ItemGettingNumber - 9) * HeightDistance);
                _documentShowAreaTransform.sizeDelta = new Vector2(_documentShowAreaTransform.sizeDelta.x, NineItemHeight + (ItemManage.ItemGettingNumber - 9) * HeightDistance);
            }
        }

        //轉換分類
        switch (status)
        {
            case Status.Item:
                OrderOnePlace = Place1.localPosition;
                ButtonDistance = new Vector3(Place1.localPosition.x, Place2.localPosition.y - Place1.localPosition.y, 0);
                ItemScrollbar.SetActive(true);
                ItemTitle.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1);
                ItemShowArea.SetActive(true);
                DocumentScrollbar.SetActive(false);
                DocumentShowArea.SetActive(false);
                DocumentTitle.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                DocumentButtonOrderRecord.Clear();
                isDocumentButtonAppear = false;
                ItemButton.NowDocumentButton = 1;
                DocumentDisplayWindowNumber = 999;
                AllTextFalse(DocumentPageList);
                break;
            case Status.Document:
                OrderOnePlace = Place1.localPosition;
                ButtonDistance = new Vector3(Place1.localPosition.x, Place2.localPosition.y - Place1.localPosition.y, 0);
                DocumentScrollbar.SetActive(true);
                DocumentShowArea.SetActive(true);
                ItemScrollbar.SetActive(false);
                ItemTitle.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                ItemShowArea.SetActive(false);
                ItemButtonOrderRecord.Clear();
                DocumentTitle.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1);
                isItemButtonAppear = false;
                ItemButton.NowItemButton = 1;
                ItemDisplayWindowNumber = 999;
                AllTextFalse(ItemPageList);
                break;
        }

        //產生按鈕
        ButtonSet();

        //顯示預覽
        ItemPreview();

        if (Input.GetKeyDown(KeyCode.RightArrow) || OldVerXboxControllerDetect.isCrossRightPressed || OldVerXboxControllerDetect.isControllerRightPressed)
        {
            isLimitReset = false;
            if (status == Status.Item)
            {
                ShouldPlayMoveSound = true;
            }
            status = Status.Document;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || OldVerXboxControllerDetect.isCrossLeftPressed || OldVerXboxControllerDetect.isControllerLeftPressed)
        {
            isLimitReset = false;
            if (status == Status.Document)
            {
                ShouldPlayMoveSound = true;
            }
            status = Status.Item;
        }

        //打開文件
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            switch (DocumentDisplayWindowNumber)
            {
                case ItemID.CommandLetter:
                    ProduceDocumentDetail(Document0Detail);
                    break;
                case ItemID.Map:
                    ProduceDocumentDetail(Document1Detail);
                    break;
                case ItemID.Letter1:
                    ProduceDocumentDetail(Document2Detail);
                    break;
                case ItemID.Letter2:
                    ProduceDocumentDetail(Document3Detail);
                    break;
                case ItemID.Letter3:
                    ProduceDocumentDetail(Document4Detail);
                    break;
                case ItemID.Letter4:
                    ProduceDocumentDetail(Document5Detail);
                    break;
            }
        }
        //選項移動
        switch (status)
        {
            case Status.Item:
                if (ItemButton.NowItemButton > 1 && !ShouldLag)
                {
                    if (Input.GetKeyDown(KeyCode.UpArrow) || OldVerXboxControllerDetect.isCrossUpPressed || OldVerXboxControllerDetect.isControllerUpPressed)
                    {
                        ShouldLag = true;
                        ShouldPlayMoveSound = true;
                        ItemButton.NowItemButton -= 1;
                    }
                }
                if (ItemButton.NowItemButton < ItemManage.ItemGettingNumber && !ShouldLag)
                {
                    if (Input.GetKeyDown(KeyCode.DownArrow) || OldVerXboxControllerDetect.isCrossDownPressed || OldVerXboxControllerDetect.isControllerDownPressed)
                    {
                        ShouldLag = true;
                        ShouldPlayMoveSound = true;
                        ItemButton.NowItemButton += 1;
                    }
                }
                break;
            case Status.Document:
                if (ItemButton.NowDocumentButton > 1 && !ShouldLag)
                {
                    if (Input.GetKeyDown(KeyCode.UpArrow) || OldVerXboxControllerDetect.isCrossUpPressed || OldVerXboxControllerDetect.isControllerUpPressed)
                    {
                        ShouldLag = true;
                        ShouldPlayMoveSound = true;
                        ItemButton.NowDocumentButton -= 1;
                    }
                }
                if (ItemButton.NowDocumentButton < ItemManage.DocumentGettingNumber && !ShouldLag)
                {
                    if (Input.GetKeyDown(KeyCode.DownArrow) || OldVerXboxControllerDetect.isCrossDownPressed || OldVerXboxControllerDetect.isControllerDownPressed)
                    {
                        ShouldLag = true;
                        ShouldPlayMoveSound = true;
                        ItemButton.NowDocumentButton += 1;
                    }
                }
                break;
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
        switch (status)
        {
            case Status.Item:
                if (ItemManage.ItemGettingNumber > 9)
                {
                    if (!isLimitReset)
                    {
                        _itemScrollbar.value = 1;
                        NowUpLimit = 1;
                        NowDownLimit = 9;
                        isLimitReset = true;
                    }
                    if (ItemButton.NowItemButton > NowDownLimit)
                    {
                        _itemScrollbar.value = 1 - NowScrollbarDistance * (ItemButton.NowItemButton - 9);
                        NowDownLimit = ItemButton.NowItemButton;
                        NowUpLimit = NowDownLimit - 8;
                    }
                    if (ItemButton.NowItemButton < NowUpLimit)
                    {
                        _itemScrollbar.value = 1 - (NowUpLimit - 2) * NowScrollbarDistance;
                        NowUpLimit -= 1;
                        NowDownLimit = NowUpLimit + 8;
                    }
                }
                else
                {
                    _itemScrollbar.value = 1;
                }
                break;
            case Status.Document:
                if (ItemManage.DocumentGettingNumber > 9)
                {
                    if (!isLimitReset)
                    {
                        _documentScrollbar.value = 1;
                        NowUpLimit = 1;
                        NowDownLimit = 9;
                        isLimitReset = true;
                    }
                    if (ItemButton.NowDocumentButton > NowDownLimit)
                    {
                        _documentScrollbar.value = 1 - NowScrollbarDistance * (ItemButton.NowDocumentButton - 9);
                        NowDownLimit = ItemButton.NowDocumentButton;
                        NowUpLimit = NowDownLimit - 8;
                    }
                    if (ItemButton.NowDocumentButton < NowUpLimit)
                    {
                        _documentScrollbar.value = 1 - (NowUpLimit - 2) * NowScrollbarDistance;
                        NowUpLimit -= 1;
                        NowDownLimit = NowUpLimit + 8;
                    }
                }
                else
                {
                    _documentScrollbar.value = 1;
                }
                break;
        }

        LagTimer();
    }

    private void InisializeItemPage()
    {
        for (int i = 0; i < ItemManage.TotalItemNumber; i++)
        {
            GameObject TemporaryButton = new GameObject();
            GameObject TemporaryText = new GameObject();
            switch (i)
            {
                case ItemID.Sword:
                    TemporaryButton = SwordButton;
                    break;
                case ItemID.Gun:
                    TemporaryButton = GunButton;
                    break;
                case ItemID.RestoreItem:
                    TemporaryButton = RestoreItemButton;
                    break;
                case ItemID.Cocktail:
                    TemporaryButton = CocktailButton;
                    break;
                case ItemID.ExplosionBottle:
                    TemporaryButton = ExplosionBottleButton;
                    break;
                case ItemID.Boss1Soul:
                    TemporaryButton = Boss1SoulButton;
                    break;
                case ItemID.Boss2Soul:
                    TemporaryButton = Boss2SoulButton;
                    break;
                case ItemID.Boss3Soul:
                    TemporaryButton = Boss3SoulButton;
                    break;
                case ItemID.UnDeadSnake:
                    TemporaryButton = UnDeadSnakeButton;
                    break;
                case ItemID.WeakUnDeadSnake:
                    TemporaryButton = WeakUnDeadSnakeButton;
                    break;
                case ItemID.RebornSnake:
                    TemporaryButton = ReBornSnakeButton;
                    break;
                case ItemID.WeakRebornSnake:
                    TemporaryButton = WeakReBornSnakeButton;
                    break;
                case ItemID.Sharpener:
                    TemporaryButton = SharpenerButton;
                    break;
                case ItemID.Cash:
                    TemporaryButton = CashButton;
                    break;
                case ItemID.ControllerRoomKey:
                    TemporaryButton = ControllerRoomKeyButton;
                    break;
                case ItemID.UnderGroundKey:
                    TemporaryButton = UnderGroundKeyButton;
                    break;
                case ItemID.FinalStairKey:
                    TemporaryButton = FinalKeyButton;
                    break;
                case ItemID.RitualSword:
                    TemporaryButton = RitualSwordButton;
                    break;
            }
            //i 的位置必須確實有物件在
            TemporaryText = IdentifyID.FindObject(ItemTextArea, i);

            ItemPageList.Add(new ItemPage(TemporaryButton, TemporaryText));
            //指定ButtonOrder和決定是否關閉ReadNotice
            if (ItemPageList[i].Button.GetComponent<ItemButton>() != null)
            {
                ItemPageList[i].Button.GetComponent<ItemButton>().ButtonOrder = i;
                if (ItemManage.ItemReadList[i])
                {
                    ItemPageList[i].Button.GetComponent<ItemButton>().TurnOffReadNotice();
                }
            }
        }
        for (int i = 0; i < ItemManage.TotalDocumentNumber; i++)
        {
            GameObject TemporyButton = new GameObject();
            GameObject TemporaryText = new GameObject();
            switch (i)
            {
                case ItemID.CommandLetter:
                    TemporyButton = Document0Button;
                    break;
                case ItemID.Map:
                    TemporyButton = Document1Button;
                    break;
                case ItemID.Letter1:
                    TemporyButton = Document2Button;
                    break;
                case ItemID.Letter2:
                    TemporyButton = Document3Button;
                    break;
                case ItemID.Letter3:
                    TemporyButton = Document4Button;
                    break;
                case ItemID.Letter4:
                    TemporyButton = Document5Button;
                    break;
            }
            //i 的位置必須確實有物件在
            TemporaryText = IdentifyID.FindObject(DocumentTextArea, i);

            DocumentPageList.Add(new ItemPage(TemporyButton, TemporaryText));
            //指定ButtonOrder和決定是否關閉ReadNotice
            if (DocumentPageList[i].Button.GetComponent<ItemButton>() != null)
            {
                DocumentPageList[i].Button.GetComponent<ItemButton>().ButtonOrder = i;
                if (ItemManage.DocumentReadList[i])
                {
                    DocumentPageList[i].Button.GetComponent<ItemButton>().TurnOffReadNotice();
                }
            }
        }
    }

    private void ButtonSet()//道具按鈕的判斷與生成(1)
    {
        if (!isItemButtonAppear && status == Status.Item)
        {
            int ItemListOrder = 0;
            //決定要顯示幾個道具
            for (int i = 0; i < ItemManage.ItemGettingNumber; i++)
            {
                ItemButtonOrderRecord.Add(0);
            }
            //登記序號
            for (int i = 0; i < ItemManage.ItemGettingNumber; i++)
            {
                bool AssignSuccess = false;
                while (!AssignSuccess)
                {
                    if (ItemManage.ItemNumberList[ItemListOrder] != 0)
                    {
                        ItemButtonOrderRecord[i] = ItemListOrder;
                        AssignSuccess = true;
                    }
                    ItemListOrder++;
                }
            }
            //實際產出
            for (int i = 0; i < ItemManage.ItemGettingNumber; i++)
            {
                ProduceButton(ItemPageList[ItemButtonOrderRecord[i]].Button, i, ItemShowAreaBottom);
            }
            isItemButtonAppear = true;
        }
        if (!isDocumentButtonAppear && status == Status.Document)
        {
            int DocumentListOrder = 0;
            //決定要顯示幾個道具
            for (int i = 0; i < ItemManage.DocumentGettingNumber; i++)
            {
                DocumentButtonOrderRecord.Add(0);
            }
            //登記序號
            for (int i = 0; i < ItemManage.DocumentGettingNumber; i++)
            {
                bool AssignSuccess = false;
                while (!AssignSuccess)
                {
                    if (ItemManage.DocumentNumberList[DocumentListOrder] != 0)
                    {
                        DocumentButtonOrderRecord[i] = DocumentListOrder;
                        AssignSuccess = true;
                    }
                    DocumentListOrder++;
                }
            }
            //實際產出
            for (int i = 0; i < ItemManage.DocumentGettingNumber; i++)
            {
                ProduceButton(DocumentPageList[DocumentButtonOrderRecord[i]].Button, i, DocumentShowAreaBottom);
            }
            isDocumentButtonAppear = true;
        }
    }

    private void ProduceButton(GameObject Button, int Order, GameObject ShowArea)
    {
        Button.GetComponent<ItemButton>().NowOrder = Order;
        Instantiate(Button, _transform.position, Quaternion.identity, ShowArea.transform);
    }

    private void ProduceDocumentDetail(GameObject Detail)
    {
        Instantiate(Detail, DetailShowArea.position, Quaternion.identity, DetailShowArea);
        NowShowDocumentDetail = DetailShowArea.GetChild(0).gameObject;
        ShouldPlayConfirmSound = true;
        isDocumentDetailAppear = true;
    }

    private void AllTextFalse(List<ItemPage> List)
    {
        for (int i = 0; i < List.Count; i++)
        {
            List[i].Text.SetActive(false);
        }
    }

    private void LagTimer()
    {
        if (ShouldLag)
        {
            MoveLagTimer -= _deltaTime;
            if (MoveLagTimer <= 0)
            {
                ShouldLag = false;
                MoveLagTimer = MoveLagTimerSet;
            }
        }
    }

    private void ItemPreview()
    {
        if (ItemDisplayWindowNumber != 999)
        {
            AllTextFalse(ItemPageList);
            ItemPageList[ItemDisplayWindowNumber].Text.SetActive(true);
            //關閉存放區的New
            if (!ItemManage.ItemReadList[ItemDisplayWindowNumber])
            {
                ShouldSave = true;
                ItemManage.ItemReadList[ItemDisplayWindowNumber] = true;
                ItemPageList[ItemDisplayWindowNumber].Button.GetComponent<ItemButton>().TurnOffReadNotice();
            }
        }
        if (DocumentDisplayWindowNumber != 999)
        {
            AllTextFalse(DocumentPageList);
            DocumentPageList[DocumentDisplayWindowNumber].Text.SetActive(true);
            //關閉存放區的New
            if (!ItemManage.DocumentReadList[DocumentDisplayWindowNumber])
            {
                ShouldSave = true;
                ItemManage.DocumentReadList[DocumentDisplayWindowNumber] = true;
                DocumentPageList[DocumentDisplayWindowNumber].Button.GetComponent<ItemButton>().TurnOffReadNotice();
            }
        }
    }

    private void SaveReadStatus()
    {
        if (ShouldSave)
        {
            _playerData.NoSignSave();
            ShouldSave = false;
        }
    }
}

public struct ItemPage
{
    public GameObject Button;
    public GameObject Text;

    public ItemPage(GameObject button, GameObject text)
    {
        Button = button;
        Text = text;
    }
}
