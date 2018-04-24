using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class blackbackground : MonoBehaviour {

	public Text story1;
	public Text story2;
	public Text story3;

	private Text skipText;
	// Use this for initialization
	void Start () {
		
		story1 = this.transform.Find("story1").GetComponent<Text>();
		story2 = this.transform.Find("story2").GetComponent<Text>();
		story3 = this.transform.Find("story3").GetComponent<Text>();
		skipText = this.transform.Find("skipText").GetComponent<Text>();
		init ();



		Invoke ("showStory2", 3f);
	}

	void init(){
		
		story1.enabled = true;

	}

	void showStory2(){

		story1.enabled = false;
		story2.enabled = true;

		Invoke ("showStory3", 7f);

	}

	void showStory3(){
		
		story2.enabled = false;
		story3.enabled = true;

		Invoke ("BlackBackgroundFinished", 3f);
	}

	void BlackBackgroundFinished(){

		Destroy (GameObject.FindGameObjectWithTag ("blackbackground"));

	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.anyKeyDown) {
			skipText.enabled = true;
			if(Input.GetKeyDown("space")){
				BlackBackgroundFinished ();
			}
		}
	}
}
