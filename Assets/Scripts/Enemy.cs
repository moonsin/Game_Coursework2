using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject {



	// Use this for initialization
	void Awake () {

		GameManager.instance.AddEnemyToList (this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
