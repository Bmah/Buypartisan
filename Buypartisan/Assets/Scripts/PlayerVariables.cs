using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerVariables : MonoBehaviour {

	// Transform is already built-in

    public int pTag = 0;
    public int money = 100;
    public int votes = 0;

	public Material selectedTexture;
	public Material unselectedTexture;
	private Renderer playerRenderer;

	private bool selected = false;

	//holds all of the shadow postions (Alex Jungroth)
	public List<GameObject> shadowPositions = new List<GameObject>();

	void Start () {
        
		//makes sure all of the players shadowPositions lists are empty at the start of the program (Alex Jungroth)
		this.shadowPositions.Clear ();

		playerRenderer = this.GetComponent<Renderer>();
       
	}
	
	
	void Update () {
	
	}

	/// <summary>
	/// Toggles whether or not the Voter is selected.
	/// </summary>
	public void ToggleSelected(){
		if (selected) {
			selected = false;
			playerRenderer.material = unselectedTexture;
		}
		else {
			selected = true;
			playerRenderer.material = selectedTexture;
		}
	}
	
	/// <summary>
	/// Gets whether or not the voter is selected.
	/// </summary>
	/// <returns><c>true</c>, if selected was gotten, <c>false</c> otherwise.</returns>
	public bool GetSelected(){
		return selected;
	}

	public void VoterSuppressStart() {
		if (money < 5) 
			Debug.Log ("You Need More Money");
		else {
			Debug.Log ("Click on a voter");
		}
	}

}
