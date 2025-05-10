using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionType : MonoBehaviour
{
    public enum Type { Meet, Skin, Metal, Cement, Stone, Carpet, Grass, Plastic};
    public Type _type;
    public bool BeAtkNoSound;//被攻擊時是否會有聲音
    public bool isChangeByPlayer;//是否會因為玩家有無踩到而變化

    [HideInInspector] public bool EntityCollision;

    private void Start()
    {
        switch (_type)
        {
            default:
                EntityCollision = true;
                break;
        }
    }
}
