using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    private BoardGameController gameController;
    private int PlayerID;
    private int curMoney;
    private int curNumVotes;
    private int curSphereSize;
    private int victoryPoints;
    private bool playerSelected;

    private GameObject sphereObject;
    private Renderer playerRenderer;
    private Renderer sphereRenderer;

    public int PartyID;
    public int StartingMoney;
    public int SphereSize;

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
