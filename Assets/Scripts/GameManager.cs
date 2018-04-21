using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	private startStoryManager startStoryScript;
	private TowerFightManager towerFightScript;
	public static bool isTowerFight = true;

	public List<Player> players;
	public List<Enemy> enemies;

	public int onMovePlayerIndex = 0;

	void Awake(){

		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}

		startStoryScript = GetComponent<startStoryManager>();

		towerFightScript = GetComponent<TowerFightManager>();

		//InitGame ();
	}


	public void AddPlayerToList (Player script){
		players.Add (script);
	}

	public void AddEnemyToList(Enemy script){
		enemies.Add (script);
	}

	void InitGame(){
		startStoryScript.SetupScene ();
	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (isTowerFight) {
			isTowerFight = false;
			towerFightScript.SetupScene ();
		}

	}
}
