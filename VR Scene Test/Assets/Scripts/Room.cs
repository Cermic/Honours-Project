using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject gameObj;
    public GameObject leftLightSphere, rightLightSphere;
    Light lightLeft, lightRight;
    Vector3 roomCordinates;
    Quaternion roomAngle;

    public Room(Object gO, GameObject leftSphere, GameObject rightSphere, Light left, Light right, Vector3 coords, Quaternion angle)
    {
            gameObj = Instantiate(gO, coords, angle) as GameObject;
            leftSphere = leftLightSphere;
            rightSphere = rightLightSphere;
            left = lightLeft;
            right = lightRight;
    }

    public Room(Object gO, GameObject leftSphere, GameObject rightSphere, Light left, Light right, Vector3 offSet, Quaternion angle, Vector3 scale, Transform parentTransform)
    {
        gameObj = Instantiate(gO, parentTransform) as GameObject;
        leftSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        rightSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        left = lightLeft;
        right = lightRight;
        gameObj.transform.localPosition = offSet;
        gameObj.transform.localRotation = angle;
        gameObj.transform.localScale = scale;
    }

    // Create the Sphere internally here, then attatch a light to it and apply the appropriate coords and light configs from
    // Passed parameters?
}
