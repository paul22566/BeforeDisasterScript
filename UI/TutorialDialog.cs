using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class TutorialDialog : MonoBehaviour
{
    //�ܤ@��J
    public InteractableObject _interactable;
    public TouchTrigger _touchTrigger;

    [HideInInspector] public int Status = 0; //���ҰʩΤ��s�b:0 �ݱҰ�:1 �Ұʤ�:2 �w�Ұ�:3 ������:4 �ҰʹL:5
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
        //�ۨ��Ұʱ���
        if (TouchTriggerOpenJudge())
        {
            OnInteract();
        }
        //��������
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
        //�p�ɾ��p��
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
        //�����Ұ�
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
        }//�Ȥ��Ұ�
    }
}
