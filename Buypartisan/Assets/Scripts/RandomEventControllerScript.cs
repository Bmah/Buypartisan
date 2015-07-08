using UnityEngine;
using System.Collections;

/// <summary>
/// Random event controller script.
/// Brian Mah
/// </summary>
public class RandomEventControllerScript : MonoBehaviour {

	public GameObject[] voters;
	public VoterVariables[] voterVars = null;
	private bool voterVarsSet = false;

	public UI_Script UIController;

	public int gridSize;

	// Use this for initialization
	void Start () {
		if (UIController == null) {
			Debug.LogError("UI_Script not set on RandomEventController");
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
	/// only call this once
	/// Brian Mah
	/// </summary>
	public void ActivateEvents(){
		int eventChoice = Random.Range (0, 10);
		switch (eventChoice) {
		case 0:
			ShiftVoters('X',1);
			UIController.alterTextBox("Newsflash! Sudden victory in the war boosts confidence in big govt! " +
				"Voters migrate 1 up on the X axis.");
			break;
		case 1:
			ShiftVoters('X',-1);
			UIController.alterTextBox("Newsflash! Sudden defeat in the war crushes confidence in big govt! " +
			                          "Voters migrate 1 down on the X axis.");
			break;
		case 2:
			ShiftVoters('Y',1);
			UIController.alterTextBox("Newsflash! New developments in the field of oil drilling lead to profit for big buisness. " +
			                          "Voters migrate 1 up on the Y axis.");
			break;
		case 3:
			ShiftVoters('Y',-1);
			UIController.alterTextBox("Newsflash! Sudden oil spill causes huge natural disaster, public outraged with big buisness." +
			                          "Voters migrate 1 down on the Y axis.");
			break;
		case 4:
			ShiftVoters('Z',1);
			UIController.alterTextBox("Newsflash! Popular celebrity endorses the Z axis. " +
			                          "Voters migrate 1 up on the Z axis.");
			break;
		case 5:
			ShiftVoters('Z',-1);
			UIController.alterTextBox("Newsflash! Popular celebrity denounces the Z axis. " +
			                          "Voters migrate 1 down on the Z axis.");
			break;
		case 6:
			EconomicBoom(2);
			UIController.alterTextBox("Newsflash! MONEY MONEY EVERYWHERE. " +
			                          "Voters now have twice the money they used to!");
			break;
		case 7:
			EconomicBust(2);
			UIController.alterTextBox("Newsflash! Poor investments in tulip market lead to market crash. " +
			                          "Voters now have half the money they used to!");
			break;
		default:
			UIController.alterTextBox("Newsflash! Little Timmy fell down the well!");
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


}
