//Alex Jungroth
//Party 5 aka Anit party aka Enlightened party aka Providence Party
//we really need to decide a name
using UnityEngine;
using System.Collections;

public class Party5Policies : MonoBehaviour {

	//holds the Game Controller
	public GameController gameController;

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
	
	void xAxisPolicy()
	{
		
		
	}
	
	void yAxisPolicy()
	{
		
		
	}
	
	void zAxisPolicy()
	{
		
		
	}
}