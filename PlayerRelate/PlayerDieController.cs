using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDieController : MonoBehaviour
{
    private float DieTimerSet = 3.34f;
    private float DieTimer;
    private FadeOutUI _fadeOut;
    private Portal _diePortal;
    private bool SoundPlay = false;
    private bool Trigger1 = false;

    public delegate void DieTimerEnd();
    public DieTimerEnd _dieTimerEnd;

    public AudioClip DieSound;
    private AudioSource DieSource;
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("UI") != null)
        {
            Transform UICanvas = GameObject.FindGameObjectWithTag("UI").transform;

            _fadeOut = IdentifyID.FindObject(UICanvas, UIID.FadeOut).GetComponent<FadeOutUI>();
        }

        _diePortal = this.GetComponent<Portal>();

        SEController.inisializeAudioSource(ref DieSource, DieSound, this.transform);

        ResetDie();
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        DieTimer -= Time.deltaTime;

        if (!SoundPlay)
        {
            SoundPlay = true;
            MusicController.BeginFadeOutBGM(0.5f, 0.4f);
            DieSource.Play();
        }

        if (DieTimer <= 0 && !Trigger1)
        {
            if (_dieTimerEnd == null)
            {
                _fadeOut._fadeOutEnd += _diePortal.OnBeginLoadScene;
                _dieTimerEnd += _fadeOut.BeginFadeOut;
            }

            _dieTimerEnd();

            MusicController.ChangeBGM();

            Trigger1 = true;
        }

        SEController.CalculateSystemSound(DieSource);
    }

    public void ResetDie()
    {
        DieTimer = DieTimerSet;
        SoundPlay = false;
        Trigger1 = false;
        _dieTimerEnd = null;
    }
}
