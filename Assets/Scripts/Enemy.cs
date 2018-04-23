using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class Enemy : MovingObject {

	private Vector3 target;
	private Vector3 BestPlace;
	private Player PlayerTarget;
	private bool EnemyOwnTurn = false;

	// Use this for initialization
	void Awake () {
		base.Awake ();
		GameManager.instance.AddEnemyToList (this);

	}

	private void delayAttack(){
		PlayerTarget = isNearPlayer ();
		if (PlayerTarget != null) {
			normalAttack (this.gameObject, PlayerTarget.gameObject);
		}
	}

	public void run(){
		EnemyOwnTurn = true;

		PlayerTarget = isNearPlayer ();

		if (PlayerTarget == null) {
			MoveEnemy ();
			Invoke ("delayAttack", 1f);

		} else {
			normalAttack (this.gameObject, PlayerTarget.gameObject);
		}

		Invoke ("turnOver", 1.5f);
	}

	private void turnOver(){
		EnemyOwnTurn = false;
		hideCharacterIndicator ();
		BoardManager.instance.MoveToNextCharacter = true;
	
	}

	private void MoveEnemy(){
		
		setMoveRange (Convert.ToInt32(ObjGridVec.x),Convert.ToInt32(ObjGridVec.y));
		//RendeMoveRange (MoveRange);
		target = FindTarget ();
		BestPlace = new Vector3 (-1f, -1f, 0f);

		float shortestDis = 10000;
		for (int i = 0; i < MoveRange.Count; i++) {
			float distance = Mathf.Abs (Vector3.Distance (target, MoveRange [i]));
			if (distance < shortestDis) {
				shortestDis = distance;
				BestPlace = MoveRange [i];
			}
		}

		//moveFinished = false;
		if(BestPlace != new Vector3(FarPosValue,FarPosValue,0f)){
			BoardManager.instance.floorMoveableArray [Convert.ToInt32 (ObjGridVec.y),Convert.ToInt32 (ObjGridVec.x)] = 1;
			setBestPath (BestPlace);
			ObjGridVec = BestPlace;
			BoardManager.instance.floorMoveableArray [Convert.ToInt32 (ObjGridVec.y), Convert.ToInt32 (ObjGridVec.x)] = 0;

		//moving = true;
			movingToNum = bestPath.Count - 1;
		//print (movingToNum);
		}
		MoveRange.Clear ();

	}

	Vector3 FindTarget(){

		List<Player> players = GameManager.instance.players;

		float shortestDis = 10000;
		Vector3 target = new Vector3(0f,0f,0f);

		for (int i = 0; i < players.Count; i++) {
			Vector3 playerPos = players [i].ObjGridVec;
			float distance = Mathf.Abs(Vector3.Distance (this.ObjGridVec, playerPos));
			if (distance < shortestDis) {
				shortestDis = distance;
				target = playerPos;
			}
		}

		return target;
	}

	private Player isNearPlayer(){
		Player nearPlayerTarget = new Player();
		setAttackRange(Convert.ToInt32(ObjGridVec.x),Convert.ToInt32(ObjGridVec.y));
		//RendeAttackRange (AttackRange);

		for (int i = 0; i < AttackRange.Count; i++) {
			//AttackObjTransFromIntoGridPos (h.transform.position)
			for(int i2 = 0; i2< GameManager.instance.players.Count; i2++){
				if (AttackRange [i] == GameManager.instance.players [i2].ObjGridVec) {
					nearPlayerTarget = GameManager.instance.players [i2];
				}
			}
		}

		return nearPlayerTarget;

	}


 	
	// Update is called once per frame
	void Update () {

		if (alive && hp <= 0) {
			BoardManager.instance.allButtonDisabled ();
			alive = false;
			animator.SetTrigger ("death");
			Invoke ("deleteCharacter", 1.5f);
		}

		if (EnemyOwnTurn) {
			showCharacterIndicator ();
		}

		base.Update ();
	}
}
