using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XBoxControllerFalse : MonoBehaviour
{
    private OldVerXboxControllerDetect _controller;

    private void Start()
    {
        _controller = this.GetComponent<OldVerXboxControllerDetect>();
    }

    // Start is called before the first frame update
    private void LateUpdate()
    {
        _controller.ControllerFalse();
    }
}
