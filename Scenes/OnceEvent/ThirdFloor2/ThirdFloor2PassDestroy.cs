using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdFloor2PassDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (GameEvent.HasPassThirdFloor2)
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
