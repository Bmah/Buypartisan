// Brian Mah
// Game Controller

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text;
using System.IO;
using System.Runtime;

public class GameController : MonoBehaviour {
	public enum GameState {PlayerSpawn, ActionTurns, RoundEnd, GameEnd, AfterEnd};
	GameState currentState = GameState.PlayerSpawn;

	public GameObject voterTemplate;
	public GameObject playerTemplate;
	
	public int gridSize;
	public GridInstanced GridInstancedController;

	public UI_Script UIController;

	public RandomEventControllerScript randomEventController;

	public int numberPlayers;  //number of players per game
	public int playersSpawned = 0; //how many players have been spawned in
	private bool spawnedNewPlayer = false; //bool for checking whether or not a new player has been spawned in
	public bool playerConfirmsPlacment = false; //bool for checking if player is done
	
	public int currentPlayerTurn = 0; //this keeps track of which player is currently taking a turn
	public int numberOfRounds; //this is a variable that you can change to however many number if rounds we want.
	private int roundCounter = 0;//will be used to keep track of rounds
	public bool playerTakingAction = false;
	public bool messaged;//Checks if Player has finished taking an acti
	
	public GameObject[] voters;//array which houses the voters
	public GameObject[] players = new GameObject[2];//array which houses the players
	
	public GameObject currentPlayer;

	private MusicController gameMusic;
	private SFXController SFX;
	public float SFXVolume;

	public bool SpawnUsingTXT = true;
	public int NumVoters = 10;
	public float VoterDistanceCheck = 1f;
	public int voterMaxMoney = 100;
	public int voterMaxVotes = 100;
	public float IgnoreNearestVoter = 0.3f;

	private bool SFXDrumrollPlaying = false;
	private float drumrollTime = 3.7f;

	//does the tallying at the start of each turn (Alex Jungroth)
	public TallyingScript tallyRoutine;

	private InputManagerScript inputManager;

	/// <summary>
	/// Start this instance.
	/// Adds in Voter Array
	/// </summary>
	void Start () {
		//VoterVariables VoterVariablesController = GameObject.FindGameObjectWithTag("Voter(Clone)").GetComponent<GameController>();
		GridInstancedController.GridInstantiate (gridSize);
		UIController.gridSize = gridSize;
		UIController.SFXvolume = SFXVolume;
		randomEventController.gridSize = gridSize;
		messaged = true;

		if (SpawnUsingTXT) {
			SpawnVotersFromTXT ();
		} else {
			SpawnUsingProbabilityMap(NumVoters,VoterDistanceCheck,voterMaxMoney,voterMaxVotes,IgnoreNearestVoter);
		}

		randomEventController.voters = voters;
		gameMusic = GameObject.FindGameObjectWithTag("Music").GetComponent<MusicController>();
		if (gameMusic == null) {
			Debug.LogError ("The Game Controller could not find the Music Controller please place it in the scene.");
		}

		SFX = GameObject.FindGameObjectWithTag("SFX").GetComponent<SFXController>();
		if (SFX == null) {
			Debug.LogError ("The Game Controller could not find the SFX Controller please place it in the scene.");
		}

		inputManager = GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManagerScript>();
		if (inputManager == null) {
			Debug.LogError ("The Game Controller could not find the Input manager please place it in the scene.");
		}
	}
	
