using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XboxCommandReceiver : IInputSource
{
    public bool isAnyButtonPressed = false;

    public bool isLTPressed = false;
    public bool isLTPressing = false;
    public bool isLTUp = false;
    public bool isRTPressed = false;
    public bool isRTPressing = false;
    public bool isRTUp = false;

    public float LeftStickHorizontalAxis = 0;
    public float LeftStickVerticalAxis = 0;
    public float RightStickHorizontalAxis = 0;
    public float RightStickVerticalAxis = 0;

    public bool isLeftStickUpPush = false;
    public bool isLeftStickUpPushing = false;
    public bool isLeftStickUpRelease = false;
    public bool isLeftStickDownPush = false;
    public bool isLeftStickDownPushing = false;
    public bool isLeftStickDownRelease = false;
    public bool isLeftStickRightPush = false;
    public bool isLeftStickRightPushing = false;
    public bool isLeftStickRightRelease = false;
    public bool isLeftStickLeftPush = false;
    public bool isLeftStickLeftPushing = false;
    public bool isLeftStickLeftRelease = false;

    public bool isCrossUpPressed = false;
    public bool isCrossUpPressing = false;
    public bool isCrossUpUp = false;
    public bool isCrossDownPressed = false;
    public bool isCrossDownPressing = false;
    public bool isCrossDownUp = false;
    public bool isCrossRightPressed = false;
    public bool isCrossRightPressing = false;
    public bool isCrossRightUp = false;
    public bool isCrossLeftPressed = false;
    public bool isCrossLeftPressing = false;
    public bool isCrossLeftUp = false;

    public bool IsLeftMovePressing(NewKeyCodeManager manager)
    {
        bool result = false;
        if (manager.LeftMovePressing)
        {
            manager.LeftMovePressing = false;
            result = true;
        }
        return result;
    }
    public bool IsLeftMoveUp(NewKeyCodeManager manager)
    {
        bool result = false;
        if (manager.LeftMoveUp)
        {
            manager.LeftMoveUp = false;
            result = true;
        }
        return result;
    }
    public bool IsRightMovePressing(NewKeyCodeManager manager)
    {
        bool result = false;
        if (manager.RightMovePressing)
        {
            manager.RightMovePressing = false;
            result = true;
        }
        return result;
    }
    public bool IsRightMoveUp(NewKeyCodeManager manager)
    {
        bool result = false;
        if (manager.RightMoveUp)
        {
            manager.RightMoveUp = false;
            result = true;
        }
        return result;
    }
    public bool IsNormalAttackPressed(NewKeyCodeManager manager)
    {
        bool result = false;
        if (manager.NormalAtkPressed)
        {
            manager.NormalAtkPressed = false;
            result = true;
        }
        return result;
    }
    public bool IsNormalAttackPressing(NewKeyCodeManager manager)
    {
        bool result = false;
        if (manager.NormalAtkPressing)
        {
            manager.NormalAtkPressing = false;
            result = true;
        }
        return result;
    }
    public bool IsNormalAttackUp(NewKeyCodeManager manager)
    {
        bool result = false;
        if (manager.NormalAtkUp)
        {
            manager.NormalAtkUp = false;
            result = true;
        }
        return result;
    }
    public bool IsStrongAttackPressed(NewKeyCodeManager manager)
    {
        bool result = false;
        if (manager.StrongAtkPressed)
        {
            manager.StrongAtkPressed = false;
            result = true;
        }
        return result;
    }
    public bool IsJumpPressed(NewKeyCodeManager manager)
    {
        bool result = false;
        if (manager.JumpPressed)
        {
            manager.JumpPressed = false;
            result = true;
        }
        return result;
    }
    public bool IsJumpUp(NewKeyCodeManager manager)
    {
        bool result = false;
        if (manager.JumpPressed)
        {
            manager.JumpPressed = false;
            result = true;
        }
        return result;
    }
    public bool IsDashPressed(NewKeyCodeManager manager)
    {
        bool result = false;
        if (manager.DashPressed)
        {
            manager.DashPressed = false;
            result = true;
        }
        return result;
    }
    public bool IsRestorePressed(NewKeyCodeManager manager)
    {
        bool result = false;
        if (manager.RestorePressed)
        {
            manager.RestorePressed = false;
            result = true;
        }
        return result;
    }
    public bool IsUseItemPressed(NewKeyCodeManager manager)
    {
        bool result = false;
        if (manager.UseItemPressed)
        {
            manager.UseItemPressed = false;
            result = true;
        }
        return result;
    }
    public bool IsUseItemUp(NewKeyCodeManager manager)
    {
        bool result = false;
        if (manager.UseItemUp)
        {
            manager.UseItemUp = false;
            result = true;
        }
        return result;
    }
    public bool IsInteractPressed(NewKeyCodeManager manager)
    {
        bool result = false;
        if (manager.InteractPressed)
        {
            manager.InteractPressed = false;
            result = true;
        }
        return result;
    }
    public bool IsWalkThrowPrepare(NewKeyCodeManager manager)
    {
        if (manager.LeftMovePressing && RightStickHorizontalAxis > 0.3)
        {
            return true;
        }
        if (manager.RightMovePressing && RightStickHorizontalAxis < -0.3)
        {
            return true;
        }
        return false;
    }
    public bool IsInteractUp(NewKeyCodeManager manager)
    {
        bool result = false;
        if (manager.InteractUp)
        {
            manager.InteractUp = false;
            result = true;
        }
        return result;
    }
    public bool IsShootPressed(NewKeyCodeManager manager)
    {
        bool result = false;
        if (manager.ShootPressed)
        {
            manager.ShootPressed = false;
            result = true;
        }
        return result;
    }
    public bool IsShootUp(NewKeyCodeManager manager)
    {
        bool result = false;
        if (manager.ShootUp)
        {
            manager.ShootUp = false;
            result = true;
        }
        return result;
    }
    public bool IsBlockPressed(NewKeyCodeManager manager)
    {
        bool result = false;
        if (manager.BlockPressed)
        {
            manager.BlockPressed = false;
            result = true;
        }
        return result;
    }
    public bool IsItemWindowPressed(NewKeyCodeManager manager)
    {
        bool result = false;
        if (manager.ItemWindowPressed)
        {
            manager.ItemWindowPressed = false;
            result = true;
        }
        return result;
    }
    public bool IsChangeItemPressed(NewKeyCodeManager manager)
    {
        bool result = false;
        if (manager.ChangeUseItemPressed)
        {
            manager.ChangeUseItemPressed = false;
            result = true;
        }
        return result;
    }
    public (float, float) InputAimDirection()
    {
        return (RightStickHorizontalAxis,  RightStickVerticalAxis);
    }

    public void InputSourceDetect(NewKeyCodeManager manager)
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            isAnyButtonPressed = true;
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            isAnyButtonPressed = true;
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton2))
        {
            isAnyButtonPressed = true;
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton3))
        {
            isAnyButtonPressed = true;
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton4))
        {
            isAnyButtonPressed = true;
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton5))
        {
            isAnyButtonPressed = true;
        }

        if (Input.GetAxis("LRT") < 0)
        {
            if (!isLTPressing)
            {
                isAnyButtonPressed = true;
                isLTPressed = true;
                isLTPressing = true;
            }
        }//LT按下
        if (Input.GetAxis("LRT") > 0)
        {
            if (!isRTPressing)
            {
                isAnyButtonPressed = true;
                isRTPressed = true;
                isRTPressing = true;
            }
        }//RT按下
        if (Input.GetAxis("LRT") <= 0)
        {
            if (isRTPressing)
            {
                isRTUp = true;
                isRTPressing = false;
            }
        }//RT放開
        if (Input.GetAxis("LRT") >= 0)
        {
            if (isLTPressing)
            {
                isLTUp = true;
                isLTPressing = false;
            }
        }//LT放開

        LeftStickHorizontalAxis = Input.GetAxis("Horizontal");
        LeftStickVerticalAxis = Input.GetAxis("Vertical");
        RightStickHorizontalAxis = Input.GetAxis("RightHorizontal");
        RightStickVerticalAxis = Input.GetAxis("RightVertical");

        if (LeftStickVerticalAxis > 0.8 && Mathf.Abs(LeftStickHorizontalAxis) < 0.7)
        {
            if (!isLeftStickUpPushing)
            {
                isLeftStickUpPush = true;
                isLeftStickUpPushing = true;
            }
        }//蘑菇頭上推
        if (LeftStickVerticalAxis <= 0)
        {
            if (isLeftStickUpPushing)
            {
                isLeftStickUpPushing = false;
            }
        }//蘑菇頭放開
        if (LeftStickVerticalAxis < -0.8 && Mathf.Abs(LeftStickHorizontalAxis) < 0.7)
        {
            if (!isLeftStickDownPushing)
            {
                isLeftStickDownPush = true;
                isLeftStickDownPushing = true;
            }
        }//蘑菇頭下推
        if (LeftStickVerticalAxis >= 0)
        {
            if (isLeftStickDownPushing)
            {
                isLeftStickDownPushing = false;
            }
        }//蘑菇頭放開
        if (LeftStickHorizontalAxis > 0.8 && Mathf.Abs(LeftStickVerticalAxis) < 0.7)
        {
            if (!isLeftStickRightPushing)
            {
                isLeftStickRightPush = true;
                isLeftStickRightPushing = true;
            }
        }//蘑菇頭右推
        if (LeftStickHorizontalAxis <= 0)
        {
            if (isLeftStickRightPushing)
            {
                isLeftStickRightPushing = false;
            }
        }//蘑菇頭放開
        if (LeftStickHorizontalAxis < -0.8 && Mathf.Abs(LeftStickVerticalAxis) < 0.7)
        {
            if (!isLeftStickLeftPushing)
            {
                isLeftStickLeftPush = true;
                isLeftStickLeftPushing = true;
            }
        }//蘑菇頭左推
        if (LeftStickHorizontalAxis >= 0)
        {
            if (isLeftStickLeftPushing)
            {
                isLeftStickLeftPushing = false;
            }
        }//蘑菇頭放開

        if (Input.GetAxis("CrossVertical") > 0)
        {
            if (!isCrossUpPressing)
            {
                isAnyButtonPressed = true;
                isCrossUpPressed = true;
                isCrossUpPressing = true;
            }
        }//十字上按下
        if (Input.GetAxis("CrossVertical") <= 0)
        {
            if (isCrossUpPressing)
            {
                isCrossUpUp = true;
                isCrossUpPressing = false;
            }
        }//十字上放開
        if (Input.GetAxis("CrossVertical") < 0)
        {
            if (!isCrossDownPressing)
            {
                isAnyButtonPressed = true;
                isCrossDownPressed = true;
                isCrossDownPressing = true;
            }
        }//十字下按下
        if (Input.GetAxis("CrossVertical") >= 0)
        {
            if (isCrossDownPressing)
            {
                isCrossDownUp = true;
                isCrossDownPressing = false;
            }
        }//十字下放開
        if (Input.GetAxis("CrossHorizontal") > 0)
        {
            if (!isCrossRightPressing)
            {
                isAnyButtonPressed = true;
                isCrossRightPressed = true;
                isCrossRightPressing = true;
            }
        }//十字右按下
        if (Input.GetAxis("CrossHorizontal") <= 0)
        {
            if (isCrossRightPressing)
            {
                isCrossRightUp = true;
                isCrossRightPressing = false;
            }
        }//十字右放開
        if (Input.GetAxis("CrossHorizontal") < 0)
        {
            if (!isCrossLeftPressing)
            {
                isAnyButtonPressed = true;
                isCrossLeftPressed = true;
                isCrossLeftPressing = true;
            }
        }//十字左按下
        if (Input.GetAxis("CrossHorizontal") >= 0)
        {
            if (isCrossLeftPressing)
            {
                isCrossLeftUp = true;
                isCrossLeftPressing = false;
            }
        }//十字左放開

        manager.XboxCommandReceive(this);

        if (isAnyButtonPressed)
        {
            isAnyButtonPressed = false;
        }
        if (isRTPressed)
        {
            isRTPressed = false;
        }
        if (isLTPressed)
        {
            isLTPressed = false;
        }
        if (isLeftStickUpPush)
        {
            isLeftStickUpPush = false;
        }
        if (isLeftStickUpRelease)
        {
            isLeftStickUpRelease = false;
        }
        if (isLeftStickDownPush)
        {
            isLeftStickDownPush = false;
        }
        if (isLeftStickDownRelease)
        {
            isLeftStickDownRelease = false;
        }
        if (isLeftStickRightPush)
        {
            isLeftStickRightPush = false;
        }
        if (isLeftStickRightRelease)
        {
            isLeftStickRightRelease = false;
        }
        if (isLeftStickLeftPush)
        {
            isLeftStickLeftPush = false;
        }
        if (isLeftStickLeftRelease)
        {
            isLeftStickLeftRelease = false;
        }

        if (isCrossUpPressed)
        {
            isCrossUpPressed = false;
        }
        if (isCrossDownPressed)
        {
            isCrossDownPressed = false;
        }
        if (isCrossRightPressed)
        {
            isCrossRightPressed = false;
        }
        if (isCrossLeftPressed)
        {
            isCrossLeftPressed = false;
        }

        if (isLTUp)
        {
            isLTUp = false;
        }
        if (isRTUp)
        {
            isRTUp = false;
        }
        if (isCrossUpUp)
        {
            isCrossUpUp = false;
        }
        if (isCrossDownUp)
        {
            isCrossDownUp = false;
        }
        if (isCrossRightUp)
        {
            isCrossRightUp = false;
        }
        if (isCrossLeftUp)
        {
            isCrossLeftUp = false;
        }
    }
}
