﻿using UnityEngine;
using System.Collections;

public class VoterVariables : MonoBehaviour {

    // Transform is built-in

    public int money = 0;
    public int votes = 0;
    public int type = 0; // Instead of string, int stores less data. Type 0 can be default, type 1 can be enviormentalist, type 2 can be mafias, etc.
	public int powerType;//used to determine what power is used, se by button press
	private bool selected = false; //Use Toggle Function to change it
	public Material selectedTexture;
	public Material unselectedTexture;
	private Renderer voterRenderer;
	public Collider Coll;//not sure if needed

	//holds the game controller (Alex Jungroth)
	private GameController gameController;

	//These variables hold a voters resistance to being moved (Alex Jungroth)
	public float baseResistance = 0;
	public float xPlusResistance = 0;
	public float xMinusResistance = 0;
	public float yPlusResistance = 0;
	public float yMinusResistance = 0;
	public float zPlusResistance = 0;
	public float zMinusResistance = 0;

	//voterOwner Brian Mah
	public GameObject CanidateChoice;
	//private bool movedRecently = false;
	private Vector3 prevPosition;
	public GameObject[] players;
	public bool contested = false;
	public Material contestedSelectedTexture;
	public Material contestedUnselectedTexture;
	private Material originalSelected;
	private Material originalUnselected;

	//These variables are used for choosing hats.
	public float extremistRatio = 0.2f;
	private bool hasHat = false;
	private int hatIndex = 0;
	public GameObject[] hatArray = new GameObject[7];

	void Start () {
		voterRenderer = this.transform.GetChild (0).transform.GetChild(1).gameObject.GetComponent<Renderer> ();	//changed this to obtain the model's renderer.
		Coll = this.GetComponent<Collider> ();
		powerType = 1; //hardcoded for testing suppression, make sure to remove when code in place for buttons assigning
		
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController>();

		//initialization for voter Owner system
		prevPosition = this.transform.position;
		originalSelected = selectedTexture;
		originalUnselected = unselectedTexture;

		DetermineHatEligibilty ();
		//Randomize rotation
		transform.rotation = Quaternion.Euler (0f, Random.Range (0f, 360f), 0f);
	}

	/// <summary>
	/// should have two checks, one controlled by a bool "powersUsed".  If powers aren't being used, clicking on a voter should bring up a 
	/// small menu showing their attributes (votes and Money).  If powers is used, this will trigger going to the powers function, and 
	/// will execute whatever power numvber "powerType" is set to.  
	/// </summary>
	void Update () {
		//bool controller goes here, should be enabled when a player clicks on a power
		if (Input.GetMouseButton (0)) {//this has to get moved to the input manager
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Coll.Raycast (ray, out hit, 100.0F)) {
				ToggleSelected();

				ToggleSelected();
			}
		}

