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
    [System.NonSerialized]
    public const int goalSection = 0;
    [System.NonSerialized]
    public const int sliderSection = 1;
    [System.NonSerialized]
    public const int voterSection = 2;
    [System.NonSerialized]
    public const int positioningSection = 3;
    [System.NonSerialized]
    public const int actionSection = 4;
    [System.NonSerialized]
    public const int randomEventSection = 5;
    [System.NonSerialized]
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

    //Holds the speech bubble for the tutorial
    public GameObject tutorialSpeechBubble;
    //Holds the buttons for the tutorial 
    public GameObject leftButton;
    public GameObject rightButton;
    public GameObject confirmButton;

    //Variable to hold a picture of a voter for tutorial purposes
    public GameObject voterImage;
    //Holds the movement buttons to disable them when needed
    public GameObject movementKeys;
    
    //Holds the cover that grays out sections of the screen
    public GameObject[] TutorialCovers;

    //Holds positions for all tutorials
    public Transform[] TutorialTransforms;

    //Holds text files for tutorials
    public TextAsset[] TutorialFiles;

    //Used to read in the text files
    StreamReader currentReader;

    //Holds whether or not the tutorial controller is currently active
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

/*

     //Parses the text files to get the strings
        //Goal Text
        currentReader = new StreamReader(Application.dataPath + "\\Tutorial Text Files\\Goal.txt", Encoding.Default);

        using(currentReader)
        {
            //Reads the first line of the file
            tempString = currentReader.ReadLine();

            //Reads line by line to the end of the text file
            while(tempString != null)
            {
                goalStrings.Add(tempString);
               tempString = currentReader.ReadLine();
            }//while

            //Closes this sections reader
            currentReader.Close();
        }//using

        //Slider Text
        currentReader = new StreamReader(Application.dataPath + "\\Tutorial Text Files\\Slider.txt", Encoding.Default);

        using (currentReader)
        {
            //Reads the first line of the file
            tempString = currentReader.ReadLine();

            //Reads line by line to the end of the text file
            while (tempString != null)
            {
                sliderStrings.Add(tempString);
                tempString = currentReader.ReadLine();
            }//while

            //Closes this sections reader
            currentReader.Close();
        }//using

        //Voter Text
        currentReader = new StreamReader(Application.dataPath + "\\Tutorial Text Files\\Voter.txt", Encoding.Default);

        using (currentReader)
        {
            //Reads the first line of the file
            tempString = currentReader.ReadLine();

            //Reads line by line to the end of the text file
            while (tempString != null)
            {
                voterStrings.Add(tempString);
                tempString = currentReader.ReadLine();
            }//while

            //Closes this sections reader
            currentReader.Close();
        }//using

        //Positioning Text
        currentReader = new StreamReader(Application.dataPath + "\\Tutorial Text Files\\Positioning.txt", Encoding.Default);

        using (currentReader)
        {
            //Reads the first line of the file
            tempString = currentReader.ReadLine();

            //Reads line by line to the end of the text file
            while (tempString != null)
            {
                positioningStrings.Add(tempString);
                tempString = currentReader.ReadLine();
            }//while

            //Closes this sections reader
            currentReader.Close();
        }//using

        //Action Text
        currentReader = new StreamReader(Application.dataPath + "\\Tutorial Text Files\\Action.txt", Encoding.Default);

        using (currentReader)
        {
            //Reads the first line of the file
            tempString = currentReader.ReadLine();

            //Reads line by line to the end of the text file
            while (tempString != null)
            {
                actionStrings.Add(tempString);
                tempString = currentReader.ReadLine();
            }//while

            //Closes this sections reader
            currentReader.Close();
        }//using

        //Random Text
        currentReader = new StreamReader(Application.dataPath + "\\Tutorial Text Files\\Random.txt", Encoding.Default);

        using (currentReader)
        {
            //Reads the first line of the file
            tempString = currentReader.ReadLine();

            //Reads line by line to the end of the text file
            while (tempString != null)
            {
                randomEventStrings.Add(tempString);
                tempString = currentReader.ReadLine();
            }//while

            //Closes this sections reader
            currentReader.Close();
        }//using

        //Election Text
        currentReader = new StreamReader(Application.dataPath + "\\Tutorial Text Files\\Election.txt", Encoding.Default);

        using (currentReader)
        {
            //Reads the first line of the file
            tempString = currentReader.ReadLine();

            //Reads line by line to the end of the text file
            while (tempString != null)
            {
                electionStrings.Add(tempString);
                tempString = currentReader.ReadLine();
            }//while

            //Closes this sections reader
            currentReader.Close();
        }//using

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
            //Sets the currentCover
            currentCover = goalSection;

            //Updates the current strings to this sections text
            currentStrings = goalStrings;

            //Sets the tutorial speech bubble to the first line of text
            tutorialSpeechBubble.gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Text>().text = currentStrings[0];

            //The tutorial is currently active
            isActiveTutorial = true;
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
            //Sets the currentCover
            currentCover = sliderSection;

            //Updates the current strings to this sections text
            currentStrings = sliderStrings;

            //Sets the tutorial speech bubble to the first line of text
            tutorialSpeechBubble.gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Text>().text = currentStrings[0];

            //The tutorial is currently active
            isActiveTutorial = true;
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
            //Sets the currentCover
            currentCover = voterSection;
            //Makes the image of the voter active so we can see what they look like
            voterImage.SetActive(true);

            //Updates the current strings to this sections text
            currentStrings = voterStrings;

            //Sets the tutorial speech bubble to the first line of text
            tutorialSpeechBubble.gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Text>().text = currentStrings[0];

            //The tutorial is currently active
            isActiveTutorial = true;
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
            //Sets the currentCover
            currentCover = positioningSection;

            //Updates the current strings to this sections text
            currentStrings = positioningStrings;

            //Sets the tutorial speech bubble to the first line of text
            tutorialSpeechBubble.gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Text>().text = currentStrings[0];

            //The tutorial is currently active
            isActiveTutorial = true;
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

            //Disable the movement buttons so they are not in the way
            movementKeys.SetActive(false);

            //Moves the speech bubble to the correct location
            tutorialSpeechBubble.transform.position = TutorialTransforms[actionSection].position;
            //Displays the tutorial speech bubble
            tutorialSpeechBubble.SetActive(true);
            //Activates the correct dark screen mask
            TutorialCovers[actionSection].SetActive(true);
            //Sets the currentCover
            currentCover = actionSection;

            //Updates the current strings to this sections text
            currentStrings = actionStrings;

            //Sets the tutorial speech bubble to the first line of text
            tutorialSpeechBubble.gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Text>().text = currentStrings[0];

            //The tutorial is currently active
            isActiveTutorial = true;
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
            //Sets the currentCover
            currentCover = randomEventSection;

            //Updates the current strings to this sections text
            currentStrings = randomEventStrings;

            //Sets the tutorial speech bubble to the first line of text
            tutorialSpeechBubble.gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Text>().text = currentStrings[0];

            //The tutorial is currently active
            isActiveTutorial = true;
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
            //Sets the currentCover
            currentCover = electionSection;

            //Updates the current strings to this sections text
            currentStrings = electionStrings;

            //Sets the tutorial speech bubble to the first line of text
            tutorialSpeechBubble.gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Text>().text = currentStrings[0];

            //The tutorial is currently active
            isActiveTutorial = true;
        }//if
    }//electionExplainer


    */
