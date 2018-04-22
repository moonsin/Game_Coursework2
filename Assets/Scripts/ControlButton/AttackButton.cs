using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackButton : ControlButton {

	public static AttackButton instance = null;

	// Use this for initialization
	void Start () {
		if (instance == null) {
			instance = this;
		} 

		Button btn = this.GetComponent<Button> ();
		btn.onClick.AddListener (OnClick);
	}

	private void OnClick(){
		//player = GameManager.instance.players [GameManager.instance.onMovePlayerIndex];

		if (!disabled) {
			player = BoardManager.instance.fightOrderArray[BoardManager.instance.CharacterOrderController].GetComponent<Player>();
			if (!player.AttackRangeShowed) {
				player.showAttackRange ();
				player.AttackRangeShowed = true;
			} else {
				player.deleteAttackRange ();
				player.AttackRangeShowed = false;
			}
		} 
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
