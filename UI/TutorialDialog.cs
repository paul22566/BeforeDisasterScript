using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class TutorialDialog : MonoBehaviour
{
    //擇一填入
    public InteractableObject _interactable;
    public TouchTrigger _touchTrigger;

    [HideInInspector] public int Status = 0; //未啟動或不存在:0 待啟動:1 啟動中:2 已啟動:3 關閉中:4 啟動過:5
    public TutorialDialog FrontDialog;
    public TutorialDialog BackDialog;
    [SerializeField] private Animator TextAni;
    [SerializeField] private Animator ImageAni;
    private float AppearTimer;
    private float AppearTimerSet = 0.75f;
    private float WaitTimer;
    private float WaitTimerSet = 2;
    private float DisappearTimer;
    private float DisappearTimerSet  = 0.75f;

    private float _deltaTime;
    // Start is called before the first frame update
    void Start()
    {
        AppearTimer = AppearTimerSet;
        WaitTimer = WaitTimerSet;
        DisappearTimer = DisappearTimerSet;

        if (_interactable != null)
        {
            _interactable._interact += OnInteract;
        }
    }

    // Update is called once per frame
    void Update()
    {
        _deltaTime = Time.deltaTime;
        //自身啟動條件
        if (TouchTriggerOpenJudge())
        {
            OnInteract();
        }
        //提早關閉
        if(BackDialog != null && BackDialog.Status == 1)
        {
            if(Status == 3)
            {
                Status = 4;
                TextAni.SetBool("Disappear", true);
                if (ImageAni != null)
                {
                    ImageAni.SetBool("Disappear", true);
                }
            }
        }
        //計時器計算
        if (Status == 2)
        {
            AppearTimer -= _deltaTime;
            if(AppearTimer<=0)
            {
                Status = 3;
            }
        }
        if (Status == 3)
        { 
            WaitTimer -= _deltaTime;
            if (WaitTimer <= 0)
            {
                Status = 4;
                TextAni.SetBool("Disappear", true);
                if (ImageAni != null)
                {
                    ImageAni.SetBool("Disappear", true);
                }
            }
        }
        if (Status == 4)
        {
            DisappearTimer -= _deltaTime;
            if (DisappearTimer <= 0)
            {
                Status = 5;
            }
        }
    }

    private bool TouchTriggerOpenJudge()
    {
        if (_touchTrigger != null)
        {
            if (_touchTrigger.isTouch)
            {
                return true;
            }
        }

        return false;
    }

    private void OnInteract()
    {
        //直接啟動
        if (FrontDialog == null || FrontDialog.Status == 5)
        {
            if (Status == 0 || Status == 1)
            {
                TextAni.SetBool("Appear", true);
                if (ImageAni != null)
                {
                    ImageAni.SetBool("Appear", true);
                }
                Status = 2;
            }
        }
        else
        {
            Status = 1;
        }//暫不啟動
    }
}
