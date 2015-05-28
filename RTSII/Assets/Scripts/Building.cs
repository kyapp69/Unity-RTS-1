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
	Transform buildBar;
	// Use this for initialization
	protected override void Start () {
		base.Start ();
		constructing = false;
		buildQueue = new List<BuildQueueItem> ();
		buildBar = GetComponentInChildren<BuildBar> ().GetComponent<Transform> ();
		buildBar.localScale = new Vector3 (buildBar.localScale.x, 0.01f, buildBar.localScale.z);
		spawnPosition = new Vector3 (transform.position.x + 4f, transform.position.y, transform.position.z);
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();
		if (constructing) {
			buildTimer += Time.deltaTime;
			buildBar.localScale = new Vector3(buildBar.localScale.x, (buildTimer / currentBuild.buildTime) / 7, buildBar.localScale.z);
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
	}

	public void CompleteBuilding() {
		//place the unit in the game world
		if (currentBuild.objectName == "Rhino") {
			Instantiate (rhino, spawnPosition, transform.rotation);
		} else if (currentBuild.objectName == "Engineer") {
			Instantiate (engineer, spawnPosition, transform.rotation);
		} else if (currentBuild.objectName == "Gunship") {
			Instantiate(gunship, new Vector3(transform.position.x, transform.position.y + 15, transform.position.z), transform.rotation);
		}
		buildBar.localScale = new Vector3 (buildBar.localScale.x, 0.01f, buildBar.localScale.z);
		buildQueue.RemoveAt (0);
		constructing = false;
	}

	public void addToBuildList(string name){
		float time = 0f;
		if (uName == "Colony") {
			if (name == "Engineer" && 30 <= SelectorScript.ore) {
				SelectorScript.ore -= 30;
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
