using UnityEngine;
using System.Collections;

public class TitleScreenUIScript : MonoBehaviour {

	//holds the title screen buttons (Alex Jungroth)
	public GameObject backButton;
	public GameObject resetButton;
	public GameObject playButton;
	public GameObject settingsButton;
	public GameObject quitButton;

	//holds the text for the grid, rounds, and election settings (Alex Jungroth)
	public GameObject gridText;
	public GameObject roundsText;
	public GameObject electionText;

	//holds the toggle group for grid size (Alex Jungroth)
	public GameObject[] toggleGridSize;

	//holds the toggle group for rounds (Alex Jungroth)
	public GameObject[] toggleRounds;

	//holds the toggle group for elections (Alex Jungroth)
	public GameObject[] toggleElection;

	// Use this for initialization
	void Start () {

		//gets the toggles for altering the grid size (Alex Jungroth)
		toggleGridSize = GameObject.FindGameObjectsWithTag("Grid");

		//gets the toggles for altering the number of rounds (Alex Jungroth)
		toggleRounds = GameObject.FindGameObjectsWithTag("Rounds");

		//gets the toggles for altering the number of elections (Alex Jungroth)
		toggleElection = GameObject.FindGameObjectsWithTag ("Election");

		//disables the back button and the reset button (Alex Jungroth)
		backButton.SetActive (false);
		resetButton.SetActive (false);

		//disables the toggles for grid size, rounds, and election settings (Alex Jungroth)
		for (int i = 0; i < 8; i++) 
		{
			toggleGridSize[i].SetActive(false);
			toggleRounds[i].SetActive(false);
		}

		for (int i = 0; i < 10; i++)
		{
			toggleElection [i].SetActive (false);
		}
		
		//disables the text for the grid size, rounds, and election settings (Alex Jungroth)
		gridText.SetActive(false);
		roundsText.SetActive(false);
		electionText.SetActive(false);
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PlayGame(){
		Application.LoadLevel("PrototypeScene");
	}

	/// <summary>
	/// This works with the settings button
	/// Enables all of the toggles, buttons, and text for the settings menu
	/// Disables itself, the play buttton, and the quit button (Alex Jungroth)
	/// </summary>
	public void SettingsMenu()
	{
		//enables the back button and the reset button (Alex Jungroth)
		backButton.SetActive (true);
		resetButton.SetActive (true);
		
		//enables  the toggles for grid size, rounds, and election settings (Alex Jungroth)
		for (int i = 0; i < 8; i++) {
			toggleGridSize [i].SetActive (true);
			toggleRounds [i].SetActive (true);
		}

		for (int i = 0; i < 10; i++)
		{
			toggleElection [i].SetActive (true);
		}

		//enables  the text for the grid size, rounds, and election settings (Alex Jungroth)
		gridText.SetActive (true);
		roundsText.SetActive(true);
		electionText.SetActive (true);

		//disables the play button, settings button, and quit button (Alex Jungroth)
		playButton.SetActive (false);
		settingsButton.SetActive (false);
		quitButton.SetActive (false);
	}

	/// <summary>
	/// This works with the back button
	/// Disables all of the toggles, buttons, and text for the settings menu
	/// Enables itself, the play buttton, and the quit button (Alex Jungroth)
	/// </summary>
	public void ExitSettings()
	{
		//disables the back button and the reset button (Alex Jungroth)
		backButton.SetActive (false);
		resetButton.SetActive (false);
		
		//disables  the toggles for grid size, rounds, and election settings (Alex Jungroth)
		for (int i = 0; i < 8; i++) 
		{
			toggleGridSize[i].SetActive(false);
			toggleRounds[i].SetActive(false);
		}

		for (int i = 0; i < 10; i++)
		{
			toggleElection [i].SetActive (false);
		}
		
		//disables the text for the grid size, rounds, and election settings (Alex Jungroth)
		gridText.SetActive (false);
		roundsText.SetActive(false);
		electionText.SetActive (false);
		
		//enables the play button, settings button, and quit button (Alex Jungroth)
		playButton.SetActive (true);
		settingsButton.SetActive (true);
		quitButton.SetActive (true);
	}

	/// <summary>
	/// Resets the settings. (Alex Jungroth)
	/// </summary>
	public void resetSettings()
	{
		//toggleGridSize[4] = true;

		//toggleRounds[2] = true;

		//toggleElection[1] = true;
	}

	public void QuitGame(){
		Application.Quit ();
	}
}
