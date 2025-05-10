using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatController
{
    public enum FloatStatus { CPositive, CNegetive };
    [HideInInspector] public FloatStatus floatStatus;//script(Dialog)

    private float OneRunTime;
    private float NowParabolaX;
    private float ParabolaXLimit;
    private float ParabolaY;
    private float ParabolaSpeed;
    private float ParabolaConstant = 100;
    private float FloatYCenter;
    private float FloatHeight;

    public void FloatVarInisialize(Transform _transform, float _oneRunTime, float _floatHeight)
    {
        OneRunTime = _oneRunTime;
        FloatHeight = _floatHeight;
        ParabolaXLimit = Mathf.Pow(4 * ParabolaConstant * FloatHeight, 0.5f);
        ParabolaSpeed = 2 * ParabolaXLimit / (OneRunTime / 2);
        FloatReset(_transform);
    }

    public void Float(Transform _transform, float _deltaTime)
    {
        switch (floatStatus)
        {
            case FloatStatus.CNegetive:
                ParabolaConstant = Mathf.Abs(ParabolaConstant) * -1;
                ParabolaY = (NowParabolaX * NowParabolaX / (ParabolaConstant * 4)) + FloatHeight;
                NowParabolaX += ParabolaSpeed * _deltaTime;
                _transform.position = new Vector3(_transform.position.x, FloatYCenter + ParabolaY, 0);
                if (NowParabolaX >= ParabolaXLimit)
                {
                    NowParabolaX = -ParabolaXLimit;
                    floatStatus = FloatStatus.CPositive;
                }
                break;
            case FloatStatus.CPositive:
                ParabolaConstant = Mathf.Abs(ParabolaConstant);
                ParabolaY = (NowParabolaX * NowParabolaX / (ParabolaConstant * 4)) - FloatHeight;
                NowParabolaX += ParabolaSpeed * _deltaTime;
                _transform.position = new Vector3(_transform.position.x, FloatYCenter + ParabolaY, 0);
                if (NowParabolaX >= ParabolaXLimit)
                {
                    NowParabolaX = -ParabolaXLimit;
                    floatStatus = FloatStatus.CNegetive;
                }
                break;
        }
    }
    
    public void FloatReset(Transform _transform)
    {
        floatStatus = FloatStatus.CNegetive;
        NowParabolaX = -ParabolaXLimit;
        FloatYCenter = _transform.position.y;
    }//連同漂浮中心重設

    public void FloatReset()
    {
        floatStatus = FloatStatus.CNegetive;
        NowParabolaX = -ParabolaXLimit;
    }
}
