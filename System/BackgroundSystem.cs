using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundSystem : MonoBehaviour
{
    public static string debugSceneName;//���Q��Lscript�Ψ�(CreatePlayer)
    public static string NowSceneName;//���Q��Lscript�Ψ�(restplace)
    //startPoint���` 1 ~10 �q��: 1x ���I: 2x
    public static float startPointNumber;//���Q��Lscript�Ψ�(playercontroller�Acreateplayer)
    public static float GameSpeed = 1;//��U�C���t��
    public static float BasicGameSpeed = 1;//����C���t�� ���ձM��
    private GameObject NormalFadeOut;
    public GameObject LeftEdge;
    public GameObject RightEdge;
    [HideInInspector] public float LeftEdgeX;//���Q��Lscript�Ψ�(CasterController�ALeftAimWall�AplayerController)
    [HideInInspector] public float RightEdgeX;//���Q��Lscript�Ψ�(CasterController�ARightAimWall�AplayerController)
    public static bool isNoticeDialogAppear = false;//���ܵ����X�{(�|���׳����ާ@)
    public static bool CantPause = true;//�ण����Ȱ���
    public enum Face { Right, Left};//�M�w���a�i�J�a�ϫ᪺��V
    public static Face PlayerAssignFace;//script(Portal, ButtonPortal)

    [HideInInspector] public GameObject playerObject;
    private PlayerController _playerController;
    private PlayerTouchJudgement _playerTouch;

    private Transform Camera;//�n��mainCamera
    private Transform _cameraFollow;

    private void Awake()
    {
        if (GameObject.Find("player") != null)
        {
            playerObject = GameObject.Find("player");
            _playerController = playerObject.GetComponent<PlayerController>();
            //�����}�����m
            _playerController.isUpArrowPressed = false;
            _playerController.ReceiveRightWalkCommand = false;
            _playerController.ReceiveLeftWalkCommand = false;
            _playerController.isKeyZPressed = false;
            _playerController.touchLeftWall = false;
            _playerController.touchRightWall = false;
            _playerController.ShouldJudgeHurt = false;
            _playerTouch = GameObject.Find("PlayerTouchJudgement").GetComponent<PlayerTouchJudgement>();
            _playerTouch.isLeftSideHaveMonster = false;
            _playerTouch.isRightSideHaveMonster = false;
            _playerTouch.isMonsterUnder = false;
        }
    }
    void Start()
    {
        //�Ѽƪ�l��
        Camera = GameObject.Find("Main Camera").transform;
        _cameraFollow = GameObject.Find("cameraFollow").transform;
        Portal.isPortal = false;
        LoadScene.SceneName = null;

        //FadeOut���m
        if (GameObject.FindGameObjectWithTag("UI") != null)
        {
            Transform UICanvas;
            UICanvas = GameObject.FindGameObjectWithTag("UI").transform;
            NormalFadeOut = IdentifyID.FindObject(UICanvas, UIID.FadeOut);
            NormalFadeOut.SetActive(false);

            NormalFadeOut.GetComponent<FadeOutUI>()._fadeOutEnd = null;
        }

        if (GameObject.Find("player") == null)
        {
            debugSceneName = SceneManager.GetActiveScene().name;
        }
        NowSceneName = SceneManager.GetActiveScene().name;
        if (LeftEdge != null)
        {
            LeftEdgeX = LeftEdge.transform.position.x;
        }
        if (RightEdge != null)
        {
            RightEdgeX = RightEdge.transform.position.x;
        }

        //����(�t��v���Mbgm)���A��l��
        if (GameObject.Find("player") != null)
        {
            //�Ψ�debug��
            if (CreatePlayer.isUseDebug)
            {
                GameObject g = GameObject.Find("1") as GameObject;
                if (g != null)
                {
                    playerObject.transform.position = g.transform.position;
                }
                CreatePlayer.isUseDebug = false;
            }
            //�M�w��V
            switch (PlayerAssignFace)
            {
                case Face.Left:
                    PlayerController.face = PlayerController.Face.Left;
                    break;
                case Face.Right:
                    PlayerController.face = PlayerController.Face.Right;
                    break;
            }
            //���`�i�J���I�ɶi�J���w��m
            if (startPointNumber != 0 && startPointNumber <= 10 && !GameEvent.isAniPlay)
            {
                if (startPointNumber >= 8)
                {
                    _playerController.isImpulse = true;
                    GameObject G = GameObject.Find(startPointNumber.ToString()) as GameObject;
                    if (G != null)
                    {
                        playerObject.transform.position = G.transform.position;
                        switch (PlayerAssignFace)
                        {
                            case Face.Right:
                                playerObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(400, 700), ForceMode2D.Impulse);
                                break;
                            case Face.Left:
                                playerObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-400, 700), ForceMode2D.Impulse);
                                break;
                        }
                    }
                    startPointNumber = 0;
                }
                else
                {
                    GameObject g = GameObject.Find(startPointNumber.ToString()) as GameObject;
                    if (g != null)
                    {
                        playerObject.transform.position = g.transform.position;
                    }
                    startPointNumber = 0;
                }
            }
            //�z�L�q��ǰe
            if(startPointNumber >= 11 && startPointNumber <= 20)
            {
                GameObject g = GameObject.Find(startPointNumber.ToString()) as GameObject;
                if (g != null)
                {
                    playerObject.transform.position = g.transform.position;
                }
                startPointNumber = 0;
            }
            if(playerObject.GetComponent<Rigidbody2D>().gravityScale == 0)
            {
                playerObject.GetComponent<Rigidbody2D>().gravityScale = 7;
            }
            // �ϥ�checkpoint
            if (PlayerController.isDie || CreatePlayer.isNewGame || CreatePlayer.isLoadGame)
            {
                PlayerController.isDie = false;
                CreatePlayer.isLoadGame = false;
                CreatePlayer.isNewGame = false;
                playerObject.GetComponent<PlayerController>().DieTimer = playerObject.GetComponent<PlayerController>().DieTimerSet;
                switch (CheckPoint.CheckPointNumber)
                {
                    case 0:
                        GameObject g0 = GameObject.Find("1") as GameObject;
                        if (g0 != null)
                        {
                            playerObject.transform.position = g0.transform.position;
                        }
                        break;
                    case 1:
                        GameObject g1 = GameObject.Find("1") as GameObject;
                        if (g1 != null)
                        {
                            playerObject.transform.position = g1.transform.position;
                        }
                        break;
                    case 2:
                        GameObject g2 = GameObject.Find("21") as GameObject;
                        if (g2 != null)
                        {
                            playerObject.transform.position = g2.transform.position;
                        }
                        break;
                    case 3:
                        GameObject g3 = GameObject.Find("22") as GameObject;
                        if (g3 != null)
                        {
                            playerObject.transform.position = g3.transform.position;
                        }
                        break;
                    case 4:
                        GameObject g4 = GameObject.Find("23") as GameObject;
                        if (g4 != null)
                        {
                            playerObject.transform.position = g4.transform.position;
                        }
                        break;
                    case 5:
                        GameObject g5 = GameObject.Find("24") as GameObject;
                        if (g5 != null)
                        {
                            playerObject.transform.position = g5.transform.position;
                        }
                        break;
                    case 6:
                        GameObject g6 = GameObject.Find("25") as GameObject;
                        if (g6 != null)
                        {
                            playerObject.transform.position = g6.transform.position;
                        }
                        break;
                }
            }
            //��l����v����m
            Camera.position = new Vector3(playerObject.transform.position.x, playerObject.transform.position.y, Camera.position.z);
            _cameraFollow.position = new Vector3(playerObject.transform.position.x, playerObject.transform.position.y, _cameraFollow.position.z);
        }
        else // debug��
        {
            SceneManager.LoadScene("CreatePlayer");
        }
    }

    private void Update()
    {
        Time.timeScale = GameSpeed * BasicGameSpeed;
    }
    public void ChangeScene() //���Q��Lscript�Ψ�(FadeOutNormal)
    {
        SceneManager.LoadScene("Loading");
    }
}
