// Chris-chan
// Michael's standardization notes:
// PLEASE TALK TO MICHAEL BEFORE WORKING ON ANY ACTIONS
// Every "1" unit of cost is $10k = 10. Which means no baseCost is below 10.
// costMultiplier is actually derived from PlayerTurnsManager.cs, which is increased in EndAction(), additively
// Check if enough money happens in Start(). Subtracting money always happen in EndAction().
// Please use totalCost for any end calculation, since this will be used to display on the UI's action button

using UnityEngine;
using System.Collections;

public class Action8Script : MonoBehaviour {
	public string actionName = "Campaign Donations";
	public int baseCost = 325;
	public int totalCost = 0; // Please use totalCost for any end calculation, since this will be used to display on the UI's action button
	public float costMultiplier = 1.0f; // Increased by fixed amount within same turn (in PlayerTurnsManager). This is reset to 1 after the END of your turn.
	
	public GameObject gameController; //this is the game controller variable. It is obtained from the scene
	public GameObject inputManager; //this is the input manager varibale. Obtained from the scene
	public GameObject uiController; //this is the UI controller variable. Obtained from the scene
	private GameObject visualAid; //this is if you need the visual aid on whatever object you're moving
	private GameObject[] voters; //array which houses the voters. Obtained from the Game Controller
	private GameObject[] players; //array which houses the players. Obtained from the Game Controller
	
	private int currentPlayer; //this variable finds which player is currently using his turn.
	private bool actionDone;
	
	[System.NonSerialized]
	public bool confirmButton = false;
	[System.NonSerialized]
	public bool cancelButton = false;

	private RandomEventControllerScript eventController;

	//Allows the Action to play sounds (Brian Mah)
	private SFXController SFX;

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
		uiController.GetComponent<UI_Script> ().activateAction5UI ();
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
		//	visualAid.GetComponent<VisualAidAxisManangerScript>().Attach(this.gameObject); // Only if you need visual aid, or else remove this. Make sure to remove the Detach under Cancel() and EndAction() too.
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
//			visualAid.GetComponent<VisualAidAxisManangerScript>().Detach(); // Remove if no need visual aid
			uiController.GetComponent<UI_Script>().toggleActionButtons();
			Destroy(gameObject);
		}

		if (confirmButton) {
			if(Random.value >= 0.2f) {
				for(int i = 0; i < voters.Length; i++) {
					if((voters[i].transform.position - players[currentPlayer].transform.position).magnitude <= players[currentPlayer].GetComponent<PlayerVariables>().sphereController.transform.localScale.x) {
						players[currentPlayer].GetComponent<PlayerVariables>().money += voters[i].GetComponent<VoterVariables>().money/10;
						voters[i].GetComponent<VoterVariables>().money -= voters[i].GetComponent<VoterVariables>().money/10;
					}
				}
			}
			actionDone = true;
		}
		if(actionDone)
		EndAction ();
	}
	
	void EndAction() {
		//visualAid.GetComponent<VisualAidAxisManangerScript>().Detach(); // Remove if no need visual aid
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

		players [currentPlayer].GetComponent<PlayerVariables> ().money -= totalCost;  // Money is subtracted
		//puts the current player and the event number into the action Counter of the event controller
		eventController.actionCounter [gameController.GetComponent<GameController>().currentPlayerTurn] [0]++; // the second number should be the number of the action!

		//updates the tv so the users know whose turn it is (Alex Jungroth)
		uiController.GetComponent<UI_Script>().alterTextBox("It is the " + players[currentPlayer].GetComponent<PlayerVariables>().politicalPartyName +
			" party's turn.\n" + gameController.GetComponent<GameController>().displayPlayerStats());

		Destroy(gameObject);
	}
}
