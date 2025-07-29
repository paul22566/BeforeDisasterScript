using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceClass
{
    public static float CalculateRelativelyDistanceX(Vector3 Point1, Vector3 Point2)
    {
        float Result = 0;

        Result = Point1.x - Point2.x;

        return Result;
    }

    public static float CalculateRelativelyDistanceY(Vector3 Point1, Vector3 Point2)
    {
        float Result = 0;

        Result = Point1.y - Point2.y;

        return Result;
    }

    public static float CalculateAbsoluteDistanceX(Vector3 Point1, Vector3 Point2)
    {
        float Result = 0;

        Result = Mathf.Abs(Point1.x - Point2.x);

        return Result;
    }

    public static float CalculateAbsoluteDistanceY(Vector3 Point1, Vector3 Point2)
    {
        float Result = 0;

        Result = Mathf.Abs(Point1.y - Point2.y);

        return Result;
    }

    public static float CalculateAbsoluteDistance(Vector3 Point1, Vector3 Point2)
    {
        float Result = 0;
        float DistanceX = 0;
        float DistanceY = 0;

        DistanceX = CalculateAbsoluteDistanceX(Point1, Point2);
        DistanceY = CalculateAbsoluteDistanceY(Point1, Point2);

        Result = DistanceX * DistanceX + DistanceY * DistanceY;
        Result = Mathf.Pow(Result, 0.5f);

        return Result;
    }

    public static Vector3 CalculateLine(Vector3 Point1, Vector3 Point2)
    {
        // ax+by+c = 0

        float a;
        float b;

        float Number1;

        if (Point1.x != Point2.x)
        {
            Number1 = Point1.y - Point2.y;
            a = Point1.x - Point2.x;
            a = Number1 / a;

            Number1 = Point1.x * a;
            b = Point1.y - Number1;

            return new Vector3(a, -1, b);
        }
        else
        {
            return new Vector3(1, 0, -Point1.x);
        }
    }//計算線方程

    public static Vector2 CalculateCrossPoint(Vector3 Line1, Vector3 Line2)
    {
        // ax+by+c = 0

        float x = 0;
        float y = 0;
        float Number1;

        if (Line1.y == 0 || Line2.y == 0)
        {
            if (Line1.y == 0)
            {
                x = -Line1.z;
                y = x * Line2.x + Line2.z;
            }
            if (Line2.y == 0)
            {
                x = -Line2.z;
                y = x * Line1.x + Line1.z;
            }
            return new Vector2(x, y);
        }
        else
        {
            x = Line2.x - Line1.x;
            Number1 = Line1.z - Line2.z;
            x = Number1 / x;

            y = x * Line1.x + Line1.z;

            return new Vector2(x, y);
        }
    }//計算交點
}
