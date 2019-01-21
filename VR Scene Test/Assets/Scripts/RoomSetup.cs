using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSetup : MonoBehaviour {

    // These define the number of rooms attached to each room and how many layers deep the maze will go.
	private const int ROOMS_PER_BRANCH = 2;
    private const int MAZE_DEPTH = 4;
	private Material emmisive_white;
    private Room[] roomSet;
	private GameObject[] rooms;
    private GameObject lightSphereGameObj;
	private Light lightLeft, lightRight;
	public enum LightConfiguration
	{ONE, TWO, THREE, FOUR, FIVE, SIX}
	public LightConfiguration lightconfig;
    // Vectors to add to the coords of the first level of rooms.
    private Vector3 firstRoomCoords;
    private Quaternion firstRoomAngle;
    private Vector3 initialLeftOffset, initialRightOffset;
    // Vectors to add to the coordinates of all other levels.
    private Vector3 leftOffset, rightOffset;
    private Vector3 roomScale; 
    private Object firstRoomObject, roomObject;
    private Quaternion leftRotation, rightRotation;

	/*
	Use text file to store light configuration values and then a series of configs eg.
	
	C1 0.5, 0.5 C2 1.0, 0.5 etc
	then for the config order
	C1, C3, C4, C2
	
	This allows the config file to be edited after the program is compiled.
	*/
	void Start () {
        // Load material for the light sphere
		emmisive_white = Resources.Load ("Materials/Emmisive_White") as Material;
        firstRoomObject = Resources.Load("Prefabs/FirstRoom");
        roomObject = Resources.Load("Prefabs/Room");
        lightSphereGameObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        firstRoomCoords = new Vector3(0, 20, 0);
        firstRoomAngle = Quaternion.Euler(-90, 0, 0);
        initialRightOffset = new Vector3(-1.8f, 2.05f, 0.33f);
        initialLeftOffset = new Vector3(1.8f, 2.05f, -0.33f);
        rightOffset = new Vector3(-0.85f, 1.5f, 0f);
        leftOffset = new Vector3(0.85f, 1.5f, 0f);
        roomScale = new Vector3(1f, 1f, 1f);
        leftRotation = Quaternion.Euler(0, 0, -60);
        rightRotation = Quaternion.Euler(0, 0, 60);
        // Populate room array
        int mazeSize = (int)Mathf.Pow(ROOMS_PER_BRANCH, MAZE_DEPTH) - 1;
        rooms = new GameObject[mazeSize];
        roomSet = new Room[mazeSize];

        //// Tags each room with the Left or Right tag as if it were in a binary tree.
        //for (int j = 0; j < rooms.Length; j++)
        //{
        //    if (j == 0)
        //    {
        //        if (rooms[j] == null)
        //        {
        //            rooms[j] = Instantiate(firstRoomObject, new Vector3(0, 20, 0), Quaternion.Euler(-90, 0, 0)) as GameObject;
        //        }
        //    }
        //    else
        //    {
        //        // Finds the parent index of the room based on it's position in the array and number of branching rooms.
        //        float indexFloat = j;
        //        int parentRoomIndex = Mathf.CeilToInt(indexFloat /= ROOMS_PER_BRANCH) - 1;
        //        if (j == 1) // First room on the right
        //        {
        //            rooms[j] = Instantiate(roomObject, rooms[parentRoomIndex].transform) as GameObject;
        //            rooms[j].transform.localScale = roomScale;
        //            rooms[j].transform.localPosition = initialRightOffset;
        //            rooms[j].transform.localRotation = rightRotation;
        //        }
        //        else if (j == 2) // First room on the left
        //        {
        //            // Room instantiated with prefab
        //            rooms[j] = Instantiate(roomObject, rooms[parentRoomIndex].transform) as GameObject;
        //            rooms[j].transform.localScale = roomScale;
        //            rooms[j].transform.localPosition = initialLeftOffset;
        //            rooms[j].transform.localRotation = leftRotation;
        //        }
        //        else
        //        {
        //            if (j % 2 == 0) // If the index is even, the room is on the left
        //            {
        //                rooms[j] = Instantiate(roomObject, rooms[parentRoomIndex].transform) as GameObject;
        //                rooms[j].transform.localScale = roomScale;
        //                rooms[j].transform.localPosition = leftOffset;
        //                rooms[j].transform.localRotation = leftRotation;
        //            }
        //            else // If the index is odd, the room is on the right
        //            {
        //                rooms[j] = Instantiate(roomObject, rooms[parentRoomIndex].transform) as GameObject;
        //                rooms[j].transform.localScale = roomScale;
        //                rooms[j].transform.localPosition = rightOffset;
        //                rooms[j].transform.localRotation = rightRotation;
        //            }
        //        }
        //    }
        //    rooms[j].name = "Room" + j.ToString();
        //}

        // Tags each room with the Left or Right tag as if it were in a binary tree.
        for (int j = 0; j < roomSet.Length; j++)
        {
            if (j == 0)
            {
                if (roomSet[j] == null)
                {
                    roomSet[j] = new Room(firstRoomObject, lightSphereGameObj, lightSphereGameObj, lightLeft, lightRight, firstRoomCoords, firstRoomAngle);
                }
            }
            else
            {
                // Finds the parent index of the room based on it's position in the array and number of branching rooms.
                float indexFloat = j;
                int parentRoomIndex = Mathf.CeilToInt(indexFloat /= ROOMS_PER_BRANCH) - 1;
                if (j == 1) // First room on the right
                {
                    roomSet[j] = new Room(roomObject, lightSphereGameObj, lightSphereGameObj, lightLeft, lightRight, initialRightOffset, rightRotation, roomScale, roomSet[parentRoomIndex].gameObj.transform);
                }
                else if (j == 2) // First room on the left
                {
                    roomSet[j] = new Room(roomObject, lightSphereGameObj, lightSphereGameObj, lightLeft, lightRight, initialLeftOffset, leftRotation, roomScale, roomSet[parentRoomIndex].gameObj.transform);
                }
                else
                {
                    if (j % 2 == 0) // If the index is even, the room is on the left
                    {
                        roomSet[j] = new Room(roomObject, lightSphereGameObj, lightSphereGameObj, lightLeft, lightRight, leftOffset, leftRotation, roomScale, roomSet[parentRoomIndex].gameObj.transform);
                    }
                    else // If the index is odd, the room is on the right
                    {
                        roomSet[j] = new Room(roomObject, lightSphereGameObj, lightSphereGameObj, lightLeft, lightRight, rightOffset, rightRotation, roomScale, roomSet[parentRoomIndex].gameObj.transform);
                    }
                }
            }
            roomSet[j].gameObj.name = "Room" + j.ToString();
        }

        // Setup Light 1
        //lightSphere1 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //lightSphere1.GetComponent<Renderer> ().material = emmisive_white;
        //lightSphere1.transform.parent = room1.transform;
        //lightSphere1.name = "Light1";
        //light1 = lightSphere1.AddComponent<Light> ();

        //// Setup Light 2
        //lightSphere2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //lightSphere2.GetComponent<Renderer> ().material = emmisive_white;
        //lightSphere2.transform.parent = room1.transform;
        //lightSphere2.name = "Light2";
        //light2 = lightSphere2.AddComponent<Light> ();

        //light1.color = Color.blue;
        //lightSphere1.transform.localPosition = new Vector3 (0, 10, 0);
        //lightSphere2.transform.localPosition = new Vector3 (0, 0, 0);
    }

    void SetupLights(LightConfiguration lightconfig)
	{
		
	}
}
