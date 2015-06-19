using UnityEngine;
using System.Collections;

public class Action1Script : MonoBehaviour {
	/*
	Power Summary: Move Player
	This power will be able to move the player one unit away from his current position to be his new position.
	*/
	public int moneyRequired = 0;
	
	public GameObject gameController; //this is the game controller variable. It is obtained from the PlayerTurnsManager
	public GameObject inputManager; //this is the input manager varibale. Obtained from the PlayerTurnManager
	private GameObject[] voters; //array which houses the voters. Obtained from the Game Controller
	private GameObject[] players; //array which houses the players. Obtained from the Game Controller
	
	private int currentPlayer; //this variable finds which player is currently using his turn.

	private Vector3 originalPosition; //this will save the original position that the player started at.
	private bool chosenPositionConfirmed = false; //final position has been chosen and confirmed.

	// Use this for initialization
	void Start () {
		gameController = GameObject.FindWithTag ("GameController");
		inputManager = GameObject.FindWithTag ("InputManager");
		
		if (gameController != null) {
			voters = gameController.GetComponent<GameController> ().voters;
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
	}
	
	// Update is called once per frame
	void Update () {
		//These below if statements check if the player has pressed a button down to move the character.
		//Right now, it makes sure that the player can only move up, down, left, right, forward, and backward from its original postion.
		//It also checks to make sure the player doesn't move outside the grid.
		//When we figure out the GUI system, we can change the inputs to then be button presses.
		Vector3 currentPos = players [currentPlayer].transform.position;
		if (inputManager.GetComponent<InputManagerScript> ().leftButtonDown) {
			if (currentPos.x - originalPosition.x < 1 && currentPos.y - originalPosition.y == 0 && currentPos.z - originalPosition.z == 0 && currentPos.x < (gameController.GetComponent<GameController>().gridSize - 1))
			players[currentPlayer].transform.position += new Vector3(1,0,0);
		}
		if (inputManager.GetComponent<InputManagerScript> ().rightButtonDown) {
			if (currentPos.x - originalPosition.x > -1 && currentPos.y - originalPosition.y == 0 && currentPos.z - originalPosition.z == 0 && currentPos.x > 0)
				players[currentPlayer].transform.position += new Vector3(-1,0,0);
		}
		if (inputManager.GetComponent<InputManagerScript> ().upButtonDown) {
			if (currentPos.x - originalPosition.x == 0 && currentPos.y - originalPosition.y == 0 && currentPos.z - originalPosition.z > -1 && currentPos.z > 0)
			players[currentPlayer].transform.position += new Vector3(0,0,-1);
		}
		if (inputManager.GetComponent<InputManagerScript> ().downButtonDown) {
			if (currentPos.x - originalPosition.x == 0 && currentPos.y - originalPosition.y == 0 && currentPos.z - originalPosition.z < 1 && currentPos.z < (gameController.GetComponent<GameController>().gridSize - 1))
			players[currentPlayer].transform.position += new Vector3(0,0,1);
		}
		if (inputManager.GetComponent<InputManagerScript> ().qButtonDown) {
			if (currentPos.x - originalPosition.x == 0 && currentPos.y - originalPosition.y < 1 && currentPos.z - originalPosition.z == 0 && currentPos.y < (gameController.GetComponent<GameController>().gridSize - 1))
			players[currentPlayer].transform.position += new Vector3(0,1,0);
		}
		if (inputManager.GetComponent<InputManagerScript> ().eButtonDown) {
			if (currentPos.x - originalPosition.x == 0 && currentPos.y - originalPosition.y > -1 && currentPos.z - originalPosition.z == 0 && currentPos.y > 0)
			players[currentPlayer].transform.position += new Vector3(0,-1,0);
		}

		//Right Now, if you press Space bar, it confirms the chosen position.
		//You can only confirm the position if it isn't the exact same position you started at, or you are not sharing a position that another player is in
		if (Input.GetKeyDown (KeyCode.Space)) {
			for (int i = 0; i < players.Length; i++) {
				if (i != currentPlayer && players[i].transform.position != players[currentPlayer].transform.position && currentPos != originalPosition) {
					chosenPositionConfirmed = true;
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
}
