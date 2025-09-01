using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBlockController : MonoBehaviour
{
    /*
    主腳本需修改項目:

    宣告變數並抓取此腳本、觸發方式 、動畫 、Status(決定方向、結束時動作、isBlock TimerMethod 放這裡) 、被中斷、怪物重疊時忽略條件

    用方法統合結束時動作 
    格擋分為 成功 不成功 被打不成功
    被格擋分為 沒被打 被普攻打 被重攻打

    宣告WeakData變數
    初始化WeakData變數 用方法初始化
    */
    private Transform _transform;

    private float BeBlockTimer;
    public float BeBlockTimerSet;
    private float WeakTimer;
    public float WeakTimerSet;
    [HideInInspector] public bool CanBeBlockAniAppear;
    private float BeBlockLagTimer;
    private float BeBlockLagTimerSet = 0.03f;
    private Rigidbody2D Rigid2D;
    private BoxCollider2D _boxCollider;
    [HideInInspector] public bool BeBlockUnHurtedEnd;//script(格檔怪物script)
    [HideInInspector] public bool BeBlockNormalAtkEnd;//script(格檔怪物script)
    [HideInInspector] public bool BeBlockCAtkEnd;//script(格檔怪物script)
    [HideInInspector] public bool BeBlockSuccess;//script (BlockJudgement)
    [HideInInspector] public bool isHurtedByCAtk;//專門用在被格檔
    [HideInInspector] public bool isHurtedByNormalAtk;//專門用在被格檔
    [HideInInspector] public bool isWeak;//script(monsterWeakjudgement)
    [HideInInspector] public bool isGetUp;
    [HideInInspector] public bool isWeakEndJudge;

    public bool ShouldBlockAniAppear;//是否有攻擊軌跡
    public float BlockSuccessTimerSet;
    private float BlockSuccessTimer;
    public float BlockAtkAppearTime;
    [HideInInspector] public bool isBlockSuucess;//script(blockJudgement)
    [HideInInspector] public bool isBlock;//script(blockJudgement)
    [HideInInspector] public bool CanBlockAniAppear;
    public float BlockTimerSet;
    private float BlockTimer;
    private float BlockLagTimer;
    private float BlockLagTimerSet = 0.03f;
    public GameObject RBlock;
    public GameObject LBlock;
    public GameObject RBlockAtkAni;
    public GameObject LBlockAtkAni;
    public GameObject RBlockAtk;
    public GameObject LBlockAtk;
    private bool AtkFirstAppear;
    public Transform AtkTemporaryArea;
    private MonsterHurtedController _hurtedController;
    [HideInInspector] public bool FaceRight;
    [HideInInspector] public bool FaceLeft;
    [HideInInspector] public bool BlockSuccessEnd;
    [HideInInspector] public bool BlockHurtedEnd;
    [HideInInspector] public bool BlockUnSuccessEnd;
    // Start is called before the first frame update
    void Start()
    {
        _transform = this.transform;
        AllVariableReset();
        Rigid2D = this.GetComponent<Rigidbody2D>();
        _boxCollider = this.GetComponent<BoxCollider2D>();
        _hurtedController = this.GetComponent<MonsterHurtedController>();
    }

    public void BeBlockTimerMethod(float _deltaTime)
    {
        if (BeBlockSuccess)
        {
            BeBlockTimer -= _deltaTime;
            BeBlockLagTimer -= _deltaTime;
            if (BeBlockLagTimer <= 0)
            {
                CanBeBlockAniAppear = true;
            }
            if (isHurtedByNormalAtk)
            {
                CanBeBlockAniAppear = false;
                isHurtedByNormalAtk = false;
                BeBlockSuccess = false;                
                BeBlockTimer = BeBlockTimerSet;
                BeBlockLagTimer = BeBlockLagTimerSet;
                BeBlockNormalAtkEnd = true;
                return;
            }
            if (isHurtedByCAtk)
            {
                CanBeBlockAniAppear = false;
                isHurtedByCAtk = false;
                BeBlockSuccess = false;
                BeBlockTimer = BeBlockTimerSet;
                BeBlockLagTimer = BeBlockLagTimerSet;
                BeBlockCAtkEnd = true;
                return;
            }
            if (BeBlockTimer <= 0)
            {
                CanBeBlockAniAppear = false;
                BeBlockSuccess = false;
                BeBlockUnHurtedEnd = true;
                BeBlockTimer = BeBlockTimerSet;
                BeBlockLagTimer = BeBlockLagTimerSet;
            }
        }
    }

    public void WeakTimerMethod(WeakData Data, float _deltaTime)
    {
        if (isWeak)
        {
            WeakTimer -= _deltaTime;
            Rigid2D.mass = Data.WeakMass;
            _boxCollider.offset = new Vector2(Data.WeakOffsetX, Data.WeakOffsetY);
            _boxCollider.size = new Vector2(Data.WeakSizeX, Data.WeakSizeY);
            if (WeakTimer <= 0)
            {
                isWeakEndJudge = true;
                isWeak = false;
                _boxCollider.offset = new Vector2(Data.EndOffsetX, Data.EndOffsetY);
                _boxCollider.size = new Vector2(Data.EndSizeX, Data.EndSizeY);
                Rigid2D.mass = Data.EndMass;
                WeakTimer = WeakTimerSet;
            }
        }      
    }

    public void WeakTimerMethod(WeakData Data, float GetUpTime, float _deltaTime)
    {
        if (isWeak)
        {
            WeakTimer -= _deltaTime;
            Rigid2D.mass = Data.WeakMass;
            _boxCollider.offset = new Vector2(Data.WeakOffsetX, Data.WeakOffsetY);
            _boxCollider.size = new Vector2(Data.WeakSizeX, Data.WeakSizeY);
            if (WeakTimer <= (WeakTimerSet - GetUpTime))
            {
                isGetUp = true;
                if (WeakTimer <= (WeakTimerSet - GetUpTime - 1))
                {
                    isWeakEndJudge = true;
                    isWeak = false;
                    isGetUp = false;
                    _boxCollider.offset = new Vector2(Data.EndOffsetX, Data.EndOffsetY);
                    _boxCollider.size = new Vector2(Data.EndSizeX, Data.EndSizeY);
                    Rigid2D.mass = Data.EndMass;
                    WeakTimer = WeakTimerSet;
                }
            }
        }
    }

    public void BlockTimerMethod(float _deltaTime)
    {
        if (BlockTimer <= 0 && !isBlock)
        {
            isBlock = true;
            BlockTimer = BlockTimerSet;
        }

        if (_hurtedController.isHurted)
        {
            BlockHurtedEnd = true;
            AtkFirstAppear = false;
            BlockTimer = 0;
            return;
        }
        if (isBlockSuucess)
        {
            AtkFirstAppear = false;
            BlockTimer = 0;
            return;
        }
        BlockTimer -= _deltaTime;
        if (BlockTimer <= (BlockTimerSet - 0.1))
        {
            if (!AtkFirstAppear)
            {
                if (FaceRight)
                {
                    Instantiate(RBlock, _transform.position, Quaternion.identity, AtkTemporaryArea);
                    AtkTemporaryArea.DetachChildren();
                }
                if (FaceLeft)
                {
                    Instantiate(LBlock, _transform.position, Quaternion.identity, AtkTemporaryArea);
                    AtkTemporaryArea.DetachChildren();
                }
                AtkFirstAppear = true;
            }
            if (BlockTimer <= 0)
            {
                BlockUnSuccessEnd = true;
                AtkFirstAppear = false;
            }
        }
    }

    public void BlockSuccessTimerMethod(float _deltaTime)
    {
        if (isBlockSuucess)
        {
            BlockLagTimer -= _deltaTime;
            BlockSuccessTimer -= Time.unscaledDeltaTime * BackgroundSystem.BasicGameSpeed;
            if (BlockLagTimer <= 0)
            {
                CanBlockAniAppear = true;
            }
            if (BlockSuccessTimer <= (BlockSuccessTimerSet - 0.5))
            {
                BackgroundSystem.CantPause = false;
                BackgroundSystem.GameSpeed = 1;
                if (BlockSuccessTimer <= (BlockSuccessTimerSet - BlockAtkAppearTime))
                {
                    if (!AtkFirstAppear)
                    {
                        if (FaceRight)
                        {
                            Instantiate(RBlockAtk, _transform.position, Quaternion.identity, AtkTemporaryArea);
                            AtkTemporaryArea.DetachChildren();
                        }
                        if (FaceLeft)
                        {
                            Instantiate(LBlockAtk, _transform.position, Quaternion.identity, AtkTemporaryArea);
                            AtkTemporaryArea.DetachChildren();
                        }
                        AtkFirstAppear = true;
                    }
                    if (BlockSuccessTimer <= 0)
                    {
                        CanBlockAniAppear = false;
                        BlockSuccessEnd = true;
                        isBlockSuucess = false;
                        AtkFirstAppear = false;
                        BlockLagTimer = BlockLagTimerSet;
                        BlockSuccessTimer = BlockSuccessTimerSet;
                    }
                }
            }
        }
    }

    public void AllVariableReset()
    {
        isHurtedByNormalAtk = false;
        isHurtedByCAtk = false;
        BeBlockSuccess = false;
        BeBlockUnHurtedEnd = false;
        isWeakEndJudge = false;
        isWeak = false;
        isGetUp = false;
        isBlock = false;
        BlockHurtedEnd = false;
        AtkFirstAppear = false;
        BlockUnSuccessEnd = false;
        CanBlockAniAppear = false;
        BlockSuccessEnd = false;
        isBlockSuucess = false;
        BeBlockTimer = BeBlockTimerSet;
        BeBlockLagTimer = BeBlockLagTimerSet;
        WeakTimer = WeakTimerSet;
        BlockTimer = 0;
        BlockLagTimer = BlockLagTimerSet;
        BlockSuccessTimer = BlockSuccessTimerSet;
    }

    public void BlockVariableReset()
    {
        isHurtedByNormalAtk = false;
        isHurtedByCAtk = false;
        BeBlockSuccess = false;
        BeBlockUnHurtedEnd = false;
        isBlock = false;
        BlockHurtedEnd = false;
        AtkFirstAppear = false;
        BlockUnSuccessEnd = false;
        CanBlockAniAppear = false;
        BlockSuccessEnd = false;
        isBlockSuucess = false;
        BeBlockTimer = BeBlockTimerSet;
        BeBlockLagTimer = BeBlockLagTimerSet;
        BlockTimer = 0;
        BlockLagTimer = BlockLagTimerSet;
        BlockSuccessTimer = BlockSuccessTimerSet;
    }

    public void WeakVariableReset()
    {
        isWeakEndJudge = false;
        isWeak = false;
        isGetUp = false;
        WeakTimer = WeakTimerSet;
    }
}

public struct WeakData
{
    public int WeakMass;
    public int EndMass;
    public float WeakOffsetX;
    public float WeakOffsetY;
    public float WeakSizeX;
    public float WeakSizeY;
    public float EndOffsetX;
    public float EndOffsetY;
    public float EndSizeX;
    public float EndSizeY;
}
