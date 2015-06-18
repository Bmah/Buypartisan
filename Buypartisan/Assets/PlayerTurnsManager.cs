using UnityEngine;
using System.Collections;

public class PlayerTurnsManager : MonoBehaviour {
	public Component[] actionArray = new Component[10];
	public int chosenAction;

	public bool actionConfirmed;

	// Use this for initialization
	void Start () {
		actionArray [0] = null;
		actionArray[1] = this.GetComponent<Action1Script>();
		//actionArray[2] = this.GetComponent<>();
		//actionArray[3] = this.GetComponent<>();
		//actionArray[4] = this.GetComponent<>();
		//actionArray[5] = this.GetComponent<>();
		//actionArray[6] = this.GetComponent<>();
		//actionArray[7] = this.GetComponent<>();
		//actionArray[8] = this.GetComponent<>();
		//actionArray[9] = this.GetComponent<>();
		//actionArray[10] = this.GetComponent<>();
	}
	
	// Update is called once per frame
	void Update () {
		if (actionConfirmed) {
			PlayAction (chosenAction);
		}

		if (Input.GetKeyDown (KeyCode.Z)) {
//			this.GetComponent<Action1Script>().enabled = true;
//			this.enabled = false;
//			Debug.Log (actionArray[0]);
			chosenAction++;
		}
	}

	void PlayAction(int actionNumber) {
		Component tempAction = actionArray [actionNumber];
//		tempAction.enabled = true;
		Debug.Log (tempAction.name);
//		actionArray [actionNumber].enabled = true;
	}
}
