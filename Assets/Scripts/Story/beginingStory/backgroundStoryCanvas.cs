using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backgroundStoryCanvas : MonoBehaviour {

	public GameObject blackbackground;
	public GameObject bg1;
	public GameObject bg2;
	public bool beginingBlackBackgroundFinished = false;

	private GameObject instance = null;
	private int storyNum = 1;

	private Transform canvasHolder;

	// Use this for initialization
	void Awake () {
		
		canvasHolder = this.transform;

		instance = SetBlackBackgroundStory ();

	}

	GameObject SetBlackBackgroundStory(){

		GameObject instance = Instantiate (blackbackground,canvasHolder) as GameObject;

		return instance;
	}

	GameObject Setbg1Story(){
		GameObject instance = Instantiate (bg1,canvasHolder) as GameObject;

		return instance;
	}

	GameObject Setbg2Story(){
		GameObject instance = Instantiate (bg2,canvasHolder) as GameObject;

		return instance;
	}

	// Update is called once per frame
	void Update () {
		if (instance == null) {
			if (storyNum == 1) {
				instance = Setbg1Story ();
				storyNum++;
			}
			else if (storyNum == 2) {
				instance = Setbg2Story ();
				storyNum++;
			}
		}
	}
}
