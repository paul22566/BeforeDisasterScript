using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasterMagicAni : MonoBehaviour
{
    private float Timer;
    public float TimerSet;
    private GameObject Monster;
    private MonsterHurtedController _hurtedController;
    private void Awake()
    {
        Monster = transform.parent.parent.gameObject;
        _hurtedController = transform.parent.parent.GetComponent<MonsterHurtedController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        Timer = TimerSet;
    }

    // Update is called once per frame
    void Update()
    {
        if (_hurtedController.isCriticAtkHurted || _hurtedController.isHurtedByBigGun || Monster == null)
        {
            Destroy(this.gameObject);
            return;
        }
        Timer -= Time.deltaTime;
        if (Timer <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
