using UnityEngine;
using System.Collections;

/*
 * 2nd Generation
 * This script controls that main camera to act like an RTS camera with edge scolling, 
 * zoom, and rotation. Scroll speed is not constant and is based on the zoom, but the 
 * multiplier is adjustable.
 */

public class CameraController : MonoBehaviour {
	public int scrollDistance = 10; // <how close to the edge the cursor must be for the camera to scroll
	public float zoomMin = 10f;
	public float zoomMax = 50f;
	public float scrollMult = 3;
	public float zoomMult = 0.1f;
	public float verticalRotationMin = 25f; // <in degrees
	public float verticalRotationMax = 65f; // < in degrees

	bool verticalRotationEnabled = true;
	float mouseX;
	float mouseY;

	void Start() {
		GetMousePosition ();
	}

	// Update is called once per frame
	void Update () {
		MoveCamera ();
		ZoomCamera ();
		RotateCamera ();
	}

	void LateUpdate() {
		GetMousePosition ();
	}

	void GetMousePosition() {
		// Get the mouse position - needed by MoveCamera() and RotateCamer()
		mouseX = Input.mousePosition.x;
		mouseY = Input.mousePosition.y;
	}

	// Moves the camera with either the mouse or wasd
	void MoveCamera() {
		float scrollSpeed = scrollMult * Camera.main.fieldOfView;
		// get the mouse and camera positions
		float x = transform.position.x;
		float z = transform.position.z;

		// adjust the scrollspeed for the update time
		float scrollAmount = scrollSpeed * Time.deltaTime;

		// mouse movement
		if (!Input.GetKey(KeyCode.LeftControl)) {
			// mouse left
			if ((mouseX < scrollDistance || Input.GetKey(KeyCode.A)) && x > -0) {
				transform.Translate(-scrollAmount, 0, 0);
			}
			// mouse right
			if ((mouseX >= Screen.width - scrollDistance || Input.GetKey (KeyCode.D)) && x < 480) {
				transform.Translate(scrollAmount, 0,0);
			}
			// mouse down
			if ((mouseY < scrollDistance || Input.GetKey(KeyCode.S)) && z > -0) {
				transform.Translate(0,0, -scrollAmount);
			}
			// mouse up
			if ((mouseY >= Screen.height - scrollDistance || Input.GetKey(KeyCode.W)) && z < 480) {
				transform.Translate(0,0,scrollAmount);
			}
		}
	}

	// Zooms the camera with the mouse scrollwheel
	void ZoomCamera() {
		// zoom out
		if (Input.GetAxis ("Mouse ScrollWheel") < -0) {
			Camera.main.fieldOfView *= 1f + zoomMult;
		} 
		// zoom in
		if (Input.GetAxis ("Mouse ScrollWheel") > -0) {
			Camera.main.fieldOfView *= 1f - zoomMult;
		}
		// clamp the zoom between the min and max values
		Camera.main.fieldOfView = Mathf.Clamp (Camera.main.fieldOfView, zoomMin, zoomMax);

	}

	// hold down left ctrl and right mouse button and drag mouse to rotate
	void RotateCamera() {
		float easeFactor = 5f;
		if (Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButton(1)) {
			// Horizontal Rotation - Rotate camera if mouse x position has changed
			if (Input.mousePosition.x != mouseX) {
				float cameraRotationY = (Input.mousePosition.x - mouseX) * easeFactor * Time.deltaTime;
				transform.Rotate(0, cameraRotationY, 0);
			}

			// Vertical Rotation - Rotate camer if mouse y position has changed
			if (verticalRotationEnabled && Input.mousePosition.y != mouseY) {
				Transform camera = Camera.main.GetComponent<Transform>();
				float cameraRotationX = (mouseY - Input.mousePosition.y) * easeFactor * Time.deltaTime;
				float desiredRotationX = camera.eulerAngles.x + cameraRotationX;

				if (desiredRotationX >= verticalRotationMin && desiredRotationX <= verticalRotationMax) {
					camera.Rotate(cameraRotationX, 0, 0);
				}
			}
		}
	}
}
