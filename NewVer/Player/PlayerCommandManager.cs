using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCommandManager
{
    public enum Command {Null, LeftMove, RightMove,NormalAtk, StrongAtk, Jump,
        Restore, UseItem, Interact, Dash, Shoot, Block, ItemWindow, ChangeItem, WalkThrow};
    public enum CommandType { Pressed, Pressing, Up};
    private Dictionary<(Command, CommandType), PlayerCommand> _commands = new Dictionary<(Command, CommandType), PlayerCommand>();

    public PlayerCommandManager()
    {
        _commands[(Command.LeftMove, CommandType.Pressing)] = new PlayerCommand();
        _commands[(Command.LeftMove, CommandType.Up)] = new PlayerCommand();
        _commands[(Command.RightMove, CommandType.Pressing)] = new PlayerCommand();
        _commands[(Command.RightMove, CommandType.Up)] = new PlayerCommand();
        _commands[(Command.NormalAtk, CommandType.Pressed)] = new PlayerCommand();
        _commands[(Command.NormalAtk, CommandType.Pressing)] = new PlayerCommand();
        _commands[(Command.NormalAtk, CommandType.Up)] = new PlayerCommand();
        _commands[(Command.StrongAtk, CommandType.Pressed)] = new PlayerCommand();
        _commands[(Command.Jump, CommandType.Pressed)] = new PlayerCommand();
        _commands[(Command.Jump, CommandType.Up)] = new PlayerCommand();
        _commands[(Command.Restore, CommandType.Pressed)] = new PlayerCommand();
        _commands[(Command.UseItem, CommandType.Pressed)] = new PlayerCommand();
        _commands[(Command.UseItem, CommandType.Up)] = new PlayerCommand();
        _commands[(Command.Interact, CommandType.Pressed)] = new PlayerCommand();
        _commands[(Command.Interact, CommandType.Up)] = new PlayerCommand();
        _commands[(Command.Dash, CommandType.Pressed)] = new PlayerCommand();
        _commands[(Command.Shoot, CommandType.Pressed)] = new PlayerCommand();
        _commands[(Command.Shoot, CommandType.Up)] = new PlayerCommand();
        _commands[(Command.Block, CommandType.Pressed)] = new PlayerCommand();
        _commands[(Command.ItemWindow, CommandType.Pressed)] = new PlayerCommand();
        _commands[(Command.ChangeItem, CommandType.Pressed)] = new PlayerCommand();

        //¯S®í«ü¥O
        _commands[(Command.WalkThrow, CommandType.Pressed)] = new PlayerCommand();
    }

    public void SubscribeCommand(Command command, CommandType type, IObserver observer)
    {
        if (_commands.TryGetValue((command, type), out var cmd))
        {
            cmd.Subscribe(observer);
        }
        else
        {
            Debug.LogWarning("NoCreateCommandDictionay");
        }
    }
    public void UnsubscribeCommand(Command command, CommandType type, IObserver observer)
    {
        if (_commands.TryGetValue((command, type), out var cmd))
        {
            cmd.Unsubscribe(observer);
        }
        else
        {
            Debug.LogWarning("NoCreateCommandDictionay");
        }
    }
    public void ExecuteCommand(Command command, CommandType type)
    {
        if (_commands.TryGetValue((command, type), out var cmd))
        {
            cmd.Execute();
        }
        else
        {
            Debug.LogWarning("NoCreateCommandDictionay");
        }
    }
}

public class PlayerCommand : IObservableCommand
{
    HashSet<IObserver> observers = new HashSet<IObserver>();
    public void Execute()
    {
        if (observers.Count > 0)
        {
            foreach (var observer in observers)
            {
                observer.ReceiveNotify();
            }
        }
    }

    public void Subscribe(IObserver observer)
    {
        observers.Add(observer);
    }

    public void Unsubscribe(IObserver observer)
    {
        if (observers.Contains(observer))
        {
            observers.Remove(observer);
        }
    }
}
