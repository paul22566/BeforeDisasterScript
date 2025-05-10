using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XBoxControllerFalse : MonoBehaviour
{
    private XboxControllerDetect _controller;

    private void Start()
    {
        _controller = this.GetComponent<XboxControllerDetect>();
    }

    // Start is called before the first frame update
    private void LateUpdate()
    {
        _controller.ControllerFalse();
    }
}
