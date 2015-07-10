using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TitleScreenUIScript : MonoBehaviour 
{
	//holds the script that the gameController will take the setting values from (Alex Jungroth)
	public TitleScreenSettings gameSettings;

	//holds the music controller (Alex Jungroth)
	public MusicController musicPlayer;

	//holds the SFX controller (Alex Jungroth)
	public SFXController sFXPlayer;

	//holds the title screen buttons (Alex Jungroth)
	public GameObject backButton;
	public GameObject resetButton;
	public GameObject playButton;
	public GameObject settingsButton;
	public GameObject quitButton;

	//holds the button that is acting as a background for the text (Alex Jungroth)
	public GameObject voterCounterButton;

	//holds the text of a button thats being used as text box for its aesthetic qualities (Alex Jungroth)
	public Text voterCounterButtonText;

	//holds the text the title screens text elements (Alex Jungroth)
	public GameObject gridText;
	public GameObject roundsText;
	public GameObject electionText;
	public GameObject voterCounterText;
	public GameObject sFXText;
	public GameObject musicText;

	//holds the	parents of the toggles (Alex Jungroth)
	public GameObject parentGridSize;
	public GameObject parentRounds;
	public GameObject parentElection;

	//holds the toggle group for grid size (Alex Jungroth)
	private Toggle[] toggleGridSize;

	//holds the toggle group for rounds (Alex Jungroth)
	private Toggle[] toggleRounds;

	//holds the toggle group for elections (Alex Jungroth)
	private Toggle[] toggleElection;

	//holds the title screen sliders (Alex Jungroth)
	public GameObject voterCounterSlider;
	public GameObject sFXSlider;
	public GameObject musicSlider;

	//holds the values that the gameController will get in the next scene (Alex Jungroth)
	public int gridSize = 7;
	public int totalRounds = 5;
	public int totalElections = 2;
	public float totalVoters = 40;
	public float musicVolume = 0.5f;
	public float sFXVolume = 0.5f;

	// Use this for initialization
	void Start () {

		//gets the toggles for altering the grid size (Alex Jungroth)
		toggleGridSize = parentGridSize.GetComponentsInChildren<Toggle>();

		//gets the toggles for altering the number of rounds (Alex Jungroth)
		toggleRounds = parentRounds.GetComponentsInChildren<Toggle>();

		//gets the toggles for altering the number of elections (Alex Jungroth)
		toggleElection = parentElection.GetComponentsInChildren<Toggle>();

		//disables the back button and the reset button (Alex Jungroth)
		backButton.SetActive(false);
		resetButton.SetActive(false);

		//disables the voter counter (Alex Jungroth)
		voterCounterButton.SetActive(false);

		//disables the toggles for grid size, rounds, and election settings (Alex Jungroth)
		parentGridSize.SetActive(false);
		parentRounds.SetActive(false);
		parentElection.SetActive(false);

		//disables the text for the title screen (Alex Jungroth)
		gridText.SetActive(false);
		roundsText.SetActive(false);
		electionText.SetActive(false);
		voterCounterText.SetActive(false);
		sFXText.SetActive(false);
		musicText.SetActive(false);

		//disables the sliders for the title screen (Alex Jungroth)
		voterCounterSlider.SetActive(false);
		sFXSlider.SetActive(false);
		musicSlider.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () 
	{
		//sets the grid size and number of rounds based on the toggle the player checked (Alex Jungroth)
		for (int i = 0; i < 8; i++) 
		{
			if(toggleGridSize[i].isOn == true)
			{
				gridSize = i + 3;

				//adjusts the max value of the slider so that user cannot have a 3X3X3 grid with more than 27 voters (Alex Jungroth)
				if(i == 0)
				{
					if(voterCounterSlider.GetComponent<Slider>().value > 27)
					{
						//prevents the settings from defaulting to 27 if the user picks a 3X3X3 grid and a voter total greater than 27 (Alex Jungroth)
						voterCounterSlider.GetComponent<Slider>().value = 12;
					}

					voterCounterSlider.GetComponent<Slider>().maxValue = 27;
				}
				else
				{
					voterCounterSlider.GetComponent<Slider>().maxValue = 50;
				}
			}

			if(toggleRounds[i].isOn == true)
			{
				totalRounds = i + 3;
			}
		}
		
		//sets the number of elections based on the toggle the player checked (Alex Jungroth)
		for (int i = 0; i < 10; i++) 
		{
			if(toggleElection[i].isOn == true)
			{
				totalElections = i + 1;
			}
		}

		//adjusts the number of voters as the user moves the handle on the slide bar (Alex Jungroth)
		voterCounterButtonText.text = voterCounterSlider.GetComponent<Slider>().value.ToString();
		totalVoters = voterCounterSlider.GetComponent<Slider>().value;

		//adjusts the music volume as the user moves the handle on the slide bar (Alex Jungroth)
		musicPlayer.audioChannels [0].volume = musicSlider.GetComponent<Slider>().value;
		musicVolume = musicSlider.GetComponent<Slider>().value;

		//adjusts the SFX volume as the user moves the handle on the slide bar (Alex Jungroth)
		sFXPlayer.AudioChannels [0].volume = sFXSlider.GetComponent<Slider>().value;
		sFXVolume = sFXSlider.GetComponent<Slider>().value;
	}

	public void PlayGame()
	{
		//sends the decided game settings to the object that the gameController will see (Alex Jungroth)
		gameSettings.FinalizeSettings (gridSize, totalRounds,totalElections, totalVoters, musicVolume, sFXVolume);

		Application.LoadLevel("PrototypeScene");
	}

	/// <summary>
	/// This works with the settings button
	/// Enables all of the toggles, buttons, and text for the settings menu
	/// Disables itself, the play buttton, and the quit button (Alex Jungroth)
	/// </summary>
	public void SettingsMenu()
	{
		//disables the play, settings, and quit button (Alex Jungroth)
		playButton.SetActive(false);
		settingsButton.SetActive(false);
		quitButton.SetActive(false);

		//enables the back button and the reset button (Alex Jungroth)
		backButton.SetActive(true);
		resetButton.SetActive(true);

		//enables the voter counter (Alex Jungroth)
		voterCounterButton.SetActive(true);

		//enables  the toggles for grid size, rounds, and election settings (Alex Jungroth)
		parentGridSize.SetActive(true);
		parentRounds.SetActive(true);
		parentElection.SetActive(true);

		//enables the text for the title screen (Alex Jungroth)
		gridText.SetActive(true);
		roundsText.SetActive(true);
		electionText.SetActive(true);
		voterCounterText.SetActive(true);
		sFXText.SetActive(true);
		musicText.SetActive(true);
		
		//enables the sliders for the title screen (Alex Jungroth)
		voterCounterSlider.SetActive(true);
		sFXSlider.SetActive(true);
		musicSlider.SetActive(true);
	}

	/// <summary>
	/// This works with the back button
	/// Disables all of the toggles, buttons, and text for the settings menu
	/// Enables itself, the play buttton, and the quit button (Alex Jungroth)
	/// </summary>
	public void ExitSettings()
	{
		//enables the play, settings, and quit button (Alex Jungroth)
		playButton.SetActive(true);
		settingsButton.SetActive(true);
		quitButton.SetActive(true);

		//disables the back button and the reset button (Alex Jungroth)
		backButton.SetActive(false);
		resetButton.SetActive(false);

		//disables the voter counter (Alex Jungroth)
		voterCounterButton.SetActive(false);

		//disables  the toggles for grid size, rounds, and election settings (Alex Jungroth)
		parentGridSize.SetActive(false);
		parentRounds.SetActive(false);
		parentElection.SetActive(false);
		
		//disables the text for the title screen (Alex Jungroth)
		gridText.SetActive(false);
		roundsText.SetActive(false);
		electionText.SetActive(false);
		voterCounterText.SetActive(false);
		sFXText.SetActive(false);
		musicText.SetActive(false);
		
		//disables the sliders for the title screen (Alex Jungroth)
		voterCounterSlider.SetActive(false);
		sFXSlider.SetActive(false);
		musicSlider.SetActive(false);
	}

	/// <summary>
	/// Resets the settings. (Alex Jungroth)
	/// </summary>
	public void resetSettings()
	{
		//resets the toggles to the default settings (Alex Jungroth)
		toggleGridSize[4].isOn = true;
		toggleRounds[2].isOn = true;
		toggleElection[1].isOn = true;

		//resets the number of voters to the default settings (Alex Jungroth)
		voterCounterButtonText.text = "40";

		//resets the sliders (Alex Jungroth)
		voterCounterSlider.GetComponent<Slider>().maxValue = 50;
		voterCounterSlider.GetComponent<Slider>().value = 40;
		sFXSlider.GetComponent<Slider>().value = 0.5f;
		musicSlider.GetComponent<Slider>().value = 0.5f;
	}

	public void QuitGame(){
		Application.Quit ();
	}
}