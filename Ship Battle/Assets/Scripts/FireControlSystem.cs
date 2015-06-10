using UnityEngine;
using System.Collections.Generic;

public class FireControlSystem : MonoBehaviour {
	
	List<TurretController> guns;
	List<TurretController> ciws;
	//array for missile launchers
	//array for aa missile turrets

	//Target lists
	List<UnitController> shipTargets;

	// Use this for initialization
	void Start () {
		shipTargets = new List<UnitController> ();
		guns = new List<TurretController> ();
		//find weapons
		TurretController[] turrets = GetComponentsInChildren<TurretController> ();//GetComponent<Transform>().Find("Weapons").GetComponentsInChildren<TurretController> ();
		foreach (TurretController t in turrets) {
			guns.Add(t);
		}
		foreach (TurretController t in guns) {
			t.SetTarget(GameObject.Find("target").GetComponent<Transform>());
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void UpdateShipTargetList(List<UnitController> t){
		shipTargets = t;
		//assign a threat index to each target

	}
}
