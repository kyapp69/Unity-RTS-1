using UnityEngine;
using System.Collections;

public class AirMovement : Movement {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void SetDestination(Vector3 d) {
		base.SetDestination (d);
		corners[0] = d;
	}
}
