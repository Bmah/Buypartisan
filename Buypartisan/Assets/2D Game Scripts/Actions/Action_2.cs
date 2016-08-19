using UnityEngine;
using System.Collections;

public class Action_2 : MonoBehaviour
{

    private BoardGameController gameController;
    private ActionController actionController;

    public string name = "Action 2";
    public int baseCost = 150;

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
