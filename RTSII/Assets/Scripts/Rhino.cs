using UnityEngine;
using System.Collections;

public class Rhino : Unit {


	// Use this for initialization
	protected override void Start () {
		uName = "Rhino";
		maxHitPoints = 100;
		cost = 150;
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
