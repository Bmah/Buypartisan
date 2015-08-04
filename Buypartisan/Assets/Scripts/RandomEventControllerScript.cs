using UnityEngine;
using System.Collections;

/// <summary>
/// Random event controller script.
/// Brian Mah
/// </summary>
public class RandomEventControllerScript : MonoBehaviour {
	
	public GameObject[] voters;
	public GameObject[] players;
	public bool playersSpawned = false;
	private int numberOfActions;
	//private bool ActionCounterSetup = false;
	
	//for debugging
	public int arrayChoice = 0;
	public int[] debugArray;
	
	public int[][] actionCounter;
	private int[] actionThreshold = {3,3,3,3,3,3,3,3};
	private bool[][] eventTriggerList;
	
	public VoterVariables[] voterVars = null;
	private bool voterVarsSet = false;
	
	public UI_Script UIController;
	
	public int gridSize;
	
	public enum ActionState {StartEvents, WaitForTriggeredEvent, CheckForTriggeredEvents, ActivateTriggeredEvents, EndRandomEvents};
	private enum TriggeredEventState {E0, Wait0, E1, Wait1, E2, Wait2, E3, Wait3, E4, Wait4, E5, Wait5, E6, Wait6, E7, Wait7, EndOfTriggeredEvents};
	TriggeredEventState triggerState = TriggeredEventState.E0;
	ActionState currentState = ActionState.StartEvents;
	int playerTriggerNumber = 0;
	
	InputManagerScript Inputs = null;

	//I made these public so I could use them for party policies (Alex Jungroth)
	public bool naturalDisaster = false;
	public bool lossInWar = false;
	public bool marketCrash = false;
	bool extinctionEvent = false;

	private SFXController SFX;
	public float SFXvolume;

	// Use this for initialization
	void Start () {
		numberOfActions = actionThreshold.Length;
		
		if (UIController == null) {
			Debug.LogError("UI_Script not set on RandomEventController");
		}

		Inputs = GameObject.FindGameObjectWithTag ("InputManager").GetComponent<InputManagerScript> ();
		if (Inputs == null) {
			Debug.LogError("Could not Find Input Manager");
		}

		SFX = GameObject.FindGameObjectWithTag ("SFX").GetComponent<SFXController> ();
		if (SFX == null) {
			Debug.LogError("SFX could not be found by random event controller please place it in the scene.");
		}
	}

	/// <summary>
	/// Initializes the random events arrays.
	/// </summary>
	public void initializeRandomEventsArrays ()
	{
		GameObject temp = GameObject.FindGameObjectWithTag ("GameController");
		if (temp != null) {
			actionCounter = new int[temp.GetComponent<GameController> ().numberPlayers][];
			eventTriggerList = new bool[actionCounter.Length][];
		}
		else {
			Debug.LogError ("Could not find Gamecontroller");
		}
		//initializing the event trigger list
		for (int i = 0; i < eventTriggerList.Length; i++) {
			eventTriggerList [i] = new bool[numberOfActions];
			for (int j = 0; j < eventTriggerList [0].Length; j++) {
				eventTriggerList [i] [j] = false;
			}
		}
		for (int i = 0; i < actionCounter.Length; i++) {
			actionCounter [i] = new int[numberOfActions];
		}
	}


	// Update is called once per frame
	void Update () {
		if (!voterVarsSet) {
			voterVars = new VoterVariables[voters.Length];
			for(int i = 0; i < voters.Length; i++){
				voterVars[i] = voters[i].GetComponent<VoterVariables>();
			}
			voterVarsSet = true;
		}
		
	}// update
	
