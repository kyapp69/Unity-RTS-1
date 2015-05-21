using UnityEngine;
using System.Collections;

public class Rhino : Unit {


	// Use this for initialization
	protected override void Start () {
		uName = "Rhino";
		flying = false;
		maxHitPoints = 100;
		cost = 150;
		moveSpeed = 5f;
		base.Start ();
	}
	
	// Update is called once per frame
	void Update () {
		base.Update ();
	}
}
