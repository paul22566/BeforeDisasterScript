using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerSpecialAni : MonoBehaviour
{
    public GameObject SkillPowerFrame;
    public GameObject SkillPowerUi;
    public GameObject SkillPowerFrame2;
    public GameObject SkillPowerUi2;
    public GameObject SkillPoint4, SkillPoint5;
    private PlayerAnimationController _aniController;
    public GameObject PlayerSpecialAnimation;
    public GameObject PlayerUI;
    private SkillPowerChangeAni _skillPowerAni;

    private GameObject OpeningAnimation;
    private GameObject TrainingEndAnimation;
    private GameObject GoInOriginalAnimation;
    private GameObject HallAnimation;
    private GameObject GoIn1F_1Animation;
    private GameObject GoInRestRoomAnimation;
    private GameObject SecretRoom1Animation;
    private GameObject Boss1RoomAnimation;
    private GameObject PlayerAbsorb2Animation;
    private GameObject PlayerWinEvilKing;
    private GameObject PlayerWinEvilKing2;
    private GameObject PlayerWinEvilKing3;

    private Animator OpeningAni;
    private Animator TrainingEndAni;
    private Animator OriginalAni;
    private Animator HallAni;
    private Animator Boss1RoomAni;

    [Header("特殊動畫參數")]
    //擊敗妖王動畫
    public GameObject SwordRoute;
    public GameObject EvilBigGun;
    public GameObject EvilBigGunDisappear;
    private GameObject EvilBigGunRecord;
    private GameObject EvilBigGunDisappearRecord;

    private bool HasPlaySpecialAni;
    // Start is called before the first frame update
    void Start()
    {
        _aniController = this.GetComponent<PlayerAnimationController>();

        _skillPowerAni = PlayerUI.GetComponent<SkillPowerChangeAni>();

        OpeningAnimation = PlayerSpecialAnimation.transform.GetChild(0).gameObject;
        TrainingEndAnimation = PlayerSpecialAnimation.transform.GetChild(1).gameObject;
        GoInOriginalAnimation = PlayerSpecialAnimation.transform.GetChild(2).gameObject;
        HallAnimation = PlayerSpecialAnimation.transform.GetChild(3).gameObject;
        GoIn1F_1Animation = PlayerSpecialAnimation.transform.GetChild(4).gameObject;
        GoInRestRoomAnimation = PlayerSpecialAnimation.transform.GetChild(5).gameObject;
        SecretRoom1Animation = PlayerSpecialAnimation.transform.GetChild(6).gameObject;
        Boss1RoomAnimation = PlayerSpecialAnimation.transform.GetChild(7).gameObject;
        PlayerAbsorb2Animation = PlayerSpecialAnimation.transform.GetChild(8).gameObject;
        PlayerWinEvilKing = PlayerSpecialAnimation.transform.GetChild(9).gameObject;
        PlayerWinEvilKing2 = PlayerSpecialAnimation.transform.GetChild(10).gameObject;
        PlayerWinEvilKing3 = PlayerSpecialAnimation.transform.GetChild(11).gameObject;

        OpeningAni = OpeningAnimation.transform.GetChild(0).GetComponent<Animator>();
        TrainingEndAni = TrainingEndAnimation.transform.GetChild(0).GetComponent<Animator>();
        OriginalAni = GoInOriginalAnimation.transform.GetChild(0).GetComponent<Animator>();
        HallAni = HallAnimation.transform.GetChild(0).GetComponent<Animator>();
        Boss1RoomAni = Boss1RoomAnimation.transform.GetChild(0).GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameEvent.isAniPlay)
        {
            HasPlaySpecialAni = true;
        }
        if (!GameEvent.isAniPlay && HasPlaySpecialAni)
        {
            SpecialAniFalse();
        }
    }
    public void OpeningAniPlay(int Phase)
    {
        switch (Phase)
        {
            case 1:
                OpeningAnimation.SetActive(true);
                _aniController.AbsoluteAniFalse();
                break;
            case 2:
                OpeningAnimation.SetActive(true);
                OpeningAni.SetBool("Begin", true);
                _aniController.AbsoluteAniFalse();
                break;
        }
    }

    public void TrainingEndAniPlay()
    {
        TrainingEndAnimation.SetActive(true);
        _aniController.AbsoluteAniFalse();
    }

    public void TrainingEndAniChange()
    {
        TrainingEndAni.SetBool("BeginAtk", true);
    }

    public void GoInOriginalAniPlay(int Phase)
    {
        GoInOriginalAnimation.SetActive(true);
        _aniController.AbsoluteAniFalse();
        if(Phase == 2)
        {
            OriginalAni.SetBool("Phase2", true);
        }
    }

    public void HallAniPlay(int Phase)
    {
        HallAnimation.SetActive(true);
        _aniController.AbsoluteAniFalse();
        switch(Phase)
        {
            case 1:
                break;
            case 2:
                HallAni.SetBool("HallEnd", true);
                break;
            case 3:
                HallAni.SetBool("Run", true);
                break;
        }
    }

    public void GoIn1F_1AniPlay()
    {
        GoIn1F_1Animation.SetActive(true);
        _aniController.AbsoluteAniFalse();
    }

    public void GoInRestRoomPlay()
    {
        GoInRestRoomAnimation.SetActive(true);
        _aniController.AbsoluteAniFalse();
    }

    public void SecretRoom1Play()
    {
        SecretRoom1Animation.SetActive(true);
        _aniController.AbsoluteAniFalse();
    }

    public void Boss1RoomPlay(int Status)
    {
        Boss1RoomAnimation.SetActive(true);
        _aniController.AbsoluteAniFalse();
        switch(Status)
        {
            case 1://firstMeet
                Boss1RoomAni.SetInteger("Status", 1);
                break;
            case 2://BossEndUnMove
                Boss1RoomAni.SetInteger("Status", 2);
                break;
            case 3://BossEndTurn
                Boss1RoomAni.SetInteger("Status", 5);
                Boss1RoomAni.SetBool("Move", true);
                break;
            case 4://BossEndWalk
                Boss1RoomAni.SetInteger("Status", 3);
                Boss1RoomAni.SetBool("Move", true);
                break;
            case 5://BossEndDash
                Boss1RoomAni.SetInteger("Status", 4);
                Boss1RoomAni.SetBool("Move", true);
                break;
            case 6://BossEndStop
                Boss1RoomAni.SetInteger("Status", 2);
                Boss1RoomAni.SetBool("Move", false);
                break;
            case 7://BossEndFinal
                Boss1RoomAni.SetInteger("Status", 6);
                break;
            case 8://BossEndFinal(Up)
                Boss1RoomAni.SetInteger("Status", 7);
                break;
            case 9://AbsorbBoss1
                Boss1RoomAni.SetInteger("Status", 8);
                break;
        }
    }

    public void AbsorbBoss2AniPlay(string Face)
    {
        PlayerAbsorb2Animation.SetActive(true);
        switch (Face)
        {
            case "R":
                PlayerAbsorb2Animation.GetComponent<SpriteRenderer>().flipX = true;
                break;
            case "L":
                PlayerAbsorb2Animation.GetComponent<SpriteRenderer>().flipX = false;
                break;
        }
        _aniController.AbsoluteAniFalse();
    }

    public void TurnSpecialFace(string Direction, int AniOrder)
    {
        Transform AniTransform = HallAnimation.transform;

        switch (AniOrder)
        {
            case 3:
                AniTransform = HallAnimation.transform;
                break;
            case 6:
                AniTransform = SecretRoom1Animation.transform;
                break;
            case 7:
                AniTransform = Boss1RoomAnimation.transform;
                break;
        }

        switch (Direction)
        {
            case "R":
                AniTransform.GetChild(0).transform.localScale = new Vector3(0.28f, 0.28f, 1);
                break;
            case "L":
                AniTransform.GetChild(0).transform.localScale = new Vector3(-0.28f, 0.28f, 1);
                break;
        }
    }

    public void SpecialAniFalse()
    {
        OpeningAnimation.SetActive(false);
        TrainingEndAnimation.SetActive(false);
        GoInOriginalAnimation.SetActive(false);
        HallAnimation.SetActive(false);
        GoIn1F_1Animation.SetActive(false);
        GoInRestRoomAnimation.SetActive(false);
        SecretRoom1Animation.SetActive(false);
        Boss1RoomAnimation.SetActive(false);
        PlayerAbsorb2Animation.SetActive(false);
        PlayerWinEvilKing.SetActive(false);
        PlayerWinEvilKing2.SetActive(false);
        PlayerWinEvilKing3.SetActive(false);
    }

    public void HidePlayerUI()
    {
        PlayerUI.SetActive(false);
    }

    public void ShowPlayerUI()
    {
        PlayerUI.SetActive(true);
    }

    public void SealSkillPower(int phase)
    {
        if (phase == 1)
        {
            _skillPowerAni.BeginSealPower();
        }
        if (phase == 2)
        {
            _skillPowerAni.SealPowerEnd();
        }
    }
}
