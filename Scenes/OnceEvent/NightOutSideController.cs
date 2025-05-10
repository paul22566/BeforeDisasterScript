using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightOutSideController : MonoBehaviour
{
    public GameObject Light;
    public GameObject BlackScreen;
    // Start is called before the first frame update
    void Start()
    {
        if (GameEvent.OpenOriginalLight)
        {
            Light.SetActive(true);
            BlackScreen.SetActive(false);
        }
    }
}
