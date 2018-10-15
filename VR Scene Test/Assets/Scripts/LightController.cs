using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour {

	public enum LightConfiguration
	{ONE, TWO, THREE, FOUR, FIVE, SIX}
	public LightConfiguration lightconfig;
	
	// Load Light Properties
	void Start () {
		// Load Lights In
		// Room 1
		Light R1L1 = GameObject.Find ("R1L1").GetComponent<Light> ();
		Light R1L2 = GameObject.Find ("R1L2").GetComponent<Light> ();
		// Room 2
		Light R2L1 = GameObject.Find ("R2L1").GetComponent<Light> ();
		Light R2L2 = GameObject.Find ("R2L2").GetComponent<Light> ();
		// Room 3
		Light R3L1 = GameObject.Find ("R3L1").GetComponent<Light> ();
		Light R3L2 = GameObject.Find ("R3L2").GetComponent<Light> ();
		// Room 4
		Light R4L1 = GameObject.Find ("R4L1").GetComponent<Light> ();
		Light R4L2 = GameObject.Find ("R4L2").GetComponent<Light> ();
		// Room 5
		Light R5L1 = GameObject.Find ("R5L1").GetComponent<Light> ();
		Light R5L2 = GameObject.Find ("R5L2").GetComponent<Light> ();
		// Room 6
		Light R6L1 = GameObject.Find ("R6L1").GetComponent<Light> ();
		Light R6L2 = GameObject.Find ("R6L2").GetComponent<Light> ();
		// Room 7
		Light R7L1 = GameObject.Find ("R7L1").GetComponent<Light> ();
		Light R7L2 = GameObject.Find ("R7L2").GetComponent<Light> ();

		if (lightconfig == LightConfiguration.ONE) {
			// Starting Room 1
			// Light 1
			R1L1.color = Color.blue; 
			// Light 2
			R1L2.color = Color.red; 

			//Room 2
			// Light 1
			R2L1.intensity = 0.1f;
			// Light 2
			R2L2.intensity = 1.0f;

			//Room 4
			//Light 1
			R4L1.intensity = 0.1f;
			R4L1.color = Color.red;	
			// Light 2
			R4L2.intensity = 1.0f;
			R4L2.color = Color.blue;

			//Room 5
			//Light 1
			R5L1.intensity = 0.1f;
			R5L1.color = Color.red;	
			// Light 2
			R5L2.intensity = 1.0f;
			R5L2.color = Color.blue;

			//Room 3
			// Light 1
			R3L1.intensity = 0.1f;
			// Light 3
			R3L2.intensity = 1.0f;

			//Room 6
			//Light 1
			R6L1.intensity = 0.1f;
			R6L1.color = Color.red;	
			// Light 2
			R6L2.intensity = 1.0f;
			R6L2.color = Color.blue;

			//Room 7
			//Light 1
			R7L1.intensity = 0.1f;
			R7L1.color = Color.red;	
			// Light 2
			R7L2.intensity = 1.0f;
			R7L2.color = Color.blue;

		} else if (lightconfig == LightConfiguration.TWO) {
			// Starting Room 1
			// Light 1
			R1L1.intensity = 0.1f;
			// Light 2
			R1L2.intensity = 1.0f;

			//Room 2
			// Light 1
			R2L1.color = Color.blue;
			// Light 2
			R2L2.color = Color.red; 

			//Room 4
			//Light 1
			R4L1.intensity = 0.1f;
			R4L1.color = Color.red;	
			// Light 2
			R4L2.intensity = 1.0f;
			R4L2.color = Color.blue;

			//Room 5
			//Light 1
			R5L1.intensity = 0.1f;
			R5L1.color = Color.red;	
			// Light 2
			R5L2.intensity = 1.0f;
			R5L2.color = Color.blue;

			//Room 3
			// Light 1
			R3L1.color = Color.blue;
			// Light 3
			R3L2.color = Color.red;

			//Room 6
			//Light 1
			R6L1.intensity = 0.1f;
			R6L1.color = Color.red;	
			// Light 2
			R6L2.intensity = 1.0f;
			R6L2.color = Color.blue;

			//Room 7
			//Light 1
			R7L1.intensity = 0.1f;
			R7L1.color = Color.red;	
			// Light 2
			R7L2.intensity = 1.0f;
			R7L2.color = Color.blue;
			
		} else {
			// Do nothing and leave the lights White.
		}
			
	}// End of Start function

	/*
	void LightSet(Light[] light, LightConfiguration config)
	{
		if (config == LightConfiguration.ONE) {
			light [0].color = Color.blue;
		}
		if (config == LightConfiguration.TWO) {

		}
		if (config == LightConfiguration.THREE) {

		}
		if (config == LightConfiguration.FOUR) {

		}
		if (config == LightConfiguration.FIVE) {

		}
		if (config == LightConfiguration.SIX) {

		}

	}
	*/
}
