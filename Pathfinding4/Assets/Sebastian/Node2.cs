using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node2
{

    public bool walkable;
    public Vector3 worldPosition;

    public Node2(bool _walkable, Vector3 _worldPos)
    {
        walkable = _walkable;
        worldPosition = _worldPos;
    }
}
