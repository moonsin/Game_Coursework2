using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerFightBoardManager : BoardManager {

	public GameObject[] firstFloorTiles;
	public GameObject[] secondFloorTiles;

	private int floorNumber = 1;

	int [,] firstFloormap = new int [8,8]{
		{3,2,2,1,1,2,1,1},
		{3,2,0,0,0,0,0,1},
		{3,2,0,1,1,0,0,1},
		{3,1,0,1,1,1,0,2},
		{3,2,0,0,1,1,0,2},
		{3,2,1,0,0,0,0,2},
		{3,4,2,2,1,1,2,2},
		{3,3,3,3,3,3,3,3}
	};

	int[,] firstFloorHeightMap = new int[8,8]{
		{6,0,0,0,0,0,0,0},
		{2,0,3,3,3,3,3,0},
		{6,0,3,0,0,5,3,0},
		{2,0,3,6,6,0,3,0},
		{6,0,3,3,0,0,3,0},
		{4,0,0,3,3,3,3,0},
		{2,8,0,0,0,0,0,0},
		{6,2,4,6,2,2,4,6}
	};

	int[] firstFloorPlayers = new int[]{0,1,2};
	int[] firstFloorEnemies = new int[]{0,1};

	int[] secondFloorEnemies = new int[]{0,0};

	GameObject[] firstFloorPlayersObj = new GameObject[3]; 
	GameObject[] firstFloorEnemiesObj = new GameObject[2];

	int [,] secondFloormap = new int [8,8]{
		{0,0,0,0,0,0,0,0},
		{0,0,0,0,0,0,0,0},
		{0,0,0,0,0,0,0,0},
		{0,0,0,0,0,0,0,0},
		{0,0,0,0,0,0,0,0},
		{0,0,0,0,0,0,0,0},
		{0,2,0,0,0,0,0,0},
		{0,0,0,0,0,0,0,0}
	};

	int[,] SecondFloorHeightMap = new int[8,8]{
		{6,0,0,0,0,0,0,0},
		{2,0,3,3,3,3,3,0},
		{6,0,3,0,0,5,3,0},
		{2,0,3,6,6,0,3,0},
		{6,0,3,3,0,0,3,0},
		{4,0,0,3,3,3,3,0},
		{2,0,0,0,0,0,0,0},
		{6,2,4,6,2,2,4,6}
	};

	int[,] floor2PlayerPos = new int[3, 2]{{1,5},{2,5},{2,6}};
	int[,] floor2EnemyPos = new int[2,2]{{7,4},{7,5}};

	// Use this for initialization
	void Awake(){

		columns = 8;
		rows = 8;

		boardHolder = new GameObject ("Board").transform;
		playersHolder = new GameObject ("Players").transform;
		enemiesHolder = new GameObject ("Enemies").transform;

		TilesInstance = new GameObject[8, 8];

		playersPos = new int[3, 2]{ {7, 4},{7, 5},{7,3}};
		enemiesPos = new int[2, 2]{ {1, 4},{1,5}};

		floorMoveableArray = new int[8, 8];
		floorAttackAbleArray = new int[8, 8];

		initFloorMoveableArray ();
		initFloorAttackAbleArray ();

		fightPlayersIndex = firstFloorPlayers;
		fightEnemiesIndex = firstFloorEnemies;

		base.Awake ();
	}

	private void initFloorMoveableArray(){
		for (int y = 0; y < rows; y++) {
			for (int x = 0; x < columns; x++) {
				if (firstFloorHeightMap [y, x] == 0) {
					floorMoveableArray [y, x] = 1;
				} else {
					floorMoveableArray [y, x] = 0;
				}
			}
		}
	}

	private void initFloorAttackAbleArray(){
		for (int y = 0; y < rows; y++) {
			for (int x = 0; x < columns; x++) {
				if (firstFloorHeightMap [y, x] == 0) {
					floorAttackAbleArray [y, x] = 1;
				} else {
					floorAttackAbleArray [y, x] = 0;
				}
			}
		}
	}


		

	public void SetupBoard(){

		firstFloorSetup ();
		for (int i = 0; i < firstFloorPlayers.Length; i++) {
			firstFloorPlayersObj [i] = Players [firstFloorPlayers [i]];
		}
		for (int i = 0; i < firstFloorEnemies.Length; i++) {
			firstFloorEnemiesObj [i] = Enemies [firstFloorEnemies [i]];
		}
		setCharacters (playersPos,firstFloorPlayersObj,0);
		setCharacters (enemiesPos,firstFloorEnemiesObj,1);
		//secondFloorSetup ();


	}

	private void firstFloorSetup (){

		int LayerOrder = 0;

		for (int y = rows - 1; y >= 0; y--) {
			for (int x = 0; x < columns; x++) {
				GameObject toInstantiate = firstFloorTiles [firstFloormap[y,x]];

				setFloor (toInstantiate, LayerOrder, x ,y,firstFloorHeightMap[y,x],0);

				LayerOrder += 1;
			}
		}
			
	}

	private void secondFloorSetup(){
		int LayerOrder = 0;

		for (int y = rows - 1; y >= 0; y--) {
			for (int x = 0; x < columns; x++) {
				GameObject toInstantiate = secondFloorTiles [secondFloormap[y,x]];

				setFloor (toInstantiate, LayerOrder, x ,y,SecondFloorHeightMap[y,x],1);

				LayerOrder += 1;
			}
		}
	}



	// Update is called once per frame
	void Update () {
		
		if (setCharactersFinished) {
			setCharactersFinished = false;
			initOrderArray ();
			initOrderNames ();


			if (fightOrderArray [0].tag == "Player") {
				fightOrderArray [0].GetComponent<Player> ().OwnTurn = true;
				fightOrderArray [0].GetComponent<Player> ().alreadyMoved = false;
				fightOrderArray [0].GetComponent<Player> ().alreadyAttacked = false;
				allButtonEnabled ();
			} else {
				allButtonDisabled ();
				fightOrderArray [0].GetComponent<Enemy> ().run ();
			}
		}

		checkAllenemieDied ();	
		if (allEnemiesdied) {
			floorNumber += 1;
			setCharactersFinished = false;
			//Destroy (playersHolder.gameObject);
			initBoard ();

			//playersHolder = new GameObject ("Players").transform;

			if (floorNumber == 2) {
				secondFloorSetup ();
				playersPos = floor2PlayerPos;
				enemiesPos = floor2EnemyPos;

				/*
				for (int i = 0; i < secondFloorEnemies.Length; i++) {
					firstFloorEnemiesObj [i] = Enemies [secondFloorEnemies [i]];
				}

				GameObject[] currentPlayersObj = GameObject.FindGameObjectsWithTag ("Player");
				int PlayersLength;
				PlayersLength = currentPlayersObj.Length;
				GameObject[] newPlayersObj = new GameObject[PlayersLength];

				for (int i = 0; i < currentPlayersObj.Length; i++) {
					newPlayersObj [i] = Instantiate (currentPlayersObj [i]);
				}

				Destroy (playersHolder.gameObject);
				playersHolder = new GameObject ("Players").transform;

				setCharacters (floor2PlayerPos,newPlayersObj,0);
				setCharacters (enemiesPos,firstFloorEnemiesObj,1);
				*/

				GameManager.instance.GetComponent<CameraController> ().goUpStairs ();
			}
		}

		base.Update ();
	}
}
