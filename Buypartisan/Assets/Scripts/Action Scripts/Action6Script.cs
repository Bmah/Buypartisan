/// <summary>
/// Character Assassination script by Daniel Schlesinger
/// </summary>
using UnityEngine;
using System.Collections;

public class Action6Script : MonoBehaviour {
	public string actionName = "Character Assassination";
	public int baseCost = 300;
	public int totalCost = 0; // Please use totalCost for any end calculation, since this will be used to display on the UI's action button
	public float costMultiplier = 1.0f; // Increased by fixed amount within same turn (in PlayerTurnsManager). This is reset to 1 after the END of your turn.
	
	public GameObject gameController; //this is the game controller variable. It is obtained from the scene
	public GameObject inputManager; //this is the input manager varibale. Obtained from the scene
	public GameObject uiController; //this is the UI controller variable. Obtained from the scene
	//private GameObject[] voters; //array which houses the voters. Obtained from the Game Controller
	private GameObject[] players; //array which houses the players. Obtained from the Game Controller
	//Brian Mah
	private RandomEventControllerScript eventController;

	private int currentPlayer; //this variable finds which player is currently using his turn.
	private int selectedPlayer;
	private bool playerSelected = false;
	//private bool foundPlayer = false;
	public float successRate = 0.25f;

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

		uiController.GetComponent<UI_Script> ().disableActionButtons ();
		uiController.GetComponent<UI_Script>().activateAction0UI();
		//Obtains the voter and player array from the gameController
		if (gameController != null) {
			//voters = gameController.GetComponent<GameController> ().voters;
			players = gameController.GetComponent<GameController> ().players;
			eventController = gameController.GetComponent<GameController> ().randomEventController;
			SFXVolume = gameController.GetComponent<GameController> ().SFXVolume;
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

		//Sets up SFX controller (Brian Mah)
		SFX = GameObject.FindGameObjectWithTag("SFX").GetComponent<SFXController>();
		if (SFX == null) {
			Debug.LogError("Could not find SFX controller");
		}

		//Get's whose turn it is from the gameController. Then checks if he has enough money to perform the action
		currentPlayer = gameController.GetComponent<GameController> ().currentPlayerTurn;
		costMultiplier = this.transform.parent.GetComponent<PlayerTurnsManager> ().costMultiplier;
		if (players[currentPlayer].GetComponent<PlayerVariables> ().money < (baseCost * costMultiplier)) {
			Debug.Log ("Current Player doesn't have enough money to make this action.");
			SFX.PlayAudioClip(15,0,SFXVolume);
			
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
		//Debug.Log ("here");
		//ends the action if the cancel button is pressed (Alex Jungroth)
		if (cancelButton) {
			uiController.GetComponent<UI_Script> ().activateAction0UI2 ();
			uiController.GetComponent<UI_Script> ().toggleActionButtons ();
			Destroy (gameObject);
		}
		if (Input.GetMouseButtonDown (0)) {
			for (selectedPlayer = 0; selectedPlayer < players.Length; selectedPlayer++) {
                //The second check prevents the sphere from being shrunk to a negative size (AAJ)
                if (players[selectedPlayer].GetComponent<PlayerVariables>().GetSelected() && players[selectedPlayer].GetComponent<PlayerVariables>().sphereController.transform.localScale.x > 0) {
					//foundPlayer = true;
					playerSelected = true;
					Debug.Log (playerSelected);
					if (Random.value >= successRate) {
						players [selectedPlayer].GetComponent<PlayerVariables> ().sphereController.transform.localScale -= new Vector3 (10f, 10f, 10f);
						SFX.PlayAudioClip (13, 0, SFXVolume);
					}
					else{
						SFX.PlayAudioClip (14, 0, SFXVolume);
					}
				}
			}
		}
		if (playerSelected) {
			EndAction ();
		}
	}
	
	void EndAction() {
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
		//puts the current player and the event number into the action counter of the event controller
		//Brian Mah
		eventController.actionCounter [gameController.GetComponent<GameController>().currentPlayerTurn] [6]++; // the second number should be the number of the action!

		//updates the tv so the users know whose turn it is (Alex Jungroth)
		uiController.GetComponent<UI_Script>().alterTextBox("It is the " + players[currentPlayer].GetComponent<PlayerVariables>().politicalPartyName +
			" party's turn.\n" + gameController.GetComponent<GameController>().displayPlayerStats());

		Destroy(gameObject);
	}
}

