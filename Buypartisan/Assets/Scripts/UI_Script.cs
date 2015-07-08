//Alex Jungroth
//edits by Brian Mah
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
//using GameController;

public class UI_Script : MonoBehaviour {

	private GameController controller;

    public GameObject playerTurnsManager;

	public int gridSize;

	//holds the main text box object
	public GameObject mainTextBox;

	//holds the main text box's text
	public Text visualText;

    //holds all the action button's texts
    public Text text0, text1, text2, text3, text4, text5, text6, text7, text8, text9;

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
	public GameObject endTurnButton;
	public GameObject leftButton;
	public GameObject rightButton;

	//holds the cancel button
	public GameObject cancelButton;

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

	//this will allow us to change states for some of the buttons, so that when the turn phase begins,
	//the buttons can therefore do something else.
	private bool turnPhase = false;

	//this is to keep track of which action has been chosen.
	private int chosenAction = 0;

	//this is so that this script can communicate with the Action script
	public GameObject instantiatedAction;

	//this allows the UI to talk to the SFX controller to play sounds
	private SFXController sfx;
	public float SFXvolume = 1;

	//this code takes care of the scaling of the scroll rect dynamically.
	//Brian Mah
	public RectTransform rectScrollView;
	public RectTransform rectScrollBar;
	public float scrollViewWidth = 360f;
	public float titleHeight = 114f;
	public float bottomTVHeight = 300f;

	// Use this for initialization
	void Start () {
		controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        playerTurnsManager = GameObject.FindGameObjectWithTag("ActionManager");

		//gets the text box
		mainTextBox = GameObject.Find ("Game Announcements TBox");

		//gets the actual text you see
		visualText = mainTextBox.GetComponent<Text>();

		//tests the above two lines of code
		visualText.text = "Testing";

        // Action button texts
        text0 = GameObject.Find("Text0").GetComponent<Text>();
        text1 = GameObject.Find("Text1").GetComponent<Text>();
        text2 = GameObject.Find("Text2").GetComponent<Text>();
        text3 = GameObject.Find("Text3").GetComponent<Text>();
        text4 = GameObject.Find("Text4").GetComponent<Text>();
        text5 = GameObject.Find("Text5").GetComponent<Text>();
        text6 = GameObject.Find("Text6").GetComponent<Text>();
        text7 = GameObject.Find("Text7").GetComponent<Text>();
        text8 = GameObject.Find("Text8").GetComponent<Text>();
        text9 = GameObject.Find("Text9").GetComponent<Text>();

		//gets the buttons for player placement
		xPlusButton = GameObject.Find ("+X");
		xMinusButton = GameObject.Find ("-X");
		yPlusButton = GameObject.Find ("+Y");
		yMinusButton = GameObject.Find ("-Y");
		zPlusButton = GameObject.Find ("+Z");
		zMinusButton = GameObject.Find ("-Z");
		confirmButton = GameObject.Find ("Confirm");
		endTurnButton = GameObject.Find ("End Turn");
		leftButton = GameObject.Find ("Left");
		rightButton = GameObject.Find ("Right");
		cancelButton = GameObject.Find ("Cancel");

		//gets the button for displaying the players stats
		displayStatsButton = GameObject.FindGameObjectWithTag ("DisplayStats");

		//gets the Action Buttons
		ActionButtonObject = GameObject.FindGameObjectsWithTag ("ActionButton");

		//disables the Action Buttons at the start
		for(int i = 0; i < 10; i++)
		{
			ActionButtonObject[i].SetActive(false);
		}

		//disables the left, right, and end turn buttons
		endTurnButton.SetActive (false);
		leftButton.SetActive (false);
		rightButton.SetActive (false);

		//disables the cancel button
		cancelButton.SetActive (false);

		//disables the display stats button at the start
		displayStatsButton.SetActive (false);

		//gets the current player
		currentPlayerPrefab = controller.GetComponent<GameController> ().players;

		sfx = GameObject.FindGameObjectWithTag ("SFX").GetComponent<SFXController> ();
		if (sfx == null) {
			Debug.LogError ("ButtonScript could not find SFX Controller please add it to the scene.");
		}

        // Initializes cost
        updateCost();


		//set size and position of scrollview and scrollBar
		//Brian Mah
		float scrollViewHeight = Screen.height - (titleHeight + bottomTVHeight);
		float heightToMove = scrollViewHeight - rectScrollBar.sizeDelta.y;
		heightToMove /= 2;
		if (scrollViewHeight < 1f) {
			Debug.LogError("Screen Height not supported");
			scrollViewHeight = 100f;
		}
		rectScrollView.sizeDelta = new Vector2 (scrollViewWidth, scrollViewHeight);
		rectScrollBar.sizeDelta = new Vector2(rectScrollBar.rect.width,scrollViewHeight);
		rectScrollView.anchoredPosition = new Vector2 (rectScrollView.anchoredPosition.x, rectScrollView.anchoredPosition.y - heightToMove);
		rectScrollBar.anchoredPosition = new Vector2 (rectScrollBar.anchoredPosition.x, rectScrollBar.anchoredPosition.y - heightToMove);
		//rectScrollView.position = new Vector3 (rectScrollView.position.x, 0, rectScrollView.position.z);
		//rectScrollBar.position = new Vector3 (rectScrollBar.position.x, 0, rectScrollBar.position.z);
		//Debug.Log ("The Height is: " + rectScrollView.localPosition.y);
		//Debug.Log ("Height move is: " + heightToMove);
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
		if (!turnPhase) {
			if (controller.currentPlayer.transform.position.x < gridSize - 1) {
				ppMove = controller.currentPlayer.transform.position;
				ppMove = ppMove + Vector3.right;
				controller.currentPlayer.transform.position = ppMove;
				//Debug.Log ("X+ Clicked");

				//another test of the new text box code
				alterTextBox ("X+ Clicked");
			}
		} else {
			if (chosenAction == 1) {
				instantiatedAction.GetComponent<Action1Script>().xPlusButton = true;
			}

			if (chosenAction == 2) {
				instantiatedAction.GetComponent<Action2Script>().xPlusButton = true;
			}

			if (chosenAction == 3) {
				instantiatedAction.GetComponent<Action3Script>().xPlusButton = true;
			}

			if (chosenAction == 4) {
				instantiatedAction.GetComponent<Action4Script>().xPlusButton = true;
			}
		}
	}
	
