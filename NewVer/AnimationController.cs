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
    protected const int CanColorAniNumber = 15;
    protected SpriteRenderer[] _aniSprArray;

    public virtual void AniPlay()
    {
        _aniObject.SetActive(true);
    }
    public virtual void AniStop()
    {
        _aniObject.SetActive(false);
        ChangeAniColor(new Color(1, 1, 1, 1));
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
    public void ChangeAniColor(Color color)
    {
        if (_aniSprArray == null)
        {
            return;
        }

        for (int i = 0; i < _aniSprArray.Length; i++)
        {
            _aniSprArray[i].color = color;
        }
    }
    protected void InitializeAni(int order, Transform objectTransform, float aniScale, int priority)
    {
        if (order < objectTransform.childCount)
        {
            _aniObject = objectTransform.GetChild(order).gameObject;
            _aniTransform = _aniObject.transform.GetChild(0);
            _animator = _aniTransform?.GetComponent<Animator>();
        }
        else
        {
            Debug.LogWarning("AniTransformOutRange");
        }
        _aniScale = aniScale;
        PlayPriority = priority;

        InitializeSprite();
    }
    private void InitializeSprite()
    {
        _aniSprArray = new SpriteRenderer[CanColorAniNumber];

        if (_aniTransform.childCount >= CanColorAniNumber)
        {
            for (int i = 0; i < CanColorAniNumber; i++)
            {
                _aniSprArray[i] = _aniTransform.GetChild(i)?.GetComponent<SpriteRenderer>();
                if (_aniSprArray[i] == null)
                {
                    Debug.LogWarning("AniDontHaveSprite");
                }
            }
        }
        else
        {
            Debug.LogWarning("AniSpriteChildOutRange");
        }
    }
}
