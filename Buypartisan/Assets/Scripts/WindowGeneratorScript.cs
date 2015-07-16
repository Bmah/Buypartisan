//Alex Jungroth
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WindowGeneratorScript : MonoBehaviour {

	//holds the UI script
	public UI_Script uiController;

	//holds the end game screen
	public GameObject endGame;

	//holds the components of the end game screen
	public GameObject endGameWindow;
	public Image victoryToken;
	public Text victoryText;
	public Slider policySlider;
	public Button continueButton;
	public Text windowName;
	public Text policyText;

	// Use this for initialization
	void Start () 
	{
		//diables the end game screen at the start of the game
		endGame.SetActive(false);
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	/// <summary>
	/// Manages window generation. (Alex Jungroth)
	/// </summary>
	public void generateElectionVictory(bool gameFinished)
	{
		//generates the correct window
		if (gameFinished == false) 
		{
			//This code generates a window for the end of an election

			//disables the end turn and player stats buttons
			uiController.endTurnButton.SetActive(false);
			uiController.displayStatsButton.SetActive(false);

			//enables the end game screen
			endGame.SetActive(true);
		} 
		else
		{
			//This code generates a window for the end of the game

			//disables the end turn and player stats buttons
			uiController.endTurnButton.SetActive(false);
			uiController.displayStatsButton.SetActive(false);
			
			//enables the end game screen
			endGame.SetActive(true);

			//diables the continue button on the end game screen
			continueButton.enabled = false;
		}
	}

	/// <summary>
	/// Continues the game. (Alex Jungroth)
	/// </summary>
	public void continueGame()
	{
		//enables the end turn and player stats buttons
		uiController.endTurnButton.SetActive(true);
		uiController.displayStatsButton.SetActive(true);

		//disables the end game screen
		endGame.SetActive(false);
	}
}