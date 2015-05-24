using UnityEngine;
using System.Collections;

public class Engineer : Unit {

	// Use this for initialization
	protected override void Start () {
		uName = "Engineer";
		maxHitPoints = 25;
		cost = 30;
		moveSpeed = 5f;
		turnSpeed = 120f;
		flying = false;
		base.Start ();
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();
	}
}
