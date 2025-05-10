using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterJumpPoint : MonoBehaviour
{
    public bool isHorizonJump;//script(�Ǫ�script)
    public bool isVerticalJump;//script(�Ǫ�script)
    public bool isGoRight;//script(�Ǫ�script)
    public bool isGoLeft;//script(�Ǫ�script)
    [HideInInspector] public Transform MonsterJumpMiddlePoint;//script(�Ǫ�script)
    public float PlatformHigh;//�u�������|�Ψ�
    private float JumpTime = 0.5f;

    private void Start()
    {
        MonsterJumpMiddlePoint = this.transform.GetChild(0);
    }

    public float JumpSpeedCalculate(ParabolaVar _Var, Vector3 ObjectNowPoint)
    {
        float Distance = 0;
        float PredictPoint = 0;

        if (isVerticalJump)
        {
            if (isGoLeft)
            {
                PredictPoint = Parabola.PredictPointX(_Var, ObjectNowPoint.y + PlatformHigh, "L");
            }
            if (isGoRight)
            {
                PredictPoint = Parabola.PredictPointX(_Var, ObjectNowPoint.y + PlatformHigh, "R");
            }

            Distance = PredictPoint - ObjectNowPoint.x;
            Distance = Mathf.Abs(Distance);

            return Distance / JumpTime;
        }

        if (isHorizonJump)
        {
            Distance = MonsterJumpMiddlePoint.position.x - ObjectNowPoint.x; 
            Distance = Mathf.Abs(Distance) * 2;

            return Distance / JumpTime;
        }

        return 0;
    }
}
