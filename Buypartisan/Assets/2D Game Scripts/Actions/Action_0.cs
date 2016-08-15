using UnityEngine;
using System.Collections;

public class Action_0 : MonoBehaviour, Action_Base
{
    private BoardGameController gameController;

    public string name = "Action 0";
    public int baseCost = 10;
    public float costMultiplier = 1.0f;

    private int curCost;
    private int curPlayer;

    void Awake()
    {

    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void Setup(int curP, MonoBehaviour gm)
    {
        gameController = (BoardGameController)gm;
        curPlayer = curP;
        Debug.Log("Cur Player: " + curPlayer + " Returning control.");
        ////TODO: DELETE
        gameController.NumActionSelected = -1;
        Destroy(this);
    }
}
