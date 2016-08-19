using UnityEngine;
using System.Collections;

public class Action_1 : MonoBehaviour
{
    private BoardGameController gameController;
    private ActionController actionController;

    public string name = "Action 1";
    public int baseCost = 100;

    private GameObject CurrentPlayer;


    public void Setup(BoardGameController gm, ActionController ac)
    {
        gameController = gm;
        actionController = ac;

        CurrentPlayer = gameController.Players[actionController.CurrentPlayer];
        CurrentPlayer.GetComponent<Player>().CurMoney -= baseCost;


    }

    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

}
