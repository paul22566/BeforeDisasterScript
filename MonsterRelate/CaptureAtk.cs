using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CaptureAtk : MonoBehaviour
{
    public float TimerSet;
    private float Timer;
    [HideInInspector] public MonsterCaptureController _captureController;
    [HideInInspector] public MonsterBasicData _basicData;

    private void Awake()
    {
        _captureController = transform.parent.parent.GetComponent<MonsterCaptureController>();
        _basicData = transform.parent.parent.GetComponent<MonsterBasicData>();
    }
    // Start is called before the first frame update
    void Start()
    {
        Timer = TimerSet;
    }

    // Update is called once per frame
    void Update()
    {
        Timer -= Time.deltaTime;
        if (Timer <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            _captureController.isCaptureSuccess = true;
            _captureController.isPlayerFollow = true;
        }
    }
}
