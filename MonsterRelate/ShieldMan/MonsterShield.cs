using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterShield : MonoBehaviour
{
    private Transform _transform;
    private ShieldManController _controller;
    private MonsterBasicData _basicData;

    //鎖定位置
    public Transform MoveAniShield;
    public Transform Atk1AniShield;
    public Transform Atk2AniShield;
    public Transform BeBlockShield;

    //阻擋
    private float ShieldScaleX = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        _transform = this.transform;
        _controller = _transform.parent.GetComponent<ShieldManController>();
        _basicData = _controller.GetComponent<MonsterBasicData>();
    }

    // Update is called once per frame
    void Update()
    {
        //Follow
        switch (_controller.status)
        {
            case ShieldManController.Status.Atk:
                FollowObjectAndRotation(Atk1AniShield);
                break;
            case ShieldManController.Status.Atk2:
                FollowObjectAndRotation(Atk2AniShield);
                break;
            case ShieldManController.Status.BeBlock:
                FollowObjectAndRotation(BeBlockShield);
                break;
            case ShieldManController.Status.Weak:
                FollowObjectAndRotation(BeBlockShield);
                break;
            default:
                FollowObjectAndRotation(MoveAniShield);
                break;
        }

        //防禦方向
        switch (_basicData.face)
        {
            case MonsterBasicData.Face.Right:
                _transform.localScale = new Vector2(-ShieldScaleX, _transform.localScale.y);
                break;
            case MonsterBasicData.Face.Left:
                _transform.localScale = new Vector2(ShieldScaleX, _transform.localScale.y);
                break;
        }
    }

    private void FollowObjectAndRotation(Transform _target)
    {
        _transform.position = _target.position;
        _transform.rotation = _target.rotation;
    }//(2)
}
