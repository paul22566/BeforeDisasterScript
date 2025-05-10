using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VBMonsterSE : MonoBehaviour
{
    private GameObject SEPlace;
    private MonsterBasicData _basicData;
    [SerializeField] private int SEPlaceNumber;
    private float _deltaTime;
    private enum MonsterType { Boss1, HallGuardian };
    [SerializeField] private MonsterType _monsterType;

    public AudioClip AtkSound;//揮爪
    public AudioClip Atk2Sound;//嘴砲聲
    public AudioClip PrepareAtkSound;//進攻低吼
    public AudioClip TouchGroundSound;//落地
    public AudioClip WalkSound;//腳步

    private AudioSource AtkSource;//揮爪
    private AudioSource Atk2Source;//嘴砲聲
    private AudioSource PrepareAtkSource;//進攻低吼
    private AudioSource TouchGroundSource;//落地
    private AudioSource WalkSource;//腳步

    private float AtkValidDistance = 30;
    private float Atk2ValidDistance = 100;
    private float PrepareAtkValidDistance = 30;
    private float TouchGroundValidDistance = 30;
    private float WalkValidDistance = 30;

    private float AtkSoundTime = 0.75f;
    private float Atk2SoundTime = 0.65f;
    private float PrepareAtkSoundTime = 0f;
    private float TouchGroundSoundTime = 0f;
    private float WalkSoundTime = 0.75f;

    private float Atk2loopSoundRate = 0;
    private float Atk2loopSoundChangeSpeed = 0.2f;
    private bool TurningOnAtk2loopSound;
    private bool TurningOffAtk2loopSound;
    private float DashSoundRate = 0;
    private float DashSoundChangeSpeed = 0.2f;
    private bool TurningOnDashSound;
    private bool TurningOffDashSound;

    [HideInInspector] public bool SEAppear;
    [HideInInspector] public bool SE2Appear;
    [HideInInspector] public bool SE3Appear;
    [HideInInspector] public bool SE4Appear;
    private bool isSEPause;

    //Boss1
    public AudioClip DashSound;//暴衝聲
    public AudioClip BackDashSound;//暴衝後撤聲
    public AudioClip Atk1_5Sound;//輕揮手
    public AudioClip Atk2BeginSound;//嘴砲持續開始聲
    public AudioClip Atk2loopSound;//嘴砲持續聲
    public AudioClip Atk4StepSound;//大嘴砲踱地
    public AudioClip CaptureAtk1Sound;//投技1
    public AudioClip CaptureAtk2Sound;//投技2
    public AudioClip CaptureAtk3Sound;//投技3
    public AudioClip FallDownSound;//倒地聲
    public AudioClip GetUpSound;//起身聲

    private AudioSource DashSource;//暴衝聲
    private AudioSource BackDashSource;//暴衝後撤聲
    private AudioSource Atk1_5Source;//輕揮手
    private AudioSource Atk2BeginSource;//嘴砲持續開始聲
    private AudioSource Atk2loopSource;//嘴砲持續聲
    private AudioSource Atk4StepSource;//大嘴砲踱地
    private AudioSource CaptureAtk1Source;//投技1
    private AudioSource CaptureAtk2Source;//投技2
    private AudioSource CaptureAtk3Source;//投技3
    private AudioSource FallDownSource;//倒地聲
    private AudioSource GetUpSource;//起身聲

    private float DashValidDistance = 30;
    private float BackDashValidDistance = 100;
    private float Atk1_5ValidDistance = 100;
    private float Atk2BeginValidDistance = 100;
    private float Atk2loopValidDistance = 100;
    private float Atk4StepValidDistance = 100;
    private float CaptureAtk1ValidDistance = 100;
    private float CaptureAtk2ValidDistance = 100;
    private float CaptureAtk3ValidDistance = 100;
    private float FallDownValidDistance = 100;
    private float GetUpValidDistance = 30;

    private float DashSoundTime = 0.8f;
    private float BackDashSoundTime = 0.2f;
    private float Atk1_5SoundTime = 0.5f;
    private float Atk2BeginSoundTime = 1.25f;
    private float Atk2loopSoundTime = 1.81f;
    private float Atk4StepSoundTime = 0f;
    private float CaptureAtk2Time = 1.3f;
    private float FallDownTime = 0.2f;
    private float GetUpTime = 0f;
    //門衛
    public AudioClip BackSound;//後撤
    public AudioClip BeginTouchSound;//beginAtk落地聲
    public AudioClip BeginAtkSound;//beginAtk
    public AudioClip BeginAtkWalkSound;//beginAtk腳步聲

    private AudioSource BackSource;//後撤
    private AudioSource BeginTouchSource;
    private AudioSource BeginAtkSource;
    private AudioSource BeginAtkWalkSource;

    private float BackValidDistance = 30;
    private float BeginTouchValidDistance = 30;
    private float BeginAtkValidDistance = 30;
    private float BeginAtkWalkValidDistance = 30;

    private float BackSoundTime = 0f;
    private float BackAtkSoundTime = 1.35f;
    private float BeginTouchTime = 0;
    private float BeginAtkTime = 2.3f;
    private float BeginAtkWalkTime = 3.77f;
    // Start is called before the first frame update
    void Start()
    {
        SEPlace = this.transform.GetChild(SEPlaceNumber).gameObject;
        _basicData = this.GetComponent<MonsterBasicData>();

        SEController.inisializeAudioSource(ref AtkSource, AtkSound, SEPlace.transform);
        SEController.inisializeAudioSource(ref Atk2Source, Atk2Sound, SEPlace.transform);
        SEController.inisializeAudioSource(ref PrepareAtkSource, PrepareAtkSound, SEPlace.transform);
        SEController.inisializeAudioSource(ref TouchGroundSource, TouchGroundSound, SEPlace.transform);
        SEController.inisializeAudioSource(ref WalkSource, WalkSound, SEPlace.transform);

        WalkSource.loop = true;

        switch (_monsterType)
        {
            case MonsterType.Boss1:
                SEController.inisializeAudioSource(ref DashSource, DashSound, SEPlace.transform);
                SEController.inisializeAudioSource(ref BackDashSource, BackDashSound, SEPlace.transform);
                SEController.inisializeAudioSource(ref Atk1_5Source, Atk1_5Sound, SEPlace.transform);
                SEController.inisializeAudioSource(ref Atk2BeginSource, Atk2BeginSound, SEPlace.transform);
                SEController.inisializeAudioSource(ref Atk2loopSource, Atk2loopSound, SEPlace.transform);
                SEController.inisializeAudioSource(ref Atk4StepSource, Atk4StepSound, SEPlace.transform);
                SEController.inisializeAudioSource(ref CaptureAtk1Source, CaptureAtk1Sound, SEPlace.transform);
                SEController.inisializeAudioSource(ref CaptureAtk2Source, CaptureAtk2Sound, SEPlace.transform);
                SEController.inisializeAudioSource(ref CaptureAtk3Source, CaptureAtk3Sound, SEPlace.transform);
                SEController.inisializeAudioSource(ref FallDownSource, FallDownSound, SEPlace.transform);
                SEController.inisializeAudioSource(ref GetUpSource, GetUpSound, SEPlace.transform);

                DashSource.loop = true;
                Atk2loopSource.loop = true;
                break;
            case MonsterType.HallGuardian:
                SEController.inisializeAudioSource(ref BackSource, BackSound, SEPlace.transform);
                SEController.inisializeAudioSource(ref BeginTouchSource, BeginTouchSound, SEPlace.transform);
                SEController.inisializeAudioSource(ref BeginAtkSource, BeginAtkSound, SEPlace.transform);
                SEController.inisializeAudioSource(ref BeginAtkWalkSource, BeginAtkWalkSound, SEPlace.transform);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        _deltaTime = Time.deltaTime;

        if (AtkSource.isPlaying)
        {
            AtkSource.volume = SEController.FOVCalculate(_basicData.AbsDistanceX, AtkValidDistance);
        }
        if (Atk2Source.isPlaying)
        {
            Atk2Source.volume = SEController.FOVCalculate(_basicData.AbsDistanceX, Atk2ValidDistance);
        }
        if (PrepareAtkSource.isPlaying)
        {
            PrepareAtkSource.volume = SEController.FOVCalculate(_basicData.AbsDistanceX, PrepareAtkValidDistance);
        }
        if (TouchGroundSource.isPlaying)
        {
            TouchGroundSource.volume = SEController.FOVCalculate(_basicData.AbsDistanceX, TouchGroundValidDistance);
        }
        if (WalkSource.isPlaying)
        {
            WalkSource.volume = SEController.FOVCalculate(_basicData.AbsDistanceX, WalkValidDistance);
        }

        switch (_monsterType)
        {
            case MonsterType.Boss1:
                if (DashSource.isPlaying)
                {
                    DashSoundRate = SEController.VolumeFadeInAndOut(ref TurningOnDashSound, ref TurningOffDashSound, DashSoundRate, DashSoundChangeSpeed, _deltaTime);
                    DashSource.volume = SEController.FOVCalculate(_basicData.AbsDistanceX, DashValidDistance) * DashSoundRate;
                }
                if (BackDashSource.isPlaying)
                {
                    BackDashSource.volume = SEController.FOVCalculate(_basicData.AbsDistanceX, BackDashValidDistance);
                }
                if (Atk1_5Source.isPlaying)
                {
                    Atk1_5Source.volume = SEController.FOVCalculate(_basicData.AbsDistanceX, Atk1_5ValidDistance);
                }
                if (Atk2BeginSource.isPlaying)
                {
                    Atk2BeginSource.volume = SEController.FOVCalculate(_basicData.AbsDistanceX, Atk2BeginValidDistance);
                }
                if (Atk2loopSource.isPlaying)
                {
                    Atk2loopSoundRate = SEController.VolumeFadeInAndOut(ref TurningOnAtk2loopSound, ref TurningOffAtk2loopSound, Atk2loopSoundRate, Atk2loopSoundChangeSpeed, _deltaTime);
                    Atk2loopSource.volume = SEController.FOVCalculate(_basicData.AbsDistanceX, Atk2loopValidDistance) * Atk2loopSoundRate;
                }
                if (Atk4StepSource.isPlaying)
                {
                    Atk4StepSource.volume = SEController.FOVCalculate(_basicData.AbsDistanceX, Atk4StepValidDistance);
                }
                if (CaptureAtk1Source.isPlaying)
                {
                    CaptureAtk1Source.volume = SEController.FOVCalculate(_basicData.AbsDistanceX, CaptureAtk1ValidDistance);
                }
                if (CaptureAtk2Source.isPlaying)
                {
                    CaptureAtk2Source.volume = SEController.FOVCalculate(_basicData.AbsDistanceX, CaptureAtk2ValidDistance);
                }
                if (CaptureAtk3Source.isPlaying)
                {
                    CaptureAtk3Source.volume = SEController.FOVCalculate(_basicData.AbsDistanceX, CaptureAtk3ValidDistance);
                }
                if (FallDownSource.isPlaying)
                {
                    FallDownSource.volume = SEController.FOVCalculate(_basicData.AbsDistanceX, FallDownValidDistance);
                }
                if (GetUpSource.isPlaying)
                {
                    GetUpSource.volume = SEController.FOVCalculate(_basicData.AbsDistanceX, GetUpValidDistance);
                }
                break;
            case MonsterType.HallGuardian:
                if (BackSource.isPlaying)
                {
                    BackSource.volume = SEController.FOVCalculate(_basicData.AbsDistanceX, BackValidDistance);
                }
                if (BeginTouchSource.isPlaying)
                {
                    BeginTouchSource.volume = SEController.FOVCalculate(_basicData.AbsDistanceX, BeginTouchValidDistance);
                }
                if (BeginAtkSource.isPlaying)
                {
                    BeginAtkSource.volume = SEController.FOVCalculate(_basicData.AbsDistanceX, BeginAtkValidDistance);
                }
                if (BeginAtkWalkSource.isPlaying)
                {
                    BeginAtkWalkSource.volume = SEController.FOVCalculate(_basicData.AbsDistanceX, BeginAtkWalkValidDistance);
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

    public void AtkSoundPlay()
    {
        AtkSource.PlayDelayed(AtkSoundTime);
    }

    public void Atk2SoundPlay(string Type)
    {
        switch (Type)
        {
            case "Atk2":
                Atk2Source.PlayDelayed(Atk2SoundTime);
                break;
            case "BackAtk2":
                Atk2Source.PlayDelayed(BackAtkSoundTime);
                break;
        }
    }

    public void Atk2BeginSoundPlay()
    {
        Atk2BeginSource.PlayDelayed(Atk2BeginSoundTime);
    }

    public void Atk2loopSoundPlay()
    {
        if (_basicData.AbsDistanceX <= Atk2loopValidDistance)
        {
            if (!Atk2loopSource.isPlaying)
            {
                Atk2loopSoundRate = 1;
                Atk2loopSource.PlayDelayed(Atk2loopSoundTime);
            }
        }
    }

    public void TurnOffAtk2loopSound()
    {
        if (Atk2loopSoundRate <= 0 && Atk2loopSource.isPlaying)
        {
            Atk2loopSource.Stop();
            return;
        }
        if(!TurningOffAtk2loopSound && Atk2loopSource.isPlaying)
        {
            TurningOffAtk2loopSound = true;
        }
    }

    public void PrepareAtkSoundPlay()
    {
        PrepareAtkSource.PlayDelayed(PrepareAtkSoundTime);
    }

    public void PrepareAtkSoundPlay(float Time)
    {
        PrepareAtkSource.PlayDelayed(Time);
    }

    public void TouchGroundSoundPlay()
    {
        TouchGroundSource.PlayDelayed(TouchGroundSoundTime);
    }

    public void WalkSoundPlay()
    {
        if (_basicData.AbsDistanceX <= WalkValidDistance)
        {
            if (!WalkSource.isPlaying)
            {
                WalkSource.PlayDelayed(WalkSoundTime);
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

    public void BackAtkSoundPlay()
    {
        BackSource.PlayDelayed(BackSoundTime);
    }

    public void DashSoundPlay()
    {
        if (_basicData.AbsDistanceX <= DashValidDistance)
        {
            if (!DashSource.isPlaying)
            {
                DashSoundRate = 1;
                DashSource.PlayDelayed(DashSoundTime);
            }
        }
    }

    public void TurnOffDashSound()
    {
        if (DashSoundRate <= 0 && DashSource.isPlaying)
        {
            DashSource.Stop();
            return;
        }
        if (!TurningOffDashSound && DashSource.isPlaying)
        {
            TurningOffDashSound = true;
        }
    }

    public void BackDashPlay()
    {
        BackDashSource.PlayDelayed(BackDashSoundTime);
    }

    public void Atk1_5SoundPlay()
    {
        Atk1_5Source.PlayDelayed(Atk1_5SoundTime);
    }

    public void Atk1_5SoundPlay(float Time)
    {
        Atk1_5Source.PlayDelayed(Time);
    }

    public void Atk4StepSoundPlay()
    {
        Atk4StepSource.PlayDelayed(Atk4StepSoundTime);
    }

    public void CaptureAtk1SoundPlay(float Time)
    {
        CaptureAtk1Source.PlayDelayed(Time);
    }

    public void CaptureAtk2SoundPlay()
    {
        CaptureAtk2Source.PlayDelayed(CaptureAtk2Time);
    }

    public void CaptureAtk3SoundPlay(float Time)
    {
        CaptureAtk3Source.PlayDelayed(Time);
    }

    public void FallDownSoundPlay()
    {
        FallDownSource.PlayDelayed(FallDownTime);
    }

    public void GetUpSoundPlay()
    {
        GetUpSource.PlayDelayed(GetUpTime);
    }

    public void BeginTouchSoundPlay()
    {
        BeginTouchSource.PlayDelayed(BeginTouchTime);
    }

    public void BeginAtkSoundPlay()
    {
        BeginAtkSource.PlayDelayed(BeginAtkTime);
    }

    public void BeginAtkWalkSoundPlay()
    {
        BeginAtkWalkSource.PlayDelayed(BeginAtkWalkTime);
    }

    public void BoolReset()
    {
        SEAppear = false;
        SE2Appear = false;
        SE3Appear = false;
        SE4Appear = false;
    }

    public void StopAllSE()
    {
        AtkSource.Stop();
        Atk2Source.Stop();
        PrepareAtkSource.Stop();
        TouchGroundSource.Stop();
        WalkSource.Stop();

        switch (_monsterType)
        {
            case MonsterType.HallGuardian:
                BackSource.Stop();
                BeginTouchSource.Stop();
                BeginAtkSource.Stop();
                BeginAtkWalkSource.Stop();
                break;
            case MonsterType.Boss1:
                DashSource.Stop();
                BackDashSource.Stop();
                Atk1_5Source.Stop();
                Atk2BeginSource.Stop();
                Atk2loopSource.Stop();
                Atk4StepSource.Stop();
                CaptureAtk1Source.Stop();
                CaptureAtk2Source.Stop();
                CaptureAtk3Source.Stop();
                FallDownSource.Stop();
                GetUpSource.Stop();
                break;
        }
    }

    public void PauseAllSE()
    {
        AtkSource.Pause();
        Atk2Source.Pause();
        PrepareAtkSource.Pause();
        TouchGroundSource.Pause();
        WalkSource.Pause();

        switch (_monsterType)
        {
            case MonsterType.HallGuardian:
                BackSource.Pause();
                BeginTouchSource.Pause();
                BeginAtkSource.Pause();
                BeginAtkWalkSource.Pause();
                break;
            case MonsterType.Boss1:
                DashSource.Pause();
                BackDashSource.Pause();
                Atk1_5Source.Pause();
                Atk2BeginSource.Pause();
                Atk2loopSource.Pause();
                Atk4StepSource.Pause();
                CaptureAtk1Source.Pause();
                CaptureAtk2Source.Pause();
                CaptureAtk3Source.Pause();
                FallDownSource.Pause();
                GetUpSource.Pause();
                break;
        }
    }

    public void UnPauseAllSE()
    {
        AtkSource.UnPause();
        Atk2Source.UnPause();
        PrepareAtkSource.UnPause();
        TouchGroundSource.UnPause();
        WalkSource.UnPause();

        switch (_monsterType)
        {
            case MonsterType.HallGuardian:
                BackSource.UnPause();
                BeginTouchSource.UnPause();
                BeginAtkSource.UnPause();
                BeginAtkWalkSource.UnPause();
                break;
            case MonsterType.Boss1:
                DashSource.UnPause();
                BackDashSource.UnPause();
                Atk1_5Source.UnPause();
                Atk2BeginSource.UnPause();
                Atk2loopSource.UnPause();
                Atk4StepSource.UnPause();
                CaptureAtk1Source.UnPause();
                CaptureAtk2Source.UnPause();
                CaptureAtk3Source.UnPause();
                FallDownSource.UnPause();
                GetUpSource.UnPause();
                break;
        }
    }
}
