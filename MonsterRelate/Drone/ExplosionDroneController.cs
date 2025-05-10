using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ExplosionDroneController : MonoBehaviour
{
    public float Speed;
    public float OpenRangeX;
    public float OpenRangeY;
    private Transform _transform;
    private Transform PlayerTransform;
    public GameObject Explosion;
    private ExplosionDroneSE _droneSE;
    private float RotateSpeed = 100;

    private GameObject MonsterWaitAnimation;
    private GameObject MonsterAtkAnimation;

    private Vector3 TopPoint;
    private float DistanceX;
    [HideInInspector] public float AbsDistanceX;
    private float DistanceY;
    private float AbsDistanceY;
    private float RotateCaculateDistance = 0.5f;
    private float ParabolaNextX;
    private float ParabolaNextY;
    private float ParabolaConstant;
    private float ParabolaNowX;
    private float ParabolaNowY;
    private float NextDistanceX;
    private float NextDistanceY;
    private float Tan;
    private float TargetRotate;
    private float _fixedDeltaTime;
    private float NowRotate;

    private float OpenTimer;
    private float OpenTimerSet = 0.3f;
    private float AtkTimer;
    private float AtkTimerSet = 0.1f;
    private bool isOpen;
    private bool isRun;

    [Header("固定角度型")]
    public bool GoLeft;
    public bool GoRight;
    public bool GoStraight;
    public bool FixedVer;
    [SerializeField] public Vector3 FixedTopPoint;
    // Start is called before the first frame update
    void Start()
    {
        _transform = this.transform;
        _droneSE = this.GetComponent<ExplosionDroneSE>();
        if (GameObject.Find("player") != null)
        {
            PlayerTransform = GameObject.Find("player").transform;
        }
        MonsterWaitAnimation = this.transform.GetChild(0).gameObject;
        MonsterAtkAnimation = this.transform.GetChild(1).gameObject;

        OpenTimer = OpenTimerSet;
        AtkTimer = AtkTimerSet;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerTransform)
        {
            DistanceX = _transform.localPosition.x - PlayerTransform.localPosition.x;
            AbsDistanceX = Mathf.Abs(DistanceX);
            DistanceY = _transform.localPosition.y - PlayerTransform.localPosition.y;
            AbsDistanceY = Mathf.Abs(DistanceY);
        }

        if(GameEvent.isAniPlay || PauseMenuController.isPauseMenuOpen)
        {
            return;
        }

        //角度控制
        CaculateRotate();

        if (!isOpen && AbsDistanceX < OpenRangeX && AbsDistanceY < OpenRangeY && DistanceY > 0)
        {
            isOpen = true;
            _droneSE.OpenSoundPlay(0);
        }

        if (isOpen && !isRun)
        {
            OpenTimer -= Time.deltaTime;
            if(OpenTimer <= 0)
            {
                CaculateRoute();
                isRun = true;
            }
        }

        SwitchAni();
    }

    private void FixedUpdate()
    {
        _fixedDeltaTime = Time.fixedDeltaTime;
        if (isRun)
        {
            AtkTimer -= _fixedDeltaTime;

            RunRotate();
            _transform.localPosition = new Vector3(ParabolaNowX, ParabolaNowY, 0);

            ParabolaNowY -= Speed * _fixedDeltaTime;
            
            if(_transform.localPosition.y <= TopPoint.y + Speed * _fixedDeltaTime * 2)
            {
                Instantiate(Explosion, _transform.localPosition, Quaternion.identity);
                Destroy(this.gameObject);
            }

            if (GoRight)
            {
                ParabolaNowX = -Mathf.Pow((ParabolaNowY - TopPoint.y) * ParabolaConstant * 4, 0.5f) + TopPoint.x;
            }
            if (GoLeft)
            {
                ParabolaNowX = Mathf.Pow((ParabolaNowY - TopPoint.y) * ParabolaConstant * 4, 0.5f) + TopPoint.x;
            }
            if (GoStraight)
            {
                ParabolaNowX = _transform.localPosition.x;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && AtkTimer <= 0)
        {
            Instantiate(Explosion, _transform.localPosition, Quaternion.identity);
            Destroy(this.gameObject);
        }
        if (collision.tag == "LeftWall" && AtkTimer <= 0)
        {
            Instantiate(Explosion, _transform.localPosition, Quaternion.identity);
            Destroy(this.gameObject);
        }
        if (collision.tag == "RightWall" && AtkTimer <= 0)
        {
            Instantiate(Explosion, _transform.localPosition, Quaternion.identity);
            Destroy(this.gameObject);
        }
        if (collision.tag == "platform" && AtkTimer <= 0)
        {
            Instantiate(Explosion, _transform.localPosition, Quaternion.identity);
            Destroy(this.gameObject);
        }
        if (collision.tag == "SpecialPlatform" && AtkTimer <= 0)
        {
            Instantiate(Explosion, _transform.localPosition, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    private void SwitchAni()
    {
        if (isOpen || FixedVer)
        {
            MonsterWaitAnimation.SetActive(false);
            MonsterAtkAnimation.SetActive(true);
        }

        SEControll();
    }

    private void CaculateRotate()
    {
        if (PlayerTransform && isRun)
        {
            ParabolaNextY = ParabolaNowY - RotateCaculateDistance;
            if (GoRight)
            {
                ParabolaNextX = -Mathf.Pow((ParabolaNextY - TopPoint.y) * ParabolaConstant * 4, 0.5f) + TopPoint.x;
            }
            if (GoLeft)
            {
                ParabolaNextX = Mathf.Pow((ParabolaNextY - TopPoint.y) * ParabolaConstant * 4, 0.5f) + TopPoint.x;
            }

            NextDistanceX = Mathf.Abs(ParabolaNowX - ParabolaNextX);
            NextDistanceY = Mathf.Abs(ParabolaNowY - ParabolaNextY);

            Tan = NextDistanceX / NextDistanceY;
            TargetRotate = Mathf.Atan(Tan);
            TargetRotate = TargetRotate / Mathf.PI * 180;

            if (GoRight)
            {
                TargetRotate = 270 + TargetRotate;
            }
            if (GoLeft)
            {
                TargetRotate = 270 - TargetRotate;
            }
            if (GoStraight)
            {
                TargetRotate = 270;
            }
        }
    }

    private void RunRotate()
    {
        NowRotate = _transform.eulerAngles.z;

        if (NowRotate < TargetRotate - 0.4)
        {
            if (Mathf.Abs(NowRotate - TargetRotate) <= 180)
            {
                NowRotate += RotateSpeed * _fixedDeltaTime;
                _transform.rotation = Quaternion.Euler(0, 0, NowRotate);
            }
            else
            {
                NowRotate -= RotateSpeed * _fixedDeltaTime;
                _transform.rotation = Quaternion.Euler(0, 0, NowRotate);
            }
        }
        if (NowRotate > TargetRotate + 0.4)
        {
            if (Mathf.Abs(NowRotate - TargetRotate) <= 180)
            {
                NowRotate -= RotateSpeed * _fixedDeltaTime;
                _transform.rotation = Quaternion.Euler(0, 0, NowRotate);
            }
            else
            {
                NowRotate += RotateSpeed * _fixedDeltaTime;
                _transform.rotation = Quaternion.Euler(0, 0, NowRotate);
            }
        }
    }

    private void CaculateRoute()
    {
        if (!FixedVer)
        {
            if (DistanceX < 0)
            {
                GoRight = true;
            }
            else
            {
                GoLeft = true;
            }
            if (AbsDistanceX < 0.1f)
            {
                GoLeft = false;
                GoRight = false;
                GoStraight = true;
            }
        }

        TopPoint = new Vector3(PlayerTransform.localPosition.x - DistanceX, _transform.localPosition.y - OpenRangeY * 1.5f, 0);
        if (GoRight)
        {
            TopPoint = new Vector3(TopPoint.x + (OpenRangeY - DistanceY) * 2, TopPoint.y, 0);
        }
        if (GoLeft)
        {
            TopPoint = new Vector3(TopPoint.x - (OpenRangeY - DistanceY) * 2, TopPoint.y, 0);
        }
        if (FixedVer)
        {
            TopPoint = new Vector3(_transform.localPosition.x + FixedTopPoint.x, _transform.localPosition.y + FixedTopPoint.y,0);
        }

        ParabolaConstant = ((_transform.localPosition.x - TopPoint.x) * (_transform.localPosition.x - TopPoint.x)) / 4 / (_transform.localPosition.y - TopPoint.y);
        if (ParabolaConstant < 0)
        {
            ParabolaConstant *= -1;
        }
        ParabolaNowX = _transform.localPosition.x;
        ParabolaNowY = _transform.localPosition.y;
    }

    private void SEControll()
    {
        if (isRun)
        {
            _droneSE.FastRunSoundPlay();
        }
    }
}
