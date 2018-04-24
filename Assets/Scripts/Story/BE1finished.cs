using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BE1finished : MonoBehaviour {

	public GameObject title;
	public GameObject Ghost;

	// Use this for initialization
	void Start () {
		
	}

	void finished(){

		title.GetComponent<Text> ().enabled = true;
		if (Ghost != null) {
			Ghost.GetComponent<Image> ().enabled = true;
			Ghost.GetComponent<Animator> ().enabled = true;
		}
	} 

	// Update is called once per frame
	void Update () {
		
	}
}
