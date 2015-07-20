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
	public GameObject victoryToken;
	public Text victoryText;
	public GameObject policySlider;
	public GameObject continueButton;
	public Text windowName;
	public Text policyText;
	public Text helpfulText;
	public Text playerText;
	public GameObject player1Slider;
	public GameObject player2Slider;
	public GameObject player3Slider;
	public GameObject player4Slider;
	public GameObject player5Slider;

	//just need this for now
	private int winner;

	//holds whether or not the elections results are displayed
	private bool resultsDisplayed = false;

	//holds whether or not the coalitions have been formed
	private bool coalitionsFormed = false;

	// Use this for initialization
	void Start () 
	{
		//diables the end game screen at the start of the game
		endGame.SetActive(false);

		//disables the victory token
		victoryToken.SetActive(false);
	}
	
	// Update is called once per frame
	void Update ()
	{
		//as the slider is adjusted the policy text will change

		//as the player sliders are adjusted the player's alignment is changed
		if(!coalitionsFormed)
		{
			for(int i = 0; i < 5; i++)
			{
				//player1Slider.GetComponent<Slider>().value;

			}
		}
	}
	
	/// <summary>
	/// Manages window generation. (Alex Jungroth)
	/// </summary>
	public void generateElectionVictory(bool gameFinished, int gameWinner)
	{
		//gets the winner from the game controler
		winner = gameWinner;
		
		//generates the correct window
		if (gameFinished == false) 
		{
			//This code generates a window for forming coalitions

			//enables the end game screen
			endGame.SetActive(true);

			//alters the window's name
			windowName.text = "Go it Alone or Choose a Coalition!";

			//uses the vicotry text to display the coalilitions
			victoryText.text = "Single       A               B";

			//diables some elements on the end game screen
			policySlider.SetActive(false);
			policyText.text = "";
			helpfulText.text = "";
		} 
		else
		{
			//This code generates a window for the end of the game
			
			//enables the end game screen
			endGame.SetActive(true);

			//displays the victory token
			victoryToken.SetActive(true);

			//diables some elements on the end game screen
			continueButton.SetActive(false);
			policySlider.SetActive(false);
			policyText.text = "";
			helpfulText.text = "";
			victoryText.text = "";
			playerText.text = "";

			player1Slider.SetActive(false);
			player2Slider.SetActive(false);
			player3Slider.SetActive(false);
			player4Slider.SetActive(false);
			player5Slider.SetActive(false);

			//alters the window's name
			windowName.text = "Game Over Man! Game Over!";
		}
	}
	
	/// <summary>
	/// Continues the game. (Alex Jungroth)
	/// </summary>
	public void continueGame()
	{		
		if(resultsDisplayed)
		{
			//enables the end turn and player stats buttons
			uiController.endTurnButton.SetActive (true);
			uiController.displayStatsButton.SetActive (true);
			
			//enables the action buttons
			for (int i = 0; i < 10; i++)
			{
				uiController.ActionButtonObject [i].SetActive (true);
			}
		    
			//resets the bools
			resultsDisplayed = false;
			coalitionsFormed = false;

			//disables the end game screen
			endGame.SetActive(false);
		}

		//The coalitions have been formed
		coalitionsFormed = true;

		//This code generates a window for the end of an election

		//alters the window's name
		windowName.text = "Election Results are in!";

		//displays the victory token
		victoryToken.SetActive(true);

		//prints the election winner from the gameController
		victoryText.text = "Player " + winner + " won the election!";

		//displays the policy slider
		policySlider.SetActive(true);

		//prints the default policy text
		policyText.text = "Conitnue without choosing \na policy!";

		//prints the default helpful text
		helpfulText.text = "Choose a policy!";

		//disables some elements of the end game screen
		playerText.text = "";
		
		player1Slider.SetActive(false);
		player2Slider.SetActive(false);
		player3Slider.SetActive(false);
		player4Slider.SetActive(false);
		player5Slider.SetActive(false);

		//the results have been displayed
		resultsDisplayed = true;
	}
}