using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilKingSpecialSituationDestroyAtk : MonoBehaviour
{
    public bool isForeverAtk;
    // Update is called once per frame
    void Update()
    {
        if (EvilKingController.isCriticAtkHurted || EvilKingController.isWeak || Boss3Controller.isFinalAtkDisappear)
        {
            Destroy(this.gameObject);
        }
        if (EvilKingController.isHurtedByBigGun && !isForeverAtk)
        {
            Destroy(this.gameObject);
        }
    }
}
