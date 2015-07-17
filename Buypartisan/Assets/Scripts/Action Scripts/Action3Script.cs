//Alex Jungroth
//This controls shadow position placement
using UnityEngine;
using System.Collections;

public class Action3Script : MonoBehaviour {

	//This is from ActionScriptTemplate.cs and Action1Script.cs
    public string actionName = "Move SP";
	public int baseCost = 200;
	public int totalCost = 0;
    public float costMultiplier = 1.0f;

	public GameObject gameController; //this is the game controller variable. It is obtained from the PlayerTurnsManager
	public GameObject inputManager; //this is the input manager varibale. Obtained from the PlayerTurnManager
	private GameObject[] players; //array which houses the players. Obtained from the Game Controller
	public GameObject uiController; //this is the UI controller variable. Obtained from the scene
    private GameObject visualAid;
	//Brian Mah
	private RandomEventControllerScript eventController;

	private int currentPlayer; //this variable finds which player is currently using his turn.

	private float distance;

	private Vector3 originalPosition; //this will save the original position that the player started at.
	private bool chosenPositionConfirmed = false; //final position has been chosen and confirmed.

	//these are to check which buttons have been pressed.
	[System.NonSerialized]
	public bool xPlusButton = false;
	[System.NonSerialized]
	public bool xMinusButton = false;
	[System.NonSerialized]
	public bool yPlusButton = false;
	[System.NonSerialized]
	public bool yMinusButton = false;
	[System.NonSerialized]
	public bool zPlusButton = false;
	[System.NonSerialized]
	public bool zMinusButton = false;
	[System.NonSerialized]
	public bool confirmButton = false;
	[System.NonSerialized]
	public bool cancelButton = false;

	//This is from game controller

	//holds the numbe of spawned players
	private int playersSpawned;

	private bool legalShadowPosition = true; //bool for checking if player is done

	//These are my variables

	//holds a pontential position that shadowPostion might be spawned in
	private Vector3 testPosition;

	//holds a a pontntial position that has passed some tests
	private Vector3 semiTestedPosition;

	//holds a primative cube to mark a potential position
	private GameObject marker;

	//holds the shadow position before it is added to the shadow positions list
	//so I can change its transparency
	private GameObject shadowPosition;

	//holds the shadow postion's renderer
	private Renderer shadowRenderer;

	//holds the player's renderer
	private Renderer playerRenderer;

	//holds the orignial color of the shadow position
	private Color transparentColor;

	//holds the player's shadow position sphere's color
	private Color transparentSphereColor;

	//holds the player's shadow position sphere's renderer
	private Renderer playerSphereRenderer;

	//holds the player's shadow postion sphere's local scale
	private Vector3 playerShadowSphereScale;

	//holds the number needed for this action to succeed (Alex Jungroth)
	public float successRate = 0.4f;

