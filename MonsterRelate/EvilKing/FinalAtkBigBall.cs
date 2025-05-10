using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalAtkBigBall : MonoBehaviour
{
    public float AtkSpeed;
    private bool AppearEnd;
    private float AppearTimer = 1f;
    private bool AtkFirstAppear;
    private bool isGround;
    public GameObject ShockWave;
    private Vector3 ShockWaveAppear = new Vector3(0, -2.37f, 0);
    private RaycastHit2D GroundCheck;
    private float AtkDestroyTimer;
    public float AtkDestroyTimerSet;
    // Start is called before the first frame update
    void Start()
    {
        AtkDestroyTimer = AtkDestroyTimerSet;
    }

    // Update is called once per frame
    void Update()
    {
        if (!AppearEnd)
        {
            AppearTimer -= Time.deltaTime;
            if (AppearTimer <= 0)
            {
                AppearEnd = true;
                this.transform.GetChild(0).gameObject.SetActive(true);
            }
        }
        else
        {
            //¦a­±°»´ú
            GroundCheck = Physics2D.Raycast(transform.position, -Vector2.up, 2.5f, 1024);
            if (GroundCheck && GroundCheck.collider.tag == "platform")
            {
                isGround = true;
            }
            else
            {
                isGround = false;
            }

            if (!isGround)
            {
                this.transform.position += new Vector3(0, -AtkSpeed * Time.deltaTime, 0);
            }
            if (isGround)
            {
                if (!AtkFirstAppear)
                {
                    Instantiate(ShockWave, this.transform.position + ShockWaveAppear, Quaternion.identity);
                    AtkFirstAppear = true;
                }
                AtkDestroyTimer -= Time.deltaTime;
                this.GetComponent<Animator>().SetBool("Disappear", true);
                if (AtkDestroyTimer <= 0)
                {
                    Destroy(this.gameObject);
                }
            }
        }
    }
}
