using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnceTimeHiddenWall : MonoBehaviour
{
    private Animator Ani;
    private bool DoEvent;
    private float Timer;
    public float TimerSet;

    public GameObject DiscoverSound;
    // Start is called before the first frame update
    void Start()
    {
        Timer = TimerSet;
        Ani = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (DoEvent)
        {
            Ani.SetBool("Disappear", true);
            Timer -= Time.deltaTime;
            if (Timer <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void BeginDisappear()
    {
        DoEvent = true;
        if (DiscoverSound != null)
        {
            Instantiate(DiscoverSound, transform.position, Quaternion.identity);
        }
    }
}
