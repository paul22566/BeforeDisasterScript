using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseObject : MonoBehaviour
{
    public bool FollowPlayer;

    public Transform Target;
    private Transform _transform;

    public bool LockX;
    public bool LockY;

    public Transform LeftLimtObject;
    public Transform RightLimtObject;
    public Transform TopLimtObject;
    public Transform BottomLimtObject;

    private float LeftLimt;
    private float RightLimt;
    private float TopLimt;
    private float BottomLimt;
    // Start is called before the first frame update
    void Start()
    {
        _transform = this.transform;
        if (FollowPlayer)
        {
            if (GameObject.Find("player") != null)
            {
                Target = GameObject.Find("player").transform;
            }
        }

        if (LeftLimtObject != null)
        {
            LeftLimt = LeftLimtObject.position.x;
        }
        if (RightLimtObject != null)
        {
            RightLimt = RightLimtObject.position.x;
        }
        if (TopLimtObject != null)
        {
            TopLimt = TopLimtObject.position.y;
        }
        if (BottomLimtObject != null)
        {
            BottomLimt = BottomLimtObject.position.y;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Target == null)
        {
            return;
        }

        Vector3 FollowPoint = new Vector3();

        FollowPoint = Target.position;

        if (LockX)
        {
            FollowPoint = new Vector3(_transform.position.x, FollowPoint.y, FollowPoint.z);
        }
        if (LockY)
        {
            FollowPoint = new Vector3(FollowPoint.x, _transform.position.y, FollowPoint.z);
        }

        if (!LockX)
        {
            if (LeftLimtObject != null && FollowPoint.x < LeftLimt)
            {
                FollowPoint = new Vector3(LeftLimt, FollowPoint.y, FollowPoint.z);
            }
            if (RightLimtObject != null && FollowPoint.x > RightLimt)
            {
                FollowPoint = new Vector3(RightLimt, FollowPoint.y, FollowPoint.z);
            }
        }
        if (!LockY)
        {
            if (TopLimtObject != null && FollowPoint.y > TopLimt)
            {
                FollowPoint = new Vector3(FollowPoint.x, TopLimt, FollowPoint.z);
            }
            if (BottomLimtObject != null && FollowPoint.y < BottomLimt)
            {
                FollowPoint = new Vector3(FollowPoint.x, BottomLimt, FollowPoint.z);
            }
        }

        _transform.position = FollowPoint;
    }
}
