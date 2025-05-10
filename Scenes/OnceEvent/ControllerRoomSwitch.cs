using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerRoomSwitch : MonoBehaviour
{
    public Animator Animation;
    private PlayerData _PlayerData;
    private InteractableObject _Interactable;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("FollowSystem") != null)
        {
            _PlayerData = GameObject.Find("FollowSystem").GetComponent<PlayerData>();
        }
        if (GameEvent.OpenOriginalLight)
        {
            Animation.SetBool("Skip", true);
        }
        _Interactable = this.GetComponent<InteractableObject>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (_Interactable.PlayerInteract && !GameEvent.OpenOriginalLight)
        {
            Animation.SetBool("isOpen", true);
            GameEvent.OpenOriginalLight = true;
            _PlayerData.CommonSave();
            _Interactable.PlayerInteract = false;
        }*/
    }
}
