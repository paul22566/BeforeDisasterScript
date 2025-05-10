using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenEye : MonoBehaviour
{
    private int TotalChildren;

    private void Start()
    {
        TotalChildren = transform.childCount;
    }
    void Update()
    {
        if (CaptainController.isSecondPhase)
        {
            this.transform.GetChild(TotalChildren - 1).gameObject.SetActive(true);
        }
        else
        {
            this.transform.GetChild(TotalChildren - 1).gameObject.SetActive(false);
        }
    }
}
