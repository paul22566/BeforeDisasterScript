using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterBasicData : MonoBehaviour
{
    //這個腳本要放大部分怪物都需要的

    //需要在各怪腳本初始化變數

    private MonsterBornController _MBController;
    private bool NoBornJudge = false;//不受MonsterBornJudge控制
    public int MapNumber;

    public enum MonsterType { Default, ShieldMan};
    public MonsterType Type;
    public bool EntityMonster;//是不是實體怪物(設定上)
    private Transform _transform;
    [HideInInspector] public Transform playerTransform;
    [HideInInspector] public float hp;//有被其他script用到(restroomEnemy)
    public int maxHp;
    public int Order;//數字越低越優先移動 script(怪物script)
    [HideInInspector] public bool ShouldIgnore;//script(怪物script)
    [SerializeField] private float GroundCheckDistance;
    [SerializeField] private float WallCheckDistance;
    public float GroundPlace;//在地上時Y值
    public enum Face { Right, Left };
    [HideInInspector] public Face face;
    public bool isDieWithOtherScript;//是否確定死亡由特殊腳本控制
    public bool SpecialOpening;//開場是否是用特殊status
    [HideInInspector] public bool isGround;
    [HideInInspector] public bool touchRightWall;
    [HideInInspector] public bool touchLeftWall;
    [HideInInspector] public bool ReverseWalk;
    [HideInInspector] public MonsterDeadInformation _deadInformation = new MonsterDeadInformation();

    //Distance
    [HideInInspector] public Vector3 MonsterPlace = new Vector3();//在怪物controller地面校正完指定
    [HideInInspector] public float DistanceX;//怪物與玩家的距離
    [HideInInspector] public float AbsDistanceX;//怪物與玩家的距離
    [HideInInspector] public float AbsDistanceY;//怪物與玩家的距離(絕對值)
    [HideInInspector] public float DistanceY;//怪物與玩家的距離
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
        //出生判斷
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
        //因休息而消滅
        if (RestPlace.MonsterShouldDestroy)
        {
            Destroy(this.gameObject);
        }
        //正常死亡
        if (hp <= 0 && !isDieWithOtherScript)
        {
            ConfirmDie();
            return;
        }
        //燃燒死亡
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
        //判斷是否在地上
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
        //判斷是否在地上
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
