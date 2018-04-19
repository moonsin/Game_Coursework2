using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGTALK.Localization;

public class firstChat : MonoBehaviour {

	public RPGTalk rpgTalk;
	public GameObject BE1;

	// Use this for initialization
	void Start(){
		rpgTalk.OnMadeChoice += OnMadeChoice;
	}

	void showBE1(){
		Destroy (GameObject.FindGameObjectWithTag ("Story"));
		GameObject instance = Instantiate (BE1) as GameObject;
	}

	void OnMadeChoice(int questionID, int choiceID){
		//Debug.Log("Aha! In the question "+questionID+" you choosed the option "+choiceID);
		Debug.Log(choiceID);
		if (choiceID == 2) {
			//rpgTalk.NewTalk("1","2");
			Invoke("showBE1",2f);
		}

	}
		
	
	// Update is called once per frame
	void Update () {
		
	}
}
