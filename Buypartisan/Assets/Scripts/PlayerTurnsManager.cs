// Chris-chan
// Michael's standardization notes: 
// Cost multiplier is always calculated as percentages (1 = 100%). After every turn, the cost multiplier increases by 50% (0.5f).
// Since the money value is always an integer full number, MAKE SURE to cast whatever final calculation your costMultiplier deals with, e.g. int num = (int)1.532f;
// You can change that value in costMultiplierIncreaseAmount. It increases ONLY when an action is SUCCESFULLY ran.
// If we want our own actions to decrease our own costMultiplier, add a public void alterCostMultiplier function and ONLY change costMultiplier.
// If you want to increase the enemy's costMultiplier, add a function public void alterEnemyCostMultiplier and call it AFTER "costMultiplier = 1" under endTurnConfirmed.

using UnityEngine;
using System.Collections;

public class PlayerTurnsManager : MonoBehaviour {
	public GameObject gameController; //Make sure to place the GameController here, so it can obtain the array.
	public GameObject inputManager; //Make sure to place the inputManager object here, so it can obtain inputs.
	public GameObject uiController; //Make sure to place the UI Controller here.
	public GameObject[] actionArray = new GameObject[10]; //This is the array of action prefabs.
	private GameObject instantiatedAction; //This saves the action prefab that was instantiated to check if it still exists. So long as this prefab exists, TurnsManager knows the turn hasn't ended yet.
	private GameObject[] voters; //array which houses the voters. Obtained from the Game Controller
	private GameObject[] players; //array which houses the players. Obtained from the Game Controller
	private int currentPlayer;//which player is currently taking their turn

	public int chosenAction; //This value changes according to what action the player has chosen
	private bool actionIsRunning = false; //This value checks if the player has chosen an action, and it is currently running

	public bool actionConfirmed; //This confirms that the action has been chosen.

	public bool endTurnConfirmed; //this confirms that the player wishes to end his turn.

//	public int costOfAction = 0; //this will be the variable that saves the cost of the action.
	public float costMultiplier = 1.0f; //this will change if the player continues to make more actions.
    public float costMultiplierIncreaseAmount = 0.5f;
	public bool firstTime = true;
	// Use this for initialization
	void Start () {
		voters = gameController.GetComponent<GameController> ().voters;
		players = gameController.GetComponent<GameController> ().players;
	}
	
	// Update is called once per frame
	void Update () {
		//If action has been chosen, play the action
		if (actionConfirmed) {
            uiController.GetComponent<UI_Script>().updateCost();
			PlayAction (chosenAction);
		}

		//If there is no action instantiated, then there is no action currently being run.
		if (instantiatedAction == null) {
			actionIsRunning = false;
			actionConfirmed = false;

			if (endTurnConfirmed) {
				gameController.GetComponent<GameController> ().playerTakingAction = true;
				endTurnConfirmed = false;
				costMultiplier = 1;
                uiController.GetComponent<UI_Script>().updateCost();
			}
		}

//		if (Input.GetKeyDown (KeyCode.Z)) {
//			actionConfirmed = true;
//		}
	}

	/// <summary>
	/// Plays the action.
	/// Once the action is chosen, confirmed, and an action isn't already running, this function Instantiates an action prefab that was chosen.
	/// IF there is no prefab in the slot chosen, it prints the string below instead.
	/// </summary>
	void PlayAction(int actionNumber) {
		if (!actionIsRunning) {
			if (actionArray [actionNumber] != null) {
				instantiatedAction = Instantiate (actionArray [actionNumber]);
				instantiatedAction.transform.parent = this.transform;
				actionIsRunning = true;
				uiController.GetComponent<UI_Script>().instantiatedAction = instantiatedAction;
			} else {
				Debug.Log ("There is no action placed in this spot of the array");
			}
		}
	}

    public void IncreaseCostMultiplier()
    {   
		currentPlayer = gameController.GetComponent<GameController> ().currentPlayerTurn;
		if (players [currentPlayer].GetComponent<PlayerVariables> ().politicalPartyName == "Coffee") {
			if (firstTime) {
				firstTime = false;
				return 0;
			}
		}
        costMultiplier += costMultiplierIncreaseAmount;
    }
}
