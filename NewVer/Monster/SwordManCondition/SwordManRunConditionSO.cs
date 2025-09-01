using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Monster/Conditions/SwordMan/Run")]
public class SwordManRunConditionSO : TransitionCondition
{
    public float ChasingDistance;

    public override bool Evaluate(Monster monster)
    {
        return monster.DistanceWithTargetX > ChasingDistance;
    }
}
