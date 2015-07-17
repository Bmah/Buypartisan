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
	public GameObject policySlider;
	public GameObject continueButton;
	public Text windowName;
	public Text policyText;
	public Text helpfulText;

	// Use this for initialization
	void Start () 
	{
		//diables the end game screen at the start of the game
		endGame.SetActive(false);
	}
	
	// Update is called once per frame
	void Update ()
	{
		//as the slider is adjusted the policy text will change

	}

	/// <summary>
	/// Manages window generation. (Alex Jungroth)
	/// </summary>
	public void generateElectionVictory(bool gameFinished, int winner)
	{
		//generates the correct window
		if (gameFinished == false) 
		{
			//This code generates a window for the end of an election

			//enables the end game screen
			endGame.SetActive(true);

			//alters the window's name
			windowName.text = "Election Results are in!";

			//prints the election winner from the gameController
			victoryText.text = "Player " + winner + " won the election!";


		} 
		else
		{
			//This code generates a window for the end of the game

			//enables the end game screen
			endGame.SetActive(true);

			//diables some elements on the end game screen
			continueButton.SetActive(false);
			policySlider.SetActive(false);
			policyText.text = "";
			helpfulText.text = "";
			victoryText.text = "";

			//alters the window's name
			windowName.text = "Game Over Man! Game Over!";
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

		//enables the action buttons
		for(int i = 0; i < 10; i++)
		{
			uiController.ActionButtonObject[i].SetActive(true);
		}
		
		//disables the end game screen
		endGame.SetActive(false);
	}
}