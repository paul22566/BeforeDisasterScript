using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent
{
    public static bool EnterGameWithNormalWay;//是否用正常方法進入遊戲，測試遊戲專用  其他script有用到(CreatePlayer，playerController，以下皆是)
    public static bool isAniPlay = false;//是否有動畫正在撥放  其他script有用到(playerController,所有怪物controller，各個房間的controller，各個CameraFollow，backgroundSystem，BlackScreen，playerspecialAni)
    public static string NowDataName;//現在在用哪個檔  其他script有用到(CreatePlayer，playerController)

    //會變化事件
    public static int CirclePlatformStatus = 1;//2F-1區域大轉盤狀態 1是左斜 2 是右斜

    //單一事件變數皆有被PlayerData和CreatePlayer用到
    public static bool TutorialComplete = false;//是否完成教學區域
    public static bool PassHall = false;//通過大廳
    public static bool OpenOriginalDoor = false;//original捷徑的開關  其他script有用到(SecondController，originaldoor，originaldoorSwitch)
    public static bool UndergroundUnlock = false;//地下室是否開通   其他script有用到(undergroundElevator)
    public static bool ControllerRoomUnlock = false;//控制室是否開通   其他script有用到(controllerroomOpen，controllerroomkey)
    public static bool Find1F_2SecretArea = false;//是否發現1F-2隱藏區域  其他script有用到(UnderGroundElevator，FirstFloor2Controller)
    public static bool Enter1F_2PipeLine = false;//進入1F-2通風管 其他script有用到(FirstFloor2Fence)
    public static bool SkipRestRoom = false;//成功跳過休息室
    public static bool FinalStairUnlock = false;//FinalStair是否開通   其他script有用到(Boss2RoomLeftDoor)
    public static bool GoRestRoom = false;//是否曾觸發過休息室事件  其他script有用到(restRoomDoor，playerController，restroomController，RestRoomCameraFollow，LeftMan，RightMan，BlackScreen)
    public static bool PassRestRoom = false;//是否突破休息室  其他script有用到(restRoomDoor，playerController，restroomController，RestRoomCameraFollow，LeftMan，RightMan，PassDestroy)
    public static bool SaveSurvivor = false;//是否放過生還者  其他script有用到(enemyEscape)
    public static bool Elevator1FUnlock = false;//其他script有用到(Elevator，playerController)
    public static bool Elevator2FUnlock = false;//其他script有用到(Elevator，playerController)
    public static bool Elevator4FUnlock = false;//其他script有用到(Elevator，playerController)
    public static bool FoundStairHiddenWall = false;//樓梯隱藏房間被發現 其他script有用到(FragileWall, StairHiddenWall, StairController)
    public static bool StairShortCutUnlock = false;//樓梯捷徑 其他script有用到(StairShortCut，StairShortCutPlatform，StairShortCutSwitch)
    public static bool ReadUndeadSnakeHint = false;//觸發不死蛇提示
    public static bool OpenSecretRoom1ShortCut = false;//打開隱藏房間1捷徑
    public static bool OpenStoreRoomDoor = false;//打開儲藏室門 其他script有用到(StoreRoomSwitch, StairController，StairDoor)
    public static bool DrunkManDie = false;//醉男死亡 其他script有用到(storeManDrunkMan)
    public static bool SecondFloorOneBigLightClose = false;//2F-1大雷射關閉 其他script有用到(SecondFloorOneSwitch)
    public static bool OpenCirclePlatform;//啟動大轉盤
    public static bool DestroyCircleDoor;//破壞大門
    public static bool Find2F_1HiddenArea;//是否發現2F1的隱藏地區
    public static bool GoInBoss1 = false;//是否觸發過Boss1  其他script有用到(BlackScreen，Boss1RoomController，Boos1RoomDoor)
    public static bool PassBoss1 = false;//是否通過Boss1  其他script有用到(WindowController，SecondController，Boss1RoomController，Boos1RoomRightDoor，Boss1Disappear)
    public static bool AbsorbBoss1 = false;//是否吸收Boss1  其他script有用到(playerspecialAni，battlesystem，deadBody，Boss1RoomController，absorbdialog，ItemWindow)
    public static bool GoInBoss2 = false;//是否觸發過Boss2  其他script有用到(BlackScreen，Boss2RoomController，Boos2RoomRightDoor)
    public static bool PassBoss2 = false;//是否通過Boss2  其他script有用到(CaptainController，Boss2RoomController，Boos2RoomRightDoor，Boss2RoomSwitch)
    public static bool AbsorbBoss2 = false;//是否吸收Boss2  其他script有用到(skillPowerChangeAni，createplayer，battleSystem，playerController，playerspecialAni，deadBodyBoss2，Boss2RoomController，absorbdialogBoss2，ItemWindow)
    public static bool GoInBoss3 = false;//是否觸發過Boss3 其他script有用到(BlackScreen，Boss3Controller, FirstComeinBoss3，SelectElevatorController，playerController)
    public static bool PassBoss3 = false;//是否通過Boss3  其他script有用到(Boss3Controller，ItemWindow)
    public static int KilledBossNumber = 0;//打敗幾隻boss
    public static bool GoIN2F2 = false;//是否觸發過2F-2  其他script有用到(ShakePlace1)
    public static bool GoIn3F1 = false;//是否觸發過3F-1  其他script有用到(BlackScreen，ThirdFloor1Controller, ThirdFloor1CameraFollow,diemanTimer，ThirdFloor1BigMonster)
    public static bool HasGoThirdFloor2;//是否觸發過3F-2  其他script有用到(BlackScreen，3F-2相關)
    public static bool HasPassThirdFloor2;//是否通過3F-2  其他script有用到(3F-2相關，3f-1controller)
    public static bool OpenOriginalLight = false;//是否打開大燈  其他script有用到(ControllerRoomSwitch，nightOutsideController)
    public static bool ControllerRoomCasterDie = false;//是否殺光控制室法師(SmallCaptainController)

    public static void AllReset()
    {
        CirclePlatformStatus = 1;

        TutorialComplete = false;
        PassHall = false;
        OpenOriginalDoor = false;
        UndergroundUnlock = false;
        ControllerRoomUnlock = false;
        Find1F_2SecretArea = false;
        Enter1F_2PipeLine = false;
        SkipRestRoom = false;
        FinalStairUnlock = false;
        GoRestRoom = false;
        PassRestRoom = false;
        SaveSurvivor = false;
        Elevator1FUnlock = false;
        Elevator2FUnlock = false;
        Elevator4FUnlock = false;
        FoundStairHiddenWall = false;
        StairShortCutUnlock = false;
        ReadUndeadSnakeHint = false;
        OpenSecretRoom1ShortCut = false;
        OpenStoreRoomDoor = false;
        DrunkManDie = false;
        SecondFloorOneBigLightClose = false;
        OpenCirclePlatform = false;
        DestroyCircleDoor = false;
        Find2F_1HiddenArea = false;
        GoInBoss1 = false;
        PassBoss1 = false;
        AbsorbBoss1 = false;
        GoInBoss2 = false;
        PassBoss2 = false;
         AbsorbBoss2 = false;
        GoInBoss3 = false;
        PassBoss3 = false;
        KilledBossNumber = 0;
        GoIN2F2 = false;
        GoIn3F1 = false;
        HasGoThirdFloor2 = false;
        HasPassThirdFloor2 = false;
        OpenOriginalLight = false;
        ControllerRoomCasterDie = false;
}
}
