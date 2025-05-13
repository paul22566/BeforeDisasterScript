using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpeedTest : MonoBehaviour
{
    [SerializeField] private Vector3 BasePoint = new Vector3 (0, 0, 0);
    public Transform ObjectTransform;
    public float Speed;
    // Update is called once per frame
    void Update()
    {
        if (ObjectTransform == null)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            ObjectTransform.position = BasePoint;
        }
    }

    private void FixedUpdate()
    {
        if(ObjectTransform == null)
        {
            return;
        }
        ObjectTransform.localPosition = new Vector3(ObjectTransform.localPosition.x + Speed * Time.fixedDeltaTime, ObjectTransform.localPosition.y, 0);
    }
}
