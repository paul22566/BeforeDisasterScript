using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerStatus;

public interface IPlayerAniUser
{
    public int GetAnimationPriority();
    public AnimationController GetAnimation();
}
public class PlayerMoveStop : IObserver
{
    private PlayerStatus _command;
    public PlayerMoveStop(PlayerStatus command)
    {
        _command = command;
    }
    public void ReceiveNotify()
    {
        _command.RemoveCommandFromSet();
    }
}
public class PlayerJumpStop : IObserver
{
    private PlayerJumpStatus _jump;

    public PlayerJumpStop(PlayerJumpStatus command)
    {
        _jump = command;
    }
    public void ReceiveNotify()
    {
        _jump.StopJump();
    }
}
public class PlayerAcumulateStop : IObserver
{
    private BattleSystem _battleSystem;
    private PlayerAcumulateStatus _Accumulate;
    private PlayerStatus _NormalAtk;
    private PlayerStatus _JumpAtk;
    private PlayerStatus _CriticAtk;

    public PlayerAcumulateStop(BattleSystem battle, PlayerAcumulateStatus command,
        PlayerStatus normalAtk, PlayerStatus jumpAtk, PlayerStatus criticAtk)
    {
        _battleSystem = battle;
        _Accumulate = command;
        _NormalAtk = normalAtk;
        _JumpAtk = jumpAtk;
        _CriticAtk = criticAtk;
    }
    public void ReceiveNotify()
    {
        if (!_Accumulate.isAccumulateBegin)
        {
            return;
        }

        if (_Accumulate.isAccumulateComplete && _battleSystem.SkillPower >= 600 && PlayerController.isGround)
        {
            _CriticAtk.AddCommandToSet();
        }
        else if(_battleSystem.CanAtk)
        {
            if (PlayerController.isGround)
            {
                _NormalAtk.AddCommandToSet();
            }
            else
            {
                _JumpAtk.AddCommandToSet();
            }
        }
    }
}
public class PlayerUseItemStart : IObserver
{
    private ItemManage _itemManager;
    private InputManager _inputManager;
    private PlayerUseNormalItemStatus UseNormalItem;
    private PlayerUseThrowItemStatus UseThrowItem;
    private PlayerLeftWalkThrowStatus LeftWalkThrow;
    private PlayerRightWalkThrowStatus RightWalkThrow;
    private PlayerJumpThrowStatus JumpAtk;

    public PlayerUseItemStart(ItemManage manager, PlayerUseNormalItemStatus useNormalItem, PlayerUseThrowItemStatus useThrowItem,
        PlayerLeftWalkThrowStatus leftWalkThrow, PlayerRightWalkThrowStatus rightWalkThrow, PlayerJumpThrowStatus jumpAtk, InputManager inputManager)
    {
        _itemManager = manager;
        UseNormalItem = useNormalItem;
        UseThrowItem = useThrowItem;
        LeftWalkThrow = leftWalkThrow;
        RightWalkThrow = rightWalkThrow;
        JumpAtk = jumpAtk;
        _inputManager = inputManager;
    }

    public void ReceiveNotify()
    {
        if (!GameEvent.TutorialComplete)
            return;

        switch (_itemManager.NowPrepareItem)
        {
            case ItemManage.UsefulItem.Cocktail:
                if (_itemManager.CocktailNumber <= 0)
                    return;

                if (!PlayerController.isGround)
                {
                    JumpAtk.SetItem(_itemManager._cocktail, _itemManager._cocktail);
                    JumpAtk.AddCommandToSet();
                    return;
                }
                if (!_inputManager.WalkThrowCheck())
                {
                    UseThrowItem.SetItem(_itemManager._cocktail, _itemManager._cocktail);
                    UseThrowItem.AddCommandToSet();
                    return;
                }

                switch (PlayerController._player.face)
                {
                    case Creature.Face.Right:
                        RightWalkThrow.SetItem(_itemManager._cocktail, _itemManager._cocktail);
                        RightWalkThrow.AddCommandToSet();
                        break;
                    case Creature.Face.Left:
                        LeftWalkThrow.SetItem(_itemManager._cocktail, _itemManager._cocktail);
                        LeftWalkThrow.AddCommandToSet();
                        break;
                }
                break;
            case ItemManage.UsefulItem.ExplosionBottle:
                if (_itemManager.ExplosionBottleNumber <= 0)
                    return;

                if (!PlayerController.isGround)
                {
                    JumpAtk.SetItem(_itemManager._explosionBottle, _itemManager._explosionBottle);
                    JumpAtk.AddCommandToSet();
                    return;
                }
                if (!_inputManager.WalkThrowCheck())
                {
                    UseThrowItem.SetItem(_itemManager._explosionBottle, _itemManager._explosionBottle);
                    UseThrowItem.AddCommandToSet();
                    return;
                }

                switch (PlayerController._player.face)
                {
                    case Creature.Face.Right:
                        RightWalkThrow.SetItem(_itemManager._explosionBottle, _itemManager._explosionBottle);
                        RightWalkThrow.AddCommandToSet();
                        break;
                    case Creature.Face.Left:
                        LeftWalkThrow.SetItem(_itemManager._explosionBottle, _itemManager._explosionBottle);
                        LeftWalkThrow.AddCommandToSet();
                        break;
                }
                break;
            case ItemManage.UsefulItem.Sharpener:
                if (ItemManage.CheckItemExist(ItemID.Sharpener))
                {
                    UseNormalItem.SetItem(_itemManager._sharpener, ItemID.Sharpener);
                    UseNormalItem.AddCommandToSet();
                }
                break;
            case ItemManage.UsefulItem.RitualSword:
                if (!ItemManage.CheckItemExist(ItemID.UnDeadSnake) && !ItemManage.CheckItemExist(ItemID.RebornSnake))
                {
                    break;
                }

                if (ItemManage.CheckItemExist(ItemID.RitualSword))
                {
                    UseNormalItem.SetItem(_itemManager._ritualSword, ItemID.RitualSword);
                    UseNormalItem.AddCommandToSet();
                }
                break;
        }
    }
}
public class PlayerAimStop : IObserver
{
    private PlayerUseThrowItemStatus _throwItemStatus;

    public PlayerAimStop(PlayerUseThrowItemStatus status)
    {
        _throwItemStatus = status;
    }

    public void ReceiveNotify()
    {
        if (!_throwItemStatus.isAimBegin)
        {
            return;
        }

        _throwItemStatus.ThrowSuccess();
    }
}
public class PlayerChangeItem: IObserver
{
    private ItemManage _itemManage;
    public PlayerChangeItem(ItemManage itemManage)
    {
        _itemManage = itemManage;
    }
    public void ReceiveNotify()
    {
        _itemManage.NowPrepareItemID = ItemManage.ChangePrepareItem(_itemManage.NowPrepareItemID);
    }
}

