using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowItemAniChange : MonoBehaviour
{
    public itemManage _itemManage;
    public GameObject Cocktail;
    public GameObject ExplosionBottle;

    public void JudgeThrowType()
    {
        switch (_itemManage.NowPrepareItem)
        {
            case itemManage.PrepareItem.Cocktail:
                Cocktail.SetActive(true);
                ExplosionBottle.SetActive(false);
                break;
            case itemManage.PrepareItem.ExplosionBottle:
                Cocktail.SetActive(false);
                ExplosionBottle.SetActive(true);
                break;
        }
    }

    public void TurnOffThrow()
    {
        Cocktail.SetActive(false);
        ExplosionBottle.SetActive(false);
    }
}
