using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveButton : ControlButton {


	public static MoveButton instance = null;


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

			player.deleteAttackRange ();
			player.deleteSkillRange ();

			if (!player.MoveRangeShowed) {
				player.showMoveRange ();
				player.MoveRangeShowed = true;
			} else {
				player.deleteMoveRange ();
				player.MoveRangeShowed = false;
			}
		} 
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
