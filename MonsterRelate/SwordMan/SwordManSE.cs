using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordManSE : MonoBehaviour
{
    private GameObject SEPlace;
    private MonsterBasicData _basicData;
    [SerializeField] private int SEPlaceNumber;
    private enum MonsterType { SwordMan, SmallCaptain, WeakSmallCaptain};
    [SerializeField] private MonsterType _monsterType;

    public AudioClip AtkSound1;//普攻1
    public AudioClip AtkSound2;//普攻2
    public AudioClip DashingSound;//衝刺行進中
    public AudioClip WalkSound;//走路
    public AudioClip SlowWalkSound;//走路
    public AudioClip BlockSound;//格檔
    public GameObject BlockSuccessSound;//格檔成功

    private AudioSource AtkSource1;//普攻1
    private AudioSource AtkSource2;//普攻2
    private AudioSource DashingSource;//衝刺行進中
    private AudioSource WalkSource;//走路
    private AudioSource BlockSource;//格檔

    private float AtkValidDistance1 = 15;//普攻1
    private float AtkValidDistance2 = 15;//普攻2
    private float DashingValidDistance = 15;//衝刺行進中
    private float WalkValidDistance = 20;//走路
    private float BlockValidDistance = 15;//格檔

    private float AtkSound1Time = 0.22f;
    private float AtkSound2Time = 0.5f;
    private float DashingSoundTime = 0.35f;
    private float BlockSoundTime = 0;
    [HideInInspector] public bool SEAppear;
    [HideInInspector] public bool SE2Appear;
    [HideInInspector] public bool SE3Appear;
    private bool isSEPause;

    //---------------------------------------------
    //小隊長
    public AudioClip TakeSwordSound;//舉劍聲
    public AudioClip StringSound;//小隊長衝刺
    public AudioClip StringAtk2Sound;//小隊長衝刺攻擊
    public AudioClip FallAtkSound;//小隊長墜落攻擊
    public AudioClip FallGroundSound;//小隊長落地聲
    private AudioSource TakeSwordSource;//舉劍聲
    private AudioSource StringSource;//小隊長衝刺
    private AudioSource StringAtk2Source;//小隊長衝刺攻擊
    private AudioSource FallAtkSource;//小隊長墜落攻擊
    private AudioSource FallGroundSource;//小隊長落地聲
    private float TakeSwordValidDistance = 30;//舉劍聲
    private float StringValidDistance = 15;//小隊長衝刺
    private float StringAtk2ValidDistance = 15;//小隊長衝刺攻擊
    private float FallAtkValidDistance = 30;//小隊長墜落攻擊
    private float FallGroundValidDistance = 15;//小隊長落地聲

    private float TakeSwordTime = 0.5f;
    private float StringTime = 0.6f;
    private float StringAtk2Time = 0.55f;
    private float FallAtkTime = 0f;
    private float FallGroundTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        SEPlace = this.transform.GetChild(SEPlaceNumber).gameObject;
        _basicData = this.GetComponent<MonsterBasicData>();

        SEController.inisializeAudioSource(ref AtkSource1, AtkSound1, SEPlace.transform);
        SEController.inisializeAudioSource(ref AtkSource2, AtkSound2, SEPlace.transform);
        SEController.inisializeAudioSource(ref WalkSource, WalkSound, SEPlace.transform);
        SEController.inisializeAudioSource(ref BlockSource, BlockSound, SEPlace.transform);

        WalkSource.loop = true;

        switch (_monsterType)
        {
            case MonsterType.SwordMan:
                SEController.inisializeAudioSource(ref DashingSource, DashingSound, SEPlace.transform);
                break;
            case MonsterType.SmallCaptain:
                SEController.inisializeAudioSource(ref TakeSwordSource, TakeSwordSound, SEPlace.transform);
                SEController.inisializeAudioSource(ref StringSource, StringSound, SEPlace.transform);
                SEController.inisializeAudioSource(ref StringAtk2Source, StringAtk2Sound, SEPlace.transform);
                SEController.inisializeAudioSource(ref FallAtkSource, FallAtkSound, SEPlace.transform);
                SEController.inisializeAudioSource(ref FallGroundSource, FallGroundSound, SEPlace.transform);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (AtkSource1.isPlaying)
        {
            AtkSource1.volume = SEController.FOVCalculate(_basicData.AbsDistanceX, AtkValidDistance1);
        }
        if (AtkSource2.isPlaying)
        {
            AtkSource2.volume = SEController.FOVCalculate(_basicData.AbsDistanceX, AtkValidDistance2);
        }
        if (WalkSource.isPlaying)
        {
            WalkSource.volume = SEController.FOVCalculate(_basicData.AbsDistanceX, WalkValidDistance);
        }
        if (BlockSource.isPlaying)
        {
            BlockSource.volume = SEController.FOVCalculate(_basicData.AbsDistanceX, BlockValidDistance);
        }

        switch (_monsterType)
        {
            case MonsterType.SwordMan:
                if (DashingSource.isPlaying)
                {
                    DashingSource.volume = SEController.FOVCalculate(_basicData.AbsDistanceX, DashingValidDistance);
                }
                break;
            case MonsterType.SmallCaptain:
                if (TakeSwordSource.isPlaying)
                {
                    TakeSwordSource.volume = SEController.FOVCalculate(_basicData.AbsDistanceX, TakeSwordValidDistance);
                }
                if (StringSource.isPlaying)
                {
                    StringSource.volume = SEController.FOVCalculate(_basicData.AbsDistanceX, StringValidDistance);
                }
                if (StringAtk2Source.isPlaying)
                {
                    StringAtk2Source.volume = SEController.FOVCalculate(_basicData.AbsDistanceX, StringAtk2ValidDistance);
                }
                if (FallAtkSource.isPlaying)
                {
                    FallAtkSource.volume = SEController.FOVCalculate(_basicData.AbsDistanceX, FallAtkValidDistance);
                }
                if (FallGroundSource.isPlaying)
                {
                    FallGroundSource.volume = SEController.FOVCalculate(_basicData.AbsDistanceX, FallGroundValidDistance);
                }
                break;
        }

        //音效暫停
        if (PauseMenuController.isPauseMenuOpen && !isSEPause)
        {
            PauseAllSE();
            isSEPause = true;
        }
        if (!PauseMenuController.isPauseMenuOpen && isSEPause)
        {
            UnPauseAllSE();
            isSEPause = false;
        }
    }

    public void AtkSound1Play()
    {
        AtkSource1.PlayDelayed(AtkSound1Time);
    }

    public void AtkSound2Play()
    {
        AtkSource2.PlayDelayed(AtkSound2Time);
    }

    public void DashingSoundPlay()
    {
        DashingSource.PlayDelayed(DashingSoundTime);
    }

    public void BlockSoundPlay()
    {
        BlockSource.PlayDelayed(BlockSoundTime);
    }

    public void BlockSuccessSoundPlay()
    {
        Instantiate(BlockSuccessSound);
    }

    public void WalkSoundPlay()
    {
        if (_basicData.AbsDistanceX <= WalkValidDistance)
        {
            if (!WalkSource.isPlaying || WalkSource.clip != WalkSound)
            {
                WalkSource.clip = WalkSound;
                WalkSource.Play();
            }
        }
    }

    public void SlowWalkSoundPlay()
    {
        if (_basicData.AbsDistanceX <= WalkValidDistance)
        {
            if (!WalkSource.isPlaying || WalkSource.clip != SlowWalkSound)
            {
                WalkSource.clip = SlowWalkSound;
                WalkSource.Play();
            }
        }
    }

    public void TurnOffWalkSound()
    {
        if (WalkSource.isPlaying)
        {
            WalkSource.Stop();
        }
    }

    public void TakeSwordSoundPlay()
    {
        TakeSwordSource.PlayDelayed(TakeSwordTime);
    }

    public void StringSoundPlay()
    {
        StringSource.PlayDelayed(StringTime);
    }

    public void StringAtk2SoundPlay()
    {
        StringAtk2Source.PlayDelayed(StringAtk2Time);
    }

    public void FallAtkSoundPlay()
    {
        FallAtkSource.PlayDelayed(FallAtkTime);
    }

    public void FallGroundSoundPlay()
    {
        FallGroundSource.PlayDelayed(FallGroundTime);
    }

    public void BoolReset()
    {
        SEAppear = false;
        SE2Appear = false;
        SE3Appear = false;
    }

    public void StopAllSE()
    {
        AtkSource1.Stop();
        AtkSource2.Stop();
        WalkSource.Stop();
        BlockSource.Stop();

        switch (_monsterType)
        {
            case MonsterType.SwordMan:
                DashingSource.Stop();
                break;
            case MonsterType.SmallCaptain:
                TakeSwordSource.Stop();
                StringSource.Stop();
                StringAtk2Source.Stop();
                FallAtkSource.Stop();
                FallGroundSource.Stop();
                break;
        }
    }

    public void PauseAllSE()
    {
        AtkSource1.Pause();
        AtkSource2.Pause();
        WalkSource.Pause();
        BlockSource.Pause();

        switch (_monsterType)
        {
            case MonsterType.SwordMan:
                DashingSource.Pause();
                break;
            case MonsterType.SmallCaptain:
                TakeSwordSource.Pause();
                StringSource.Pause();
                StringAtk2Source.Pause();
                FallAtkSource.Pause();
                FallGroundSource.Pause();
                break;
        }
    }

    public void UnPauseAllSE()
    {
        AtkSource1.UnPause();
        AtkSource2.UnPause();
        WalkSource.UnPause();
        BlockSource.UnPause();

        switch (_monsterType)
        {
            case MonsterType.SwordMan:
                DashingSource.UnPause();
                break;
            case MonsterType.SmallCaptain:
                TakeSwordSource.UnPause();
                StringSource.UnPause();
                StringAtk2Source.UnPause();
                FallAtkSource.UnPause();
                FallGroundSource.UnPause();
                break;
        }
    }
}
