using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TurretController : MonoBehaviour {
	public float rightRotation = 135f;
	public float leftRotation = 225f;
	public float elevMax = 315f;
	public float elevMin = 2f; 
	public float turnRate = 0.5f;
	public float elevRate = 0.5f;
	public string turretType = "TurretLvl1";
	public float reloadTime = 2.5f;
	public GameObject shell;
	public float shellVelocity = 15f;

	public Text text1;
	public Text text2;

	bool turning;
	Transform transform;
	float rotation;
	Transform gunBase;
	float reloadTimer;
	Transform gun;
	float firePoint;
	Transform target;
	float maxRange;
	// Use this for initialization
	void Start () {
		turning = false;
		transform = GetComponent<Transform> ();
		target = null;
		rotation = 0;
		if (turretType == "TurretLvl1") {
			gunBase = transform.Find("BarrelLvl1Base").GetComponent<Transform>();
			gun = gunBase.Find("BarrelLvl1Animated").GetComponent<Transform>();
			firePoint = 0.2f;
		}
		reloadTimer = 0f;
		maxRange = (shellVelocity * shellVelocity) / 9.81f; 
	}
	
	// Update is called once per frame
	void Update () {
		reloadTimer += Time.deltaTime;
		Vector3 angles = transform.localEulerAngles;
		Vector3 gunAngle = gunBase.localEulerAngles;
		Vector3 turretFacing = transform.forward;

		text1.text = "Gun direction: " + turretFacing;

		//find angle to target
		if (target) {
			Vector3 targetHeading = target.position - transform.position;
			float targetDistance = targetHeading.magnitude;
			Vector3 targetDirection = transform.InverseTransformPoint(target.position);

			text2.text = "Target direction: " + targetDirection.ToString();

			//Turn turret to face target
			if (targetDirection.x < -0.1  && (angles.y > leftRotation || angles.y < rightRotation + 1f)){
				//turn left
				transform.localEulerAngles = new Vector3(0, angles.y - turnRate, 0);
			} else if (targetDirection.x > 0.1 && (angles.y < rightRotation || angles.y > leftRotation - 1f)) {
				//turn right
				transform.localEulerAngles = new Vector3(0, angles.y + turnRate, 0);
			}

			//angle gun to firing angle
			float desiredAngle = (1/2) * Mathf.Asin((9.81f * targetDistance)/(shellVelocity * shellVelocity));
			text2.text = "Target Distance: " + targetDistance + " Desired Angle=" + desiredAngle;
		}



		/*
		float angle = Mathf.Atan2 (localTarget.x, localTarget.z) * Mathf.Rad2Deg;
		if (speed > .5f){
			Vector3 eulerAngleVelocity = new Vector3 (0, angle, 0);
			Quaternion deltaRotation = Quaternion.Euler (eulerAngleVelocity * turnSpeed * Time.deltaTime);
			rigidbody.MoveRotation (rigidbody.rotation * deltaRotation);
		}
		*/

		if (Input.GetKey (KeyCode.RightArrow) && (angles.y < rightRotation || angles.y > leftRotation - 1f)) {
			transform.localEulerAngles = new Vector3(0, angles.y + turnRate, 0);
		}
		if (Input.GetKey (KeyCode.LeftArrow) && (angles.y > leftRotation || angles.y < rightRotation + 1f)) {
			transform.localEulerAngles = new Vector3(0, angles.y - turnRate, 0);
		}
		if (Input.GetKey (KeyCode.UpArrow) && (gunAngle.x > elevMax || gunAngle.x < elevMin + 1f)) {
			gunBase.localEulerAngles = new Vector3(gunAngle.x - elevRate, 0, 0);
		}
		if (Input.GetKey (KeyCode.DownArrow) && (gunAngle.x < elevMin || gunAngle.x > elevMax - 1f)) {
			gunBase.localEulerAngles = new Vector3(gunAngle.x + elevRate, 0, 0);
		}
		if (Input.GetKey (KeyCode.Space) && reloadTimer >= reloadTime) {
			Fire ();
			reloadTimer = 0f;
		}
	}

	void Fire() {
		//play fire animation and effects
		Instantiate (shell, new Vector3 (gun.position.x, gun.position.y, gun.position.z), gun.rotation);//Quaternion.Euler(gunAngle.x, gunAngle.z, gunAngle.y));
	}

	//preconditions - assume the target is correct type of target, ships for gun turrets, aircraft for aa turrets, etc.
	public void SetTarget(Transform t){
		Debug.Log ("TurretController: SetTarget: target=" + t.ToString(), this.gameObject);
		target = t;
	}
}
