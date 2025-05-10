using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SecondFloor2Controller : MonoBehaviour
{
    private float Timer = 5;
    private bool CanReturn;
    private bool isRunning;
    public GameObject Text1;
    public GameObject Text2;
    public GameObject Text3;
    public FadeOutUI _fadeOut;

    private void Start()
    {
        _fadeOut._fadeOutEnd += GoTitle;
    }

    private void Update()
    {
        if (isRunning)
        {
            return;
        }

        if (CanReturn)
        {
            if (Input.anyKeyDown)
            {
                isRunning = true;
                MusicController.ChangeBGM();
                _fadeOut.BeginFadeOut();
            }
        }
    }

    private void FixedUpdate()
    {
        if (isRunning)
        {
            return;
        }

        Timer -= Time.deltaTime;

        if (Timer <= 4)
        {
            Text1.SetActive(true);
        }
        if (Timer <= 2)
        {
            Text2.SetActive(true);
        }
        if (Timer <= 0)
        {
            CanReturn = true;
            Text3.SetActive(true);
        }
    }

    private void GoTitle()
    {
        SceneManager.LoadScene("LoadingTitle");
    }
}
