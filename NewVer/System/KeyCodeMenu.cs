using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static PlayerCommandManager;

public class KeyCodeMenu : MonoBehaviour
{
    //變數後 + C 的都是手把專用
    private NewKeyCodeManager _manager;

    private Dictionary<string, Button> Buttons = new Dictionary<string, Button>();
    private Dictionary<Command, Text> KeyboardButtonTexts = new Dictionary<Command, Text>();
    private Dictionary<Command, Text> XboxButtonTexts = new Dictionary<Command, Text>();
    private DefaultButton NowDefaultButton;

    [HideInInspector] public bool isInputOpen;//其他Script會用到(PauseMenuController)
    [HideInInspector] public bool isInputOpenX;//其他Script會用到(PauseMenuController)
    [HideInInspector] public bool isReChooseNoticeAppear;//其他Script會用到(ReChooseNotice)
    public GameObject UI;
    private GameObject ReChooseNotice;
    private GameObject InputNotice;
    private Text CurrentRightMoveText;
    private Text CurrentLeftMoveText;
    private Text CurrentJumpText;
    private Text CurrentNormalAtkText;
    private Text CurrentStrongAtkText;
    private Text CurrentDashText;
    private Text CurrentUseItemText;
    private Text CurrentRestoreText;
    private Text CurrentInteractText;
    private Text CurrentShootText;
    private Text CurrentBlockText;
    private Text CurrentItemWindowText;
    private Text CurrentChangeItemText;
    private Button RightMoveButton;
    private Button LeftMoveButton;
    private Button JumpButton;
    private Button NormalAtkButton;
    private Button StrongAtkButton;
    private Button DashButton;
    private Button UseItemButton;
    private Button RestoreButton;
    private Button InteractButton;
    private Button ShootButton;
    private Button BlockButton;
    private Button ItemWindowButton;
    private Button ChangeItemButton;
    private Button ResetButton;

    [HideInInspector] public bool isAlert;//其他Script會用到(PauseMenuController，LackKeyCodeNotice)

    private Text CurrentJumpTextX;
    private Text CurrentNormalAtkTextX;
    private Text CurrentStrongAtkTextX;
    private Text CurrentDashTextX;
    private Text CurrentUseItemTextX;
    private Text CurrentRestoreTextX;
    private Text CurrentInteractTextX;
    private Text CurrentShootTextX;
    private Text CurrentBlockTextX;
    private Text CurrentChangeItemTextX;
    private Button JumpButtonX;
    private Button NormalAtkButtonX;
    private Button StrongAtkButtonX;
    private Button DashButtonX;
    private Button UseItemButtonX;
    private Button RestoreButtonX;
    private Button InteractButtonX;
    private Button ShootButtonX;
    private Button BlockButtonX;
    private Button ChangeItemButtonX;

    private bool BeginCoolDown;
    private float ButtonCoolDownTime = 0.03f;
    private float ButtonCoolDownLeft = 0.03f;

