using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBodyBoss2 : MonoBehaviour
{
    private Dialog _dialog;
    private InteractableObject _interactable;

    private void Start()
    {
        _dialog = this.GetComponent<Dialog>();
        _interactable = this.GetComponent<InteractableObject>();
    }

    void Update()
    {
        /*if (_interactable.PlayerInteract)
        {
            GameEvent.AbsorbBoss2 = true;
            _dialog.TurnOffDialog();
            _interactable.PlayerInteract = false;
        }*/
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _dialog.EnterPlayer(collision, ref GameEvent.AbsorbBoss2);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _dialog.ExitPlayer(collision);
    }
}
