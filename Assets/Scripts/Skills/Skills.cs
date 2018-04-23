using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skills : MonoBehaviour {
	public bool usingSkill = false;
	public string usingSkillName;
	private Vector3 clickPos;
	private GameObject Skilluser;

	public void useSkill(string Name, GameObject user){
		if (Name == "Heal") {
			Heal (user);
		}else if(Name == "Blizzard"){
			Blizzard (user);
		}
	}

	public int getCost(string Name){

		if (Name == "Heal") {
			return 3;
		}
		if (Name == "Cleave") {
			return 3;
		}
		if (Name == "Blizzard") {
			return 4;
		}
		return 0;
	}

	public string getIntroductionContent(string Name){

		if (Name == "Heal") {
			return "Cleric restores 9 Health to individual friendly role.";
		}
		if (Name == "Cleave") {
			return "(This passive skill) Whenever Prince destroys an enemy, one more Attack can be released immediately. This passive skill will cost extra 2 Skill Points.";
		}
		if (Name == "Blizzard") {
			return "Mage makes 10 fixed Attack Damage to the goal enemy. ";
		}
		return "0";
	}

	public void Cleave(){
		
	}

	public bool Heal(GameObject user){

		if (user.tag == "Player") {
			Player playerUser;
			playerUser = user.GetComponent<Player> ();

			if (playerUser.skillPoint < getCost ("Heal")) {
				return false;
			}
			if (!playerUser.SkillRangeShowed) {
				
				playerUser.deleteAttackRange ();
				playerUser.deleteMoveRange ();

				playerUser.showSkillRange ();
				Skilluser = user;
				usingSkillName = "Heal";
				usingSkill = true;
			} else {
				playerUser.deleteSkillRange ();
				playerUser.SkillRangeShowed = false;
			}

		}
	
		return true;
	}

	public bool Blizzard(GameObject user){
		if (user.tag == "Player") {
			Player playerUser;
			playerUser = user.GetComponent<Player> ();

			if (playerUser.skillPoint < getCost ("Blizzard")) {
				return false;
			}

			if (!playerUser.SkillRangeShowed) {

				playerUser.deleteAttackRange ();
				playerUser.deleteMoveRange ();

				playerUser.showSkillRange ();
				Skilluser = user;
				usingSkillName = "Blizzard";
				usingSkill = true;
			} else {
				playerUser.deleteSkillRange ();
				playerUser.SkillRangeShowed = false;
			}
		}

		return true;
	}

	private bool IsMouseOnSkillRange(Vector3 mousePos){
		Collider2D h = Physics2D.OverlapPoint (mousePos);
		if (h == null) {
			return false;
		}
		else 
			return ( h.tag == "skillRange");
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if (usingSkill) {
			clickPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);


			if (usingSkillName == "Heal") {
				if (Input.GetMouseButtonUp (0) && IsMouseOnSkillRange (clickPos)) {
					Collider2D h = Physics2D.OverlapPoint (clickPos);
					Skilluser.GetComponent<Player> ().deleteSkillRange ();

					for (int i = 0; i < GameManager.instance.players.Count; i++) {
						if (GameManager.instance.players [i] != null) {
							if (GameManager.instance.players [i].ObjGridVec == Skilluser.GetComponent<Player>().AttackObjTransFromIntoGridPos (h.transform.position)) {

								Skilluser.GetComponent<Player> ().animator.SetTrigger ("skill_2");
								Skilluser.GetComponent<Player> ().skillPoint -= 2;
								Skilluser.GetComponent<Player> ().updatePlayerIndicator ();

								GameManager.instance.players [i].hp += 9;
								if (GameManager.instance.players [i].hp > GameManager.instance.players [i].totalHP) {
									GameManager.instance.players [i].hp = GameManager.instance.players [i].totalHP;
								}
								UseSkill.instance.disable ();

							}
						}
					}
				} 
			}


			if (usingSkillName == "Blizzard") {
				if (Input.GetMouseButtonUp (0) && IsMouseOnSkillRange (clickPos)) {
					Collider2D h = Physics2D.OverlapPoint (clickPos);
					Skilluser.GetComponent<Player> ().deleteSkillRange ();

					for (int i = 0; i < GameManager.instance.enemies.Count; i++) {
						if (GameManager.instance.enemies [i] != null) {
							if (GameManager.instance.enemies [i].ObjGridVec == Skilluser.GetComponent<Player>().AttackObjTransFromIntoGridPos (h.transform.position)) {

								Skilluser.GetComponent<Player> ().animator.SetTrigger ("skill_2");
								Skilluser.GetComponent<Player> ().skillPoint -= 4;
								Skilluser.GetComponent<Player> ().updatePlayerIndicator ();

								GameManager.instance.enemies [i].GetComponent<Enemy> ().hit (10, 0);
								
								UseSkill.instance.disable ();

							}
						}
					}
				}
			
			}


		}
	}

}
