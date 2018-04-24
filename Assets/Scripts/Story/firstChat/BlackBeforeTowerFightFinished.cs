﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBeforeTowerFightFinished : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	void finished(){
		Destroy (GameObject.FindGameObjectWithTag ("Story"));
	
		GameManager.instance.isTowerFight = true;
		GameManager.instance.fighting = true;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
