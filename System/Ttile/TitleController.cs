using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{
    private GameObject UI;
    public GameObject TitleWord;
    public FadeOutUI FadeOut;
    public GameObject CreateDataChooseMenu;
    public GameObject SoundMenu;
    public GameObject KeyCodeMenu;
    private KeyCodeManage _keyCodeManage;
    public GameObject Option;
    public GameObject LoadMenu;
    public GameObject CoverDataNotice1;
    public GameObject CoverDataNotice2;
    public GameObject CoverDataNotice3;
    public GameObject LackKeyCodeNotice;
    private PlayerData _PlayerData;
    private SaveSystem SL;
    public Text Data1InCreate;
    public Text Data2InCreate;
    public Text Data3InCreate;
    public Text Data1InLoad;
    public GameObject Data1KilledBossSoul1;
    public GameObject Data1KilledBossSoul2;
    public GameObject Data1KilledBossSoul3;
    public GameObject LData1KilledBossSoul1;
    public GameObject LData1KilledBossSoul2;
    public GameObject LData1KilledBossSoul3;
    public Text Data2InLoad;
    public GameObject Data2KilledBossSoul1;
    public GameObject Data2KilledBossSoul2;
    public GameObject Data2KilledBossSoul3;
    public GameObject LData2KilledBossSoul1;
    public GameObject LData2KilledBossSoul2;
    public GameObject LData2KilledBossSoul3;
    public Text Data3InLoad;
    public GameObject Data3KilledBossSoul1;
    public GameObject Data3KilledBossSoul2;
    public GameObject Data3KilledBossSoul3;
    public GameObject LData3KilledBossSoul1;
    public GameObject LData3KilledBossSoul2;
    public GameObject LData3KilledBossSoul3;
    public Button NewGameButton;
    public Button LoadButton;
    public Button OptionButton;
    public Button ExitButton;
    public Button Data1Button;
    public Button Data2Button;
    public Button Data3Button;
    public Button OpenKeyCodeButton;
    public Button OpenSoundControllerButton;
    public Slider BGMSlider;
    public Slider SESlider;
    private bool isLoadMenuOpen;
    private bool isOptionOpen;
    private bool isKeyCodeManageOpen;
    private bool isCreateDataChooseMenuOpen;
    private bool isSoundControllerOpen;
    private bool ShouldDetectData = false;
    private bool isCoverDataNoticeOpen;
    private bool DataExist;

    private TitleAni _titleAni;

    private void Awake()
    {
        UI = GameObject.FindWithTag("UI");
        _PlayerData = GameObject.Find("PlayerData").GetComponent<PlayerData>();
        SL = GameObject.Find("PlayerData").GetComponent<SaveSystem>();
        _keyCodeManage = UI.GetComponent<KeyCodeManage>();
        _titleAni = this.GetComponent<TitleAni>();
        BGMSlider.value = MusicController.BGMVolume;
        SESlider.value = SEController.SEVolume;
        FadeOut._fadeOutEnd += OnFadeOutEnd;
        SelectableInitialize();
    }

    private void Start()
    {
        LoadKeyCodeData();
        LoadSoundVolume();
        SelectButtonController.OpenSelectButtonController();
        SelectButtonController.MaxSelectNumberSet(1, 4);

        LoadButtonDetect();

        if (!DataExist)
        {
            LoadButton.GetComponent<Image>().color = new Color(0.3f, 0.3f, 0.3f, 1);
            LoadButton.GetComponent<UIButtonBeSelected>().VNumber = 0;
            OptionButton.GetComponent<UIButtonBeSelected>().VNumber = 2;
            ExitButton.GetComponent<UIButtonBeSelected>().VNumber = 3;
            SelectButtonController.MaxSelectNumberSet(1, 3);
        }
        if (DataExist)
        {
            UI.GetComponent<DefaultButton>().onStart = LoadButton;
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (ShouldDetectData)
        {
            DetectDataAndShowSoulInCreatDataMenu();
            DetectDataAndShowSoulInLoadMenu();
            ShouldDetectData = false;
        }
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            if (isOptionOpen)
            {
                if(!isKeyCodeManageOpen && !isSoundControllerOpen)
                {
                    NewGameButton.interactable = true;
                    LoadButton.interactable = true;
                    OptionButton.interactable = true;
                    ExitButton.interactable = true;
                    if (DataExist)
                    {
                        SelectButtonController.MaxSelectNumberSet(1, 4);
                    }
                    else
                    {
                        SelectButtonController.MaxSelectNumberSet(1, 3);
                    }
                    Option.SetActive(false);
                    isOptionOpen = false;
                    UI.GetComponent<DefaultButton>().ShouldOpen = true;
                }
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
                            isKeyCodeManageOpen = false;
                            KeyCodeMenu.SetActive(false);
                            LackKeyCodeNotice.SetActive(false);
                            SelectButtonController.MaxSelectNumberSet(1, 2);
                            SelectButtonController.UnLockButton();
                            Option.SetActive(true);
                            Option.GetComponent<DefaultButton>().ShouldOpen = true;
                            _PlayerData.KeyCodeSave();
                        }
                        if (_keyCodeManage.isInputOpen)
                        {
                            _keyCodeManage.isInputOpen = false;
                        }
                        if (_keyCodeManage.isInputOpenC)
                        {
                            _keyCodeManage.isInputOpenC = false;
                        }
                    }
                }
                if (isSoundControllerOpen)
                {
                    SoundMenu.SetActive(false);
                    Option.SetActive(true);
                    isSoundControllerOpen = false;
                    SelectButtonController.MaxSelectNumberSet(1, 2);
                    Option.GetComponent<DefaultButton>().ShouldOpen = true;
                    _PlayerData.SoundVolumeSave();
                }
            }
            if (isLoadMenuOpen)
            {
                NewGameButton.interactable = true;
                LoadButton.interactable = true;
                OptionButton.interactable = true;
                ExitButton.interactable = true;
                isLoadMenuOpen = false;
                LoadMenu.SetActive(false);
                if (DataExist)
                {
                    SelectButtonController.MaxSelectNumberSet(1, 4);
                }
                else
                {
                    SelectButtonController.MaxSelectNumberSet(1, 3);
                }
                UI.GetComponent<DefaultButton>().ShouldOpen = true;
            }
            if (isCreateDataChooseMenuOpen)
            {
                if (isCoverDataNoticeOpen)
                {
                    isCoverDataNoticeOpen = false;
                    CoverDataNotice1.SetActive(false);
                    CoverDataNotice2.SetActive(false);
                    CoverDataNotice3.SetActive(false);
                    Data1Button.interactable = true;
                    Data2Button.interactable = true;
                    Data3Button.interactable = true;
                    SelectButtonController.MaxSelectNumberSet(1, 3);
                    CreateDataChooseMenu.GetComponent<DefaultButton>().ShouldOpen = true;
                }
                else
                {
                    NewGameButton.interactable = true;
                    LoadButton.interactable = true;
                    OptionButton.interactable = true;
                    ExitButton.interactable = true;
                    isCreateDataChooseMenuOpen = false;
                    CreateDataChooseMenu.SetActive(false);
                    if (DataExist)
                    {
                        SelectButtonController.MaxSelectNumberSet(1, 4);
                    }
                    else
                    {
                        SelectButtonController.MaxSelectNumberSet(1, 3);
                    }
                    UI.GetComponent<DefaultButton>().ShouldOpen = true;
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

    private void LoadButtonDetect()
    {
        string FilePath = Application.dataPath + "/StreamingAssets" + "/Save";
        string NameAndPath = "";
        string DataName;

        for (int i = 0; i < 3; i++)
        {
            switch (i)
            {
                case 0:
                    DataName = "Data1";
                    NameAndPath = FilePath + "/" + DataName;

                    if (File.Exists(NameAndPath))
                    {
                        DataExist = true;
                    }
                    break;
                case 1:
                    DataName = "Data2";
                    NameAndPath = FilePath + "/" + DataName;

                    if (File.Exists(NameAndPath))
                    {
                        DataExist = true;
                    }
                    break;
                case 2:
                    DataName = "Data3";
                    NameAndPath = FilePath + "/" + DataName;

                    if (File.Exists(NameAndPath))
                    {
                        DataExist = true;
                    }
                    break;
            }
        }
    }

    public void CreateGameStart()
    {
        CreatePlayer.isNewGame = true;
        UI.SetActive(false);
        TitleWord.SetActive(false);
        _titleAni.BeginOpenDoor();
        SelectButtonController.CloseSelectButtonController();
        MusicController.BeginFadeOutBGM(0.5f, 0.3f);
        GameEvent.EnterGameWithNormalWay = true;
    }

    public void LoadGameStart(string DataName)
    {
        string FilePath = Application.dataPath + "/StreamingAssets" + "/Save";
        string NameAndPath = "";

        NameAndPath = FilePath + "/" + DataName;

        if (File.Exists(NameAndPath))
        {
            GameEvent.NowDataName = DataName;
            CreatePlayer.isLoadGame = true;
            FadeOut.BeginFadeOut(2.5f);
            SelectButtonController.CloseSelectButtonController();
            MusicController.ChangeBGM();
            GameEvent.EnterGameWithNormalWay = true;
        }
    }

    public void OnFadeOutEnd()
    {
        SceneManager.LoadScene("CreatePlayer");
    }

    public void OpenOption()
    {
        NewGameButton.interactable = false;
        LoadButton.interactable = false;
        OptionButton.interactable = false;
        ExitButton.interactable = false;
        Option.SetActive(true);
        isOptionOpen = true;
        SelectButtonController.MaxSelectNumberSet(1,2);
        UI.GetComponent<DefaultButton>().onStart = OptionButton;
        Option.GetComponent<DefaultButton>().ShouldOpen = true;
    }

    public void OpenLoadMenu()
    {
        NewGameButton.interactable = false;
        LoadButton.interactable = false;
        OptionButton.interactable = false;
        ExitButton.interactable = false;
        ShouldDetectData = true;
        SelectButtonController.MaxSelectNumberSet(1, 3);
        UI.GetComponent<DefaultButton>().onStart = LoadButton;
        LoadMenu.GetComponent<DefaultButton>().ShouldOpen = true;
        LoadMenu.SetActive(true);
        isLoadMenuOpen = true;
    }

    public void OpenKeyCodeManage()
    {
        KeyCodeMenu.SetActive(true);
        isKeyCodeManageOpen = true;
        SelectButtonController.MaxSelectNumberSet(2, 14);
        Option.GetComponent<DefaultButton>().onStart = OpenKeyCodeButton;
        KeyCodeMenu.GetComponent<DefaultButton>().ShouldOpen = true;
        SelectButtonController.LockButton(2, 1, "L", "D");
        SelectButtonController.LockButton(2, 2, "L", "D");
        SelectButtonController.LockButton(2, 13, "L", "U");
        SelectButtonController.LockButton(2, 14, "L", "U");
        Option.SetActive(false);
    }

    public void OpenSoundController()
    {
        SoundMenu.SetActive(true);
        isSoundControllerOpen = true;
        SelectButtonController.MaxSelectNumberSet(1, 2);
        Option.GetComponent<DefaultButton>().onStart = OpenSoundControllerButton;
        SoundMenu.GetComponent<DefaultButton>().ShouldOpen = true;
        Option.SetActive(false);
    }

    public void ChangeBGM(float value)
    {
        MusicController.BGMVolume = value;
    }

    public void ChangeSE(float value)
    {
        SEController.SEVolume = value;
    }

    public void ExitGame()
    {
        print("exit");
        Application.Quit();
    }

    public void OpenCreateDataChooseMenu()
    {
        NewGameButton.interactable = false;
        LoadButton.interactable = false;
        OptionButton.interactable = false;
        ExitButton.interactable = false;
        ShouldDetectData = true;
        CreateDataChooseMenu.SetActive(true);
        isCreateDataChooseMenuOpen = true;
        SelectButtonController.MaxSelectNumberSet(1, 3);
        UI.GetComponent<DefaultButton>().onStart = NewGameButton;
        CreateDataChooseMenu.GetComponent<DefaultButton>().ShouldOpen = true;
    }

    private void AlertCoverData1()
    {
        isCoverDataNoticeOpen = true;
        Data1Button.interactable = false;
        Data2Button.interactable = false;
        Data3Button.interactable = false;
        CoverDataNotice1.SetActive(true);
        SelectButtonController.MaxSelectNumberSet(2, 1);
        CreateDataChooseMenu.GetComponent<DefaultButton>().onStart = Data1Button;
        CoverDataNotice1.GetComponent<DefaultButton>().ShouldOpen = true;
    }
    private void AlertCoverData2()
    {
        isCoverDataNoticeOpen = true;
        Data1Button.interactable = false;
        Data2Button.interactable = false;
        Data3Button.interactable = false;
        CoverDataNotice2.SetActive(true);
        SelectButtonController.MaxSelectNumberSet(2, 1);
        CreateDataChooseMenu.GetComponent<DefaultButton>().onStart = Data2Button;
        CoverDataNotice2.GetComponent<DefaultButton>().ShouldOpen = true;
    }
    private void AlertCoverData3()
    {
        isCoverDataNoticeOpen = true;
        Data1Button.interactable = false;
        Data2Button.interactable = false;
        Data3Button.interactable = false;
        CoverDataNotice3.SetActive(true);
        SelectButtonController.MaxSelectNumberSet(2, 1);
        CreateDataChooseMenu.GetComponent<DefaultButton>().onStart = Data3Button;
        CoverDataNotice3.GetComponent<DefaultButton>().ShouldOpen = true;
    }

    public void CancelCoverDate()
    {
        isCoverDataNoticeOpen = false;
        CoverDataNotice1.SetActive(false);
        CoverDataNotice2.SetActive(false);
        CoverDataNotice3.SetActive(false);
        Data1Button.interactable = true;
        Data2Button.interactable = true;
        Data3Button.interactable = true;
        SelectButtonController.MaxSelectNumberSet(1, 3);
        CreateDataChooseMenu.GetComponent<DefaultButton>().ShouldOpen = true;
    }

    public void DetectData(string DataName)
    {
        string FilePath = Application.dataPath + "/StreamingAssets" + "/Save";
        string NameAndPath = "";

        NameAndPath = FilePath + "/" + DataName;

        GameEvent.NowDataName = DataName;
        if (File.Exists(NameAndPath))
        {
            switch (DataName)
            {
                case "Data1":
                    AlertCoverData1();
                    break;
                case "Data2":
                    AlertCoverData2();
                    break;
                case "Data3":
                    AlertCoverData3();
                    break;
            }
        }
        else
        {
            CreateGameStart();
        }
    }

    public void LoadKeyCodeData()
    {
        string FilePath = Application.dataPath + "/StreamingAssets" + "/Save";
        string NameAndPath = "";
        NameAndPath = FilePath + "/" + "KeyCodeSave";
        if (File.Exists(NameAndPath))
        {
            _PlayerData.KeyCodeLoad();
        }
    }

    public void LoadSoundVolume()
    {
        float BGMValue = 0;
        float SEValue = 0;

        string FilePath = Application.dataPath + "/StreamingAssets" + "/Save";
        string NameAndPath = "";
        NameAndPath = FilePath + "/" + "SoundVolumeSave";
        if (File.Exists(NameAndPath))
        {
            _PlayerData.SoundVolumeLoad(ref BGMValue, ref SEValue);
            BGMSlider.value = BGMValue;
            SESlider.value = SEValue;
        }
    }

    private void DetectDataAndShowSoulInCreatDataMenu()
    {
        string FilePath = Application.dataPath + "/StreamingAssets" + "/Save";
        string NameAndPath = "";
        string DataName = "";

        for (int i = 0; i < 3; i++)
        {
            switch (i)
            {
                case 0:
                    DataName = "Data1";
                    break;
                case 1:
                    DataName = "Data2";
                    break;
                case 2:
                    DataName = "Data3";
                    break;
            }
            NameAndPath = FilePath + "/" + DataName;

            if (File.Exists(NameAndPath))
            {
                switch (i)
                {
                    case 0:
                        PlayerDataType p1 = (PlayerDataType)SL.LoadData(typeof(PlayerDataType), DataName);
                        GameEvent.KilledBossNumber = p1.KilledBossNumber;
                        Data1InCreate.text = "Data 1";
                        if (GameEvent.KilledBossNumber >= 1)
                        {
                            Data1KilledBossSoul1.SetActive(true);
                        }
                        if (GameEvent.KilledBossNumber >= 2)
                        {
                            Data1KilledBossSoul2.SetActive(true);
                        }
                        if (GameEvent.KilledBossNumber >= 3)
                        {
                            Data1KilledBossSoul3.SetActive(true);
                        }
                        break;
                    case 1:
                        PlayerDataType p2 = (PlayerDataType)SL.LoadData(typeof(PlayerDataType), DataName);
                        GameEvent.KilledBossNumber = p2.KilledBossNumber;
                        Data2InCreate.text = "Data 2";
                        if (GameEvent.KilledBossNumber >= 1)
                        {
                            Data2KilledBossSoul1.SetActive(true);
                        }
                        if (GameEvent.KilledBossNumber >= 2)
                        {
                            Data2KilledBossSoul2.SetActive(true);
                        }
                        if (GameEvent.KilledBossNumber >= 3)
                        {
                            Data2KilledBossSoul3.SetActive(true);
                        }
                        break;
                    case 2:
                        PlayerDataType p3 = (PlayerDataType)SL.LoadData(typeof(PlayerDataType), DataName);
                        GameEvent.KilledBossNumber = p3.KilledBossNumber;
                        Data3InCreate.text = "Data 3";
                        if (GameEvent.KilledBossNumber >= 1)
                        {
                            Data3KilledBossSoul1.SetActive(true);
                        }
                        if (GameEvent.KilledBossNumber >= 2)
                        {
                            Data3KilledBossSoul2.SetActive(true);
                        }
                        if (GameEvent.KilledBossNumber >= 3)
                        {
                            Data3KilledBossSoul3.SetActive(true);
                        }
                        break;
                }
            }
            else
            {
                switch (i)
                {
                    case 0:
                        Data1InCreate.text = "No Data";
                        Data1KilledBossSoul1.SetActive(false);
                        Data1KilledBossSoul2.SetActive(false);
                        Data1KilledBossSoul3.SetActive(false);
                        break;
                    case 1:
                        Data2InCreate.text = "No Data";
                        Data2KilledBossSoul1.SetActive(false);
                        Data2KilledBossSoul2.SetActive(false);
                        Data2KilledBossSoul3.SetActive(false);
                        break;
                    case 2:
                        Data3InCreate.text = "No Data";
                        Data3KilledBossSoul1.SetActive(false);
                        Data3KilledBossSoul2.SetActive(false);
                        Data3KilledBossSoul3.SetActive(false);
                        break;
                }
            }
        }
    }

    private void DetectDataAndShowSoulInLoadMenu()
    {
        string FilePath = Application.dataPath + "/StreamingAssets" + "/Save";
        string NameAndPath = "";
        string DataName = "";

        for (int i = 0; i < 3; i++)
        {
            switch (i)
            {
                case 0:
                    DataName = "Data1";
                    break;
                case 1:
                    DataName = "Data2";
                    break;
                case 2:
                    DataName = "Data3";
                    break;
            }
            NameAndPath = FilePath + "/" + DataName;

            if (File.Exists(NameAndPath))
            {
                switch (i)
                {
                    case 0:
                        PlayerDataType p1 = (PlayerDataType)SL.LoadData(typeof(PlayerDataType), DataName);
                        GameEvent.KilledBossNumber = p1.KilledBossNumber;
                        Data1InLoad.text = "Data 1";
                        if (GameEvent.KilledBossNumber >= 1)
                        {
                            LData1KilledBossSoul1.SetActive(true);
                        }
                        if (GameEvent.KilledBossNumber >= 2)
                        {
                            LData1KilledBossSoul2.SetActive(true);
                        }
                        if (GameEvent.KilledBossNumber >= 3)
                        {
                            LData1KilledBossSoul3.SetActive(true);
                        }
                        break;
                    case 1:
                        PlayerDataType p2 = (PlayerDataType)SL.LoadData(typeof(PlayerDataType), DataName);
                        GameEvent.KilledBossNumber = p2.KilledBossNumber;
                        Data2InLoad.text = "Data 2";
                        if (GameEvent.KilledBossNumber >= 1)
                        {
                            LData2KilledBossSoul1.SetActive(true);
                        }
                        if (GameEvent.KilledBossNumber >= 2)
                        {
                            LData2KilledBossSoul2.SetActive(true);
                        }
                        if (GameEvent.KilledBossNumber >= 3)
                        {
                            LData2KilledBossSoul3.SetActive(true);
                        }
                        break;
                    case 2:
                        PlayerDataType p3 = (PlayerDataType)SL.LoadData(typeof(PlayerDataType), DataName);
                        GameEvent.KilledBossNumber = p3.KilledBossNumber;
                        Data3InLoad.text = "Data 3";
                        if (GameEvent.KilledBossNumber >= 1)
                        {
                            LData3KilledBossSoul1.SetActive(true);
                        }
                        if (GameEvent.KilledBossNumber >= 2)
                        {
                            LData3KilledBossSoul2.SetActive(true);
                        }
                        if (GameEvent.KilledBossNumber >= 3)
                        {
                            LData3KilledBossSoul3.SetActive(true);
                        }
                        break;
                }
            }
            else
            {
                switch (i)
                {
                    case 0:
                        Data1InLoad.text = "No Data";
                        LData1KilledBossSoul1.SetActive(false);
                        LData1KilledBossSoul2.SetActive(false);
                        LData1KilledBossSoul3.SetActive(false);
                        break;
                    case 1:
                        Data2InLoad.text = "No Data";
                        LData2KilledBossSoul1.SetActive(false);
                        LData2KilledBossSoul2.SetActive(false);
                        LData2KilledBossSoul3.SetActive(false);
                        break;
                    case 2:
                        Data3InLoad.text = "No Data";
                        LData3KilledBossSoul1.SetActive(false);
                        LData3KilledBossSoul2.SetActive(false);
                        LData3KilledBossSoul3.SetActive(false);
                        break;
                }
            }
        }
    }

    private void SelectableInitialize()
    {
        //newGame&&LoadGame內的按鈕需自行掛上
        NewGameButton.onClick.AddListener(OpenCreateDataChooseMenu);
        LoadButton.onClick.AddListener(OpenLoadMenu);
        OptionButton.onClick.AddListener(OpenOption);
        ExitButton.onClick.AddListener(ExitGame);
        CoverDataNotice1.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(CreateGameStart);
        CoverDataNotice2.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(CreateGameStart);
        CoverDataNotice3.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(CreateGameStart);
        CoverDataNotice1.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(CancelCoverDate);
        CoverDataNotice2.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(CancelCoverDate);
        CoverDataNotice3.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(CancelCoverDate);
        OpenKeyCodeButton.onClick.AddListener(OpenKeyCodeManage);
        OpenSoundControllerButton.onClick.AddListener(OpenSoundController);
        BGMSlider.onValueChanged.AddListener(ChangeBGM);
        SESlider.onValueChanged.AddListener(ChangeSE);
    }
}
