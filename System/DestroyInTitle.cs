using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroyInTitle : MonoBehaviour
{
    void Update()
    {
        if(SceneManager.GetActiveScene().name == "LoadingTitle")
        {
            Destroy(this.gameObject);
        }
    }
}
