//Alex Jungroth
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    private List<string> goalStrings;
    private List<string> sliderStrings;
    private List<string> voterStrings;
    private List<string> actionStrings;
    private List<string> randomEventStrings;
    private List<string> electionStrings;

    //Holds a temporary string that is read in from a text file and stored into one of the list of strings
    private string tempString;

    //Holds the speech bubble for the tutorial
    public GameObject tutorialSpeechBubble;
    
    //Holds the cover that grays out sections of the screen
    public GameObject tutorialCover;
    
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
        //Disables the tutorial speech bubble and cover
        tutorialSpeechBubble.SetActive(false);
        tutorialCover.SetActive(false);
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
        }//if
    }//electionExplainer
}
