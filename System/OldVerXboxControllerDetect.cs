using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldVerXboxControllerDetect : MonoBehaviour
{
    private static bool HasOpenController;

    public static bool isLTPressed = false;//手把專用
    private bool LTPressBarrier;//手把專用 需放開才能按的限制
    public static bool isLTUp = false;//手把專用
    public static bool isRTPressed = false;//手把專用
    private bool RTPressBarrier;//手把專用 需放開才能按的限制
    public static bool isRTUp = false;//手把專用

    public static bool isControllerUpPressed = false;//手把專用
    private bool isControllerUpBarrier = false;//手把專用 需放開才能按的限制
    public static bool isControllerDownPressed = false;//手把專用
    private bool isControllerDownBarrier = false;//手把專用 需放開才能按的限制
    public static bool isControllerRightPressed = false;//手把專用
    private bool isControllerRightBarrier = false;//手把專用 需放開才能按的限制
    public static bool isControllerLeftPressed = false;//手把專用
    private bool isControllerLeftBarrier = false;//手把專用 需放開才能按的限制

    public static bool isCrossUpPressed = false;//手把專用
    private bool CrossUpPressBarrier;//手把專用 需放開才能按的限制
    public static bool isCrossUpUp = false;//手把專用
    public static bool isCrossDownPressed = false;//手把專用
    private bool CrossDownPressBarrier;//手把專用 需放開才能按的限制
    public static bool isCrossDownUp = false;//手把專用
    public static bool isCrossRightPressed = false;//手把專用
    private bool CrossRightPressBarrier;//手把專用 需放開才能按的限制
    public static bool isCrossRightUp = false;//手把專用
    public static bool isCrossLeftPressed = false;//手把專用
    private bool CrossLeftPressBarrier;//手把專用 需放開才能按的限制
    public static bool isCrossLeftUp = false;//手把專用

    private void Start()
    {
        if (HasOpenController)
        {
            Destroy(this.gameObject);
            return;
        }
        if (!HasOpenController)
        {
            HasOpenController = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.LogWarning("Old Var!!");
        ControllerJudge();
    }

    private void ControllerJudge()
    {
        if (Input.GetAxis("LRT") < 0)
        {
            if (!LTPressBarrier)
            {
                isLTPressed = true;
                LTPressBarrier = true;
            }
        }//LT按下
        if (Input.GetAxis("LRT") > 0)
        {
            if (!RTPressBarrier)
            {
                isRTPressed = true;
                RTPressBarrier = true;
            }
        }//RT按下
        if (Input.GetAxis("LRT") <= 0)
        {
            if (RTPressBarrier)
            {
                isRTUp = true;
                RTPressBarrier = false;
            }
        }//RT放開
        if (Input.GetAxis("LRT") >= 0)
        {
            if (LTPressBarrier)
            {
                isLTUp = true;
                LTPressBarrier = false;
            }
        }//LT放開

        if (Input.GetAxis("Vertical") > 0.8 && Mathf.Abs(Input.GetAxis("Horizontal")) < 0.7)
        {
            if (!isControllerUpBarrier)
            {
                isControllerUpPressed = true;
                isControllerUpBarrier = true;
            }
        }//蘑菇頭上推
        if (Input.GetAxis("Vertical") <= 0)
        {
            if (isControllerUpBarrier)
            {
                isControllerUpBarrier = false;
            }
        }//蘑菇頭放開
        if (Input.GetAxis("Vertical") < -0.8 && Mathf.Abs(Input.GetAxis("Horizontal")) < 0.7)
        {
            if (!isControllerDownBarrier)
            {
                isControllerDownPressed = true;
                isControllerDownBarrier = true;
            }
        }//蘑菇頭下推
        if (Input.GetAxis("Vertical") >= 0)
        {
            if (isControllerDownBarrier)
            {
                isControllerDownBarrier = false;
            }
        }//蘑菇頭放開
        if (Input.GetAxis("Horizontal") > 0.8 && Mathf.Abs(Input.GetAxis("Vertical")) < 0.7)
        {
            if (!isControllerRightBarrier)
            {
                isControllerRightPressed = true;
                isControllerRightBarrier = true;
            }
        }//蘑菇頭右推
        if (Input.GetAxis("Horizontal") <= 0)
        {
            if (isControllerRightBarrier)
            {
                isControllerRightBarrier = false;
            }
        }//蘑菇頭放開
        if (Input.GetAxis("Horizontal") < -0.8 && Mathf.Abs(Input.GetAxis("Vertical")) < 0.7)
        {
            if (!isControllerLeftBarrier)
            {
                isControllerLeftPressed = true;
                isControllerLeftBarrier = true;
            }
        }//蘑菇頭左推
        if (Input.GetAxis("Horizontal") >= 0)
        {
            if (isControllerLeftBarrier)
            {
                isControllerLeftBarrier = false;
            }
        }//蘑菇頭放開

        if (Input.GetAxis("CrossVertical") > 0)
        {
            if (!CrossUpPressBarrier)
            {
                isCrossUpPressed = true;
                CrossUpPressBarrier = true;
            }
        }//十字上按下
        if (Input.GetAxis("CrossVertical") <= 0)
        {
            if (CrossUpPressBarrier)
            {
                isCrossUpUp = true;
                CrossUpPressBarrier = false;
            }
        }//十字上放開
        if (Input.GetAxis("CrossVertical") < 0)
        {
            if (!CrossDownPressBarrier)
            {
                isCrossDownPressed = true;
                CrossDownPressBarrier = true;
            }
        }//十字下按下
        if (Input.GetAxis("CrossVertical") >= 0)
        {
            if (CrossDownPressBarrier)
            {
                isCrossDownUp = true;
                CrossDownPressBarrier = false;
            }
        }//十字下放開
        if (Input.GetAxis("CrossHorizontal") > 0)
        {
            if (!CrossRightPressBarrier)
            {
                isCrossRightPressed = true;
                CrossRightPressBarrier = true;
            }
        }//十字右按下
        if (Input.GetAxis("CrossHorizontal") <= 0)
        {
            if (CrossRightPressBarrier)
            {
                isCrossRightUp = true;
                CrossRightPressBarrier = false;
            }
        }//十字右放開
        if (Input.GetAxis("CrossHorizontal") < 0)
        {
            if (!CrossLeftPressBarrier)
            {
                isCrossLeftPressed = true;
                CrossLeftPressBarrier = true;
            }
        }//十字左按下
        if (Input.GetAxis("CrossHorizontal") >= 0)
        {
            if (CrossLeftPressBarrier)
            {
                isCrossLeftUp = true;
                CrossLeftPressBarrier = false;
            }
        }//十字左放開
    }

    public void ControllerFalse()
    {
        if (isRTPressed)
        {
            isRTPressed = false;
        }
        if (isLTPressed)
        {
            isLTPressed = false;
        }
        if (isControllerUpPressed)
        {
            isControllerUpPressed = false;
        }
        if (isControllerDownPressed)
        {
            isControllerDownPressed = false;
        }
        if (isControllerRightPressed)
        {
            isControllerRightPressed = false;
        }
        if (isControllerLeftPressed)
        {
            isControllerLeftPressed = false;
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
