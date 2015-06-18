using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Game controller.
/// Brian Mah
/// </summary>

public class GameController : MonoBehaviour {
	public enum GameState {PlayerSpawn, ActionTurns, RoundEnd, GameEnd, AfterEnd};
	GameState currentState = GameState.PlayerSpawn;

	public GameObject voterTemplate;
	public GameObject playerTemplate;
	
	public Button[] playerPlacmentButtons;
	public Image[] playerPlacmentButtonImages;
	
	public int gridSize;
	public GridInstanced GridInstancedController;
	//public VoterVariables VoterVariablesController;
	public int numberPlayers;  //number of players per game
	public int playersSpawned = 0; //how many players have been spawned in
	private bool spawnedNewPlayer = false; //bool for checking whether or not a new player has been spawned in
	public bool playerConfirmsPlacment = false; //bool for checking if player is done
	
	private int currentPlayerTurn = 0; //this keeps track of which player is currently taking a turn
	public int numberOfTurns; //this is a variable that you can change to however many number if turns we want.
	private int turnCounter = 0;//will be used to keep track of turns
	public bool playerTakingAction = false;
	public bool messaged;//Checks if Player has finished taking an acti
	
	public GameObject[] voters = new GameObject[2];//array which houses the voters
	public GameObject[] players = new GameObject[2];//array which houses the players
	
	public GameObject currentPlayer;
	
	/// <summary>
	/// Start this instance.
	/// Adds in Voter Array
	/// </summary>
	void Start () {
		//VoterVariables VoterVariablesController = GameObject.FindGameObjectWithTag("Voter(Clone)").GetComponent<GameController>();
		GridInstancedController.GridInstantiate (gridSize);
		messaged = true;
		SpawnVoters ();
	}
	
	/// <summary>
	/// Spawns the voters according to map
	/// </summary>
	void SpawnVoters(){
		Vector3 voterLocation = new Vector3(1,3,2);
		voters[0] = Instantiate (voterTemplate, voterLocation, Quaternion.identity) as GameObject;
		voters [0].GetComponent<VoterVariables> ().votes = 3;
		voterLocation = new Vector3 (0,2,3);
		voters[1] = Instantiate (voterTemplate, voterLocation, Quaternion.identity) as GameObject;
		voters [1].GetComponent<VoterVariables> ().votes = 3;
	}
	
	// Update is called once per frame
	void Update () {

		if (currentState == GameState.PlayerSpawn) {
			if (playersSpawned < numberPlayers) { //Players are still spawning in
				SpawnPlayer ();
			} else {
				currentState = GameState.ActionTurns;
				playerTakingAction = false;
				Debug.Log ("Turn " + (turnCounter + 1) + " begin!");
				Debug.Log ("It's Player " + (currentPlayerTurn + 1) + "'s turn!");
			}
		} else if (currentState == GameState.ActionTurns) {
			// In Game Heirchy, GameController must set Number Of Turns greater than 0 in order for this to be called
			if (turnCounter < numberOfTurns) {
				PlayerTurn ();
				if (Input.GetKeyDown (KeyCode.P))
					playerTakingAction = true;//this stops the player 1 turn form infinitely looping

			} else {
				currentState = GameState.RoundEnd;
			}
			
		} else if (currentState == GameState.RoundEnd) {
			for (int i = 0; i < voters.Length; i++) {
				float leastDistance = 1000f;
				int closestPlayer = 0;
				float tieDistance = 1000f;
				int tiePlayer = 0;
				for (int j = 0; j < players.Length; j++) {//calculates the distance of voters from players
					Vector3 distanceVector = players [j].transform.position - voters [i].transform.position;
					float distance = Mathf.Abs (distanceVector.x) + Mathf.Abs (distanceVector.y) + Mathf.Abs (distanceVector.z);
					if (distance < leastDistance) {//determines if there is a player that beat the last one
						leastDistance = distance;
						closestPlayer = j;
					} else if (distance == leastDistance) {//creates a tie between two players (3 way ties can suck it)
						tieDistance = distance;
						tiePlayer = j;
					}
					
				}
				if (tieDistance == leastDistance) {//checks if least distance is still tied with the tie player, if not, it is shorter, so don't split
					Debug.Log ("Checking if least distance is still tied with the tied player...if not, it's shorter so don't split votes");
					players [closestPlayer].GetComponent<PlayerVariables> ().votes += voters [i].GetComponent<VoterVariables> ().votes / 2;
					players [tiePlayer].GetComponent<PlayerVariables> ().votes += voters [i].GetComponent<VoterVariables> ().votes / 2;
					players [closestPlayer].GetComponent<PlayerVariables> ().money += voters [i].GetComponent<VoterVariables> ().money / 2;
					players [tiePlayer].GetComponent<PlayerVariables> ().money += voters [i].GetComponent<VoterVariables> ().money / 2;
				} else {//do normal assignments if least distance is not tied
					players [closestPlayer].GetComponent<PlayerVariables> ().votes += voters [i].GetComponent<VoterVariables> ().votes;
					players [closestPlayer].GetComponent<PlayerVariables> ().money += voters [i].GetComponent<VoterVariables> ().money;
				}

				//Sets the gamemode to game end, and calculates the final score
				currentState = GameState.GameEnd;
			}
		// Once the game ends and calculation is needed, this is called
		} else if (currentState == GameState.GameEnd) {
			CompareVotes(messaged);
		}

	}// Update
	
	
	/// <summary>
	/// Spawns the player and enables player placment controlls.
	/// Disables controlls upon confirmation and last player placed
	/// </summary>
	void SpawnPlayer(){
		if (!spawnedNewPlayer) {
			currentPlayer = Instantiate(playerTemplate,new Vector3(0,0,0), Quaternion.identity) as GameObject;
			players[playersSpawned] = currentPlayer;
			spawnedNewPlayer = true;
			playerConfirmsPlacment = false;
		}
		
		if (!playerPlacmentButtons [0].enabled) {
			for(int i = 0; i < playerPlacmentButtons.Length; i++){
				playerPlacmentButtons[i].enabled = true;
				playerPlacmentButtonImages[i].enabled = true;
			}
		}
		
		//Player Uses Buttons to choose where the player goes in the scene
		
		if (playerConfirmsPlacment) {
			//checks the player against all of the previous players to ensure no duplicates
			for(int i = 0; i < playersSpawned; i++){
				if (currentPlayer.transform.position == players[i].transform.position){//if they are on the same spot
					playerConfirmsPlacment = false;
				}
			}
			if(playerConfirmsPlacment){ //if the player placment is legal
				playersSpawned++;
				spawnedNewPlayer = false;
				playerConfirmsPlacment = false;
			}
		}
	}//SpawnPlayer
	
