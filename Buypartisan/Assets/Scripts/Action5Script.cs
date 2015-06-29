// Michael Lee
// Placeholder for an action for now

using UnityEngine;
using System.Collections;

public class Action5Script : MonoBehaviour
{
    public string actionName = "default";
    public int baseCost = 0;
    public int totalCost = 0; 
    public float costMultiplier = 1.0f; 

    public GameObject gameController; 
    public GameObject inputManager; 
    public GameObject uiController; 
    private GameObject[] voters; 
    private GameObject[] players; 

    private int currentPlayer; 

  
    void Start()
    {
        gameController = GameObject.FindWithTag("GameController");
        inputManager = GameObject.FindWithTag("InputManager");
        uiController = GameObject.Find("UI Controller");

        //Obtains the voter and player array from the gameController
        if (gameController != null)
        {
            voters = gameController.GetComponent<GameController>().voters;
            players = gameController.GetComponent<GameController>().players;
        }
        else
        {
            Debug.Log("Failed to obtain voters and players array from Game Controller");
        }

        //Disables the Action UI buttons
        uiController.GetComponent<UI_Script>().disableActionButtons();

        //The start function will not end until gets to the end
        //if you want to destroy the object in the start function,
        //it has to be the last thing you do, otherwise the flow of
        //controll will stay with the destroyed instance and 
        //that will crash the game (Alex Jungroth)

        //Get's whose turn it is from the gameController. Then checks if he has enough money to perform the action
        currentPlayer = gameController.GetComponent<GameController>().currentPlayerTurn;
        costMultiplier = this.transform.parent.GetComponent<PlayerTurnsManager>().costMultiplier;
        if (players[currentPlayer].GetComponent<PlayerVariables>().money < (baseCost * costMultiplier))
        {
            Debug.Log("Current Player doesn't have enough money to make this action.");

            uiController.GetComponent<UI_Script>().toggleActionButtons();
            Destroy(gameObject);
        }
        else
        {
            totalCost = (int)(baseCost * costMultiplier);
        }
    }

    // Update is called once per frame
    void Update()
    {


        //This is where the action should be placed.
        //action action action. blah blah. E.g. move a voter or player one block over.
        //When action is finished, run this bit of code below to tell the Game Manager the turn is over, 
        //and deletes itself so the PlayerTurnsManager knows the turn is over as well.

        /*
        EndAction ();
        */
    }

    void EndAction()
    {
        uiController.GetComponent<UI_Script>().toggleActionButtons();
        this.transform.parent.GetComponent<PlayerTurnsManager>().IncreaseCostMultiplier();
        players[currentPlayer].GetComponent<PlayerVariables>().money -= totalCost;  // Money is subtracted
        Destroy(gameObject);
    }
}
