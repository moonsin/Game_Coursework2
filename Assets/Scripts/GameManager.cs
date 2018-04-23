﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	private startStoryManager startStoryScript;
	private TowerFightManager towerFightScript;
	public bool isTowerFight = false;
	public bool fighting = false;
	public Canvas controlCanvas;

	public List<Player> players;
	public List<Enemy> enemies;



	void Awake(){

		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
		controlCanvas.enabled = false;
		startStoryScript = GetComponent<startStoryManager>();

		towerFightScript = GetComponent<TowerFightManager>();

		InitGame ();
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
			controlCanvas.enabled = true;
			towerFightScript.SetupScene ();
		}
	}
}
