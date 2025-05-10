using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnouncementController : MonoBehaviour
{
    public GameObject Announcement;
    public static bool isOpenAnnouncement;
    private InteractableObject _interactable;
    // Start is called before the first frame update
    void Start()
    {
        _interactable = this.GetComponent<InteractableObject>();
        _interactable._interact += OnInteract;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_interactable.inRange)
        {
            if (isOpenAnnouncement)
            {
                isOpenAnnouncement = false;
                BackgroundSystem.CantPause = false;
                Announcement.SetActive(false);
            }
            return;
        }

        if (isOpenAnnouncement)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1))
            {
                isOpenAnnouncement = false;
                BackgroundSystem.CantPause = false;
                Announcement.SetActive(false);
            }
        }
    }

    private void OnInteract()
    {
        Announcement.SetActive(true);
        isOpenAnnouncement = true;
        BackgroundSystem.CantPause = true;
    }
}
