using UnityEngine;
using System.Collections;

public class Unit : Selectable {
	public float moveSpeed;
	public float turnSpeed;
	public bool flying;
	public float flyingHeight = 25f;

	bool moving;
	NavMeshAgent nav;
	// Use this for initialization
	protected override void Start () {
		base.Start ();
		moving = false;
		nav = GetComponent<NavMeshAgent> ();
		nav.updateRotation = true;
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		base.Update ();
	}

	public void SetDestination(Vector3 destination) {
		if (flying) {
			nav.SetDestination(new Vector3(destination.x, destination.y + flyingHeight, destination.z));
		} else {
			nav.SetDestination(destination);
		}
	}
}
