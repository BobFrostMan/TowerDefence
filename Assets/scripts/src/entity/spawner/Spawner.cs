using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

	//Point to spawn enemy
	public Transform spawnPoint;
	//Enemy prefab or enemy object
	public Transform enemy;

	public int count;
	public float spawnInterval;
	public float delayBeforeSpawn;
	public int enemiesSpawned = 0;

	void Awake() {
		GameManager.instance.nextWave ();
	}

	void Update () {
		if (enemiesSpawned > count - 1) {
			this.enabled = false;
			return;
		}

		if (delayBeforeSpawn < 0) {
			spawnEnemy (enemy);	
			delayBeforeSpawn = spawnInterval;
		}

		delayBeforeSpawn -= Time.deltaTime;
	}

	private void spawnEnemy(Transform enemy) {
		enemiesSpawned++;
		Instantiate (enemy, spawnPoint.position, spawnPoint.rotation);
	}
}
