using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSetup : MonoBehaviour {

    // Defines the number of rooms attached to each room and how many layers deep the maze will go.
	private const int ROOMS_PER_BRANCH = 2;
    private const int MAZE_DEPTH = 4;
    // Material for the light sphere
	private Material emmisive_white;
    // Array of Room objects
    private Room[] roomSet;
    private GameObject lightSphereGameObj;
	private Light lightLeft, lightRight;
    // Light config enum that can be changed in editor
	public enum LightConfiguration
	{ONE, TWO, THREE, FOUR, FIVE, SIX}
	public LightConfiguration lightconfig;
    // Bool to be used to turn on and off the experiment state
    public bool isControl = false;
    // Coords and angle for the First Room.
    private Vector3 firstRoomCoords;
    private Quaternion firstRoomAngle;
    // left and right offsets for the first level of child rooms.
    private Vector3 initialLeftOffset, initialRightOffset;
    // left and right offsets for all sub child rooms
    private Vector3 leftOffset, rightOffset;
    // Standard room scale
    private Vector3 roomScale; 
    private Object firstRoomObject, roomObject;
    // Left and right rotation values
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
        // Load room objects
        firstRoomObject = Resources.Load("Prefabs/FirstRoom");
        roomObject = Resources.Load("Prefabs/Room");
        // Load sphere primitive to represent a light
        lightSphereGameObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        firstRoomCoords = new Vector3(0, 20, 0);
        firstRoomAngle = Quaternion.Euler(-90, 0, 0);
        initialLeftOffset = new Vector3(1.8f, 2.05f, -0.33f);
        initialRightOffset = new Vector3(-1.8f, 2.05f, 0.33f);
        leftOffset = new Vector3(0.85f, 1.5f, 0f);
        rightOffset = new Vector3(-0.85f, 1.5f, 0f);
        // Standard room scale - this is used to scale down the rooms as they adopt the scale of their parent (far too big)
        roomScale = new Vector3(1f, 1f, 1f);
        leftRotation = Quaternion.Euler(0, 0, -60);
        rightRotation = Quaternion.Euler(0, 0, 60);

        // Populate room array - This is done using the ROOMS_PER_BRANCH value to the power of MAZE_DEPTH -1. 
        // This ensures the maze is always the appropriate size.
        int mazeSize = (int)Mathf.Pow(ROOMS_PER_BRANCH, MAZE_DEPTH) - 1;
        roomSet = new Room[mazeSize];

        // Tags each room with the Left or Right tag as if it were in a binary tree.
        for (int j = 0; j < roomSet.Length; j++)
        {
            if (j == 0)
            {
                if (roomSet[j] == null) // If the first room is null construct it.
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
                    // Child rooms of the first two ---
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
            // Give the room the name room + it's array index.
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
