//Alex Jungroth
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
//using GameController;

public class UI_Script : MonoBehaviour {

	private GameController controller;

	public int gridSize;

	//holds the main text box object
	public GameObject mainTextBox;

	//holds the main text box's text
	public Text visualText;

	//used for the increment and decrement of X,Y, and Z
	Vector3 ppMove = Vector3.zero;

	//holds the buttons for player placement as game objects
	public GameObject xPlusButton;
	public GameObject xMinusButton;
	public GameObject yPlusButton;
	public GameObject yMinusButton;
	public GameObject zPlusButton;
	public GameObject zMinusButton;
	public GameObject confirmButton;

	//holds the button for displaying player's stats
	public GameObject displayStatsButton;

	//holds the Action Buttons by the tag ActionButton
	public GameObject[] ActionButtonObject;

	//holds the Turn Manager prefab
	public PlayerTurnsManager actionManager;

	//holds the current Player
	public GameObject[] currentPlayerPrefab;

	//holds the current player's indexing number
	private int currentPlayer = 0; 

	//holds the current player's money
	private int currentPlayerMoney = 0;

	//holds the current player's votes
	private int currentPlayerVotes = 0;

	//holds a string that will update the main textbox
	private string currentPlayerStats;

	//holds a integer that will be used to display which player's turn it is
	private int actualTurn = 0;

