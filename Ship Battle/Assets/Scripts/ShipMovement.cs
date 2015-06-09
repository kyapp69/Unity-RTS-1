using UnityEngine;
using System.Collections;

public class ShipMovement : MonoBehaviour {
	public float maxSpeed = 5f;
	public float maxTurnSpeed = 5f;
	public float acceleration = 0.75f;

	//public Text text1;
	//public Text text2;

	bool moving;
	bool turning;
	bool decelerating;
	bool limitSpeed;
	Vector3 destination;
	Rigidbody rigidbody;
	float speed;
	float turnSpeed;
	// Use this for initialization
	void Start () {
		moving = false;
		turning = false;
		decelerating = false;
		destination = Vector3.zero;
		rigidbody = GetComponent<Rigidbody> ();
		speed = 0f;
		turnSpeed = 0f;
		limitSpeed = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (moving)
		//the 1f should be replaced with the distance needed to decelerate to a stop
		if ((rigidbody.position - destination).magnitude <= .5f) { 
			Stop ();
		}

		//display current position and destination 
		//text1.text = "Current Position: " + transform.position.ToString() + " Decelerating: " + decelerating+ "Speed: " + speed;
		//text2.text = "Destination: " + destination.ToString () + " Distance: " + (transform.position - destination).magnitude;
	}

	void FixedUpdate() {
		if (turning) {
			turnSpeed = maxTurnSpeed * (speed / maxSpeed);
			float targetDistance = (transform.position - destination).magnitude;
			if (targetDistance < 5 || limitSpeed) {
				turnSpeed *= 2f;
			}
			Vector3 targetDirection = destination - transform.position;
			Vector3 localTarget = transform.InverseTransformPoint (destination);
			  
			float angle = Mathf.Atan2 (localTarget.x, localTarget.z) * Mathf.Rad2Deg;
			if (speed > .5f){
				Vector3 eulerAngleVelocity = new Vector3 (0, angle, 0);
				Quaternion deltaRotation = Quaternion.Euler (eulerAngleVelocity * turnSpeed * Time.deltaTime);
				rigidbody.MoveRotation (rigidbody.rotation * deltaRotation);
			}
			bool acc = false;
			bool dec = false;
			//move towards destination
			float distance = - Mathf.Pow(speed, 2) / (2 * -acceleration);
			if (targetDistance <= distance) { 
				decelerating = true;
			} else
				decelerating = false;

			if (angle > 90f && !decelerating) {
				if (speed < maxSpeed / 5) {
					acc = true;
				} else 
					dec = true;
			} else if (angle > 45f && !decelerating) {
				if (speed < maxSpeed / 4) {
					acc = true;
				} else
					dec = true;
			} else if (angle < 15f && speed < maxSpeed && !decelerating)
				acc = true;
			else if (decelerating)
				dec = true;
		
			if (limitSpeed) {
				if (speed > maxSpeed / 7) {
					acc = false;
					dec = true;
				} else { 
					acc = true;
					dec = false;
				}
			}

			if (acc & !decelerating) {
				speed += acceleration * Time.deltaTime;
			} else if (dec || decelerating) {
				speed -= acceleration * Time.deltaTime;
			}

			Vector3 targetVelocity = (transform.forward).normalized * speed;
			rigidbody.velocity = targetVelocity;
		}
	}

	void Stop() {
		moving = false;
		decelerating = false;
		turning = false;
		rigidbody.velocity = Vector3.zero;
	}

	public void SetDestination(Vector3 d) {
		float distance = (transform.position - d).magnitude;
		Vector3 targetDirection = d - transform.position;
		Vector3 localTarget = transform.InverseTransformPoint (d);
		float angle = Mathf.Atan2 (localTarget.x, localTarget.z) * Mathf.Rad2Deg;
		if (angle < 10) {
			limitSpeed = false;
			destination = d;
			moving = true;
			turning = true;
			decelerating = false;
		} else if (distance > 15) {
			destination = d;
			if (distance <= 20) {
				limitSpeed = true;
			} else 
				limitSpeed = false;
			moving = true;
			turning = true;
			decelerating = false;
		}
	}

	public bool IsMoving() {
		return moving;
	}

	public bool IsTurning() {
		return turning;
	}

	public bool IsDecelerating() {
		return decelerating;
	}
}
