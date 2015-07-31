using UnityEngine;
using System.Collections;

public class Action4Script : MonoBehaviour {
	public string actionName = "Campaign Tour";
	public int baseCost = 400;
    public int totalCost = 0;
    public float costMultiplier = 1.0f;
	
	public GameObject gameController; //this is the game controller variable. It is obtained from the scene
	public GameObject inputManager; //this is the input manager varibale. Obtained from the scene
	public GameObject uiController; //this is the UI controller variable. Obtained from the scene
	private GameObject[] voters; //array which houses the voters. Obtained from the Game Controller
	private GameObject[] players; //array which houses the players. Obtained from the Game Controller
	//Brian Mah
	private RandomEventControllerScript eventController;
	
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
	private Vector3[] voterAdjustedPositions;

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

	private bool running = false;
	private Vector3 inputVec = Vector3.zero;

	//Allows the Action to play sounds (Brian Mah)
	private SFXController SFX;

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
			eventController = gameController.GetComponent<GameController> ().randomEventController;
		} else {
			Debug.Log ("Failed to obtain voters and players array from Game Controller");
		}

		//Disables the Action UI buttons
		uiController.GetComponent<UI_Script>().disableActionButtons();
		
		//obtain the original positions of the voters
		voterOriginalPositions = new Vector3[voters.Length];
		voterFinalPositions = new Vector3[voters.Length];
		voterAdjustedPositions = new Vector3[voters.Length];

		for (int i = 0; i < voters.Length; i++) {
			voterOriginalPositions[i] = voters[i].transform.position;
			voterFinalPositions[i] = new Vector3(1023f, 1234f, 4923f); //Used to check if a position has been placed or not.

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

		//Sets up SFX controller (Brian Mah)
		SFX = GameObject.FindGameObjectWithTag("SFX").GetComponent<SFXController>();
		if (SFX == null) {
			Debug.LogError("Could not find SFX controller");
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
			if (!running) {
				UpdatePositions2(1f,0f,0f);
				finalDirection = 1;
			}
			xPlusButton = false;
		}
		
		if (xMinusButton) {
			if (!running) {
				UpdatePositions2(-1f,0f,0f);
				finalDirection = 2;
			}
			xMinusButton = false;
		}
		
		if (yPlusButton) {
			if (!running) {
				UpdatePositions2 (0f, 1f, 0f);
				finalDirection = 3;
			}
			yPlusButton = false;
		}
		
		if (yMinusButton) {
			if (!running) {
				UpdatePositions2(0f,-1f,0f);
				finalDirection = 4;
			}
			yMinusButton = false;
		}
		
		if (zPlusButton) {
			if (!running) {
				UpdatePositions2(0f,0f,1f);
				finalDirection = 5;
			}
			zPlusButton = false;
		}
		
		if (zMinusButton) {
			if (!running) {
				UpdatePositions2(0f,0f,-1f);
				finalDirection = 6;
			}
			zMinusButton = false;
		}

		if (running)
			UpdatePositionsCont (inputVec);

		if (confirmButton) {
			if (finalDirection !=0 && !running) {
				actionConfirmed = true;
				EndActionInitial();
			}
			confirmButton = false;
		}

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

	///By Chris
	//New Update Positions function works in two parts. First there is the initialization function below:
	//This function sets running to true and first checks all the borders to see if any voters won't be moved off the grid.
	//Once running is true, the next function UpdatePositionsCont() begins to run in the Update() function.
	void UpdatePositions2(float x, float y, float z) {
		running = true;
		inputVec = new Vector3 (x, y, z);

		for (int p = 0; p < voters.Length; p++) {
			voterFinalPositions[p] = new Vector3(1023f, 1234f, 4923f);
		}

		for (int o = 0; o < voters.Length; o++) {
			if (x > 0 && y == 0 && z == 0) {
				if (voterOriginalPositions[o].x == (gameController.GetComponent<GameController>().gridSize - 1)) {
					voterFinalPositions[o] = voterOriginalPositions[o];
				}
			} else if (x < 0 && y == 0 && z == 0) {
				if (voterOriginalPositions[o].x == 0f) {
					voterFinalPositions[o] = voterOriginalPositions[o];
				}
			} else if (x == 0 && y > 0 && z == 0) {
				if (voterOriginalPositions[o].y == (gameController.GetComponent<GameController>().gridSize - 1)) {
					voterFinalPositions[o] = voterOriginalPositions[o];
				}
			} else if (x == 0 && y < 0 && z == 0) {
				if (voterOriginalPositions[o].y == 0f) {
					voterFinalPositions[o] = voterOriginalPositions[o];
				}
			} else if (x == 0 && y == 0 && z > 0) {
				if (voterOriginalPositions[o].z == (gameController.GetComponent<GameController>().gridSize - 1)) {
					voterFinalPositions[o] = voterOriginalPositions[o];
				}
			} else if (x == 0 && y == 0 && z < 0) {
				if (voterOriginalPositions[o].z == 0f){
					voterFinalPositions[o] = voterOriginalPositions[o];
				}
			} else {
				Debug.Log ("Action 4: Inputted vector is multidirectional!");
			}
		}
	}

	///By Chris Ng
	//This is part 2 of the new UpdatePositions function.
	//This function is designed to only run when the variable running is true in the Update function.
	//This is so that the positions are updated frame by frame, rather than all at once in a single frame.
	//This function first goes through the array checking if any voters will be moved onto a position that is occupied by another voter's final position.
	//Once it checks frame by frame that no more voters will overlap with one another, it sets the final voters final positions and ends the process by setting running to false.
	void UpdatePositionsCont (Vector3 inputedVector) {
		bool overlap = false;

		for (int i = 0; i < voters.Length; i++) {
			if (voterFinalPositions[i] == new Vector3(1023f, 1234f, 4923f)) {
				for (int j = 0; j < voters.Length; j++) {
					if (voterOriginalPositions[i] + inputedVector == voterFinalPositions[j]) {
						voterFinalPositions[i] = voterOriginalPositions[i];
						overlap = true;
					}
				}
			}
		}

		if (!overlap) {
			for (int k = 0; k < voters.Length; k++) {
				if (voterFinalPositions[k] == new Vector3(1023f, 1234f, 4923f)) {
					voterFinalPositions[k] = voterOriginalPositions[k] + inputedVector;
				}
				voters[k].transform.position = voterFinalPositions[k];
			}
			running = false;
		}
	}

	void EndActionInitial() {
		for (int p = 0; p < voters.Length; p++) {
			voterAdjustedPositions[p] = voterFinalPositions[p];
			voterFinalPositions[p] = new Vector3(1023f, 1234f, 4923f);
		}

		for (int o = 0; o < voters.Length; o++) {
			if (inputVec.x > 0 && inputVec.y == 0 && inputVec.z == 0) {
				if (voterOriginalPositions[o].x == (gameController.GetComponent<GameController>().gridSize - 1)) {
					voterFinalPositions[o] = voterOriginalPositions[o];
				}
			} else if (inputVec.x < 0 && inputVec.y == 0 && inputVec.z == 0) {
				if (voterOriginalPositions[o].x == 0f) {
					voterFinalPositions[o] = voterOriginalPositions[o];
				}
			} else if (inputVec.x == 0 && inputVec.y > 0 && inputVec.z == 0) {
				if (voterOriginalPositions[o].y == (gameController.GetComponent<GameController>().gridSize - 1)) {
					voterFinalPositions[o] = voterOriginalPositions[o];
				}
			} else if (inputVec.x == 0 && inputVec.y < 0 && inputVec.z == 0) {
				if (voterOriginalPositions[o].y == 0f) {
					voterFinalPositions[o] = voterOriginalPositions[o];
				}
			} else if (inputVec.x == 0 && inputVec.y == 0 && inputVec.z > 0) {
				if (voterOriginalPositions[o].z == (gameController.GetComponent<GameController>().gridSize - 1)) {
					voterFinalPositions[o] = voterOriginalPositions[o];
				}
			} else if (inputVec.x == 0 && inputVec.y == 0 && inputVec.z < 0) {
				if (voterOriginalPositions[o].z == 0f){
					voterFinalPositions[o] = voterOriginalPositions[o];
				}
			} else {
				Debug.Log ("Action 4: Inputted vector is multidirectional!");
			}
		}

		for (int i = 0; i < voters.Length; i++) 
		{
			//Checks to see if they have moved at all. e.g. next to edge (Chris Ng)
			if (voterFinalPositions[i] != new Vector3(1023f, 1234f, 4923f)) {
				//tests to see if the action is successful (Alex Jungroth)
				if(compareResistance(i))
				{
					//increases the base resistance of the voter by 1% (Alex Jungroth)
					voters[i].GetComponent<VoterVariables>().baseResistance += voters[i].GetComponent<VoterVariables>().baseResistance * 0.01f;
				}
				else
				{
					//resets the voters postions if they resist the action (Alex Jungroth)
					voterFinalPositions[i] = voterOriginalPositions[i];
					running = true;
					Debug.Log (i + " " + voterFinalPositions[i] + " " + voterOriginalPositions[i]);
				}
			}
		}

		if (!running) {
			for (int p = 0; p < voters.Length; p++) {
				voterFinalPositions[p] = voterAdjustedPositions[p];
			}
			Debug.Log ("all passed");
		}
	}

	void EndAction() {
		if (!running) {
			//alligns the voters to their action finishing postions (Alex Jungroth)
			setFinalPosition ();

			uiController.GetComponent<UI_Script> ().toggleActionButtons ();
			this.transform.parent.GetComponent<PlayerTurnsManager> ().IncreaseCostMultiplier ();

			if (string.Compare ((players [currentPlayer].GetComponent<PlayerVariables> ().politicalPartyName), "Windy") == 0)
				players [currentPlayer].GetComponent<PlayerVariables> ().money += totalCost / 4;

			//gives the Espresso party a refund based on their action cost modifier (Alex Jungroth)
			if (players [currentPlayer].GetComponent<PlayerVariables> ().politicalPartyName == "Espresso" && players [currentPlayer].GetComponent<PlayerVariables> ().actionCostModifier > 0) {
				players [currentPlayer].GetComponent<PlayerVariables> ().money += (int)Mathf.Ceil
				(totalCost * (1.0f + players [currentPlayer].GetComponent<PlayerVariables> ().actionCostModifier)); 
			}

			players [currentPlayer].GetComponent<PlayerVariables> ().money -= totalCost;
			//puts the current player and the event number into the action counter of the event controller
			//Brian Mah
			eventController.actionCounter [gameController.GetComponent<GameController> ().currentPlayerTurn] [4]++; // the second number should be the number of the action!

			//updates the tv so the users know whose turn it is (Alex Jungroth)
			uiController.GetComponent<UI_Script>().alterTextBox("It is the " + players[currentPlayer].GetComponent<PlayerVariables>().politicalPartyName +
				" party's turn.\n" + gameController.GetComponent<GameController>().displayPlayerStats());

			Destroy (gameObject);
		}
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
			if(Random.value > voters[i].GetComponent<VoterVariables>().xPlusResistance + (int)voters[i].GetComponent<VoterVariables>().baseResistance)
			{
				resistanceCheck = true;
			}
			break;
			
		case 2:
			if(Random.value > voters[i].GetComponent<VoterVariables>().xMinusResistance + (int)voters[i].GetComponent<VoterVariables>().baseResistance)
			{
				resistanceCheck = true;
			}
			break;
			
		case 3:
			if(Random.value > voters[i].GetComponent<VoterVariables>().yPlusResistance + (int)voters[i].GetComponent<VoterVariables>().baseResistance)
			{
				resistanceCheck = true;
			}
			break;
			
		case 4:
			if(Random.value > voters[i].GetComponent<VoterVariables>().yMinusResistance + (int)voters[i].GetComponent<VoterVariables>().baseResistance)
			{
				resistanceCheck = true;
			}
			break;
			
		case 5:
			if(Random.value > voters[i].GetComponent<VoterVariables>().zPlusResistance + (int)voters[i].GetComponent<VoterVariables>().baseResistance)
			{
				resistanceCheck = true;
			}
			break;
			
		case 6:
			if(Random.value > voters[i].GetComponent<VoterVariables>().zMinusResistance + (int)voters[i].GetComponent<VoterVariables>().baseResistance)
			{
				resistanceCheck = true;
			}
			break;
		}		
		
		//returns the result of the test
		return resistanceCheck;	
	}
}