using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
	public float moveSpeed = 5f;
	public float turnSpeed = 45f;
	public Vector3[] corners;

	bool moving;
	Vector3 destination;
	Rigidbody rigidbody;
	// Use this for initialization
	protected virtual void Start () {
		moving = false;
		rigidbody = GetComponent<Rigidbody> ();
		corners = new Vector3[1];
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		//check if reached destination
		if ((rigidbody.position - destination).magnitude <= 1f) { 
			Stop ();
		}
	}

	public virtual void SetDestination(Vector3 d) {
		destination = d;
	}

	public void Stop() {
		moving = false;
		rigidbody.velocity = Vector3.zero;
	}

	public bool IsMoving() {
		return moving;
	}
}
