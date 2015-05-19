using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RTSCamera : MonoBehaviour {
	public int scrollDistance = 10;
	public float zoomMin = 15f;
	public float zoomMax = 100f;
	public Texture2D selectionHightlight;
	public static Rect selection = new Rect(0,0,0,0);
	public static bool selecting;
	public float mousePosX;
	public float mousePosY;
	public static int mouseYLowerBound = 125;

	//test variables
	public Text testText; //<displays the number of selected units

	float scrollAmount;
	float x; 
	float z;
	float scrollSpeed; 
	Vector3 invalidPosition;
	Vector3 startClick = -Vector3.one;
	GameObject[] selectedObjects;
	int selectedIndex;

	// Use this for initialization
	void Start () {
		invalidPosition = new Vector3(-99999,-99999,-99999);
		selecting = false;
		selectedObjects = new GameObject[100];
		selectedIndex = 0;
	}
	
	// Update is called once per frame
	void Update () {
			MoveCamera();
			MouseActivity();
	}

	void OnGUI() {
		if (startClick != -Vector3.one && selecting) {
			GUI.color = new Color(1,1,1,0.3f);
			GUI.DrawTexture(selection, selectionHightlight);
		}
	}

	void MoveCamera() {
		scrollSpeed = 2 * Camera.main.fieldOfView;

		//get mouse position
		mousePosX = Input.mousePosition.x;
		mousePosY = Input.mousePosition.y;
		
		//get x and z coordinates of camera
		x = transform.position.x;
		z = transform.position.z;
		
		scrollAmount = scrollSpeed * Time.deltaTime;
		
		//mouse movement
		//mouse left
		if (mousePosX < scrollDistance && x > -0) {
			transform.Translate(-scrollAmount, 0, 0, Space.World);
		}
		//mouse right
		if (mousePosX >= Screen.width - scrollDistance && x < 480) {
			transform.Translate(scrollAmount, 0,0, Space.World);
		}
		//mouse down
		if (mousePosY < scrollDistance && z > -0) {
			transform.Translate(0,0, -scrollAmount, Space.World);
		}
		//mouse up
		if (mousePosY >= Screen.height - scrollDistance && z < 480) {
			transform.Translate(0,0,scrollAmount, Space.World);
		}
		
		//key movement
		//a-left
		if (Input.GetKey("a") && x > -0) {
			transform.Translate(-scrollAmount, 0, 0, Space.World);
		}
		//d-right
		if (Input.GetKey ("d") && x < 480) {
			transform.Translate(scrollAmount, 0,0, Space.World);
		}
		//s-down
		if (Input.GetKey("s") && z > -0) {
			transform.Translate(0,0, -scrollAmount, Space.World);
		}
		//w-up
		if (Input.GetKey("w") && z < 480) {
			transform.Translate(0,0,scrollAmount, Space.World);
		}

		//Zoom
		//zoom out
		if (Input.GetAxis ("Mouse ScrollWheel") < -0) {
			Camera.main.fieldOfView *= 1.1f;
		} 
		if (Input.GetAxis ("Mouse ScrollWheel") > -0) {
			Camera.main.fieldOfView *= 0.9f;
		}
		
		Camera.main.fieldOfView = Mathf.Clamp (Camera.main.fieldOfView, zoomMin, zoomMax);
		
		//Camera Rotation
		//must press ctrl and right mouse button to rotate
		if (Input.GetKey (KeyCode.LeftAlt) && Input.GetMouseButton (1)) {
			
		}
	}

	void MouseActivity() {
		//mouse interaction with game objects
		if (Input.GetMouseButtonDown (0) && mousePosY > mouseYLowerBound) {
			LeftMouseClick ();
		} else if (Input.GetMouseButtonDown (1)) {
			//RightMouseClick ();
		} else if (Input.GetMouseButtonUp (0) && selecting) {
			DragBox ();
		} else if (Input.GetMouseButton (0) && selecting) {
			selection = new Rect(startClick.x, InvertMouseY(startClick.y), Input.mousePosition.x - startClick.x, InvertMouseY(Input.mousePosition.y) - InvertMouseY(startClick.y));

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

	/*
	 * Left mouse button is used to select units
	 */
	void LeftMouseClick() {
		//clear array of selected objects
		for (int i = 0; i <= selectedIndex; i++) {
			selectedObjects[i] = null;
		}
		selectedIndex = 0;

		//determine what was clicked on
		GameObject hitObject = FindHitObject();
		Vector3 hitPoint = FindHitPoint();
		if (hitObject && hitPoint != invalidPosition) {
			Unit unit = hitObject.GetComponentInParent<Unit>(); 
			if (unit && unit.team == 1) { //<if clicked on unit -> select the unit
				unit.setSelected();
			} else if (hitObject.name == "Terrain") { //<if clicked on terrain -> begin drawing selection box
				startClick = Input.mousePosition;
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
		return invalidPosition;
	}

	/*
	 * Resets the drag selection box, the units themselves handle selection
	 */ 
	void DragBox(){
		selecting = false;
		startClick = -Vector3.one;
		selection = new Rect(0,0,0,0);
	}

	public static float InvertMouseY(float y) {
		return Screen.height - y;
	}

	public void addSelected(GameObject unit) { //<can only have 100 units selected at a time
		if (selectedObjects[99] == null) {
			selectedObjects[selectedIndex] = unit;
			selectedIndex++;
			testText.text = "number selected: " + selectedIndex;
		} else {
			unit.GetComponent<Unit>().setSelected();
		}
	}

	public Vector3 GetGroupCenter() {
		Vector3 total = new Vector3(0, 0, 0);
		int totalMovable = 0;
		for (int i = 0; i < selectedIndex; i++) {
			if (!selectedObjects[i].GetComponent<Unit>().IsBuilding()) {
				totalMovable++;
				total += selectedObjects[i].GetComponent<Transform>().position;
			}
		}
		return total / (totalMovable);
	}

	public int GetNumRhinosSelected() {
		Debug.Log ("RTSCamer: GetNumRhinos: selectedIndex: " + selectedIndex, Camera.main);
		int total = 0;
		for (int i = 0; i < selectedIndex; i++) {
			if (selectedObjects [i].GetComponent<Unit> ().name == "Rhino") {
				total++;
			} else {
				Debug.Log ("RTSCamer: GetNumRhinos: name = " + selectedObjects [i].GetComponent<Unit> ().name ,Camera.main);
			}
		}
		Debug.Log ("RTSCamer: GetNumRhinos: total = " + total, Camera.main);
		return total;
	}

}
