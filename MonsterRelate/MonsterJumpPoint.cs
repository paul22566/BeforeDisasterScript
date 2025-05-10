using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterJumpPoint : MonoBehaviour
{
    public bool isHorizonJump;//script(怪物script)
    public bool isVerticalJump;//script(怪物script)
    public bool isGoRight;//script(怪物script)
    public bool isGoLeft;//script(怪物script)
    [HideInInspector] public Transform MonsterJumpMiddlePoint;//script(怪物script)
    public float PlatformHigh;//只有跳高會用到
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