	// Use this for initialization
	void Start () {
		gameController = GameObject.FindWithTag ("GameController");
		inputManager = GameObject.FindWithTag ("InputManager");
		uiController = GameObject.Find ("UI Controller");
        visualAid = GameObject.FindWithTag("VisualAidManager");

		if (gameController != null) {
			//			voters = gameController.GetComponent<GameController> ().voters;
			players = gameController.GetComponent<GameController> ().players;
			eventController = gameController.GetComponent<GameController> ().randomEventController;
		} else {
			Debug.Log ("Failed to obtain voters and players array from Game Controller");
		}

		//Get's whose turn it is from the gameController. Then checks if he has enough money to perform the action
		currentPlayer = gameController.GetComponent<GameController> ().currentPlayerTurn;
		costMultiplier = this.transform.parent.GetComponent<PlayerTurnsManager> ().costMultiplier;

		//gets the original postion of the player who is spawning a shadow positon
		originalPosition = players[currentPlayer].transform.position;
		
		//sets the semiTestedPosition equal originalPosition so that semiTestedPosition will have a value when confirm is pressed
		semiTestedPosition = originalPosition;
		
		//gets the number of spawned players
		playersSpawned = gameController.GetComponent<GameController> ().playersSpawned;
		
		//Disables the Action UI buttons
		uiController.GetComponent<UI_Script>().disableActionButtons();
		
		//activates the buttons for Action 3
		uiController.GetComponent<UI_Script> ().activateAction3UI ();
		
		//creates a primitive cube to show potential positions on screen
		marker = GameObject.CreatePrimitive(PrimitiveType.Cube);
		
		//initializes the marker to the original position of the player spawning a shadow positon
		marker.transform.position = originalPosition;
		
		//scales the marker so its not larger than everything else on the grid
		marker.transform.localScale = new Vector3(0.099f, 0.099f, 0.099f);

		//scales this prefab so its not larger than everything else on the grid
		transform.localScale = new Vector3 (0.098f, 0.098f, 0.098f);

		//see ActionScriptTemplate.cs for my explination on this change (Alex Jungroth)
		
		if (players [currentPlayer].GetComponent<PlayerVariables> ().money >= (baseCost * costMultiplier)) {

			totalCost = (int)(baseCost * costMultiplier);
            visualAid.GetComponent<VisualAidAxisManangerScript>().Attach(this.gameObject);
		}
        else
        {
			Debug.Log ("Current Player doesn't have enough money to make this action.");
			uiController.GetComponent<UI_Script>().toggleActionButtons();
			Destroy (marker);
			Destroy(gameObject);
        }
	}
	
