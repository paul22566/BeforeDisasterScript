using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Monster/StatusTransitionTable")]
public class MonsterTransitionTable : ScriptableObject
{
    public List<TransitionRule> Rules;
}
