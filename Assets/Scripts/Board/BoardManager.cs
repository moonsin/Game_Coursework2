using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {

	public static BoardManager instance = null;

	public int columns ; 										//Number of columns in our game board.
	public int rows ;											//Number of rows in our game board.

	protected Transform boardHolder;
	protected Transform playersHolder;
	protected Transform enemiesHolder;

	public GameObject[,] TilesInstance;
	protected int[,] playersPos;
	protected int[,] enemiesPos;

	public GameObject[] Players;
	public GameObject[] Enemies;

	public int[,] floorMoveableArray;
	public int[] fightOrderArray;

	public Vector3 TransFromWorldToISO(Vector3 vec){
		Vector3 newVec = new Vector3();

		newVec.x = vec.x + vec.y;
		newVec.y = (vec.y - vec.x) / 2;

		return newVec;
	}

	protected void setFloor(GameObject toInstantiate, int LayerOrder, int x, int y, int adjustPos){


		//adjustPos == 2 表示变高一半
		//adjustPos == 3 表示变低一半
		//adjustPos == 4 表示变高1/4
		//adjustPos == 5 表示变低1/4
		//adjustPos == 6 表示变高1/8
		//adjustPos == 7 表示变低1/8
		//adjustPos == 8 表示变高一倍

		toInstantiate.GetComponent<SpriteRenderer> ().sortingOrder = LayerOrder;
		Vector3 ISOVec = new Vector3 ();



		ISOVec = TransFromWorldToISO (new Vector3 (x, y, 0f));

		if (adjustPos == 2) {
			ISOVec.y += 1f;
		} else if (adjustPos == 3) {
			ISOVec.y -= 0.7f;
		} else if (adjustPos == 4) {
			ISOVec.y += 0.75f;
		} else if (adjustPos == 5) {
			ISOVec.y -= 0.55f;
		} else if (adjustPos == 6) {
			ISOVec.y += 0.4f;
		} else if (adjustPos == 7) {
			ISOVec.y -= 0.3f;
		} else if (adjustPos == 8){
			ISOVec.y += 1.5f;
		}else {
			ISOVec.y += 0.2f * Random.value;
		}

		//随机高或低，增加立体感 Random.value (0-1)


		GameObject instance =
			Instantiate (toInstantiate, ISOVec, Quaternion.identity) as GameObject;

		instance.transform.SetParent (boardHolder);

		TilesInstance [x, y] = instance;

	}

	//type = 0 player, type = 1 enemy
	protected void setCharacters(int[,] Pos, int[] index, int type){
		GameObject[] character;
		Transform Holder;

		if (type == 0) {
			character = Players ;
			Holder = playersHolder;
		} else {
			character = Enemies ;
			Holder = enemiesHolder;

		}

		for (int i = 0; i < index.Length; i++) {
			rendeCharacter (Pos [index[i], 0], Pos [index[i], 1], character[index[i]], Holder, type);
			floorMoveableArray [Pos [index[i], 1], Pos [index[i], 0]] = 0;
			if (type == 0) {
				character [index [i]].GetComponent<Player> ().ObjGridVec = new Vector3(playersPos[i,0],playersPos [i, 1],0);
			} else {
				character [index [i]].GetComponent<Enemy> ().ObjGridVec = new Vector3(playersPos[i,0],playersPos [i, 1],0);
			}
		}

	}

	protected void rendeCharacter(int x, int y, GameObject character, Transform Holder, int type){
		Vector3 newVec = new Vector3();
		newVec.x = x + y;
		if (type == 0) {
			newVec.y = TilesInstance [x, y].transform.position.y + 1.3f;
			character.GetComponent<SpriteRenderer> ().sortingOrder = TilesInstance [x,y].GetComponent<SpriteRenderer> ().sortingOrder + 1;
		} else {
			newVec.y = TilesInstance [x, y].transform.position.y +0.7f;
			character.GetComponent<UnityEngine.Rendering.SortingGroup>().sortingOrder = TilesInstance [x,y].GetComponent<SpriteRenderer> ().sortingOrder + 1;
		}


		GameObject instance =
			Instantiate (character, newVec, Quaternion.identity) as GameObject;
		instance.transform.SetParent (Holder);
	}

	protected void SetPlayers (int[,] playersPos){
		for (int i = 0; i < Players.Length; i++) {
			rendePlayer (playersPos [i, 0], playersPos [i, 1], Players[i], playersHolder);
			floorMoveableArray [playersPos [i, 1], playersPos [i, 0]] = 0;
			Players[i].GetComponent<Player>().ObjGridVec = new Vector3(playersPos[i,0],playersPos [i, 1],0);
		}
	}

	protected void SetEnemies (int[,] enemiesPos,int beginIndex, int EndIndex){
		for (int i = 0; i>= beginIndex && i <= EndIndex; i++) {
			rendeEnemy (enemiesPos [i, 0], enemiesPos [i, 1], Enemies[i], enemiesHolder);
			floorMoveableArray [enemiesPos [i, 1], enemiesPos [i, 0]] = 0;
			Enemies[i].GetComponent<Enemy>().ObjGridVec = new Vector3(enemiesPos[i,0],enemiesPos [i, 1],0);
		}
	}

	protected void rendeEnemy(int x, int y, GameObject enemy, Transform enemiesHolder){
		Vector3 newVec = new Vector3();
		newVec.x = x + y;
		newVec.y = TilesInstance [x, y].transform.position.y +0.7f;

		//player.GetComponent<SpriteRenderer> ().sortingOrder = TilesInstance [x,y].GetComponent<SpriteRenderer> ().sortingOrder + 1;
		enemy.GetComponent<UnityEngine.Rendering.SortingGroup>().sortingOrder = TilesInstance [x,y].GetComponent<SpriteRenderer> ().sortingOrder + 1;

		GameObject instance =
			Instantiate (enemy, newVec, Quaternion.identity) as GameObject;

		instance.transform.SetParent (enemiesHolder);
	}

	protected void rendePlayer(int x, int y, GameObject player, Transform playersHolder){
		Vector3 newVec = new Vector3();
		newVec.x = x + y;
		newVec.y = TilesInstance [x, y].transform.position.y +1.3f;

		player.GetComponent<SpriteRenderer> ().sortingOrder = TilesInstance [x,y].GetComponent<SpriteRenderer> ().sortingOrder + 1;

		GameObject instance =
			Instantiate (player, newVec, Quaternion.identity) as GameObject;

		instance.transform.SetParent (playersHolder);
	}

	// Use this for initialization
	protected void Awake () {
		if (instance == null) {
			instance = this;
		} 
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
