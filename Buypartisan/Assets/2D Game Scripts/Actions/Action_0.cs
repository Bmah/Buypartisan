using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//MOVE PLAYER

public class Action_0 : MonoBehaviour
{
    private BoardGameController gameController;
    private ActionController actionController;

    public string name = "Action 0";
    public int baseCost = 100;

    private bool ActionFinished = false;
    private GameObject CurrentPlayer;
    private GameObject MovementKeys;
    private Vector3 StartLoc;

    public void Setup(BoardGameController gm, ActionController ac)
    {
        gameController = gm;
        actionController = ac;

        CurrentPlayer = gameController.Players[actionController.CurrentPlayer];
        CurrentPlayer.GetComponent<Player>().CurMoney -= baseCost;

        gameController.TooltipPanel.GetComponentInChildren<Text>().text = "Move your player one space. Use button at the bottom when done.";

        MovementKeys = Instantiate(gameController.MovementKeys, CurrentPlayer.transform.position, Quaternion.identity) as GameObject;
        StartLoc = CurrentPlayer.transform.position;
    }

    void Awake()
    {

    }

    // Update is called once per frame
    void Update ()
    {

        if (actionController.ActionConfirm)
        {
            Vector3 CurLoc = new Vector3(CurrentPlayer.transform.position.x - 0.5f, CurrentPlayer.transform.position.y, CurrentPlayer.transform.position.z - 0.5f);
            if (gameController.Board[(int)CurLoc.x, (int)CurLoc.z] <= 0) // VALID PLACEMENT
            {
                Debug.Log("PLACEMENT VALID, ENDING ACTION");
                gameController.Board[(int)StartLoc.x, (int)StartLoc.z] = -1;
                gameController.Board[(int)CurLoc.x, (int)CurLoc.z] = 2;
                ActionFinished = true;
            }
            else //NOT VALID POSITION
            {
                //DO ANYTHING?
            }

            actionController.ActionConfirm = false;
        }

        if (ActionFinished)
        {
            Destroy(MovementKeys);
            actionController.EndAction();
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, gameController.RayMask))
            {
                Vector3 NewLoc;

                Debug.DrawLine(ray.origin, hit.point);
                if (hit.transform.tag == ("MOVE_UP"))
                {
                    Debug.Log("UP HIT");
                    NewLoc = new Vector3(StartLoc.x, StartLoc.y, StartLoc.z + 1.0f);
                    MovePlayer(NewLoc);
                }
                else if (hit.transform.tag == ("MOVE_DOWN"))
                {
                    Debug.Log("DOWN HIT");
                    NewLoc = new Vector3(StartLoc.x, StartLoc.y, StartLoc.z - 1.0f);
                    MovePlayer(NewLoc);
                }
                else if (hit.transform.tag == ("MOVE_RIGHT"))
                {
                    Debug.Log("RIGHT HIT");
                    NewLoc = new Vector3(StartLoc.x + 1.0f, StartLoc.y, StartLoc.z);
                    MovePlayer(NewLoc);
                }
                else if (hit.transform.tag == ("MOVE_LEFT"))
                {
                    Debug.Log("LEFT HIT");
                    NewLoc = new Vector3(StartLoc.x - 1.0f, StartLoc.y, StartLoc.z);
                    MovePlayer(NewLoc);
                }
            }
        }
    }

    private void MovePlayer(Vector3 NewLoc)
    {
        float x = NewLoc.x;
        float y = NewLoc.y;
        float z = NewLoc.z;

        //Board Bounds Checks
        if (x <= 0 || z <= 0)
            ;//DO NOTHING
        else if (x >= gameController.BoardSize || z >= gameController.BoardSize)
            ;// DO NOTHING
        else
            CurrentPlayer.transform.position = NewLoc;
    }

}
