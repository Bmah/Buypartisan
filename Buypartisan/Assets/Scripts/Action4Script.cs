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

	private Vector3[] voterOriginalPositions; //this is an array of the saved original positions of the voters.
	private Vector3[] voterFinalPositions;	//this will be where the final positions of the voters will be saved.

	private bool noOverlap = true; //this boolean will be used to check if theres any overlaps with other voters
	private int overlap1; //these two ints save which ones are overlapping.
	private int overlap2;

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
		voterFinalPositions = new Vector3[voters.Length];
		for (int i = 0; i < voters.Length; i++) {
			voterOriginalPositions[i] = voters[i].transform.position;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (xPlusButton) {
			for (int i = 0; i < voters.Length; i++) {
				if (voterOriginalPositions[i].x < (gameController.GetComponent<GameController>().gridSize - 1)) {
					voterFinalPositions[i] = voterOriginalPositions[i] + new Vector3(1,0,0);
				} else {
					voterFinalPositions[i] = voterOriginalPositions[i];
				}
			}
			OverlapCheck();
			setFinalPosition();
			xPlusButton = false;
		}
		if (xMinusButton) {
			for (int i = 0; i < voters.Length; i++) {
				if (voterOriginalPositions[i].x > 0) {
					voterFinalPositions[i] = voterOriginalPositions[i] + new Vector3(-1,0,0);
				} else {
					voterFinalPositions[i] = voterOriginalPositions[i];
				}
			}
			OverlapCheck();
			setFinalPosition();
			xMinusButton = false;
		}
		if (yPlusButton) {
			for (int i = 0; i < voters.Length; i++) {
				if (voterOriginalPositions[i].y < (gameController.GetComponent<GameController>().gridSize - 1)) {
					voterFinalPositions[i] = voterOriginalPositions[i] + new Vector3(0,1,0);
				} else {
					voterFinalPositions[i] = voterOriginalPositions[i];
				}
			}
			OverlapCheck();
			setFinalPosition();
			yPlusButton = false;
		}
		if (yMinusButton) {
			for (int i = 0; i < voters.Length; i++) {
				if (voterOriginalPositions[i].y > 0) {
					voterFinalPositions[i] = voterOriginalPositions[i] + new Vector3(0,-1,0);
				} else {
					voterFinalPositions[i] = voterOriginalPositions[i];
				}
			}
			OverlapCheck();
			setFinalPosition();
			yMinusButton = false;
		}
		if (zPlusButton) {
			for (int i = 0; i < voters.Length; i++) {
				if (voterOriginalPositions[i].z < (gameController.GetComponent<GameController>().gridSize - 1)) {
					voterFinalPositions[i] = voterOriginalPositions[i] + new Vector3(0,0,1);
				} else {
					voterFinalPositions[i] = voterOriginalPositions[i];
				}
			}
			OverlapCheck();
			setFinalPosition();
			zPlusButton = false;
		}
		if (zMinusButton) {
			for (int i = 0; i < voters.Length; i++) {
				if (voterOriginalPositions[i].z > 0){
					voterFinalPositions[i] = voterOriginalPositions[i] + new Vector3(0,0,-1);
				} else {
					voterFinalPositions[i] = voterOriginalPositions[i];
				}
			}
			OverlapCheck();
			setFinalPosition();
			zMinusButton = false;
		}

		if (confirmButton) {
			actionConfirmed = true;
			confirmButton = false;
		}

		if (Input.GetKeyDown (KeyCode.B))
			OverlapCheck ();
	

		if (actionConfirmed)
		EndAction ();
	}

	void OverlapCheck() {
		if (noOverlap) {
			for (int i = 0; i < voters.Length; i++) {
				if (noOverlap) {
					for (int j = 0; j < voters.Length; j++) {
						if (i == j) {
							//do nothing
						} else {
							if (voterFinalPositions [i] == voterFinalPositions [j]) {
								noOverlap = false;
								overlap1 = i;
								overlap2 = j;
								break;
							}
						}
					}
				} else {
					break;
				}
			}
		}

		if (!noOverlap) {
			if (voterFinalPositions[overlap1] != voterOriginalPositions[overlap1]) {
				voterFinalPositions[overlap1] = voterOriginalPositions[overlap1];
			}

			if (voterFinalPositions[overlap2] != voterOriginalPositions[overlap2]) {
				voterFinalPositions[overlap2] = voterOriginalPositions[overlap2];
			}
			noOverlap = true;
			OverlapCheck();
		}
	}

	void setFinalPosition() {
		for (int i = 0; i < voters.Length; i++) {
			voters[i].transform.position = voterFinalPositions[i];
		}
	}

	void EndAction() {
		uiController.GetComponent<UI_Script>().toggleActionButtons();
		this.transform.parent.GetComponent<PlayerTurnsManager> ().costMultiplier += 1;
		Destroy(gameObject);
	}
}
