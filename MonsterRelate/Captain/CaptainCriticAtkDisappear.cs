using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptainCriticAtkDisappear : MonoBehaviour
{
    private MonsterBlockController _blockController;

    private void Start()
    {
        _blockController = GameObject.Find("Boss2").GetComponent<MonsterBlockController>();
    }
    void Update()
    {
        if (CaptainController.isCriticAtkHurted || _blockController.isWeak || CaptainController.SecondPhaseTimerSwitch || _blockController.BeBlockSuccess)
        {
            Destroy(this.gameObject);
        }
    }
}