	// Update is called once per frame
	void Update () {

		//ends the action if the cancel button is pressed (Alex Jungroth)
		if (cancelButton) 
		{
			//handles early canceling(Alex Jungroth)
            visualAid.GetComponent<VisualAidAxisManangerScript>().Detach();
			uiController.GetComponent<UI_Script>().toggleActionButtons();
			Destroy (marker);
			Destroy(gameObject);
		}

		//These below if statements check if the player has pressed a button down to move the character.
		//Right now, it makes sure that the player can only move up, down, left, right, forward, and backward from its original postion.
		//It also checks to make sure the player doesn't move outside the grid.
		//When we figure out the GUI system, we can change the inputs to then be button presses.

		//Tests to make sure that the shadow position does not overlap another player or
		//another shadow position, and that the shadow position does not move more than 
		//one space from the player in a given direction
		if  (xPlusButton) {

			//resets test position to the original position
			testPosition = originalPosition;
			testPosition.x = originalPosition.x + 1;

			if (testPosition.x <= (gameController.GetComponent<GameController>().gridSize - 1))
			{	
				if(TestShadowPosition(testPosition)) 
				{
					semiTestedPosition = originalPosition + new Vector3(1,0,0);

					Debug.Log ("Cost to place a shadow position: $" + totalCost + ".");

					//updates the positon of the marker
					marker.transform.position = testPosition;

					//updates the postion of the prefab, which the axis arrows will follow
					transform.position = testPosition;

				}
				else
				{
					Debug.Log ("Not a valid placement.");
				}
			}
			else
			{
				Debug.Log ("Off the grid.");
			}

			xPlusButton = false;
		}
		if (xMinusButton) {

			//resets test position to the original position
			testPosition = originalPosition;
			testPosition.x = originalPosition.x - 1;

			if (testPosition.x >= 0)
			{
				if(TestShadowPosition(testPosition))
				{
					semiTestedPosition = originalPosition + new Vector3(-1,0,0);

					Debug.Log ("Cost to place a shadow position: $" + totalCost + ".");

					//updates the positon of the marker
					marker.transform.position = testPosition;

					//updates the postion of the prefab, which the axis arrows will follow
					transform.position = testPosition;

				}
				else
				{
					Debug.Log ("Not a valid placement.");
				}
			}
			else
			{
				Debug.Log ("Off the grid.");
			}

			xMinusButton = false;
		}
		if (zMinusButton) {

			//resets test position to the original position
			testPosition = originalPosition;
			testPosition.z = originalPosition.z - 1;

			if (testPosition.z >= 0)
			{
				if(TestShadowPosition(testPosition))
				{
					semiTestedPosition = originalPosition + new Vector3(0,0,-1);

					Debug.Log ("Cost to place a shadow position: $" + totalCost + ".");

					//updates the positon of the marker
					marker.transform.position = testPosition;

					//updates the postion of the prefab, which the axis arrows will follow
					transform.position = testPosition;
				
				}
				else
				{
					Debug.Log ("Not a valid placement.");
				}
			}
			else
			{
				Debug.Log ("Off the grid.");
			}

			zMinusButton = false;
		}
		if (zPlusButton) {

			//resets test position to the original position
			testPosition = originalPosition;
			testPosition.z = originalPosition.z + 1;

			if (testPosition.z <= (gameController.GetComponent<GameController>().gridSize - 1))
			{
				if(TestShadowPosition(testPosition))
				{
					semiTestedPosition = originalPosition + new Vector3(0,0,1);

					Debug.Log ("Cost to place a shadow position: $" + totalCost + ".");
					
					//updates the positon of the marker
					marker.transform.position = testPosition;

					//updates the postion of the prefab, which the axis arrows will follow
					transform.position = testPosition;

				}
				else
				{
					Debug.Log ("Not a valid placement.");
				}
			}
			else
			{
				Debug.Log ("Off the grid.");
			}
		
			zPlusButton = false;
		}
		if (yPlusButton) {

			//resets test position to the original position
			testPosition = originalPosition;
			testPosition.y = originalPosition.y + 1;

			if (testPosition.y <= (gameController.GetComponent<GameController>().gridSize - 1))
			{	
				if(TestShadowPosition(testPosition))
				{
					semiTestedPosition = originalPosition + new Vector3(0,1,0);

					Debug.Log ("Cost to place a shadow position: $" + totalCost + ".");

					//updates the positon of the marker
					marker.transform.position = testPosition;

					//updates the postion of the prefab, which the axis arrows will follow
					transform.position = testPosition;

				}
				else
				{
					Debug.Log ("Not a valid placement.");
				}
			}
			else
			{
				Debug.Log ("Off the grid.");
			}

			yPlusButton = false;
		}
		if (yMinusButton) {

			//resets test position to the original position
			testPosition = originalPosition;
			testPosition.y = originalPosition.y - 1;

			if (testPosition.y >= 0)
			{	
				if(TestShadowPosition(testPosition))
				{
					semiTestedPosition = originalPosition + new Vector3(0,-1,0);

					Debug.Log ("Cost to place a shadow position: $" + totalCost + ".");

					//updates the positon of the marker
					marker.transform.position = testPosition;

					//updates the postion of the prefab, which the axis arrows will follow
					transform.position = testPosition;

				}
				else
				{
					Debug.Log ("Not a valid placement.");
				}

			}
			else
			{
				Debug.Log ("Off the grid.");
			}

			yMinusButton = false;
		}
		
		//Right Now, if you press the confirm button, it confirms the chosen position.
		//You can only confirm the position if it isn't the exact same position you started at, or you are not sharing a position that another player is in
		//You also must have enough money to move to that position.
		if (confirmButton) {
			for (int i = 0; i < players.Length; i++) {
				if (i != currentPlayer && players[i].transform.position != semiTestedPosition && semiTestedPosition != originalPosition) {
					if (totalCost > players[currentPlayer].GetComponent<PlayerVariables>().money) {
						Debug.Log ("You don't have enough money to move to this spot!");
					} 
					else 
					{
						chosenPositionConfirmed = true;
					}
				}
			}
			confirmButton = false;
		}
		
		if (chosenPositionConfirmed) {

			//checks to see if the power succeeded (Alex Jungroth)
			if(Random.value >= successRate)
			{
				//instantiates a new instance of player that will be the shadow postion and sets it position to the player who spawned
				shadowPosition = Instantiate(gameController.GetComponent<GameController>().playerTemplate,semiTestedPosition, Quaternion.identity) as GameObject;

				//gets the players Renderer
				playerRenderer = players[currentPlayer].GetComponent<Renderer>();

				//gets the shadows position's renderer
				shadowRenderer = shadowPosition.GetComponent<Renderer>();

				//sets the shadow position's color equal to the player's render color
				shadowRenderer.material.color = playerRenderer.material.color;

				//makes the players shadow positions transparent without changing their color
				transparentColor = shadowRenderer.material.color;
				transparentColor.a = 0.5f;
				shadowRenderer.material.SetColor("_Color", transparentColor);  

				//makes the shadow positions sphere of influence the same color as the players sphere of inclufence
				playerSphereRenderer = players[currentPlayer].GetComponent<PlayerVariables>().sphereController.GetComponent<Renderer>();
				transparentSphereColor = playerSphereRenderer.material.color;
				transparentSphereColor.a = 0.2f;
				shadowPosition.GetComponent<PlayerVariables>().sphereController.GetComponent<Renderer>().material.SetColor("_Color", transparentSphereColor);

				//sets the shadow position's sphere of influence equal to the player's sphere of influence (doesn't work for some reason)
				//playerShadowSphereScale = players[currentPlayer].GetComponent<PlayerVariables>().sphereController.transform.localScale;

				//shadowPosition.GetComponent<PlayerVariables>().sphereController.transform.localScale = playerShadowSphereScale;

				//adds the shadow position to the players array list of shadowpositions
				players[currentPlayer].GetComponent<PlayerVariables>().shadowPositions.Add(shadowPosition);
			}

			//removes the marker
			Destroy(marker);

			EndAction ();
		}

	}
	
