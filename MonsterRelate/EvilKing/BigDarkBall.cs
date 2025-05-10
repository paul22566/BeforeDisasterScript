using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BigDarkBall : MonoBehaviour
{
    public float FloatChangeSpeed;
    public float Speed;
    public float AtkSpeed;
    public int AtkNumberSet;
    private float ParabolaX = -10;
    private float ParabolaY;
    private float ParabolaConstant = 50;
    private float FloatYCenter;
    private int AtkNumber;
    private bool isAtk;
    private bool AtkFirstAppear;
    private bool isGround;
    private bool FloatTimerSwitch = true;
    private float Timer;
    public float TimerSet;
    private bool TimerSwitch = true;
    private float AtkTimer;
    public float AtkTimerSet;
    private bool AtkTimerSwitch;
    private float AtkEndTimer;
    public float AtkEndTimerSet;
    private bool AtkEndTimerSwitch;
    private float AtkDestroyTimer;
    public float AtkDestroyTimerSet;
    private RaycastHit2D GroundCheck;
    private GameObject Player;
    public GameObject ShockWave;
    private Vector3 ShockWaveAppear = new Vector3(0, -2.37f, 0);
    private float Distance;
    public enum FloatStatus { Up, Down };
    private FloatStatus floatStatus;
    private bool AppearEnd;
    private float AppearTimer = 1f;
    public static bool isAtkEnd;//Script(EvilKingController)
    // Start is called before the first frame update
    void Start()
    {
        FloatYCenter = this.transform.position.y;
        Timer = TimerSet;
        AtkEndTimer = AtkEndTimerSet;
        AtkTimer = AtkTimerSet;
        Player = GameObject.Find("player");
        floatStatus = FloatStatus.Down;
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
                TimerSwitch = true;
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

            Distance = this.transform.position.x - Player.transform.position.x;

            TimerMethod();
            AtkTimerMethod();
        }
    }

    private void FixedUpdate()
    {
        Distance = this.transform.position.x - Player.transform.position.x;
        if (!isAtk)
        {
            if (Distance < -0.2)
            {
                this.transform.position += new Vector3(Speed * Time.fixedDeltaTime, 0, 0);
            }
            if (Distance > 0.2)
            {
                this.transform.position += new Vector3(-Speed * Time.fixedDeltaTime, 0, 0);
            }
        }
        if (FloatTimerSwitch)
        {
            switch (floatStatus)
            {
                case FloatStatus.Up:
                    ParabolaConstant = Mathf.Abs(ParabolaConstant) * -1;
                    ParabolaY = (ParabolaX * ParabolaX / (ParabolaConstant * 4)) + 0.5f;
                    ParabolaX += FloatChangeSpeed;
                    this.transform.position = new Vector3(this.transform.position.x, FloatYCenter + ParabolaY, 0);
                    if (ParabolaX >= 10)
                    {
                        ParabolaX = -10;
                        floatStatus = FloatStatus.Down;
                    }
                    break;
                case FloatStatus.Down:
                    ParabolaConstant = Mathf.Abs(ParabolaConstant);
                    ParabolaY = (ParabolaX * ParabolaX / (ParabolaConstant * 4)) - 0.5f;
                    ParabolaX += FloatChangeSpeed;
                    this.transform.position = new Vector3(this.transform.position.x, FloatYCenter + ParabolaY, 0);
                    if (ParabolaX >= 10)
                    {
                        ParabolaX = -10;
                        floatStatus = FloatStatus.Up;
                    }
                    break;
            }
        }
    }

    void TimerMethod()
    {
        if (TimerSwitch)
        {
            Timer -= Time.deltaTime;
            if (Timer <= 0 && Mathf.Abs(Distance) <= 1)
            {
                isAtk = true;
                TimerSwitch = false;
                AtkTimerSwitch = true;
                FloatTimerSwitch = false;
                Timer = TimerSet;
                AtkNumber += 1;
            }
        }
    }

    void AtkTimerMethod()
    {
        if (AtkTimerSwitch)
        {
            AtkTimer -= Time.deltaTime;
            if (AtkTimer <= (AtkTimerSet - 0.5))
            {
                if (!isGround)
                {
                    this.transform.position += new Vector3(0, -AtkSpeed * Time.deltaTime, 0);
                }
            }
            if (isGround)
            {
                if (!AtkFirstAppear)
                {
                    Instantiate(ShockWave, this.transform.position + ShockWaveAppear, Quaternion.identity);
                    AtkFirstAppear = true;
                }
                if (AtkNumber == AtkNumberSet)
                {
                    AtkDestroyTimer -= Time.deltaTime;
                    this.GetComponent<Animator>().SetBool("Disappear", true);
                    if (AtkDestroyTimer <= 0)
                    {
                        isAtkEnd = true;
                        Destroy(this.gameObject);
                    }
                }
                else
                {
                    AtkTimer = AtkTimerSet;
                    AtkTimerSwitch = false;
                    AtkEndTimerSwitch = true;
                }
            }
        }
        if (AtkEndTimerSwitch)
        {
            AtkEndTimer -= Time.deltaTime;
            if (AtkEndTimer <= (AtkEndTimerSet - 0.5))
            {
                if (this.transform.position.y < FloatYCenter)
                {
                    this.transform.position += new Vector3(0, (AtkSpeed / 3f) * Time.deltaTime, 0);
                }
                else
                {
                    this.transform.position = new Vector3(this.transform.position.x, FloatYCenter, 0);
                    isAtk = false;
                    AtkFirstAppear = false;
                    AtkEndTimerSwitch = false;
                    TimerSwitch = true;
                    FloatTimerSwitch = true;
                    AtkEndTimer = AtkEndTimerSet;
                    floatStatus = FloatStatus.Up;
                }
            }
        }
    }
}
