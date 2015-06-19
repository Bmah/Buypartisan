using UnityEngine;
using System.Collections;

public class ActionScriptTemplate : MonoBehaviour {
	public int moneyRequired = 0;

	public GameObject gameController; //this is the game controller variable. It is obtained from the PlayerTurnsManager
	public GameObject inputManager; //this is the input manager varibale. Obtained from the PlayerTurnManager
	private GameObject[] voters; //array which houses the voters. Obtained from the Game Controller
	private GameObject[] players; //array which houses the players. Obtained from the Game Controller

	private int currentPlayer; //this variable finds which player is currently using his turn.

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
	}
	
	// Update is called once per frame
	void Update () {


		//This is where the action should be placed.
		//action action action. blah blah. E.g. move a voter or player one block over.
		//When action is finished, run this bit of code below to tell the Game Manager the turn is over, 
		//and deletes itself so the PlayerTurnsManager knows the turn is over as well.

		/*
		EndAction ();
		*/
	}

	void EndAction() {
		gameController.GetComponent<GameController> ().playerTakingAction = true;
		Destroy(gameObject);
	}
}
