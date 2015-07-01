using UnityEngine;
using System.Collections;

public class TitleScreenUIScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PlayGame(){
		Application.LoadLevel("TestScene");
	}

	public void QuitGame(){
		Application.Quit ();
	}
}
