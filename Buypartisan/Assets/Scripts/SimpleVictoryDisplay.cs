//Alex Jungroth
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SimpleVictoryDisplay : MonoBehaviour {

    //Holds the game Controller
    public GameController gameController;

    //These objects will be unique to this script
    public GameObject victoryScreens;

    //Holds the components of the victory screens
    public GameObject espressoGameOver;
    public GameObject droneGameOver;
    public GameObject applePieGameOver;
    public GameObject windTurbinoGameOver;
    public GameObject providenceGameOver;
    public GameObject noOneVotedGameOver;
    public Text playerText;
    public Text voteTotalText;
    public Text victoryPointTotalText;
    public Text moneyTotalText;

    //Holds the winner
    public string winner = "no winner";
    public int winnerNumber = 0;

    //Holds the max score
    private int max;

    //Holds an array of int to parallel the ordering of the players (AAJ)
    private int[] playerArray = new int[5];

    //Holds a temporar integer for swapping numbers
    private int temp = 0;

    //Use this for initialization
    void Start()
    {
        //Diables the victory screens at the start of the game
        victoryScreens.SetActive(false);
        espressoGameOver.SetActive(false);
        droneGameOver.SetActive(false);
        applePieGameOver.SetActive(false);
        windTurbinoGameOver.SetActive(false);
        providenceGameOver.SetActive(false);
        noOneVotedGameOver.SetActive(false);

        //Clears the player text
        playerText.text = "";
        voteTotalText.text = "";
        victoryPointTotalText.text = "";
        moneyTotalText.text = "";

        //Initializes the player array (AAJ)
        for (int i = 0; i < 5; i++)
        {
            playerArray[i] = i;
        }//for
    }
	
    /// <summary>
    /// This is will display the winner of an election, or if the game is over, the winner of the game 
    /// The function caller does need to specify if they want the player ordered votes or victory points (Alex Jungroth)
    /// </summary>
    public void displayWinner(bool isVotes)
    {
        //Gets the winner from game controller
        winner = gameController.winner;
        winnerNumber = gameController.winnerNumber;
        
        //Determines how the players will be ordered
        if (isVotes == true)
        {
            //Sets the max to votes
            max = gameController.maxVotes;

            //Orders the players by votes (AAJ)
            for(int i = 0; i < gameController.numberPlayers; i++)
            {
                for(int j = i + 1; j < gameController.numberPlayers; j++)
                {
                    if(gameController.players[playerArray[j]].GetComponent<PlayerVariables>().votes > gameController.players[playerArray[i]].GetComponent<PlayerVariables>().votes)
                    {
                        //Swaps the order of the players
                        temp = playerArray[i];
                        playerArray[i] = playerArray[j];
                        playerArray[j] = temp;
                    }//if
                }//for
            }//for
        }//if
        else
        {
            //Sets the max to victory points
            max = gameController.maxVictoryPoints;

            //Orders the players by victory points
            for (int i = 0; i < gameController.numberPlayers; i++)
            {
                for (int j = i + 1; j < gameController.numberPlayers; j++)
                {
                    if (gameController.players[playerArray[j]].GetComponent<PlayerVariables>().victoryPoints > gameController.players[playerArray[i]].GetComponent<PlayerVariables>().victoryPoints)
                    {
                        //Swaps the order of the players
                        temp = playerArray[i];
                        playerArray[i] = playerArray[j];
                        playerArray[j] = temp;
                    }//if
                }//for
            }//for
        }//else

        if (max > 0)
        {
            //enables the win screens
            if (winner == "Espresso")
            {
                espressoGameOver.SetActive(true);
            }
            else if (winner == "Drone")
            {
                droneGameOver.SetActive(true);
            }
            else if (winner == "Apple Pie")
            {
                applePieGameOver.SetActive(true);
            }
            else if (winner == "Windy")
            {
                windTurbinoGameOver.SetActive(true);
            }
            else if (winner == "Providence")
            {
                providenceGameOver.SetActive(true);
            }
        }
        else
        {
            //A special screen for the case where no one voted
            noOneVotedGameOver.SetActive(true);
        }//else

        //Prints the players scores in the correct order 
        for (int i = 0; i < gameController.numberPlayers; i++)
        {
            playerText.text += (i + 1) + ". Player " + (playerArray[i] + 1) + " " + gameController.players[playerArray[i]].GetComponent<PlayerVariables>().politicalPartyName + " Party\n\n";
            voteTotalText.text += gameController.players[playerArray[i]].GetComponent<PlayerVariables>().votes + "\n\n";
            victoryPointTotalText.text += gameController.players[playerArray[i]].GetComponent<PlayerVariables>().victoryPoints + "\n\n";
            moneyTotalText.text += gameController.players[playerArray[i]].GetComponent<PlayerVariables>().money + "\n\n";
        }//for

        //Enables the victory screens when everything else is ready (AAJ)
        victoryScreens.SetActive(true);

    }//displayWinner

    /// <summary>
    /// This will close the window when the user presses the continue button (Alex Jungroth)
    /// </summary>
    public void dismissWindow()
    {
        //Diables the victory screens
        victoryScreens.SetActive(false);
        espressoGameOver.SetActive(false);
        droneGameOver.SetActive(false);
        applePieGameOver.SetActive(false);
        windTurbinoGameOver.SetActive(false);
        providenceGameOver.SetActive(false);
        noOneVotedGameOver.SetActive(false);

        //Clears the player text
        playerText.text = "";
        voteTotalText.text = "";
        victoryPointTotalText.text = "";
        moneyTotalText.text = "";

    }//dismissWindow
}