using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMonsterAtkFollow : MonoBehaviour
{
    private Transform target;
    // Start is called before the first frame update

    private void Awake()
    {
        target = transform.parent.parent.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            Vector3 followPos = new Vector3(target.position.x, target.position.y, this.transform.position.z);
            this.transform.position = followPos;
        }
    }
}
