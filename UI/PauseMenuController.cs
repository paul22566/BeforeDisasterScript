using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    private GameObject PauseMenu;
    private GameObject SelectElevatorMenu;
    private GameObject KeyCodeMenu;
    private KeyCodeManage _keyCodeManage;
    private GameObject SoundMenu;
    private FadeOutUI TitleFadeOut;
    private GameObject LackKeyCodeNotice;
    private GameObject Option;
    private GameObject TutorialMenu;
    private Transform _transform;
    private ItemWindow Item;
    private TutorialWindow _tutorialWindow;
    private bool isKeyCodeManageOpen;
    private bool isOptionOpen;
    private bool isSoundControllerOpen;
    public static bool isPauseMenuOpen = false;//其他Script會用到(playerController，NormalFadeOut)
    public static bool OpenAnyMenu = false;
    private PlayerData _PlayerData;
    private Button GoToTitleButton;
    private Button OpenOptionButton;
    private Button KeyCodeButton;
    private Button SoundControllerButton;
    private Button _tutorialButton;
    [HideInInspector] public Button Select4FButton;//script(Elevator)
    [HideInInspector] public Button Select3FButton;//script(Elevator)
    [HideInInspector] public Button Select2FButton;//script(Elevator)
    [HideInInspector] public Button Select1FButton;//script(Elevator)
    private Slider BGMSlider;
    private Slider SESlider;
    private Portal TitlePortal;

    void Start()
    {
        _transform = this.transform;
        if (_transform != null)
        {
            TutorialMenu = IdentifyID.FindObject(_transform, UIID.TutorialMenu);
            _tutorialWindow = TutorialMenu.GetComponent<TutorialWindow>();
            PauseMenu = IdentifyID.FindObject(_transform, UIID.PauseMenu);
            SelectElevatorMenu = IdentifyID.FindObject(_transform, UIID.SelectPortalMenu);
            KeyCodeMenu = IdentifyID.FindObject(_transform, UIID.KeyCodeManage);
            _keyCodeManage = _transform.GetComponent<KeyCodeManage>();
            SoundMenu = IdentifyID.FindObject(_transform, UIID.SoundController);
            TitleFadeOut = IdentifyID.FindObject(_transform, UIID.FadeOut).GetComponent<FadeOutUI>();
            LackKeyCodeNotice = KeyCodeMenu.transform.GetChild(16).gameObject;
            Option = IdentifyID.FindObject(_transform, UIID.Option);
            Item = IdentifyID.FindObject(_transform, UIID.Item).GetComponent<ItemWindow>();
            OpenOptionButton = PauseMenu.transform.GetChild(0).GetComponent<Button>();
            _tutorialButton = PauseMenu.transform.GetChild(1).GetComponent<Button>();
            GoToTitleButton = PauseMenu.transform.GetChild(2).GetComponent<Button>();
            KeyCodeButton = Option.transform.GetChild(1).GetComponent<Button>();
            SoundControllerButton = Option.transform.GetChild(2).GetComponent<Button>();
            Select4FButton = SelectElevatorMenu.transform.GetChild(0).GetComponent<Button>();
            Select3FButton = SelectElevatorMenu.transform.GetChild(1).GetComponent<Button>();
            Select2FButton = SelectElevatorMenu.transform.GetChild(2).GetComponent<Button>();
            Select1FButton = SelectElevatorMenu.transform.GetChild(3).GetComponent<Button>();
            BGMSlider = SoundMenu.transform.GetChild(1).GetComponent<Slider>();
            SESlider = SoundMenu.transform.GetChild(2).GetComponent<Slider>();
            OpenOptionButton.onClick.AddListener(OpenOption);
            _tutorialButton.onClick.AddListener(OpenTutorial);
            GoToTitleButton.onClick.AddListener(GoToTitle);
            KeyCodeButton.onClick.AddListener(OpenKeyCodeManage);
            SoundControllerButton.onClick.AddListener(OpenSoundController);
            BGMSlider.onValueChanged.AddListener(ChangeBGM);
            SESlider.onValueChanged.AddListener(ChangeSE);
            BGMSlider.value = MusicController.BGMVolume;
            SESlider.value = SEController.SEVolume;
        }

        TitlePortal = this.GetComponent<Portal>();

        if (GameObject.Find("FollowSystem") != null)
        {
            _PlayerData = GameObject.Find("FollowSystem").GetComponent<PlayerData>();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton7))
        {
            if (!BackgroundSystem.CantPause && !isPauseMenuOpen)
            {
                OpenPauseMenu();
                return;
            }
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            if (!BackgroundSystem.CantPause && !_tutorialWindow.isTutorialDetailAppear)
            {
                if (!Boss3Controller.EndAppear)
                {
                    if (isPauseMenuOpen)
                    {
                        if (!isOptionOpen && !_tutorialWindow.isTutorialOpen)
                        {
                            ClosePauseMenu();
                        }
                        if (isOptionOpen)
                        {
                            if (!isKeyCodeManageOpen && !isSoundControllerOpen)
                            {
                                CloseOption();
                            }
                            else
                            {
                                if (isKeyCodeManageOpen)
                                {
                                    if (_keyCodeManage.isAlert)
                                    {
                                        LackKeyCodeNotice.SetActive(true);
                                    }
                                    else
                                    {
                                        if (!_keyCodeManage.isInputOpen && !_keyCodeManage.isInputOpenC)
                                        {
                                            CloseKeyCodeMenu();
                                        }
                                        if (_keyCodeManage.isInputOpenC)
                                        {
                                            _keyCodeManage.isInputOpenC = false;
                                        }
                                    }
                                }
                                if (isSoundControllerOpen)
                                {
                                    CloseSoundMenu();
                                }
                            }
                        }
                        if (_tutorialWindow.isTutorialOpen)
                        {
                            CloseTutorialMenu();
                        }
                    }
                }
                else
                {
                    Boss3Controller.EndAppear = false;
                    SceneManager.LoadScene("title");
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!BackgroundSystem.CantPause && !_tutorialWindow.isTutorialDetailAppear)
            {
                if (!Boss3Controller.EndAppear)
                {
                    if (!isPauseMenuOpen)
                    {
                        OpenPauseMenu();
                        return;
                    }

                    if (isPauseMenuOpen)
                    {
                        if (!isOptionOpen && !_tutorialWindow.isTutorialOpen)
                        {
                            ClosePauseMenu();
                        }
                        if (isOptionOpen)
                        {
                            if (!isKeyCodeManageOpen && !isSoundControllerOpen)
                            {
                                CloseOption();
                            }
                            else
                            {
                                if (isKeyCodeManageOpen)
                                {
                                    if (_keyCodeManage.isAlert)
                                    {
                                        LackKeyCodeNotice.SetActive(true);
                                    }
                                    else
                                    {
                                        if (!_keyCodeManage.isInputOpen && !_keyCodeManage.isSelectC)
                                        {
                                            CloseKeyCodeMenu();
                                        }
                                        if (_keyCodeManage.isInputOpen)
                                        {
                                            _keyCodeManage.isInputOpen = false;
                                        }
                                    }
                                }
                                if (isSoundControllerOpen)
                                {
                                    CloseSoundMenu();
                                }
                            }
                        }
                        if (_tutorialWindow.isTutorialOpen)
                        {
                            CloseTutorialMenu();
                        }
                    }
                }
                else
                {
                    Boss3Controller.EndAppear = false;
                    SceneManager.LoadScene("LoadingTitle");
                }
            }
        }

        if (_keyCodeManage != null)
        {
            if (!_keyCodeManage.isSelect && _keyCodeManage.isInputOpen)
            {
                _keyCodeManage.isInputOpen = false;
            }
            if (!_keyCodeManage.isSelectC && _keyCodeManage.isInputOpenC)
            {
                _keyCodeManage.isInputOpenC = false;
            }
            if (_keyCodeManage.isInputOpen || _keyCodeManage.isInputOpenC)
            {
                return;
            }
        }
    }

    private void GoToTitle()//Button專用
    {
        BackgroundSystem.GameSpeed = 1;
        MusicController.ChangeBGM();
        TitleFadeOut._fadeOutEnd += TitlePortal.OnBeginLoadScene;
        TitleFadeOut.BeginFadeOut();
    }

    public void OpenKeyCodeManage()//Button專用
    {
        SelectButtonController.MaxSelectNumberSet(2, 14);
        KeyCodeMenu.SetActive(true);
        isKeyCodeManageOpen = true;
        SelectButtonController.LockButton(2, 1, "L", "D");
        SelectButtonController.LockButton(2, 2, "L", "D");
        SelectButtonController.LockButton(2, 13, "L", "U");
        SelectButtonController.LockButton(2, 14, "L", "U");
        KeyCodeMenu.GetComponent<DefaultButton>().ShouldOpen = true;
        Option.GetComponent<DefaultButton>().onStart = KeyCodeButton;
        Option.SetActive(false);
    }

    public void OpenSoundController()//Button專用
    {
        SelectButtonController.MaxSelectNumberSet(1, 2);
        SoundMenu.SetActive(true);
        isSoundControllerOpen = true;
        SoundMenu.GetComponent<DefaultButton>().ShouldOpen = true;
        Option.GetComponent<DefaultButton>().onStart = SoundControllerButton;
        Option.SetActive(false);
    }

    public void OpenOption()//Button專用
    {
        SelectButtonController.MaxSelectNumberSet(1, 2);
        Option.SetActive(true);
        isOptionOpen = true;
        Option.GetComponent<DefaultButton>().ShouldOpen = true;
        PauseMenu.GetComponent<DefaultButton>().onStart = OpenOptionButton;
        PauseMenu.SetActive(false);
    }

    public void OpenTutorial()//Button專用
    {
        TutorialMenu.SetActive(true);
        _tutorialWindow.isTutorialOpen = true;
        SelectButtonController.CloseSelectButtonController();
        PauseMenu.GetComponent<DefaultButton>().onStart = _tutorialButton;
        PauseMenu.SetActive(false);
    }

    public void ChangeBGM(float value)//Button專用
    {
        MusicController.BGMVolume = value;
    }

    public void ChangeSE(float value)//Button專用
    {
        SEController.SEVolume = value;
    }

    private void OpenPauseMenu()
    {
        isPauseMenuOpen = true;
        OpenAnyMenu = true;
        BackgroundSystem.GameSpeed = 0;
        PauseMenu.SetActive(true);
        SelectButtonController.MaxSelectNumberSet(1, 3);
        SelectButtonController.OpenSelectButtonController();
        PauseMenu.GetComponent<DefaultButton>().ShouldOpen = true;
    }

    private void ClosePauseMenu()
    {
        isPauseMenuOpen = false;
        OpenAnyMenu = false;
        BackgroundSystem.GameSpeed = 1;
        SelectButtonController.CloseSelectButtonController();
        PauseMenu.SetActive(false);
    }

    private void CloseOption()
    {
        Option.SetActive(false);
        PauseMenu.SetActive(true);
        SelectButtonController.MaxSelectNumberSet(1, 3);
        PauseMenu.GetComponent<DefaultButton>().ShouldOpen = true;
        isOptionOpen = false;
    }

    private void CloseKeyCodeMenu()
    {
        isKeyCodeManageOpen = false;
        KeyCodeMenu.SetActive(false);
        LackKeyCodeNotice.SetActive(false);
        SelectButtonController.MaxSelectNumberSet(1, 2);
        SelectButtonController.UnLockButton();
        Option.SetActive(true);
        Option.GetComponent<DefaultButton>().ShouldOpen = true;
        _PlayerData.KeyCodeSave();
    }
    
    private void CloseSoundMenu()
    {
        isSoundControllerOpen = false;
        SoundMenu.SetActive(false);
        SelectButtonController.MaxSelectNumberSet(1, 2);
        Option.SetActive(true);
        Option.GetComponent<DefaultButton>().ShouldOpen = true;
        _PlayerData.SoundVolumeSave();
    }

    private void CloseTutorialMenu()
    {
        _tutorialWindow.isTutorialOpen = false;
        _tutorialWindow.TutorialWindowNumber = 999;
        _tutorialWindow.isTutorialButtonAppear = false;
        TutorialButton.NowTutorialButton = 1;
        _tutorialWindow.TutorialButtonOrderRecord.Clear();
        _tutorialWindow.SaveReadStatus();

        PauseMenu.SetActive(true);
        SelectButtonController.MaxSelectNumberSet(1, 3);
        SelectButtonController.OpenSelectButtonController();
        PauseMenu.GetComponent<DefaultButton>().ShouldOpen = true;
    }
}