	/// <summary>
	/// Spawns the voters according to map
	/// </summary>
	void SpawnVotersFromTXT(){
		Vector3 voterLocation = new Vector3 (0, 0, 0);
		int voterNumber = 0;

		try{
			string currentLine;
			int temp;
			StreamReader levelReader = new StreamReader(Application.dataPath + "\\level.txt",Encoding.Default);
			using(levelReader){
				currentLine = levelReader.ReadLine();
				int.TryParse(currentLine,out temp);
				voters = new GameObject[temp];

				do{
					currentLine = levelReader.ReadLine();
					if(currentLine != null){
						string[] voterDataRaw = currentLine.Split(new char[]{',',' '});
						int[] voterDataInt = new int[12];
						if(voterDataRaw.Length == 12){
							for(int i = 0; i < 12; i++){
								if(int.TryParse(voterDataRaw[i], out temp)){
									voterDataInt[i] = temp;
								}
								else{
									Debug.LogError("Could not Parse string: " + voterDataRaw[i] + "into int");
								}
							}
							voterLocation = new Vector3(voterDataInt[0],voterDataInt[1],voterDataInt[2]);
							voters[voterNumber] = Instantiate (voterTemplate, voterLocation, Quaternion.identity) as GameObject;
							voters [voterNumber].GetComponent<VoterVariables> ().votes = voterDataInt[3];
							voters [voterNumber].GetComponent<VoterVariables> ().money = voterDataInt[4];

							//These get the resistance variables for a voter (Alex Jungroth)
							voters [voterNumber].GetComponent<VoterVariables> ().baseResistance = voterDataInt[5];
							voters [voterNumber].GetComponent<VoterVariables> ().xPlusResistance = voterDataInt[6];
							voters [voterNumber].GetComponent<VoterVariables> ().xMinusResistance = voterDataInt[7];
							voters [voterNumber].GetComponent<VoterVariables> ().yPlusResistance = voterDataInt[8];
							voters [voterNumber].GetComponent<VoterVariables> ().yMinusResistance = voterDataInt[9];
							voters [voterNumber].GetComponent<VoterVariables> ().zPlusResistance = voterDataInt[10];
							voters [voterNumber].GetComponent<VoterVariables> ().zMinusResistance = voterDataInt[11];

							voterNumber++;
						}
						else{
							Debug.LogError("Incorrect number of inputs into level.txt on line:\n" + currentLine);
						}
					}
				}
				while(currentLine != null);

				levelReader.Close();
			}

		}
		catch(IOException e){
			Debug.LogError("ERROR did not load file properly Exception: " + e);
		}
	}

