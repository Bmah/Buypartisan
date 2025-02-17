﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerVariables : MonoBehaviour {

	//Transform is already built-in

	//holds the number of different policies that I'm going to code (Alex Jungroth)
	private const int policyLimit = 4;
    //public bool UsingModel;
    [HideInInspector]
    public int PlayerTag = 0;
    public int money = 100;
    public int votes = 0;
	public int sphereSize = 4;

	//holds the number of victory points the player has (Alex Jungroth)
    [HideInInspector]
	public int victoryPoints = 0;

	//holds the alignment for forming coalitions  (Alex Jungroth)
	//1 = going alone, 2 = coalition A, 3 = coalition B
    [HideInInspector]
	public int alignment = 1;

	//holds the player's political party name (Alex Jungroth)
	public string politicalPartyName;

    //holds the player's number (Alex Jungorth)
    [HideInInspector]
    public int playerNumber = 0;

	//holds the player's choice of policy when they get elected (Alex Jungroth)
	//= 0 means no policy was chosen >= 1 means a certain policy was picked
    [HideInInspector]
	public int chosenPolicy = 0;

	//holds the modifier for the cost of actions (Alex Jungroth)
    [HideInInspector]
	public float actionCostModifier = 0;

    //holds the text for each of the parites policies
    //[TextArea]
    //public string[] policiesText = new string[policyLimit];
    //POLICIES ARE BEING DISABLED ENTIRELY FOR NOW
    private Renderer playerRenderer;
    private Renderer sphereRenderer;
    private bool selected = false;

    public Material PlayerSelectedTexture;
	public Material PlayerUnselectedTexture;
	public Material PlayerTransparentTexture;
	public GameObject SphereObject;
	public Color transparentColor;

	//holds whether or not this instance of the player is a shadow position (Alex Jungroth)
    [HideInInspector]
	public bool isShadowPosition = false;

	//holds all of the shadow postions (Alex Jungroth)
    [HideInInspector]
	public List<GameObject> shadowPositions = new List<GameObject>();

	//Brian Mah
	//code for voter's canidate color
	private GameController gameController;
	private Vector3 prevPosition;
	private float prevSphereSize;
    

	void Start () {
        //Brian Mah
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        if (gameController == null)
        {
            Debug.LogError("Could not find the Game controller");
        }

        //makes sure all of the players shadowPositions lists are empty at the start of the program (Alex Jungroth)
        this.shadowPositions.Clear ();

        //If there are no unique parties then the parties will be renamed to "Apple Pie" and there sphere size set to 5 (Alex Jungroth
        //if(gameController.uniqueParties == false)
        //{
        //    politicalPartyName = "Apple Pie";
        //    sphereSize = 5;
        //}//if

        //gets the players render (Alex Jungroth)
        //if (!UsingModel)
		playerRenderer = this.transform.GetChild(1).gameObject.GetComponent<Renderer> ();
        //else
        //    playerRenderer = this.transform.GetChild(1).transform.GetChild(1).gameObject.GetComponent<Renderer>();
        //sets up the spheres for the players (Daniel Schlesinger)
        sphereSize = 10 * sphereSize;
		SphereObject = this.gameObject.transform.GetChild (0).gameObject;
		sphereRenderer = SphereObject.GetComponent<Renderer> ();
		transparentColor = sphereRenderer.material.color;
		transparentColor.a = 0.2f;
		sphereRenderer.material.SetColor ("_Color", transparentColor);
		SphereObject.transform.localScale = new Vector3 (sphereSize, sphereSize, sphereSize);

		//resets the sphere size (Alex Jungroth)
		sphereSize /= 10;
		
		//Brian Mah
		//previous position initialization
		prevPosition = this.transform.position;
		prevSphereSize = SphereObject.transform.localScale.x;

		//an initial check for a player is spawned in (Alex Jungroth)
		//gameController.UpdateVoterCanidates();

		//playerRenderer = this.GetComponent<Renderer>();
	//	GameObject sphere = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		//sphere.transform.position = this.transform.position;
		//sphere.transform.SetParent (this);
       
	}

	void Update () {
		//checks if the position has changed from previous update
		if ((prevPosition != this.transform.position || prevSphereSize != SphereObject.transform.localScale.x) && (gameController.IsActionState))
		{
			gameController.UpdateVoterCanidates ();
		}// if position is not the previous position
		prevPosition = this.transform.position;
		prevSphereSize = SphereObject.transform.localScale.x;
	}

	/// <summary>
	/// Toggles whether or not the Voter is selected.
	/// Brian Mah
	/// </summary>
	public void ToggleSelected(){
		if (selected)
        {
			selected = false;
			playerRenderer.material = PlayerUnselectedTexture;
		}
		else
        {
			selected = true;
			playerRenderer.material = PlayerSelectedTexture;
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

			//This was interfering with the parites money being updated after an action (Alex Jungroth)
			gameController.GetComponent<GameController>().popUpTVScript.GetComponent<PopUpTVScript>().SetPopupTextBox("Money: " + money + "\nVotes: " + votes +
				"\nPoints: " + victoryPoints);
			gameController.GetComponent<GameController>().popUpTVScript.GetComponent<PopUpTVScript>().ShortWaitForUIToolTip();

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
		if (!isShadowPosition)
        {
			ToggleSelected ();

			//This was interfering with the parites money being updated after an action (Alex Jungroth)
			gameController.GetComponent<GameController> ().popUpTVScript.GetComponent<PopUpTVScript> ().ExitUIToolTip ();
		}
	}

	/// <summary>
	/// Gets whether or not the voter is selected.
	/// Brian Mah
	/// </summary>
	/// <returns><c>true</c>, if selected was gotten, <c>false</c> otherwise.</returns>
	public bool GetSelected()
    {
		return selected;
	}
}