using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerFightBoardManager : BoardManager {

	public GameObject[] firstFloorTiles;
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
	int[] firstFloorPlayers = new int[]{0};
	int[] firstFloorEnemies = new int[]{0};	


	// Use this for initialization
	void Awake(){

		columns = 8;
		rows = 8;

		boardHolder = new GameObject ("Board").transform;
		playersHolder = new GameObject ("Players").transform;
		enemiesHolder = new GameObject ("Enemies").transform;

		TilesInstance = new GameObject[8, 8];

		playersPos = new int[1, 2]{ {7, 4}};
		enemiesPos = new int[1, 2]{ {1, 4}};

		floorMoveableArray = new int[8, 8];

		initFloorMoveableArray ();
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


		

	public void SetupBoard(){

		firstFloorSetup ();
		setCharacters (playersPos,firstFloorPlayers,0);
		setCharacters (enemiesPos,firstFloorEnemies,1);
	
	}

	private void firstFloorSetup (){

		int LayerOrder = 0;

		for (int y = rows - 1; y >= 0; y--) {
			for (int x = 0; x < columns; x++) {
				GameObject toInstantiate = firstFloorTiles [firstFloormap[y,x]];

				setFloor (toInstantiate, LayerOrder, x ,y,firstFloorHeightMap[y,x]);

				LayerOrder += 1;
			}
		}
			
	}

	// Update is called once per frame
	void Update () {
		
	}
}
