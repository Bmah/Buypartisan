using UnityEngine;
using System.Collections;

public class PlayerTurnsManager : MonoBehaviour {
	public GameObject gameController; //Make sure to place the GameController here, so it can obtain the array.
	public GameObject[] actionArray = new GameObject[10]; //This is the array of action prefabs.
	private GameObject instantiatedAction; //This saves the action prefab that was instantiated to check if it still exists. So long as this prefab exists, TurnsManager knows the turn hasn't ended yet.

	public int chosenAction; //This value changes according to what action the player has chosen
	private bool actionIsRunning = false; //This value checks if the player has chosen an action, and it is currently running

	public bool actionConfirmed; //This confirms that the action has been chosen.

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
		}

		if (Input.GetKeyDown (KeyCode.Z)) {
			actionConfirmed = true;
		}
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
			} else {
				Debug.Log ("There is no action placed in this spot of the array");
			}
		}
	}
}
