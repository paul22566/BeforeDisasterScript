using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestRoomEnemyAppear : MonoBehaviour
{
    public GameObject AppearMonster;
    public float Timer;
    private int OrderRecord;
    private Transform AtkTemporaryArea;

    private void Start()
    {
        OrderRecord = RestRoomController.RestRoomKilledNumber;
        AtkTemporaryArea = this.transform.GetChild(0);
    }
    // Update is called once per frame
    void Update()
    {
        Timer -= Time.deltaTime;
        if(Timer <= 0)
        {
            Instantiate(AppearMonster, this.gameObject.transform.position, Quaternion.identity, AtkTemporaryArea);
            if(AtkTemporaryArea.GetChild(0).GetComponent<MonsterBasicData>() != null)
            {
                AtkTemporaryArea.GetChild(0).GetComponent<MonsterBasicData>().Order = OrderRecord;
            }
            AtkTemporaryArea.DetachChildren();
            Destroy(this.gameObject);
        }
    }
}
