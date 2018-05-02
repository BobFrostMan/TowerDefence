using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;

	[Header("Text to show")]
	public Text moneyText;
	public Text waveNumberText;
	public Text enemiesKilledText;
	public Text livesLeftText;
	public Text message;

	[Header("Player attributes")]
	public int money = 100;
	public int waveNumber = 0;
	public int enemiesKilled = 0;
	public int livesLeft = 10;

	public bool gameOver = false;

	void Awake () {
		if (instance == null) {
			instance = this;
		}
		initGame ();
	}
	
	// Update is called once per frame
	void Update () {
		updateTextStats ();
	}
		
	void updateTextStats(){
		moneyText.text = Constants.Gameplay.MONEY_TEXT + money;	
		waveNumberText.text = Constants.Gameplay.WAVE_NUMBER_TEXT + waveNumber;
		enemiesKilledText.text = Constants.Gameplay.ENEMIES_KILLED_TEXT + enemiesKilled;
		livesLeftText.text = Constants.Gameplay.LIVES_LEFT_TEXT + livesLeft;
	}

	void initGame() {
		updateTextStats ();	
	}

	public void decreaseLife() {
		livesLeft--;
	}

	public void nextWave() {
		waveNumber++;
		Debug.Log ("New wave " + waveNumber);
	}

	public void enemyDestroyed(Enemy enemy) {
		enemiesKilled++;
		money += enemy.reward;
	}

	public void cantBuildThere(){
		StartCoroutine (cantBuildEvent());
	}

	public bool isAllowedToBuild(GameObject tower){
		Tower towerToBuild = tower.GetComponent<Tower> ();
		bool allowed = money >= towerToBuild.price;
		if (!allowed) {
			noMoneyToBuild();
		}
		return allowed;
	}

	public void towerBuilt(GameObject tower){
		int price = tower.GetComponent<Tower> ().price;
		money -= price;
	}

	public void noMoneyToBuild() {
		StartCoroutine (noMoneyEvent());
	}

	private IEnumerator noMoneyEvent(){
		message.text = Constants.Gameplay.NO_MONEY;
		yield return new WaitForSeconds(2);
		message.text = "";
	}

	private IEnumerator cantBuildEvent(){
		message.text = Constants.Gameplay.CANT_BUILD_THERE;
		yield return new WaitForSeconds(2);
		message.text = "";
	}
}
