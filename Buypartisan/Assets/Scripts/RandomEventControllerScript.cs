using UnityEngine;
using System.Collections;

public class RandomEventControllerScript : MonoBehaviour {

	public GameObject[] voters;
	public GameObject[] players;

	// Use this for initialization
	void Start () {

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
			break;
		case 1:
			ShiftVoters('X',-1);
			break;
		case 2:
			ShiftVoters('Y',1);
			break;
		case 3:
			ShiftVoters('Y',-1);
			break;
		case 4:
			ShiftVoters('Z',1);
			break;
		case 5:
			ShiftVoters('Z',-1);
			break;
		case 6:
			EconomicBoom(2);
			break;
		case 7:
			EconomicBust(2);
			break;
		default:
			//flavor text goes here
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
				temporaryPosition.x = temporaryPosition.x + magnitude;
				voters[i].transform.position = temporaryPosition;
			}
			break;
		case 'Y':
			for(int i = 0; i < voters.Length; i++){
				Vector3 temporaryPosition = voters[i].transform.position;
				temporaryPosition.y = temporaryPosition.y + magnitude;
				voters[i].transform.position = temporaryPosition;
			}
			break;
		case 'Z':
			for(int i = 0; i < voters.Length; i++){
				Vector3 temporaryPosition = voters[i].transform.position;
				temporaryPosition.z = temporaryPosition.z + magnitude;
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