    private void Start()
    {
        if (GameObject.Find("FollowSystem").GetComponent<InputManager>() != null)
        {
            _manager = GameObject.Find("FollowSystem").GetComponent<InputManager>()._keyCodeManager;
        }

        InisializeDictionary();

        if (UI == null)
        {
            UI = GameObject.FindGameObjectWithTag("UI");
        }
        if (UI != null)
        {
            if (SceneManager.GetActiveScene().name == "title")
            {
                ReChooseNotice = UI.transform.GetChild(15).gameObject;
                InputNotice = UI.transform.GetChild(14).gameObject;
                CurrentRightMoveText = UI.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentLeftMoveText = UI.transform.GetChild(2).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentJumpText = UI.transform.GetChild(3).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentNormalAtkText = UI.transform.GetChild(4).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentStrongAtkText = UI.transform.GetChild(5).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentDashText = UI.transform.GetChild(6).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentUseItemText = UI.transform.GetChild(7).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentRestoreText = UI.transform.GetChild(8).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentInteractText = UI.transform.GetChild(9).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentShootText = UI.transform.GetChild(10).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentBlockText = UI.transform.GetChild(11).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentChangeItemText = UI.transform.GetChild(12).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentItemWindowText = UI.transform.GetChild(13).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Text>();
                RightMoveButton = UI.transform.GetChild(1).gameObject.transform.GetChild(1).GetComponent<Button>();
                LeftMoveButton = UI.transform.GetChild(2).gameObject.transform.GetChild(1).GetComponent<Button>();
                JumpButton = UI.transform.GetChild(3).gameObject.transform.GetChild(1).GetComponent<Button>();
                NormalAtkButton = UI.transform.GetChild(4).gameObject.transform.GetChild(1).GetComponent<Button>();
                StrongAtkButton = UI.transform.GetChild(5).gameObject.transform.GetChild(1).GetComponent<Button>();
                DashButton = UI.transform.GetChild(6).gameObject.transform.GetChild(1).GetComponent<Button>();
                UseItemButton = UI.transform.GetChild(7).gameObject.transform.GetChild(1).GetComponent<Button>();
                RestoreButton = UI.transform.GetChild(8).gameObject.transform.GetChild(1).GetComponent<Button>();
                InteractButton = UI.transform.GetChild(9).gameObject.transform.GetChild(1).GetComponent<Button>();
                ShootButton = UI.transform.GetChild(10).gameObject.transform.GetChild(1).GetComponent<Button>();
                BlockButton = UI.transform.GetChild(11).gameObject.transform.GetChild(1).GetComponent<Button>();
                ChangeItemButton = UI.transform.GetChild(12).gameObject.transform.GetChild(1).GetComponent<Button>();
                ItemWindowButton = UI.transform.GetChild(13).gameObject.transform.GetChild(1).GetComponent<Button>();
                ResetButton = UI.transform.GetChild(17).GetComponent<Button>();

                CurrentJumpTextX = UI.transform.GetChild(3).gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentNormalAtkTextX = UI.transform.GetChild(4).gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentStrongAtkTextX = UI.transform.GetChild(5).gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentDashTextX = UI.transform.GetChild(6).gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentUseItemTextX = UI.transform.GetChild(7).gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentRestoreTextX = UI.transform.GetChild(8).gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentInteractTextX = UI.transform.GetChild(9).gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentShootTextX = UI.transform.GetChild(10).gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentBlockTextX = UI.transform.GetChild(11).gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentChangeItemTextX = UI.transform.GetChild(12).gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).GetComponent<Text>();
                JumpButtonX = UI.transform.GetChild(3).gameObject.transform.GetChild(2).GetComponent<Button>();
                NormalAtkButtonX = UI.transform.GetChild(4).gameObject.transform.GetChild(2).GetComponent<Button>();
                StrongAtkButtonX = UI.transform.GetChild(5).gameObject.transform.GetChild(2).GetComponent<Button>();
                DashButtonX = UI.transform.GetChild(6).gameObject.transform.GetChild(2).GetComponent<Button>();
                UseItemButtonX = UI.transform.GetChild(7).gameObject.transform.GetChild(2).GetComponent<Button>();
                RestoreButtonX = UI.transform.GetChild(8).gameObject.transform.GetChild(2).GetComponent<Button>();
                InteractButtonX = UI.transform.GetChild(9).gameObject.transform.GetChild(2).GetComponent<Button>();
                ShootButtonX = UI.transform.GetChild(10).gameObject.transform.GetChild(2).GetComponent<Button>();
                BlockButtonX = UI.transform.GetChild(11).gameObject.transform.GetChild(2).GetComponent<Button>();
                ChangeItemButtonX = UI.transform.GetChild(12).gameObject.transform.GetChild(2).GetComponent<Button>();
            }
            else
            {
                UI = UI.transform.GetChild(6).gameObject;
                ReChooseNotice = UI.transform.GetChild(15).gameObject;
                InputNotice = UI.transform.GetChild(14).gameObject;
                CurrentRightMoveText = UI.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentLeftMoveText = UI.transform.GetChild(2).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentJumpText = UI.transform.GetChild(3).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentNormalAtkText = UI.transform.GetChild(4).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentStrongAtkText = UI.transform.GetChild(5).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentDashText = UI.transform.GetChild(6).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentUseItemText = UI.transform.GetChild(7).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentRestoreText = UI.transform.GetChild(8).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentInteractText = UI.transform.GetChild(9).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentShootText = UI.transform.GetChild(10).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentBlockText = UI.transform.GetChild(11).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentChangeItemText = UI.transform.GetChild(12).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentItemWindowText = UI.transform.GetChild(13).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Text>();
                RightMoveButton = UI.transform.GetChild(1).gameObject.transform.GetChild(1).GetComponent<Button>();
                LeftMoveButton = UI.transform.GetChild(2).gameObject.transform.GetChild(1).GetComponent<Button>();
                JumpButton = UI.transform.GetChild(3).gameObject.transform.GetChild(1).GetComponent<Button>();
                NormalAtkButton = UI.transform.GetChild(4).gameObject.transform.GetChild(1).GetComponent<Button>();
                StrongAtkButton = UI.transform.GetChild(5).gameObject.transform.GetChild(1).GetComponent<Button>();
                DashButton = UI.transform.GetChild(6).gameObject.transform.GetChild(1).GetComponent<Button>();
                UseItemButton = UI.transform.GetChild(7).gameObject.transform.GetChild(1).GetComponent<Button>();
                RestoreButton = UI.transform.GetChild(8).gameObject.transform.GetChild(1).GetComponent<Button>();
                InteractButton = UI.transform.GetChild(9).gameObject.transform.GetChild(1).GetComponent<Button>();
                ShootButton = UI.transform.GetChild(10).gameObject.transform.GetChild(1).GetComponent<Button>();
                BlockButton = UI.transform.GetChild(11).gameObject.transform.GetChild(1).GetComponent<Button>();
                ChangeItemButton = UI.transform.GetChild(12).gameObject.transform.GetChild(1).GetComponent<Button>();
                ItemWindowButton = UI.transform.GetChild(13).gameObject.transform.GetChild(1).GetComponent<Button>();
                ResetButton = UI.transform.GetChild(17).GetComponent<Button>();

                CurrentJumpTextX = UI.transform.GetChild(3).gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentNormalAtkTextX = UI.transform.GetChild(4).gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentStrongAtkTextX = UI.transform.GetChild(5).gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentDashTextX = UI.transform.GetChild(6).gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentUseItemTextX = UI.transform.GetChild(7).gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentRestoreTextX = UI.transform.GetChild(8).gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentInteractTextX = UI.transform.GetChild(9).gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentShootTextX = UI.transform.GetChild(10).gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentBlockTextX = UI.transform.GetChild(11).gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).GetComponent<Text>();
                CurrentChangeItemTextX = UI.transform.GetChild(12).gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).GetComponent<Text>();
                JumpButtonX = UI.transform.GetChild(3).gameObject.transform.GetChild(2).GetComponent<Button>();
                NormalAtkButtonX = UI.transform.GetChild(4).gameObject.transform.GetChild(2).GetComponent<Button>();
                StrongAtkButtonX = UI.transform.GetChild(5).gameObject.transform.GetChild(2).GetComponent<Button>();
                DashButtonX = UI.transform.GetChild(6).gameObject.transform.GetChild(2).GetComponent<Button>();
                UseItemButtonX = UI.transform.GetChild(7).gameObject.transform.GetChild(2).GetComponent<Button>();
                RestoreButtonX = UI.transform.GetChild(8).gameObject.transform.GetChild(2).GetComponent<Button>();
                InteractButtonX = UI.transform.GetChild(9).gameObject.transform.GetChild(2).GetComponent<Button>();
                ShootButtonX = UI.transform.GetChild(10).gameObject.transform.GetChild(2).GetComponent<Button>();
                BlockButtonX = UI.transform.GetChild(11).gameObject.transform.GetChild(2).GetComponent<Button>();
                ChangeItemButtonX = UI.transform.GetChild(12).gameObject.transform.GetChild(2).GetComponent<Button>();
            }

            NowDefaultButton = UI.GetComponent<DefaultButton>();
        }
        
        ResetButton.onClick.AddListener(ReSetKeyCode);

        _manager.UnValidInputNotice += UnValidInputNotice;
        _manager.ValidInputNotice += InputEnd;
        _manager.ChangeTextNotice += ChangeButtonText;
    }

    void Update()
    {
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
    }

    private void InisializeDictionary()
    {
        Buttons.Add("LeftMove", LeftMoveButton);
        Buttons.Add("RightMove", RightMoveButton);
        Buttons.Add("NormalAtk", NormalAtkButton);
        Buttons.Add("StrongAtk", StrongAtkButton);
        Buttons.Add("Jump", JumpButton);
        Buttons.Add("Restore", RestoreButton);
        Buttons.Add("UseItem", UseItemButton);
        Buttons.Add("Interact", InteractButton);
        Buttons.Add("Dash", DashButton);
        Buttons.Add("Shoot", ShootButton);
        Buttons.Add("Block", BlockButton);
        Buttons.Add("ItemWindow", ItemWindowButton);
        Buttons.Add("ChangeItem", ChangeItemButton);

        Buttons.Add("NormalAtkX", NormalAtkButtonX);
        Buttons.Add("StrongAtkX", StrongAtkButtonX);
        Buttons.Add("JumpX", JumpButtonX);
        Buttons.Add("RestoreX", RestoreButtonX);
        Buttons.Add("UseItemX", UseItemButtonX);
        Buttons.Add("InteractX", InteractButtonX);
        Buttons.Add("DashX", DashButtonX);
        Buttons.Add("ShootX", ShootButtonX);
        Buttons.Add("BlockX", BlockButtonX);
        Buttons.Add("ChangeItemX", ChangeItemButtonX);

        KeyboardButtonTexts.Add(PlayerCommandManager.Command.LeftMove, CurrentLeftMoveText);
        KeyboardButtonTexts.Add(PlayerCommandManager.Command.RightMove, CurrentRightMoveText);
        KeyboardButtonTexts.Add(PlayerCommandManager.Command.NormalAtk, CurrentNormalAtkText);
        KeyboardButtonTexts.Add(PlayerCommandManager.Command.StrongAtk, CurrentStrongAtkText);
        KeyboardButtonTexts.Add(PlayerCommandManager.Command.Jump, CurrentJumpText);
        KeyboardButtonTexts.Add(PlayerCommandManager.Command.Restore, CurrentJumpText);
        KeyboardButtonTexts.Add(PlayerCommandManager.Command.UseItem, CurrentUseItemText);
        KeyboardButtonTexts.Add(PlayerCommandManager.Command.Interact, CurrentInteractText);
        KeyboardButtonTexts.Add(PlayerCommandManager.Command.Dash, CurrentDashText);
        KeyboardButtonTexts.Add(PlayerCommandManager.Command.Shoot, CurrentShootText);
        KeyboardButtonTexts.Add(PlayerCommandManager.Command.Block, CurrentBlockText);
        KeyboardButtonTexts.Add(PlayerCommandManager.Command.ItemWindow, CurrentItemWindowText);
        KeyboardButtonTexts.Add(PlayerCommandManager.Command.ChangeItem, CurrentChangeItemText);

        XboxButtonTexts.Add(PlayerCommandManager.Command.NormalAtk, CurrentNormalAtkTextX);
        XboxButtonTexts.Add(PlayerCommandManager.Command.StrongAtk, CurrentStrongAtkTextX);
        XboxButtonTexts.Add(PlayerCommandManager.Command.Jump, CurrentJumpTextX);
        XboxButtonTexts.Add(PlayerCommandManager.Command.Restore, CurrentRestoreTextX);
        XboxButtonTexts.Add(PlayerCommandManager.Command.UseItem, CurrentUseItemTextX);
        XboxButtonTexts.Add(PlayerCommandManager.Command.Interact, CurrentInteractTextX);
        XboxButtonTexts.Add(PlayerCommandManager.Command.Dash, CurrentDashTextX);
        XboxButtonTexts.Add(PlayerCommandManager.Command.Shoot, CurrentShootTextX);
        XboxButtonTexts.Add(PlayerCommandManager.Command.Block, CurrentBlockTextX);
        XboxButtonTexts.Add(PlayerCommandManager.Command.ChangeItem, CurrentChangeItemTextX);
    }

    public void BeginChangeKeyCode(string target)
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            _manager.isSelectKeyboard = true;

            isInputOpen = true;
            InputNotice.SetActive(true);
            _manager.SetChosenType(target);

            NowDefaultButton.onStart = Buttons[target];

            SelectButtonController.CloseSelectButtonController();
            AllKeyButtonFalse();
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            _manager.isSelectXbox = true;

            isInputOpenX = true;
            InputNotice.SetActive(true);
            _manager.SetChosenType(target);

            NowDefaultButton.onStart = Buttons[target + "X"];

            SelectButtonController.CloseSelectButtonController();
            AllKeyButtonFalse();
        }
    }

    private void ChangeButtonText(PlayerCommandManager.Command Command, string NewText)
    {
        if (_manager.isSelectKeyboard)
        {
            KeyboardButtonTexts[Command].text = NewText;
        }
        if (_manager.isSelectXbox)
        {
            XboxButtonTexts[Command].text = NewText;
        }
    }

    private void UnValidInputNotice()
    {
        ReChooseNotice.SetActive(true);
    }

    public void InputEnd()
    {
        if (_manager.isSelectKeyboard)
        {
            _manager.isSelectKeyboard = false;
            isInputOpen = false;
        }
        if (_manager.isSelectXbox)
        {
            _manager.isSelectXbox = false;
            isInputOpenX = false;
        }

        BeginCoolDown = true;
        InputNotice.SetActive(false);
        SelectButtonController.OpenSelectButtonController();
        AllKeyButtonTrue();
    }

    private void ReSetKeyCode()
    {
        isAlert = false;

        _manager.ReSetKeyCode();

        CurrentRightMoveText.text = "RightArrow";
        CurrentLeftMoveText.text = "LeftArrow";
        CurrentJumpText.text = "Z";
        CurrentNormalAtkText.text = "X";
        CurrentStrongAtkText.text = "C";
        CurrentDashText.text = "Space";
        CurrentUseItemText.text = "A";
        CurrentRestoreText.text = "Q";
        CurrentInteractText.text = "UpArrow";
        CurrentShootText.text = "S";
        CurrentBlockText.text = "LeftControl";
        CurrentChangeItemText.text = "D";
        CurrentItemWindowText.text = "E";

        CurrentJumpTextX.text = "A";
        CurrentNormalAtkTextX.text = "RB";
        CurrentStrongAtkTextX.text = "RT";
        CurrentDashTextX.text = "X";
        CurrentUseItemTextX.text = "LT";
        CurrentRestoreTextX.text = "UpArrow";
        CurrentInteractTextX.text = "B";
        CurrentShootTextX.text = "Y";
        CurrentBlockTextX.text = "LB";
        CurrentChangeItemTextX.text = "RightArrow";
    }//Button專用

    private void AllKeyButtonFalse()
    {
        RightMoveButton.interactable = false;
        LeftMoveButton.interactable = false;
        JumpButton.interactable = false;
        NormalAtkButton.interactable = false;
        StrongAtkButton.interactable = false;
        DashButton.interactable = false;
        UseItemButton.interactable = false;
        RestoreButton.interactable = false;
        InteractButton.interactable = false;
        ShootButton.interactable = false;
        BlockButton.interactable = false;
        ChangeItemButton.interactable = false;
        ItemWindowButton.interactable = false;
        ResetButton.interactable = false;
        JumpButtonX.interactable = false;
        NormalAtkButtonX.interactable = false;
        StrongAtkButtonX.interactable = false;
        DashButtonX.interactable = false;
        UseItemButtonX.interactable = false;
        RestoreButtonX.interactable = false;
        InteractButtonX.interactable = false;
        ShootButtonX.interactable = false;
        BlockButtonX.interactable = false;
        ChangeItemButtonX.interactable = false;
    }

    private void AllKeyButtonTrue()
    {
        RightMoveButton.interactable = true;
        LeftMoveButton.interactable = true;
        JumpButton.interactable = true;
        NormalAtkButton.interactable = true;
        StrongAtkButton.interactable = true;
        DashButton.interactable = true;
        UseItemButton.interactable = true;
        RestoreButton.interactable = true;
        InteractButton.interactable = true;
        ShootButton.interactable = true;
        BlockButton.interactable = true;
        ChangeItemButton.interactable = true;
        ItemWindowButton.interactable = true;
        ResetButton.interactable = true;
        JumpButtonX.interactable = true;
        NormalAtkButtonX.interactable = true;
        StrongAtkButtonX.interactable = true;
        DashButtonX.interactable = true;
        UseItemButtonX.interactable = true;
        RestoreButtonX.interactable = true;
        InteractButtonX.interactable = true;
        ShootButtonX.interactable = true;
        BlockButtonX.interactable = true;
        ChangeItemButtonX.interactable = true;
    }
}
