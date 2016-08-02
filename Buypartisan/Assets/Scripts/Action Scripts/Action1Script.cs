// Chris-chan
// Power Summary: Move Player
// This power will be able to move the player one unit away from his current position to be his new position.

using UnityEngine;
using System.Collections;

public class Action1Script : MonoBehaviour {

    public string actionName = "Move Self";
	public int baseCost = 50;
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
    private GameObject visualAid;
	//Brian Mah
	private RandomEventControllerScript eventController;

	private int currentPlayer; // Tells you the source player who's using the action

	private Vector3 originalPosition; // Original position that the player started at.
	private bool chosenPositionConfirmed = false; // Destination been chosen and confirmed.
	private Vector3 currentPos;

	//keeps track of whether or not there is a shadow position where the player is trying to move (Alex Jungroth)
	bool occupiedByShadow = false;

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

	//Allows the Action to play sounds (Brian Mah)
	private SFXController SFX;
	private float SFXVolume;

	// Use this for initialization
	void Start () {
		gameController = GameObject.FindWithTag ("GameController");
		inputManager = GameObject.FindWithTag ("InputManager");
		uiController = GameObject.Find ("UI Controller");
        visualAid = GameObject.FindWithTag("VisualAidManager");

		uiController.GetComponent<UI_Script>().disableActionButtons();
		uiController.GetComponent<UI_Script> ().activateAction1UI ();
		
		if (gameController != null) {
			players = gameController.GetComponent<GameController> ().Players;
			eventController = gameController.GetComponent<GameController> ().randomEventController;
			SFXVolume = gameController.GetComponent<GameController> ().SFXVolume;
		} else {
			Debug.Log ("Failed to obtain voters and players array from Game Controller");
		}
		
		currentPlayer = gameController.GetComponent<GameController> ().CurrentPlayerTurn;
		costMultiplier = this.transform.parent.GetComponent<PlayerTurnsManager> ().costMultiplier;

		originalPosition = players[currentPlayer].transform.position;
		this.transform.position = originalPosition;

		//see ActionScriptTemplate.cs for my explanation on this change (Alex Jungroth)

		//Sets up SFX controller (Brian Mah)
		SFX = GameObject.FindGameObjectWithTag("SFX").GetComponent<SFXController>();
		if (SFX == null) {
			Debug.LogError("Could not find SFX controller");
		}

		if (players [currentPlayer].GetComponent<PlayerVariables> ().money < (baseCost * costMultiplier)) {
			Debug.Log ("Current Player doesn't have enough money to make this action.");
			SFX.PlayAudioClip(15,0,SFXVolume);
			uiController.GetComponent<UI_Script>().toggleActionButtons();
			Destroy(gameObject);
		}
        else
        {
            visualAid.GetComponent<VisualAidAxisManangerScript>().Attach(this.gameObject);
        }
	}
	
	// Update is called once per frame
	void Update () {

		//ends the action if the cancel button is pressed (Alex Jungroth)
		if (cancelButton) 
		{
			//handles early canceling(Alex Jungroth)
            visualAid.GetComponent<VisualAidAxisManangerScript>().Detach();
			uiController.GetComponent<UI_Script>().toggleActionButtons();
			Destroy(gameObject);
		}

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
			chosenPositionConfirmed = true;

			if (transform.position == originalPosition)
				chosenPositionConfirmed = false;

			for (int i = 0; i < players.Length; i++) {
				if (i != currentPlayer && players[i].transform.position == transform.position) {
					chosenPositionConfirmed = false;
				}
			}

			if (totalCost > players[currentPlayer].GetComponent<PlayerVariables>().money) {
				Debug.Log ("You don't have enough money to move to this spot!");
				chosenPositionConfirmed = false;
			} else {
				//prevents players from moving onto shadow positions (Alex Jungroth)
				for(int j = 0; j < players.Length; j++)
				{
					for(int k = 0; k < players[j].GetComponent<PlayerVariables>().shadowPositions.Count; k++)
					{
						if(transform.position == players[j].GetComponent<PlayerVariables>().shadowPositions[k].GetComponent<PlayerVariables>().transform.position)
						{
							occupiedByShadow = true;
						}
					}
				}
				
				
			}

			//only allows the player to move if there is no shadow position occupying that position
			if(!occupiedByShadow && chosenPositionConfirmed)
			{
				players[currentPlayer].transform.position = transform.position;
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
		
        // Example when you're using this as 3rd power: Total Cost = $50 * 2.0 * (1.5)^3 * 4
		totalCost = (int)((baseCost * costMultiplier) * (Mathf.Pow (stepCostMultiplier, (distance - 1))) * distance);
	}

	void EndAction() {
        visualAid.GetComponent<VisualAidAxisManangerScript>().Detach();
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


		players[currentPlayer].GetComponent<PlayerVariables>().money -= totalCost; // Money is subtracted
		//puts the current player and the event number into the action counter of the event controller
		//Brian Mah
		eventController.actionCounter [gameController.GetComponent<GameController>().CurrentPlayerTurn] [1]++; // the second number should be the number of the action!

		//updates the tv so the users know whose turn it is (Alex Jungroth)
		uiController.GetComponent<UI_Script>().alterTextBox("It is the " + players[currentPlayer].GetComponent<PlayerVariables>().politicalPartyName +
			" party's turn.\n" + gameController.GetComponent<GameController>().displayPlayerStats());

		SFX.PlayAudioClip (13, 0, SFXVolume);

		Destroy(gameObject);
	}
}
