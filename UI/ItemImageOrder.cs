using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemImageOrder : MonoBehaviour
{
    [HideInInspector] public int Order;//script(GetItem)
    private float MoveSpeed = 350;
    private int UpNumber;
    private float TimerSet = 0.5f;
    private float Timer;

    // Start is called before the first frame update
    void Start()
    {
        Timer = TimerSet;
        Order = ItemManage.ItemImageTotalNumber;
    }

    // Update is called once per frame
    void Update()
    {
        if(Order < ItemManage.ItemImageTotalNumber)
        {
            UpNumber += 1;
            Order += 1;
        }
        if (Order > ItemManage.ItemImageTotalNumber)
        {
            Order = ItemManage.ItemImageTotalNumber;
        }
    }

    private void FixedUpdate()
    {
        if(UpNumber > 0)
        {
            DialogUp();
        }
    }

    private void DialogUp()
    {
        this.gameObject.transform.position += new Vector3(0, MoveSpeed * Time.fixedDeltaTime, 0);
        Timer -= Time.fixedDeltaTime;
        if (Timer <= 0)
        {
            UpNumber -= 1;
            Timer = TimerSet;
        }
    }
}
