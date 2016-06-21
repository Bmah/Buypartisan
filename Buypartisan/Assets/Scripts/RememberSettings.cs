//Alex Jungroth
using UnityEngine;
using System.Collections;

public class RememberSettings : MonoBehaviour 
{

	//holds a potential copy of this script if the prototypescene is called again during run time
	private GameObject[] duplicateRememberSettings;

	//holds the settings from the Game Controller that will be saved to the title scene
	public int gridSize = 7;
	public int totalRounds = 5;
	public int totalElections = 2;
	public int totalVoters = 40;
	public float musicVolume = 0.1f;
	public float sFXVolume = 0.1f;
    public bool uniqueParties = true;
    public bool complexElections = true;

    // Use this for initialization
    void Start () 
	{
		//potetnially finds duplicate RememberSettings if there are any
		duplicateRememberSettings = GameObject.FindGameObjectsWithTag("RememberSettings");

		//makes sure that there is only one copy of the RememberSettings
		if (duplicateRememberSettings.Length == 1) 
		{
			//preserves the original copy of RememberSettings so the Game Controller can see its variables
			DontDestroyOnLoad(duplicateRememberSettings[0]);
		}
		else
		{
			//deletes copies of the RememberSettings Script
			Destroy(duplicateRememberSettings[0]);
			DontDestroyOnLoad(duplicateRememberSettings[1]);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	/// <summary>
	/// Saves the settings.
	/// </summary>
	public void SaveSettings(float size, float rounds, float elections, float voters, float music, float sFX, bool unique, bool complex)
	{
		//gets the values that will be sent to the Game Controller
		gridSize = (int)size;
		totalRounds = (int)rounds;
		totalElections = (int)elections;
		totalVoters = (int)voters;
		musicVolume = music;
		sFXVolume = sFX;
        uniqueParties = unique;
        complexElections = complex;
	}
}