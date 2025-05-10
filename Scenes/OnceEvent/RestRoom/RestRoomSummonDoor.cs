using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestRoomSummonDoor : MonoBehaviour
{
    public SummonMonsterController _summonController;
    public Animator _animator;

    private float TimerSet = 2;
    private float Timer;
    private bool Open;
    private bool Close;

    public AudioClip OpenSound;
    public AudioClip CloseSound;

    private AudioSource OpenSource;
    private AudioSource CloseSource;

    // Start is called before the first frame update
    void Start()
    {
        Timer = TimerSet;

        SEController.inisializeAudioSource(ref OpenSource, OpenSound, this.transform);
        SEController.inisializeAudioSource(ref CloseSource, CloseSound, this.transform);
    }

    private void FixedUpdate()
    {
        if (GameEvent.PassRestRoom)
        {
            return;
        }
        if (_summonController.isAniPlay)
        {
            Timer -= Time.fixedDeltaTime;
            if (!Open)
            {
                _animator.SetBool("isOpen", true);
                OpenSource.Play();
                Open = true;
            }
            if (Timer <= (0.5f) && !Close)
            {
                _animator.SetBool("isOpen", false);
                CloseSource.Play();
                Close = true;
            }
            if (Timer <= 0)
            {
                Open = false;
                Close = false;
                Timer = TimerSet;
            }
        }
        else
        {
            Timer = TimerSet;
            Open = false;
            Close = false;
        }

        SEController.CalculateSystemSound(OpenSource, 0.7f);
        SEController.CalculateSystemSound(CloseSource, 0.7f);
    }
}
