using UnityEngine;
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
	private UI_Script UIController;

	//holds the game controller (Alex Jungroth)
	private GameController gameController;

	string holdingText;

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


	void Start () {
		//voterRenderer = this.GetComponent<Renderer>();
		voterRenderer = transform.Find ("PawnGroup").transform.Find ("SketchUp").GetComponent<Renderer> ();
		Coll = this.GetComponent<Collider> ();
		powerType = 1; //hardcoded for testing suppression, make sure to remove when code in place for buttons assigning
		//ControllerSquared = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		UIController = GameObject.FindGameObjectWithTag ("UI_Controller").GetComponent<UI_Script> ();

		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController>();

		//initialization for voter Owner system
		prevPosition = this.transform.position;
		originalSelected = selectedTexture;
		originalUnselected = unselectedTexture;
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
		if(gameController.isActionTurn == true)
		{
			//checks if the position has changed from previous update
			if (prevPosition != this.transform.position) {
				FindCanidate();
			}// if position is not the previous position
			prevPosition = this.transform.position;
		}

	}//Update

	//looks at all canidates and finds which one this voter belongs to
	public void FindCanidate(){
		contested = false;
		float closestDistance = 1000f;
		float currentCanidateDistance;
		//stops a null reference when the players are spawning (Alex Jungroth)
		if (gameController.numberPlayers == gameController.playersSpawned) 
		{
			for (int i = 0; i < gameController.numberPlayers; i++) {
				//prevents a index out of bounds exception if there are no shadow positions
				//in the player's array of shadow positions (Alex Jungroth)
				if (players [i].GetComponent<PlayerVariables> ().shadowPositions.Count > 0) {
					for (int j = 0; j < players[i].GetComponent<PlayerVariables>().shadowPositions.Count; j++) {
						//distance to the canidate's shadow positions being checked
						currentCanidateDistance = (this.transform.position - players [i].GetComponent<PlayerVariables> ().shadowPositions [j].transform.position).magnitude;
					
						//if that is closer than the previous closestDistance
						if (currentCanidateDistance < closestDistance && currentCanidateDistance <= players [i].GetComponent<PlayerVariables> ().shadowPositions [j].GetComponent<PlayerVariables> ().sphereController.transform.localScale.x / 20f) {
							closestDistance = currentCanidateDistance;
							CanidateChoice = players [i];
						} else if (currentCanidateDistance == closestDistance) {
							CanidateChoice = null;
						}
					}//for shadowpositions
				}
				//distance to the canidate you are checking
				currentCanidateDistance = (this.transform.position - players [i].transform.position).magnitude;
			
				//if that is closer than the previous closestDistance
				if (currentCanidateDistance < closestDistance && currentCanidateDistance <= players [i].GetComponent<PlayerVariables> ().sphereController.transform.localScale.x / 20f) {
					closestDistance = currentCanidateDistance;
					CanidateChoice = players [i];
				} else if (currentCanidateDistance == closestDistance) {
					CanidateChoice = null;
				}
			
			}//for players
		}
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
			selectedTexture = CanidateChoice.GetComponent<PlayerVariables>().selectedTexture;
			unselectedTexture = CanidateChoice.GetComponent<PlayerVariables>().unselectedTexture;
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
		holdingText = UIController.visualText.text;
		UIController.alterTextBox ("Money: " + money + "\nVotes: " + votes);
	}

	/// <summary>
	/// Raises the mouse exit event.
	/// Used to unselect the voter and put the text back to what it was.
	/// Brian Mah
	/// </summary>
	void OnMouseExit(){
		ToggleSelected ();
		UIController.alterTextBox (holdingText);
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


}
