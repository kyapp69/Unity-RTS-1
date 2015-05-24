using UnityEngine;
using System.Collections;

public class OreCluster : Selectable {
	public int maxOre = 1000;

	int remainingOre;
	// Use this for initialization
	void Start () {
		name = "Ore Cluster";
		team = 0;
		maxHitPoints = 1;
		remainingOre = maxOre;
		base.Start();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Mine(int amount){
		remainingOre -= amount;
	}
}
