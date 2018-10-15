using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

	private Material emmisive_white = Resources.Load ("Materials/Emmisive_White") as Material;
	private const int NUM_ROOMS = 2;
	// This approach causes problems because the values in rooms are treated as reference values and
	// Initialise to null values. This stops the loop executing properly.
	//private GameObject[] rooms = new GameObject[NUM_ROOMS];
	private GameObject room1, room2;
	private GameObject lightSphere1, lightSphere2;
	private Light light1, light2;
	public enum LightConfiguration
	{ONE, TWO, THREE, FOUR, FIVE, SIX}
	public LightConfiguration lightconfig;

	void Start () {
		// Room instantiated with prefab
		/*for (int i = 0; i < NUM_ROOMS; i++) {
			rooms[i] = Instantiate (Resources.Load ("Prefabs/Room_Right"), new Vector3 (0, 30 + (i*10), 0), Quaternion.identity) as GameObject;
			rooms[i].name = "Room" + i.ToString(); 
		}*/
		room1 = Instantiate (Resources.Load ("Prefabs/Room_Right"), new Vector3 (0, 30 , 0), Quaternion.identity) as GameObject;
		room1.name = "Room1";

		room2 = Instantiate (Resources.Load ("Prefabs/Room_Right"), new Vector3 (0, 40, 0), Quaternion.identity) as GameObject;
		room2.name = "Room2";
		room2.transform.parent = room1.transform;
		// Setup Light 1
		lightSphere1 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		lightSphere1.GetComponent<Renderer> ().material = emmisive_white;
		lightSphere1.transform.parent = room1.transform;
		lightSphere1.name = "Light1";
		light1 = lightSphere1.AddComponent<Light> ();

		// Setup Light 2
		lightSphere2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		lightSphere2.GetComponent<Renderer> ().material = emmisive_white;
		lightSphere2.transform.parent = room1.transform;
		lightSphere2.name = "Light2";
		light2 = lightSphere2.AddComponent<Light> ();

		light1.color = Color.blue;
		lightSphere1.transform.localPosition = new Vector3 (0, 10, 0);
		lightSphere2.transform.localPosition = new Vector3 (0, 0, 0);
	}

	void SetupLights(LightConfiguration lightconfig)
	{
		
	}
}
