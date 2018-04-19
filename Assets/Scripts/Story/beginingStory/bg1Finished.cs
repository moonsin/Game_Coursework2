using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGTALK.Localization;


public class bg1Finished : MonoBehaviour {

	public RPGTalk rpgTalk;

	// Use this for initialization
	void Start () {
		
	}

	void finished(){
		Destroy (GameObject.FindGameObjectWithTag ("blackbackground"));
	} 
	
	// Update is called once per frame
	void Update () {
		
	}
}
