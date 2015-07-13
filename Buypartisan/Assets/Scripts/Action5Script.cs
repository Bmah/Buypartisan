/// <summary>
/// Sphere of Influence script by Daniel Schlesinger
/// </summary>
using UnityEngine;
using System.Collections;

public class Action5Script : MonoBehaviour {
	public string actionName = "Sphere of Influence";
	public int baseCost = 300;
	public int totalCost = 0; // Please use totalCost for any end calculation, since this will be used to display on the UI's action button
	public float costMultiplier = 1.0f; // Increased by fixed amount within same turn (in PlayerTurnsManager). This is reset to 1 after the END of your turn.
	
	public GameObject gameController; //this is the game controller variable. It is obtained from the scene
	public GameObject inputManager; //this is the input manager varibale. Obtained from the scene
	public GameObject uiController; //this is the UI controller variable. Obtained from the scene
	private GameObject[] voters; //array which houses the voters. Obtained from the Game Controller
	private GameObject[] players; //array which houses the players. Obtained from the Game Controller
	
	private int currentPlayer; //this variable finds which player is currently using his turn.

	[System.NonSerialized]
	public bool confirmButton = false;
	[System.NonSerialized]
	public bool cancelButton = false;
	
	// Use this for initialization
	void Start () {
		gameController = GameObject.FindWithTag ("GameController");
		inputManager = GameObject.FindWithTag ("InputManager");
		uiController = GameObject.Find ("UI Controller");

		uiController.GetComponent<UI_Script> ().disableActionButtons ();
		uiController.GetComponent<UI_Script>().activateAction5UI();
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
		//Debug.Log ("here");
		//ends the action if the cancel button is pressed (Alex Jungroth)
		if (cancelButton) 
		{
			uiController.GetComponent<UI_Script>().toggleActionButtons();
			Destroy(gameObject);
		}

		if (confirmButton) {
			players [currentPlayer].GetComponent<PlayerVariables>().sphereController.transform.localScale += new Vector3 (10f, 10f, 10f);
			EndAction ();
		}
	}
	
	void EndAction() {
		uiController.GetComponent<UI_Script>().toggleActionButtons();
		this.transform.parent.GetComponent<PlayerTurnsManager> ().IncreaseCostMultiplier();
		players [currentPlayer].GetComponent<PlayerVariables> ().money -= totalCost;  // Money is subtracted
		Destroy(gameObject);
	}
}

