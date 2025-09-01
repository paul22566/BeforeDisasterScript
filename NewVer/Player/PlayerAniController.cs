using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAniController
{
    public PlayerWaitAni waitAni;
    public PlayerRunAni runAni;
    public PlayerJumpAni jumpAni;
    public PlayerShootAni shootAni;
    public PlayerDashAni dashAni;
    public PlayerHurtedAni hurtedAni;
    public PlayerNormalAtkAni normalAtkAni;
    public PlayerStrongAtkAni strongAtkAni;
    public PlayerCriticAtkAni criticAtkAni;
    public PlayerThrowItemAni throwItemAni;
    public PlayerWalkThrowAni walkThrowAni;
    public PlayerJumpThrowAni jumpThrowAni;
    public PlayerUseItemAni useItemAni;
    public PlayerBlockAni blockAni;
    public PlayerBlockAtkAni blockAtkAni;
    public PlayerBeBlockAni beBlockAtkAni;
    public PlayerWeakAni weakAni;
    public PlayerDieAni dieAni;
    public PlayerRestoreAni restoreAni;
    public PlayerCocktailCriticAtkAni cocktailAni;

    public PlayerAniController(Transform _transform, float scale)
    {
        waitAni = new PlayerWaitAni(0, _transform, scale, 1);
        runAni = new PlayerRunAni(0, _transform, scale, 2);
        dashAni = new PlayerDashAni(3, _transform, 1);
        jumpAni = new PlayerJumpAni(1, _transform, scale, 3);
        shootAni = new PlayerShootAni(9, _transform, scale, 4);
        normalAtkAni = new PlayerNormalAtkAni(4, _transform, scale, 4);
        strongAtkAni = new PlayerStrongAtkAni(6, _transform, scale, 1);
        criticAtkAni = new PlayerCriticAtkAni(7, _transform, scale, 1);
        throwItemAni = new PlayerThrowItemAni(8, _transform, scale, 1);
        walkThrowAni = new PlayerWalkThrowAni(0, _transform, scale, 3);
        jumpThrowAni = new PlayerJumpThrowAni(4, _transform, scale, 4);
        useItemAni = new PlayerUseItemAni(20, _transform, scale, 1);
        blockAni = new PlayerBlockAni(10, _transform, scale, 1);
        blockAtkAni = new PlayerBlockAtkAni(10, _transform, scale, 2);
        beBlockAtkAni = new PlayerBeBlockAni(11, _transform, scale, 1);
        weakAni = new PlayerWeakAni(12, _transform, scale, 1);
        hurtedAni = new PlayerHurtedAni(17, _transform, scale, 5);
        dieAni = new PlayerDieAni(18, _transform, scale, 1);
        restoreAni = new PlayerRestoreAni(14, _transform, scale, 1);
        cocktailAni = new PlayerCocktailCriticAtkAni(16, _transform, scale, 1);
    }
}

public class PlayerWaitAni : AnimationController
{
    public PlayerWaitAni(int order, Transform objectTransform, float aniScale, int priority)
    {
        InitializeAni(order, objectTransform, aniScale, priority);
    }
}
public class PlayerRunAni : AnimationController
{
    public PlayerRunAni(int order, Transform objectTransform, float aniScale, int priority)
    {
        InitializeAni(order, objectTransform, aniScale, priority);
    }

    public override void AniPlay()
    {
        _aniObject.SetActive(true);
        _animator.SetBool("Move", true);
    }
}
public class PlayerDashAni : AnimationController
{
    private SpriteRenderer sprite;
    public PlayerDashAni(int order, Transform objectTransform, int priority)
    {
        if (order < objectTransform.childCount)
        {
            _aniObject = objectTransform.GetChild(order).gameObject;
            sprite = _aniObject?.GetComponent<SpriteRenderer>();
        }
        else
        {
            Debug.LogWarning("AniTransformOutRange");
        }

        PlayPriority = priority;
    }

