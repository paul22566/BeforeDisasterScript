using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProduceLightPoint : MonoBehaviour
{
    private Transform _transform;
    public GameObject LightAtk;
    private float Timer;
    public float TimerSet;

    private Vector3 StartAppearPoint;
    private PeriodicMove _motherLightPeriodicMove;
    private PeriodicMove _childLightPeriodicMove;
    private float LightTotalDistance;
    private float LightIndividualDistance;
    private int StartProduceNumber;
    private void Start()
    {
        _transform = transform;
        Timer = TimerSet;
        
        ProduceInisialObject();
    }

    private void FixedUpdate()
    {
        Timer -= Time.fixedDeltaTime;
        if (Timer <= 0)
        {
            Instantiate(LightAtk, _transform.localPosition, Quaternion.identity);
            Timer = TimerSet;
        }
    }

    private void ProduceInisialObject()
    {
        _motherLightPeriodicMove = LightAtk.GetComponent<PeriodicMove>();
        LightTotalDistance = _motherLightPeriodicMove.PreviewDistanceCalculate();
        LightIndividualDistance = _motherLightPeriodicMove.OneWayStartIndividualCalculate(TimerSet);
        StartProduceNumber = (int)((LightTotalDistance + 1) / LightIndividualDistance);

        switch (_motherLightPeriodicMove.direction)
        {
            case PeriodicMove.Direction.Up:
                for (int i = 0; i < StartProduceNumber; i++)
                {
                    StartAppearPoint = new Vector3(_transform.localPosition.x, _transform.localPosition.y + i * LightIndividualDistance, _transform.localPosition.z);
                    Instantiate(LightAtk, StartAppearPoint, Quaternion.identity, _transform);
                    _childLightPeriodicMove = _transform.GetChild(0).GetComponent<PeriodicMove>();
                    _childLightPeriodicMove.OneWayStartAppearInisialize(i, TimerSet);
                    _childLightPeriodicMove = null;
                    _transform.DetachChildren();
                }
                break;
            case PeriodicMove.Direction.Down:
                for (int i = 0; i < StartProduceNumber; i++)
                {
                    StartAppearPoint = new Vector3(_transform.localPosition.x, _transform.localPosition.y - i * LightIndividualDistance, _transform.localPosition.z);
                    Instantiate(LightAtk, StartAppearPoint, Quaternion.identity, _transform);
                    _childLightPeriodicMove = _transform.GetChild(0).GetComponent<PeriodicMove>();
                    _childLightPeriodicMove.OneWayStartAppearInisialize(i, TimerSet);
                    _childLightPeriodicMove = null;
                    _transform.DetachChildren();
                }
                break;
            case PeriodicMove.Direction.Right:
                for (int i = 0; i < StartProduceNumber; i++)
                {
                    StartAppearPoint = new Vector3(_transform.localPosition.x + i * LightIndividualDistance, _transform.localPosition.y, _transform.localPosition.z);
                    Instantiate(LightAtk, StartAppearPoint, Quaternion.identity, _transform);
                    _childLightPeriodicMove = _transform.GetChild(0).GetComponent<PeriodicMove>();
                    _childLightPeriodicMove.OneWayStartAppearInisialize(i, TimerSet);
                    _childLightPeriodicMove = null;
                    _transform.DetachChildren();
                }
                break;
            case PeriodicMove.Direction.Left:
                for (int i = 0; i < StartProduceNumber; i++)
                {
                    StartAppearPoint = new Vector3(_transform.localPosition.x - i * LightIndividualDistance, _transform.localPosition.y, _transform.localPosition.z);
                    Instantiate(LightAtk, StartAppearPoint, Quaternion.identity, _transform);
                    _childLightPeriodicMove = _transform.GetChild(0).GetComponent<PeriodicMove>();
                    _childLightPeriodicMove.OneWayStartAppearInisialize(i, TimerSet);
                    _childLightPeriodicMove = null;
                    _transform.DetachChildren();
                }
                break;
        }
    }
}
