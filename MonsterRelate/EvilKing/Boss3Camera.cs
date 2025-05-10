using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3Camera : MonoBehaviour
{
    public static bool isFighting;//script(Boss3Controller)
    public Transform FightingTarget;
    private Transform target;
    Transform _transform;
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("player") != null)
        {
            target = GameObject.Find("player").transform;
        }
        _transform = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null && !GameEvent.isAniPlay && !isFighting)
        {
            Vector3 followPos = new Vector3(target.position.x, target.position.y, _transform.position.z);
            _transform.position = followPos;
        }
        if (isFighting)
        {
            Vector3 followPos = new Vector3(FightingTarget.position.x, FightingTarget.position.y, _transform.position.z);
            _transform.position = followPos;
        }
    }
}
