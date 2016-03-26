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
	public GameObject coalitionScreen;
	public GameObject espressoScreen;
	public GameObject droneScreen;
	public GameObject applePieScreen;
	public GameObject windTurbinoScreen;
	public GameObject providenceScreen;
	public GameObject noOneVotedScreen;
	public GameObject espressoGameOver;
	public GameObject droneGameOver;
	public GameObject applePieGameOver;
	public GameObject windTurbinoGameOver;
	public GameObject providenceGameOver;
	public GameObject noOneVotedGameOver;
	public GameObject statsText;
	public Text moneyText;
	public Text victoryText;
	public Text votesText;
	public Text victoryPointsText;
	public GameObject policySlider;
	public GameObject policySliderHandle;
	public GameObject continueButton;
	public Text policyText;
	public Text policyText1;
	public Text policyText2;
	public Text policyText3;
	public GameObject player1Slider;
	public GameObject player2Slider;
	public GameObject player3Slider;
	public GameObject player4Slider;
	public GameObject player5Slider;
	public GameObject player1SliderHandle;
	public GameObject player2SliderHandle;
	public GameObject player3SliderHandle;
	public GameObject player4SliderHandle;
	public GameObject player5SliderHandle;

	//holds images for the player slider handles for the coalition screen
	//these are taken from the prefabs in slider handles
	public Image espressoHandleImage;
	public Image droneHandleImage;
	public Image applePieHandleImage;
	public Image windyHandleImage;
	public Image providenceHandleImage;
	public Image whiteHandleImage;

	//This is the same sprite that is used by the title screen sliders so don't delete it
	public Image redHandleImage;

	//holds whether or not the game is over
	private bool gameOver;

	//holds the winner
	public string winner = "no winner";
    public int winnerNumber = 0;

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

	//hold the scores at the end of election
	private string partyNames;
	private string moneyTotals;
	private string voteTotals;
	private string victoryPointTotals;

	//holds whether or not the Game Controller can continue
	public bool resumeGame = false;

	//holds whether or not the game can be reset
	private bool resetGame = false;

	// Use this for initialization
	void Start () 
	{
		//diables the end game screen at the start of the game
		endGame.SetActive(false);

		//disables the player sliders
		player1Slider.SetActive(false);
		player2Slider.SetActive(false);
		player3Slider.SetActive(false);
		player4Slider.SetActive(false);
		player5Slider.SetActive(false);
		
		//disables the win screens and end game screens
		espressoScreen.SetActive(false);
		droneScreen.SetActive(false);
		applePieScreen.SetActive(false);
		windTurbinoScreen.SetActive(false);
		providenceScreen.SetActive(false);
		noOneVotedScreen.SetActive(false);
		espressoGameOver.SetActive(false);
		droneGameOver.SetActive(false);
		applePieGameOver.SetActive(false);
		windTurbinoGameOver.SetActive(false);
		providenceGameOver.SetActive(false);
		noOneVotedGameOver.SetActive(false);
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
				if(gameController.players[i].GetComponent<PlayerVariables>().playerNumber == winnerNumber)
				{
					gameController.players[i].GetComponent<PlayerVariables>().chosenPolicy = (int) policySlider.GetComponent<Slider>().value;

				}
			}

            //If there are no unique parties skip the party policies
            if(gameController.uniqueParties == false)
            {
                policySlider.SetActive(false);
                policyText.text = "";
                policyText1.text = "";
                policyText2.text = "";
                policyText3.text = "";
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

		//resets the winner
		winner = "no winner";
        winnerNumber = 0;

		//enables the coalitions screen
		coalitionScreen.SetActive(true);

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

		//updates the player text
		/*for(int i = 0; i < totalPlayers; i++)
		{
			playerText.text +=  gameController.players[i].GetComponent<PlayerVariables>().politicalPartyName + " Party\n\n";
		}
		*/

		//This code generates a window for forming coalitions

		//disables the statistic labels
		statsText.SetActive(false);

		//clears the money totals
		moneyText.text = "";

		//enables the end game screen
		endGame.SetActive(true);

		//alters the window's name
		//windowName.text = "Go it Alone or Choose a Coalition!";

		//uses the vicotry text to display the coalilitions
		//victoryText.text = "Single       A               B";

		//diables some elements on the end game screen
		policySlider.SetActive(false);
		policyText.text = "";
		policyText1.text = "";
		policyText2.text = "";
		policyText3.text = "";
		victoryText.text = "";
		votesText.text = "";
		victoryPointsText.text = "";

		//Sets the sliders nobes for the coalition screen to the correct image
		for(int i = 0; i < gameController.numberPlayers; i++)
		{
			if(gameController.players[i].GetComponent<PlayerVariables>().politicalPartyName == "Espresso")
			{
				setHandleImage(i,gameController.players[i].GetComponent<PlayerVariables>().politicalPartyName);
			}
			else if(gameController.players[i].GetComponent<PlayerVariables>().politicalPartyName == "Drone")
			{
				setHandleImage(i,gameController.players[i].GetComponent<PlayerVariables>().politicalPartyName);
			}
			else if(gameController.players[i].GetComponent<PlayerVariables>().politicalPartyName == "Apple Pie")
			{
				setHandleImage(i,gameController.players[i].GetComponent<PlayerVariables>().politicalPartyName);
			}
			else if(gameController.players[i].GetComponent<PlayerVariables>().politicalPartyName == "Windy")
			{
				setHandleImage(i,gameController.players[i].GetComponent<PlayerVariables>().politicalPartyName);
			}
			else if(gameController.players[i].GetComponent<PlayerVariables>().politicalPartyName == "Providence")
			{
				setHandleImage(i,gameController.players[i].GetComponent<PlayerVariables>().politicalPartyName);
			}
		}
	}

	/// <summary>
	/// Sets the handle image.
	/// </summary>
	void setHandleImage(int i, string politicalPartyName)
	{
		switch(i)
		{
			case 0:
				
				//Player 1 Handle
				switch(politicalPartyName)
				{
					case "Espresso":
						player1SliderHandle.GetComponent<Image>().sprite = espressoHandleImage.sprite;
						
					break;
					
					case "Drone":
						player1SliderHandle.GetComponent<Image>().sprite = droneHandleImage.sprite;
						
					break;
					
					case "Apple Pie":
						player1SliderHandle.GetComponent<Image>().sprite = applePieHandleImage.sprite;
						
					break;
					
					case "Windy":
						player1SliderHandle.GetComponent<Image>().sprite = windyHandleImage.sprite;
						
					break;
					
					case "Providence":
						player1SliderHandle.GetComponent<Image>().sprite = providenceHandleImage.sprite;
						
					break;
				}
				
			break;
				
			case 1:
				
				//Player 2 Handle
				switch(politicalPartyName)
				{
					case "Espresso":
						player2SliderHandle.GetComponent<Image>().sprite = espressoHandleImage.sprite;
						
					break;
					
					case "Drone":
						player2SliderHandle.GetComponent<Image>().sprite = droneHandleImage.sprite;
						
					break;
					
					case "Apple Pie":
						player2SliderHandle.GetComponent<Image>().sprite = applePieHandleImage.sprite;
						
					break;
					
					case "Windy":
						player2SliderHandle.GetComponent<Image>().sprite = windyHandleImage.sprite;
						
					break;
					
					case "Providence":
						player2SliderHandle.GetComponent<Image>().sprite = providenceHandleImage.sprite;
						
					break;
				}
				
			break;
				
			case 2:
				
				//Player 3 Handle
				switch(politicalPartyName)
				{
					case "Espresso":
						player3SliderHandle.GetComponent<Image>().sprite = espressoHandleImage.sprite;
						
					break;
					
					case "Drone":
						player3SliderHandle.GetComponent<Image>().sprite = droneHandleImage.sprite;
						
					break;
					
					case "Apple Pie":
						player3SliderHandle.GetComponent<Image>().sprite = applePieHandleImage.sprite;
						
					break;
					
					case "Windy":
						player3SliderHandle.GetComponent<Image>().sprite = windyHandleImage.sprite;
						
					break;
					
					case "Providence":
						player3SliderHandle.GetComponent<Image>().sprite = providenceHandleImage.sprite;
						
					break;
				}
				
			break;
				
			case 3:
				
				//Player 4 Handle
				switch(politicalPartyName)
				{
					case "Espresso":
						player4SliderHandle.GetComponent<Image>().sprite = espressoHandleImage.sprite;
						
					break;
					
					case "Drone":
						player4SliderHandle.GetComponent<Image>().sprite = droneHandleImage.sprite;
						
					break;
					
					case "Apple Pie":
						player4SliderHandle.GetComponent<Image>().sprite = applePieHandleImage.sprite;
						
					break;
					
					case "Windy":
						player4SliderHandle.GetComponent<Image>().sprite = windyHandleImage.sprite;
						
					break;
					
					case "Providence":
						player4SliderHandle.GetComponent<Image>().sprite = providenceHandleImage.sprite;

					break;
				}
				
			break;
				
			case 4:
				
				//Player 5 Handle
				switch(politicalPartyName)
				{
					case "Espresso":
						player5SliderHandle.GetComponent<Image>().sprite = espressoHandleImage.sprite;
					
					break;
					
					case "Drone":
						player5SliderHandle.GetComponent<Image>().sprite = droneHandleImage.sprite;
						
					break;
					
					case "Apple Pie":
						player5SliderHandle.GetComponent<Image>().sprite = applePieHandleImage.sprite;
						
					break;
					
					case "Windy":
						player5SliderHandle.GetComponent<Image>().sprite = windyHandleImage.sprite;
						
					break;
					
					case "Providence":
						player5SliderHandle.GetComponent<Image>().sprite = providenceHandleImage.sprite;
						
					break;
				}
					
			break;
		}
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

			//resets the policy slider
			policySlider.GetComponent<Slider> ().value = 0;

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
			//victoryToken.SetActive(false);

			//resets the scores
			partyNames = "";
			voteTotals = "";
			victoryPointTotals = "";

			uiController.SetPlayerAndParyNameInUpperLeft (gameController.players [0].GetComponent<PlayerVariables> ().politicalPartyName, 1);

			//disables the win screens
			espressoScreen.SetActive (false);
			droneScreen.SetActive (false);
			applePieScreen.SetActive (false);
			windTurbinoScreen.SetActive (false);
			providenceScreen.SetActive (false);
			noOneVotedScreen.SetActive (false);

			//disables the end game screen
			endGame.SetActive (false);

			//lets the game controller know it can continue
			resumeGame = true;
		} 
		else if (!resultsDisplayed && !resetGame)
		{
			//This code forms the coalitions

			//The coalitions have been formed
			coalitionsFormed = true;

			//disables the coalitions screen image
			coalitionScreen.SetActive (false);

			//disables the player sliders
			player1Slider.SetActive (false);
			player2Slider.SetActive (false);
			player3Slider.SetActive (false);
			player4Slider.SetActive (false);
			player5Slider.SetActive (false);

			//tallies the votes in the coalitions
			for (int i = 0; i < totalPlayers; i++) 
			{
				if(gameController.players [i].GetComponent<PlayerVariables> ().alignment == 2) 
				{
					coalitionA += gameController.players [i].GetComponent<PlayerVariables> ().votes;

				} 
				else if(gameController.players [i].GetComponent<PlayerVariables> ().alignment == 3)
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
                        winnerNumber = gameController.players[i].GetComponent<PlayerVariables>().playerNumber;
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
                            winnerNumber = gameController.players[i].GetComponent<PlayerVariables>().playerNumber;

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
                            winnerNumber = gameController.players[i].GetComponent<PlayerVariables>().playerNumber;
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
						if (winner == gameController.players [i].GetComponent<PlayerVariables> ().politicalPartyName && winnerNumber == gameController.players[i].GetComponent<PlayerVariables>().playerNumber) 
						{
							gameController.players [i].GetComponent<PlayerVariables> ().victoryPoints += 10;
						}
					}
				}
			}

			//in the case that a coalition won this correctly updates max votes for display
			for(int i = 0; i < totalPlayers; i++) 
			{
				if(gameController.players [i].GetComponent<PlayerVariables> ().politicalPartyName == winner && winnerNumber == gameController.players[i].GetComponent<PlayerVariables>().playerNumber) 
				{
					maxVotes = gameController.players [i].GetComponent<PlayerVariables> ().votes;
				}
			}

			//generates the correct window
			if (gameOver == false) 
			{
				//This code generates a window for the end of an election
				if (maxVotes > 0) 
				{
					//if someone won the election

					//enables the win screens
					if (winner == "Espresso") 
					{
						espressoScreen.SetActive (true);

						//sets the correct policy slider sprite
						policySliderHandle.GetComponent<Image> ().sprite = redHandleImage.sprite;

					}
					else if (winner == "Drone") 
					{
						droneScreen.SetActive (true);

						//sets the correct policy slider sprite
						policySliderHandle.GetComponent<Image> ().sprite = whiteHandleImage.sprite;

					}
					else if (winner == "Apple Pie") 
					{
						applePieScreen.SetActive (true);

						//sets the correct policy slider sprite
						policySliderHandle.GetComponent<Image> ().sprite = redHandleImage.sprite;
					}
					else if (winner == "Windy") 
					{
						windTurbinoScreen.SetActive (true);

						//sets the correct policy slider sprite
						policySliderHandle.GetComponent<Image> ().sprite = redHandleImage.sprite;
					}
					else if (winner == "Providence") 
					{
						providenceScreen.SetActive (true);

						//sets the correct policy slider sprite
						policySliderHandle.GetComponent<Image> ().sprite = redHandleImage.sprite;
					}

					//displays the policy slider
					policySlider.SetActive (true);

					for (int i = 0; i < totalPlayers; i++) 
					{
						//sets the winners policy
						if (gameController.players [i].GetComponent<PlayerVariables> ().politicalPartyName == winner && winnerNumber == gameController.players[i].GetComponent<PlayerVariables>().playerNumber)
						{
							//prints the policy texts
							policyText.text = gameController.players [i].GetComponent<PlayerVariables> ().policiesText [0];
							
							policyText1.text = gameController.players [i].GetComponent<PlayerVariables> ().policiesText [1];
							
							policyText2.text = gameController.players [i].GetComponent<PlayerVariables> ().policiesText [2];
							
							policyText3.text = gameController.players [i].GetComponent<PlayerVariables> ().policiesText [3];
							
						}//if
					}//for
				} 
				else
				{
					//if no one won the election
					winner = "no winner";
                    winnerNumber = 0;

					noOneVotedScreen.SetActive (true);

					policySlider.SetActive (false);
				}

				//prints each player's victory points to the big tv 
				for (int i = 0; i < totalPlayers; i++)
				{
					partyNames += "Player " + (i + 1) + ":" + "\n" + gameController.players [i].GetComponent<PlayerVariables> ().politicalPartyName + " Party" + "\n";
					voteTotals += gameController.players [i].GetComponent<PlayerVariables> ().votes + "\n\n";
					victoryPointTotals += gameController.players [i].GetComponent<PlayerVariables> ().victoryPoints + "\n\n";
				}
				
				//updates the election victory screen with the parties' votes and victory point stats
				victoryText.text = partyNames;
				votesText.text = voteTotals;
				victoryPointsText.text = victoryPointTotals;

				//the results have been displayed
				resultsDisplayed = true;
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
                        winnerNumber = gameController.players[i].GetComponent<PlayerVariables>().playerNumber;
                    }
				}
			
				//This code generates a window for the end of the game
			
				//enables the end game screen
				endGame.SetActive (true);

				//enables the statistic labels
				statsText.SetActive (true);

				//diables some elements on the end game screen
				policySlider.SetActive (false);
				policyText.text = "";
				policyText1.text = "";
				policyText2.text = "";
				policyText3.text = "";

				if (maxVictoryPoints > 0) 
				{
					//enables the win screens
					if (winner == "Espresso") 
					{
						espressoGameOver.SetActive (true);
					}
					else if (winner == "Drone") 
					{
						droneGameOver.SetActive (true);
					}
					else if (winner == "Apple Pie") 
					{
						applePieGameOver.SetActive (true);
					}
					else if (winner == "Windy") 
					{
						windTurbinoGameOver.SetActive (true);
					} 
					else if (winner == "Providence") 
					{
						providenceGameOver.SetActive (true);
					}

				} 
				else 
				{
					//a special screen for the case where no one voted
					noOneVotedGameOver.SetActive (true);

				}
				
				//prints each player's victory points to the big tv 
				for (int i = 0; i < totalPlayers; i++)
				{
					partyNames += "Player " + (i + 1) + ":" + "\n" + gameController.players [i].GetComponent<PlayerVariables> ().politicalPartyName + " Party" + "\n";
					moneyTotals += gameController.players [i].GetComponent<PlayerVariables> ().money + "\n\n";
					voteTotals += gameController.players [i].GetComponent<PlayerVariables> ().votes + "\n\n";
					victoryPointTotals += gameController.players [i].GetComponent<PlayerVariables> ().victoryPoints + "\n\n";
				}
				
				//updates the election victory screen with the parties' votes and victory point stats
				victoryText.text = partyNames;
				moneyText.text = moneyTotals;
				votesText.text = voteTotals;
				victoryPointsText.text = victoryPointTotals;

				//lets the user know they can reset the game
				uiController.alterTextBox("Press confirm or the escape key to return to the title screen. Thanks for playing!");

				//allows the game to be reset
				resetGame = true;
			}

			player1Slider.SetActive (false);
			player2Slider.SetActive (false);
			player3Slider.SetActive (false);
			player4Slider.SetActive (false);
			player5Slider.SetActive (false);

		}//else if
		else if(resetGame)
		{
			//resets the game
			Application.LoadLevel("TitleScene");

		}//else
	}//continueGame()
}//WindowGeneratorScript()