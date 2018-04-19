using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bg2Finished : MonoBehaviour {

	public GameObject firstChat;

	// Use this for initialization
	void Start () {
		
	}

	void finished(){
		
		Destroy (GameObject.FindGameObjectWithTag ("Story"));

		GameObject instance = Instantiate (firstChat) as GameObject;

	} 

	// Update is called once per frame
	void Update () {
		
	}
}
