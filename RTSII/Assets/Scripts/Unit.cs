using UnityEngine;
using System.Collections;

public class Unit : Selectable {
	public float moveSpeed;
	public float turnSpeed;
	public float flyingHeight = 25f;
	public bool flying;

	bool moving;
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
		if (transform.position == destination) {
			moving = false;
		}
		base.Update ();
		if (flying) {
			if (moving) {
				// Turn to face the right direction
				Vector3 targetPos = new Vector3(destination.x, destination.y+25, destination.z);
				Vector3 targetDir = targetPos - transform.position;
				Vector3 localTarget = transform.InverseTransformPoint(targetPos);
				
				float angle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;
				Vector3 eulerAngleVelocity = new Vector3 (0, angle, 0);
				Quaternion deltaRotation = Quaternion.Euler(eulerAngleVelocity * Time.deltaTime );
				rigidbody.MoveRotation(rigidbody.rotation * deltaRotation);
			}
		} else {
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
	}

	public virtual void SetDestination(Vector3 d) {
		destination = d;
		if (flying) {
			
		} else {
			nav.SetDestination(destination);
		}
		moving = true;
	}
}
