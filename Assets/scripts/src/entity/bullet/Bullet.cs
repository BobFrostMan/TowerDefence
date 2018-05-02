using UnityEngine;

public class Bullet : MonoBehaviour {

	[Header("Attributes")]
	public float speed;

	[Header("Multi update parameters")]
	public Transform target;

	[Header("Effects")]
	public GameObject impactEffect;

	private int damage;

	void Start () {
		
	}
	
	void Update () {
		if (target == null) {
			Destroy (gameObject);
			return;
		}

		Vector3 direction = target.position - transform.position;
		float distanceThisFrame = speed * Time.deltaTime;

		if (direction.magnitude <= distanceThisFrame) {
			hit ();
			return;
		}
			
		transform.Translate (direction.normalized * distanceThisFrame, Space.World);
	}

	public void chaiseTarget(Transform target) {
		this.target = target;
	}

	private void hit(){
		GameObject effect = (GameObject)Instantiate (impactEffect, transform.position, transform.rotation);	
		Destroy (effect, 1);
		Destroy (gameObject);
		target.gameObject.GetComponent<Enemy>().sufferFrom(this);
	}

	public void setDamage(int damage){
		this.damage = damage;
	}

	public int getDamage(){
		return damage;
	}

}
