using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInController : MonoBehaviour
{
    public double FadeInTimeSet;
    private double FadeInTime;

    public bool HasLongFadeIn;
    private bool Open;

    public static bool ContinueCantPause = false;//�p�G�s�����Ȯɤ���Ȱ��A�N�ݭn�Ұʳo��
    // Start is called before the first frame update
    void Start()
    {
        FadeInTime = FadeInTimeSet;
        if (!HasLongFadeIn)
        {
            OpenFadeIn();
        }
    }

    private void FixedUpdate()
    {
        if (!Open)
        {
            return;
        }

        FadeInTime -= Time.fixedDeltaTime;
        if (FadeInTime <= 0)
        {
            if (!PauseMenuController.OpenAnyMenu && !ContinueCantPause)
            {
                BackgroundSystem.CantPause = false;
            }
            ContinueCantPause = false;
            this.gameObject.SetActive(false);
        }
    }

    public void OpenFadeIn()
    {
        Open = true;
        this.GetComponent<Animator>().SetBool("Disappear", true);
    }
}
