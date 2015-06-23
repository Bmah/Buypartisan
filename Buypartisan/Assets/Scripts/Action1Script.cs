using UnityEngine;
using System.Collections;

public class Action1Script : MonoBehaviour {
	/*
	Power Summary: Move Player
	This power will be able to move the player one unit away from his current position to be his new position.
	*/
	public int moneyRequired = 10;
	public int currentCost = 0;
	public int costMultiplier = 2;

	private float xDis;
	private float yDis;
	private float zDis;
	private float distance;
	
	public GameObject gameController; //this is the game controller variable. It is obtained from the PlayerTurnsManager
	public GameObject inputManager; //this is the input manager varibale. Obtained from the PlayerTurnManager
	public GameObject uiController; //this is the UI controller variable. Obtained from the scene
//	private GameObject[] voters; //array which houses the voters. Obtained from the Game Controller
	private GameObject[] players; //array which houses the players. Obtained from the Game Controller
	
	private int currentPlayer; //this variable finds which player is currently using his turn.

	private Vector3 originalPosition; //this will save the original position that the player started at.
	private bool chosenPositionConfirmed = false; //final position has been chosen and confirmed.
	private Vector3 currentPos;

	// Use this for initialization
	void Start () {
		gameController = GameObject.FindWithTag ("GameController");
		inputManager = GameObject.FindWithTag ("InputManager");
		uiController = GameObject.Find ("UI Controller");

		uiController.GetComponent<UI_Script>().disableActionButtons();
		
		if (gameController != null) {
//			voters = gameController.GetComponent<GameController> ().voters;
			players = gameController.GetComponent<GameController> ().players;
		} else {
			Debug.Log ("Failed to obtain voters and players array from Game Controller");
		}
		
		currentPlayer = gameController.GetComponent<GameController> ().currentPlayerTurn;
		int actionCostMultiplier = this.transform.parent.GetComponent<PlayerTurnsManager> ().costMultiplier;
		if (players [currentPlayer].GetComponent<PlayerVariables> ().money < (moneyRequired + (moneyRequired * actionCostMultiplier))) {
			Debug.Log ("Current Player doesn't have enough money to make this action.");
			uiController.GetComponent<UI_Script>().toggleActionButtons();
			Destroy(gameObject);
		}

		originalPosition = players[currentPlayer].transform.position;
		this.transform.position = originalPosition;
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
			this.transform.position += new Vector3(1,0,0);
			UpdateCostAndDistance();
			Debug.Log ("Cost to move " + distance + " spaces: $" + currentCost + ".");
		}
		if (inputManager.GetComponent<InputManagerScript> ().leftButtonDown) {
			if (transform.position.x > 0)
			this.transform.position += new Vector3(-1,0,0);
			UpdateCostAndDistance();
			Debug.Log ("Cost to move " + distance + " spaces: $" + currentCost + ".");
		}
		if (inputManager.GetComponent<InputManagerScript> ().downButtonDown) {
			if (transform.position.z > 0)
			this.transform.position += new Vector3(0,0,-1);
			UpdateCostAndDistance();
			Debug.Log ("Cost to move " + distance + " spaces: $" + currentCost + ".");
		}
		if (inputManager.GetComponent<InputManagerScript> ().upButtonDown) {
			if (transform.position.z < (gameController.GetComponent<GameController>().gridSize - 1))
			this.transform.position += new Vector3(0,0,1);
			UpdateCostAndDistance();
			Debug.Log ("Cost to move " + distance + " spaces: $" + currentCost + ".");
		}
		if (inputManager.GetComponent<InputManagerScript> ().qButtonDown) {
			if (transform.position.y < (gameController.GetComponent<GameController>().gridSize - 1))
			this.transform.position += new Vector3(0,1,0);
			UpdateCostAndDistance();
			Debug.Log ("Cost to move " + distance + " spaces: $" + currentCost + ".");
		}
		if (inputManager.GetComponent<InputManagerScript> ().eButtonDown) {
			if (transform.position.y > 0)
			this.transform.position += new Vector3(0,-1,0);
			UpdateCostAndDistance();
			Debug.Log ("Cost to move " + distance + " spaces: $" + currentCost + ".");
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

	//The below calculates how many squares away from the original position the new position is.
	//The cost increases the more the player has moved away.
	void UpdateCostAndDistance () {
		xDis = Mathf.Abs (transform.position.x - currentPos.x);
		yDis = Mathf.Abs (transform.position.y - currentPos.y);
		zDis = Mathf.Abs (transform.position.z - currentPos.z);
		distance = xDis + yDis + zDis;
		
		currentCost = moneyRequired * Mathf.RoundToInt (Mathf.Pow (costMultiplier, (distance - 1)));
	}

	void EndAction() {
		uiController.GetComponent<UI_Script>().toggleActionButtons();
		this.transform.parent.GetComponent<PlayerTurnsManager> ().costMultiplier += 1;
		Destroy(gameObject);
	}
}
