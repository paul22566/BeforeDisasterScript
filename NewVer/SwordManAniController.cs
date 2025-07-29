using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SwordManAniController
{
    public SwordManAlertAni alertAni;
    public SwordManWalkAni walkAni;
    public SwordManRunAni runAni;
    public SwordManAtkAni normalAtkAni;
    public SwordManStringAtkAni stringAtkAni;
    public SwordManCooldownAni cooldownAni;

    public SwordManAniController(Transform _transform, float scale)
    {
        alertAni = new SwordManAlertAni(3, _transform, scale);
        walkAni = new SwordManWalkAni(3, _transform, scale);
        runAni = new SwordManRunAni(0, _transform, scale);
        normalAtkAni = new SwordManAtkAni(1, _transform, scale);
        stringAtkAni = new SwordManStringAtkAni(2, _transform, scale);
        cooldownAni = new SwordManCooldownAni(3, _transform, scale);
    }
}

public class SwordManAlertAni : AnimationController
{
    public SwordManAlertAni(int order, Transform objectTransform, float aniScale)
    {
        _aniObject = objectTransform.GetChild(order).gameObject;
        _aniTransform = _aniObject.transform.GetChild(0);
        _animator = _aniTransform.GetComponent<Animator>();
        _aniScale = aniScale;
    }

    public override void AniPlay()
    {
        _aniObject.SetActive(true);
        _animator.SetBool("isSlowWalk", true);
    }
}

public class SwordManWalkAni: AnimationController
{
    public SwordManWalkAni(int order, Transform objectTransform, float aniScale)
    {
        _aniObject = objectTransform.GetChild(order).gameObject;
        _aniTransform = _aniObject.transform.GetChild(0);
        _animator = _aniTransform.GetComponent<Animator>();
        _aniScale = aniScale;
    }

    public override void AniPlay()
    {
        _aniObject.SetActive(true);
        _animator.SetBool("isSlowWalk", true);
    }
}

public class SwordManRunAni : AnimationController
{
    public SwordManRunAni(int order, Transform objectTransform, float aniScale)
    {
        _aniObject = objectTransform.GetChild(order).gameObject;
        _aniTransform = _aniObject.transform.GetChild(0);
        _animator = _aniTransform.GetComponent<Animator>();
        _aniScale = aniScale;
    }

    public override void AniPlay()
    {
        _aniObject.SetActive(true);
        _animator.SetBool("isWalking", true);
    }
}

public class SwordManAtkAni : AnimationController
{
    public SwordManAtkAni(int order, Transform objectTransform, float aniScale)
    {
        _aniObject = objectTransform.GetChild(order).gameObject;
        _aniTransform = _aniObject.transform.GetChild(0);
        _animator = _aniTransform.GetComponent<Animator>();
        _aniScale = aniScale;
    }
}

public class SwordManStringAtkAni : AnimationController
{
    public SwordManStringAtkAni(int order, Transform objectTransform, float aniScale)
    {
        _aniObject = objectTransform.GetChild(order).gameObject;
        _aniTransform = _aniObject.transform.GetChild(0);
        _animator = _aniTransform.GetComponent<Animator>();
        _aniScale = aniScale;
    }
}

public class SwordManCooldownAni : AnimationController
{
    public SwordManCooldownAni(int order, Transform objectTransform, float aniScale)
    {
        _aniObject = objectTransform.GetChild(order).gameObject;
        _aniTransform = _aniObject.transform.GetChild(0);
        _animator = _aniTransform.GetComponent<Animator>();
        _aniScale = aniScale;
    }

    public override void AniPlay()
    {
        _aniObject.SetActive(true);
        _animator.SetBool("isSlowWalk", true);
        _animator.SetFloat("Speed", -1);
    }

    public override void AniStop()
    {
        _aniObject.SetActive(false);
        _animator.SetBool("isSlowWalk", false);
        _animator.SetFloat("Speed", 1);
    }
}
