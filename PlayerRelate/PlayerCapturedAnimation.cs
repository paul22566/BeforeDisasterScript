using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Creature;

public class PlayerCapturedAnimation : MonoBehaviour
{
    private Transform _transform;
    public Transform PlayerCapturedAni;

    private OldPlayerAnimationController _aniController;
    private string MonsterType;

    private GameObject CaptureByVBMonsterAnimation;
    private GameObject CaptureByCaptainAnimation;

    [HideInInspector] public MonsterCaptureController _monsterCaptureController;
    private Transform MonsterTransform;

    private bool BeCapture;
    // Start is called before the first frame update
    void Start()
    {
        _transform = this.transform;
        _aniController = this.GetComponent<OldPlayerAnimationController>();

        CaptureByVBMonsterAnimation = PlayerCapturedAni.GetChild(0).gameObject;
        CaptureByCaptainAnimation = PlayerCapturedAni.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (_monsterCaptureController != null && _monsterCaptureController.GetComponent<MonsterCaptureController>().isPlayerFollow)
        {
            BeCapture = true;

            switch (MonsterType)
            {
                case "Boss1":
                    switch (PlayerController._player.face)
                    {
                        case Face.Right:
                            CapturedByBoss1AniPlay("R");
                            break;
                        case Face.Left:
                            CapturedByBoss1AniPlay("L");
                            break;
                    }
                    break;
                case "Boss2":
                    switch (PlayerController._player.face)
                    {
                        case Face.Right:
                            CapturedByBoss2AniPlay("R");
                            break;
                        case Face.Left:
                            CapturedByBoss2AniPlay("L");
                            break;
                    }
                    break;
            }
        }
        else
        {
            if (BeCapture)
            {
                switch (MonsterType)
                {
                    case "Boss1":
                        switch (PlayerController._player.face)
                        {
                            case Face.Right:
                                _transform.position = MonsterTransform.position + new Vector3(-5.91f, -2.06f, 0);
                                break;
                            case Face.Left:
                                _transform.position = MonsterTransform.position + new Vector3(5.91f, -2.06f, 0);
                                break;
                        }
                        CaptureByVBMonsterAnimation.SetActive(false);
                        break;
                    case "Boss2":
                        switch (PlayerController._player.face)
                        {
                            case Face.Right:
                                _transform.position = MonsterTransform.position + new Vector3(-3.4f, 0.1f, 0);
                                break;
                            case Face.Left:
                                _transform.position = MonsterTransform.position + new Vector3(3.4f, 0.1f, 0);
                                break;
                        }
                        CaptureByCaptainAnimation.SetActive(false);
                        break;
                }
                BeCapture = false;
            }
        }
    }

    public void CapturedByBoss1AniPlay(string Face)
    {
        CaptureByVBMonsterAnimation.SetActive(true);
        switch (Face)
        {
            case "R":
                CaptureByVBMonsterAnimation.transform.GetChild(0).localScale = new Vector3(0.28f, 0.28f, 0);
                break;
            case "L":
                CaptureByVBMonsterAnimation.transform.GetChild(0).localScale = new Vector3(-0.28f, 0.28f, 0);
                break;
        }
        _aniController.AbsoluteAniFalse();
    }

    public void CapturedByBoss2AniPlay(string Face)
    {
        CaptureByCaptainAnimation.SetActive(true);
        switch (Face)
        {
            case "R":
                CaptureByCaptainAnimation.transform.GetChild(1).localScale = new Vector3(0.28f, 0.28f, 0);
                break;
            case "L":
                CaptureByCaptainAnimation.transform.GetChild(1).localScale = new Vector3(-0.28f, 0.28f, 0);
                break;
        }
        _aniController.AbsoluteAniFalse();
    }

    public void InisializeCaptureAni(MonsterBasicData _basicData, MonsterCaptureController _captureController)
    {
        _monsterCaptureController = _captureController;
        MonsterType = _monsterCaptureController.MonsterType;
        MonsterTransform = _monsterCaptureController.transform;

        switch (_basicData.face)
        {
            case MonsterBasicData.Face.Right:
                switch (MonsterType)
                {
                    case "Boss1":
                        PlayerController._player.face = Face.Left;
                        break;
                    case "Boss2":
                        PlayerController._player.face = Face.Left;
                        break;
                }
                break;
            case MonsterBasicData.Face.Left:
                switch (MonsterType)
                {
                    case "Boss1":
                        PlayerController._player.face = Face.Right;
                        break;
                    case "Boss2":
                        PlayerController._player.face = Face.Right;
                        break;
                }
                break;
        }
    }
}
