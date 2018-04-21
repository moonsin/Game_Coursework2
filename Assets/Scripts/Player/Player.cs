using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Player : MovingObject {

	//Move
	public bool MoveRangeShowed = false;
	public bool alreadyMoved = false;
	private Vector3 clickPos;
	protected int movingToNum;

	GameObject moveRangeHolder;

	// Use this for initialization
	void Awake () {
		GameManager.instance.AddPlayerToList (this);
	}

	public void deleteMoveRange(){
		GameObject[] moveRangeInstances;
		moveRangeInstances = GameObject.FindGameObjectsWithTag ("moveRange");
		moveRangeHolder = GameObject.Find ("moveRange");
		print (moveRangeInstances);
		//Thread.Sleep (50);
		foreach (GameObject element in moveRangeInstances) {
			Destroy (element);
		}
		Destroy (moveRangeHolder);
		//moveRanges.Clear ();

	}

	private bool IsMouseOnMoveRange(Vector3 mousePos){
		Collider2D h = Physics2D.OverlapPoint (mousePos);
		if (h == null) {
			return false;
		}
		else 
			return ( h.tag == "moveRange");
	}

	public void showMoveRange(){
		setMoveRange (Convert.ToInt32(ObjGridVec.x),Convert.ToInt32(ObjGridVec.y));
		RendeMoveRange (MoveRange);
	}

	// Update is called once per frame
	void Update () {

		clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		if (Input.GetMouseButtonUp (0) && IsMouseOnMoveRange (clickPos)) {

			Collider2D h = Physics2D.OverlapPoint (clickPos);
			BoardManager.instance.floorMoveableArray [Convert.ToInt32 (ObjGridVec.y),Convert.ToInt32 (ObjGridVec.x)] = 1;
			setBestPath(MovingObjTransFromIntoGridPos (h.transform.position));

			ObjGridVec = MovingObjTransFromIntoGridPos (h.transform.position);
			BoardManager.instance.floorMoveableArray [Convert.ToInt32 (ObjGridVec.y), Convert.ToInt32 (ObjGridVec.x)] = 0;
			//setBestPath (new Vector3(6f,8f,0f));
			movingToNum = bestPath.Count - 1;
		}


		for (int i = bestPath.Count - 1; i >= 0; i--) {
			if (Vector3.Distance (this.transform.position, MovingObjTransFromIntoWorldPos(bestPath [i])) >= float.Epsilon && movingToNum == i) {
				//如果向近的地方移动就先增加order，远的地方就后改变
				if (this.transform.position.y > MovingObjTransFromIntoWorldPos (bestPath [i]).y ||(
					this.transform.position.y == MovingObjTransFromIntoWorldPos (bestPath [i]).y && this.transform.position.x < MovingObjTransFromIntoWorldPos (bestPath [i]).x
				)) {
					this.GetComponent<SpriteRenderer> ().sortingOrder = BoardManager.instance.TilesInstance[Convert.ToInt32(bestPath [i].x), Convert.ToInt32((bestPath [i].y))].GetComponent<SpriteRenderer> ().sortingOrder + 1;
				}

				move (bestPath [i]);
			}else if (Vector3.Distance (this.transform.position, MovingObjTransFromIntoWorldPos(bestPath [i])) <= float.Epsilon && movingToNum == i){
				movingToNum -= 1;
				this.GetComponent<SpriteRenderer> ().sortingOrder = BoardManager.instance.TilesInstance[Convert.ToInt32(bestPath [i].x), Convert.ToInt32((bestPath [i].y))].GetComponent<SpriteRenderer> ().sortingOrder + 1;
			}
		}

		if (movingToNum == -1) {
			deleteMoveRange ();
			MoveRangeShowed = false;
			movingToNum -= 1;
		}
	}
}