    public override void AniTurnFace(Creature.Face face)
    {
        switch (face)
        {
            case Creature.Face.Left:
                sprite.flipX = true;
                break;
            case Creature.Face.Right:
                sprite.flipX = false;
                break;
        }
    }
}
public class PlayerNormalAtkAni : AnimationController
{
    public PlayerNormalAtkAni(int order, Transform objectTransform, float aniScale, int priority)
    {
        InitializeAni(order, objectTransform, aniScale, priority);
    }
}
public class PlayerStrongAtkAni : AnimationController
{
    public PlayerStrongAtkAni(int order, Transform objectTransform, float aniScale, int priority)
    {
        InitializeAni(order, objectTransform, aniScale, priority);
    }
}
public class PlayerCriticAtkAni : AnimationController
{
    public PlayerCriticAtkAni(int order, Transform objectTransform, float aniScale, int priority)
    {
        InitializeAni(order, objectTransform, aniScale, priority);
    }
}
public class PlayerJumpAni : AnimationController
{
    public PlayerJumpAni(int order, Transform objectTransform, float aniScale, int priority)
    {
        InitializeAni(order, objectTransform, aniScale, priority);
    }
}
public class PlayerShootAni : AnimationController
{
    public PlayerShootAni(int order, Transform objectTransform, float aniScale, int priority)
    {
        InitializeAni(order, objectTransform, aniScale, priority);
    }
}
public class PlayerThrowItemAni : AnimationController
{
    private GameObject CocktailImage;
    private GameObject ExplosionBottleImage;
    public PlayerThrowItemAni(int order, Transform objectTransform, float aniScale, int priority)
    {
        InitializeAni(order, objectTransform, aniScale, priority);
    }
    public void PlayThrowItemAni(ItemManage.UsefulItem item)
    {
        if(CocktailImage == null ||  ExplosionBottleImage == null)
        {
            Debug.LogError("ImageNull");
            return;
        }

        CocktailImage.SetActive(false);
        ExplosionBottleImage.SetActive(false);

        switch (item)
        {
            case ItemManage.UsefulItem.Cocktail:
                CocktailImage.SetActive(true);
                break;
            case ItemManage.UsefulItem.ExplosionBottle:
                ExplosionBottleImage.SetActive(true);
                break;
        }
    }
    public void AssignImage(GameObject cocktail, GameObject explosion)
    {
        CocktailImage = cocktail;
        ExplosionBottleImage = explosion;
    }
}
public class PlayerWalkThrowAni : AnimationController
{
    private GameObject CocktailImage;
    private GameObject ExplosionBottleImage;
    public PlayerWalkThrowAni(int order, Transform objectTransform, float aniScale, int priority)
    {
        InitializeAni(order, objectTransform, aniScale, priority);
    }

    public override void AniPlay()
    {
        _aniObject.SetActive(true);
        _animator.SetBool("Throw", true);
    }
    public override void AniStop()
    {
        _aniObject.SetActive(false);
        CocktailImage.SetActive(false);
        ExplosionBottleImage.SetActive(false);
    }
    public void PlayThrowItemAni(ItemManage.UsefulItem item)
    {
        if (CocktailImage == null || ExplosionBottleImage == null)
        {
            Debug.LogError("ImageNull");
            return;
        }

        CocktailImage.SetActive(false);
        ExplosionBottleImage.SetActive(false);

        switch (item)
        {
            case ItemManage.UsefulItem.Cocktail:
                CocktailImage.SetActive(true);
                break;
            case ItemManage.UsefulItem.ExplosionBottle:
                ExplosionBottleImage.SetActive(true);
                break;
        }
    }
    public void AssignImage(GameObject cocktail, GameObject explosion)
    {
        CocktailImage = cocktail;
        ExplosionBottleImage = explosion;
    }
}
public class PlayerJumpThrowAni : AnimationController
{
    private GameObject CocktailImage;
    private GameObject ExplosionBottleImage;
    public PlayerJumpThrowAni(int order, Transform objectTransform, float aniScale, int priority)
    {
        InitializeAni(order, objectTransform, aniScale, priority);
    }

