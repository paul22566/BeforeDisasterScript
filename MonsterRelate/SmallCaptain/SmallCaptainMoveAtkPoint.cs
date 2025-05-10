using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallCaptainMoveAtkPoint : MonoBehaviour
{
    private Transform _transform;
    private GameObject Monster;
    private Transform PlayerTransform;
    private Transform MonsterTargetPoint;
    private RaycastHit2D CeilingCheck;

    private void Awake()
    {
        Monster = this.transform.parent.transform.parent.gameObject;
    }
    // Start is called before the first frame update
    void Start()
    {
        _transform = this.transform;
        MonsterTargetPoint = _transform.GetChild(0);
        if (GameObject.Find("player") != null)
        {
            PlayerTransform = GameObject.Find("player").transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Monster == null)
        {
            Destroy(this.gameObject);
        }
        else if(PlayerTransform)
        {
            this.transform.position = PlayerTransform.position;
        }

        CeilingCheck = Physics2D.Raycast(_transform.position, Vector2.up, 11f, 1024);
        if (CeilingCheck)
        {
            MonsterTargetPoint.position = new Vector3(_transform.position.x, CeilingCheck.point.y - 1, _transform.position.y);
        }
        else
        {
            MonsterTargetPoint.position = new Vector3(_transform.position.x, _transform.position.y + 10, _transform.position.y);
        }
    }
}
