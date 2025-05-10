using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchUICamera : MonoBehaviour
{
    private Canvas Canvas;
    // Start is called before the first frame update
    void Start()
    {
        Canvas = this.GetComponent<Canvas>();

        if (GameObject.Find("UICamera") != null)
        {
            Canvas.worldCamera = GameObject.Find("UICamera").GetComponent<Camera>();
        }
    }
}
