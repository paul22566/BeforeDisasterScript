using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerCapturedAnimation : MonoBehaviour
{
    public Transform PlayerCapturedAni;

    private PlayerAnimationController _aniController;
    private BoxCollider2D _boxCollider;
    private string MonsterType;

    private GameObject CaptureByVBMonsterAnimation;
    private GameObject CaptureByCaptainAnimation;

    [HideInInspector] public MonsterCaptureController _monsterCaptureController;
    private Transform MonsterTransform;

    private bool BeCapture;
    // Start is called before the first frame update
    void Start()
    {
        _aniController = this.GetComponent<PlayerAnimationController>();
        _boxCollider = this.GetComponent<BoxCollider2D>();

        CaptureByVBMonsterAnimation = PlayerCapturedAni.GetChild(0).gameObject;
        CaptureByCaptainAnimation = PlayerCapturedAni.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (_monsterCaptureController != null && _monsterCaptureController.GetComponent<MonsterCaptureController>().isPlayerFollow)
        {
            BeCapture = true;
            MonsterType = _monsterCaptureController.MonsterType;
            MonsterTransform = _monsterCaptureController.transform;
            _boxCollider.isTrigger = true;

            switch (MonsterType)
            {
                case "Boss1":
                    if (MonsterTransform.position.x >= this.transform.position.x)
                    {
                        CapturedByBoss1AniPlay("R");
                    }
                    else
                    {
                        CapturedByBoss1AniPlay("L");
                    }
                    break;
                case "Boss2":
                    if (MonsterTransform.position.x >= this.transform.position.x)
                    {
                        CapturedByBoss2AniPlay("R");
                    }
                    else
                    {
                        CapturedByBoss2AniPlay("L");
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
                        if (MonsterTransform.position.x >= this.transform.position.x)
                        {
                            this.transform.position = MonsterTransform.position + new Vector3(-5.91f, -2.06f, 0);
                        }
                        else
                        {
                            this.transform.position = MonsterTransform.position + new Vector3(5.91f, -2.06f, 0);
                        }
                        CaptureByVBMonsterAnimation.SetActive(false);
                        break;
                    case "Boss2":
                        if (MonsterTransform.position.x >= this.transform.position.x)
                        {
                            this.transform.position = MonsterTransform.position + new Vector3(-3.4f, 0.1f, 0);
                        }
                        else
                        {
                            this.transform.position = MonsterTransform.position + new Vector3(3.4f, 0.1f, 0);
                        }
                        CaptureByCaptainAnimation.SetActive(false);
                        break;
                }
                _boxCollider.isTrigger = false;
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
                CaptureByCaptainAnimation.transform.GetChild(0).localScale = new Vector3(0.28f, 0.28f, 0);
                break;
            case "L":
                CaptureByCaptainAnimation.transform.GetChild(0).localScale = new Vector3(-0.28f, 0.28f, 0);
                break;
        }
        _aniController.AbsoluteAniFalse();
    }
}
