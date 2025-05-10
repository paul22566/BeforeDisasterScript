using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptainEyeFollow : MonoBehaviour
{
    public Transform Target;
    private Transform _transform;
    private SpriteRenderer _spr;
    private CaptainController _controller;
    // Start is called before the first frame update
    void Start()
    {
        _transform = transform;
        _spr = this.GetComponent<SpriteRenderer>();
        _controller = transform.parent.parent.GetComponent<CaptainController>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (_controller.face)
        {
            case CaptainController.Face.Right:
                if (Target.localPosition.y > 0)
                {
                    _spr.flipX = true;
                }
                else
                {
                    _spr.flipX = false;
                }
                break;
            case CaptainController.Face.Left:
                if (Target.localPosition.y > 0)
                {
                    _spr.flipX = false;
                }
                else
                {
                    _spr.flipX = true;
                }
                break;
        }
        _transform.position = Target.position;
    }
}
