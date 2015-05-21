using UnityEngine;
using System.Collections;

public class Colony : Building {

	// Use this for initialization
	protected override void Start () {
		uName = "Colony";
		maxHitPoints = 1000;
		cost = 500;
		base.Start ();
	}
	
	// Update is called once per frame
	void Update () {
		base.Update ();
	}
}
