using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdFloor2NormalSwordMan : MonoBehaviour
{
    public Animator ThisAni;
    public GameObject GhostMan;
    public int GetUpOrder;
    private float Timer = 2f;
    private int OrderRecord;

    private void Start()
    {
        OrderRecord = ThirdFloor2Controller.ThirdFloor2KilledNumber;
    }
    void Update()
    {
        if (ThirdFloor2Controller.ThirdFloor2KilledNumber >= GetUpOrder)
        {
            Timer -= Time.deltaTime;
            ThisAni.SetBool("GetUp", true);
            if (Timer <= 0)
            {
                Instantiate(GhostMan, this.transform.position, Quaternion.identity, this.transform);
                if (this.transform.GetChild(0).GetComponent<MonsterBasicData>() != null)
                {
                    this.transform.GetChild(0).GetComponent<MonsterBasicData>().Order = OrderRecord;
                }
                this.transform.DetachChildren();
                Destroy(this.gameObject);
            }
        }
    }
}
