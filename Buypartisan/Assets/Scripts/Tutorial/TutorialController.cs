//Alex Jungroth
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class TutorialController : MonoBehaviour {

    //Holds the number of tutorial sections
    private const int numberOfTutorialSections = 7;

    //Hold the index numbers of different tutorial sections
    [HideInInspector]
    public const int goalSection = 0;
    [HideInInspector]
    public const int sliderSection = 1;
    [HideInInspector]
    public const int voterSection = 2;
    [HideInInspector]
    public const int positioningSection = 3;
    [HideInInspector]
    public const int actionSection = 4;
    [HideInInspector]
    public const int randomEventSection = 5;
    [HideInInspector]
    public const int electionSection = 6;
    private const int numFiles = 7;

    //Holds an array of bools for which tutorial sections have been used during the game
    private bool[] tutorialSectionsUsed;

    //Hold strings for the different sections the of tutorial
    private List<string> goalStrings = new List<string>();
    private List<string> sliderStrings = new List<string>();
    private List<string> voterStrings = new List<string>();
    private List<string> positioningStrings = new List<string>();
    private List<string> actionStrings = new List<string>();
    private List<string> randomEventStrings = new List<string>();
    private List<string> electionStrings = new List<string>();

    //Holds strings for different sections of the tutorial
    private List<string>[] TutorialStrings = new List<string>[numFiles];

    //Holds the strings for the current section of the tutorial
    private List<string> currentStrings = new List<string>();

    //Holds a counter for which string the tutorial is currently on
    private int currentStringCounter = 0;

    //Holds a temporary string that is read in from a text file and stored into one of the list of strings
    private string tempString;

    //Which cover we are on
    private int currentCover;

    //Holds the cover that grays out sections of the screen
    public GameObject[] TutorialCovers;

    //Holds positions for all tutorials
    public Transform[] TutorialTransforms;

    //Holds text files for tutorials
    public TextAsset[] TutorialFiles;

    [Header("Images")]
    //Holds the speech bubble for the tutorial
    public GameObject tutorialSpeechBubble;
    //Variable to hold a picture of a voter for tutorial purposes
    public GameObject voterImage;
    //Holds the buttons for the tutorial 
    [Header("Buttons")]
    public GameObject leftButton;
    public GameObject rightButton;
    public GameObject confirmButton;
    //Holds the movement buttons to disable them when needed
    public GameObject movementKeys;

    //Used to read in the text files
    StreamReader currentReader;

    //Holds whether or not the tutorial controller is currently active
    [HideInInspector]
    public bool isActiveTutorial = false;
    
    //Use this for initialization
    void Start()
    {
        //Initializes the size of the tutorial sections used
        tutorialSectionsUsed = new bool[numberOfTutorialSections];

        //Initializes the tutorial sections that have been used
        for (int i = 0; i < numberOfTutorialSections; i++)
        {
            tutorialSectionsUsed[i] = false;
        }//for

        //Clears the tutorial text box
        tutorialSpeechBubble.gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Text>().text = "";

        //Disables the left button and confirm button
        leftButton.SetActive(false);
        confirmButton.SetActive(false);

        //Parse text
        //This variable are what we will use to cut lines. "\r" is 'carriage return' and "\n" is 'new line'
        var FileSplit = new string[] { "\r\n", "\n", "\r" };
        for (int curFile = 0; curFile < numFiles; curFile++)
        {
            //Initializes the current TutorialString element for the current file.
            TutorialStrings[curFile] = new List<string>();
            //TutLines is an automatic variable that will end up being an array of lines. Each line correspondes to one line in the file
            //TutorialFiles is a TextAsset that has a .text element which can then use .Split to split up the file into individual lines
            var TutLines = TutorialFiles[curFile].text.Split(FileSplit, System.StringSplitOptions.None);
            for (int i = 0; i < TutLines.Length; i++)
            {
                TutorialStrings[curFile].Add(TutLines[i]);
                Debug.Log(TutorialStrings[curFile][i]);
            }
        }
       
    }//Start

    //Update is called once per frame
    void Update ()
    {
        //Disables the left button if the tutorial is displaying the first part of a section
        if(currentStringCounter <= 0)
        {
            leftButton.SetActive(false);
        }//if
        else
        {
            leftButton.SetActive(true);
        }//else

        //Disables the right button if the tutorial is displaying the last part of a section
        if(currentStringCounter >= currentStrings.Count - 1)
        {
            rightButton.SetActive(false);

            //Enables the confirm button when the end of a tutorial section is reached
            confirmButton.SetActive(true);
        }//if
        else
        {
            rightButton.SetActive(true);
        }//else
    }//Update

    /// <summary>
    /// Exits the current section of the tutorial (Alex Jungroth)
    /// </summary>
    public void tutorialExit()
    {
        //Clears the tutorial text box
        tutorialSpeechBubble.gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Text>().text = "";

                //Disables the left button and confirm button
        leftButton.SetActive(false);
        confirmButton.SetActive(false);

        //Disables the tutorial speech bubble and cover
        tutorialSpeechBubble.SetActive(false);
        //Disable the dark tutorial cover
        TutorialCovers[currentCover].SetActive(false);
        //Overkill; deactivates the voter image, but will only actually do anything after the voter tutorial
        voterImage.SetActive(false);
        //Overkill; reenables the movement buttons, only actually happening after the actions tutorial
        movementKeys.SetActive(true);

        //Resets the current string counter
        currentStringCounter = 0;

        //The tutorial is no longer active
        isActiveTutorial = false;
    }//tutorialExit

    /// <summary>
    /// Explains the goal of the game to the players (Alex Jungroth)
    /// </summary>
    //Function to load the needed tutorial.
    public void loadTutorial(int currentTutorial)
    {
        //Prevents this function from being called more than once a game
        if (tutorialSectionsUsed[currentTutorial] == false)
        {
            //Sets the current section of the tutorial to true so it won't be called again
            tutorialSectionsUsed[currentTutorial] = true;

            //Special section checks
            if (currentTutorial == actionSection)
            {
                //Disable the movement buttons so they are not in the way
                movementKeys.SetActive(false);
            }
            if(currentTutorial == voterSection)
            {
                //Makes the image of the voter active so we can see what they look like
                voterImage.SetActive(true);
            }

            //Moves the speech bubble to the correct location
            tutorialSpeechBubble.transform.position = TutorialTransforms[currentTutorial].position;
            //Displays the tutorial speech bubble
            tutorialSpeechBubble.SetActive(true);
            //Activates the correct dark screen mask
            TutorialCovers[currentTutorial].SetActive(true);
            //Sets the currentCover
            currentCover = currentTutorial;

            //Updates the current strings to this sections text
            currentStrings = TutorialStrings[currentTutorial];

            //Sets the tutorial speech bubble to the first line of text
            tutorialSpeechBubble.gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Text>().text = currentStrings[0];

            //The tutorial is currently active
            isActiveTutorial = true;
        }//if
    }//loadTutorial

    /// <summary>
    /// Cycles backwards through the tutorial text (Alex Jungroth)
    /// </summary>
    public void backButton()
    {
        //Decrements the counter if it is greater than zero
        if(currentStringCounter > 0)
        {
            currentStringCounter--;

            //Displays the new line of text
            tutorialSpeechBubble.gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Text>().text = currentStrings[currentStringCounter];
        }//if
    }//backButton

    /// <summary>
    /// Cycles forward throught the tutorial text (Alex JUngroth)
    public void forwardButton()
    {
        //Increments the counter if it is less than the length of the current string list
        if (currentStringCounter < currentStrings.Count - 1)
        {
            currentStringCounter++;

            //Displays the new line of text
            tutorialSpeechBubble.gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Text>().text = currentStrings[currentStringCounter];
        }//if
    }//forwardButton
}


