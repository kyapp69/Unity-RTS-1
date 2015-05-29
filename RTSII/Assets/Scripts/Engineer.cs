using UnityEngine;
using System.Collections;

public class Engineer : Unit {
	public int oreCapacity = 10;
	public float miningTime = 0.5f; //half second per ore

	int currentOre;
	bool mining;
	bool collecting;
	Transform ore;
	float timer;
	LineRenderer lineRenderer;
	// Use this for initialization
	protected override void Start () {
		uName = "Engineer";
		maxHitPoints = 25;
		cost = 30;
		moveSpeed = 5f;
		turnSpeed = 120f;
		currentOre = 0;
		flying = false;   
		base.Start ();
		mining = false;
		collecting = false;
		lineRenderer = GetComponent<LineRenderer> ();
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();
		if (collecting && (ore.position - GetComponent<Transform> ().position).magnitude <= 5) {
			if (mining) {
				timer += Time.deltaTime;
				if (timer >= miningTime) {
					currentOre++;
					ore.GetComponent<OreCluster>().Mine(1);
					if (currentOre == oreCapacity) {
						//go to nearest team colony
					}
				}
			} else {
				mining = true;
				//display mining effects
				lineRenderer.enabled = true;
				//lineRenderer.SetPosition(1, )
			}
		}
	}

	public void startMining(Transform o) {
		ore = o;
		collecting = true;
		SetDestination (ore.position);
	}
}
