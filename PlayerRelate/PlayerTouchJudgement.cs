using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerTouchJudgement : MonoBehaviour
{
    private Transform _playerRightJudgement;
    private Transform _playerLeftJudgement;
    private Transform _playerUnderJudgement;

    private Transform _transform;
    private Transform Player;
    private PlayerController _playerController;
    private SlopeControll _slopeControll;
    private bool IgnoreCheck;

    private RaycastHit2D RightWallCheck;
    private RaycastHit2D LeftWallCheck;
    private RaycastHit2D RightMonsterCheck;
    private RaycastHit2D LeftMonsterCheck;
    public float TouchWallLineDistance = 1.83f;

    [HideInInspector] public bool isMonsterUnder;//script(playerController)
    [HideInInspector] public bool isRightSideHaveMonster;//有被其他script用到(playerController)
    [HideInInspector] public bool isLeftSideHaveMonster;//有被其他script用到(playerController)
    // Start is called before the first frame update
    void Start()
    {
        _playerController = GameObject.Find("player").GetComponent<PlayerController>();
        _slopeControll = _playerController._slopeControll;
        Player = _playerController.transform;

        _transform = this.transform;

        _playerRightJudgement = _transform.GetChild(0);
        _playerLeftJudgement = _transform.GetChild(1);
        _playerUnderJudgement = _transform.GetChild(2);
    }

    // Update is called once per frame
    void Update()
    {
        _transform.position = Player.position;

        RaycastCheck();

        BoolJudgement();

        IgnoreCheck = false;
    }

    private void RaycastCheck()
    {
        RightWallCheck = Physics2D.Raycast(_playerRightJudgement.position, -Vector2.up, TouchWallLineDistance, 1024);
        if (!RightWallCheck)
        {
            RightWallCheck = Physics2D.Raycast(_playerRightJudgement.position, -Vector2.up, TouchWallLineDistance, 32768);
        }

        if (_playerController.isCeiling)
        {
            IgnoreCheck = true;
        }

        if (!RightWallCheck || IgnoreCheck)
        {
            if (Physics2D.Raycast(_playerRightJudgement.position, -Vector2.up, TouchWallLineDistance, 128))
            {
                IgnoreCheck = false;
                RightWallCheck = Physics2D.Raycast(_playerRightJudgement.position, -Vector2.up, TouchWallLineDistance, 128);
            }
        }

        LeftWallCheck = Physics2D.Raycast(_playerLeftJudgement.position, -Vector2.up, TouchWallLineDistance, 1024);
        if (!LeftWallCheck)
        {
            LeftWallCheck = Physics2D.Raycast(_playerLeftJudgement.position, -Vector2.up, TouchWallLineDistance, 32768);
        }

        if (_playerController.isCeiling)
        {
            IgnoreCheck = true;
        }

        if (!LeftWallCheck || IgnoreCheck)
        {
            if (Physics2D.Raycast(_playerLeftJudgement.position, -Vector2.up, TouchWallLineDistance, 64))
            {
                IgnoreCheck = false;
                LeftWallCheck = Physics2D.Raycast(_playerLeftJudgement.position, -Vector2.up, TouchWallLineDistance, 64);
            }
        }

        RightMonsterCheck = Physics2D.Raycast(_playerRightJudgement.position, -Vector2.up, TouchWallLineDistance, 512);
        LeftMonsterCheck = Physics2D.Raycast(_playerLeftJudgement.position, -Vector2.up, TouchWallLineDistance, 512);

        //斜坡上不計入touchWall
        if (RightWallCheck)
        {
            if (Mathf.Abs(RightWallCheck.normal.x) != 0 || _slopeControll.onRightSlope)
            {
                IgnoreCheck = true;
            }
        }
        if (LeftWallCheck)
        {
            if (Mathf.Abs(LeftWallCheck.normal.x) != 0 || _slopeControll.onLeftSlope)
            {
                IgnoreCheck = true;
            }
        }
    }

    private void BoolJudgement()
    {
        if (LeftWallCheck && !IgnoreCheck)
        {
            _playerController.touchLeftWall = true;
        }
        else
        {
            _playerController.touchLeftWall = false;
        }
        if (RightWallCheck && !IgnoreCheck)
        {
            _playerController.touchRightWall = true;
        }
        else
        {
            _playerController.touchRightWall = false;
        }

        if (LeftMonsterCheck)
        {
            if (LeftMonsterCheck.collider.GetComponent<MonsterWeakJudgement>() != null)
            {
                if (!LeftMonsterCheck.collider.GetComponent<MonsterWeakJudgement>().isWeak)
                {
                    isLeftSideHaveMonster = true;
                }
                else
                {
                    isLeftSideHaveMonster = false;
                }
            }
            else
            {
                isLeftSideHaveMonster = true;
            }
        }
        else
        {
            isLeftSideHaveMonster = false;
        }
        if (RightMonsterCheck)
        {
            if (RightMonsterCheck.collider.GetComponent<MonsterWeakJudgement>() != null)
            {
                if (!RightMonsterCheck.collider.GetComponent<MonsterWeakJudgement>().isWeak)
                {
                    isRightSideHaveMonster = true;
                }
                else
                {
                    isRightSideHaveMonster = false;
                }
            }
            else
            {
                isRightSideHaveMonster = true;
            }
        }
        else
        {
            isRightSideHaveMonster = false;
        }
    }
}
