﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace GameStates
{
    public class RoundState : AbstractGameObjectState
    {
        private BoardGameController gameController;
        private ActionController actionController;

        private int currentPlayer;
        private GameObject Player;
        private GameObject[] Players;
        private GameObject[] Voters;

        private int SelectedAction;
        private bool ActionIsReady;
        private GameObject currentAction;

        private int CurRound;
        private int CurElection;

        private Text[] DisplayInfo = new Text[4];

        public RoundState(MonoBehaviour parent) : base(parent)
        {
            gameController = (BoardGameController)parent;
            actionController = gameController.ActionControl;

            gameController.ToggleInfoDisplay(true);
            gameController.ToggleActionDisplay(true);
            gameController.ToggleTurnPanel(true);
            gameController.ToggleTooltipPanel(true);

            currentPlayer = 0;
            CurRound = 0;
            CurElection = gameController.CurrentElection;

            ActionIsReady = false;
            currentAction = null;

            Players = gameController.Players;
            Player = Players[currentPlayer];
            Voters = gameController.Voters;

            DisplayInfo = gameController.InfoDisplayScreen.GetComponentsInChildren<Text>();
            UpdateInfoDisplay();
            gameController.TooltipPanel.GetComponentInChildren<Text>().text = "Select an action to the left.\n Mouse over a voter to see more info.";

            DetermineVoterParty();
        }

        public override void Update()
        {

            UpdateInfoDisplay();

            if (CurRound >= gameController.NumOfRounds)
            {
                if (gameController.CurrentElection >= gameController.NumOfElections)
                {
                    gameController.ToggleInfoDisplay(false);
                    gameController.ToggleActionDisplay(false);
                    gameController.ToggleTurnPanel(false);
                    gameController.ToggleTooltipPanel(false);
                    gameController.currentState = new GameStates.EndGameState(gameController);
                }
                else
                {
                    gameController.currentState = new GameStates.EndOfElectionState(gameController);
                    gameController.ToggleInfoDisplay(false);
                    gameController.ToggleActionDisplay(false);
                    gameController.ToggleTurnPanel(false);
                    gameController.ToggleTooltipPanel(false);

                }
            }

            if (gameController.NumActionSelected >= 0)
            {
                if (!ActionIsReady)
                {
                    if (gameController.NumActionSelected == 0)
                    {
                        //currentAction = Instantiate the game object
                        if (Player.GetComponent<Player>().CurMoney >= gameController.Action_0_Cost)
                        {
                            currentAction = actionController.StartAction(gameController.NumActionSelected, currentPlayer);
                            ActionIsReady = true;
                        }
                    }
                    else if (gameController.NumActionSelected == 1)
                    {
                        if (Player.GetComponent<Player>().CurMoney >= gameController.Action_1_Cost)
                        {
                            currentAction = actionController.StartAction(gameController.NumActionSelected, currentPlayer);
                            ActionIsReady = true;
                        }
                    }
                    else if (gameController.NumActionSelected == 2)
                    {
                        if (Player.GetComponent<Player>().CurMoney >= gameController.Action_2_Cost)
                        {
                            currentAction = actionController.StartAction(gameController.NumActionSelected, currentPlayer);
                            ActionIsReady = true;
                        }
                    }
                    else
                    {
                        Debug.Log("Invalid Action or Not ");
                        gameController.NumActionSelected = -1;
                    }
                }
                
            }
            else
            {
                //Debug.Log("Action Ended");
                ActionIsReady = false;
                currentAction = null;
            }

            if (gameController.endTurn)
            {
                currentPlayer++;
                if (currentPlayer >= gameController.NumberOfPlayers)
                {
                    //End of round
                    TallyVotes();
                    currentPlayer = 0;
                    Player = Players[currentPlayer];
                    CurRound++;
                    
                }
                else
                {
                    //Next player
                    Player = Players[currentPlayer];
                }
                gameController.endTurn = false;
            }

        }

        public void DetermineVoterParty()
        {
            float CurrentPlayerDist;

            for(int v = 0; v < Voters.Length; v++)
            {
                for (int p = 0; p < Players.Length; p++)
                {
                    CurrentPlayerDist = (Voters[v].transform.position - Players[p].transform.position).magnitude;
                    //Debug.Log("Voter " + v + " Dist from player " + p + " " + CurrentPlayerDist);
                    //Debug.Log("Cur Dist " + Voters[v].GetComponent<Voter>().DistanceToPlayer);
                    if (CurrentPlayerDist < Players[p].GetComponent<Player>().Radius && CurrentPlayerDist < Voters[v].GetComponent<Voter>().DistanceToPlayer) 
                    {
                        //IF THE VOTER IS WITHIN THE RADIUS OF SPHERE

                        //Debug.Log("Changing Voter " + v + " to Party " + p);
                        Voters[v].GetComponent<Voter>().ChangeParty(p, CurrentPlayerDist);
                    }
                    else
                    {
                     //   Debug.Log("Changing Voter " + Voters[v].GetComponent<Voter>().VoterNum + " to default - 1");
                     //   Voters[v].GetComponent<Voter>().ChangeParty(-1, int.MaxValue);
                     // Stay Default
                    }
                }
            }
        }

        public void TallyVotes()
        {
            for(int p = 0; p < gameController.NumberOfPlayers; p++)
            {
                gameController.Players[p].GetComponent<Player>().Tally();
            }
        }


        public void UpdateInfoDisplay()
        {
            DisplayInfo[0].text = "Current Player: " + Player.GetComponent<Player>().PartyName;
            DisplayInfo[1].text = "Money: " + Player.GetComponent<Player>().CurMoney;
            DisplayInfo[2].text = "Rounds Until Election: " + (gameController.NumOfRounds - CurRound);
            DisplayInfo[3].text = "Election # " + CurElection;
        }


    }
}