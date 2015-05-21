﻿using UnityEngine;
using System.Collections;

public class SelectorScript : MonoBehaviour {
	public static Rect selection = new Rect(0,0,0,0);
	public static bool selecting;
	public Texture2D selectionHighlight;

	Vector3 startPosition;
	GameObject[] selectedObjects;
	int selectedIndex;

	// Use this for initialization
	void Start () {
		startPosition = -Vector3.one;
		selecting = false;
		selectedObjects = new GameObject[100];
		selectedIndex = 0;
	}
	
	// Update is called once per frame
	void Update () {
		MouseActivity();
	}

	void OnGUI() {
		if (startPosition != -Vector3.one && selecting) {
			GUI.color = new Color(1,1,1,0.3f);
			GUI.DrawTexture(selection, selectionHighlight);
		}
	}

	void MouseActivity() {
		if (Input.GetMouseButtonDown (0)) {
			LeftMouseClick();
		} else if (Input.GetMouseButtonDown (1)) {
			//RightMouseClick();
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
		ClearSelected ();
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
	
	void ClearSelected() {
		//clear array of selected objects
		if (selectedObjects [0]) {
			Debug.Log ("SelectorScript: ClearSelected: number of selected objects: " + selectedIndex, this.gameObject);
			for (int i = 0; i <= selectedIndex; i++) {
				Debug.Log ("SelectorScript: ClearSelected: clearing: " + selectedObjects[i].name, this.gameObject);
				selectedObjects [i].GetComponent<Selectable> ().SetSelected (false);
				selectedObjects [i] = null;
			}
			selectedIndex = 0;
		}
	}

	void ResetSelectionBox() {
		selecting = false;
		startPosition = -Vector3.one;
		selection = new Rect(0,0,0,0);
	}
	
	public static float InvertMouseY(float y) {
		return Screen.height - y;
	}

}
