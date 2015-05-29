using UnityEngine;
using System.Collections.Generic;

public class SelectorScript : MonoBehaviour {
	public static Rect selection = new Rect(0,0,0,0);
	public static bool selecting;
	public Texture2D selectionHighlight;
	public static float mouseYLowerBound = 60f;
	public static int ore = 100;

	Vector3 startPosition;
	static GameObject[] selectedObjects;
	static int selectedIndex;
	UIScript uiscript;

	// Use this for initialization
	void Start () {
		startPosition = -Vector3.one;
		selecting = false;
		selectedObjects = new GameObject[100];
		selectedIndex = 0;
		uiscript = transform.GetComponent<UIScript> ();
	}
	
	// Update is called once per frame
	void Update () {
		MouseActivity();
		Selectable sameSelectedType = CheckForSelection ();
		if (sameSelectedType) {
			if (sameSelectedType.uName == "Colony") {
				uiscript.DisplayColonyBuildButtons();
				if (selectedIndex == 1) {
					DisplayBuildQueue();
				}
			}
		}
	}

	void OnGUI() {
		if (startPosition != -Vector3.one && selecting) {
			GUI.color = new Color(1,1,1,0.3f);
			GUI.DrawTexture(selection, selectionHighlight);
		}
	}

	void MouseActivity() {
		if (Input.GetMouseButtonDown (0) && Input.mousePosition.y > mouseYLowerBound) {
			LeftMouseClick();
		} else if (Input.GetMouseButtonDown (1)) {
			RightMouseClick();
		} else if (Input.GetMouseButtonUp (0) && selecting) {
			ResetSelectionBox();
		} else if (Input.GetMouseButton (0) && selecting) {
			selection = new Rect(startPosition.x, InvertMouseY(startPosition.y), Input.mousePosition.x - startPosition.x, InvertMouseY(Input.mousePosition.y) - InvertMouseY(startPosition.y));
			if (selection.width < 0) {
				selection.x += selection.width;
				selection.width = -selection.width;
			}
			if (selection.height < 0) {
				selection.y += selection.height;
				selection.height = -selection.height;
			}
		}
	}

	void LeftMouseClick() {
		ClearSelected ("all");
		GameObject hitObject = FindHitObject ();
		Vector3 hitPoint = FindHitPoint ();
		if (hitObject && hitPoint != -Vector3.one) {
			Debug.Log ("SelectorScrip: LeftMouseClick: hitObject: " + hitObject.name, this.gameObject);
			Selectable unit = hitObject.GetComponent<Selectable>();
			if (unit && unit.team == 1) {
				unit.SetSelected (true);
				selectedObjects[0] = hitObject;
				selectedIndex  = 1;
			} else if (hitObject.name == "Terrain") {
				startPosition = Input.mousePosition;
				selecting = true;
			}
		}
	}

	void RightMouseClick() {
		if (Input.mousePosition.y > mouseYLowerBound) {
			Debug.Log ("SelectorScript: RightMouseClick: selectedIndex: " + selectedIndex, this.gameObject);
			GameObject hitObject = FindHitObject ();
			Vector3 hitPoint = FindHitPoint ();
			if (hitObject && hitObject.name == "Terrain" && hitPoint != -Vector3.one && selectedIndex > 0) {

				for (int i = 0; i < selectedIndex; i++) {
					Unit unit = selectedObjects [i].GetComponent<Unit> ();
					if (unit) {
						Debug.Log ("SelectorScript: RightMouseClick: moving object: " + selectedObjects [i].name, selectedObjects [i]);
						unit.SetDestination (hitPoint);
					}
				}
			}
		}
	}