	// Use this for initialization
	void Start () {
		controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

		//gets the text box
		mainTextBox = GameObject.Find ("Game Announcements TBox");

		//gets the actual text you see
		visualText = mainTextBox.GetComponent<Text>();

		//tests the above two lines of code
		visualText.text = "Testing";

		//gets the buttons for player placement
		xPlusButton = GameObject.Find ("+X");
		xMinusButton = GameObject.Find ("-X");
		yPlusButton = GameObject.Find ("+Y");
		yMinusButton = GameObject.Find ("-Y");
		zPlusButton = GameObject.Find ("+Z");
		zMinusButton = GameObject.Find ("-Z");
		confirmButton = GameObject.Find ("Confirm");

		//gets the button for displaying the players stats
		displayStatsButton = GameObject.FindGameObjectWithTag ("DisplayStats");

		//gets the Action Buttons
		ActionButtonObject = GameObject.FindGameObjectsWithTag ("ActionButton");

		//disables the Action Buttons at the start
		for(int i = 0; i < 10; i++)
		{
			ActionButtonObject[i].SetActive(false);
		}

		//disables the display stats button at the start
		displayStatsButton.SetActive (false);

		//gets the current player
		currentPlayerPrefab = controller.GetComponent<GameController> ().players;

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	/// These six functions increment and decrement
	/// currentPlayer's X,Y, and Z variables.
	/// 
	/// PP = Player Placement
	/// </summary>
	
	public void PP_X_Plus()
	{
		if (controller.currentPlayer.transform.position.x < gridSize - 1) {
			ppMove = controller.currentPlayer.transform.position;
			ppMove = ppMove + Vector3.right;
			controller.currentPlayer.transform.position = ppMove;
			//Debug.Log ("X+ Clicked");

			//another test of the new text box code
			alterTextBox ("X+ Clicked");
		}
	}
	
	public void PP_X_Minus()
	{
		if (controller.currentPlayer.transform.position.x > 0) {
			ppMove = controller.currentPlayer.transform.position;
			ppMove = ppMove + Vector3.left;
			controller.currentPlayer.transform.position = ppMove;
			//Debug.Log ("X- Clicked");

			//another test of the new text box code
			alterTextBox ("X- Clicked");
		}
	}
	
	public void PP_Y_Plus()
	{
		if (controller.currentPlayer.transform.position.y < gridSize - 1) {
			ppMove = controller.currentPlayer.transform.position;
			ppMove = ppMove + Vector3.up;
			controller.currentPlayer.transform.position = ppMove;
			//Debug.Log ("Y+ Clicked");

			//another test of the new text box code
			alterTextBox ("Y+ Clicked");
		}
	}
	
	public void PP_Y_Minus()
	{
		if (controller.currentPlayer.transform.position.y > 0) {
			ppMove = controller.currentPlayer.transform.position;
			ppMove = ppMove + Vector3.down;
			controller.currentPlayer.transform.position = ppMove;
			//Debug.Log ("Y- Clicked");

			//another test of the new text box code
			alterTextBox ("Y- Clicked");
		}
	}
	
	public void PP_Z_Plus()
	{
		if (controller.currentPlayer.transform.position.z < gridSize - 1) {
			ppMove = controller.currentPlayer.transform.position;
			ppMove = ppMove + Vector3.forward;
			controller.currentPlayer.transform.position = ppMove;
			//Debug.Log ("Z+ Clicked");

			//another test of the new text box code
			alterTextBox ("Z+ Clicked");
		}
	}
	
	public void PP_Z_Minus()
	{
		if (controller.currentPlayer.transform.position.z > 0) {
			ppMove = controller.currentPlayer.transform.position;
			ppMove = ppMove + Vector3.back;
			controller.currentPlayer.transform.position = ppMove;
			//Debug.Log ("Z- Clicked");

			//another test of the new text box code
			alterTextBox ("Z- Clicked");
		}
	}

	//calls the confirm function
	public void PP_Confirm()
	{
		controller.playerConfirmsPlacment = true;
		//Debug.Log ("Confirm Clicked");

		//another test of the new text box code
		alterTextBox ("Confirm Clicked");

		//quick test of the disabling player placement buttons
		//disablePPButtons ();
	}

	public void alterTextBox(string inputText)
	{
		visualText.text = inputText;

	}

	public void disablePPButtons()
	{
		Debug.Log("Pressed");
		//disables the Player Placement buttons
		xPlusButton.SetActive (false);
		xMinusButton.SetActive (false);
		yPlusButton.SetActive (false);
		yMinusButton.SetActive (false);
		zPlusButton.SetActive (false);
		zMinusButton.SetActive (false);
		confirmButton.SetActive (false);
	}

	public void toggleActionButtons()
	{
		//this enables the action buttons
		//Note: could recode it so that specific 
		//buttons can be toggled on or off
		for(int i = 0; i < 10; i++)
		{
			ActionButtonObject[i].SetActive(true);
		}

		//also enables the display button
		displayStatsButton.SetActive(true);
	}

	public void displayPlayerStats()
	{
		//gets the current player
		currentPlayer = controller.GetComponent <GameController> ().currentPlayerTurn;

		//resets acutalTurn
		actualTurn = 0;

		//gets the current player plus 1 to make the first player be player 1 and not player 0
		actualTurn = currentPlayer + 1;
		
		//gets the current players money
		currentPlayerMoney = currentPlayerPrefab[currentPlayer].GetComponent<PlayerVariables> ().money; 
		
		//gets the current players votes
		currentPlayerVotes = currentPlayerPrefab[currentPlayer].GetComponent<PlayerVariables> ().votes;
		
		//compiles the players stats into one string
		currentPlayerStats = "Player "+ actualTurn.ToString() + " has " + currentPlayerMoney.ToString() + 
			" dollar(s) and " + currentPlayerVotes.ToString() + " vote(s).";
		
		//updates the text box with player 1's stats
		alterTextBox (currentPlayerStats);
	}

	public void activateActionButton0()
	{
		actionManager.chosenAction = 0;
		actionManager.actionConfirmed = true;

	}

	public void activateActionButton1()
	{
		actionManager.chosenAction = 1;
		actionManager.actionConfirmed = true;
		//alterTextBox ("Test Action 1");
	}

	public void activateActionButton2()
	{
		actionManager.chosenAction = 2;
		actionManager.actionConfirmed = true;
		
	}

	public void activateActionButton3()
	{
		actionManager.chosenAction = 3;
		actionManager.actionConfirmed = true;
		
	}

	public void activateActionButton4()
	{
		actionManager.chosenAction = 4;
		actionManager.actionConfirmed = true;
		
	}

	public void activateActionButton5()
	{
		actionManager.chosenAction = 5;
		actionManager.actionConfirmed = true;
		
	}

	public void activateActionButton6()
	{
		actionManager.chosenAction = 6;
		actionManager.actionConfirmed = true;
		
	}

	public void activateActionButton7()
	{
		actionManager.chosenAction = 7;
		actionManager.actionConfirmed = true;
		
	}

	public void activateActionButton8()
	{
		actionManager.chosenAction = 8;
		actionManager.actionConfirmed = true;
		
	}

	public void activateActionButton9()
	{
		actionManager.chosenAction = 9;
		actionManager.actionConfirmed = true;
		
	}
}