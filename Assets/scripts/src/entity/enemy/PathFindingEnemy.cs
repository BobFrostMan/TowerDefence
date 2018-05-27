using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathFindingEnemy : MonoBehaviour {

	[Header("Attributes")]
	public string type;
	public int health;
	public long speed;
	public long turnSpeed;
	public int reward;
	public double switchDistance;

	[Header("Path finder attributes")]
	private Vector3 currentPosition;
	private Vector3 lastPosition;
	private bool ready = true;
	private GameObject closestEndpoint;
	private GameObject[] endpoints;

	private Transform target;
	private float angle;

	void Start () {
		currentPosition = transform.localPosition;
		lastPosition = currentPosition;
		endpoints = GameObject.FindGameObjectsWithTag(Constants.Tags.Endpoint);
		GameObject e = findEndpoint ();
		Debug.Log ("Move to x=" +e.transform.position.x + ", y=" + e.transform.position.z);
	}

	void Update() {
			if (ready) {
				endpoints = GameObject.FindGameObjectsWithTag(Constants.Tags.Endpoint);
				if (endpoints.Length == 0){
					Debug.Log ("Something wrong!!!");
					return;
				}
				
				GameObject closestEndpoint = findEndpoint();
				int[,] wayMap = findWay(closestEndpoint);
				move (wayMap); 

				// запоминаем новую позицию после перемещения и делаем ее текущей
				currentPosition = transform.localPosition; 

				//если мы переместились, то на старой клетки пишем, что она освободилась
				if (currentPosition != lastPosition) {
					lastPosition = currentPosition;
				}
				
				if (Vector3.Distance (transform.position, closestEndpoint.transform.position) <= switchDistance) {
					Debug.Log ("Reaching target!");
					reachTarget ();
				}
		}
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
		
	private void move(int[,] gameMapCopy){
		ready = false; 
		int[] neighbors = new int[8]; //значение весов соседних клеток
		//int[] neighbors = new int[4]; //значение весов соседних клеток

		// будем хранить в векторе координаты клетки в которую нужно переместиться
		Vector3 moveTO = new Vector3(-1,1,0); 

		//FIXME: x = -1 here that means that we can't place spawnpoint at 0 point from all sides

		neighbors[0] = gameMapCopy[(int)currentPosition.x+1, (int)currentPosition.z+1];
		neighbors[1] = gameMapCopy[(int)currentPosition.x, (int)currentPosition.z+1];
		neighbors[2] = gameMapCopy[(int)currentPosition.x-1, (int)currentPosition.z+1];
		neighbors[3] = gameMapCopy[(int)currentPosition.x-1, (int)currentPosition.z];
		neighbors[4] = gameMapCopy[(int)currentPosition.x-1,(int) currentPosition.z-1];
		neighbors[5] = gameMapCopy[(int)currentPosition.x, (int)currentPosition.z-1];
		neighbors[6] = gameMapCopy[(int)currentPosition.x+1,(int) currentPosition.z-1];
		neighbors[7] = gameMapCopy[(int)currentPosition.x+1,(int) currentPosition.z];

		/*
		neighbors[0] = gameMapCopy[(int)currentPosition.x, (int)currentPosition.z+1];
		neighbors[1] = gameMapCopy[(int)currentPosition.x-1, (int)currentPosition.z];
		neighbors[2] = gameMapCopy[(int)currentPosition.x, (int)currentPosition.z-1];
		neighbors[3] = gameMapCopy[(int)currentPosition.x+1,(int) currentPosition.z];
		*/
		for(int i = 0; i < neighbors.Length; i++){
			if(neighbors[i] == -2)
				// если клетка является непроходимой, делаем так, чтобы на нее юнит точно не попал
				// делаем этот костыль для того, чтобы потом было удобно брать первый элемент в
				// отсортированом по возрастанию массиве
				neighbors[i] = 99999; 
		}

		//первый элемент массива будет вес клетки куда нужно двигаться
		Array.Sort(neighbors); 

		/**/
		//ищем координаты клетки с минимальным весом. 
		for (int x = (int)currentPosition.x-1; x <= (int)currentPosition.x+1; x++) {
			for (int y = (int)currentPosition.z+1; y >= (int)currentPosition.z-1; y--) {
				if(gameMapCopy[x,y] == neighbors[0]){
					// и указываем вектору координаты клетки, в которую переместим нашего юнита
					moveTO = new Vector3(x,1,y); 
					//break;
				} 
			}
		}
		//*/

		//ищем координаты клетки с минимальным весом. 
		//int myX = (int)currentPosition.x;
		//int myY = (int)currentPosition.z;



		//for (int x = (int)currentPosition.x-1; x <= (int)currentPosition.x+1; x++) {
		//	for (int y = (int)currentPosition.z+1; y >= (int)currentPosition.z-1; y--) {
		//		if(gameMapCopy[x,y] == neighbors[0]){
					// и указываем вектору координаты клетки, в которую переместим нашего юнита
		//if ((int)currentPosition.x-1 == )

		//			moveTO = new Vector3(x,1,y); 
					//break;
		//		} 
		//	}
		//}

		//если мы не нашли куда перемещать юнита, то оставляем его на старой позиции.
		//это случается, если вокруг юнита, во всех 8 клетках, уже размещены другие юниты
		if (moveTO == new Vector3 (-1, 1, 0)) {
			moveTO = new Vector3 (currentPosition.x, 1, currentPosition.z);
			Debug.Log ("Staying on the place: " + moveTO);
		} else {
			//перемещаем нашего юнита
			//if (GameMap.battlefield[(int)moveTO.x, (int)moveTO.z] != 1){
				Debug.Log ("Go to next point: " + moveTO);
				MovementController.turnTo (transform, moveTO, Time.deltaTime * turnSpeed);
	//			StartCoroutine (moveIt (moveTO));
				Vector3 direction = moveTO - transform.position;
				transform.Translate (direction.normalized * speed * Time.deltaTime, Space.World);
		}
		ready = true;
	}
	/*
	private IEnumerator moveIt(Vector3 moveTO){
	    yield return new WaitForSecondsRealtime(1);
		MovementController.turnTo (transform, moveTO, Time.deltaTime * turnSpeed);
		transform.position.x = moveTO.x;
		transform.position.z = moveTO.z;
	}
	*/	
	public int[,] findWay(GameObject endpoint) {
		int targetX = (int) endpoint.transform.localPosition.x;
		int targetY = (int) endpoint.transform.localPosition.z;

		// делаем копию карты локации, для дальнейшей ее разметки
		int[,] localGameMap = new int[GameMap.x, GameMap.y];
		for (int x = 0; x < GameMap.x; x++) {
			for (int y = 0; y < GameMap.y; y++) {
				if (GameMap.battlefield[x,y] == 1) {
					//can't go to this grid cell
					localGameMap[x,y] = -2;
				} else {
					//free to go
					localGameMap[x,y] = -1; 
				}
			}
		}

		//make quick copy
		//int[,] localGameMap = new int[GameMap.x, GameMap.y];
		//Array.Copy (GameMap.battlefield, localGameMap, localGameMap.Length);
			
		localGameMap [targetX,targetY] = 0;
		int step = 0;
		while (true) {
			for (int x = 0; x < GameMap.x; x++) {
				for (int y = 0; y < GameMap.y; y++) {
					if (localGameMap [x, y] == step) {
						// если соседняя клетка не стена, и если она еще не помечена
						// то помечаем ее значением шага + 1
						if (y - 1 >= 0 && localGameMap [x, y - 1] != -2 && localGameMap [x, y - 1] == -1) {
							localGameMap [x, y - 1] = step + 1;
						}

						if (x - 1 >= 0 && localGameMap [x - 1, y] != -2 && localGameMap [x - 1, y] == -1) {
							localGameMap [x - 1, y] = step + 1;
						}

						if (y + 1 >= 0 && localGameMap [x, y + 1] != -2 && localGameMap [x, y + 1] == -1) {
							localGameMap [x, y + 1] = step + 1;
						}

						if (x + 1 >= 0 && localGameMap [x + 1, y] != -2 && localGameMap [x + 1, y] == -1) {
							localGameMap [x + 1, y] = step + 1;
						}
					}
				}
			}
			step++;
			if (localGameMap [(int)transform.localPosition.x, (int)transform.localPosition.y] > 0) {
				//Debug.Log ("Enemy found the way!");
				//printMatrix(localGameMap);
				break;
			}
			if (step > GameMap.x * GameMap.y) {
				//решение не найдено, если шагов больше чем клеток
				//Debug.LogError("No solutions found! Step count is: " + step); 442
				break;
			}
		}
		return localGameMap; // возвращаем помеченную матрицу, для востановления пути в методе move()
	}
		
	private GameObject findEndpoint() {            
		float distance = Mathf.Infinity;
		Vector3 position = transform.position;
		foreach (GameObject go in endpoints) {
			float curDistance = Vector3.Distance(go.transform.position, position);
			if (curDistance < distance) {
				closestEndpoint = go;
				distance = curDistance;
			}
		}
		return closestEndpoint;
	}

	public static void printMatrix(int [,] matrix){
		string arr = "";
		for (int i = 0; i < 21; i++) {
			for (int j = 0; j < 21; j++) {
				arr += matrix[i,j] + ",";
			}
			arr += "\n";
		}
		print (arr);
	}
		
}
