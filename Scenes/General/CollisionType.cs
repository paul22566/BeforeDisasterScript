using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionType : MonoBehaviour
{
    public enum Type { Meet, Skin, Metal, Cement, Stone, Carpet, Grass, Plastic};
    public Type _type;
    public bool BeAtkNoSound;//�Q�����ɬO�_�|���n��
    public bool isChangeByPlayer;//�O�_�|�]�����a���L�����ܤ�

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
