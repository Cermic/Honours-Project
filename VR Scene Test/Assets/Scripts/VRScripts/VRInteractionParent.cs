using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRInteractionParent : MonoBehaviour
{
    public enum MovementMethod {Roam, Teleport}
    public MovementMethod movementType;
    protected SteamVR_TrackedObject trackedObj;
    public SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }
    protected Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
    protected Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
    protected Valve.VR.EVRButtonId touchPad = Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad;
    protected Valve.VR.EVRButtonId dpadUp = Valve.VR.EVRButtonId.k_EButton_DPad_Up;
    protected Valve.VR.EVRButtonId dpadRight = Valve.VR.EVRButtonId.k_EButton_DPad_Right;
    protected Valve.VR.EVRButtonId dpadLeft = Valve.VR.EVRButtonId.k_EButton_DPad_Left;
    protected Valve.VR.EVRButtonId dpadDown = Valve.VR.EVRButtonId.k_EButton_DPad_Down;
    protected Valve.VR.EVRButtonId menuBttn = Valve.VR.EVRButtonId.k_EButton_ApplicationMenu;

    //Object we are holding (null if we are not holding an object)
    protected GameObject heldObject;
    //Object we are able to pickup (null if no object is able to be picked up)
    protected GameObject objectInRange;
    //If we are holding the right trigger down
    protected bool _triggerIsDown;

    public void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    // Update is called once per frame
    public void Update ()
    {
        if (objectInRange != null)
            Debug.Log("object in range: " + objectInRange.name);

        //When grip button is pressed DOWN
		if(controller.GetPressDown(gripButton))
        {
            PickUpObject();
        }

        //When grip button is pressed UP
        if(controller.GetPressUp(gripButton))
        {
            DropObject();
        }

        Move();

    }


    //***TASK 1***
    public void PickUpObject()
    {
        if (objectInRange == null)
            return;

        //TODO:
        //Your task is to set the objectInRange to be parented to the controller
        //For help, look up the unity tutorials for SetParent. 
        //Tip: The current controller gameobject is this.gameobject and the pickup object is objectInRange

        heldObject = objectInRange;
        heldObject.transform.SetParent(this.transform);
    }

    //***TASK 2***
    public void DropObject()
    {
        if (heldObject == null)
            return;

        //TODO:
        //When dropping the object, you do not want the objectInRange to be parented to the controller
        //Use the same steps used in pickup object to make the parent null (null = none in programming terms).
        //Tip instead of setting the parent to this.gameobject, set it to null.

        heldObject.transform.SetParent(null);
        heldObject = null;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Interactable")
            objectInRange = other.gameObject;
    }

    public void OnTriggerExit(Collider other)
    {
        if(other.gameObject.Equals(objectInRange))
            objectInRange = null;
    }

    //-------------------------------------------------------------------DO NOT TOUCH ANY CODE BELOW THIS POINT -------------------------------------------------------------------------
    public void Move()
    {
        if (movementType == MovementMethod.Roam)
            Roam();
        if (movementType == MovementMethod.Teleport)
            Teleport();
    }

    public void Teleport()
    {
        RaycastHit hit;
        bool validHit = Physics.Raycast(this.transform.position, this.transform.forward, out hit, Mathf.Infinity);

        if (controller.GetPressDown(triggerButton))
        {
            _triggerIsDown = true;
        }

        if (_triggerIsDown)
        {
            if (this.GetComponent<SteamVR_LaserPointer>() == null)
                this.gameObject.AddComponent<SteamVR_LaserPointer>();
        }
        else
        {
            if (this.gameObject.GetComponent<SteamVR_LaserPointer>() != null)
                DestroyImmediate(this.gameObject.GetComponent<SteamVR_LaserPointer>());
        }

        if (controller.GetPressUp(triggerButton))
        {
            if (validHit && hit.collider.tag == "Ground")
            {
                GameObject.Find("[CameraRig]").gameObject.transform.position = hit.point;
            }
            _triggerIsDown = false;
        }
    }


    public void Roam()
    {
        GameObject Parent_VR_Holder = GameObject.Find("[CameraRig]").gameObject;
        float movement_speed = 2f;
        if (controller.GetAxis(touchPad).y > 0.5f || controller.GetAxis(touchPad).y < -0.5f)
            Parent_VR_Holder.transform.position += this.gameObject.transform.forward * Time.deltaTime * (controller.GetAxis(touchPad).y * movement_speed);
        if (controller.GetAxis(touchPad).x > 0.5f || controller.GetAxis(touchPad).x < -0.5f)
            Parent_VR_Holder.transform.position += this.gameObject.transform.right * Time.deltaTime * (controller.GetAxis(touchPad).x * movement_speed);

        Parent_VR_Holder.transform.position = new Vector3(Parent_VR_Holder.transform.position.x, 0f, Parent_VR_Holder.transform.position.z);
    }
}
