using UnityEngine;
using System.Collections;

public class Action4Script : MonoBehaviour {
	public int moneyRequired = 0;
	
	public GameObject gameController; //this is the game controller variable. It is obtained from the scene
	public GameObject inputManager; //this is the input manager varibale. Obtained from the scene
	public GameObject uiController; //this is the UI controller variable. Obtained from the scene
	private GameObject[] voters; //array which houses the voters. Obtained from the Game Controller
	private GameObject[] players; //array which houses the players. Obtained from the Game Controller
	
	private int currentPlayer; //this variable finds which player is currently using his turn.

	public bool actionConfirmed; //this confirms the action done.

	//these are to check which buttons have been pressed.
	[System.NonSerialized]
	public bool xPlusButton = false;
	[System.NonSerialized]
	public bool xMinusButton = false;
	[System.NonSerialized]
	public bool yPlusButton = false;
	[System.NonSerialized]
	public bool yMinusButton = false;
	[System.NonSerialized]
	public bool zPlusButton = false;
	[System.NonSerialized]
	public bool zMinusButton = false;
	[System.NonSerialized]
	public bool confirmButton = false;

	public Vector3[] voterOriginalPositions; //this is an array of the saved original positions of the voters.

	// Use this for initialization
	void Start () {
		gameController = GameObject.FindWithTag ("GameController");
		inputManager = GameObject.FindWithTag ("InputManager");
		uiController = GameObject.Find ("UI Controller");

		uiController.GetComponent<UI_Script> ().activateAction4UI ();
		
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

		//obtain the original positions of the voters
		voterOriginalPositions = new Vector3[voters.Length];
		for (int i = 0; i < voters.Length; i++) {
			voterOriginalPositions[i] = voters[i].transform.position;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (xPlusButton) {
			for (int i = 0; i < voters.Length; i++) {
				if (voterOriginalPositions[i].x < (gameController.GetComponent<GameController>().gridSize - 1)) {
				voters[i].transform.position = voterOriginalPositions[i] + new Vector3(1,0,0);
				} else {
					voters[i].transform.position = voterOriginalPositions[i];
				}
			}
			xPlusButton = false;
		}
		if (xMinusButton) {
			for (int i = 0; i < voters.Length; i++) {
				if (voterOriginalPositions[i].x > 0) {
					voters[i].transform.position = voterOriginalPositions[i] + new Vector3(-1,0,0);
				} else {
					voters[i].transform.position = voterOriginalPositions[i];
				}
			}
			xMinusButton = false;
		}
		if (yPlusButton) {
			for (int i = 0; i < voters.Length; i++) {
				if (voterOriginalPositions[i].y < (gameController.GetComponent<GameController>().gridSize - 1)) {
					voters[i].transform.position = voterOriginalPositions[i] + new Vector3(0,1,0);
				} else {
					voters[i].transform.position = voterOriginalPositions[i];
				}
			}
			yPlusButton = false;
		}
		if (yMinusButton) {
			for (int i = 0; i < voters.Length; i++) {
				if (voterOriginalPositions[i].y > 0) {
					voters[i].transform.position = voterOriginalPositions[i] + new Vector3(0,-1,0);
				} else {
					voters[i].transform.position = voterOriginalPositions[i];
				}
			}
			yMinusButton = false;
		}
		if (zPlusButton) {
			for (int i = 0; i < voters.Length; i++) {
				if (voterOriginalPositions[i].z < (gameController.GetComponent<GameController>().gridSize - 1)) {
					voters[i].transform.position = voterOriginalPositions[i] + new Vector3(0,0,1);
				} else {
					voters[i].transform.position = voterOriginalPositions[i];
				}
			}
			zPlusButton = false;
		}
		if (zMinusButton) {
			for (int i = 0; i < voters.Length; i++) {
				if (voterOriginalPositions[i].z > 0){
					voters[i].transform.position = voterOriginalPositions[i] + new Vector3(0,0,-1);
				} else {
					voters[i].transform.position = voterOriginalPositions[i];
				}
			}
			zMinusButton = false;
		}

		if (confirmButton) {
			actionConfirmed = true;
			confirmButton = false;
		}


		
		if (actionConfirmed)
		EndAction ();
	}
	
	void EndAction() {
		uiController.GetComponent<UI_Script>().toggleActionButtons();
		this.transform.parent.GetComponent<PlayerTurnsManager> ().costMultiplier += 1;
		Destroy(gameObject);
	}
}
