using UnityEngine;
using System.Collections;

public class RandomEventControllerScript : MonoBehaviour {

	public GameObject[] voters;
	public GameObject[] players;

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
	
	}

	/// <summary>
	/// Activates the events.
	/// only call this once
	/// </summary>
	public void ActivateEvents(){
		int eventChoice = Random.Range (0, 20);
		switch (eventChoice) {
		case 0:
			ShiftVoters('X',1);
			UIController.alterTextBox("Newsflash! Sudden victory in the war boosts confidence in big govt!\n" +
				"Voters migrate 1 up on the X axis.");
			break;
		case 1:
			ShiftVoters('X',-1);
			UIController.alterTextBox("Newsflash! Sudden defeat in the war crushes confidence in big govt!\n" +
			                          "Voters migrate 1 down on the X axis.");
			break;
		case 2:
			ShiftVoters('Y',1);
			UIController.alterTextBox("Newsflash! New developments in the field of oil drilling lead to profit for big buisness\n" +
			                          "Voters migrate 1 up on the Y axis.");
			break;
		case 3:
			ShiftVoters('Y',-1);
			UIController.alterTextBox("Newsflash! Sudden oil spill causes huge natural disaster, public outraged with big buisness\n" +
			                          "Voters migrate 1 down on the Y axis.");
			break;
		case 4:
			ShiftVoters('Z',1);
			UIController.alterTextBox("Newsflash! Popular celebrity endorses the Z axis\n" +
			                          "Voters migrate 1 up on the Z axis.");
			break;
		case 5:
			ShiftVoters('Z',-1);
			UIController.alterTextBox("Newsflash! Popular celebrity denounces the Z axis\n" +
			                          "Voters migrate 1 down on the Z axis.");
			break;
		case 6:
			EconomicBoom(2);
			UIController.alterTextBox("Newsflash! MONEY MONEY EVERYWHERE\n" +
			                          "Voters now have twice the money they used to!");
			break;
		case 7:
			EconomicBust(2);
			UIController.alterTextBox("Newsflash! Poor investments in tulip market lead to market crash\n" +
			                          "Voters now have half the money they used to!");
			break;
		default:
			UIController.alterTextBox("Newsflash! Little Timmy fell down the well!");
			break;
		}
	}

	/// <summary>
	/// Shifts all voters by 1 distance in the specified direction.
	/// </summary>
	/// <value>The shift voters.</value>
	void ShiftVoters(char direction, int magnitude){
		direction = char.ToUpper (direction);
		switch (direction) {
		case 'X':
			for(int i = 0; i < voters.Length; i++){
				Vector3 temporaryPosition = voters[i].transform.position;
				if((magnitude > 0 && temporaryPosition.x >= gridSize - 1 )||(magnitude < 0 && temporaryPosition.x <= 0)){
					temporaryPosition.x = temporaryPosition.x + magnitude;
				}
				voters[i].transform.position = temporaryPosition;
			}
			break;
		case 'Y':
			for(int i = 0; i < voters.Length; i++){
				Vector3 temporaryPosition = voters[i].transform.position;
				if((magnitude > 0 && temporaryPosition.y >= gridSize - 1 )||(magnitude < 0 && temporaryPosition.y <= 0)){
					temporaryPosition.y = temporaryPosition.y + magnitude;
				}
				voters[i].transform.position = temporaryPosition;
			}
			break;
		case 'Z':
			for(int i = 0; i < voters.Length; i++){
				Vector3 temporaryPosition = voters[i].transform.position;
				if((magnitude > 0 && temporaryPosition.z >= gridSize - 1 )||(magnitude < 0 && temporaryPosition.z <= 0)){
					temporaryPosition.z = temporaryPosition.z + magnitude;
				}
				voters[i].transform.position = temporaryPosition;
			}
			break;
		default:
			Debug.LogError("Non XYZ axis specified in ShiftVoters");
			break;
		}
	}

	/// <summary>
	/// Multiplies every voter's money by the boom amount.
	/// </summary>
	/// <param name="multiplier">Multiplier.</param>
	void EconomicBoom(int multiplier){
		for(int i = 0; i < voters.Length; i++){
			voters[i].GetComponent<VoterVariables>().money *= multiplier;
		}
	}

	/// <summary>
	/// Divides every voter's money by the bust amount.
	/// </summary>
	/// <param name="divisor">Divisor.</param>
	void EconomicBust(int divisor){
		for(int i = 0; i < voters.Length; i++){
			voters[i].GetComponent<VoterVariables>().money /= divisor;
		}
	}


}
