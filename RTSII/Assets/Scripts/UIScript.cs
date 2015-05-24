using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIScript : MonoBehaviour {
	public GameObject unitPanel;
	public Button RhinoSelectedButton;
	//buttons for other units as well

	//RTSCamera cam;

	void Start() {
		//cam = transform.GetComponent<RTSCamera> (); 
	}

	// Update is called once per frame
	void Update () {
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
		//put buildings first, but cannot do yet

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
				y += 55;
			}
		}
	}
}