	public void PP_X_Minus()
	{
		if (!turnPhase) {
			if (controller.currentPlayer.transform.position.x > 0) {
				ppMove = controller.currentPlayer.transform.position;
				ppMove = ppMove + Vector3.left;
				controller.currentPlayer.transform.position = ppMove;
				//Debug.Log ("X- Clicked");

				//another test of the new text box code
				alterTextBox ("X- Clicked");
			}
		} else {
			if (chosenAction == 1) {
				instantiatedAction.GetComponent<Action1Script>().xMinusButton = true;
			}

			if (chosenAction == 2) {
				instantiatedAction.GetComponent<Action2Script>().xMinusButton = true;
			}

			if (chosenAction == 3) {
				instantiatedAction.GetComponent<Action3Script>().xMinusButton = true;
			}

			if (chosenAction == 4) {
				instantiatedAction.GetComponent<Action4Script>().xMinusButton = true;
			}
		}
	}
	
	public void PP_Y_Plus()
	{
		if (!turnPhase) {
			if (controller.currentPlayer.transform.position.y < gridSize - 1) {
				ppMove = controller.currentPlayer.transform.position;
				ppMove = ppMove + Vector3.up;
				controller.currentPlayer.transform.position = ppMove;
				//Debug.Log ("Y+ Clicked");

				//another test of the new text box code
				alterTextBox ("Y+ Clicked");
			}
		} else {
			if (chosenAction == 1) {
				instantiatedAction.GetComponent<Action1Script>().yPlusButton = true;
			}

			if (chosenAction == 2) {
				instantiatedAction.GetComponent<Action2Script>().yPlusButton = true;
			}

			if (chosenAction == 3) {
				instantiatedAction.GetComponent<Action3Script>().yPlusButton = true;
			}

			if (chosenAction == 4) {
				instantiatedAction.GetComponent<Action4Script>().yPlusButton = true;
			}
		}
	}
	
