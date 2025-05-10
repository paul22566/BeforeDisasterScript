using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StairShortCutSwitch : MonoBehaviour
{
    private PlayerData _PlayerData;
    private Dialog _dialog;
    private InteractableObject _interactable;

    void Start()
    {
        if (GameObject.Find("FollowSystem") != null)
        {
            _PlayerData = GameObject.Find("FollowSystem").GetComponent<PlayerData>();
        }
        _dialog = this.GetComponent<Dialog>();
        _interactable = this.GetComponent<InteractableObject>();
    }

    void Update()
    {
        if (!_interactable.inRange || GameEvent.StairShortCutUnlock)
        {
            return;
        }

        /*if (_interactable.PlayerInteract)
        {
            GameEvent.StairShortCutUnlock = true;
            _dialog.TurnOffDialog();
            _PlayerData.CommonSave();
            _interactable.PlayerInteract = false;
        }*/
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _dialog.EnterPlayer(collision, ref GameEvent.StairShortCutUnlock);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _dialog.ExitPlayer(collision);
    }
}
