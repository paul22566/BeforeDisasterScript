using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestGameSpeed : MonoBehaviour
{
    public Text Number;

    // Update is called once per frame
    void Update()
    {
        Number.text = BackgroundSystem.BasicGameSpeed.ToString();
    }

    public  void ChangeGameSpeed()
    {
        if (BackgroundSystem.BasicGameSpeed == 1)
        {
            BackgroundSystem.BasicGameSpeed = 0.5f;
            return;
        }
        if (BackgroundSystem.BasicGameSpeed == 0.5f)
        {
            BackgroundSystem.BasicGameSpeed = 0.25f;
            return;
        }
        if (BackgroundSystem.BasicGameSpeed == 0.25f)
        {
            BackgroundSystem.BasicGameSpeed = 2;
            return;
        }
        if (BackgroundSystem.BasicGameSpeed == 2)
        {
            BackgroundSystem.BasicGameSpeed = 1;
            return;
        }
    }
}
