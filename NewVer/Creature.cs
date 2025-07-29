using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Creature
{
    public enum Face { Left, Right };
    public Face face;
    public int CampID;

    public void TurnFace()
    {
        face = face == Face.Right ? Face.Left : Face.Right;
    }
    public void TurnFace(Face f)
    {
        face = f;
    }
    public int GetCamp()
    {
        return CampID;
    }
}
