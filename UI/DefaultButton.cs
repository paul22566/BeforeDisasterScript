using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefaultButton : MonoBehaviour
{
    public Selectable onStart;//�]�w�w�]���s  ��LScript�|�Ψ�(keyCodeManage�APauseMenuController�ATitleController)
    [HideInInspector] public bool ShouldOpen = true;//��LScript�|�Ψ�(PauseMenuController�ATitleController)

    // Update is called once per frame
    void Update()
    {
        if (ShouldOpen)
        {
            SelectButtonController.SelectPointSet(onStart.GetComponent<UIButtonBeSelected>().HNumber, onStart.GetComponent<UIButtonBeSelected>().VNumber);
            ShouldOpen = false;
        }
    }
}
