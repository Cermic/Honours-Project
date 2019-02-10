using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public struct RoomProperties
{
    // Coordinate offset
    public Vector3 offset;
    // Standard room scale
    public Vector3 scale;
    public Object rObject;
    // Rotation Value
    public Quaternion orientation;
    public Transform parentTransform;
    public int roomIndex;
}