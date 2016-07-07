using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class TitleScreenUIScript : MonoBehaviour 
{
	//Constants values that will be used throughout the scripts (Alex Jungroth)
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

	//Holds the script that the gameController will take the setting values from (Alex Jungroth)
	public TitleScreenSettings gameSettings;

	//Holds the settings from the Game Controller (Alex Jungroth)
	//public RememberSettings rememberSettings;

	//Holds the music controller (Alex Jungroth)
	public MusicController musicPlayer;

	//Holds the SFX controller (Alex Jungroth)
	public SFXController sFXPlayer;

	//Holds the title screen images (Alex Jungroth)
	public GameObject panel2;
	public GameObject panel2BG;
	public GameObject settingsText;

	//Holds the title screen buttons (Alex Jungroth)
	public GameObject backButton;
	public GameObject resetButton;
	public GameObject playButton;
	public GameObject settingsButton;
	public GameObject quitButton;
    public GameObject creditsButton;

	//Holds the text the title screens text elements (Alex Jungroth)
	public GameObject gridText;
	public GameObject roundsText;
	public GameObject electionText;
	public GameObject voterCounterText;
	public GameObject sFXText;
	public GameObject musicText;
    
	//Holds the credits background and text
	public GameObject creditsTextBackground;

	//Holds the title screen sliders (Alex Jungroth)
	public GameObject gridSizeSlider;
	public GameObject roundsSlider;
	public GameObject electionSlider;
	public GameObject voterCounterSlider;
	public GameObject sFXSlider;
	public GameObject musicSlider;

    //Holds a reference to the toggle for unique parties (Alex Jungroth)
    public Toggle uniquePartiesToggle;

    //Holds a reference to the toggle for complex elections (Alex Jungroth)
    public Toggle complexElectionsToggle;

    //Holds a reference to the toggle for pedestals (Alex Jungroth)
    public Toggle usePedestalsToggle;

    //Holds the values that the gameController will get in the next scene (Alex Jungroth)
    public float gridSize = gSSlider;
	public float totalRounds = rSlider;
	public float totalElections = eSlider;
	public float totalVoters = vCSValue;
	public float sFXVolume = sSlider;
	public float musicVolume = mSlider;

    private bool inCredits = false;

	//Holds the cover the other buttons when the settings are being adjusted (Alex Jungroth)
	public GameObject settingsCover;

	private bool LoadingGameScene = false;
	private float LoadSceneTime;

	//Use this for initialization
	void Start () 
	{
		//Disables the settings images (Alex Jungroth)
		panel2.SetActive(false);
		panel2BG.SetActive(false);
		settingsText.SetActive(false);

		//Disables the back button and the reset button (Alex Jungroth)
		backButton.SetActive(false);
		resetButton.SetActive(false);

		//Disables the text for the title screen (Alex Jungroth)
		gridText.SetActive(false);
		roundsText.SetActive(false);
		electionText.SetActive(false);
		voterCounterText.SetActive(false);
		sFXText.SetActive(false);
		musicText.SetActive(false);
        creditsTextBackground.SetActive(false);

		//Disables the sliders for the title screen (Alex Jungroth)
		gridSizeSlider.SetActive(false);
		roundsSlider.SetActive(false);
		electionSlider.SetActive(false);
		voterCounterSlider.SetActive(false);
		sFXSlider.SetActive(false);
		musicSlider.SetActive(false);

		//Disable the setting cover at the start (Alex Jungroth)
		settingsCover.SetActive(false);

		//Gets the settings from the remember settings if it exists (Alex Jungroth)
		try 
		{
            gameSettings = GameObject.FindGameObjectWithTag("TitleSettings").GetComponent<TitleScreenSettings>();
		}
		catch
		{
			
		}
		if (gameSettings != null) 
		{
			//Gets the following variables from the title UI settings (Alex Jungroth)
			gridSize = gameSettings.gridSize;
			totalRounds = gameSettings.totalRounds;
			totalElections = gameSettings.totalElections;
			totalVoters = gameSettings.totalVoters;
			musicVolume = gameSettings.musicVolume;
			sFXVolume = gameSettings.sFXVolume;

			gridSizeSlider.GetComponent<Slider>().value = gridSize;
			roundsSlider.GetComponent<Slider>().value = totalRounds;
			electionSlider.GetComponent<Slider>().value = totalElections;
			voterCounterSlider.GetComponent<Slider>().value = totalVoters;
			musicSlider.GetComponent<Slider>().value = musicVolume;
			sFXSlider.GetComponent<Slider>().value = sFXVolume;

            uniquePartiesToggle.GetComponent<Toggle>().isOn = gameSettings.uniqueParties;
            complexElectionsToggle.GetComponent<Toggle>().isOn = gameSettings.complexElections;
            usePedestalsToggle.GetComponent<Toggle>().isOn = gameSettings.usePedestals;
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
			SceneManager.LoadScene("PrototypeScene");
		}
	}

	public void PlayGame()
	{
		//sends the decided game settings to the object that the gameController will see (Alex Jungroth)
		settingsCover.SetActive(true);
		gameSettings.FinalizeSettings (gridSize, totalRounds,totalElections, totalVoters, musicVolume, sFXVolume, uniquePartiesToggle.GetComponent<Toggle>().isOn, complexElectionsToggle.GetComponent<Toggle>().isOn, usePedestalsToggle.GetComponent<Toggle>().isOn);
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

        //Resets the toggles (Alex Jungroth)
        uniquePartiesToggle.isOn = false;
        complexElectionsToggle.isOn = false;
        usePedestalsToggle.isOn = false;
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