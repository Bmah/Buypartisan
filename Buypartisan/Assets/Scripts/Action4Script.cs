using UnityEngine;
using System.Collections;

public class Action4Script : MonoBehaviour {
	public string actionName = "Campaign Tour";
	public int baseCost = 10;
    public int totalCost = 0;
    public float costMultiplier = 1.0f;
	
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
	[System.NonSerialized]
	public bool cancelButton = false;

	private Vector3[] voterOriginalPositions; //this is an array of the saved original positions of the voters.
	private Vector3[] voterFinalPositions;	//this will be where the final positions of the voters will be saved.

	private bool noOverlap = true; //this boolean will be used to check if theres any overlaps with other voters
	private int overlap1; //these two ints save which ones are overlapping.
	private int overlap2;

	//holds the direction that the voter is being moved in (Alex Jungroth)
	private int finalDirection = 0;

	//Final Direction Key (Alex Jungroth)
	//0 = Unset
	//1 = X+
	//2 = X-
	//3 = Y+
	//4 = Y-
	//5 = Z+
	//6 = Z-

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

		//Disables the Action UI buttons
		uiController.GetComponent<UI_Script>().disableActionButtons();
		
		//obtain the original positions of the voters
		voterOriginalPositions = new Vector3[voters.Length];
		voterFinalPositions = new Vector3[voters.Length];

		for (int i = 0; i < voters.Length; i++) {
			voterOriginalPositions[i] = voters[i].transform.position;

			//see ActionScriptTemplate.cs for my explination on this change (Alex Jungroth)

			//Get's whose turn it is from the gameController. Then checks if he has enough money to perform the action
			currentPlayer = gameController.GetComponent<GameController> ().currentPlayerTurn;
			costMultiplier = this.transform.parent.GetComponent<PlayerTurnsManager> ().costMultiplier;
			if (players [currentPlayer].GetComponent<PlayerVariables> ().money >= (baseCost * costMultiplier)) {

				totalCost = (int)(baseCost * costMultiplier);
			}
        	else
        	{
				Debug.Log ("Current Player doesn't have enough money to make this action.");
				uiController.GetComponent<UI_Script>().toggleActionButtons();
				Destroy(gameObject);
        	}
			
		}
	}
	
	// Update is called once per frame
	void Update () {

		//ends the action if the cancel button is pressed (Alex Jungroth)
		if (cancelButton) 
		{
			//handles early canceling(Alex Jungroth)
			uiController.GetComponent<UI_Script>().toggleActionButtons();
			for (int i = 0; i < voters.Length; i++) 
			{
				voterFinalPositions[i] = voterOriginalPositions[i];
			}
			setFinalPosition();
			Destroy(gameObject);
		}
		
		if (xPlusButton) {
			for (int i = 0; i < voters.Length; i++) {
				if (voterOriginalPositions[i].x < (gameController.GetComponent<GameController>().gridSize - 1)) {
					voterFinalPositions[i] = voterOriginalPositions[i] + new Vector3(1,0,0);

					//This sets final direction to X+ (Alex Jungroth)
					finalDirection = 1;
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

					//This sets final direction to X- (Alex Jungroth)
					finalDirection = 2;
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

					//This sets final direction to Y+ (Alex Jungroth)
					finalDirection = 3;
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

					//This sets final direction to Y- (Alex Jungroth)
					finalDirection = 4;
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

					//This sets final direction to Z+ (Alex Jungroth)
					finalDirection = 5;
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

					//This sets final direction to Z- (Alex Jungroth)
					finalDirection = 6;
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
			if(compareResistance(i))
			{
				//tests to see if the action is successful (Alex Jungroth)
				voters[i].transform.position = voterFinalPositions[i];

				//increases the base resistance of the voter by 1% (Alex Jungroth)
				voters[i].GetComponent<VoterVariables>().baseResistance += voters[i].GetComponent<VoterVariables>().baseResistance * 0.01f;
			}
		}
	}

	void EndAction() {
		uiController.GetComponent<UI_Script>().toggleActionButtons();
		this.transform.parent.GetComponent<PlayerTurnsManager> ().IncreaseCostMultiplier();
		players [currentPlayer].GetComponent<PlayerVariables> ().money -= totalCost;
		Destroy(gameObject);
	}

	bool compareResistance(int i)
	{
		//holds wether or not the action overcame the voter's resitance
		bool resistanceCheck = false;
		
		//This case statment will handle the different directions that the voter can be moved in (Alex Jungroth)
		switch (finalDirection)
		{
		case 0:
			Debug.Log ("Case 0 was chosen which means something has gone wrong!");
			resistanceCheck = true;
			break;
			
		case 1:
			if(Random.Range(0.1f,1) > voters[i].GetComponent<VoterVariables>().xPlusResistance)
			{
				resistanceCheck = true;
			}
			break;
			
		case 2:
			if(Random.Range(0.1f,1) > voters[i].GetComponent<VoterVariables>().xMinusResistance)
			{
				resistanceCheck = true;
			}
			break;
			
		case 3:
			if(Random.Range(0.1f,1) > voters[i].GetComponent<VoterVariables>().yPlusResistance)
			{
				resistanceCheck = true;
			}
			break;
			
		case 4:
			if(Random.Range(0.1f,1) > voters[i].GetComponent<VoterVariables>().yMinusResistance)
			{
				resistanceCheck = true;
			}
			break;
			
		case 5:
			if(Random.Range(0.1f,1) > voters[i].GetComponent<VoterVariables>().zPlusResistance)
			{
				resistanceCheck = true;
			}
			break;
			
		case 6:
			if(Random.Range(0.1f,1) > voters[i].GetComponent<VoterVariables>().zMinusResistance)
			{
				resistanceCheck = true;
			}
			break;
		}		
		
		//returns the result of the test
		return resistanceCheck;	
	}
}