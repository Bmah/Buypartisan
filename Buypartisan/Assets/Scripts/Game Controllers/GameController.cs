﻿// Brian Mah
// Game Controller

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text;
using System.IO;
using System.Runtime;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    //TODO:  FURTHER REFINE THE ORGANIZATION WITHIN THE SUBGROUPS
    //***********************************************************
    //
    //       Board initialization
    // Here are all the variables for things that we place
    // on the board at the start of the game, like pieces, and
    // templates of parties and models
    //
    //***********************************************************
    [Header("Debug Settings")]
    public bool TOGGLE_TUT;
    public bool TOGGLE_PEDESTAL;
    public bool DEBUG_UNLIM_MONEY;
    public int GRID_SIZE;
    public int NUM_OF_ROUNDS;
    public int NUM_OF_ELECTIONS;

    [Header("Templates")]
    public GameObject voterTemplate;
	public GameObject Party_1Template;
    public GameObject Party_2Template;
    public GameObject Party_3Template;
    public GameObject Party_4Template;
    public GameObject Party_5Template;
    //public GameObject coffeeTemplate;//coffee
    //public GameObject party3Template;//Drone
    //public GameObject party4Template;//Windy
    //public GameObject party5Template; //The fifth party(Alex Jungroth)
    //public GameObject playerTemplate;//generic

    //apple pie
    //public GameObject Sphere_1Template;
    //public GameObject Sphere_2Template;
    //public GameObject Sphere_3Template;
    //public GameObject Sphere_4Template;
    //public GameObject Sphere_5Template;

    //public GameObject coffeeSphereTemplate;//coffee
    //public GameObject party3SphereTemplate;//Drone
    //public GameObject party4SphereTemplate;//Windy
    //public GameObject party5SphereTemplate; //The fifth party(Alex Jungroth)

    //*************************************************************
    //
    //          Game Initialization
    // Here are the various settings or variables that
    // we need in order to start up the game, such as 
    // number of players and voters, or max money
    //
    //**************************************************************
    private const int totalPoliticalParties = 5;//holds the max number of political parties and by extension the max number of players in the game (Alex Jungroth)
    private bool votersAppear = true;
    private bool goalSectionDisplayed = false; //The first three sections of the tutorial happen in the same game state, so two booleans are needed to separate them (Alex Jungroth)
    private bool sliderSectionDisplayed = false;
    private int totalPlayersSpawned = 0;

    [HideInInspector]
    public int gridSize;
    [HideInInspector]
    public int numberOfRounds; //this is a variable that you can change to however many number if rounds we want.
    [HideInInspector]
    public int numberOfElections = 1;
    [HideInInspector]
    public int NumVoters = 10;
    [HideInInspector]
    public int voterMaxMoney = 100;
    [HideInInspector]
    public int voterMaxVotes = 100;
    [HideInInspector]
    public bool SpawnUsingTXT = true;
    [HideInInspector]
    public float VoterDistanceCheck = 1f;
    [HideInInspector]
    public float IgnoreNearestVoter = 0.3f;


    [HideInInspector]
    public bool uniqueParties = false;
    [HideInInspector]
    public bool complexElections = false;//If this is false, window generator will not be used (Alex Jungroth)
    [HideInInspector]
    public bool usePedestals = false;
    [HideInInspector]
    public bool useTutorial = true;


    [HideInInspector]
    //Num players in current game
    public int numberPlayers;  



    //*************************************************************
    //
    //      Game Control
    // Variables directly related to regulating the flow
    // and control of the game, such as latches, logistical 
    // vars for spawning, and so on
    //
    //*************************************************************
    //constants that are used to calculate a winning player's money at the end of an election (Alex Jungroth)
    private const float threeQuatersDecrease = 0.75f;
    private const float half = 0.5f;
    private const float tenPercentIncrease = 1.1f;
    private const float tenPercentDecrease = 0.9f;
    private int roundCounter = 0;//will be used to keep track of rounds
    private int electionCounter = 0;//Holds the number of elections that have happened (Alex Jungroth)

    [HideInInspector]
    public enum GameState { PlayerSelect,
                            PlayerSpawn,
                            ActionTurns,
                            RoundEnd,
                            GameEnd,
                            EnactPolicies,
                            AfterEnd };
    GameState currentState = GameState.PlayerSelect;

    [HideInInspector]
    public bool CurPlayerConfirmPlacement = false; //bool for checking if player is done
    [HideInInspector]
    public int CurPlayerSpawning = 0;//holds the current player being spawned (Alex Jungroth)
    [HideInInspector]
    public int CurrentPlayerTurn = 0; //this keeps track of which player is currently taking a turn
    [HideInInspector]
    public bool CurPlayerAction = false;
    [HideInInspector]
    public bool CurPlayerActionFinished;//Checks if Player has finished taking an action
    [HideInInspector]
    public bool CurPlayerFinishedSpawning = false;//Holds whether or not a player needs to spawned (Alex Jungroth)
    [HideInInspector]
    public bool IsActionState = false;//Holds wether or not it is the action turns state yet (Alex Jungroth)
    [HideInInspector]
    public bool IsREAInitalized = false;//Holds whether or not the random events arrays have been initialized (Alex Jungroth)
    [HideInInspector]
    public bool MaxNumPlayerSelected = false;//Holds whether or not the number of players has been selected (Alex Jungroth)
    [HideInInspector]
    public string WinnerName = "no winner";//Holds the winner of an election's name (Alex Jungroth)
    [HideInInspector]
    public int WinnerPlayerNum = 0;//Holds the winner of an elections number (Alex Jungroth)
    [HideInInspector]
    public int MaxVote = 0;//Holds the highest vote total (Alex Jungroth)
    [HideInInspector]
    public int MaxVictoryPoints = 0;//Holds the highest victory point total (Alex Jungroth)
    [HideInInspector]
    public int NumPlayersSpawned = 0; //how many players have been spawned in

    //*******************************************************************
    //
    //      Player and Element Collection
    //  Here are any arrays or large data types used to keep track of
    // pieces or integral things to the game, or variables used to keep track 
    // of detailed player and information data.
    //
    //*********************************************************************
    private int currentPlayerMoney = 0;//holds the current player's money (Alex Jungroth)
    private int currentPlayerVotes = 0;//holds the current player's votes (Alex Jungroth)
    private bool musicSettingsReceived = false;//holds whether or not the gameController got the music volume settings (Alex Jungroth)

    [HideInInspector]
    public GameObject[] Voters;//array which houses the voters
    [HideInInspector]
    public GameObject[] Players;//array which houses the players
    [HideInInspector]
    public int[] ChosenParty = new int[5]; //Maps player number to a party
    [HideInInspector]
    public GameObject currentPlayer;
    [HideInInspector]
    public int electionWinner = 0;//holds the winner of an election (Alex Jungroth)
    [HideInInspector]
    public float SFXVolume;

    //******************************************************************
    //
    //           Referenced Scripts and Objects
    // Here are references and objects that we use in some capacity
    // or another throughout the game controller
    //
    //*******************************************************************
    private TitleScreenSettings gameSettings; //holds the title screen settings (Alex Jungroth)
    private MusicController gameMusic;
    private SFXController SFX;
    private InputManagerScript inputManager;

    [Header("Game Controllers")]
    public GridInstanced GridInstancedController;
    public UI_Script UIController;
	public PopUpTVScript popUpTVScript;
    public WindowGeneratorScript WindowGenerator;//holds the WindowGenerator script (Alex Jungroth)
    public GameObject partyPolicyManager;//holds the party policies scripts (Alex Jungroth)
    public RandomEventControllerScript randomEventController;
    public PlayerTurnsManager turnsManager;
    public TallyingScript tallyRoutine;//does the tallying at the start of each turn (Alex Jungroth)
    public CalculateWinner calculateWinnnerManager;//Calcuates the winner of elections and the game (Alex Jungroth)
    public SimpleVictoryDisplay simpleVictoryDisplay;//Displays the winner in on single screen that does not impact game flow (Alex Jungroth)
    public TutorialController tutorialController;//Controls the game's tutorial (Alex Jungroth)

    //*********************************************************************
    //
    //      Misc
    // Didn't know what they were, or what to do with them
    //
    //*********************************************************************

    private bool PreAnnouncmentSFXPlaying = false;
	private float PreAnnouncmentSFXTime = 3.7f;
    private int tracker = 0;
	/// <summary>
	/// Start this instance.
	/// Adds in Voter Array
	/// </summary>
	void Start () {
        //VoterVariables VoterVariablesController = GameObject.FindGameObjectWithTag("Voter(Clone)").GetComponent<GameController>();

        GettingVariableReferences();
		        
		GridInstancedController.GridInstantiate (gridSize);
		UIController.gridSize = gridSize;
		UIController.SFXvolume = SFXVolume;
		randomEventController.gridSize = gridSize;
		randomEventController.SFXvolume = SFXVolume;
		CurPlayerActionFinished = true;
        UIController.uniqueParties = uniqueParties;

		if (SpawnUsingTXT)
        {
			SpawnVotersFromTXT ();
		}
        else
        {
			SpawnUsingProbabilityMap(NumVoters,VoterDistanceCheck,voterMaxMoney,voterMaxVotes,IgnoreNearestVoter);
		}
		
		randomEventController.voters = Voters;
		
		//sets the games starting message (Alex Jungroth)
		UIController.alterTextBox("How many players are running for office?\nChoose a party and a political position.");

		//gives instructions for player placement (Alex Jungroth)
		UIController.alterTextBox("Choose a party and a political position.");
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
				Voters = new GameObject[temp];
				
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
							Voters[voterNumber] = Instantiate (voterTemplate, voterLocation, Quaternion.identity) as GameObject;
							Voters [voterNumber].GetComponent<VoterVariables> ().votes = voterDataInt[3];
							Voters [voterNumber].GetComponent<VoterVariables> ().money = voterDataInt[4];
							
							//These get the resistance variables for a voter (Alex Jungroth)
							Voters [voterNumber].GetComponent<VoterVariables> ().baseResistance = voterDataInt[5];
							Voters [voterNumber].GetComponent<VoterVariables> ().xPlusResistance = voterDataInt[6];
							Voters [voterNumber].GetComponent<VoterVariables> ().xMinusResistance = voterDataInt[7];
							Voters [voterNumber].GetComponent<VoterVariables> ().yPlusResistance = voterDataInt[8];
							Voters [voterNumber].GetComponent<VoterVariables> ().yMinusResistance = voterDataInt[9];
							Voters [voterNumber].GetComponent<VoterVariables> ().zPlusResistance = voterDataInt[10];
							Voters [voterNumber].GetComponent<VoterVariables> ().zMinusResistance = voterDataInt[11];
							
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
		int numRichppl = 0;
		int numSemiRichppl = 0;

		Voters = new GameObject[numberofVoters];
		
		for (int i = 0; i < numberofVoters; i++) {
			//until a unique location is found continually search for a new position.
			uniqueLocation = false;
			while (!uniqueLocation) {
				voterLocation = new Vector3 (Random.Range (0, gridSize), Random.Range (0, gridSize), Random.Range (0, gridSize));
				uniqueLocation = true;
				for (int j = 0; j < i; j++) {
					if ((Voters [j].transform.position - voterLocation).magnitude < distanceToNearestVoter) {
						uniqueLocation = false;
					}
				}
				if (Random.value < probabilityToIgnoreNearestVoter) {
					uniqueLocation = true;
				}
				for (int j = 0; j < i; j++) {
					if (Voters [j].transform.position == voterLocation) {
						uniqueLocation = false;
					}
				}
			}
			Voters [i] = Instantiate (voterTemplate, voterLocation, Quaternion.identity) as GameObject;
			voterInfoTemp = Voters [i].GetComponent<VoterVariables> ();

			voterInfoTemp.money = voterMaxMoney;

			for (int k = 0; k < i; k++) {
				if ((Voters [k].transform.position - voterLocation).magnitude < distanceToNearestVoter) {
					Voters [k].GetComponent<VoterVariables> ().money /= 2;
					voterInfoTemp.money /= 2;
				}
			}

			//voterInfoTemp.votes = Mathf.RoundToInt(voterMaxVotes*(1-moneyToVotesRatio));
			voterInfoTemp.votes = 1;

			voterInfoTemp.xMinusResistance = Random.value * Random.value;
			voterInfoTemp.xPlusResistance = Random.value * Random.value;
			voterInfoTemp.yMinusResistance = Random.value * Random.value;
			voterInfoTemp.yPlusResistance = Random.value * Random.value;
			voterInfoTemp.zMinusResistance = Random.value * Random.value;
			voterInfoTemp.zPlusResistance = Random.value * Random.value;
			voterInfoTemp.baseResistance = 0;
			Voters [i].gameObject.SetActive (false);
		}

		for (int i = 0; i < numberofVoters; i++) {
			if(Voters[i].GetComponent<VoterVariables>().money == voterMaxMoney){
				if(numRichppl*10 > numberofVoters){
					Voters[i].GetComponent<VoterVariables>().money /= 2;
				}
				else{
					numRichppl++;
				}
			}
			else if(Voters[i].GetComponent<VoterVariables>().money == voterMaxMoney/2){
				if(numSemiRichppl*5 > numberofVoters){
					Voters[i].GetComponent<VoterVariables>().money /= 2;
				}
				else{
					numSemiRichppl++;
				}
			}
		}
	
	}
	// Update is called once per frame
	void Update() 
	{	
		if(currentState == GameState.PlayerSelect)
		{
			//sets the music volume at the start of the game (Alex Jungroth)
			if(musicSettingsReceived)
			{
                AdjustMusic();
            }//if

            //Displays the goal, slider, and voter sections of the tutorial if the tutorial is being used (Alex Jungroth)
            if(useTutorial == true)
            {
                //These if statements make sure each section waits for the others to finsih (Alex Jungroth)
                //Goal Explainer
                if(!goalSectionDisplayed)
                {
                    //Tyler - DEPRECATED, NEW SYSTEM
                    //tutorialController.goalExplainer();
                    tutorialController.loadTutorial(TutorialController.goalSection);

                    //Moves to the next section of the tutorial when this one has finished (Alex Jungroth)
                    if(tutorialController.isActiveTutorial == false)
                    {
                        goalSectionDisplayed = true;
                    }//if
                }//if

                 //Slider Explainer
                if(goalSectionDisplayed && !sliderSectionDisplayed)
                {
                    //Tyler - DEPRECATED, NEW SYSTEM
                    //tutorialController.sliderExplainer();
                    tutorialController.loadTutorial(TutorialController.sliderSection);

                    //Moves to the next section of the tutorial when this one has finished (Alex Jungroth)
                    if(tutorialController.isActiveTutorial == false)
                    {
                        sliderSectionDisplayed = true;
                    }//if
                }//if

                //Voter Explainer
                if(sliderSectionDisplayed == true)
                {
                    //Tyler - DEPRECATED, NEW SYSTEM
                    //tutorialController.voterExplainer();
                    tutorialController.loadTutorial(TutorialController.voterSection);
                }//if
            }//if

            //waits until the number of players has been determined (Alex Jungroth)
            if(MaxNumPlayerSelected)
			{
                SelectParties();
            }//if
		}
		else if(currentState == GameState.PlayerSpawn)
		{	
            //Displays the positioning section of the tutorial if the tutorial is being used (Alex Jungroth)
            if(useTutorial == true)
            {
                //Tyler - DEPRECATED, NEW SYSTEM
                //tutorialController.positioningExplainer();
                tutorialController.loadTutorial(TutorialController.positioningSection);
            }//if

			//spawns players until every player has been spawned (Alex Jungroth)
			if(NumPlayersSpawned < numberPlayers)
			{
				if(votersAppear) 
				{
					tracker = MakeAppear(tracker);
					votersAppear = false;
				}//if

				SpawnPlayer();
			}//if
			else 
			{
                //Sets up the first turn of the game (Alex Jungroth)
                SetUpFirstTurn();
			}//else
		}//else if 
		else if(currentState == GameState.ActionTurns)
		{
            /*if (!TVMoved)
            {
                popUpTVScript.UpdateTVPos(1);
                TVMoved = true;
            }*/

            //Displays the action section of the tutorial if the tutorial is being used (Alex Jungroth)
            if(useTutorial == true)
            {
                //Tyler - DEPRECATED, NEW SYSTEM
                //tutorialController.actionExplainer();
                tutorialController.loadTutorial(TutorialController.actionSection);
            }//if

            //checks to see if the real event array has been initialized (Alex Jungroth)
            if (!IsREAInitalized)
			{
				//sets the isActionTurns to true (Alex Jungroth)
				IsActionState = true;

				//initializes the random event arrays (Alex Jungroth)
				randomEventController.initializeRandomEventsArrays();

				//prevents the random event arrays from being initialized more than once (Alex Jungroth)
				IsREAInitalized = true;
			}//if

			// In Game Heirchy, GameController must set Number Of Rounds greater than 0 in order for this to be called
			if(roundCounter < numberOfRounds)
			{
				PlayerTurn();
			}//if
			else 
			{
				currentState = GameState.RoundEnd;
            }//else


            
        }//else if
        else if(currentState == GameState.RoundEnd)
		{
            //Ends the round (Alex Jungroth)
            EndTheRound();

		}//else if
		else if(currentState == GameState.GameEnd)
		{
            //Displays the election section of the tutorial if the tutorial is being used (Alex Jungroth)
            if(useTutorial == true)
            {
                //Tyler - DEPRECATED, NEW SYSTEM
                //tutorialController.electionExplainer();
                tutorialController.loadTutorial(TutorialController.electionSection);
            }//if

            //Ends the election (Alex Jungroth)
            EndTheElection();
		}
		else if(currentState == GameState.EnactPolicies)
		{
            //Enacts the party policies depending on which party won (Alex Jungroth)
            EnactThePolicies();
		}//else if

		if(inputManager.escButtonDown) 
		{
			SceneManager.LoadScene("TitleScene");
		}
		
	}//Update

    /// <summary>
    /// This sets the audio settings at the start of the game based on the
    /// title screen settings (Alex Jungroth)
    /// </summary>
    void AdjustMusic()
    {
        gameMusic.audioChannels[0].volume = gameSettings.musicVolume;
        
        //sets music settings recieved to false so it doesn't update 
        //the volume every time update is called (Alex Jungroth)
        musicSettingsReceived = false;
    }//AdjustMusic()

    /// <summary>
    /// This sets the players political parties (Alex Jungroth)
    /// </summary>
    void SelectParties()
    {
        //dynamically sizes the players array, but this array should never be
        //larger than the total political parties variable (Alex Jungroth)
        Players = new GameObject[numberPlayers];

        //sends the newly made array of players to the UIScript (Alex Jungroth)
        UIController.setPlayerArray(Players);

        //disables the total party selection (Alex Jungroth)
        UIController.PlayerSelectDisable();

        //updates the game state (Alex Jungroth)
        currentState = GameState.PlayerSpawn;
    }//SelectPlayers()

    /// <summary>
    /// Setups the first turn of the game (Alex Jungroth)
    /// </summary>
    void SetUpFirstTurn()
    {
        //if the player placment is legal sets up the next party selection buttons (Alex Jungroth)
        UIController.correctPlacement();

        //this is the button configuration once all players have been placed (Alex Jungroth)
        //MakeRest(tracker);
        UIController.disablePPButtons();
        UIController.toggleActionButtons();

        currentState = GameState.ActionTurns;
        CurPlayerAction = false;
        //Debug.Log("Round " + (roundCounter + 1) + " begin!");
        //Debug.Log("It's Player " + (currentPlayerTurn + 1) + "'s turn!");

        PlayStartOfTurnAudio();

        //does the tallying before the first player's turn starts (Alex Jungroth)
        tallyRoutine.preTurnTalling();

        //updates the tv so the users know whose turn it is (Alex Jungroth)
        UIController.alterTextBox("It is the " + Players[CurrentPlayerTurn].GetComponent<PlayerVariables>().politicalPartyName +
            " party's turn.\n" + displayPlayerStats());
        UIController.SetPlayerAndParyNameInUpperLeft(Players[CurrentPlayerTurn].GetComponent<PlayerVariables>().politicalPartyName, CurrentPlayerTurn + 1);

        turnsManager.firstTime = true;
        //Gives the randomEventController the list of newly spawned players Brian Mah
        randomEventController.players = Players;
        randomEventController.playersSpawned = true;

        //lets the voters know what the players are
        for (int i = 0; i < Voters.Length; i++)
        {
            Voters[i].GetComponent<VoterVariables>().players = Players;
        }

        //updates the voters (Alex Jungroth)
        UpdateVoterCanidates();

    }//SetUpFirstTurn()

    /// <summary>
    /// Ends one round of the players turns (Alex Jungroth)
    /// </summary>
    void EndTheRound()
    {
        //disables the action buttons (Alex Jungroth)
        for (int i = 0; i < UIController.numActions; i++)
        {
            UIController.ActionButtonObject[i].SetActive(false);
        }//for

        //disables the end turn button(Alex Jungroth)
        UIController.endTurnButton.SetActive(false);

        // Brian Mah
        UIController.alterTextBox("And the Winner is...");

        if (!PreAnnouncmentSFXPlaying)
        {
            gameMusic.FadeOut(0);

            if (electionCounter == numberOfElections - 1)
            {
                SFX.PlayAudioClip(2, 0, SFXVolume);
                PreAnnouncmentSFXTime = Time.time + 5.07f;
            }//if
            else
            {
                gameMusic.LoadTrack(1, 1);
                gameMusic.audioChannels[1].volume = 0;
                gameMusic.FadeIn(1);
                gameMusic.audioChannels[1].Play();
                PreAnnouncmentSFXTime = Time.time + 7.52f;
            }//else
            PreAnnouncmentSFXPlaying = true;
        }//if

        if (Time.time >= PreAnnouncmentSFXTime)
        { // when the sound is done playing

            if (electionCounter == numberOfElections - 1)
            {
                SFX.PlayAudioClip(4, 0, SFXVolume);
            }
            else {
                gameMusic.LoadTrack(2, 1);
                gameMusic.audioChannels[1].Stop();
                gameMusic.PlayElectionTheme();
            }
            
            //does the tallying at the end of the game (Alex Jungroth)
            tallyRoutine.preTurnTalling();

            //Sets the gamemode to game end, and calculates the final score
            currentState = GameState.GameEnd;
        }
        // Once the game ends and calculation is needed, this is called

    }//EndTheRound()

    /// <summary>
    /// Ends one of the games elections (Alex Jungroth)
    /// </summary>
    void EndTheElection()
    {
        //Comapre Votes is no longer used, I have left it here
        //in case we need it for future bug testing (Alex Jungroth)
        //CompareVotes(messaged);

        //increments the election counter (Alex Jungroth)
        electionCounter += 1;

        if (electionCounter < numberOfElections)
        {
            //If the user does not want extra windows, window genrator will not run (Alex Jungroth)
            if (complexElections == true)
            {
                //tells the game controller to not skip over enact policies (Alex Jungroth)
                //WindowGenerator.resumeGame = false;
                Debug.Log("ERROR: COMPLEX ELECTIONS HAVE BEEN DISABLED.");
                //displays who won an election (Alex Jungroth)
                //WindowGenerator.generateElectionVictory(false);
            }//if
            else
            {
                //Calculates the votes independently of window generator (Alex Jungroth)
                calculateWinnnerManager.CalculateVotes();

                //Displays the winner of the election based on votes (Alex Jungroth)
                simpleVictoryDisplay.displayWinner(true);
            }//else

            //manages things between elections (Alex Jungroth)
            prepareElection();

            //sets the game state enact policies(Alex Jungroth)
            currentState = GameState.EnactPolicies;
        }//if
        else
        {
            //If the user does not want extra windows, window genrator will not run (Alex Jungroth)
            if (complexElections == true)
            {
                //displays who won the game (Alex Jungroth)
                //WindowGenerator.generateElectionVictory(true);
                Debug.Log("ERROR: COMPLEX ELECTIONS HAVE BEEN DISABLED.");
                //lets the user know they can reset the game
                //UIController.alterTextBox("Press confirm or the escape key to return to the title screen. Thanks for playing!");
            }//if
            else
            {
                //Calculates the votes independently of window generator (Alex Jungroth)
                calculateWinnnerManager.CalculateVotes();

                //Calculates the victory points independently of window generator (Alex Jungroth)
                calculateWinnnerManager.CalculateVictoryPoints();

                //Displays the winner of the game based on victory points (Alex Jungroth)
                simpleVictoryDisplay.displayWinner(false);

                //Lets the user know they can reset the game
                UIController.alterTextBox("Press the escape key to return to the title screen. Thanks for playing!");
            }//else
            
            //ends the game (Alex Jungroth)
            currentState = GameState.AfterEnd;
        }//else

    }//EndTheElection()

    /// <summary>
    /// 
    /// </summary>
    void EnactThePolicies()
    {
        //If the user does not want extra windows, window genrator will not run (Alex Jungroth)
        /*if (complexElections == true)
        {
            //only enacts policies when the window generator has finished
            if (WindowGenerator.resumeGame == true)
            {
                //enacts a policy (Alex Jungroth)
                if (WindowGenerator.winner != "no winner")
                {
                    for (int i = 0; i < numberPlayers; i++)
                    {
                        if (Players[i].GetComponent<PlayerVariables>().politicalPartyName == WindowGenerator.winner)
                        {
                            //sets electionWinner to the player who won (Alex Jungroth)
                            electionWinner = i;
                        }//if
                    }//for

                    //The winner deposits 25% of their money into a war chest for their constituents (Alex Jungroth)
                    Players[electionWinner].GetComponent<PlayerVariables>().money = (int)Mathf.Floor
                        (Players[electionWinner].GetComponent<PlayerVariables>().money * threeQuatersDecrease);

                    if (Players[electionWinner].GetComponent<PlayerVariables>().chosenPolicy != 0)
                    {
                        //a case structure for calling the correct set of functions based on which player won (Alex Jungroth)
                        switch (Players[electionWinner].GetComponent<PlayerVariables>().politicalPartyName)
                        {
                            case "Apple Pie":
                                partyPolicyManager.GetComponent<NeutralPolicies>().redirectPolicyRequest(Players[electionWinner].GetComponent<PlayerVariables>().chosenPolicy);
                                break;

                            case "Espresso":
                                partyPolicyManager.GetComponent<EspressoPolicies>().redirectPolicyRequest(Players[electionWinner].GetComponent<PlayerVariables>().chosenPolicy);
                                break;

                            case "Drone":
                                partyPolicyManager.GetComponent<DronePolicies>().redirectPolicyRequest(Players[electionWinner].GetComponent<PlayerVariables>().chosenPolicy);
                                break;

                            case "Windy":
                                partyPolicyManager.GetComponent<WindyPolicies>().redirectPolicyRequest(Players[electionWinner].GetComponent<PlayerVariables>().chosenPolicy);
                                break;

                            case "Providence":
                                partyPolicyManager.GetComponent<Party5Policies>().redirectPolicyRequest(Players[electionWinner].GetComponent<PlayerVariables>().chosenPolicy);
                                break;

                            default:
                                //something went wrong with the players names
                                Debug.LogError("Unrecognized player name!");
                                break;
                        }//switch

                        //determines if the party's policy was well received or not (Alex Jungroth)
                        if (Random.value >= half)
                        {
                            //If the party's policy was well received then the party gains money equal to 10% of its total money (Alex Jungroth)
                            Players[electionWinner].GetComponent<PlayerVariables>().money = (int)Mathf.Ceil
                                (Players[electionWinner].GetComponent<PlayerVariables>().money * tenPercentIncrease);
                            SFX.PlayAudioClip(13, 0, SFXVolume);
                        }
                        else
                        {
                            //If the party's policy was poorly received then the party loses money equal to 10% of its total money (Alex Jungroth)
                            Players[electionWinner].GetComponent<PlayerVariables>().money = (int)Mathf.Floor
                                (Players[electionWinner].GetComponent<PlayerVariables>().money * tenPercentDecrease);
                            SFX.PlayAudioClip(14, 0, SFXVolume);
                        }//else
                    }//if
                }//if

                //resets the selected policies for all players (Alex Jungroth)
                for (int i = 0; i < numberPlayers; i++)
                {
                    Players[i].GetComponent<PlayerVariables>().chosenPolicy = 0;
                }//for

                //The UI will not be updated until window generator finishes (Alex Jungroth)

                //updates the winner's money on the main TV and displays to the TV that it is player 1's turn (Alex Jungroth)
                UIController.alterTextBox("It is the " + Players[0].GetComponent<PlayerVariables>().politicalPartyName +
                    " Party's turn.\n" + displayPlayerStats());

                //enables the end turn and player stats buttons
                UIController.endTurnButton.SetActive(true);

                //enables the action buttons
                for (int i = 0; i < UIController.numActions; i++)
                {
                    UIController.ActionButtonObject[i].SetActive(true);
                }

                UIController.SetPlayerAndParyNameInUpperLeft(Players[0].GetComponent<PlayerVariables>().politicalPartyName, 1);

                //resets the game state to action turns (Alex Jungroth)
                currentState = GameState.ActionTurns;

                //Brian Mah
                //audio stuff, fades in regular music, fades out election music
                //also stops the custom loop in music controller
                gameMusic.FadeIn(0);
                gameMusic.FadeOut(1);
                gameMusic.StopElectionTheme();
            }//if
        }*///if
        //else
        //{
            //Updates the UI immediately if window generator is not being used (Alex Jungroth)

            //updates the winner's money on the main TV and displays to the TV that it is player 1's turn (Alex Jungroth)
            UIController.alterTextBox("It is the " + Players[CurrentPlayerTurn].GetComponent<PlayerVariables>().politicalPartyName +
                " Party's turn.\n" + displayPlayerStats());

            //enables the end turn and player stats buttons
            UIController.endTurnButton.SetActive(true);

            //enables the action buttons
            for (int i = 0; i < UIController.numActions; i++)
            {
                UIController.ActionButtonObject[i].SetActive(true);
            }

            UIController.SetPlayerAndParyNameInUpperLeft(Players[0].GetComponent<PlayerVariables>().politicalPartyName, 1);

            //resets the game state to action turns (Alex Jungroth)
            currentState = GameState.ActionTurns;

            //Brian Mah
            //audio stuff, fades in regular music, fades out election music
            //also stops the custom loop in music controller
            gameMusic.FadeIn(0);
            gameMusic.FadeOut(1);
            gameMusic.StopElectionTheme();
        //}  
    }//EnactThePolicies()

    /// <summary>
    /// This is the part of the game controler that I changed to support 5 players (Alex Jungroth)
    /// Spawns the player and enables player placment controlls.
    /// Disables controlls upon confirmation and last player placed
    /// </summary>
    void SpawnPlayer()
	{
		//prevents update from spamming this part of the function (Alex Jungroth)
		if (CurPlayerFinishedSpawning) 
		{
			//enables the player placement movement controls (Alex Jungroth)
			UIController.enablePPButtonsPartySelection ();

            //this is code for spawning different parites
            //depending on what party the player chose, this is what they will spawn as
            //each party can only be chosen once
            switch (ChosenParty[CurPlayerSpawning])
            {
                case 0:
                    //If unique parties are enabled give the players each unique models (Alex Jungroth) 
                    //if(uniqueParties == true)
                    //{
                    currentPlayer = Instantiate(Party_1Template, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                        //this.playerTemplate = coffeeTemplate;
                    //}
                    //else
                    //{
                    //    currentPlayer = Instantiate(Sphere_1Template, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                        //this.playerTemplate = coffeeSphereTemplate;
                    //}//else

                    //Tells the user which party they picked (Alex Jungroth)
                    //if (uniqueParties == true)
                    //{
                    //    UIController.alterTextBox("You have chosen the Espresso Party.");
                    //}//if
                    //else
                    //{
                    UIController.alterTextBox("You have chosen the Brown Party.");
                    //}//else

				break;
				
				case 1:
                    //If unique parties are enabled give the players each unique models (Alex Jungroth) 
                    //if (uniqueParties == true)
                    //{
                    currentPlayer = Instantiate(Party_2Template, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                        //this.playerTemplate = party3Template;
                    //}
                    //else
                    //{
                    //    currentPlayer = Instantiate(Sphere_2Template, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                        //this.playerTemplate = party3SphereTemplate;
                    //}//else
            
                    //Tells the user which party they picked (Alex Jungroth)
                    //if (uniqueParties == true)
                    //{
                    //    UIController.alterTextBox("You have chosen the Espresso Party.");
                    //}//if
                    //else
                    //{
                    UIController.alterTextBox("You have chosen the Red Party.");
                    //}//else

                    break;

				case 2:
                    //If unique parties are enabled give the players each unique models (Alex Jungroth) 
                    //if (uniqueParties == true)
                    //{
                    currentPlayer = Instantiate(Party_3Template, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                        //this.playerTemplate = neutralTemplate;
                    //}
                    //else
                    //{
                    //    currentPlayer = Instantiate(Sphere_3Template, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                        //this.playerTemplate = neutralSphereTemplate;
                    //}//else

                    //Tells the user which party they picked (Alex Jungroth)
                    //if (uniqueParties == true)
                    //{
                    //    UIController.alterTextBox("You have chosen the Espresso Party.");
                    //}//if
                    //else
                    //{
                    UIController.alterTextBox("You have chosen the Yellow Party.");
                    //}//else

                    break;
		
				case 3:
                    //If unique parties are enabled give the players each unique models (Alex Jungroth) 
                    //if (uniqueParties == true)
                    //{
                    currentPlayer = Instantiate(Party_4Template, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                        //this.playerTemplate = party4Template;
                    //}
                    //else
                    //{
                    //    currentPlayer = Instantiate(Sphere_4Template, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                        //this.playerTemplate = party4SphereTemplate;
                    //}//else

                    //Tells the user which party they picked (Alex Jungroth)
                    //if (uniqueParties == true)
                    //{
                    //    UIController.alterTextBox("You have chosen the Espresso Party.");
                    //}//if
                    //else
                    //{
                    UIController.alterTextBox("You have chosen the Green Party.");
                    //}//else

                    break;
		
				case 4:
                    //If unique parties are enabled give the players each unique models (Alex Jungroth) 
                    //if (uniqueParties == true)
                    //{
                    currentPlayer = Instantiate(Party_5Template, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                        //this.playerTemplate = party5Template;
                    //}
                    //else
                    //{
                    //    currentPlayer = Instantiate(Sphere_5Template, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                        //this.playerTemplate = party5SphereTemplate;
                    //}//else

                    //Tells the user which party they picked (Alex Jungroth)
                    //if (uniqueParties == true)
                    //{
                    //    UIController.alterTextBox("You have chosen the Espresso Party.");
                    //}//if
                    //else
                    //{
                    UIController.alterTextBox("You have chosen the Purple Party.");
                    //}//else

                    break;
			}

            //Increments the number of players that have spawned (Alex Jungroth)
            totalPlayersSpawned++;

            //Updates the current player with its player number (Alex Jungroth)
            currentPlayer.GetComponent<PlayerVariables>().playerNumber = totalPlayersSpawned;

			//stores the current player into the array of players (Alex Jungroth)
			Players [NumPlayersSpawned] = currentPlayer;

			//prevents update from calling this part of the function again (Alex Jungroth)
			CurPlayerFinishedSpawning = false;
		}

		//checks the player against all of the previous players to ensure no duplicates
		for(int i = 0; i < NumPlayersSpawned; i++)
		{
			if (currentPlayer.transform.position == Players[i].transform.position)
			{
				//if they are on the same spot
				CurPlayerConfirmPlacement = false;
			}
		}

		if(CurPlayerConfirmPlacement)
		{ 
			//increments the players spawned (Alex Jungroth)
			NumPlayersSpawned++;

			//increments the player being spawned (Alex Jungroth)
			CurPlayerSpawning++;

			//prevents more than five players from spawning (Alex Jungroth)
			CurPlayerFinishedSpawning = true;

			//at most, confirm should only be able to be pressed five times (Alex Jungroth)
			CurPlayerConfirmPlacement = false;
			votersAppear = true;

			if(numberPlayers == NumPlayersSpawned)
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
		if(CurPlayerAction) {

			if(CurrentPlayerTurn < numberPlayers)
			{
				/*I am moving the incrementing of current player turn to 
				inside this if statement to prevent current player turn 
				form endlessly growing when the random event controller 
				is being. This can not go into the if statement below
				as that will call an index out of range exception (Alex Jungroth)*/
				CurrentPlayerTurn++;
			}

			if(CurrentPlayerTurn < numberPlayers) {
				CurPlayerAction = false;
				//Debug.Log ("It's Player " + (currentPlayerTurn + 1) + "'s turn!");
				//updates the tv so the users know whose turn it is (Alex Jungroth)
				UIController.alterTextBox("It is the " + Players[CurrentPlayerTurn].GetComponent<PlayerVariables>().politicalPartyName +
					" party's turn.\n" + displayPlayerStats());
				UIController.SetPlayerAndParyNameInUpperLeft(Players[CurrentPlayerTurn].GetComponent<PlayerVariables>().politicalPartyName, CurrentPlayerTurn + 1);
				PlayStartOfTurnAudio ();
			}

			if(CurrentPlayerTurn >= numberPlayers) {
                //this is when all players have made their turns

                //Displays the random event section of the tutorial if the tutorial is being used (Alex Jungroth)
                if(useTutorial == true)
                {
                    //Tyler - DEPRECATED, NEW SYSTEM
                    //tutorialController.randomEventExplainer();
                    tutorialController.loadTutorial(TutorialController.randomEventSection);
                }//if

                //disables the action buttons during the random events (Alex Jungroth)
                UIController.disableActionButtons();

				if(randomEventController.ActivateEvents()){  //continually goes to random event controller until randomEventController returns true

					//does the tallying after the players ends there turns (Alex Jungroth)
					tallyRoutine.preTurnTalling();
					
					//this is when the new round begins
					roundCounter++;
					CurrentPlayerTurn = 0;
					CurPlayerAction = false;
					
					if (roundCounter < numberOfRounds) {
						//Debug.Log ("Round " + (roundCounter + 1) + " begin!");
						//Debug.Log ("It's Player " + (currentPlayerTurn + 1) + "'s turn!");

						//updates the tv so the users know whose turn it is (Alex Jungroth)
						UIController.alterTextBox("It is the " + Players[CurrentPlayerTurn].GetComponent<PlayerVariables>().politicalPartyName +
							" party's turn.\n" + displayPlayerStats());
						UIController.SetPlayerAndParyNameInUpperLeft(Players[CurrentPlayerTurn].GetComponent<PlayerVariables>().politicalPartyName, CurrentPlayerTurn + 1);
						PlayStartOfTurnAudio ();
					} else {
						//Debug.Log ("Game Ends!");
					}
				}
			}
		}
	}

	/// <summary>
	/// Plays the start of turn audio.
	/// </summary>
	void PlayStartOfTurnAudio ()
	{
		switch(Players [CurrentPlayerTurn].GetComponent<PlayerVariables> ().politicalPartyName) {
		case "Espresso":
			SFX.PlayAudioClip (8, 0, SFXVolume);
			break;
		case "Windy":
			SFX.PlayAudioClip (12, 0, SFXVolume);
			break;
		case "Providence":
			SFX.PlayAudioClip (11, 0, SFXVolume);
			break;
		case "Apple Pie":
			SFX.PlayAudioClip (10, 0, SFXVolume);
			break;
		case "Drone":
			SFX.PlayAudioClip (9, 0, SFXVolume);
			break;
		}
	}

	/// <summary>
	/// Displaies the player stats.
	/// </summary>
	/// <returns>The player stats.</returns>
	public string displayPlayerStats()
	{
		UIController.tvAnimator.SetTrigger ("StatsAnimation");

		//gets the current players money
		currentPlayerMoney = Players[CurrentPlayerTurn].GetComponent<PlayerVariables> ().money; 
		
		//gets the current players votes
		currentPlayerVotes = Players[CurrentPlayerTurn].GetComponent<PlayerVariables> ().votes;
		
		//compiles the players stats into one string
		return("The "+ Players[CurrentPlayerTurn].GetComponent<PlayerVariables>().politicalPartyName + " Party has $" + currentPlayerMoney.ToString() + 
			"m and " + currentPlayerVotes.ToString() + "k votes.");
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
			if(Players[i].GetComponent<PlayerVariables>().votes > mostVotes){
				mostVotes = Players[i].GetComponent<PlayerVariables>().votes; 
				winningPlayer = i + 1;
			}
			else if(Players[i].GetComponent<PlayerVariables>().votes == mostVotes) {
				//Debug.Log ("here");
				tieVotes = Players[i].GetComponent<PlayerVariables>().votes;
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
        PreAnnouncmentSFXPlaying = false;
		PreAnnouncmentSFXTime = 3.7f;
		
		for (int i = 0; i < NumVoters; i++) 
		{
			//this handles voter resistance cool down (Alex Jungroth)
			Voters [i].GetComponent<VoterVariables> ().baseResistance *= 0.5f;
			
			//gets a random value from 0 to 5 (Alex Jungroth)
			tempRandom = Random.Range(0f, 6.0f);
			
			//has a chance to move the voters in a random direction (Alex Jungroth)
			switch ((int)tempRandom)
			{
			case 0:
				if (Random.value > Voters [i].GetComponent<VoterVariables> ().xPlusResistance + (int)Voters [i].GetComponent<VoterVariables> ().baseResistance)
				{
					//move voter in the +X direction (Alex Jungroth)
					temp = Voters[i].transform.position;
					temp += new Vector3(1,0,0);
					
					//makes sure the voter doesn't move off the grid or onto another voter (Alex Jungroth)
					if((temp.x < gridSize - 1) && (overlapCheck(temp) == true))
					{
						Voters[i].transform.position = temp;
					}
				}
				break;
				
			case 1:
				if (Random.value > Voters [i].GetComponent<VoterVariables> ().xMinusResistance + (int)Voters [i].GetComponent<VoterVariables> ().baseResistance)
				{
					//move voter in the -X direction (Alex Jungroth)
					temp = Voters[i].transform.position;
					temp -= new Vector3(1,0,0);
					
					//makes sure the voter doesn't move off the grid or onto another voter (Alex Jungroth)
					if((temp.x > 0)  && (overlapCheck(temp) == true))
					{
						Voters[i].transform.position = temp;
					}
				}
				break;
				
			case 2:
				if (Random.value > Voters [i].GetComponent<VoterVariables> ().yPlusResistance + (int)Voters [i].GetComponent<VoterVariables> ().baseResistance) 
				{
					//move voter in the +Y direction (Alex Jungroth)
					temp = Voters[i].transform.position;
					temp += new Vector3(0,1,0);
					
					//makes sure the voter doesn't move off the grid or onto another voter (Alex Jungroth)
					if((temp.y < gridSize - 1) && (overlapCheck(temp) == true))
					{
						Voters[i].transform.position = temp;
					}
				}
				break;
				
			case 3:
				if (Random.value > Voters [i].GetComponent<VoterVariables> ().yMinusResistance + (int)Voters [i].GetComponent<VoterVariables> ().baseResistance) 
				{
					//move voter the -Y direction (Alex Jungroth)
					temp = Voters[i].transform.position;
					temp -= new Vector3(0,1,0);
					
					//makes sure the voter doesn't move off the grid or onto another voter (Alex Jungroth)
					if((temp.y > 0) && (overlapCheck(temp) == true))
					{
						Voters[i].transform.position = temp;
					}
				}
				break;
				
			case 4:
				if (Random.value > Voters [i].GetComponent<VoterVariables> ().zPlusResistance + (int)Voters [i].GetComponent<VoterVariables> ().baseResistance) 
				{
					//move voter the +Z direction (Alex Jungroth)
					temp = Voters[i].transform.position;
					temp += new Vector3(0,0,1);
					
					//makes sure the voter doesn't move off the grid or onto another voter (Alex Jungroth)
					if((temp.z < gridSize - 1) && (overlapCheck(temp) == true))
					{
						Voters[i].transform.position = temp;
					}
				}
				break;
				
			case 5:
				if (Random.value > Voters [i].GetComponent<VoterVariables> ().zMinusResistance + (int)Voters [i].GetComponent<VoterVariables> ().baseResistance)
				{
					//move voter the -Z direction (Alex Jungroth)
					temp = Voters[i].transform.position;
					temp -= new Vector3(0,0,1);
					
					//makes sure the voter doesn't move off the grid or onto another voter (Alex Jungroth)
					if((temp.z > 0) && (overlapCheck(temp) == true))
					{
						Voters[i].transform.position = temp;
					}
				}
				break;
			}
		}
		
		//deletes all of the players shadow positions and clears their shadow position list (Alex Jungroth)
		for (int i = 0; i < numberPlayers; i++) 
		{
			for(int j = 0; j < Players[i].GetComponent<PlayerVariables>().shadowPositions.Count; j++)
			{
				Destroy(Players[i].GetComponent<PlayerVariables>().shadowPositions[j]);
			}
			
			Players[i].GetComponent<PlayerVariables>().shadowPositions.Clear();
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
			if(temp == Voters[i].transform.position)
			{
				return false;
			}
		}
		
		return true;	
	}

	//function to make all voters check who they are closest to
	public void UpdateVoterCanidates()
    {
		//Debug.Log("voters updated");
		for (int i = 0; i < Voters.Length; i++)
        {
			Voters[i].GetComponent<VoterVariables>().FindCanidate();
		}
	}

	public int MakeAppear(int tracker)
	{
		for (int i = 0; i < Voters.Length/numberPlayers; i++) {
			Voters [tracker].gameObject.SetActive (true);
			tracker++;
		}
		return tracker;
	}

	public void MakeRest(int tracker)
	{
		for (int i = 0; i < Voters.Length%numberPlayers; i++)
        {
			//Debug.Log (tracker);
			Voters [tracker].gameObject.SetActive (true);
			tracker++;
		}

	}

    public void GettingVariableReferences()
    {
        try
        {
            gameSettings = GameObject.FindGameObjectWithTag("TitleSettings").GetComponent<TitleScreenSettings>();
        }
        catch
        {
            Debug.LogError("Could not find the title screen settings, because you did not start from the title screen!");
        }
        if (gameSettings == null)
        {
            //throws an error if the gameController did not receive the title screen settings (Alex Jungroth)
            //Debug.Log("You may continue play testing!");

            //The default play test from the prototype scene is to have unique parties, complex elections, pedestals, and the tutorial (Alex Jungroth)
            uniqueParties = false;
            complexElections = false;
            usePedestals = TOGGLE_PEDESTAL;
            useTutorial = TOGGLE_TUT;
            gridSize = GRID_SIZE;
            numberOfRounds = NUM_OF_ROUNDS;
            numberOfElections = NUM_OF_ELECTIONS;
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
            uniqueParties = gameSettings.uniqueParties;
            complexElections = gameSettings.complexElections;
            usePedestals = gameSettings.usePedestals;
            useTutorial = gameSettings.useTutorial;
        }
        gameMusic = GameObject.FindGameObjectWithTag("Music").GetComponent<MusicController>();
        if (gameMusic == null)
        {
            Debug.LogError("The Game Controller could not find the Music Controller please place it in the scene.");
        }

        SFX = GameObject.FindGameObjectWithTag("SFX").GetComponent<SFXController>();
        if (SFX == null)
        {
            Debug.LogError("The Game Controller could not find the SFX Controller please place it in the scene.");
        }

        inputManager = GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManagerScript>();
        if (inputManager == null)
        {
            Debug.LogError("The Game Controller could not find the Input manager please place it in the scene.");
        }

    }

    //THESE FUNCTIONS ARE COMPLETELY UNNECCASARY 
    /*
    /// <summary>
    /// Calls the function that calculates the votes. (Alex Jungroth)
    /// </summary>
    public void callCalculateVotes()
    {
        
    }

    /// <summary>
    /// Calls the function that calculates the victory points. (Alex Jungroth)
    /// </summary>
    public void callCalculateVictoryPoints()
    {
        
    }
    */
}//Gamecontroller Class