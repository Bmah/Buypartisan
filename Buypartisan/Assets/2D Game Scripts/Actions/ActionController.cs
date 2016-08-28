using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ActionController : MonoBehaviour
{

    public BoardGameController gameController;

    public GameObject[] ActionPrefabs;

    [HideInInspector]
    public int CurrentPlayer;
    [HideInInspector]
    public bool ActionConfirm = false;

    private int CurrentActionNum;
    private GameObject currentAction;

    private string prevText;



    // Use this for initialization
    void Start ()
    {
        CurrentPlayer = -1;
        CurrentActionNum = -1;
        ActionConfirm = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    public GameObject StartAction(int ActionNum, int curPlayer)
    {
        CurrentPlayer = curPlayer;
        CurrentActionNum = ActionNum;

        GameObject action = Instantiate(ActionPrefabs[ActionNum]);
        currentAction = action;

        gameController.ToggleActionDisplay(false);
        gameController.ToggleTurnPanel(false);
        gameController.ToggleConfirmAction(true);

        prevText = gameController.TooltipPanel.GetComponentInChildren<Text>().text;

        if (ActionNum == 0)
            action.GetComponent<Action_0>().Setup(gameController, this);
        else if (ActionNum == 1)
            action.GetComponent<Action_1>().Setup(gameController, this);
        else if (ActionNum == 2)
            action.GetComponent<Action_2>().Setup(gameController, this);
        else
            Debug.LogError("Wrong Action Num Was Sent");


        return action;
    }

    public void ActionConfirmButton()
    {
        Debug.Log("ACTION CONFIRMED");
        ActionConfirm = true;
    }

    public void EndAction()
    {
        Debug.Log("ENDING ACTION NOW");
        gameController.NumActionSelected = -1;
        Destroy(currentAction);
        gameController.ToggleActionDisplay(true);
        gameController.ToggleTurnPanel(true);
        gameController.ToggleConfirmAction(false);
        gameController.TooltipPanel.GetComponentInChildren<Text>().text = prevText;
    }
}
