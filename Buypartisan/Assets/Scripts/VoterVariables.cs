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
	private GameController ControllerSquared;//used for power calls
	private UI_Script UIController;

	string holdingText;

	//These variables hold a voters resistance to being moved (Alex Jungroth)
	public float baseResistance = 0;
	public float xPlusResistance = 0;
	public float xMinusResistance = 0;
	public float yPlusResistance = 0;
	public float yMinusResistance = 0;
	public float zPlusResistance = 0;
	public float zMinusResistance = 0;

	void Start () {
		voterRenderer = this.GetComponent<Renderer>();
		Coll = this.GetComponent<Collider> ();
		powerType = 1; //hardcoded for testing suppression, make sure to remove when code in place for buttons assigning
		ControllerSquared = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		UIController = GameObject.FindGameObjectWithTag ("UI_Controller").GetComponent<UI_Script> ();
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
				ControllerSquared.PowerCall(powerType);
				ToggleSelected();

			}
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
