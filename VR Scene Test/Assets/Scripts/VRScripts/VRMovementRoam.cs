using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRMovementRoam : MonoBehaviour {

    protected SteamVR_TrackedObject trackedObj;
    public SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }

    //Input buttons from the controllers....
    protected Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
    protected Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
    protected Valve.VR.EVRButtonId touchPad = Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad;
    protected Valve.VR.EVRButtonId dpadUp = Valve.VR.EVRButtonId.k_EButton_DPad_Up;
    protected Valve.VR.EVRButtonId dpadRight = Valve.VR.EVRButtonId.k_EButton_DPad_Right;
    protected Valve.VR.EVRButtonId dpadLeft = Valve.VR.EVRButtonId.k_EButton_DPad_Left;
    protected Valve.VR.EVRButtonId dpadDown = Valve.VR.EVRButtonId.k_EButton_DPad_Down;
    protected Valve.VR.EVRButtonId menuBttn = Valve.VR.EVRButtonId.k_EButton_ApplicationMenu;
	
	// Update is called once per frame
	void Update ()
    {
        //Calling our roam funtion during update for each frame.
        Roam();
    }

    public void Roam()
    {
        //Locatting the camera rig holder for the VR setup
        GameObject Parent_VR_Holder = GameObject.Find("[CameraRig]").gameObject;
        //The set movement speed for how fast we move in VR
        float movement_speed = 2f;
        //Getting input from the controllers to determine if we are moving Forwad, Left, Right or Back. 
        //We move the parent rig by the total calucated amount in that direction of the input modified by the time, angle of the controller and speed.
        if (controller.GetAxis(touchPad).y > 0.5f || controller.GetAxis(touchPad).y < -0.5f)
            Parent_VR_Holder.transform.position += this.gameObject.transform.forward * Time.deltaTime * (controller.GetAxis(touchPad).y * movement_speed);
        if (controller.GetAxis(touchPad).x > 0.5f || controller.GetAxis(touchPad).x < -0.5f)
            Parent_VR_Holder.transform.position += this.gameObject.transform.right * Time.deltaTime * (controller.GetAxis(touchPad).x * movement_speed);

        //For a basic teleportation, we keep the ground level constant.
        //More advanced systems are required to move up/down environments that are not even.
        Parent_VR_Holder.transform.position = new Vector3(Parent_VR_Holder.transform.position.x, 0f, Parent_VR_Holder.transform.position.z);
    }

}
