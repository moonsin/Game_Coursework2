using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveButton : MonoBehaviour {

	public Player player;
	private bool disabled;
	// Use this for initialization
	void Start () {
		Button btn = this.GetComponent<Button> ();
		btn.onClick.AddListener (OnClick);
	}

	private void OnClick(){
		player = GameManager.instance.players [GameManager.instance.onMovePlayerIndex];
		if (!player.alreadyMoved) {
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
