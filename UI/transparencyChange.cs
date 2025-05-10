using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class transparencyChange : MonoBehaviour
{
    private enum ChangeMode {Increase, Decrease }
    private ChangeMode changeMode;
    public float RunTime;
    private float Speed;
    public float InitialTransparency;
    public float TargetTransparency;
    [HideInInspector] public bool StartChange;
    private float NowTransparency;
    private Image _image;
    private float _fixedDeltaTime;
    // Start is called before the first frame update
    void Start()
    {
        _image = this.GetComponent<Image>();
        InitialTransparency = InitialTransparency / 255;
        TargetTransparency = TargetTransparency / 255;

        NowTransparency = InitialTransparency;

        Speed = Mathf.Abs(TargetTransparency - InitialTransparency) / RunTime;
        

        if(InitialTransparency <= TargetTransparency)
        {
            changeMode = ChangeMode.Increase;
        }
        else
        {
            changeMode = ChangeMode.Decrease;
        }

        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, NowTransparency);
    }

    private void FixedUpdate()
    {
        if (StartChange)
        {
            _fixedDeltaTime = Time.fixedDeltaTime;
            switch(changeMode)
            {
                case ChangeMode.Increase:
                    if (NowTransparency < TargetTransparency)
                    {
                        NowTransparency += Speed * _fixedDeltaTime;
                    }
                    else
                    {
                        NowTransparency = TargetTransparency;
                        StartChange = false;
                    }
                    break;
                case ChangeMode.Decrease:
                    if (NowTransparency > TargetTransparency)
                    {
                        NowTransparency -= Speed * _fixedDeltaTime;
                    }
                    else
                    {
                        NowTransparency = TargetTransparency;
                        StartChange = false;
                    }
                    break;
            }

            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, NowTransparency);
        }
    }
}
