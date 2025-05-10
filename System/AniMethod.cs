using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AniMethod : MonoBehaviour
{
    public GameObject NewTutorialHint;
    public GameObject Flash;
    public GameObject TemporaryBlackScreen;
    public Animator TopBlackScreen;
    public Animator BottomBlackScreen;
    public Transform SpecialUIAni;
    private GameObject SkillPowerLose;
    private bool isFlashOpen;
    private bool RePlayFlash;

    private void Start()
    {
        Flash.GetComponent<ObjectDisappear>()._turnOff += OnTurnOffFlash;

        SkillPowerLose = SpecialUIAni.GetChild(0).gameObject;
    }

    private void Update()
    {
        if (RePlayFlash)
        {
            OpenFlash();
            RePlayFlash = false;
        }
    }

    public void ObjectHorizontalMove(float Distance, float RunTime, float DeltaTime, Transform _transform)
    {
        float Speed;
        Speed = Distance / RunTime;
        _transform.localPosition = new Vector3(_transform.localPosition.x + Speed * DeltaTime, _transform.localPosition.y, 0);
    }

    public void ObjectVerticalMove(float Distance, float RunTime, float DeltaTime, Transform _transform)
    {
        float Speed;
        Speed = Distance / RunTime;
        _transform.localPosition = new Vector3(_transform.localPosition.x, _transform.localPosition.y + Speed * DeltaTime, 0);
    }

    public void FollowTarget(Transform Target, Transform _transform)
    {
        _transform.localPosition = Target.localPosition;
    }

    public void FollowTarget(Vector3 Target, Transform _transform)
    {
        _transform.localPosition = Target;
    }
    //使用時方向一律正數
    public void ObjectParabolaMove(ParabolaVar _Var, float Distance,float Time, float DeltaTime, Transform _transform)
    {
        _Var.Speed = Distance / Time;
        Parabola.ParabolaMove(_Var, DeltaTime, _transform);
    }

    public void ObjectSlashMove(SlashResult Result, float RunTime, float DeltaTime, Transform _transform)
    {
        Result.Speed = Result.XDistance / RunTime;
        Slash.SlashMove(Result, DeltaTime, _transform);
    }

    public void ObjectSlashMove(float ProportionX, float ProportionY, float Speed, float DeltaTime, Transform _transfotm)
    {
        _transfotm.localPosition = new Vector3(_transfotm.localPosition.x + ProportionX * Speed * DeltaTime, _transfotm.localPosition.y + ProportionY * Speed * DeltaTime, 0);
    }

    public void OpenFlash()
    {
        if (isFlashOpen)
        {
            Flash.SetActive(false);
            isFlashOpen = false;
            RePlayFlash = true;
            return;
        }
        Flash.SetActive(true);
        isFlashOpen = true;
    }

    private void OnTurnOffFlash()
    {
        isFlashOpen = false;
    }

    public void TurnFace(Transform _transform, float Scale)
    {
        _transform.localScale = new Vector3(Scale, Mathf.Abs(Scale), 1);
    }

    public float CalculateDistance(Transform _transform1, Transform _transform2)
    {
        float Distance;
        Distance = Mathf.Abs(_transform1.localPosition.x - _transform2.localPosition.x);
        return Distance;
    }

    public void OpenTemporaryBlackScreen()
    {
        TemporaryBlackScreen.SetActive(true);
    }//專門給其他script用

    public void CloseTemporaryBlackScreen()
    {
        TemporaryBlackScreen.SetActive(false);
    }//專門給其他script用

    public void OpenBlackScreen()//專門給其他script用
    {
        TopBlackScreen.SetBool("Open", true);
        BottomBlackScreen.SetBool("Open", true);
    }

    public void CloseBlackScreen()//專門給其他script用
    {
        TopBlackScreen.SetBool("Open", false);
        BottomBlackScreen.SetBool("Open", false);
    }

    public void PlaySpecialAni(int Type)
    {
        switch (Type)
        {
            case 0:
                SkillPowerLose.SetActive(true);
                break;
        }
    }
}
