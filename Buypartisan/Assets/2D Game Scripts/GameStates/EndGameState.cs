using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace GameStates
{
    public class EndGameState : AbstractGameObjectState
    {
        BoardGameController gameController;

        private float TimeUntilDisplay = 1.5f;
        private float elapsedTime;
        private int winnerNum = -1;

        private bool notToggled = true;

        private int PartyName = 0;
        private int Votes = 1;
        private int Money = 2;

        //TODO DOES NOT ACCOUNT FOR TIES

        public EndGameState(MonoBehaviour parent) : base(parent)
        {
            gameController = (BoardGameController)parent;
            notToggled = true;
            elapsedTime = 0;
            CalculateWinner();
            UpdateWinnerText();
        }

        void UpdateWinnerText()
        {
            Text[] WinnerValues = gameController.Values_Text.GetComponentsInChildren<Text>();
            Debug.Log("Got text assets: " + WinnerValues.Length);
            Player Winner;
            if (winnerNum >= 0)
                Winner = gameController.Players[winnerNum].GetComponent<Player>();
            else
            {
                Winner = null;
                Debug.LogError("NO WINNER");
            }

            WinnerValues[PartyName].text = Winner.PartyName;
            WinnerValues[Votes].text = Winner.victoryPoints.ToString();
            WinnerValues[Money].text = Winner.CurMoney.ToString();

            gameController.Winner_Text.GetComponent<Text>().text = "Player " + (winnerNum + 1).ToString();

        }

        public override void Update()
        {
            if (elapsedTime >= TimeUntilDisplay && notToggled)
            {
                gameController.ToggleEndOfGame(true);
                notToggled = false;
            }
            else
                elapsedTime += Time.deltaTime;
        }

        void CalculateWinner()
        {
            int curWinner = -1;
            int highestVotes = -1;

            for (int p = 0; p < gameController.Players.Length; p++)
            {
                Player curPlayer = gameController.Players[p].GetComponent<Player>();

                Debug.Log("Cur player: " + curPlayer.PartyName);

                if (curPlayer.victoryPoints > highestVotes)
                {
                    curWinner = curPlayer.PlayerID;
                    highestVotes = curPlayer.victoryPoints;
                    Debug.Log("Cur Winnner Is: " + curWinner);
                }
            }

            winnerNum = curWinner;
            if (winnerNum < 0)
                Debug.LogError("Error, got no winner");
        }
    }
}