using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeforeUnReadyFight : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	void finished(){
		Destroy (GameObject.FindGameObjectWithTag ("Story"));
		GameManager.instance.isUnReadyFight = true;
		GameManager.instance.fighting = true;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
