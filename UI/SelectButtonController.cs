using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SelectButtonController : MonoBehaviour
{
    private static bool HasOpenController;
    private static List<BeLockButton> BeLockButtonList = new List<BeLockButton>();
    private static bool isOpen;

    private static int NowVerticalSelectNumber = 1;//��LScript�|�Ψ�(PauseMenuController�ATitleController�AUIButtonBeSelected)
    private static int NowHorizontalSelectNumber = 1;//��LScript�|�Ψ�(PauseMenuController�ATitleController�AUIButtonBeSelected)
    private static int MaxVerticalSelectNumber = 1;//��LScript�|�Ψ�(PauseMenuController�ATitleController�AplayerController)
    private static int MaxHorizontalSelectNumber = 1;//��LScript�|�Ψ�(PauseMenuController�ATitleController�AplayerController)

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
    }//����I����

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
    }//��������

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
                    //�}�l����޾ɦ�m
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
                    }//���k
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
                    }//����
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
                    }//���W
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
                    }//���U
                }
            });
        }
    }//�ˬd�Ӧ�m�O�_���Q��w��m�ä޾ɦܷs��m

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
        }//�˴��O�_���Q��w���s

        if (HNumber > MaxHorizontalSelectNumber || VNumber > MaxVerticalSelectNumber || HNumber < 1 || VNumber < 1)
        {
            DetectResult = false;
        }//�ˬd�O�_�W�X�d��

        return DetectResult;
    }//�ˬd�Ӧ�m�O�_����

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
    public string RLDefaultExit;//���k
    public string UDDefaultExit;//�W�U
}
