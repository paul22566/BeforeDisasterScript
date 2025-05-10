using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakePlace2F2Controller : MonoBehaviour
{
    private GameObject Player;
    public GameObject Camera;
    private bool isDoevent;
    private float Timer = 2.5f;
    private bool isOpenCamera = false;
    private PlayerData _PlayerData;
    private void Awake()
    {
        if (GameObject.Find("FollowSystem") != null)
        {
            _PlayerData = GameObject.Find("FollowSystem").GetComponent<PlayerData>();
        }
        if (GameObject.Find("player") != null)
        {
            Player = GameObject.Find("player");
            if (!GameEvent.GoIN2F2)
            {
                isDoevent = true;
                GameEvent.isAniPlay = true;
                GameObject g = GameObject.Find("0") as GameObject;
                if (g != null)
                {
                    Player.transform.position = g.transform.position;
                    Player.GetComponent<Rigidbody2D>().gravityScale = 7;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isDoevent)
        {
            Timer -= Time.deltaTime;
            if (Timer <= 1.5f)
            {
                if (!isOpenCamera)
                {
                    Camera.GetComponent<Shake>().Restart();
                    isOpenCamera = true;
                }
                if (Timer <= 0)
                {
                    isDoevent = false;
                    GameEvent.isAniPlay = false;
                    GameEvent.GoIN2F2 = true;
                    _PlayerData.CommonSave();
                }
            }
        }
    }
}
