using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPowerChangeAni : MonoBehaviour
{
    public Animator SkillPower;
    public Animator SkillPowerBackground;
    public Animator SkillPowerFrame;
    public GameObject Image1;
    public GameObject Image2;
    private bool isDoEvent;
    // Start is called before the first frame update
    void Start()
    {
        if (!GameEvent.AbsorbBoss2)
        {
            isDoEvent = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isDoEvent)
        {
            if (GameEvent.AbsorbBoss2)
            {
                SkillPower.SetBool("ON", true);
                SkillPowerBackground.SetBool("ON", true);
                SkillPowerFrame.SetBool("ON", true);
                Image1.SetActive(false);
                Image2.SetActive(false);
                isDoEvent = false;
            }
        }
    }

    public void BeginSealPower()
    {
        SkillPowerFrame.SetBool("SealOn", true);
    }

    public void SealPowerEnd()
    {
        SkillPowerFrame.SetBool("SealOn", false);
    }
}
