using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VBmonsterSwitchPhase : MonoBehaviour
{
    private VeryBigMonsterController _controller;

    private VBMonsterComponent WaitComponent;
    private VBMonsterComponent WalkComponent;
    private VBMonsterComponent JumpComponent;
    private VBMonsterComponent Atk1Component;
    private VBMonsterComponent Atk1_5Component;
    private VBMonsterComponent Atk2Component;
    private VBMonsterComponent Atk3Component;
    private VBMonsterComponent Atk4Component;
    private VBMonsterComponent SummonComponent;
    private VBMonsterComponent CaptureComponent;
    private VBMonsterComponent StopComponent;

    private bool HasChange;
    // Start is called before the first frame update
    void Start()
    {
        _controller = this.GetComponent<VeryBigMonsterController>();

        WaitComponent = this.transform.GetChild(0).GetComponent<VBMonsterComponent>();
        WalkComponent = this.transform.GetChild(1).GetComponent<VBMonsterComponent>();
        JumpComponent = this.transform.GetChild(2).GetComponent<VBMonsterComponent>();
        Atk1Component = this.transform.GetChild(3).GetComponent<VBMonsterComponent>();
        Atk1_5Component = this.transform.GetChild(4).GetComponent<VBMonsterComponent>();
        Atk2Component = this.transform.GetChild(5).GetComponent<VBMonsterComponent>();
        Atk3Component = this.transform.GetChild(6).GetComponent<VBMonsterComponent>();
        Atk4Component = this.transform.GetChild(7).GetComponent<VBMonsterComponent>();
        SummonComponent = this.transform.GetChild(8).GetComponent<VBMonsterComponent>();
        CaptureComponent = this.transform.GetChild(9).GetComponent<VBMonsterComponent>();
        StopComponent = this.transform.GetChild(11).GetComponent<VBMonsterComponent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!HasChange && _controller.isSecondPhase)
        {
            WaitComponent.ChangePhase();
            WalkComponent.ChangePhase();
            JumpComponent.ChangePhase();
            Atk1Component.ChangePhase();
            Atk1_5Component.ChangePhase();
            Atk2Component.ChangePhase();
            Atk3Component.ChangePhase();
            Atk4Component.ChangePhase();
            SummonComponent.ChangePhase();
            CaptureComponent.ChangePhase();
            StopComponent.ChangePhase();
            HasChange = true;
        }
    }
}
