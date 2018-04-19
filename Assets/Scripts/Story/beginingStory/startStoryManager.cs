using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startStoryManager : MonoBehaviour {

	public GameObject startStoryCanvas;

	// Use this for initialization
	void Start () {
		
	}

	void CanvasSetup(){
		
		Instantiate (startStoryCanvas);

	}

	public void SetupScene (){
		CanvasSetup ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
