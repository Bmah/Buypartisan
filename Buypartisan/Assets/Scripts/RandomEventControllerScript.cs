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
	private int numberOfActions = 8;
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

	public enum ActionState {StartEvents, WaitForTriggeredEvent0, TriggeredEvent0, WaitForTriggeredEvent1,
		TriggeredEvent1, WaitForTriggeredEvent2, TriggeredEvent2, WaitForTriggeredEvent3, TriggeredEvent3};
	ActionState currentState = ActionState.StartEvents;

	InputManagerScript Inputs = null;

	// Use this for initialization
	void Start () {
		numberOfActions = actionThreshold.Length;

		if (UIController == null) {
			Debug.LogError("UI_Script not set on RandomEventController");
		}
		GameObject temp = GameObject.FindGameObjectWithTag ("GameController");
		if (temp != null) {
			actionCounter = new int[temp.GetComponent<GameController> ().numberPlayers][];
			eventTriggerList = new bool[actionCounter.Length][];
		}
		else {
			Debug.LogError("Could not fing Gamecontroller");
		}

		//initializing the event trigger list

		for(int i = 0; i < eventTriggerList.Length; i++){
			eventTriggerList[i] = new bool[numberOfActions];
			for(int j = 0; j < eventTriggerList[0].Length; j++){
				eventTriggerList[i][j] = false;
			}
		}

		for (int i = 0; i < actionCounter.Length; i++){
			actionCounter[i] = new int[numberOfActions];
		}

		Inputs = GameObject.FindGameObjectWithTag ("InputManager").GetComponent<InputManagerScript> ();
		if (Inputs == null) {
			Debug.LogError("Could not Find Input Manager");
		}
	}
	
	// Update is called once per frame
	void Update () {
		debugArray = actionCounter [arrayChoice];
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
	/// only call this once
	/// Brian Mah
	/// </summary>
	public bool ActivateEvents(){

		switch (currentState){
		case ActionState.StartEvents :
			currentState = ActionState.WaitForTriggeredEvent0;
			StandardEvents ();
			break;
		case ActionState.WaitForTriggeredEvent0 :
			if(Inputs.leftClickDown){

			}
			break;

		}

		CheckForTriggeredEvents();


		return true;
	}

	void StandardEvents ()
	{
		int eventChoice = Random.Range (0, 10);
		switch (eventChoice) {
		case 0:
			ShiftVoters ('X', 1);
			UIController.alterTextBox ("Newsflash! Sudden victory in the war boosts confidence in big govt! " + "Voters migrate 1 up on the X axis.");
			break;
		case 1:
			ShiftVoters ('X', -1);
			UIController.alterTextBox ("Newsflash! Sudden defeat in the war crushes confidence in big govt! " + "Voters migrate 1 down on the X axis.");
			break;
		case 2:
			ShiftVoters ('Y', 1);
			UIController.alterTextBox ("Newsflash! New developments in the field of oil drilling lead to profit for big buisness. " + "Voters migrate 1 up on the Y axis.");
			break;
		case 3:
			ShiftVoters ('Y', -1);
			UIController.alterTextBox ("Newsflash! Sudden oil spill causes huge natural disaster, public outraged with big buisness." + "Voters migrate 1 down on the Y axis.");
			break;
		case 4:
			ShiftVoters ('Z', 1);
			UIController.alterTextBox ("Newsflash! Popular celebrity endorses the Z axis. " + "Voters migrate 1 up on the Z axis.");
			break;
		case 5:
			ShiftVoters ('Z', -1);
			UIController.alterTextBox ("Newsflash! Popular celebrity denounces the Z axis. " + "Voters migrate 1 down on the Z axis.");
			break;
		case 6:
			EconomicBoom (2);
			UIController.alterTextBox ("Newsflash! MONEY MONEY EVERYWHERE. " + "Voters now have twice the money they used to!");
			break;
		case 7:
			EconomicBust (2);
			UIController.alterTextBox ("Newsflash! Poor investments in tulip market lead to market crash. " + "Voters now have half the money they used to!");
			break;
		default:
			UIController.alterTextBox ("Newsflash! Little Timmy fell down the well!");
			break;
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

			if(eventTriggerList[i][0]){
				eventTriggerList[i][0] = false;
				//VoterSupression
				VoterOutrage(players[i]);
				UIController.alterTextBox("Triggered Event\nNewsflash! Voters outraged at supression by player "+ (i+1) +
				                          " Voters gather at the polls to vote against them!");
			}
			if(eventTriggerList[i][1]){
				eventTriggerList[i][1] = false;
				//MoveParty
				FlipFlopping(players[i]);
				UIController.alterTextBox("Triggered Event\nNewsflash! Voters irrited by player "+ (i+1) + "'s flip flopping, " +
				                          "voters distance themselves from the candidate!");
			}
			if(eventTriggerList[i][2]){
				eventTriggerList[i][2] = false;
				//InfluenceVoters
				VoterManipulation(players[i]);
				UIController.alterTextBox("Triggered Event\nNewsflash! Voters shocked at player "+ (i+1) + "'s manipulation of votes " +
				                          "player "+ (i+1) + " fined for their crime");
			}
			if(eventTriggerList[i][3]){
				eventTriggerList[i][3] = false;
				//ShadowPosition
				ContradictoryPositions (players[i]);
				UIController.alterTextBox("Triggered Event\nNewsflash! Player "+ (i+1) + " called out on contradictory positions " +
				                          "player"+ (i+1) + "'s shadow position is removed");
			}
			if(eventTriggerList[i][4]){
				eventTriggerList[i][4] = false;
				//CampaignTour
				AdBurnout(players[i]);
				UIController.alterTextBox("Triggered Event\nNewsflash! Voters tired of Player "+ (i+1) + "'s ads" +
				                          " voters now harder to move!");
				//smaller size sphere
			}
			if(eventTriggerList[i][5]){
				eventTriggerList[i][5] = false;
				//SphereOfInfluence
				OverreachingCampaign(players[i]);
				UIController.alterTextBox("Triggered Event\nNewsflash! Player "+ (i+1) + " tries to expand their campeign's reach too far " +
				                          " no consequences for this action as of yet");
			}
		}//for each player
	}//Check For Triggered events

	/// <summary>
	/// Voter Outrage:
	/// Voters flock to the opposing canidate's position
	/// one of their voters gain a 200 voter boost in votes
	/// </summary>
	void VoterOutrage(GameObject TargetPlayer){
		bool foundOpposingVoter = false;
		for (int i = 0; i < voters.Length && !foundOpposingVoter; i++) {
			if(voterVars[i].CanidateChoice != null && voterVars[i].CanidateChoice != TargetPlayer){
				voterVars[i].votes += 200;
				foundOpposingVoter = true;
			}
		}
	}

	/// <summary>
	/// Flips the flopping.
	/// </summary>
	void FlipFlopping(GameObject TargetPlayer){
		bool failToMove = false;
		int magnitude;
		Vector3 newLocation;
		float direction;

		for (int i = 0; i < voters.Length; i++) {
			if(voterVars[i].CanidateChoice == TargetPlayer){
				newLocation = voters[i].transform.position;
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
