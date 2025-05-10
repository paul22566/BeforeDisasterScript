using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OriginalDoorSwitch : MonoBehaviour
{
    private PlayerData _PlayerData;
    private Dialog _dialog;
    private InteractableObject _interactabe;

    void Start()
    {
        if (GameObject.Find("FollowSystem") != null)
        {
            _PlayerData = GameObject.Find("FollowSystem").GetComponent<PlayerData>();
        }
        _dialog = this.GetComponent<Dialog>();
        _interactabe = this.GetComponent<InteractableObject>();
    }

    void Update()
    {
        if (!_interactabe.inRange || GameEvent.OpenOriginalDoor)
        {
            return;
        }

        /*if (_interactabe.PlayerInteract)
        {
            GameEvent.OpenOriginalDoor = true;
            _PlayerData.CommonSave();
            _interactabe.PlayerInteract = false;
        }*/
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _dialog.EnterPlayer(collision, ref GameEvent.OpenOriginalDoor);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _dialog.ExitPlayer(collision);
    }
}
