using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TransitionCondition : ScriptableObject
{
    public abstract bool Evaluate(Monster monster);
}
