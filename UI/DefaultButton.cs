using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefaultButton : MonoBehaviour
{
    public Selectable onStart;//設定預設按鈕  其他Script會用到(keyCodeManage，PauseMenuController，TitleController)
    [HideInInspector] public bool ShouldOpen = true;//其他Script會用到(PauseMenuController，TitleController)

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
