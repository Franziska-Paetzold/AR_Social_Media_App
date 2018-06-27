using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Line {
    public Color Colour; // not using it as a property cause I want to see the val in the inspector
    public Vector3[] Positions;

    public Vector2[] PositionsForDatabase;


    public Line(Color colour, Vector3[] positions)
    {
        Colour = colour;
        Positions = positions;
        PositionsForDatabase = MyVector3Extension.toVector2(positions);

    }

}

public static class MyVector3Extension
{
    public static Vector2[] toVector2(this Vector3[] v3)
    {
        return System.Array.ConvertAll<Vector3, Vector2>(v3, getV3fromV2);
    }

    public static Vector2 getV3fromV2(Vector3 v3)
    {
        return new Vector2(v3.x, v3.y);
    }
}
