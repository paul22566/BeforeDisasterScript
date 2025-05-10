using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallDarkLightSword : MonoBehaviour
{
    public float Speed;

    private void Start()
    {
        if (EvilKingController.isSecondPhase)
        {
            Speed = 18;
        }
    }
    void Update()
    {
        this.transform.position += new Vector3(0, -Speed * Time.deltaTime, 0);
    }
}
