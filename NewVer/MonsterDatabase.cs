using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDatabase
{
    public MonsterData SwordManBasicData;

    void SetAllMonsterDefaultData()
    {
        //SwordManData = new MonsterData(50, 1);
    }
}

[System.Serializable]
public struct MonsterData
{
    public int Hp;
    public float NormalSpeed;
    public float CooldownTime;
    public float AniScale;
    public float AlertDistanceX;
    public float ValidDistanceY;//在此範圍內才會做出有效判斷
    public float GiveUpDistanceX;
    public float GiveUpDistanceY;
    public Monster.Face initialFace;
    public int CampID;
    public MonsterData(int hp, float speed, float cooldown, float scale, float aD , float vDY, float gDX, float gDY, Monster.Face initFace, int camp)
    {
        Hp = hp;
        NormalSpeed = speed;
        CooldownTime = cooldown;
        AniScale = scale;
        AlertDistanceX = aD;
        ValidDistanceY = vDY;
        GiveUpDistanceX = gDX;
        GiveUpDistanceY = gDY;
        initialFace = initFace;
        CampID = camp;
    }
}

[System.Serializable]
public struct SwordManData
{
    public GameObject Atk1_1;
    public GameObject Atk1_2;
    public GameObject StringAtk;

    public float PatrolTime;
    public float RunSpeed;
    public float ChasingDistance;
    public float AtkDistance;
    public float AtkSpeed;
    public float AtkTime;
    public float StringAtkDistance;
    public float StringAtkSpeed;
    public float StringAtkTime;
    public SwordManData(GameObject atk1_1, GameObject atk1_2, GameObject stringAtk,
        float patrolT, float speed, float chasing, float atkD, 
        float atkS, float atkTime, float SAtkS, float sAtkD, float stringAtkTime)
    {
        Atk1_1 = atk1_1;
        Atk1_2 = atk1_2;
        StringAtk = stringAtk;

        PatrolTime = patrolT;
        RunSpeed = speed;
        ChasingDistance = chasing;
        AtkDistance = atkD;
        AtkSpeed = atkS;
        AtkTime = atkTime;
        StringAtkDistance = sAtkD;
        StringAtkSpeed = SAtkS;
        StringAtkTime = stringAtkTime;
    }
}


