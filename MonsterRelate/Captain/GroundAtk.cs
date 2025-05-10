using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundAtk : MonoBehaviour
{
    private CaptainController _controller;
    // Start is called before the first frame update
    void Start()
    {
        _controller = GameObject.Find("Boss2").GetComponent<CaptainController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_controller.Atk3MoveEnd)
        {
            Destroy(this.gameObject);
        }
    }
}