	/// <summary>
	/// Activates the events.
	/// Brian Mah
	/// </summary>
	public bool ActivateEvents(){
		
		switch (currentState){
		case ActionState.StartEvents :
			currentState = ActionState.WaitForTriggeredEvent;
			StandardEvents ();
			SFX.PlayAudioClip(5,0,SFXvolume);
			break;
		case ActionState.WaitForTriggeredEvent :
			if(Inputs.leftClickDown){
				currentState = ActionState.CheckForTriggeredEvents;
			}
			break;
		case ActionState.CheckForTriggeredEvents :
			CheckForTriggeredEvents();
			playerTriggerNumber = 0;
			currentState = ActionState.ActivateTriggeredEvents;
			break;
		case ActionState.ActivateTriggeredEvents :
			if(DoTriggeredEvents(playerTriggerNumber)){
				currentState = ActionState.EndRandomEvents;
			}
			//This needs to happen sooner in the Game Controller function Player Turn (Alex Jungroth)
			//UIController.disableActionButtons();
			break;
			
		case ActionState.EndRandomEvents:
			currentState = ActionState.StartEvents;
			UIController.alterTextBox("Events are over,\nthe Election Continues!");
			UIController.toggleActionButtons();
			return true;
		}
		
		return false;
	}
	
	void StandardEvents ()
	{
		int eventChoice = Random.Range (0, 10);
		switch (eventChoice) {
		case 0:
			ShiftVoters ('X', 1);
			UIController.alterTextBox ("Newsflash! Sudden victory in the war boosts confidence in big govt! " + 
			                           "Voters migrate 1 up on the X axis.\nLeft Click to Continue");
			lossInWar = false;
			break;
		case 1:
			ShiftVoters ('X', -1);
			UIController.alterTextBox ("Newsflash! Sudden defeat in the war crushes confidence in big govt! " + 
			                           "Voters migrate 1 down on the X axis.\nLeft Click to Continue");
			lossInWar = true;
			break;
		case 2:
			ShiftVoters ('Y', 1);
			UIController.alterTextBox ("Newsflash! New developments in the field of oil drilling lead to profit for big buisness. " + 
			                           "Voters migrate 1 up on the Y axis.\nLeft Click to Continue");
			naturalDisaster = false;
			break;
		case 3:
			ShiftVoters ('Y', -1);
			UIController.alterTextBox ("Newsflash! Sudden oil spill causes huge natural disaster, public outraged with big buisness." + 
			                           "Voters migrate 1 down on the Y axis.\nLeft Click to Continue");
			naturalDisaster = true;
			break;
		case 4:
			ShiftVoters ('Z', 1);
			UIController.alterTextBox ("Newsflash! Popular celebrity endorses the Z axis. " + 
			                           "Voters migrate 1 up on the Z axis.\nLeft Click to Continue");
			break;
		case 5:
			ShiftVoters ('Z', -1);
			UIController.alterTextBox ("Newsflash! Popular celebrity denounces the Z axis. " + 
			                           "Voters migrate 1 down on the Z axis.\nLeft Click to Continue");
			break;
		case 6:
			EconomicBoom (2);
			UIController.alterTextBox ("Newsflash! Market Bubble. " + 
			                           "Voters now have twice the money they used to!\nLeft Click to Continue");
			marketCrash = false;
			break;
		case 7:
			EconomicBust (2);
			UIController.alterTextBox ("Newsflash! Poor investments in tulip market lead to market crash. " + 
			                           "Voters now have half the money they used to!\nLeft Click to Continue");
			marketCrash = true;
			break;
		case 8:
			if(naturalDisaster && marketCrash && lossInWar && !extinctionEvent){
				UIController.alterTextBox ("PANIC: due to loss in the War, the Market Crash, and Oil Spill, " + 
				                           "Mass Extinction has occurred!\nLeft Click to Continue");
				extinctionEvent = true;
				SFX.PlayAudioClip(7,0,SFXvolume);
				ExtinctionEvent();
			}
			else{
				UIController.alterTextBox ("Newsflash! Little Tommy fell down the well!\nLeft Click to Continue");
			}
			break;
		default:
			UIController.alterTextBox ("Newsflash! Little Timmy fell down the well!\nLeft Click to Continue");
			break;
		}
	}

