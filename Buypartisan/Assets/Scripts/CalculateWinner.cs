//Alex Jungroth
using UnityEngine;
using System.Collections;

public class CalculateWinner : MonoBehaviour {

    //Holds the game Controller
    public GameController gameController;

    //Holds the winner
    [HideInInspector]
    public string winner = "No Winner";
    [HideInInspector]
    public int winnerNumber = 0;

    //Holds the number of players
    [HideInInspector]
    public int totalPlayers = 0;

    //Holds coalition A's votes
    [HideInInspector]
    public int coalitionA = 0;

    //Holds coalition B's votes
    [HideInInspector]
    public int coalitionB = 0;

    //Holds the highest vote total
    [HideInInspector]
    public int maxVotes = 0;

    //Holds the highest percentage total
    [HideInInspector]
    public int maxPercent = 0;

    //Holds the highest victory point total
    [HideInInspector]
    public int maxVictoryPoints = 0;

    public void CalculateVotes()
    {
        //resets the coalitions vote totals and all of the max values
        coalitionA = 0;
        coalitionB = 0;
        maxVotes = 0;
        maxPercent = 0;

        //gets the number of players from the game controller
        totalPlayers = gameController.numberPlayers;

        //determines the player(s) with the most votes
        for (int i = 0; i < totalPlayers; i++)
        {
            if (gameController.Players[i].GetComponent<PlayerVariables>().alignment == 1)
            {
                if (gameController.Players[i].GetComponent<PlayerVariables>().votes >= maxVotes)
                {
                    maxVotes = gameController.Players[i].GetComponent<PlayerVariables>().votes;
                    winner = gameController.Players[i].GetComponent<PlayerVariables>().politicalPartyName;
                    winnerNumber = gameController.Players[i].GetComponent<PlayerVariables>().playerNumber;
                }//if
            }//if
        }//for

        if (maxVotes > 0)
        {
            //if a single player won the election
            for (int i = 0; i < totalPlayers; i++)
            {
                if (gameController.Players[i].GetComponent<PlayerVariables>().alignment == 1)
                {
                    if (winner == gameController.Players[i].GetComponent<PlayerVariables>().politicalPartyName && winnerNumber == gameController.Players[i].GetComponent<PlayerVariables>().playerNumber)
                    {
                        gameController.Players[i].GetComponent<PlayerVariables>().victoryPoints += 10;
                    }//if
                }//if
            }//for
        }//else if

        //in the case that a coalition won this correctly updates max votes for display
        for (int i = 0; i < totalPlayers; i++)
        {
            if (gameController.Players[i].GetComponent<PlayerVariables>().politicalPartyName == winner && winnerNumber == gameController.Players[i].GetComponent<PlayerVariables>().playerNumber)
            {
                maxVotes = gameController.Players[i].GetComponent<PlayerVariables>().votes;
            }//if
        }//for

        //The Game Controller gets the information on who won (Alex Jungroth)
        gameController.WinnerName = winner;
        gameController.WinnerPlayerNum = winnerNumber;
        gameController.MaxVote = maxVotes;
    }

    /// <summary>
    /// Calculates the victory points (Alex Jungroth)
    /// </summary>
    public void CalculateVictoryPoints()
    {
        //Resets the total
        maxVictoryPoints = 0;

        //determines who won the game
        for (int i = 0; i < totalPlayers; i++)
        {
            if (gameController.Players[i].GetComponent<PlayerVariables>().victoryPoints >= maxVictoryPoints)
            {
                maxVictoryPoints = gameController.Players[i].GetComponent<PlayerVariables>().victoryPoints;
                winner = gameController.Players[i].GetComponent<PlayerVariables>().politicalPartyName;
                winnerNumber = gameController.Players[i].GetComponent<PlayerVariables>().playerNumber;
            }//if
        }//for

        //The Game Controller gets the information on who won (Alex Jungroth)
        gameController.MaxVictoryPoints = maxVictoryPoints;

    }//CalculateVictoryPoints

