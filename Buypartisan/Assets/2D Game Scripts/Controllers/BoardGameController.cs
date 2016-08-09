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
    public GameObject PlayerSelectScreen;
    public GameObject[] PlayerSelectGameModels;
    public GameObject[] PlayerPrefabs;
    public GameObject VoterObject;

    //////////////////////////////////////////////////
    // Game Settings                                //
    //////////////////////////////////////////////////
    [Header("Game Settings")]
    public float NumberOfPlayers;
    public int BoardSize = 10;
    public TextAsset VoterLocations;

    //////////////////////////////////////////////////
    // Various Variables                            //
    //////////////////////////////////////////////////
    [Header("Misc")]
    public Transform[] CamPositions = new Transform[2];
    public GameObject VoterContainer;

    public const int Orange_Party = 0;
    public const int Green_Party = 1;
    public const int Purple_Party = 2;
    public const int StartingCamPos = 0;
    public const int PlayerSelectCamPos = 1;
    public const int SpawningCamPos = 2;
    public const int PlacePlayerCamPos = 3;

    [HideInInspector]
    public GameObject[] Players;
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

	// Use this for initialization
	void Start ()
    {
        PlayerSelectScreen.SetActive(false);
        NumberOfPlayers = 2;
        //When game starts, state is set to Player Select State
        currentState = new GameStates.BeforePlayState(this);
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
    public void BeginGame(GameObject StartingScreen)
    {
        GameHasBegun = true;
        StartingScreen.SetActive(false);
    }

    public void EnablePlayerSelect()
    {
        PlayerSelectScreen.SetActive(true);
    }

    public void DisablePlayerSelect()
    {
        PlayerSelectScreen.SetActive(false);
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
    public void OnValueChanged(float NewValue)
    {
        NumberOfPlayers = NewValue;
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
}
