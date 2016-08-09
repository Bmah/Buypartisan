using UnityEngine;
using System.Collections;

namespace GameStates
{
    public class GameSetupState : AbstractGameObjectState
    {
        private BoardGameController gameController;


        //CAN DO PRESET TEXT FILE OR RANDOM GENERATED
        //TODO: RANDOM GENERATION

        private int MaxVoters = 5;
        private float TimeBetweenSpawn = 0.5f;
        private float elapsedTime;
        private Vector3[] VoterLocations = new Vector3[5];
        private TextAsset VoterLocationsFile;
        private int curSpawned = 0;

        public GameSetupState(MonoBehaviour parent) : base(parent)
        {
            gameController = (BoardGameController)parent;
            elapsedTime = 0.0f;
            VoterLocationsFile = gameController.VoterLocations;
            GetVoterLocs();
            Debug.Log("Game Setup");
        }
        // Update is called once per frame
        public override void Update()
        {
            if (!gameController.IsCamMoving)
            {
                if (elapsedTime > TimeBetweenSpawn && curSpawned < MaxVoters)
                {
                    Debug.Log("Spawning");
                    elapsedTime = 0;

                    GameObject Voter = MonoBehaviour.Instantiate(gameController.VoterObject, VoterLocations[curSpawned], Quaternion.identity) as GameObject;
                    if (Voter)
                    {
                        Debug.Log("Spawned Correctly");
                        curSpawned++;
                    }
                    Voter.transform.parent = gameController.VoterContainer.transform;
                    //SET VOTER VALUES                
                }

                //STATE CHANGE
                if (curSpawned >= MaxVoters)
                {
                    gameController.PlacePlayerCam();
                    gameController.currentState = new PlacePlayerState(gameController);
                }
                elapsedTime += Time.deltaTime;
            }
        }

        public void GetVoterLocs()
        {
            float x, y, z;
            //Characters that split the lines (like newline)
            var FileSplit = new string[] { "\r\n", "\n", "\r" };
            //Characters that split individual numbers (space or comma)
            var LineSplit = new char[] { ' ', ',' };
            //Get all lines in the text file seperated by FileSplit
            var VoterLines = VoterLocationsFile.text.Split(FileSplit, System.StringSplitOptions.None);
            //For loop to step through each line and parse the info
            for (int i = 0; i < VoterLines.Length; i++)
            {
                var curLine = VoterLines[i].Split(LineSplit, System.StringSplitOptions.None);
                float.TryParse(curLine[0], out x);
                float.TryParse(curLine[1], out y);
                float.TryParse(curLine[2], out z);
                VoterLocations[i] = new Vector3(x, y, z);
                Debug.Log(VoterLocations[i]);
            }
            
        }
    }
}
