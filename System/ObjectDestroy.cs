using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroy : MonoBehaviour
{
    public double DisappearTimeSet;
    private double DisappearTime;
    // Start is called before the first frame update
    void Start()
    {
        DisappearTime = DisappearTimeSet;
    }

    // Update is called once per frame
    void Update()
    {
        timer();
    }

    void timer()
    {
        DisappearTime -= Time.deltaTime;
        if (DisappearTime <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
