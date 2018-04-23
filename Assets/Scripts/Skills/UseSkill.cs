using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseSkill : ControlButton {
	public static UseSkill instance = null;


	// Use this for initialization
	void Start () {
		if (instance == null) {
			instance = this;
		} 

		Button btn = this.GetComponent<Button> ();
		btn.onClick.AddListener (OnClick);
	}

	private void OnClick(){


		if (!disabled) {
			player = BoardManager.instance.fightOrderArray[BoardManager.instance.CharacterOrderController].GetComponent<Player>();
			player.GetComponent<Skills> ().useSkill (player.skillNames [0], player.gameObject);
		}
	
	}
	
	// Update is called once per frame
	void Update () {
		
		if (!disabled && GameManager.instance.fighting) {
			player = BoardManager.instance.fightOrderArray[BoardManager.instance.CharacterOrderController].GetComponent<Player>();
			if (player.skillPoint < player.GetComponent<Skills> ().getCost (player.skillNames [0])) {
				disable ();
			}
			if (player.skillNames [0] == "Cleave") {
				disable ();
			}
		}	

	}
}
