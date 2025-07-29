using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test:MonoBehaviour
{
    public Transform a; 
    private Transform b; 
    private Transform c;

    public Transform d;
    private void Start()
    {
        b = a;
        a = d;
        c = b;

        Debug.Log(a);
        Debug.Log(b);
        Debug.Log(c);
    }

    public void aaaa(int a)
    {
        print(a);
    }
}