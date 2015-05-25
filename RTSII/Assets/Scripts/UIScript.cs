using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIScript : MonoBehaviour {
	public GameObject unitPanel;
	public Text oreText;
	public Button rhinoSelectedButton;
	public Button gunshipSelectedButton;
	public Button engineerSelectedButton;
	public Button colonySelectedButton;
	//buttons for other units as well

	public Button buildEngineerButton;

	SelectorScript selectorScript;

	void Start() {
		selectorScript = transform.GetComponent<SelectorScript> ();   
	}

	// Update is called once per frame
	void Update () {
		oreText.text = "" + selectorScript.ore;
		if (Input.GetMouseButtonUp (0) && Input.mousePosition.y > SelectorScript.mouseYLowerBound) {
			Debug.Log ("UIScript: Update: registered left mouse button release", Camera.main);
			WhichButtonsToDisplay();
		}
	}

	public void WhichButtonsToDisplay() {
		Button[] buttons = new Button[9]; //<at most 9 are needed
		int buttonIndex = 0;
		//put buildings first
		int numColonies = SelectorScript.GetNumTypeSelected ("Colony");
		if (numColonies > 0) {
			colonySelectedButton.GetComponentInChildren<Text>().text = "" + numColonies;
			buttons[buttonIndex] = colonySelectedButton;
			buttonIndex++;
		}
		int numEngineers = SelectorScript.GetNumTypeSelected ("Engineer");
		if (numEngineers > 0) {
			engineerSelectedButton.GetComponentInChildren<Text>().text = "" + numEngineers;
			buttons[buttonIndex] = engineerSelectedButton;
			buttonIndex++;
		}
		//display rhino selection image and number of rhinos selected
		int numRhinos = SelectorScript.GetNumTypeSelected("Rhino");
		//Debug.Log ("UIScript: WhichButtonsToDisplay: numRhinos: " + numRhinos, Camera.main);
		if (numRhinos > 0) {
			rhinoSelectedButton.GetComponentInChildren<Text>().text = "" + numRhinos;
			buttons[buttonIndex] = rhinoSelectedButton;
			buttonIndex++;
		}
		int numGunships = SelectorScript.GetNumTypeSelected ("Gunship");
		if (numGunships > 0) {
			gunshipSelectedButton.GetComponentInChildren<Text>().text = "" + numGunships;
			buttons[buttonIndex] = gunshipSelectedButton;
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

	public void RemoveButtons(string name) {
		string buttonName = "";
		if (name == "Rhino") {
			buttonName = "RhinoSelectedButton";
		} else if (name == "Gunship") {
			buttonName = "GunshipSelectedButton";
		} else if (name == "Engineer") {
			buttonName = "EngineerSelectedButton";
		} else if (name == "Colony") {
			buttonName = "ColonySelectesButton";
		}
		foreach (Transform child in unitPanel.GetComponentsInChildren<RectTransform>()) {
			if (child.gameObject.name != "UIPanel" && child.gameObject.name != "Text" && child.gameObject.name != buttonName) {
				RectTransform trans = child.gameObject.GetComponent<RectTransform>();
				trans.position = new Vector3(trans.position.x, trans.position.y-60, 0);
			} else if (child.gameObject.name == buttonName) {
				child.gameObject.GetComponent<RectTransform>().position = new Vector3(5,5,0);
			}
		}
	}

	public void DisplayColonyBuildList(){

	}
}