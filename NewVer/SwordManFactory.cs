using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordManFactory : MonoBehaviour, IMonsterFactory
{
    //[SerializeField] private bool UseDefaultData;
    [SerializeField] private MonsterData baseData;
    [SerializeField] private SwordManData swordManData;

    private NewSwordManController _controller;
    private string path = "prefab/Monster/NewMonster/SwordMan/";

    private void Awake()
    {
        _controller = new NewSwordManController(baseData, swordManData, this.transform);
    }

    public Monster CreateMonster()
    {
        Monster monster = _controller;
        return monster;
    }
}
