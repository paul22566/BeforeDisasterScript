using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Training2Controller : MonoBehaviour
{
    private Transform PlayerTransform;
    private KeyCodeManage _keyCodeManage;
    public GameObject WeakCaptain;
    public TouchTrigger BossTrigger;
    private bool isDoEvent;
    private PlayerData _PlayerData;
    public BoxCollider2D AirWall;
    public Animator Tutorial1Ani;
    public Animator Tutorial2Ani;
    public Animator Tutorial3Ani;
    private GameObject TrainingDie;
    private PlayerDieController DIeUI;

    public GameObject HpUI;

    public GameObject AccumulateLightA;
    public GameObject AccumulateLightB;

    private float AniOneTimer;
    private float AniOneTimerSet = 2.5f;
    private bool AniOneComplete;
    private float AniTwoTimer;
    private float AniTwoTimerSet = 3.1f;

    private bool isAccumulate;
    private float AccumulateTimer;
    private float AccumulateTimerSet = 1;
    private bool AccumulateComplete;
    private bool isDecrease;
    [HideInInspector] public bool BeginAtk;

    //音效
    public AudioClip WakeUpSound;
    public AudioClip AccumulateSound;
    public AudioClip CriticAtkSound;
    private AudioSource WakeUpSource;
    private AudioSource AccumulateSource;
    private AudioSource CriticAtkSource;
    // Start is called before the first frame update
    void Start()
    {
        AccumulateTimer = AccumulateTimerSet;
        AniOneTimer = AniOneTimerSet;
        AniTwoTimer = AniTwoTimerSet;
        if (GameObject.Find("player") != null)
        {
            PlayerTransform = GameObject.Find("player").transform;
        }
        if (GameObject.Find("FollowSystem") != null)
        {
            _PlayerData = GameObject.Find("FollowSystem").GetComponent<PlayerData>();
        }
        if (GameObject.FindGameObjectWithTag("UI") != null)
        {
            Transform UICanvas = GameObject.FindGameObjectWithTag("UI").transform;

            _keyCodeManage = GameObject.FindGameObjectWithTag("UI").GetComponent<KeyCodeManage>();
            DIeUI = IdentifyID.FindObject(UICanvas, UIID.Die).GetComponent<PlayerDieController>();
        }
        if (GameObject.Find("TrainingUI") != null)
        {
            TrainingDie = GameObject.Find("TrainingUI");
        }

        //音效
        WakeUpSource = this.AddComponent<AudioSource>();
        AccumulateSource = this.AddComponent<AudioSource>();
        CriticAtkSource = this.AddComponent<AudioSource>();

        AccumulateSource.loop = true;

        WakeUpSource.clip = WakeUpSound;
        AccumulateSource.clip = AccumulateSound;
        CriticAtkSource.clip = CriticAtkSound;
    }

    // Update is called once per frame
    void Update()
    {
        if (BossTrigger.isTouch)
        {
            BossTrigger.gameObject.SetActive(false);
            BossTrigger.isTouch = false;
            HpUI.SetActive(true);
        }

        if (!isDoEvent && WeakCaptain == null)
        {
            isDoEvent = true;
            MusicController.ChangeBGM();
            BattleSystem.IncreaseTimes += 60;
            DIeUI._dieTimerEnd = null;
            Destroy(TrainingDie);
        }

        if (isDoEvent)
        {
            GameEvent.isAniPlay = true;

            if (AirWall.isTrigger == false)
            {
                AirWall.isTrigger = true;
            }

            if (AniOneTimer > 0)
            {
                AniOneTimer -= Time.deltaTime;
                if (AniOneTimer <= (AniOneTimerSet - 1.5))
                {
                    AniOneComplete = true;
                }
                if (AniOneTimer <= 0)
                {
                    Tutorial1Ani.SetBool("Appear", true);
                }
            }

            if (!AniOneComplete)
            {
                return;
            }

            if (BeginAtk)
            {
                if (!isDecrease)
                {
                    BattleSystem.DecreaseTimes += 20;
                    WakeUpSource.PlayDelayed(1f);
                    CriticAtkSource.PlayDelayed(1.5f);
                    isDecrease = true;
                }
                Tutorial2Ani.SetBool("Disappear", true);
                Tutorial3Ani.SetBool("Disappear", true);
                AniTwoTimer -= Time.deltaTime;
                if (AniTwoTimer <= 0)
                {
                    LoadScene.LoadSceneWithWhiteBackground = true;
                    LoadScene.SceneName = "original";
                    SceneManager.LoadScene("RestoreSpace");
                }
                return;
            }

            AccumulateLightA.transform.localPosition = PlayerTransform.localPosition;
            AccumulateLightB.transform.localPosition = PlayerTransform.localPosition;

            /*if (Input.GetKeyDown(KeyCodeManage.NormalAtk))
            {
                isAccumulate = true;
            }
            if (Input.GetKeyUp(KeyCodeManage.NormalAtk))
            {
                isAccumulate = false;
                if (AccumulateComplete)
                {
                    BeginAtk = true;
                    CheckPoint.CheckPointNumber = 1;
                    GameEvent.TutorialComplete = true;
                    _PlayerData.CommonSave();
                }
            }
            if (_keyCodeManage.NormalAtkPressed)
            {
                isAccumulate = true;
                _keyCodeManage.NormalAtkPressed = false;
            }
            if (_keyCodeManage.NormalAtkUp)
            {
                isAccumulate = false;
                if (AccumulateComplete)
                {
                    GameEvent.TutorialComplete = true;
                    CheckPoint.CheckPointNumber = 1;
                    BeginAtk = true;
                    _PlayerData.CommonSave();
                }
                _keyCodeManage.NormalAtkUp = false;
            }*/
            if (_keyCodeManage.QTENormalAtkPressed)
            {
                isAccumulate = true;
            }
            if (_keyCodeManage.QTENormalAtkUp)
            {
                isAccumulate = false;
                if (AccumulateComplete)
                {
                    GameEvent.TutorialComplete = true;
                    CheckPoint.CheckPointNumber = 1;
                    BeginAtk = true;
                    _PlayerData.CommonSave();
                }
            }

            if (isAccumulate)
            {
                AccumulateTimer -= Time.deltaTime;
                Tutorial1Ani.SetBool("Disappear", true);
                if (AccumulateTimer <= 0)
                {
                    AccumulateComplete = true;
                    Tutorial2Ani.SetBool("Appear", true);
                    Tutorial3Ani.SetBool("Appear", true);
                }
            }
            else
            {
                AccumulateTimer = AccumulateTimerSet;
            }

            //蓄力動畫
            if (isAccumulate)
            {
                if (!AccumulateSource.isPlaying)
                {
                    AccumulateSource.Play();
                }
                AccumulateLightA.SetActive(true);
                AccumulateLightA.SetActive(true);
            }
            else
            {
                if (AccumulateSource.isPlaying)
                {
                    AccumulateSource.Stop();
                }
                AccumulateLightA.SetActive(false);
                AccumulateLightA.SetActive(false);
            }

            //音效
            SEController.CalculateSystemSound(WakeUpSource);
            SEController.CalculateSystemSound(AccumulateSource);
            SEController.CalculateSystemSound(CriticAtkSource);
        }
    }
}
