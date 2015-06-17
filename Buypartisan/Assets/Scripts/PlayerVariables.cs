using UnityEngine;
using System.Collections;

public class PlayerVariables : MonoBehaviour {

	// Transform is already built-in

    public int pTag = 0;
    public int money = 0;
    public int votes = 0;

	public Material selectedTexture;
	public Material unselectedTexture;
	private Renderer playerRenderer;

	private bool selected = false;

	void Start () {
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
}
