using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerCommandManager;

public class NewKeyCodeManager
{
    private Command NowChosenCommand;

    public event Action ValidInputNotice;
    public event Action UnValidInputNotice;
    public event Action LackKeyCodeNotice;
    public event Action<Command, string> ChangeTextNotice;

    public Dictionary<Command, KeyCode> KeyboardKeyCodes = new Dictionary<Command, KeyCode>();
    public Dictionary<Command, int> XboxKeyCodes = new Dictionary<Command, int>();

    private Dictionary<string, Command> CommandsNameTable = new Dictionary<string, Command>();

    [HideInInspector] public bool isSelectKeyboard;//其他Script會用到(PauseMenuController)
    [HideInInspector] public bool isSelectXbox;//其他Script會用到(PauseMenuController)

    //手把
    [HideInInspector] public bool LeftMovePressing;
    [HideInInspector] public bool LeftMoveUp;
    [HideInInspector] public bool RightMovePressing;
    [HideInInspector] public bool RightMoveUp;
    [HideInInspector] public bool JumpPressed;
    [HideInInspector] public bool JumpUp;
    [HideInInspector] public bool NormalAtkPressing;
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
    [HideInInspector] public bool ItemWindowPressed;
    [HideInInspector] public bool ChangeUseItemPressed;

    public NewKeyCodeManager()
    {
        inisializeDictionary();
    }

    private void inisializeDictionary()
    {
        CommandsNameTable.Add("LeftMove", Command.LeftMove);
        CommandsNameTable.Add("RightMove", Command.RightMove);
        CommandsNameTable.Add("NormalAtk", Command.NormalAtk);
        CommandsNameTable.Add("StrongAtk", Command.StrongAtk);
        CommandsNameTable.Add("Jump", Command.Jump);
        CommandsNameTable.Add("Restore", Command.Restore);
        CommandsNameTable.Add("UseItem", Command.UseItem);
        CommandsNameTable.Add("Interact", Command.Interact);
        CommandsNameTable.Add("Dash", Command.Dash);
        CommandsNameTable.Add("Shoot", Command.Shoot);
        CommandsNameTable.Add("Block", Command.Block);
        CommandsNameTable.Add("ItemWindow", Command.ItemWindow);
        CommandsNameTable.Add("ChangeItem", Command.ChangeItem);

        KeyboardKeyCodes.Add(Command.LeftMove, KeyCode.LeftArrow);
        KeyboardKeyCodes.Add(Command.RightMove, KeyCode.RightArrow);
        KeyboardKeyCodes.Add(Command.NormalAtk, KeyCode.X);
        KeyboardKeyCodes.Add(Command.StrongAtk, KeyCode.C);
        KeyboardKeyCodes.Add(Command.Jump, KeyCode.Z);
        KeyboardKeyCodes.Add(Command.Restore, KeyCode.Q);
        KeyboardKeyCodes.Add(Command.UseItem, KeyCode.A);
        KeyboardKeyCodes.Add(Command.Interact, KeyCode.UpArrow);
        KeyboardKeyCodes.Add(Command.Dash, KeyCode.Space);
        KeyboardKeyCodes.Add(Command.Shoot, KeyCode.S);
        KeyboardKeyCodes.Add(Command.Block, KeyCode.LeftControl);
        KeyboardKeyCodes.Add(Command.ItemWindow, KeyCode.E);
        KeyboardKeyCodes.Add(Command.ChangeItem, KeyCode.D);

        //數字是xbox按鍵的編號
        XboxKeyCodes.Add(Command.LeftMove, 14);
        XboxKeyCodes.Add(Command.RightMove, 15);
        XboxKeyCodes.Add(Command.NormalAtk, 6);
        XboxKeyCodes.Add(Command.StrongAtk, 8);
        XboxKeyCodes.Add(Command.Jump, 1);
        XboxKeyCodes.Add(Command.Restore, 9);
        XboxKeyCodes.Add(Command.UseItem, 7);
        XboxKeyCodes.Add(Command.Interact, 2);
        XboxKeyCodes.Add(Command.Dash, 3);
        XboxKeyCodes.Add(Command.Shoot, 4);
        XboxKeyCodes.Add(Command.Block, 5);
        XboxKeyCodes.Add(Command.ItemWindow, 13);
        XboxKeyCodes.Add(Command.ChangeItem, 10);
    }

