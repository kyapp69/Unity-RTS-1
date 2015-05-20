using UnityEngine;
using System.Collections;

/*
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
	
	// Update is called once per frame
	void Update () {
		MoveCamera ();
		ZoomCamera ();
	}

	// Moves the camera with either the mouse or wasd
	void MoveCamera() {
		float scrollSpeed = scrollMult * Camera.main.fieldOfView;
		// get the mouse and camera positions
		Vector3 mousePos = Input.mousePosition;
		float x = transform.position.x;
		float z = transform.position.z;

		// adjust the scrollspeed for the update time
		float scrollAmount = scrollSpeed * Time.deltaTime;

		// mouse movement
		// mouse left
		if (mousePos.x < scrollDistance && x > -0) {
			transform.Translate(-scrollAmount, 0, 0, Space.World);
		}
		// mouse right
		if (mousePos.x >= Screen.width - scrollDistance && x < 480) {
			transform.Translate(scrollAmount, 0,0, Space.World);
		}
		// mouse down
		if (mousePos.y < scrollDistance && z > -0) {
			transform.Translate(0,0, -scrollAmount, Space.World);
		}
		// mouse up
		if (mousePos.y >= Screen.height - scrollDistance && z < 480) {
			transform.Translate(0,0,scrollAmount, Space.World);
		}
		
		// key movement
		// a-left
		if (Input.GetKey("a") && x > -0) {
			transform.Translate(-scrollAmount, 0, 0, Space.World);
		}
		// d-right
		if (Input.GetKey ("d") && x < 480) {
			transform.Translate(scrollAmount, 0,0, Space.World);
		}
		// s-down
		if (Input.GetKey("s") && z > -0) {
			transform.Translate(0,0, -scrollAmount, Space.World);
		}
		// w-up
		if (Input.GetKey("w") && z < 480) {
			transform.Translate(0,0,scrollAmount, Space.World);
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
}
