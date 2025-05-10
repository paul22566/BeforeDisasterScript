using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowController : MonoBehaviour
{
    private GameObject Window1;
    private GameObject Window2;
    // Start is called before the first frame update
    private void Awake()
    {
        Window1 = this.gameObject.transform.GetChild(0).gameObject;
        Window2 = this.gameObject.transform.GetChild(1).gameObject;
    }
    void Start()
    {
        if (GameEvent.PassBoss1)
        {
            Window1.SetActive(false);
            Window2.SetActive(true);
        }
    }
}
