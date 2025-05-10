using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccumulateLightAni : MonoBehaviour
{
    private BattleSystem _battleSystem;
    private Transform PlayerTransform;
    private Transform _transform;
    // Start is called before the first frame update
    void Start()
    {
        _transform = transform;
        _battleSystem = GameObject.Find("player").GetComponent<BattleSystem>();
        PlayerTransform = GameObject.Find("player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        _transform.localPosition = PlayerTransform.localPosition;

        if (!_battleSystem.isAccumulate)
        {
            Destroy(this.gameObject);
        }
    }
}
