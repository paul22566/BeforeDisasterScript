using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldManSE : MonoBehaviour
{
    private GameObject SEPlace;
    private MonsterBasicData _basicData;
    [SerializeField] private int SEPlaceNumber;

    public AudioClip AtkSound1;//普攻1
    public AudioClip AtkSound2;//普攻2
    public AudioClip WalkSound;//走路
    public AudioClip HeavyWalkSound;//走路

    private AudioSource AtkSource1;//普攻1
    private AudioSource AtkSource2;//普攻2
    private AudioSource WalkSource;//走路

    private float AtkValidDistance1 = 15;//普攻1
    private float AtkValidDistance2 = 15;//普攻2
    private float WalkValidDistance = 20;//走路

    private float AtkSound1Time = 0.5f;
    private float AtkSound2Time = 0.7f;
    [HideInInspector] public bool SEAppear;
    [HideInInspector] public bool SE2Appear;
    [HideInInspector] public bool SE3Appear;
    private bool isSEPause;

    void Start()
    {
        SEPlace = this.transform.GetChild(SEPlaceNumber).gameObject;
        _basicData = this.GetComponent<MonsterBasicData>();

        SEController.inisializeAudioSource(ref AtkSource1, AtkSound1, SEPlace.transform);
        SEController.inisializeAudioSource(ref AtkSource2, AtkSound2, SEPlace.transform);
        SEController.inisializeAudioSource(ref WalkSource, WalkSound, SEPlace.transform);

        WalkSource.loop = true;
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

    public void HeavyWalkSoundPlay()
    {
        if (_basicData.AbsDistanceX <= WalkValidDistance)
        {
            if (!WalkSource.isPlaying || WalkSource.clip != HeavyWalkSound)
            {
                WalkSource.clip = HeavyWalkSound;
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
    }

    public void PauseAllSE()
    {
        AtkSource1.Pause();
        AtkSource2.Pause();
        WalkSource.Pause();
    }

    public void UnPauseAllSE()
    {
        AtkSource1.UnPause();
        AtkSource2.UnPause();
        WalkSource.UnPause();
    }
}
