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
    private const int goalSection = 0;
    private const int sliderSection = 1;
    private const int voterSection = 2;
    private const int positioningSection = 3;
    private const int actionSection = 4;
    private const int randomEventSection = 5;
    private const int electionSection = 6;

    //Holds an array of bools for which tutorial sections have been used during the game
    private bool[] tutorialSectionsUsed;

    //Hold strings for the different sections the of tutorial
    private List<string> goalStrings = new List<string>();
    private List<string> sliderStrings = new List<string>();
    private List<string> voterStrings = new List<string>();
    private List<string> positionStrings = new List<string>();
    private List<string> actionStrings = new List<string>();
    private List<string> randomEventStrings = new List<string>();
    private List<string> electionStrings = new List<string>();

    //Holds the strings for the current section of the tutorial
    private List<string> currentStrings = new List<string>();

    //Holds a counter for which string the tutorial is currently on (AAJ)
    private int currentStringCounter = 0;

    //Holds a temporary string that is read in from a text file and stored into one of the list of strings
    private string tempString;

    //Which cover we are on
    private int currentCover;

    //Holds the speech bubble for the tutorial
    public GameObject tutorialSpeechBubble;
    //Variable to hold a picture of a voter for tutorial purposes
    public GameObject voterImage;
    //Holds the movement buttons to disable them when needed
    public GameObject movementKeys;
    
    //Holds the cover that grays out sections of the screen
    public GameObject[] TutorialCovers;

    //Holds positions for all tutorials
    public Transform[] TutorialTransforms;

    //Used to read in the text files
    StreamReader sampleReader;
    
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

        //Parses the text files to get the strings
        sampleReader = new StreamReader(Application.dataPath + "\\Tutorial Text Files\\Sample.txt", Encoding.Default);

        using(sampleReader)
        {
            //Reads the first line of the file
            tempString = sampleReader.ReadLine();

            //Reads line by line to the end of the text file
            while(tempString != null)
            {
                goalStrings.Add(tempString);
               tempString = sampleReader.ReadLine();
            }//while

            //Closes this sections reader
            sampleReader.Close();
        }//using

        //A test of goal explainer
        goalExplainer();

    }//Start

    //Update is called once per frame
    void Update ()
    {
	
	}//Update

    /// <summary>
    /// Exits the current section of the tutorial (Alex Jungroth)
    /// </summary>
    public void tutorialExit()
    {
        //Clears the tutorial text box
        tutorialSpeechBubble.gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Text>().text = "";

        //Disables the tutorial speech bubble and cover
        tutorialSpeechBubble.SetActive(false);
        TutorialCovers[currentCover].SetActive(false);

        //Resets the current string counter
        currentStringCounter = 0;
    }//tutorialExit

    /// <summary>
    /// Explains the goal of the game to the players (Alex Jungroth)
    /// </summary>
    public void goalExplainer()
    {
        //Prevents this function from being called more than once a game
        if(tutorialSectionsUsed[goalSection] == false)
        {
            //Sets the current section of the tutorial to true so it won't be called again
            tutorialSectionsUsed[goalSection] = true;

            //Moves the speech bubble to the correct location
            tutorialSpeechBubble.transform.position = TutorialTransforms[goalSection].position;
            //Displays the tutorial speech bubble
            tutorialSpeechBubble.SetActive(true);
            //Activates the correct dark screen mask
            TutorialCovers[goalSection].SetActive(true);

            //Updates the current strings to this sections text
            currentStrings = goalStrings;

            //Sets the tutorial speech bubble to the first line of text
            tutorialSpeechBubble.gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Text>().text = goalStrings[0];
        }//if
    }//goalExplainer

    /// <summary>
    /// Explains how to use the sliders on the choose your party screen (Alex Jungroth)
    /// </summary>
    public void sliderExplainer()
    {
        //Prevents this function from being called more than once a game
        if (tutorialSectionsUsed[sliderSection] == false)
        {
            //Sets the current section of the tutorial to true so it won't be called again
            tutorialSectionsUsed[sliderSection] = true;

            //Moves the speech bubble to the correct location
            tutorialSpeechBubble.transform.position = TutorialTransforms[sliderSection].position;
            //Displays the tutorial speech bubble
            tutorialSpeechBubble.SetActive(true);
            //Activates the correct dark screen mask
            TutorialCovers[sliderSection].SetActive(true);


        }//if
    }//sliderExplainer

    /// <summary>
    /// Explains everything about the voters (Alex Jungroth)
    /// </summary>
    public void voterExplainer()
    {
        //Prevents this function from being called more than once a game
        if (tutorialSectionsUsed[voterSection] == false)
        {
            //Sets the current section of the tutorial to true so it won't be called again
            tutorialSectionsUsed[voterSection] = true;

            //Moves the speech bubble to the correct location
            tutorialSpeechBubble.transform.position = TutorialTransforms[voterSection].position;
            //Displays the tutorial speech bubble
            tutorialSpeechBubble.SetActive(true);
            //Activates the correct dark screen mask
            TutorialCovers[voterSection].SetActive(true);
        }//if
    }//voterExplainer

    /// <summary>
    /// Explains initially positioning a party on the grid (Alex Jungroth)
    /// </summary>
    public void positioningExplainer()
    {
        //Prevents this function from being called more than once a game
        if (tutorialSectionsUsed[positioningSection] == false)
        {
            //Sets the current section of the tutorial to true so it won't be called again
            tutorialSectionsUsed[positioningSection] = true;

            //Moves the speech bubble to the correct location
            tutorialSpeechBubble.transform.position = TutorialTransforms[positioningSection].position;
            //Displays the tutorial speech bubble
            tutorialSpeechBubble.SetActive(true);
            //Activates the correct dark screen mask
            TutorialCovers[positioningSection].SetActive(true);
        }//if
    }//positioningExplainer

    /// <summary>
    /// Explains the actions that can be done during a players turn (Alex Jungroth)
    /// </summary>
    public void actionExplainer()
    {
        //Prevents this function from being called more than once a game
        if (tutorialSectionsUsed[actionSection] == false)
        {
            //Sets the current section of the tutorial to true so it won't be called again
            tutorialSectionsUsed[actionSection] = true;

            //Moves the speech bubble to the correct location
            tutorialSpeechBubble.transform.position = TutorialTransforms[actionSection].position;
            //Displays the tutorial speech bubble
            tutorialSpeechBubble.SetActive(true);
            //Activates the correct dark screen mask
            TutorialCovers[actionSection].SetActive(true);
        }//if
    }//actionExplainer

    /// <summary>
    /// Explains the random events that happen at the end of each round (Alex Jungroth)
    /// </summary>
    public void randomEventExplainer()
    {
        //Prevents this function from being called more than once a game
        if (tutorialSectionsUsed[randomEventSection] == false)
        {
            //Sets the current section of the tutorial to true so it won't be called again
            tutorialSectionsUsed[randomEventSection] = true;

            //Moves the speech bubble to the correct location
            tutorialSpeechBubble.transform.position = TutorialTransforms[randomEventSection].position;
            //Displays the tutorial speech bubble
            tutorialSpeechBubble.SetActive(true);
            //Activates the correct dark screen mask
            TutorialCovers[randomEventSection].SetActive(true);
        }//if
    }//randomEventExplainer

    /// <summary>
    /// Explains the election(s) that happen after a set number of rounds (Alex Jungroth)
    /// </summary>
    public void electionExplainer()
    {
        //Prevents this function from being called more than once a game
        if (tutorialSectionsUsed[electionSection] == false)
        {
            //Sets the current section of the tutorial to true so it won't be called again
            tutorialSectionsUsed[electionSection] = true;

            //Moves the speech bubble to the correct location
            tutorialSpeechBubble.transform.position = TutorialTransforms[electionSection].position;
            //Displays the tutorial speech bubble
            tutorialSpeechBubble.SetActive(true);
            //Activates the correct dark screen mask
            TutorialCovers[electionSection].SetActive(true);
        }//if
    }//electionExplainer

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