		//checks if the position has changed from previous update
		if (prevPosition != this.transform.position) {
			FindCanidate();
		}// if position is not the previous position
		prevPosition = this.transform.position;


	}//Update

	//looks at all canidates and finds which one this voter belongs to
	public void FindCanidate(){
		contested = false;
		float closestDistance = 1000f;
		float currentCanidateDistance;

		for (int i = 0; i < gameController.NumPlayersSpawned; i++) {
			for (int j = 0; j < players[i].GetComponent<PlayerVariables>().shadowPositions.Count; j++) {
				//distance to the canidate's shadow positions being checked
				currentCanidateDistance = (this.transform.position - players [i].GetComponent<PlayerVariables> ().shadowPositions [j].transform.position).magnitude;
			
				//if that is closer than the previous closestDistance
				if (currentCanidateDistance < closestDistance && currentCanidateDistance <= players [i].GetComponent<PlayerVariables> ().shadowPositions [j].GetComponent<PlayerVariables> ().SphereObject.transform.localScale.x / 20f) {
					closestDistance = currentCanidateDistance;
					CanidateChoice = players [i];
				} else if (currentCanidateDistance == closestDistance) {
					CanidateChoice = null;
				}
			}//for shadowpositions

			//distance to the canidate you are checking
			currentCanidateDistance = (this.transform.position - players [i].transform.position).magnitude;
		
			//if that is closer than the previous closestDistance
			if (currentCanidateDistance < closestDistance && currentCanidateDistance <= players [i].GetComponent<PlayerVariables> ().SphereObject.transform.localScale.x / 20f) {
				closestDistance = currentCanidateDistance;
				CanidateChoice = players [i];
			} else if (currentCanidateDistance == closestDistance) {
				CanidateChoice = null;
			}
		
		}//for players

		//if no canidates are within range
		if(closestDistance == 1000f){
			CanidateChoice = null;
			selectedTexture = originalSelected;
			unselectedTexture = originalUnselected;
			voterRenderer.material = unselectedTexture;
		}
		else if(CanidateChoice == null){  //if there is a tie
			selectedTexture = contestedSelectedTexture;
			contested = true;
			unselectedTexture = contestedUnselectedTexture;
			voterRenderer.material = unselectedTexture;
		}
		else{ //if a canidate has been selected
//			selectedTexture = CanidateChoice.GetComponent<Renderer>().material;
			//selectedTexture.color = new Color(selectedTexture.color.r + 0.5f, selectedTexture.color.g + 0.5f, selectedTexture.color.b + 0.5f);
//			unselectedTexture = CanidateChoice.GetComponent<Renderer>().material;
//			selectedTexture = CanidateChoice.transform.GetChild (1).transform.GetChild(1).gameObject.GetComponent<Renderer> ().material;
//			unselectedTexture = CanidateChoice.transform.GetChild (1).transform.GetChild(1).gameObject.GetComponent<Renderer> ().material;
			selectedTexture = CanidateChoice.GetComponent<PlayerVariables>().PlayerSelectedTexture;
			unselectedTexture = CanidateChoice.GetComponent<PlayerVariables>().PlayerUnselectedTexture;
			voterRenderer.material = unselectedTexture;
		}

	}
	
	/// <summary>
	/// Raises the mouse enter event.
	/// Used to select the voter and display their stats on the textbox
	/// Brian Mah
	/// </summary>
	void OnMouseEnter(){
		ToggleSelected ();

		//This was interfering with the parites money being updated after an action (Alex Jungroth)
		gameController.GetComponent<GameController>().popUpTVScript.GetComponent<PopUpTVScript>().SetPopupTextBox("Money: " + money + "\nVotes: " + votes);
		gameController.GetComponent<GameController>().popUpTVScript.GetComponent<PopUpTVScript>().ShortWaitForUIToolTip();
	}

	/// <summary>
	/// Raises the mouse exit event.
	/// Used to unselect the voter and put the text back to what it was.
	/// Brian Mah
	/// </summary>
	void OnMouseExit(){
		ToggleSelected ();

		//This was interfering with the parites money being updated after an action (Alex Jungroth)
		gameController.GetComponent<GameController>().popUpTVScript.GetComponent<PopUpTVScript>().ExitUIToolTip();
	}

	/// <summary>
	/// Toggles whether or not the Voter is selected.
	/// Brian Mah
	/// </summary>
	public void ToggleSelected(){
		if (selected) {
			selected = false;
			voterRenderer.material = unselectedTexture;
		}
		else {
			selected = true;
			voterRenderer.material = selectedTexture;
		}
	}

	/// <summary>
	/// Gets whether or not the voter is selected.
	/// Brian Mah
	/// </summary>
	/// <returns><c>true</c>, if selected was gotten, <c>false</c> otherwise.</returns>
	public bool GetSelected(){
		return selected;
	}

	void DetermineHatEligibilty() {
		float largestAxis = 0f;

		if (transform.position.x >= Mathf.Round(gameController.gridSize * (1f - extremistRatio)) - 1f) {
			if (transform.position.x > largestAxis) {
				largestAxis = transform.position.x;
				hatIndex = 0;
			}
			if (transform.position.x == largestAxis) {
				if (Mathf.Round (Random.value) == 0) {
					hatIndex = 0;
				} else {
					//Other hat is chosen.
				}
			}
			hasHat = true;
		}
		if (transform.position.y >= Mathf.Round(gameController.gridSize * (1f - extremistRatio)) - 1f) {
			if (transform.position.y > largestAxis) {
				largestAxis = transform.position.y;
				hatIndex = 2;
			}
			if (transform.position.y == largestAxis) {
				if (Mathf.Round (Random.value) == 0) {
					hatIndex = 2;
				} else {
					//Other hat is chosen.
				}
			}
			hasHat = true;
		}
		if (transform.position.z >= Mathf.Round(gameController.gridSize * (1f - extremistRatio)) - 1f) {
			if (transform.position.z > largestAxis) {
				largestAxis = transform.position.z;
				hatIndex = 4;
			}
			if (transform.position.z == largestAxis) {
				if (Mathf.Round (Random.value) == 0) {
					hatIndex = 4;
				} else {
					//Other hat is chosen.
				}
			}
			hasHat = true;
		}
		if (transform.position.x <= Mathf.Round (gameController.gridSize * extremistRatio)) {
			if (gameController.gridSize - 1f - transform.position.x > largestAxis) {
				largestAxis = gameController.gridSize - 1f - transform.position.x;
				hatIndex = 1;
			}
			if (gameController.gridSize - 1f - transform.position.x == largestAxis) {
				if (Mathf.Round (Random.value) == 0) {
					hatIndex = 1;
				} else {
					//Other hat is chosen.
				}
			}
			hasHat = true;
		}
		if (transform.position.y <= Mathf.Round (gameController.gridSize * extremistRatio)) {
			if (gameController.gridSize - 1f - transform.position.y > largestAxis) {
				largestAxis = gameController.gridSize - 1f - transform.position.y;
				hatIndex = 3;
			}
			if (gameController.gridSize - 1f - transform.position.y == largestAxis) {
				if (Mathf.Round (Random.value) == 0) {
					hatIndex = 3;
				} else {
					//Other hat is chosen.
				}
			}
			hasHat = true;
		}
		if (transform.position.z <= Mathf.Round (gameController.gridSize * extremistRatio)) {
			if (gameController.gridSize - 1f - transform.position.z > largestAxis) {
				largestAxis = gameController.gridSize - 1f - transform.position.z;

				if (Mathf.Round (Random.value) == 0) {
					hatIndex = 5;
				} else {
					hatIndex = 6;
				}
			}
			if (gameController.gridSize - 1f - transform.position.z == largestAxis) {
				if (Mathf.Round (Random.value) == 0) {
					if (Mathf.Round (Random.value) == 0) {
						hatIndex = 5;
					} else {
						hatIndex = 6;
					}
				} else {
					//Other hat is chosen.
				}
			}
			hasHat = true;
		}

		if (hasHat) {
			GameObject currentHat = Instantiate(hatArray[hatIndex]) as GameObject;
			currentHat.transform.parent = this.transform.GetChild (0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0);
			currentHat.transform.localPosition = new Vector3(-1f,0f,0f);
			currentHat.transform.localRotation = Quaternion.Euler(90f,-90f,0f);
			currentHat.transform.localScale = new Vector3(1f,1f,1f);
		}
	}
}
