using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimationController
{
    public int PlayPriority;
    protected GameObject _aniObject;
    protected Transform _aniTransform;
    protected Animator _animator;
    protected float _aniScale;

    public virtual void AniPlay()
    {
        _aniObject.SetActive(true);
    }
    public virtual void AniStop()
    {
        _aniObject.SetActive(false);
    }
    public virtual void AniTurnFace(Creature.Face face)
    {
        switch (face)
        {
            case Creature.Face.Left:
                _aniTransform.localScale = new Vector3(-_aniScale, _aniScale, 0);
                break;
            case Creature.Face.Right:
                _aniTransform.localScale = new Vector3(_aniScale, _aniScale, 0);
                break;
        }
    }
    public void ChangeAnimator(string name, string value)
    {
        if (value == "true" || value == "false")
        {
            bool resultBool = false;
            if (value == "true")
                resultBool = true;
            if (value == "false")
                resultBool = false;

            _animator.SetBool(name, resultBool);
            return;
        }

        int resultInt = 0;
        if (int.TryParse(value, out resultInt))
        {
            _animator.SetInteger(name, resultInt);
            return;
        }

        Debug.LogWarning("is not a valid bool or int value.");
    }
    public void ChangeSpeed(string name, float value)
    {
        _animator.SetFloat(name, value);
    }
}
