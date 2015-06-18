using UnityEngine;
using System.Collections;

public class VoterVariables : MonoBehaviour {

    // Transform is built-in

    public int money = 0;
    public int votes = 0;
    public int type = 0; // Instead of string, int stores less data. Type 0 can be default, type 1 can be enviormentalist, type 2 can be mafias, etc.
	public int powerType;//used to determine what power is used, se by button press
	private bool selected = false; //Use Toggle Function to change it
	public Material selectedTexture;
	public Material unselectedTexture;
	private Renderer voterRenderer;
	public Collider Coll;//not sure if needed
	public GameController ControllerSquared;//used for power calls

	void Start () {
		voterRenderer = this.GetComponent<Renderer>();
		Coll = this.GetComponent<Collider> ();
		powerType = 1; //hardcoded for testing suppression
		ControllerSquared = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
	}
	

	void Update () {
		//bool controller goes here
		if (Input.GetMouseButton (0)) {//this has to get moved to the input manager
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Coll.Raycast (ray, out hit, 100.0F)) {
				ToggleSelected();
				ControllerSquared.PowerCall(powerType);
				ToggleSelected();

			}
		}

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
