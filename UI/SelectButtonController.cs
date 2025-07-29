using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SelectButtonController : MonoBehaviour
{
    private static bool HasOpenController;
    private static List<BeLockButton> BeLockButtonList = new List<BeLockButton>();
    private static bool isOpen;

    private static int NowVerticalSelectNumber = 1;//其他Script會用到(PauseMenuController，TitleController，UIButtonBeSelected)
    private static int NowHorizontalSelectNumber = 1;//其他Script會用到(PauseMenuController，TitleController，UIButtonBeSelected)
    private static int MaxVerticalSelectNumber = 1;//其他Script會用到(PauseMenuController，TitleController，playerController)
    private static int MaxHorizontalSelectNumber = 1;//其他Script會用到(PauseMenuController，TitleController，playerController)

    private static int LastVerticalSelectNumber = 1;
    private static int LastHorizontalSelectNumber = 1;

    private static bool ShouldPlayMoveSound;

    public AudioClip MenuMoveSound;
    public AudioClip MenuConfirmSound;
    public AudioClip ItemMoveSound;

    private AudioSource MoveSource;
    private static AudioSource ConfirmSource;
    private static AudioSource ItemMoveSource;

    // Start is called before the first frame update
    void Start()
    {
        if (HasOpenController)
        {
            Destroy(this.gameObject);
            return;
        }
        if (!HasOpenController)
        {
            HasOpenController = true;
        }

        MoveSource = this.AddComponent<AudioSource>();
        ConfirmSource = this.AddComponent<AudioSource>();
        ItemMoveSource = this.AddComponent<AudioSource>();

        MoveSource.clip = MenuMoveSound;
        ConfirmSource.clip = MenuConfirmSound;
        ItemMoveSource.clip = ItemMoveSound;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpen)
        {
            SelectPointControll();
        }

        SEController.CalculateSystemSound(MoveSource);
        SEController.CalculateSystemSound(ConfirmSource);
        SEController.CalculateSystemSound(ItemMoveSource);
    }

    public static void OpenSelectButtonController()
    {
        isOpen = true;
    }

    public static void CloseSelectButtonController()
    {
        isOpen = false;
    }

    private void SelectPointControll()
    {
        if (GameEvent.isAniPlay)
        {
            return;
        }

        if (NowHorizontalSelectNumber < MaxHorizontalSelectNumber)
        {
            if (OldVerXboxControllerDetect.isCrossRightPressed || OldVerXboxControllerDetect.isControllerRightPressed || Input.GetKeyDown(KeyCode.RightArrow))
            {
                ShouldPlayMoveSound = true;
                NowHorizontalSelectNumber += 1;
            }
        }
        if (NowHorizontalSelectNumber > 1)
        {
            if (OldVerXboxControllerDetect.isCrossLeftPressed || OldVerXboxControllerDetect.isControllerLeftPressed || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                ShouldPlayMoveSound = true;
                NowHorizontalSelectNumber -= 1;
            }
        }
        if (NowVerticalSelectNumber > 1)
        {
            if (OldVerXboxControllerDetect.isCrossUpPressed || OldVerXboxControllerDetect.isControllerUpPressed || Input.GetKeyDown(KeyCode.UpArrow))
            {
                ShouldPlayMoveSound = true;
                NowVerticalSelectNumber -= 1;
            }
        }
        if (NowVerticalSelectNumber < MaxVerticalSelectNumber)
        {
            if (OldVerXboxControllerDetect.isCrossDownPressed || OldVerXboxControllerDetect.isControllerDownPressed || Input.GetKeyDown(KeyCode.DownArrow))
            {
                ShouldPlayMoveSound = true;
                NowVerticalSelectNumber += 1;
            }
        }

        DetectLockButton();

        if (ShouldPlayMoveSound)
        {
            MoveSoundPlay();
            ShouldPlayMoveSound = false;
        }

        LastHorizontalSelectNumber = NowHorizontalSelectNumber;
        LastVerticalSelectNumber = NowVerticalSelectNumber;
    }//選擇點控制

    public static float SliderControll(float Value, string Type, float Max, float Min, float ChangeNumber)
    {
        if (GameEvent.isAniPlay)
        {
            return Value;
        }

        switch (Type)
        {
            case "H":
                if (Value < Max)
                {
                    if (OldVerXboxControllerDetect.isCrossRightPressed || Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        ShouldPlayMoveSound = true;
                        Value += ChangeNumber;
                    }
                }
                if (Value > Min)
                {
                    if (OldVerXboxControllerDetect.isCrossLeftPressed || Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        ShouldPlayMoveSound = true;
                        Value -= ChangeNumber;
                    }
                }
                break;
            case "V":
                if (Value > Min)
                {
                    if (OldVerXboxControllerDetect.isCrossUpPressed || Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        ShouldPlayMoveSound = true;
                        Value -= ChangeNumber;
                    }
                }
                if (Value < Max)
                {
                    if (OldVerXboxControllerDetect.isCrossDownPressed || Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        ShouldPlayMoveSound = true;
                        Value += ChangeNumber;
                    }
                }
                break;
        }

        return Value;
    }//條狀控制

    public static bool DetectButtonSelected(int HorizontalNumber, int VerticalNumber)
    {
        if(NowHorizontalSelectNumber == HorizontalNumber && NowVerticalSelectNumber == VerticalNumber)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static void MaxSelectNumberSet(int NewMaxHorizontalNumber, int NewMaxVerticalNumber)
    {
        MaxHorizontalSelectNumber = NewMaxHorizontalNumber;
        MaxVerticalSelectNumber = NewMaxVerticalNumber;
    }

    public static void SelectPointSet(int HorizontalNumber, int VerticalNumber)
    {
        NowHorizontalSelectNumber = HorizontalNumber;
        NowVerticalSelectNumber = VerticalNumber;
    }

    public static void LockButton(int HorizontalNumber, int VerticalNumber, string RLExit, string UDExit)
    {
        BeLockButton beLockButton = new BeLockButton();
        beLockButton.ButtonHorizontalNumber = HorizontalNumber;
        beLockButton.ButtonVerticalNumber = VerticalNumber;
        beLockButton.RLDefaultExit = RLExit;
        beLockButton.UDDefaultExit = UDExit;

        BeLockButtonList.Add(beLockButton);
    }

    public static void UnLockButton()
    {
        BeLockButtonList.Clear();
    }

    private void DetectLockButton()
    {
        if (BeLockButtonList.Count > 0)
        {
            BeLockButtonList.ForEach(delegate (BeLockButton _button)
            {
                if(DetectButtonSelected(_button.ButtonHorizontalNumber, _button.ButtonVerticalNumber))
                {
                    //開始執行引導位置
                    if (LastHorizontalSelectNumber < NowHorizontalSelectNumber)
                    {
                        if (DetectButtonValidable(NowHorizontalSelectNumber + 1, NowVerticalSelectNumber))
                        {
                            NowHorizontalSelectNumber += 1;
                        }
                        else
                        {
                            switch (_button.UDDefaultExit)
                            {
                                case "U":
                                    if (DetectButtonValidable(NowHorizontalSelectNumber, NowVerticalSelectNumber - 1))
                                    {
                                        NowVerticalSelectNumber -= 1;
                                    }
                                    else
                                    {
                                        ShouldPlayMoveSound = false;
                                        NowHorizontalSelectNumber = LastHorizontalSelectNumber;
                                        NowVerticalSelectNumber = LastVerticalSelectNumber;
                                    }
                                    break;
                                case "D":
                                    if (DetectButtonValidable(NowHorizontalSelectNumber, NowVerticalSelectNumber + 1))
                                    {
                                        NowVerticalSelectNumber += 1;
                                    }
                                    else
                                    {
                                        ShouldPlayMoveSound = false;
                                        NowHorizontalSelectNumber = LastHorizontalSelectNumber;
                                        NowVerticalSelectNumber = LastVerticalSelectNumber;
                                    }
                                    break;
                            }
                        }
                        return;
                    }//按右
                    if (LastHorizontalSelectNumber > NowHorizontalSelectNumber)
                    {
                        if (DetectButtonValidable(NowHorizontalSelectNumber - 1, NowVerticalSelectNumber))
                        {
                            NowHorizontalSelectNumber -= 1;
                        }
                        else
                        {
                            switch (_button.UDDefaultExit)
                            {
                                case "U":
                                    if (DetectButtonValidable(NowHorizontalSelectNumber, NowVerticalSelectNumber - 1))
                                    {
                                        NowVerticalSelectNumber -= 1;
                                    }
                                    else
                                    {
                                        ShouldPlayMoveSound = false;
                                        NowHorizontalSelectNumber = LastHorizontalSelectNumber;
                                        NowVerticalSelectNumber = LastVerticalSelectNumber;
                                    }
                                    break;
                                case "D":
                                    if (DetectButtonValidable(NowHorizontalSelectNumber, NowVerticalSelectNumber + 1))
                                    {
                                        NowVerticalSelectNumber += 1;
                                    }
                                    else
                                    {
                                        ShouldPlayMoveSound = false;
                                        NowHorizontalSelectNumber = LastHorizontalSelectNumber;
                                        NowVerticalSelectNumber = LastVerticalSelectNumber;
                                    }
                                    break;
                            }
                        }
                        return;
                    }//按左
                    if (LastVerticalSelectNumber > NowVerticalSelectNumber)
                    {
                        if (DetectButtonValidable(NowHorizontalSelectNumber, NowVerticalSelectNumber - 1))
                        {
                            NowVerticalSelectNumber -= 1;
                        }
                        else
                        {
                            switch (_button.RLDefaultExit)
                            {
                                case "R":
                                    if (DetectButtonValidable(NowHorizontalSelectNumber+1, NowVerticalSelectNumber))
                                    {
                                        NowHorizontalSelectNumber += 1;
                                    }
                                    else
                                    {
                                        ShouldPlayMoveSound = false;
                                        NowHorizontalSelectNumber = LastHorizontalSelectNumber;
                                        NowVerticalSelectNumber = LastVerticalSelectNumber;
                                    }
                                    break;
                                case "L":
                                    if (DetectButtonValidable(NowHorizontalSelectNumber - 1, NowVerticalSelectNumber))
                                    {
                                        NowHorizontalSelectNumber -= 1;
                                    }
                                    else
                                    {
                                        ShouldPlayMoveSound = false;
                                        NowHorizontalSelectNumber = LastHorizontalSelectNumber;
                                        NowVerticalSelectNumber = LastVerticalSelectNumber;
                                    }
                                    break;
                            }
                        }
                        return;
                    }//按上
                    if (LastVerticalSelectNumber < NowVerticalSelectNumber)
                    {
                        if (DetectButtonValidable(NowHorizontalSelectNumber, NowVerticalSelectNumber + 1))
                        {
                            NowVerticalSelectNumber += 1;
                        }
                        else
                        {
                            switch (_button.RLDefaultExit)
                            {
                                case "R":
                                    if (DetectButtonValidable(NowHorizontalSelectNumber + 1, NowVerticalSelectNumber))
                                    {
                                        NowHorizontalSelectNumber += 1;
                                    }
                                    else
                                    {
                                        ShouldPlayMoveSound = false;
                                        NowHorizontalSelectNumber = LastHorizontalSelectNumber;
                                        NowVerticalSelectNumber = LastVerticalSelectNumber;
                                    }
                                    break;
                                case "L":
                                    if (DetectButtonValidable(NowHorizontalSelectNumber - 1, NowVerticalSelectNumber))
                                    {
                                        NowHorizontalSelectNumber -= 1;
                                    }
                                    else
                                    {
                                        ShouldPlayMoveSound = false;
                                        NowHorizontalSelectNumber = LastHorizontalSelectNumber;
                                        NowVerticalSelectNumber = LastVerticalSelectNumber;
                                    }
                                    break;
                            }
                        }
                        return;
                    }//按下
                }
            });
        }
    }//檢查該位置是否為被鎖定位置並引導至新位置

    private bool DetectButtonValidable(int HNumber, int VNumber)
    {
        bool DetectResult = true;

        if (BeLockButtonList.Count > 0)
        {
            BeLockButtonList.ForEach(delegate (BeLockButton _button)
            {
                if (HNumber == _button.ButtonHorizontalNumber && VNumber == _button.ButtonVerticalNumber)
                {
                    DetectResult = false;
                }
            });
        }//檢測是否為被鎖定按鈕

        if (HNumber > MaxHorizontalSelectNumber || VNumber > MaxVerticalSelectNumber || HNumber < 1 || VNumber < 1)
        {
            DetectResult = false;
        }//檢查是否超出範圍

        return DetectResult;
    }//檢查該位置是否有效

    public void MoveSoundPlay()
    {
        MoveSource.Play();
    }

    public static void ConfirmSoundPlay()
    {
        ConfirmSource.Play();
    }

    public static void ItemMoveSoundPlay()
    {
        ItemMoveSource.Play();
    }
}

public class BeLockButton
{
    public int ButtonHorizontalNumber = 0;
    public int ButtonVerticalNumber = 0;
    public string RLDefaultExit;//左右
    public string UDDefaultExit;//上下
}
