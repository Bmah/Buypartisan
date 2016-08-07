using UnityEngine;
using System.Collections;

namespace GameStates
{
    public class BeforePlayState : AbstractGameObjectState
    {
        //Main Board Game Controller
        private BoardGameController gameController;
        //Main GUI Controller
        private GUIController GUI;
        //Main Camera Object
        private GameObject MainCam;

        public BeforePlayState(MonoBehaviour parent) : base(parent)
        {
            //Assign the parent script
            gameController = (BoardGameController)parent;
            //Get Main Cam
            MainCam = gameController.MainCam;
            //Set the Cam Rotater to true. Rotates camera around the board
            MainCam.GetComponent<CamController>().CamRotater = true;
        }

        // Update is called once per frame
        public override void Update()
        { 
            //If game has begun, disable the camera rotater and set the next state
            if(gameController.GameHasBegun)
            {
                //Disables board game cam rotater
                MainCam.GetComponent<CamController>().CamRotater = false;
                //Sets next state to player select
                gameController.currentState = new PlayerSelectState(gameController);
            }
        }
    }
}
