using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerData : MonoBehaviour
{
    SaveSystem SL;
   public GameObject SaveGameMark;

    public void CommonSave()
    {
        if (GameEvent.EnterGameWithNormalWay)
        {
            Save(GameEvent.NowDataName);
            SaveGameMark.SetActive(true);
        }
    }

    public void NoSignSave()
    {
        if (GameEvent.EnterGameWithNormalWay)
        {
            Save(GameEvent.NowDataName);
        }
    }

    public void Save(string DataName)
    {
        PlayerDataType p = new PlayerDataType();
        p.CheckPointNumber = CheckPoint.CheckPointNumber;
        p.TutorialTotalNumber = TutorialWindow.TutorialGettingNumber;
        p.KillerPoint = BattleSystem.KillerPoint;
        //Tutorial
        p.TutorialGettingNumber = TutorialWindow.TutorialGettingNumber;
        p.GetTutorial0 = TutorialWindow.TutorialGetList[0];
        p.GetTutorial1 = TutorialWindow.TutorialGetList[1];
        p.GetTutorial2 = TutorialWindow.TutorialGetList[2];
        p.GetTutorial3 = TutorialWindow.TutorialGetList[3];
        p.GetTutorial4 = TutorialWindow.TutorialGetList[4];
        p.GetTutorial5 = TutorialWindow.TutorialGetList[5];
        p.hasReadTutorial0 = TutorialWindow.TutorialReadList[0];
        p.hasReadTutorial1 = TutorialWindow.TutorialReadList[1];
        p.hasReadTutorial2 = TutorialWindow.TutorialReadList[2];
        p.hasReadTutorial3 = TutorialWindow.TutorialReadList[3];
        p.hasReadTutorial4 = TutorialWindow.TutorialReadList[4];
        p.hasReadTutorial5 = TutorialWindow.TutorialReadList[5];
        //item
        p.ItemGettingNumber = ItemManage.ItemGettingNumber;
        p.DocumentGettingNumber = ItemManage.DocumentGettingNumber;
        p.ItemID0Number = ItemManage.ItemNumberList[0];
        p.ItemID1Number = ItemManage.ItemNumberList[1];
        p.ItemID2Number = ItemManage.ItemNumberList[2];
        p.ItemID3Number = ItemManage.ItemNumberList[3];
        p.ItemID4Number = ItemManage.ItemNumberList[4];
        p.ItemID5Number = ItemManage.ItemNumberList[5];
        p.ItemID6Number = ItemManage.ItemNumberList[6];
        p.ItemID7Number = ItemManage.ItemNumberList[7];
        p.ItemID8Number = ItemManage.ItemNumberList[8];
        p.ItemID9Number = ItemManage.ItemNumberList[9];
        p.ItemID10Number = ItemManage.ItemNumberList[10];
        p.ItemID11Number = ItemManage.ItemNumberList[11];
        p.ItemID12Number = ItemManage.ItemNumberList[12];
        p.ItemID13Number = ItemManage.ItemNumberList[13];
        p.ItemID14Number = ItemManage.ItemNumberList[14];
        p.ItemID15Number = ItemManage.ItemNumberList[15];
        p.ItemID16Number = ItemManage.ItemNumberList[16];
        p.ItemID17Number = ItemManage.ItemNumberList[17];
        p.DocumentID0Number = ItemManage.DocumentNumberList[0];
        p.DocumentID1Number = ItemManage.DocumentNumberList[1];
        p.DocumentID2Number = ItemManage.DocumentNumberList[2];
        p.DocumentID3Number = ItemManage.DocumentNumberList[3];
        p.DocumentID4Number = ItemManage.DocumentNumberList[4];
        p.DocumentID5Number = ItemManage.DocumentNumberList[5];
        p.DocumentID6Number = ItemManage.DocumentNumberList[6];
        p.DocumentID7Number = ItemManage.DocumentNumberList[7];
        p.DocumentID8Number = ItemManage.DocumentNumberList[8];
        p.DocumentID9Number = ItemManage.DocumentNumberList[9];
        p.MapItemID0Get = ItemManage.MapItemList[0];
        p.MapItemID1Get = ItemManage.MapItemList[1];
        p.MapItemID2Get = ItemManage.MapItemList[2];
        p.MapItemID3Get = ItemManage.MapItemList[3];
        p.MapItemID4Get = ItemManage.MapItemList[4];
        p.MapItemID5Get = ItemManage.MapItemList[5];
        p.MapItemID6Get = ItemManage.MapItemList[6];
        p.MapItemID7Get = ItemManage.MapItemList[7];
        p.MapItemID8Get = ItemManage.MapItemList[8];
        p.MapItemID9Get = ItemManage.MapItemList[9];
        p.MapItemID10Get = ItemManage.MapItemList[10];
        p.MapItemID11Get = ItemManage.MapItemList[11];
        p.MapItemID12Get = ItemManage.MapItemList[12];
        p.MapItemID13Get = ItemManage.MapItemList[13];
        p.MapItemID14Get = ItemManage.MapItemList[14];
        p.MapItemID15Get = ItemManage.MapItemList[15];
        p.MapItemID16Get = ItemManage.MapItemList[16];
        p.MapItemID17Get = ItemManage.MapItemList[17];
        p.MapItemID18Get = ItemManage.MapItemList[18];
        p.MapItemID19Get = ItemManage.MapItemList[19];
        p.MapItemID20Get = ItemManage.MapItemList[20];
        p.MapItemID21Get = ItemManage.MapItemList[21];
        p.MapItemID22Get = ItemManage.MapItemList[22];
        p.MapItemID23Get = ItemManage.MapItemList[23];
        p.MapItemID24Get = ItemManage.MapItemList[24];
        p.MapItemID25Get = ItemManage.MapItemList[25];
        p.MapItemID26Get = ItemManage.MapItemList[26];
        p.MapItemID27Get = ItemManage.MapItemList[27];
        p.MapItemID28Get = ItemManage.MapItemList[28];
        p.MapItemID29Get = ItemManage.MapItemList[29];
        p.MapItemID30Get = ItemManage.MapItemList[30];
        p.MapItemID31Get = ItemManage.MapItemList[31];
        p.MapItemID32Get = ItemManage.MapItemList[32];
        p.MapItemID33Get = ItemManage.MapItemList[33];
        p.MapItemID34Get = ItemManage.MapItemList[34];
        p.MapItemID35Get = ItemManage.MapItemList[35];
        p.MapItemID36Get = ItemManage.MapItemList[36];
        p.MapItemID37Get = ItemManage.MapItemList[37];
        p.MapItemID38Get = ItemManage.MapItemList[38];
        p.MapItemID39Get = ItemManage.MapItemList[39];

        p.hasReadItem0 = ItemManage.ItemReadList[0];
        p.hasReadItem1 = ItemManage.ItemReadList[0];
        p.hasReadItem2 = ItemManage.ItemReadList[0];
        p.hasReadItem3 = ItemManage.ItemReadList[0];
        p.hasReadItem4 = ItemManage.ItemReadList[0];
        p.hasReadItem5 = ItemManage.ItemReadList[0];
        p.hasReadItem6 = ItemManage.ItemReadList[0];
        p.hasReadItem7 = ItemManage.ItemReadList[0];
        p.hasReadItem8 = ItemManage.ItemReadList[0];
        p.hasReadItem9 = ItemManage.ItemReadList[0];
        p.hasReadItem10 = ItemManage.ItemReadList[0];
        p.hasReadItem11 = ItemManage.ItemReadList[0];
        p.hasReadItem12 = ItemManage.ItemReadList[0];
        p.hasReadItem13 = ItemManage.ItemReadList[0];
        p.hasReadItem14 = ItemManage.ItemReadList[0];
        p.hasReadDocument0 = ItemManage.DocumentReadList[0];
        p.hasReadDocument1 = ItemManage.DocumentReadList[1];
        p.hasReadDocument2 = ItemManage.DocumentReadList[2];
        p.hasReadDocument3 = ItemManage.DocumentReadList[3];
        p.hasReadDocument4 = ItemManage.DocumentReadList[4];
        p.hasReadDocument5 = ItemManage.DocumentReadList[5];
        p.hasReadDocument6 = ItemManage.DocumentReadList[6];
        p.hasReadDocument7 = ItemManage.DocumentReadList[7];
        p.hasReadDocument8 = ItemManage.DocumentReadList[8];
        p.hasReadDocument9 = ItemManage.DocumentReadList[9];
        //GameEvent
        p.CirclePlatformStatus = GameEvent.CirclePlatformStatus;

        p.TutorialComplete = GameEvent.TutorialComplete;
        p.PassHall = GameEvent.PassHall;
        p.OpenOriginalDoor = GameEvent.OpenOriginalDoor;
        p.UndergroundUnlock = GameEvent.UndergroundUnlock;
        p.ControllerRoomUnlock = GameEvent.ControllerRoomUnlock;
        p.Find1F_2SecretArea = GameEvent.Find1F_2SecretArea;
        p.Enter1F_2PipeLine = GameEvent.Enter1F_2PipeLine;
        p.SkipRestRoom = GameEvent.SkipRestRoom;
        p.FinalStairUnlock = GameEvent.FinalStairUnlock;
        p.HasGoRestRoom = GameEvent.GoRestRoom;
        p.HasPassRestRoom = GameEvent.PassRestRoom;
        p.SaveSurvivor = GameEvent.SaveSurvivor;
        p.Elevator1FUnlock = GameEvent.Elevator1FUnlock;
        p.Elevator2FUnlock = GameEvent.Elevator2FUnlock;
        p.Elevator4FUnlock = GameEvent.Elevator4FUnlock;
        p.FoundStairHiddenWall = GameEvent.FoundStairHiddenWall;
        p.StairShortCutUnlock = GameEvent.StairShortCutUnlock;
        p.ReadUndeadSnakeHint = GameEvent.ReadUndeadSnakeHint;
        p.OpenSecretRoom1ShortCut = GameEvent.OpenSecretRoom1ShortCut;
        p.OpenStoreRoomDoor = GameEvent.OpenStoreRoomDoor;
        p.DrunkManDie = GameEvent.DrunkManDie;
        p.SecondFloorOneBigLightClose = GameEvent.SecondFloorOneBigLightClose;
        p.OpenCirclePlatform = GameEvent.OpenCirclePlatform;
        p.DestroyCircleDoor = GameEvent.DestroyCircleDoor;
        p.Find2F_1HiddenArea = GameEvent.Find2F_1HiddenArea;
        p.GoInBoss1 = GameEvent.GoInBoss1;
        p.PassBoss1 = GameEvent.PassBoss1;
        p.AbsorbBoss1 = GameEvent.AbsorbBoss1;
        p.GoInBoss2 = GameEvent.GoInBoss2;
        p.PassBoss2 = GameEvent.PassBoss2;
        p.AbsorbBoss2 = GameEvent.AbsorbBoss2;
        p.GoInBoss3 = GameEvent.GoInBoss3;
        p.PassBoss3 = GameEvent.PassBoss3;
        p.KilledBossNumber = GameEvent.KilledBossNumber;
        p.GoIN2F2 = GameEvent.GoIN2F2;
        p.GoIn3F1 = GameEvent.GoIn3F1;
        p.HasGoThirdFloor2 = GameEvent.HasGoThirdFloor2;
        p.HasPassThirdFloor2 = GameEvent.HasPassThirdFloor2;
        p.OpenOriginalLight = GameEvent.OpenOriginalLight;
        p.ControllerRoomCasterDie = GameEvent.ControllerRoomCasterDie;
        p.Boss1DeadPointX = Boss1RoomController._deadInfo.DiePosition.x;
        p.Boss1DeadPointY = Boss1RoomController._deadInfo.DiePosition.y;
        p.Boss1FaceLeft = Boss1RoomController._deadInfo.FaceLeft;
        p.Boss1FaceRight = Boss1RoomController._deadInfo.FaceRight;
        SL.SaveData(p,DataName);
    }//其他script(createPlayer)

    public void Load(string DataName)
    {
        PlayerDataType p = (PlayerDataType)SL.LoadData(typeof(PlayerDataType), DataName);
        //Basic
        CheckPoint.CheckPointNumber = p.CheckPointNumber;
        TutorialWindow.TutorialGettingNumber = p.TutorialTotalNumber;
        BattleSystem.KillerPoint = p.KillerPoint;
        //Tutorial
        TutorialWindow.TutorialGettingNumber = p.TutorialGettingNumber;
        TutorialWindow.TutorialReadList[0] = p.hasReadTutorial0;
        TutorialWindow.TutorialReadList[1] = p.hasReadTutorial1;
        TutorialWindow.TutorialReadList[2] = p.hasReadTutorial2;
        TutorialWindow.TutorialReadList[3] = p.hasReadTutorial3;
        TutorialWindow.TutorialReadList[4] = p.hasReadTutorial4;
        TutorialWindow.TutorialReadList[5] = p.hasReadTutorial5;

        //item
        ItemManage.ItemGettingNumber = p.ItemGettingNumber;
        ItemManage.DocumentGettingNumber = p.DocumentGettingNumber;
        ItemManage.ItemNumberList[0] = p.ItemID0Number;
        ItemManage.ItemNumberList[1] = p.ItemID1Number;
        ItemManage.ItemNumberList[2] = p.ItemID2Number;
        ItemManage.ItemNumberList[3] = p.ItemID3Number;
        ItemManage.ItemNumberList[4] = p.ItemID4Number;
        ItemManage.ItemNumberList[5] = p.ItemID5Number;
        ItemManage.ItemNumberList[6] = p.ItemID6Number;
        ItemManage.ItemNumberList[7] = p.ItemID7Number;
        ItemManage.ItemNumberList[8] = p.ItemID8Number;
        ItemManage.ItemNumberList[9] = p.ItemID9Number;
        ItemManage.ItemNumberList[10] = p.ItemID10Number;
        ItemManage.ItemNumberList[11] = p.ItemID11Number;
        ItemManage.ItemNumberList[12] = p.ItemID12Number;
        ItemManage.ItemNumberList[13] = p.ItemID13Number;
        ItemManage.ItemNumberList[14] = p.ItemID14Number;
        ItemManage.ItemNumberList[15] = p.ItemID15Number;
        ItemManage.ItemNumberList[16] = p.ItemID16Number;
        ItemManage.ItemNumberList[17] = p.ItemID17Number;
        ItemManage.DocumentNumberList[0] = p.DocumentID0Number;
        ItemManage.DocumentNumberList[1] = p.DocumentID1Number;
        ItemManage.DocumentNumberList[2] = p.DocumentID2Number;
        ItemManage.DocumentNumberList[3] = p.DocumentID3Number;
        ItemManage.DocumentNumberList[4] = p.DocumentID4Number;
        ItemManage.DocumentNumberList[5] = p.DocumentID5Number;
        ItemManage.DocumentNumberList[6] = p.DocumentID6Number;
        ItemManage.DocumentNumberList[7] = p.DocumentID7Number;
        ItemManage.DocumentNumberList[8] = p.DocumentID8Number;
        ItemManage.DocumentNumberList[9] = p.DocumentID9Number;
        ItemManage.MapItemList[0] = p.MapItemID0Get;
        ItemManage.MapItemList[1] = p.MapItemID1Get;
        ItemManage.MapItemList[2] = p.MapItemID2Get;
        ItemManage.MapItemList[3] = p.MapItemID3Get;
        ItemManage.MapItemList[4] = p.MapItemID4Get;
        ItemManage.MapItemList[5] = p.MapItemID5Get;
        ItemManage.MapItemList[6] = p.MapItemID6Get;
        ItemManage.MapItemList[7] = p.MapItemID7Get;
        ItemManage.MapItemList[8] = p.MapItemID8Get;
        ItemManage.MapItemList[9] = p.MapItemID9Get;
        ItemManage.MapItemList[10] = p.MapItemID10Get;
        ItemManage.MapItemList[11] = p.MapItemID11Get;
        ItemManage.MapItemList[12] = p.MapItemID12Get;
        ItemManage.MapItemList[13] = p.MapItemID13Get;
        ItemManage.MapItemList[14] = p.MapItemID14Get;
        ItemManage.MapItemList[15] = p.MapItemID15Get;
        ItemManage.MapItemList[16] = p.MapItemID16Get;
        ItemManage.MapItemList[17] = p.MapItemID17Get;
        ItemManage.MapItemList[18] = p.MapItemID18Get;
        ItemManage.MapItemList[19] = p.MapItemID19Get;
        ItemManage.MapItemList[20] = p.MapItemID20Get;
        ItemManage.MapItemList[21] = p.MapItemID21Get;
        ItemManage.MapItemList[22] = p.MapItemID22Get;
        ItemManage.MapItemList[23] = p.MapItemID23Get;
        ItemManage.MapItemList[24] = p.MapItemID24Get;
        ItemManage.MapItemList[25] = p.MapItemID25Get;
        ItemManage.MapItemList[26] = p.MapItemID26Get;
        ItemManage.MapItemList[27] = p.MapItemID27Get;
        ItemManage.MapItemList[28] = p.MapItemID28Get;
        ItemManage.MapItemList[29] = p.MapItemID29Get;
        ItemManage.MapItemList[30] = p.MapItemID30Get;
        ItemManage.MapItemList[31] = p.MapItemID31Get;
        ItemManage.MapItemList[32] = p.MapItemID32Get;
        ItemManage.MapItemList[33] = p.MapItemID33Get;
        ItemManage.MapItemList[34] = p.MapItemID34Get;
        ItemManage.MapItemList[35] = p.MapItemID35Get;
        ItemManage.MapItemList[36] = p.MapItemID36Get;
        ItemManage.MapItemList[37] = p.MapItemID37Get;
        ItemManage.MapItemList[38] = p.MapItemID38Get;
        ItemManage.MapItemList[39] = p.MapItemID39Get;

        ItemManage.ItemReadList[0] = p.hasReadItem0;
        ItemManage.ItemReadList[1] = p.hasReadItem1;
        ItemManage.ItemReadList[2] = p.hasReadItem2;
        ItemManage.ItemReadList[3] = p.hasReadItem3;
        ItemManage.ItemReadList[4] = p.hasReadItem4;
        ItemManage.ItemReadList[5] = p.hasReadItem5;
        ItemManage.ItemReadList[6] = p.hasReadItem6;
        ItemManage.ItemReadList[7] = p.hasReadItem7;
        ItemManage.ItemReadList[8] = p.hasReadItem8;
        ItemManage.ItemReadList[9] = p.hasReadItem9;
        ItemManage.ItemReadList[10] = p.hasReadItem10;
        ItemManage.ItemReadList[11] = p.hasReadItem11;
        ItemManage.ItemReadList[12] = p.hasReadItem12;
        ItemManage.ItemReadList[13] = p.hasReadItem13;
        ItemManage.ItemReadList[14] = p.hasReadItem14;
        ItemManage.DocumentReadList[0] = p.hasReadDocument0;
        ItemManage.DocumentReadList[1] = p.hasReadDocument1;
        ItemManage.DocumentReadList[2] = p.hasReadDocument2;
        ItemManage.DocumentReadList[3] = p.hasReadDocument3;
        ItemManage.DocumentReadList[4] = p.hasReadDocument4;
        ItemManage.DocumentReadList[5] = p.hasReadDocument5;
        ItemManage.DocumentReadList[6] = p.hasReadDocument6;
        ItemManage.DocumentReadList[7] = p.hasReadDocument7;
        ItemManage.DocumentReadList[8] = p.hasReadDocument8;
        ItemManage.DocumentReadList[9] = p.hasReadDocument9;
        ItemManage.LoadPrepareItemList();

        //GameEvent
        GameEvent.CirclePlatformStatus = p.CirclePlatformStatus;

        GameEvent.TutorialComplete = p.TutorialComplete; 
        GameEvent.PassHall = p.PassHall;
        GameEvent.OpenOriginalDoor = p.OpenOriginalDoor;
        GameEvent.UndergroundUnlock = p.UndergroundUnlock;
        GameEvent.ControllerRoomUnlock = p.ControllerRoomUnlock;
        GameEvent.Find1F_2SecretArea = p.Find1F_2SecretArea;
        GameEvent.Enter1F_2PipeLine = p.Enter1F_2PipeLine;
        GameEvent.SkipRestRoom = p.SkipRestRoom;
        GameEvent.FinalStairUnlock = p.FinalStairUnlock;
        GameEvent.GoRestRoom = p.HasGoRestRoom;
        GameEvent.PassRestRoom = p.HasPassRestRoom;
        GameEvent.SaveSurvivor = p.SaveSurvivor;
        GameEvent.Elevator1FUnlock = p.Elevator1FUnlock;
        GameEvent.Elevator2FUnlock = p.Elevator2FUnlock;
        GameEvent.Elevator4FUnlock = p.Elevator4FUnlock;
        GameEvent.FoundStairHiddenWall = p.FoundStairHiddenWall;
        GameEvent.StairShortCutUnlock = p.StairShortCutUnlock;
        GameEvent.ReadUndeadSnakeHint = p.ReadUndeadSnakeHint;
        GameEvent.OpenSecretRoom1ShortCut = p.OpenSecretRoom1ShortCut;
        GameEvent.OpenStoreRoomDoor = p.OpenStoreRoomDoor;
        GameEvent.DrunkManDie = p.DrunkManDie;
        GameEvent.SecondFloorOneBigLightClose = p.SecondFloorOneBigLightClose;
        GameEvent.OpenCirclePlatform = p.OpenCirclePlatform;
        GameEvent.DestroyCircleDoor = p.DestroyCircleDoor;
         GameEvent.Find2F_1HiddenArea = p.Find2F_1HiddenArea;
        GameEvent.GoInBoss1 = p.GoInBoss1;
        GameEvent.PassBoss1 = p.PassBoss1;
        GameEvent.AbsorbBoss1 = p.AbsorbBoss1;
        GameEvent.GoInBoss2 = p.GoInBoss2;
        GameEvent.PassBoss2 = p.PassBoss2;
        GameEvent.AbsorbBoss2 = p.AbsorbBoss2;
        GameEvent.GoInBoss3 = p.GoInBoss3;
        GameEvent.PassBoss3 = p.PassBoss3;
        GameEvent.KilledBossNumber = p.KilledBossNumber;
        GameEvent.GoIN2F2 = p.GoIN2F2;
        GameEvent.GoIn3F1 = p.GoIn3F1;
        GameEvent.HasGoThirdFloor2 = p.HasGoThirdFloor2;
        GameEvent.HasPassThirdFloor2 = p.HasPassThirdFloor2;
        GameEvent.OpenOriginalLight = p.OpenOriginalLight;
        GameEvent.ControllerRoomCasterDie = p.ControllerRoomCasterDie;
        Boss1RoomController._deadInfo.DiePosition = new Vector3(p.Boss1DeadPointX, p.Boss1DeadPointY, 0);
        Boss1RoomController._deadInfo.FaceLeft = p.Boss1FaceLeft;
        Boss1RoomController._deadInfo.FaceRight = p.Boss1FaceRight;
    }

    public void KeyCodeSave()
    {
        KeyCodeData k = new KeyCodeData();
        k.ThrowCocktail = KeyCodeManage.UseItem;
        k.Restore = KeyCodeManage.Restore;
        k.NormalAtk = KeyCodeManage.NormalAtk;
        k.StrongAtk = KeyCodeManage.StrongAtk;
        k.GoRight = KeyCodeManage.GoRight;
        k.GoLeft = KeyCodeManage.GoLeft;
        k.Interact = KeyCodeManage.Interact;
        k.Jump = KeyCodeManage.Jump;
        k.Dash = KeyCodeManage.Dash;
        k.Shoot = KeyCodeManage.Shoot;
        k.Block = KeyCodeManage.Block;
        k.ChangeThrowType = KeyCodeManage.ChangeUseItem;
        k.OpenItem = KeyCodeManage.OpenItemWindow;
        k.JumpNumber = KeyCodeManage.JumpNumber;
        k.NormalAtkNumber = KeyCodeManage.NormalAtkNumber;
        k.StrongAtkNumber = KeyCodeManage.StrongAtkNumber;
        k.DashNumber = KeyCodeManage.DashNumber;
        k.ThrowCocktailNumber = KeyCodeManage.UseItemNumber;
        k.RestoreNumber = KeyCodeManage.RestoreNumber;
        k.InteractNumber = KeyCodeManage.InteractNumber;
        k.ShootNumber = KeyCodeManage.ShootNumber;
        k.BlockNumber = KeyCodeManage.BlockNumber;
        k.ChangeThrowTypeNumber = KeyCodeManage.ChangeUseItemNumber;
        k.GoRightText = KeyCodeManage.GoRightText;
        k.GoLeftText = KeyCodeManage.GoLeftText;
        k.JumpText = KeyCodeManage.JumpText;
        k.NormalAtkText = KeyCodeManage.NormalAtkText;
        k.StrongAtkText = KeyCodeManage.StrongAtkText;
        k.DashText = KeyCodeManage.DashText;
        k.ThrowCocktailText = KeyCodeManage.UseItemText;
        k.RestoreText = KeyCodeManage.RestoreText;
        k.InteractText = KeyCodeManage.InteractText;
        k.ShootText = KeyCodeManage.ShootText;
        k.BlockText = KeyCodeManage.BlockText;
        k.ChangeThrowTypeText = KeyCodeManage.ChangeUseItemText;
        k.OpenItemText = KeyCodeManage.OpenItemWindowText;
        k.JumpTextC = KeyCodeManage.JumpTextC;
        k.NormalAtkTextC = KeyCodeManage.NormalAtkTextC;
        k.StrongAtkTextC = KeyCodeManage.StrongAtkTextC;
        k.DashTextC = KeyCodeManage.DashTextC;
        k.ThrowCocktailTextC = KeyCodeManage.UseItemTextC;
        k.RestoreTextC = KeyCodeManage.RestoreTextC;
        k.InteractTextC = KeyCodeManage.InteractTextC;
        k.ShootTextC = KeyCodeManage.ShootTextC;
        k.BlockTextC = KeyCodeManage.BlockTextC;
        k.ChangeThrowTypeTextC = KeyCodeManage.ChangeUseItemTextC;
        SL.SaveData(k, 1);
    }

    public void SoundVolumeSave()
    {
        SoundVolume s = new SoundVolume();
        s.BGMVolume = MusicController.BGMVolume;
        s.SEVolume = SEController.SEVolume;
        SL.SaveData(s,2);
    }

    //其他script(TitleController)
    public void KeyCodeLoad()
    {
        KeyCodeData k = (KeyCodeData)SL.LoadData(typeof(KeyCodeData), 1);
        KeyCodeManage.UseItem = k.ThrowCocktail;
        KeyCodeManage.Restore = k.Restore;
        KeyCodeManage.NormalAtk = k.NormalAtk;
        KeyCodeManage.StrongAtk = k.StrongAtk;
        KeyCodeManage.GoRight = k.GoRight;
        KeyCodeManage.GoLeft = k.GoLeft;
        KeyCodeManage.Interact = k.Interact;
        KeyCodeManage.Jump = k.Jump;
        KeyCodeManage.Dash = k.Dash;
        KeyCodeManage.Shoot = k.Shoot;
        KeyCodeManage.Block = k.Block;
        KeyCodeManage.ChangeUseItem = k.ChangeThrowType;
        KeyCodeManage.OpenItemWindow = k.OpenItem;
        KeyCodeManage.JumpNumber = k.JumpNumber;
        KeyCodeManage.NormalAtkNumber = k.NormalAtkNumber;
        KeyCodeManage.StrongAtkNumber = k.StrongAtkNumber;
        KeyCodeManage.DashNumber = k.DashNumber;
        KeyCodeManage.UseItemNumber = k.ThrowCocktailNumber;
        KeyCodeManage.RestoreNumber = k.RestoreNumber;
        KeyCodeManage.InteractNumber = k.InteractNumber;
        KeyCodeManage.ShootNumber = k.ShootNumber;
        KeyCodeManage.BlockNumber = k.BlockNumber;
        KeyCodeManage.ChangeUseItemNumber = k.ChangeThrowTypeNumber;
        KeyCodeManage.GoRightText = k.GoRightText;
        KeyCodeManage.GoLeftText = k.GoLeftText;
        KeyCodeManage.JumpText = k.JumpText;
        KeyCodeManage.NormalAtkText = k.NormalAtkText;
        KeyCodeManage.StrongAtkText = k.StrongAtkText;
        KeyCodeManage.DashText = k.DashText;
        KeyCodeManage.UseItemText = k.ThrowCocktailText;
        KeyCodeManage.RestoreText = k.RestoreText;
        KeyCodeManage.InteractText = k.InteractText;
        KeyCodeManage.ShootText = k.ShootText;
        KeyCodeManage.BlockText = k.BlockText;
        KeyCodeManage.ChangeUseItemText = k.ChangeThrowTypeText;
        KeyCodeManage.OpenItemWindowText = k.OpenItemText;
        KeyCodeManage.JumpTextC = k.JumpTextC;
        KeyCodeManage.NormalAtkTextC = k.NormalAtkTextC;
        KeyCodeManage.StrongAtkTextC = k.StrongAtkTextC;
        KeyCodeManage.DashTextC = k.DashTextC;
        KeyCodeManage.UseItemTextC = k.ThrowCocktailTextC;
        KeyCodeManage.RestoreTextC = k.RestoreTextC;
        KeyCodeManage.InteractTextC = k.InteractTextC;
        KeyCodeManage.ShootTextC = k.ShootTextC;
        KeyCodeManage.BlockTextC = k.BlockTextC;
        KeyCodeManage.ChangeUseItemTextC = k.ChangeThrowTypeTextC;
    }

    public void SoundVolumeLoad(ref float BGMValue, ref float SEValue)
    {
        SoundVolume s = (SoundVolume)SL.LoadData(typeof(SoundVolume), 2);
        MusicController.BGMVolume = s.BGMVolume;
        SEController.SEVolume = s.SEVolume;
        BGMValue = MusicController.BGMVolume;
        SEValue = SEController.SEVolume;
    }

    public void AllReset()
    {
        CheckPoint.CheckPointNumber = 0;
        TutorialWindow.TutorialGettingNumber = 3;
        BattleSystem.KillerPoint = 0;

        //Tutorial
        TutorialWindow.TutorialListReset();
        //item
        ItemManage.ItemListReset();
        ItemManage.DocumentListReset();
        ItemManage.MapItemListReset();
        //GameEvent
        GameEvent.AllReset();
        Boss1RoomController._deadInfo = new MonsterDeadInformation();
    }

    private void Awake()
    {
        SL = GetComponent<SaveSystem>();
    }
}

