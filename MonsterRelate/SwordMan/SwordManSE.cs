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

    public AudioClip AtkSound1;//����1
    public AudioClip AtkSound2;//����2
    public AudioClip DashingSound;//�Ĩ��i��
    public AudioClip WalkSound;//����
    public AudioClip SlowWalkSound;//����
    public AudioClip BlockSound;//����
    public GameObject BlockSuccessSound;//���ɦ��\

    private AudioSource AtkSource1;//����1
    private AudioSource AtkSource2;//����2
    private AudioSource DashingSource;//�Ĩ��i��
    private AudioSource WalkSource;//����
    private AudioSource BlockSource;//����

    private float AtkValidDistance1 = 15;//����1
    private float AtkValidDistance2 = 15;//����2
    private float DashingValidDistance = 15;//�Ĩ��i��
    private float WalkValidDistance = 20;//����
    private float BlockValidDistance = 15;//����

    private float AtkSound1Time = 0.22f;
    private float AtkSound2Time = 0.5f;
    private float DashingSoundTime = 0.35f;
    private float BlockSoundTime = 0;
    [HideInInspector] public bool SEAppear;
    [HideInInspector] public bool SE2Appear;
    [HideInInspector] public bool SE3Appear;
    private bool isSEPause;

    //---------------------------------------------
    //�p����
    public AudioClip TakeSwordSound;//�|�C�n
    public AudioClip StringSound;//�p�����Ĩ�
    public AudioClip StringAtk2Sound;//�p�����Ĩ����
    public AudioClip FallAtkSound;//�p�����Y������
    public AudioClip FallGroundSound;//�p�������a�n
    private AudioSource TakeSwordSource;//�|�C�n
    private AudioSource StringSource;//�p�����Ĩ�
    private AudioSource StringAtk2Source;//�p�����Ĩ����
    private AudioSource FallAtkSource;//�p�����Y������
    private AudioSource FallGroundSource;//�p�������a�n
    private float TakeSwordValidDistance = 30;//�|�C�n
    private float StringValidDistance = 15;//�p�����Ĩ�
    private float StringAtk2ValidDistance = 15;//�p�����Ĩ����
    private float FallAtkValidDistance = 30;//�p�����Y������
    private float FallGroundValidDistance = 15;//�p�������a�n

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

        //���ļȰ�
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
