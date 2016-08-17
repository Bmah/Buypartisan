using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    private BoardGameController gameController;
    public int PlayerID;
    private int curNumVotes;
    private int curSphereSize;
    private int numVoters;
    private bool playerSelected;

    private GameObject sphereObject;
    private Renderer playerRenderer;
    private Renderer sphereRenderer;

    public int PartyID;
    public string PartyName;
    public int StartingMoney;
    public int SphereSize;

    public Hashtable AlignedVoters = new Hashtable();
    
    [HideInInspector]
    public int CurMoney;
    [HideInInspector]
    public int victoryPoints;
    [HideInInspector]
    public float Radius;


    public GameObject PlayerMovementKeys;

    public Material SelectedTexture;
    public Material UnselectedTexture;

    public Color TransparantColor;


    public void SetupPlayer(int playerID, MonoBehaviour gmCon)
    {
        PlayerID = playerID;
        gameController = (BoardGameController)gmCon;

        sphereObject = this.gameObject.transform.GetChild(0).gameObject;
        sphereRenderer = sphereObject.GetComponent<Renderer>();
        playerRenderer = this.gameObject.transform.GetChild(1).gameObject.GetComponent<Renderer>();

        if (!sphereRenderer)
            Debug.Log("Sphere Renderer Not Assigned");
        if (!playerRenderer)
            Debug.Log("Player Renderer Not Assigned");

        SphereSize *= 10;
        TransparantColor = sphereRenderer.material.color;
        TransparantColor.a = 0.2f;
        sphereRenderer.material.SetColor("_Color", TransparantColor);
        sphereObject.transform.localScale = new Vector3(SphereSize, SphereSize, SphereSize);
        Radius = SphereSize / 20.0f;
        CurMoney = StartingMoney;

        numVoters = 0;

    }

    public void AsignNewVoter(GameObject voter)
    {
        Debug.Log("Player " + PartyName + " got new voter, ID: " + voter.GetComponent<Voter>().VoterNum);
        AlignedVoters.Add(voter.GetComponent<Voter>().VoterNum, voter);
        numVoters++;
    }

    public void DeleteVoter(GameObject voter)
    {
        Debug.Log("Deleting Voter for party: " + PartyName);
        if(AlignedVoters.Contains(voter.GetComponent<Voter>().VoterNum))
        {
            AlignedVoters.Remove(voter.GetComponent<Voter>().VoterNum);
        }
        else
        {
            Debug.LogError("Voter didn't exist");
        }
    }

    public void Tally()
    {
        //Debug.Log("Tallying Votes for " + PartyName + ". Has " + AlignedVoters.Count + " voters");
        foreach(DictionaryEntry voter in AlignedVoters)
        {
            GameObject v = (GameObject)voter.Value;
            CurMoney += v.GetComponent<Voter>().money;
            victoryPoints += v.GetComponent<Voter>().votes;
            Debug.Log("Player: " + PartyName + " got $" + v.GetComponent<Voter>().money + " and " + v.GetComponent<Voter>().votes + " votes");
        }
    }


    public void ToggleMovementKeys(bool state)
    {
        PlayerMovementKeys.SetActive(state);
    }

    public void ToggleSelected()
    {
        if (playerSelected)
        {
            playerSelected = false;
            //playerRenderer.material = UnselectedTexture;
        }
        else
        {
            playerSelected = true;
            //playerRenderer.material = SelectedTexture;
        }
    }

    void OnMouseEnter()
    {
        ToggleSelected();
    }

    void OnMouseExit()
    {
        ToggleSelected();
    }


}
