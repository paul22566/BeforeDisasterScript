using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent
{
    public static bool EnterGameWithNormalWay;//�O�_�Υ��`��k�i�J�C���A���չC���M��  ��Lscript���Ψ�(CreatePlayer�AplayerController�A�H�U�ҬO)
    public static bool isAniPlay = false;//�O�_���ʵe���b����  ��Lscript���Ψ�(playerController,�Ҧ��Ǫ�controller�A�U�өж���controller�A�U��CameraFollow�AbackgroundSystem�ABlackScreen�AplayerspecialAni)
    public static string NowDataName;//�{�b�b�έ�����  ��Lscript���Ψ�(CreatePlayer�AplayerController)

    //�|�ܤƨƥ�
    public static int CirclePlatformStatus = 1;//2F-1�ϰ�j��L���A 1�O���� 2 �O�k��

    //��@�ƥ��ܼƬҦ��QPlayerData�MCreatePlayer�Ψ�
    public static bool TutorialComplete = false;//�O�_�����оǰϰ�
    public static bool PassHall = false;//�q�L�j�U
    public static bool OpenOriginalDoor = false;//original���|���}��  ��Lscript���Ψ�(SecondController�Aoriginaldoor�AoriginaldoorSwitch)
    public static bool UndergroundUnlock = false;//�a�U�ǬO�_�}�q   ��Lscript���Ψ�(undergroundElevator)
    public static bool ControllerRoomUnlock = false;//����ǬO�_�}�q   ��Lscript���Ψ�(controllerroomOpen�Acontrollerroomkey)
    public static bool Find1F_2SecretArea = false;//�O�_�o�{1F-2���ðϰ�  ��Lscript���Ψ�(UnderGroundElevator�AFirstFloor2Controller)
    public static bool Enter1F_2PipeLine = false;//�i�J1F-2�q���� ��Lscript���Ψ�(FirstFloor2Fence)
    public static bool SkipRestRoom = false;//���\���L�𮧫�
    public static bool FinalStairUnlock = false;//FinalStair�O�_�}�q   ��Lscript���Ψ�(Boss2RoomLeftDoor)
    public static bool GoRestRoom = false;//�O�_��Ĳ�o�L�𮧫Ǩƥ�  ��Lscript���Ψ�(restRoomDoor�AplayerController�ArestroomController�ARestRoomCameraFollow�ALeftMan�ARightMan�ABlackScreen)
    public static bool PassRestRoom = false;//�O�_��}�𮧫�  ��Lscript���Ψ�(restRoomDoor�AplayerController�ArestroomController�ARestRoomCameraFollow�ALeftMan�ARightMan�APassDestroy)
    public static bool SaveSurvivor = false;//�O�_��L���٪�  ��Lscript���Ψ�(enemyEscape)
    public static bool Elevator1FUnlock = false;//��Lscript���Ψ�(Elevator�AplayerController)
    public static bool Elevator2FUnlock = false;//��Lscript���Ψ�(Elevator�AplayerController)
    public static bool Elevator4FUnlock = false;//��Lscript���Ψ�(Elevator�AplayerController)
    public static bool FoundStairHiddenWall = false;//�ӱ����éж��Q�o�{ ��Lscript���Ψ�(FragileWall, StairHiddenWall, StairController)
    public static bool StairShortCutUnlock = false;//�ӱ豶�| ��Lscript���Ψ�(StairShortCut�AStairShortCutPlatform�AStairShortCutSwitch)
    public static bool ReadUndeadSnakeHint = false;//Ĳ�o�����D����
    public static bool OpenSecretRoom1ShortCut = false;//���}���éж�1���|
    public static bool OpenStoreRoomDoor = false;//���}�x�ëǪ� ��Lscript���Ψ�(StoreRoomSwitch, StairController�AStairDoor)
    public static bool DrunkManDie = false;//�K�k���` ��Lscript���Ψ�(storeManDrunkMan)
    public static bool SecondFloorOneBigLightClose = false;//2F-1�j�p�g���� ��Lscript���Ψ�(SecondFloorOneSwitch)
    public static bool OpenCirclePlatform;//�Ұʤj��L
    public static bool DestroyCircleDoor;//�}�a�j��
    public static bool Find2F_1HiddenArea;//�O�_�o�{2F1�����æa��
    public static bool GoInBoss1 = false;//�O�_Ĳ�o�LBoss1  ��Lscript���Ψ�(BlackScreen�ABoss1RoomController�ABoos1RoomDoor)
    public static bool PassBoss1 = false;//�O�_�q�LBoss1  ��Lscript���Ψ�(WindowController�ASecondController�ABoss1RoomController�ABoos1RoomRightDoor�ABoss1Disappear)
    public static bool AbsorbBoss1 = false;//�O�_�l��Boss1  ��Lscript���Ψ�(playerspecialAni�Abattlesystem�AdeadBody�ABoss1RoomController�Aabsorbdialog�AItemWindow)
    public static bool GoInBoss2 = false;//�O�_Ĳ�o�LBoss2  ��Lscript���Ψ�(BlackScreen�ABoss2RoomController�ABoos2RoomRightDoor)
    public static bool PassBoss2 = false;//�O�_�q�LBoss2  ��Lscript���Ψ�(CaptainController�ABoss2RoomController�ABoos2RoomRightDoor�ABoss2RoomSwitch)
    public static bool AbsorbBoss2 = false;//�O�_�l��Boss2  ��Lscript���Ψ�(skillPowerChangeAni�Acreateplayer�AbattleSystem�AplayerController�AplayerspecialAni�AdeadBodyBoss2�ABoss2RoomController�AabsorbdialogBoss2�AItemWindow)
    public static bool GoInBoss3 = false;//�O�_Ĳ�o�LBoss3 ��Lscript���Ψ�(BlackScreen�ABoss3Controller, FirstComeinBoss3�ASelectElevatorController�AplayerController)
    public static bool PassBoss3 = false;//�O�_�q�LBoss3  ��Lscript���Ψ�(Boss3Controller�AItemWindow)
    public static int KilledBossNumber = 0;//���ѴX��boss
    public static bool GoIN2F2 = false;//�O�_Ĳ�o�L2F-2  ��Lscript���Ψ�(ShakePlace1)
    public static bool GoIn3F1 = false;//�O�_Ĳ�o�L3F-1  ��Lscript���Ψ�(BlackScreen�AThirdFloor1Controller, ThirdFloor1CameraFollow,diemanTimer�AThirdFloor1BigMonster)
    public static bool HasGoThirdFloor2;//�O�_Ĳ�o�L3F-2  ��Lscript���Ψ�(BlackScreen�A3F-2����)
    public static bool HasPassThirdFloor2;//�O�_�q�L3F-2  ��Lscript���Ψ�(3F-2�����A3f-1controller)
    public static bool OpenOriginalLight = false;//�O�_���}�j�O  ��Lscript���Ψ�(ControllerRoomSwitch�AnightOutsideController)
    public static bool ControllerRoomCasterDie = false;//�O�_��������Ǫk�v(SmallCaptainController)

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
