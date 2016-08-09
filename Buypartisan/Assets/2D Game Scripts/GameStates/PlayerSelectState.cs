using UnityEngine;
using System.Collections;

//Tyler Welsh - 2016
/*
    Player Select State

*/

namespace GameStates
{
    public class PlayerSelectState : AbstractGameObjectState
    {
        //The main game controller
        private BoardGameController gameController;
        //Is Player Select Screen Enabled?
        private bool ScreenEnabled;
        //Number of Players in the game
        private float NumOfPlayers;

        private int CurrentPlayerSelect;

        //Constructor to instantiate the player select state and assign the parent controller and needed variables
        public PlayerSelectState(MonoBehaviour parent) : base(parent)
        {
            gameController = (BoardGameController)parent;
            gameController.PlayerSelectStartCam();
            ScreenEnabled = false;
            NumOfPlayers = gameController.NumberOfPlayers;
            gameController.Players = new GameObject[(int)NumOfPlayers];
            gameController.PlayerPartyMapping = new int[(int)NumOfPlayers];
            CurrentPlayerSelect = 0;
            Debug.Log("Game Setup");
        }

        public override void Update()
        {
            //If camera is finished moving and player select isn't enabled
            if(!gameController.IsCamMoving && !ScreenEnabled)
            {
                //Enable player select info
                gameController.EnablePlayerSelect();
                ScreenEnabled = true;
            }

            //When left mouse click, shoot raycast
            if (Input.GetMouseButtonDown(0) && !gameController.IsCamMoving)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                //TODO: ADD CONFIRMATION SELECT

                if (Physics.Raycast(ray, out hit)) 
                {

                    Debug.DrawLine(ray.origin, hit.point);
                    if(hit.transform.tag == ("Orange_Party"))
                    {
                        gameController.PlayerPartyMapping[CurrentPlayerSelect] = BoardGameController.Orange_Party;
                        gameController.PlayerSelectGameModels[BoardGameController.Orange_Party].SetActive(false);
                        Debug.Log("Current Player: " + CurrentPlayerSelect + " Selected Party: " + gameController.PlayerPartyMapping[CurrentPlayerSelect]);
                        CurrentPlayerSelect++;
                    }
                    else if(hit.transform.tag == ("Green_Party"))
                    {
                        gameController.PlayerPartyMapping[CurrentPlayerSelect] = BoardGameController.Green_Party;
                        gameController.PlayerSelectGameModels[BoardGameController.Green_Party].SetActive(false);
                        Debug.Log("Current Player: " + CurrentPlayerSelect + " Selected Party: " + gameController.PlayerPartyMapping[CurrentPlayerSelect]);
                        CurrentPlayerSelect++;
                    }
                    else if(hit.transform.tag == ("Purple_Party"))
                    {
                        gameController.PlayerPartyMapping[CurrentPlayerSelect] = BoardGameController.Purple_Party;
                        gameController.PlayerSelectGameModels[BoardGameController.Purple_Party].SetActive(false);
                        Debug.Log("Current Player: " + CurrentPlayerSelect + " Selected Party: " + gameController.PlayerPartyMapping[CurrentPlayerSelect]);
                        CurrentPlayerSelect++;
                    }
                }
            }

            //STATE CHANGE
            if(CurrentPlayerSelect >= NumOfPlayers)
            {
                //FINISHED PLAYER SELECT
                gameController.DisablePlayerSelect();
                gameController.PlayerSelectEndCam();
                gameController.currentState = new GameSetupState(gameController);

            }

        }
    }
}