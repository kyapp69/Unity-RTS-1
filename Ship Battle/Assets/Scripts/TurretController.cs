using UnityEngine;
using System.Collections;

public class TurretController : MonoBehaviour {
	float rightRotation = 135f;
	float leftRotation = 225f;
	float elevMax = 30f;
	float elevMin = -5f;
	float turnRate = 0.5f;
	float elevRate = 0.5f;

	bool turning;
	Transform transform;
	float rotation;
	// Use this for initialization
	void Start () {
		turning = false;
		transform = GetComponent<Transform> ();
		rotation = 0;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 angles = transform.eulerAngles;
		if (Input.GetKey (KeyCode.RightArrow) && (angles.y < rightRotation || angles.y > leftRotation - 1f)) {
			transform.eulerAngles = new Vector3(0, angles.y + turnRate, 0);
		}
		if (Input.GetKey (KeyCode.LeftArrow) && (angles.y > leftRotation || angles.y < rightRotation + 1f)) {
			transform.eulerAngles = new Vector3(0, angles.y - turnRate, 0);
		}
	}
}
