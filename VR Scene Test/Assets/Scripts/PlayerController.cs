using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	/*
    Simple flycam
    Controls:
    WASD  : Directional movement
    Shift : Increase speed
    Space : Moves camera directly up per its local Y-axis
	*/

	public float movementSpeed = 2.0f;   // Regular speed
	public float shiftAdd  = 50.0f;   // Amount to accelerate when shift is pressed
	public float maxShift  = 100.0f;  // Maximum speed when holding shift
	public float camSens   = 0.15f;   // Mouse sensitivity

	private Vector3 lastMouse = new Vector3(255, 255, 255);

	void Update()
	{
		lastMouse = Input.mousePosition - lastMouse;
		lastMouse = new Vector3(-lastMouse.y * camSens, lastMouse.x * camSens, 0);
		lastMouse = new Vector3(transform.eulerAngles.x + lastMouse.x, transform.eulerAngles.y + lastMouse.y, 0);
		transform.eulerAngles = lastMouse;
		lastMouse = Input.mousePosition;
		// Mouse camera angle done.  

		// Keyboard commands
		Vector3 playerPosition = GetInput();
		playerPosition *= movementSpeed;
		playerPosition *= Time.deltaTime;
		transform.Translate(playerPosition);
	}

	// Returns the basic values, if it's 0 than it's not active.
	private Vector3 GetInput()
	{
		Vector3 playerMovement = new Vector3();

		// Forwards
		if (Input.GetKey(KeyCode.W))
			playerMovement += new Vector3(0, 0, 1);

		// Backwards
		if (Input.GetKey(KeyCode.S))
			playerMovement += new Vector3(0, 0, -1);

		// Left
		if (Input.GetKey(KeyCode.A))
			playerMovement += new Vector3(-1, 0, 0);

		// Right
		if (Input.GetKey(KeyCode.D))
			playerMovement += new Vector3(1, 0, 0);

		// Up
		if (Input.GetKey(KeyCode.Space))
			playerMovement += new Vector3(0, 1, 0);

		// Down
		if (Input.GetKey(KeyCode.LeftControl))
			playerMovement += new Vector3(0, -1, 0);

		return playerMovement;
	}
}