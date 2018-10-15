using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomLoad : MonoBehaviour {

	private GameObject roomRight;

	// Use this for initialization
	void Start () {
		roomRight = Instantiate(Resources.Load("Prefabs/Room_Right"), new Vector3(0,30,0), Quaternion.identity) as GameObject;
		Light light1 = roomRight.GetComponentInChildren (typeof(Light)) as Light;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
     