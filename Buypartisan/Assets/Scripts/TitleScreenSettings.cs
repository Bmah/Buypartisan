//Alex Jungroth
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TitleScreenSettings : MonoBehaviour 
{
	//holds a potential copy of this script if the title screen scene is called again during run time
	private GameObject[] duplicatTitleSettingsUI;

	//holds the values that the gameController will get in the next scene
	public int gridSize = 7;
	public int totalRounds = 5;
	public int totalElections = 2;
	public int totalVoters = 40;
	public float musicVolume = 0.5f;
	public float sFXVolume = 0.5f;

	// Use this for initialization
	void Start () 
	{
		//potetnially finds duplicate TitleSceenUIScripts if there are any
		duplicatTitleSettingsUI = GameObject.FindGameObjectsWithTag("TitleSettings");
		
		//makes sure that there is only one copy of the TitleScreenUIScript
		if (duplicatTitleSettingsUI.Length == 1) 
		{
			//preserves the original copy of TitleScreenSettingsScript so the gameController can see its variables
			DontDestroyOnLoad(duplicatTitleSettingsUI[0]);
		}
		else
		{
			//deletes copies of the TitleScreenSettings Script
			Destroy(duplicatTitleSettingsUI[0]);
			DontDestroyOnLoad(duplicatTitleSettingsUI[1]);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	/// <summary>
	/// Finalizes the settings. (Alex Jungroth)
	/// </summary>
	public void FinalizeSettings(int size, int rounds, int elections, float voters, float music, float sFX)
	{
		//gets the values that will be sent to the gameController
		gridSize = size;
		totalRounds = rounds;
		totalElections = elections;
		totalVoters = (int)voters; //I think an int will be expected/voter markers can't be halved, cut in fourths, etc.
		musicVolume = music;
		sFXVolume = sFX;
	}
}