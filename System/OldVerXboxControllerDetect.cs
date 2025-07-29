using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldVerXboxControllerDetect : MonoBehaviour
{
    private static bool HasOpenController;

    public static bool isLTPressed = false;//���M��
    private bool LTPressBarrier;//���M�� �ݩ�}�~���������
    public static bool isLTUp = false;//���M��
    public static bool isRTPressed = false;//���M��
    private bool RTPressBarrier;//���M�� �ݩ�}�~���������
    public static bool isRTUp = false;//���M��

    public static bool isControllerUpPressed = false;//���M��
    private bool isControllerUpBarrier = false;//���M�� �ݩ�}�~���������
    public static bool isControllerDownPressed = false;//���M��
    private bool isControllerDownBarrier = false;//���M�� �ݩ�}�~���������
    public static bool isControllerRightPressed = false;//���M��
    private bool isControllerRightBarrier = false;//���M�� �ݩ�}�~���������
    public static bool isControllerLeftPressed = false;//���M��
    private bool isControllerLeftBarrier = false;//���M�� �ݩ�}�~���������

    public static bool isCrossUpPressed = false;//���M��
    private bool CrossUpPressBarrier;//���M�� �ݩ�}�~���������
    public static bool isCrossUpUp = false;//���M��
    public static bool isCrossDownPressed = false;//���M��
    private bool CrossDownPressBarrier;//���M�� �ݩ�}�~���������
    public static bool isCrossDownUp = false;//���M��
    public static bool isCrossRightPressed = false;//���M��
    private bool CrossRightPressBarrier;//���M�� �ݩ�}�~���������
    public static bool isCrossRightUp = false;//���M��
    public static bool isCrossLeftPressed = false;//���M��
    private bool CrossLeftPressBarrier;//���M�� �ݩ�}�~���������
    public static bool isCrossLeftUp = false;//���M��

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
        }//LT���U
        if (Input.GetAxis("LRT") > 0)
        {
            if (!RTPressBarrier)
            {
                isRTPressed = true;
                RTPressBarrier = true;
            }
        }//RT���U
        if (Input.GetAxis("LRT") <= 0)
        {
            if (RTPressBarrier)
            {
                isRTUp = true;
                RTPressBarrier = false;
            }
        }//RT��}
        if (Input.GetAxis("LRT") >= 0)
        {
            if (LTPressBarrier)
            {
                isLTUp = true;
                LTPressBarrier = false;
            }
        }//LT��}

        if (Input.GetAxis("Vertical") > 0.8 && Mathf.Abs(Input.GetAxis("Horizontal")) < 0.7)
        {
            if (!isControllerUpBarrier)
            {
                isControllerUpPressed = true;
                isControllerUpBarrier = true;
            }
        }//Ĩۣ�Y�W��
        if (Input.GetAxis("Vertical") <= 0)
        {
            if (isControllerUpBarrier)
            {
                isControllerUpBarrier = false;
            }
        }//Ĩۣ�Y��}
        if (Input.GetAxis("Vertical") < -0.8 && Mathf.Abs(Input.GetAxis("Horizontal")) < 0.7)
        {
            if (!isControllerDownBarrier)
            {
                isControllerDownPressed = true;
                isControllerDownBarrier = true;
            }
        }//Ĩۣ�Y�U��
        if (Input.GetAxis("Vertical") >= 0)
        {
            if (isControllerDownBarrier)
            {
                isControllerDownBarrier = false;
            }
        }//Ĩۣ�Y��}
        if (Input.GetAxis("Horizontal") > 0.8 && Mathf.Abs(Input.GetAxis("Vertical")) < 0.7)
        {
            if (!isControllerRightBarrier)
            {
                isControllerRightPressed = true;
                isControllerRightBarrier = true;
            }
        }//Ĩۣ�Y�k��
        if (Input.GetAxis("Horizontal") <= 0)
        {
            if (isControllerRightBarrier)
            {
                isControllerRightBarrier = false;
            }
        }//Ĩۣ�Y��}
        if (Input.GetAxis("Horizontal") < -0.8 && Mathf.Abs(Input.GetAxis("Vertical")) < 0.7)
        {
            if (!isControllerLeftBarrier)
            {
                isControllerLeftPressed = true;
                isControllerLeftBarrier = true;
            }
        }//Ĩۣ�Y����
        if (Input.GetAxis("Horizontal") >= 0)
        {
            if (isControllerLeftBarrier)
            {
                isControllerLeftBarrier = false;
            }
        }//Ĩۣ�Y��}

        if (Input.GetAxis("CrossVertical") > 0)
        {
            if (!CrossUpPressBarrier)
            {
                isCrossUpPressed = true;
                CrossUpPressBarrier = true;
            }
        }//�Q�r�W���U
        if (Input.GetAxis("CrossVertical") <= 0)
        {
            if (CrossUpPressBarrier)
            {
                isCrossUpUp = true;
                CrossUpPressBarrier = false;
            }
        }//�Q�r�W��}
        if (Input.GetAxis("CrossVertical") < 0)
        {
            if (!CrossDownPressBarrier)
            {
                isCrossDownPressed = true;
                CrossDownPressBarrier = true;
            }
        }//�Q�r�U���U
        if (Input.GetAxis("CrossVertical") >= 0)
        {
            if (CrossDownPressBarrier)
            {
                isCrossDownUp = true;
                CrossDownPressBarrier = false;
            }
        }//�Q�r�U��}
        if (Input.GetAxis("CrossHorizontal") > 0)
        {
            if (!CrossRightPressBarrier)
            {
                isCrossRightPressed = true;
                CrossRightPressBarrier = true;
            }
        }//�Q�r�k���U
        if (Input.GetAxis("CrossHorizontal") <= 0)
        {
            if (CrossRightPressBarrier)
            {
                isCrossRightUp = true;
                CrossRightPressBarrier = false;
            }
        }//�Q�r�k��}
        if (Input.GetAxis("CrossHorizontal") < 0)
        {
            if (!CrossLeftPressBarrier)
            {
                isCrossLeftPressed = true;
                CrossLeftPressBarrier = true;
            }
        }//�Q�r�����U
        if (Input.GetAxis("CrossHorizontal") >= 0)
        {
            if (CrossLeftPressBarrier)
            {
                isCrossLeftUp = true;
                CrossLeftPressBarrier = false;
            }
        }//�Q�r����}
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
