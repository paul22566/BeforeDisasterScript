using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalStairDoor : MonoBehaviour
{
    public Animator Animation;
    private SpriteRenderer thisSpr;
    private double DestroyTimer;
    public double DestroyTimerSet;
    private bool timerSwitch;
    // Start is called before the first frame update
    void Start()
    {
        thisSpr = this.gameObject.GetComponent<SpriteRenderer>();
        if (GameEvent.FinalStairUnlock)
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameEvent.FinalStairUnlock)
        {
            Animation.SetBool("isOpen", true);
            thisSpr.sortingLayerName = "background";
            thisSpr.sortingOrder = 3;
            timerSwitch = true;
        }

        timer();
    }

    void timer()
    {
        if (timerSwitch)
        {
            DestroyTimer -= Time.deltaTime;
            if (DestroyTimer <= 0)
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            DestroyTimer = DestroyTimerSet;
        }
    }
}