public class PlayerDataType
{
    //Basic
    public int CheckPointNumber;
    public int TutorialTotalNumber;
    public int KillerPoint;

    //Tutorial
    public int TutorialGettingNumber;
    public bool GetTutorial0;
    public bool GetTutorial1;
    public bool GetTutorial2;
    public bool GetTutorial3;
    public bool GetTutorial4;
    public bool GetTutorial5;
    public bool hasReadTutorial0;
    public bool hasReadTutorial1;
    public bool hasReadTutorial2;
    public bool hasReadTutorial3;
    public bool hasReadTutorial4;
    public bool hasReadTutorial5;
    //Item
    public int ItemGettingNumber;
    public int DocumentGettingNumber;
    public int ItemID0Number;
    public int ItemID1Number;
    public int ItemID2Number;
    public int ItemID3Number;
    public int ItemID4Number;
    public int ItemID5Number;
    public int ItemID6Number;
    public int ItemID7Number;
    public int ItemID8Number;
    public int ItemID9Number;
    public int ItemID10Number;
    public int ItemID11Number;
    public int ItemID12Number;
    public int ItemID13Number;
    public int ItemID14Number;
    public int ItemID15Number;
    public int ItemID16Number;
    public int ItemID17Number;
    public int DocumentID0Number;
    public int DocumentID1Number;
    public int DocumentID2Number;
    public int DocumentID3Number;
    public int DocumentID4Number;
    public int DocumentID5Number;
    public int DocumentID6Number;
    public int DocumentID7Number;
    public int DocumentID8Number;
    public int DocumentID9Number;
    public bool MapItemID0Get;
    public bool MapItemID1Get;
    public bool MapItemID2Get;
    public bool MapItemID3Get;
    public bool MapItemID4Get;
    public bool MapItemID5Get;
    public bool MapItemID6Get;
    public bool MapItemID7Get;
    public bool MapItemID8Get;
    public bool MapItemID9Get;
    public bool MapItemID10Get;
    public bool MapItemID11Get;
    public bool MapItemID12Get;
    public bool MapItemID13Get;
    public bool MapItemID14Get;
    public bool MapItemID15Get;
    public bool MapItemID16Get;
    public bool MapItemID17Get;
    public bool MapItemID18Get;
    public bool MapItemID19Get;
    public bool MapItemID20Get;
    public bool MapItemID21Get;
    public bool MapItemID22Get;
    public bool MapItemID23Get;
    public bool MapItemID24Get;
    public bool MapItemID25Get;
    public bool MapItemID26Get;
    public bool MapItemID27Get;
    public bool MapItemID28Get;
    public bool MapItemID29Get;
    public bool MapItemID30Get;
    public bool MapItemID31Get;
    public bool MapItemID32Get;
    public bool MapItemID33Get;
    public bool MapItemID34Get;
    public bool MapItemID35Get;
    public bool MapItemID36Get;
    public bool MapItemID37Get;
    public bool MapItemID38Get;
    public bool MapItemID39Get;

