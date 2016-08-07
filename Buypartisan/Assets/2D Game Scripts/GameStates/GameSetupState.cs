using UnityEngine;
using System.Collections;

namespace GameStates
{
    public class GameSetupState : AbstractGameObjectState
    {
        private BoardGameController gameController;

        public GameSetupState(MonoBehaviour parent) : base(parent)
        {
            gameController = (BoardGameController)parent;
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
}
