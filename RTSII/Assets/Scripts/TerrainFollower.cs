using UnityEngine;
using System.Collections;

public class TerrainFollower : MonoBehaviour {

	Terrain terrain;
	Unit unit;

	// Use this for initialization
	void Start () {
		terrain = GameObject.Find ("Terrain").GetComponent<Terrain>();
		unit = GetComponentInParent<Unit>();
	}
	
	// Update is called once per frame
	void Update () {
		//Quaternion qLook = Quaternion.LookRotation (Vector3.back, GetTerrainNormal ());
		//transform.localRotation = qLook;

		/*
		if (unit.IsMoving ()) {
			NavMeshHit nMHit;
			NavMesh.SamplePosition (transform.position, out nMHit, 0.1f, NavMesh.AllAreas);
			Debug.DrawLine(transform.position, nMHit.position);
			Debug.DrawRay(transform.position, nMHit.normal);
			transform.rotation = Quaternion.LookRotation(nMHit.position);
		}
		*/
	}

	Vector3 GetTerrainNormal() {
		Vector3 terrainLocalPos = transform.position - terrain.transform.position;
		Vector2 normalizedPos = new Vector2 (terrainLocalPos.x / terrain.terrainData.size.x, terrainLocalPos.z / terrain.terrainData.size.y);
		return terrain.terrainData.GetInterpolatedNormal (normalizedPos.x, normalizedPos.y);
	}
}