    public bool hasReadItem0;
    public bool hasReadItem1;
    public bool hasReadItem2;
    public bool hasReadItem3;
    public bool hasReadItem4;
    public bool hasReadItem5;
    public bool hasReadItem6;
    public bool hasReadItem7;
    public bool hasReadItem8;
    public bool hasReadItem9;
    public bool hasReadItem10;
    public bool hasReadItem11;
    public bool hasReadItem12;
    public bool hasReadItem13;
    public bool hasReadItem14;
    public bool hasReadDocument0;
    public bool hasReadDocument1;
    public bool hasReadDocument2;
    public bool hasReadDocument3;
    public bool hasReadDocument4;
    public bool hasReadDocument5;
    public bool hasReadDocument6;
    public bool hasReadDocument7;
    public bool hasReadDocument8;
    public bool hasReadDocument9;
    //GameEvent
    public int CirclePlatformStatus;

    public bool TutorialComplete;
    public bool PassHall;
    public bool OpenOriginalDoor;
    public bool UndergroundUnlock;
    public bool ControllerRoomUnlock;
    public bool Find1F_2SecretArea;
    public bool Enter1F_2PipeLine;
    public bool SkipRestRoom;
    public bool FinalStairUnlock;
    public bool HasGoRestRoom;
    public bool HasPassRestRoom;
    public bool SaveSurvivor;
    public bool Elevator1FUnlock;
    public bool Elevator2FUnlock;
    public bool Elevator4FUnlock;
    public bool FoundStairHiddenWall;
    public bool StairShortCutUnlock;
    public bool ReadUndeadSnakeHint;
    public bool OpenSecretRoom1ShortCut;
    public bool OpenStoreRoomDoor;
    public bool DrunkManDie;
    public bool SecondFloorOneBigLightClose;
    public bool OpenCirclePlatform;
    public bool DestroyCircleDoor;
    public bool Find2F_1HiddenArea;
    public bool GoInBoss1;
    public bool PassBoss1;
    public bool AbsorbBoss1;
    public bool GoInBoss2;
    public bool PassBoss2;
    public bool AbsorbBoss2;
    public bool GoInBoss3;
    public bool PassBoss3;
    public int KilledBossNumber;
    public bool GoIN2F2;
    public bool GoIn3F1;
    public bool HasGoThirdFloor2;
    public bool HasPassThirdFloor2;
    public bool OpenOriginalLight;
    public bool ControllerRoomCasterDie;
    public float Boss1DeadPointX;
    public float Boss1DeadPointY;
    public bool Boss1FaceRight;
    public bool Boss1FaceLeft;
}

