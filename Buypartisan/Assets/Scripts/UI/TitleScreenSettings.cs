﻿//Alex Jungroth
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TitleScreenSettings : MonoBehaviour 
{
	//holds a potential copy of this script if the title screen scene is called again during run time
	private GameObject[] duplicateTitleSettingsUI;
    
	//holds the values that the gameController will get in the next scene
	public int gridSize = 7;
	public int totalRounds = 5;
	public int totalElections = 2;
	public int totalVoters = 40;
	public float musicVolume = 0.1f;
    public float sFXVolume = 0.1f;

    //Holds whether or not there will be unique parties
    public bool uniqueParties = false;

    //Holds whether or not window generator will be used
    public bool complexElections;

    //Holds whether or not the pedestals will be used 
    public bool usePedestals;

    //Holds whether or not the tutorial will be used 
    public bool useTutorial;

    // Use this for initialization
    void Start () 
	{
		//potetnially finds duplicate TitleSceenUIScripts if there are any
		duplicateTitleSettingsUI = GameObject.FindGameObjectsWithTag("TitleSettings");
		
		//makes sure that there is only one copy of the TitleScreenUIScript
		if (duplicateTitleSettingsUI.Length == 1) 
		{
			//preserves the original copy of TitleScreenSettingsScript so the gameController can see its variables
			DontDestroyOnLoad(duplicateTitleSettingsUI[0]);
		}
		else
		{
            //deletes copies of the TitleScreenSettings Script
            Destroy(duplicateTitleSettingsUI[1]);
            DontDestroyOnLoad(duplicateTitleSettingsUI[0]);
		}
	}

	/// <summary>
	/// Finalizes the settings. (Alex Jungroth)
	/// </summary>
	public void FinalizeSettings(float size, float rounds, float elections, float voters, float music, float sFX, bool unique, bool complex, bool pedestals, bool tutorial)
	{
		//gets the values that will be sent to the gameController
		gridSize = (int)size;
		totalRounds = (int)rounds;
		totalElections = (int)elections;
		totalVoters = (int)voters;
		musicVolume = music;
		sFXVolume = sFX;
        uniqueParties = false;
        complexElections = complex;
        usePedestals = pedestals;
        useTutorial = tutorial;
    }
}