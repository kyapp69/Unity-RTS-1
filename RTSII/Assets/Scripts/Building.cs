using UnityEngine;
using System.Collections.Generic;

public class Building : Selectable {
	public List<BuildQueueItem> buildQueue;
	public GameObject rhino;
	public GameObject engineer;
	public GameObject gunship;

	bool constructing;
	float buildTimer;
	Vector3 spawnPosition;
	BuildQueueItem currentBuild;
	// Use this for initialization
	protected override void Start () {
		base.Start ();
		constructing = false;
		buildQueue = new List<BuildQueueItem> ();
		spawnPosition = new Vector3 (transform.position.x + 1.5f, transform.position.y, transform.position.z);
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();
		if (constructing) {
			buildTimer += Time.deltaTime;
			if (buildTimer >= currentBuild.buildTime) {
				CompleteBuilding ();
			}
		} else {
			//start building the next thing in the build queue
			if (buildQueue.Count > 0) {
				StartBuilding();
			}
		}
	}

	public void StartBuilding() {
		constructing = true;
		buildTimer = 0f;
		currentBuild = buildQueue[0];
		buildQueue.RemoveAt (0);
	}

	public void CompleteBuilding() {
		constructing = false;
		//place the unit in the game world
		if (currentBuild.name == "Rhino") {
			Instantiate (rhino, spawnPosition, transform.rotation);
		} else if (currentBuild.name == "Engineer") {
			Instantiate (engineer, spawnPosition, transform.rotation);
		} else if (currentBuild.name == "Gunship") {
			Instantiate(gunship, new Vector3(transform.position.x, transform.position.y + 15, transform.position.z), transform.rotation);
		}
	}

	public void addToBuildList(string name){
		float time = 0f;
		if (uName == "Colony") {
			if (name == "Engineer") {
				time = 20f;
			}
		} else if (uName == "Factory") {
			if (name == "Rhino") {
				time = 40f;
			} else if (name == "Gunship") {
				time = 45f;
			}
		}

		if (time != 0f){
			BuildQueueItem bUI = new BuildQueueItem (name, time);
			buildQueue.Add (bUI);
		}
	}
}
