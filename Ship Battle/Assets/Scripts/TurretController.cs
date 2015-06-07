using UnityEngine;
using System.Collections;

public class TurretController : MonoBehaviour {
	public float rightRotation = 135f;
	public float leftRotation = 225f;
	public float elevMax = 320f;
	public float elevMin = 2f; 
	public float turnRate = 0.5f;
	public float elevRate = 0.5f;
	public string turretType = "TurretLvl1";
	public float reloadTime = 2.5f;
	public GameObject shell;

	bool turning;
	Transform transform;
	float rotation;
	Transform gunBase;
	float reloadTimer; //also used as the recoil timer
	Transform gun;
	float firePoint;
	// Use this for initialization
	void Start () {
		turning = false;
		transform = GetComponent<Transform> ();
		rotation = 0;
		if (turretType == "TurretLvl1") {
			gunBase = transform.Find("BarrelLvl1Base").GetComponent<Transform>();
			gun = gunBase.Find("BarrelLvl1Animated").GetComponent<Transform>();
			firePoint = 0.2f;
		}
		reloadTimer = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		reloadTimer += Time.deltaTime;
		Vector3 angles = transform.eulerAngles;
		Vector3 gunAngle = gunBase.eulerAngles;
		if (Input.GetKey (KeyCode.RightArrow) && (angles.y < rightRotation || angles.y > leftRotation - 1f)) {
			transform.eulerAngles = new Vector3(0, angles.y + turnRate, 0);
		}
		if (Input.GetKey (KeyCode.LeftArrow) && (angles.y > leftRotation || angles.y < rightRotation + 1f)) {
			transform.eulerAngles = new Vector3(0, angles.y - turnRate, 0);
		}
		if (Input.GetKey (KeyCode.UpArrow) && (gunAngle.x > elevMax || gunAngle.x < elevMin + 1f)) {
			gunBase.eulerAngles = new Vector3(gunAngle.x - elevRate, angles.y, 0);
		}
		if (Input.GetKey (KeyCode.DownArrow) && (gunAngle.x < elevMin || gunAngle.x > elevMax - 1f)) {
			gunBase.eulerAngles = new Vector3(gunAngle.x + elevRate, angles.y, 0);
		}
		if (Input.GetKey (KeyCode.Space) && reloadTimer >= reloadTime) {
			Fire ();
		}
	}

	void Fire() {
		Vector3 gunAngle = gun.eulerAngles;
		Instantiate(shell, new Vector3(gun.position.x, gun.position.y, gun.position.z + firePoint), Quaternion.Euler(gunAngle.x, gunAngle.z, gunAngle.y));
	}
}
