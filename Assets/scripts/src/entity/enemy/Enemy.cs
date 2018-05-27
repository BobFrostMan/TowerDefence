using UnityEngine;

public class Enemy : MonoBehaviour {

	[Header("Attributes")]
	public string type;
	public int health;
	public long speed;
	public long turnSpeed;
	public int reward;
	public double switchDistance;

	private Transform target;
	private int pointIndex = 0;

	private float angle;

	void Start () {
		target = WayPath.points [0];
		MovementController.turnToTarget(transform, target);
	}

	void Update() {
		Vector3 direction = target.position - transform.position;
		transform.Translate (direction.normalized * speed * Time.deltaTime, Space.World);

		if (Vector3.Distance (transform.position, target.position) <= switchDistance) {
			Transform nextPoint = GetNextCheckpoint ();

			if (nextPoint != null) {
				target = nextPoint;
				MovementController.turnToTarget (transform, target, Time.deltaTime * turnSpeed);
			} else {
				reachTarget ();			
			}
		}
	}

	private Transform GetNextCheckpoint() {
		if (pointIndex > WayPath.points.Length - 1) {
			return null;
		}
		return WayPath.points [pointIndex++];		
	}

	private void reachTarget() {
		Debug.Log ("Enemy reached the target, player lost his life");
		GameManager.instance.decreaseLife ();
		Destroy (gameObject);
	}

	public void die() {
		GameManager.instance.enemyDestroyed (this);
		Destroy (gameObject);
	}

	public void sufferFrom(Bullet bullet) {
		health -= bullet.getDamage ();
		if (health <= 0) {
			die ();
		}
	}

}
