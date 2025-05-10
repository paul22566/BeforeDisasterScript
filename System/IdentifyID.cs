using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IdentifyID : MonoBehaviour
{
    public int ID;

    public static GameObject FindObject(Transform TargetList, int TargetID)
    {
        for (int i = 0; i < TargetList.childCount; i++)
        {
            if (TargetList.GetChild(i).GetComponent<IdentifyID>() == null)
            {
                continue;
            }

            if (TargetList.GetChild(i).GetComponent<IdentifyID>().ID == TargetID)
            {
                return TargetList.GetChild(i).gameObject;
            }
        }
        print("Wrong!!");
        return null;
    }
}

public struct UIID
{
    public const int NewTutorial = 0;
    public const int Flash = 1;
    public const int SaveGameMark = 2;
    public const int Die = 3;
    public const int FadeOut = 4;
    public const int SelectPortalMenu = 5;
    public const int Option = 6;
    public const int KeyCodeManage = 7;
    public const int SoundController = 8;
    public const int Item = 9;
    public const int TutorialMenu = 10;
    public const int PauseMenu = 11;
    public const int SpecialAni = 12;
    public const int BlackScreenTop = 13;
    public const int BlackScreenBottom = 14;
    public const int TemporaryBlackScreen = 15;
}
