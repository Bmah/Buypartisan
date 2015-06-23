using UnityEngine;
using System.Collections;

public class PlayerTurnsManager : MonoBehaviour {
	public GameObject gameController; //Make sure to place the GameController here, so it can obtain the array.
	public GameObject inputManager; //Make sure to place the inputManager object here, so it can obtain inputs.
	public GameObject uiController; //Make sure to place the UI Controller here.
	public GameObject[] actionArray = new GameObject[10]; //This is the array of action prefabs.
	private GameObject instantiatedAction; //This saves the action prefab that was instantiated to check if it still exists. So long as this prefab exists, TurnsManager knows the turn hasn't ended yet.

	public int chosenAction; //This value changes according to what action the player has chosen
	private bool actionIsRunning = false; //This value checks if the player has chosen an action, and it is currently running

	public bool actionConfirmed; //This confirms that the action has been chosen.

	public bool endTurnConfirmed; //this confirms that the player wishes to end his turn.

//	public int costOfAction = 0; //this will be the variable that saves the cost of the action.
	public int costMultiplier = 0; //this will change if the player continues to make more actions.

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		//If action has been chosen, play the action
		if (actionConfirmed) {
			PlayAction (chosenAction);
		}

		//If there is no action instantiated, then there is no action currently being run.
		if (instantiatedAction == null) {
			actionIsRunning = false;
			actionConfirmed = false;

			if (endTurnConfirmed) {
				gameController.GetComponent<GameController> ().playerTakingAction = true;
				endTurnConfirmed = false;
				costMultiplier = 0;
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
}
