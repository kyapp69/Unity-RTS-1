using UnityEngine;
using System.Collections;

public class Gunship : Unit {
	// Use this for initialization
	protected override void Start () {
		uName = "Gunship";
		maxHitPoints = 75;
		cost = 175;
		moveSpeed = 9f;
		turnSpeed = 2f;
		flying = true;
		base.Start ();
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();

	}
}
