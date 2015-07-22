using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerVariables : MonoBehaviour {

	//Transform is already built-in

	//holds the number of different policies that I'm going to code (Alex Jungroth)
	private const int policyLimit = 4;

    public int pTag = 0;
    public int money = 100;
    public int votes = 0;
	public int sphereSize = 4;

	//holds the number of victory points the player has (Alex Jungroth)
	public int victoryPoints = 0;

	//holds the alignment for forming coalitions  (Alex Jungroth)
	//0 = going alone, 1 = coalition A, 2 = coalition B
	public int alignment = 0;

	//holds the player's political party (Alex Jungroth)
	public string politicalPartyName;

	//holds the player's choice of policy when they get elected (Alex Jungroth)
	//= 0 means no policy was chosen >= 1 means a certain policy was picked
	public int chosenPolicy = 0;

	//holds the modifier for the money players get from voters (Alex Jungroth)
	public int revenueModifier = 0;

	//holds the modifier for the cost of actions (Alex Jungroth)
	public int actionCostModifier = 0;

	//holds the text for each of the parites policies
	public string[] policiesText = new string[policyLimit];

	public Material selectedTexture;
	public Material unselectedTexture;
	public Material transparentTexture;
	private Renderer playerRenderer;
	public Renderer sphereRenderer;
	public GameObject sphereController;
	public Color transparentColor;
	private UI_Script UIController;

	private bool selected = false;
	string holdingText;

	//holds whether or not this instance of the player is a shadow position (Alex Jungroth)
	public bool isShadowPosition = false;

	//holds all of the shadow postions (Alex Jungroth)
	public List<GameObject> shadowPositions = new List<GameObject>();

	//Brian Mah
	//code for voter's canidate color
	private GameController gameController;
	private Vector3 prevPosition;
	private float prevSphereSize;

	void Start () {
        
		//makes sure all of the players shadowPositions lists are empty at the start of the program (Alex Jungroth)
		this.shadowPositions.Clear ();

		//gets the players render (Alex Jungroth)
		playerRenderer = this.transform.GetChild (1).transform.GetChild(1).gameObject.GetComponent<Renderer> ();

		//gets the UI Controller script (Alex Jungroth)
		UIController = GameObject.FindGameObjectWithTag ("UI_Controller").GetComponent<UI_Script> ();

		//sets up the spheres for the players (Daniel Schlesinger)
		sphereSize = 10 * sphereSize;
		sphereController = this.gameObject.transform.GetChild (0).gameObject;
		sphereRenderer = sphereController.GetComponent<Renderer> ();
		transparentColor = sphereRenderer.material.color;
		transparentColor.a = 0.2f;
		sphereRenderer.material.SetColor ("_Color", transparentColor);
		sphereController.transform.localScale = new Vector3 (sphereSize, sphereSize, sphereSize);

		//resets the sphere size (Alex Jungroth)
		sphereSize /= 10;
		
		//Brian Mah
		//previous position initialization
		prevPosition = this.transform.position;
		prevSphereSize = sphereController.transform.localScale.x;
		gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		if (gameController == null) {
			Debug.LogError("Could not find the Game controller");
		}

		//an initial check for a player is spawned in (Alex Jungroth)
		//gameController.UpdateVoterCanidates();

		//playerRenderer = this.GetComponent<Renderer>();
	//	GameObject sphere = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		//sphere.transform.position = this.transform.position;
		//sphere.transform.SetParent (this);
       
	}

	void Update () {
		//checks if the position has changed from previous update
		if ((prevPosition != this.transform.position || prevSphereSize != sphereSize) && (gameController.isActionTurns))
		{
			gameController.UpdateVoterCanidates ();
		}// if position is not the previous position
		prevPosition = this.transform.position;
		prevSphereSize = sphereController.transform.localScale.x;
	}

	/// <summary>
	/// Toggles whether or not the Voter is selected.
	/// Brian Mah
	/// </summary>
	public void ToggleSelected(){
		if (selected) {
			selected = false;
			playerRenderer.material = unselectedTexture;
		}
		else {
			selected = true;
			playerRenderer.material = selectedTexture;
		}
	}
	/// <summary>
	/// Raises the mouse enter event.
	/// Used to select the voter and display their stats on the textbox
	/// Brian Mah
	/// </summary>
	void OnMouseEnter()
	{
		//shadow positions should not be moused over (Alex Jungroth)
		if(!isShadowPosition)
		{
			ToggleSelected ();
			holdingText = UIController.visualText.text;
			UIController.alterTextBox ("Money: " + money + "\nVotes: " + votes);
		}
	}
	
	/// <summary>
	/// Raises the mouse exit event.
	/// Used to unselect the voter and put the text back to what it was.
	/// Brian Mah
	/// </summary>
	void OnMouseExit()
	{
		//shadow positions should not be moused over (Alex Jungroth)
		if(!isShadowPosition)
		{
			ToggleSelected ();
			UIController.alterTextBox (holdingText);
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