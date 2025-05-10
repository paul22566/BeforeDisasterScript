using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialButton : MonoBehaviour
{
    public static int NowTutorialButton = 1;//script(TutorialWindow，PauseMenuController)
    public static bool isDestroy;//script(TutorialWindow)
    [HideInInspector] public int NowOrder;
    [HideInInspector] public int ButtonOrder;//script(TutorialWindow)
    private TutorialWindow _tutorialWindow;
    private Transform _transform;
    private Image _image;
    private float ButtonDistanceY;
    private float _orderOnePlaceX;
    private float _orderOnePlaceY;

    private bool HasRead;

    private void Awake()
    {
        _image = this.GetComponent<Image>();
        _transform = this.transform;
        _tutorialWindow = _transform.parent.parent.parent.parent.gameObject.GetComponent<TutorialWindow>();
        _orderOnePlaceX = _tutorialWindow.OrderOnePlace.x;
        _orderOnePlaceY = _tutorialWindow.OrderOnePlace.y;
        ButtonDistanceY = _tutorialWindow.ButtonDistance.y;

        //決定位置
        _transform.localPosition = new Vector3(_orderOnePlaceX, _orderOnePlaceY + NowOrder * ButtonDistanceY, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //銷毀
        if (Input.GetKeyDown(KeyCode.Escape) && !_tutorialWindow.isTutorialDetailAppear)
        {
            isDestroy = true;
            Destroy(this.gameObject);
            return;
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton1) && !_tutorialWindow.isTutorialDetailAppear)
        {
            isDestroy = true;
            Destroy(this.gameObject);
            return;
        }

        //選項是否被選中&& new關閉
        if (NowOrder == NowTutorialButton - 1)
        {
            _image.color = new Color(0.66f, 0.66f, 0.66f, 1);
            _tutorialWindow.TutorialWindowNumber = ButtonOrder;
            if (!HasRead)
            {
                TurnOffReadNotice();
            }//關閉現在顯示的New
        }
        else
        {
            _image.color = new Color(1, 1, 1, 1);
        }
    }

    public void TurnOffReadNotice()
    {
        this.transform.GetChild(1).gameObject.SetActive(false);
        HasRead = true;
    }
}
