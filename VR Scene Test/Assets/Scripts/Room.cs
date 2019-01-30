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

    public Room(RoomProperties rp, LightProperties lp)
    {
        // If the first room 
        
        if (rp.parentTransform == null && rp.scale == Vector3.zero)
        {
            gameObj = Instantiate(rp.rObject, rp.offset, rp.orientation) as GameObject;
        }
        else // All other rooms
        {
            gameObj = Instantiate(rp.rObject, rp.parentTransform) as GameObject;
            gameObj.transform.localPosition = rp.offset;
            gameObj.transform.localRotation = rp.orientation;
            gameObj.transform.localScale = rp.scale;
        }

        leftSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        leftSphere.GetComponent<Renderer>().material = lp.lightMat;
        rightSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        rightSphere.GetComponent<Renderer>().material = lp.lightMat;
        // Setup transforms and coords
        leftSphere.transform.parent = gameObj.transform;
        leftSphere.transform.localPosition = lp.lightLeftOffset;
        lightLeft = leftSphere.AddComponent<Light>();
        lightLeft.name = "Left Light";

        rightSphere.transform.parent = gameObj.transform;
        rightSphere.transform.localPosition = lp.lightRightOffset;
        lightRight = rightSphere.AddComponent<Light>();
        lightRight.name = "Right Light";

        lightLeft.transform.parent = leftSphere.transform;
        lightRight.transform.parent = rightSphere.transform;

        lightLeft.color = new Color(lp.lightConfig.leftLightColour.x, lp.lightConfig.leftLightColour.y, lp.lightConfig.leftLightColour.z);
        lightLeft.intensity = lp.lightConfig.leftLightIntensity;

        lightRight.color = new Color(lp.lightConfig.rightLightColour.x, lp.lightConfig.rightLightColour.y, lp.lightConfig.rightLightColour.z);
        lightRight.intensity = lp.lightConfig.rightLightIntensity;
    }
}
