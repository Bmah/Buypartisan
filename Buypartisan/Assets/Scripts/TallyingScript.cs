//Alex Jungroth
using UnityEngine;
using System.Collections;

public class TallyingScript : MonoBehaviour {

	//holds the game controller 
	public GameObject gameController;

	//holds the players
	private GameObject[] players;

	//holds the voters
	private GameObject[] voters;

	//holds the distance between a given player or shadow position and a given voter
	private Vector3 distanceVector;
	
	//holds the absolute value of the distance vector
	private float distance;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	/// <summary>
	/// Tallying script. Tallies each players votes and money at the start of each turn.
	/// This includes the votes and money earned by a player's shadow positions.
	/// </summary>
	public void preTurnTalling()
	{
/*		//gets the players
		players = gameController.GetComponent<GameController> ().players;
		
		//gets the voters
		voters = gameController.GetComponent<GameController> ().voters;
		
		for (int i = 0; i < voters.Length; i++) 
		{
			float leastDistance = 1000f;
			int closestPlayer = 0;
			float tieDistance = 1000f;
			int tiePlayer = 0;
			
			//calculates the distance of voters from players
			for (int j = 0; j < players.Length; j++)
			{
				distanceVector = players [j].GetComponent<PlayerVariables>().transform.position - voters [i].GetComponent<VoterVariables>().transform.position;
				distance = Mathf.Abs (distanceVector.x) + Mathf.Abs (distanceVector.y) + Mathf.Abs (distanceVector.z);
				
				//determines if there is a player that beat the last one
				if (distance < leastDistance) 
				{
					leastDistance = distance;
					closestPlayer = j;
				} 
				else if (distance == leastDistance) 
				{
					//creates a tie between two players (3 way ties can suck it)
					tieDistance = distance;
					tiePlayer = j;
				}
				
				for (int k = 0; k < players[j].GetComponent<PlayerVariables>().shadowPositions.Count; k++)
				{
					distanceVector = players [j].GetComponent<PlayerVariables>().shadowPositions[k].GetComponent<PlayerVariables>().transform.position - 
						voters [i].GetComponent<VoterVariables>().transform.position;
					distance = Mathf.Abs (distanceVector.x) + Mathf.Abs (distanceVector.y) + Mathf.Abs (distanceVector.z);
					
					//determines if there is a player that beat the last one
					if (distance < leastDistance) 
					{
						leastDistance = distance;
						closestPlayer = j;
					} 
					else if (distance == leastDistance) 
					{
						//creates a tie between two players (3 way ties can suck it)
						tieDistance = distance;
						tiePlayer = j;
					}
				}
			}
			//checks if least distance is still tied with the tie player, if not, it is shorter, so don't split
			if (tieDistance == leastDistance) 
			{
				Debug.Log ("Checking if least distance is still tied with the tied player...if not, it's shorter so don't split votes");
				players [closestPlayer].GetComponent<PlayerVariables> ().votes += voters [i].GetComponent<VoterVariables> ().votes / 2;
				players [tiePlayer].GetComponent<PlayerVariables> ().votes += voters [i].GetComponent<VoterVariables> ().votes / 2;
				players [closestPlayer].GetComponent<PlayerVariables> ().money += voters [i].GetComponent<VoterVariables> ().money / 2;
				players [tiePlayer].GetComponent<PlayerVariables> ().money += voters [i].GetComponent<VoterVariables> ().money / 2;
			} 
			else 
			{
				//do normal assignments if least distance is not tied
				players [closestPlayer].GetComponent<PlayerVariables> ().votes += voters [i].GetComponent<VoterVariables> ().votes;
				players [closestPlayer].GetComponent<PlayerVariables> ().money += voters [i].GetComponent<VoterVariables> ().money;
			}
		}
*/	}
}