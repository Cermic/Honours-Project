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
        }
        else // All other rooms
        {
            gameObj = Instantiate(rp.rObject, rp.parentTransform) as GameObject;
            gameObj.transform.localPosition = rp.offset;
            gameObj.transform.localRotation = rp.orientation;
            gameObj.transform.localScale = rp.scale;
        }
        // Give the gameObj the Room tag.
        gameObj.tag = rp.tag;
        if (gameObj.tag == "FIRST ROOM")
        {
            wayPoints = new GameObject[9];
            // If the first room, add more waypoints.
        }
        else if (gameObj.tag == "ROOM")// for end rooms that only need 1 waypoint
        {
            wayPoints = new GameObject[3];
        }
        else if (gameObj.tag == "END ROOM")
        {
            wayPoints = new GameObject[1];
        }
        gameObj.AddComponent<MeshCollider>();
        if (gameObj.tag == "END ROOM") // Check if this is the last level of rooms.
        {
            gameObj.AddComponent<BoxCollider>().isTrigger = true;
            wayPoints[0] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            wayPoints[0].transform.parent = gameObj.transform;
            wayPoints[0].name = "Final Waypoint";
            wayPoints[0].tag = "WAYPOINT";
            wayPoints[0].transform.localPosition = new Vector3(0f, 1f, -15f);
            wayPoints[0].transform.localScale = new Vector3(150f, 100f, 150f);
        }
        else
        {
            for (int i = 0; i < wayPoints.Length; i++)
            {
                wayPoints[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                wayPoints[i].transform.parent = gameObj.transform;
                wayPoints[i].name = "Waypoint" + i.ToString();
                wayPoints[i].tag = "WAYPOINT";
            }
            wayPoints[0].transform.localPosition = new Vector3(0f, 0.7f, -0.01f);
            wayPoints[0].transform.localScale = new Vector3(0.15f, 0.1f, 0.15f);
            wayPoints[1].transform.localPosition = new Vector3(0.33f, 1.2f, -0.01f);
            wayPoints[1].transform.localScale = new Vector3(0.15f, 0.1f, 0.15f);
            wayPoints[2].transform.localPosition = new Vector3(-0.33f, 1.2f, -0.01f);
            wayPoints[2].transform.localScale = new Vector3(0.15f, 0.1f, 0.15f);
        }
            // Adjust the extra waypoints positions for the first room.
            if (rp.roomIndex == 0)
        {
            // Right hand side tunnel waypoints.
            wayPoints[3].transform.localPosition = new Vector3(-1.145f, 1.665f, 0.02f);
            wayPoints[4].transform.localPosition = new Vector3(-1.84f, 2.06f, 0.3f);
            wayPoints[5].transform.localPosition = new Vector3(-2.05f, 2.18f, 0.32f);
            // Left hand side tunnel waypoints.
            wayPoints[6].transform.localPosition = new Vector3(0.75f, 1.43f, -0.02f);
            wayPoints[7].transform.localPosition = new Vector3(1.04f, 1.6f, -0.06f);
            wayPoints[8].transform.localPosition = new Vector3(1.77f, 2.03f, -0.32f);
            // wayPoints[8].transform.localRotation = Quaternion.Euler(120.4f, -90f, -107f);
            foreach (GameObject wayP in wayPoints)
            {
                wayP.transform.localScale = new Vector3(0.15f, 0.1f, 0.15f);
            }
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
