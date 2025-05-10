using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RestRoomEnemy : MonoBehaviour
{
    public GameObject EscapeAnimation;
    private bool isIncrease = false;
    public bool HasItem;
    public RestRoomController _RoomController;

    private MonsterBasicData _basicData;
    private bool FaceJudge;
    private void Start()
    {
        _basicData = this.GetComponent<MonsterBasicData>();
        _basicData.TurnFaceJudge();
        if (this.GetComponent<SwordManController>() != null)
        {
            this.GetComponent<SwordManController>().status = SwordManController.Status.walk;
        }
        if (this.GetComponent<SmallCaptainController>() != null)
        {
            this.GetComponent<SmallCaptainController>().status = SmallCaptainController.Status.Atk3;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!FaceJudge)
        {
            _basicData.TurnFaceJudge();
            FaceJudge = true;
        }

        if (!isIncrease)
        {
            if (this.gameObject.GetComponent<MonsterBasicData>().hp <= 0 || this.gameObject.GetComponent<MonsterBasicData>()._deadInformation.BurningDie)
            {
                RestRoomController.RestRoomKilledNumber += 1;
                isIncrease = true;
                if (HasItem)
                {
                    _RoomController.Monster1DiePosition = this.transform.localPosition;
                }
                this.gameObject.GetComponent<MonsterBasicData>().ConfirmDie();
                return;
            }
        }

        if (!GameEvent.SkipRestRoom)
        {
            if (RestRoomController.RestRoomKilledNumber >= 6)
            {
                if (!(this.gameObject.GetComponent<MonsterBasicData>().hp <= 0))
                {
                    Instantiate(EscapeAnimation, this.transform.position, Quaternion.identity);
                }
                Destroy(this.gameObject);
            }
        }
        else
        {
            if (RestRoomController.RestRoomKilledNumber >= 5)
            {
                if (!(this.gameObject.GetComponent<MonsterBasicData>().hp <= 0))
                {
                    Instantiate(EscapeAnimation, this.transform.position, Quaternion.identity);
                }
                Destroy(this.gameObject);
            }
        }
    }
}
