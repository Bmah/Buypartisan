//Alex Jungroth
using UnityEngine;
using System.Collections;

public class EspressoPolicies : MonoBehaviour {

	//holds the Game Controller
	public GameController gameController;

	//holds the Random Event Controller
	public RandomEventControllerScript randomEventController;

	//holds a constant value for random checks
	private const float half = 0.5f;

	//holds a potential position for a voter to move to
	private Vector3 temp = Vector3.zero;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/// <summary>
	/// Redirects the policy requests for the Espresso Party. (Alex Jungroth)
	/// </summary>
	/// <param name="policyNumber">Policy number.</param>
	public void redirectPolicyRequest(int policyNumber)
	{
		switch (policyNumber) 
		{
			case 1:
				xAxisPolicy();
			break;
			
			case 2:
				yAxisPolicy();
			break;
			
			case 3:
				zAxisPolicy();
			break;
		}
	}

	//starts a tea tax and has a 50-50 chance to add towards triggering mass extinction
	void xAxisPolicy()
	{
		if (Random.value >= half) 
		{
			randomEventController.marketCrash = true;
		}
	}

	//spilled coffee on a foreign leader's shoes, everyone moves one down on the y-axis 
	void yAxisPolicy()
	{
		//cycles through the voters and sees which end of the axis the voter is on
		for(int i = 0; i < gameController.NumVoters; i++) 
		{
			//sets the temporary variable equal to the voter's current position
			temp = gameController.voters[i].transform.position;

			temp -= new Vector3(0,1,0);
					
			if((temp.y > 0) && (overlapCheck(temp)))
			{
				gameController.voters[i].transform.position = temp;
			}
		}
	}

	//get a ten perecent refund on all actions
	void zAxisPolicy()
	{
		gameController.players [gameController.electionWinner].GetComponent<PlayerVariables> ().actionCostModifier += 0.1f;
	}
	
	/// <summary>
	/// Checks to make sure none of the voters overlap each other on an axis(Alex Jungroth)
	/// </summary>
	bool overlapCheck(Vector3 temp)
	{
		for(int i = 0; i < gameController.NumVoters; i++)
		{
			if(temp == gameController.voters[i].transform.position)
			{
				return false;
			}
		}
		
		return true;
	}
}