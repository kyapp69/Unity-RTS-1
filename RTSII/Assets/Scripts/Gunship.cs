using UnityEngine;
using System.Collections;

public class Gunship : Unit {

	// Use this for initialization
	protected override void Start () {
		uName = "Gunship";
		flying = true;
		maxHitPoints = 75;
		cost = 175;
		moveSpeed = 7f;
		base.Start ();
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();
	}
}
