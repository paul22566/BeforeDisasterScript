using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class StairController : MonoBehaviour
{
    [HideInInspector] public int EventNumber;//1透過通風管進入
    private float AniTimerSet = 3;
    private float AniTimer;

    public GameObject Door;
    private PlayerData _playerData;
    public FadeInController _fadeIn;

    public Transform FencePoint;
    public Transform FenceTransform;

    public FragileWall _fragileWall;
    public OnceTimeHiddenWall _hiddenWall;

    private void Awake()
    {
        if (FirstFloor2Controller.Use1F_2Pipeline)
        {
            EventNumber = 1;
            GameEvent.isAniPlay = true;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        AniTimer = AniTimerSet;
        if (GameObject.Find("FollowSystem") != null)
        {
            _playerData = GameObject.Find("FollowSystem").GetComponent<PlayerData>();
        }
        if (EventNumber != 1)
        {
            _fadeIn.OpenFadeIn();
        }
        if (GameEvent.OpenStoreRoomDoor)
        {
            Destroy(Door);
        }
        if (GameEvent.Enter1F_2PipeLine)
        {
            FenceTransform.position = FencePoint.position;
        }
        if (!GameEvent.FoundStairHiddenWall)
        {
            _fragileWall.InisializeFragileWall();
        }
        else
        {
            Destroy(_hiddenWall.gameObject);
            Destroy(_fragileWall.gameObject);
        }
    }

    private void Update()
    {
        FragileWallControll();
    }

    private void FixedUpdate()
    {
        if (EventNumber == 1)
        {
            AniTimer -= Time.fixedDeltaTime;
            if (AniTimer <= 0)
            {
                GameEvent.isAniPlay = false;
                FirstFloor2Controller.Use1F_2Pipeline = false;
                EventNumber = 0;
                _playerData.CommonSave();
            }
        }
    }

    private void FragileWallControll()
    {
        if (!GameEvent.FoundStairHiddenWall)
        {
            if (_fragileWall.isOpen)
            {
                GameEvent.FoundStairHiddenWall = true;
                _hiddenWall.BeginDisappear();
                _playerData.CommonSave();
                _fragileWall.isOpen = false;
            }
        }
    }
}
