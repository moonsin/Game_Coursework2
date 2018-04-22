using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class MovingObject : MonoBehaviour {

	public int attackPoint;
	public int Dexterity;
	public int defendPoint;
	public int hp;
	public int MovingPoint;
	public int AttackDisPoint;
	public int speed = 4;
	public int manaPoint;
	public int skillPoint;
	public int FirstAttackPoint;
	protected bool alive = true;

	public Vector3 ObjGridVec; //网格上的位置

	protected List<Vector3> MoveRange = new List<Vector3> ();
	protected int[,] PathArray;
	protected int[] bestPathArray;
	protected int[] dis;
	protected Transform rangeHolder;
	protected float FarPosValue = 1000f;
	public GameObject moveRangeTile;
	protected List<Vector3> bestPath = new List<Vector3> ();
	protected int movingToNum;
	public bool MoveRangeShowed = false;
	//public bool moving = false;
	//public bool moveFinished = false;

	protected List<Vector3> AttackRange = new List<Vector3> ();
	public GameObject attackRangeTile;

	public Animator animator; 


	// Use this for initialization
	protected void Awake () {
		
		this.MovingPoint = (int)Mathf.Floor ((float)Dexterity / 2f);
		this.FirstAttackPoint = (int)Mathf.Floor (UnityEngine.Random.value* 20 + Dexterity);
		animator = GetComponent<Animator>();

	}

	private void setPathArrayByPos(Vector3 pos, int i){
		int index = MoveRange.IndexOf(pos);
		if(index != -1){
			PathArray[i,index] = 1;
		}
	}

	private void initPathArray(int Count){
		PathArray = new int[Count,Count];
		for(int i = 0 ; i < Count;i++){
			for (int i2 = 0; i2 < Count; i2++) {
				PathArray[i, i2] = 100000;
			}
		}
	}
		
	private void initBestPathArray(int Count){
		bestPathArray = new int[MoveRange.Count];

		for (int i = 0; i < MoveRange.Count; i++) {
			bestPathArray [i] = 0;
		}
	}
		
	protected void setMoveRange(int x, int y){

		Vector3 newMovePos;
		MoveRange.Clear ();

		MoveRange.Add(new Vector3 (x, y, -0f));

		for (int i = 0; i <= MovingPoint; i++) {
			if (i == 0) {
				for (int i2 = -MovingPoint; i2 <= MovingPoint; i2++) {
					if (x + i2 < BoardManager.instance.columns && y + i < BoardManager.instance.rows && x +i2 >=0) {
						if (BoardManager.instance.floorMoveableArray [y + i,x + i2] != 0) {
							newMovePos = new Vector3 (x + i2, y + i, 0f);
							MoveRange.Add (newMovePos);
						}
					}

				}
			}else {
				for (int i2 = -MovingPoint+i; i2 <= MovingPoint-i; i2++) {
					if (x + i2 < BoardManager.instance.columns && y + i < BoardManager.instance.rows && x+i2 >=0 && y+i >=0) {
						
						if (BoardManager.instance.floorMoveableArray [y + i,x + i2] != 0) {
							newMovePos = new Vector3 (x + i2, y + i, -0f);
							MoveRange.Add (newMovePos);
						}
					}

					if (x + i2 < BoardManager.instance.columns && y - i > -1 && x + i2 >=0 ) {
						if (BoardManager.instance.floorMoveableArray [y - i,x + i2] != 0) {
							newMovePos = new Vector3 (x + i2, y - i, -0f);
							MoveRange.Add (newMovePos);
						}
					}

				}
			}
		}

		initPathArray(MoveRange.Count);
		initBestPathArray (MoveRange.Count);

		for (int i = 0; i < MoveRange.Count; i++) {
			PathArray [i,i] = 0;
			Vector3 currentPos = MoveRange [i];
			Vector3 leftPos = new Vector3 (currentPos.x - 1, currentPos.y, currentPos.z);
			Vector3 rightPos = new Vector3 (currentPos.x + 1, currentPos.y, currentPos.z);
			Vector3 topPos = new Vector3 (currentPos.x, currentPos.y + 1, currentPos.z);
			Vector3 downPos = new Vector3 (currentPos.x, currentPos.y - 1, currentPos.z);
			setPathArrayByPos (leftPos, i);
			setPathArrayByPos (rightPos, i);
			setPathArrayByPos (topPos, i);
			setPathArrayByPos (downPos, i);
		}

		dis = new int[MoveRange.Count];
		for (int i = 0; i < MoveRange.Count; i++) {
			dis [i] = PathArray [0, i];
		}

		bool[] doneArray = new bool[MoveRange.Count];
		doneArray [0] = true;
		for (int i = 1; i < MoveRange.Count; i++) {
			doneArray [i] = false;
		}

		for(int i = 0 ; i< MoveRange.Count-1;i++){
			int bestIndex = 0;
			int nerestPathLength = 100000;

			for (int i2 = 0; i2 < MoveRange.Count; i2++) {
				if (dis [i2] <= nerestPathLength && !doneArray [i2]) {
					bestIndex = i2;
					nerestPathLength = dis [i2];
				}
			}

			doneArray [bestIndex] = true;
			for (int i2 = 0; i2 < MoveRange.Count; i2++) {
				int newPathLength = PathArray [bestIndex, i2] + nerestPathLength;
				if (newPathLength < dis [i2]) {
					bestPathArray [i2] = bestIndex;
					dis [i2] = newPathLength;
				}
			}
		}

		for (int i = 0; i < MoveRange.Count; i++) {
			if (dis [i] > MovingPoint) {
				MoveRange[i] = new Vector3(x,y,-0f);
			}
		}
		//moveRanges.Remove(moveRanges[0]);

		int originIndex = MoveRange.IndexOf (new Vector3 (x, y, -0f));

		while ( originIndex != -1) {
			//MoveRange.Remove(MoveRange[originIndex]);
			MoveRange[originIndex] = new Vector3(FarPosValue,FarPosValue,0f);
			originIndex = MoveRange.IndexOf (new Vector3 (x, y, -0f));
		}


	}


	protected Vector3 RangeTransFromIntoWorldPos(Vector3 pos){
		Vector3 vec = new Vector3();
		int x = Convert.ToInt32 (pos.x);
		int y = Convert.ToInt32 (pos.y);

		vec.x = x+y;
		vec.y = BoardManager.instance.TilesInstance[x, y].transform.position.y + 0.75f;

		return vec;
	}

	protected void setSingleRange(int x, int y, string Type, GameObject toInstantiate){


		//跳过原地坐标的图
		if (x < FarPosValue) {
			Vector3 vec  =RangeTransFromIntoWorldPos(new Vector3(x,y,0f));

			if (Type == "move") {
				toInstantiate = moveRangeTile;
			} else if(Type == "attack") {
				toInstantiate = attackRangeTile;
			}

			toInstantiate.GetComponent<SpriteRenderer> ().sortingOrder = BoardManager.instance.TilesInstance[x, y].GetComponent<SpriteRenderer> ().sortingOrder + 1;

			GameObject instance =
				Instantiate (toInstantiate, vec, Quaternion.identity) as GameObject;


			//Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
			instance.transform.SetParent (rangeHolder);
		} 

	}

	protected void RendeMoveRange(List<Vector3> MoveRange){
		rangeHolder = new GameObject ("moveRange").transform;
		foreach (Vector3 pos in MoveRange) {
			setSingleRange (Convert.ToInt32(pos.x),Convert.ToInt32(pos.y), "move", moveRangeTile);
		}
	}

	protected void RendeAttackRange(List<Vector3> AttackRange){
		rangeHolder = new GameObject ("attackRange").transform;
		foreach (Vector3 pos in AttackRange) {
			setSingleRange (Convert.ToInt32(pos.x),Convert.ToInt32(pos.y), "attack", attackRangeTile);
		}
	}

	protected void setBestPath(Vector3 newPos){

		bestPath = new List<Vector3> ();

		int targetIndex = MoveRange.IndexOf (newPos);
		bestPath.Add (newPos);
		while(bestPathArray[targetIndex]!=0){
			bestPath.Add (MoveRange [bestPathArray [targetIndex]]);
			targetIndex = bestPathArray [targetIndex];
		}
	}


	protected Vector3 MovingObjTransFromIntoGridPos(Vector3 Pos){
		Vector3 newPos = new Vector3();

		foreach(Vector3 vec in MoveRange){
			if (vec.x != FarPosValue) {
				if (RangeTransFromIntoWorldPos (vec) == Pos) {
					newPos = vec;
				}
			}
		}

		return newPos;
	}

	protected Vector3 AttackObjTransFromIntoGridPos(Vector3 Pos){
		Vector3 newPos = new Vector3();

		foreach(Vector3 vec in AttackRange){
			if (vec.x != FarPosValue) {
				if (RangeTransFromIntoWorldPos (vec) == Pos) {
					newPos = vec;
				}
			}
		}

		return newPos;
	}

	protected Vector3 MovingObjTransFromIntoWorldPos(Vector3 newPos){


		Vector3 realPos = new Vector3();

		int x = Convert.ToInt32 (newPos.x);
		int y = Convert.ToInt32 (newPos.y);
		realPos.x = x + y;
		if (this.tag == "PlayerOwn") {
			realPos.y = BoardManager.instance.TilesInstance [x, y].transform.position.y + 1.3f;
		} else {
			realPos.y = BoardManager.instance.TilesInstance [x, y].transform.position.y + 0.7f;
		}

		return realPos;

	}

	protected void move(Vector3 newPos){
		
		setMoveAnimate ();
		float step = speed * Time.deltaTime;
		if (MovingObjTransFromIntoWorldPos (newPos).x < this.transform.position.x) {
			this.transform.rotation = Quaternion.Euler(0, 180, 0);
		} else {
			this.transform.rotation = Quaternion.Euler(0, 0, 0);
		}
		this.transform.position = Vector3.MoveTowards (this.transform.position, MovingObjTransFromIntoWorldPos(newPos), step);
	}

	protected void setMoveAnimate(){
		if (this.tag != "PlayerOwn") {
			animator.SetTrigger ("run");
		}
	}

	protected void stopMoveAnimate(){
		if (this.tag != "PlayerOwn") {
			animator.ResetTrigger ("run");
			animator.SetTrigger ("idle_1");

		}
	}


	protected void setAttackRange(int x, int y){
		Vector3 newMovePos;
		AttackRange.Clear ();

		//AttackRange.Add(new Vector3 (x, y, -0f));

		for (int i = 0; i <= AttackDisPoint; i++) {
			if (i == 0) {
				for (int i2 = -AttackDisPoint; i2 <= AttackDisPoint; i2++) {
					if (x + i2 < BoardManager.instance.columns && y + i < BoardManager.instance.rows && x +i2 >=0) {
						if (BoardManager.instance.floorAttackAbleArray [y + i,x + i2] != 0) {
							newMovePos = new Vector3 (x + i2, y + i, 0f);
							AttackRange.Add (newMovePos);
						}
					}

				}
			}else {
				for (int i2 = -AttackDisPoint+i; i2 <= AttackDisPoint-i; i2++) {
					if (x + i2 < BoardManager.instance.columns && y + i < BoardManager.instance.rows && x+i2 >=0 && y+i >=0) {

						if (BoardManager.instance.floorAttackAbleArray [y + i,x + i2] != 0) {
							newMovePos = new Vector3 (x + i2, y + i, -0f);
							AttackRange.Add (newMovePos);
						}
					}

					if (x + i2 < BoardManager.instance.columns && y - i > -1 && x + i2 >=0 ) {
						if (BoardManager.instance.floorAttackAbleArray [y - i,x + i2] != 0) {
							newMovePos = new Vector3 (x + i2, y - i, -0f);
							AttackRange.Add (newMovePos);
						}
					}

				}
			}
		}
	}

	public void normalAttack(GameObject attacker, GameObject defender){
		
		setAttackAnimate (attacker);

		int damage = 0;
		if (attacker.tag == "Player") {
			damage = attacker.GetComponent<Player> ().attackPoint - defender.GetComponent<Enemy> ().defendPoint;
			if (damage < 0) {
				damage = 0;
			}

			defender.GetComponent<Enemy> ().hit (damage);
		
		} else {
			damage = attacker.GetComponent<Enemy> ().attackPoint - defender.GetComponent<Player> ().defendPoint;
			if (damage < 0) {
				damage = 0;
			}
			defender.GetComponent<Player> ().hit (damage);
		}
			
	}

	protected void setHitAnimate(GameObject defender){
		if (defender.tag != "PlayerOwn") {
			defender.GetComponent<Animator>().SetTrigger ("hit_1");
		}
	}

	protected void setAttackAnimate(GameObject attacker){
		if (attacker.tag != "PlayerOwn") {
			attacker.GetComponent<Animator>().SetTrigger ("skill_1");
		}
	}

	protected void hit(int damage){
		setHitAnimate (this.gameObject);
		this.hp -= damage;
	}

	private void deleteCharacter(){
		BoardManager.instance.floorMoveableArray[Convert.ToInt32 (ObjGridVec.y),Convert.ToInt32 (ObjGridVec.x)] = 1;
		int i = 0;
		for (; BoardManager.instance.fightOrderArray [i] != this.gameObject; i++);
		GameObject.FindGameObjectsWithTag ("orderInName")[i].GetComponent<Text>().color = Color.grey;
		Destroy (this.gameObject);

		if (this.gameObject.tag == "Enemy") {
			BoardManager.instance.allButtonEnabled ();
			if (BoardManager.instance.fightOrderArray [BoardManager.instance.CharacterOrderController].GetComponent<Player> ().alreadyMoved) {
				MoveButton.instance.disable ();
			}
			if (BoardManager.instance.fightOrderArray [BoardManager.instance.CharacterOrderController].GetComponent<Player> ().alreadyAttacked) {
				AttackButton.instance.disable ();
			}
		}
			
	}

	// Update is called once per frame
	protected void Update () {

		for (int i = bestPath.Count - 1; i >= 0; i--) {
			if (Vector3.Distance (this.transform.position, MovingObjTransFromIntoWorldPos(bestPath [i])) >= float.Epsilon && movingToNum == i) {
				//如果向近的地方移动就先增加order，远的地方就后改变
				if (this.transform.position.y > MovingObjTransFromIntoWorldPos (bestPath [i]).y ||(
					this.transform.position.y == MovingObjTransFromIntoWorldPos (bestPath [i]).y && this.transform.position.x < MovingObjTransFromIntoWorldPos (bestPath [i]).x
				)) {
					if (this.tag == "PlayerOwn") {
						this.GetComponent<SpriteRenderer> ().sortingOrder = BoardManager.instance.TilesInstance [Convert.ToInt32 (bestPath [i].x), Convert.ToInt32 ((bestPath [i].y))].GetComponent<SpriteRenderer> ().sortingOrder + 1;
					} else {
						this.GetComponent<UnityEngine.Rendering.SortingGroup>().sortingOrder = BoardManager.instance.TilesInstance [Convert.ToInt32 (bestPath [i].x), Convert.ToInt32 ((bestPath [i].y))].GetComponent<SpriteRenderer> ().sortingOrder + 1;
					}
				}

				move (bestPath [i]);
			}else if (Vector3.Distance (this.transform.position, MovingObjTransFromIntoWorldPos(bestPath [i])) <= float.Epsilon && movingToNum == i){
				movingToNum -= 1;
				if(this.tag == "PlayerOwn"){
					this.GetComponent<SpriteRenderer> ().sortingOrder = BoardManager.instance.TilesInstance[Convert.ToInt32(bestPath [i].x), Convert.ToInt32((bestPath [i].y))].GetComponent<SpriteRenderer> ().sortingOrder + 1;
				}else{
					this.GetComponent<UnityEngine.Rendering.SortingGroup>().sortingOrder = BoardManager.instance.TilesInstance[Convert.ToInt32(bestPath [i].x), Convert.ToInt32((bestPath [i].y))].GetComponent<SpriteRenderer> ().sortingOrder + 1;
				}

			}
		}

		if (movingToNum == -1) {
			movingToNum -= 1;
			stopMoveAnimate ();
		}


	}


}
