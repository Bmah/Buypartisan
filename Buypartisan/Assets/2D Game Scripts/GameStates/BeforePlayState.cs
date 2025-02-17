﻿using UnityEngine;
using System.Collections;

namespace GameStates
{
    public class BeforePlayState : AbstractGameObjectState
    {
        //Main Board Game Controller
        private BoardGameController gameController;
        //Main GUI Controller
        private GUIController GUI;

        public BeforePlayState(MonoBehaviour parent) : base(parent)
        {
            //Assign the parent script
            gameController = (BoardGameController)parent;
            //Get Main Cam
            //Set the Cam Rotater to true. Rotates camera around the board
            gameController.camController.ToggleCamControls(false, true);
            Debug.Log("Before Game Start");

            gameController.TogglePlayerSelect(false);
            gameController.ToggleConfirmPlacement(false);
            gameController.ToggleActionDisplay(false);
            gameController.ToggleInfoDisplay(false);
            gameController.ToggleTurnPanel(false);
            gameController.ToggleTooltipPanel(false);
            gameController.ToggleEndOfElection(false);
            gameController.ToggleEndOfGame(false);
            gameController.ToggleConfirmAction(false);
            gameController.ToggleStartingScreen(true);

            gameController.NumberOfPlayers = 2;
            gameController.CurrentElection = 1;
        }

        // Update is called once per frame
        public override void Update()
        { 
            //If game has begun, disable the camera rotater and set the next state
            //STATE CHANGE
            if(gameController.GameHasBegun)
            {
                //Disables board game cam rotater
                gameController.camController.ToggleCamControls(false, false);
                //Sets next state to player select
                gameController.currentState = new PlayerSelectState(gameController);
            }
        }
    }
}
