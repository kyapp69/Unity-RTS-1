using UnityEngine;
using System.Collections.Generic;

public class TargetAquisitionSystem : MonoBehaviour {
	public float radarRange = 100f;
	//public float sonarRange = 50f; //not used currently
	public float scanTime = 2f;

	float scanTimer;
	LayerMask layer;
	int team;
	FireControlSystem fcs;
	List<UnitController> shipTargets;
	// Use this for initialization
	void Start () {
		scanTimer = 0f;
		layer = 8;
		team = GetComponent<UnitController> ().team;
		shipTargets = new List<UnitController> ();
		fcs = GetComponent<FireControlSystem> ();
	}

	void Update() {
		scanTimer += Time.deltaTime;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (scanTimer >= scanTime) {
			Scan ();
			scanTimer = 0f;
		}
	}

	void Scan() {
		shipTargets.Clear ();
		Collider[] possibleTargets = Physics.OverlapSphere (transform.position, radarRange, layer);
		foreach (Collider c in possibleTargets) {
			UnitController uc = c.GetComponent<UnitController>();
			if (uc) {
				if (uc.team != team) {
					shipTargets.Add(uc);
				}
			}
		}
		//call method in fcs
		fcs.UpdateShipTargetList (shipTargets);
	}
}
