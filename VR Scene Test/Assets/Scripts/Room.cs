using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room 
{
     public GameObject gameObj;
     public string name;
     Light lightLeft, lightRight;

    public Room(GameObject gO, string n, Light left, Light right)
    {
        gameObj = gO;
        name = n;
        left = lightLeft;
        right = lightRight;
    }
}
