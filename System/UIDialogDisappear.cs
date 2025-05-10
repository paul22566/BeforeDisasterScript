using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDialogDisappear : MonoBehaviour
{
    private Animator thisAni;
    private double DisappearTimeSet = 3.5f;
    private double DisappearTime;
    // Start is called before the first frame update
    void Start()
    {
        DisappearTime = DisappearTimeSet;
        thisAni = this.gameObject.GetComponent<Animator>();
        thisAni.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    // Update is called once per frame
    void Update()
    {
        timer();
    }

    void timer()
    {
        DisappearTime -= Time.unscaledDeltaTime;
        if (DisappearTime <= (DisappearTimeSet - 2.75))
        {
            thisAni.SetBool("Disappear", true);
            if (DisappearTime <= 0)
            {
                if (BackgroundSystem.isNoticeDialogAppear)
                {
                    BackgroundSystem.isNoticeDialogAppear = false;
                }
                this.gameObject.SetActive(false);
                DisappearTime = DisappearTimeSet;
            }
        }
    }
}
