using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public struct LightProperties
{
    // left and right offsets for the first room's lights of child rooms.
    public Vector3 lightLeftOffset;
    public Vector3 lightRightOffset;
    public Material lightMat;
    public LightConfig lightConfig;
}