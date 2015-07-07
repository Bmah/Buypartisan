using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerVariables : MonoBehaviour {

	// Transform is already built-in

    public int pTag = 0;
    public int money = 100;
    public int votes = 0;
	public int sphereSize = 4;

	public Material selectedTexture;
	public Material unselectedTexture;
	private Renderer playerRenderer;
	public Renderer sphereRenderer;
	public GameObject sphereController;
	public Color transparentColor;

	private bool selected = false;

	//holds all of the shadow postions (Alex Jungroth)
	public List<GameObject> shadowPositions = new List<GameObject>();

	void Start () {
        
		//makes sure all of the players shadowPositions lists are empty at the start of the program (Alex Jungroth)
		this.shadowPositions.Clear ();

		//sets up the spheres for the players (Daniel Schlesinger)
		sphereSize = 10 * sphereSize;
		sphereController = this.gameObject.transform.GetChild (0).gameObject;
		sphereRenderer = sphereController.GetComponent<Renderer> ();
		transparentColor = sphereRenderer.material.color;
		transparentColor.a = 0.2f;
		sphereRenderer.material.SetColor ("_Color", transparentColor);
		sphereController.transform.localScale = new Vector3 (sphereSize, sphereSize, sphereSize);

		//playerRenderer = this.GetComponent<Renderer>();
	//	GameObject sphere = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		//sphere.transform.position = this.transform.position;
		//sphere.transform.SetParent (this);
       
	}
	
	
	void Update () {
	
	}

	/// <summary>
	/// Toggles whether or not the Voter is selected.
	/// Brian Mah
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
	/// Brian Mah
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
