using UnityEngine;
using System.Collections;

public class RhinoShotScript : MonoBehaviour {
	public float shotSpeed = 105f;
	public int team;
	public int damage = 25;

	Vector3 startPosition;
	Rigidbody rb;
	// Use this for initialization
	void Start () {
		startPosition = transform.position;
		rb = GetComponent<Rigidbody> ();
		rb.AddForce (transform.forward * shotSpeed, ForceMode.VelocityChange);
	}
	
	// Update is called once per frame
	void Update () {
		if ((transform.position - startPosition).magnitude > 25) {
			Destroy(this.gameObject); 
		}
	}

	void OnTriggerEnter(Collider other) {
	//	Unit hitUnit = other.gameObject.GetComponent<Unit> ();
		//if (hitUnit && hitUnit.team != team) {
		//	hitUnit.Damage (damage);
		//}
//		if ((transform.position - startPosition).magnitude > 3) {
			//Destroy (this.gameObject);
		//}
	}
}
