using UnityEngine;
using System.Collections;

public class InputManagerScript : MonoBehaviour {
	public bool leftClickDown = false;
	public bool leftClickHold = false;
	public bool leftClickUp = false;
	public bool rightClickDown = false;
	public bool rightClickHold = false;
	public bool rightClickUp = false;

	public bool upButtonDown = false;
	public bool upButtonHold = false;
	public bool upButtonUp = false;
	public bool leftButtonDown = false;
	public bool leftButtonHold = false;
	public bool leftButtonUp = false;
	public bool downButtonDown = false;
	public bool downButtonHold = false;
	public bool downButtonUp = false;
	public bool rightButtonDown = false;
	public bool rightButtonHold = false;
	public bool rightButtonUp = false;

	public bool fButtonDown = false;
	public bool fButtonHold = false;
	public bool fButtonUp = false;

	public float mouseAxisX;
	public float mouseAxisY;

	public bool qButtonDown = false;
	public bool qButtonHold = false;
	public bool qButtonUp = false;
	public bool eButtonDown = false;
	public bool eButtonHold = false;
	public bool eButtonUp = false;

	public bool zButtonUp = false;
	public bool zButtonDown = false;
	public bool zButtonHold = false;
	public bool xButtonUp = false;
	public bool xButtonDown = false;
	public bool xButtonHold = false;
	public bool cButtonUp = false;
	public bool cButtonDown = false;
	public bool cButtonHold = false;
	public bool vButtonDown = false;
	public bool vButtonHold = false;
	public bool vButtonUp = false;
	public bool bButtonDown = false;
	public bool bButtonHold = false;
	public bool bButtonUp = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//Checks if left click is being pressed or being held down.
		//leftClickDown only calls for one frame. Use it like you would GetButtonDown.
		//leftClickHold calls so long as the button is held. Use it like you would GetButton.
		//leftClickUp calls only once when the button is released. Use it like you would GetButtonUp
		//The same code is used for the rest of the buttons.
		if (leftClickUp) {
			leftClickUp = false;
		} else if (Input.GetButtonUp ("Fire1")) {
			leftClickUp = true;
		}
		if (Input.GetButton ("Fire1")) {
			leftClickHold = true;
			leftClickDown = false;
		}
		if (Input.GetButtonDown ("Fire1")) {
			leftClickDown = true;
		}
		if (Input.GetButtonUp ("Fire1")) {
			leftClickHold = false;
			leftClickDown = false;
		}

		if (rightClickUp) {
			rightClickUp = false;
		} else if (Input.GetButtonUp ("Fire2")) {
			rightClickUp = true;
		}
		if (Input.GetButton ("Fire2")) {
			rightClickHold = true;
			rightClickDown = false;
		}
		if (Input.GetButtonDown ("Fire2")) {
			rightClickDown = true;
		}
		if (Input.GetButtonUp ("Fire2")) {
			rightClickHold = false;
			rightClickDown = false;
		}

