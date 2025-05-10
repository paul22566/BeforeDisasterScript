using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallDarkBallController : MonoBehaviour
{
    public float Speed;

    private void Start()
    {
        if (EvilKingController.isSecondPhase)
        {
            Speed = 10;
        }
    }
    void Update()
    {
        this.transform.position += new Vector3(-Speed * Time.deltaTime,-Speed * 2 * Time.deltaTime, 0 );
    }
}
