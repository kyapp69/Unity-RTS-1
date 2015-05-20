using UnityEngine;
using System.Collections;

// 3rd Generation

public class RTSCameraController : MonoBehaviour {

	public struct BoxLimit {
		public float leftLimit;
		public float rightLimit;
		public float topLimit;
		public float bottomLimit;
	}

	public static BoxLimit cameraLimits = new BoxLimit();
	public static BoxLimit mouseScrollLimits = new BoxLimit();
	public float scrollMult = 3;
	public int scrollDistance = 10; // <how close to the edge the cursor must be for the camera to scroll

	// Use this for initialization
	void Start () {
		// Declare Camera Limits
		cameraLimits.leftLimit = 10f;
		cameraLimits.rightLimit = 2000f;
		cameraLimits.topLimit = 2000f;
		cameraLimits.bottomLimit = -20;

		//Declare Mouse Scroll Limits
		mouseScrollLimits.leftLimit = scrollDistance;
		mouseScrollLimits.rightLimit = scrollDistance;
		mouseScrollLimits.topLimit = scrollDistance;
		mouseScrollLimits.bottomLimit = scrollDistance;
	}
	
	// Update is called once per frame
	void Update () {
		if (CheckIfUserCameraInput ()) {
			Vector3 cameraDesiredMove = GetDesiredTranslation();
			if (!isDesiredPositionOverBoundaries(cameraDesiredMove)) {
				transform.Translate (cameraDesiredMove);
			}
		}
	}

	public bool CheckIfUserCameraInput() {
		bool keyBoardMove;
		bool mouseMove;
		bool canMove;

		if (RTSCameraController.AreCameraKeyboardButtonsPressed ()) {
			keyBoardMove = true;
		} else {
			keyBoardMove = false;
		}

		if (RTSCameraController.IsMousePositionWithinBoundaries()) 
			mouseMove = true; else mouseMove = false;

		if (keyBoardMove || mouseMove) 
			canMove = true; else canMove = false;
		return canMove;
	}

	public Vector3 GetDesiredTranslation() {
		float moveSpeed = 0f;
		float desiredX = 0f;
		float desiredZ = 0f;

		if (Input.GetKey (KeyCode.W) || Input.mousePosition.y > (Screen.height - mouseScrollLimits.topLimit)) {
			desiredZ = moveSpeed;
		}

		if (Input.GetKey (KeyCode.S) || Input.mousePosition.y < mouseScrollLimits.bottomLimit) {
			desiredZ = moveSpeed * -1;
		}

		if (Input.GetKey(KeyCode.A) || Input.mousePosition.x < mouseScrollLimits.leftLimit) {
			desiredX = moveSpeed * -1;
		}

		if (Input.GetKey (KeyCode.D) || Input.mousePosition.x > (Screen.width - mouseScrollLimits.rightLimit)){
			desiredX = moveSpeed;
		}

		return new Vector3(desiredX, 0, desiredZ);
	}

	public bool isDesiredPositionOverBoundaries(Vector3 desiredPosition) {
		bool overBoundaries = false;
		if ((transform.position.x + desiredPosition.x) < cameraLimits.leftLimit)
			overBoundaries = true;
		if ((transform.position.x + desiredPosition.x) > cameraLimits.rightLimit)
			overBoundaries = true;
		if ((transform.position.z + desiredPosition.z) > cameraLimits.topLimit)
			overBoundaries = true;
		if ((transform.position.z + desiredPosition.z) < cameraLimits.bottomLimit)
			overBoundaries = true;

		return overBoundaries;
	}

	public static bool AreCameraKeyboardButtonsPressed() {
		if (Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.D))
			return true; else return false;
	}

	public static bool IsMousePositionWithinBoundaries() {
		if ((Input.mousePosition.x < mouseScrollLimits.leftLimit && Input.mousePosition.x > -5) ||
			(Input.mousePosition.x > (Screen.width - mouseScrollLimits.rightLimit) && Input.mousePosition.x < (Screen.width + 5)) ||
			(Input.mousePosition.y < mouseScrollLimits.bottomLimit && Input.mousePosition.y > -5) ||
			(Input.mousePosition.y > (Screen.height - mouseScrollLimits.topLimit) && Input.mousePosition.y < (Screen.height + 5))) 
			return true; else return false;
	}
}
