using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterWeakJudgement : MonoBehaviour
{
    private GameObject Monster;
    [HideInInspector] public bool isWeak;//script(playerleftJudgement�AplayerRightJudgement)
    // Start is called before the first frame update
    private void Awake()
    {
        Monster = this.transform.parent.transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Monster != null && Monster.GetComponent<MonsterBlockController>()!=null)
        {
            if (Monster.GetComponent<MonsterBlockController>().isWeak)
            {
                isWeak = true;
            }
            else
            {
                isWeak = false;
            }
            /*switch (Monster.name)
            {
                case "�H��(�M)":
                    if (Monster.GetComponent<SwordManController>().isWeak)
                    {
                        isWeak = true;
                    }
                    else
                    {
                        isWeak = false;
                    }
                    break;
                case "�����M�k":
                    if (Monster.GetComponent<GhostManController>().isWeak)
                    {
                        isWeak = true;
                    }
                    else
                    {
                        isWeak = false;
                    }
                    break;
                case "����":
                    if (Monster.GetComponent<BigMonsterController>().isWeak)
                    {
                        isWeak = true;
                    }
                    else
                    {
                        isWeak = false;
                    }
                    break;
                case "Boss2":
                    if (CaptainController.isWeak)
                    {
                        isWeak = true;
                    }
                    else
                    {
                        isWeak = false;
                    }
                    break;
                case "Boss3":
                    if (EvilKingController.isWeak)
                    {
                        isWeak = true;
                    }
                    else
                    {
                        isWeak = false;
                    }
                    break;
            }*/
        }
    }
}
