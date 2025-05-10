using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class itemManage : MonoBehaviour
{
    /* 回復劑地點:1F-2
     * 汽油彈地點:1F-2
     * 爆裂瓶地點:1F-1, StoreRoom
     */
    public static List<int> ItemNumberList = new List<int>();
    public static List<int> DocumentNumberList = new List<int>();
    public static List<bool> MapItemList = new List<bool>();

    public static List<bool> ItemReadList = new List<bool>();
    public static List<bool> DocumentReadList = new List<bool>();

    public static List<bool> PrepareItemList = new List<bool>();

    public static int TotalItemNumber = 18;//所有道具種類數
    public static int TotalDocumentNumber = 10;//所有文件種類數
    private static int TotalMapItemNumber = 40;//所有地圖上道具數

    private PlayerData _PlayerData;

    [HideInInspector] public int RestoreItemNumber;//有被其他script用到(playerController，RestoreSpace)
    [HideInInspector] public int CocktailNumber;//有被其他script用到(playerController，PauseMenuController，RestoreSpace)
    [HideInInspector] public int ExplosionBottleNumber;//有被其他script用到(playerController，PauseMenuController，RestoreSpace)
    public enum PrepareItem { Cocktail, ExplosionBottle, Sharpener, RitualSword }
    [HideInInspector] public PrepareItem NowPrepareItem;//有被其他script用到(playerController，BattleSystem)
    [HideInInspector] public int NowPrepareItemID;

    public static int ItemImageTotalNumber = 0;//有被其他script用到(ItemImageOrder，GetItem)
    public Text RestoreItem;
    public Text ThrowItem;
    public GameObject CocktailImage;
    public GameObject ExplosionBottleImage;
    public GameObject SharpenerImage;
    public GameObject RitualSwordImage;
    private float DisappearTimerSet = 3.25f;
    private float DisappearTimer;

    public AudioClip ChangeItemSound;
    private AudioSource ChangeItemSource;
    private static bool ShouldPlayChangeItemSound;

    //需存檔變數
    public static int ItemGettingNumber = 3;//script(ItemWindow，GetItem，itemButton，playerData，Boss1RoomController，Boss2RoomController，Boss3Controller)
    public static int DocumentGettingNumber = 2;//script(ItemWindow，GetItem，itemButton，playerData)
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("FollowSystem") != null)
        {
            _PlayerData = GameObject.Find("FollowSystem").GetComponent<PlayerData>();
        }
        NowPrepareItem = PrepareItem.Cocktail;
        DisappearTimer = DisappearTimerSet;

        SEController.inisializeAudioSource(ref ChangeItemSource, ChangeItemSound, this.transform);
    }

    // Update is called once per frame
    void Update()
    {
        RestoreItem.text = RestoreItemNumber.ToString();
        switch (NowPrepareItemID)
        {
            case 0:
                NowPrepareItem = PrepareItem.Cocktail;
                CocktailImage.SetActive(true);
                ExplosionBottleImage.SetActive(false);
                SharpenerImage.SetActive(false);
                RitualSwordImage.SetActive(false);

                ThrowItem.gameObject.SetActive(true);
                ThrowItem.text = CocktailNumber.ToString();
                break;
            case 1:
                NowPrepareItem = PrepareItem.ExplosionBottle;
                CocktailImage.SetActive(false);
                ExplosionBottleImage.SetActive(true);
                SharpenerImage.SetActive(false);
                RitualSwordImage.SetActive(false);

                ThrowItem.gameObject.SetActive(true);
                ThrowItem.text = ExplosionBottleNumber.ToString();
                break;
            case 2:
                NowPrepareItem = PrepareItem.Sharpener;
                CocktailImage.SetActive(false);
                ExplosionBottleImage.SetActive(false);
                SharpenerImage.SetActive(true);
                RitualSwordImage.SetActive(false);

                ThrowItem.gameObject.SetActive(false);
                break;
            case 3:
                NowPrepareItem = PrepareItem.RitualSword;
                CocktailImage.SetActive(false);
                ExplosionBottleImage.SetActive(false);
                SharpenerImage.SetActive(false);
                RitualSwordImage.SetActive(true);

                ThrowItem.gameObject.SetActive(false);
                break;
        }
        if (ItemImageTotalNumber > 0)
        {
            DisappearMethod();
        }

        if (ShouldPlayChangeItemSound)
        {
            ChangeItemSource.Play();
            ShouldPlayChangeItemSound = false;
        }

        SEController.CalculateSystemSound(ChangeItemSource);
    }

    public void ItemGet(int _itemID, int MapItemID)
    {
        ItemNumberList[_itemID] += 1;
        if (MapItemID != 999)
        {
            MapItemList[MapItemID] = true;
        }

        //沒撿過的道具才會加
        if (ItemNumberList[_itemID] == 1)
        {
            ItemGettingNumber += 1;
        }

        //消耗品系列增加
        switch (_itemID)
        {
            case ItemID.RestoreItem:
                RestoreItemNumber += 1;
                break;
            case ItemID.Cocktail:
                CocktailNumber += 1;
                break;
            case ItemID.ExplosionBottle:
                ExplosionBottleNumber += 1;
                break;
        }

        //可使用道具列表開啟
        switch (_itemID)
        {
            case ItemID.Sharpener:
                PrepareItemList[2] = true;
                break;
            case ItemID.RitualSword:
                PrepareItemList[3] = true;
                break;
        }

        _PlayerData.CommonSave();
    }

    public void DocumentGet(int DocumentID, int MapItemID)
    {
        DocumentNumberList[DocumentID] += 1;
        if (MapItemID != 999)
        {
            MapItemList[MapItemID] = true;
        }
        DocumentGettingNumber += 1;
        _PlayerData.CommonSave();
    }

    private void DisappearMethod()
    {
        DisappearTimer -= Time.deltaTime;
        if (DisappearTimer <= 0)
        {
            ItemImageTotalNumber -= 1;
            DisappearTimer = DisappearTimerSet;
        }
    }

    public void RestoreUseItem()
    {
        RestoreItemNumber = ItemNumberList[ItemID.RestoreItem];
        CocktailNumber = ItemNumberList[ItemID.Cocktail];
        ExplosionBottleNumber = ItemNumberList[ItemID.ExplosionBottle];
    }//(1)補充消耗品

    public static bool CheckItemExist(int ID)
    {
        if (ItemNumberList.Count == 0)
        {
            return false;
        }

        if (ItemNumberList[ID] >= 1)
        {
            return true;
        }

        return false;
    }

    public static bool CheckDocumentExist(int ID)
    {
        if (DocumentNumberList.Count == 0)
        {
            return false;
        }

        if (DocumentNumberList[ID] >= 1)
        {
            return true;
        }

        return false;
    }

    public bool CheckMapItemExist(int ID)
    {
        if (MapItemList.Count == 0)
        {
            return false;
        }

        if (MapItemList[ID])
        {
            return true;
        }
        return false;
    }

    public static void ItemListReset()
    {
        ItemGettingNumber = 3;

        ItemNumberList.Clear();
        ItemReadList.Clear();
        for (int i = 0; i < TotalItemNumber; i++)
        {
            ItemNumberList.Add(0);
            ItemReadList.Add(false);
        }

        ItemNumberList[ItemID.Sword] += 1;
        ItemNumberList[ItemID.Gun] += 1;
        ItemNumberList[ItemID.RestoreItem] += 3;

        ItemReadList[0] = true;

        for (int i = 0; i < 4; i++)
        {
            PrepareItemList.Add(false);
        }
        PrepareItemList[0] = true;
        PrepareItemList[1] = true;
    }

    public static void DocumentListReset()
    {
        DocumentGettingNumber = 2;

        DocumentNumberList.Clear();
        DocumentReadList.Clear();
        for (int i = 0; i < TotalDocumentNumber; i++)
        {
            DocumentNumberList.Add(0);
            DocumentReadList.Add(false);
        }

        DocumentNumberList[ItemID.CommandLetter] += 1;
        DocumentNumberList[ItemID.Map] += 1;

        DocumentReadList[0] = true;
    }

    public static void MapItemListReset()
    {
        MapItemList.Clear();
        for (int i = 0; i < TotalMapItemNumber; i++)
        {
            MapItemList.Add(false);
        }
    }

    public static int ChangePrepareItem(int NowUseOrder)
    {
        int  i = NowUseOrder + 1;

        if (i == PrepareItemList.Count)
        {
            ShouldPlayChangeItemSound = true;
            return 0;
        }

        while (i < PrepareItemList.Count)
        {
            if (PrepareItemList[i])
            {
                ShouldPlayChangeItemSound = true;
                return i;
            }
            i++;
        }

        ShouldPlayChangeItemSound = true;
        return 0;
    }

    public static void LoadPrepareItemList()
    {
        if (CheckItemExist(ItemID.Sharpener))
        {
            PrepareItemList[2] = true;
        }
        if (CheckItemExist(ItemID.RitualSword))
        {
            PrepareItemList[3] = true;
        }
    }
}

public struct ItemID
{
    public const int Sword = 0;
    public const int Gun = 1;
    public const int RestoreItem = 2;
    public const int Cocktail = 3;
    public const int ExplosionBottle = 4;
    public const int Boss1Soul = 5;
    public const int Boss2Soul = 6;
    public const int Boss3Soul = 7;
    public const int UnDeadSnake = 8;
    public const int WeakUnDeadSnake = 9;
    public const int RebornSnake = 10;
    public const int WeakRebornSnake = 11;
    public const int Sharpener = 12;
    public const int Cash = 13;
    public const int ControllerRoomKey = 14;
    public const int UnderGroundKey = 15;
    public const int FinalStairKey = 16;
    public const int RitualSword = 17;

    public const int CommandLetter = 0;
    public const int Map = 1;
    public const int Letter1 = 2;
    public const int Letter2 = 3;
    public const int Letter3 = 4;
    public const int Letter4 = 5;
}
