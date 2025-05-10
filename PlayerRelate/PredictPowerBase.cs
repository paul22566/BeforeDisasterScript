using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredictPowerBase : MonoBehaviour
{
    public enum Face { Right, Left }
    public Face face;
    private Transform _transform;
    public GameObject AimPower;
    private Transform AimPowerTransform;
    private BattleSystem _battleSystem;
    private Transform PlayerTransform;
    private float NowScaleX;
    private float NowScaleY;
    private float NowRotation;

    private float UpLimit = 4.24f;
    private float DownLimit = 1.29f;
    private float RightLimit = 4f;
    private float LeftLimit = 1.01f;
    private float NowUpLimit;
    private float NowDownLimit;
    private float NowRightLimit;
    private float NowLeftLimit;
    private float LeftUpPointScaleX = 0.124f;
    private float LeftUpPointScaleY = 0.248f;
    private float RightUpPointScaleX = 0.528f;
    private float RightUpPointScaleY = 0.163f;
    private float LeftDownPointScaleX = 0.078f;
    private float LeftDownPointScaleY = 0.013f;
    private float RightDownPointScaleX = 0.33f;
    private float RightDownPointScaleY = 0.047f;

    private float NowProportionX;//��eX�b��� �ѥ��ӥk
    private float NowProportionY;//��eY�b��� �ѤU�ӤW
    private float NowUpProportionXScale;//�W�b�q����Xscale
    private float NowDownProportionXScale;//�U�b�q����Xscale
    private float NowUpProportionYScale;//�W�b�q����Yscale
    private float NowDownProportionYScale;//�U�b�q����Yscale
    private float BottomRotation = -20;

    private void Start()
    {
        _transform = this.transform;
        AimPowerTransform = AimPower.transform;
        if (GameObject.Find("player") != null)
        {
            PlayerTransform = GameObject.Find("player").transform;
            _battleSystem = GameObject.Find("player").GetComponent<BattleSystem>();
        }
    }
    // Update is called once per frame
    void Update()
    {
        _transform.localPosition = AimPowerTransform.localPosition;

        NowRightLimit = PlayerTransform.position.x + RightLimit;
        NowLeftLimit = PlayerTransform.position.x + LeftLimit;
        NowUpLimit = PlayerTransform.position.y + UpLimit;
        NowDownLimit = PlayerTransform.position.y + DownLimit;
        //��ڭp��
        NowProportionY = (_transform.position.y - NowDownLimit) / (NowUpLimit - NowDownLimit);
        switch (face)
        {
            case Face.Left:
                NowProportionX = (PlayerTransform.position.x - LeftLimit - _transform.position.x) / (NowRightLimit - NowLeftLimit);
                break;
            case Face.Right:
                NowProportionX = (_transform.position.x - NowLeftLimit) / (NowRightLimit - NowLeftLimit);
                break;
        }
        NowUpProportionXScale = LeftUpPointScaleX * (1 - NowProportionX) + RightUpPointScaleX * NowProportionX;
        NowDownProportionXScale = LeftDownPointScaleX * (1 - NowProportionX) + RightDownPointScaleX * NowProportionX;
        NowScaleX = NowDownProportionXScale * (1 - NowProportionY) + NowUpProportionXScale * NowProportionY;
        NowUpProportionYScale = LeftUpPointScaleY * (1 - NowProportionX) + RightUpPointScaleY * NowProportionX;
        NowDownProportionYScale = LeftDownPointScaleY * (1 - NowProportionX) + RightDownPointScaleY * NowProportionX;
        NowScaleY = NowDownProportionYScale * (1 - NowProportionY) + NowUpProportionYScale * NowProportionY;
        switch (face)
        {
            case Face.Left:
                NowRotation = BottomRotation * (1 - NowProportionY) * -1;
                break;
            case Face.Right:
                NowRotation = BottomRotation * (1 - NowProportionY);
                break;
        }

        _transform.localScale = new Vector3(NowScaleX, NowScaleY, 0);
        _transform.rotation = Quaternion.Euler(0, 0, NowRotation);

        if (!_battleSystem.isAim || PlayerController.isHurted || PlayerController.isDie)
        {
            this.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            _transform.localScale = new Vector3(0.241f, 0.083f, 0);
            _transform.rotation = Quaternion.Euler(0, 0, -14);
            this.gameObject.SetActive(false);
        }
    }
}
