//Alex Jungroth
//Party 5 aka Anit party aka Enlightened party aka Providence Party
//we really need to decide a name
using UnityEngine;
using System.Collections;

public class Party5Policies : MonoBehaviour {

	//holds the Game Controller
	public GameController gameController;

	//holds some constant values
	private const int even = 2;
	private const int odd = 1;
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
	/// Redirects the policy requests for Party 5. (Alex Jungroth)
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

	//Pushes voters to the opposite ends of the X-axis
	void xAxisPolicy()
	{
		//cycles through the voters and sees which end of the axis the voter is on
		for(int i = 0; i < gameController.NumVoters; i++) 
		{
			//sets the temporary variable equal to the voter's current position
			temp = gameController.voters[i].transform.position;
			
			if(gameController.gridSize%even == odd)
			{
				//if the grid size is odd do these checks
				if(gameController.voters[i].transform.position.x < (gameController.gridSize/even) - half)
				{
					temp -= new Vector3(1,0,0);
					
					if((temp.x > 0) && (overlapCheck(temp)))
					{
						gameController.voters[i].transform.position = temp;
					}
				}
				else if(gameController.voters[i].transform.position.x > (gameController.gridSize/even))
				{
					temp += new Vector3(1,0,0);
					
					if((temp.x < gameController.gridSize - 1) && (overlapCheck(temp)))
					{
						gameController.voters[i].transform.position = temp;
					}
				}
			}
			else
			{
				//the grid size is even do these checks
				if(gameController.voters[i].transform.position.x < (gameController.gridSize/even) - odd)
				{
					temp -= new Vector3(1,0,0);
					
					if((temp.x > 0) && (overlapCheck(temp)))
					{
						gameController.voters[i].transform.position = temp;
					}
				}
				else if(gameController.voters[i].transform.position.x > (gameController.gridSize/even))
				{
					temp += new Vector3(1,0,0);
					
					if((temp.x < gameController.gridSize - 1) && (overlapCheck(temp)))
					{
						gameController.voters[i].transform.position = temp;
					}//if
				}//else
			}//else
		}//for
	}//xAxisPolicy

	//Pushes voters to the opposite ends of the Y-axis
	void yAxisPolicy()
	{
		//cycles through the voters and sees which end of the axis the voter is on
		for(int i = 0; i < gameController.NumVoters; i++) 
		{
			//sets the temporary variable equal to the voter's current position
			temp = gameController.voters[i].transform.position;
			
			if(gameController.gridSize%even == odd)
			{
				//if the grid size is odd do these checks
				if(gameController.voters[i].transform.position.y < (gameController.gridSize/even) - half)
				{
					temp -= new Vector3(0,1,0);
					
					if((temp.y > 0) && (overlapCheck(temp)))
					{
						gameController.voters[i].transform.position = temp;
					}
				}
				else if(gameController.voters[i].transform.position.y > (gameController.gridSize/even))
				{
					temp += new Vector3(0,1,0);
					
					if((temp.y < gameController.gridSize - 1) && (overlapCheck(temp)))
					{
						gameController.voters[i].transform.position = temp;
					}
				}
			}
			else
			{
				//the grid size is even do these checks
				if(gameController.voters[i].transform.position.y < (gameController.gridSize/even) - odd)
				{
					temp -= new Vector3(0,1,0);
					
					if((temp.y > 0) && (overlapCheck(temp)))
					{
						gameController.voters[i].transform.position = temp;
					}
				}
				else if(gameController.voters[i].transform.position.y > (gameController.gridSize/even))
				{
					temp += new Vector3(0,1,0);
					
					if((temp.y < gameController.gridSize - 1) && (overlapCheck(temp)))
					{
						gameController.voters[i].transform.position = temp;
					}//if
				}//else
			}//else
		}//for
	}//yAxisPolicy

	//Pushes voters to the opposite ends of the Z-axis
	void zAxisPolicy()
	{
		//cycles through the voters and sees which end of the axis the voter is on
		for(int i = 0; i < gameController.NumVoters; i++) 
		{
			//sets the temporary variable equal to the voter's current position
			temp = gameController.voters[i].transform.position;
			
			if(gameController.gridSize%even == odd)
			{
				//if the grid size is odd do these checks
				if(gameController.voters[i].transform.position.z < (gameController.gridSize/even) - half)
				{
					temp -= new Vector3(0,0,1);
					
					if((temp.z > 0) && (overlapCheck(temp)))
					{
						gameController.voters[i].transform.position = temp;
					}
				}
				else if(gameController.voters[i].transform.position.z > (gameController.gridSize/even))
				{
					temp += new Vector3(0,0,1);
					
					if((temp.z < gameController.gridSize - 1) && (overlapCheck(temp)))
					{
						gameController.voters[i].transform.position = temp;
					}
				}
			}
			else
			{
				//the grid size is even do these checks
				if(gameController.voters[i].transform.position.z < (gameController.gridSize/even) - odd)
				{
					temp -= new Vector3(0,0,1);
					
					if((temp.z > 0) && (overlapCheck(temp)))
					{
						gameController.voters[i].transform.position = temp;
					}
				}
				else if(gameController.voters[i].transform.position.z > (gameController.gridSize/even))
				{
					temp += new Vector3(0,0,1);
					
					if((temp.z < gameController.gridSize - 1) && (overlapCheck(temp)))
					{
						gameController.voters[i].transform.position = temp;
					}//if
				}//else
			}//else
		}//for
	}//zAxisPolicy

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