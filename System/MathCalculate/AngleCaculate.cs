using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AngleCaculate : MonoBehaviour
{
    public static float CaculateAngle(string ImageDirection, Transform _transform, Transform PlayerTransform)
    {
        float DistanceX = 0;
        float DistanceY = 0;
        float AbsDistanceX = 0;
        float AbsDistanceY = 0;
        bool AtPlayerRight = false;
        bool AtPlayerLeft = false;
        bool isHigherThanPlayer = false;
        float Tan = 0;
        float TargetRotate = 0;

        DistanceX = _transform.position.x - PlayerTransform.position.x;
        AbsDistanceX = Mathf.Abs(DistanceX);
        DistanceY = PlayerTransform.position.y - _transform.position.y;
        AbsDistanceY = Mathf.Abs(DistanceY);

        //�p��۹���
        if (DistanceX <= 0)
        {
            AtPlayerLeft = true;
            AtPlayerRight = false;
        }
        else
        {
            AtPlayerLeft = false;
            AtPlayerRight = true;
        }
        if (DistanceY <= 0)
        {
            isHigherThanPlayer = true;
        }
        else
        {
            isHigherThanPlayer = false;
        }

        //�p�⨤��
        Tan = AbsDistanceY / AbsDistanceX;
        TargetRotate = Mathf.Atan(Tan);
        TargetRotate = TargetRotate / Mathf.PI * 180;

        //�M�Ϊ��p
        switch (ImageDirection)
        {
            case "R":
                if (AtPlayerRight)
                {
                    if (isHigherThanPlayer)
                    {
                        TargetRotate = 180 + TargetRotate;
                    }
                    else
                    {
                        //���κ�
                    }
                }
                if (AtPlayerLeft)
                {
                    if (isHigherThanPlayer)
                    {
                        TargetRotate = 360 - TargetRotate;
                    }
                    else
                    {
                        //���`��
                    }
                }
                break;
            case "L":
                if (AtPlayerRight)
                {
                    if (!isHigherThanPlayer)
                    {
                        TargetRotate = 360 - TargetRotate;
                    }
                    else
                    {
                        //���`��
                    }
                }
                if (AtPlayerLeft)
                {
                    if (isHigherThanPlayer)
                    {
                        TargetRotate = 180 - TargetRotate;
                    }
                    else
                    {
                        //���κ�
                    }
                }
                break;
        }

        if (TargetRotate >= 360)
        {
            TargetRotate -= 360;
        }
        if (TargetRotate < 0)
        {
            TargetRotate += 360;
        }

        return TargetRotate;
    }//(3)

    public static float CaculateAngle(string ImageDirection, Transform _transform, Transform PlayerTransform, float CorrectionNumber)
    {
        float DistanceX = 0;
        float DistanceY = 0;
        float AbsDistanceX = 0;
        float AbsDistanceY = 0;
        bool AtPlayerRight = false;
        bool AtPlayerLeft = false;
        bool isHigherThanPlayer = false;
        float Tan = 0;
        float TargetRotate = 0;

        DistanceX = _transform.position.x - PlayerTransform.position.x;
        AbsDistanceX = Mathf.Abs(DistanceX);
        DistanceY = PlayerTransform.position.y - _transform.position.y;
        AbsDistanceY = Mathf.Abs(DistanceY);

        //�p��۹���
        if (DistanceX <= 0)
        {
            AtPlayerLeft = true;
            AtPlayerRight = false;
        }
        else
        {
            AtPlayerLeft = false;
            AtPlayerRight = true;
        }
        if (DistanceY <= 0)
        {
            isHigherThanPlayer = true;
        }
        else
        {
            isHigherThanPlayer = false;
        }

        //�p�⨤��
        Tan = AbsDistanceY / AbsDistanceX;
        TargetRotate = Mathf.Atan(Tan);
        TargetRotate = TargetRotate / Mathf.PI * 180;

        //�M�Ϊ��p
        switch (ImageDirection)
        {
            case "R":
                if (AtPlayerRight)
                {
                    if (isHigherThanPlayer)
                    {
                        TargetRotate = 180 + TargetRotate;
                    }
                    else
                    {
                        //���κ�
                    }
                }
                if (AtPlayerLeft)
                {
                    if (isHigherThanPlayer)
                    {
                        TargetRotate = 360 - TargetRotate;
                    }
                    else
                    {
                        //���`��
                    }
                }
                break;
            case "L":
                if (AtPlayerRight)
                {
                    if (!isHigherThanPlayer)
                    {
                        TargetRotate = 360 - TargetRotate;
                    }
                    else
                    {
                        //���`��
                    }
                }
                if (AtPlayerLeft)
                {
                    if (isHigherThanPlayer)
                    {
                        TargetRotate = 180 - TargetRotate;
                    }
                    else
                    {
                        //���κ�
                    }
                }
                break;
        }

        //�̲׸ɥ�
        switch (ImageDirection)
        {
            case "R":
                TargetRotate = CorrectionNumber + TargetRotate;
                break;
            case "L":
                TargetRotate = TargetRotate  - CorrectionNumber;
                break;
        }
        if (TargetRotate >= 360)
        {
            TargetRotate -= 360;
        }
        if (TargetRotate < 0)
        {
            TargetRotate += 360;
        }

        return TargetRotate;
    }//(3)

    public static float AngleConvert(string InputDirection, string OutputDirection, float InputAngle,  float CorrectionNumber)
    {
        float OutputAngle = 0;

        switch (InputDirection)
        {
            case "R":
                switch (OutputDirection)
                {
                    case "R":
                        if (InputAngle > 180)
                        {
                            OutputAngle = InputAngle + CorrectionNumber;
                        }
                        else
                        {
                            OutputAngle = InputAngle + CorrectionNumber;
                            if (OutputAngle < 0)
                            {
                                OutputAngle = OutputAngle + 360;
                            }
                        }
                        break;
                    case "L":
                        if (InputAngle > 180)
                        {
                            OutputAngle = 180 - (360 - InputAngle - CorrectionNumber);
                        }
                        else
                        {
                            OutputAngle = 180 + InputAngle + CorrectionNumber;
                        }
                        break;
                }
                break;
            case "L":
                switch (OutputDirection)
                {
                    case "R":
                        if (InputAngle > 180)
                        {
                            OutputAngle = InputAngle - 180 - CorrectionNumber;
                        }
                        else
                        {
                            OutputAngle = InputAngle - CorrectionNumber + 180;
                        }
                        break;
                    case "L":
                        if (InputAngle > 180)
                        {
                            OutputAngle = InputAngle - CorrectionNumber - 360;
                        }
                        else
                        {
                            OutputAngle = InputAngle - CorrectionNumber;
                        }
                        break;
                }
                break;
        }

        return OutputAngle;
    }//(3)���״���

    public static float AngleDirectionChange(float InputAngle)
    {
        float OutputAngle = 0;
        OutputAngle = 360 - InputAngle;
        if (OutputAngle >= 360)
        {
            OutputAngle = OutputAngle - 360;
        }
        if (OutputAngle < 0)
        {
            OutputAngle = OutputAngle + 360;
        }
        return OutputAngle;
    }//���k����

    public static Vector2 CaculateSpeedRate(string ImageDirection, float Angle)
    {
        float XProportion = 0;
        float YProportion = 0;
        float Sin = 0;
        float Tan = 0;

        if (Angle < 0)
        {
            Angle += 360;
        }
        if (Angle >= 360)
        {
            Angle -= 360;
        }

        switch (ImageDirection)
        {
            case "R":
                if (Mathf.Abs(Angle - 180) < 90)
                {
                    Sin = Mathf.Abs(Angle - 180);
                    Sin = Sin * (Mathf.PI / 180);
                    Sin = Mathf.Sin(Sin);
                    Tan = Mathf.Pow(1 - Sin * Sin, 0.5f);
                    Tan = Sin / Tan;
                    XProportion = -1 / (1 + Tan);
                    YProportion = Tan / (1 + Tan);

                    if (Angle > 180)
                    {
                        YProportion *= -1;
                    }
                }
                if (Mathf.Abs(Angle - 180) > 90)
                {
                    if (Angle > 180)
                    {
                        Sin = Mathf.Abs(360 - Angle);
                    }
                    if (Angle < 180)
                    {
                        Sin = Mathf.Abs(Angle);
                    }
                    Sin = Sin * (Mathf.PI / 180);
                    Sin = Mathf.Sin(Sin);
                    Tan = Mathf.Pow(1 - Sin * Sin, 0.5f);
                    Tan = Sin / Tan;
                    XProportion = 1 / (1 + Tan);
                    YProportion = Tan / (1 + Tan);

                    if (Angle > 180)
                    {
                        YProportion *= -1;
                    }
                }

                if (Angle == 90)
                {
                    XProportion = 0;
                    YProportion = 1;
                }
                if (Angle == 270)
                {
                    XProportion = 0;
                    YProportion = -1;
                }
                break;
            case "L":
                if (Mathf.Abs(Angle - 180) < 90)
                {
                    Sin = Mathf.Abs(Angle - 180);
                    Sin = Sin * (Mathf.PI / 180);
                    Sin = Mathf.Sin(Sin);
                    Tan = Mathf.Pow(1 - Sin * Sin, 0.5f);
                    Tan = Sin / Tan;
                    XProportion = 1 / (1 + Tan);
                    YProportion = Tan / (1 + Tan);

                    if (Angle < 180)
                    {
                        YProportion *= -1;
                    }
                }
                if (Mathf.Abs(Angle - 180) > 90)
                {
                    if (Angle > 180)
                    {
                        Sin = Mathf.Abs(360 - Angle);
                    }
                    if (Angle < 180)
                    {
                        Sin = Mathf.Abs(Angle - 180);
                    }
                    Sin = Sin * (Mathf.PI / 180);
                    Sin = Mathf.Sin(Sin);
                    Tan = Mathf.Pow(1 - Sin * Sin, 0.5f);
                    Tan = Sin / Tan;
                    XProportion = -1 / (1 + Tan);
                    YProportion = Tan / (1 + Tan);

                    if (Angle < 180)
                    {
                        YProportion *= -1;
                    }
                }

                if (Angle == 90)
                {
                    XProportion = 0;
                    YProportion = -1;
                }
                if (Angle == 270)
                {
                    XProportion = 0;
                    YProportion = 1;
                }
                break;
        }

        return new Vector2(XProportion, YProportion);
    }//(3)
}
