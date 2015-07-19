﻿//Alex Jungroth
using UnityEngine;
using System.Collections;

public class TallyingScript : MonoBehaviour {

	//holds the game controller 
	public GameObject gameController;

	//holds the players
	private GameObject[] players;

	//holds the voters
	private GameObject[] voters;

	//holds the number of players
	private int numberPlayers;

	//holds the distance between a given player or shadow position and a given voter
	private Vector3 distanceVector;
	
	//holds the absolute value of the distance vector
	private float distance;

	//holds wether or not the voter is within a given player's sphere of influence
	private bool influenced = true;

	//holds wether or not the voter is within a tying player's sphere of influence
	private bool tieInfluenced = true;

	//holds the size of the player's sphere of influence
	private int sphereSize;

	//holds the size of the player's sphere of influence
	private int shadowSphereSize;

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
		//gets the players
		players = gameController.GetComponent<GameController> ().players;
		
		//gets the voters
		voters = gameController.GetComponent<GameController> ().voters;

		numberPlayers = gameController.GetComponent<GameController> ().numberPlayers;

		//resets the players votes so they can be properly be counted 
		for (int i = 0; i < numberPlayers; i++) 
		{
			players[i].GetComponent<PlayerVariables>().votes = 0;

		}
		
		for (int i = 0; i < numberPlayers; i++) 
		{
			float leastDistance = 1000f;
			int closestPlayer = 0;
			float tieDistance = 1000f;
			int tiePlayer = 0;
			
			//calculates the distance of voters from players
			for (int j = 0; j < numberPlayers; j++)
			{

				//gets the player's sphere of influence size
				sphereSize = players[j].GetComponent<PlayerVariables>().sphereSize;

				distanceVector = players [j].GetComponent<PlayerVariables>().transform.position - voters [i].GetComponent<VoterVariables>().transform.position;
				distance = Mathf.Abs (distanceVector.magnitude);

				//determines if there is a player that beat the last one
				if (distance < leastDistance) 
				{
					leastDistance = distance;
					closestPlayer = j;

					if(sphereSize / 20 >= distance)
					{
						influenced = true;
					}
					else
					{
						influenced = false;
					}
				} 
				else if (distance == leastDistance) 
				{
					//creates a tie between two players (3 way ties can suck it)
					tieDistance = distance;
					tiePlayer = j;

					if(sphereSize / 20 >= distance)
					{
						tieInfluenced = true;
					}
					else
					{
						tieInfluenced = false;
					}
				}
				
				for (int k = 0; k < players[j].GetComponent<PlayerVariables>().shadowPositions.Count; k++)
				{

					//gets the player's shadow postion sphere of influence
					shadowSphereSize = players[j].GetComponent<PlayerVariables>().shadowPositions[k].GetComponent<PlayerVariables>().sphereSize;

					distanceVector = players [j].GetComponent<PlayerVariables>().shadowPositions[k].GetComponent<PlayerVariables>().transform.position - 
						voters [i].GetComponent<VoterVariables>().transform.position;
					distance = Mathf.Abs (distanceVector.magnitude);
					
					//determines if there is a player that beat the last one
					if (distance < leastDistance) 
					{
						leastDistance = distance;
						closestPlayer = j;

						if(shadowSphereSize / 20 >= distance)
						{
							influenced = true;
						}
						else
						{
							influenced = false;
						}
					} 
					else if (distance == leastDistance) 
					{
						//creates a tie between two players (3 way ties can suck it)
						tieDistance = distance;
						tiePlayer = j;

						if(shadowSphereSize / 20 >= distance)
						{
							tieInfluenced = true;
						}
						else
						{
							tieInfluenced = false;
						}
					}
				}
			}
			//checks if least distance is still tied with the tie player, if not, it is shorter, so don't split
			if (tieDistance == leastDistance) 
			{
				//Debug.Log ("Checking if least distance is still tied with the tied player...if not, it's shorter so don't split votes");

				if(influenced && tieInfluenced)
				{
					//the players tied and the spheres of influence overlapped the voter
					players [closestPlayer].GetComponent<PlayerVariables> ().votes += voters [i].GetComponent<VoterVariables> ().votes / 2;
					players [tiePlayer].GetComponent<PlayerVariables> ().votes += voters [i].GetComponent<VoterVariables> ().votes / 2;
					players [closestPlayer].GetComponent<PlayerVariables> ().money += voters [i].GetComponent<VoterVariables> ().money / 2;
					players [tiePlayer].GetComponent<PlayerVariables> ().money += voters [i].GetComponent<VoterVariables> ().money / 2;
				}
				else if(influenced && !tieInfluenced)
				{
					//only closet player's sphere of influence covered the voter
					players [closestPlayer].GetComponent<PlayerVariables> ().votes += voters [i].GetComponent<VoterVariables> ().votes;
					players [closestPlayer].GetComponent<PlayerVariables> ().money += voters [i].GetComponent<VoterVariables> ().money;
				}
				else if(!influenced && tieInfluenced)
				{
					//only tie player's sphere of the influence covered the voter
					players [tiePlayer].GetComponent<PlayerVariables> ().votes += voters [i].GetComponent<VoterVariables> ().votes;
					players [tiePlayer].GetComponent<PlayerVariables> ().money += voters [i].GetComponent<VoterVariables> ().money;
				}
			} 
			else 
			{
				if(influenced == true)
				{
					//do normal assignments if least distance is not tied
					players [closestPlayer].GetComponent<PlayerVariables> ().votes += voters [i].GetComponent<VoterVariables> ().votes;
					players [closestPlayer].GetComponent<PlayerVariables> ().money += voters [i].GetComponent<VoterVariables> ().money;
				}
			}
		}
	}
}