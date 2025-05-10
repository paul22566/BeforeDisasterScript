using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public static string SceneName = "Training0"; //有被其他script用到(playerController，undergroundElevator，backGroundSystem)
    private float Timer;
    private float TimerSet = 0.1f;
    private float _deltaTime;
    public GameObject BlackBackground;
    public GameObject WhiteBackground;

    public static bool LoadSceneWithWhiteBackground;
    // Start is called before the first frame update
    void Start()
    {
        Timer = TimerSet;
    }
    private void Update()
    {
        if (!CreatePlayer.isLoadGame && !PlayerController.isDie && !CreatePlayer.isNewGame)
        {
            if (LoadSceneWithWhiteBackground)
            {
                BlackBackground.SetActive(false);
                WhiteBackground.SetActive(true);
                LoadSceneWithWhiteBackground = false;
            }
            _deltaTime = Time.deltaTime;
            Timer -= _deltaTime;
            if (Timer <= 0)
            {
                SceneManager.LoadScene(SceneName);
            }
        }
    }
}
