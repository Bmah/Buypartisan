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
	public GameObject neutralTemplate;
	public GameObject coffeeTemplate;
	public GameObject party3Template;
	public GameObject party4Template;
	public GameObject playerTemplate;
	public Button party1;
	public Button party2;
	public Button party3;
	public Button party4;
	
	public int gridSize;
	public GridInstanced GridInstancedController;
	
	//holds the title screen settings (Alex Jungroth)
	private TitleScreenSettings gameSettings;
	
	public UI_Script UIController;
	
	//holds the WindowGenerator script (Alex Jungroth)
	public WindowGeneratorScript WindowGenerator;
	
	public RandomEventControllerScript randomEventController;
	
	public int numberPlayers;  //number of players per game
	public int playersSpawned = 0; //how many players have been spawned in
	private bool spawnedNewPlayer = false; //bool for checking whether or not a new player has been spawned in
	public bool playerConfirmsPlacment = false; //bool for checking if player is done
	
	public int currentPlayerTurn = 0; //this keeps track of which player is currently taking a turn
	public int numberOfRounds; //this is a variable that you can change to however many number if rounds we want.
	private int roundCounter = 0;//will be used to keep track of rounds
	public bool playerTakingAction = false;
	public bool messaged;//Checks if Player has finished taking an action
	public bool player2Spawning = false;
	public bool partyChosen = false;
	
	//holds total the number of electons that will happen in the game (Alex Jungroth)
	public int numberOfElections = 1;
	
	//holds the number of elections that have happened (Alex Jungroth)
	private int electionCounter = 0;
	
	public GameObject[] voters;//array which houses the voters
	public GameObject[] players = new GameObject[2];//array which houses the players
	
	public GameObject currentPlayer;
	public Material Player2Material;
	
	//holds player2's sphere renderer (Alex Jungroth)
	private Renderer player2Renderer;
	
	//holds player2 sphere's transparency (Alex Jungroth)
	private Color player2SphereTransparency;
	
	private MusicController gameMusic;
	//holds wether or not the gameController got the music volume settings (Alex Jungroth)
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
	
	//holds the winner of an election (Alex Jungroth)
	public int electionWinner;
	
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
			
			//sets the music volume at the start of the game (Alex Jungroth)
			if(musicSettingsReceived == true)
			{
				gameMusic.audioChannels[0].volume = gameSettings.musicVolume;
			}
			
			if (playersSpawned < numberPlayers) {//Players are still spawning in
				if(partyChosen) {
					SpawnPlayer (); 
				}
			} else {
				currentState = GameState.ActionTurns;
				playerTakingAction = false;
				Debug.Log ("Round " + (roundCounter + 1) + " begin!");
				Debug.Log ("It's Player " + (currentPlayerTurn + 1) + "'s turn!");
				
				//does the tallying before the first player's turn starts (Alex Jungroth)
				tallyRoutine.preTurnTalling ();
				
				//Gives the randomEventController the list of newly spawned players
				//Brian Mah
				randomEventController.players = players;
				randomEventController.playersSpawned = true;
				
				//lets the voters know what the players are
				for(int i = 0; i < voters.Length; i++){
					voters[i].GetComponent<VoterVariables>().players = players;
				}
				
				//Brian Mah
				//Makes voters the right colors
				UpdateVoterCanidates();
			}
		} else if (currentState == GameState.ActionTurns) {
			
			// In Game Heirchy, GameController must set Number Of Rounds greater than 0 in order for this to be called
			if (roundCounter < numberOfRounds) {
				PlayerTurn ();
				
			} else {
				currentState = GameState.RoundEnd;
				
			}
			
		} else if (currentState == GameState.RoundEnd) {
			
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
			
			//increments the election counter (Alex Jungroth)
			electionCounter += 1;
			
			if(electionCounter <  numberOfElections)
			{
				//displays who won an election (Alex Jungroth)
				WindowGenerator.generateElectionVictory(false, electionWinner);
				
				//manages things between elections (Alex Jungroth)
				prepareElection();
				
				//resets the game state (Alex Jungroth)
				currentState = GameState.ActionTurns;
			}
			else
			{
				//displays who won the game (Alex Jungroth)
				WindowGenerator.generateElectionVictory(true, electionWinner);
				
				//ends the game (Alex Jungroth)
				currentState = GameState.AfterEnd;
			}
			
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
			UIController.PartyDisable();
			UIController.enablePPButtonsPartySelection();
			switch (Party){//this is code for spawning different parites.  Parties are set as an enum, and assigned at title
				//from here, depending on what party the player chose, this is what they will spawn as
			case 0 : currentPlayer = Instantiate(neutralTemplate,new Vector3(0,0,0), Quaternion.identity) as GameObject; this.playerTemplate = neutralTemplate; break;
			case 1 : currentPlayer = Instantiate(coffeeTemplate,new Vector3(0,0,0), Quaternion.identity) as GameObject; this.playerTemplate = coffeeTemplate; break;
			case 2 : currentPlayer = Instantiate(party3Template,new Vector3(0,0,0), Quaternion.identity) as GameObject; this.playerTemplate = party3Template; break;
			case 3 : currentPlayer = Instantiate(party4Template,new Vector3(0,0,0), Quaternion.identity) as GameObject; this.playerTemplate = party4Template; break;
			}
			
			//currentPlayer = Instantiate(playerTemplate,new Vector3(0,0,0), Quaternion.identity) as GameObject;
			players[playersSpawned] = currentPlayer;
			spawnedNewPlayer = true;
			playerConfirmsPlacment = false;
			
		}
		
		//Player Uses Buttons to choose where the player goes in the scene
		/*if (player2Spawning) {
			players [playersSpawned].GetComponent<Renderer> ().material = Player2Material;

			//trying an alternate way of changing the sphere's color (Alex Jungroth)
			player2Renderer = players[playersSpawned].GetComponent<PlayerVariables>().sphereController.GetComponent<Renderer>();
			player2Renderer.material = Player2Material;
			player2SphereTransparency = player2Renderer.material.color;
			player2SphereTransparency.a = 0.2f;
			player2Renderer.material.SetColor("_Color", player2SphereTransparency);

			//players [playersSpawned].GetComponent<PlayerVariables> ().transparentColor = players [playersSpawned].GetComponent<PlayerVariables> ().sphereRenderer.material.color;
			//players [playersSpawned].GetComponent<PlayerVariables> ().transparentColor.a = 0.2f;
			//players [playersSpawned].GetComponent<PlayerVariables> ().sphereRenderer.material.SetColor ("_Color", players [playersSpawned].GetComponent<PlayerVariables> ().transparentColor);
			//players [playersSpawned].GetComponent<PlayerVariables> ().sphereController.transform.localScale = new Vector3 (players [playersSpawned].GetComponent<PlayerVariables> ().sphereSize, players [playersSpawned].GetComponent<PlayerVariables> ().sphereSize, players [playersSpawned].GetComponent<PlayerVariables> ().sphereSize);
		}*/
		if (playerConfirmsPlacment) {
			//checks the player against all of the previous players to ensure no duplicates
			for(int i = 0; i < playersSpawned; i++){
				if (currentPlayer.transform.position == players[i].transform.position){//if they are on the same spot
					playerConfirmsPlacment = false;
				}
			}
			if(playerConfirmsPlacment){ //if the player placment is legal
				
				UIController.correctPlacement();
				
				playersSpawned++;
				partyChosen = false;
				spawnedNewPlayer = false;
				player2Spawning = true;
				playerConfirmsPlacment = false;
				if(!(playersSpawned < numberPlayers)){
					UIController.disablePPButtons();
					UIController.PartyDisable();
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
					} else {
						Debug.Log ("Game Ends!");
					}
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
			electionWinner = winningPlayer;
			
		}
		else if(messaged){
			Debug.Log("Winning Player is: " + winningPlayer + " with " + mostVotes + " votes!");
			UIController.alterTextBox("Winning Player is: " + winningPlayer + " with " + mostVotes + " votes!");
			messaged = false;
			
			//gets the winner (Alex Jungroth)
			electionWinner = winningPlayer;
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
	
	//function to make all voters check who they are closest to
	public void UpdateVoterCanidates(){
		Debug.Log("voters updated");
		for (int i = 0; i < voters.Length; i++) {
			voters[i].GetComponent<VoterVariables>().FindCanidate();
		}
	}
	
	public void SetPartyNeutral() {
		Party = 0;
		partyChosen = true;
	}
	public void SetPartyCoffee() {
		Party = 1;
		partyChosen = true;
	}
	public void SetParty3() {
		Party = 2;
		partyChosen = true;
		Debug.Log ("yooooooo");
	}
	public void SetParty4() {
		Party = 3;
		partyChosen = true;
	}
}//Gamecontroller Class
