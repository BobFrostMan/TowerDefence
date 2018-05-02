using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerButtonHandler : MonoBehaviour {

	public GameObject towerToBuild;

	public void OnClick() {
		if (GameManager.instance.isAllowedToBuild (towerToBuild)) {
			TowerBuildManager.instance.setTower(towerToBuild);
			TowerBuildManager.instance.enterBuildMode ();
			Debug.Log ("Tower selected!");
		} else {
			Debug.Log ("No money, bitch!!");
			GameManager.instance.noMoneyToBuild ();
		}
	}
}
