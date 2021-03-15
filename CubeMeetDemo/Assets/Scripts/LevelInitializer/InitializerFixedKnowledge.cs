using System.Collections.Generic;
using UnityEngine;

public static class InitializerFixedKnowledge
{
    public static Vector3 GetRotationForCubletFace(int faceIndex)
    {
        Vector3 rotation = Vector3.zero;

        switch (faceIndex)
        {
            case 0:
                rotation = new Vector3(0, 0, 90);
                break;
            case 1:
                rotation = new Vector3(0, 0, -90);
                break;
            case 2:
                rotation = new Vector3(-180, 0, 0);
                break;
            case 3:
                rotation = new Vector3(0, 0, 0);
                break;
            case 4:
                rotation = new Vector3(-90, 0, 0);
                break;
            case 5:
                rotation = new Vector3(90, 0, 0);
                break;
        }

        return rotation;
    }

    public static Vector3 GetNormalForCubletFace(int faceIndex)
    {
        Vector3 normal = Vector3.zero;

        switch (faceIndex)
        {
            case 0:
                normal = new Vector3(-1, 0, 0);
                break;
            case 1:
                normal = new Vector3(1, 0, 0);
                break;
            case 2:
                normal = new Vector3(0, -1, 0);
                break;
            case 3:
                normal = new Vector3(0, 1, 0);
                break;
            case 4:
                normal = new Vector3(0, 0, -1);
                break;
            case 5:
                normal = new Vector3(0, 0, 1);
                break;
        }

        return normal;
    }

    public static List<Vector3Int> GetPosiblePositionsForCornerElements(int dimensions)
    {
        List<Vector3Int> result = new List<Vector3Int>();

        result.Add(new Vector3Int(0, 0, 0));
        result.Add(new Vector3Int(dimensions-1, 0, 0));
        result.Add(new Vector3Int(dimensions-1, 0, dimensions-1));
        result.Add(new Vector3Int(0, dimensions-1, 0));
        result.Add(new Vector3Int(dimensions-1, dimensions-1, 0));
        result.Add(new Vector3Int(dimensions-1, dimensions-1, dimensions-1));
        result.Add(new Vector3Int(0, dimensions-1, dimensions-1));

        return result;
    }

    public static List<Vector3Int> GetPosiblePositionsForEdgeElements3x3()
    {
        List<Vector3Int> posiblePositionsForEdgeElements3x3 = new List<Vector3Int>();

        posiblePositionsForEdgeElements3x3.Add(new Vector3Int(0, 1, 0));
        posiblePositionsForEdgeElements3x3.Add(new Vector3Int(1, 0, 0));
        posiblePositionsForEdgeElements3x3.Add(new Vector3Int(2, 1, 0));
        posiblePositionsForEdgeElements3x3.Add(new Vector3Int(1, 2, 0));
        posiblePositionsForEdgeElements3x3.Add(new Vector3Int(2, 0, 1));
        posiblePositionsForEdgeElements3x3.Add(new Vector3Int(2, 1, 2));
        posiblePositionsForEdgeElements3x3.Add(new Vector3Int(0, 2, 1));
        posiblePositionsForEdgeElements3x3.Add(new Vector3Int(2, 2, 1));
        posiblePositionsForEdgeElements3x3.Add(new Vector3Int(1, 2, 2));

        return posiblePositionsForEdgeElements3x3;
    }

    public static List<Vector3Int> GetPosiblePositionsForEdgeElements4x4()
    {
        List<Vector3Int> posiblePositionsForEdgeElements4x4 = new List<Vector3Int>();

        posiblePositionsForEdgeElements4x4.Add(new Vector3Int(0, 2, 0));
        posiblePositionsForEdgeElements4x4.Add(new Vector3Int(0, 1, 0));
        posiblePositionsForEdgeElements4x4.Add(new Vector3Int(1, 0, 0));
        posiblePositionsForEdgeElements4x4.Add(new Vector3Int(2, 0, 0));
        posiblePositionsForEdgeElements4x4.Add(new Vector3Int(3, 1, 0));
        posiblePositionsForEdgeElements4x4.Add(new Vector3Int(3, 2, 0));
        posiblePositionsForEdgeElements4x4.Add(new Vector3Int(2, 3, 0));
        posiblePositionsForEdgeElements4x4.Add(new Vector3Int(1, 3, 0));
        posiblePositionsForEdgeElements4x4.Add(new Vector3Int(3, 0, 1));
        posiblePositionsForEdgeElements4x4.Add(new Vector3Int(3, 0, 2));
        posiblePositionsForEdgeElements4x4.Add(new Vector3Int(3, 1, 3));
        posiblePositionsForEdgeElements4x4.Add(new Vector3Int(3, 2, 3));
        posiblePositionsForEdgeElements4x4.Add(new Vector3Int(3, 3, 2));
        posiblePositionsForEdgeElements4x4.Add(new Vector3Int(3, 3, 1));
        posiblePositionsForEdgeElements4x4.Add(new Vector3Int(0, 3, 1));
        posiblePositionsForEdgeElements4x4.Add(new Vector3Int(0, 3, 2));
        posiblePositionsForEdgeElements4x4.Add(new Vector3Int(1, 3, 3));
        posiblePositionsForEdgeElements4x4.Add(new Vector3Int(2, 3, 3));

        return posiblePositionsForEdgeElements4x4;
    }
}
