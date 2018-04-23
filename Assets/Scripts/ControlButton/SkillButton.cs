using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : ControlButton {
	
	public static SkillButton instance = null;
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
			BoardManager.instance.SkillIndicator.GetComponent<Image> ().sprite = Resources.Load<Sprite> (player.skillNames [0]);
			BoardManager.instance.SkillIndicatorText.text = player.skillNames [0] + " Cost: " + player.GetComponent<Skills> ().getCost (player.skillNames [0]).ToString ();

			if (!player.skillShowed) {
				BoardManager.instance.SkillIndicator.SetActive(true);
				player.skillShowed = true;
			} else {
				BoardManager.instance.SkillIndicator.SetActive(false);
				player.skillShowed = false;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
