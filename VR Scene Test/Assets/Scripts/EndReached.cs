using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndReached : MonoBehaviour {
    // Use this for initialization
    private GameObject canvasGO;
    private void Awake()
    {
        // Load the Arial font from the Unity Resources folder.
        Font arial = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        // Create Canvas GameObject.
        canvasGO = new GameObject();
        canvasGO.name = "Canvas";
        canvasGO.transform.parent = gameObject.transform;
        canvasGO.AddComponent<Canvas>();
        Image img = canvasGO.AddComponent<Image>();
        img.color = new Color(0.2f, 0.1f, 0.1f);
        canvasGO.SetActive(false);

        // Get canvas from the GameObject.
        Canvas canvas = canvasGO.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        // Create the Text GameObject.
        GameObject textGO = new GameObject();
        textGO.transform.parent = canvasGO.transform;
        textGO.AddComponent<Text>();

        // Set Text component properties.
        Text text = textGO.GetComponent<Text>();
        text.font = arial;
        text.text = "You have found an End Room! \n\n Please take off the VR equipment to finish the experiment.";
        text.fontSize = 24;
        text.color = Color.green;
        text.alignment = TextAnchor.MiddleCenter;

        // Provide Text position and size using RectTransform.
        RectTransform rectTransform = text.GetComponent<RectTransform>();
        rectTransform.localPosition = new Vector3(0, 0, 0);
        rectTransform.sizeDelta = new Vector2(600, 200);
    }
    void Start () {
        //gameOverPanel.SetActive(false);
        gameObject.AddComponent<Rigidbody>().useGravity = false;
        BoxCollider bC = gameObject.AddComponent<BoxCollider>();
        bC.isTrigger = true;
        bC.size = new Vector3(3, 9, 3);
    }
	
	// Update is called once per frame
	void Update () {
        
	}

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "END ROOM")
        {
            canvasGO.SetActive(true);
            Debug.Log("End room reached.");
        }
    }
}
