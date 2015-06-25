using UnityEngine;
using System.Collections;

public class ActionScriptTemplate : MonoBehaviour {
	public int moneyRequired = 0;

	public GameObject gameController; //this is the game controller variable. It is obtained from the scene
	public GameObject inputManager; //this is the input manager varibale. Obtained from the scene
	public GameObject uiController; //this is the UI controller variable. Obtained from the scene
	private GameObject[] voters; //array which houses the voters. Obtained from the Game Controller
	private GameObject[] players; //array which houses the players. Obtained from the Game Controller

	private int currentPlayer; //this variable finds which player is currently using his turn.

	// Use this for initialization
	void Start () {
		gameController = GameObject.FindWithTag ("GameController");
		inputManager = GameObject.FindWithTag ("InputManager");
		uiController = GameObject.Find ("UI Controller");

		//Obtains the voter and player array from the gameController
		if (gameController != null) {
			voters = gameController.GetComponent<GameController> ().voters;
			players = gameController.GetComponent<GameController> ().players;
		} else {
			Debug.Log ("Failed to obtain voters and players array from Game Controller");
		}

		//Get's whose turn it is from the gameController. Then checks if he has enough money to perform the action
		currentPlayer = gameController.GetComponent<GameController> ().currentPlayerTurn;
		int actionCostMultiplier = this.transform.parent.GetComponent<PlayerTurnsManager> ().costMultiplier;
		if (players [currentPlayer].GetComponent<PlayerVariables> ().money < (moneyRequired + (moneyRequired * actionCostMultiplier))) {
			Debug.Log ("Current Player doesn't have enough money to make this action.");
			uiController.GetComponent<UI_Script>().toggleActionButtons();
			Destroy(gameObject);
		}

		//Disables the Action UI buttons
		uiController.GetComponent<UI_Script>().disableActionButtons();
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
		uiController.GetComponent<UI_Script>().toggleActionButtons();
		this.transform.parent.GetComponent<PlayerTurnsManager> ().costMultiplier += 1;
		players [currentPlayer].GetComponent<PlayerVariables> ().money -= moneyRequired + (moneyRequired * this.transform.parent.GetComponent<PlayerTurnsManager> ().costMultiplier);
		Destroy(gameObject);
	}
}
