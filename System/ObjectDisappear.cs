using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDisappear : MonoBehaviour
{
    public delegate void TurnOff();
    public event TurnOff _turnOff;

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
            this.gameObject.SetActive(false);
            DisappearTime = DisappearTimeSet;
            if (_turnOff != null)
            {
                _turnOff();
            }
        }
    }
}
