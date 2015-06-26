// Chris-chan
// Power Summary: Move Player
// This power will be able to move the player one unit away from his current position to be his new position.

using UnityEngine;
using System.Collections;

public class Action1Script : MonoBehaviour {

    public string actionName = "Move Self";
	public int baseCost = 10;
	public int totalCost = 10;
	public float costMultiplier = 1.0f;
    public float stepCostMultiplier = 1.5f; // This increases baseCost by 50% for every extra step, multiplicatively

	private float xDis;
	private float yDis;
	private float zDis;
	private float distance;
	
	public GameObject gameController;
	public GameObject inputManager;
	public GameObject uiController; 
	private GameObject[] players; // Need this to search which player to move
	
	private int currentPlayer; // Tells you the source player who's using the action

	private Vector3 originalPosition; // Original position that the player started at.
	private bool chosenPositionConfirmed = false; // Destination been chosen and confirmed.
	private Vector3 currentPos;

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

	// Use this for initialization
	void Start () {
		gameController = GameObject.FindWithTag ("GameController");
		inputManager = GameObject.FindWithTag ("InputManager");
		uiController = GameObject.Find ("UI Controller");

		uiController.GetComponent<UI_Script>().disableActionButtons();
		uiController.GetComponent<UI_Script> ().activateAction1UI ();
		
		if (gameController != null) {
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
		if (xPlusButton) {
			if (transform.position.x < (gameController.GetComponent<GameController>().gridSize - 1))
			this.transform.position += new Vector3(1,0,0);
			UpdateCostAndDistance();
			Debug.Log ("Cost to move " + distance + " spaces: $" + totalCost + ".");
			xPlusButton = false;
		}
		if (xMinusButton) {
			if (transform.position.x > 0)
			this.transform.position += new Vector3(-1,0,0);
			UpdateCostAndDistance();
			Debug.Log ("Cost to move " + distance + " spaces: $" + totalCost + ".");
			xMinusButton = false;
		}
		if (zMinusButton) {
			if (transform.position.z > 0)
			this.transform.position += new Vector3(0,0,-1);
			UpdateCostAndDistance();
			Debug.Log ("Cost to move " + distance + " spaces: $" + totalCost + ".");
			zMinusButton = false;
		}
		if (zPlusButton) {
			if (transform.position.z < (gameController.GetComponent<GameController>().gridSize - 1))
			this.transform.position += new Vector3(0,0,1);
			UpdateCostAndDistance();
			Debug.Log ("Cost to move " + distance + " spaces: $" + totalCost + ".");
			zPlusButton = false;
		}
		if (yPlusButton) {
			if (transform.position.y < (gameController.GetComponent<GameController>().gridSize - 1))
			this.transform.position += new Vector3(0,1,0);
			UpdateCostAndDistance();
			Debug.Log ("Cost to move " + distance + " spaces: $" + totalCost + ".");
			yPlusButton = false;
		}
		if (yMinusButton) {
			if (transform.position.y > 0)
			this.transform.position += new Vector3(0,-1,0);
			UpdateCostAndDistance();
			Debug.Log ("Cost to move " + distance + " spaces: $" + totalCost + ".");
			yMinusButton = false;
		}

		//Right Now, if you press Space bar, it confirms the chosen position.
		//You can only confirm the position if it isn't the exact same position you started at, or you are not sharing a position that another player is in
		//You also must have enough money to move to that position.
		if (confirmButton) {
			for (int i = 0; i < players.Length; i++) {
				if (i != currentPlayer && players[i].transform.position != transform.position && transform.position != originalPosition) {
					if (totalCost > players[currentPlayer].GetComponent<PlayerVariables>().money) {
						Debug.Log ("You don't have enough money to move to this spot!");
					} else {
						chosenPositionConfirmed = true;
						players[currentPlayer].transform.position = transform.position;
					}
				}
			}
			confirmButton = false;
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
		
        // Example when you're using this as 3rd power: Total Cost = $50 * 2.0 * (1.5)^3
		totalCost = (int)((baseCost * costMultiplier) * Mathf.RoundToInt (Mathf.Pow (stepCostMultiplier, (distance - 1))));
	}

	void EndAction() {
		uiController.GetComponent<UI_Script>().toggleActionButtons();
		this.transform.parent.GetComponent<PlayerTurnsManager> ().IncreaseCostMultiplier();
        players[currentPlayer].GetComponent<PlayerVariables>().money -= totalCost; // Money is subtracted
		Destroy(gameObject);
	}
}
