using UnityEngine;
using System.Collections;

public class VoterVariables : MonoBehaviour {

    // Transform is built-in

    public int money = 0;
    public int votes = 0;
    public int type = 0; // Instead of string, int stores less data. Type 0 can be default, type 1 can be enviormentalist, type 2 can be mafias, etc.

	private bool selected = false; //Use Toggle Function to change it
	public Material selectedTexture;
	public Material unselectedTexture;
	private Renderer voterRenderer;

	void Start () {
		voterRenderer = this.GetComponent<Renderer>();
	}
	

	void Update () {

	}


	/// <summary>
	/// Toggles whether or not the Voter is selected.
	/// </summary>
	public void ToggleSelected(){
		if (selected) {
			selected = false;
			voterRenderer.material = unselectedTexture;
		}
		else {
			selected = true;
			voterRenderer.material = selectedTexture;
		}
	}

	/// <summary>
	/// Gets whether or not the voter is selected.
	/// </summary>
	/// <returns><c>true</c>, if selected was gotten, <c>false</c> otherwise.</returns>
	public bool GetSelected(){
		return selected;
	}
}
