using UnityEngine;
using System.Collections;

public class Voter : MonoBehaviour
{
    private BoardGameController gameController;

    public int VoterNum;

    [HideInInspector]
    public int money;
    [HideInInspector]
    public int votes;

    [HideInInspector]
    public Material PartyTexture;
    [HideInInspector]
    public Material DefaultTexture;

    [HideInInspector]
    public int AlignedParty;
    [HideInInspector]
    public float DistanceToPlayer;



    //Resistance todo

    private Vector3 prevPos;

    public void SetupVoter(MonoBehaviour gm, int VNum)
    {
        gameController = (BoardGameController)gm;
        money = (int)Mathf.Round(Random.Range(10.0f, 50.0f));
        votes = (int)Mathf.Round(Random.Range(1.0f, 2.0f));
        AlignedParty = -1;
        DistanceToPlayer = int.MaxValue;
        VoterNum = VNum;
        DefaultTexture = this.transform.GetChild(0).GetChild(1).GetComponent<Renderer>().material;
    }

    public void ChangeParty(int player, float dist)
    {
        if (player < 0)
        {
            if(this.AlignedParty >= 0)
            {
                //Debug.Log("Got Player: " + player + " with aligned party: " + AlignedParty + " for voter #: " + VoterNum);
                gameController.Players[AlignedParty].GetComponent<Player>().DeleteVoter(this.gameObject);
            }
            AlignedParty = -1;
            dist = -1;
        }
        else
        {
            //Debug.Log("Got player: " + player + " changing color now");
            AlignedParty = player;
            PartyTexture = gameController.Players[player].GetComponent<Player>().UnselectedTexture;
            DistanceToPlayer = dist;
            gameController.Players[AlignedParty].GetComponent<Player>().AsignNewVoter(this.gameObject);


            this.transform.GetChild(0).GetChild(1).GetComponent<Renderer>().material = PartyTexture;
        }
    }
	
}
