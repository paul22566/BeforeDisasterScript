using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalStairSwitch : MonoBehaviour
{
    private GameObject InvestigateUI;
    public GameObject NeedKeyNotice;
    private PlayerData _PlayerData;
    private InteractableObject _Interactable;
    private Dialog _dialog;

    void Start()
    {
        if (GameObject.Find("FollowSystem") != null)
        {
            _PlayerData = GameObject.Find("FollowSystem").GetComponent<PlayerData>();
        }
        InvestigateUI = this.gameObject.transform.GetChild(0).gameObject;
        _Interactable = this.GetComponent<InteractableObject>();
        _dialog = this.GetComponent<Dialog>();
    }

    void Update()
    {
        /*if (_Interactable.PlayerInteract && !BackgroundSystem.isKeyNoticeOpen)
        {
            if (itemManage.CheckItemExist(14))
            {
                GameEvent.FinalStairUnlock = true;
                InvestigateUI.SetActive(false);
                _PlayerData.CommonSave();
            }
            else
            {
                NeedKeyNotice.SetActive(true);
                BackgroundSystem.isKeyNoticeOpen = true;
            }
        }*/
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _dialog.EnterPlayer(collision, ref GameEvent.FinalStairUnlock);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _dialog.ExitPlayer(collision);
    }
}
