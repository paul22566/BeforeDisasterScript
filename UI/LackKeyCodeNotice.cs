using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LackKeyCodeNotice : MonoBehaviour
{
    private KeyCodeManage _keyCodeManage;

    private void Start()
    {
        _keyCodeManage = this.transform.parent.parent.gameObject.GetComponent<KeyCodeManage>();
    }
    void Update()
    {
        if (!_keyCodeManage.isAlert)
        {
            this.gameObject.SetActive(false);
        }
    }
}
