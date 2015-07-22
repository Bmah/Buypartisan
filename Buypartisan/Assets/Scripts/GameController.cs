// Brian Mah
// Game Controller

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text;
using System.IO;
using System.Runtime;

public class GameController : MonoBehaviour {
	public enum GameState {TotalPlayersSelect,PlayerSpawn, ActionTurns, RoundEnd, GameEnd, EnactPolicies, AfterEnd};
	GameState currentState = GameState.TotalPlayersSelect;
	
	public GameObject voterTemplate;
	public GameObject neutralTemplate;
	public GameObject coffeeTemplate;
	public GameObject party3Template;
	public GameObject party4Template;
	public GameObject party5Template; //The fifth party(Alex Jungroth)
	public GameObject playerTemplate;

	//holds the max number of political parties and by extension the max number of players in the game (Alex Jungroth)
	private const int totalPoliticalParties = 5;

	//holds the whether or not a part has been chosen (Alex Jungroth)
	public bool[] politicalPartyChosen = new bool[totalPoliticalParties];

	//holds wether or not it is the action turns state yet (Alex Jungroth)
	public bool isActionTurns = false;

	//holds whether or not the random events arrays have been initialized (Alex Jungroth)
	public bool isREAInitialized = false;

	public int gridSize;
	public GridInstanced GridInstancedController;
	
	//holds the title screen settings (Alex Jungroth)
	private TitleScreenSettings gameSettings;
	
	public UI_Script UIController;

	//holds the WindowGenerator script (Alex Jungroth)
	public WindowGeneratorScript WindowGenerator;

	//holds the party policies scripts (Alex Jungroth)
	public GameObject partyPolicyManager;

	public RandomEventControllerScript randomEventController;

	//holds whether or not the number of players has been selected (Alex Jungroth)
	public bool totalPlayersPicked = false;

	public int numberPlayers;  //number of players per game
	public int playersSpawned = 0; //how many players have been spawned in
	public bool playerConfirmsPlacement = false; //bool for checking if player is done

	//holds whether or not the player has been spawned (Alex Jungroth)
	private bool spawnFinished = true;
	
	public int currentPlayerTurn = 0; //this keeps track of which player is currently taking a turn
	public int numberOfRounds; //this is a variable that you can change to however many number if rounds we want.
	private int roundCounter = 0;//will be used to keep track of rounds
	public bool playerTakingAction = false;
	public bool messaged;//Checks if Player has finished taking an action
	
	//holds total the number of electons that will happen in the game (Alex Jungroth)
	public int numberOfElections = 1;
	
	//holds the number of elections that have happened (Alex Jungroth)
	private int electionCounter = 0;
	
	public GameObject[] voters;//array which houses the voters
	public GameObject[] players;//array which houses the players
	
	public GameObject currentPlayer;

	private MusicController gameMusic;
	//holds whether or not the gameController got the music volume settings (Alex Jungroth)
	private bool musicSettingsReceived = false;
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
	public int Party;
	private int tracker = 0;
	private bool votersAppear = true;
	
	//holds the winner of an election (Alex Jungroth)
	public int electionWinner = 0;
	
	//does the tallying at the start of each turn (Alex Jungroth)
	public TallyingScript tallyRoutine;
	
	private InputManagerScript inputManager;
	