	public void PP_Y_Minus()
	{
		if (!turnPhase) {
			if (controller.currentPlayer.transform.position.y > 0) {
				ppMove = controller.currentPlayer.transform.position;
				ppMove = ppMove + Vector3.down;
				controller.currentPlayer.transform.position = ppMove;
				//Debug.Log ("Y- Clicked");

				//another test of the new text box code
				alterTextBox ("Y- Clicked");
			}
		} else {
			if (chosenAction == 1) {
				instantiatedAction.GetComponent<Action1Script>().yMinusButton = true;
			}

			if (chosenAction == 2) {
				instantiatedAction.GetComponent<Action2Script>().yMinusButton = true;
			}

			if (chosenAction == 3) {
				instantiatedAction.GetComponent<Action3Script>().yMinusButton = true;
			}

			if (chosenAction == 4) {
				instantiatedAction.GetComponent<Action4Script>().yMinusButton = true;
			}
		}
	}
	
	public void PP_Z_Plus()
	{
		if (!turnPhase) {
			if (controller.currentPlayer.transform.position.z < gridSize - 1) {
				ppMove = controller.currentPlayer.transform.position;
				ppMove = ppMove + Vector3.forward;
				controller.currentPlayer.transform.position = ppMove;
				//Debug.Log ("Z+ Clicked");

				//another test of the new text box code
				alterTextBox ("Z+ Clicked");
			}
		} else {
			if (chosenAction == 1) {
				instantiatedAction.GetComponent<Action1Script>().zPlusButton = true;
			}

			if (chosenAction == 2) {
				instantiatedAction.GetComponent<Action2Script>().zPlusButton = true;
			}

			if (chosenAction == 3) {
				instantiatedAction.GetComponent<Action3Script>().zPlusButton = true;
			}

			if (chosenAction == 4) {
				instantiatedAction.GetComponent<Action4Script>().zPlusButton = true;
			}
		}
	}
	
	public void PP_Z_Minus()
	{
		if (!turnPhase) {
			if (controller.currentPlayer.transform.position.z > 0) {
				ppMove = controller.currentPlayer.transform.position;
				ppMove = ppMove + Vector3.back;
				controller.currentPlayer.transform.position = ppMove;
				//Debug.Log ("Z- Clicked");

				//another test of the new text box code
				alterTextBox ("Z- Clicked");
			}
		} else {
			if (chosenAction == 1) {
				instantiatedAction.GetComponent<Action1Script>().zMinusButton = true;
			}

			if (chosenAction == 2) {
				instantiatedAction.GetComponent<Action2Script>().zMinusButton = true;
			}

			if (chosenAction == 3) {
				instantiatedAction.GetComponent<Action3Script>().zMinusButton = true;
			}

			if (chosenAction == 4) {
				instantiatedAction.GetComponent<Action4Script>().zMinusButton = true;
			}
		}
	}

	//calls the confirm function
	public void PP_Confirm()
	{
		if (!turnPhase) {
			controller.playerConfirmsPlacment = true;
			//Debug.Log ("Confirm Clicked");

			//another test of the new text box code
			alterTextBox ("Confirm Clicked");

			//quick test of the disabling player placement buttons
			//disablePPButtons ();
		} else {
			if (chosenAction == 0) {
				instantiatedAction.GetComponent<Action0Script>().confirmButton = true;
			}

			if (chosenAction == 1) {
				instantiatedAction.GetComponent<Action1Script>().confirmButton = true;
			}

			if (chosenAction == 2) {
				instantiatedAction.GetComponent<Action2Script>().confirmButton = true;
			}

			if (chosenAction == 3) {
				instantiatedAction.GetComponent<Action3Script>().confirmButton = true;
			}

			if (chosenAction == 4) {
				instantiatedAction.GetComponent<Action4Script>().confirmButton = true;
			}

			if (chosenAction == 5) {
				instantiatedAction.GetComponent<Action5Script>().confirmButton = true;
			}
		}
	}

