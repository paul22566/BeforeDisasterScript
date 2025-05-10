using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostManEye : MonoBehaviour
{
    public Transform Target;
    private Transform _transform;
    private MonsterBasicData _basicData;
    private SpriteRenderer _spr;
    // Start is called before the first frame update
    void Start()
    {
        _transform = transform;
        _basicData = transform.parent.parent.GetComponent<MonsterBasicData>();
        _spr = this.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (_basicData.face)
        {
            case MonsterBasicData.Face.Right:
                if (Target.localPosition.y > 0)
                {
                    _spr.flipX = true;
                }
                else
                {
                    _spr.flipX = false;
                }
                _transform.rotation = Quaternion.Euler(0, 0, Target.localRotation.z);
                break;
            case MonsterBasicData.Face.Left:
                if (Target.localPosition.y > 0)
                {
                    _spr.flipX = false;
                }
                else
                {
                    _spr.flipX = true;
                }
                _transform.rotation = Quaternion.Euler(0, 0, -Target.localRotation.z);
                break;
        }
        _transform.position = Target.position;

    }
}
