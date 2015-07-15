// Chris-chan
// Michael's standardization notes:
// PLEASE TALK TO MICHAEL BEFORE WORKING ON ANY ACTIONS
// Every "1" unit of cost is $10k = 10. Which means no baseCost is below 10.
// costMultiplier is actually derived from PlayerTurnsManager.cs, which is increased in EndAction(), additively
// Check if enough money happens in Start(). Subtracting money always happen in EndAction().
// Please use totalCost for any end calculation, since this will be used to display on the UI's action button

using UnityEngine;
using System.Collections;

public class ActionScriptTemplate : MonoBehaviour {
    public string actionName = "default";
	public int baseCost = 0;
    public int totalCost = 0; // Please use totalCost for any end calculation, since this will be used to display on the UI's action button
    public float costMultiplier = 1.0f; // Increased by fixed amount within same turn (in PlayerTurnsManager). This is reset to 1 after the END of your turn.

	public GameObject gameController; //this is the game controller variable. It is obtained from the scene
	public GameObject inputManager; //this is the input manager varibale. Obtained from the scene
	public GameObject uiController; //this is the UI controller variable. Obtained from the scene
    private GameObject visualAid; //this is if you need the visual aid on whatever object you're moving
	private GameObject[] voters; //array which houses the voters. Obtained from the Game Controller
	private GameObject[] players; //array which houses the players. Obtained from the Game Controller

	private int currentPlayer; //this variable finds which player is currently using his turn.

	[System.NonSerialized]
	public bool cancelButton = false;

	private RandomEventControllerScript eventController;

	// Use this for initialization
	void Start () {
		gameController = GameObject.FindWithTag ("GameController");
		inputManager = GameObject.FindWithTag ("InputManager");
		uiController = GameObject.Find ("UI Controller");

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

		//The start function will not end until gets to the end
		//if you want to destroy the object in the start function,
		//it has to be the last thing you do, otherwise the flow of
		//controll will stay with the destroyed instance and 
		//that will crash the game (Alex Jungroth)

		//Get's whose turn it is from the gameController. Then checks if he has enough money to perform the action
		currentPlayer = gameController.GetComponent<GameController> ().currentPlayerTurn;
		costMultiplier = this.transform.parent.GetComponent<PlayerTurnsManager> ().costMultiplier;
		if (players[currentPlayer].GetComponent<PlayerVariables> ().money < (baseCost * costMultiplier)) {
			Debug.Log ("Current Player doesn't have enough money to make this action.");

			uiController.GetComponent<UI_Script>().toggleActionButtons();
			Destroy(gameObject);
		}
        else
        {
            totalCost = (int)(baseCost * costMultiplier);
            visualAid.GetComponent<VisualAidAxisManangerScript>().Attach(this.gameObject); // Only if you need visual aid, or else remove this. Make sure to remove the Detach under Cancel() and EndAction() too.
        }
	}
	
	// Update is called once per frame
	void Update () {

		//ends the action if the cancel button is pressed (Alex Jungroth)
		if (cancelButton) 
		{
            visualAid.GetComponent<VisualAidAxisManangerScript>().Detach(); // Remove if no need visual aid
            Destroy(gameObject);
		}

		//This is where the action should be placed.
		//action action action. blah blah. E.g. move a voter or player one block over.
		//When action is finished, run this bit of code below to tell the Game Manager the turn is over, 
		//and deletes itself so the PlayerTurnsManager knows the turn is over as well.

		/*
		EndAction ();
		*/
	}

	void EndAction() {
        visualAid.GetComponent<VisualAidAxisManangerScript>().Detach(); // Remove if no need visual aid
		uiController.GetComponent<UI_Script>().toggleActionButtons();
		this.transform.parent.GetComponent<PlayerTurnsManager> ().IncreaseCostMultiplier();
		players [currentPlayer].GetComponent<PlayerVariables> ().money -= totalCost;  // Money is subtracted
		//puts the current player and the event number into the action Counter of the event controller
		eventController.actionCounter [gameController.GetComponent<GameController>().currentPlayerTurn] [0]++; // the second number should be the number of the action!
		Destroy(gameObject);
	}
}
