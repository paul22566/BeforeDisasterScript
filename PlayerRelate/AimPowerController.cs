using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimPowerController : MonoBehaviour
{
    private Transform _transform;
    public float Speed;
    public Transform PlayerTransform;
    private BattleSystem _battleSystem;
    private bool isRightArrowPressed;
    private bool isLeftArrowPressed;
    private bool isUpArrowPressed;
    private bool isDownArrowPressed;
    private bool touchLeftWall;
    private bool touchRightWall;
    private bool touchUpWall;
    private bool touchDownWall;
    private int status;
    [HideInInspector] public bool HasStatusSet = false;//其他script有用到(PredictPowerBase)
    private float PowerPlaceX = 5;
    private float PowerPlaceY = 3;
    [HideInInspector] public float PowerX = 5;//其他script有用到(BattleSystem)
    [HideInInspector] public float PowerY = 3;//其他script有用到(BattleSystem)

    private float LeftUpPowerX = 45;
    private float LeftUpPowerY = 330;
    private float RightUpPowerX = 210;
    private float RightUpPowerY = 300;
    private float LeftDownPowerX = 80;
    private float LeftDownPowerY = 100;
    private float RightDownPowerX = 240;
    private float RightDownPowerY = 100;

    private float NowProportionX;//當前X軸比例 由左而右
    private float NowProportionY;//當前Y軸比例 由下而上
    private float NowUpProportionXPower;//上半段平均Xscale
    private float NowDownProportionXPower;//下半段平均Xscale
    private float NowUpProportionYPower;//上半段平均Yscale
    private float NowDownProportionYPower;//下半段平均Yscale

    private void Start()
    {
        PowerPlaceX = 5;
        PowerPlaceY = 3;
        _transform = this.transform;
        _battleSystem = PlayerTransform.GetComponent<BattleSystem>();
    }
    void Update()
    {
        if (!HasStatusSet && _battleSystem.isAim)
        {
            switch (PlayerController._player.face)
            {
                case Creature.Face.Left:
                    status = 2;
                    break;
                case Creature.Face.Right:
                    status = 1;
                    break;
            }
            HasStatusSet = true;
        }
        //顯示位置
        switch (status)
        {
            case 1:
                _transform.localPosition = new Vector3(1.01f + PowerPlaceX * 0.299f, 1.29f + PowerPlaceY * 0.295f, 0);
                break;
            case 2:
                _transform.localPosition = new Vector3(-1.01f - PowerPlaceX * 0.299f, 1.29f + PowerPlaceY * 0.295f, 0);
                break;
        }

        NowProportionX = PowerPlaceX / 10;
        NowProportionY = PowerPlaceY / 10;

        NowUpProportionXPower = LeftUpPowerX * (1 - NowProportionX) + RightUpPowerX * NowProportionX;
        NowDownProportionXPower = LeftDownPowerX * (1 - NowProportionX) + RightDownPowerX * NowProportionX;
        PowerX = NowDownProportionXPower * (1 - NowProportionY) + NowUpProportionXPower * NowProportionY;
        NowUpProportionYPower = LeftUpPowerY * (1 - NowProportionX) + RightUpPowerY * NowProportionX;
        NowDownProportionYPower = LeftDownPowerY * (1 - NowProportionX) + RightDownPowerY * NowProportionX;
        PowerY = NowDownProportionYPower * (1 - NowProportionY) + NowUpProportionYPower * NowProportionY;

        LimitJudgement();
        if(Input.GetAxis("RightHorizontal") > 0.3)
        {
            isRightArrowPressed = true;
            if (!isLeftArrowPressed && !touchRightWall)
            {
                switch (status)
                {
                    case 1:
                        PowerPlaceX += Speed;
                        break;
                    case 2:
                        PowerPlaceX -= Speed;
                        break;
                }
            }
        }
        else
        {
            isRightArrowPressed = false;
        }
        if(Input.GetAxis("RightHorizontal") < -0.3)
        {
            isLeftArrowPressed = true;
            if (!isRightArrowPressed && !touchLeftWall)
            {
                switch (status)
                {
                    case 1:
                        PowerPlaceX -= Speed;
                        break;
                    case 2:
                        PowerPlaceX += Speed;
                        break;
                }
            }
        }
        else
        {
            isLeftArrowPressed = false;
        }
        if (Input.GetAxis("RightVertical") < -0.3)
        {
            isUpArrowPressed = true;
            if (!isDownArrowPressed && !touchUpWall)
            {
                PowerPlaceY += Speed;
            }
        }
        else
        {
            isUpArrowPressed = false;
        }
        if (Input.GetAxis("RightVertical") > 0.3)
        {
            isDownArrowPressed = true;
            if (!isUpArrowPressed && !touchDownWall)
            {
                PowerPlaceY -= Speed;
            }
        }
        else
        {
            isDownArrowPressed = false;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            isRightArrowPressed = true;
            if(!isLeftArrowPressed && !touchRightWall)
            {
                switch (status)
                {
                    case 1:
                        PowerPlaceX += Speed;
                        break;
                    case 2:
                        PowerPlaceX -= Speed;
                        break;
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            isRightArrowPressed = false;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            isLeftArrowPressed = true;
            if (!isRightArrowPressed && !touchLeftWall)
            {
                switch (status)
                {
                    case 1:
                        PowerPlaceX -= Speed;
                        break;
                    case 2:
                        PowerPlaceX += Speed;
                        break;
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            isLeftArrowPressed = false;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            isUpArrowPressed = true;
            if(!isDownArrowPressed && !touchUpWall)
            {
                PowerPlaceY += Speed;
            }
        }
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            isUpArrowPressed = false;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            isDownArrowPressed = true;
            if (!isUpArrowPressed && !touchDownWall)
            {
                PowerPlaceY -= Speed;
            }
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            isDownArrowPressed = false;
        }

        if (!_battleSystem.isAim || PlayerController.isHurted || PlayerController.isDie)
        {
            this.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            PowerPlaceX = 5;
            PowerPlaceY = 3;
            switch (status)
            {
                case 1:
                    _transform.position = new Vector3(PlayerTransform.position.x + 1.01f + PowerPlaceX * 0.299f, PlayerTransform.position.y + 1.29f + PowerPlaceY * 0.295f, 0);
                    break;
                case 2:
                    _transform.position = new Vector3(PlayerTransform.position.x - 1.01f - PowerPlaceX * 0.299f, PlayerTransform.position.y + 1.29f + PowerPlaceY * 0.295f, 0);
                    break;
            }
            HasStatusSet = false;
            this.gameObject.SetActive(false);
        }
    }

    private void LimitJudgement()
    {
        switch (status)
        {
            case 1:
                if (PowerPlaceX >= 10)
                {
                    touchRightWall = true;
                    PowerPlaceX = 10;
                }
                else
                {
                    touchRightWall = false;
                }

                if (PowerPlaceX <= 0)
                {
                    touchLeftWall = true;
                    PowerPlaceX = 0;
                }
                else
                {
                    touchLeftWall = false;
                }
                break;
            case 2:
                if (PowerPlaceX <= 0)
                {
                    touchRightWall = true;
                    PowerPlaceX = 0;
                }
                else
                {
                    touchRightWall = false;
                }

                if (PowerPlaceX >= 10)
                {
                    touchLeftWall = true;
                    PowerPlaceX = 10;
                }
                else
                {
                    touchLeftWall = false;
                }
                break;
        }

        if (PowerPlaceY >= 10)
        {
            touchUpWall = true;
            PowerPlaceY = 10;
        }
        else
        {
            touchUpWall = false;
        }

        if (PowerPlaceY <= 0)
        {
            touchDownWall = true;
            PowerPlaceY = 0;
        }
        else
        {
            touchDownWall = false;
        }
    }
}
