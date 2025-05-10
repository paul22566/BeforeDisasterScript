using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PeriodicMove : MonoBehaviour
{
    private Transform _transform;
    private Animator MachineAni;
    public float SpeedSet;//script(ProduceLightPoint)
    [HideInInspector] public float Speed;//script(MovePlatform�AProduceLightPoint)
    public float OneWayRunTime;//��V�B���`�ɪ�(�t�w��)
    private float RunTime;
    private bool StartMove;
    private float WaitTimer;
    public float WaitTimerSet;
    public float BufferTime;//�w�Įɶ�
    private float _fixedDeltaTime;
    private bool OneWayVerAppear;
    private float FixBufferTime;
    private bool DirectMove;//�p�G�O���a�@�i�a�ϴN�s�b���A�h�|�ܦ�true
    public enum Ver { TwoWay, OneWay };
    public Ver ver;

    public enum Direction { Up, Down, Right, Left };
    public Direction direction;//script(MovePlatform�AProduceLightPoint)

    //�ˬd���ܼ�
    public float Distance;//�ѷӥΤ�����J
    public float EndAndStartDistance;//�ѷӥΤ�����J
    public float StableMoveDistance;//�ѷӥΤ�����J
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

    public void DistanceCalculate()//��K�]�p��
    {
        //�p���t���q�����Z��
        EndAndStartDistance = SpeedSet + (SpeedSet - (SpeedSet / FixBufferTime) * (FixBufferTime - 1));//�W���[�U��
        EndAndStartDistance = EndAndStartDistance * (BufferTime * 50) / 2;//������2
        EndAndStartDistance = EndAndStartDistance * _fixedDeltaTime;//�o�X�Z��

        StableMoveDistance = (OneWayRunTime - BufferTime) * SpeedSet - SpeedSet * 0.02f;

        Distance = EndAndStartDistance + StableMoveDistance;
    }

    public float PreviewDistanceCalculate()
    {
        FixBufferTime = BufferTime * 50;

        //�p���t���q�����Z��
        EndAndStartDistance = SpeedSet + (SpeedSet - (SpeedSet / FixBufferTime) * (FixBufferTime - 1));//�W���[�U��
        EndAndStartDistance = EndAndStartDistance * (BufferTime * 50) / 2;//������2
        EndAndStartDistance = EndAndStartDistance * _fixedDeltaTime;//�o�X�Z��

        StableMoveDistance = (OneWayRunTime - BufferTime) * SpeedSet - SpeedSet * 0.02f;

        Distance = EndAndStartDistance + StableMoveDistance;

        return Distance;
    }//��K�]�p��(�M����Produce�}����)

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
