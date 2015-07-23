//Alex Jungroth
using UnityEngine;
using System.Collections;

public class DronePolicies : MonoBehaviour {

	//holds the Game Controller
	public GameController gameController;

	//holds the Random Event Controller
	public RandomEventControllerScript randomEventController;

	//holds a constant value for random checks
	private const float half = 0.5f;

	//holds a constant value for an monetary increase in millions of dollars
	private const int billion = 1000;

	//holds constants to modifiy the partys' vote totals
	private const float fifteenPercentIncrease = 1.15f;
	private const float fivePercentDecrease = 0.95f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/// <summary>
	/// Redirects the policy requests for the Drone Party. (Alex Jungroth)
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

	//50-50 chance to get a billion dollars
	void xAxisPolicy()
	{
		if (Random.value >= half) 
		{
			gameController.players[gameController.electionWinner].GetComponent<PlayerVariables>().money += billion;
		}
	}

	//starts a war and has a 50-50 chance to add towards triggering mass extinction
	void yAxisPolicy()
	{
		if (Random.value >= half) 
		{
			randomEventController.lossInWar = true;
		}
	}

	//has a 50-50 chance to start you in the next election with 15% more votes or 5% less votes
	void zAxisPolicy()
	{
		if (Random.value >= half) 
		{
			gameController.players[gameController.electionWinner].GetComponent<PlayerVariables>().votes = (int) Mathf.Ceil
				(gameController.players[gameController.electionWinner].GetComponent<PlayerVariables>().votes * fifteenPercentIncrease);
		} 
		else
		{
			gameController.players[gameController.electionWinner].GetComponent<PlayerVariables>().votes = (int) Mathf.Floor
				(gameController.players[gameController.electionWinner].GetComponent<PlayerVariables>().votes * fivePercentDecrease);
		}
	}
}