	/// <summary>
	/// Kills all the voters but one
	/// </summary>
	void ExtinctionEvent(){
		int survivor = Random.Range(0,voters.Length);
		for (int i = 0; i < voters.Length; i++) {
			if (i != survivor){
				voters[i].SetActive(false);

				//stops the dead voters from voting (Alex Jungroth) & (Brian Mah)
				voters[i].GetComponent<VoterVariables>().votes = 0;
				voters[i].GetComponent<VoterVariables>().money = 0;
			}
		}
	}
	
	/// <summary>
	/// Shifts all voters by magnitude length in the specified direction.
	/// Brian Mah
	/// </summary>
	/// <value>The shift voters.</value>
	void ShiftVoters(char direction, int magnitude){
		direction = char.ToUpper (direction);
		bool failToMove = false;
		switch (direction) {
		case 'X':
			for(int i = 0; i < voters.Length; i++){
				failToMove = false;
				Vector3 temporaryPosition = voters[i].transform.position;
				if((magnitude > 0 && temporaryPosition.x < gridSize - magnitude)||(magnitude < 0 && temporaryPosition.x > -1 - magnitude)){
					temporaryPosition.x = temporaryPosition.x + magnitude;
					for(int j = 0; j < voters.Length && !failToMove; j++){
						if(voters[j].transform.position == temporaryPosition){
							failToMove = true;
						}
					}
				}
				
				//check to see if voter resistance prevents event from moving voter
				if(magnitude < 0 &&  Random.value < voterVars[i].xMinusResistance + voterVars[i].baseResistance){
					failToMove = true;
				}
				else if(magnitude > 0 &&  Random.value < voterVars[i].xPlusResistance + voterVars[i].baseResistance){
					failToMove = true;
				}
				
				if(!failToMove)
				{
					voters[i].transform.position = temporaryPosition;
				}
			}
			break;
		case 'Y':
			for(int i = 0; i < voters.Length; i++){
				failToMove = false;
				Vector3 temporaryPosition = voters[i].transform.position;
				if((magnitude > 0 && temporaryPosition.y < gridSize - magnitude)||(magnitude < 0 && temporaryPosition.y > -1 - magnitude)){
					temporaryPosition.y = temporaryPosition.y + magnitude;
					for(int j = 0; j < voters.Length && !failToMove; j++){
						if(voters[j].transform.position == temporaryPosition){
							failToMove = true;
						}
					}
				}
				
				//check to see if voter resistance prevents event from moving voter
				if(magnitude < 0 &&  Random.value < voterVars[i].yMinusResistance + voterVars[i].baseResistance){
					failToMove = true;
				}
				else if(magnitude > 0 &&  Random.value < voterVars[i].yPlusResistance + voterVars[i].baseResistance){
					failToMove = true;
				}
				
				if(!failToMove)
				{
					voters[i].transform.position = temporaryPosition;
				}
			}
			break;
		case 'Z':
			for(int i = 0; i < voters.Length; i++){
				failToMove = false;
				Vector3 temporaryPosition = voters[i].transform.position;
				if((magnitude > 0 && temporaryPosition.z < gridSize - magnitude)||(magnitude < 0 && temporaryPosition.z > -1 - magnitude)){
					temporaryPosition.z = temporaryPosition.z + magnitude;
					for(int j = 0; j < voters.Length && !failToMove; j++){
						if(voters[j].transform.position == temporaryPosition){
							failToMove = true;
						}
					}
				}
				
				//check to see if voter resistance prevents event from moving voter
				if(magnitude < 0 &&  Random.value < voterVars[i].zMinusResistance + voterVars[i].baseResistance){
					failToMove = true;
				}
				else if(magnitude > 0 &&  Random.value < voterVars[i].zPlusResistance + voterVars[i].baseResistance){
					failToMove = true;
				}
				
				if(!failToMove)
				{
					voters[i].transform.position = temporaryPosition;
				}
			}
			break;
		default:
			Debug.LogError("Non XYZ axis specified in ShiftVoters");
			break;
		}
	}
	
