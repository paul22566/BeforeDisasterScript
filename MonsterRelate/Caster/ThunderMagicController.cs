using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderMagicController : MonoBehaviour
{
    public float AtkTimer;
    private float TotalTime;
    private GameObject Player;
    public GameObject Thunder;
    private bool isFollow = true;
    private bool isAtkAppear = false;
    // Start is called before the first frame update
    void Start()
    {
        TotalTime = AtkTimer;
        if (GameObject.Find("player") != null)
        {
            Player = GameObject.Find("player");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isFollow)
        {
            this.gameObject.transform.position = new Vector3(Player.gameObject.transform.position.x, Player.gameObject.transform.position.y + 2, Player.gameObject.transform.position.z);
        }

        AtkTimer -= Time.deltaTime;
        if (AtkTimer <= (TotalTime - 2))
        {
            isFollow = false;
            if(AtkTimer<= (TotalTime - 2.2))
            {
                if (!isAtkAppear)
                {
                    Instantiate(Thunder, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y - 2, this.gameObject.transform.position.z), Quaternion.identity);
                    isAtkAppear = true;
                }
                if(AtkTimer <= 0)
                {
                    Destroy(this.gameObject);
                }
            }
        }
    }
}
