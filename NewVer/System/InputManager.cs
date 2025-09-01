using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public NewKeyCodeManager _keyCodeManager;
    private PlayerCommandManager _commandManager;
    private IInputSource NowInputSource;

    private KeyboardCommandRecevier _keyboardRecevier;

    private HashSet<IAimSystemUser> aimSystemUsers = new HashSet<IAimSystemUser>();

    private void Awake()
    {
        _commandManager = new PlayerCommandManager();
        _keyCodeManager = new NewKeyCodeManager();
        _keyboardRecevier = new KeyboardCommandRecevier();

        NowInputSource = _keyboardRecevier;
    }

    void Update()
    {
        NowInputSource.InputSourceDetect(_keyCodeManager);

        if (NowInputSource.IsLeftMovePressing(_keyCodeManager))
        {
            _commandManager.ExecuteCommand(PlayerCommandManager.Command.LeftMove, PlayerCommandManager.CommandType.Pressing);
        }
        if (NowInputSource.IsLeftMoveUp(_keyCodeManager))
        {
            _commandManager.ExecuteCommand(PlayerCommandManager.Command.LeftMove, PlayerCommandManager.CommandType.Up);
        }
        if (NowInputSource.IsRightMovePressing(_keyCodeManager))
        {
            _commandManager.ExecuteCommand(PlayerCommandManager.Command.RightMove, PlayerCommandManager.CommandType.Pressing);
        }
        if (NowInputSource.IsRightMoveUp(_keyCodeManager))
        {
            _commandManager.ExecuteCommand(PlayerCommandManager.Command.RightMove, PlayerCommandManager.CommandType.Up);
        }

        if (NowInputSource.IsNormalAttackPressed(_keyCodeManager))
        {
            _commandManager.ExecuteCommand(PlayerCommandManager.Command.NormalAtk, PlayerCommandManager.CommandType.Pressed);
        }
        if (NowInputSource.IsNormalAttackPressing(_keyCodeManager))
        {
            _commandManager.ExecuteCommand(PlayerCommandManager.Command.NormalAtk, PlayerCommandManager.CommandType.Pressing);
        }
        if (NowInputSource.IsNormalAttackUp(_keyCodeManager))
        {
            _commandManager.ExecuteCommand(PlayerCommandManager.Command.NormalAtk, PlayerCommandManager.CommandType.Up);
        }
        if (NowInputSource.IsStrongAttackPressed(_keyCodeManager))
        {
            _commandManager.ExecuteCommand(PlayerCommandManager.Command.StrongAtk, PlayerCommandManager.CommandType.Pressed);
        }

        if (NowInputSource.IsJumpPressed(_keyCodeManager))
        {
            _commandManager.ExecuteCommand(PlayerCommandManager.Command.Jump, PlayerCommandManager.CommandType.Pressed);
        }
        if (NowInputSource.IsJumpUp(_keyCodeManager))
        {
            _commandManager.ExecuteCommand(PlayerCommandManager.Command.Jump, PlayerCommandManager.CommandType.Up);
        }
        if (NowInputSource.IsRestorePressed(_keyCodeManager))
        {
            _commandManager.ExecuteCommand(PlayerCommandManager.Command.Restore, PlayerCommandManager.CommandType.Pressed);
        }
        if (NowInputSource.IsUseItemPressed(_keyCodeManager))
        {
            _commandManager.ExecuteCommand(PlayerCommandManager.Command.UseItem, PlayerCommandManager.CommandType.Pressed);
        }
        if (NowInputSource.IsUseItemUp(_keyCodeManager))
        {
            _commandManager.ExecuteCommand(PlayerCommandManager.Command.UseItem, PlayerCommandManager.CommandType.Up);
        }
        if (NowInputSource.IsInteractPressed(_keyCodeManager))
        {
            _commandManager.ExecuteCommand(PlayerCommandManager.Command.Interact, PlayerCommandManager.CommandType.Pressed);
        }
        if (NowInputSource.IsInteractUp(_keyCodeManager))
        {
            _commandManager.ExecuteCommand(PlayerCommandManager.Command.Interact, PlayerCommandManager.CommandType.Up);
        }

        if (NowInputSource.IsDashPressed(_keyCodeManager))
        {
            _commandManager.ExecuteCommand(PlayerCommandManager.Command.Dash, PlayerCommandManager.CommandType.Pressed);
        }

        if (NowInputSource.IsShootPressed(_keyCodeManager))
        {
            _commandManager.ExecuteCommand(PlayerCommandManager.Command.Shoot, PlayerCommandManager.CommandType.Pressed);
        }
        if (NowInputSource.IsShootUp(_keyCodeManager))
        {
            _commandManager.ExecuteCommand(PlayerCommandManager.Command.Shoot, PlayerCommandManager.CommandType.Up);
        }
        if (NowInputSource.IsBlockPressed(_keyCodeManager))
        {
            _commandManager.ExecuteCommand(PlayerCommandManager.Command.Block, PlayerCommandManager.CommandType.Pressed);
        }
        if (NowInputSource.IsItemWindowPressed(_keyCodeManager))
        {
            _commandManager.ExecuteCommand(PlayerCommandManager.Command.ItemWindow, PlayerCommandManager.CommandType.Pressed);
        }
        if (NowInputSource.IsChangeItemPressed(_keyCodeManager))
        {
            _commandManager.ExecuteCommand(PlayerCommandManager.Command.ChangeItem, PlayerCommandManager.CommandType.Pressed);
        }

        if (aimSystemUsers.Count > 0)
            foreach (var user in aimSystemUsers)
            {
                user.ReceiveAimDirection(NowInputSource.InputAimDirection(), Time.deltaTime);
            }
    }

    public void SubscribeCommand(PlayerCommandManager.Command command, PlayerCommandManager.CommandType type, IObserver observer)
    {
        _commandManager.SubscribeCommand(command, type, observer);
    }

    public void SubscribeAimInput(IAimSystemUser observer)
    {
        aimSystemUsers.Add(observer);
    }
    public void UnSubscribeAimInput(IAimSystemUser observer)
    {
        if(aimSystemUsers.Contains(observer))
            aimSystemUsers.Remove(observer);
    }
    public bool WalkThrowCheck()
    {
        return NowInputSource.IsWalkThrowPrepare(_keyCodeManager);
    }
}
