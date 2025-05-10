using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSE : MonoBehaviour
{
    public GameObject SEPlace;
    private PlayerController _playerController;
    private BattleSystem _battleSystem;

    //����
    public AudioClip WalkSoundNormal;//����(�w�])
    public AudioClip WalkSoundGrass;//����(��a)
    public AudioClip WalkSoundMetal;//����(����)
    public AudioClip SlowWalkSoundNormal;//����(�w�])
    public AudioClip SlowWalkSoundGrass;//����(��a)
    public AudioClip SlowWalkSoundMetal;//����(����)
    private bool walkSoundDataExist;
    public AudioClip TouchGroundSoundNormal;//���a(�w�])
    public AudioClip TouchGroundSoundGrass;//���a(��a)
    public AudioClip TouchGroundSoundMetal;//���a(����)
    public AudioClip DashSound;//�{��
    public AudioClip SecondJumpSound;//�G�q��
    public AudioClip ImpulseJumpSound;//�z���~���D
    //����
    public AudioClip AtkSound1;//���C(����)
    public AudioClip HeavyAtkSound;//�����C(����)
    public AudioClip CriticAtkBeginingSound;//�j�۩ޤM
    public AudioClip CriticAtkFlashSound;//�j�۰{��
    public AudioClip AccumulateSound;//�����n
    public AudioClip BlockSound;//�۬[�n
    public AudioClip BlockSuccessSound;//�۬[���\�n
    public AudioClip ThrowSound;//���Y
    public AudioClip SpecialThrowSound;//�S����Y(����A�]��)
    public AudioClip CocktailCriticAtkSound;//�U�N�~����
    //��L
    public AudioClip RestoreSound;//�^���n
    public AudioClip HurtedSound;//����
    public AudioClip LaserHurtedSound;//�Q�p�g��
    public AudioClip SharpenSound;//�i�M�n
    public AudioClip DieSound;//���`�n
    public AudioClip RitualSwordSound;//�ϥλ����M�n

    private PlayerSEData WalkSource = new PlayerSEData();
    private PlayerSEData TouchGroundSource = new PlayerSEData();
    private PlayerSEData DashSource = new PlayerSEData();//�{��
    private PlayerSEData SecondJumpSource = new PlayerSEData();//�G�q��
    private PlayerSEData ImpulseJumpSource = new PlayerSEData();//�z���~���D
    //����
    private PlayerSEData AtkSource = new PlayerSEData();//���C(����)
    private PlayerSEData HeavyAtkSource = new PlayerSEData();//�����C(����)
    private PlayerSEData CriticAtkBeginingSource = new PlayerSEData();//�j�۩ޤM
    private PlayerSEData CriticAtkFlashSource = new PlayerSEData();//�j�۰{��
    private PlayerSEData AccumulateSource = new PlayerSEData();//�����n
    private PlayerSEData BlockSource = new PlayerSEData();//�۬[�n
    private PlayerSEData BlockSuccessSource = new PlayerSEData();//�۬[���\�n
    private PlayerSEData ThrowSource = new PlayerSEData();//���Y
    private PlayerSEData SpecialThrowSource = new PlayerSEData();//�S����Y(����A�]��)
    private PlayerSEData CocktailCriticAtkSource = new PlayerSEData();//�U�N�~����
    //��L
    private PlayerSEData RestoreSource = new PlayerSEData();//�^���n
    private PlayerSEData HurtedSource = new PlayerSEData();//����
    private PlayerSEData SharpenSource = new PlayerSEData();//�i�M�n
    private PlayerSEData RitualSwordSource = new PlayerSEData();//�ϥλ����M�n

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
            if (WalkSource.Source.clip.name == "�}�B�n(���d)")
            {
                if(!_playerController.TouchNormalGround || _battleSystem.isShooting)
                {
                    ChangeWalkSound();
                }
            }
            if (WalkSource.Source.clip.name == "�}�B�n(��a)")
            {
                if (!_playerController.TouchGrassGround || _battleSystem.isShooting)
                {
                    ChangeWalkSound();
                }
            }
            if (WalkSource.Source.clip.name == "�}�B�n(����)")
            {
                if (!_playerController.TouchMetalGround || _battleSystem.isShooting)
                {
                    ChangeWalkSound();
                }
            }

            if (WalkSource.Source.clip.name == "�C�}�B�n(���d)")
            {
                if (!_playerController.TouchNormalGround || !_battleSystem.isShooting)
                {
                    ChangeWalkSound();
                }
            }
            if (WalkSource.Source.clip.name == "�C�}�B�n(��a)")
            {
                if (!_playerController.TouchGrassGround || !_battleSystem.isShooting)
                {
                    ChangeWalkSound();
                }
            }
            if (WalkSource.Source.clip.name == "�C�}�B�n(����)")
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
    }//playerController����

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
