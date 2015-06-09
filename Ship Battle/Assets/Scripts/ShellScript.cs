using UnityEngine;
using System.Collections;

public class ShellScript : MonoBehaviour {
	public float shellSpeed = 1f;

	Rigidbody rb;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		Destroy (this.gameObject, 5f);
		rb.AddForce (transform.forward.normalized * shellSpeed, ForceMode.Impulse);
	}
}
