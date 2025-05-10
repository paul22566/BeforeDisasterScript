using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectShield : MonoBehaviour
{
    public Transform ProtectTarget;
    public MonsterHurtedController _hurtedController;
    [HideInInspector] public bool ProtectSuccess;
    private Transform PlayerAtkTransform;

    private enum ProtectType { Line, priority};
    [SerializeField] private ProtectType _protectType;

    public CollisionType.Type _ProtectObjectType;//如果被保護對象無指定材質，這裡填入地毯(無聲音)即可

    [Header("線性防禦")]
    public Transform ShieldLinePoint;
    private Transform ShieldPoint1;
    private Transform ShieldPoint2;

    private void Start()
    {
        if (ShieldLinePoint != null)
        {
            ShieldPoint1 = ShieldLinePoint.GetChild(0);
            ShieldPoint2 = ShieldLinePoint.GetChild(1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerAtkController>() != null)
        {
            if(_hurtedController != null)
            {
                _hurtedController.DetectHurtedType(collision.gameObject);
            }
            if (collision != null)
            {
                PlayerAtkTransform = collision.transform;
            }
            ProtectJudge();
            if (ProtectSuccess)
            {
                collision.GetComponent<PlayerAtkController>().HitShield(_ProtectObjectType);
            }
        }
    }

    private void ProtectJudge()
    {
        switch (_protectType)
        {
            case ProtectType.Line:
                if (PlayerAtkTransform != null)
                {
                    ProtectSuccess = LineTypeJudge();
                }
                break;
            case ProtectType.priority:
                ProtectSuccess = true;
                break;
        }
    }

    private bool LineTypeJudge()
    {
        Vector3 ShieldLine;
        Vector3 ObjectLine;
        Vector2 Point;

        bool YJudge = false;
        bool XJudge = false;
        bool Judge = false;

        ShieldLine = Distance.CalculateLine(ShieldPoint1.position, ShieldPoint2.position);
        ObjectLine = Distance.CalculateLine(PlayerAtkTransform.position, ProtectTarget.position);
        Point = Distance.CalculateCrossPoint(ShieldLine, ObjectLine);
        
        if (PlayerAtkTransform.position.x > ProtectTarget.position.x)
        {
            if (Point.x < PlayerAtkTransform.position.x && Point.x > ProtectTarget.position.x)
            {
                XJudge = true;
            }
        }
        else
        {
            if (Point.x > PlayerAtkTransform.position.x && Point.x < ProtectTarget.position.x)
            {
                XJudge = true;
            }
        }
        if (PlayerAtkTransform.position.y > ProtectTarget.position.y)
        {
            if (Point.y < PlayerAtkTransform.position.y && Point.y > ProtectTarget.position.y)
            {
                YJudge = true;
            }
        }
        else
        {
            if (Point.y > PlayerAtkTransform.position.y && Point.y < ProtectTarget.position.y)
            {
                YJudge = true;
            }
        }

        if (XJudge && YJudge)
        {
            Judge = true;
        }

        return Judge;
    }
}
