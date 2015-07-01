﻿// Chris-chan
// Michael's standardization notes:
// PLEASE TALK TO MICHAEL BEFORE WORKING ON ANY ACTIONS
// Every "1" unit of cost is $10k = 10. Which means no baseCost is below 10.
// costMultiplier is actually derived from PlayerTurnsManager.cs, which is increased in EndAction(), additively
// Check if enough money happens in Start(). Subtracting money always happen in EndAction().
// Please use totalCost for any end calculation, since this will be used to display on the UI's action button

using UnityEngine;
using System.Collections;
/// <summary>
/// Action0 script by Daniel Schlesinger
/// 
/// </summary>
public class Action0Script : MonoBehaviour {
    public string actionName = "VoterSuppression";
	public int baseCost = 20;
    public int totalCost = 0; // Please use totalCost for any end calculation, since this will be used to display on the UI's action button
    public float costMultiplier = 1.0f; // Increased by fixed amount within same turn (in PlayerTurnsManager). This is reset to 1 after the END of your turn.

	public GameObject gameController; //this is the game controller variable. It is obtained from the scene
	public GameObject inputManager; //this is the input manager varibale. Obtained from the scene
	public GameObject uiController; //this is the UI controller variable. Obtained from the scene
	private GameObject[] voters; //array which houses the voters. Obtained from the Game Controller
	private GameObject[] players; //array which houses the players. Obtained from the Game Controller

	private int currentPlayer; //this variable finds which player is currently using his turn.
	private int selectedVoter = 0;
	private bool voterSelected = false;

	[System.NonSerialized]
	public bool leftButton = false; //checks if left button has been pressed
	[System.NonSerialized]
	public bool rightButton = false; //checks if right button has been pressed
	[System.NonSerialized]
	public bool confirmButton = false; //checks if confirm button has been pressed
	[System.NonSerialized]
	public bool cancelButton = false;

	//holds the number needed for this action to succeed (Alex Jungroth)
	public float successRate = 0.5f;

	// Use this for initialization
	void Start () {
		gameController = GameObject.FindWithTag ("GameController");
		inputManager = GameObject.FindWithTag ("InputManager");
		uiController = GameObject.Find ("UI Controller");

		uiController.GetComponent<UI_Script> ().disableActionButtons ();
		uiController.GetComponent<UI_Script>().activateAction0UI();
		//Obtains the voter and player array from the gameController
		if (gameController != null) {
			voters = gameController.GetComponent<GameController> ().voters;
			players = gameController.GetComponent<GameController> ().players;
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

		this.transform.position = voters [selectedVoter].transform.position;

		if (players[currentPlayer].GetComponent<PlayerVariables> ().money < (baseCost * costMultiplier)) {
			Debug.Log ("Current Player doesn't have enough money to make this action.");

			uiController.GetComponent<UI_Script>().toggleActionButtons();
			Destroy(gameObject);
		}
        else
        {
            totalCost = (int)(baseCost * costMultiplier);
        }
	}
	
	// Update is called once per frame
	void Update () {

		//ends the action if the cancel button is pressed (Alex Jungroth)
		if (cancelButton) 
		{
			//handles early canceling(Alex Jungroth)
			uiController.GetComponent<UI_Script>().activateAction0UI2();
			uiController.GetComponent<UI_Script>().toggleActionButtons();
			Destroy(gameObject);
		}

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
				Debug.Log ("Here");
				voterSelected = true;
				confirmButton = false;
				uiController.GetComponent<UI_Script>().activateAction0UI2();

				//checks to see if the power succeeded (Alex Jungroth)
				if(Random.Range(0.5f,1) >= successRate)
				{
					voters [selectedVoter].GetComponent<VoterVariables> ().votes = 0;
				}
			}
		}
		if (voterSelected)
			EndAction();

	}

	void EndAction() {
		uiController.GetComponent<UI_Script>().activateAction0UI2();
		uiController.GetComponent<UI_Script>().toggleActionButtons();
		this.transform.parent.GetComponent<PlayerTurnsManager> ().IncreaseCostMultiplier();
		players [currentPlayer].GetComponent<PlayerVariables> ().money -= totalCost;  // Money is subtracted
		Destroy(gameObject);
	}
}
