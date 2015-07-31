using UnityEngine;
using System.Collections;

public class Action2Script : MonoBehaviour {
    public string actionName = "Move Voter";
	public int baseCost = 200;
    public int totalCost = 0;
    public float costMultiplier = 1.0f;

	public GameObject gameController; 
	public GameObject inputManager; 
	public GameObject uiController;
    private GameObject visualAid;
	private GameObject[] voters;
	private GameObject[] players;
	//Brian Mah
	private RandomEventControllerScript eventController;
	
	private int currentPlayer; //this variable finds which player is currently using his turn.

	private int selectedVoter = 0; //this will keep track of which voter is selected.
	public bool voterSelected = false; //this confirms if voter has been selected.
	private bool positionSelected = false; //this confirms if the correct position has been selected.
	private Vector3 originalPosition;//this saves the original position of the voter.
	private bool foundVoter = false;

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
	[System.NonSerialized]
	public bool cancelButton = false;

	//holds the direction that the voter is being moved in (Alex Jungroth)
	private int finalDirection = 0;

	//Final Direction Key (Alex Jungroth)
	//0 = Unset
	//1 = X+
	//2 = X-
	//3 = Z-
	//4 = Z+
	//5 = Y+
	//6 = Y-

	//Allows the Action to play sounds (Brian Mah)
	private SFXController SFX;

