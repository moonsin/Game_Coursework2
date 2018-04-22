using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestButton : ControlButton {

	public static RestButton instance = null;

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
			player.OwnTurn = false;
			player.deleteMoveRange ();
			player.deleteAttackRange ();
			BoardManager.instance.allButtonDisabled();
			BoardManager.instance.MoveToNextCharacter = true;
		} 
	}

	// Update is called once per frame
	void Update () {
		
	}
}
