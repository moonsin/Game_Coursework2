using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnReadyFight : BoardManager {
	public GameObject[] firstFloorTiles;
	public bool unreadyFightBegin = false;
	public GameObject unreadyFightDialog;

	private int floorNumber = 1;

	int [,] firstFloormap = new int [8,8]{
		{0,0,0,0,0,0,0,0},
		{0,0,0,0,0,0,0,0},
		{0,0,0,0,0,0,0,0},
		{0,0,0,0,0,0,0,0},
		{0,0,0,0,0,0,0,0},
		{0,0,0,0,0,0,0,0},
		{0,0,0,0,0,0,0,0},
		{0,0,0,0,0,0,0,0}
	};

	int[,] firstFloorHeightMap = new int[8,8]{
		{0,0,0,0,0,0,0,0},
		{0,0,0,0,0,0,0,0},
		{0,0,0,0,0,0,0,0},
		{0,0,0,0,0,0,0,0},
		{0,0,0,0,0,0,0,0},
		{0,0,0,0,0,0,0,0},
		{0,0,0,0,0,0,0,0},
		{0,0,0,0,0,0,0,0}
	};

	int[] firstFloorPlayers = new int[]{0};
	int[] firstFloorEnemies = new int[]{0};

	GameObject[] firstFloorPlayersObj = new GameObject[1]; 
	GameObject[] firstFloorEnemiesObj = new GameObject[1];

	private void initFloorMoveableArray(int[,] HeightMap){
		for (int y = 0; y < rows; y++) {
			for (int x = 0; x < columns; x++) {
				if (HeightMap [y, x] == 0) {
					floorMoveableArray [y, x] = 1;
				} else {
					floorMoveableArray [y, x] = 0;
				}
			}
		}
	}

	private void initFloorAttackAbleArray(int[,] HeightMap){
		for (int y = 0; y < rows; y++) {
			for (int x = 0; x < columns; x++) {
				if (HeightMap [y, x] == 0) {
					floorAttackAbleArray [y, x] = 1;
				} else {
					floorAttackAbleArray [y, x] = 0;
				}
			}
		}
	}

	void Awake(){

	}

	public void SetupBoard(){

		columns = 8;
		rows = 8;

		boardHolder = new GameObject ("Board").transform;
		playersHolder = new GameObject ("Players").transform;
		enemiesHolder = new GameObject ("Enemies").transform;

		TilesInstance = new GameObject[8, 8];

		playersPos = new int[1, 2]{ {7, 5}};
		enemiesPos = new int[1, 2]{ {1, 5}};

		floorMoveableArray = new int[8, 8];
		floorAttackAbleArray = new int[8, 8];

		initFloorMoveableArray (firstFloorHeightMap);
		initFloorAttackAbleArray (firstFloorHeightMap);

		fightPlayersIndex = firstFloorPlayers;
		fightEnemiesIndex = firstFloorEnemies;

		base.Awake ();


		firstFloorSetup ();
		for (int i = 0; i < firstFloorPlayers.Length; i++) {
			firstFloorPlayersObj [i] = Players [firstFloorPlayers [i]];
		}
		for (int i = 0; i < firstFloorEnemies.Length; i++) {
			firstFloorEnemiesObj [i] = Enemies [firstFloorEnemies [i]];
		}
		setCharacters (playersPos,firstFloorPlayersObj,0);
		setCharacters (enemiesPos,firstFloorEnemiesObj,1);


		showDialog ();


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

	private void showDialog (){

		Instantiate (unreadyFightDialog);
		//towerFightBegin = true;
	}

	protected void lose(){
		Instantiate (BE1);
	}
	protected void win(){
		Instantiate (WE1);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!unreadyFightBegin) {
			return;
		}

		if (setCharactersFinished) {
			setCharactersFinished = false;
			initOrderArray ();
			initOrderNames ();


			if (fightOrderArray [0].tag == "Player") {
				showTurnIndicator ("Player");
				fightOrderArray [0].GetComponent<Player> ().updatePlayerIndicator ();
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
			unreadyFightBegin = false;
			GameManager.instance.fighting = false;
			TurnIndicator.text = "You Win!";
			TurnIndicator.enabled = true;
			Invoke ("win", 1.5f);
		}
		checkAllplayerDied ();

		if (allPlayersdied && GameManager.instance.fighting) {
			GameManager.instance.fighting = false;
			TurnIndicator.text = "You lose!";
			TurnIndicator.enabled = true;
			Invoke ("lose", 1.5f);
		}

		base.Update ();
	}
}