		//The input right now uses both WASD and up/left/down/right arrows.
		if (upButtonUp) {
			upButtonUp = false;
		} else if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp (KeyCode.UpArrow)) {
			upButtonUp = true;
		}
		if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
			upButtonHold = true;
			upButtonDown = false;
		}
		if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
			upButtonDown = true;
		}
		if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow)) {
			upButtonHold = false;
			upButtonDown = false;
		}

		if (leftButtonUp) {
			leftButtonUp = false;
		} else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp (KeyCode.LeftArrow)) {
			leftButtonUp = true;
		}
		if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
			leftButtonHold = true;
			leftButtonDown = false;
		}
		if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
			leftButtonDown = true;
		}
		if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow)) {
			leftButtonHold = false;
			leftButtonDown = false;
		}

		if (downButtonUp) {
			downButtonUp = false;
		} else if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp (KeyCode.DownArrow)) {
			downButtonUp = true;
		}
		if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
			downButtonHold = true;
			downButtonDown = false;
		}
		if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
			downButtonDown = true;
		}
		if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow)) {
			downButtonDown = false;
			downButtonHold = false;
		}

		if (rightButtonUp) {
			rightButtonUp = false;
		} else if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp (KeyCode.RightArrow)) {
			rightButtonUp = true;
		}
		if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
			rightButtonHold = true;
			rightButtonDown = false;
		}
		if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
			rightButtonDown = true;
		}
		if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow)) {
			rightButtonDown = false;
			rightButtonHold = false;
		}

		if (fButtonUp) {
			fButtonUp = false;
		} else if (Input.GetKeyUp(KeyCode.F)) {
			fButtonUp = true;
		}
		if (Input.GetKey (KeyCode.F)) {
			fButtonHold = true;
			fButtonDown = false;
		}
		if (Input.GetKeyDown(KeyCode.F)) {
			fButtonDown = true;
		}
		if (Input.GetKeyUp (KeyCode.F)) {
			fButtonDown = false;
			fButtonHold = false;
		}

		mouseAxisX = Input.GetAxis ("Mouse X");
		mouseAxisY = Input.GetAxis ("Mouse Y");

		if (eButtonUp) {
			eButtonUp = false;
		} else if (Input.GetKeyUp (KeyCode.E)) {
			eButtonUp = true;
		}
		if (Input.GetKey (KeyCode.E)) {
			eButtonHold = true;
			eButtonDown = false;
		}
		if (Input.GetKeyDown (KeyCode.E)) {
			eButtonDown = true;
		}
		if (Input.GetKeyUp (KeyCode.E)) {
			eButtonDown = false;
			eButtonHold = false;
		}

		if (qButtonUp) {
			qButtonUp = false;
		} else if (Input.GetKeyUp (KeyCode.Q)) {
			qButtonUp = true;
		}
		if (Input.GetKey (KeyCode.Q)) {
			qButtonHold = true;
			qButtonDown = false;
		}
		if (Input.GetKeyDown (KeyCode.Q)) {
			qButtonDown = true;
		}
		if (Input.GetKeyUp (KeyCode.Q)) {
			qButtonDown = false;
			qButtonHold = false;
		}

		if (zButtonUp) {
			zButtonUp = false;
		} else if (Input.GetKeyUp (KeyCode.Z)) {
			zButtonUp = true;
		}
		if (Input.GetKey (KeyCode.Z)) {
			zButtonHold = true;
			zButtonDown = false;
		}
		if (Input.GetKeyDown (KeyCode.Z)) {
			zButtonDown = true;
		}
		if (Input.GetKeyUp (KeyCode.Z)) {
			zButtonDown = false;
			zButtonHold = false;
		}

		if (xButtonUp) {
			xButtonUp = false;
		} else if (Input.GetKeyUp (KeyCode.X)) {
			xButtonUp = true;
		}
		if (Input.GetKey (KeyCode.X)) {
			xButtonHold = true;
			xButtonDown = false;
		}
		if (Input.GetKeyDown (KeyCode.X)) {
			xButtonDown = true;
		}
		if (Input.GetKeyUp (KeyCode.X)) {
			xButtonDown = false;
			xButtonHold = false;
		}

		if (cButtonUp) {
			cButtonUp = false;
		} else if (Input.GetKeyUp (KeyCode.C)) {
			cButtonUp = true;
		}
		if (Input.GetKey (KeyCode.C)) {
			cButtonHold = true;
			cButtonDown = false;
		}
		if (Input.GetKeyDown (KeyCode.C)) {
			cButtonDown = true;
		}
		if (Input.GetKeyUp (KeyCode.C)) {
			cButtonDown = false;
			cButtonHold = false;
		}

		if (vButtonUp) {
			vButtonUp = false;
		} else if (Input.GetKeyUp (KeyCode.V)) {
			vButtonUp = true;
		}
		if (Input.GetKey (KeyCode.V)) {
			vButtonHold = true;
			vButtonDown = false;
		}
		if (Input.GetKeyDown (KeyCode.V)) {
			vButtonDown = true;
		}
		if (Input.GetKeyUp (KeyCode.V)) {
			vButtonDown = false;
			vButtonHold = false;
		}

		if (bButtonUp) {
			bButtonUp = false;
		} else if (Input.GetKeyUp (KeyCode.B)) {
			bButtonUp = true;
		}
		if (Input.GetKey (KeyCode.B)) {
			bButtonHold = true;
			bButtonDown = false;
		}
		if (Input.GetKeyDown (KeyCode.B)) {
			bButtonDown = true;
		}
		if (Input.GetKeyUp (KeyCode.B)) {
			bButtonDown = false;
			bButtonHold = false;
		}
	}
}