	//calls the cancel function
	public void Cancel()
	{
		if (!turnPhase) {
			controller.playerConfirmsPlacment = true;
			//Debug.Log ("Cancel Clicked");
			
			//another test of the new text box code
			alterTextBox ("Cancel Clicked");
			
			//quick test of the disabling player placement buttons
			//disablePPButtons ();
		} else {
			if (chosenAction == 0) {
				instantiatedAction.GetComponent<Action0Script>().cancelButton = true;
			}
			
			if (chosenAction == 1) {
				instantiatedAction.GetComponent<Action1Script>().cancelButton = true;
			}
			
			if (chosenAction == 2) {
				instantiatedAction.GetComponent<Action2Script>().cancelButton = true;
			}
			
			if (chosenAction == 3) {
				instantiatedAction.GetComponent<Action3Script>().cancelButton = true;
			}
			
			if (chosenAction == 4) {
				instantiatedAction.GetComponent<Action4Script>().cancelButton = true;
			}

			if (chosenAction == 5) {
				instantiatedAction.GetComponent<Action5Script>().cancelButton = true;
			}
		}
	}

	public void alterTextBox(string inputText)
	{
		visualText.text = inputText;

	}

	public void disablePPButtons()
	{
		//Debug.Log("Pressed");
		//disables the Player Placement buttons and the cancel button
		xPlusButton.SetActive (false);
		xMinusButton.SetActive (false);
		yPlusButton.SetActive (false);
		yMinusButton.SetActive (false);
		zPlusButton.SetActive (false);
		zMinusButton.SetActive (false);
		confirmButton.SetActive (false);
		cancelButton.SetActive (false);
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

		//also enables the end turn button
		endTurnButton.SetActive (true);

		//also sets turnphase to begin
		turnPhase = true;

		//also disables the unneeded buttons
		disablePPButtons ();
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

	public void activateActionButton(int num)
	{
		actionManager.chosenAction = num;
		actionManager.actionConfirmed = true;
		chosenAction = num;
	}

	public void disableActionButtons()
	{
		for(int i = 0; i < 10; i++)
		{
			ActionButtonObject[i].SetActive(false);
		}

		endTurnButton.SetActive (false);
	}

	public void activateEndTurnButton()
	{
		actionManager.endTurnConfirmed = true;
	}

	public void activateAction0UI()
	{
		//leftButton.SetActive (true);
		//rightButton.SetActive (true);
		cancelButton.SetActive (true);
	}

	public void activateAction0UI2()
	{	
		confirmButton.SetActive (true);
		leftButton.SetActive (false);
		rightButton.SetActive (false);

	}

	public void activateAction1UI()
	{
		xPlusButton.SetActive (true);
		xMinusButton.SetActive (true);
		yPlusButton.SetActive (true);
		yMinusButton.SetActive (true);
		zPlusButton.SetActive (true);
		zMinusButton.SetActive (true);
		confirmButton.SetActive (true);
		cancelButton.SetActive (true);
	}

	public void activateAction2UI1()
	{
		disablePPButtons ();
		//leftButton.SetActive (true);
		//rightButton.SetActive (true);
		cancelButton.SetActive (true);
	}

	public void activateAction2UI2()
	{	
		confirmButton.SetActive (true);
		leftButton.SetActive (false);
		rightButton.SetActive (false);
		xPlusButton.SetActive (true);
		xMinusButton.SetActive (true);
		yPlusButton.SetActive (true);
		yMinusButton.SetActive (true);
		zPlusButton.SetActive (true);
		zMinusButton.SetActive (true);
	}

	public void activateAction3UI()
	{
		xPlusButton.SetActive (true);
		xMinusButton.SetActive (true);
		yPlusButton.SetActive (true);
		yMinusButton.SetActive (true);
		zPlusButton.SetActive (true);
		zMinusButton.SetActive (true);
		confirmButton.SetActive (true);
		cancelButton.SetActive (true);
	}

	public void activateAction4UI()
	{
		leftButton.SetActive (false);
		rightButton.SetActive (false);
		xPlusButton.SetActive (true);
		xMinusButton.SetActive (true);
		yPlusButton.SetActive (true);
		yMinusButton.SetActive (true);
		zPlusButton.SetActive (true);
		zMinusButton.SetActive (true);
		confirmButton.SetActive (true);
		cancelButton.SetActive (true);
	}

	public void activateAction5UI() {
		confirmButton.SetActive (true);
		cancelButton.SetActive (true);
	}

	public void leftButtonClicked()
	{
		if (chosenAction == 2)
			instantiatedAction.GetComponent<Action2Script> ().leftButton = true;
		if (chosenAction == 0)
			instantiatedAction.GetComponent<Action0Script> ().leftButton = true;
	}

	public void rightButtonClicked()
	{
		if (chosenAction == 2)
			instantiatedAction.GetComponent<Action2Script> ().rightButton = true;
		if (chosenAction == 0)
			instantiatedAction.GetComponent<Action0Script> ().rightButton = true;
	}

	/// <summary>
	/// Sound Effect player
	/// Brian Mah
	/// </summary>
	public void PlayMouseOverSound(){
		sfx.PlayAudioClip (0, 0, SFXvolume);
	}

	/// <summary>
	/// Plays the confirm sound.
	/// Brian Mah
	/// </summary>
	public void PlayConfirmSound(){
		sfx.PlayAudioClip (1, 0, SFXvolume);
	}

    // This updates the visual cost on each action button. It shows default costs (including multiplier! =D) if no actions are spawned; if an action is spawned, then it shows the updated cost instead!
    // THIS IS LABOROUS CODE because it won't let me set the array in declaration on top. Someone pleeeeeeeease help me solve it =`( - Michael
    // This works for now but needs to be re-written because of an Action bug
    public void updateCost()
    {
		text0.text = playerTurnsManager.GetComponent<PlayerTurnsManager>().actionArray[0].GetComponent<Action0Script>().actionName + "\n$" + (playerTurnsManager.GetComponent<PlayerTurnsManager>().actionArray[0].GetComponent<Action0Script>().baseCost * playerTurnsManager.GetComponent<PlayerTurnsManager>().costMultiplier);

        text1.text = playerTurnsManager.GetComponent<PlayerTurnsManager>().actionArray[1].GetComponent<Action1Script>().actionName + "\n$" + (playerTurnsManager.GetComponent<PlayerTurnsManager>().actionArray[1].GetComponent<Action1Script>().baseCost * playerTurnsManager.GetComponent<PlayerTurnsManager>().costMultiplier);

        text2.text = playerTurnsManager.GetComponent<PlayerTurnsManager>().actionArray[2].GetComponent<Action2Script>().actionName + "\n$" + (playerTurnsManager.GetComponent<PlayerTurnsManager>().actionArray[2].GetComponent<Action2Script>().baseCost * playerTurnsManager.GetComponent<PlayerTurnsManager>().costMultiplier);

        text3.text = playerTurnsManager.GetComponent<PlayerTurnsManager>().actionArray[3].GetComponent<Action3Script>().actionName + "\n$" + (playerTurnsManager.GetComponent<PlayerTurnsManager>().actionArray[3].GetComponent<Action3Script>().baseCost * playerTurnsManager.GetComponent<PlayerTurnsManager>().costMultiplier);

        text4.text = playerTurnsManager.GetComponent<PlayerTurnsManager>().actionArray[4].GetComponent<Action4Script>().actionName + "\n$" + (playerTurnsManager.GetComponent<PlayerTurnsManager>().actionArray[4].GetComponent<Action4Script>().baseCost * playerTurnsManager.GetComponent<PlayerTurnsManager>().costMultiplier);

		text5.text = playerTurnsManager.GetComponent<PlayerTurnsManager>().actionArray[5].GetComponent<Action5Script>().actionName + "\n$" + (playerTurnsManager.GetComponent<PlayerTurnsManager>().actionArray[5].GetComponent<Action5Script>().baseCost * playerTurnsManager.GetComponent<PlayerTurnsManager>().costMultiplier);
    }
}