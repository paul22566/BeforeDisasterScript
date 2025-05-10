using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDroneSE : MonoBehaviour
{
    private GameObject SEPlace;
    private ExplosionDroneController _controller;

    public AudioClip OpenSound;//�}���n
    public AudioClip FastRunSound;//�ֳt��i�n

    private AudioSource OpenSource;//�}���n
    private AudioSource FastRunSource;//�ֳt��i�n

    private float OpenValidDistance = 15;
    private float FastRunValidDistance = 15;

    private bool isSEPause;
    // Start is called before the first frame update
    void Start()
    {
        SEPlace = this.gameObject;
        _controller = this.GetComponent<ExplosionDroneController>();

        SEController.inisializeAudioSource(ref OpenSource, OpenSound, SEPlace.transform);
        SEController.inisializeAudioSource(ref FastRunSource, FastRunSound, SEPlace.transform);

        FastRunSource.loop = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (OpenSource.isPlaying)
        {
            OpenSource.volume = SEController.FOVCalculate(_controller.AbsDistanceX, OpenValidDistance);
        }
        if (FastRunSource.isPlaying)
        {
            FastRunSource.volume = SEController.FOVCalculate(_controller.AbsDistanceX, FastRunValidDistance);
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

    public void OpenSoundPlay(float Time)
    {
        OpenSource.PlayDelayed(Time);
    }

    public void FastRunSoundPlay()
    {
        if (!FastRunSource.isPlaying)
        {
            FastRunSource.Play();
        }
    }

    public void TurnOffFastRunSound()
    {
        if (FastRunSource.isPlaying)
        {
            FastRunSource.Stop();
        }
    }

    public void PauseAllSE()
    {
        OpenSource.Pause();
        FastRunSource.Pause();
    }

    public void UnPauseAllSE()
    {
        OpenSource.UnPause();
        FastRunSource.UnPause();
    }
}
