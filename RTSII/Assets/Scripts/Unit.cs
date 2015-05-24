using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Unit : Selectable {
	public float moveSpeed;
	public float turnSpeed;
	public float flyingHeight = 25f;
	public bool flying;

	public Text text1;
	public Text text2;

	bool moving, turning;
	NavMeshAgent nav;
	Vector3 destination;
	Rigidbody rigidbody;
	// Use this for initialization
	protected override void Start () {
		rigidbody = GetComponent<Rigidbody> ();
		if (!flying) {
			nav = GetComponent<NavMeshAgent> ();
			nav.updateRotation = true;
			nav.speed = moveSpeed;
			nav.angularSpeed = turnSpeed;
		}
		base.Start ();
		moving = false;
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		base.Update ();
		if (flying) { 
			Debug.DrawRay(destination, Vector3.up * 10, Color.white, 10f, false);
			text1.text = "Destination - position " + (destination -rigidbody.position).magnitude.ToString();
			text2.text = "Position: " + rigidbody.position.ToString ();
		}

		//check if the unit has reached its destination
		if ((rigidbody.position - destination).magnitude <= 1f) { 
			moving = false;
			turning = false;
			if (flying) {
				rigidbody.velocity = Vector3.zero;
			}
		}
		if (flying && turning) {
			Vector3 targetDir = destination - transform.position;
			float angle = Vector3.Angle (targetDir, transform.forward);
			if (angle < 45.0f) {
				moving = true;
			}
		} else if (!flying) {
			if (moving) {
				/*
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
				*/
			}

		}
	}

	void FixedUpdate() {
		if (flying) {
			if (turning) {
				// Turn to face the right direction
				Vector3 targetDir = destination - transform.position;
				Vector3 localTarget = transform.InverseTransformPoint (destination);
				
				float angle = Mathf.Atan2 (localTarget.x, localTarget.z) * Mathf.Rad2Deg;
				Vector3 eulerAngleVelocity = new Vector3 (0, angle, 0);
				Quaternion deltaRotation = Quaternion.Euler (eulerAngleVelocity * turnSpeed * Time.deltaTime);
				rigidbody.MoveRotation (rigidbody.rotation * deltaRotation);
			} if (moving) {
				Vector3 targetDir = (destination - transform.position).normalized * moveSpeed;
				rigidbody.velocity = targetDir;

			}
		}
	}

	public virtual void SetDestination(Vector3 d) {
		if (flying) {
			Vector3 startPosition = rigidbody.position - SelectorScript.GetAirGroupCenter();
			destination = new Vector3 (d.x, d.y + 18, d.z) + startPosition;
			turning = true;
		} else {
			Vector3 startPosition = rigidbody.position - SelectorScript.GetLandGroupCenter();
			destination = d + startPosition;
			nav.SetDestination(destination);
			moving = true;
		}
	}
}
