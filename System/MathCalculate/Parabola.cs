using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parabola : MonoBehaviour
{
    public static float CalculateParabolaConstant(ParabolaVar _Var)
    {
        float Constant;
        //拋物線計算
        Constant = (_Var.OtherPoint.x - _Var.MiddlePoint.x) * (_Var.OtherPoint.x - _Var.MiddlePoint.x);
        Constant = Constant / (_Var.OtherPoint.y - _Var.MiddlePoint.y) / 4;
        switch (_Var.VerticalDirection)
        {
            case "Up":
                Constant = Mathf.Abs(Constant);
                break;
            case "Down":
                Constant = -Mathf.Abs(Constant);
                break;
        }

        return Constant;
    }//(1)其他變數定好再用

    public static float PredictPointX(ParabolaVar _Var, float Y, string Direction)//(1)
    {
        switch (Direction)
        {
            case "R":
                return Mathf.Pow(4 * _Var.ParabolaConstant * (Y - _Var.MiddlePoint.y), 0.5f) + _Var.MiddlePoint.x;
            case "L":
                return -Mathf.Pow(4 * _Var.ParabolaConstant * (Y - _Var.MiddlePoint.y), 0.5f) + _Var.MiddlePoint.x;
        }

        return 0;
    }

    public static void PredictPointY(ParabolaVar _Var, float X)//(1)
    {
        print((X - _Var.MiddlePoint.x) * (X - _Var.MiddlePoint.x) / 4 / _Var.ParabolaConstant + _Var.MiddlePoint.y);
    }

    public static void ShowParabolaVar(ParabolaVar _Var)//(1)
    {
        print(_Var.MiddlePoint);
        print(_Var.OtherPoint);
        print(_Var.HorizontalDirection); 
        print(_Var.VerticalDirection);
        print(_Var.ParabolaConstant);
        print(_Var.Speed);
    }

    public static void ParabolaMove(ParabolaVar _Var, float DeltaTime, Transform _transform)
    {
        if (_Var.ParabolaConstant == 0)
        {
            print("ConstantWrong");
            return;
        }

        switch (_Var.HorizontalDirection)
        {
            case "Right":
                _Var.ParabolaNowX = _Var.ParabolaNowX + _Var.Speed * DeltaTime;
                _transform.localPosition = new Vector3(_Var.ParabolaNowX, (_Var.ParabolaNowX - _Var.MiddlePoint.x) * (_Var.ParabolaNowX - _Var.MiddlePoint.x) / 4 / _Var.ParabolaConstant + _Var.MiddlePoint.y, 0);
                break;
            case "Left":
                _Var.ParabolaNowX = _Var.ParabolaNowX - _Var.Speed * DeltaTime;
                _transform.localPosition = new Vector3(_Var.ParabolaNowX, (_Var.ParabolaNowX - _Var.MiddlePoint.x) * (_Var.ParabolaNowX - _Var.MiddlePoint.x) / 4 / _Var.ParabolaConstant + _Var.MiddlePoint.y, 0);
                break;
        }
    }//(2)

    public static void ParabolaMove(ParabolaVar _Var, float DeltaTime, Transform _transform, bool LeftLimit, bool RightLimit)
    {
        switch (_Var.HorizontalDirection)
        {
            case "Right":
                _Var.ParabolaNowX = _Var.ParabolaNowX + _Var.Speed * DeltaTime;
                if (!RightLimit)
                {
                    _transform.localPosition = new Vector3(_Var.ParabolaNowX, (_Var.ParabolaNowX - _Var.MiddlePoint.x) * (_Var.ParabolaNowX - _Var.MiddlePoint.x) / 4 / _Var.ParabolaConstant + _Var.MiddlePoint.y, 0);
                }
                else
                {
                    _transform.localPosition = new Vector3(_transform.localPosition.x, (_Var.ParabolaNowX - _Var.MiddlePoint.x) * (_Var.ParabolaNowX - _Var.MiddlePoint.x) / 4 / _Var.ParabolaConstant + _Var.MiddlePoint.y, 0);
                }
                break;
            case "Left":
                _Var.ParabolaNowX = _Var.ParabolaNowX - _Var.Speed * DeltaTime;
                if (!LeftLimit)
                {
                    _transform.localPosition = new Vector3(_Var.ParabolaNowX, (_Var.ParabolaNowX - _Var.MiddlePoint.x) * (_Var.ParabolaNowX - _Var.MiddlePoint.x) / 4 / _Var.ParabolaConstant + _Var.MiddlePoint.y, 0);
                }
                else
                {
                    _transform.localPosition = new Vector3(_transform.localPosition.x, (_Var.ParabolaNowX - _Var.MiddlePoint.x) * (_Var.ParabolaNowX - _Var.MiddlePoint.x) / 4 / _Var.ParabolaConstant + _Var.MiddlePoint.y, 0);
                }
                break;
        }
    }//(2)
}

public class ParabolaVar
{
    public float ParabolaConstant;
    public float Speed;
    public Vector3 MiddlePoint;
    public Vector3 OtherPoint;
    public float ParabolaNowX;
    public string VerticalDirection;//開口朝向
    public string HorizontalDirection;
}
