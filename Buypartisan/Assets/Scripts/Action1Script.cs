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
		Vector3 currentPos = players [currentPlayer].transform.position;
		if (inputManager.GetComponent<InputManagerScript> ().leftButtonDown) {
			if (currentPos.x < 1 && currentPos.y == 0 && currentPos.z == 0)
			players[currentPlayer].transform.position += new Vector3(1,0,0);
		}
		if (inputManager.GetComponent<InputManagerScript> ().upButtonDown) {
			if (currentPos.x == 0 && currentPos.y == 0 && currentPos.z > -1)
			players[currentPlayer].transform.position += new Vector3(0,0,-1);
		}
		if (inputManager.GetComponent<InputManagerScript> ().downButtonDown) {
			if (currentPos.x == 0 && currentPos.y == 0 && currentPos.z < 1)
			players[currentPlayer].transform.position += new Vector3(0,0,1);
		}
		if (inputManager.GetComponent<InputManagerScript> ().rightButtonDown) {
			if (currentPos.x > -1 && currentPos.y == 0 && currentPos.z == 0)
			players[currentPlayer].transform.position += new Vector3(-1,0,0);
		}
		if (inputManager.GetComponent<InputManagerScript> ().qButtonDown) {
			if (currentPos.x == 0 && currentPos.y < 1 && currentPos.z == 0)
			players[currentPlayer].transform.position += new Vector3(0,1,0);
		}
		if (inputManager.GetComponent<InputManagerScript> ().eButtonDown) {
			if (currentPos.x == 0 && currentPos.y > -1 && currentPos.z == 0)
			players[currentPlayer].transform.position += new Vector3(0,-1,0);
		}


	/*	if (Input.GetKeyDown (KeyCode.Space)) {
			int counter = 0;
			for (int i = 0; i < players.Length; i++) {
				if (players[i].transform.position == currentPos) {
					counter ++;
				}
				    chosenPositionConfirmed = true;
			}
		}*/

		if (Input.GetKeyDown (KeyCode.Space)) {
			chosenPositionConfirmed = true;
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
