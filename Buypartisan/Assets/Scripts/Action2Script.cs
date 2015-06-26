using UnityEngine;
using System.Collections;

public class Action2Script : MonoBehaviour {
    public string actionName = "Move Voter";
	public int baseCost = 15;
    public int totalCost = 0;
    public float costMultiplier = 1.0f;

	public GameObject gameController; 
	public GameObject inputManager; 
	public GameObject uiController; 
	private GameObject[] voters;
	private GameObject[] players; 
	
	private int currentPlayer; //this variable finds which player is currently using his turn.

	private int selectedVoter = 0; //this will keep track of which voter is selected.
	public bool voterSelected = false; //this confirms if voter has been selected.
	private bool positionSelected = false; //this confirms if the correct position has been selected.
	private Vector3 originalPosition; //this saves the original position of the voter.

	[System.NonSerialized]
	public bool leftButton = false; //checks if left button has been pressed
	[System.NonSerialized]
	public bool rightButton = false; //checks if right button has been pressed
	[System.NonSerialized]
	public bool confirmButton = false; //checks if confirm button has been pressed
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

	// Use this for initialization
	void Start () {
		gameController = GameObject.FindWithTag ("GameController");
		inputManager = GameObject.FindWithTag ("InputManager");
		uiController = GameObject.Find ("UI Controller");

		uiController.GetComponent<UI_Script> ().disableActionButtons ();
		uiController.GetComponent<UI_Script>().activateAction2UI1();
		
		if (gameController != null) {
			voters = gameController.GetComponent<GameController> ().voters;
			players = gameController.GetComponent<GameController> ().players;
		} else {
			Debug.Log ("Failed to obtain voters and players array from Game Controller");
		}
		
		currentPlayer = gameController.GetComponent<GameController> ().currentPlayerTurn;
		costMultiplier = this.transform.parent.GetComponent<PlayerTurnsManager> ().costMultiplier;
		if (players [currentPlayer].GetComponent<PlayerVariables> ().money < (baseCost * costMultiplier)) {
			Debug.Log ("Current Player doesn't have enough money to make this action.");
			uiController.GetComponent<UI_Script>().toggleActionButtons();
			Destroy(gameObject);
		}
        else
        {
            totalCost = (int)(baseCost * costMultiplier);
        }

		this.transform.position = voters [selectedVoter].transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (!voterSelected) {
			if (leftButton) {
				if (selectedVoter == 0) {
					selectedVoter = voters.Length - 1;
				} else {
					selectedVoter -= 1;
				}
				this.transform.position = voters [selectedVoter].transform.position;

				leftButton = false;
			}
			if (rightButton) {
				if (selectedVoter == (voters.Length - 1)) {
					selectedVoter = 0;
				} else {
					selectedVoter += 1;
				}
				this.transform.position = voters [selectedVoter].transform.position;

				rightButton = false;
			}
			if (confirmButton) {
				voterSelected = true;
				this.GetComponent<MeshRenderer>().enabled = true;
				originalPosition = voters[selectedVoter].transform.position;
				confirmButton = false;
				uiController.GetComponent<UI_Script>().activateAction2UI2();
			}
		} else {
			//This is the state in which the player has now chosen which voter to act upon.
			//Here, the player can move the voter one space away from its current spot, but can't move off the grid.
			if (xPlusButton) {
				if ((originalPosition.x + 1) < (gameController.GetComponent<GameController>().gridSize)) {
					this.transform.position = originalPosition + new Vector3(1,0,0);
				} else {
					this.transform.position = originalPosition;
				}

				xPlusButton = false;
			}
			if (xMinusButton) {
				if ((originalPosition.x - 1) > -1) {
					this.transform.position = originalPosition + new Vector3(-1,0,0);
				} else {
					this.transform.position = originalPosition;
				}

				xMinusButton = false;
			}
			if (zMinusButton) {
				if ((originalPosition.z - 1) > -1) {
					this.transform.position = originalPosition + new Vector3(0,0,-1);
				} else {
					this.transform.position = originalPosition;
				}

				zMinusButton = false;
			}
			if (zPlusButton) {
				if ((originalPosition.z + 1) < (gameController.GetComponent<GameController>().gridSize)) {
					this.transform.position = originalPosition + new Vector3(0,0,1);
				} else {
					this.transform.position = originalPosition;
				}

				zPlusButton = false;
			}
			if (yPlusButton) {
				if ((originalPosition.y + 1) < (gameController.GetComponent<GameController>().gridSize)) {
					this.transform.position = originalPosition + new Vector3(0,1,0);
				} else {
					this.transform.position = originalPosition;
				}

				yPlusButton = false;
			}
			if (yMinusButton) {
				if ((originalPosition.y - 1) > -1) {
					this.transform.position = originalPosition + new Vector3(0,-1,0);
				} else {
					this.transform.position = originalPosition;
				}

				yMinusButton = false;
			}

			//This is where the player confirms he has chosen the spot he wants to move the voter to.
			if (confirmButton){
				for (int i = 0; i < voters.Length; i++) {
					if (i != selectedVoter && voters[i].transform.position != this.transform.position && this.transform.position != originalPosition) {
						positionSelected = true;
						voters[selectedVoter].transform.position = this.transform.position;
					}
				}
				confirmButton = false;
			}
		}
		
		if (positionSelected) {
			EndAction ();
		}
	}
	
	void EndAction() {
		uiController.GetComponent<UI_Script>().toggleActionButtons();
		this.transform.parent.GetComponent<PlayerTurnsManager> ().IncreaseCostMultiplier();
		players [currentPlayer].GetComponent<PlayerVariables> ().money -= totalCost; // Money is subtracted
		Destroy(gameObject);
	}
}
