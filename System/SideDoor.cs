using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideDoor : MonoBehaviour
{
    private Animator DoorAni;
    private BoxCollider2D _boxCollider;

    public AudioClip OpenSound;
    public AudioClip CloseSound;

    private AudioSource OpenSource;
    private AudioSource CloseSource;

    private enum Status { Open, Close };
    [SerializeField] private Status _inisialStatus;
    // Start is called before the first frame update
    void Awake()
    {
        _boxCollider = this.gameObject.GetComponent<BoxCollider2D>();
        DoorAni = this.gameObject.GetComponent<Animator>();

        switch (_inisialStatus)
        {
            case Status.Open:
                DoorAni.SetInteger("Status", 2);
                break;
        }

        SEController.inisializeAudioSource(ref OpenSource, OpenSound, this.transform);
        SEController.inisializeAudioSource(ref CloseSource, CloseSound, this.transform);
    }

    private void Update()
    {
        SEController.CalculateSystemSound(OpenSource);
        SEController.CalculateSystemSound(CloseSource);
    }

    public void CloseDoor()
    {
        CloseSource.Play();
        DoorAni.SetInteger("Status", 0);
        _boxCollider.isTrigger = false;
    }

    public void OpenDoor()
    {
        OpenSource.Play();
        DoorAni.SetInteger("Status", 1);
        _boxCollider.isTrigger = true;
    }
}