	GameObject FindHitObject() {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit))
			return hit.collider.gameObject;
		return null;
	}
	
	Vector3 FindHitPoint() {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit))
			return hit.point;
		return -Vector3.one;
	}
	
	void ClearSelected(string name) {
		//clear array of selected objects
		if (selectedObjects [0]) {
			Debug.Log ("SelectorScript: ClearSelected: number of selected objects: " + selectedIndex, this.gameObject);
			for (int i = 0; i < selectedIndex; i++) {
				Debug.Log ("SelectorScript: ClearSelected: clearing: " + selectedObjects[i].name, this.gameObject);
				selectedObjects [i].GetComponent<Selectable> ().SetSelected (false);
				selectedObjects [i] = null;
			}
			uiscript.RemoveButtons(name);
			selectedIndex = 0;
		}
	}

	void ResetSelectionBox() {
		selecting = false;
		startPosition = -Vector3.one;
		selection = new Rect(0,0,0,0);
	}

	Selectable CheckForSelection() {
		if (selectedIndex > 0) {
			Selectable first = selectedObjects[0].GetComponent<Selectable>();
			bool allSame = true;
			for (int i = 0; i < selectedIndex; i++) {
				if (first.uName != selectedObjects[i].GetComponent<Selectable>().uName) {
					allSame = false;
				}
			}
			if (allSame) {
						return first;
			}
		}
		return null;
	}

	void DisplayBuildQueue() {
		Building building = selectedObjects [0].GetComponent<Building> ();
		int num = 1;
		if (building.buildQueue.Count > 0) {
			string lastName = building.buildQueue[0].objectName; 
			for (int i = 1; i < building.buildQueue.Count; i++){
				if (building.buildQueue[i].objectName == lastName){
					num++;
				} else {
					//create a button with the given number of same units in a row
					Debug.Log ("SelectorScript: DisplayBuildQueue: adding button name: " + building.buildQueue[i].objectName + " number: " + num, this.gameObject);
					uiscript.AddBuildQueue(lastName, num);
					num = 1;
				}
			}
			uiscript.AddBuildQueue(lastName, num);
		}
	}

	public static Vector3 GetLandGroupCenter() {
		Vector3 totalLand = new Vector3(0, 0, 0);
		int land = 0;
		for (int i = 0; i < selectedIndex; i++) {
			if (selectedObjects [i].GetComponent<Unit> () && !selectedObjects[i].GetComponent<Unit>().flying) {
				land++;
				totalLand += selectedObjects[i].GetComponent<Transform>().position;
			}
		}
		return totalLand / land;
	}

	public static Vector3 GetAirGroupCenter() {
		Vector3 totalAir = new Vector3(0, 0, 0);
		int air = 0;
		for (int i = 0; i < selectedIndex; i++) {
			if (selectedObjects [i].GetComponent<Unit> () && selectedObjects [i].GetComponent<Unit> ().flying) {
				air++;
				totalAir += selectedObjects [i].GetComponent<Transform> ().position;
			}
		}
		return totalAir / air;
	}
	
	public static float InvertMouseY(float y) {
		return Screen.height - y;
	}

	public static void AddSelected(GameObject obj) {
		selectedObjects [selectedIndex] = obj;
		selectedIndex++;
	}

	public static int GetNumTypeSelected(string n) {
		int total = 0;
		//Debug.Log ("SelectorScript: GetNumTypeSelected: selectedIndex = " + selectedIndex + ", n = " + n, Camera.main);
		for (int i = 0; i < selectedIndex; i++) {
			Selectable selectable = selectedObjects[i].GetComponent<Selectable>();
			if (selectable && selectable.uName == n) {
				total++;
			}
		}
		return total;
	}

	public void UnitSelect(string name) {
		Debug.Log ("SelectorScript: UnitSelect: Button Pressed", this.gameObject);
		List<GameObject> rhinos = new List<GameObject> ();
		for (int i = 0; i < selectedIndex; i++) {
			if (selectedObjects[i].GetComponent<Selectable>().uName == name) {
				Debug.Log ("SelectorScript: RhinoSelect: Adding to list", this.gameObject);
				rhinos.Add(selectedObjects[i]);
			}
		}
		ClearSelected (name);
		Debug.Log ("SelectorScript: RhinoSelect: list = ," + rhinos.ToString(), this.gameObject);
		foreach (GameObject obj in rhinos) {
			obj.GetComponent<Selectable>().SetSelected(true);
			AddSelected(obj); 
			selectedIndex = rhinos.Count;
		}
	}

	public void BuildEngineer() {
		for (int i = 0; i < selectedIndex; i++) {
			selectedObjects[i].GetComponent<Building>().addToBuildList("Engineer");
		}
	}
}
