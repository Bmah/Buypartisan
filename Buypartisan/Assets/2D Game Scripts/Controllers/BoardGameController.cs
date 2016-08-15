using UnityEngine;
using System.Collections;

//Tyler Welsh - 2016
/*
    Game Controller for the board game version of Buypartisan. This controller implements game states using an interface and abstract class.
*/

public class BoardGameController : MonoBehaviour
{
    //Holds the current game state
    public IGameObjectState currentState;

    //////////////////////////////////////////////////
    // Controllers                                  //
    //////////////////////////////////////////////////
    [Header("Controllers")]
    public GUIController GUI;
    public CamController camController;

    //////////////////////////////////////////////////
    // Game Objects                                 //
    //////////////////////////////////////////////////
    [Header("Game Objects")]
    public GameObject MainCam;
    public GameObject[] PlayerSelectGameModels;
    public GameObject[] PlayerPrefabs;
    public GameObject[] ActionPrefabs;
    public GameObject VoterObject;

    //////////////////////////////////////////////////
    // UI Objects                                   //
    //////////////////////////////////////////////////
    [Header("UI Objects")]
    public GameObject StartingScreen;
    public GameObject PlayerSelectScreen;
    public GameObject ConfirmPlacementScreen;
    public GameObject InfoDisplayScreen;
    public GameObject ActionDisplayScreen;
    public GameObject TurnPanel;
    public GameObject ElectionResultsScreen;
    public GameObject GameOverScreen;

    //////////////////////////////////////////////////
    // Game Settings                                //
    //////////////////////////////////////////////////
    [Header("Game Settings")]
    public int NumberOfPlayers;
    public int BoardSize = 10;
    public TextAsset VoterLocations;
    public int NumOfRounds = 5;
    public int NumOfElections = 2;

    //////////////////////////////////////////////////
    // Various Variables                            //
    //////////////////////////////////////////////////
    [Header("Misc")]
    public Transform[] CamPositions = new Transform[2];
    public GameObject VoterContainer;
    public LayerMask RayMask;


    public const int Orange_Party = 0;
    public const int Green_Party = 1;
    public const int Purple_Party = 2;
    public const int StartingCamPos = 0;
    public const int PlayerSelectCamPos = 1;
    public const int SpawningCamPos = 2;
    public const int PlacePlayerCamPos = 3;

    [HideInInspector]
    public GameObject[] Players;
    [HideInInspector]
    public GameObject[] Voters;
    //TODO: MAKE BOARD SIZE VARIABLE
    [HideInInspector]
    public int[,] Board = new int[10,10];
    [HideInInspector]
    public int[] PlayerPartyMapping;    
    //Is Camera Currently Moving?
    [HideInInspector]
    public bool IsCamMoving = false;
    //Has Game Begun?
    [HideInInspector]
    public bool GameHasBegun = false;
    [HideInInspector]
    public bool BoardIsReady = false;
    [HideInInspector]
    public bool ConfirmPlayerPlaced = false;
    [HideInInspector]
    public int NumActionSelected = -1;
    [HideInInspector]
    public bool endTurn = false;

    // Use this for initialization
    void Start ()
    {
        TogglePlayerSelect(false);
        ToggleConfirmPlacement(false);
        ToggleActionDisplay(false);
        ToggleInfoDisplay(false);
        ToggleTurnPanel(false);
        ToggleStartingScreen(true);
        NumberOfPlayers = 2;
        //When game starts, state is set to Player Select State
        currentState = new GameStates.BeforePlayState(this);
        StartCoroutine(InitBoard());
    }
	
	// Update is called once per frame
	void Update ()
    {
        currentState.Update();
	}

    void LateUpdate()
    {
        currentState.LateUpdate();
    }

    void FixedUpdate()
    {
        currentState.FixedUpdate();
    }

    //////////////////////////////////////////////////
    // Game State Functions                         //
    //////////////////////////////////////////////////
    public void BeginGame()
    {
        GameHasBegun = true;
        ToggleStartingScreen(false);
        Players = new GameObject[NumberOfPlayers];
        PlayerPartyMapping = new int[NumberOfPlayers];
    }

