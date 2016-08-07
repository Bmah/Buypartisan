using UnityEngine;
using System.Collections;

//Tyler Welsh - 2016
/*
    Game Controller for the board game version of Buypartisan. This controller implements game states using an interface and abstract class.
*/

public class BoardGameController : MonoBehaviour
{
    //////////////////////////////////////////////////
    // Controllers                                  //
    //////////////////////////////////////////////////
    public GUIController GUI;
    public CamController camController;

    //////////////////////////////////////////////////
    // Game Objects                                 //
    //////////////////////////////////////////////////
    public GameObject MainCam;
    public GameObject PlayerSelectScreen;

    //Holds the current game state
    public IGameObjectState currentState;

    //////////////////////////////////////////////////
    // Game Settings                                //
    //////////////////////////////////////////////////
    public float NumberOfPlayers;
    public int BoardSize = 10;

    //////////////////////////////////////////////////
    // Various Variables                            //
    //////////////////////////////////////////////////
    [HideInInspector]
    public GameObject[] Players;
    [HideInInspector]
    public int[] PlayerPartyMapping;
    //0 - Original, 1 - PlayerSelect
    public Transform[] CamPositions = new Transform[2];
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
        StartCoroutine(MoveCamera(MainCam.transform, CamPositions[1], 1.0f));
    }
    
    public void PlayerSelectEndCam()
    {
        PlayerSelectScreen.SetActive(false);
        StartCoroutine(MoveCamera(MainCam.transform, CamPositions[0], 1.0f));
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
    public IEnumerator MoveCamera(Transform CurPos, Transform Dest, float time)
    {
        float elapsedTime = 0.0f;
        IsCamMoving = true;
        Debug.Log("CAM START MOVING");
        Debug.Log("FROM: " + CurPos.position.x + " " + CurPos.position.y + " " + CurPos.position.z);
        Debug.Log("TO: " + Dest.position.x + " " + Dest.position.y + " " + Dest.position.z);
        while(elapsedTime < time)
        {
            MainCam.transform.position = Vector3.Lerp(CurPos.position, Dest.position, elapsedTime/time);
            MainCam.transform.rotation = Quaternion.Lerp(CurPos.rotation, Dest.rotation, elapsedTime/time);
            elapsedTime += Time.deltaTime / time;
            yield return new WaitForEndOfFrame();
        }

        IsCamMoving = false;
        Debug.Log("CAM FINISHED MOVING");
        
    }
}
