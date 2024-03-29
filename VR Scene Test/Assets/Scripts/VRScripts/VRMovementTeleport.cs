﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class VRMovementTeleport : MonoBehaviour {

    string testFilePath, newTestFilePath;
    List<string> testFileNames;
    List<int> testFileNumbers;
    string nextTestFileNumber;
    string nextTestFileName;
    string lightConfiguration;

    protected SteamVR_TrackedObject trackedObj;
    public SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }

    //Input buttons from the VR controllers
    protected Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
    protected Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
    protected Valve.VR.EVRButtonId touchPad = Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad;
    protected Valve.VR.EVRButtonId dpadUp = Valve.VR.EVRButtonId.k_EButton_DPad_Up;
    protected Valve.VR.EVRButtonId dpadRight = Valve.VR.EVRButtonId.k_EButton_DPad_Right;
    protected Valve.VR.EVRButtonId dpadLeft = Valve.VR.EVRButtonId.k_EButton_DPad_Left;
    protected Valve.VR.EVRButtonId dpadDown = Valve.VR.EVRButtonId.k_EButton_DPad_Down;
    protected Valve.VR.EVRButtonId menuBttn = Valve.VR.EVRButtonId.k_EButton_ApplicationMenu;
    //If we are holding the right trigger down
    protected bool _triggerIsDown;

    private SteamVR_Fade steamVRFade;

    public void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        steamVRFade = this.gameObject.AddComponent<SteamVR_Fade>();
    }



    void Start()
    {
        // Looks for the AssetController Game Object
        GameObject assetController = GameObject.Find("AssetController");
        Camera camera = GetComponent<Camera>();
        // Locates the RoomSetup Script and takes the current light config setting
        lightConfiguration = assetController.GetComponent<RoomSetup>().lightconfig.ToString();
        testFileNames = new List<string>();
        testFileNumbers = new List<int>();
        testFilePath = ("TestResults");
        DirectoryInfo dir = new DirectoryInfo(testFilePath);
        FileInfo[] info = dir.GetFiles("*.txt");
        // Collects all file names in directory
        foreach (FileInfo f in info)
        {
            testFileNames.Add(f.Name);
        }
        // Checks to see if there are no files in the directory
        if (testFileNames.Count == 0)
        {
            nextTestFileNumber = 1.ToString();
        }
        else // If files exist in the directory
        {
            // Parses all file names and filters out all non numeric chracters
            for (int i = 0; i < testFileNames.Count; i++)
            {
                testFileNames[i] = GetNumbers(testFileNames[i]);
            }
            // Test file numbers are converted to ints and sent to an int list
            testFileNumbers = testFileNames.ConvertAll(int.Parse);
            // New test file will be generated that is marked one larger than the last most recent test file (largest number)
            nextTestFileNumber = (testFileNumbers.Max() + 1).ToString();
        }
        // Generates the new file name by combining strings with the new file number
        nextTestFileName = "\\Test-" + nextTestFileNumber + "-LightConfiguration-" + lightConfiguration + ".txt";
        // Creates the new file path for the test file by combining the folder path with the file name
        newTestFilePath = testFilePath.Insert(testFilePath.Length, nextTestFileName);
        using (StreamWriter sw = File.CreateText(newTestFilePath))
        {
            sw.WriteLine("Light Configuration " + lightConfiguration + " test conducted on: {0}", DateTime.Now.ToString("dd/MM/yyyy @ HH:mm" + "."));
            sw.WriteLine("Author: Jack D Smith");
        }
    }
        // Update is called once per frame
        void Update()
    {
        Teleport();
    }

    public void Teleport()
    {
        string s = "";
        //Determining if we hit a position in the gameworld
        RaycastHit hit;
        bool validHit = Physics.Raycast(this.transform.position, this.transform.forward, out hit, Mathf.Infinity);

        //If the trigger on the vr controller is down, we set the _triggerISDown to true.
        if (controller.GetPressDown(triggerButton))
        {
            _triggerIsDown = true;
        }

        if (validHit && hit.collider.tag == "WAYPOINT")
        {
            CreatePointer(Color.green);
            GameObject[] list = GameObject.FindGameObjectsWithTag("WAYPOINT");
            for(int i=0; i<list.Length; i++) { list[i].GetComponent<Renderer>().material.color = Color.white; }
            hit.collider.gameObject.GetComponent<Renderer>().material.color = Color.green;
        }
        else
        {
            CreatePointer(Color.red);
            GameObject[] list = GameObject.FindGameObjectsWithTag("WAYPOINT");
            for (int i = 0; i < list.Length; i++) { list[i].GetComponent<Renderer>().material.color = Color.white; }
        }
            //IF the trigger is up and we have a valid teleport position on a Room
            //We move the camera rig object to that position in the world and reset the trigger is down
            if (controller.GetPressUp(triggerButton))
        {
            if (validHit && hit.collider.tag == "WAYPOINT") 
            {
                CreatePointer(Color.green);
                GameObject cR = GameObject.Find("Camera Rig");
                GameObject cH = GameObject.Find("Camera (eye)");
                double xCoord, yCoord, zCoord; // Round all coords to 2 decimal places
                double cameraDirX, cameraDirY, cameraDirZ;
                // This will take the camera to the centre of the object.
                cR.gameObject.transform.position = hit.collider.transform.position;
                // Coordinates of Camera Rig
                xCoord = Math.Round(cR.transform.position.x, 2);
                yCoord = Math.Round(cR.transform.position.y, 2);
                zCoord = Math.Round(cR.transform.position.z, 2);
                // Direction that the Camera is facing
                cameraDirX = Math.Round(cH.transform.eulerAngles.x, 2);
                cameraDirY = Math.Round(cH.transform.eulerAngles.y, 2);
                cameraDirZ = Math.Round(cH.transform.eulerAngles.z, 2);
                // Add coords into the file followed by a new line
                s = "Coords X: " + xCoord + " Y: " + yCoord + " Z: " + zCoord + "\n" 
                + " Camera Direction X: " + cameraDirX + " Y: " + cameraDirY + " Z: " + cameraDirZ;
                File.AppendAllText(newTestFilePath, s + Environment.NewLine);
            }
            else
            {
                
            }
            _triggerIsDown = false;
        }
    }

    IEnumerator StartFade()
    {
        steamVRFade.OnStartFade(Color.black, 0.5f, false);
        yield return new WaitForSeconds(1f);
        steamVRFade.OnStartFade(Color.clear, 0.5f, false);
    }

    private void CreatePointer(Color colourCodeID)
    {
        if (this.gameObject.GetComponent<SteamVR_LaserPointer>() != null)
        {
            Destroy(this.gameObject.transform.GetChild(1).gameObject);
            DestroyImmediate(this.gameObject.GetComponent<SteamVR_LaserPointer>());
        }
        this.gameObject.AddComponent<SteamVR_LaserPointer>();
        this.gameObject.GetComponent<SteamVR_LaserPointer>().color = colourCodeID;
    }

    private static string GetNumbers(string input)
    {
        return new string(input.Where(c => char.IsDigit(c)).ToArray());
    }
}
