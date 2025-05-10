using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    public enum Status { Item, Document}
    public Status status;
    public static int NowItemButton = 1;//script(itemWindow，PauseMenuController)
    public static int NowDocumentButton = 1;//script(itemWindow，PauseMenuController)
    public static bool isDestroy;//script(itemWindow)
    [HideInInspector] public int ButtonOrder;
    [HideInInspector] public int NowOrder;
    private ItemWindow _itemWindow;
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
        _itemWindow = _transform.parent.parent.parent.parent.gameObject.GetComponent<ItemWindow>();
        _orderOnePlaceX = _itemWindow.OrderOnePlace.x;
        _orderOnePlaceY = _itemWindow.OrderOnePlace.y;
        ButtonDistanceY = _itemWindow.ButtonDistance.y;
        //決定位置
        _transform.localPosition = new Vector3(_orderOnePlaceX, _orderOnePlaceY + NowOrder * ButtonDistanceY, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //銷毀
        switch (status)
        {
            case Status.Item:
                if(_itemWindow.status != ItemWindow.Status.Item || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1) || PlayerController.isHurted)
                {
                    isDestroy = true;
                    Destroy(this.gameObject);
                    return;
                }
                break;
            case Status.Document:
                if (_itemWindow.status != ItemWindow.Status.Document || PlayerController.isHurted)
                {
                    isDestroy = true;
                    Destroy(this.gameObject);
                    return;
                }
                if (Input.GetKeyDown(KeyCode.Escape) && !_itemWindow.isDocumentDetailAppear)
                {
                    isDestroy = true;
                    Destroy(this.gameObject);
                    return;
                }
                if (Input.GetKeyDown(KeyCode.JoystickButton1) && !_itemWindow.isDocumentDetailAppear)
                {
                    isDestroy = true;
                    Destroy(this.gameObject);
                    return;
                }
                break;
        }

        //選項是否被選中
        switch (status)
        {
            case Status.Item:
                if(NowOrder == NowItemButton - 1)
                {
                    _image.color = new Color(0.66f, 0.66f, 0.66f, 1);
                    _itemWindow.ItemDisplayWindowNumber = ButtonOrder;
                    if (!HasRead)
                    {
                        TurnOffReadNotice();
                    }//關閉現在顯示的New
                }
                else
                {
                    _image.color = new Color(1, 1, 1, 1);
                }
                break;
            case Status.Document:
                if (NowOrder == NowDocumentButton - 1)
                {
                    _image.color = new Color(0.66f, 0.66f, 0.66f, 1);
                    _itemWindow.DocumentDisplayWindowNumber = ButtonOrder;
                    if (!HasRead)
                    {
                        TurnOffReadNotice();
                    }//關閉現在顯示的New
                }
                else
                {
                    _image.color = new Color(1, 1, 1, 1);
                }
                break;
        }
    }

    public void TurnOffReadNotice()
    {
        this.transform.GetChild(1).gameObject.SetActive(false);
        HasRead = true;
    }
}
