using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPoint : MonoBehaviour
{
    public static int CheckPointNumber = 0;// 有被其他script用到(BackgroundSystem，playerController，playerData，titleController，CreatePlayer)
    private float Timer;
    private float TimerSet = 0.5f;
    private float _deltaTime;

    // Start is called before the first frame update
    void Start()
    {
        Timer = TimerSet;
    }

    private void Update()
    {
        if (CreatePlayer.isNewGame || CreatePlayer.isLoadGame || PlayerController.isDie)
        {
            _deltaTime = Time.deltaTime;
            Timer -= _deltaTime;
            if (Timer <= 0)
            {
                switch (CheckPointNumber)
                {
                    case 0:
                        SceneManager.LoadScene("Training0");
                        break;
                    case 1:
                        SceneManager.LoadScene("original");
                        break;
                    case 2:
                        SceneManager.LoadScene("1F-1");
                        break;
                    case 3:
                        SceneManager.LoadScene("Stair");
                        break;
                    case 4:
                        SceneManager.LoadScene("2F-2");
                        break;
                    case 5:
                        SceneManager.LoadScene("3F-1");
                        break;
                    case 6:
                        SceneManager.LoadScene("FinalStair");
                        break;
                    default:
                        print("Error");
                        break;
                }
            }
        }
    }
}
