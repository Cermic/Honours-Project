using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSetup : MonoBehaviour {

    // These define the number of rooms attached to each room and how many layers deep the maze will go.
	private const int ROOMS_PER_BRANCH = 2;
    private const int MAZE_DEPTH = 3;

	private Material emmisive_white;
    private GameObject firstRoom;
	private GameObject[] rooms;
    private LinkedList<GameObject> roomsList;
	private GameObject lightSphere1, lightSphere2;
	private Light light1, light2;
	public enum LightConfiguration
	{ONE, TWO, THREE, FOUR, FIVE, SIX}
	public LightConfiguration lightconfig;

	void Start () {
        // Load material for the light sphere
		emmisive_white = Resources.Load ("Materials/Emmisive_White") as Material;
        // Populate room array
        rooms = new GameObject[ROOMS_PER_BRANCH * MAZE_DEPTH + 1];

        // Populates the rooms with GameObjects accoring to the Maze Depth and Rooms per branch.
        for (int i = 0; i < MAZE_DEPTH; i++)
        {
            for (int k = 0; k < ROOMS_PER_BRANCH + 1; k++)
            {
                //Instatiate the first room.
                if (i == 0)
                {
                    if (rooms[i] == null)
                    { rooms[i] = Instantiate(Resources.Load("Prefabs/FirstRoom"), new Vector3(0, 20, 0), Quaternion.Euler(-90, 0, 0)) as GameObject; }
                }
                // Instantiate the remaining levels.
                if (rooms[i * ROOMS_PER_BRANCH + k] == null)
                { rooms[i * ROOMS_PER_BRANCH + k] = Instantiate(Resources.Load("Prefabs/Room")) as GameObject; }
            }
        }

        // Tags each room with the Left or Right tag as if it were in a binary tree.
        for (int j = 0; j < rooms.Length; j++)
        {
            if (j == 0)
            {
                rooms[j].tag = "Start";
            }
            // If j is even, add the Left Room tag, else add Right Tag
            else if (j % 2 == 0)
            {
                rooms[j].tag = "Right";
            }
            else
            {
                rooms[j].tag = "Left";
            }
            // Give the room a number
            rooms[j].name = "Room" + j.ToString();
        }
        // Room instantiated with prefab
        //rooms[i] = Instantiate(Resources.Load("Prefabs/Room"), firstRoom.transform) as GameObject;
        //rooms[i].transform.localScale = new Vector3(1f, 1f, 1f);
        //rooms[i].transform.localPosition = new Vector3(1.8f, 2.05f, -0.33f);
        //rooms[i].transform.localRotation = Quaternion.Euler(0, 0, -60);

        //rooms[i + 1] = Instantiate(Resources.Load("Prefabs/Room"), firstRoom.transform) as GameObject;
        //rooms[i + 1].transform.localScale = new Vector3(1f, 1f, 1f);
        //rooms[i + 1].transform.localPosition = new Vector3(-1.8f, 2.05f, 0.33f);
        //rooms[i + 1].transform.localRotation = Quaternion.Euler(0, 0, 60);

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
