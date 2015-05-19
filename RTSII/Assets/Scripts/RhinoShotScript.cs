using UnityEngine;
using System.Collections;

public class RhinoShotScript : MonoBehaviour {
	public float shotSpeed = 10f;
	public int team;
	public int damage = 10;

	Vector3 startPosition;
	// Use this for initialization
	void Start () {
		startPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (Vector3.up* shotSpeed * Time.deltaTime);
		if ((transform.position - startPosition).magnitude > 25) {
			Destroy(this.gameObject);
		}
	}
}