public class KeyCodeData
{
    public KeyCode ThrowCocktail;
    public KeyCode Restore;
    public KeyCode NormalAtk;
    public KeyCode StrongAtk;
    public KeyCode GoRight;
    public KeyCode GoLeft;
    public KeyCode Interact;
    public KeyCode Jump;
    public KeyCode Dash;
    public KeyCode Shoot;
    public KeyCode Block;
    public KeyCode ChangeThrowType;
    public KeyCode OpenItem;
    public int JumpNumber;
    public int NormalAtkNumber;
    public int StrongAtkNumber;
    public int DashNumber;
    public int ThrowCocktailNumber;
    public int RestoreNumber;
    public int InteractNumber;
    public int ShootNumber;
    public int BlockNumber;
    public int ChangeThrowTypeNumber;
    public string GoRightText;
    public string GoLeftText;
    public string JumpText;
    public string NormalAtkText;
    public string StrongAtkText;
    public string DashText;
    public string ThrowCocktailText;
    public string RestoreText;
    public string InteractText;
    public string ShootText;
    public string BlockText;
    public string ChangeThrowTypeText;
    public string OpenItemText;
    public string JumpTextC;
    public string NormalAtkTextC;
    public string StrongAtkTextC;
    public string DashTextC;
    public string ThrowCocktailTextC;
    public string RestoreTextC;
    public string InteractTextC;
    public string ShootTextC;
    public string BlockTextC;
    public string ChangeThrowTypeTextC;
    public string OpenItemTextC;
}

public class SoundVolume
{
    public float BGMVolume;
    public float SEVolume;
}
