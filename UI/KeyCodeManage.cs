using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class KeyCodeManage : MonoBehaviour
{
    //變數後 + C 的都是手把專用
    private GameObject Player;
    private PlayerController _playerController;
    private BattleSystem _battleSystem;
    private ItemWindow _itemWindow;

    public static KeyCode UseItem = KeyCode.A;
    public static KeyCode Restore = KeyCode.Q;
    public static KeyCode NormalAtk = KeyCode.X;
    public static KeyCode StrongAtk = KeyCode.C;
    public static KeyCode GoRight = KeyCode.RightArrow;
    public static KeyCode GoLeft = KeyCode.LeftArrow;
    public static KeyCode Interact = KeyCode.UpArrow;
    public static KeyCode Jump = KeyCode.Z;
    public static KeyCode Dash = KeyCode.Space;
    public static KeyCode Shoot = KeyCode.S;
    public static KeyCode Block = KeyCode.LeftControl;
    public static KeyCode OpenItemWindow = KeyCode.E;
    public static KeyCode ChangeUseItem = KeyCode.D;
    private KeyCode TemporaryKeyCode;
    private string TempopraryText;
    [HideInInspector] public bool isSelect;//其他Script會用到(PauseMenuController)
    [HideInInspector] public bool isSelectC;//其他Script會用到(PauseMenuController)
    [HideInInspector] public bool isInputOpen;//其他Script會用到(PauseMenuController)
    [HideInInspector] public bool isInputOpenC;//其他Script會用到(PauseMenuController)
    private string SelectedObject;
    private bool isSelectablePressed;
    private bool ShouldTextChange;
    [HideInInspector] public bool isReChooseNoticeAppear;//其他Script會用到(ReChooseNotice)
    public GameObject UI;
    private GameObject ReChooseNotice;
    private GameObject InputNotice;
    private Text CurrentGoRightText;
    private Text CurrentGoLeftText;
    private Text CurrentJumpText;
    private Text CurrentNormalAtkText;
    private Text CurrentStrongAtkText;
    private Text CurrentDashText;
    private Text CurrentUseItemText;
    private Text CurrentRestoreText;
    private Text CurrentInteractText;
    private Text CurrentShootText;
    private Text CurrentBlockText;
    private Text CurrentOpenItemWindowText;
    private Text CurrentChangeUseItemText;
    private Button GoRightButton;
    private Button GoLeftButton;
    private Button JumpButton;
    private Button NormalAtkButton;
    private Button StrongAtkButton;
    private Button DashButton;
    private Button UseItemButton;
    private Button RestoreButton;
    private Button InteractButton;
    private Button ShootButton;
    private Button BlockButton;
    private Button OpenItemWindowButton;
    private Button ChangeUseItemButton;
    private Button ResetButton;
    public static string GoRightText = "RightArrow";
    public static string GoLeftText = "LeftArrow";
    public static string JumpText = "Z";
    public static string NormalAtkText = "X";
    public static string StrongAtkText = "C";
    public static string DashText = "Space";
    public static string UseItemText = "A";
    public static string RestoreText = "Q";
    public static string InteractText = "UpArrow";
    public static string ShootText = "S";
    public static string BlockText = "LeftControl";
    public static string OpenItemWindowText = "E";
    public static string ChangeUseItemText = "D";
    private bool isEnterPressed;//避免一點開就跑出無效視窗(需改寫)
    //防止重複
    private string GoRightRecord = "RightArrow";
    private string GoLeftRecord = "LeftArrow";
    private string JumpRecord = "Z";
    private string NormalAtkRecord = "X";
    private string StrongAtkRecord = "C";
    private string DashRecord = "Space";
    private string UseItemRecord = "A";
    private string RestoreRecord = "Q";
    private string InteractRecord = "UpArrow";
    private string ShootRecord = "S";
    private string BlockRecord = "LeftControl";
    private string OpenItemWindowRecord = "E";
    private string ChangeUseItemRecord = "D";
    private bool ShouldDetectRepect;
    private bool isRepect;
    [HideInInspector] public bool isAlert;//其他Script會用到(PauseMenuController，LackKeyCodeNotice)
    private int AlertNumber;

    //手把
    [HideInInspector] public bool JumpPressed;
    [HideInInspector] public bool JumpUp;
    [HideInInspector] public bool NormalAtkPressed;
    [HideInInspector] public bool NormalAtkUp;
    [HideInInspector] public bool StrongAtkPressed;
    [HideInInspector] public bool DashPressed;
    [HideInInspector] public bool UseItemPressed;
    [HideInInspector] public bool UseItemUp;
    [HideInInspector] public bool RestorePressed;
    [HideInInspector] public bool InteractPressed;
    [HideInInspector] public bool InteractUp;
    [HideInInspector] public bool ShootPressed;
    [HideInInspector] public bool ShootUp;
    [HideInInspector] public bool BlockPressed;
    [HideInInspector] public bool ChangeUseItemPressed;

    //QTE
    [HideInInspector] public bool QTENormalAtkPressed;
    [HideInInspector] public bool QTENormalAtkUp;

    public static int JumpNumber = 1;
    public static int NormalAtkNumber = 6;
    public static int StrongAtkNumber = 8;
    public static int DashNumber = 3;
    public static int UseItemNumber = 7;
    public static int RestoreNumber = 9;
    public static int InteractNumber = 2;
    public static int ShootNumber = 4;
    public static int BlockNumber = 5;
    public static int ChangeUseItemNumber = 10;

    private int TemporaryNumberC;
    private string TempopraryTextC;
    private bool ShouldTextChangeC;
    private bool ShouldDetectRepectC;

    private Text CurrentJumpTextC;
    private Text CurrentNormalAtkTextC;
    private Text CurrentStrongAtkTextC;
    private Text CurrentDashTextC;
    private Text CurrentUseItemTextC;
    private Text CurrentRestoreTextC;
    private Text CurrentInteractTextC;
    private Text CurrentShootTextC;
    private Text CurrentBlockTextC;
    private Text CurrentChangeUseItemTextC;
    private Button JumpButtonC;
    private Button NormalAtkButtonC;
    private Button StrongAtkButtonC;
    private Button DashButtonC;
    private Button UseItemButtonC;
    private Button RestoreButtonC;
    private Button InteractButtonC;
    private Button ShootButtonC;
    private Button BlockButtonC;
    private Button ChangeUseItemButtonC;
    public static string JumpTextC = "A";
    public static string NormalAtkTextC = "RB";
    public static string StrongAtkTextC = "RT";
    public static string DashTextC = "X";
    public static string UseItemTextC = "LT";
    public static string RestoreTextC = "UpArrow";
    public static string InteractTextC = "B";
    public static string ShootTextC = "Y";
    public static string BlockTextC = "LB";
    public static string ChangeUseItemTextC = "RightArrow";

    //防止重複
    private string JumpRecordC = "A";
    private string NormalAtkRecordC = "RB";
    private string StrongAtkRecordC = "RT";
    private string DashRecordC = "X";
    private string UseItemRecordC = "LT";
    private string RestoreRecordC = "UpArrow";
    private string InteractRecordC = "B";
    private string ShootRecordC = "Y";
    private string BlockRecordC = "LB";
    private string ChangeUseItemRecordC = "RightArrow";

    /*private bool isLTPressed = false;//手把專用
    private bool LTPressBarrier;//手把專用 需放開才能按的限制
    private bool isLTUp = false;//手把專用
    private bool isRTPressed = false;//手把專用
    private bool RTPressBarrier;//手把專用 需放開才能按的限制
    private bool isRTUp = false;//手把專用
    private bool isCrossUpPressed = false;//手把專用
    private bool CrossUpPressBarrier;//手把專用 需放開才能按的限制
    private bool isCrossUpUp = false;//手把專用
    private bool isCrossDownPressed = false;//手把專用
    private bool CrossDownPressBarrier;//手把專用 需放開才能按的限制
    private bool isCrossDownUp = false;//手把專用
    private bool isCrossRightPressed = false;//手把專用
    private bool CrossRightPressBarrier;//手把專用 需放開才能按的限制
    private bool isCrossRightUp = false;//手把專用
    private bool isCrossLeftPressed = false;//手把專用
    private bool CrossLeftPressBarrier;//手把專用 需放開才能按的限制
    private bool isCrossLeftUp = false;//手把專用*/

    private bool BeginCoolDown;
    private float ButtonCoolDownTime = 0.03f;
    private float ButtonCoolDownLeft = 0.03f;
    private bool FirstTurnSkip;

    private void Start()
    {
        if (GameObject.Find("player") != null)
        {
            Player = GameObject.Find("player");
            _playerController = Player.GetComponent<PlayerController>();
            _battleSystem = Player.GetComponent<BattleSystem>();
        }
        if(UI == null)
        {
            UI = GameObject.FindGameObjectWithTag("UI");
        }
        if(UI != null)
        {
            if (SceneManager.GetActiveScene().name == "title")
            {               
                ReChooseNotice = UI.transform.GetChild(15).gameObject;
                InputNotice = UI.transform.GetChild(14).gameObject;
                CurrentGoRightText = UI.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentGoLeftText = UI.transform.GetChild(2).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentJumpText = UI.transform.GetChild(3).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentNormalAtkText = UI.transform.GetChild(4).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentStrongAtkText = UI.transform.GetChild(5).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentDashText = UI.transform.GetChild(6).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentUseItemText = UI.transform.GetChild(7).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentRestoreText = UI.transform.GetChild(8).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentInteractText = UI.transform.GetChild(9).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentShootText = UI.transform.GetChild(10).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentBlockText = UI.transform.GetChild(11).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentChangeUseItemText = UI.transform.GetChild(12).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentOpenItemWindowText = UI.transform.GetChild(13).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Text>();
                GoRightButton = UI.transform.GetChild(1).gameObject.transform.GetChild(1).GetComponent<Button>();
                GoLeftButton = UI.transform.GetChild(2).gameObject.transform.GetChild(1).GetComponent<Button>();
                JumpButton = UI.transform.GetChild(3).gameObject.transform.GetChild(1).GetComponent<Button>();
                NormalAtkButton = UI.transform.GetChild(4).gameObject.transform.GetChild(1).GetComponent<Button>();
                StrongAtkButton = UI.transform.GetChild(5).gameObject.transform.GetChild(1).GetComponent<Button>();
                DashButton = UI.transform.GetChild(6).gameObject.transform.GetChild(1).GetComponent<Button>();
                UseItemButton = UI.transform.GetChild(7).gameObject.transform.GetChild(1).GetComponent<Button>();
                RestoreButton = UI.transform.GetChild(8).gameObject.transform.GetChild(1).GetComponent<Button>();
                InteractButton = UI.transform.GetChild(9).gameObject.transform.GetChild(1).GetComponent<Button>();
                ShootButton = UI.transform.GetChild(10).gameObject.transform.GetChild(1).GetComponent<Button>();
                BlockButton = UI.transform.GetChild(11).gameObject.transform.GetChild(1).GetComponent<Button>();
                ChangeUseItemButton = UI.transform.GetChild(12).gameObject.transform.GetChild(1).GetComponent<Button>();
                OpenItemWindowButton = UI.transform.GetChild(13).gameObject.transform.GetChild(1).GetComponent<Button>();
                ResetButton = UI.transform.GetChild(17).GetComponent<Button>();

                CurrentJumpTextC = UI.transform.GetChild(3).gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentNormalAtkTextC = UI.transform.GetChild(4).gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentStrongAtkTextC = UI.transform.GetChild(5).gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentDashTextC = UI.transform.GetChild(6).gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentUseItemTextC = UI.transform.GetChild(7).gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentRestoreTextC = UI.transform.GetChild(8).gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentInteractTextC = UI.transform.GetChild(9).gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentShootTextC = UI.transform.GetChild(10).gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentBlockTextC = UI.transform.GetChild(11).gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentChangeUseItemTextC = UI.transform.GetChild(12).gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).GetComponent<Text>();
                JumpButtonC = UI.transform.GetChild(3).gameObject.transform.GetChild(2).GetComponent<Button>();
                NormalAtkButtonC = UI.transform.GetChild(4).gameObject.transform.GetChild(2).GetComponent<Button>();
                StrongAtkButtonC = UI.transform.GetChild(5).gameObject.transform.GetChild(2).GetComponent<Button>();
                DashButtonC = UI.transform.GetChild(6).gameObject.transform.GetChild(2).GetComponent<Button>();
                UseItemButtonC = UI.transform.GetChild(7).gameObject.transform.GetChild(2).GetComponent<Button>();
                RestoreButtonC = UI.transform.GetChild(8).gameObject.transform.GetChild(2).GetComponent<Button>();
                InteractButtonC = UI.transform.GetChild(9).gameObject.transform.GetChild(2).GetComponent<Button>();
                ShootButtonC = UI.transform.GetChild(10).gameObject.transform.GetChild(2).GetComponent<Button>();
                BlockButtonC = UI.transform.GetChild(11).gameObject.transform.GetChild(2).GetComponent<Button>();
                ChangeUseItemButtonC = UI.transform.GetChild(12).gameObject.transform.GetChild(2).GetComponent<Button>();
            }
            else
            {
                _itemWindow = UI.transform.GetChild(8).GetComponent<ItemWindow>();
                UI = UI.transform.GetChild(6).gameObject;
                ReChooseNotice = UI.transform.GetChild(15).gameObject;
                InputNotice = UI.transform.GetChild(14).gameObject;
                CurrentGoRightText = UI.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentGoLeftText = UI.transform.GetChild(2).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentJumpText = UI.transform.GetChild(3).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentNormalAtkText = UI.transform.GetChild(4).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentStrongAtkText = UI.transform.GetChild(5).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentDashText = UI.transform.GetChild(6).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentUseItemText = UI.transform.GetChild(7).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentRestoreText = UI.transform.GetChild(8).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentInteractText = UI.transform.GetChild(9).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentShootText = UI.transform.GetChild(10).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentBlockText = UI.transform.GetChild(11).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentChangeUseItemText = UI.transform.GetChild(12).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentOpenItemWindowText = UI.transform.GetChild(13).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Text>();
                GoRightButton = UI.transform.GetChild(1).gameObject.transform.GetChild(1).GetComponent<Button>();
                GoLeftButton = UI.transform.GetChild(2).gameObject.transform.GetChild(1).GetComponent<Button>();
                JumpButton = UI.transform.GetChild(3).gameObject.transform.GetChild(1).GetComponent<Button>();
                NormalAtkButton = UI.transform.GetChild(4).gameObject.transform.GetChild(1).GetComponent<Button>();
                StrongAtkButton = UI.transform.GetChild(5).gameObject.transform.GetChild(1).GetComponent<Button>();
                DashButton = UI.transform.GetChild(6).gameObject.transform.GetChild(1).GetComponent<Button>();
                UseItemButton = UI.transform.GetChild(7).gameObject.transform.GetChild(1).GetComponent<Button>();
                RestoreButton = UI.transform.GetChild(8).gameObject.transform.GetChild(1).GetComponent<Button>();
                InteractButton = UI.transform.GetChild(9).gameObject.transform.GetChild(1).GetComponent<Button>();
                ShootButton = UI.transform.GetChild(10).gameObject.transform.GetChild(1).GetComponent<Button>();
                BlockButton = UI.transform.GetChild(11).gameObject.transform.GetChild(1).GetComponent<Button>();
                ChangeUseItemButton = UI.transform.GetChild(12).gameObject.transform.GetChild(1).GetComponent<Button>();
                OpenItemWindowButton = UI.transform.GetChild(13).gameObject.transform.GetChild(1).GetComponent<Button>();
                ResetButton = UI.transform.GetChild(17).GetComponent<Button>();

                CurrentJumpTextC = UI.transform.GetChild(3).gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentNormalAtkTextC = UI.transform.GetChild(4).gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentStrongAtkTextC = UI.transform.GetChild(5).gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentDashTextC = UI.transform.GetChild(6).gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentUseItemTextC = UI.transform.GetChild(7).gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentRestoreTextC = UI.transform.GetChild(8).gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentInteractTextC = UI.transform.GetChild(9).gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentShootTextC = UI.transform.GetChild(10).gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentBlockTextC = UI.transform.GetChild(11).gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentChangeUseItemTextC = UI.transform.GetChild(12).gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).GetComponent<Text>();
                JumpButtonC = UI.transform.GetChild(3).gameObject.transform.GetChild(2).GetComponent<Button>();
                NormalAtkButtonC = UI.transform.GetChild(4).gameObject.transform.GetChild(2).GetComponent<Button>();
                StrongAtkButtonC = UI.transform.GetChild(5).gameObject.transform.GetChild(2).GetComponent<Button>();
                DashButtonC = UI.transform.GetChild(6).gameObject.transform.GetChild(2).GetComponent<Button>();
                UseItemButtonC = UI.transform.GetChild(7).gameObject.transform.GetChild(2).GetComponent<Button>();
                RestoreButtonC = UI.transform.GetChild(8).gameObject.transform.GetChild(2).GetComponent<Button>();
                InteractButtonC = UI.transform.GetChild(9).gameObject.transform.GetChild(2).GetComponent<Button>();
                ShootButtonC = UI.transform.GetChild(10).gameObject.transform.GetChild(2).GetComponent<Button>();
                BlockButtonC = UI.transform.GetChild(11).gameObject.transform.GetChild(2).GetComponent<Button>();
                ChangeUseItemButtonC = UI.transform.GetChild(12).gameObject.transform.GetChild(2).GetComponent<Button>();
            }
        }
        CurrentGoRightText.text = GoRightText;
        CurrentGoLeftText.text = GoLeftText;
        CurrentJumpText.text = JumpText;
        CurrentNormalAtkText.text = NormalAtkText;
        CurrentStrongAtkText.text = StrongAtkText;
        CurrentDashText.text = DashText;
        CurrentUseItemText.text = UseItemText;
        CurrentRestoreText.text = RestoreText;
        CurrentInteractText.text = InteractText;
        CurrentShootText.text = ShootText;
        CurrentBlockText.text = BlockText;
        CurrentChangeUseItemText.text = ChangeUseItemText;
        CurrentOpenItemWindowText.text = OpenItemWindowText;
        GoRightButton.onClick.AddListener(ChangeGoRight);
        GoLeftButton.onClick.AddListener(ChangeGoLeft);
        JumpButton.onClick.AddListener(ChangeJump);
        NormalAtkButton.onClick.AddListener(ChangeNormalAtk);
        StrongAtkButton.onClick.AddListener(ChangeStrongAtk);
        DashButton.onClick.AddListener(ChangeDash);
        UseItemButton.onClick.AddListener(ChangeThrowCocktail);
        RestoreButton.onClick.AddListener(ChangeRestore);
        InteractButton.onClick.AddListener(ChangeInteract);
        ShootButton.onClick.AddListener(ChangeShoot);
        BlockButton.onClick.AddListener(ChangeBlock);
        ChangeUseItemButton.onClick.AddListener(ChangeChangeThrowType);
        OpenItemWindowButton.onClick.AddListener(ChangeOpenItem);
        ResetButton.onClick.AddListener(ReSetKeyCode);

        CurrentJumpTextC.text = JumpTextC;
        CurrentNormalAtkTextC.text = NormalAtkTextC;
        CurrentStrongAtkTextC.text = StrongAtkTextC;
        CurrentDashTextC.text = DashTextC;
        CurrentUseItemTextC.text = UseItemTextC;
        CurrentRestoreTextC.text = RestoreTextC;
        CurrentInteractTextC.text = InteractTextC;
        CurrentShootTextC.text = ShootTextC;
        CurrentBlockTextC.text = BlockTextC;
        CurrentChangeUseItemTextC.text = ChangeUseItemTextC;
        JumpButtonC.onClick.AddListener(ChangeJumpC);
        NormalAtkButtonC.onClick.AddListener(ChangeNormalAtkC);
        StrongAtkButtonC.onClick.AddListener(ChangeStrongAtkC);
        DashButtonC.onClick.AddListener(ChangeDashC);
        UseItemButtonC.onClick.AddListener(ChangeThrowCocktailC);
        RestoreButtonC.onClick.AddListener(ChangeRestoreC);
        InteractButtonC.onClick.AddListener(ChangeInteractC);
        ShootButtonC.onClick.AddListener(ChangeShootC);
        BlockButtonC.onClick.AddListener(ChangeBlockC);
        ChangeUseItemButtonC.onClick.AddListener(ChangeChangeThrowTypeC);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            isEnterPressed = true;
        }
        if (Input.GetKeyUp(KeyCode.Return))
        {
            isEnterPressed = false;
        }

        if (BeginCoolDown)
        {
            ButtonCoolDownLeft -= Time.unscaledDeltaTime;
            if (ButtonCoolDownLeft <= 0)
            {
                AllKeyButtonTrue();
                UI.GetComponent<DefaultButton>().ShouldOpen = true;
                SelectButtonController.OpenSelectButtonController();
                ButtonCoolDownLeft = ButtonCoolDownTime;
                BeginCoolDown = false;
            }
        }

    Begin:
        if (isSelect)
        {
            AllKeyButtonFalse();
            if (Input.anyKeyDown)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    isSelect = false;
                    InputNotice.SetActive(false);
                    ShouldTextChange = false;
                    TempopraryText = null;
                    goto Begin;
                }
                InputSelect();
                if (!isSelectablePressed && !isEnterPressed)
                {
                    if (!isReChooseNoticeAppear)
                    {
                        ReChooseNotice.SetActive(true);
                        isReChooseNoticeAppear = true;
                    }
                }
                switch (SelectedObject)
                {
                    case "GoRight":
                        if (isSelectablePressed)
                        {
                            GoRight = TemporaryKeyCode;
                            InputNotice.SetActive(false);
                            isSelect = false;
                        }
                        break;
                    case "GoLeft":
                        if (isSelectablePressed)
                        {
                            GoLeft = TemporaryKeyCode;
                            InputNotice.SetActive(false);
                            isSelect = false;
                        }
                        break;
                    case "Jump":
                        if (isSelectablePressed)
                        {
                            Jump = TemporaryKeyCode;
                            InputNotice.SetActive(false);
                            isSelect = false;
                        }
                        break;
                    case "NormalAtk":
                        if (isSelectablePressed)
                        {
                            NormalAtk = TemporaryKeyCode;
                            InputNotice.SetActive(false);
                            isSelect = false;
                        }
                        break;
                    case "StrongAtk":
                        if (isSelectablePressed)
                        {
                            StrongAtk = TemporaryKeyCode;
                            InputNotice.SetActive(false);
                            isSelect = false;
                        }
                        break;
                    case "Dash":
                        if (isSelectablePressed)
                        {
                            Dash = TemporaryKeyCode;
                            InputNotice.SetActive(false);
                            isSelect = false;
                        }
                        break;
                    case "ThrowCocktail":
                        if (isSelectablePressed)
                        {
                            UseItem = TemporaryKeyCode;
                            InputNotice.SetActive(false);
                            isSelect = false;
                        }
                        break;
                    case "Restore":
                        if (isSelectablePressed)
                        {
                            Restore = TemporaryKeyCode;
                            InputNotice.SetActive(false);
                            isSelect = false;
                        }
                        break;
                    case "Interact":
                        if (isSelectablePressed)
                        {
                            Interact = TemporaryKeyCode;
                            InputNotice.SetActive(false);
                            isSelect = false;
                        }
                        break;
                    case "Shoot":
                        if (isSelectablePressed)
                        {
                            Shoot = TemporaryKeyCode;
                            InputNotice.SetActive(false);
                            isSelect = false;
                        }
                        break;
                    case "Block":
                        if (isSelectablePressed)
                        {
                            Block = TemporaryKeyCode;
                            InputNotice.SetActive(false);
                            isSelect = false;
                        }
                        break;
                    case "ChangeThrowType":
                        if (isSelectablePressed)
                        {
                            ChangeUseItem = TemporaryKeyCode;
                            InputNotice.SetActive(false);
                            isSelect = false;
                        }
                        break;
                    case "OpenItem":
                        if (isSelectablePressed)
                        {
                            OpenItemWindow = TemporaryKeyCode;
                            InputNotice.SetActive(false);
                            isSelect = false;
                        }
                        break;
                }
            }
        }
        else
        {
            if (ShouldTextChange)
            {
                switch (SelectedObject)
                {
                    case "GoRight":
                        CurrentGoRightText.text = TempopraryText;
                        GoRightText = TempopraryText;
                        break;
                    case "GoLeft":
                        CurrentGoLeftText.text = TempopraryText;
                        GoLeftText = TempopraryText;
                        break;
                    case "Jump":
                        CurrentJumpText.text = TempopraryText;
                        JumpText = TempopraryText;
                        break;
                    case "NormalAtk":
                        CurrentNormalAtkText.text = TempopraryText;
                        NormalAtkText = TempopraryText;
                        break;
                    case "StrongAtk":
                        CurrentStrongAtkText.text = TempopraryText;
                        StrongAtkText = TempopraryText;
                        break;
                    case "Dash":
                        CurrentDashText.text = TempopraryText;
                        DashText = TempopraryText;
                        break;
                    case "ThrowCocktail":
                        CurrentUseItemText.text = TempopraryText;
                        UseItemText = TempopraryText;
                        break;
                    case "Restore":
                        CurrentRestoreText.text = TempopraryText;
                        RestoreText = TempopraryText;
                        break;
                    case "Interact":
                        CurrentInteractText.text = TempopraryText;
                        InteractText = TempopraryText;
                        break;
                    case "Shoot":
                        CurrentShootText.text = TempopraryText;
                        ShootText = TempopraryText;
                        break;
                    case "Block":
                        CurrentBlockText.text = TempopraryText;
                        BlockText = TempopraryText;
                        break;
                    case "ChangeThrowType":
                        CurrentChangeUseItemText.text = TempopraryText;
                        ChangeUseItemText = TempopraryText;
                        break;
                    case "OpenItem":
                        CurrentOpenItemWindowText.text = TempopraryText;
                        OpenItemWindowText = TempopraryText;
                        break;
                }
                ShouldTextChange = false;
            }
            if (ShouldDetectRepect)
            {
                if (TempopraryText == GoRightRecord && SelectedObject != "GoRight")
                {
                    isRepect = true;
                    AlertNumber = 1;
                }
                if (TempopraryText == GoLeftRecord && SelectedObject != "GoLeft")
                {
                    isRepect = true;
                    AlertNumber = 2;
                }
                if (TempopraryText == JumpRecord && SelectedObject != "Jump")
                {
                    isRepect = true;
                    AlertNumber = 3;
                }
                if (TempopraryText == NormalAtkRecord && SelectedObject != "NormalAtk")
                {
                    isRepect = true;
                    AlertNumber = 4;
                }
                if (TempopraryText == StrongAtkRecord && SelectedObject != "StrongAtk")
                {
                    isRepect = true;
                    AlertNumber = 5;
                }
                if (TempopraryText == DashRecord && SelectedObject != "Dash")
                {
                    isRepect = true;
                    AlertNumber = 6;
                }
                if (TempopraryText == UseItemRecord && SelectedObject != "ThrowCocktail")
                {
                    isRepect = true;
                    AlertNumber = 7;
                }
                if (TempopraryText == RestoreRecord && SelectedObject != "Restore")
                {
                    isRepect = true;
                    AlertNumber = 8;
                }
                if (TempopraryText == InteractRecord && SelectedObject != "Interact")
                {
                    isRepect = true;
                    AlertNumber = 9;
                }
                if (TempopraryText == ShootRecord && SelectedObject != "Shoot")
                {
                    isRepect = true;
                    AlertNumber = 10;
                }
                if (TempopraryText == BlockRecord && SelectedObject != "Block")
                {
                    isRepect = true;
                    AlertNumber = 11;
                }
                if (TempopraryText == ChangeUseItemRecord && SelectedObject != "ChangeThrowType")
                {
                    isRepect = true;
                    AlertNumber = 12;
                }
                if (TempopraryText == OpenItemWindowRecord && SelectedObject != "OpenItem")
                {
                    isRepect = true;
                    AlertNumber = 13;
                }

                if (isRepect)
                {
                    isAlert = true;
                    switch (AlertNumber)
                    {
                        case 1:
                            CurrentGoRightText.text = null;
                            GoRight = KeyCode.None;
                            break;
                        case 2:
                            CurrentGoLeftText.text = null;
                            GoLeft = KeyCode.None;
                            break;
                        case 3:
                            CurrentJumpText.text = null;
                            Jump = KeyCode.None;
                            break;
                        case 4:
                            CurrentNormalAtkText.text = null;
                            NormalAtk = KeyCode.None;
                            break;
                        case 5:
                            CurrentStrongAtkText.text = null;
                            StrongAtk = KeyCode.None;
                            break;
                        case 6:
                            CurrentDashText.text = null;
                            Dash = KeyCode.None;
                            break;
                        case 7:
                            CurrentUseItemText.text = null;
                            UseItem = KeyCode.None;
                            break;
                        case 8:
                            CurrentRestoreText.text = null;
                            Restore = KeyCode.None;
                            break;
                        case 9:
                            CurrentInteractText.text = null;
                            Interact = KeyCode.None;
                            break;
                        case 10:
                            CurrentShootText.text = null;
                            Shoot = KeyCode.None;
                            break;
                        case 11:
                            CurrentBlockText.text = null;
                            Block = KeyCode.None;
                            break;
                        case 12:
                            CurrentChangeUseItemText.text = null;
                            ChangeUseItem = KeyCode.None;
                            break;
                        case 13:
                            CurrentOpenItemWindowText.text = null;
                            OpenItemWindow = KeyCode.None;
                            break;
                        case 0:
                            break;
                    }
                    isRepect = false;
                    AlertNumber = 0;
                }

                //做紀錄
                GoRightRecord = CurrentGoRightText.text;
                GoLeftRecord = CurrentGoLeftText.text;
                JumpRecord = CurrentJumpText.text;
                NormalAtkRecord = CurrentNormalAtkText.text;
                StrongAtkRecord = CurrentStrongAtkText.text;
                DashRecord = CurrentDashText.text;
                UseItemRecord = CurrentUseItemText.text;
                RestoreRecord = CurrentRestoreText.text;
                InteractRecord = CurrentInteractText.text;
                ShootRecord = CurrentShootText.text;
                BlockRecord = CurrentBlockText.text;
                ChangeUseItemRecord = CurrentChangeUseItemText.text;
                OpenItemWindowRecord = CurrentOpenItemWindowText.text;

                //偵測是否有空缺
                DetectLack();

                ShouldDetectRepect = false;
                BeginCoolDown = true;
            }//enter和mouse的重置也寫在這
        }
        if (isSelectC)
        {
            if (!FirstTurnSkip)
            {
                FirstTurnSkip = true;
                goto Skip;
            }
            if (Input.anyKeyDown || OldVerXboxControllerDetect.isLTPressed || OldVerXboxControllerDetect.isRTPressed || OldVerXboxControllerDetect.isCrossDownPressed || OldVerXboxControllerDetect.isCrossLeftPressed || OldVerXboxControllerDetect.isCrossRightPressed || OldVerXboxControllerDetect.isCrossUpPressed)
            {
                InputSelectC();
                if (!isSelectablePressed)
                {
                    if (!isReChooseNoticeAppear)
                    {
                        ReChooseNotice.SetActive(true);
                        isReChooseNoticeAppear = true;
                    }
                }
                switch (SelectedObject)
                {
                    case "JumpC":
                        if (isSelectablePressed)
                        {
                            JumpNumber = TemporaryNumberC;
                            InputNotice.SetActive(false);
                            isSelectC = false;
                        }
                        break;
                    case "NormalAtkC":
                        if (isSelectablePressed)
                        {
                            NormalAtkNumber = TemporaryNumberC;
                            InputNotice.SetActive(false);
                            isSelectC = false;
                        }
                        break;
                    case "StrongAtkC":
                        if (isSelectablePressed)
                        {
                            StrongAtkNumber = TemporaryNumberC;
                            InputNotice.SetActive(false);
                            isSelectC = false;
                        }
                        break;
                    case "DashC":
                        if (isSelectablePressed)
                        {
                            DashNumber = TemporaryNumberC;
                            InputNotice.SetActive(false);
                            isSelectC = false;
                        }
                        break;
                    case "ThrowCocktailC":
                        if (isSelectablePressed)
                        {
                            UseItemNumber = TemporaryNumberC;
                            InputNotice.SetActive(false);
                            isSelectC = false;
                        }
                        break;
                    case "RestoreC":
                        if (isSelectablePressed)
                        {
                            RestoreNumber = TemporaryNumberC;
                            InputNotice.SetActive(false);
                            isSelectC = false;
                        }
                        break;
                    case "InteractC":
                        if (isSelectablePressed)
                        {
                            InteractNumber = TemporaryNumberC;
                            InputNotice.SetActive(false);
                            isSelectC = false;
                        }
                        break;
                    case "ShootC":
                        if (isSelectablePressed)
                        {
                            ShootNumber = TemporaryNumberC;
                            InputNotice.SetActive(false);
                            isSelectC = false;
                        }
                        break;
                    case "BlockC":
                        if (isSelectablePressed)
                        {
                            BlockNumber = TemporaryNumberC;
                            InputNotice.SetActive(false);
                            isSelectC = false;
                        }
                        break;
                    case "ChangeThrowTypeC":
                        if (isSelectablePressed)
                        {
                            ChangeUseItemNumber = TemporaryNumberC;
                            InputNotice.SetActive(false);
                            isSelectC = false;
                        }
                        break;
                }
            }
        Skip:;
        }
        else
        {
            if (ShouldTextChangeC)
            {
                switch (SelectedObject)
                {
                    case "JumpC":
                        CurrentJumpTextC.text = TempopraryTextC;
                        JumpTextC = TempopraryTextC;
                        break;
                    case "NormalAtkC":
                        CurrentNormalAtkTextC.text = TempopraryTextC;
                        NormalAtkTextC = TempopraryTextC;
                        break;
                    case "StrongAtkC":
                        CurrentStrongAtkTextC.text = TempopraryTextC;
                        StrongAtkTextC = TempopraryTextC;
                        break;
                    case "DashC":
                        CurrentDashTextC.text = TempopraryTextC;
                        DashTextC = TempopraryTextC;
                        break;
                    case "ThrowCocktailC":
                        CurrentUseItemTextC.text = TempopraryTextC;
                        UseItemTextC = TempopraryTextC;
                        break;
                    case "RestoreC":
                        CurrentRestoreTextC.text = TempopraryTextC;
                        RestoreTextC = TempopraryTextC;
                        break;
                    case "InteractC":
                        CurrentInteractTextC.text = TempopraryTextC;
                        InteractTextC = TempopraryTextC;
                        break;
                    case "ShootC":
                        CurrentShootTextC.text = TempopraryTextC;
                        ShootTextC = TempopraryTextC;
                        break;
                    case "BlockC":
                        CurrentBlockTextC.text = TempopraryTextC;
                        BlockTextC = TempopraryTextC;
                        break;
                    case "ChangeThrowTypeC":
                        CurrentChangeUseItemTextC.text = TempopraryTextC;
                        ChangeUseItemTextC = TempopraryTextC;
                        break;
                }
                ShouldTextChangeC = false;
            }
            if (ShouldDetectRepectC)
            {
                if (TempopraryTextC == JumpRecordC && SelectedObject != "JumpC")
                {
                    isRepect = true;
                    AlertNumber = 3;
                }
                if (TempopraryTextC == NormalAtkRecordC && SelectedObject != "NormalAtkC")
                {
                    isRepect = true;
                    AlertNumber = 4;
                }
                if (TempopraryTextC == StrongAtkRecordC && SelectedObject != "StrongAtkC")
                {
                    isRepect = true;
                    AlertNumber = 5;
                }
                if (TempopraryTextC == DashRecordC && SelectedObject != "DashC")
                {
                    isRepect = true;
                    AlertNumber = 6;
                }
                if (TempopraryTextC == UseItemRecordC && SelectedObject != "ThrowCocktailC")
                {
                    isRepect = true;
                    AlertNumber = 7;
                }
                if (TempopraryTextC == RestoreRecordC && SelectedObject != "RestoreC")
                {
                    isRepect = true;
                    AlertNumber = 8;
                }
                if (TempopraryTextC == InteractRecordC && SelectedObject != "InteractC")
                {
                    isRepect = true;
                    AlertNumber = 9;
                }
                if (TempopraryTextC == ShootRecordC && SelectedObject != "ShootC")
                {
                    isRepect = true;
                    AlertNumber = 10;
                }
                if (TempopraryTextC == BlockRecordC && SelectedObject != "BlockC")
                {
                    isRepect = true;
                    AlertNumber = 11;
                }
                if (TempopraryTextC == ChangeUseItemRecordC && SelectedObject != "ChangeThrowTypeC")
                {
                    isRepect = true;
                    AlertNumber = 12;
                }

                if (isRepect)
                {
                    isAlert = true;
                    switch (AlertNumber)
                    {
                        case 3:
                            CurrentJumpTextC.text = null;
                            JumpNumber = 0;
                            break;
                        case 4:
                            CurrentNormalAtkTextC.text = null;
                            NormalAtkNumber = 0;
                            break;
                        case 5:
                            CurrentStrongAtkTextC.text = null;
                            StrongAtkNumber = 0;
                            break;
                        case 6:
                            CurrentDashTextC.text = null;
                            DashNumber = 0;
                            break;
                        case 7:
                            CurrentUseItemTextC.text = null;
                            UseItemNumber = 0;
                            break;
                        case 8:
                            CurrentRestoreTextC.text = null;
                            RestoreNumber = 0;
                            break;
                        case 9:
                            CurrentInteractTextC.text = null;
                            InteractNumber = 0;
                            break;
                        case 10:
                            CurrentShootTextC.text = null;
                            ShootNumber = 0;
                            break;
                        case 11:
                            CurrentBlockTextC.text = null;
                            BlockNumber = 0;
                            break;
                        case 12:
                            CurrentChangeUseItemTextC.text = null;
                            ChangeUseItemNumber = 0;
                            break;
                        case 0:
                            break;
                    }
                    isRepect = false;
                    AlertNumber = 0;
                }

                //做紀錄
                JumpRecordC = CurrentJumpTextC.text;
                NormalAtkRecordC = CurrentNormalAtkTextC.text;
                StrongAtkRecordC = CurrentStrongAtkTextC.text;
                DashRecordC = CurrentDashTextC.text;
                UseItemRecordC = CurrentUseItemTextC.text;
                RestoreRecordC = CurrentRestoreTextC.text;
                InteractRecordC = CurrentInteractTextC.text;
                ShootRecordC = CurrentShootTextC.text;
                BlockRecordC = CurrentBlockTextC.text;
                ChangeUseItemRecordC = CurrentChangeUseItemTextC.text;

                //偵測是否有空缺
                DetectLackC();

                FirstTurnSkip = false;
                ShouldDetectRepectC = false;
                BeginCoolDown = true;
            }//enter和mouse的重置也寫在這
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            isEnterPressed = true;
        }
        if (Input.GetKeyUp(KeyCode.Return))
        {
            isEnterPressed = false;
        }

        if (!Player)
        {
            return;
        }

        ActionJudgement();
    }

    private void ChangeGoRight()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            isSelect = true;
            isInputOpen = true;
            ShouldTextChange = true;
            InputNotice.SetActive(true);
            isSelectablePressed = false;
            ShouldDetectRepect = true;
            SelectedObject = "GoRight";
            UI.GetComponent<DefaultButton>().onStart = GoRightButton;
        }
    }//Button專用
    private void ChangeGoLeft()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            isSelect = true;
            isInputOpen = true;
            ShouldTextChange = true;
            InputNotice.SetActive(true);
            isSelectablePressed = false;
            ShouldDetectRepect = true;
            SelectedObject = "GoLeft";
            UI.GetComponent<DefaultButton>().onStart = GoLeftButton;
        }
    }//Button專用
    private void ChangeJump()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            isSelect = true;
            isInputOpen = true;
            ShouldTextChange = true;
            InputNotice.SetActive(true);
            isSelectablePressed = false;
            ShouldDetectRepect = true;
            SelectedObject = "Jump";
            UI.GetComponent<DefaultButton>().onStart = JumpButton;
        }
    }//Button專用
    private void ChangeNormalAtk()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            isSelect = true;
            isInputOpen = true;
            ShouldTextChange = true;
            InputNotice.SetActive(true);
            isSelectablePressed = false;
            ShouldDetectRepect = true;
            SelectedObject = "NormalAtk";
            UI.GetComponent<DefaultButton>().onStart = NormalAtkButton;
        }
    }//Button專用
    private void ChangeStrongAtk()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            isSelect = true;
            isInputOpen = true;
            ShouldTextChange = true;
            InputNotice.SetActive(true);
            isSelectablePressed = false;
            ShouldDetectRepect = true;
            SelectedObject = "StrongAtk";
            UI.GetComponent<DefaultButton>().onStart = StrongAtkButton;
        }
    }//Button專用
    private void ChangeDash()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            isSelect = true;
            isInputOpen = true;
            ShouldTextChange = true;
            InputNotice.SetActive(true);
            isSelectablePressed = false;
            ShouldDetectRepect = true;
            SelectedObject = "Dash";
            UI.GetComponent<DefaultButton>().onStart = DashButton;
        }
    }//Button專用
    private void ChangeThrowCocktail()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            isSelect = true;
            isInputOpen = true;
            ShouldTextChange = true;
            InputNotice.SetActive(true);
            isSelectablePressed = false;
            ShouldDetectRepect = true;
            SelectedObject = "ThrowCocktail";
            UI.GetComponent<DefaultButton>().onStart = UseItemButton;
        }
    }//Button專用
    private void ChangeRestore()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            isSelect = true;
            isInputOpen = true;
            ShouldTextChange = true;
            InputNotice.SetActive(true);
            isSelectablePressed = false;
            ShouldDetectRepect = true;
            SelectedObject = "Restore";
            UI.GetComponent<DefaultButton>().onStart = RestoreButton;
        }
    }//Button專用
    private void ChangeInteract()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            isSelect = true;
            isInputOpen = true;
            ShouldTextChange = true;
            InputNotice.SetActive(true);
            isSelectablePressed = false;
            ShouldDetectRepect = true;
            SelectedObject = "Interact";
            UI.GetComponent<DefaultButton>().onStart = InteractButton;
        }
    }//Button專用
    private void ChangeShoot()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            isSelect = true;
            isInputOpen = true;
            ShouldTextChange = true;
            InputNotice.SetActive(true);
            isSelectablePressed = false;
            ShouldDetectRepect = true;
            SelectedObject = "Shoot";
            UI.GetComponent<DefaultButton>().onStart = ShootButton;
        }
    }//Button專用
    private void ChangeBlock()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            isSelect = true;
            isInputOpen = true;
            ShouldTextChange = true;
            InputNotice.SetActive(true);
            isSelectablePressed = false;
            ShouldDetectRepect = true;
            SelectedObject = "Block";
            UI.GetComponent<DefaultButton>().onStart = BlockButton;
        }
    }//Button專用
    private void ChangeChangeThrowType()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            isSelect = true;
            isInputOpen = true;
            ShouldTextChange = true;
            InputNotice.SetActive(true);
            isSelectablePressed = false;
            ShouldDetectRepect = true;
            SelectedObject = "ChangeThrowType";
            UI.GetComponent<DefaultButton>().onStart = ChangeUseItemButton;
        }
    }//Button專用
    private void ChangeOpenItem()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            isSelect = true;
            isInputOpen = true;
            ShouldTextChange = true;
            InputNotice.SetActive(true);
            isSelectablePressed = false;
            ShouldDetectRepect = true;
            SelectedObject = "OpenItem";
            UI.GetComponent<DefaultButton>().onStart = OpenItemWindowButton;
        }
    }//Button專用

    private void ChangeJumpC()
    {
        if (Input.GetKey(KeyCode.JoystickButton0))
        {
            BeginChangeKeyCodeC();
            SelectedObject = "JumpC";
            UI.GetComponent<DefaultButton>().onStart = JumpButtonC;
        }
    }//Button專用
    private void ChangeNormalAtkC()
    {
        if (Input.GetKey(KeyCode.JoystickButton0))
        {
            BeginChangeKeyCodeC();
            SelectedObject = "NormalAtkC";
            UI.GetComponent<DefaultButton>().onStart = NormalAtkButtonC;
        }
    }//Button專用
    private void ChangeStrongAtkC()
    {
        if (Input.GetKey(KeyCode.JoystickButton0))
        {
            BeginChangeKeyCodeC();
            SelectedObject = "StrongAtkC";
            UI.GetComponent<DefaultButton>().onStart = StrongAtkButtonC;
        }
    }//Button專用
    private void ChangeDashC()
    {
        if (Input.GetKey(KeyCode.JoystickButton0))
        {
            BeginChangeKeyCodeC();
            SelectedObject = "DashC";
            UI.GetComponent<DefaultButton>().onStart = DashButtonC;
        }
    }//Button專用
    private void ChangeThrowCocktailC()
    {
        if (Input.GetKey(KeyCode.JoystickButton0))
        {
            BeginChangeKeyCodeC();
            SelectedObject = "ThrowCocktailC";
            UI.GetComponent<DefaultButton>().onStart = UseItemButtonC;
        }
    }//Button專用
    private void ChangeRestoreC()
    {
        if (Input.GetKey(KeyCode.JoystickButton0))
        {
            BeginChangeKeyCodeC();
            SelectedObject = "RestoreC";
            UI.GetComponent<DefaultButton>().onStart = RestoreButtonC;
        }
    }//Button專用
    private void ChangeInteractC()
    {
        if (Input.GetKey(KeyCode.JoystickButton0))
        {
            BeginChangeKeyCodeC();
            SelectedObject = "InteractC";
            UI.GetComponent<DefaultButton>().onStart = InteractButtonC;
        }
    }//Button專用
    private void ChangeShootC()
    {
        if (Input.GetKey(KeyCode.JoystickButton0))
        {
            BeginChangeKeyCodeC();
            SelectedObject = "ShootC";
            UI.GetComponent<DefaultButton>().onStart = ShootButtonC;
        }
    }//Button專用
    private void ChangeBlockC()
    {
        if (Input.GetKey(KeyCode.JoystickButton0))
        {
            BeginChangeKeyCodeC();
            SelectedObject = "BlockC";
            UI.GetComponent<DefaultButton>().onStart = BlockButtonC;
        }
    }//Button專用
    private void ChangeChangeThrowTypeC()
    {
        if (Input.GetKey(KeyCode.JoystickButton0))
        {
            BeginChangeKeyCodeC();
            SelectedObject = "ChangeThrowTypeC";
            UI.GetComponent<DefaultButton>().onStart = ChangeUseItemButtonC;
        }
    }//Button專用
    private void BeginChangeKeyCodeC()
    {
        isSelectC = true;
        isInputOpenC = true;
        ShouldTextChangeC = true;
        InputNotice.SetActive(true);
        isSelectablePressed = false;
        ShouldDetectRepectC = true;
        SelectButtonController.CloseSelectButtonController();
        AllKeyButtonFalse();
    }

    private void InputSelect()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            TemporaryKeyCode = KeyCode.A;
            TempopraryText = "A";
            isSelectablePressed = true;
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            TemporaryKeyCode = KeyCode.B;
            TempopraryText = "B";
            isSelectablePressed = true;
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            TemporaryKeyCode = KeyCode.C;
            TempopraryText = "C";
            isSelectablePressed = true;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            TemporaryKeyCode = KeyCode.D;
            TempopraryText = "D";
            isSelectablePressed = true;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            TemporaryKeyCode = KeyCode.E;
            TempopraryText = "E";
            isSelectablePressed = true;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            TemporaryKeyCode = KeyCode.F;
            TempopraryText = "F";
            isSelectablePressed = true;
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            TemporaryKeyCode = KeyCode.G;
            TempopraryText = "G";
            isSelectablePressed = true;
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            TemporaryKeyCode = KeyCode.H;
            TempopraryText = "H";
            isSelectablePressed = true;
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            TemporaryKeyCode = KeyCode.I;
            TempopraryText = "I";
            isSelectablePressed = true;
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            TemporaryKeyCode = KeyCode.J;
            TempopraryText = "J";
            isSelectablePressed = true;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            TemporaryKeyCode = KeyCode.K;
            TempopraryText = "K";
            isSelectablePressed = true;
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            TemporaryKeyCode = KeyCode.L;
            TempopraryText = "L";
            isSelectablePressed = true;
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            TemporaryKeyCode = KeyCode.M;
            TempopraryText = "M";
            isSelectablePressed = true;
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            TemporaryKeyCode = KeyCode.N;
            TempopraryText = "N";
            isSelectablePressed = true;
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            TemporaryKeyCode = KeyCode.O;
            TempopraryText = "O";
            isSelectablePressed = true;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            TemporaryKeyCode = KeyCode.P;
            TempopraryText = "P";
            isSelectablePressed = true;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TemporaryKeyCode = KeyCode.Q;
            TempopraryText = "Q";
            isSelectablePressed = true;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            TemporaryKeyCode = KeyCode.R;
            TempopraryText = "R";
            isSelectablePressed = true;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            TemporaryKeyCode = KeyCode.S;
            TempopraryText = "S";
            isSelectablePressed = true;
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            TemporaryKeyCode = KeyCode.T;
            TempopraryText = "T";
            isSelectablePressed = true;
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            TemporaryKeyCode = KeyCode.U;
            TempopraryText = "U";
            isSelectablePressed = true;
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            TemporaryKeyCode = KeyCode.V;
            TempopraryText = "V";
            isSelectablePressed = true;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            TemporaryKeyCode = KeyCode.W;
            TempopraryText = "W";
            isSelectablePressed = true;
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            TemporaryKeyCode = KeyCode.X;
            TempopraryText = "X";
            isSelectablePressed = true;
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            TemporaryKeyCode = KeyCode.Y;
            TempopraryText = "Y";
            isSelectablePressed = true;
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            TemporaryKeyCode = KeyCode.Z;
            TempopraryText = "Z";
            isSelectablePressed = true;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TemporaryKeyCode = KeyCode.Space;
            TempopraryText = "Space";
            isSelectablePressed = true;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            TemporaryKeyCode = KeyCode.RightArrow;
            TempopraryText = "RightArrow";
            isSelectablePressed = true;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            TemporaryKeyCode = KeyCode.LeftArrow;
            TempopraryText = "LeftArrow";
            isSelectablePressed = true;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            TemporaryKeyCode = KeyCode.UpArrow;
            TempopraryText = "UpArrow";
            isSelectablePressed = true;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            TemporaryKeyCode = KeyCode.DownArrow;
            TempopraryText = "DownArrow";
            isSelectablePressed = true;
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            TemporaryKeyCode = KeyCode.LeftControl;
            TempopraryText = "LeftControl";
            isSelectablePressed = true;
        }
        if (Input.GetKeyDown(KeyCode.RightControl))
        {
            TemporaryKeyCode = KeyCode.RightControl;
            TempopraryText = "RightControl";
            isSelectablePressed = true;
        }
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            TemporaryKeyCode = KeyCode.RightShift;
            TempopraryText = "RightShift";
            isSelectablePressed = true;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            TemporaryKeyCode = KeyCode.LeftShift;
            TempopraryText = "LeftShift";
            isSelectablePressed = true;
        }
        if (Input.GetKeyDown(KeyCode.Comma))
        {
            TemporaryKeyCode = KeyCode.Comma;
            TempopraryText = "<";
            isSelectablePressed = true;
        }
        if (Input.GetKeyDown(KeyCode.Period))
        {
            TemporaryKeyCode = KeyCode.Period;
            TempopraryText = ">";
            isSelectablePressed = true;
        }
        if (Input.GetKeyDown(KeyCode.Slash))
        {
            TemporaryKeyCode = KeyCode.Slash;
            TempopraryText = "?";
            isSelectablePressed = true;
        }
        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            TemporaryKeyCode = KeyCode.LeftBracket;
            TempopraryText = "{";
            isSelectablePressed = true;
        }
        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            TemporaryKeyCode = KeyCode.RightBracket;
            TempopraryText = "}";
            isSelectablePressed = true;
        }
        if (Input.GetKeyDown(KeyCode.Semicolon))
        {
            TemporaryKeyCode = KeyCode.Semicolon;
            TempopraryText = ":";
            isSelectablePressed = true;
        }
        if (Input.GetKeyDown(KeyCode.Quote))
        {
            TemporaryKeyCode = KeyCode.Quote;
            TempopraryText = "\"";
            isSelectablePressed = true;
        }
    }

    private void InputSelectC()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            TemporaryNumberC = 1;
            TempopraryTextC = "A";
            isSelectablePressed = true;
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            TemporaryNumberC = 2;
            TempopraryTextC = "B";
            isSelectablePressed = true;
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton2))
        {
            TemporaryNumberC = 3;
            TempopraryTextC = "X";
            isSelectablePressed = true;
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton3))
        {
            TemporaryNumberC = 4;
            TempopraryTextC = "Y";
            isSelectablePressed = true;
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton4))
        {
            TemporaryNumberC = 5;
            TempopraryTextC = "LB";
            isSelectablePressed = true;
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton5))
        {
            TemporaryNumberC = 6;
            TempopraryTextC = "RB";
            isSelectablePressed = true;
        }
        if (OldVerXboxControllerDetect.isLTPressed)
        {
            TemporaryNumberC = 7;
            TempopraryTextC = "LT";
            isSelectablePressed = true;
        }
        if (OldVerXboxControllerDetect.isRTPressed)
        {
            TemporaryNumberC = 8;
            TempopraryTextC = "RT";
            isSelectablePressed = true;
        }
        if (OldVerXboxControllerDetect.isCrossUpPressed)
        {
            TemporaryNumberC = 9;
            TempopraryTextC = "UpArrow";
            isSelectablePressed = true;
        }
        if (OldVerXboxControllerDetect.isCrossRightPressed)
        {
            TemporaryNumberC = 10;
            TempopraryTextC = "RightArrow";
            isSelectablePressed = true;
        }
        if (OldVerXboxControllerDetect.isCrossDownPressed)
        {
            TemporaryNumberC = 11;
            TempopraryTextC = "DownArrow";
            isSelectablePressed = true;
        }
        if (OldVerXboxControllerDetect.isCrossLeftPressed)
        {
            TemporaryNumberC = 12;
            TempopraryTextC = "LeftArrow";
            isSelectablePressed = true;
        }
    }

    private void ReSetKeyCode()
    {
        isAlert = false;
        GoRight = KeyCode.RightArrow;
        GoLeft = KeyCode.LeftArrow;
        Jump = KeyCode.Z;
        NormalAtk = KeyCode.X;
        StrongAtk = KeyCode.C;
        Dash = KeyCode.Space;
        UseItem = KeyCode.A;
        Restore = KeyCode.Q;
        Interact = KeyCode.UpArrow;
        Shoot = KeyCode.S;
        Block = KeyCode.LeftControl;
        ChangeUseItem = KeyCode.D;
        OpenItemWindow = KeyCode.E;
        CurrentGoRightText.text = "RightArrow";
        CurrentGoLeftText.text = "LeftArrow";
        CurrentJumpText.text = "Z";
        CurrentNormalAtkText.text = "X";
        CurrentStrongAtkText.text = "C";
        CurrentDashText.text = "Space";
        CurrentUseItemText.text = "A";
        CurrentRestoreText.text = "Q";
        CurrentInteractText.text = "UpArrow";
        CurrentShootText.text = "S";
        CurrentBlockText.text = "LeftControl";
        CurrentChangeUseItemText.text = "D";
        CurrentOpenItemWindowText.text = "E";
        GoRightText = "RightArrow";
        GoLeftText = "LeftArrow";
        JumpText = "Z";
        NormalAtkText = "X";
        StrongAtkText = "C";
        DashText = "Space";
        UseItemText = "A";
        RestoreText = "Q";
        InteractText = "UpArrow";
        ShootText = "S";
        BlockText = "LeftControl";
        ChangeUseItemText = "D";
        OpenItemWindowText = "E";
        GoRightRecord = "RightArrow";
        GoLeftRecord = "LeftArrow";
        JumpRecord = "Z";
        NormalAtkRecord = "X";
        StrongAtkRecord = "C";
        DashRecord = "Space";
        UseItemRecord = "A";
        RestoreRecord = "Q";
        InteractRecord = "UpArrow";
        ShootRecord = "S";
        BlockRecord = "LeftControl";
        ChangeUseItemRecord = "D";
        OpenItemWindowRecord = "E";

        JumpNumber = 1;
        NormalAtkNumber = 6;
        StrongAtkNumber = 8;
        DashNumber = 3;
        UseItemNumber = 7;
        RestoreNumber = 9;
        InteractNumber = 2;
        ShootNumber = 4;
        BlockNumber = 5;
        ChangeUseItemNumber = 10;
        CurrentJumpTextC.text = "A";
        CurrentNormalAtkTextC.text = "RB";
        CurrentStrongAtkTextC.text = "RT";
        CurrentDashTextC.text = "X";
        CurrentUseItemTextC.text = "LT";
        CurrentRestoreTextC.text = "UpArrow";
        CurrentInteractTextC.text = "B";
        CurrentShootTextC.text = "Y";
        CurrentBlockTextC.text = "LB";
        CurrentChangeUseItemTextC.text = "RightArrow";
        JumpTextC = "A";
        NormalAtkTextC = "RB";
        StrongAtkTextC = "RT";
        DashTextC = "X";
        UseItemTextC = "LT";
        RestoreTextC = "UpArrow";
        InteractTextC = "B";
        ShootTextC = "Y";
        BlockTextC = "LB";
        ChangeUseItemTextC = "RightArrow";
        JumpRecordC = "A";
        NormalAtkRecordC = "RB";
        StrongAtkRecordC = "RT";
        DashRecordC = "X";
        UseItemRecordC = "LT";
        RestoreRecordC = "UpArrow";
        InteractRecordC = "B";
        ShootRecordC = "Y";
        BlockRecordC = "LB";
        ChangeUseItemRecordC = "RightArrow";
    }//Button專用

    private void DetectLack()
    {
        if (GoRightRecord.Length == 0 || GoLeftRecord.Length == 0 || JumpRecord.Length == 0 || NormalAtkRecord.Length == 0 || StrongAtkRecord.Length == 0 || DashRecord.Length == 0 || UseItemRecord.Length == 0 || RestoreRecord.Length == 0 || InteractRecord.Length == 0 || ShootRecord.Length == 0 || BlockRecord.Length == 0 || ChangeUseItemRecord.Length == 0 || OpenItemWindowRecord.Length == 0)
        {
            isAlert = true;
        }
        else
        {
            isAlert = false;
        }
    }

    private void DetectLackC()
    {
        if (JumpRecordC.Length == 0 || NormalAtkRecordC.Length == 0 || StrongAtkRecordC.Length == 0 || DashRecordC.Length == 0 || UseItemRecordC.Length == 0 || RestoreRecordC.Length == 0 || InteractRecordC.Length == 0 || ShootRecordC.Length == 0 || BlockRecordC.Length == 0 || ChangeUseItemRecordC.Length == 0)
        {
            isAlert = true;
        }
        else
        {
            isAlert = false;
        }
    }

    private void ActionJudgement()
    {
        int a = 0;
        int A = 0;
        int b = 0;
        int B = 0;
        if (Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            if(a == 0)
            {
                a = 1;
            }
            else
            {
                b = 1;
            }
        }
        if (Input.GetKeyUp(KeyCode.JoystickButton0))
        {
            if (A == 0)
            {
                A = 1;
            }
            else
            {
                B = 1;
            }
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            if (a == 0)
            {
                a = 2;
            }
            else
            {
                b = 2;
            }
        }
        if (Input.GetKeyUp(KeyCode.JoystickButton1))
        {
            if (A == 0)
            {
                A = 2;
            }
            else
            {
                B = 2;
            }
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton2))
        {
            if (a == 0)
            {
                a = 3;
            }
            else
            {
                b = 3;
            }
        }
        if (Input.GetKeyUp(KeyCode.JoystickButton2))
        {
            if (A == 0)
            {
                A = 3;
            }
            else
            {
                B = 3;
            }
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton3))
        {
            if (a == 0)
            {
                a = 4;
            }
            else
            {
                b = 4;
            }
        }
        if (Input.GetKeyUp(KeyCode.JoystickButton3))
        {
            if (A == 0)
            {
                A = 4;
            }
            else
            {
                B = 4;
            }
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton4))
        {
            if (a == 0)
            {
                a = 5;
            }
            else
            {
                b = 5;
            }
        }
        if (Input.GetKeyUp(KeyCode.JoystickButton4))
        {
            if (A == 0)
            {
                A = 5;
            }
            else
            {
                B = 5;
            }
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton5))
        {
            if (a == 0)
            {
                a = 6;
            }
            else
            {
                b = 6;
            }
        }
        if (Input.GetKeyUp(KeyCode.JoystickButton5))
        {
            if (A == 0)
            {
                A = 6;
            }
            else
            {
                B = 6;
            }
        }
        if (OldVerXboxControllerDetect.isLTPressed)
        {
            if (a == 0)
            {
                a = 7;
            }
            else
            {
                b = 7;
            }
        }
        if (OldVerXboxControllerDetect.isLTUp)
        {
            if (A == 0)
            {
                A = 7;
            }
            else
            {
                B = 7;
            }
        }
        if (OldVerXboxControllerDetect.isRTPressed)
        {
            if (a == 0)
            {
                a = 8;
            }
            else
            {
                b = 8;
            }
        }
        if (OldVerXboxControllerDetect.isRTUp)
        {
            if (A == 0)
            {
                A = 8;
            }
            else
            {
                B = 8;
            }
        }
        if (OldVerXboxControllerDetect.isCrossUpPressed)
        {
            if (a == 0)
            {
                a = 9;
            }
            else
            {
                b = 9;
            }
        }
        if (OldVerXboxControllerDetect.isCrossUpUp)
        {
            if (A == 0)
            {
                A = 9;
            }
            else
            {
                B = 9;
            }
        }
        if (OldVerXboxControllerDetect.isCrossRightPressed)
        {
            if (a == 0)
            {
                a = 10;
            }
            else
            {
                b = 10;
            }
        }
        if (OldVerXboxControllerDetect.isCrossRightUp)
        {
            if (A == 0)
            {
                A = 10;
            }
            else
            {
                B = 10;
            }
        }
        if (OldVerXboxControllerDetect.isCrossDownPressed)
        {
            if (a == 0)
            {
                a = 11;
            }
            else
            {
                b = 11;
            }
        }
        if (OldVerXboxControllerDetect.isCrossDownUp)
        {
            if (A == 0)
            {
                A = 11;
            }
            else
            {
                B = 11;
            }
        }
        if (OldVerXboxControllerDetect.isCrossLeftPressed)
        {
            if (a == 0)
            {
                a = 12;
            }
            else
            {
                b = 12;
            }
        }
        if (OldVerXboxControllerDetect.isCrossLeftUp)
        {
            if (A == 0)
            {
                A = 12;
            }
            else
            {
                B = 12;
            }
        }

        if (JumpNumber == a || JumpNumber == b)
        {
            JumpPressed = true;
        }
        if (JumpNumber == A || JumpNumber == B)
        {
            JumpUp = true;
        }

        if (NormalAtkNumber == a || NormalAtkNumber == b)
        {
            NormalAtkPressed = true;
        }
        if (NormalAtkNumber == A || NormalAtkNumber == B)
        {
            NormalAtkUp = true;
        }

        if (StrongAtkNumber == a || StrongAtkNumber == b)
        {
            StrongAtkPressed = true;
        }

        if (DashNumber == a || DashNumber == b)
        {
            DashPressed = true;
        }

        if (UseItemNumber == a || UseItemNumber == b)
        {
            UseItemPressed = true;
        }
        if (UseItemNumber == A || UseItemNumber == B)
        {
            UseItemUp = true;
        }

        if (RestoreNumber == a || RestoreNumber == b)
        {
            RestorePressed = true;
        }

        if (InteractNumber == a || InteractNumber == b)
        {
            InteractPressed = true;
        }
        if (InteractNumber == A || InteractNumber == B)
        {
            InteractUp = true;
        }

        if (ShootNumber == a || ShootNumber == b)
        {
            ShootPressed = true;
        }
        if (ShootNumber == A || ShootNumber == B)
        {
            ShootUp = true;
        }

        if (BlockNumber == a || BlockNumber == b)
        {
            BlockPressed = true;
        }

        if (ChangeUseItemNumber == a || ChangeUseItemNumber == b)
        {
            ChangeUseItemPressed = true;
        }
    }

    public void XboxActionJudge(XboxCommandReceiver recevier)
    {
        int a = 0;
        int A = 0;
        int b = 0;
        int B = 0;
        if (Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            if (a == 0)
            {
                a = 1;
            }
            else
            {
                b = 1;
            }
        }
        if (Input.GetKeyUp(KeyCode.JoystickButton0))
        {
            if (A == 0)
            {
                A = 1;
            }
            else
            {
                B = 1;
            }
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            if (a == 0)
            {
                a = 2;
            }
            else
            {
                b = 2;
            }
        }
        if (Input.GetKeyUp(KeyCode.JoystickButton1))
        {
            if (A == 0)
            {
                A = 2;
            }
            else
            {
                B = 2;
            }
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton2))
        {
            if (a == 0)
            {
                a = 3;
            }
            else
            {
                b = 3;
            }
        }
        if (Input.GetKeyUp(KeyCode.JoystickButton2))
        {
            if (A == 0)
            {
                A = 3;
            }
            else
            {
                B = 3;
            }
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton3))
        {
            if (a == 0)
            {
                a = 4;
            }
            else
            {
                b = 4;
            }
        }
        if (Input.GetKeyUp(KeyCode.JoystickButton3))
        {
            if (A == 0)
            {
                A = 4;
            }
            else
            {
                B = 4;
            }
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton4))
        {
            if (a == 0)
            {
                a = 5;
            }
            else
            {
                b = 5;
            }
        }
        if (Input.GetKeyUp(KeyCode.JoystickButton4))
        {
            if (A == 0)
            {
                A = 5;
            }
            else
            {
                B = 5;
            }
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton5))
        {
            if (a == 0)
            {
                a = 6;
            }
            else
            {
                b = 6;
            }
        }
        if (Input.GetKeyUp(KeyCode.JoystickButton5))
        {
            if (A == 0)
            {
                A = 6;
            }
            else
            {
                B = 6;
            }
        }
        if (recevier.isLTPressed)
        {
            if (a == 0)
            {
                a = 7;
            }
            else
            {
                b = 7;
            }
        }
        if (recevier.isLTUp)
        {
            if (A == 0)
            {
                A = 7;
            }
            else
            {
                B = 7;
            }
        }
        if (recevier.isRTPressed)
        {
            if (a == 0)
            {
                a = 8;
            }
            else
            {
                b = 8;
            }
        }
        if (recevier.isRTUp)
        {
            if (A == 0)
            {
                A = 8;
            }
            else
            {
                B = 8;
            }
        }
        if (recevier.isCrossUpPressed)
        {
            if (a == 0)
            {
                a = 9;
            }
            else
            {
                b = 9;
            }
        }
        if (recevier.isCrossUpUp)
        {
            if (A == 0)
            {
                A = 9;
            }
            else
            {
                B = 9;
            }
        }
        if (recevier.isCrossRightPressed)
        {
            if (a == 0)
            {
                a = 10;
            }
            else
            {
                b = 10;
            }
        }
        if (recevier.isCrossRightUp)
        {
            if (A == 0)
            {
                A = 10;
            }
            else
            {
                B = 10;
            }
        }
        if (recevier.isCrossDownPressed)
        {
            if (a == 0)
            {
                a = 11;
            }
            else
            {
                b = 11;
            }
        }
        if (recevier.isCrossDownUp)
        {
            if (A == 0)
            {
                A = 11;
            }
            else
            {
                B = 11;
            }
        }
        if (recevier.isCrossLeftPressed)
        {
            if (a == 0)
            {
                a = 12;
            }
            else
            {
                b = 12;
            }
        }
        if (recevier.isCrossLeftUp)
        {
            if (A == 0)
            {
                A = 12;
            }
            else
            {
                B = 12;
            }
        }

        if (JumpNumber == a || JumpNumber == b)
        {
            JumpPressed = true;
        }
        if (JumpNumber == A || JumpNumber == B)
        {
            JumpUp = true;
        }

        if (NormalAtkNumber == a || NormalAtkNumber == b)
        {
            NormalAtkPressed = true;
        }
        if (NormalAtkNumber == A || NormalAtkNumber == B)
        {
            NormalAtkUp = true;
        }

        if (StrongAtkNumber == a || StrongAtkNumber == b)
        {
            StrongAtkPressed = true;
        }

        if (DashNumber == a || DashNumber == b)
        {
            DashPressed = true;
        }

        if (UseItemNumber == a || UseItemNumber == b)
        {
            UseItemPressed = true;
        }
        if (UseItemNumber == A || UseItemNumber == B)
        {
            UseItemUp = true;
        }

        if (RestoreNumber == a || RestoreNumber == b)
        {
            RestorePressed = true;
        }

        if (InteractNumber == a || InteractNumber == b)
        {
            InteractPressed = true;
        }
        if (InteractNumber == A || InteractNumber == B)
        {
            InteractUp = true;
        }

        if (ShootNumber == a || ShootNumber == b)
        {
            ShootPressed = true;
        }
        if (ShootNumber == A || ShootNumber == B)
        {
            ShootUp = true;
        }

        if (BlockNumber == a || BlockNumber == b)
        {
            BlockPressed = true;
        }

        if (ChangeUseItemNumber == a || ChangeUseItemNumber == b)
        {
            ChangeUseItemPressed = true;
        }
    }

    public void ResetControllerCommand()
    {
        if (JumpPressed)
        {
            JumpPressed = false;
        }
        if (JumpUp)
        {
            JumpUp = false;
        }
        if (DashPressed)
        {
            DashPressed = false;
        }
        if (NormalAtkPressed)
        {
            NormalAtkPressed = false;
        }
        if (NormalAtkUp)
        {
            NormalAtkUp = false;
        }
        if (StrongAtkPressed)
        {
            StrongAtkPressed = false;
        }
        if (ChangeUseItemPressed)
        {
            ChangeUseItemPressed = false;
        }
        if (UseItemPressed)
        {
            UseItemPressed = false;
        }
        if (UseItemUp)
        {
            UseItemUp = false;
        }
        if (RestorePressed)
        {
            RestorePressed = false;
        }
        if (ShootPressed)
        {
            ShootPressed = false;
        }
        if (ShootUp)
        {
            ShootUp = false;
        }
        if (BlockPressed)
        {
            BlockPressed = false;
        }
    }

    private void AllKeyButtonFalse()
    {
        GoRightButton.interactable = false;
        GoLeftButton.interactable = false;
        JumpButton.interactable = false;
        NormalAtkButton.interactable = false;
        StrongAtkButton.interactable = false;
        DashButton.interactable = false;
        UseItemButton.interactable = false;
        RestoreButton.interactable = false;
        InteractButton.interactable = false;
        ShootButton.interactable = false;
        BlockButton.interactable = false;
        ChangeUseItemButton.interactable = false;
        OpenItemWindowButton.interactable = false;
        ResetButton.interactable = false;
        JumpButtonC.interactable = false;
        NormalAtkButtonC.interactable = false;
        StrongAtkButtonC.interactable = false;
        DashButtonC.interactable = false;
        UseItemButtonC.interactable = false;
        RestoreButtonC.interactable = false;
        InteractButtonC.interactable = false;
        ShootButtonC.interactable = false;
        BlockButtonC.interactable = false;
        ChangeUseItemButtonC.interactable = false;
    }

    private void AllKeyButtonTrue()
    {
        GoRightButton.interactable = true;
        GoLeftButton.interactable = true;
        JumpButton.interactable = true;
        NormalAtkButton.interactable = true;
        StrongAtkButton.interactable = true;
        DashButton.interactable = true;
        UseItemButton.interactable = true;
        RestoreButton.interactable = true;
        InteractButton.interactable = true;
        ShootButton.interactable = true;
        BlockButton.interactable = true;
        ChangeUseItemButton.interactable = true;
        OpenItemWindowButton.interactable = true;
        ResetButton.interactable = true;
        JumpButtonC.interactable = true;
        NormalAtkButtonC.interactable = true;
        StrongAtkButtonC.interactable = true;
        DashButtonC.interactable = true;
        UseItemButtonC.interactable = true;
        RestoreButtonC.interactable = true;
        InteractButtonC.interactable = true;
        ShootButtonC.interactable = true;
        BlockButtonC.interactable = true;
        ChangeUseItemButtonC.interactable = true;
    }
}