	/// <summary>
	/// Multiplies every voter's money by the boom amount.
	/// Brian Mah
	/// </summary>
	/// <param name="multiplier">Multiplier.</param>
	void EconomicBoom(int multiplier){
		for(int i = 0; i < voters.Length; i++){
			voters[i].GetComponent<VoterVariables>().money *= multiplier;
		}
	}
	
	/// <summary>
	/// Divides every voter's money by the bust amount.
	/// Brian Mah
	/// </summary>
	/// <param name="divisor">Divisor.</param>
	void EconomicBust(int divisor){
		for(int i = 0; i < voters.Length; i++){
			voters[i].GetComponent<VoterVariables>().money /= divisor;
		}
	}
	
	void CheckForTriggeredEvents(){
		for (int i = 0; i < actionCounter.Length; i++) {//for each player
			for(int j = 0; j < actionCounter[0].Length; j++){//for each action
				if (actionCounter[i][j] >= actionThreshold[j] &&  //if you have reached the threshold
				    (Random.value < ((actionCounter[i][j] - actionThreshold[j]) * 0.1f + 0.3f))){ //and rng decides you 
					//activate triggered event j with probability of 30% plus 10% * amount you have gone over threshold
					eventTriggerList[i][j] = true;
				}
				
				//After checking to see if the event is triggered cool down the check
				if(actionCounter[i][j] > 0){
					actionCounter[i][j]--;
				}
			}
		}//for each player
	}//Check For Triggered events
	
	bool DoTriggeredEvents(int player){
		switch (triggerState){
		case TriggeredEventState.E0:
			if(eventTriggerList[player][0]){
				eventTriggerList[player][0] = false;
				//VoterSupression
				VoterOutrage(players[player]);
				SFX.PlayAudioClip(5,0,SFXvolume);
				UIController.alterTextBox("Triggered Event\nNewsflash! Voters outraged at suppression by player "+ (player+1) +
				                          " Voters gather at the polls to vote against them!\nLeft Click to Continue");
				triggerState = TriggeredEventState.Wait0;
			}
			else{
				triggerState = TriggeredEventState.E1;
			}
			break;
		case TriggeredEventState.Wait0:
			if(Inputs.leftClickDown){
				triggerState = TriggeredEventState.E1;
			}
			break;
		case TriggeredEventState.E1:
			if(eventTriggerList[player][1]){
				eventTriggerList[player][1] = false;
				//MoveParty
				FlipFlopping(players[player]);
				SFX.PlayAudioClip(5,0,SFXvolume);
				UIController.alterTextBox("Triggered Event\nNewsflash! Voters irritated by player "+ (player+1) + "'s flip flopping, " +
				                          "voters distance themselves from the candidate!\nLeft Click to Continue");
				triggerState = TriggeredEventState.Wait1;
			}
			else{
				triggerState = TriggeredEventState.E2;
			}
			break;
		case TriggeredEventState.Wait1:
			if(Inputs.leftClickDown){
				triggerState = TriggeredEventState.E2;
			}
			break;
			
		case TriggeredEventState.E2:
			if(eventTriggerList[player][2]){
				eventTriggerList[player][2] = false;
				//InfluenceVoters
				VoterManipulation(players[player]);
				SFX.PlayAudioClip(5,0,SFXvolume);
				UIController.alterTextBox("Triggered Event\nNewsflash! Voters shocked at player "+ (player+1) + "'s manipulation of votes.  " +
				                          "Player "+ (player+1) + " fined for their crime\nLeft Click to Continue");
				triggerState = TriggeredEventState.Wait2;
			}
			else{
				triggerState = TriggeredEventState.E3;
			}
			break;
		case TriggeredEventState.Wait2:
			if(Inputs.leftClickDown){
				triggerState = TriggeredEventState.E3;
			}
			break;
			
		case TriggeredEventState.E3:
			if(eventTriggerList[player][3]){
				eventTriggerList[player][3] = false;
				//ShadowPosition
				ContradictoryPositions (players[player]);
				SFX.PlayAudioClip(5,0,SFXvolume);
				UIController.alterTextBox("Triggered Event\nNewsflash! Player "+ (player+1) + " called out on contradictory positions " +
				                          "player "+ (player+1) + "'s shadow position is removed\nLeft Click to Continue");
				triggerState = TriggeredEventState.Wait3;
			}
			else{
				triggerState = TriggeredEventState.E4;
			}
			break;
		case TriggeredEventState.Wait3:
			if(Inputs.leftClickDown){
				triggerState = TriggeredEventState.E4;
			}
			break;
			
		case TriggeredEventState.E4:
			if(eventTriggerList[player][4]){
				eventTriggerList[player][4] = false;
				//CampaignTour
				AdBurnout(players[player]);
				SFX.PlayAudioClip(5,0,SFXvolume);
				UIController.alterTextBox("Triggered Event\nNewsflash! Voters tired of Player "+ (player+1) + "'s ads" +
				                          " voters now harder to move!\nLeft Click to Continue");
				//smaller size sphere
				triggerState = TriggeredEventState.Wait4;
			}
			else{
				triggerState = TriggeredEventState.E5;
			}
			break;
		case TriggeredEventState.Wait4:
			if(Inputs.leftClickDown){
				triggerState = TriggeredEventState.E5;
			}
			break;
			
		case TriggeredEventState.E5:
			if(eventTriggerList[player][5]){
				eventTriggerList[player][5] = false;
				//SphereOfInfluence
				OverreachingCampaign(players[player]);
				SFX.PlayAudioClip(5,0,SFXvolume);
				UIController.alterTextBox("Triggered Event\nNewsflash! Player "+ (player+1) + " tries to expand their campaign's reach too far " +
				                          " no consequences for this action as of yet\nLeft Click to Continue");
				triggerState = TriggeredEventState.Wait5;
			}
			else{
				triggerState = TriggeredEventState.EndOfTriggeredEvents;
			}
			break;
		case TriggeredEventState.Wait5:
			if(Inputs.leftClickDown){
				triggerState = TriggeredEventState.EndOfTriggeredEvents;
			}
			break;
		case TriggeredEventState.EndOfTriggeredEvents:
			triggerState = TriggeredEventState.E0;
			if (playerTriggerNumber < actionCounter.Length - 1){
				playerTriggerNumber++;
			}
			else{
				return true;
			}
			break;
		}//switch
		
		return false;
	}
	
