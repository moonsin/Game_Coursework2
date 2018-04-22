using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlButton : MonoBehaviour {

	protected bool disabled = false;
	public Player player;

	// Use this for initialization
	public void disable(){
		disabled = true;
		this.GetComponent<Image> ().color = Color.grey;
	}

	public void enable(){
		disabled = false;
		this.GetComponent<Image> ().color = Color.white;
	}



	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
