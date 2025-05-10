using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSE : MonoBehaviour
{
    public GameObject SEPlace;
    private PlayerController _playerController;
    private BattleSystem _battleSystem;

    //移動
    public AudioClip WalkSoundNormal;//走路(預設)
    public AudioClip WalkSoundGrass;//走路(草地)
    public AudioClip WalkSoundMetal;//走路(金屬)
    public AudioClip SlowWalkSoundNormal;//走路(預設)
    public AudioClip SlowWalkSoundGrass;//走路(草地)
    public AudioClip SlowWalkSoundMetal;//走路(金屬)
    private bool walkSoundDataExist;
    public AudioClip TouchGroundSoundNormal;//落地(預設)
    public AudioClip TouchGroundSoundGrass;//落地(草地)
    public AudioClip TouchGroundSoundMetal;//落地(金屬)
    public AudioClip DashSound;//閃避
    public AudioClip SecondJumpSound;//二段跳
    public AudioClip ImpulseJumpSound;//爆裂瓶跳躍
    //攻擊
    public AudioClip AtkSound1;//揮劍(揮空)
    public AudioClip HeavyAtkSound;//重揮劍(揮空)
    public AudioClip CriticAtkBeginingSound;//大招拔刀
    public AudioClip CriticAtkFlashSound;//大招閃光
    public AudioClip AccumulateSound;//集氣聲
    public AudioClip BlockSound;//招架聲
    public AudioClip BlockSuccessSound;//招架成功聲
    public AudioClip ThrowSound;//投擲
    public AudioClip SpecialThrowSound;//特殊投擲(跳投，跑投)
    public AudioClip CocktailCriticAtkSound;//燃燒瓶炸裂
    //其他
    public AudioClip RestoreSound;//回血聲
    public AudioClip HurtedSound;//受擊
    public AudioClip LaserHurtedSound;//被雷射打
    public AudioClip SharpenSound;//磨刀聲
    public AudioClip DieSound;//死亡聲
    public AudioClip RitualSwordSound;//使用儀式刀聲

    private PlayerSEData WalkSource = new PlayerSEData();
    private PlayerSEData TouchGroundSource = new PlayerSEData();
    private PlayerSEData DashSource = new PlayerSEData();//閃避
    private PlayerSEData SecondJumpSource = new PlayerSEData();//二段跳
    private PlayerSEData ImpulseJumpSource = new PlayerSEData();//爆裂瓶跳躍
    //攻擊
    private PlayerSEData AtkSource = new PlayerSEData();//揮劍(揮空)
    private PlayerSEData HeavyAtkSource = new PlayerSEData();//重揮劍(揮空)
    private PlayerSEData CriticAtkBeginingSource = new PlayerSEData();//大招拔刀
    private PlayerSEData CriticAtkFlashSource = new PlayerSEData();//大招閃光
    private PlayerSEData AccumulateSource = new PlayerSEData();//集氣聲
    private PlayerSEData BlockSource = new PlayerSEData();//招架聲
    private PlayerSEData BlockSuccessSource = new PlayerSEData();//招架成功聲
    private PlayerSEData ThrowSource = new PlayerSEData();//投擲
    private PlayerSEData SpecialThrowSource = new PlayerSEData();//特殊投擲(跳投，跑投)
    private PlayerSEData CocktailCriticAtkSource = new PlayerSEData();//燃燒瓶炸裂
    //其他
    private PlayerSEData RestoreSource = new PlayerSEData();//回血聲
    private PlayerSEData HurtedSource = new PlayerSEData();//受擊
    private PlayerSEData SharpenSource = new PlayerSEData();//磨刀聲
    private PlayerSEData RitualSwordSource = new PlayerSEData();//使用儀式刀聲

    private void Start()
    {
        _playerController = this.transform.parent.GetComponent<PlayerController>();
        _battleSystem = this.transform.parent.GetComponent<BattleSystem>();

        InitializePlayerSEData(WalkSource, 0.5f);
        InitializePlayerSEData(TouchGroundSource, 0.5f);
        InitializePlayerSEData(DashSource, DashSound, 0.5f);
        InitializePlayerSEData(SecondJumpSource, SecondJumpSound, 0.3f);
        InitializePlayerSEData(ImpulseJumpSource, ImpulseJumpSound, 0.8f);
        InitializePlayerSEData(AtkSource, AtkSound1, 0.6f);
        InitializePlayerSEData(HeavyAtkSource, HeavyAtkSound, 0.6f);
        InitializePlayerSEData(CriticAtkBeginingSource, CriticAtkBeginingSound, 1f);
        InitializePlayerSEData(CriticAtkFlashSource, CriticAtkFlashSound, 1f);
        InitializePlayerSEData(AccumulateSource, AccumulateSound, 0.7f);
        InitializePlayerSEData(BlockSource, BlockSound, 0.6f);
        InitializePlayerSEData(BlockSuccessSource, BlockSuccessSound, 0.8f);
        InitializePlayerSEData(ThrowSource, ThrowSound, 0.6f);
        InitializePlayerSEData(SpecialThrowSource, SpecialThrowSound, 0.6f);
        InitializePlayerSEData(CocktailCriticAtkSource, CocktailCriticAtkSound, 0.8f);
        InitializePlayerSEData(RestoreSource, RestoreSound, 0.3f);
        InitializePlayerSEData(HurtedSource, HurtedSound, 0.4f);
        InitializePlayerSEData(SharpenSource, SharpenSound, 0.7f);
        InitializePlayerSEData(RitualSwordSource, RitualSwordSound, 0.8f);

        WalkSource.Source.loop = true;
        AccumulateSource.Source.loop = true;
    }

    private void Update()
    {
        SEController.DetectPlayerSE();
    }

    public void AtkSound1Play()
    {
        SEController.PlayPlayerSE(AtkSource);
    }

    public void HeavyAtkSoundPlay()
    {
        SEController.PlayPlayerSE(HeavyAtkSource);
    }

    public void WalkSoundPlay()
    {
        if (!walkSoundDataExist)
        {
            if (_playerController.TouchNormalGround)
            {
                walkSoundDataExist = true;
                if (!_battleSystem.isShooting)
                {
                    WalkSource.Source.clip = WalkSoundNormal;
                }
                else
                {
                    WalkSource.Source.clip = SlowWalkSoundNormal;
                }
            }
            if (_playerController.TouchGrassGround)
            {
                walkSoundDataExist = true;
                if (!_battleSystem.isShooting)
                {
                    WalkSource.Source.clip = WalkSoundGrass;
                }
                else
                {
                    WalkSource.Source.clip = SlowWalkSoundGrass;
                }
            }
            if (_playerController.TouchMetalGround)
            {
                walkSoundDataExist = true;
                if (!_battleSystem.isShooting)
                {
                    WalkSource.Source.clip = WalkSoundMetal;
                }
                else
                {
                    WalkSource.Source.clip = SlowWalkSoundMetal;
                }
            }
            if (walkSoundDataExist)
            {
                SEController.PlayPlayerSE(WalkSource);
            }
        }
        if (walkSoundDataExist)
        {
            if (WalkSource.Source.clip.name == "腳步聲(水泥)")
            {
                if(!_playerController.TouchNormalGround || _battleSystem.isShooting)
                {
                    ChangeWalkSound();
                }
            }
            if (WalkSource.Source.clip.name == "腳步聲(草地)")
            {
                if (!_playerController.TouchGrassGround || _battleSystem.isShooting)
                {
                    ChangeWalkSound();
                }
            }
            if (WalkSource.Source.clip.name == "腳步聲(金屬)")
            {
                if (!_playerController.TouchMetalGround || _battleSystem.isShooting)
                {
                    ChangeWalkSound();
                }
            }

            if (WalkSource.Source.clip.name == "慢腳步聲(水泥)")
            {
                if (!_playerController.TouchNormalGround || !_battleSystem.isShooting)
                {
                    ChangeWalkSound();
                }
            }
            if (WalkSource.Source.clip.name == "慢腳步聲(草地)")
            {
                if (!_playerController.TouchGrassGround || !_battleSystem.isShooting)
                {
                    ChangeWalkSound();
                }
            }
            if (WalkSource.Source.clip.name == "慢腳步聲(金屬)")
            {
                if (!_playerController.TouchMetalGround || !_battleSystem.isShooting)
                {
                    ChangeWalkSound();
                }
            }

            if (!_playerController.TouchNormalGround && !_playerController.TouchGrassGround && !_playerController.TouchMetalGround)
            {
                TurnOffWalkSound();
            }
        }
    }

    public void TouchGroundSoundPlay()
    {
        bool ShouldPlaySound = false; 

        if (_playerController.TouchNormalGround)
        {
            ShouldPlaySound = true;
            TouchGroundSource.Source.clip = TouchGroundSoundNormal;
        }
        if (_playerController.TouchGrassGround)
        {
            ShouldPlaySound = true;
            TouchGroundSource.Source.clip = TouchGroundSoundGrass;
        }
        if (_playerController.TouchMetalGround)
        {
            ShouldPlaySound = true;
            TouchGroundSource.Source.clip = TouchGroundSoundMetal;
        }
        if (ShouldPlaySound)
        {
            SEController.PlayPlayerSE(TouchGroundSource);
        }
    }

    public void SecondJumpSoundPlay()
    {
        SEController.PlayPlayerSE(SecondJumpSource);
    }

    public void DashSoundPlay()
    {
        SEController.PlayPlayerSE(DashSource);
    }

    public void AccumulateSoundPlay()
    {
        if (!AccumulateSource.Source.isPlaying)
        {
            SEController.PlayPlayerSE(AccumulateSource);
        }
    }

    public void CriticAtkBeginingSoundPlay()
    {
        SEController.PlayPlayerSE(CriticAtkBeginingSource);
    }

    public void CriticAtkFlashSoundPlay()
    {
        SEController.PlayPlayerSE(CriticAtkFlashSource);
    }

    public void ThrowSoundPlay()
    {
        SEController.PlayPlayerSE(ThrowSource);
    }

    public void BlockSoundPlay()
    {
        SEController.PlayPlayerSE(BlockSource);
    }

    public void BlockSuccessSoundPlay()
    {
        SEController.PlayPlayerSE(BlockSuccessSource);
    }

    public void SpecialThrowSoundPlay()
    {
        SEController.PlayPlayerSE(SpecialThrowSource);
    }

    public void RestoreSoundPlay()
    {
        SEController.PlayPlayerSE(RestoreSource);
    }

    public void ImpulseJumpSoundPlay()
    {
        SEController.PlayPlayerSE(ImpulseJumpSource);
    }

    public void CocktailCriticAtkSoundPlay()
    {
        SEController.PlayPlayerSE(CocktailCriticAtkSource);
    }

    public void SharpenSoundPlay()
    {
        SEController.PlayPlayerSE(SharpenSource);
    }

    public void SharpenSoundStop()
    {
        if (SharpenSource.Source.isPlaying)
        {
            SharpenSource.Source.Stop();
        }
    }

    public void RitualSwordSoundPlay()
    {
        SEController.PlayPlayerSE(RitualSwordSource);
    }

    public void HurtedSoundPlay(NormalMonsterAtk.AtkType _type)
    {
        switch (_type)
        {
            case NormalMonsterAtk.AtkType.Normal:
                HurtedSource.Source.clip = HurtedSound;
                break;
            case NormalMonsterAtk.AtkType.Laser:
                HurtedSource.Source.clip = LaserHurtedSound;
                break;
        }
        SEController.PlayPlayerSE(HurtedSource);
    }//playerController控制

    public void TurnOffWalkSound()
    {
        if(WalkSource.Source.isPlaying)
        {
            walkSoundDataExist = false;
            WalkSource.Source.Stop();
        }
    }

    public void TurnOffAccumulateSound()
    {
        if (AccumulateSource.Source.isPlaying)
        {
            AccumulateSource.Source.Stop();
        }
    }

    private void ChangeWalkSound()
    {
        TurnOffWalkSound();
        WalkSoundPlay();
    }

    private void InitializePlayerSEData(PlayerSEData _playerSEData, float Weight)
    {
        _playerSEData.Source = SEPlace.AddComponent<AudioSource>();
        _playerSEData.Source.volume = SEController.SEVolume;
        _playerSEData.Weight = Weight;
    }

    private void InitializePlayerSEData(PlayerSEData _playerSEData, AudioClip Clip, float Weight)
    {
        _playerSEData.Source = SEPlace.AddComponent<AudioSource>();
        _playerSEData.Source.volume = SEController.SEVolume;
        _playerSEData.Source.clip = Clip;
        _playerSEData.Weight = Weight;
    }

    public void TurnOffLoopSE()
    {
        TurnOffAccumulateSound();
        TurnOffWalkSound();
    }
}

public class PlayerSEData
{
    public AudioSource Source;
    public float Weight;
    public bool isPlaying = false;
    public float TargetVolume;
}
