using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System;

public class PositionRecorder : MonoBehaviour {

    string testFilePath, newTestFilePath;
    List<string> testFileNames;
    List<int> testFileNumbers;
    string nextTestFileNumber;
    string nextTestFileName;
    string lightConfiguration;
    
	// Use this for initialization
	void Start () {
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
        nextTestFileName = "\\Test-" + nextTestFileNumber +"-LightConfiguration-" + lightConfiguration + ".txt";
        // Creates the new file path for the test file by combining the folder path with the file name
        newTestFilePath = testFilePath.Insert(testFilePath.Length, nextTestFileName);
        using (StreamWriter sw = File.CreateText(newTestFilePath))
        {
            sw.WriteLine("Light Configuration " + lightConfiguration + " test conducted on: {0}", DateTime.Now.ToString("dd/MM/yyyy @ HH:mm" + "."));
            sw.WriteLine("Author: Jack D Smith");
        }
    }
	
	// Update is called once per frame
	void Update () {
        
            string s = "";
            if (Input.GetKeyDown("space")) // Capture coords on key press
            {
                 double xCoord, yCoord, zCoord; // Round all coords to 2 decimal places
                 xCoord = Math.Round(GetComponent<Camera>().transform.position.x, 2);
                 yCoord = Math.Round(GetComponent<Camera>().transform.position.y, 2);
                 zCoord = Math.Round(GetComponent<Camera>().transform.position.z, 2);
                 // Add coords into the file followed by a new line
                s =  "Coords X: "+ xCoord + " Y: " + yCoord  + " Z: "+ zCoord;
                File.AppendAllText(newTestFilePath, s + Environment.NewLine);
            }
        
	}

    private static string GetNumbers(string input)
    {
        return new string(input.Where(c => char.IsDigit(c)).ToArray());
    }
}
