using UnityEngine;
using System.Collections;

namespace GameStates
{
    public class PlacePlayerState : AbstractGameObjectState
    {
        BoardGameController gameController;

        private int currentPlayer;
        private bool CurrentPlayerHasSpanwed = false;

        public PlacePlayerState (MonoBehaviour parent) : base(parent)
        {
            gameController = (BoardGameController)parent;
            currentPlayer = 0;
            gameController.camController.ToggleCamControls(true, false);
        }

        // Update is called once per frame
        public override void Update()
        {
            if(!gameController.IsCamMoving)
            {
                if(!CurrentPlayerHasSpanwed)
                {
                    Debug.Log("SPAWNING PLAYER");
                    int partyToSelect = gameController.PlayerPartyMapping[currentPlayer];
                    GameObject NewPlayer =  MonoBehaviour.Instantiate(gameController.PlayerPrefabs[partyToSelect], new Vector3(0.5f, 0.1f, 0.5f), Quaternion.identity) as GameObject;
                    NewPlayer.GetComponent<Player>().SetupPlayer(currentPlayer, gameController);
                    CurrentPlayerHasSpanwed = true;
                }
            }
        }
    }
}