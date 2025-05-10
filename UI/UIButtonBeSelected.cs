using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonBeSelected : MonoBehaviour
{
    private Selectable _selectable;
    private Button _button;
    private Slider _slider;
    private ColorBlock SliderColorBlock = new ColorBlock();
    private Color UnSelectedColor = new Color(1, 1, 1, 1);
    private Color SelectedColor = new Color(0.57f, 0.57f, 0.57f, 1);

    private bool BeSelected;
    private bool isClick;
    private float Timer;
    private float TimerSet = 0.05f;

    public int VNumber;//其他Script會用到(keyCodeManage，PauseMenuController，TitleController，selectElevatorController)
    public int HNumber;//其他Script會用到(keyCodeManage，PauseMenuController，TitleController，selectElevatorController)

    public bool BlockContinuousClick;//是否要阻擋連續點擊
    //slider
    private bool SliderExist;
    public string SliderType;
    public float SliderMax;
    public float SliderMin;
    public float SliderChangeNumber;
    // Start is called before the first frame update
    void Start()
    {
        _selectable = this.GetComponent<Selectable>();

        if(this.GetComponent<Button>() != null)
        {
            _button = this.GetComponent<Button>();
            _button.onClick.AddListener(OnClick);
            InisializeColor();
        }
        if (this.GetComponent<Slider>() != null)
        {
            SliderExist = true;
            _slider = this.GetComponent<Slider>();
            SliderColorBlock = _slider.colors;
            _slider.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(SelectButtonController.DetectButtonSelected(HNumber, VNumber))
        {
            BeSelected = true;
            _selectable.Select();
            if(SliderExist)
            {
                ChangeSliderColor(SelectedColor);
                _slider.value = SelectButtonController.SliderControll(_slider.value, SliderType, SliderMax, SliderMin, SliderChangeNumber);
            }
        }
        else
        {
            if (BeSelected)
            {
                if (SliderExist)
                {
                    ChangeSliderColor(UnSelectedColor);
                }
                BeSelected = false;
            }
        }
        if (isClick)
        {
            Timer += Time.unscaledDeltaTime;
            if (Timer > TimerSet)
            {
                Timer = 0;
                isClick = false;
                if (BlockContinuousClick)
                {
                    _button.interactable = true;
                }
            }
        }
    }

    public void OnClick()
    {
         isClick = true;
        if (BlockContinuousClick)
        {
            _button.interactable = false;
        }
        SelectButtonController.ConfirmSoundPlay();
    }

    private void InisializeColor()
    {
        ColorBlock cb = _button.colors;
        cb.disabledColor = _button.colors.selectedColor;
        _button.colors = cb;
    }

    private void ChangeSliderColor(Color ChangeColor)
    {
        SliderColorBlock.normalColor = ChangeColor;
        _slider.colors = SliderColorBlock;
    }
}
