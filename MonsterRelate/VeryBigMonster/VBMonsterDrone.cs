using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class VBMonsterDrone : MonoBehaviour
{
    public enum Direction { Left, Right };
    public Direction direction;
    private Vector3 BeginPoint;
    private Vector3 EndPoint;
    public int TotalSummonNumber;
    private int SummonNumber = 0;
    private float Distance;
    public GameObject Drone;
    private float Timer;
    public float TimerSet;

    public float AppearPointY;
    public float AppearPointXRight;
    public float AppearPointXLeft;
    // Start is called before the first frame update
    void Start()
    {
        Timer = TimerSet;
        switch (direction)
        {
            case Direction.Left:
                BeginPoint = new Vector3(AppearPointXRight, AppearPointY, 0);
                EndPoint = new Vector3(AppearPointXLeft, AppearPointY, 0);
                break;
            case Direction.Right:
                BeginPoint = new Vector3(AppearPointXLeft, AppearPointY, 0);
                EndPoint = new Vector3(AppearPointXRight, AppearPointY, 0);
                break;
        }
        Distance = Mathf.Abs(BeginPoint.x - EndPoint.x) / (TotalSummonNumber - 1);
    }

    private void FixedUpdate()
    {
        Timer -= Time.fixedDeltaTime;
        if (Timer <= 0)
        {
            Instantiate(Drone, BeginPoint, Drone.transform.rotation);
            switch (direction)
            {
                case Direction.Left:
                    BeginPoint = new Vector3(BeginPoint.x - Distance, BeginPoint.y, 0);
                    break;
                case Direction.Right:
                    BeginPoint = new Vector3(BeginPoint.x + Distance, BeginPoint.y, 0);
                    break;
            }
            SummonNumber += 1;
            Timer = TimerSet;
            if (SummonNumber == TotalSummonNumber)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