	// Use this for initialization
	void Start () {
		gameController = GameObject.FindWithTag ("GameController");
		inputManager = GameObject.FindWithTag ("InputManager");
		uiController = GameObject.Find ("UI Controller");
        visualAid = GameObject.FindWithTag("VisualAidManager");

		uiController.GetComponent<UI_Script> ().disableActionButtons ();
		uiController.GetComponent<UI_Script>().activateAction2UI1();
		
		if (gameController != null) {
			voters = gameController.GetComponent<GameController> ().voters;
			players = gameController.GetComponent<GameController> ().players;
			eventController = gameController.GetComponent<GameController> ().randomEventController;
		} else {
			Debug.Log ("Failed to obtain voters and players array from Game Controller");
		}

		currentPlayer = gameController.GetComponent<GameController> ().currentPlayerTurn;
		costMultiplier = this.transform.parent.GetComponent<PlayerTurnsManager> ().costMultiplier;

		this.transform.position = voters [selectedVoter].transform.position;

		//see ActionScriptTemplate.cs for my explination on this change (Alex Jungroth)

		if (players [currentPlayer].GetComponent<PlayerVariables> ().money >= (baseCost * costMultiplier)) {

			totalCost = (int)(baseCost * costMultiplier);
            visualAid.GetComponent<VisualAidAxisManangerScript>().Attach(this.gameObject);
            transform.position = new Vector3(999, 999, 999); // This is a cheat in making it invisible when spawning

		}
        else
        {
			Debug.Log ("Current Player doesn't have enough money to make this action.");

			//If this isn't called then the buttons will not be removed (Alex Jungroth)
			uiController.GetComponent<UI_Script>().activateAction2UI2();

			uiController.GetComponent<UI_Script>().toggleActionButtons();
			Destroy(gameObject);
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
            visualAid.GetComponent<VisualAidAxisManangerScript>().Detach();
			uiController.GetComponent<UI_Script>().activateAction2UI2();
			uiController.GetComponent<UI_Script>().toggleActionButtons();
			Destroy(gameObject);

		}

		if (!voterSelected) {
			/*if (leftButton) {
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
			}*/
			if(Input.GetMouseButtonDown(0)) {
			   for (selectedVoter = 0; selectedVoter < voters.Length; selectedVoter++) {
					if (voters [selectedVoter].GetComponent<VoterVariables> ().GetSelected ()) {
						voterSelected = true;
						this.GetComponent<MeshRenderer>().enabled = true;
						originalPosition = voters[selectedVoter].transform.position;
						confirmButton = false;
						uiController.GetComponent<UI_Script>().activateAction2UI2();
						foundVoter = true;
                        transform.position = voters[selectedVoter].transform.position;
						break;
					}
				}
				if(!foundVoter)
					Debug.Log ("Please Select A Voter By Mousing Over Them and Clicking Them");
			}
		} else {
			//This is the state in which the player has now chosen which voter to act upon.
			//Here, the player can move the voter one space away from its current spot, but can't move off the grid.
			if (xPlusButton) {
				if ((originalPosition.x + 1) < (gameController.GetComponent<GameController>().gridSize)) {
					this.transform.position = originalPosition + new Vector3(1,0,0);

					//This sets final direction to X+ (Alex Jungroth)
					finalDirection = 1;
				} else {
					this.transform.position = originalPosition;
				}

				xPlusButton = false;
			}
			if (xMinusButton) {
				if ((originalPosition.x - 1) > -1) {
					this.transform.position = originalPosition + new Vector3(-1,0,0);

					//This sets final direction to X- (Alex Jungroth)
					finalDirection = 2;
				} else {
					this.transform.position = originalPosition;
				}

				xMinusButton = false;
			}
			if (zMinusButton) {
				if ((originalPosition.z - 1) > -1) {
					this.transform.position = originalPosition + new Vector3(0,0,-1);

					//This sets final direction to Z- (Alex Jungroth)
					finalDirection = 3;
				} else {
					this.transform.position = originalPosition;
				}

				zMinusButton = false;
			}
			if (zPlusButton) {
				if ((originalPosition.z + 1) < (gameController.GetComponent<GameController>().gridSize)) {
					this.transform.position = originalPosition + new Vector3(0,0,1);

					//This sets final direction to Z+ (Alex Jungroth)
					finalDirection = 4;
				} else {
					this.transform.position = originalPosition;
				}

				zPlusButton = false;
			}
			if (yPlusButton) {
				if ((originalPosition.y + 1) < (gameController.GetComponent<GameController>().gridSize)) {
					this.transform.position = originalPosition + new Vector3(0,1,0);

					//This sets final direction to Y+ (Alex Jungroth)
					finalDirection = 5;
				} else {
					this.transform.position = originalPosition;
				}

				yPlusButton = false;
			}
			if (yMinusButton) {
				if ((originalPosition.y - 1) > -1) {
					this.transform.position = originalPosition + new Vector3(0,-1,0);

					//This sets final direction to Y- (Alex Jungroth)
					finalDirection = 6;
				} else {
					this.transform.position = originalPosition;
				}

				yMinusButton = false;
			}

			//This is where the player confirms he has chosen the spot he wants to move the voter to.
			if (confirmButton){
				//Set initial bool to true, and if it fails any of the checks below, positionSelected becomes false;
				positionSelected = true;

				//Checks if chosen position is the same as the original position, hence not moved at all.
				if (this.transform.position == originalPosition)
					positionSelected = false;

				//Checks if theres any other voters that are in the same position as the chosen position, except for itself.
				for (int i = 0; i < voters.Length; i++) {
					if (i != selectedVoter && voters[i].transform.position == this.transform.position) {
						positionSelected = false;
					}
				}

				if (positionSelected) {
					//tests to see if the action is successful (Alex Jungroth)
					if(compareResistance())
					{
						voters[selectedVoter].transform.position = this.transform.position;
						
						//increases the base resistance of the voter by 1% (Alex Jungroth)
						voters[selectedVoter].GetComponent<VoterVariables>().baseResistance += voters[selectedVoter].GetComponent<VoterVariables>().baseResistance * 0.01f;
						
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
        visualAid.GetComponent<VisualAidAxisManangerScript>().Detach();
		uiController.GetComponent<UI_Script>().activateAction2UI2();//handles early canceling(Alex Jungroth)
		uiController.GetComponent<UI_Script>().toggleActionButtons();
		this.transform.parent.GetComponent<PlayerTurnsManager> ().IncreaseCostMultiplier();

		if (string.Compare((players[currentPlayer].GetComponent<PlayerVariables> ().politicalPartyName), "Windy")== 0)
			players [currentPlayer].GetComponent<PlayerVariables> ().money += totalCost / 4;

		//gives the Espresso party a refund based on their action cost modifier (Alex Jungroth)
		if (players [currentPlayer].GetComponent<PlayerVariables> ().politicalPartyName == "Espresso" && players [currentPlayer].GetComponent<PlayerVariables> ().actionCostModifier > 0) 
		{
			players[currentPlayer].GetComponent<PlayerVariables>().money += (int) Mathf.Ceil
				(totalCost * (1.0f + players [currentPlayer].GetComponent<PlayerVariables> ().actionCostModifier)); 
		}

		players [currentPlayer].GetComponent<PlayerVariables> ().money -= totalCost; // Money is subtracted
		//puts the current player and the event number into the action counter of the event controller
		//Brian Mah
		eventController.actionCounter [gameController.GetComponent<GameController>().currentPlayerTurn] [2]++; // the second number should be the number of the action!

		//updates the tv so the users know whose turn it is (Alex Jungroth)
		uiController.GetComponent<UI_Script>().alterTextBox("It is the " + players[currentPlayer].GetComponent<PlayerVariables>().politicalPartyName +
			" party's turn.\n" + gameController.GetComponent<GameController>().displayPlayerStats());

		Destroy(gameObject);
	}

	bool compareResistance()
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
			if(Random.value > voters[selectedVoter].GetComponent<VoterVariables>().xPlusResistance + voters[selectedVoter].GetComponent<VoterVariables>().baseResistance)
			{
				resistanceCheck = true;
			}
			break;
		
		case 2:
			Debug.Log (selectedVoter);
			if(Random.value > voters[selectedVoter].GetComponent<VoterVariables>().xMinusResistance + voters[selectedVoter].GetComponent<VoterVariables>().baseResistance)
			{
				resistanceCheck = true;
			}
			break;

		case 3:
			if(Random.value > voters[selectedVoter].GetComponent<VoterVariables>().zMinusResistance + voters[selectedVoter].GetComponent<VoterVariables>().baseResistance)
			{
				resistanceCheck = true;
			}
			break;

		case 4:
			if(Random.value > voters[selectedVoter].GetComponent<VoterVariables>().zPlusResistance + voters[selectedVoter].GetComponent<VoterVariables>().baseResistance)
			{
				resistanceCheck = true;
			}
			break;

		case 5:
			if(Random.value > voters[selectedVoter].GetComponent<VoterVariables>().yPlusResistance + voters[selectedVoter].GetComponent<VoterVariables>().baseResistance)
			{
				resistanceCheck = true;
			}
			break;

		case 6:
			if(Random.value > voters[selectedVoter].GetComponent<VoterVariables>().yMinusResistance + voters[selectedVoter].GetComponent<VoterVariables>().baseResistance)
			{
				resistanceCheck = true;
			}
			break;
		}		

		//returns the result of the test
		return resistanceCheck;

	}
}