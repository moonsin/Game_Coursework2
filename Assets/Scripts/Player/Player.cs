﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Player : MovingObject {

	//Move

	public bool alreadyMoved = false;
	public bool alreadyAttacked = false;

	private Vector3 clickPos;
	public bool AttackRangeShowed = false;
	public int ObjectIndex;
	public bool skillShowed = false;
	public bool SkillRangeShowed = false;
	public bool isCleave = false;

	public bool usingSkill = false;
	public string usingSkillName;

	private bool EnemyKilled = false;



	GameObject moveRangeHolder;

	// Use this for initialization
	void Awake () {
		
		base.Awake ();
		GameManager.instance.AddPlayerToList (this);
	}

	public void deleteMoveRange(){
		GameObject[] moveRangeInstances;
		moveRangeInstances = GameObject.FindGameObjectsWithTag ("moveRange");
		moveRangeHolder = GameObject.Find ("moveRange");

		//Thread.Sleep (50);
		foreach (GameObject element in moveRangeInstances) {
			Destroy (element);
		}
		Destroy (moveRangeHolder);
		//moveRanges.Clear ();
		MoveRangeShowed = false;
	}

	public void deleteAttackRange(){
		GameObject[] moveRangeInstances;
		moveRangeInstances = GameObject.FindGameObjectsWithTag ("attackRange");
		moveRangeHolder = GameObject.Find ("attackRange");

		//Thread.Sleep (50);
		foreach (GameObject element in moveRangeInstances) {
			Destroy (element);
		}
		Destroy (moveRangeHolder);

		AttackRangeShowed = false;
	}

	public void deleteSkillRange(){
		GameObject[] moveRangeInstances;
		moveRangeInstances = GameObject.FindGameObjectsWithTag ("skillRange");
		moveRangeHolder = GameObject.Find ("attackRange");

		//Thread.Sleep (50);
		foreach (GameObject element in moveRangeInstances) {
			Destroy (element);
		}
		Destroy (moveRangeHolder);

		SkillRangeShowed = false;
	}


	private bool IsMouseOnMoveRange(Vector3 mousePos){
		Collider2D h = Physics2D.OverlapPoint (mousePos);
		if (h == null) {
			return false;
		}
		else 
			return ( h.tag == "moveRange");
	}

	private bool IsMouseOnAttackRange(Vector3 mousePos){
		Collider2D h = Physics2D.OverlapPoint (mousePos);
		if (h == null) {
			return false;
		}
		else 
			return ( h.tag == "attackRange");
	}

	private bool IsMouseOnSkillRange(Vector3 mousePos){
		Collider2D h = Physics2D.OverlapPoint (mousePos);
		if (h == null) {
			return false;
		}
		else 
			return ( h.tag == "skillRange");
	}

	public void showMoveRange(){
		setMoveRange (Convert.ToInt32(ObjGridVec.x),Convert.ToInt32(ObjGridVec.y));
		RendeMoveRange (MoveRange);
	}

	public void showAttackRange(){
		setAttackRange(Convert.ToInt32(ObjGridVec.x),Convert.ToInt32(ObjGridVec.y));
		RendeAttackRange (AttackRange);
	}

	public void showSkillRange(){
		setAttackRange(Convert.ToInt32(ObjGridVec.x),Convert.ToInt32(ObjGridVec.y));
		RendeSkillRange (AttackRange);
	}
		

	// Update is called once per frame
	void Update () {
		if (OwnTurn) {
			
			showCharacterIndicator ();

			clickPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);

			if (Input.GetMouseButtonUp (0) && IsMouseOnMoveRange (clickPos)) {

				Collider2D h = Physics2D.OverlapPoint (clickPos);

				deleteMoveRange ();
				BoardManager.instance.floorMoveableArray [Convert.ToInt32 (ObjGridVec.y), Convert.ToInt32 (ObjGridVec.x)] = 1;
				setBestPath (MovingObjTransFromIntoGridPos (h.transform.position));

				ObjGridVec = MovingObjTransFromIntoGridPos (h.transform.position);
				BoardManager.instance.floorMoveableArray [Convert.ToInt32 (ObjGridVec.y), Convert.ToInt32 (ObjGridVec.x)] = 0;
				//setBestPath (new Vector3(6f,8f,0f));
				movingToNum = bestPath.Count - 1;
				MoveButton.instance.disable ();
				alreadyMoved = true;
			}


			if (Input.GetMouseButtonUp (0) && IsMouseOnAttackRange (clickPos)) {

				Collider2D h = Physics2D.OverlapPoint (clickPos);

				deleteAttackRange ();
				//print(AttackObjTransFromIntoGridPos (h.transform.position));
				for (int i = 0; i < GameManager.instance.enemies.Count; i++) {
					//print (GameManager.instance.enemies [BoardManager.instance.fightEnemiesIndex [i]]);
					if (GameManager.instance.enemies [i] != null) {
						if (GameManager.instance.enemies [i].ObjGridVec == AttackObjTransFromIntoGridPos (h.transform.position)) {

							if (isCleave) {
								skillPoint -= 4;
								updatePlayerIndicator ();
							}

							normalAttack (this.gameObject, GameManager.instance.enemies [i].gameObject);

							UseSkill.instance.disable ();
							AttackButton.instance.disable ();
							alreadyAttacked = true;

						}
					}
				}
			}

			if (Input.GetMouseButtonUp (0) && IsMouseOnSkillRange (clickPos)) {



				Collider2D h = Physics2D.OverlapPoint (clickPos);
				deleteSkillRange ();

				if (usingSkillName == "Heal") {

					for (int i = 0; i < GameManager.instance.players.Count; i++) {
						if (GameManager.instance.players [i] != null) {
							if (GameManager.instance.players [i].ObjGridVec == AttackObjTransFromIntoGridPos (h.transform.position)) {

								this.animator.SetTrigger ("skill_2");
								this.skillPoint -= 4;


								GameManager.instance.players [i].hp += 9;
								if (GameManager.instance.players [i].hp > GameManager.instance.players [i].totalHP) {
									GameManager.instance.players [i].hp = GameManager.instance.players [i].totalHP;
								}

								updatePlayerIndicator ();
								AttackButton.instance.disable ();
								UseSkill.instance.disable ();

							}
						}
					}

				}


				if (usingSkillName == "Blizzard") {

						for (int i = 0; i < GameManager.instance.enemies.Count; i++) {
							if (GameManager.instance.enemies [i] != null) {
								if (GameManager.instance.enemies [i].ObjGridVec == AttackObjTransFromIntoGridPos (h.transform.position)) {

									this.animator.SetTrigger ("skill_2");
									this.skillPoint -= 4;
									updatePlayerIndicator ();

									GameManager.instance.enemies [i].GetComponent<Enemy> ().hit (10, 0);
									AttackButton.instance.disable ();
									UseSkill.instance.disable ();

								}
							}
						}
				
				}
			
			
			} 


			if (isCleave) {
				if (skillPoint < 4) {
					isCleave = false;
				}
				AttackButton.instance.enable ();
			}

		} else {
			isCleave = false;
		}

		if (alive && hp <= 0) {
			alive = false;
			animator.SetTrigger ("death");
			Invoke ("deleteCharacter", 1.5f);

		}


		base.Update ();

	}
}
