using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDieController : MonoBehaviour
{
    public Transform RMonsterDieAnimation;
    public Transform LMonsterDieAnimation;

    [Header("燃燒死亡(不一定要放)")]
    public Transform RMonsterBurningDieAnimation;
    public Transform LMonsterBurningDieAnimation;

    public void BeginDie(MonsterBornController _MBController, int MapNumber, int Order, MonsterDeadInformation deadInfo)
    {
        MonsterBasicData _basicData = this.GetComponent<MonsterBasicData>();

        BattleSystem.KillerPoint += 1;
        if (MapNumber != 999)
        {
            _MBController.MonsterBornList[MapNumber][Order] = false;
        }
        if (deadInfo.FaceRight)
        {
            if (!deadInfo.BurningDie)
            {
                RMonsterDieAnimation.position = _basicData.MonsterPlace;
                RMonsterDieAnimation.gameObject.SetActive(true);
            }
            else
            {
                RMonsterBurningDieAnimation.position = _basicData.MonsterPlace;
                RMonsterBurningDieAnimation.gameObject.SetActive(true);
            }
        }
        if (deadInfo.FaceLeft)
        {
            if (!deadInfo.BurningDie)
            {
                LMonsterDieAnimation.position = _basicData.MonsterPlace;
                LMonsterDieAnimation.gameObject.SetActive(true);
            }
            else
            {
                LMonsterBurningDieAnimation.position = _basicData.MonsterPlace;
                LMonsterBurningDieAnimation.gameObject.SetActive(true);
            }
        }
        Destroy(this.gameObject);
    }

    public void BeginDie(MonsterDeadInformation deadInfo)
    {
        MonsterBasicData _basicData = this.GetComponent<MonsterBasicData>();

        BattleSystem.KillerPoint += 1;
        if (deadInfo.FaceRight)
        {
            if (!deadInfo.BurningDie)
            {
                RMonsterDieAnimation.position = _basicData.MonsterPlace;
                RMonsterDieAnimation.gameObject.SetActive(true);
            }
            else
            {
                RMonsterBurningDieAnimation.position = _basicData.MonsterPlace;
                RMonsterBurningDieAnimation.gameObject.SetActive(true);
            }
        }
        if (deadInfo.FaceLeft)
        {
            if (!deadInfo.BurningDie)
            {
                LMonsterDieAnimation.position = _basicData.MonsterPlace;
                LMonsterDieAnimation.gameObject.SetActive(true);
            }
            else
            {
                LMonsterBurningDieAnimation.position = _basicData.MonsterPlace;
                LMonsterBurningDieAnimation.gameObject.SetActive(true);
            }
        }
        Destroy(this.gameObject);
    }

    public void RecordDiePosition(MonsterDeadInformation deadInfo, Vector3 Position)
    {
        deadInfo.DiePosition = Position;
    }
}
