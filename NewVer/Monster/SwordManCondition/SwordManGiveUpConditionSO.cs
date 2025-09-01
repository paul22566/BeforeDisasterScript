using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Monster/Conditions/SwordMan/GiveUp")]
public class SwordManGiveUpConditionSO : TransitionCondition
{
    public float GiveUpDistanceX;
    public float GiveUpDistanceY;

    public override bool Evaluate(Monster monster)
    {
        return monster.DistanceWithTargetX > GiveUpDistanceX ||
               monster.DistanceWithTargetY > GiveUpDistanceY;
    }
}
