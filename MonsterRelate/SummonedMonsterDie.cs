using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonedMonsterDie : MonoBehaviour
{
    //���}���Φb�|�]�l��̦��`�Ӥ@�_������
    private GameObject _parent;
    public GameObject MonsterDie;
    private Transform _transform;
    private SpriteRenderer _spr;
    private void Awake()
    {
        _transform = transform;
        _spr = _transform.GetComponent<SpriteRenderer>();
        _parent = _transform.parent.GetComponent<BeSummonMonster>()._parent;
        _transform.rotation = Quaternion.Euler(0, 0, _transform.parent.eulerAngles.z);
        _spr.flipX = _transform.parent.GetComponent<BeSummonMonster>()._spr.flipX;
        //�̾ڥl��̪����P�h���w
        if(_parent.GetComponent<SmallCaptainController>() != null)
        {
            _parent.GetComponent<SmallCaptainController>().SummonMonster = this.gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(_parent == null)
        {
            Instantiate(MonsterDie, _transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