    private bool ChangeKeyCode(Command type, KeyCode InputCode)
    {
        if (InputCode == KeyCode.None)
        {
            return false;
        }

        var RepectCommand = DetectRepect(InputCode, KeyboardKeyCodes);
        if (RepectCommand != Command.Null)
        {
            KeyboardKeyCodes[RepectCommand] = KeyCode.None;
            ChangeTextNotice.Invoke(RepectCommand, "");
        }

        KeyboardKeyCodes[type] = InputCode;

        return true;
    }
    private bool ChangeKeyCode(Command type, int InputCode)
    {
        if (InputCode == 0)
        {
            return false;
        }

        var RepectCommand = DetectRepect(InputCode, XboxKeyCodes);
        if (RepectCommand != Command.Null)
        {
            XboxKeyCodes[RepectCommand] = 0;
            ChangeTextNotice.Invoke(RepectCommand, "");
        }

        XboxKeyCodes[type] = InputCode;

        return true;
    }

    public void SetChosenType(string type)
    {
        NowChosenCommand = CommandsNameTable[type];
    }

    private Command DetectRepect(KeyCode NewCode ,Dictionary<Command, KeyCode> dictionary)
    {
        foreach (var item in dictionary)
        {
            if(NewCode == item.Value)
            {
                return item.Key;
            }
        }

        return Command.Null;
    }
    private Command DetectRepect(int NewCode, Dictionary<Command, int> dictionary)
    {
        foreach (var item in dictionary)
        {
            if (NewCode == item.Value)
            {
                return item.Key;
            }
        }

        return Command.Null;
    }
    public void DetectLack()
    {
        foreach (var item in KeyboardKeyCodes)
        {
            if (item.Value == KeyCode.None)
            {
                LackKeyCodeNotice?.Invoke();
                return;
            }
        }
        foreach (var item in XboxKeyCodes)
        {
            if (item.Value == 0)
            {
                LackKeyCodeNotice?.Invoke();
                return;
            }
        }
    }

