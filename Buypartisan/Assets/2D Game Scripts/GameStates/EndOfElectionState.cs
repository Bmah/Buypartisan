using UnityEngine;
using UnityEngine.UI;
using System.Collections;


namespace GameStates
{
    public class EndOfElectionState : AbstractGameObjectState
    {
        BoardGameController gameController;

        private int NumOfPlayers;
        private GameObject[] Players;

        private float TimeUntilDisplay = 1.5f;
        private float elapsedTime;

        private bool notToggled = true;

        private int PartyName = 1;
        private int Votes = 2;
        private int Money = 3;

        public EndOfElectionState(MonoBehaviour parent) : base(parent)
        {
            gameController = (BoardGameController)parent;

            NumOfPlayers = gameController.NumberOfPlayers;
            Players = gameController.Players;

            UpdateText();

            elapsedTime = 0.0f;

        }

        // Update is called once per frame
        public override void Update()
        {

            if (elapsedTime >= TimeUntilDisplay && notToggled)
            {
                gameController.ToggleEndOfElection(true);

                notToggled = false;
            }

            elapsedTime += Time.deltaTime;

            if (gameController.continueGame)
            {
                gameController.continueGame = false;
                gameController.CurrentElection++;
                gameController.ToggleEndOfElection(false);
                gameController.currentState = new GameStates.RoundState(gameController);
            }
        }

        void UpdateText()
        {
            Text[] Player1 = gameController.Player1_Text.GetComponentsInChildren<Text>();
            Text[] Player2 = gameController.Player2_Text.GetComponentsInChildren<Text>();
            Text[] Player3 = gameController.Player3_Text.GetComponentsInChildren<Text>();

            Player1[PartyName].text = Players[0].GetComponent<Player>().PartyName;
            Player1[Votes].text = Players[0].GetComponent<Player>().victoryPoints.ToString();
            Player1[Money].text = Players[0].GetComponent<Player>().CurMoney.ToString();

            Player2[PartyName].text = Players[1].GetComponent<Player>().PartyName;
            Player2[Votes].text = Players[1].GetComponent<Player>().victoryPoints.ToString();
            Player2[Money].text = Players[1].GetComponent<Player>().CurMoney.ToString();

            if (gameController.NumberOfPlayers >= 3)
            {
                Player3[PartyName].text = Players[2].GetComponent<Player>().PartyName;
                Player3[Votes].text = Players[2].GetComponent<Player>().victoryPoints.ToString();
                Player3[Money].text = Players[2].GetComponent<Player>().CurMoney.ToString();
            }
            else
                gameController.Player3_Text.SetActive(false);


        }


    }
}