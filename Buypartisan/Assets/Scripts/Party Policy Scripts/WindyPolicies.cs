//Alex Jungroth
using UnityEngine;
using System.Collections;

public class WindyPolicies : MonoBehaviour {

	//holds the Game Controller
	public GameController gameController;

	//holds the Random Event Controller
	public RandomEventControllerScript randomEventController;

	//holds a constant value for random checks
	private const float half = 0.5f;

	//holds constants to modifiy the partys' vote totals
	private const float fifteenPercentDecrease = 0.85f;

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
	/// Redirects the policy requests for the Windy Party. (Alex Jungroth)
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

	//reduce the revenue of your opponents by giving everyone tax breaks
	void xAxisPolicy()
	{
		for (int i = 0; i < gameController.numberPlayers; i++) 
		{
			if(i != gameController.electionWinner)
			{
				gameController.players[i].GetComponent<PlayerVariables>().money = (int) Mathf.Ceil
					(gameController.players[i].GetComponent<PlayerVariables>().money * fifteenPercentDecrease);
			}
		}
	}

	//has a 50-50 chance to start you in the next election with 15% more votes or 5% less votes
	void yAxisPolicy()
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

	//blows up a dam and has a 50-50 chance to add towards triggering mass extinction
	void zAxisPolicy()
	{
		if (Random.value >= half) 
		{
			randomEventController.naturalDisaster = true;
		}
	}
}