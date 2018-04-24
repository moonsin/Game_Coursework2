using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skills : MonoBehaviour {

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
			return 4;
		}
		if (Name == "Cleave") {
			return 4;
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
				user.GetComponent<Player>().usingSkillName = "Heal";
				user.GetComponent<Player>().usingSkill = true;
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
				user.GetComponent<Player>().usingSkillName = "Blizzard";
				user.GetComponent<Player>().usingSkill = true;
			} else {
				playerUser.deleteSkillRange ();
				playerUser.SkillRangeShowed = false;
			}
		}

		return true;
	}



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

}
