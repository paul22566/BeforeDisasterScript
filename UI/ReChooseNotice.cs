using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReChooseNotice : MonoBehaviour
{
    private Animator thisAni;
    public double DisappearTimeSet;
    private double DisappearTime;
    private KeyCodeManage _keyCodeMange;
    private float _deltaTime;
    // Start is called before the first frame update
    void Start()
    {
        _keyCodeMange = this.transform.parent.parent.GetComponent<KeyCodeManage>();
        DisappearTime = DisappearTimeSet;
        thisAni = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        timer();
    }

    void timer()
    {
        _deltaTime = Time.unscaledDeltaTime * BackgroundSystem.BasicGameSpeed;
        DisappearTime -= _deltaTime;
        if (DisappearTime <= (DisappearTimeSet - 2.75))
        {
            thisAni.SetBool("Disappear", true);
            if (DisappearTime <= 0)
            {
                this.gameObject.SetActive(false);
                _keyCodeMange.isReChooseNoticeAppear = false;
                DisappearTime = DisappearTimeSet;
            }
        }
    }
}
