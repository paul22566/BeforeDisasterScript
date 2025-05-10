using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Slash : MonoBehaviour
{
    public static SlashResult SlashCalculate(float HorizontalDistance, float VerticalDistance, string HorizontalDirection, string VerticalDirection)
    {
        float XProportion = 0;
        float YProportion = 0;
        float Slope = 0;
        float HDistance = 0;
        float VDistance = 0;

        SlashResult Result = new SlashResult();

        HDistance = Mathf.Abs(HorizontalDistance);
        VDistance = Mathf.Abs(VerticalDistance);

        Result.XDistance = HDistance;
        if (HDistance <= 0.01f && HDistance >= -0.01f)
        {
            XProportion = 0;
            YProportion = 1;
        }
        else
        {
            Slope = VDistance / HDistance;
            XProportion = 1 / (1 + Slope);
            YProportion = Slope / (1 + Slope);
        }

        Result.SlopeRate = new Vector2(XProportion, YProportion);

        switch (HorizontalDirection)
        {
            case "R":
                Result.SlopeRate = new Vector2(Result.SlopeRate.x, Result.SlopeRate.y);
                break;
            case "L":
                Result.SlopeRate = new Vector2(-Result.SlopeRate.x, Result.SlopeRate.y);
                break;
        }
        switch (VerticalDirection)
        {
            case "U":
                Result.SlopeRate = new Vector2(Result.SlopeRate.x, Result.SlopeRate.y);
                break;
            case "D":
                Result.SlopeRate = new Vector2(Result.SlopeRate.x, -Result.SlopeRate.y);
                break;
        }
        return Result;
    }

    public static void SlashMove(SlashResult Result, float DeltaTime, Transform transform)
    {
        transform.localPosition = new Vector3(transform.localPosition.x + Result.Speed * DeltaTime * Result.SlopeRate.x, transform.localPosition.y + Result.Speed * Result.SlopeRate.y * DeltaTime, 0);
    }

    public static void ShowSlashData(SlashResult Result)
    {
        print(Result.Speed);
        print(Result.XDistance);
        print(Result.SlopeRate);
    }//Debug用
}

public class SlashResult
{
    public float Speed;
    public float XDistance;
    public Vector2 SlopeRate;//蘊含方向性
}
