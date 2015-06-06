using UnityEngine;
using System.Collections;

public class UnitController : MonoBehaviour {
	public int hitpoints = 100;
	public int team = 1;
	public string u_name = "Frigate";

	SpriteRenderer selectionBox;
	bool selected;
	Player player;
	ShipMovement mover;
	// Use this for initialization
	void Start () {
		selectionBox = GetComponentInChildren<SpriteRenderer> ();
		selected = false;
		player = GameObject.Find ("Player " + team).GetComponent<Player>();
		mover = GetComponent<ShipMovement> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (team == 1) {
			CheckSelectionBox();
		}
	}

	void CheckSelectionBox() {
		if (GetComponentInChildren<Renderer> ().isVisible && Input.GetMouseButtonUp (0) && player.GetSelector().selecting && team == 1 && Input.mousePosition.y > player.GetSelector().mouseYLowerBound) {
			Vector3 camPos = Camera.main.WorldToScreenPoint (transform.position);
			camPos.y = UnitSelector.InvertMouseY (camPos.y);
			bool s;
			if (player.GetSelector().selection.width > 0 && player.GetSelector().selection.height > 0) {
				s = player.GetSelector().selection.Contains (camPos);
			} else {
				s = new Rect (Input.mousePosition.x, UnitSelector.InvertMouseY (Input.mousePosition.y), -player.GetSelector().selection.width, -player.GetSelector().selection.height).Contains (camPos);
			}
			SetSelected (s);
		}
	}

	public void SetSelected(bool s) {
		if (team == 1 && selected != s) {
			selected = s;
			Color color = selectionBox.color;
			if (selected) {
				Debug.Log("Selectable: SetSelected: selecting: " + u_name, this.gameObject);
				player.GetSelector().AddSelected(this.gameObject);
				color.a = 255;
			} else {
				color.a = 0;
			}
			selectionBox.color = color;
		}
	}

	//tell the unit to move to a location
	public void Move(Vector3 location) {
		mover.SetDestination(location);
	}
}
