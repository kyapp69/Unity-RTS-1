using UnityEngine;
using System.Collections;

public class LandMovement : Movement {

	NavMeshAgent nav;
	// Use this for initialization
	void Start () {
		nav = GetComponent<NavMeshAgent> ();
		base.Start ();
	}
	
	// Update is called once per frame
	void Update () {
		base.Update ();
	}

	void FixedUpdate() {
		
	}

	public override void SetDestination(Vector3 d) {
		base.SetDestination (d);
		corners = nav.path.corners;
	}
}
