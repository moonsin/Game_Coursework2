using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerFightManager : MonoBehaviour {

	private TowerFightBoardManager TowerFightBoardScript;

	// Use this for initialization
	void Awake () {
		TowerFightBoardScript = GetComponent<TowerFightBoardManager>();
	}

	public void SetupScene (){
		TowerFightBoardScript.SetupBoard ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
