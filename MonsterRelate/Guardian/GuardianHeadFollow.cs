using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianHeadFollow : MonoBehaviour
{
    private GuardianController _controller;
    private Transform _transform;
    public Transform WaitHead;
    public Transform WalkHead;
    public Transform JumpHead;
    public Transform AtkHead;
    public Transform Atk2Head;
    public Transform BackAtk2Head;
    public Transform BeginingHead;

    private void Start()
    {
        _transform = this.transform;
        _controller = _transform.parent.GetComponent<GuardianController>();
    }

    private void Update()
    {
        switch (_controller.NowAni)
        {
            case GuardianController.AniStatus.wait:
                _transform.position = WaitHead.position;
                break;
            case GuardianController.AniStatus.walk:
                _transform.position = WalkHead.position;
                break;
            case GuardianController.AniStatus.Jump:
                _transform.position = JumpHead.position;
                break;
            case GuardianController.AniStatus.Atk1:
                _transform.position = AtkHead.position;
                break;
            case GuardianController.AniStatus.Atk2:
                _transform.position = Atk2Head.position;
                break;
            case GuardianController.AniStatus.BackAtk2:
                _transform.position = BackAtk2Head.position;
                break;
            case GuardianController.AniStatus.Begining:
                _transform.position = BeginingHead.position;
                break;
        }
    }
}
