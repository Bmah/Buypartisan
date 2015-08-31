using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TitleScreenUIScript : MonoBehaviour 
{
	//constants values that will be used throughout the scripts (Alex Jungroth)
	//vCS = voter Counter Slider
	//vCB = voter Counter Button
	//gS = grid Size
	//r = rounds
	//e = elections
	//s = SFX
	//m = music
	private const float gSSlider = 7;
	private const float gSSliderMinValue = 3;
	private const float rSlider = 5;
	private const float eSlider = 2;
	private const float vCSMaxValue = 50;
	private const float vCSLowerBound = 27;
	private const float vCSReasonableValue = 12;
	private const float vCSValue = 40;
	private const float sSlider = 0.1f;
	private const float mSlider = 0.1f;
	private const int mChannel = 0;
	private const int sChannel = 0;
	private const int mouseDown = 0;

	//holds the script that the gameController will take the setting values from (Alex Jungroth)
	public TitleScreenSettings gameSettings;

	//holds the settings from the Game Controller (Alex Jungroth)
	public RememberSettings rememberSettings;

	//holds the music controller (Alex Jungroth)
	public MusicController musicPlayer;

	//holds the SFX controller (Alex Jungroth)
	public SFXController sFXPlayer;

	//holds the title screen images (Alex Jungroth)
	public GameObject panel2;
	public GameObject panel2BG;
	public GameObject settingsText;

	//holds the title screen buttons (Alex Jungroth)
	public GameObject backButton;
	public GameObject resetButton;
	public GameObject playButton;
	public GameObject settingsButton;
	public GameObject quitButton;
    public GameObject creditsButton;

	//holds the text the title screens text elements (Alex Jungroth)
	public GameObject gridText;
	public GameObject roundsText;
	public GameObject electionText;
	public GameObject voterCounterText;
	public GameObject sFXText;
	public GameObject musicText;
    
	//holds the credits background and text
	public GameObject creditsTextBackground;

	//holds the title screen sliders (Alex Jungroth)
	public GameObject gridSizeSlider;
	public GameObject roundsSlider;
	public GameObject electionSlider;
	public GameObject voterCounterSlider;
	public GameObject sFXSlider;
	public GameObject musicSlider;

	//holds the values that the gameController will get in the next scene (Alex Jungroth)
	public float gridSize = gSSlider;
	public float totalRounds = rSlider;
	public float totalElections = eSlider;
	public float totalVoters = vCSValue;
	public float sFXVolume = sSlider;
	public float musicVolume = mSlider;

    private bool inCredits = false;

	//holds the cover the other buttons when the settings are being adjusted (Alex Jungroth)
	public GameObject settingsCover;

	private bool LoadingGameScene = false;
	private float LoadSceneTime;

	// Use this for initialization
	void Start () 
	{
		//disables the settings images (Alex Jungroth)
		panel2.SetActive(false);
		panel2BG.SetActive(false);
		settingsText.SetActive(false);

		//disables the back button and the reset button (Alex Jungroth)
		backButton.SetActive(false);
		resetButton.SetActive(false);

		//disables the text for the title screen (Alex Jungroth)
		gridText.SetActive(false);
		roundsText.SetActive(false);
		electionText.SetActive(false);
		voterCounterText.SetActive(false);
		sFXText.SetActive(false);
		musicText.SetActive(false);
        creditsTextBackground.SetActive(false);

		//disables the sliders for the title screen (Alex Jungroth)
		gridSizeSlider.SetActive(false);
		roundsSlider.SetActive(false);
		electionSlider.SetActive(false);
		voterCounterSlider.SetActive(false);
		sFXSlider.SetActive(false);
		musicSlider.SetActive(false);

		//disable the setting cover at the start (Alex Jungroth)
		settingsCover.SetActive(false);

		//gets the settings from the remember settings if it exists (Alex Jungroth)
		try 
		{
			rememberSettings = GameObject.FindGameObjectWithTag("RememberSettings").GetComponent<RememberSettings>();
		}
		catch
		{
			
		}
		if (rememberSettings != null) 
		{
			//gets the following variables from the title UI settings (Alex Jungroth)
			gridSize = rememberSettings.gridSize;
			totalRounds = rememberSettings.totalRounds;
			totalElections = rememberSettings.totalElections;
			totalVoters = rememberSettings.totalVoters;
			musicVolume = rememberSettings.musicVolume;
			sFXVolume = rememberSettings.sFXVolume;

			gridSizeSlider.GetComponent<Slider>().value = gridSize;
			roundsSlider.GetComponent<Slider>().value = totalRounds;
			electionSlider.GetComponent<Slider>().value = totalElections;
			voterCounterSlider.GetComponent<Slider>().value = totalVoters;
			musicSlider.GetComponent<Slider>().value = musicVolume;
			sFXSlider.GetComponent<Slider>().value = sFXVolume;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		//updates the grid size, total rounds, and total elections and their corresponding text elements (Alex Jungroth)
		gridSize = gridSizeSlider.GetComponent<Slider>().value;
		gridText.GetComponent<Text>().text = gridSizeSlider.GetComponent<Slider>().value.ToString();

		totalRounds = roundsSlider.GetComponent<Slider>().value;
		roundsText.GetComponent<Text>().text = roundsSlider.GetComponent<Slider>().value.ToString();

		totalElections = electionSlider.GetComponent<Slider>().value;
		electionText.GetComponent<Text> ().text = electionSlider.GetComponent<Slider>().value.ToString();

		//adjusts the max value of the slider so that user cannot have, for example, a 3X3X3 grid with more than 27 voters (Alex Jungroth)
		if(gridSize == gSSliderMinValue)
		{
			if(voterCounterSlider.GetComponent<Slider>().value > vCSLowerBound)
			{
				//prevents the settings from defaulting to 27 if the user picks a 3X3X3 grid and a voter total greater than 27 (Alex Jungroth)
				voterCounterSlider.GetComponent<Slider>().value = vCSReasonableValue;
			}

			//sets the max value on the voter counter slider to the restricted value based on the number of avaible grid spaces (Alex Jungroth)
			voterCounterSlider.GetComponent<Slider>().maxValue = vCSLowerBound;
		}
		else
		{
			//resets the max value on the voter counter slider to vCSMaxValue (Alex Jungroth)
			voterCounterSlider.GetComponent<Slider>().maxValue = vCSMaxValue;
		}
		
		//adjusts the number of voters as the user moves the handle on the slide bar (Alex Jungroth)
		voterCounterText.GetComponent<Text>().text = voterCounterSlider.GetComponent<Slider>().value.ToString();
		totalVoters = voterCounterSlider.GetComponent<Slider>().value;

		//Brian Mah
		//If loading do the fade
		if (!LoadingGameScene) {
			//adjusts the music volume as the user moves the handle on the slide bar (Alex Jungroth)
			musicPlayer.audioChannels [mChannel].volume = musicSlider.GetComponent<Slider> ().value;
			musicVolume = musicSlider.GetComponent<Slider> ().value;
			musicText.GetComponent<Text> ().text = musicSlider.GetComponent<Slider> ().value.ToString ();
		}

		//adjusts the SFX volume as the user moves the handle on the slide bar (Alex Jungroth)
		sFXPlayer.AudioChannels [sChannel].volume = sFXSlider.GetComponent<Slider>().value;
		sFXVolume = sFXSlider.GetComponent<Slider>().value;
		sFXText.GetComponent<Text>().text = sFXSlider.GetComponent<Slider> ().value.ToString();

        if (inCredits && Input.GetMouseButtonDown(mouseDown))
        {
            toggleCredits(false);
        }

		if (LoadingGameScene && Time.time > LoadSceneTime) {
			Application.LoadLevel("PrototypeScene");
		}
	}

	public void PlayGame()
	{
		//sends the decided game settings to the object that the gameController will see (Alex Jungroth)
		settingsCover.SetActive(true);
		gameSettings.FinalizeSettings (gridSize, totalRounds,totalElections, totalVoters, musicVolume, sFXVolume);
		//Brian Mah
		LoadSceneTime = Time.time + 0.5f;
		LoadingGameScene = true;
	}

	/// <summary>
	/// This works with the settings button
	/// Enables all of sliders, buttons, and text for the settings menu
	/// Disables the buttons (Alex Jungroth)
	/// </summary>
	public void SettingsMenu()
	{
		//enables the settings images (Alex Jungroth)
		panel2.SetActive(true);
		panel2BG.SetActive(true);
		settingsText.SetActive(true);

		//enables the settings cover (Alex Jungroth)
		settingsCover.SetActive(true);

		//enables the back button and the reset button (Alex Jungroth)
		backButton.SetActive(true);
		resetButton.SetActive(true);
		
		//enables the text for the title screen (Alex Jungroth)
		gridText.SetActive(true);
		roundsText.SetActive(true);
		electionText.SetActive(true);
		voterCounterText.SetActive(true);
		sFXText.SetActive(true);
		musicText.SetActive(true);
		
		//enables the sliders for the title screen (Alex Jungroth)
		gridSizeSlider.SetActive(true);
		roundsSlider.SetActive(true);
		electionSlider.SetActive(true);
		voterCounterSlider.SetActive(true);
		sFXSlider.SetActive(true);
		musicSlider.SetActive(true);
	}

	/// <summary>
	/// This works with the back button
	/// Disables all of the sliders, buttons, and text for the settings menu
	/// Enables the buttons (Alex Jungroth)
	/// </summary>
	public void ExitSettings()
	{
		//disables the settings images (Alex Jungroth)
		panel2.SetActive(false);
		panel2BG.SetActive(false);
		settingsText.SetActive(false);

		//disables the settings cover (Alex Jungroth)
		settingsCover.SetActive(false);

		//disables the back button and the reset button (Alex Jungroth)
		backButton.SetActive(false);
		resetButton.SetActive(false);

		//disables the text for the title screen (Alex Jungroth)
		gridText.SetActive(false);
		roundsText.SetActive(false);
		electionText.SetActive(false);
		voterCounterText.SetActive(false);
		sFXText.SetActive(false);
		musicText.SetActive(false);
		
		//disables the sliders for the title screen (Alex Jungroth)
		gridSizeSlider.SetActive(false);
		roundsSlider.SetActive(false);
		electionSlider.SetActive(false);
		voterCounterSlider.SetActive(false);
		sFXSlider.SetActive(false);
		musicSlider.SetActive(false);
	}

	/// <summary>
	/// Resets the settings. (Alex Jungroth)
	/// </summary>
	public void resetSettings()
	{
		//resets the number the text to the default settings (Alex Jungroth)
		gridText.GetComponent<Text>().text = gSSlider.ToString();
		
		roundsText.GetComponent<Text>().text = rSlider.ToString();
		
		electionText.GetComponent<Text>().text = eSlider.ToString();

		voterCounterText.GetComponent<Text>().text = vCSValue.ToString();

		sFXText.GetComponent<Text>().text = sSlider.ToString();

		musicText.GetComponent<Text>().text = mSlider.ToString();

		//resets the sliders (Alex Jungroth)
		gridSizeSlider.GetComponent<Slider>().value = gSSlider;
		roundsSlider.GetComponent<Slider>().value = rSlider;
		electionSlider.GetComponent<Slider>().value = eSlider;
		voterCounterSlider.GetComponent<Slider>().maxValue = vCSMaxValue;
		voterCounterSlider.GetComponent<Slider>().value = vCSValue;
		sFXSlider.GetComponent<Slider>().value = sSlider;
		musicSlider.GetComponent<Slider>().value = mSlider;
	}

	public void QuitGame()
	{
		Application.Quit ();
	}

	/// <summary>
	/// Toggles the credits. Michael Lee
	/// </summary>
	/// <param name="buttonClick">If set to <c>true</c> button click.</param>
    public void toggleCredits(bool buttonClick)
    {
        // If button click, show credits
        if (buttonClick)
        {
            // Hides main menu buttons
            inCredits = true;
            playButton.SetActive(false);
            settingsButton.SetActive(false);
            quitButton.SetActive(false);
            creditsButton.SetActive(false);
            // Show credits
            creditsTextBackground.SetActive(true);
        }
        // If mouse left click, then go back to main menu
        else
        {
            // Show main menu
            inCredits = false;
            playButton.SetActive(true);
            settingsButton.SetActive(true);
            quitButton.SetActive(true);
            creditsButton.SetActive(true);
            // Hide credits
            creditsTextBackground.SetActive(false);
        }
    }

	/// <summary>
	/// Plays the mouse over sound. (Alex Jungroth)
	/// </summary>
	public void sFXPlayerMouseOver()
	{
		sFXPlayer.GetComponent<SFXController>().PlayAudioClip(0,0,sFXVolume);
	}

	/// <summary>
	/// Plays the on click sound. (Alex Jungroth)
	/// </summary>
	public void sFXPlayerOnClick()
	{
		sFXPlayer.GetComponent<SFXController>().PlayAudioClip(1,0,sFXVolume);
	}
}