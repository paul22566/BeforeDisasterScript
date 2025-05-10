using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public Transform Target;
    private Transform _transform;
    public float XFixedDistance;
    public float YFixedDistance;
    // Start is called before the first frame update
    void Start()
    {
        _transform = transform;

        if (Target == null)
        {
            return;
        }
        _transform.position = new Vector3(Target.position.x + XFixedDistance, Target.position.y + YFixedDistance, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Target == null)
        {
            return;
        }
        _transform.position = new Vector3(Target.position.x + XFixedDistance, Target.position.y + YFixedDistance, 0);
    }
}
