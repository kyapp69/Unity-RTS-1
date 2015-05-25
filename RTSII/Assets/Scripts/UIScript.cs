using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIScript : MonoBehaviour {
	public GameObject unitPanel;
	public Text oreText;
	public Button RhinoSelectedButton;
	public Button GunshipSelectedButton;
	public Button EngineerSelectedButton;
	public Button ColonySelectedButton;
	//buttons for other units as well

	//RTSCamera cam;

	void Start() {
		//cam = transform.GetComponent<RTSCamera> (); 
	}

	// Update is called once per frame
	void Update () {
		oreText.text = "" + SelectorScript.ore;
		if (Input.GetMouseButtonUp (0) && Input.mousePosition.y > SelectorScript.mouseYLowerBound) {
			Debug.Log ("UIScript: Update: registered left mouse button release", Camera.main);
			WhichButtonsToDisplay();
		}
		if (Input.GetMouseButtonDown (0) & Input.mousePosition.y > SelectorScript.mouseYLowerBound) {
			foreach (Transform child in unitPanel.GetComponentsInChildren<RectTransform>()) {
				if (child.gameObject.name != "UIPanel") {
					Destroy(child.gameObject);
				}
			}
		}
	}

	void WhichButtonsToDisplay() {
		Button[] buttons = new Button[9]; //<at most 9 are needed
		int buttonIndex = 0;
		//put buildings first
		int numColonies = SelectorScript.GetNumTypeSelected ("Colony");
		if (numColonies > 0) {
			Button colonyButton = Instantiate (ColonySelectedButton);
			colonyButton.GetComponent<RectTransform>().SetParent (unitPanel.GetComponent<Transform>(), false);
			colonyButton.GetComponentInChildren<Text>().text = "" + numColonies;
			colonyButton.GetComponentInChildren<Text>().color = Color.white;
			colonyButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);
			buttons[buttonIndex] = colonyButton;
			buttonIndex++;
		}
		int numEngineers = SelectorScript.GetNumTypeSelected ("Engineer");
		if (numEngineers > 0) {
			Button engineerButton = Instantiate (EngineerSelectedButton);
			engineerButton.GetComponent<RectTransform>().SetParent (unitPanel.GetComponent<Transform>(), false);
			engineerButton.GetComponentInChildren<Text>().text = "" + numEngineers;
			engineerButton.GetComponentInChildren<Text>().color = Color.white;
			engineerButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);
			buttons[buttonIndex] = engineerButton;
			buttonIndex++;
		}
		//display rhino selection image and number of rhinos selected
		int numRhinos = SelectorScript.GetNumTypeSelected("Rhino");
		//Debug.Log ("UIScript: WhichButtonsToDisplay: numRhinos: " + numRhinos, Camera.main);
		if (numRhinos > 0) {
			Button rhinoButton = Instantiate (RhinoSelectedButton);
			rhinoButton.GetComponent<RectTransform>().SetParent (unitPanel.GetComponent<Transform>(), false);
			rhinoButton.GetComponentInChildren<Text>().text = "" + numRhinos;
			rhinoButton.GetComponentInChildren<Text>().color = Color.white;
			rhinoButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);
			buttons[buttonIndex] = rhinoButton;
			buttonIndex++;
		}
		int numGunships = SelectorScript.GetNumTypeSelected ("Gunship");
		if (numGunships > 0) {
			Button gunshipButton = Instantiate (GunshipSelectedButton);
			gunshipButton.GetComponent<RectTransform>().SetParent (unitPanel.GetComponent<Transform>(), false);
			gunshipButton.GetComponentInChildren<Text>().text = "" + numGunships;
			gunshipButton.GetComponentInChildren<Text>().color = Color.white;
			gunshipButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);
			buttons[buttonIndex] = gunshipButton;
			buttonIndex++;
		}
		//same shit for other units

		DisplaySelectedButtons (buttons);
	}

	void DisplaySelectedButtons(Button[] buttons) {
		int x = 5; 
		int y = 5;
		foreach (Button b in buttons) {
			if (b) {
				b.GetComponent<RectTransform>().position = new Vector3(x, y, 0);
				x += 55; 
			}
		}
	}
}