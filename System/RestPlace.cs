using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestPlace : MonoBehaviour
{
    public int CheckPointNumber;//有被其他script用到(playercontroller)
    public GameObject RestPlaceAni;
    private float AniTimer;
    public float AniTimerSet;
    private bool BeginRest;
    private PlayerController _playerController;
    private InteractableObject _interactable;
    private itemManage _itemManage;
    private PlayerData _PlayerData;
    public GameObject FadeIn;
    public GameObject FadeInRestPlace;

    public static bool isOpenRestPlace;
    public static bool MonsterShouldDestroy;

    public GameObject RestSound;
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("player") != null)
        {
            _playerController = GameObject.Find("player").GetComponent<PlayerController>();
        }
        else
        {
            return;
        }
        AniTimer = AniTimerSet;
        if (GameObject.Find("FollowSystem") != null)
        {
            _itemManage = GameObject.Find("FollowSystem").GetComponent<itemManage>();
            _PlayerData = GameObject.Find("FollowSystem").GetComponent<PlayerData>();
        }
        _interactable = this.GetComponent<InteractableObject>();
        if (isOpenRestPlace)
        {
            FadeInRestPlace.SetActive(true);
            FadeIn.SetActive(false);
            isOpenRestPlace = false;
            MonsterShouldDestroy = false;
            _playerController.Hp = _playerController.MaxHp;
            _itemManage.RestoreUseItem();
            CheckPoint.CheckPointNumber = CheckPointNumber;
            Portal.isPortal = false;
            _PlayerData.CommonSave();
        }

        _interactable._interact += OnInteract;
    }

    // Update is called once per frame
    void Update()
    {
        TimerMethod();
    }

    private void OnInteract()
    {
        isOpenRestPlace = true;
        MonsterShouldDestroy = true;
        RestPlaceAni.SetActive(true);
        Instantiate(RestSound, this.transform.localPosition, Quaternion.identity);
        Portal.isPortal = true;
        BeginRest = true;
        _interactable.InteractSuccess();
    }

    void TimerMethod()
    {
        if (BeginRest)
        {
            if (AniTimer <= 0)
            {
                AniTimer = AniTimerSet;
            }

            AniTimer -= Time.deltaTime;

            if (AniTimer <= 0)
            {
                SceneManager.LoadScene(BackgroundSystem.NowSceneName);
                BeginRest = false;
            }
        }
    }
}
