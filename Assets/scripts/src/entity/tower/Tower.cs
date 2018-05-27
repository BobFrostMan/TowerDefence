using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {

	[Header("Connected objects")]
	public GameObject bulletPrefab;
	public Transform firePoint;

	[Header("Attributes")]
	public int price;
	public float range;
	public int damage;

	[Header("Multi update attributes")]
	public Transform target;
	public float turnSpeed;
	public float fireRate;

	private float fireCountDown = 0;
	private string enemyTag = Constants.Tags.Enemy;

	void Start () {
		// frome the initialization each 0,5 seconds invoke method with given name 
		InvokeRepeating ("detectTarget", 0, 0.5f);
	}

	void detectTarget() {
		GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
		if (enemies.Length == 0) {
			return;
		}

		GameObject closestEnemy = null;
		float closestDistance = Mathf.Infinity;

		foreach (GameObject enemy in enemies) {
			if (enemy != null) {
				float disToEnemy = Vector3.Distance (enemy.transform.position, gameObject.transform.position);
				if (disToEnemy < closestDistance) {
					closestDistance = disToEnemy;
					closestEnemy = enemy;
				}
			}
		}

		if (closestEnemy != null && closestDistance <= range) {
			target = closestEnemy.transform;
		} else {
			target = null;
		}
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (transform.position, range);
	}

	// Update is called once per frame
	void Update () {
		if (target == null) {
			return;
		}

		// defining quaternion agles
		Vector3 direction = target.position - transform.position;
		Quaternion lookRotation = Quaternion.LookRotation (direction);
		transform.rotation = Quaternion.Lerp (transform.rotation, lookRotation, Time.deltaTime * turnSpeed);

		//actual rotation
		Vector3 rotation = lookRotation.eulerAngles;
		transform.rotation = Quaternion.Euler (0, rotation.y, 0);

		if (fireCountDown <= 0) {
			shoot ();
			fireCountDown = 1f / fireRate;
		}

		fireCountDown -= Time.deltaTime;
	}

	void shoot() {
		GameObject bulletObj = (GameObject) Instantiate (bulletPrefab, firePoint.position, firePoint.rotation);
		Bullet bullet = bulletObj.GetComponent<Bullet> ();
		bullet.setDamage (damage);
		if (bullet != null) {
			bullet.chaiseTarget (target);
		}
	}

}
