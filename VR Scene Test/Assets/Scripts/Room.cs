using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject gameObj;
    public GameObject leftSphere, rightSphere;
    public GameObject[] wayPoints;
    Light lightLeft, lightRight;

    public Room(RoomProperties rp, LightProperties lp)
    {
        // If the first room 
        
        if (rp.roomIndex == 0)
        {
            gameObj = Instantiate(rp.rObject, rp.offset, rp.orientation) as GameObject;
            wayPoints = new GameObject[7];
            // If the first room, add more waypoints.
        }
        else // All other rooms
        {
            gameObj = Instantiate(rp.rObject, rp.parentTransform) as GameObject;
            gameObj.transform.localPosition = rp.offset;
            gameObj.transform.localRotation = rp.orientation;
            gameObj.transform.localScale = rp.scale;
            wayPoints = new GameObject[3];
}
        // Give the gameObj the Room tag.
        gameObj.tag = rp.tag;
        gameObj.AddComponent<MeshCollider>();
        if (gameObj.tag == "END ROOM") // Check if this is the last level of rooms.
        {
            wayPoints[0] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wayPoints[0].transform.parent = gameObj.transform;
            wayPoints[0].name = "Final Waypoint";
            wayPoints[0].tag = "WAYPOINT";
            wayPoints[0].transform.localPosition = new Vector3(0f, 0.7f, -0.049f);
        }
        else
        {
            for (int i = 0; i < wayPoints.Length; i++)
            {
                wayPoints[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
                wayPoints[i].transform.parent = gameObj.transform;
                wayPoints[i].name = "Waypoint" + i.ToString();
                wayPoints[i].tag = "WAYPOINT";
            }
            wayPoints[0].transform.localPosition = new Vector3(0f, 0.7f, -0.049f);
            wayPoints[1].transform.localPosition = new Vector3(0.33f, 1.2f, -0.049f);
            wayPoints[2].transform.localPosition = new Vector3(-0.33f, 1.2f, -0.049f);
        }
        //for (int i = 0; i < wayPoints.Length; i++)
        //    {
        //        wayPoints[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //        wayPoints[i].transform.parent = gameObj.transform;
        //        wayPoints[i].name = "Waypoint" + i.ToString();
        //        wayPoints[i].tag = "WAYPOINT";
        //    }
        //    wayPoints[0].transform.localPosition = new Vector3(0f, 0.7f, -0.049f);
        //    wayPoints[1].transform.localPosition = new Vector3(0.33f, 1.2f, -0.049f);
        //    wayPoints[2].transform.localPosition = new Vector3(-0.33f, 1.2f, -0.049f);

            // Adjust the extra waypoints positions for the first room.
            if (rp.roomIndex == 0)
        {
            // Right hand side tunnel waypoints.
            wayPoints[3].transform.localPosition = new Vector3(-1.145f, 1.665f, 0.032f);
            wayPoints[3].transform.localRotation = Quaternion.Euler(60f, -90f, 73f);
            wayPoints[4].transform.localPosition = new Vector3(-1.84f, 2.06f, 0.275f);
            wayPoints[4].transform.localRotation = Quaternion.Euler(60f, -90f, 73f);
            // Left hand side tunnel waypoints.
            wayPoints[5].transform.localPosition = new Vector3(1.08f, 1.625f, -0.11f);
            wayPoints[5].transform.localRotation = Quaternion.Euler(120.4f, -90f, -107f);
            wayPoints[6].transform.localPosition = new Vector3(1.77f, 2.03f, -0.355f);
            wayPoints[6].transform.localRotation = Quaternion.Euler(120.4f, -90f, -107f);
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
    public bool FindChildWithTag(Transform parent, string tag)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.tag == tag)
            {
                return true;
            }
        }
        return false;
    }
}
