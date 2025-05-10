using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstFloor2Fence : MonoBehaviour
{
    public GameObject EnterRange;
    public Animator FenceAni;

    public AudioClip FenceFallSound;
    private AudioSource FenceFallSource;

    // Start is called before the first frame update
    void Start()
    {
        if (GameEvent.Enter1F_2PipeLine)
        {
            FenceAni.SetBool("HasFall", true);
        }
        else
        {
            SEController.inisializeAudioSource(ref FenceFallSource, FenceFallSound, this.transform);
        }
    }

    private void Update()
    {
        if (GameEvent.Enter1F_2PipeLine)
        {
            return;
        }

        SEController.CalculateSystemSound(FenceFallSource);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "normalAtk")
        {
            FenceFall();
        }
        if (collision.tag == "CAtk")
        {
            FenceFall();
        }
    }

    private void FenceFall()
    {
        if (!GameEvent.Enter1F_2PipeLine)
        {
            FenceFallSource.Play();
            FenceAni.SetBool("Fall", true);
            EnterRange.SetActive(true);
        }
    }
}
