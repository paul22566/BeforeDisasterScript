using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdFloor2Enemy : MonoBehaviour
{
    private bool isIncrease = false;
    public GameObject RMonsterDieAnimation;
    public GameObject LMonsterDieAnimation;

    // Update is called once per frame
    void Update()
    {
        if (!isIncrease)
        {
            if (this.gameObject.GetComponent<MonsterBasicData>().hp <= 0)
            {
                ThirdFloor2Controller.ThirdFloor2KilledNumber += 1;
                isIncrease = true;
                switch (this.gameObject.GetComponent<MonsterBasicData>().face)
                {
                    case MonsterBasicData.Face.Left:
                        Instantiate(LMonsterDieAnimation, this.transform.position, Quaternion.identity);
                        break;
                    case MonsterBasicData.Face.Right:
                        Instantiate(RMonsterDieAnimation, this.transform.position, Quaternion.identity);
                        break;
                }
                Destroy(this.gameObject);
                return;
            }
        }
    }
}
