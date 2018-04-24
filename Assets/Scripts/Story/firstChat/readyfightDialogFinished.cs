using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class readyfightDialogFinished : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	void finished(){
		Destroy (GameObject.FindGameObjectWithTag ("Story"));
		GameManager.instance.GetComponent<ReadyFight> ().readyFightBegin = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
