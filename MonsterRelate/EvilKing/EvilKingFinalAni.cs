using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilKingFinalAni : MonoBehaviour
{
    private enum Phase { one,two,three,four,five,six,seven,eight,nine };
    private Phase phase;

    private float Timer1 = 3f;
    private float Timer2 = 3f;

    private float JumpSpeed = 0.05f;
    private bool JumpToRight;
    private float ParabolaX;
    private float ParabolaConstant;
    private float ParabolaOriginalPointX;
    private float ParabolaOriginalPointY;
    private bool isParabolaCaculate;
    private bool AniBool1;
    private bool AniBool2;

    public GameObject Sword;
    public GameObject Explosion;
    public GameObject BigSwordDoor;
    public GameObject BigSword;
    private GameObject BigSwordDoorRecord;
    private GameObject BigSwordRecord;
    public GameObject Ani2;
    // Start is called before the first frame update
    void Start()
    {
        phase = Phase.one;
    }

    // Update is called once per frame
    void Update()
    {
        if (Boss3Controller.PlayerShoot && phase == Phase.one)
        {
            this.GetComponent<Animator>().SetBool("isHurted", true);
            JumpToRight = true;
            if (!isParabolaCaculate)
            {
                JumpSpeed = 0.2f;
                ParabolaOriginalPointX = 22.89f;
                ParabolaOriginalPointY = -0.7f;
                ParabolaX = 22.89f;
                ParabolaConstant = (30.5f - 22.89f) * (30.5f - 22.89f) / 4 / (-8 + 0.7f);
                isParabolaCaculate = true;
            }
            if(this.transform.position.x >= 30.5f)
            {
                JumpToRight = false;
                this.transform.position = new Vector3(30.5f, -8f, 0);
                this.GetComponent<Animator>().SetBool("stop", true);
                phase = Phase.two;
                isParabolaCaculate = false;
            }
        }
        if (phase == Phase.two && Boss3Controller.PlayerSecondJump)
        {
            this.GetComponent<Animator>().SetBool("Throw", true);
            Timer1 -= Time.deltaTime;
            if (Timer1 <= (3 - 0.2f))
            {
                if (!AniBool1)
                {
                    Instantiate(Sword, new Vector3(29.86f, -7.37f, 0), Sword.transform.rotation);
                    AniBool1 = true;
                }
                if(Timer1 <= (3-2))
                {
                    if (!AniBool2)
                    {
                        Instantiate(Explosion, new Vector3(29.61f, -7.06f, 0), Quaternion.identity);
                        AniBool2 = true;
                    }
                    if (Timer1 <= (3 - 2.05))
                    {
                        Boss3Controller.EvilKingAtkAni = true;
                    }
                    if (Timer1<=0)
                    {
                        Boss3Controller.EvilKingPrepare = true;
                        AniBool1 = false;
                        AniBool2 = false;
                        phase = Phase.three;
                    }
                }
            }
        }
        if (phase == Phase.three && Boss3Controller.PlayerPrepare && Boss3Controller.EvilKingPrepare)
        {
            this.GetComponent<Animator>().SetBool("Prepare", true);
            Timer2 -= Time.deltaTime;
            if (Timer2 <= (3 - 0.1))
            {
                if (!AniBool1)
                {
                    Instantiate(BigSwordDoor, new Vector3(29.2f, -4.12f, 0), BigSwordDoor.transform.rotation, this.transform);
                    BigSwordDoorRecord = this.transform.GetChild(0).gameObject;
                    this.transform.DetachChildren();
                    AniBool1 = true;
                }
                if (Timer2 <= (3 - 0.6))
                {
                    if (!AniBool2)
                    {
                        Instantiate(BigSword, new Vector3(29.16f, -4.07f, 0), BigSword.transform.rotation, this.transform);
                        BigSwordRecord = this.transform.GetChild(0).gameObject;
                        this.transform.DetachChildren();
                        AniBool2 = true;
                    }
                    if (Timer2 <= (3 - 2.2) && BigSwordDoorRecord!=null)
                    {
                        BigSwordDoorRecord.GetComponent<Animator>().SetBool("Disappear", true);
                        if (Timer2 <= (3 - 2.7))
                        {
                            Destroy(BigSwordDoorRecord);
                        }
                    }
                    if (Boss3Controller.BeginToSlowSpeed)
                    {
                        Instantiate(Ani2, this.transform.position, Quaternion.identity);
                        Destroy(BigSwordRecord);
                        Destroy(this.gameObject);
                    }
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (JumpToRight)
        {
            this.transform.position = new Vector3(ParabolaX, (ParabolaX - ParabolaOriginalPointX) * (ParabolaX - ParabolaOriginalPointX) / 4 / ParabolaConstant + ParabolaOriginalPointY, 0);
            ParabolaX += JumpSpeed;
        }
    }
}