    public void ConfirmPlacement()
    {
        ConfirmPlayerPlaced = true;
    }

    //////////////////////////////////////////////////
    // Camera Functions                             //
    //////////////////////////////////////////////////
    public void PlayerSelectStartCam()
    {
        StartCoroutine(MoveCamera(MainCam.transform, CamPositions[PlayerSelectCamPos], 2.0f));
    }
    
    public void PlayerSelectEndCam()
    {
        StartCoroutine(MoveCamera(MainCam.transform, CamPositions[SpawningCamPos], 2.0f));
    }

    public void PlacePlayerCam()
    {
        StartCoroutine(MoveCamera(MainCam.transform, CamPositions[StartingCamPos], 2.0f));
    }
    //////////////////////////////////////////////////
    // GUI Functions                                //
    //////////////////////////////////////////////////
    //TODO: MOVE GUI FUNCTIONS TO GUI CONTROLLER. FOR NOW THEY WILL ALL BE HERE
    public void OnValueChanged(float NewValue)
    {
        NumberOfPlayers = (int)NewValue;
        Debug.Log("NUM PLAYERS CHANGED TO: " + NumberOfPlayers);
    }

    public void ToggleInfoDisplay(bool state)
    {
        InfoDisplayScreen.SetActive(state);
    }

    public void ToggleActionDisplay(bool state)
    {
        ActionDisplayScreen.SetActive(state);
    }

    public void TogglePlayerSelect(bool state)
    {
        PlayerSelectScreen.SetActive(state);
    }

    public void ToggleConfirmPlacement(bool state)
    {
        ConfirmPlacementScreen.SetActive(state);
    }

    public void ToggleStartingScreen(bool state)
    {
        StartingScreen.SetActive(state);
    }

    public void ToggleTurnPanel(bool state)
    {
        TurnPanel.SetActive(state);
    }

    //////////////////////////////////////////////////
    // Button Functions                             //
    //////////////////////////////////////////////////

    public void ActionButtons(int actionNum)
    {
        NumActionSelected = actionNum;
    }

    public void EndTurn()
    {
        endTurn = true;
    }

    //////////////////////////////////////////////////
    // Misc Functions                               //
    //////////////////////////////////////////////////
    /// <summary>
    /// Moves the camera from CurPos to Dest in time seconds
    /// </summary>
    /// <param name="CurPos"></param>
    /// <param name="Dest"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    public IEnumerator MoveCamera(Transform InitPos, Transform Dest, float endTime)
    {
        IsCamMoving = true;
        float elapsedTime = 0;
        Debug.Log("CAM START MOVING");
        //Debug.Log("FROM: " + InitPos.position.x + " " + InitPos.position.y + " " + InitPos.position.z);
        //Debug.Log("TO: " + Dest.position.x + " " + Dest.position.y + " " + Dest.position.z);
        while (elapsedTime < endTime)
        {
            MainCam.transform.position = Vector3.Lerp(InitPos.position, Dest.position, elapsedTime / endTime );
            MainCam.transform.rotation = Quaternion.Lerp(InitPos.rotation, Dest.rotation, elapsedTime /endTime );
            //yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
            if (MainCam.transform.position == Dest.position)
            {
                Debug.Log("Arrived");
                IsCamMoving = false;
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        //FALL BACK
        IsCamMoving = false;
        Debug.Log("CAM FINISHED MOVING, " + elapsedTime);
    }

    public IEnumerator InitBoard()
    {
        for(int i = 0; i < BoardSize; i++)
        {
            for(int j = 0; j < BoardSize; j++)
            {
                Board[i, j] = -1;
            }

            yield return null;
        }

        BoardIsReady = true;
    }

    //CURRENTLY BROKEN
    public void DEBUG_BOARD_STATE()
    {
        string BoardState = "                           ";
        for (int i = 0; i < BoardSize; i++)
        {
            for (int j = 0; j < BoardSize; j++)
            {
                BoardState.Insert(j, Board[i, j].ToString());
            }
            Debug.Log(BoardState);
            BoardState = "";
        }
    }
}