	/// <summary>
	/// Players turn.
	/// </summary>
	void PlayerTurn(){
		if (playerTakingAction) {
			currentPlayerTurn++;
			if (currentPlayerTurn < numberPlayers) {
				playerTakingAction = false;
				Debug.Log ("It's Player " + (currentPlayerTurn + 1) + "'s turn!");
			}
			if (currentPlayerTurn >= numberPlayers) {
				//this is when all players have made their turns
				turnCounter++;
				currentPlayerTurn = 0;
				playerTakingAction = false;
				
				if (turnCounter < numberOfTurns) {
					Debug.Log ("Turn " + (turnCounter + 1) + " begin!");
					Debug.Log ("It's Player " + (currentPlayerTurn + 1) + "'s turn!");
				} else {
					Debug.Log ("Round Ends!");
				}
			}
		}
	}

	void CompareVotes(bool messaged){

		int mostVotes = 0;
		int winningPlayer = 0;
		int tieVotes = 0;
		int tieFighter = 0;//player that ties
		
		
		for(int i = 0; i < players.Length; i++){
			if(players[i].GetComponent<PlayerVariables>().votes > mostVotes){
				mostVotes = players[i].GetComponent<PlayerVariables>().votes; 
				winningPlayer = i;
			}
			else if(players[i].GetComponent<PlayerVariables>().votes == mostVotes) {
				//Debug.Log ("here");
				tieVotes = players[i].GetComponent<PlayerVariables>().votes;
				tieFighter = i;
			}
		}
		if(messaged && mostVotes == tieVotes){
			Debug.Log ("Winning Players are " + winningPlayer +" and " + tieFighter + " with a tie vote of: " + tieVotes + "!");
			messaged = false;
		}
		else if(messaged){
			Debug.Log("Winning Player is: " + winningPlayer + " with " + mostVotes + " votes!");
			messaged = false;
		}

		// Brings us to the results UI with all the information displayed
		currentState = GameState.AfterEnd;
	}
	/// <summary>
	/// So, this is the skeleton for the power functions.  Every power will be assigned an int, and checked for either though 
	/// a switch statement or simple if checks. This function is called through a variety of means, although right now, only through
	/// clicking on a voter.  Once its called, it checks int power for the power to execute, and then executes the code contained in the 
	/// block.
	/// </summary>
	/// <param name="power">Power.</param>
	public void PowerCall(int power) {
		if (power == 1) {//VOTER SUPPRESSION
			for (int i = 0; i < voters.Length; i++) {
				if (voters[i].GetComponent<VoterVariables>().GetSelected())//looks through voters to get the one selected
					voters [i].GetComponent<VoterVariables> ().votes = 0;//sets votes equal to zero
			}
		}
		//TODO: implement the rest of the powers, which will be further increments of power
	}
	
}//Gamecontroller Class
