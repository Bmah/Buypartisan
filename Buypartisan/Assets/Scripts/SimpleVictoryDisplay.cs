//Alex Jungroth
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SimpleVictoryDisplay : MonoBehaviour {

    //Holds the game Controller
    public GameController gameController;

    //These objects will be unique to this script
    
    public GameObject EndGameObject;
    public GameObject VictoryScreen;

    [Header("Player End Of Election Images")]
    public Sprite BrownElection;
    public Sprite RedElection;
    public Sprite YellowElection;
    public Sprite GreenElection;
    public Sprite PurpleElection;
    //Holds the components of the victory screens
    [Header("Player End Game Images")]
    public Sprite BrownGameOver;
    public Sprite RedGameOver;
    public Sprite YellowGameOver;
    public Sprite GreenGameOver;
    public Sprite PurpleGameOver;
    public Sprite NoWinner;
    [Header("Victory Texts")]
    public Text playerText;
    public Text voteTotalText;
    public Text victoryPointTotalText;
    public Text moneyTotalText;
    [Space(10)]
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
        EndGameObject.SetActive(false);


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
    public void displayWinner(bool EndOfElection)
    {
        //Gets the winner from game controller
        winner = gameController.WinnerName;
        winnerNumber = gameController.WinnerPlayerNum;
        
        //Determines how the players will be ordered
        if (EndOfElection == true)
        {
            //Sets the max to votes
            max = gameController.MaxVote;

            //Orders the players by votes (AAJ)
            for (int i = 0; i < gameController.numberPlayers; i++)
            {
                for (int j = i + 1; j < gameController.numberPlayers; j++)
                {
                    if (gameController.Players[playerArray[j]].GetComponent<PlayerVariables>().votes >= gameController.Players[playerArray[i]].GetComponent<PlayerVariables>().votes)
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
            max = gameController.MaxVictoryPoints;

            //Orders the players by victory points
            for (int i = 0; i < gameController.numberPlayers; i++)
            {
                for (int j = i + 1; j < gameController.numberPlayers; j++)
                {
                    if (gameController.Players[playerArray[j]].GetComponent<PlayerVariables>().victoryPoints >= gameController.Players[playerArray[i]].GetComponent<PlayerVariables>().victoryPoints)
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
            if (winner == "Brown")
            {
                if (EndOfElection)
                    SetVictoryImage(BrownElection);
                else
                    SetVictoryImage(BrownGameOver);
            }
            else if (winner == "Red")
            {
                if (EndOfElection)
                    SetVictoryImage(RedElection);
                else
                    SetVictoryImage(RedGameOver);
            }
            else if (winner == "Yellow")
            {
                if (EndOfElection)
                    SetVictoryImage(YellowElection);
                else
                    SetVictoryImage(YellowGameOver);
            }
            else if (winner == "Green")
            {
                if (EndOfElection)
                    SetVictoryImage(GreenElection);
                else
                    SetVictoryImage(GreenGameOver);
            }
            else if (winner == "Purple")
            {
                if (EndOfElection)
                    SetVictoryImage(PurpleElection);
                else
                    SetVictoryImage(PurpleGameOver);
            }
        }
        else
        {
            //A special screen for the case where no one voted
            SetVictoryImage(NoWinner);
        }//else

        VictoryScreen.SetActive(true);

        //Prints the players scores in the correct order 
        for (int i = 0; i < gameController.numberPlayers; i++)
        {
            playerText.text += (i + 1) + ". Player " + (playerArray[i] + 1) + " " + gameController.Players[playerArray[i]].GetComponent<PlayerVariables>().politicalPartyName + " Party\n\n";
            voteTotalText.text += gameController.Players[playerArray[i]].GetComponent<PlayerVariables>().votes + "\n\n";
            victoryPointTotalText.text += gameController.Players[playerArray[i]].GetComponent<PlayerVariables>().victoryPoints + "\n\n";
            moneyTotalText.text += gameController.Players[playerArray[i]].GetComponent<PlayerVariables>().money + "\n\n";
        }//for

        //Enables the victory screens when everything else is ready (AAJ)
        EndGameObject.SetActive(true);

    }//displayWinner

    /// <summary>
    /// This will close the window when the user presses the continue button (Alex Jungroth)
    /// </summary>
    public void dismissWindow()
    {
        //Diables the victory screens
        EndGameObject.SetActive(false);
        VictoryScreen.SetActive(false);

        //Clears the player text
        playerText.text = "";
        voteTotalText.text = "";
        victoryPointTotalText.text = "";
        moneyTotalText.text = "";

        //Resets the player array (AAJ)
        for (int i = 0; i < 5; i++)
        {
            playerArray[i] = i;
        }//for

    }//dismissWindow

    public void SetVictoryImage(Sprite imageToSet) 
    {
        VictoryScreen.GetComponent<Image>().sprite = imageToSet;
    }

}