using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RoomSetup : MonoBehaviour {

    // Defines the number of rooms attached to each room and how many layers deep the maze will go.
    private const int ROOMS_PER_BRANCH = 2;
    private const int MAZE_DEPTH = 4;
    private int lightConfigs = 0;
    // Material for the light sphere
    private Material emmisive_white;
    // Array of Room objects
    private Room[] roomSet;
    // Light config enum that can be changed in editor
    public enum LightConfiguration
    { ONE, TWO, THREE, FOUR, FIVE, SIX, SEVEN, EIGHT, NINE, TEN, ELEVEN, TWELVE}

    public LightConfiguration lightconfig;
    private Vector3 firstRoomCoords;
    private Quaternion firstRoomAngle;
    private Quaternion firstRoomOrientation, leftOrientation, rightOrientation;
    private Vector3 initialLeftOffset, initialRightOffset, leftOffset, rightOffset;
    private Vector3 lightLeftOffset, lightRightOffset;
    // Bool to be used to turn on and off the experiment state
    public bool isControl = false;
    private RoomProperties roomProp;
    private LightProperties lightProp;
    Object firstRoomObject, roomObject;

    private List<float> colourValues;
    private List<int> chainValues;
    private int[] roomCombination;
    private List<Vector3> colourValues2;
    private Vector3 firstRoomColour;
    private int endIndex;
    private int startIndex;

    void Start()
    {
        // Load material for the light sphere
        emmisive_white = Resources.Load("Materials/Emmisive_White") as Material;
        lightProp.lightMat = emmisive_white;
        // Load room objects
        firstRoomObject = Resources.Load("Prefabs/FirstRoom");
        roomObject = Resources.Load("Prefabs/Room");
        // Load sphere primitive to represent a light
        firstRoomCoords = new Vector3(0, 0, 0);
        firstRoomOrientation = Quaternion.Euler(-90, 0, 0);
        initialLeftOffset = new Vector3(1.9f, 2.095f, -0.35f);
        initialRightOffset = new Vector3(-1.9f, 2.095f, 0.35f);

        // Left & right offsets and material will always be the same so they are set here.
        lightProp.lightLeftOffset = new Vector3(0.36f, 1.2f, 0.26f);
        lightProp.lightRightOffset = new Vector3(-0.36f, 1.2f, 0.26f);
        lightProp.lightMat = emmisive_white;

        lightLeftOffset = new Vector3(0.36f, 1.2f, 0.26f);
        lightRightOffset = new Vector3(-0.36f, 1.2f, 0.26f);

        leftOffset = new Vector3(0.85f, 1.5f, 0f);
        rightOffset = new Vector3(-0.85f, 1.5f, 0f);
        // Standard room scale - this is used to scale down the rooms as they adopt the scale of their parent (far too big)
        roomProp.scale = new Vector3(1f, 1f, 1f);
        leftOrientation = Quaternion.Euler(0, 0, -60);
        rightOrientation = Quaternion.Euler(0, 0, 60);

        // Populate room array - This is done using the ROOMS_PER_BRANCH value to the power of MAZE_DEPTH -1. 
        // This ensures the maze is always the appropriate size.
        int mazeSize = (int)Mathf.Pow(ROOMS_PER_BRANCH, MAZE_DEPTH) - 1;
        roomSet = new Room[mazeSize];

        // Read in colour values from config file
        colourValues = new List<float>();
        chainValues = new List<int>();
        string configText = File.ReadAllText("LightConfig.txt");
        colourValues = ReadConfig(configText, colourValues);
        // Sets the number of configurations based on the amount read from external file.
        lightConfigs = LightConfigs(colourValues);
        // A collection of all possible Light Configurations
        LightConfig[] lc = new LightConfig[lightConfigs];

        string currentConfigs = File.ReadAllText("LightConfigChains.txt");
        // Array of Light Configurations for the current maze to be built
        LightConfig[] mazeConfig = new LightConfig[MAZE_DEPTH - 1];
        roomCombination = new int[MAZE_DEPTH - 1];

        ReadConfigChain(currentConfigs, chainValues);

        // Process all configs and setup an array of LightConfigs.
        ProcessConfig(colourValues, lc);
        // Set colours of the first room lights
        SetupLights(lightconfig);
        LightConfig[] roomArray = new LightConfig[roomSet.Length];

        // Picks out the correct light configuration for each level of the maze.
        startIndex = 1;
        endIndex = 1;
        for (int i = 0; i < MAZE_DEPTH - 1; i++)
        {
            if (i == 0)
            {
                roomArray[0] = lc[roomCombination[0] - 1]; // First room config
            }
            else
            {
                startIndex = (int)Mathf.Pow(ROOMS_PER_BRANCH, i); // Defines the index to start inserting the roomCombination at
                endIndex = (int)Mathf.Pow(ROOMS_PER_BRANCH, i + 1); //  Defines the index to stop inserting the roomCombination at
                for (int j = startIndex - 1; j < endIndex - 1; j++)
                {
                    roomArray[j] = lc[roomCombination[i] - 1]; // All other levels.
                }
            }
        }
        // Assign end rooms to deafult values
        for (int j = lc.Length - 1; j < roomArray.Length; j++)
        {
            // Left Light rgb values
            roomArray[j].leftLightColour = new Vector3(1.0f, 1.0f, 1.0f);
            // Right light rgb values
            roomArray[j].rightLightColour = new Vector3(1.0f, 1.0f, 1.0f);
            // Left light intensity
            roomArray[j].leftLightIntensity = 1.0f;
            // Right light intensity
            roomArray[j].rightLightIntensity = 1.0f;
        }
        // Constructs each room in a tree structure
        for (int j = 0; j < roomSet.Length; j++)
        {
            roomProp.roomIndex = j;
            if (j == 0)
            {
                if (roomSet[j] == null) // If the first room is null construct it.
                {
                    // Set room properties for the first room.
                    roomProp.rObject = firstRoomObject;
                    roomProp.offset = firstRoomCoords;
                    roomProp.orientation = firstRoomOrientation;
                    roomProp.scale = Vector3.zero;
                    roomProp.parentTransform = null;
                    // Set light properties for the first room.
                    lightProp.lightMat = emmisive_white;
                    lightProp.lightConfig = roomArray[j];

                    roomSet[j] = new Room(roomProp, lightProp);
                }
            }
            else
            {
                // Finds the parent index of the room based on it's position in the array and number of branching rooms.
                float indexFloat = j;
                int parentRoomIndex = Mathf.CeilToInt(indexFloat /= ROOMS_PER_BRANCH) - 1;
                // Set room properties for the current room.
                // if j is => endIndex , then load the end room into roomObject instead.
                // Probably use this index to stop waypoints 2 and 3 being generated in there too.
                roomProp.rObject = roomObject;
                roomProp.scale = new Vector3(1,1,1);
                roomProp.parentTransform = roomSet[parentRoomIndex].gameObj.transform;
                // Set light properties for the current room.
                lightProp.lightConfig = roomArray[j];
                if (j == 1) // First room on the right
                {
                    roomProp.offset = initialRightOffset;
                    roomProp.orientation = rightOrientation;
                    roomSet[j] = new Room(roomProp, lightProp);
                }
                else if (j == 2) // First room on the left
                {
                    roomProp.offset = initialLeftOffset;
                    roomProp.orientation = leftOrientation;
                    roomSet[j] = new Room(roomProp, lightProp);
                }
                else
                {
                    // Child rooms of the first two ---
                    if (j % 2 == 0) // If the index is even, the room is on the left
                    {
                        roomProp.offset = leftOffset;
                        roomProp.orientation = leftOrientation;
                        roomSet[j] = new Room(roomProp, lightProp);
                    }
                    else // If the index is odd, the room is on the right
                    {
                        roomProp.offset = rightOffset;
                        roomProp.orientation = rightOrientation;
                        roomSet[j] = new Room(roomProp, lightProp);
                    }
                }
            }
            // Give the room the name room + it's array index.
            roomSet[j].gameObj.name = "Room" + j.ToString();
        }
    }
    // Reads the Config file that governs light configurations
    List<float> ReadConfig(string configText, List<float> colourValues)
    {
        char[] seperators = {'C', 'I', ',', '<','>', '|'};		
        string[] strValues = configText.Split(seperators);
		int i=0;
		
        foreach (string str in strValues)
        {			
            float value = 0.0f;
            if (float.TryParse(str, out value))
            {
                colourValues.Add(value);
             }
            i++;
        }
        return colourValues;
    }
    // Reads the Config file that governs light configurations
    List<int> ReadConfigChain(string configText, List<int> configChain)
    {
        char[] seperators = {'C'};
        string[] strValues = configText.Split(seperators);

        foreach (string str in strValues)
        {
            int value = 0;
            if (int.TryParse(str, out value))
            {
                configChain.Add(value);
            }
        }
        return configChain;
    }
    int LightConfigs(List<float> colourValues)
    {
        return colourValues.Count / 8;
    }
    void ProcessConfig(List<float> colourValues, LightConfig[] lc)
    {
        int offset = lc.Length;
        for (int i = 0; i < lc.Length; i++)
        {
            // Left Light rgb values
            lc[i].leftLightColour.x = colourValues[i * offset];
            lc[i].leftLightColour.y = colourValues[i * offset + 1];
            lc[i].leftLightColour.z = colourValues[i * offset + 2];
            // Right light rgb values
            lc[i].rightLightColour.x = colourValues[i * offset + 3];
            lc[i].rightLightColour.y = colourValues[i * offset + 4];
            lc[i].rightLightColour.z = colourValues[i * offset + 5];
            // Left light intensity
            lc[i].leftLightIntensity = colourValues[i * offset + 6];
            // Right light intensity
            lc[i].rightLightIntensity = colourValues[i * offset + 7];
        }

    }

    void SetupLights(LightConfiguration lightconfig)
    {
        int offset = 3;
        if (lightconfig == LightConfiguration.ONE)
        {
            for (int i = 0; i < 3; i++)
            { roomCombination[i] = chainValues[i]; }
        }
        if (lightconfig == LightConfiguration.TWO)
        {
            for (int i = 0; i < 3; i++)
            { roomCombination[i] = chainValues[i + offset]; }
        }
        if (lightconfig == LightConfiguration.THREE)
        {
            for (int i = 0; i < 3; i++)
            { roomCombination[i] = chainValues[i + (offset * 2)]; }
        }
        if (lightconfig == LightConfiguration.FOUR)
        {
            for (int i = 0; i < 3; i++)
            { roomCombination[i] = chainValues[i + (offset * 3)]; }
        }
        if (lightconfig == LightConfiguration.FIVE)
        {
            for (int i = 0; i < 3; i++)
            { roomCombination[i] = chainValues[i + (offset * 4)]; }
        }
        if (lightconfig == LightConfiguration.SIX)
        {
            for (int i = 0; i < 3; i++)
            { roomCombination[i] = chainValues[i + (offset * 5)]; }
        }
        if (lightconfig == LightConfiguration.SEVEN)
        {
            for (int i = 0; i < 3; i++)
            { roomCombination[i] = chainValues[i + (offset * 6)]; }
        }
        if (lightconfig == LightConfiguration.EIGHT)
        {
            for (int i = 0; i < 3; i++)
            { roomCombination[i] = chainValues[i + (offset * 7)]; }
        }
        if (lightconfig == LightConfiguration.NINE)
        {
            for (int i = 0; i < 3; i++)
            { roomCombination[i] = chainValues[i + (offset * 8)]; }
        }
        if (lightconfig == LightConfiguration.TEN)
        {
            for (int i = 0; i < 3; i++)
            { roomCombination[i] = chainValues[i + (offset * 9)]; }
        }
        if (lightconfig == LightConfiguration.ELEVEN)
        {
            for (int i = 0; i < 3; i++)
            { roomCombination[i] = chainValues[i + (offset * 10)]; }
        }
        if (lightconfig == LightConfiguration.TWELVE)
        {
            for (int i = 0; i < 3; i++)
            { roomCombination[i] = chainValues[i + (offset * 11)]; }
        }
    }
}
