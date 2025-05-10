using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    private Transform target;
    public Transform SpecialTarget;
    private Transform _transform;
    [HideInInspector] public MapFrame MainMapFrame = new MapFrame();
    private Transform MapFrameTransform;
    private MapFrame NowUseMapFrame = new MapFrame();
    private CinemachineVirtualCamera vcam;
    private bool isLock;
    [HideInInspector] public bool ShouldReCalculateValidLine;//有其他script有管理MapFrame時才會用到
    [HideInInspector] public bool UseDefaultMapFrame = true;//如果要關掉，要用其他script在awake關掉
    [HideInInspector] public float CameraMoveDistanceX;//特殊事件中相機移動距離
    [HideInInspector] public float CameraMoveDistanceY;//特殊事件中相機移動距離

    public bool FixedX;
    public bool FixedY;
    public float FixedXLine;
    public float FixedYLine;

    private float ChangeTime = 2f;
    private float ChangeTimeSet = 2f;
    private bool Changing;

    private float DefaultCameraSize = 7;// 7是相機固定值(從相機介面調整)
    private float NowScreenWidth = 0;
    private float NowScreenHeight = 0;
    private float WidthDistance = 0;
    private float HeightDistance = 0;
    private float NowScreenScale = 0;
    private float LastScreenWidth = 0;
    private float LastScreenHeight = 0;

    public bool PerspectiveMap;//是否為透視視角地圖

    //只有需要地圖邊框移動的圖才會需要用到
    public Transform RealityMapTransform;
    private MapFrame RealityMapFrame = new MapFrame();
    private bool UseImmediatelyChange;
    private float ResetTimerSet = 4;
    private float ResetTimer = 4;

    void Start()
    {
        if (GameObject.Find("player") != null)
        {
            target = GameObject.Find("player").transform;
        }
        if (GameObject.Find("Camera") != null)
        {
            vcam = GameObject.Find("Camera").GetComponent<CinemachineVirtualCamera>();
        }
        if (target == null)
        {
            return;
        }

        _transform = this.transform;
        if (GameObject.FindGameObjectWithTag("MapFrame").transform != null)
        {
            MapFrameTransform = GameObject.FindGameObjectWithTag("MapFrame").transform;
        }
        ScreenDataCalculate();
        InitializeMapFrame(MainMapFrame, MapFrameTransform.localPosition.x, MapFrameTransform.localPosition.y, MapFrameTransform.localScale.x, MapFrameTransform.localScale.y);
        LastScreenHeight = Screen.height;
        LastScreenWidth = Screen.width;

        if (RealityMapTransform != null)
        {
            InitializeMapFrame(RealityMapFrame, RealityMapTransform.localPosition.x, RealityMapTransform.localPosition.y, RealityMapTransform.localScale.x, RealityMapTransform.localScale.y);
        }

        if (UseDefaultMapFrame)
        {
            if (RealityMapTransform != null)
            {
                ImmediatelyFrameSet(MainMapFrame);
            }
            else
            {
                FrameSet(MainMapFrame);
            }
        }
    }
    
    void Update()
    {
        //鎖定
        if (target == null || GameEvent.isAniPlay || Changing)
        {
            isLock = true;
        }
        //解鎖
        if (target != null && !GameEvent.isAniPlay && !Changing)
        {
            isLock = false;
        }

        ScreenDataCalculate();

        if (!isLock)
        {
            Vector3 followPos = new Vector3(target.position.x, target.position.y, _transform.position.z);

            if (!PerspectiveMap && !Changing)
            {
                followPos = TargetPointLimit(followPos, NowUseMapFrame);
            }

            followPos = TargetFixed(followPos);

            _transform.position = followPos;
        }
        if (NowScreenWidth != LastScreenWidth || NowScreenHeight != LastScreenHeight)
        {
            ShouldReCalculateValidLine = true;
            CalculateValidLine(MainMapFrame);
            CalculateValidLine(NowUseMapFrame);
            LastScreenHeight = Screen.height;
            LastScreenWidth = Screen.width;
        }

        ReturnToRealityMapTransform();
    }

    public void FollowPlayer()
    {
        Vector3 followPos = new Vector3(target.position.x, target.position.y, _transform.position.z);

        if (!PerspectiveMap && !Changing)
        {
            followPos = TargetPointLimit(followPos, NowUseMapFrame);
        }

        followPos = TargetFixed(followPos);

        _transform.position = followPos;
    }

    public void FollowSpecialTarget()
    {
        Vector3 followPos = new Vector3(SpecialTarget.position.x, SpecialTarget.position.y, _transform.position.z);

        if (!PerspectiveMap && !Changing)
        {
            followPos = TargetPointLimit(followPos, NowUseMapFrame);
        }

        followPos = TargetFixed(followPos);

        _transform.position = followPos;
    }

    private Vector3 TargetFixed(Vector3 FollowPos)
    {
        if (FixedX && !FixedY)
        {
            FollowPos = new Vector3(FixedXLine, FollowPos.y, FollowPos.z);
        }
        if (FixedY && !FixedX)
        {
            FollowPos = new Vector3(FollowPos.x, FixedYLine, FollowPos.z);
        }
        if (FixedY && FixedX)
        {
            FollowPos = new Vector3(FixedXLine, FixedYLine, FollowPos.z);
        }

        return FollowPos;
    }

    private void ScreenDataCalculate()
    {
        NowScreenHeight = Screen.height;
        NowScreenWidth = Screen.width;
        NowScreenScale = NowScreenWidth / NowScreenHeight;

        HeightDistance = vcam.m_Lens.OrthographicSize * 2;
        WidthDistance = HeightDistance * NowScreenScale;
    }

    public void FrameSet(MapFrame TargetMapFrame)
    {
        NowUseMapFrame = TargetMapFrame;

        if (target == null)
        {
            return;
        }

        Vector3 followPos = new Vector3(target.position.x, target.position.y, _transform.position.z);

        followPos = TargetPointLimit(followPos, NowUseMapFrame);

        followPos = TargetFixed(followPos);

        _transform.position = followPos;
    }

    public void ImmediatelyFrameSet(MapFrame TargetMapFrame)
    {
        NowUseMapFrame = TargetMapFrame;

        if (target == null)
        {
            return;
        }

        Vector3 followPos = new Vector3(target.position.x, target.position.y, _transform.position.z);

        followPos = TargetPointLimit(followPos, NowUseMapFrame);

        followPos = TargetFixed(followPos);

        _transform.position = followPos;

        UseImmediatelyChange = true;
        RealityMapTransform.position = new Vector3(TargetMapFrame.PointX, TargetMapFrame.PointY, 0);
        if(TargetMapFrame.ScaleX < WidthDistance)
        {
            RealityMapTransform.localScale = new Vector3(WidthDistance, TargetMapFrame.ScaleY, 1);
        }
        else
        {
            RealityMapTransform.localScale = new Vector3(TargetMapFrame.ScaleX, TargetMapFrame.ScaleY, 1);
        }
    }//順間將攝影機移動完成(需要有其他特效配合)

    private void ReturnToRealityMapTransform()
    {
        if (UseImmediatelyChange)
        {
            ResetTimer -= Time.deltaTime;

            if (ResetTimer <= ResetTimerSet - 2)
            {
                RealityMapTransform.localScale = new Vector3(RealityMapTransform.localScale.x + 3 * Time.deltaTime, RealityMapTransform.localScale.y + 3 * Time.deltaTime, 1);
            }
            if (ResetTimer <= 0)
            {
                RealityMapTransform.position = new Vector3(RealityMapFrame.PointX, RealityMapFrame.PointY, 0);
                RealityMapTransform.localScale = new Vector3(RealityMapFrame.ScaleX, RealityMapFrame.ScaleY, 1);
                UseImmediatelyChange = false;
                ResetTimer = ResetTimerSet;
            }
        }
    }

    public void ChangeTimeReset()
    {
        ChangeTime = ChangeTimeSet;
    }

    //使用在切換地圖
    public void FrameTransformChange(MapFrame OriginalFrame, MapFrame TargetFrame, float DeltaTime, ref bool Complete)
    {
        if (!Changing)
        {
            //MapFrameTransform.localPosition = new Vector3(29.2498f, 4.3523f, 0);
            //MapFrameTransform.localScale = new Vector3(92.59263f, 24.18251f, 1);
            Changing = true;
        }

        ChangeTime -= DeltaTime;

        FrameChangeCameraFollow(OriginalFrame, TargetFrame, DeltaTime);

        if (ChangeTime <= 0)
        {
            Changing = false;
            ChangeTime = ChangeTimeSet;
            Complete = true;
            FrameSet(TargetFrame);
        }
    }

    private void FrameChangeCameraFollow(MapFrame OriginalFrame, MapFrame TargetFrame, float DeltaTime)//邊框移動時，cameraFollow的移動路徑
    {
        Vector3 OriginalCameraPoint;
        Vector3 TargetCameraPoint;
        Vector3 NowPosition;
        float PointXChange;
        float PointYChange;
        float PointXSpeed;
        float PointYSpeed;

        OriginalCameraPoint = _transform.localPosition;

        //決定方向與TargetPoint
        TargetCameraPoint = OriginalCameraPoint;
        if (TargetCameraPoint.x > TargetFrame.ValidRightLine)
        {
            TargetCameraPoint = new Vector3(TargetFrame.ValidRightLine, TargetCameraPoint.y, 0);
        }
        if (TargetCameraPoint.x < TargetFrame.ValidLeftLine)
        {
            TargetCameraPoint = new Vector3(TargetFrame.ValidLeftLine, TargetCameraPoint.y, 0);
        }
        if (TargetCameraPoint.y > TargetFrame.ValidTopLine)
        {
            TargetCameraPoint = new Vector3(TargetCameraPoint.x, TargetFrame.ValidTopLine, 0);
        }
        if (TargetCameraPoint.x < TargetFrame.ValidBottomLine)
        {
            TargetCameraPoint = new Vector3(TargetCameraPoint.x, TargetFrame.ValidBottomLine, 0);
        }
        
        //決定移動速度 預設0.5秒跑完
        PointXChange = TargetCameraPoint.x - OriginalCameraPoint.x;
        PointYChange = TargetCameraPoint.y - OriginalCameraPoint.y;
        PointXSpeed = PointXChange / 0.5f;
        PointYSpeed = PointYChange / 0.5f;

        //實際移動
        NowPosition = new Vector3(_transform.localPosition.x + PointXSpeed * DeltaTime, _transform.localPosition.y + PointYSpeed * DeltaTime, 0);
        if (PointXChange > 0)
        {
            if (NowPosition.x > TargetCameraPoint.x)
            {
                NowPosition = new Vector3(TargetCameraPoint.x, NowPosition.y, 0);
            }
        }
        if (PointXChange < 0)
        {
            if (NowPosition.x < TargetCameraPoint.x)
            {
                NowPosition = new Vector3(TargetCameraPoint.x, NowPosition.y, 0);
            }
        }
        if (PointYChange > 0)
        {
            if (NowPosition.y > TargetCameraPoint.y)
            {
                NowPosition = new Vector3(NowPosition.x, TargetCameraPoint.y, 0);
            }
        }
        if (PointYChange < 0)
        {
            if (NowPosition.y < TargetCameraPoint.y)
            {
                NowPosition = new Vector3(NowPosition.x, TargetCameraPoint.y, 0);
            }
        }
        _transform.localPosition = NowPosition;
    }

    public void SpecialTargetHorizontalMove(float Distance, float RunTime, float DeltaTime, MapFrame Map)
    {
        float Speed;
        Vector3 TargetPoint;
        Speed = Distance / RunTime;
        TargetPoint = new Vector3(SpecialTarget.localPosition.x + Speed * DeltaTime, SpecialTarget.localPosition.y, 0);
        if (TargetPoint.x > Map.ValidRightLine)
        {
            TargetPoint = new Vector3(Map.ValidRightLine, SpecialTarget.localPosition.y, 0);
        }
        if (TargetPoint.x < Map.ValidLeftLine)
        {
            TargetPoint = new Vector3(Map.ValidLeftLine, SpecialTarget.localPosition.y, 0);
        }
        SpecialTarget.localPosition = TargetPoint;
    }

    public void SpecialTargetVerticalMove(float Distance, float RunTime, float DeltaTime, MapFrame Map)
    {
        float Speed;
        Vector3 TargetPoint;
        Speed = Distance / RunTime;
        TargetPoint = new Vector3(SpecialTarget.localPosition.x, SpecialTarget.localPosition.y + Speed * DeltaTime, 0);
        if (TargetPoint.y > Map.ValidTopLine)
        {
            TargetPoint = new Vector3(SpecialTarget.localPosition.x, Map.ValidTopLine, 0);
        }
        if (TargetPoint.y < Map.ValidBottomLine)
        {
            TargetPoint = new Vector3(SpecialTarget.localPosition.x, Map.ValidBottomLine, 0);
        }
        SpecialTarget.localPosition = TargetPoint;
    }

    public void InitializeMapFrame(MapFrame Map, float PointX, float PointY, float ScaleX, float ScaleY)
    {
        Map.PointX = PointX;
        Map.PointY = PointY;
        Map.ScaleX = ScaleX;
        Map.ScaleY = ScaleY;
        CalculateValidLine(Map);
    }

    private Vector3 TargetPointLimit(Vector3 TargetPoint, MapFrame Map)
    {
        if (TargetPoint.x > Map.ValidRightLine)
        {
            TargetPoint = new Vector3(Map.ValidRightLine, TargetPoint.y, 0);
        }
        if (TargetPoint.x < Map.ValidLeftLine)
        {
            TargetPoint = new Vector3(Map.ValidLeftLine, TargetPoint.y, 0);
        }
        if (TargetPoint.y > Map.ValidTopLine)
        {
            TargetPoint = new Vector3(TargetPoint.x, Map.ValidTopLine, 0);
        }
        if (TargetPoint.y < Map.ValidBottomLine)
        {
            TargetPoint = new Vector3(TargetPoint.x, Map.ValidBottomLine, 0);
        }

        return TargetPoint;
    }

    public void CalculateValidLine(MapFrame Map)
    {
        Map.MapFrameTop = Map.PointY + Map.ScaleY / 2;
        Map.MapFrameBottom = Map.PointY - Map.ScaleY / 2;
        Map.MapFrameLeft = Map.PointX - Map.ScaleX / 2;
        Map.MapFrameRight = Map.PointX + Map.ScaleX / 2;
        Map.ValidTopLine = Map.MapFrameTop - DefaultCameraSize;
        Map.ValidBottomLine = Map.MapFrameBottom + DefaultCameraSize;
        Map.ValidLeftLine = Map.MapFrameLeft + DefaultCameraSize * NowScreenScale;
        Map.ValidRightLine = Map.MapFrameRight - DefaultCameraSize * NowScreenScale;
    }

    public Vector3 CalculateValidPoint(Vector3 ValidPoint)
    {

        if (ValidPoint.x > NowUseMapFrame.ValidRightLine)
        {
            ValidPoint = new Vector3(NowUseMapFrame.ValidRightLine, ValidPoint.y, 0);
        }
        if (ValidPoint.x < NowUseMapFrame.ValidLeftLine)
        {
            ValidPoint = new Vector3(NowUseMapFrame.ValidLeftLine, ValidPoint.y, 0);
        }
        if (ValidPoint.y > NowUseMapFrame.ValidTopLine)
        {
            ValidPoint = new Vector3(ValidPoint.x, NowUseMapFrame.ValidTopLine, 0);
        }
        if (ValidPoint.y < NowUseMapFrame.ValidBottomLine)
        {
            ValidPoint = new Vector3(ValidPoint.x, NowUseMapFrame.ValidBottomLine, 0);
        }

        return ValidPoint;
    }//計算最接近的有效點

    public void ChangeCameraSize(float Speed, float Target, float DeltaTime)
    {
        float Size = vcam.m_Lens.OrthographicSize;

        Size += Speed * DeltaTime;

        if (Speed > 0 && Size >= Target)
        {
            vcam.m_Lens.OrthographicSize = Target;
            return;
        }
        if (Speed < 0 && Size <= Target)
        {
            vcam.m_Lens.OrthographicSize = Target;
            return;
        }
        
        vcam.m_Lens.OrthographicSize = Size;
    }

    public void MovingChangeCameraSize(Transform Target, float Speed, float DeltaTime)
    {
        float OriginalTargetDistance = 0;
        float TargetDistance = 0;
        float WallCheck = 0;
        bool TouchWall = false;
        bool TouchRight = false;
        bool TouchLeft = false;
        float TargetSize;
        float Constant = 2f;//距離邊界多少就變化
        float TouchWallDistance = 0;
        float Offsets = 0;
        float LimitSize = 0;

        OriginalTargetDistance = Target.position.x - _transform.position.x;
        TargetDistance = Mathf.Abs(OriginalTargetDistance);

        //判斷擴張後的攝影機會不會撞牆
        WallCheck = TargetDistance + Constant;
        if ((_transform.position.x - WallCheck) < NowUseMapFrame.MapFrameLeft)
        {
            TouchWallDistance = Mathf.Abs(_transform.position.x - NowUseMapFrame.MapFrameLeft);
            TouchWall = true;
            TouchLeft = true;
        }
        if ((_transform.position.x + WallCheck) > NowUseMapFrame.MapFrameRight)
        {
            TouchWallDistance = Mathf.Abs(_transform.position.x - NowUseMapFrame.MapFrameRight);
            TouchWall = true;
            TouchRight = true;
        }

        if (TouchWall)
        {
            Offsets = Mathf.Abs(TargetDistance - (TouchWallDistance - 2)) / 2;
            if (TargetDistance < (TouchWallDistance - 2))
            {
                Offsets = 0;
            }
        }

        TargetSize = (TargetDistance + Constant - Offsets) / NowScreenScale;

        LimitSize = NowUseMapFrame.MapFrameRight - NowUseMapFrame.MapFrameLeft;
        LimitSize = LimitSize / NowScreenScale / 2;

        //限制範圍
        if (TouchLeft && OriginalTargetDistance < 0 && !TouchRight)
        {
            TargetSize = 7;
        }
        if (TouchRight && OriginalTargetDistance > 0 && !TouchLeft)
        {
            TargetSize = 7;
        }
        if (TargetSize < 7)
        {
            TargetSize = 7;
        }
        if (TargetSize > LimitSize)
        {
            TargetSize = LimitSize;
        }

        if (TargetSize < vcam.m_Lens.OrthographicSize)
        {
            Speed *= -1;
        }
        
        ChangeCameraSize(Speed, TargetSize, DeltaTime);
        
    }//依據target位置判斷size(limit是以左邊螢幕為準)

    public void ShowNowUseFrame()
    {
        print(NowUseMapFrame.PointX);
        print(NowUseMapFrame.PointY);
        print(NowUseMapFrame.ScaleX);
        print(NowUseMapFrame.ScaleY);
    }//debug專用

    public void ShowMapDetail(MapFrame Map)
    {
        print(Map.PointX);
        print(Map.PointY);
        print(Map.ScaleX);
        print(Map.ScaleY);
        print(Map.MapFrameTop);
        print(Map.MapFrameBottom);
        print(Map.MapFrameLeft);
        print(Map.MapFrameRight);
        print(Map.ValidTopLine);
        print(Map.ValidBottomLine);
        print(Map.ValidLeftLine); 
        print(Map.ValidRightLine);
    }//debug專用
}

public class MapFrame
{
    public float PointX;
    public float PointY;
    public float ScaleX;
    public float ScaleY;

    public float MapFrameTop;
    public float MapFrameBottom;
    public float MapFrameLeft;
    public float MapFrameRight;
    //followPoint極限有效座標
    public float ValidTopLine;
    public float ValidBottomLine;
    public float ValidLeftLine;
    public float ValidRightLine;
}
