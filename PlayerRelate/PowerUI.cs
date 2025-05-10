using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUI : MonoBehaviour
{
    public Animator ThisAnimation;
    public int Number;
    private BattleSystem _battleSystem;

    private void Start()
    {
        if (GameObject.Find("player") != null)
        {
            _battleSystem = GameObject.Find("player").GetComponent<BattleSystem>();
        }
    }
    void Update()
    {
        switch (Number)
        {
            case 1:
                if (_battleSystem.SkillPower >= 300)
                {
                    ThisAnimation.SetBool("Full", true);
                }
                else
                {
                    ThisAnimation.SetBool("Full", false);
                }
                break;
            case 2:
                if (_battleSystem.SkillPower >= 600)
                {
                    ThisAnimation.SetBool("Full", true);
                }
                else
                {
                    ThisAnimation.SetBool("Full", false);
                }
                break;
            case 3:
                if (_battleSystem.SkillPower >= 900)
                {
                    ThisAnimation.SetBool("Full", true);
                }
                else
                {
                    ThisAnimation.SetBool("Full", false);
                }
                break;
            case 4:
                if (_battleSystem.SkillPower >= 1200)
                {
                    ThisAnimation.SetBool("Full", true);
                }
                else
                {
                    ThisAnimation.SetBool("Full", false);
                }
                break;
            case 5:
                if (_battleSystem.SkillPower >= 1500)
                {
                    ThisAnimation.SetBool("Full", true);
                }
                else
                {
                    ThisAnimation.SetBool("Full", false);
                }
                break;
        }
    }
}
