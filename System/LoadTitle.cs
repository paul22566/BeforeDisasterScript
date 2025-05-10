using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadTitle : MonoBehaviour
{
    private float Timer;
    private float TimerSet = 0.1f;
    private float _deltaTime;
    // Start is called before the first frame update
    void Start()
    {
        Timer = TimerSet;
    }
    private void Update()
    {
        _deltaTime = Time.deltaTime;
        Timer -= _deltaTime;
        if (Timer <= 0)
        {
            SceneManager.LoadScene("title");
        }
    }
}
