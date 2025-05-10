using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdFloor1Controller : MonoBehaviour
{
    public GameObject DieMan;
    public GameObject BigMonster;
    public static bool canDoEvent = false;//script(firstGoInThirdFloor1)
    private float Timer = 4.1f;
    private PlayerData _PlayerData;
    public GameObject Door3F_2;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("FollowSystem") != null)
        {
            _PlayerData = GameObject.Find("FollowSystem").GetComponent<PlayerData>();
        }
        if (GameEvent.GoIn3F1)
        {
            BigMonster.SetActive(true);
            this.GetComponent<MusicJudgement>().Number = 2;
        }
        if (GameEvent.HasPassThirdFloor2)
        {
            Door3F_2.GetComponent<Portal>().ChangeBGM = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (canDoEvent)
        {
            DieMan.SetActive(true);
            BigMonster.SetActive(true);
            GameEvent.isAniPlay = true;
            canDoEvent = false;
        }
        if (GameEvent.isAniPlay)
        {
            Timer -= Time.deltaTime;
            if (Timer <= 0)
            {
                GameEvent.isAniPlay = false;
                GameEvent.GoIn3F1 = true;
                this.GetComponent<MusicJudgement>().Number = 2;
                MusicController.BeginFadeInBGM();
                _PlayerData.CommonSave();
            }
        }
    }
}
