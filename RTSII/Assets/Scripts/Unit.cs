using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Unit : MonoBehaviour {
	public int team = 1;
	public	bool building;

	//test variables
	public Text testText1;
	public Text testText2;
	

	RTSCamera camera;
	bool selected, moving;
	Vector3 destination;
	Vector3 invalidPosition;
	NavMeshAgent nav;
	Vector3 startPosition; //<used for off position for group movement
	Animator anim;

	// Use this for initialization
	void Start () {
		selected = false;
		camera = GameObject.Find ("Main Camera").GetComponent<RTSCamera>();
		moving = false;
		invalidPosition = new Vector3(-99999,-99999,-99999);
		if (!building) {
			nav = GetComponent<NavMeshAgent> ();
			nav.updateRotation = true;
		}
		anim = GetComponentInChildren<Animator> ();
	}

	void Update () {
		//handes movable unit specific things
		if (!building) {
			//set animation state
			anim.SetBool("moving", moving);
			//sets moving to false if reached target destination
			if (transform.position == destination) {
				moving = false;
			}
			if (moving) {
				//turns slowly until facing is within tolerance of desired facing
				Vector3 targetDir = new Vector3(nav.steeringTarget.x, transform.position.y, nav.steeringTarget.z) - transform.position;
				float angle = Vector3.Angle (targetDir, transform.forward);
				if (angle < 5.0f) {
					nav.speed = 5f;
				} else if (angle < 45.0f) {
					nav.speed = 3.5f;
				} else if (angle > 150) {
					nav.speed = 0.5f;
				} else {
					nav.speed = 2;
				}
			}
		}

		//if right mouse button is clicked, set the mouse position as the destination for the nav mesh agent.
		if (selected && Input.GetMouseButtonDown(1) && !building && Input.mousePosition.y > 125) {
			if (moving){
				nav.SetDestination(transform.position);
			}
			startPosition = transform.position - camera.GetGroupCenter();
			Vector3 hitPoint = FindHitPoint();
			if (hitPoint != null) {
				float x = hitPoint.x;
				float y = hitPoint.y;
				float z = hitPoint.z;
				destination = new Vector3 (x, y ,z) + startPosition;
				MakeMove ();
			}
		}

		//If the unit is within the selection drag box when the left mouse button is released then select the unit
		if (GetComponentInChildren<Renderer>().isVisible && Input.GetMouseButtonUp (0) && RTSCamera.selecting) {
			Vector3 camPos = Camera.main.WorldToScreenPoint(transform.position);
			camPos.y = RTSCamera.InvertMouseY(camPos.y);
			bool s;
			if (RTSCamera.selection.width > 0 && RTSCamera.selection.height > 0) {
				s = RTSCamera.selection.Contains (camPos);
			} else {
				s = new Rect(camera.mousePosX, RTSCamera.InvertMouseY(camera.mousePosY), -RTSCamera.selection.width, -RTSCamera.selection.height).Contains(camPos);
			}
			if (s != selected){
				setSelected();
			}
		}

		//unselects the unit if the left mouse button is clicked, can be reselected in the same click
		if (Input.GetMouseButtonDown (0) && selected ) {
			GameObject hitObject = FindHitObject();
			if (hitObject != transform.gameObject) {
				setSelected();
			}
		}
	}

	//finds the object under the mouse
	GameObject FindHitObject() {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit))
			return hit.collider.gameObject;
		return null;
	}

	//finds the point under the mouse
	Vector3 FindHitPoint() {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit))
			return hit.point;
		return invalidPosition;
	}

	/*
	 * Sets the unit to selected if it isnt or unselects it if it is already selected
	 */ 
	public void setSelected() {
		//controls the opacity of the selection box indicator
		SpriteRenderer selectionBox = transform.Find("SelectionBox").gameObject.GetComponent<SpriteRenderer>();
		Color color = selectionBox.color;
		if (selected) {
			selected = false;
			color.a = 0;
		} else {
			selected = true;
			color.a = 255;
			camera.addSelected(transform.gameObject);
		}
		selectionBox.color = color;
	}

	//sets the nav mesh agent destination and sets moving to true.
	void MakeMove() {
		nav.SetDestination (destination);
		moving = true;
	} 

	public bool IsMoving() {
		return moving;
	}

	public NavMeshAgent getNavMeshAgent() {
		return nav;
	}

	public bool IsBuilding() {
		return building;
	}
}
