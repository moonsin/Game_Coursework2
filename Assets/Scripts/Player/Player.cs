using System.Collections;
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
								skillPoint -= 2;
								updatePlayerIndicator ();
							}
							normalAttack (this.gameObject, GameManager.instance.enemies [i].gameObject);

							AttackButton.instance.disable ();
							alreadyAttacked = true;

						}
					}
				}
			}

			if (isCleave) {
				if (skillPoint < 2) {
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
