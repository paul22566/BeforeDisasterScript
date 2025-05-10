using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowRotate : MonoBehaviour
{
    public Transform Target;
    public float FixedAngle;
    private Transform _transform;

    private float Rotation;
    // Start is called before the first frame update
    void Start()
    {
        _transform = transform;
        Rotation = Target.eulerAngles.z;
        Rotation = Rotation + FixedAngle;
        _transform.rotation = Quaternion.Euler(0, 0, Rotation);
    }

    // Update is called once per frame
    void Update()
    {
        Rotation = Target.eulerAngles.z;
        Rotation = Rotation + FixedAngle;
        _transform.rotation = Quaternion.Euler(0, 0, Rotation);
    }
}
