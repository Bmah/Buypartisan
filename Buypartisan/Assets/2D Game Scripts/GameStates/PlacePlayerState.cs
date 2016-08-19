using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace GameStates
{
    public class PlacePlayerState : AbstractGameObjectState
    {
        BoardGameController gameController;

        private int currentPlayer;
        private bool CurrentPlayerHasSpawned = false;
        private bool PlayerIsPlaced = false;

        GameObject Player;
        

        public PlacePlayerState (MonoBehaviour parent) : base(parent)
        {
            gameController = (BoardGameController)parent;
            currentPlayer = 0;
            gameController.camController.ToggleCamControls(true, false);
            gameController.ToggleConfirmPlacement(true);
            gameController.ToggleTooltipPanel(true);
            gameController.TooltipPanel.GetComponentInChildren<Text>().text = "Tip: Use arrows around player to move.\n Mouse over a voter to see more info.";
        }

        // Update is called once per frame
        public override void Update()
        {
            if(!gameController.IsCamMoving)
            {
                if(!CurrentPlayerHasSpawned)
                {
                    Debug.Log("SPAWNING PLAYER");
                    int partyToSelect = gameController.PlayerPartyMapping[currentPlayer];
                    GameObject NewPlayer =  MonoBehaviour.Instantiate(gameController.PlayerPrefabs[partyToSelect], new Vector3(0.5f, 0.1f, 0.5f), Quaternion.identity) as GameObject;
                    NewPlayer.GetComponent<Player>().SetupPlayer(currentPlayer, gameController);
                    gameController.Players[currentPlayer] = NewPlayer;

                    NewPlayer.transform.parent = gameController.PlayerContainer.transform;

                    Player = NewPlayer;
                    CurrentPlayerHasSpawned = true;
                    PlayerIsPlaced = false;
                }

                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    //TODO: ADD CONFIRMATION SELECT

                    if (Physics.Raycast(ray, out hit, gameController.RayMask))
                    {
                        Vector3 CurLoc = Player.transform.position;
                        Vector3 NewLoc;

                        Debug.DrawLine(ray.origin, hit.point);
                        if (hit.transform.tag == ("MOVE_UP"))
                        {
                            Debug.Log("UP HIT");
                            NewLoc = new Vector3(CurLoc.x, CurLoc.y, CurLoc.z + 1.0f);
                            MovePlayer(NewLoc);
                        }
                        else if (hit.transform.tag == ("MOVE_DOWN"))
                        {
                            Debug.Log("DOWN HIT");
                            NewLoc = new Vector3(CurLoc.x, CurLoc.y, CurLoc.z - 1.0f);
                            MovePlayer(NewLoc);
                        }
                        else if (hit.transform.tag == ("MOVE_RIGHT"))
                        {
                            Debug.Log("RIGHT HIT");
                            NewLoc = new Vector3(CurLoc.x + 1.0f, CurLoc.y, CurLoc.z);
                            MovePlayer(NewLoc);
                        }
                        else if (hit.transform.tag == ("MOVE_LEFT"))
                        {
                            Debug.Log("LEFT HIT");
                            NewLoc = new Vector3(CurLoc.x - 1.0f, CurLoc.y, CurLoc.z);
                            MovePlayer(NewLoc);
                        }
                    }
                }

                if(gameController.ConfirmPlayerPlaced)
                {
                    //CHECK IF PLACEMENT IS OK
                    Vector3 CurLoc = new Vector3(Player.transform.position.x - 0.5f, Player.transform.position.y, Player.transform.position.z - 0.5f);
                    if (gameController.Board[(int)CurLoc.x, (int)CurLoc.z] <= 0) // VALID PLACEMENT
                    {
                        gameController.Board[(int)CurLoc.x, (int)CurLoc.z] = 2;
                        currentPlayer++;
                        CurrentPlayerHasSpawned = false;
                        Player.GetComponent<Player>().ToggleMovementKeys(false);
                    }
                    else //NOT VALID POSITION
                    {
                        //DO ANYTHING?
                    }

                    gameController.ConfirmPlayerPlaced = false;

                }

                if(currentPlayer >= gameController.NumberOfPlayers)
                {
                    gameController.ToggleConfirmPlacement(false);
                    gameController.currentState = new GameStates.RoundState(gameController);
                }
            }
        }

        public void MovePlayer(Vector3 NewLoc)
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
                Player.transform.position = NewLoc;
        }
    }
}