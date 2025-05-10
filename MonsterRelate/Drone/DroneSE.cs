using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneSE : MonoBehaviour
{
    private GameObject SEPlace;
    private DroneController _controller;
    private float _deltaTime;

    public AudioClip OpenSound;//開機聲
    public AudioClip NormalRunSound;//普通行進聲

    private AudioSource OpenSource;//開機聲
    private AudioSource NormalRunSource;//普通行進聲

    private float OpenValidDistance = 15;
    private float NormalRunValidDistance = 15;

    private float NormalRunSoundRate = 0;
    private float NormalRunSoundChangeSpeed = 0.5f;
    private bool TurningOnNormalRunSound;
    private bool TurningOffNormalRunSound;

    private bool isSEPause;
    // Start is called before the first frame update
    void Start()
    {
        SEPlace = this.gameObject;
        _controller = this.GetComponent<DroneController>();

        SEController.inisializeAudioSource(ref OpenSource, OpenSound, SEPlace.transform);
        SEController.inisializeAudioSource(ref NormalRunSource, NormalRunSound, SEPlace.transform);

        NormalRunSource.loop = true;
    }

    // Update is called once per frame
    void Update()
    {
        _deltaTime = Time.deltaTime;

        if (OpenSource.isPlaying)
        {
            OpenSource.volume = SEController.FOVCalculate(_controller.AbsDistanceX, OpenValidDistance);
        }
        if (NormalRunSource.isPlaying)
        {
            NormalRunSoundRate = SEController.VolumeFadeInAndOut(ref TurningOnNormalRunSound, ref TurningOffNormalRunSound, NormalRunSoundRate, NormalRunSoundChangeSpeed, _deltaTime);
            NormalRunSource.volume = SEController.FOVCalculate(_controller.AbsDistanceX, NormalRunValidDistance) * NormalRunSoundRate;
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

    public void OpenSoundPlay(float Time)
    {
        OpenSource.PlayDelayed(Time);
    }

    public void NormalRunSoundPlay()
    {
        if (_controller.AbsDistanceX <= NormalRunValidDistance && !NormalRunSource.isPlaying)
        {
            TurningOnNormalRunSound = true;
            NormalRunSource.Play();
        }
    }

    public void TurnOffNormalRunSound()
    {
        if (NormalRunSoundRate == 0 && NormalRunSource.isPlaying)
        {
            NormalRunSource.Stop();
            return;
        }
        if (!TurningOffNormalRunSound && NormalRunSource.isPlaying)
        {
            TurningOffNormalRunSound = true;
        }
    }

    public void AbsoluteTurnOffNormalRunSound()
    {
        if (NormalRunSource.isPlaying)
        {
            TurningOffNormalRunSound = false;
            TurningOnNormalRunSound = false;
            NormalRunSoundRate = 0;
            NormalRunSource.Stop();
        }
    }

    public void StopAllSound()
    {
        OpenSource.Stop();
        NormalRunSource.Stop();
    }

    public void PauseAllSE()
    {
        OpenSource.Pause();
        NormalRunSource.Pause();
    }

    public void UnPauseAllSE()
    {
        OpenSource.UnPause();
        NormalRunSource.UnPause();
    }
}
