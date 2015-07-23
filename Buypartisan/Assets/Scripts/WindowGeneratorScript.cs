//Alex Jungroth
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WindowGeneratorScript : MonoBehaviour {

	//holds the game Controller
	public GameController gameController;

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

	//holds whether or not the game is over
	private bool gameOver;

	//holds the winner
	public string winner = "no winner";

	//holds the number of players
	private int totalPlayers = 0;

	//holds whether or not the elections results are displayed
	private bool resultsDisplayed = false;

	//holds whether or not the coalitions have been formed
	private bool coalitionsFormed = false;

	//holds coalition A's votes
	private int coalitionA = 0;

	//holds coalition B's votes
	private int coalitionB = 0;

	//holds the highest vote total
	private int maxVotes = 0;

	//holds the highest percentage total
	private int maxPercent = 0;

	//hols the highest victory point total
	private int maxVictoryPoints = 0;

	//holds the victory point totals which will be sent to the main tv
	private string victoryPointTotals;

	//holds whether or not the Game Controller can continue
	public bool resumeGame = false;

	// Use this for initialization
	void Start () 
	{
		//diables the end game screen at the start of the game
		endGame.SetActive(false);

		//disables the victory token
		victoryToken.SetActive(false);

		//disables the player sliders
		player1Slider.SetActive(false);
		player2Slider.SetActive(false);
		player3Slider.SetActive(false);
		player4Slider.SetActive(false);
		player5Slider.SetActive(false);
	}
	
	// Update is called once per frame
	void Update ()
	{
		//as the slider is adjusted the policy text will change
		if (coalitionsFormed && !gameOver) 
		{
			for(int i = 0; i < totalPlayers; i++)
			{
				//sets the winners policy
				if(gameController.players[i].GetComponent<PlayerVariables>().politicalPartyName == winner)
				{
					gameController.players[i].GetComponent<PlayerVariables>().chosenPolicy = (int) policySlider.GetComponent<Slider>().value;

					policyText.text = gameController.players[i].GetComponent<PlayerVariables>().policiesText[(int) policySlider.GetComponent<Slider>().value];
				}
			}
		}

		//as the player sliders are adjusted the player's alignment is changed
		if((!coalitionsFormed) && (totalPlayers > 0))
		{
			for(int i = 0; i < totalPlayers; i++)
			{
				switch(i)
				{
					case 0:
						gameController.players[i].GetComponent<PlayerVariables>().alignment = (int) player1Slider.GetComponent<Slider>().value;
					break;

					case 1:
						gameController.players[i].GetComponent<PlayerVariables>().alignment = (int) player2Slider.GetComponent<Slider>().value;
					break;

					case 2:
						gameController.players[i].GetComponent<PlayerVariables>().alignment = (int) player3Slider.GetComponent<Slider>().value;
					break;

					case 3:
						gameController.players[i].GetComponent<PlayerVariables>().alignment = (int) player4Slider.GetComponent<Slider>().value;
					break;

					case 4:
						gameController.players[i].GetComponent<PlayerVariables>().alignment = (int) player5Slider.GetComponent<Slider>().value;
					break;

				}//switch
			}//for
		}//if
	}//update
	
	/// <summary>
	/// Manages window generation. (Alex Jungroth)
	/// </summary>
	public void generateElectionVictory(bool gameFinished)
	{
		//gets whether or not the game is over from the gameController
		gameOver = gameFinished;

		//gets the number of players from the game controller
		totalPlayers = gameController.numberPlayers;

		//enables the sliders for the players who are present
		if(totalPlayers >= 2) 
		{
			player1Slider.SetActive(true);
			player2Slider.SetActive(true);
		}

		if(totalPlayers >= 3)
		{
			player3Slider.SetActive(true);
		}

		if(totalPlayers >= 4)
		{
			player4Slider.SetActive(true);
		}

		if(totalPlayers >= 5)
		{
			player5Slider.SetActive(true);
		}

		//clears the player text
		playerText.text = "";

		//updates the player text
		for(int i = 0; i < totalPlayers; i++)
		{
			playerText.text +=  gameController.players[i].GetComponent<PlayerVariables>().politicalPartyName + " Party\n\n";
		}
		
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
	
	/// <summary>
	/// Continues the game. (Alex Jungroth)
	/// </summary>
	public void continueGame()
	{		
		if (resultsDisplayed)
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

			//resets the coalitions vote totals and all of the max values
			coalitionA = 0;
			coalitionB = 0;
			maxVotes = 0;
			maxPercent = 0;
			maxVictoryPoints = 0;

			//disables the victory token
			victoryToken.SetActive(false);

			//resets the victoryPointTotals
			victoryPointTotals = "";

			//displays to the TV that it is player 1's turn
			uiController.alterTextBox("It is the " + gameController.players[0].GetComponent<PlayerVariables>().politicalPartyName + " Party's turn.");

			//disables the end game screen
			endGame.SetActive (false);

			//lets the game controller know it can continue
			resumeGame = true;
		} 
		else 
		{

			//This code forms the coalitions

			//The coalitions have been formed
			coalitionsFormed = true;

			//disables the player sliders
			player1Slider.SetActive (false);
			player2Slider.SetActive (false);
			player3Slider.SetActive (false);
			player4Slider.SetActive (false);
			player5Slider.SetActive (false);

			//tallies the votes in the coalitions
			for (int i = 0; i < totalPlayers; i++)
			{
				if (gameController.players [i].GetComponent<PlayerVariables> ().alignment == 2) 
				{
					coalitionA += gameController.players [i].GetComponent<PlayerVariables> ().votes;
				}
				else if (gameController.players [i].GetComponent<PlayerVariables> ().alignment == 3)
				{
					coalitionB += gameController.players [i].GetComponent<PlayerVariables> ().votes;
				}
			}

			//checks to see if the coalitions have the most votes
			if (coalitionA >= maxVotes)
			{
				maxVotes = coalitionA;
				winner = "coalitionA";
			}

			if (coalitionB >= maxVotes) 
			{
				maxVotes = coalitionB;
				winner = "coalitionB";
			}

			//determines the player(s) with the most votes
			for (int i = 0; i < totalPlayers; i++)
			{
				if (gameController.players [i].GetComponent<PlayerVariables> ().alignment == 1) 
				{
					if (gameController.players [i].GetComponent<PlayerVariables> ().votes >= maxVotes) 
					{
						maxVotes = gameController.players [i].GetComponent<PlayerVariables> ().votes;
						winner = gameController.players [i].GetComponent<PlayerVariables> ().politicalPartyName;
					}
				}
			}

			if ((winner == "coalitionA") && (maxVotes > 0)) 
			{
				//if coalition A won, determines the winner in the coaliton and divides up the victory points
				for (int i = 0; i < totalPlayers; i++) 
				{
					if (gameController.players [i].GetComponent<PlayerVariables> ().alignment == 2) 
					{
						gameController.players [i].GetComponent<PlayerVariables> ().victoryPoints += (int)Mathf.Ceil
							(10 * gameController.players [i].GetComponent<PlayerVariables> ().votes / maxVotes);

						if (gameController.players [i].GetComponent<PlayerVariables> ().votes >= maxPercent)
						{
							maxPercent = gameController.players [i].GetComponent<PlayerVariables> ().votes;
							winner = gameController.players [i].GetComponent<PlayerVariables> ().politicalPartyName;
						}
					}
				}
			} 
			else if ((winner == "coalitionB") && (maxVotes > 0)) 
			{
				//if coaliton B won, determines the winner in the coalition and divides up the victory points
				for (int i = 0; i < totalPlayers; i++) 
				{
					if (gameController.players [i].GetComponent<PlayerVariables> ().alignment == 3) 
					{
						gameController.players [i].GetComponent<PlayerVariables> ().victoryPoints += (int)Mathf.Ceil
							(10 * gameController.players [i].GetComponent<PlayerVariables> ().votes / maxVotes);
					
						if (gameController.players [i].GetComponent<PlayerVariables> ().votes >= maxPercent) 
						{
							maxPercent = gameController.players [i].GetComponent<PlayerVariables> ().votes;
							winner = gameController.players [i].GetComponent<PlayerVariables> ().politicalPartyName;
						}
					}
				}
			}
			else if (maxVotes > 0) 
			{
				//if a single player won the election
				for (int i = 0; i < totalPlayers; i++)
				{
					if (gameController.players [i].GetComponent<PlayerVariables> ().alignment == 1) 
					{
						if (winner == gameController.players [i].GetComponent<PlayerVariables> ().politicalPartyName) 
						{
							gameController.players [i].GetComponent<PlayerVariables> ().victoryPoints += 10;
						}
					}
				}
			}

			//in the case that a coalition won this correctly updates max votes for display
			for(int i = 0; i < totalPlayers; i++)
			{
				if(gameController.players[i].GetComponent<PlayerVariables>().politicalPartyName == winner)
				{
					maxVotes = gameController.players[i].GetComponent<PlayerVariables>().votes;
				}
			}

			//generates the correct window
			if (gameOver == false) 
			{
				//This code generates a window for the end of an election
				if (maxVotes > 0) 
				{
					//if someone won the election
				
					//alters the window's name
					windowName.text = "Election Results are in!";
				
					//displays the victory token
					victoryToken.SetActive (true);
				
					//prints the election winner from the gameController
					victoryText.text = "The " + winner + " Party won the election with " + maxVotes.ToString() + " votes!";
				
					//displays the policy slider
					policySlider.SetActive (true);
				
					//prints the default policy text
					policyText.text = "Conitnue without choosing \na policy!";
				
					//prints the default helpful text
					helpfulText.text = "Choose a policy!";
				
					//prints each player's victory points to the main tv 
					for (int i = 0; i < totalPlayers; i++) 
					{
						victoryPointTotals += gameController.players [i].GetComponent<PlayerVariables> ().politicalPartyName + " Party: " +
							gameController.players [i].GetComponent<PlayerVariables> ().victoryPoints + "VP\n";
					}

					uiController.alterTextBox (victoryPointTotals);

				} 
				else 
				{
					//if no one won the election
					winner = "no winner";

					windowName.text = "No one voted, so no one was elected!";
				
					victoryText.text = "";
				
					policySlider.SetActive (false);
				
					//prints each player's victory points to the main tv 
					for (int i = 0; i < totalPlayers; i++)
					{
						victoryPointTotals += gameController.players [i].GetComponent<PlayerVariables> ().politicalPartyName + " Party: " +
							gameController.players [i].GetComponent<PlayerVariables> ().victoryPoints + "VP\n";
					}
				
					uiController.alterTextBox (victoryPointTotals);
				}
			} 
			else 
			{
				//determines who won the game
				for (int i = 0; i < totalPlayers; i++) 
				{
					if (gameController.players [i].GetComponent<PlayerVariables> ().victoryPoints >= maxVictoryPoints) 
					{
						maxVictoryPoints = gameController.players [i].GetComponent<PlayerVariables> ().victoryPoints;
						winner = gameController.players [i].GetComponent<PlayerVariables> ().politicalPartyName;
					}
				}
			
				//This code generates a window for the end of the game
			
				//enables the end game screen
				endGame.SetActive (true);
			
				//displays the victory token
				victoryToken.SetActive (true);
			
				//diables some elements on the end game screen
				continueButton.SetActive (false);
				policySlider.SetActive (false);
				policyText.text = "";
				helpfulText.text = "";
				playerText.text = "";
			
				//alters the window's name
				windowName.text = "Game Over Man! Game Over!";

				if(winner == "Providence")
				{
					helpfulText.text = "All Hail the Overlords!";
				}

				if (maxVictoryPoints > 0) 
				{
					victoryText.text = "The " + winner + " Party has won BuyPartisan!";
				}
				else
				{
					victoryText.text = "The nation falls into chaos as no one was elected once!";
				}
				
				//prints each player's victory points to the main tv 
				for (int i = 0; i < totalPlayers; i++)
				{
					victoryPointTotals += gameController.players [i].GetComponent<PlayerVariables> ().politicalPartyName + " Party: " +
						gameController.players [i].GetComponent<PlayerVariables> ().victoryPoints + "VP\n";
				}
				
				uiController.alterTextBox (victoryPointTotals);
			}

			//disables some elements of the end game screen
			playerText.text = "";
		
			player1Slider.SetActive (false);
			player2Slider.SetActive (false);
			player3Slider.SetActive (false);
			player4Slider.SetActive (false);
			player5Slider.SetActive (false);

			//the results have been displayed
			resultsDisplayed = true;

		}//else
	}//continueGame()
}//WindowGeneratorScript()