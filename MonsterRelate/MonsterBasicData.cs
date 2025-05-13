using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterBasicData : MonoBehaviour
{
    //�o�Ӹ}���n��j�����Ǫ����ݭn��

    //�ݭn�b�U�Ǹ}����l���ܼ�

    private MonsterBornController _MBController;
    private bool NoBornJudge = false;//����MonsterBornJudge����
    public int MapNumber;

    public enum MonsterType { Default, ShieldMan};
    public MonsterType Type;
    public bool EntityMonster;//�O���O����Ǫ�(�]�w�W)
    private Transform _transform;
    [HideInInspector] public Transform playerTransform;
    [HideInInspector] public float hp;//���Q��Lscript�Ψ�(restroomEnemy)
    public int maxHp;
    public int Order;//�Ʀr�V�C�V�u������ script(�Ǫ�script)
    [HideInInspector] public bool ShouldIgnore;//script(�Ǫ�script)
    [SerializeField] private float GroundCheckDistance;
    [SerializeField] private float WallCheckDistance;
    public float GroundPlace;//�b�a�W��Y��
    public enum Face { Right, Left };
    [HideInInspector] public Face face;
    public bool isDieWithOtherScript;//�O�_�T�w���`�ѯS��}������
    public bool SpecialOpening;//�}���O�_�O�ίS��status
    [HideInInspector] public bool isGround;
    [HideInInspector] public bool touchRightWall;
    [HideInInspector] public bool touchLeftWall;
    [HideInInspector] public bool ReverseWalk;
    [HideInInspector] public MonsterDeadInformation _deadInformation = new MonsterDeadInformation();

    //Distance
    [HideInInspector] public Vector3 MonsterPlace = new Vector3();//�b�Ǫ�controller�a���ե������w
    [HideInInspector] public float DistanceX;//�Ǫ��P���a���Z��
    [HideInInspector] public float AbsDistanceX;//�Ǫ��P���a���Z��
    [HideInInspector] public float AbsDistanceY;//�Ǫ��P���a���Z��(�����)
    [HideInInspector] public float DistanceY;//�Ǫ��P���a���Z��
    [HideInInspector] public bool isPlayerAtRightSide;
    [HideInInspector] public bool isPlayerAtLeftSide;

    //touch
    private RaycastHit2D GroundCheck;
    private RaycastHit2D LeftWallCheck;
    private RaycastHit2D RightWallCheck;
    private RaycastHit2D MonsterLeftWallCheck;
    private RaycastHit2D MonsterRightWallCheck;
    private RaycastHit2D LeftPlatformCheck;
    private RaycastHit2D RightPlatformCheck;

    public delegate void CoolDownVar(ref bool Target);

    private void Awake()
    {
        //�X�ͧP�_
        if(MapNumber == 999)
        {
            NoBornJudge = true;
        }

        if (!NoBornJudge && GameObject.Find("FollowSystem") != null)
        {
            _MBController = GameObject.Find("FollowSystem").GetComponent<MonsterBornController>();
            if (!_MBController.MonsterBornList[MapNumber][Order])
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void CoolDownCalculate(float _time, float LastTime, float CoolDown, ref bool SkillBool)
    {
        if (_time >= (LastTime + CoolDown) && !SkillBool)
        {
            SkillBool = true;
        }
    }

    public void DistanceCalculate()
    {
        if(playerTransform == null)
        {
            return;
        }
        DistanceX = Distance.CalculateRelativelyDistanceX(MonsterPlace, playerTransform.position);
        AbsDistanceX = Mathf.Abs(DistanceX);
        DistanceY = Distance.CalculateRelativelyDistanceY(MonsterPlace, playerTransform.position);
        AbsDistanceY = Mathf.Abs(DistanceY);
        if (DistanceX >= 0)
        {
            isPlayerAtLeftSide = true;
            isPlayerAtRightSide = false;
        }
        else
        {
            isPlayerAtLeftSide = false;
            isPlayerAtRightSide = true;
        }
    }

    public void DieJudge()
    {
        //�]�𮧦Ӯ���
        if (RestPlace.MonsterShouldDestroy)
        {
            Destroy(this.gameObject);
        }
        //���`���`
        if (hp <= 0 && !isDieWithOtherScript)
        {
            ConfirmDie();
            return;
        }
        //�U�N���`
        if (_deadInformation.BurningDie && !isDieWithOtherScript)
        {
            ConfirmDie();
            return;
        }
    }

    public void ConfirmDie()
    {
        switch (face)
        {
            case Face.Left:
                _deadInformation.FaceLeft = true;
                break;
            case Face.Right:
                _deadInformation.FaceRight = true;
                break;
        }

        if (!NoBornJudge)
        {
            this.GetComponent<MonsterDieController>().BeginDie(_MBController, MapNumber, Order, _deadInformation);
        }
        else
        {
            this.GetComponent<MonsterDieController>().BeginDie(_deadInformation);
        }
    }

    public MonsterDeadInformation RecordCompleteDeadInfo(Vector3 Position)
    {
        _deadInformation.DiePosition = Position;
        return _deadInformation;
    }

    public void CheckTouchGround(ref float GroundPointY)
    {
        //�P�_�O�_�b�a�W
        GroundCheck = Physics2D.Raycast(transform.position, -Vector2.up, GroundCheckDistance, 1024);
        if (GroundCheck)
        {
            isGround = true;
            GroundPointY = GroundCheck.point.y;
        }
        else
        {
            isGround = false;
        }
    }

    public void CheckTouchGround()
    {
        //�P�_�O�_�b�a�W
        GroundCheck = Physics2D.Raycast(transform.position, -Vector2.up, GroundCheckDistance, 1024);
        if (GroundCheck)
        {
            isGround = true;
        }
        else
        {
            isGround = false;
        }
    }

    public void CheckTouchWall()
    {
        MonsterLeftWallCheck = Physics2D.Raycast(transform.position, Vector2.left, WallCheckDistance, 4096);
        LeftWallCheck = Physics2D.Raycast(transform.position, Vector2.left, WallCheckDistance, 64);
        LeftPlatformCheck = Physics2D.Raycast(transform.position, Vector2.left, WallCheckDistance, 1024);
        if (LeftWallCheck || MonsterLeftWallCheck || LeftPlatformCheck)
        {
            touchLeftWall = true;
        }
        else
        {
            touchLeftWall = false;
        }
        MonsterRightWallCheck = Physics2D.Raycast(transform.position, Vector2.right, WallCheckDistance, 2048);
        RightWallCheck = Physics2D.Raycast(transform.position, Vector2.right, WallCheckDistance, 128);
        RightPlatformCheck = Physics2D.Raycast(transform.position, Vector2.right, WallCheckDistance, 1024);
        if (RightWallCheck || MonsterRightWallCheck || RightPlatformCheck)
        {
            touchRightWall = true;
        }
        else
        {
            touchRightWall = false;
        }
    }

    public void BasicVarInisialize(Transform JudgeFaceTransform, string PresetDirection)
    {
        _transform = transform;
        if (GameObject.Find("player") != null)
        {
            playerTransform = GameObject.Find("player").transform;
        }
        hp = maxHp;
        switch (PresetDirection)
        {
            case "R":
                if (JudgeFaceTransform.localScale.x > 0)
                {
                    face = Face.Right;
                }
                else
                {
                    face = Face.Left;
                }
                break;
            case "L":
                if (JudgeFaceTransform.localScale.x > 0)
                {
                    face = Face.Left;
                }
                else
                {
                    face = Face.Right;
                }
                break;
                default: break;
        }
    }

    public void BasicVarInisialize(SpriteRenderer JudgeFaceSpr, string PresetDirection)
    {
        _transform = transform;
        if (GameObject.Find("player") != null)
        {
            playerTransform = GameObject.Find("player").transform;
        }
        hp = maxHp;
        switch (PresetDirection)
        {
            case "R":
                if (!JudgeFaceSpr.flipX)
                {
                    face = Face.Right;
                }
                else
                {
                    face = Face.Left;
                }
                break;
            case "L":
                if (!JudgeFaceSpr.flipX)
                {
                    face = Face.Left;
                }
                else
                {
                    face = Face.Right;
                }
                break;
            default: break;
        }
    }

    public void TurnFaceJudge()
    {
        if (playerTransform == null)
        {
            return;
        }
        if (_transform.position.x >= playerTransform.position.x)
        {
            face = Face.Left;
        }
        if (_transform.position.x < playerTransform.position.x)
        {
            face = Face.Right;
        }
    }

    public void LagTurnFaceJudge(ref float Timer, float TimerSet, float _deltaTime)
    {
        if (playerTransform == null)
        {
            return;
        }
        if (_transform.position.x >= playerTransform.position.x)
        {
            if (face == Face.Right)
            {
                Timer -= _deltaTime;
                if (Timer <= 0)
                {
                    Timer = TimerSet;
                    face = Face.Left;
                }
            }
            else
            {
                Timer = TimerSet;
            }
        }
        if (_transform.position.x < playerTransform.position.x)
        {
            if (face == Face.Left)
            {
                Timer -= _deltaTime;
                if (Timer <= 0)
                {
                    Timer = TimerSet;
                    face = Face.Right;
                }
            }
            else
            {
                Timer = TimerSet;
            }
        }
    }

    public void ReverseTurnFaceJudge()
    {
        if (playerTransform == null)
        {
            return;
        }
        if (_transform.position.x >= playerTransform.position.x)
        {
            face = Face.Right;
        }
        if (_transform.position.x < playerTransform.position.x)
        {
            face = Face.Left;
        }
    }

    public void ReverseLagTurnFaceJudge(ref float Timer, float TimerSet, float _deltaTime)
    {
        if (playerTransform == null)
        {
            return;
        }
        if (_transform.position.x >= playerTransform.position.x)
        {
            if (face == Face.Left)
            {
                Timer -= _deltaTime;
                if (Timer <= 0)
                {
                    Timer = TimerSet;
                    face = Face.Right;
                }
            }
            else
            {
                Timer = TimerSet;
            }
        }
        if (_transform.position.x < playerTransform.position.x)
        {
            if (face == Face.Right)
            {
                Timer -= _deltaTime;
                if (Timer <= 0)
                {
                    Timer = TimerSet;
                    face = Face.Left;
                }
            }
            else
            {
                Timer = TimerSet;
            }
        }
    }
}

public struct MonsterDeadInformation
{
    public Vector3 DiePosition;
    public bool FaceRight;
    public bool FaceLeft;
    public bool BurningDie;
}
