using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreRoomDrunkMan : MonoBehaviour
{
    private MonsterBasicData _basicData;
    private MonsterHurtedController _hurtedController;
    public StoreRoomController _roomController;
    private GameObject Ani;
    private GameObject MonsterStopAnimation;
    private Transform MonsterStopTr;

    private void Start()
    {
        _basicData = this.GetComponent<MonsterBasicData>();
        _basicData.BasicVarInisialize(this.transform.GetChild(0), "R");
        _hurtedController = this.GetComponent<MonsterHurtedController>();

        Ani = this.transform.GetChild(0).gameObject;
        MonsterStopAnimation = this.transform.GetChild(2).gameObject;
        MonsterStopTr = MonsterStopAnimation.transform.GetChild(0);

        _hurtedController._getCriticHurted += PlayStopAni;

        this.transform.GetChild(1).DetachChildren();
    }

    private void Update()
    {
        _basicData.MonsterPlace = this.transform.position;

        if (_basicData.hp <= 0)
        {
            GameEvent.DrunkManDie = true;
            _roomController.DrunkManSveData();
            _basicData.ConfirmDie();
        }
        if (_basicData._deadInformation.BurningDie)
        {
            _basicData.ConfirmDie();
            return;
        }

        _hurtedController.CriticAtkHurtedTimerMethod(Time.deltaTime);
    }

    private void PlayStopAni()
    {
        switch (_basicData.face)
        {
            case MonsterBasicData.Face.Left:
                MonsterStopTr.localScale = new Vector3(-0.28f, 0.28f, 0);
                break;
            case MonsterBasicData.Face.Right:
                MonsterStopTr.localScale = new Vector3(0.28f, 0.28f, 0);
                break;
        }
        Ani.SetActive(false);
        MonsterStopAnimation.SetActive(true);
    }
}
