using UnityEngine;
using System.Collections;

public class healthBar : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		Vector3 targetDir = new Vector3 (Camera.main.GetComponent<Transform> ().position.x, Camera.main.GetComponent<Transform> ().position.y, transform.position.z);
		transform.LookAt (targetDir, Vector3.forward);
	}
}