	private void SpawnUsingProbabilityMap(int numberofVoters, float distanceToNearestVoter, int voterMaxMoney, int voterMaxVotes,
	                                      float probabilityToIgnoreNearestVoter){
		Vector3 voterLocation = new Vector3 (0, 0, 0);
		VoterVariables voterInfoTemp;
		bool uniqueLocation = false;
		float moneyToVotesRatio;

		voters = new GameObject[numberofVoters];

		for (int i = 0; i < numberofVoters; i++) {
			//until a unique location is found continually search for a new position.
			uniqueLocation = false;
			while(!uniqueLocation){
				voterLocation = new Vector3(Random.Range(0,gridSize),Random.Range(0,gridSize),Random.Range(0,gridSize));
				uniqueLocation = true;
				for(int j = 0; j < i; j++){
					if ((voters[j].transform.position - voterLocation).magnitude < distanceToNearestVoter){
						uniqueLocation = false;
					}
				}
				if(Random.value < probabilityToIgnoreNearestVoter){
					uniqueLocation = true;
				}
				for(int j = 0; j < i; j++){
					if (voters[j].transform.position == voterLocation){
						uniqueLocation = false;
					}
				}
			}

			voters[i] = Instantiate (voterTemplate, voterLocation, Quaternion.identity) as GameObject;
			voterInfoTemp = voters[i].GetComponent<VoterVariables>();
			moneyToVotesRatio = Random.value;
			voterInfoTemp.money = Mathf.RoundToInt(voterMaxMoney*moneyToVotesRatio);
			voterInfoTemp.votes = Mathf.RoundToInt(voterMaxVotes*(1-moneyToVotesRatio));

			voterInfoTemp.xMinusResistance = Random.value*Random.value;
			voterInfoTemp.xPlusResistance = Random.value*Random.value;
			voterInfoTemp.yMinusResistance = Random.value*Random.value;
			voterInfoTemp.yPlusResistance = Random.value*Random.value;
			voterInfoTemp.zMinusResistance = Random.value*Random.value;
			voterInfoTemp.zPlusResistance = Random.value*Random.value;
			voterInfoTemp.baseResistance = 0;

		}

	}

	
	// Update is called once per frame
	void Update () {

		if (currentState == GameState.PlayerSpawn) {
			if (playersSpawned < numberPlayers) { //Players are still spawning in
				SpawnPlayer ();
			} else {
				currentState = GameState.ActionTurns;
				playerTakingAction = false;
				Debug.Log ("Round " + (roundCounter + 1) + " begin!");
				Debug.Log ("It's Player " + (currentPlayerTurn + 1) + "'s turn!");

				//does the tallying before the first player's turn starts (Alex Jungroth)
				tallyRoutine.preTurnTalling ();
			
			}
		} else if (currentState == GameState.ActionTurns) {

			// In Game Heirchy, GameController must set Number Of Rounds greater than 0 in order for this to be called
			if (roundCounter < numberOfRounds) {
				PlayerTurn ();

				if (Input.GetKeyDown (KeyCode.P))
					playerTakingAction = true;//this skips the current turn by ending the turn.
			} else {
				currentState = GameState.RoundEnd;

			}
			
		} else if (currentState == GameState.RoundEnd) {

			// Brian Mah
			UIController.alterTextBox("And the Winner is...");

			if(!SFXDrumrollPlaying){
				SFX.PlayAudioClip(2,0,SFXVolume);
				SFXDrumrollPlaying = true;
				drumrollTime += Time.time;
			}

			if(Time.time >= drumrollTime){ // when the sound is done playing

				//does the tallying at the end of the game (Alex Jungroth)
				tallyRoutine.preTurnTalling ();

				//Sets the gamemode to game end, and calculates the final score
				currentState = GameState.GameEnd;
			}

		// Once the game ends and calculation is needed, this is called
		} else if (currentState == GameState.GameEnd) {
			CompareVotes(messaged);
		}
		
		if (inputManager.escButtonDown) {
			Application.LoadLevel("TitleScene");
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
				if(!(playersSpawned < numberPlayers)){
					UIController.disablePPButtons();
					//adding this in to enable action buttons/test functionality (Alex Jungroth)
					UIController.toggleActionButtons();
				}
			}
		}
	}//SpawnPlayer
	
	/// <summary>
	/// Players turn.
	/// So this system works similarly to Brian's player placement Script.
	/// It first keeps track of which player is currently taking an action. 
	/// When the action is finished, it moves on to the next player until all players have moved.
	/// When all players have moved, it ends the round, and continues till all rounds are done.
	/// NOTE: Make sure that you set the number of Rounds in the scene editor. Default is 0 right now.
	/// Another note, in order to end the turn, an outside script needs to tell the Game Controller that "playerTakingAction" is true for the turn to end.
	/// For now, just press P to end a turn.
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

                //does the tallying after the players ends there turns (Alex Jungroth)
                tallyRoutine.preTurnTalling();

				randomEventController.ActivateEvents();

				//this is when the new round begins
				roundCounter++;
				currentPlayerTurn = 0;
				playerTakingAction = false;
				
				if (roundCounter < numberOfRounds) {
					Debug.Log ("Round " + (roundCounter + 1) + " begin!");
					Debug.Log ("It's Player " + (currentPlayerTurn + 1) + "'s turn!");
				} else {
					Debug.Log ("Game Ends!");
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
			UIController.alterTextBox("Winning Players are " + winningPlayer +" and " + tieFighter + " with a tie vote of: " + tieVotes + "!");
			messaged = false;
		}
		else if(messaged){
			Debug.Log("Winning Player is: " + winningPlayer + " with " + mostVotes + " votes!");
			UIController.alterTextBox("Winning Player is: " + winningPlayer + " with " + mostVotes + " votes!");
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
