using UnityEngine;
using System.Collections;

public class ShellScript : MonoBehaviour {
	public float shellSpeed = 10f;

	Rigidbody rb;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		Destroy (this.gameObject, 5f);
	}
	
	// Update is called once per frame
	void Update () {
		rb.velocity = transform.forward * shellSpeed * Time.deltaTime;
	}
}
