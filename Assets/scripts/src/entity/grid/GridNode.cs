using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNode : MonoBehaviour {

	[Header("Attributes")]
	public Color hoverColor;

	private Renderer rend;
	private Color startColor;
	private GameObject tower;

	void Start () {
		rend = GetComponent<Renderer>();
		startColor = rend.material.color;
	}

	void OnMouseDown() {
		if (TowerBuildManager.instance.allowedToBuild) {
			if (tower != null) {
				GameManager.instance.cantBuildThere ();
				return;
			}

			GameObject towerToBuild = TowerBuildManager.instance.getTowerToBuild ();

			if (towerToBuild == null) {
				//if tower is not selected - nothing to build
				return;
			}

			if (GameManager.instance.isAllowedToBuild (towerToBuild)) {
				build (towerToBuild);
			}
		}
	}

	void OnMouseEnter() {
		if (TowerBuildManager.instance.isBuildMode ()) {
			rend.material.color = hoverColor;
		}
	}

	void OnMouseExit() {
		rend.material.color = startColor;
	}

	void build (GameObject towerToBuild) {
		tower = (GameObject) Instantiate (towerToBuild, transform.position, transform.rotation);
		GameManager.instance.towerBuilt (towerToBuild);
		GameMap.battlefield[(int)transform.position.x, (int)transform.position.z] = 1;
		Debug.Log("Cell is blocked: x=" + (int)transform.position.x + ", y=" + (int)transform.position.z);
		//GameMap.battlefield[(int)transform.localPosition.x, (int)transform.localPosition.z] = -2;
	}

}
