// Michael Lee
// Instansiates the grid, dynamic from the GameController, and stores all grids into a 3 dimensional array
// The array can be used to change grid opacity (for xyz "selection")
// Because the way Daniel put this in GameController, if you want to change the default/min/max Opacity of the grids, it must be done within this script down in declaration

using UnityEngine;
using System.Collections;

public class GridInstanced : MonoBehaviour {

	private InputManagerScript inputManager;
	private GameController gameController; // Gets gridSize here

	public GameObject grid;
	public GameObject[, ,] grids = new GameObject[10, 10, 10];
	private int gridSize = 10; // NOT for array declaration, but for going through an array

	private int axisMode = 0;
	private int setX = 3, setY = 3, setZ = 3;

	public int defaultOpacity = 75;
	public int minimumOpacity = 12;
	public int maxOpacity = 180;

	// Since coloration is copmlicated, just know that this makes whatever you put in the colorwheel be set in here normally.
	private float r = 0, g = 0, b = 0;



	void Start() {

		inputManager = GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManagerScript>();
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		
		gridSize = gameController.GetComponent<GameController>().gridSize;

		r = grid.GetComponent<SpriteRenderer> ().color.r;
		g = grid.GetComponent<SpriteRenderer> ().color.g;
		b = grid.GetComponent<SpriteRenderer> ().color.b;
	}




	
	public void GridInstantiate (int max) {
		
		
		for (int x = 0; x < max; x++)
		{
			for (int y = 0; y < max; y++)
			{
				for (int z = 0; z < max; z++)
				{
					grids[x, y, z] = Instantiate(grid, new Vector3(x, y, z), Quaternion.identity) as GameObject;
				}
			}
		}
		
	}
	
	// Update is called once per frame
	void Update () {

		if (inputManager.zButtonDown) {
			SetZ ();
		}

		if (inputManager.xButtonDown) {
			SetX ();
		}

		if (inputManager.cButtonDown) {
			SetY ();
		}

		if (inputManager.vButtonDown) {
			ResetV ();
		}

		if (inputManager.bButtonDown) {
			ClearB ();
		}

		// If you're in xyz axis mode AND you're not holding right click to zoom in with scrollwheel, then run this
		if (axisMode >= 1 && axisMode <= 3 && inputManager.rightClickHold == false) 
		{

			if(Input.GetAxis("Mouse ScrollWheel") != 0)
			{
				ScrollAxis((int)Input.GetAxis("Mouse ScrollWheel"), axisMode);
			}
		}



	}



	void SetZ() {
		
		GridMakeOpaque();

		for (int z = 0; z < gridSize; z++) {
			for (int y = 0; y < gridSize; y++){
				grids [setX, y, z].GetComponent<SpriteRenderer> ().color = new Color (r, g, b, maxOpacity/255f);
			}
		}

		axisMode = 1;
	}

	void SetX() {
		
		GridMakeOpaque();

		for (int x = 0; x < gridSize; x++) {
			for (int y = 0; y < gridSize; y++){
				grids [x, y, setZ].GetComponent<SpriteRenderer> ().color = new Color (r, g, b, maxOpacity/255f);
			}
		}

		axisMode = 2;
	}

	// Take note that SetY activates with the C key
	void SetY() {

		GridMakeOpaque();

		for (int x = 0; x < gridSize; x++) {
			for (int z = 0; z < gridSize; z++){
				grids [x, setY, z].GetComponent<SpriteRenderer> ().color = new Color (r, g, b, maxOpacity/255f);
			}
		}

		axisMode = 3;
	}

	// This function makes all grids equally opaque to default value
	void ResetV() {
		for (int x = 0; x < gridSize; x++) {
			for (int y = 0; y < gridSize; y++) {
				for (int z = 0; z < gridSize; z++) {
					grids[x, y, z].GetComponent<SpriteRenderer>().color = new Color (r, g, b, defaultOpacity/255f);
				}
			}
		}

		axisMode = 0;
	}

	// This function makes all grids totally invisible
	void ClearB() {

		for (int x = 0; x < gridSize; x++) {
			for (int y = 0; y < gridSize; y++) {
				for (int z = 0; z < gridSize; z++) {
					grids[x, y, z].GetComponent<SpriteRenderer>().color = new Color (r, g, b, 0/255f);
				}
			}
		}

		axisMode = 4;
	}

	// This function basically clears and make all the grids almost-invisible
	void GridMakeOpaque() {
		for (int x = 0; x < gridSize; x++) {
			for (int y = 0; y < gridSize; y++) {
				for (int z = 0; z < gridSize; z++) {
					grids[x, y, z].GetComponent<SpriteRenderer>().color = new Color (r, g, b, minimumOpacity/255f);
				}
			}
		}
	}

	void ScrollAxis(int direction, int axisMode){

		if (direction != 0) 
		{

			// Don't make scrollwheel be able to jump +2 or -2; dangerous for out of bounds
			if (direction > 0)
				direction = 1;
			if (direction < 0)
				direction = -1; 

			switch(axisMode)
			{
			case 1:
				setX += direction;
				if (setX > gridSize - 1)
					setX = gridSize - 1;
				if (setX < 0)
					setX = 0;
				SetZ();
				break;
			case 2:
				setZ += direction;
				if (setZ > gridSize - 1)
					setZ = gridSize - 1;
				if (setZ < 0)
					setZ = 0;
				SetX();
				break;
			case 3:
				setY += direction;
				if (setY > gridSize - 1)
					setY = gridSize - 1;
				if (setY < 0)
					setY = 0;
				SetY();
				break;
			}
		}
	}
}

