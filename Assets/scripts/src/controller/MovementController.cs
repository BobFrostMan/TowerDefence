using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour {

	//turns object to the target dircetion
	public static void turnToTarget(Transform objToTurn, Transform target) {
		turnToTarget (objToTurn, target, 0f);
	}

	//Uses rotation speed to animate the rotation itself 
	//for instance Quaternion.Lerp with given speed)
	public static void turnToTarget(Transform objToTurn, Transform target, float speed) {
		turn (objToTurn, target.position, speed);
	}

	public static void turnTo(Transform objToTurn, Vector3 target, float speed) {
		turn (objToTurn, target, speed);
	}

	private static void turn(Transform objToTurn, Vector3 target, float speed) {
		Vector3 direction = target - objToTurn.position;
		//Debug.Log ("Direction: " + direction);
		Quaternion lookRotation = Quaternion.LookRotation (direction);
		//Debug.Log ("lookrotation: " + lookRotation);
		if (speed != 0f) {
			//Debug.Log ("Direction: " + Quaternion.Lerp(objToTurn.rotation, lookRotation, speed));
			objToTurn.rotation = Quaternion.Lerp(objToTurn.rotation, lookRotation, speed);
		} else {
			objToTurn.rotation = lookRotation;
		}
	}
}
