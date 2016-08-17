using UnityEngine;
using System.Collections;

namespace GameStates
{
    public class EndGameState : AbstractGameObjectState
    {
        BoardGameController gameController;

        public EndGameState(MonoBehaviour parent) : base(parent)
        {
            gameController = (BoardGameController)parent;
            gameController.ToggleEndOfGame(true);
        }

        // Update is called once per frame
        public override void Update()
        {

        }

    }
}