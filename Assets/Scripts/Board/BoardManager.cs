using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour {

	public static BoardManager instance = null;

	public int columns ; 										//Number of columns in our game board.
	public int rows ;											//Number of rows in our game board.

	protected Transform boardHolder;
	protected Transform playersHolder;
	protected Transform enemiesHolder;
	protected Transform orderNameHolder;
	public GameObject OrderNameText;
	public Text TurnIndicator;


	public Text EnemyName;
	public Text EnemyHP;
	public Text EnemyATK;
	public Text EnemyDEF;
	public Text EnemyDEX;
	public Enemy infoEnemy;
	public Player infoPlayer;

	public GameObject[,] TilesInstance;
	protected int[,] playersPos;
	protected int[,] enemiesPos;

	public GameObject[] Players;
	public GameObject[] Enemies;

	public int[,] floorMoveableArray;
	public int[,] floorAttackAbleArray;

	protected bool setCharactersFinished = false;
	public int[] fightPlayersIndex;
	public int[] fightEnemiesIndex;
	public GameObject[] fightOrderArray;
	private int FirstAttackIndex;
	private int FirstAttacPoi;
	private GameObject tmpCharacter;

	public int CharacterOrderController = 0;
	public bool MoveToNextCharacter = false;
	public bool allEnemiesdied = false;

	public GameObject EnemyInfo;

	public Vector3 TransFromWorldToISO(Vector3 vec){
		Vector3 newVec = new Vector3();

		newVec.x = vec.x + vec.y;
		newVec.y = (vec.y - vec.x) / 2;

		return newVec;
	}

	protected void setFloor(GameObject toInstantiate, int LayerOrder, int x, int y, int adjustPos, int floorNumber){


		//adjustPos == 2 表示变高一半
		//adjustPos == 3 表示变低一半
		//adjustPos == 4 表示变高1/4
		//adjustPos == 5 表示变低1/4
		//adjustPos == 6 表示变高1/8
		//adjustPos == 7 表示变低1/8
		//adjustPos == 8 表示变高一倍

		toInstantiate.GetComponent<SpriteRenderer> ().sortingOrder = LayerOrder;
		Vector3 ISOVec = new Vector3 ();



		ISOVec = TransFromWorldToISO (new Vector3 (x, y, 0f));

		if (adjustPos == 2) {
			ISOVec.y += 1f;
		} else if (adjustPos == 3) {
			ISOVec.y -= 0.7f;
		} else if (adjustPos == 4) {
			ISOVec.y += 0.75f;
		} else if (adjustPos == 5) {
			ISOVec.y -= 0.55f;
		} else if (adjustPos == 6) {
			ISOVec.y += 0.4f;
		} else if (adjustPos == 7) {
			ISOVec.y -= 0.3f;
		} else if (adjustPos == 8){
			ISOVec.y += 1.5f;
		}else {
			//随机高或低，增加立体感 Random.value (0-1)
			ISOVec.y += 0.2f * Random.value;
		}

		//渲染不同层
		ISOVec.y += floorNumber *20f;


		GameObject instance =
			Instantiate (toInstantiate, ISOVec, Quaternion.identity) as GameObject;

		instance.transform.SetParent (boardHolder);

		TilesInstance [x, y] = instance;

	}

	//type = 0 player, type = 1 enemy
	protected void setCharacters(int[,] Pos, GameObject[] characters, int type){
		//GameObject[] character;
		Transform Holder;

		if (type == 0) {
			//character = Players ;
			Holder = playersHolder;
		} else {
			//character = Enemies ;
			Holder = enemiesHolder;

		}

		for (int i = 0; i < characters.Length; i++) {

			if (type == 0) {
				characters [i].GetComponent<Player> ().ObjGridVec = new Vector3(Pos[i,0],Pos [i, 1],0);
			} else {
				characters [i].GetComponent<Enemy> ().ObjGridVec = new Vector3(Pos[i,0],Pos [i, 1],0);
			}

			rendeCharacter (Pos [i, 0], Pos [i, 1], characters[i], Holder, type);
			floorMoveableArray [Pos [i, 1], Pos [i, 0]] = 0;
		}

		if (type == 1) {
			setCharactersFinished = true;
		}
	}

	protected void rendeCharacter(int x, int y, GameObject character, Transform Holder, int type){
		Vector3 newVec = new Vector3();
		newVec.x = x + y;
		if (type == 3) {
			newVec.y = TilesInstance [x, y].transform.position.y + 1.3f;
			character.GetComponent<SpriteRenderer> ().sortingOrder = TilesInstance [x,y].GetComponent<SpriteRenderer> ().sortingOrder + 1;
		} else {
			newVec.y = TilesInstance [x, y].transform.position.y +0.7f;
			character.GetComponent<UnityEngine.Rendering.SortingGroup>().sortingOrder = TilesInstance [x,y].GetComponent<SpriteRenderer> ().sortingOrder + 1;
		}


		GameObject instance =
			Instantiate (character, newVec, Quaternion.identity) as GameObject;

		instance.name = instance.name.Remove (instance.name.Length - 7, 7);
		instance.transform.SetParent (Holder);
	}
		


	// Use this for initialization
	protected void Awake () {
		
		if (instance == null) {
			instance = this;
		} 
		orderNameHolder = GameObject.Find ("OrderList").transform;

		TurnIndicator = GameObject.Find ("TurnIndicator").GetComponent<Text>();
		TurnIndicator.enabled = false;

		EnemyInfo = GameObject.Find ("EnemyInfo");
		EnemyName = GameObject.Find ("EnemyName").GetComponent<Text>();
		EnemyHP =  GameObject.Find ("EnemyHP").GetComponent<Text>();
		EnemyATK = GameObject.Find ("EnemyATK").GetComponent<Text>();
		EnemyDEF = GameObject.Find ("EnemyDEF").GetComponent<Text>();
		EnemyDEX = GameObject.Find ("EnemyDEX").GetComponent<Text>();
		EnemyInfo.SetActive (false);


	}

	protected void initOrderArray(){
		
		int length = fightPlayersIndex.Length + fightEnemiesIndex.Length;
		fightOrderArray = new GameObject[length];
		int i = 0;

		for (; i < fightPlayersIndex.Length; i++) {

			fightOrderArray [i] = GameManager.instance.players[i].gameObject;
		}
		for(int i2 = 0 ; i2<fightEnemiesIndex.Length;i2++){
			fightOrderArray[i] =  GameManager.instance.enemies[i2].gameObject;
			i++;
		}

		for(int i3 = 0; i3<fightOrderArray.Length;i3++){
			FirstAttacPoi = 0;
			FirstAttackIndex = i3;

			for(int i2 = i3; i2<fightOrderArray.Length ; i2++){
				if (fightOrderArray [i2].GetComponent<Enemy> () == null){
					
					if(fightOrderArray[i2].GetComponent<Player>().FirstAttackPoint >FirstAttacPoi){
						FirstAttacPoi = fightOrderArray[i2].GetComponent<Player>().FirstAttackPoint;
						FirstAttackIndex = i2;
					}
				}else{
					
					if(fightOrderArray[i2].GetComponent<Enemy>().FirstAttackPoint >FirstAttacPoi){
						FirstAttacPoi = fightOrderArray[i2].GetComponent<Enemy>().FirstAttackPoint;
						FirstAttackIndex = i2;
					}
				}
			}

			tmpCharacter = fightOrderArray [i3];
			fightOrderArray [i3] = fightOrderArray [FirstAttackIndex];
			fightOrderArray [FirstAttackIndex] = tmpCharacter;
		}

	}

	public void allButtonDisabled(){
		MoveButton.instance.disable ();
		RestButton.instance.disable ();
		AttackButton.instance.disable ();
	}

	public void allButtonEnabled(){
		MoveButton.instance.enable ();
		RestButton.instance.enable ();
		AttackButton.instance.enable ();
	}

	public void SetOrderNames(RectTransform _mRect, RectTransform _parent,float height)
	{
		_mRect.anchoredPosition = _parent.position;
		_mRect.anchorMin = new Vector2(1, 0);
		_mRect.anchorMax = new Vector2(0, 1);
		_mRect.pivot = new Vector2(0.5f, 0.5f);
		_mRect.sizeDelta =new Vector2(200f,50f) ;
		_mRect.position= new Vector3(100f,height);
		_mRect.transform.SetParent(_parent);
	}

	public void initOrderNames(){
		float height = 400;
		for (int i = 0; i < fightOrderArray.Length; i++) {

			height -= 50; 
			GameObject toInstantiate = OrderNameText;

			toInstantiate.name = fightOrderArray [i].name;

			toInstantiate.GetComponent<Text> ().text = fightOrderArray [i].name;



			GameObject instance =
				Instantiate (toInstantiate) as GameObject;

			if (i == 0) {
				instance.GetComponent<Text> ().color = Color.yellow;
			} 
		
			SetOrderNames (instance.GetComponent<RectTransform> (), orderNameHolder.GetComponent<RectTransform> (),height);

			//instance.transform.SetParent (orderNameHolder);
		}
	}

	public void checkAllenemieDied(){
		bool result = true;

		for (int i = 0; i < GameManager.instance.enemies.Count; i++) {
			if (GameManager.instance.enemies [i] != null) {
				result = false;
			}
		}
		allEnemiesdied = result;
	}

	protected void initBoard(){
		GameObject[] orderNames;
		GameManager.instance.players = new List<Player> ();
		GameManager.instance.enemies = new List<Enemy> ();
		CharacterOrderController = 0;
		orderNames = GameObject.FindGameObjectsWithTag ("orderInName");
		for (int i = 0; i < orderNames.Length; i++) {
			Destroy (orderNames [i]);
		}

	}

	public void hideTurnIndicator(){
		TurnIndicator.enabled = false;
	}

	public void showTurnIndicator(string role){
		TurnIndicator.text = role + "'s Turn !";
		TurnIndicator.enabled = true;
		Invoke ("hideTurnIndicator", 1f);
	}

	protected Enemy isEnemy(Vector3 Pos){
		Vector3 mousePos;
		mousePos = Camera.main.ScreenToWorldPoint (Pos);
		mousePos.z = 0f;

		for (int i = 0; i < GameManager.instance.enemies.Count; i++) {
			if (GameManager.instance.enemies [i] != null) {
				if (Mathf.Abs (GameManager.instance.enemies [i].transform.position.x - mousePos.x) < 0.3f && 
					mousePos.y - GameManager.instance.enemies [i].transform.position.y > 0 && 
					mousePos.y - GameManager.instance.enemies [i].transform.position.y < 1) {
					return GameManager.instance.enemies [i];
				}
			}
		}

		return new Enemy();
	}

	protected Player isPlayer(Vector3 Pos){
		Vector3 mousePos;
		mousePos = Camera.main.ScreenToWorldPoint (Pos);
		mousePos.z = 0f;

		for (int i = 0; i < GameManager.instance.players.Count; i++) {
			if (GameManager.instance.players [i] != null) {
				if (Mathf.Abs (GameManager.instance.players [i].transform.position.x - mousePos.x) < 0.3f &&
				   mousePos.y - GameManager.instance.players [i].transform.position.y > 0 &&
				   mousePos.y - GameManager.instance.players [i].transform.position.y < 1) {
					return GameManager.instance.players [i];
				}
			}
		}

		return new Player();
	}

	// Update is called once per frame
	protected void Update () {

		infoEnemy = isEnemy (Input.mousePosition);

		if (infoEnemy != null) {
			EnemyName.text = infoEnemy.name;
			EnemyATK.text = "ATK: " +infoEnemy.attackPoint.ToString();
			EnemyDEF.text = "DEF: " + infoEnemy.defendPoint.ToString();
			EnemyHP.text = "HP: " + infoEnemy.hp.ToString();
			EnemyDEX.text = "DEX: " + infoEnemy.Dexterity.ToString();
			EnemyInfo.SetActive (true);
		} else if( infoPlayer ==null){
			EnemyInfo.SetActive (false);
		};

		infoPlayer = isPlayer (Input.mousePosition);

		if (infoPlayer != null) {
			EnemyName.text = infoPlayer.name;
			EnemyATK.text = "ATK: " +infoPlayer.attackPoint.ToString();
			EnemyDEF.text = "DEF: " + infoPlayer.defendPoint.ToString();
			EnemyHP.text = "HP: " + infoPlayer.hp.ToString();
			EnemyDEX.text = "DEX: " + infoPlayer.Dexterity.ToString();
			EnemyInfo.SetActive (true);
		} else if(infoEnemy == null){
			EnemyInfo.SetActive (false);
		};
		
		if (MoveToNextCharacter) {
			MoveToNextCharacter = false;

			GameObject.FindGameObjectsWithTag ("orderInName")[CharacterOrderController].GetComponent<Text>().color = Color.white;
			for (; CharacterOrderController + 1 < fightOrderArray.Length && fightOrderArray [CharacterOrderController + 1] == null; CharacterOrderController++)
				;
			if (CharacterOrderController + 1 < fightOrderArray.Length) {
				CharacterOrderController += 1; 
			} else {
				CharacterOrderController = 0;
				if (fightOrderArray [0] == null) {
					for (; CharacterOrderController + 1 < fightOrderArray.Length && fightOrderArray [CharacterOrderController + 1] == null; CharacterOrderController++)
						;
					CharacterOrderController += 1;
				}
			}

			GameObject.FindGameObjectsWithTag ("orderInName")[CharacterOrderController].GetComponent<Text>().color = Color.yellow;

			if (fightOrderArray [CharacterOrderController].tag == "Player") {
				showTurnIndicator ("Player");

				fightOrderArray [CharacterOrderController].GetComponent<Player> ().OwnTurn = true;
				fightOrderArray [CharacterOrderController].GetComponent<Player> ().alreadyMoved = false;
				fightOrderArray [CharacterOrderController].GetComponent<Player> ().alreadyAttacked = false;
				allButtonEnabled ();

			} else {
				//showTurnIndicator ("Enemy");
				fightOrderArray [CharacterOrderController].GetComponent<Enemy> ().run ();
			}
		}



	}
}
