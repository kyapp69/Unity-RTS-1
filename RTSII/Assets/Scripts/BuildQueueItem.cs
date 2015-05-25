using UnityEngine;
using System.Collections;

public class BuildQueueItem : MonoBehaviour {
	public string objectName;
	public float buildTime;

	public BuildQueueItem(string name, float time){
		objectName = name;
		buildTime = time;
	}
}
