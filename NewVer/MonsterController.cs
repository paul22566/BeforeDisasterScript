using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour, IHurtedObject
{
    private IMonsterFactory _monsterFactory;

    private Monster _monster;
    // Start is called before the first frame update
    void Start()
    {
        _monsterFactory = this.GetComponent<IMonsterFactory>();
        _monster = _monsterFactory.CreateMonster();
    }

    // Update is called once per frame
    void Update()
    {
        _monster.UpdateLogic();
    }

    private void FixedUpdate()
    {
        _monster.DoAction();
    }

    public void HurtedControll(int damage)
    {
        _monster.HurtedControll(damage);
    }
    public int GetCamp()
    {
        return _monster.GetCamp();
    }
}
