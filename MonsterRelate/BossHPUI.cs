using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHPUI : MonoBehaviour
{
    public GameObject Boss;
    private Image HpImage;
    private MonsterBasicData _basicData;
    private float HpMoveTimeSet = 0.5f;
    private float HpMoveTime;
    private float SpeedSet;
    private float Speed;
    private float SlowestSpeed = 1;
    private float NowHpUILength;
    private float TargetHpUILength;
    private float TargetFinalRecord;
    private float MoveLength;

    // Start is called before the first frame update
    void Start()
    {
        _basicData = Boss.GetComponent<MonsterBasicData>();
        HpImage = this.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        TargetHpUILength = (float)_basicData.hp / (float)_basicData.maxHp;
        NowHpUILength = TargetHpUILength;
        TargetFinalRecord = TargetHpUILength;
        HpMoveTime = HpMoveTimeSet;
    }

    // Update is called once per frame
    void Update()
    {
        if (Boss == null)
        {
            Destroy(this.gameObject);
        }

        TargetHpUILength = (float)_basicData.hp / (float)_basicData.maxHp;
        if (TargetFinalRecord != TargetHpUILength)
        {
            MoveStartSet();
        }
    }

    private void FixedUpdate()
    {
        if (NowHpUILength > TargetHpUILength)
        {
            HpMoveTime -= Time.fixedDeltaTime;
            if (HpMoveTime <= 0)
            {
                if (Speed > SlowestSpeed)
                {
                    Speed -= SpeedSet / 25;
                }
                else
                {
                    Speed = SlowestSpeed;
                }
            }
            NowHpUILength -= (Speed / 100) * Time.fixedDeltaTime;
        }
        if (NowHpUILength <= TargetHpUILength)
        {
            MoveEndSet();
        }
        HpImage.transform.localScale = new Vector3(NowHpUILength, HpImage.transform.localScale.y, HpImage.transform.localScale.z);
    }

    private void MoveStartSet()
    {
        MoveLength = Mathf.Abs(NowHpUILength - TargetHpUILength);
        SpeedSet = MoveLength * 0.75f * 100 / HpMoveTimeSet;
        Speed = SpeedSet;
        TargetFinalRecord = TargetHpUILength;
        HpMoveTime = HpMoveTimeSet;
    }

    private void MoveEndSet()
    {
        SpeedSet = 0;
        Speed = 0;
        NowHpUILength = TargetHpUILength;
        MoveLength = 0;
    }
}
