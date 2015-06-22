using UnityEngine;
using System.Collections;

public class Action2Script : MonoBehaviour {
	public int moneyRequired = 0;

	public GameObject gameController; //this is the game controller variable. It is obtained from the PlayerTurnsManager
	public GameObject inputManager; //this is the input manager varibale. Obtained from the PlayerTurnManager
	private GameObject[] voters; //array which houses the voters. Obtained from the Game Controller
	private GameObject[] players; //array which houses the players. Obtained from the Game Controller
	
	private int currentPlayer; //this variable finds which player is currently using his turn.

	private int selectedVoter = 0; //this will keep track of which voter is selected.
	private bool voterSelected = false; //this confirms if voter has been selected.
	private bool positionSelected = false; //this confirms if the correct position has been selected.
	private Vector3 originalPosition; //this saves the original position of the voter.

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

		this.transform.position = voters [selectedVoter].transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (!voterSelected) {
			if (inputManager.GetComponent<InputManagerScript> ().leftButtonDown) {
				if (selectedVoter == 0) {
					selectedVoter = voters.Length - 1;
				} else {
					selectedVoter -= 1;
				}
				this.transform.position = voters [selectedVoter].transform.position;
			}
			if (inputManager.GetComponent<InputManagerScript> ().rightButtonDown) {
				if (selectedVoter == (voters.Length - 1)) {
					selectedVoter = 0;
				} else {
					selectedVoter += 1;
				}
				this.transform.position = voters [selectedVoter].transform.position;
			}
			if (Input.GetKeyDown(KeyCode.B)) {
				voterSelected = true;
				this.GetComponent<MeshRenderer>().enabled = true;
				originalPosition = voters[selectedVoter].transform.position;
			}
		} else {
			//This is the state in which the player has now chosen which voter to act upon.
			//Here, the player can move the voter one space away from its current spot, but can't move off the grid.
			if (inputManager.GetComponent<InputManagerScript> ().leftButtonDown) {
				if ((originalPosition.x + 1) < (gameController.GetComponent<GameController>().gridSize)) {
					this.transform.position = originalPosition + new Vector3(1,0,0);
				} else {
					this.transform.position = originalPosition;
				}
			}
			if (inputManager.GetComponent<InputManagerScript> ().rightButtonDown) {
				if ((originalPosition.x - 1) > -1) {
					this.transform.position = originalPosition + new Vector3(-1,0,0);
				} else {
					this.transform.position = originalPosition;
				}
			}
			if (inputManager.GetComponent<InputManagerScript> ().upButtonDown) {
				if ((originalPosition.z - 1) > -1) {
					this.transform.position = originalPosition + new Vector3(0,0,-1);
				} else {
					this.transform.position = originalPosition;
				}
			}
			if (inputManager.GetComponent<InputManagerScript> ().downButtonDown) {
				if ((originalPosition.z + 1) < (gameController.GetComponent<GameController>().gridSize)) {
					this.transform.position = originalPosition + new Vector3(0,0,1);
				} else {
					this.transform.position = originalPosition;
				}
			}
			if (inputManager.GetComponent<InputManagerScript> ().qButtonDown) {
				if ((originalPosition.y + 1) < (gameController.GetComponent<GameController>().gridSize)) {
					this.transform.position = originalPosition + new Vector3(0,1,0);
				} else {
					this.transform.position = originalPosition;
				}
			}
			if (inputManager.GetComponent<InputManagerScript> ().eButtonDown) {
				if ((originalPosition.y - 1) > -1) {
					this.transform.position = originalPosition + new Vector3(0,-1,0);
				} else {
					this.transform.position = originalPosition;
				}
			}

			//This is where the player confirms he has chosen the spot he wants to move the voter to.
			if (Input.GetKeyDown (KeyCode.B)){
				for (int i = 0; i < voters.Length; i++) {
					if (i != selectedVoter && voters[i].transform.position != this.transform.position && this.transform.position != originalPosition) {
						positionSelected = true;
						voters[selectedVoter].transform.position = this.transform.position;
					}
				}
			}
		}
		
		if (positionSelected) {
			EndAction ();
		}
	}
	
	void EndAction() {
		gameController.GetComponent<GameController> ().playerTakingAction = true;
		Destroy(gameObject);
	}
}
