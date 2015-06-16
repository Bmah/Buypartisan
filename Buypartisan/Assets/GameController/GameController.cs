using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Game controller.
/// Brian Mah
/// </summary>

public class GameController : MonoBehaviour {

	public GameObject voterTemplate;
	public GameObject playerTemplate;

	public Button[] playerPlacmentButtons;
	public Image[] playerPlacmentButtonImages;

	public int numberPlayers;  //number of players per game
	private int playersSpawned = 0; //how many players have been spawned in
	private bool spawnedNewPlayer = false; //bool for checking whether or not a new player has been spawned in
	private bool playerConfirmsPlacment = false; //bool for checking if player is done

	private int turnCounter = 0;

	public GameObject[] voters = new GameObject[10];//array which houses the voters
	public GameObject[] players = new GameObject[4];//array which houses the players

	public GameObject currentPlayer;
	
	/// <summary>
	/// Start this instance.
	/// Adds in Voter Array
	/// </summary>
	void Start () {
		SpawnVoters ();
	}

	/// <summary>
	/// Spawns the voters according to map
	/// </summary>
	void SpawnVoters(){
		Vector3 voterLocation = new Vector3(0,0,0);
		voters[0] = Instantiate (voterTemplate, voterLocation, Quaternion.identity) as GameObject;
		voterLocation = new Vector3 (0,2,0);
		voters[1] = Instantiate (voterTemplate, voterLocation, Quaternion.identity) as GameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if (playersSpawned < numberPlayers) { //Players are still spawning in
			SpawnPlayer();
		} 
		else { // Players are done spawning

			//Turns go here
			//*INCOMPLETE*//
			//currentPlayer = players[turnCounter];
			//PlayerTurn();

			for(int i = 0; i < voters.Length; i++) {
				float leastDistance = 1000f;
				int closestPlayer = 0;
				for(int j = 0; j < players.Length; j++){
					Vector3 distanceVector = players[0].transform.position - voters[2].transform.position;
					float distance = Mathf.Abs(distanceVector.x) + Mathf.Abs(distanceVector.y) + Mathf.Abs(distanceVector.z);
					if(distance < leastDistance){
						leastDistance = distance;
						closestPlayer = j;
					}
				}
				players[closestPlayer].GetComponent<PlayerVariables>().votes += voters[i].GetComponent<VoterVariables>().votes;
				players[closestPlayer].GetComponent<PlayerVariables>().money += voters[i].GetComponent<VoterVariables>().money;
			}

			int mostVotes = 0;
			int winningPlayer = 0;

			//no Tie functionality as of yet
			for(int i = 0; i < players.Length; i++){
				if(players[i].GetComponent<PlayerVariables>().votes > mostVotes){
					mostVotes = players[i].GetComponent<PlayerVariables>().votes; 
					winningPlayer = i;
				}
			}

			Debug.Log("Winning Player is: " + winningPlayer + "!");
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
			playersSpawned++;
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
				spawnedNewPlayer = false;
			}
		}
	}//SpawnPlayer

	/// <summary>
	/// Players turn.
	/// </summary>
	void PlayerTurn(){
		//do player turn stuff
		//currentPlayer is the player that will be affected
		//*INCOMPLETE*//
	}

}//Gamecontroller Class
