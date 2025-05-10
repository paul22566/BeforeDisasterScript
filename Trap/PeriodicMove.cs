using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PeriodicMove : MonoBehaviour
{
    private Transform _transform;
    private Animator MachineAni;
    public float SpeedSet;//script(ProduceLightPoint)
    [HideInInspector] public float Speed;//script(MovePlatform，ProduceLightPoint)
    public float OneWayRunTime;//單向運行總時長(含緩衝)
    private float RunTime;
    private bool StartMove;
    private float WaitTimer;
    public float WaitTimerSet;
    public float BufferTime;//緩衝時間
    private float _fixedDeltaTime;
    private bool OneWayVerAppear;
    private float FixBufferTime;
    private bool DirectMove;//如果是玩家一進地圖就存在的，則會變成true
    public enum Ver { TwoWay, OneWay };
    public Ver ver;

    public enum Direction { Up, Down, Right, Left };
    public Direction direction;//script(MovePlatform，ProduceLightPoint)

    //檢查用變數
    public float Distance;//參照用不須輸入
    public float EndAndStartDistance;//參照用不須輸入
    public float StableMoveDistance;//參照用不須輸入
    // Start is called before the first frame update
    void Start()
    {
        _transform = this.transform;
        Speed = SpeedSet;
        if (!DirectMove)
        {
            WaitTimer = WaitTimerSet;
        }
        RunTime = OneWayRunTime;
        if (ver == Ver.OneWay)
        {
            OneWayVerAppear = true;
            MachineAni = _transform.GetComponent<Animator>();
        }
        FixBufferTime = BufferTime * 50;
    }

    private void FixedUpdate()
    {
        _fixedDeltaTime = Time.fixedDeltaTime;

        if (RunTime > 0 && !OneWayVerAppear)
        {
            RunTime -= _fixedDeltaTime;
        }
        if (OneWayVerAppear)
        {
            WaitTimer -= _fixedDeltaTime;
            if (WaitTimer <= 0)
            {
                WaitTimer = WaitTimerSet;
                OneWayVerAppear = false;
            }
            else
            {
                return;
            }
        }
        if (StartMove)
        {
            Speed += SpeedSet / FixBufferTime;
            if (Speed >= SpeedSet)
            {
                Speed = SpeedSet;
                StartMove = false;
            }
        }
        switch (direction)
        {
            case Direction.Up:
                if (RunTime <= BufferTime)
                {
                    if (Speed > 0)
                    {
                        Speed -= SpeedSet / FixBufferTime;
                    }
                    if (Speed <= 0)
                    {
                        Speed = 0;
                        WaitTimer -= _fixedDeltaTime;
                        if (WaitTimer <= 0 && ver == Ver.TwoWay)
                        {
                            WaitTimer = WaitTimerSet;
                            RunTime = OneWayRunTime * 2;
                            direction = Direction.Down;
                            StartMove = true;
                        }
                        if (ver == Ver.OneWay)
                        {
                            MachineAni.SetBool("Disappear", true);
                            if(WaitTimer <= 0)
                            {
                                Destroy(this.gameObject);
                            }
                        }
                    }
                }
                _transform.localPosition = new Vector3(_transform.localPosition.x, _transform.localPosition.y + Speed * _fixedDeltaTime, 0);
                break;
            case Direction.Down:
                if (RunTime <= BufferTime)
                {
                    if (Speed > 0)
                    {
                        Speed -= SpeedSet / FixBufferTime;
                    }
                    if (Speed <= 0)
                    {
                        Speed = 0;
                        WaitTimer -= _fixedDeltaTime;
                        if (WaitTimer <= 0 && ver == Ver.TwoWay)
                        {
                            WaitTimer = WaitTimerSet;
                            direction = Direction.Up;
                            RunTime = OneWayRunTime * 2;
                            StartMove = true;
                        }
                        if (ver == Ver.OneWay)
                        {
                            MachineAni.SetBool("Disappear", true);
                            if (WaitTimer <= 0)
                            {
                                Destroy(this.gameObject);
                            }
                        }
                    }
                }
                _transform.localPosition = new Vector3(_transform.localPosition.x, _transform.localPosition.y - Speed * _fixedDeltaTime, 0);
                break;
            case Direction.Left:
                if (RunTime <= BufferTime)
                {
                    if (Speed > 0)
                    {
                        Speed -= SpeedSet / FixBufferTime;
                    }
                    if (Speed <= 0)
                    {
                        Speed = 0;
                        WaitTimer -= _fixedDeltaTime;
                        if (WaitTimer <= 0 && ver == Ver.TwoWay)
                        {
                            WaitTimer = WaitTimerSet;
                            direction = Direction.Right;
                            RunTime = OneWayRunTime * 2;
                            StartMove = true;
                        }
                        if (ver == Ver.OneWay)
                        {
                            MachineAni.SetBool("Disappear", true);
                            if (WaitTimer <= 0)
                            {
                                Destroy(this.gameObject);
                            }
                        }
                    }
                }
                _transform.localPosition = new Vector3(_transform.localPosition.x - Speed * _fixedDeltaTime, _transform.localPosition.y, 0);
                break;
            case Direction.Right:
                if (RunTime <= BufferTime)
                {
                    if (Speed > 0)
                    {
                        Speed -= SpeedSet / FixBufferTime;
                    }
                    if (Speed <= 0)
                    {
                        Speed = 0;
                        WaitTimer -= _fixedDeltaTime;
                        if (WaitTimer <= 0 && ver == Ver.TwoWay)
                        {
                            WaitTimer = WaitTimerSet;
                            direction = Direction.Left;
                            RunTime = OneWayRunTime * 2;
                            StartMove = true;
                        }
                        if (ver == Ver.OneWay)
                        {
                            MachineAni.SetBool("Disappear", true);
                            if (WaitTimer <= 0)
                            {
                                Destroy(this.gameObject);
                            }
                        }
                    }
                }
                _transform.localPosition = new Vector3(_transform.localPosition.x + Speed * _fixedDeltaTime, _transform.localPosition.y, 0);
                break;
        }
        DistanceCalculate();
    }

    public void DistanceCalculate()//方便設計用
    {
        //計算減速階段走的距離
        EndAndStartDistance = SpeedSet + (SpeedSet - (SpeedSet / FixBufferTime) * (FixBufferTime - 1));//上底加下底
        EndAndStartDistance = EndAndStartDistance * (BufferTime * 50) / 2;//乘高除2
        EndAndStartDistance = EndAndStartDistance * _fixedDeltaTime;//得出距離

        StableMoveDistance = (OneWayRunTime - BufferTime) * SpeedSet - SpeedSet * 0.02f;

        Distance = EndAndStartDistance + StableMoveDistance;
    }

    public float PreviewDistanceCalculate()
    {
        FixBufferTime = BufferTime * 50;

        //計算減速階段走的距離
        EndAndStartDistance = SpeedSet + (SpeedSet - (SpeedSet / FixBufferTime) * (FixBufferTime - 1));//上底加下底
        EndAndStartDistance = EndAndStartDistance * (BufferTime * 50) / 2;//乘高除2
        EndAndStartDistance = EndAndStartDistance * _fixedDeltaTime;//得出距離

        StableMoveDistance = (OneWayRunTime - BufferTime) * SpeedSet - SpeedSet * 0.02f;

        Distance = EndAndStartDistance + StableMoveDistance;

        return Distance;
    }//方便設計用(專門給Produce腳本用)

    public void OneWayStartAppearInisialize(int i, float TimerSet)
    {
        OneWayRunTime = OneWayRunTime - i * TimerSet;
        WaitTimer = 0;
        DirectMove = true;
    }

    public float OneWayStartIndividualCalculate(float TimerSet)
    {
        return SpeedSet * TimerSet;
    }
}
