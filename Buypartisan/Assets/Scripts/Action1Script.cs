using UnityEngine;
using System.Collections;

public class Action1Script : MonoBehaviour {
	public GameObject gameController; //this is the game controller variable. It is obtained from the PlayerTurnsManager
	public GameObject inputManager; //this is the input manager varibale. Obtained from the PlayerTurnManager
	private GameObject[] voters; //array which houses the voters. Obtained from the Game Controller
	private GameObject[] players; //array which houses the players. Obtained from the Game Controller

	private int currentPlayer;

	// Use this for initialization
	void Start () {
		if (this.transform.parent.GetComponent<PlayerTurnsManager> ().gameController != null) {
			gameController = this.transform.parent.GetComponent<PlayerTurnsManager> ().gameController;
		} else {
			Debug.Log ("Set the PlayerTurnsManager variable game controller to the Game Controller in the scene!");
		}
		
		if (this.transform.parent.GetComponent<PlayerTurnsManager> ().inputManager != null) {
			inputManager = this.transform.parent.GetComponent<PlayerTurnsManager> ().inputManager;
		} else {
			Debug.Log ("Set the PlayerTurnsManager variable inputManager to the input manager in the scene!");
		}
		
		if (gameController != null) {
			voters = gameController.GetComponent<GameController> ().voters;
			players = gameController.GetComponent<GameController> ().players;
		} else {
			Debug.Log ("Failed to obtain voters and players array from Game Controller");
		}

		currentPlayer = gameController.GetComponent<GameController> ().currentPlayerTurn;
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