    public void KeyboardCommandReceive()
    {
        if (isSelectKeyboard && Input.anyKeyDown)
        {
            var inputCode = InputSelect();

            if (ChangeKeyCode(NowChosenCommand, inputCode.Item1))
            {
                ValidInputNotice?.Invoke();
                ChangeTextNotice?.Invoke(NowChosenCommand, inputCode.Item2);
            }
            else
            {
                UnValidInputNotice?.Invoke();
            }
        }
    }
    private (KeyCode, string) InputSelect()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            return (KeyCode.A, "A");
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            return (KeyCode.B, "B");
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            return (KeyCode.C, "C");
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            return (KeyCode.D, "D");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            return (KeyCode.E, "E");
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            return (KeyCode.F, "F");
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            return (KeyCode.G, "G");
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            return (KeyCode.H, "H");
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            return (KeyCode.I, "I");
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            return (KeyCode.J, "J");
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            return (KeyCode.K, "K");
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            return (KeyCode.L, "L");
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            return (KeyCode.M, "M");
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            return (KeyCode.N, "N");
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            return (KeyCode.O, "O");
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            return (KeyCode.P, "P");
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            return (KeyCode.Q, "Q");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            return (KeyCode.R, "R");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            return (KeyCode.S, "S");
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            return (KeyCode.T, "T");
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            return (KeyCode.U, "U");
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            return (KeyCode.V, "V");
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            return (KeyCode.W, "W");
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            return (KeyCode.X, "X");
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            return (KeyCode.Y, "Y");
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            return (KeyCode.Z, "Z");
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            return (KeyCode.Space, "Space");
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            return (KeyCode.RightArrow, "RightArrow");
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            return (KeyCode.LeftArrow, "LeftArrow");
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            return (KeyCode.UpArrow, "UpArrow");
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            return (KeyCode.DownArrow, "DownArrow");
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            return (KeyCode.LeftControl, "LeftControl");
        }
        if (Input.GetKeyDown(KeyCode.RightControl))
        {
            return (KeyCode.RightControl, "RightControl");
        }
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            return (KeyCode.RightShift, "RightShift");
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            return (KeyCode.LeftShift, "LeftShift");
        }
        if (Input.GetKeyDown(KeyCode.Comma))
        {
            return (KeyCode.Comma, ",");
        }
        if (Input.GetKeyDown(KeyCode.Period))
        {
            return (KeyCode.Period, ".");
        }
        if (Input.GetKeyDown(KeyCode.Slash))
        {
            return (KeyCode.Slash, "/");
        }
        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            return (KeyCode.LeftBracket, "[");
        }
        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            return (KeyCode.RightBracket, "]");
        }
        if (Input.GetKeyDown(KeyCode.Semicolon))    
        {
            return (KeyCode.Semicolon, ";");
        }
        if (Input.GetKeyDown(KeyCode.Quote))
        {
            return (KeyCode.Quote, "'");
        }

        return (KeyCode.None, "");
    }

    public void XboxCommandReceive(XboxCommandReceiver recevier)
    {
        if (!isSelectXbox)
        {
            XboxActionJudge(recevier);
        }
        else if(recevier.isAnyButtonPressed)
        {
            var inputCode = InputSelectC(recevier);

            if (ChangeKeyCode(NowChosenCommand, inputCode.Item1))
            {
                ValidInputNotice?.Invoke();
                ChangeTextNotice?.Invoke(NowChosenCommand, inputCode.Item2);
            }
            else
            {
                UnValidInputNotice?.Invoke();
            }
        }
    }
    private (int, string) InputSelectC(XboxCommandReceiver recevier)
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            return (1, "A");
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            return (2, "B");
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton2))
        {
            return (3, "X");
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton3))
        {
            return (4, "Y");
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton4))
        {
            return (5, "LB");
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton5))
        {
            return (6, "RB");
        }
        if (recevier.isLTPressed)
        {
            return (7, "LT");
        }
        if (recevier.isRTPressed)
        {
            return (8, "RT");
        }
        if (recevier.isCrossUpPressed)
        {
            return (9, "CrossUp");
        }
        if (recevier.isCrossRightPressed)
        {
            return (10, "CrossRight");
        }
        if (recevier.isCrossDownPressed)
        {
            return (11, "CrossDown");
        }
        if (recevier.isCrossLeftPressed)
        {
            return (12, "CrossLeft");
        }

        return (0, "");
    }
    private void XboxActionJudge(XboxCommandReceiver recevier)
    {
        int Pressed = 0;
        int Pressing = 0;
        int Up = 0;
        int PressedB = 0;
        int PressingB = 0;
        int UpB = 0;
        if (Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            if (Pressed == 0)
            {
                Pressed = 1;
            }
            else
            {
                PressedB = 1;
            }
        }
        if (Input.GetKey(KeyCode.JoystickButton0))
        {
            if (Pressing == 0)
            {
                Pressing = 1;
            }
            else
            {
                PressingB = 1;
            }
        }
        if (Input.GetKeyUp(KeyCode.JoystickButton0))
        {
            if (Up == 0)
            {
                Up = 1;
            }
            else
            {
                UpB = 1;
            }
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            if (Pressed == 0)
            {
                Pressed = 2;
            }
            else
            {
                PressedB = 2;
            }
        }
        if (Input.GetKey(KeyCode.JoystickButton1))
        {
            if (Pressing == 0)
            {
                Pressing = 2;
            }
            else
            {
                PressingB = 2;
            }
        }
        if (Input.GetKeyUp(KeyCode.JoystickButton1))
        {
            if (Up == 0)
            {
                Up = 2;
            }
            else
            {
                UpB = 2;
            }
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton2))
        {
            if (Pressed == 0)
            {
                Pressed = 3;
            }
            else
            {
                PressedB = 3;
            }
        }
        if (Input.GetKey(KeyCode.JoystickButton2))
        {
            if (Pressing == 0)
            {
                Pressing = 3;
            }
            else
            {
                PressingB = 3;
            }
        }
        if (Input.GetKeyUp(KeyCode.JoystickButton2))
        {
            if (Up == 0)
            {
                Up = 3;
            }
            else
            {
                UpB = 3;
            }
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton3))
        {
            if (Pressed == 0)
            {
                Pressed = 4;
            }
            else
            {
                PressedB = 4;
            }
        }
        if (Input.GetKey(KeyCode.JoystickButton3))
        {
            if (Pressing == 0)
            {
                Pressing = 4;
            }
            else
            {
                PressingB = 4;
            }
        }
        if (Input.GetKeyUp(KeyCode.JoystickButton3))
        {
            if (Up == 0)
            {
                Up = 4;
            }
            else
            {
                UpB = 4;
            }
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton4))
        {
            if (Pressed == 0)
            {
                Pressed = 5;
            }
            else
            {
                PressedB = 5;
            }
        }
        if (Input.GetKey(KeyCode.JoystickButton4))
        {
            if (Pressing == 0)
            {
                Pressing = 5;
            }
            else
            {
                PressingB = 5;
            }
        }
        if (Input.GetKeyUp(KeyCode.JoystickButton4))
        {
            if (Up == 0)
            {
                Up = 5;
            }
            else
            {
                UpB = 5;
            }
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton5))
        {
            if (Pressed == 0)
            {
                Pressed = 6;
            }
            else
            {
                PressedB = 6;
            }
        }
        if (Input.GetKey(KeyCode.JoystickButton5))
        {
            if (Pressing == 0)
            {
                Pressing = 6;
            }
            else
            {
                PressingB = 6;
            }
        }
        if (Input.GetKeyUp(KeyCode.JoystickButton5))
        {
            if (Up == 0)
            {
                Up = 6;
            }
            else
            {
                UpB = 6;
            }
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton6))
        {
            if (Pressed == 0)
            {
                Pressed = 13;
            }
            else
            {
                PressedB = 13;
            }
        }

        if (recevier.isLeftStickLeftPushing)
        {
            if (Pressing == 0)
            {
                Pressing = 14;
            }
            else
            {
                PressingB = 14;
            }
        }
        if (recevier.isLeftStickLeftRelease)
        {
            if (Up == 0)
            {
                Up = 14;
            }
            else
            {
                UpB = 14;
            }
        }
        if (recevier.isLeftStickRightPushing)
        {
            if (Pressing == 0)
            {
                Pressing = 15;
            }
            else
            {
                PressingB = 15;
            }
        }
        if (recevier.isLeftStickRightRelease)
        {
            if (Up == 0)
            {
                Up = 15;
            }
            else
            {
                UpB = 15;
            }
        }
        if (recevier.isLTPressed)
        {
            if (Pressed == 0)
            {
                Pressed = 7;
            }
            else
            {
                PressedB = 7;
            }
        }
        if (recevier.isLTPressing)
        {
            if (Pressing == 0)
            {
                Pressing = 7;
            }
            else
            {
                PressingB = 7;
            }
        }
        if (recevier.isLTUp)
        {
            if (Up == 0)
            {
                Up = 7;
            }
            else
            {
                UpB = 7;
            }
        }
        if (recevier.isRTPressed)
        {
            if (Pressed == 0)
            {
                Pressed = 8;
            }
            else
            {
                PressedB = 8;
            }
        }
        if (recevier.isRTPressing)
        {
            if (Pressing == 0)
            {
                Pressing = 8;
            }
            else
            {
                PressingB = 8;
            }
        }
        if (recevier.isRTUp)
        {
            if (Up == 0)
            {
                Up = 8;
            }
            else
            {
                UpB = 8;
            }
        }
        if (recevier.isCrossUpPressed)
        {
            if (Pressed == 0)
            {
                Pressed = 9;
            }
            else
            {
                PressedB = 9;
            }
        }
        if (recevier.isCrossDownPressing)
        {
            if (Pressing == 0)
            {
                Pressing = 9;
            }
            else
            {
                PressingB = 9;
            }
        }
        if (recevier.isCrossUpUp)
        {
            if (Up == 0)
            {
                Up = 9;
            }
            else
            {
                UpB = 9;
            }
        }
        if (recevier.isCrossRightPressed)
        {
            if (Pressed == 0)
            {
                Pressed = 10;
            }
            else
            {
                PressedB = 10;
            }
        }
        if (recevier.isCrossRightPressing)
        {
            if (Pressing == 0)
            {
                Pressing = 10;
            }
            else
            {
                PressingB = 10;
            }
        }
        if (recevier.isCrossRightUp)
        {
            if (Up == 0)
            {
                Up = 10;
            }
            else
            {
                UpB = 10;
            }
        }
        if (recevier.isCrossDownPressed)
        {
            if (Pressed == 0)
            {
                Pressed = 11;
            }
            else
            {
                PressedB = 11;
            }
        }
        if (recevier.isCrossDownPressing)
        {
            if (Pressing == 0)
            {
                Pressing = 11;
            }
            else
            {
                PressingB = 11;
            }
        }
        if (recevier.isCrossDownUp)
        {
            if (Up == 0)
            {
                Up = 11;
            }
            else
            {
                UpB = 11;
            }
        }
        if (recevier.isCrossLeftPressed)
        {
            if (Pressed == 0)
            {
                Pressed = 12;
            }
            else
            {
                PressedB = 12;
            }
        }
        if (recevier.isCrossLeftPressing)
        {
            if (Pressing == 0)
            {
                Pressing = 12;
            }
            else
            {
                PressingB = 12;
            }
        }
        if (recevier.isCrossLeftUp)
        {
            if (Up == 0)
            {
                Up = 12;
            }
            else
            {
                UpB = 12;
            }
        }

        if (XboxKeyCodes[Command.LeftMove] == Pressing || XboxKeyCodes[Command.LeftMove] == PressingB)
        {
            LeftMovePressing = true;
        }
        if (XboxKeyCodes[Command.LeftMove] == Up || XboxKeyCodes[Command.LeftMove] == UpB)
        {
            LeftMoveUp = true;
        }

        if (XboxKeyCodes[Command.RightMove] == Pressing || XboxKeyCodes[Command.RightMove] == PressingB)
        {
            RightMovePressing = true;
        }
        if (XboxKeyCodes[Command.RightMove] == Up || XboxKeyCodes[Command.RightMove] == UpB)
        {
            RightMoveUp = true;
        }

        if (XboxKeyCodes[Command.Jump] == Pressed || XboxKeyCodes[Command.Jump] == PressedB)
        {
            JumpPressed = true;
        }
        if (XboxKeyCodes[Command.Jump] == Up || XboxKeyCodes[Command.Jump] == UpB)
        {
            JumpUp = true;
        }

        if (XboxKeyCodes[Command.NormalAtk] == Pressing || XboxKeyCodes[Command.NormalAtk] == PressingB)
        {
            NormalAtkPressing = true;
        }
        if (XboxKeyCodes[Command.NormalAtk] == Up || XboxKeyCodes[Command.NormalAtk] == UpB)
        {
            NormalAtkUp = true;
        }

        if (XboxKeyCodes[Command.StrongAtk] == Pressed || XboxKeyCodes[Command.StrongAtk] == PressedB)
        {
            StrongAtkPressed = true;
        }

        if (XboxKeyCodes[Command.Dash] == Pressed || XboxKeyCodes[Command.Dash] == PressedB)
        {
            DashPressed = true;
        }

        if (XboxKeyCodes[Command.UseItem] == Pressed || XboxKeyCodes[Command.UseItem] == PressedB)
        {
            UseItemPressed = true;
        }
        if (XboxKeyCodes[Command.UseItem] == Up || XboxKeyCodes[Command.UseItem] == UpB)
        {
            UseItemUp = true;
        }

        if (XboxKeyCodes[Command.Restore] == Pressed || XboxKeyCodes[Command.Restore] == PressedB)
        {
            RestorePressed = true;
        }

        if (XboxKeyCodes[Command.Interact] == Pressed || XboxKeyCodes[Command.Interact] == PressedB)
        {
            InteractPressed = true;
        }
        if (XboxKeyCodes[Command.Interact] == Up || XboxKeyCodes[Command.Interact] == UpB)
        {
            InteractUp = true;
        }

        if (XboxKeyCodes[Command.Shoot] == Pressed || XboxKeyCodes[Command.Shoot] == PressedB)
        {
            ShootPressed = true;
        }
        if (XboxKeyCodes[Command.Shoot] == Up || XboxKeyCodes[Command.Shoot] == UpB)
        {
            ShootUp = true;
        }

        if (XboxKeyCodes[Command.Block] == Pressed || XboxKeyCodes[Command.Block] == PressedB)
        {
            BlockPressed = true;
        }

        if (XboxKeyCodes[Command.ChangeItem] == Pressed || XboxKeyCodes[Command.ChangeItem] == PressedB)
        {
            ChangeUseItemPressed = true;
        }

        if (XboxKeyCodes[Command.ItemWindow] == Pressed || XboxKeyCodes[Command.ItemWindow] == PressedB)
        {
            ItemWindowPressed = true;
        }
    }

    public void ReSetKeyCode()
    {
        KeyboardKeyCodes[Command.LeftMove] = KeyCode.LeftArrow;
        KeyboardKeyCodes[Command.RightMove] = KeyCode.RightArrow;
        KeyboardKeyCodes[Command.NormalAtk] = KeyCode.X;
        KeyboardKeyCodes[Command.StrongAtk] = KeyCode.C;
        KeyboardKeyCodes[Command.Jump] = KeyCode.Z;
        KeyboardKeyCodes[Command.Restore] = KeyCode.Q;
        KeyboardKeyCodes[Command.UseItem] = KeyCode.A;
        KeyboardKeyCodes[Command.Interact] = KeyCode.UpArrow;
        KeyboardKeyCodes[Command.Dash] = KeyCode.Space;
        KeyboardKeyCodes[Command.Shoot] = KeyCode.S;
        KeyboardKeyCodes[Command.Block] = KeyCode.LeftControl;
        KeyboardKeyCodes[Command.ItemWindow] = KeyCode.E;
        KeyboardKeyCodes[Command.ChangeItem] = KeyCode.D;

        XboxKeyCodes[Command.NormalAtk] = 6;
        XboxKeyCodes[Command.StrongAtk] = 8;
        XboxKeyCodes[Command.Jump] = 1;
        XboxKeyCodes[Command.Restore] = 9;
        XboxKeyCodes[Command.UseItem] = 7;
        XboxKeyCodes[Command.Interact] = 2;
        XboxKeyCodes[Command.Dash] = 3;
        XboxKeyCodes[Command.Shoot] = 4;
        XboxKeyCodes[Command.Block] = 5;
        XboxKeyCodes[Command.ChangeItem] = 10;
    }
}
