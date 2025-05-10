using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBornController : MonoBehaviour
{
    private int TotalMapNumber = 4;//只限有普通小怪的場景
    [HideInInspector] public List<bool>[] MonsterBornList;
    //該場景小怪數量
    private int HallMonsterNumber = 3;
    private int F1_1MonsterNumber = 2;
    private int F1_2MonsterNumber = 4;
    private int F2_1MonsterNumber = 3;
    /*private int F3_1MonsterNumber = 7;
    private int F4_1MonsterNumber = 4;
    private int F4_2MonsterNumber = 3;
    private int F5MonsterNumber = 5;
    private int FB1_1MonsterNumber = 2;
    private int FB1_2MonsterNumber = 3;*/
    private int NowMapMonsterNumber;

    private bool isReset;
    // Start is called before the first frame update
    void Start()
    {
        MonsterBornList = new List<bool>[TotalMapNumber];
        for (int i = 0; i < TotalMapNumber; i++)
        {
            MonsterBornList[i] = new List<bool>();
        }
        for (int i = 0; i < TotalMapNumber; i++)
        {
            //MapNumber對照點
            switch (i)
            {
                case 0:
                    NowMapMonsterNumber = HallMonsterNumber;
                    break;
                case 1:
                    NowMapMonsterNumber = F1_1MonsterNumber;
                    break;
                case 2:
                    NowMapMonsterNumber = F1_2MonsterNumber;
                    break;
                case 3:
                    NowMapMonsterNumber = F2_1MonsterNumber;
                    break;
            }
            for (int a = 0; a < NowMapMonsterNumber; a++)
            {
                MonsterBornList[i].Add(true);
            }
        }
    }

    private void Update()
    {
        if(RestPlace.MonsterShouldDestroy || PlayerController.isDie)
        {
            if (!isReset)
            {
                for (int i = 0; i < TotalMapNumber; i++)
                {
                    switch (i)
                    {
                        case 0:
                            NowMapMonsterNumber = HallMonsterNumber;
                            break;
                        case 1:
                            NowMapMonsterNumber = F1_1MonsterNumber;
                            break;
                        case 2:
                            NowMapMonsterNumber = F1_2MonsterNumber;
                            break;
                        case 3:
                            NowMapMonsterNumber = F2_1MonsterNumber;
                            break;
                    }
                    for (int a = 0; a < NowMapMonsterNumber; a++)
                    {
                        MonsterBornList[i][a] = true;
                    }
                }
                isReset = true;
            }
        }
        if(!RestPlace.MonsterShouldDestroy && !PlayerController.isDie)
        {
            isReset = false;
        }
    }
}
