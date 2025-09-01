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

        //計算相對方位
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

        //計算角度
        if (AbsDistanceX != 0)
        {
            Tan = AbsDistanceY / AbsDistanceX;
            TargetRotate = Mathf.Atan(Tan);
            TargetRotate = TargetRotate / Mathf.PI * 180;
        }

        //套用狀況
        switch (ImageDirection)
        {
            case "R":
                if (AbsDistanceX == 0)
                {
                    if (isHigherThanPlayer)
                    {
                        return 90;
                    }
                    else
                    {
                        return 270;
                    }
                }

                if (AtPlayerRight)
                {
                    if (isHigherThanPlayer)
                    {
                        //不用算
                    }
                    else
                    {
                        TargetRotate = 360 - TargetRotate;
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
                        TargetRotate = 180 + TargetRotate;
                    }
                }
                break;
            case "L":
                if (AbsDistanceX == 0)
                {
                    if (isHigherThanPlayer)
                    {
                        return 270;
                    }
                    else
                    {
                        return 90;
                    }
                }

                if (AtPlayerRight)
                {
                    if (isHigherThanPlayer)
                    {
                        TargetRotate = 180 + TargetRotate;
                    }
                    else
                    {
                        TargetRotate = 180 - TargetRotate;
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
                        //不用算
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

        //計算相對方位
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

        //計算角度
        if(AbsDistanceX != 0)
        {
            Tan = AbsDistanceY / AbsDistanceX;
            TargetRotate = Mathf.Atan(Tan);
            TargetRotate = TargetRotate / Mathf.PI * 180;
        }

        //套用狀況
        switch (ImageDirection)
        {
            case "R":
                if (AbsDistanceX == 0)
                {
                    if (isHigherThanPlayer)
                    {
                        return 90 + CorrectionNumber;
                    }
                    else
                    {
                        return 270 + CorrectionNumber;
                    }
                }

                if (AtPlayerRight)
                {
                    if (isHigherThanPlayer)
                    {
                        //不用算
                    }
                    else
                    {
                        TargetRotate = 360 - TargetRotate;
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
                        TargetRotate = 180 + TargetRotate;
                    }
                }
                break;
            case "L":
                if (AbsDistanceX == 0)
                {
                    if (isHigherThanPlayer)
                    {
                        return 270 - CorrectionNumber;
                    }
                    else
                    {
                        return 90 - CorrectionNumber;
                    }
                }

                if (AtPlayerRight)
                {
                    if (isHigherThanPlayer)
                    {
                        TargetRotate = 180 + TargetRotate;
                    }
                    else
                    {
                        TargetRotate = 180 - TargetRotate;
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
                        //不用算
                    }
                }
                break;
        }

        //補正值以預設朝向往上多少計算
        //最終補正
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
    }//(3)角度換算

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
    }//左右互換

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