    public override void AniPlay()
    {
        _aniObject.SetActive(true);
        _animator.SetBool("Throw", true);
    }
    public override void AniStop()
    {
        _aniObject.SetActive(false);
        CocktailImage.SetActive(false);
        ExplosionBottleImage.SetActive(false);
    }
    public void PlayThrowItemAni(ItemManage.UsefulItem item)
    {
        if (CocktailImage == null || ExplosionBottleImage == null)
        {
            Debug.LogError("ImageNull");
            return;
        }

        CocktailImage.SetActive(false);
        ExplosionBottleImage.SetActive(false);

        switch (item)
        {
            case ItemManage.UsefulItem.Cocktail:
                CocktailImage.SetActive(true);
                break;
            case ItemManage.UsefulItem.ExplosionBottle:
                ExplosionBottleImage.SetActive(true);
                break;
        }
    }
    public void AssignImage(GameObject cocktail, GameObject explosion)
    {
        CocktailImage = cocktail;
        ExplosionBottleImage = explosion;
    }
}
public class PlayerUseItemAni : AnimationController
{
    private Transform RitualParticle;
    public PlayerUseItemAni(int order, Transform objectTransform, float aniScale, int priority)
    {
        InitializeAni(order, objectTransform, aniScale, priority);
    }

    public override void AniTurnFace(Creature.Face face)
    {
        base.AniTurnFace(face);
        switch (face)
        {
            case Creature.Face.Right:
                RitualParticle.transform.localRotation = Quaternion.Euler(0,0,-174f);
                break;
            case Creature.Face.Left:
                RitualParticle.transform.localRotation = Quaternion.Euler(0, 0, -274.2f);
                break;
        }
    }
    public void AssignParticle(Transform ritual)
    {
        RitualParticle= ritual;
    }
}
public class PlayerBlockAni : AnimationController
{
    public PlayerBlockAni(int order, Transform objectTransform, float aniScale, int priority)
    {
        InitializeAni(order, objectTransform, aniScale, priority);
    }
}
public class PlayerBlockAtkAni : AnimationController
{
    public PlayerBlockAtkAni(int order, Transform objectTransform, float aniScale, int priority)
    {
        InitializeAni(order, objectTransform, aniScale, priority);
    }
}
public class PlayerBeBlockAni : AnimationController
{
    public PlayerBeBlockAni(int order, Transform objectTransform, float aniScale, int priority)
    {
        InitializeAni(order, objectTransform, aniScale, priority);
    }
}
public class PlayerWeakAni : AnimationController
{
    public PlayerWeakAni(int order, Transform objectTransform, float aniScale, int priority)
    {
        InitializeAni(order, objectTransform, aniScale, priority);
    }
}
public class PlayerHurtedAni : AnimationController
{
    public PlayerHurtedAni(int order, Transform objectTransform, float aniScale, int priority)
    {
        InitializeAni(order, objectTransform, aniScale, priority);
    }
}
public class PlayerDieAni : AnimationController
{
    public PlayerDieAni(int order, Transform objectTransform, float aniScale, int priority)
    {
        InitializeAni(order, objectTransform, aniScale, priority);
    }
}
public class PlayerRestoreAni : AnimationController
{
    public PlayerRestoreAni(int order, Transform objectTransform, float aniScale, int priority)
    {
        InitializeAni(order, objectTransform, aniScale, priority);
    }
}
public class PlayerCocktailCriticAtkAni : AnimationController
{
    public PlayerCocktailCriticAtkAni(int order, Transform objectTransform, float aniScale, int priority)
    {
        InitializeAni(order, objectTransform, aniScale, priority);
    }
}

public class PlayerAimLineAnimation
{
    private GameObject LPredictPowerCurve;
    private GameObject LPowerLine;
    private GameObject LAimPoint;
    private GameObject RPredictPowerCurve;
    private GameObject RPowerLine;
    private GameObject RAimPoint;

    private float LineLengthRate;//長度轉換成localScale的比例

    public PlayerAimLineAnimation(BattleSystem battle)
    {
        LPredictPowerCurve = battle.LPredictPowerBase;
        LPowerLine = battle.LPowerLine;
        LAimPoint = battle.LAimPoint;
        RPredictPowerCurve = battle.RPredictPowerBase;
        RPowerLine = battle.RPowerLine;
        RAimPoint = battle.RAimPoint;

        InisializeAimLine();

        AniClose(Creature.Face.Left);
        AniClose(Creature.Face.Right);
    }

