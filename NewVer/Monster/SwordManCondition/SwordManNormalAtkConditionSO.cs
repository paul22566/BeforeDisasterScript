using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Monster/Conditions/SwordMan/NormalAtk")]
public class SwordManNormalAtkConditionSO : TransitionCondition
{
    public float AtkDistance;
    public float ValidDistanceY;

    public override bool Evaluate(Monster monster)
    {
        return monster.DistanceWithTargetX <= AtkDistance &&
               monster.DistanceWithTargetY <= ValidDistanceY;
    }
}
