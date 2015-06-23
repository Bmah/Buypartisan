//Alex Jungroth
//This controls shadow position placement
using UnityEngine;
using System.Collections;

public class Action3Script : MonoBehaviour {

	//This is from ActionScriptTemplate.cs and Action1Script.cs
	public int moneyRequired = 0;
	public int currentCost = 0;

	public GameObject gameController; //this is the game controller variable. It is obtained from the PlayerTurnsManager
	public GameObject inputManager; //this is the input manager varibale. Obtained from the PlayerTurnManager
	private GameObject[] players; //array which houses the players. Obtained from the Game Controller

	private int currentPlayer; //this variable finds which player is currently using his turn.

	private float distance;

	private Vector3 originalPosition; //this will save the original position that the player started at.
	private bool chosenPositionConfirmed = false; //final position has been chosen and confirmed.
	private Vector3 currentPos;

	//This is from game controller

	//The template used to define and format a player object
	public GameObject playerTemplate;

	//holds the numbe of spawned players
	private int playersSpawned;


	private bool legalShadowPostion = true; //bool for checking if player is done

	//These are my variables

	//holds the shadow postion
	public GameObject shadowPosition;

	// Use this for initialization
	void Start () {
		gameController = GameObject.FindWithTag ("GameController");
		inputManager = GameObject.FindWithTag ("InputManager");
		
		if (gameController != null) {
			//			voters = gameController.GetComponent<GameController> ().voters;
			players = gameController.GetComponent<GameController> ().players;
		} else {
			Debug.Log ("Failed to obtain voters and players array from Game Controller");
		}
		
		currentPlayer = gameController.GetComponent<GameController> ().currentPlayerTurn;
		if (players [currentPlayer].GetComponent<PlayerVariables> ().money < moneyRequired) {
			Debug.Log ("Current Player doesn't have enough money to make this action.");
			Destroy(gameObject);
		}
		
		originalPosition = players[currentPlayer].transform.position;
		this.transform.position = originalPosition;

		//spawns in a shadow position for the player

		//instantiates a new instance of player that will be the shadow postion
		shadowPosition = Instantiate(playerTemplate,new Vector3(0,0,0), Quaternion.identity) as GameObject;
		
		//changes the shadow postion transform's postion to the player who spawned it
		shadowPosition.transform.position =  players[currentPlayer].transform.position;

		//gets the number of spawned players

	}
	
	// Update is called once per frame
	void Update () {
		//These below if statements check if the player has pressed a button down to move the character.
		//Right now, it makes sure that the player can only move up, down, left, right, forward, and backward from its original postion.
		//It also checks to make sure the player doesn't move outside the grid.
		//When we figure out the GUI system, we can change the inputs to then be button presses.
		currentPos = players [currentPlayer].transform.position;
		if (inputManager.GetComponent<InputManagerScript> ().rightButtonDown) {
			if (transform.position.x < (gameController.GetComponent<GameController>().gridSize - 1))
			{	
				if(TestShadowPosition())
				{
					this.transform.position += new Vector3(1,0,0);

					Debug.Log ("Cost to place a shadow position: $" + currentCost + ".");
				}
			}
		}
		if (inputManager.GetComponent<InputManagerScript> ().leftButtonDown) {
			if (transform.position.x > 0)
			{
				if(TestShadowPosition())
				{
					this.transform.position += new Vector3(-1,0,0);

					Debug.Log ("Cost to place a shadow position: $" + currentCost + ".");
				}
			}
		}
		if (inputManager.GetComponent<InputManagerScript> ().downButtonDown) {
			if (transform.position.z > 0)
			{	
				if(TestShadowPosition())
				{
					this.transform.position += new Vector3(0,0,-1);

					Debug.Log ("Cost to place a shadow position: $" + currentCost + ".");
				}
			}
		}
		if (inputManager.GetComponent<InputManagerScript> ().upButtonDown) {
			if (transform.position.z < (gameController.GetComponent<GameController>().gridSize - 1))
			{
				if(TestShadowPosition())
				{
					this.transform.position += new Vector3(0,0,1);

					Debug.Log ("Cost to place a shadow position: $" + currentCost + ".");
				}
			}
		}
		if (inputManager.GetComponent<InputManagerScript> ().qButtonDown) {
			if (transform.position.y < (gameController.GetComponent<GameController>().gridSize - 1))
			{	
				if(TestShadowPosition())
				{
					this.transform.position += new Vector3(0,1,0);

					Debug.Log ("Cost to place a shadow position: $" + currentCost + ".");
				}
			}
		}
		if (inputManager.GetComponent<InputManagerScript> ().eButtonDown) {
			if (transform.position.y > 0)
			{	
				if(TestShadowPosition())
				{
					this.transform.position += new Vector3(0,-1,0);

					Debug.Log ("Cost to place a shadow position: $" + currentCost + ".");
				}
			}
		}
		
		//Right Now, if you press Space bar, it confirms the chosen position.
		//You can only confirm the position if it isn't the exact same position you started at, or you are not sharing a position that another player is in
		//You also must have enough money to move to that position.
		if (Input.GetKeyDown (KeyCode.B)) {
			for (int i = 0; i < players.Length; i++) {
				if (i != currentPlayer && players[i].transform.position != transform.position && transform.position != originalPosition) {
					if (currentCost > players[currentPlayer].GetComponent<PlayerVariables>().money) {
						Debug.Log ("You don't have enough money to move to this spot!");
					} else {
						chosenPositionConfirmed = true;
						players[currentPlayer].transform.position = transform.position;
						players[currentPlayer].GetComponent<PlayerVariables>().money = players[currentPlayer].GetComponent<PlayerVariables>().money - currentCost;
					}
				}
			}
		}
		
		if (chosenPositionConfirmed) {
			EndAction ();
		}

	}
	
	void EndAction() {
		gameController.GetComponent<GameController> ().playerTakingAction = true;
		Destroy(gameObject);
	}

	bool TestShadowPosition(){

		//resets playerConifrmsPlacement
		legalShadowPostion = true;

		//checks the player against all of the previous players to ensure no duplicates
		for(int i = 0; i < playersSpawned; i++){
			if (shadowPosition.transform.position == players[i].transform.position){//if they are on the same spot
					legalShadowPostion = false;
			}
		}

		//if the player placment is legal
		if (legalShadowPostion) 
		{
			return(true);
		}
		else
		{
			return(false);
		}

	}//SpawnPlayer
	
}