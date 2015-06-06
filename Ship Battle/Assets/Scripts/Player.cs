using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public string p_name = "Capn Crunch";
	public int team = 1;

	public UnitSelector GetSelector() {
		return GetComponent<UnitSelector> ();
	}

	public CameraController GetCameraController() {
		return GetComponent<CameraController> ();
	}
}
