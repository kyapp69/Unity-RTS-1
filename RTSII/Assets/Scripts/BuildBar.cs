using UnityEngine;
using System.Collections;

public class BuildBar : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 targetDir = new Vector3 (Camera.main.GetComponent<Transform> ().position.x, transform.position.y, Camera.main.GetComponent<Transform> ().position.z);
		transform.LookAt (targetDir, Vector3.forward);
	}
}
