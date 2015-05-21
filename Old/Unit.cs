using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Unit : MonoBehaviour {
	public int team = 1;
	public int hp = 100;
	public int cost = 100;
	public	bool building;
	public bool flying;
	public string name = "Rhino";
	public Material enemyMat;
	public float targetSearchTime = 1.0f; //<how frequently to search for target, in seconds
	public int range = 25;
	public float reloadTime = 1.5f;
	public GameObject RhinoShot;

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
	float targetSearchTimer;
	Transform target;
	float rangeToTarget;
	float reloadTimer;
	Transform fireLocation;
	Transform gun;
	Rigidbody rb;

	// Use this for initialization
	void Start () {
		selected = false;
		camera = GameObject.Find ("Main Camera").GetComponent<RTSCamera>();
		rb = GetComponent<Rigidbody> ();
		if (name == "Rhino") {
			fireLocation = transform.Find ("medium 1").Find ("mediumvehicle_gun").Find("gunmount").Find ("gun_fx_mount");
			gun = transform.Find ("medium 1").Find ("mediumvehicle_gun").Find("gunmount");
		}
		moving = false;
		invalidPosition = new Vector3(-99999,-99999,-99999);
		if (!building) {
			nav = GetComponent<NavMeshAgent> ();
			nav.updateRotation = true;
		}
		anim = GetComponentInChildren<Animator> ();
		if (team == 2) {
			gameObject.GetComponentInChildren<Renderer>().material = enemyMat;
			gameObject.tag = "Enemy";
		}
		targetSearchTimer = 0f;
		target = null; //<null if there is no target in range
		reloadTimer = 0f;
	}

	void Update () {
		//target things
		targetSearchTimer += Time.deltaTime;
		reloadTimer += Time.deltaTime;
		if (target && team != 2) {
			rangeToTarget = (target.position - transform.position).magnitude;
			if (rangeToTarget < range) {
				nav.enabled =false;
				Vector3 targetDirHor = new Vector3(target.position.x, transform.position.y, target.position.z) - transform.position;
				float angleHor = Vector3.Angle (targetDirHor, transform.forward);
				Vector3 targetDirVert = target.position - gun.position;
				float angleVert = Vector3.Angle (targetDirVert, gun.forward);
				if (angleHor < 30 && angleVert < 30 && reloadTimer > reloadTime) {
					moving = false;
					//fire 
					GameObject shot = (GameObject)Instantiate(RhinoShot, fireLocation.position, gun.rotation);
					shot.GetComponent<RhinoShotScript>().team = team;
					reloadTimer = 0f;
				}
				if (angleHor > 1) {
					//turn toward target
					Quaternion lookAtRotation = Quaternion.LookRotation(new Vector3(target.position.x, transform.position.y, target.position.z) - transform.position);
					// Will assume you mean to divide by damping meanings it will take damping seconds to face target assuming it doesn't move
					transform.rotation = Quaternion.Slerp(transform.rotation, lookAtRotation, Time.deltaTime/1.0f);
				}
				if (angleVert > 1) {
					Quaternion lookAtRotation = Quaternion.LookRotation(target.position - gun.position);
					gun.rotation = Quaternion.Slerp(gun.rotation, lookAtRotation, Time.deltaTime/1.0f);
				}
			} else {
				nav.enabled = true;
			}
		}
		//find targets in range
		//only looks for target every second if it does not already have a target in range.
		if (targetSearchTimer > targetSearchTime && team == 1 && !building && rangeToTarget < range) {
			targetSearchTimer = 0f;
			GameObject[] targets = GameObject.FindGameObjectsWithTag ("Enemy");
			int minRangeIndex = -1;
			float minRange = Mathf.Infinity;
			for (int i = 0; i < targets.Length; i++) {
				float targetRange = (targets [i].GetComponent<Transform> ().position - transform.position).magnitude;
				if (targetRange < minRange){
					minRange = targetRange;
					minRangeIndex = i;
				}
			}
			if (minRange < range) {
				target = targets[minRangeIndex].GetComponent<Transform>();
			}
			else { //<There is no target in range
				target = null;
			}
		}



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
				} else {					nav.speed = 2;
				}
			}
		}

		//if right mouse button is clicked, set the mouse position as the destination for the nav mesh agent.
		if (selected && Input.GetMouseButtonDown(1) && !building && Input.mousePosition.y > RTSCamera.mouseYLowerBound && team == 1) {
			if (moving && nav.enabled == true){
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
		if (GetComponentInChildren<Renderer>().isVisible && Input.GetMouseButtonUp (0) && RTSCamera.selecting && team == 1) {
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
		if (Input.GetMouseButtonDown (0) && selected && team == 1) {
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
		if (team == 1) {
			selectionBox.color = color;
		}
	}

	//sets the nav mesh agent destination and sets moving to true.
	void MakeMove() {
		if (nav.enabled == true) {
			nav.SetDestination (destination);
			moving = true;
		}
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

	public Transform GetTarget() {
		return target;
	}

	public void Damage(int amount) {
		Debug.Log ("Unit: Damage: amount: " + amount, this.gameObject);
		hp -= amount;
		if (hp <= 0) {
			Destroy (this.gameObject);
		}
	}

	void OnTriggerEnter(Collider other) {
		Debug.Log ("Unit: OnTriggerEnter", this.gameObject);
	}
}
