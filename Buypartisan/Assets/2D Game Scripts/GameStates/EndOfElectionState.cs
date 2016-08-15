using UnityEngine;
using System.Collections;


namespace GameStates
{
    public class EndOfElectionState : AbstractGameObjectState
    {
        BoardGameController gameController;

        public EndOfElectionState(MonoBehaviour parent) : base(parent)
        {
            gameController = (BoardGameController)parent;

        }

        // Update is called once per frame
        public override void Update()
        {

        }

        void CalculateWinner()
        {

        }
    }
}