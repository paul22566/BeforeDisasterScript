using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Monster/Conditions/SwordMan/StringAtk")]
public class SwordManStringAtkConditionSO : TransitionCondition
{
    public float LeastDistance;
    public float StringAtkDistance;
    public float ValidDistanceY;

    public override bool Evaluate(Monster monster)
    {
        return monster.DistanceWithTargetX > LeastDistance &&
            monster.DistanceWithTargetX <= StringAtkDistance &&
            monster.DistanceWithTargetY <= ValidDistanceY;
    }
}