	/// <summary>
	/// Start this instance.
	/// Adds in Voter Array
	/// </summary>
	void Start () {
		//VoterVariables VoterVariablesController = GameObject.FindGameObjectWithTag("Voter(Clone)").GetComponent<GameController>();
		
		//gets the title screen settings script (Alex Jungroth)
		try {
			gameSettings = GameObject.FindGameObjectWithTag ("TitleSettings").GetComponent<TitleScreenSettings>();
		}
		catch  {
			Debug.LogError ("Could not find the title screen settings, because you did not start from the title screen!");
			
		}
		if (gameSettings == null) 
		{
			//throws an error if the gameController did not receive the title screen settings (Alex Jungroth)
			Debug.Log("You may continue play testing!");
			
		} 
		else 
		{
			//gets the following variables from the title UI settings (Alex Jungroth)
			gridSize = gameSettings.gridSize;
			numberOfRounds = gameSettings.totalRounds;
			numberOfElections = gameSettings.totalElections;
			NumVoters = gameSettings.totalVoters;
			//the music has to set during the update function (Alex Jungroth)
			musicSettingsReceived = true;
			SFXVolume = gameSettings.sFXVolume;
		}
		
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

		//sets all of the political parties to not chosen (Alex Jungroth)
		for(int i = 0; i < totalPoliticalParties; i++)
		{
			politicalPartyChosen[i] = false;
		}

		//sets the games starting message (Alex Jungroth)
		UIController.alterTextBox("How many players are running for office?");
	
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
			voters[i].gameObject.SetActive (false);
		}
		
	}
	
	
	// Update is called once per frame
	void Update () 
	{	
		if(currentState == GameState.TotalPlayersSelect)
		{
			//sets the music volume at the start of the game (Alex Jungroth)
			if(musicSettingsReceived == true)
			{
				gameMusic.audioChannels[0].volume = gameSettings.musicVolume;

				//sets music settings recieved to false so it doesn't update 
				//the volume every time update is called (Alex Jungroth)
				musicSettingsReceived = false;
			}
			
			//waits until the number of players has been determined (Alex Jungroth)
			if(totalPlayersPicked)
			{
				//gets the number of players (Alex Jungroth)
				numberPlayers = (int)UIController.totalPlayersSlider.GetComponent<Slider>().value;

				//dynamically sizes the players array, but this array should never be
				//larger than the total political parties variable (Alex Jungroth)
				players = new GameObject[numberPlayers];

				//sends the newly made array of players to the UIScript (Alex Jungroth)
				UIController.getPlayerArray(players);

				//disables the total party selection (Alex Jungroth)
				UIController.TotalPlayersDisable();
				
				//enables the party selection buttons (Alex Jungroth)
				UIController.PartyEnable();
				
				//gives instructions for player placement (Alex Jungroth)
				UIController.alterTextBox("Choose a party and a political position.");

				//updates the game state (Alex Jungroth)
				currentState = GameState.PlayerSpawn;
			}
			else
			{
				//sets the display text equal to value of the slide bar (Alex Jungroth)
				UIController.totalPlayersText.GetComponent<Text>().text = UIController.totalPlayersSlider.GetComponent<Slider>().value.ToString();
			}
		}
		else if (currentState == GameState.PlayerSpawn)
		{	
			//spawns players until every player has been spawned (Alex Jungroth)
			if (playersSpawned < numberPlayers)
			{
				if(votersAppear) {
					tracker = MakeAppear(tracker);
					votersAppear = false;
				}
				SpawnPlayer(); 
			}

			else 
			{
				//this is the button configuration once all players have been placed (Alex Jungroth)
				//MakeRest(tracker);
				UIController.disablePPButtons();
				UIController.PartyDisable();
				UIController.toggleActionButtons();
				
				currentState = GameState.ActionTurns;
				playerTakingAction = false;
				Debug.Log ("Round " + (roundCounter + 1) + " begin!");
				Debug.Log ("It's Player " + (currentPlayerTurn + 1) + "'s turn!");

				//updates the tv so the users know whose turn it is (Alex Jungroth)
				UIController.alterTextBox("It is the " + players[currentPlayerTurn].GetComponent<PlayerVariables>().politicalPartyName + " party's turn.");

				//does the tallying before the first player's turn starts (Alex Jungroth)
				tallyRoutine.preTurnTalling ();
				
				//Gives the randomEventController the list of newly spawned players Brian Mah
				randomEventController.players = players;
				randomEventController.playersSpawned = true;

				//lets the voters know what the players are
				for(int i = 0; i < voters.Length; i++)
				{
					voters[i].GetComponent<VoterVariables>().players = players;
				}

				//updates the voters (Alex Jungroth)
				UpdateVoterCanidates();
			}
		} 
		else if (currentState == GameState.ActionTurns)
		{
			//checks to see if the real event array has been initialized (Alex Jungroth)
			if(!isREAInitialized)
			{
				//sets the isActionTurns to true (Alex Jungroth)
				isActionTurns = true;

				//initializes the random event arrays (Alex Jungroth)
				randomEventController.initializeRandomEventsArrays();

				//prevents the random event arrays from being initialized more than once (Alex Jungroth)
				isREAInitialized = true;
			}

			// In Game Heirchy, GameController must set Number Of Rounds greater than 0 in order for this to be called
			if (roundCounter < numberOfRounds)
			{
				PlayerTurn ();

			}
			else 
			{
				currentState = GameState.RoundEnd;
				
			}
			
		} 
		else if (currentState == GameState.RoundEnd)
		{
			
			//disables the action buttons (Alex Jungroth)
			for(int i = 0; i < 10; i++)
			{
				UIController.ActionButtonObject[i].SetActive(false);
			}
			
			//disables the end turn and player stats buttons (Alex Jungroth)
			UIController.endTurnButton.SetActive(false);
			UIController.displayStatsButton.SetActive(false);
			
			// Brian Mah
			UIController.alterTextBox("And the Winner is...");
			
			if(!SFXDrumrollPlaying)
			{
				gameMusic.FadeOut(0);
				SFX.PlayAudioClip(2,0,SFXVolume);
				SFXDrumrollPlaying = true;
				drumrollTime += Time.time;
			}
			
			if(Time.time >= drumrollTime)
			{ // when the sound is done playing
				
				//does the tallying at the end of the game (Alex Jungroth)
				tallyRoutine.preTurnTalling ();
				
				//Sets the gamemode to game end, and calculates the final score
				currentState = GameState.GameEnd;
			}
			
			// Once the game ends and calculation is needed, this is called
		}
		else if (currentState == GameState.GameEnd)
		{
			//Comapre Votes is no longer used, I have left it here
			//in case we need it for future bug testing (Alex Jungroth)
			//CompareVotes(messaged);
			
			//increments the election counter (Alex Jungroth)
			electionCounter += 1;
			
			if(electionCounter <  numberOfElections)
			{
				//displays who won an election (Alex Jungroth)
				WindowGenerator.generateElectionVictory(false);
				
				//manages things between elections (Alex Jungroth)
				prepareElection();
				
				//sets the game state enact policies(Alex Jungroth)
				currentState = GameState.EnactPolicies;
			}
			else
			{
				//displays who won the game (Alex Jungroth)
				WindowGenerator.generateElectionVictory(true);
				
				//ends the game (Alex Jungroth)
				currentState = GameState.AfterEnd;
			}
		}
		else if(currentState == GameState.EnactPolicies)
		{
			//enacts a policy (Alex Jungroth)
			if(WindowGenerator.winner != "no winner")
			{
				for(int i = 0; i < numberPlayers; i++)
				{
					if(players[i].GetComponent<PlayerVariables>().politicalPartyName == WindowGenerator.winner)
					{
						//sets electionWinner to the player who won (Alex Jungroth)
						electionWinner = i;
					}
				}

				if(players[electionWinner].GetComponent<PlayerVariables>().chosenPolicy != 0)
				{
					//a case structure for calling the correct set of functions based on which player won (Alex Jungroth)
					switch(players[electionWinner].GetComponent<PlayerVariables>().politicalPartyName)
					{
						case "Neutral":
							partyPolicyManager.GetComponent<NeutralPolicies>().redirectPolicyRequest(players[electionWinner].GetComponent<PlayerVariables>().chosenPolicy);
						break;

						case "Coffee":
							partyPolicyManager.GetComponent<EspressoPolicies>().redirectPolicyRequest(players[electionWinner].GetComponent<PlayerVariables>().chosenPolicy);
						break;

						case "Drone":
							partyPolicyManager.GetComponent<DronePolicies>().redirectPolicyRequest(players[electionWinner].GetComponent<PlayerVariables>().chosenPolicy);
						break;

						case "Windy":
							partyPolicyManager.GetComponent<WindyPolicies>().redirectPolicyRequest(players[electionWinner].GetComponent<PlayerVariables>().chosenPolicy);
						break;

						case "Anti":
							partyPolicyManager.GetComponent<Party5Policies>().redirectPolicyRequest(players[electionWinner].GetComponent<PlayerVariables>().chosenPolicy);
						break;

						default:
							//something went wrong with the players names
							Debug.LogError("Unrecognized player name!");
						break;
					}
				}
			}
			
			//resets the selected policies for all players (Alex Jungroth)
			for(int i = 0; i < numberPlayers; i++)
			{
				players[i].GetComponent<PlayerVariables>().chosenPolicy = 0;
			}

			//resets the game state to action turns (Alex Jungroth)
			currentState = GameState.ActionTurns;
		}

		if (inputManager.escButtonDown) 
		{
			Application.LoadLevel("TitleScene");
		}
		
	}// Update
	
	
	/// <summary>
	/// This is the part of the game controler that I changed to support 5 players (Alex Jungroth)
	/// Spawns the player and enables player placment controlls.
	/// Disables controlls upon confirmation and last player placed
	/// </summary>
	void SpawnPlayer()
	{
		//prevents update from spamming this part of the function (Alex Jungroth)
		if (!spawnFinished) 
		{
			//enables the player placement movement controls (Alex Jungroth)
			UIController.PartyDisable ();
			UIController.enablePPButtonsPartySelection ();

			//this is code for spawning different parites
			//depending on what party the player chose, this is what they will spawn as
			//each party can only be chosen once
			switch (Party) 
			{
				case 0: 
					currentPlayer = Instantiate (neutralTemplate, new Vector3 (0, 0, 0), Quaternion.identity) as GameObject; 
					this.playerTemplate = neutralTemplate; 
				break;
		
				case 1: 
					currentPlayer = Instantiate (coffeeTemplate, new Vector3 (0, 0, 0), Quaternion.identity) as GameObject; 
					this.playerTemplate = coffeeTemplate; 
				break;

				case 2:
					currentPlayer = Instantiate (party3Template, new Vector3 (0, 0, 0), Quaternion.identity) as GameObject;
					this.playerTemplate = party3Template;
				break;
		
				case 3:
					currentPlayer = Instantiate (party4Template, new Vector3 (0, 0, 0), Quaternion.identity) as GameObject;
					this.playerTemplate = party4Template;
				break;
		
				case 4:
					currentPlayer = Instantiate (party5Template, new Vector3 (0, 0, 0), Quaternion.identity) as GameObject; 
					this.playerTemplate = party5Template;
				break;
			}

			//stores the current player into the array of players (Alex Jungroth)
			players [playersSpawned] = currentPlayer;

			//prevents update from calling this part of the function again (Alex Jungroth)
			spawnFinished = true;
		}

		//checks the player against all of the previous players to ensure no duplicates
		for(int i = 0; i < playersSpawned; i++)
		{
			if (currentPlayer.transform.position == players[i].transform.position)
			{
				//if they are on the same spot
				playerConfirmsPlacement = false;
			}
		}

		if(playerConfirmsPlacement)
		{ 
			//if the player placment is legal sets up the next party selection buttons (Alex Jungroth)
			UIController.correctPlacement();

			//increments the players spawned (Alex Jungroth)
			playersSpawned++;

			//prevents more than five players from spawning (Alex Jungroth)
			spawnFinished = true;

			//at most, confirm should only be able to be pressed five times (Alex Jungroth)
			playerConfirmsPlacement = false;
			votersAppear = true;

			//gives instructions for player placement (Alex Jungroth)
			UIController.alterTextBox("Choose a party and a political position.");
			if(numberPlayers == playersSpawned)
				MakeRest(tracker);

		}//if

	}//SpawnPlayer
	
	/// <summary>
	/// Players turn.
	/// So this system works similarly to Brian's player placement Script.
	/// It first keeps track of which player is currently taking an action. 
	/// When the action is finished, it moves on to the next player until all players have moved.
	/// When all players have moved, it ends the round, and continues till all rounds are done.
	/// NOTE: Make sure that you set the number of Rounds in the scene editor. Default is 0 right now.
	/// Another note, in order to end the turn, an outside script needs to tell the Game Controller that "playerTakingAction" is true for the turn to end.
	/// </summary>
	void PlayerTurn(){
		if (playerTakingAction) {
			if(currentPlayerTurn < numberPlayers)
			{
				/*I am moving the incrementing of current player turn to 
				inside this if statement to prevent current player turn 
				form endlessly growing when the random event controller 
				is being. This can not go into the if statement below
				as that will call an index out of range exception (Alex Jungroth)*/
				currentPlayerTurn++;
			}

			if (currentPlayerTurn < numberPlayers) {
				playerTakingAction = false;
				Debug.Log ("It's Player " + (currentPlayerTurn + 1) + "'s turn!");
				//updates the tv so the users know whose turn it is (Alex Jungroth)
				UIController.alterTextBox("It is the " + players[currentPlayerTurn].GetComponent<PlayerVariables>().politicalPartyName + " party's turn.");
			}
			if (currentPlayerTurn >= numberPlayers) {
				//this is when all players have made their turns
				
				if(randomEventController.ActivateEvents()){  //continually goes to random event controller until randomEventController returns true

					//does the tallying after the players ends there turns (Alex Jungroth)
					tallyRoutine.preTurnTalling();
					
					//this is when the new round begins
					roundCounter++;
					currentPlayerTurn = 0;
					playerTakingAction = false;
					
					if (roundCounter < numberOfRounds) {
						Debug.Log ("Round " + (roundCounter + 1) + " begin!");
						Debug.Log ("It's Player " + (currentPlayerTurn + 1) + "'s turn!");

						//updates the tv so the users know whose turn it is (Alex Jungroth)
						UIController.alterTextBox("It is the " + players[currentPlayerTurn].GetComponent<PlayerVariables>().politicalPartyName + " party's turn.");

					} else {
						Debug.Log ("Game Ends!");
					}
				}
			}
		}
	}

	/// <summary>
	/// Compares the votes. This function is no longer used. I'm leaving it here for now
	/// in case we want it for bug testing in the future. (Alex Jungroth)
	/// </summary>
	/// <param name="messaged">If set to <c>true</c> messaged.</param>
	void CompareVotes(bool messaged){
		
		int mostVotes = 0;
		int winningPlayer = 0;
		int tieVotes = 0;
		int tieFighter = 0;//player that ties
		
		
		for(int i = 0; i < numberPlayers; i++){
			if(players[i].GetComponent<PlayerVariables>().votes > mostVotes){
				mostVotes = players[i].GetComponent<PlayerVariables>().votes; 
				winningPlayer = i + 1;
			}
			else if(players[i].GetComponent<PlayerVariables>().votes == mostVotes) {
				//Debug.Log ("here");
				tieVotes = players[i].GetComponent<PlayerVariables>().votes;
				tieFighter = i + 1;
			}
		}
		if(messaged && mostVotes == tieVotes){
			Debug.Log ("Winning Players are " + winningPlayer +" and " + tieFighter + " with a tie vote of: " + tieVotes + "!");
			UIController.alterTextBox("Winning Players are " + winningPlayer +" and " + tieFighter + " with a tie vote of: " + tieVotes + "!");
			messaged = false;
			
			//gets the winner (Alex Jungroth)
			//electionWinner = winningPlayer;
			
		}
		else if(messaged){
			Debug.Log("Winning Player is: " + winningPlayer + " with " + mostVotes + " votes!");
			UIController.alterTextBox("Winning Player is: " + winningPlayer + " with " + mostVotes + " votes!");
			messaged = false;
			
			//gets the winner (Alex Jungroth)
			//electionWinner = winningPlayer;
		}
	}
	
	/// <summary>
	/// Prepares the election. (Alex Jungroth)
	/// </summary>
	void prepareElection()
	{
		//holds the vector 3 of the voters so they can be altered (Alex Jungroth)
		Vector3 temp = Vector3.zero;
		
		//holds a random float to determine the direction the voters are moving in (Alex Jungroth)
		float tempRandom = 0;
		
		//resets the drum roll (Alex Jungroth)
		SFXDrumrollPlaying = false;
		drumrollTime = 3.7f;
		
		for (int i = 0; i < NumVoters; i++) 
		{
			//this handles voter resistance cool down (Alex Jungroth)
			voters [i].GetComponent<VoterVariables> ().baseResistance *= 0.5f;
			
			//gets a random value from 0 to 5 (Alex Jungroth)
			tempRandom = Random.Range(0f, 6.0f);
			
			//has a chance to move the voters in a random direction (Alex Jungroth)
			switch ((int)tempRandom)
			{
			case 0:
				if (Random.value > voters [i].GetComponent<VoterVariables> ().xPlusResistance + (int)voters [i].GetComponent<VoterVariables> ().baseResistance)
				{
					//move voter in the +X direction (Alex Jungroth)
					temp = voters[i].transform.position;
					temp += new Vector3(1,0,0);
					
					//makes sure the voter doesn't move off the grid or onto another voter (Alex Jungroth)
					if((temp.x < gridSize - 1) && (overlapCheck(temp) == true))
					{
						voters[i].transform.position = temp;
					}
				}
				break;
				
			case 1:
				if (Random.value > voters [i].GetComponent<VoterVariables> ().xMinusResistance + (int)voters [i].GetComponent<VoterVariables> ().baseResistance)
				{
					//move voter in the -X direction (Alex Jungroth)
					temp = voters[i].transform.position;
					temp -= new Vector3(1,0,0);
					
					//makes sure the voter doesn't move off the grid or onto another voter (Alex Jungroth)
					if((temp.x > 0)  && (overlapCheck(temp) == true))
					{
						voters[i].transform.position = temp;
					}
				}
				break;
				
			case 2:
				if (Random.value > voters [i].GetComponent<VoterVariables> ().yPlusResistance + (int)voters [i].GetComponent<VoterVariables> ().baseResistance) 
				{
					//move voter in the +Y direction (Alex Jungroth)
					temp = voters[i].transform.position;
					temp += new Vector3(0,1,0);
					
					//makes sure the voter doesn't move off the grid or onto another voter (Alex Jungroth)
					if((temp.y < gridSize - 1) && (overlapCheck(temp) == true))
					{
						voters[i].transform.position = temp;
					}
				}
				break;
				
			case 3:
				if (Random.value > voters [i].GetComponent<VoterVariables> ().yMinusResistance + (int)voters [i].GetComponent<VoterVariables> ().baseResistance) 
				{
					//move voter the -Y direction (Alex Jungroth)
					temp = voters[i].transform.position;
					temp -= new Vector3(0,1,0);
					
					//makes sure the voter doesn't move off the grid or onto another voter (Alex Jungroth)
					if((temp.y > 0) && (overlapCheck(temp) == true))
					{
						voters[i].transform.position = temp;
					}
				}
				break;
				
			case 4:
				if (Random.value > voters [i].GetComponent<VoterVariables> ().zPlusResistance + (int)voters [i].GetComponent<VoterVariables> ().baseResistance) 
				{
					//move voter the +Z direction (Alex Jungroth)
					temp = voters[i].transform.position;
					temp += new Vector3(0,0,1);
					
					//makes sure the voter doesn't move off the grid or onto another voter (Alex Jungroth)
					if((temp.z < gridSize - 1) && (overlapCheck(temp) == true))
					{
						voters[i].transform.position = temp;
					}
				}
				break;
				
			case 5:
				if (Random.value > voters [i].GetComponent<VoterVariables> ().zMinusResistance + (int)voters [i].GetComponent<VoterVariables> ().baseResistance)
				{
					//move voter the -Z direction (Alex Jungroth)
					temp = voters[i].transform.position;
					temp -= new Vector3(0,0,1);
					
					//makes sure the voter doesn't move off the grid or onto another voter (Alex Jungroth)
					if((temp.z > 0) && (overlapCheck(temp) == true))
					{
						voters[i].transform.position = temp;
					}
				}
				break;
			}
		}
		
		//deletes all of the players shadow positions and clears their shadow position list (Alex Jungroth)
		for (int i = 0; i < numberPlayers; i++) 
		{
			for(int j = 0; j < players[i].GetComponent<PlayerVariables>().shadowPositions.Count; j++)
			{
				Destroy(players[i].GetComponent<PlayerVariables>().shadowPositions[j]);
			}
			
			players[i].GetComponent<PlayerVariables>().shadowPositions.Clear();
		}
		
		//resets the rounds counter (Alex Jungroth)
		roundCounter = 0;
	}
	
	/// <summary>
	/// Checks to make sure none of the voters overlap each other (Alex Jungroth)
	/// </summary>
	bool overlapCheck(Vector3 temp)
	{
		for(int i = 0; i < NumVoters; i++)
		{
			if(temp == voters[i].transform.position)
			{
				return false;
			}
		}
		
		return true;	
	}

	//function to make all voters check who they are closest to
	public void UpdateVoterCanidates(){
		//Debug.Log("voters updated");
		for (int i = 0; i < voters.Length; i++) {
			voters[i].GetComponent<VoterVariables>().FindCanidate();
		}
	}
	
	public void SetPartyNeutral()
	{
		if (politicalPartyChosen [0] == false) {
			Party = 0;

			//doesn't spawn a player until one of the parties has been selected
			spawnFinished = false;

			//prevents this party from being chosen twice (Alex Jungroth)
			politicalPartyChosen [0] = true;

			//Tells the user which party they picked (Alex Jungroth)
			UIController.alterTextBox("You have chosen the Neutral Party.");

		} 
		else
		{
			//displays an error message to the users (Alex Jungroth)
			UIController.alterTextBox("This party has already been chosen.");

		}
	}
	public void SetPartyCoffee() 
	{
		if (politicalPartyChosen [1] == false) 
		{
			Party = 1;

			//doesn't spawn a player until one of the parties has been selected
			spawnFinished = false;

			//prevents this party from being chosen twice (Alex Jungroth)
			politicalPartyChosen [1] = true;

			//Tells the user which party they picked (Alex Jungroth)
			UIController.alterTextBox("You have chosen the Coffee Party.");
		}
		else
		{
			//displays an error message to the users (Alex Jungroth)
			UIController.alterTextBox("This party has already been chosen.");
			
		}
	
	}
	public void SetParty3() 
	{
		if (politicalPartyChosen [2] == false)
		{
			Party = 2;

			//doesn't spawn a player until one of the parties has been selected
			spawnFinished = false;

			//prevents this party from being chosen twice (Alex Jungroth)
			politicalPartyChosen [2] = true;

			//Tells the user which party they picked (Alex Jungroth)
			UIController.alterTextBox("You have chosen the Drone Party.");
		}
		else
		{
			//displays an error message to the users (Alex Jungroth)
			UIController.alterTextBox("This party has already been chosen.");

		}
	}
	public void SetParty4() 
	{
		if (politicalPartyChosen [3] == false)
		{
			Party = 3;

			//doesn't spawn a player until one of the parties has been selected
			spawnFinished = false;

			//prevents this party from being chosen twice (Alex Jungroth)
			politicalPartyChosen [3] = true;

			//Tells the user which party they picked (Alex Jungroth)
			UIController.alterTextBox("You have chosen the Windy Party.");
		}
		else
		{
			//displays an error message to the users (Alex Jungroth)
			UIController.alterTextBox("This party has already been chosen.");
			
		}
	}
	public void SetParty5()
	{
		if (politicalPartyChosen [4] == false) 
		{
			Party = 4;

			//doesn't spawn a player until one of the parties has been selected
			spawnFinished = false;

			//prevents this party from being chosen twice (Alex Jungroth)
			politicalPartyChosen [4] = true;

			//Tells the user which party they picked (Alex Jungroth)
			UIController.alterTextBox("You have chosen the Anti Party.");
		}
		else
		{
			//displays an error message to the users (Alex Jungroth)
			UIController.alterTextBox("This party has already been chosen.");
			
		}
	}

	public int MakeAppear(int tracker)
	{
		for (int i = 0; i < voters.Length/numberPlayers; i++) {
			voters [tracker].gameObject.SetActive (true);
			tracker++;
		}
		return tracker;
	}

	public void MakeRest(int tracker)
	{
		for (int i = 0; i < voters.Length%numberPlayers; i++) {
			Debug.Log (tracker);
			voters [tracker].gameObject.SetActive (true);
			tracker++;
		}

	}

}//Gamecontroller Class