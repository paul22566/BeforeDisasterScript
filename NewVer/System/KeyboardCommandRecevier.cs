using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardCommandRecevier : IInputSource
{
    public bool IsLeftMovePressing(NewKeyCodeManager manager)
    {
        return Input.GetKey(manager.KeyboardKeyCodes[PlayerCommandManager.Command.LeftMove]);
    }
    public bool IsLeftMoveUp(NewKeyCodeManager manager)
    {
        return Input.GetKeyUp(manager.KeyboardKeyCodes[PlayerCommandManager.Command.LeftMove]);
    }
    public bool IsRightMovePressing(NewKeyCodeManager manager)
    {
        return Input.GetKey(manager.KeyboardKeyCodes[PlayerCommandManager.Command.RightMove]);
    }
    public bool IsRightMoveUp(NewKeyCodeManager manager)
    {
        return Input.GetKeyUp(manager.KeyboardKeyCodes[PlayerCommandManager.Command.RightMove]);
    }
    public bool IsNormalAttackPressing(NewKeyCodeManager manager)
    {
        return Input.GetKey(manager.KeyboardKeyCodes[PlayerCommandManager.Command.NormalAtk]);
    }
    public bool IsNormalAttackUp(NewKeyCodeManager manager)
    {
        return Input.GetKeyUp(manager.KeyboardKeyCodes[PlayerCommandManager.Command.NormalAtk]);
    }
    public bool IsStrongAttackPressed(NewKeyCodeManager manager)
    {
        return Input.GetKeyDown(manager.KeyboardKeyCodes[PlayerCommandManager.Command.StrongAtk]);
    }
    public bool IsJumpPressed(NewKeyCodeManager manager)
    {
        return Input.GetKeyDown(manager.KeyboardKeyCodes[PlayerCommandManager.Command.Jump]);
    }
    public bool IsJumpUp(NewKeyCodeManager manager)
    {
        return Input.GetKeyUp(manager.KeyboardKeyCodes[PlayerCommandManager.Command.Jump]);
    }
    public bool IsDashPressed(NewKeyCodeManager manager)
    {
        return Input.GetKeyDown(manager.KeyboardKeyCodes[PlayerCommandManager.Command.Dash]);
    }
    public bool IsRestorePressed(NewKeyCodeManager manager)
    {
        return Input.GetKeyDown(manager.KeyboardKeyCodes[PlayerCommandManager.Command.Restore]);
    }
    public bool IsUseItemPressed(NewKeyCodeManager manager)
    {
        return Input.GetKeyDown(manager.KeyboardKeyCodes[PlayerCommandManager.Command.UseItem]);
    }
    public bool IsUseItemUp(NewKeyCodeManager manager)
    {
        return Input.GetKeyUp(manager.KeyboardKeyCodes[PlayerCommandManager.Command.UseItem]);
    }
    public bool IsInteractPressed(NewKeyCodeManager manager)
    {
        return Input.GetKeyDown(manager.KeyboardKeyCodes[PlayerCommandManager.Command.Interact]);
    }
    public bool IsInteractUp(NewKeyCodeManager manager)
    {
        return Input.GetKeyUp(manager.KeyboardKeyCodes[PlayerCommandManager.Command.Interact]);
    }
    public bool IsShootPressed(NewKeyCodeManager manager)
    {
        return Input.GetKeyDown(manager.KeyboardKeyCodes[PlayerCommandManager.Command.Shoot]);
    }
    public bool IsShootUp(NewKeyCodeManager manager)
    {
        return Input.GetKeyUp(manager.KeyboardKeyCodes[PlayerCommandManager.Command.Shoot]);
    }
    public bool IsBlockPressed(NewKeyCodeManager manager)
    {
        return Input.GetKeyDown(manager.KeyboardKeyCodes[PlayerCommandManager.Command.Block]);
    }
    public bool IsItemWindowPressed(NewKeyCodeManager manager)
    {
        return Input.GetKeyDown(manager.KeyboardKeyCodes[PlayerCommandManager.Command.ItemWindow]);
    }
    public bool IsChangeItemPressed(NewKeyCodeManager manager)
    {
        return Input.GetKeyDown(manager.KeyboardKeyCodes[PlayerCommandManager.Command.ChangeItem]);
    }
    public bool IsWalkThrowPrepare(NewKeyCodeManager manager)
    {
        if (!Input.GetKey(manager.KeyboardKeyCodes[PlayerCommandManager.Command.LeftMove]) &&
            !Input.GetKey(manager.KeyboardKeyCodes[PlayerCommandManager.Command.RightMove]))
        {
            return false;
        }
        return Input.GetKey(manager.KeyboardKeyCodes[PlayerCommandManager.Command.Interact]);
    }
    public (float, float) InputAimDirection()
    {
        float lr = 0;
        float ud = 0;

        if (Input.GetKey(KeyCode.RightArrow))
        {
            lr += 1;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            lr -= 1;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            ud -= 1;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            ud += 1;
        }

        return(lr, ud);
    }
    public void InputSourceDetect(NewKeyCodeManager manager)
    {
        manager.KeyboardCommandReceive();
    }
}
