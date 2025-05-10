using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItem : MonoBehaviour
{
    private enum ItemType {Item, Document}
    [SerializeField] private ItemType _itemType;
    [SerializeField] private int itemID;
    [SerializeField] private int MapItemID;//999不適用常態規則

    public GameObject ItemImage;
    private itemManage _itemManage;
    private InteractableObject _interactable;

    public GameObject GetItemSound;

    void Start()
    {
        _interactable = this.GetComponent<InteractableObject>();
        _interactable._interact += OnInteractItem;
        if (GameObject.Find("FollowSystem") != null)
        {
            _itemManage = GameObject.Find("FollowSystem").GetComponent<itemManage>();
        }
        else
        {
            return;
        }
        if (MapItemID != 999 && _itemManage.CheckMapItemExist(MapItemID))
        {
            Destroy(this.gameObject);
        }
    }

    private void OnInteractItem()
    {
        switch (_itemType)
        {
            case ItemType.Item:
                _itemManage.ItemGet(itemID, MapItemID);
                break;
            case ItemType.Document:
                _itemManage.DocumentGet(itemID, MapItemID);
                break;
        }

        Instantiate(GetItemSound, this.transform.localPosition, Quaternion.identity);
        if (ItemImage != null)
        {
            ItemImage.SetActive(true);
            ItemImage.GetComponent<ItemImageOrder>().Order = itemManage.ItemImageTotalNumber;
        }
        else
        {
            print("NoImage");
        }
        itemManage.ItemImageTotalNumber += 1;
        Destroy(this.gameObject);
    }
}
