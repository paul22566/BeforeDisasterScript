using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VBMonsterComponent : MonoBehaviour
{
    public GameObject RArm;
    public GameObject Body;
    public GameObject SRArm;
    public GameObject SBody;

    public void ChangePhase()
    {
        SRArm.SetActive(true);
        SBody.SetActive(true);
        RArm.SetActive(false);
        Body.SetActive(false);
    }
}
