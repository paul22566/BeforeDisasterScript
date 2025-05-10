using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atk4Eye : MonoBehaviour
{
    private Transform Captain;
    public GameObject target;
    Transform _transform;
    private CaptainController _controller;
    private Sprite TargetSprite;
    private SpriteRenderer thisSpr;
    public Sprite RotateAtk1, RotateAtk2, RotateAtk3, RotateAtk4, RotateAtk5, RotateAtk6;
    // Start is called before the first frame update
    void Start()
    {
        Captain = this.transform.parent.parent;
        _controller = Captain.GetComponent<CaptainController>();
        _transform = this.transform;
        thisSpr = this.gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Captain == null)
        {
            Destroy(this.gameObject);
            return;
        }
        switch (_controller.face)
        {
            case CaptainController.Face.Left:
                thisSpr.flipX = true;
                break;
            case CaptainController.Face.Right:
                thisSpr.flipX = false;
                break;
        }
        TargetSprite = target.GetComponent<SpriteRenderer>().sprite;
        switch (_controller.face)
        {
            case CaptainController.Face.Left:
                if (TargetSprite == RotateAtk1)
                    _transform.localPosition = new Vector3(-2.04f, 2.51f, 0);
                if (TargetSprite == RotateAtk2 || TargetSprite == RotateAtk3 || TargetSprite == RotateAtk4 || TargetSprite == RotateAtk5)
                    _transform.localPosition = new Vector3(1000f, 1000f, 0);
                if (TargetSprite == RotateAtk6)
                    _transform.localPosition = new Vector3(-1.74f, 3.18f, 0);
                break;
            case CaptainController.Face.Right:
                if (TargetSprite == RotateAtk1)
                    _transform.localPosition = new Vector3(2.04f, 2.51f, 0);
                if (TargetSprite == RotateAtk2 || TargetSprite == RotateAtk3 || TargetSprite == RotateAtk4 || TargetSprite == RotateAtk5)
                    _transform.localPosition = new Vector3(1000f, 1000f, 0);
                if (TargetSprite == RotateAtk6)
                    _transform.localPosition = new Vector3(1.74f, 3.18f, 0);
                break;
        }
    }
}
