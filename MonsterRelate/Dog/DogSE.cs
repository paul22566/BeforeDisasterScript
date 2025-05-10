using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogSE : MonoBehaviour
{
    private GameObject SEPlace;
    private MonsterBasicData _basicData;

    public AudioClip BarkSound;//發現吼
    public AudioClip LowBarkSound;//攻擊吼
    public AudioClip WalkSound;//走路

    private AudioSource BarkSource;
    private AudioSource LowBarkSource;
    private AudioSource WalkSource;

    private float BarkValidDistance = 15;
    private float LowBarkValidDistance= 15;
    private float WalkValidDistance = 30;

    private bool isSEPause;
    // Start is called before the first frame update
    void Start()
    {
        SEPlace = this.transform.GetChild(5).gameObject;
        _basicData = this.GetComponent<MonsterBasicData>();

        SEController.inisializeAudioSource(ref BarkSource, BarkSound, SEPlace.transform);
        SEController.inisializeAudioSource(ref LowBarkSource, LowBarkSound, SEPlace.transform);
        SEController.inisializeAudioSource(ref WalkSource, WalkSound, SEPlace.transform);

        WalkSource.loop = true;
    }

    private void Update()
    {
        if (LowBarkSource.isPlaying)
        {
            LowBarkSource.volume = SEController.FOVCalculate(_basicData.AbsDistanceX, LowBarkValidDistance);
        }
        if (WalkSource.isPlaying)
        {
            WalkSource.volume = SEController.FOVCalculate(_basicData.AbsDistanceX, WalkValidDistance);
        }
        if (BarkSource.isPlaying)
        {
            BarkSource.volume = SEController.FOVCalculate(_basicData.AbsDistanceX, BarkValidDistance);
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

    public void BarkSoundPlay(float Time)
    {
        BarkSource.PlayDelayed(Time);
    }

    public void LowBarkSoundPlay(float Time)
    {
        LowBarkSource.PlayDelayed(Time);
    }

    public void WalkSoundPlay()
    {
        if (!WalkSource.isPlaying && _basicData.AbsDistanceX <= WalkValidDistance)
        {
            WalkSource.Play();
        }
    }

    public void TurnOffWalkSound()
    {
        if (WalkSource.isPlaying)
        {
            WalkSource.Stop();
        }
    }

    public void StopAllSE()
    {
        BarkSource.Stop();
        LowBarkSource.Stop();
        WalkSource.Stop();
    }

    private void PauseAllSE()
    {
        BarkSource.Pause();
        LowBarkSource.Pause();
        WalkSource.Pause();
    }

    private void UnPauseAllSE()
    {
        BarkSource.UnPause();
        LowBarkSource.UnPause();
        WalkSource.UnPause();
    }
}