    /// <summary>
    /// Calculates the votes. (Alex Jungroth)
    /// </summary>
    public void CalculateCoalitionVotes()
    {
        //resets the coalitions vote totals and all of the max values
        coalitionA = 0;
        coalitionB = 0;
        maxVotes = 0;
        maxPercent = 0;

        //gets the number of players from the game controller
        totalPlayers = gameController.numberPlayers;

        //tallies the votes in the coalitions
        for (int i = 0; i < totalPlayers; i++)
        {
            if (gameController.Players[i].GetComponent<PlayerVariables>().alignment == 2)
            {
                coalitionA += gameController.Players[i].GetComponent<PlayerVariables>().votes;
            }//if
            else if (gameController.Players[i].GetComponent<PlayerVariables>().alignment == 3)
            {
                coalitionB += gameController.Players[i].GetComponent<PlayerVariables>().votes;
            }//if
        }//if

        //checks to see if the coalitions have the most votes
        if (coalitionA >= maxVotes)
        {
            maxVotes = coalitionA;
            winner = "coalitionA";
        }//if

        if (coalitionB >= maxVotes)
        {
            maxVotes = coalitionB;
            winner = "coalitionB";
        }//if

        //determines the player(s) with the most votes
        for (int i = 0; i < totalPlayers; i++)
        {
            if (gameController.Players[i].GetComponent<PlayerVariables>().alignment == 1)
            {
                if (gameController.Players[i].GetComponent<PlayerVariables>().votes >= maxVotes)
                {
                    maxVotes = gameController.Players[i].GetComponent<PlayerVariables>().votes;
                    winner = gameController.Players[i].GetComponent<PlayerVariables>().politicalPartyName;
                    winnerNumber = gameController.Players[i].GetComponent<PlayerVariables>().playerNumber;
                }//if
            }//if
        }//for

        if ((winner == "coalitionA") && (maxVotes > 0))
        {
            //if coalition A won, determines the winner in the coaliton and divides up the victory points
            for (int i = 0; i < totalPlayers; i++)
            {
                if (gameController.Players[i].GetComponent<PlayerVariables>().alignment == 2)
                {
                    gameController.Players[i].GetComponent<PlayerVariables>().victoryPoints += (int)Mathf.Ceil
                        (10 * gameController.Players[i].GetComponent<PlayerVariables>().votes / maxVotes);

                    if (gameController.Players[i].GetComponent<PlayerVariables>().votes >= maxPercent)
                    {
                        maxPercent = gameController.Players[i].GetComponent<PlayerVariables>().votes;
                        winner = gameController.Players[i].GetComponent<PlayerVariables>().politicalPartyName;
                        winnerNumber = gameController.Players[i].GetComponent<PlayerVariables>().playerNumber;
                    }//if
                }//if
            }//for
        }//if
        else if ((winner == "coalitionB") && (maxVotes > 0))
        {
            //if coaliton B won, determines the winner in the coalition and divides up the victory points
            for (int i = 0; i < totalPlayers; i++)
            {
                if (gameController.Players[i].GetComponent<PlayerVariables>().alignment == 3)
                {
                    gameController.Players[i].GetComponent<PlayerVariables>().victoryPoints += (int)Mathf.Ceil
                        (10 * gameController.Players[i].GetComponent<PlayerVariables>().votes / maxVotes);

                    if (gameController.Players[i].GetComponent<PlayerVariables>().votes >= maxPercent)
                    {
                        maxPercent = gameController.Players[i].GetComponent<PlayerVariables>().votes;
                        winner = gameController.Players[i].GetComponent<PlayerVariables>().politicalPartyName;
                        winnerNumber = gameController.Players[i].GetComponent<PlayerVariables>().playerNumber;
                    }//if
                }//if
            }//for
        }//else if
        else if (maxVotes > 0)
        {
            //if a single player won the election
            for (int i = 0; i < totalPlayers; i++)
            {
                if (gameController.Players[i].GetComponent<PlayerVariables>().alignment == 1)
                {
                    if (winner == gameController.Players[i].GetComponent<PlayerVariables>().politicalPartyName && winnerNumber == gameController.Players[i].GetComponent<PlayerVariables>().playerNumber)
                    {
                        gameController.Players[i].GetComponent<PlayerVariables>().victoryPoints += 10;
                    }//if
                }//if
            }//for
        }//else if

        //in the case that a coalition won this correctly updates max votes for display
        for (int i = 0; i < totalPlayers; i++)
        {
            if (gameController.Players[i].GetComponent<PlayerVariables>().politicalPartyName == winner && winnerNumber == gameController.Players[i].GetComponent<PlayerVariables>().playerNumber)
            {
                maxVotes = gameController.Players[i].GetComponent<PlayerVariables>().votes;
            }//if
        }//for

        //The Game Controller gets the information on who won (Alex Jungroth)
        gameController.WinnerName = winner;
        gameController.WinnerPlayerNum = winnerNumber;
        gameController.MaxVote = maxVotes;

    }//Calculate Coalition Votes
}
