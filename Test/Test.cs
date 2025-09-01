using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Test:MonoBehaviour
{
    private float Timer = 1f;

    private void Start()
    {
        Invoke("aaa", 3);

        StartCoroutine("timer");
    }

    private void Update()
    {
        Timer -= Time.deltaTime;

        if (Timer <= 0)
        {
            this.enabled = false;
        }
    }

    private void aaa()
    {
        Debug.Log("aaa");
    }

    IEnumerator timer()
    {
        yield return new WaitForSeconds(3);

        Debug.Log("bbb");
    }
}