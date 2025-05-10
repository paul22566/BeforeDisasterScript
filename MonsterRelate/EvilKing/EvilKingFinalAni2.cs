using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilKingFinalAni2 : MonoBehaviour
{
    private enum Phase { one, two, three, four, five, six, seven, eight, nine };
    private Phase phase;

    private float Timer3 = 6.5f;

    private float JumpSpeed = 0.05f;
    private bool JumpToRight;
    private float ParabolaX;
    private float ParabolaConstant;
    private float ParabolaOriginalPointX;
    private float ParabolaOriginalPointY;
    private bool isParabolaCaculate;
    // Start is called before the first frame update
    void Start()
    {
        phase = Phase.one;
    }

    // Update is called once per frame
    void Update()
    {
        if (phase == Phase.one)
        {
            JumpToRight = true;
            if (!isParabolaCaculate)
            {
                ParabolaOriginalPointX = 30.5f;
                ParabolaOriginalPointY = -8;
                ParabolaX = 30.5f;
                JumpSpeed = 0.04f;
                ParabolaConstant = (31.27f - 30.5f) * (31.27f - 30.5f) / 4 / (-10.3f + 8);
                isParabolaCaculate = true;
            }
            if (this.transform.position.x >= 31.27f)
            {
                this.GetComponent<Animator>().SetBool("isGround", true);
                JumpToRight = false;
                isParabolaCaculate = false;
                this.transform.position = new Vector3(31.27f, -10.3f, 0);
                phase = Phase.two;
            }
        }
        if (phase == Phase.two)
        {
            Timer3 -= Time.deltaTime;
            if (Timer3 <= 0)
            {
                Boss3Controller.EvilKingLeave = true;
                Destroy(this.gameObject);
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
