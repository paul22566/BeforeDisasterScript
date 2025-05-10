using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TrainingDie : MonoBehaviour
{
    private bool DoEvent;

    private float Timer = 6;
    private PlayerDieController DIeUI;
    private FadeOutUI FadeOut;
    public GameObject BlackScreen;
    public Animator Letter;
    public Animator FailWord;

    public AudioClip FailSound;
    public AudioClip PaperSound;
    private AudioSource FailSource;
    private AudioSource PaperSource;

    private bool Trigger1;
    private bool Trigger2;
    private bool Trigger3;

    private Portal _portal;
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("UI") != null)
        {
            Transform UICanvas = GameObject.FindGameObjectWithTag("UI").transform;

            DIeUI = IdentifyID.FindObject(UICanvas, UIID.Die).GetComponent<PlayerDieController>();
            FadeOut = IdentifyID.FindObject(UICanvas, UIID.FadeOut).GetComponent<FadeOutUI>();

            DIeUI._dieTimerEnd += OnLetterAppear;
        }

        _portal = this.GetComponent<Portal>();

        SEController.inisializeAudioSource(ref PaperSource, PaperSound, this.transform);
        SEController.inisializeAudioSource(ref FailSource, FailSound, this.transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (!DoEvent)
        {
            return;
        }

        Timer -= Time.deltaTime;

        if (!Trigger1)
        {
            Trigger1 = true;
            BlackScreen.SetActive(true);
        }

        if (Timer <= (6 - 1.5f))
        {
            if (!Trigger2)
            {
                Trigger2 = true;
                Letter.SetBool("Appear", true);
                PaperSource.Play();
            }
        }
        if (Timer <= (6 - 3.5f))
        {
            if (!Trigger3)
            {
                Trigger3 = true;
                FailWord.SetBool("Immediately", true);
                FailSource.Play();
            }
        }
        if (Timer <= 0)
        {
            FadeOut._fadeOutEnd += _portal.OnBeginLoadScene;
            FadeOut.BeginFadeOut();
            FailWord.SetBool("Disappear", true);
            Letter.SetBool("Disappear", true);
            DoEvent = false;
        }

        SEController.CalculateSystemSound(PaperSource);
        SEController.CalculateSystemSound(FailSource);
    }

    private void OnLetterAppear()
    {
        DoEvent = true;
    }
}
