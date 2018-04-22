using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class Enemy : MovingObject {

	private Vector3 target;
	private Vector3 BestPlace;
	private bool alive = true;

	// Use this for initialization
	void Awake () {
		base.Awake ();
		GameManager.instance.AddEnemyToList (this);

	}

	public void run(){
		//target = isNearPlayer ();

		//if (target == null) {
			MoveEnemy ();
		//}

		Invoke ("turnOver", 1f);
	}

	private void turnOver(){
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

	private void deleteEnemy(){
		BoardManager.instance.floorMoveableArray[Convert.ToInt32 (ObjGridVec.y),Convert.ToInt32 (ObjGridVec.x)] = 1;
		int i = 0;
		for (; BoardManager.instance.fightOrderArray [i] != this.gameObject; i++);
		GameObject.FindGameObjectsWithTag ("orderInName")[i].GetComponent<Text>().color = Color.grey;
		Destroy (this.gameObject);
	}
 	
	// Update is called once per frame
	void Update () {

		if (alive && hp <= 0) {
			alive = false;
			animator.SetTrigger ("death");
			Invoke ("deleteEnemy", 1.5f);
		}

		base.Update ();
	}
}
