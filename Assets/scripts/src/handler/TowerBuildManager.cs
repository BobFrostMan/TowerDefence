using UnityEngine;

public class TowerBuildManager : MonoBehaviour {

	public static TowerBuildManager instance;

	private GameObject towerToBuild;

	private bool buildModeOn = false;

	void Awake () {
		if (instance == null) {
			instance = this;
		}
	}

	public GameObject getTowerToBuild(){
		return towerToBuild;
	}

	public void setTower(GameObject obj){
		towerToBuild = obj;
	}
		
	public void enterBuildMode(){
		buildModeOn = true;
	}

	public void quitBuildMode(){
		buildModeOn = false;
	}

	public bool isBuildMode(){
		return buildModeOn;
	}
}
