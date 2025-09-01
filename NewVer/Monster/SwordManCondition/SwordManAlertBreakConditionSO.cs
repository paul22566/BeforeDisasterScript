using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Monster/Conditions/SwordMan/AlertBreak")]
public class SwordManAlertBreakConditionSO : TransitionCondition
{
    public float AlertDistanceX;
    public float ValidDistanceY;

    public override bool Evaluate(Monster monster)
    {
        return monster.DistanceWithTargetX <= AlertDistanceX &&
            monster.DistanceWithTargetY <= ValidDistanceY;
    }
}