	/// <summary>
	/// Voter Outrage:
	/// Voters flock to the opposing canidate's position
	/// one of their voters gain a 200 voter boost in votes
	/// </summary>
	void VoterOutrage(GameObject TargetPlayer){
		bool foundOpposingVoter = false;
		for (int i = 0; i < voters.Length && !foundOpposingVoter; i++) {
			if(voterVars[i].CanidateChoice != null && voterVars[i].CanidateChoice != TargetPlayer){
				if(voterVars[i].votes == 0){
					voterVars[i].votes = 1;
					foundOpposingVoter = true;
				}
			}
		}
	}
	
	/// <summary>
	/// Flips the flopping.
	/// </summary>
	void FlipFlopping(GameObject TargetPlayer){
		bool failToMove = false;
		int magnitude;
		//Vector3 newLocation;
		float direction;
		
		for (int i = 0; i < voters.Length; i++) {
			if(voterVars[i].CanidateChoice == TargetPlayer){
				//newLocation = voters[i].transform.position;
				direction = Random.value;
				
				//choose a direction
				if(direction < 0.3333f){
					//move along x axis
					if(voters[i].transform.position.x - TargetPlayer.transform.position.x < 0){
						magnitude = -1;
					}
					else{
						magnitude = 1;
					}
					failToMove = false;
					Vector3 temporaryPosition = voters[i].transform.position;
					if((magnitude > 0 && temporaryPosition.x < gridSize - magnitude)||(magnitude < 0 && temporaryPosition.x > -1 - magnitude)){
						temporaryPosition.x = temporaryPosition.x + magnitude;
						for(int j = 0; j < voters.Length && !failToMove; j++){
							if(voters[j].transform.position == temporaryPosition){
								failToMove = true;
							}
						}
					}
					
					//check to see if voter resistance prevents event from moving voter
					if(magnitude < 0 &&  Random.value < voterVars[i].xMinusResistance + voterVars[i].baseResistance){
						failToMove = true;
					}
					else if(magnitude > 0 &&  Random.value < voterVars[i].xPlusResistance + voterVars[i].baseResistance){
						failToMove = true;
					}
					
					if(!failToMove)
					{
						voters[i].transform.position = temporaryPosition;
					}
				}
				else if(direction < 0.6666f){
					//move voter in y direction
					if(voters[i].transform.position.y - TargetPlayer.transform.position.y < 0){
						magnitude = -1;
					}
					else{
						magnitude = 1;
					}
					failToMove = false;
					Vector3 temporaryPosition = voters[i].transform.position;
					if((magnitude > 0 && temporaryPosition.y < gridSize - magnitude)||(magnitude < 0 && temporaryPosition.y > -1 - magnitude)){
						temporaryPosition.y = temporaryPosition.y + magnitude;
						for(int j = 0; j < voters.Length && !failToMove; j++){
							if(voters[j].transform.position == temporaryPosition){
								failToMove = true;
							}
						}
					}
					
					//check to see if voter resistance prevents event from moving voter
					if(magnitude < 0 &&  Random.value < voterVars[i].yMinusResistance + voterVars[i].baseResistance){
						failToMove = true;
					}
					else if(magnitude > 0 &&  Random.value < voterVars[i].yPlusResistance + voterVars[i].baseResistance){
						failToMove = true;
					}
					
					if(!failToMove)
					{
						voters[i].transform.position = temporaryPosition;
					}
				}
				else{
					//move voter in z direction
					if(voters[i].transform.position.z - TargetPlayer.transform.position.z < 0){
						magnitude = -1;
					}
					else{
						magnitude = 1;
					}
					failToMove = false;
					Vector3 temporaryPosition = voters[i].transform.position;
					if((magnitude > 0 && temporaryPosition.z < gridSize - magnitude)||(magnitude < 0 && temporaryPosition.z > -1 - magnitude)){
						temporaryPosition.z = temporaryPosition.z + magnitude;
						for(int j = 0; j < voters.Length && !failToMove; j++){
							if(voters[j].transform.position == temporaryPosition){
								failToMove = true;
							}
						}
					}
					
					//check to see if voter resistance prevents event from moving voter
					if(magnitude < 0 &&  Random.value < voterVars[i].zMinusResistance + voterVars[i].baseResistance){
						failToMove = true;
					}
					else if(magnitude > 0 &&  Random.value < voterVars[i].zPlusResistance + voterVars[i].baseResistance){
						failToMove = true;
					}
					
					if(!failToMove)
					{
						voters[i].transform.position = temporaryPosition;
					}
				}
			}
		}
	}
	
	/// <summary>
	/// Voters the manipulation.
	/// </summary>
	void VoterManipulation(GameObject TargetPlayer){
		//charged for a fine
		TargetPlayer.GetComponent<PlayerVariables> ().money -= 300;
	}
	
	/// <summary>
	/// Contradictories the positions.
	/// </summary>
	void ContradictoryPositions(GameObject TargetPlayer){
		// pop a shadow position
		if(TargetPlayer.GetComponent<PlayerVariables> ().shadowPositions.Count > 0){
			TargetPlayer.GetComponent<PlayerVariables> ().shadowPositions.RemoveAt(0);
		}
		// also political scandal
	}
	
	/// <summary>
	/// Ads the burnout.
	/// </summary>
	void AdBurnout(GameObject TargetPlayer){
		// people get tired of ads and become harder to move
		for (int i = 0; i < voters.Length; i++) {
			voterVars[i].baseResistance += 0.2f;
		}
	}
	
	/// <summary>
	/// Overreachings the campeign.
	/// </summary>
	void OverreachingCampaign(GameObject TargetPlayer){
		//Sphere of influence shrinks?
	}
	
	
}//RandomEventController script
