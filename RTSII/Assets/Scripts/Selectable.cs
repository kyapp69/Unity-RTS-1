using UnityEngine;
using System.Collections;

public class Selectable : MonoBehaviour {
	public int team;
	public string uName;
	public int maxHitPoints;
	public int cost;
	public Texture2D barTexture;

	bool selected;
	SpriteRenderer selectionBox;
	int hitPoints;
	Animator anim;
	float barWidth = 800f;
	float barHeight = 28f;

	// Use this for initialization
	protected virtual void Start () {
		selectionBox = transform.Find("SelectionBox").gameObject.GetComponent<SpriteRenderer>();
		hitPoints = maxHitPoints;
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		if (team == 1) {
			CheckSelectionBox ();
		}
	}

	void CheckSelectionBox() {
		if (GetComponentInChildren<Renderer>().isVisible && Input.GetMouseButtonUp (0) && SelectorScript.selecting && team == 1 && Input.mousePosition.y > SelectorScript.mouseYLowerBound) {
			Vector3 camPos = Camera.main.WorldToScreenPoint(transform.position);
			camPos.y = SelectorScript.InvertMouseY(camPos.y);
			bool s;
			if (SelectorScript.selection.width > 0 && SelectorScript.selection.height > 0) {
				s = SelectorScript.selection.Contains (camPos);
			} else {
				s = new Rect(Input.mousePosition.x, SelectorScript.InvertMouseY(Input.mousePosition.y), -SelectorScript.selection.width, -SelectorScript.selection.height).Contains(camPos);
			}
			SetSelected(s);
		}

	}

	public void Damage(int amount) {
		Debug.Log ("Unit: Damage: amount: " + amount, this.gameObject);
		hitPoints -= amount;
	}

	public void SetSelected(bool s) {
		if (team == 1 && selected != s) {
			selected = s;
			Color color = selectionBox.color;
			if (selected) {
				Debug.Log("Selectable: SetSelected: selecting: " + uName, this.gameObject);
				SelectorScript.AddSelected(this.gameObject);
				color.a = 255;
			} else {
				color.a = 0;
			}
			selectionBox.color = color;
		}
	}
}
