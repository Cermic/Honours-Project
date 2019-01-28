using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Room : MonoBehaviour
{
    public GameObject gameObj;
    public GameObject leftSphere, rightSphere;
    Light lightLeft, lightRight;
    Vector3 roomCordinates;
    Quaternion roomAngle;

    public Room(Object gO, Material lightMaterial, LightConfig lc, Vector3 leftLightOffset, Vector3 rightLightOffset, Vector3 offSet, Quaternion angle, Vector3 scale, Transform parentTransform)
    {
        // If the first room
        if (parentTransform == null && scale == Vector3.zero)
        {
            gameObj = Instantiate(gO, offSet, angle) as GameObject;
        }
        else // All other rooms
        {
            gameObj = Instantiate(gO, parentTransform) as GameObject;
            gameObj.transform.localPosition = offSet;
            gameObj.transform.localRotation = angle;
            gameObj.transform.localScale = scale;
        }

        leftSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        leftSphere.GetComponent<Renderer>().material = lightMaterial;
        rightSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        rightSphere.GetComponent<Renderer>().material = lightMaterial;
        // Setup transforms and coords
        leftSphere.transform.parent = gameObj.transform;
        leftSphere.transform.localPosition = leftLightOffset;
        lightLeft = leftSphere.AddComponent<Light>();
        lightLeft.name = "Left Light";

        rightSphere.transform.parent = gameObj.transform;
        rightSphere.transform.localPosition = rightLightOffset;
        lightRight = rightSphere.AddComponent<Light>();
        lightRight.name = "Right Light";

        lightLeft.transform.parent = leftSphere.transform;
        lightRight.transform.parent = rightSphere.transform;

        lightLeft.color = new Color(lc.leftLightColour.x, lc.leftLightColour.y, lc.leftLightColour.z);
        lightLeft.intensity = lc.leftLightIntensity;

        lightRight.color = new Color(lc.rightLightColour.x, lc.rightLightColour.y, lc.rightLightColour.z);
        lightRight.intensity = lc.rightLightIntensity;
    }
}
