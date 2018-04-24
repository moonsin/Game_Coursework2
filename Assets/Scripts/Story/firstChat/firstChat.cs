using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGTALK.Localization;

public class firstChat : MonoBehaviour {

	public RPGTalk rpgTalk;
	public GameObject BE1;
	public GameObject BlackBeforeTowerFight;
	public GameObject BlackBeforeReadyFight;
	public GameObject BeforeUnReadyFight;

	// Use this for initialization
	void Start(){
		rpgTalk.OnMadeChoice += OnMadeChoice;
	}

	void showBE1(){
		Destroy (GameObject.FindGameObjectWithTag ("Story"));
		GameObject instance = Instantiate (BE1) as GameObject;
	}
		
	void readyFight(){
		Destroy (GameObject.FindGameObjectWithTag ("Story"));
		GameObject instance = Instantiate (BlackBeforeReadyFight) as GameObject;
	}

	void towerFight(){
		Destroy (GameObject.FindGameObjectWithTag ("Story"));
		GameObject instance = Instantiate (BlackBeforeTowerFight) as GameObject;
	}

	void unreadyFight(){
		Destroy (GameObject.FindGameObjectWithTag ("Story"));
		GameObject instance = Instantiate (BeforeUnReadyFight) as GameObject;
	}

	void OnMadeChoice(int questionID, int choiceID){
		//Debug.Log("Aha! In the question "+questionID+" you choosed the option "+choiceID);
		Debug.Log(choiceID);
		if (choiceID == 2) {
			//rpgTalk.NewTalk("1","2");
			Invoke ("unreadyFight", 1.5f);
		} else if (choiceID == 1) {
			Invoke ("towerFight", 1.5f);
		} else if (choiceID == 0) {
			Invoke ("readyFight", 1.5f);

		}

	}
		
	
	// Update is called once per frame
	void Update () {
		
	}
}