	void EndAction() {
        visualAid.GetComponent<VisualAidAxisManangerScript>().Detach();
		uiController.GetComponent<UI_Script>().toggleActionButtons();
		this.transform.parent.GetComponent<PlayerTurnsManager> ().IncreaseCostMultiplier();
        players[currentPlayer].GetComponent<PlayerVariables>().money -= totalCost; // Money is subtracted
		//puts the current player and the event number into the action counter of the event controller
		//Brian Mah
		eventController.actionCounter [gameController.GetComponent<GameController>().currentPlayerTurn] [3]++; // the second number should be the number of the action!
		Destroy(gameObject);
	}

	bool TestShadowPosition(Vector3 potentialShadowPosition){

		//temporary holds a shadow position
		GameObject tempShadowPosition;

		//resets playerConifrmsPlacement
		legalShadowPosition = true;

		//checks the shadow position against all of the previous players to ensure that they don't overlap
		for(int i = 0; i < playersSpawned && legalShadowPosition; i++)
		{
			//we don't want to compare the current player to the new shadow postion as it will always fail that test
			if (players[i].GetComponent<PlayerVariables>().transform.position != originalPosition
			    && potentialShadowPosition == players[i].GetComponent<PlayerVariables>().transform.position)
			{//if they are on the same spot
					legalShadowPosition = false;
			}//if
			
			//if the count of any array list is zero, than that means there is nothing in it
			//and it should not be looked at as it will cause a runtime error
			if(players[i].GetComponent<PlayerVariables>().shadowPositions.Count > 0)
			{
				//checks the shadow position against all other shadow postions owned by a single player to ensure that they don't overlap
				for(int j = 0; j < players[i].GetComponent<PlayerVariables>().shadowPositions.Count && legalShadowPosition; j++)
				{
					//gets the next shadow postion in the arraylist
					tempShadowPosition = players[i].GetComponent<PlayerVariables>().shadowPositions[j];

					if(potentialShadowPosition == tempShadowPosition.transform.position)
					{
						//if two shadow positions are on the same spot
						legalShadowPosition = false;
					}//if
				}//for
			}//if
		}//for

		//if the player placment is legal
		return legalShadowPosition;

	}//SpawnPlayer
}