    public void AniOpen(Creature.Face face)
    {
        switch (face)
        {
            case Creature.Face.Right:
                RPredictPowerCurve.SetActive(true);
                RPowerLine.SetActive(true);
                RAimPoint.SetActive(true);
                break;
            case Creature.Face.Left:
                LPredictPowerCurve.SetActive(true);
                LPowerLine.SetActive(true);
                LAimPoint.SetActive(true);
                break;
        }
    }
    public void AniClose(Creature.Face face)
    {
        switch (face)
        {
            case Creature.Face.Right:
                RPredictPowerCurve.SetActive(false);
                RPowerLine.SetActive(false);
                RAimPoint.SetActive(false);
                break;
            case Creature.Face.Left:
                LPredictPowerCurve.SetActive(false);
                LPowerLine.SetActive(false);
                LAimPoint.SetActive(false);
                break;
        }
    }
    public void OperateAni(Creature.Face face, (float, float) AimPower, Vector3 playerLocation)
    {
        OperateAimPowerPoint(face, AimPower);
        OperateAimPowerLine(face);
        OperateAimPowerCurve(face, playerLocation);
    }

    private void OperateAimPowerPoint(Creature.Face face, (float, float) AimPower)
    {
        switch (face)
        {
            case Creature.Face.Left:
                LAimPoint.transform.localPosition = new Vector3(-1.01f + AimPower.Item1 * 0.299f, 1.29f + AimPower.Item2 * 0.295f, 0);
                break;
            case Creature.Face.Right:
                RAimPoint.transform.localPosition = new Vector3(1.01f + AimPower.Item1 * 0.299f, 1.29f + AimPower.Item2 * 0.295f, 0);
                break;
        }
    }
    private void OperateAimPowerLine(Creature.Face face)
    {
        float DistanceX;
        float DistanceY;
        float NowLength;
        float Angel;
        switch (face)
        {
            case Creature.Face.Left:
                DistanceX = Mathf.Abs(LPowerLine.transform.localPosition.x - LAimPoint.transform.localPosition.x);
                DistanceY = Mathf.Abs(LPowerLine.transform.localPosition.y - LAimPoint.transform.localPosition.y);
                //決定長度
                NowLength = Mathf.Pow(DistanceX * DistanceX + DistanceY * DistanceY, 0.5f);
                LPowerLine.transform.localScale = new Vector3(NowLength * LineLengthRate, LPowerLine.transform.localScale.y, 0);
                //決定角度
                Angel = Mathf.Atan2(DistanceY, DistanceX) * 180 / Mathf.PI;
                LPowerLine.transform.rotation = Quaternion.Euler(0, 0, 180 - Angel);
                break;
            case Creature.Face.Right:
                DistanceX = Mathf.Abs(RPowerLine.transform.localPosition.x - RAimPoint.transform.localPosition.x);
                DistanceY = Mathf.Abs(RPowerLine.transform.localPosition.y - RAimPoint.transform.localPosition.y);
                //決定長度
                NowLength = Mathf.Pow(DistanceX * DistanceX + DistanceY * DistanceY, 0.5f);
                RPowerLine.transform.localScale = new Vector3(NowLength * LineLengthRate, RPowerLine.transform.localScale.y, 0);
                //決定角度
                Angel = Mathf.Atan2(DistanceY, DistanceX) * 180 / Mathf.PI;
                RPowerLine.transform.rotation = Quaternion.Euler(0, 0, Angel);
                break;
        }
    }
    private void InisializeAimLine()
    {
        float DistanceX = Mathf.Abs(RPowerLine.transform.localPosition.x - RAimPoint.transform.localPosition.x);
        float DistanceY = Mathf.Abs(RPowerLine.transform.localPosition.y - RAimPoint.transform.localPosition.y);
        LineLengthRate = 0.05f / Mathf.Pow(DistanceX * DistanceX + DistanceY * DistanceY, 0.5f);
    }
    private void OperateAimPowerCurve(Creature.Face face, Vector3 playerLocation)
    {
        float NowScaleX;
        float NowScaleY;
        float NowRotation;

        float UpLimit = 4.24f;
        float DownLimit = 1.29f;
        float RightLimit = 4f;
        float LeftLimit = 1.01f;
        float NowUpLimit;
        float NowDownLimit;
        float NowRightLimit;
        float NowLeftLimit;
        float LeftUpPointScaleX = 0.124f;
        float LeftUpPointScaleY = 0.248f;
        float RightUpPointScaleX = 0.528f;
        float RightUpPointScaleY = 0.163f;
        float LeftDownPointScaleX = 0.078f;
        float LeftDownPointScaleY = 0.013f;
        float RightDownPointScaleX = 0.33f;
        float RightDownPointScaleY = 0.047f;

        float NowProportionX = 0;//當前X軸比例 由左而右
        float NowProportionY = 0;//當前Y軸比例 由下而上
        float NowUpProportionXScale;//上半段平均Xscale
        float NowDownProportionXScale;//下半段平均Xscale
        float NowUpProportionYScale;//上半段平均Yscale
        float NowDownProportionYScale;//下半段平均Yscale
        float BottomRotation = -20;

        switch (PlayerController._player.face)
        {
            case Creature.Face.Left:
                LPredictPowerCurve.transform.localPosition = LAimPoint.transform.localPosition;
                break;
            case Creature.Face.Right:
                RPredictPowerCurve.transform.localPosition = RAimPoint.transform.localPosition;
                break;
        }

        NowRightLimit = playerLocation.x + RightLimit;
        NowLeftLimit = playerLocation.x + LeftLimit;
        NowUpLimit = playerLocation.y + UpLimit;
        NowDownLimit = playerLocation.y + DownLimit;
        //實際計算
        switch (face)
        {
            case Creature.Face.Left:
                NowProportionY = (LPredictPowerCurve.transform.position.y - NowDownLimit) / (NowUpLimit - NowDownLimit);
                NowProportionX = (playerLocation.x - LeftLimit - LPredictPowerCurve.transform.position.x) / (NowRightLimit - NowLeftLimit);
                break;
            case Creature.Face.Right:
                NowProportionY = (RPredictPowerCurve.transform.position.y - NowDownLimit) / (NowUpLimit - NowDownLimit);
                NowProportionX = (RPredictPowerCurve.transform.position.x - NowLeftLimit) / (NowRightLimit - NowLeftLimit);
                break;
        }
        NowUpProportionXScale = LeftUpPointScaleX * (1 - NowProportionX) + RightUpPointScaleX * NowProportionX;
        NowDownProportionXScale = LeftDownPointScaleX * (1 - NowProportionX) + RightDownPointScaleX * NowProportionX;
        NowScaleX = NowDownProportionXScale * (1 - NowProportionY) + NowUpProportionXScale * NowProportionY;
        NowUpProportionYScale = LeftUpPointScaleY * (1 - NowProportionX) + RightUpPointScaleY * NowProportionX;
        NowDownProportionYScale = LeftDownPointScaleY * (1 - NowProportionX) + RightDownPointScaleY * NowProportionX;
        NowScaleY = NowDownProportionYScale * (1 - NowProportionY) + NowUpProportionYScale * NowProportionY;
        switch (face)
        {
            case Creature.Face.Left:
                NowRotation = BottomRotation * (1 - NowProportionY) * -1;
                LPredictPowerCurve.transform.localScale = new Vector3(NowScaleX, NowScaleY, 0);
                LPredictPowerCurve.transform.rotation = Quaternion.Euler(0, 0, NowRotation);
                break;
            case Creature.Face.Right:
                NowRotation = BottomRotation * (1 - NowProportionY);
                RPredictPowerCurve.transform.localScale = new Vector3(NowScaleX, NowScaleY, 0);
                RPredictPowerCurve.transform.rotation = Quaternion.Euler(0, 0, NowRotation);
                break;
        }
    }
}
